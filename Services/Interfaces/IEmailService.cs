using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IEmailService
{
    /// <summary>
    /// Opens a new email draft with the recipient, subject, and attachments pre-filled.
    /// </summary>
    void DraftEmail(Student student, string studentPdfPath);
}
