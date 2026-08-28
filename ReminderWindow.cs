using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Interactivity;

namespace HealthyWindows;

public class ReminderWindow : Window
{
    private long _reminderId;
    private readonly string _videoType;
    private readonly string _videoPath;

    public ReminderWindow(string videoType, string videoPath)
    {
        Width = 650;
        Height = 350;
        _videoType = videoType;
        _videoPath = videoPath;
        WindowStartupLocation = WindowStartupLocation.Manual;
        WindowDecorations = WindowDecorations.None;

        TransparencyLevelHint = new[]
        {
            WindowTransparencyLevel.Transparent
        };

        Background = Brushes.Transparent;
        ShowInTaskbar = false;
        CanResize = false;
        Topmost = true;

        Opened += ReminderWindow_Opened;

        // -------------------------
        // Cartoon character
        // -------------------------

        var character = new Image
        {
            Source = new Bitmap("Assets/winnie.png"),
            Width = 280,
            Height = 320,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            Margin = new Thickness(0, 0, 10, 0)
        };

        // -------------------------
        // Speech bubble
        // -------------------------

        var title = new TextBlock
        {
            Text = "Hey!",
            FontSize = 26,
            FontWeight = FontWeight.Bold,
            Foreground = Brushes.Black
        };

        var message = new TextBlock
        {
            Text = "You've been looking at the screen for a while.\nTake a moment to rest your eyes.",
            FontSize = 17,
            Foreground = Brushes.Black,
            TextWrapping = TextWrapping.Wrap
        };

        // -------------------------
        // Buttons
        // -------------------------

        var skipButton = new Button
        {
            Content = "Maybe later",
            Padding = new Thickness(15, 8)
        };

        var breakButton = new Button
        {
            Content = "Take a break",
            Padding = new Thickness(15, 8)
        };

        skipButton.Click += SkipButton_Click;
        breakButton.Click += TakeBreak_Click;

        var buttons = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 10,
            Margin = new Thickness(0, 10, 0, 0)
        };

        buttons.Children.Add(breakButton);
        buttons.Children.Add(skipButton);

        // -------------------------
        // Speech bubble content
        // -------------------------

        var bubbleContent = new StackPanel
        {
            Spacing = 8
        };

        bubbleContent.Children.Add(title);
        bubbleContent.Children.Add(message);
        bubbleContent.Children.Add(buttons);

        // -------------------------
        // Speech bubble
        // -------------------------

        var bubble = new Border
        {
            Background = Brushes.White,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(2),
            CornerRadius = new CornerRadius(22),
            Padding = new Thickness(22),
            Child = bubbleContent,

            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,

            Margin = new Thickness(20, 30, 0, 0)
        };

        // -------------------------
        // Root
        // -------------------------

        var root = new Grid();

        root.Children.Add(bubble);
        root.Children.Add(character);

        Content = root;
    }

    private async void SkipButton_Click(object? sender, RoutedEventArgs e)
    {
        await App.Database.SetChoiceAsync(
            _reminderId,
            "SKIP"
        );

        Close();
    }

    private async void TakeBreak_Click(object? sender, RoutedEventArgs e)
    {
        await App.Database.SetChoiceAsync(
            _reminderId,
            "TAKE_BREAK"
        );

        var exerciseWindow = new ExerciseWindow(
            _reminderId,
            _videoPath
        );

        exerciseWindow.Show();
        Close();
    }

    private async void ReminderWindow_Opened(object? sender, EventArgs e)
    {
        var screen = Screens.Primary;

        if (screen == null)
            return;

        var workingArea = screen.WorkingArea;

        Position = new PixelPoint(
            workingArea.X + workingArea.Width - (int)Width - 20,
            workingArea.Y + workingArea.Height - (int)Height - 20
        );

        _reminderId = await App.Database.CreateReminderAsync(
            DateTime.UtcNow,
            _videoType
        );
    }
}