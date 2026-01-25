using System.Diagnostics;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.Services;

public class NoteService : INoteService
{
    public async Task OpenOrCreateNotesAsync(string studentFolderPath, string studentName)
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
            var date = DateTime.Now.ToString("yyyyMMdd");
            var fileName = $"feedback-{sanitizedName}-{date}.txt";

            notePath = Path.Combine(studentFolderPath, fileName);

            var content = $"{studentName} Feedback Notes:\n";
            await File.WriteAllTextAsync(notePath, content);
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
