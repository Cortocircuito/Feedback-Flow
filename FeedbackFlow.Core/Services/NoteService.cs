using System.Diagnostics;
using FeedbackFlow.Core.Services.Interfaces;

namespace FeedbackFlow.Core.Services;

public class NoteService : INoteService
{
    public async Task OpenOrCreateNotesAsync(string studentFolderPath, string studentName, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(studentFolderPath))
            throw new ArgumentException("Folder path cannot be empty.", nameof(studentFolderPath));

        if (!Directory.Exists(studentFolderPath))
        {
            throw new DirectoryNotFoundException($"The folder for this student does not exist yet at: {studentFolderPath}");
        }

        string notePath;

        var existingTxt = Directory.GetFiles(studentFolderPath, "*.txt")
            .OrderByDescending(File.GetLastWriteTime)
            .FirstOrDefault();

        if (existingTxt != null)
        {
            notePath = existingTxt;
        }
        else
        {
            // Format: feedback-alumn-name-yyyyMMdd.txt
            // Sanitize name for filename
            var sanitizedName = studentName.Trim().Replace(" ", "-").ToLower();
            var dateStamp = date.ToString("yyyyMMdd");
            var fileName = $"feedback-{sanitizedName}-{dateStamp}.txt";

            notePath = Path.Combine(studentFolderPath, fileName);

            await File.WriteAllTextAsync(notePath, string.Empty);
        }

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = notePath,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Could not open the text editor. {ex.Message}", ex);
        }
    }
}
