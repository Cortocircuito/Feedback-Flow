using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IEmailService
{
    /// <summary>
    /// Opens a new email draft with the recipient, subject, and attachments pre-filled.
    /// </summary>
    /// <param name="feedbackReportPath">Absolute path to the generated feedback report (PDF) to attach.</param>
    void DraftEmail(StudentSessionView session, string feedbackReportPath);
}
