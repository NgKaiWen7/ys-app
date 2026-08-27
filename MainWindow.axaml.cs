using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HealthyWindows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ShowReminder_Click(object? sender, RoutedEventArgs e)
    {
        var reminder = new ReminderWindow();

        reminder.Show();
    }
}