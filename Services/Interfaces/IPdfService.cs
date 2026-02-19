using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IPdfService
{
    /// <summary>
    /// Generates a PDF for the student based on their note content.
    /// Returns the absolute path of the generated PDF.
    /// </summary>
    string GenerateStudentPdf(Student student, string content, string outputFolder);
}
