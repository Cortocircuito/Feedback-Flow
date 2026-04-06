using System.ComponentModel;
using System.Diagnostics;
using FeedbackFlow.Core.Models;
using FeedbackFlow.Core.Services.Interfaces;
using MimeKit;

namespace FeedbackFlow.Core.Services;

public class OutlookEmailService : IEmailService
{
    public void DraftEmail(StudentSessionView session, string feedbackReportPath)
    {
        if (session == null) throw new ArgumentNullException(nameof(session));
        if (string.IsNullOrWhiteSpace(feedbackReportPath))
            throw new ArgumentNullException(nameof(feedbackReportPath));
        if (!File.Exists(feedbackReportPath))
            throw new FileNotFoundException("Feedback report file not found", feedbackReportPath);

        string learningMaterialPath = session.AssignedMaterial ?? string.Empty;

        try
        {
            var message = new MimeMessage();
            message.Subject = $"Feedback for {session.FullName} - {DateTime.Now:yyyy-MM-dd}";

            if (!string.IsNullOrWhiteSpace(session.Email))
                message.To.Add(new MailboxAddress(session.FullName, session.Email));

            message.Headers.Add("X-Unsent", "1");

            var builder = new BodyBuilder
            {
                TextBody = $"Hello {session.FullName},\n\nPlease find attached your class feedback and notes.\n\nBest regards,\nYour Teacher"
            };

            // Material is optional — only attach if path exists
            if (!string.IsNullOrWhiteSpace(learningMaterialPath) && File.Exists(learningMaterialPath))
                builder.Attachments.Add(learningMaterialPath);

            builder.Attachments.Add(feedbackReportPath);
            message.Body = builder.ToMessageBody();

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string tempDir = Path.Combine(documents, "Feedback-Flow", "TempEmails");

            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            string safeName = session.GetFolderName();
            string emlFileName = $"{safeName}_{DateTime.Now:yyyyMMdd-HHmm}_{Guid.NewGuid().ToString()[..4]}.eml";
            string emlPath = Path.Combine(tempDir, emlFileName);

            message.WriteTo(emlPath);

            bool isOutlookRunning = Process.GetProcessesByName("OUTLOOK").Length > 0;

            Process.Start(new ProcessStartInfo { FileName = emlPath, UseShellExecute = true });

            Thread.Sleep(isOutlookRunning ? 500 : 5000);
        }
        catch (Win32Exception ex)
        {
            throw new InvalidOperationException(
                $"Could not open the generated .eml file. Ensure a default mail client is configured.\nFile: {session.FullName}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to draft email for {session.FullName}: {ex.Message}", ex);
        }
    }
}