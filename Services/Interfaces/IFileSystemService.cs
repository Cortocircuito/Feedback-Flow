using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IFileSystemService
{
    /// <summary>
    /// Ensures the root "Documents/Feedback-Flow" and the "YYYYMMDD" folder for
    /// the specified date exist. Returns the path to that date's folder.
    /// </summary>
    string InitializeDailyFolder(DateTime date);

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
