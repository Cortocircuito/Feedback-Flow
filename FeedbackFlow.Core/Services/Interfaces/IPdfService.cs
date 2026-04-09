using FeedbackFlow.Core.Models;

namespace FeedbackFlow.Core.Services.Interfaces;

public interface IPdfService
{
    /// <summary>
    /// Generates a PDF for the student session based on their note content.
    /// Returns the absolute path of the generated PDF.
    /// </summary>
    string GenerateStudentPdf(StudentSessionView session, string content, string outputFolder);
}
