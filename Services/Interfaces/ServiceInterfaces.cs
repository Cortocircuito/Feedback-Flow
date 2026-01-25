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
    /// Expected file location: {StudentDateFolder}/{StudentName}.txt or similar convention?
    /// Plan said: "find the .txt file inside the student's specific subfolder"
    /// </summary>
    string? GetStudentNoteContent(string studentFolderPath);
}

public interface IDataService
{
    /// <summary>
    /// Loads students from the CSV file.
    /// </summary>
    IEnumerable<Student> LoadStudents(string csvPath);
}

public interface IPdfService
{
    /// <summary>
    /// Generates a PDF for the student based on their note content.
    /// Returns the absolute path of the generated PDF.
    /// </summary>
    string GenerateStudentPdf(Student student, string content, string outputFolder);
}

public interface IEmailService
{
    /// <summary>
    /// Opens a new Outlook email window with recipients and attachments pre-filled.
    /// </summary>
    void DraftEmail(Student student, string masterPdfPath, string studentPdfPath);
}