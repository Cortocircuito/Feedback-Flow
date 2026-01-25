using System.Diagnostics;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.Services;

public class NoteService : INoteService
{
    private const string DefaultNoteFileName = "Notes.txt";

    public async Task OpenOrCreateNotesAsync(string studentFolderPath)
    {
        if (string.IsNullOrWhiteSpace(studentFolderPath))
            throw new ArgumentException("Folder path cannot be empty.", nameof(studentFolderPath));

        if (!Directory.Exists(studentFolderPath))
        {
            throw new DirectoryNotFoundException($"The folder for this student does not exist yet at: {studentFolderPath}");
        }

        string notePath = Path.Combine(studentFolderPath, DefaultNoteFileName);

        if (!File.Exists(notePath))
        {
            var existingTxt = Directory.GetFiles(studentFolderPath, "*.txt").FirstOrDefault();
            
            if (existingTxt != null)
            {
                notePath = existingTxt;
            }
            else
            {
                await File.WriteAllTextAsync(notePath, "Student Feedback Notes:\n");
            }
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
