using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace HealthyWindows;

public class ReminderWindow : Window
{
    public ReminderWindow()
    {
        Width = 500;
        Height = 300;

        WindowDecorations = WindowDecorations.None;
        TransparencyLevelHint = new[] { WindowTransparencyLevel.Transparent };
        Background = Brushes.Transparent;
        ShowInTaskbar = false;
        CanResize = false;

        var image = new Image
        {
            Source = new Bitmap("Assets/winnie.png"),
            Stretch = Stretch.Fill
        };

        var title = new TextBlock
        {
            Text = "Time for a break",
            FontSize = 24,
            FontWeight = FontWeight.Bold
        };

        var message = new TextBlock
        {
            Text = "Take a moment to rest your eyes.",
            FontSize = 16
        };

        var skipButton = new Button
        {
            Content = "Skip"
        };

        var breakButton = new Button
        {
            Content = "Take a break"
        };

        skipButton.Click += (_, _) => Close();
        breakButton.Click += (_, _) => TakeBreak();

        var buttons = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        buttons.Children.Add(skipButton);
        buttons.Children.Add(breakButton);

        var content = new StackPanel
        {
            Spacing = 10,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };

        content.Children.Add(title);
        content.Children.Add(message);
        content.Children.Add(buttons);

        var root = new Grid();
        // root.Children.Add(image);
        root.Children.Add(content);

        Content = root;
    }

    private void TakeBreak()
    {
        Close();

        // Your break logic here
    }
}