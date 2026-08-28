using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Interactivity;
using LibVLCSharp.Shared;
using LibVLCSharp.Avalonia;
using System;
using System.IO;
namespace HealthyWindows;

public class ExerciseWindow : Window
{
    private readonly VideoView _videoView;
    private readonly Button _redoButton;
    private readonly Button _closeButton;

    private readonly LibVLC _libVLC;
    private readonly MediaPlayer _mediaPlayer;

    private Media? _media;
    private readonly long _reminderId;
    private readonly string _videoPath;
    public ExerciseWindow(long reminderId, string videoPath)
    {
        Width = 700;
        Height = 500;
        _videoPath = videoPath;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        Core.Initialize();

        _libVLC = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVLC);

        _videoView = new VideoView
        {
            Width = 650,
            Height = 400,
            MediaPlayer = _mediaPlayer
        };

        _redoButton = new Button
        {
            Content = "Redo",
            Width = 100,
            IsVisible = false
        };

        _closeButton = new Button
        {
            Content = "Close",
            Width = 100,
            IsVisible = false
        };

        _redoButton.Click += RedoButton_Click;
        _closeButton.Click += CloseButton_Click;
        _mediaPlayer.EndReached += VideoEnded;

        var buttons = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Spacing = 20,
            Children =
            {
                _redoButton,
                _closeButton
            }
        };

        Content = new StackPanel
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Spacing = 20,
            Children =
            {
                _videoView,
                buttons
            }
        };

        Opened += ExerciseWindow_Opened;
        Closed += ExerciseWindow_Closed;
    }

    private void ExerciseWindow_Opened(object? sender, EventArgs e)
    {
        PlayExercise();
    }

    private void PlayExercise()
    {
        var videoPath = Path.Combine(
            AppContext.BaseDirectory,
            _videoPath
        );

        _media?.Dispose();

        _media = new Media(
            _libVLC,
            videoPath,
            FromType.FromPath
        );

        _mediaPlayer.Play(_media);
    }

    private void VideoEnded(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Post(async () =>
        {
            await App.Database.IncrementWatchCountAsync(_reminderId);

            _redoButton.IsVisible = true;
            _closeButton.IsVisible = true;
        });
    }

    private async void RedoButton_Click(object? sender, RoutedEventArgs e)
    {
        _redoButton.IsVisible = false;
        _closeButton.IsVisible = false;

        _mediaPlayer.Stop();

        PlayExercise();

        await App.Database.IncrementWatchCountAsync(_reminderId);
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ExerciseWindow_Closed(object? sender, EventArgs e)
    {
        _mediaPlayer.EndReached -= VideoEnded;

        _mediaPlayer.Stop();

        _media?.Dispose();
        _mediaPlayer.Dispose();
        _libVLC.Dispose();
    }
}