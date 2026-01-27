using System.ComponentModel;
using System.Diagnostics;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;
using MimeKit;

namespace Feedback_Flow.Services;

public class OutlookEmailService : IEmailService
{
    public void DraftEmail(Student student, string studentPdfPath)
    {
        if (student == null) throw new ArgumentNullException(nameof(student));
        // Master path is now per student
        string learningMaterialPath = student.LearningMaterialPath;

        if (string.IsNullOrWhiteSpace(learningMaterialPath))
            throw new InvalidOperationException($"No learning material assigned for student {student.FullName}");
        if (string.IsNullOrWhiteSpace(studentPdfPath))
            throw new ArgumentNullException(nameof(studentPdfPath));

        if (!File.Exists(learningMaterialPath))
            throw new FileNotFoundException("Learning Material PDF not found", learningMaterialPath);
        if (!File.Exists(studentPdfPath)) throw new FileNotFoundException("Student PDF not found", studentPdfPath);

        try
        {
            // 1. Create MimeMessage
            var message = new MimeMessage();
            message.Subject = $"Feedback for {student.FullName} - {DateTime.Now:yyyy-MM-dd}";

            // Add Recipient
            if (!string.IsNullOrWhiteSpace(student.Email))
            {
                message.To.Add(new MailboxAddress(student.FullName, student.Email));
            }

            // Add "X-Unsent" header to open as Draft
            message.Headers.Add("X-Unsent", "1");

            // 2. Build Body and Attachments
            var builder = new BodyBuilder
            {
                TextBody =
                    $"Hello {student.FullName},\n\nPlease find attached your class feedback and notes.\n\nBest regards,\nYour Teacher"
            };

            // Add Attachments
            builder.Attachments.Add(learningMaterialPath);
            builder.Attachments.Add(studentPdfPath);

            message.Body = builder.ToMessageBody();

            // 3. Save to Temporary Directory
            // "TempEmails inside the Feedback-Flow root folder"
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string appRoot = Path.Combine(documents, "Feedback-Flow");
            string tempDir = Path.Combine(appRoot, "TempEmails");

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            string safeName = student.GetFolderName();
            // Ensure unique filename if multiple generated in same minute
            string emlFileName =
                $"{safeName}_{DateTime.Now:yyyyMMdd-HHmm}_{Guid.NewGuid().ToString().Substring(0, 4)}.eml";
            string emlPath = Path.Combine(tempDir, emlFileName);

            message.WriteTo(emlPath);

            // 4. Open with Default Mail Client (Outlook)

            // Detect if Outlook is currently running
            // If the process name is different (e.g., Thunderbird), this check simply defaults to adding a safe delay.
            bool isOutlookRunning = Process.GetProcessesByName("OUTLOOK").Length > 0;

            var psi = new ProcessStartInfo
            {
                FileName = emlPath,
                UseShellExecute = true
            };

            Process.Start(psi);

            if (!isOutlookRunning)
            {
                // Outlook was closed and is now starting via the command above.
                // We must wait for it to initialize; otherwise, subsequent emails in the loop 
                // will be ignored/lost during the startup phase.
                Thread.Sleep(5000);
            }
            else
            {
                // Even if running, a small delay prevents overwhelming the inter-process communication
                // and ensures the OS processes the file associations in order.
                Thread.Sleep(500);
            }
        }
        catch (Win32Exception ex)
        {
            throw new InvalidOperationException(
                $"Could not open the generated .eml file. Ensure a default mail client is configured.\nFile: {student.FullName}",
                ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to draft email for {student.FullName}: {ex.Message}", ex);
        }
    }
}