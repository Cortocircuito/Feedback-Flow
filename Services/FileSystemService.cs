using System;
using System.IO;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly string _rootFolderName = "Feedback-Flow";

        public string InitializeDailyFolder()
        {
            try
            {
                string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string rootPath = Path.Combine(documents, _rootFolderName);

                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                string dateFolder = DateTime.Now.ToString("yyyyMMdd");
                string dailyPath = Path.Combine(rootPath, dateFolder);

                if (!Directory.Exists(dailyPath))
                {
                    Directory.CreateDirectory(dailyPath);
                }

                return dailyPath;
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to initialize folders: {ex.Message}", ex);
            }
        }

        public string CreateStudentFolder(string dailyFolderPath, Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));

            string folderName = student.GetFolderName();
            string studentPath = Path.Combine(dailyFolderPath, folderName);

            if (!Directory.Exists(studentPath))
            {
                Directory.CreateDirectory(studentPath);
            }

            return studentPath;
        }

        public string? GetStudentNoteContent(string studentFolderPath)
        {
            if (!Directory.Exists(studentFolderPath)) return null;

            // Strategy: Look for any .txt file. 
            // If multiple, take the first one? Or specific named one?
            // "The app must find the .txt file inside the student's specific subfolder"
            // We'll assume any .txt file is the note.
            string[] txtFiles = Directory.GetFiles(studentFolderPath, "*.txt");

            if (txtFiles.Length == 0) return null;

            // Return content of the first found text file
            return File.ReadAllText(txtFiles[0]);
        }
    }
}
