using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IFileSystemService
{
    /// <summary>
    /// Ensures the root "Documents/Feedback-Flow" and today's "YYYYMMDD" folders exist.
    /// Returns the path to today's folder.
    /// </summary>
    string InitializeDailyFolder();

    /// <summary>
    /// Ensures a subfolder exists for the specific student within the daily folder.
    /// Returns the student's folder path.
    /// </summary>
    string CreateStudentFolder(string dailyFolderPath, Student student);

    /// <summary>
    /// Reads the content of the student's specific note file (.txt).
    /// </summary>
    string? GetStudentNoteContent(string studentFolderPath);

    /// <summary>
    /// Renames a student folder when their name changes.
    /// </summary>
    void RenameStudentFolder(string dailyFolderPath, string oldName, string newName);
}
