using System.Security;
using System.Text.Json;

namespace Feedback_Flow.UI.Theme;

/// <summary>
/// Persists user preferences (currently only the dark-mode flag) to a small
/// JSON file under the user's local application data folder.
/// </summary>
internal sealed class UserPreferences
{
    private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "FeedbackFlow",
        "preferences.json");

    public bool DarkMode { get; set; }

    /// <summary>
    /// Loads saved preferences.  Returns a default instance if the file does
    /// not exist or cannot be read.
    /// </summary>
    public static UserPreferences Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<UserPreferences>(json)
                    ?? new UserPreferences();
            }
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            // Intentionally ignored: corrupt, missing, or unreadable file → fall back to defaults.
        }

        return new UserPreferences();
    }

    /// <summary>Persists the current preferences.  Errors are silently swallowed.</summary>
    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or SecurityException)
        {
            // Intentionally ignored: non-critical – a failure to persist the preference
            // is recoverable (the default light mode is used on next launch).
        }
    }
}
