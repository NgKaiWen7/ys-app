using Avalonia.Controls;
using Avalonia.Layout;

namespace HealthyWindows;

public class MainWindow : Window
{
    public MainWindow()
    {
        Width = 500;
        Height = 300;
        Title = "Healthy Windows";

        var title = new TextBlock
        {
            Text = "Healthy Windows",
            FontSize = 32,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var button = new Button
        {
            Content = "Show Reminder",
            Width = 160,
            Height = 40
        };

        button.Click += ShowReminder_Click;

        var panel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 15
        };

        panel.Children.Add(title);
        panel.Children.Add(button);

        Content = panel;
    }

    private void ShowReminder_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var reminder = new ReminderWindow();
        reminder.Show();
    }
}