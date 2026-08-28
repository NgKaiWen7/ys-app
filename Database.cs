using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HealthyWindows;

public class Database
{
    private readonly string _connectionString;

    public Database()
    {
        var dataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "HealthyWindows"
        );

        Directory.CreateDirectory(dataDirectory);

        var databasePath = Path.Combine(
            dataDirectory,
            "healthywindows.db"
        );

        _connectionString = $"Data Source={databasePath}";
    }

    public async Task InitializeAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();

        command.CommandText = """
            CREATE TABLE IF NOT EXISTS reminders (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                reminder_datetime TEXT NOT NULL,
                user_choice TEXT,
                watch_count INTEGER NOT NULL DEFAULT 0,
                video_type TEXT
            );
            """;

        await command.ExecuteNonQueryAsync();
    }

    public async Task<long> CreateReminderAsync(
        DateTime reminderDateTime,
        string videoType)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO reminders (
                reminder_datetime,
                user_choice,
                watch_count,
                video_type
            )
            VALUES (
                $datetime,
                NULL,
                0,
                $videoType
            );

            SELECT last_insert_rowid();
            """;

        command.Parameters.AddWithValue(
            "$datetime",
            reminderDateTime.ToUniversalTime().ToString("O")
        );

        command.Parameters.AddWithValue("$videoType", videoType);

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt64(result);
    }

    public async Task SetChoiceAsync(
        long reminderId,
        string choice)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();

        command.CommandText = """
            UPDATE reminders
            SET user_choice = $choice
            WHERE id = $id;
            """;

        command.Parameters.AddWithValue("$choice", choice);
        command.Parameters.AddWithValue("$id", reminderId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task IncrementWatchCountAsync(
        long reminderId)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();

        command.CommandText = """
            UPDATE reminders
            SET watch_count = watch_count + 1
            WHERE id = $id;
            """;

        command.Parameters.AddWithValue("$id", reminderId);

        await command.ExecuteNonQueryAsync();
    }
}