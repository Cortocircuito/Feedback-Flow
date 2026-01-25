namespace Feedback_Flow.Services.Interfaces;

public interface INoteService
{
    /// <summary>
    /// Opens the student's note file in the default text editor.
    /// Creates the file if it doesn't exist, using a custom naming convention.
    /// </summary>
    /// <param name="studentFolderPath">Absolute path to the student's folder.</param>
    /// <param name="studentName">The name of the student for file naming.</param>
    Task OpenOrCreateNotesAsync(string studentFolderPath, string studentName);
}
