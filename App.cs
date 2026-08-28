using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace HealthyWindows;

public class App : Application
{
    public static Database Database { get; private set; } = null!;

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Database = new Database();
            Database.InitializeAsync().GetAwaiter().GetResult();

            var reminderWindow = new ReminderWindow(
                "EYE_EXERCISE",
                "Assets/exercise.mp4"
            );

            reminderWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }
}