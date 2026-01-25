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

        if (!File.Exists(learningMaterialPath)) throw new FileNotFoundException("Learning Material PDF not found", learningMaterialPath);
        if (!File.Exists(studentPdfPath)) throw new FileNotFoundException("Student PDF not found", studentPdfPath);

        try
        {
            // 1. Create MimeMessage
            var message = new MimeMessage();
            message.Subject = $"Feedback - {DateTime.Now:yyyy-MM-dd}";

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
            // Assuming Root Folder is where other files are? Or App folder?
            // Plan said: "Feedback-Flow root folder". I'll use AppDomain Base or Document Folder? 
            // "TempEmails inside the Feedback-Flow root folder" -> Documents/Feedback-Flow/TempEmails seems safest for user visibility.

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string appRoot = Path.Combine(documents, "Feedback-Flow");
            string tempDir = Path.Combine(appRoot, "TempEmails");

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            string safeName = student.GetFolderName();
            string emlFileName = $"{safeName}_{DateTime.Now:yyyyMMdd-HHmm}.eml";
            string emlPath = Path.Combine(tempDir, emlFileName);

            message.WriteTo(emlPath);

            // 4. Open with Default Mail Client (Outlook)
            var psi = new ProcessStartInfo
            {
                FileName = emlPath,
                UseShellExecute = true
            };

            Process.Start(psi);
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