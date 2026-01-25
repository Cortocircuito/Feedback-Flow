using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.Services;

public class FileSystemService : IFileSystemService
{
    private readonly string _rootFolderName = "Feedback-Flow";

    public string InitializeDailyFolder()
    {
        try
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string rootPath = Path.Combine(documents, _rootFolderName);

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string dailyPath = Path.Combine(rootPath, dateFolder);

            if (!Directory.Exists(dailyPath))
            {
                Directory.CreateDirectory(dailyPath);
            }

            return dailyPath;
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to initialize folders: {ex.Message}", ex);
        }
    }

    public string CreateStudentFolder(string dailyFolderPath, Student student)
    {
        if (student == null) throw new ArgumentNullException(nameof(student));

        string folderName = student.GetFolderName();
        string studentPath = Path.Combine(dailyFolderPath, folderName);

        if (!Directory.Exists(studentPath))
        {
            Directory.CreateDirectory(studentPath);
        }

        return studentPath;
    }

    public string? GetStudentNoteContent(string studentFolderPath)
    {
        if (!Directory.Exists(studentFolderPath)) return null;

        // Strategy: Look for all .txt files and take the most recent one by Last Write Time.
        var mostRecentFile = new DirectoryInfo(studentFolderPath)
            .GetFiles("*.txt")
            .OrderByDescending(f => f.LastWriteTime)
            .FirstOrDefault();

        if (mostRecentFile == null) return null;

        return File.ReadAllText(mostRecentFile.FullName);
    }

    public void RenameStudentFolder(string dailyFolderPath, string oldName, string newName)
    {
        if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName)) return;
        if (oldName == newName) return;

        // "Jane Doe" -> "Jane-Doe"
        string oldFolderName = oldName.Trim().Replace(" ", "-");
        string newFolderName = newName.Trim().Replace(" ", "-");

        string oldPath = Path.Combine(dailyFolderPath, oldFolderName);
        string newPath = Path.Combine(dailyFolderPath, newFolderName);

        if (Directory.Exists(oldPath) && !Directory.Exists(newPath))
        {
            Directory.Move(oldPath, newPath);
        }
    }
}