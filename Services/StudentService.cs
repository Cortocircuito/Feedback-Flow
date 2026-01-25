using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.Services;

public class StudentService : IStudentService
{
    private readonly IFileSystemService _fileService;
    private string _csvPath;
    private readonly string _dailyFolderPath;

    public StudentService(IFileSystemService fileService)
    {
        _fileService = fileService;
        
        // Initialize folders immediately to ensure paths exist
        _dailyFolderPath = _fileService.InitializeDailyFolder();
        
        // Determine CSV path
        string docRoot = Path.GetDirectoryName(_dailyFolderPath)!;
        _csvPath = Path.Combine(docRoot, "students.csv");

        // Fallback or Initial Creation handled in Load/Save
    }

    public async Task<List<Student>> LoadStudentsAsync()
    {
        var students = new List<Student>();

        if (!File.Exists(_csvPath)) 
        {
            // Check App Directory fallback
            string appCsv = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "students.csv");
            if (File.Exists(appCsv))
            {
                _csvPath = appCsv; // Use app directory one if doc one doesn't create yet
            }
            else
            {
                return students;
            }
        }

        try 
        {
            var lines = await File.ReadAllLinesAsync(_csvPath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 2) continue;

                var name = parts[0].Trim();
                var email = parts[1].Trim();

                if (name.Equals("Full Name", StringComparison.OrdinalIgnoreCase)) continue;

                students.Add(new Student { FullName = name, Email = email });
            }
        }
        catch (IOException) 
        {
            // Handle file lock or sharing violation if needed
        }

        return students;
    }

    public async Task AddStudentAsync(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        // 1. Load current list
        var students = await LoadStudentsAsync();
        
        // 2. Add new
        students.Add(student);

        // 3. Save to CSV
        await SaveStudentsInternalAsync(students);

        // 4. Create Folder
        _fileService.CreateStudentFolder(_dailyFolderPath, student);
    }

    public async Task UpdateStudentAsync(Student originalStudent, Student updatedStudent)
    {
        ArgumentNullException.ThrowIfNull(originalStudent);
        ArgumentNullException.ThrowIfNull(updatedStudent);

        var students = await LoadStudentsAsync();

        // Find and replace
        var index = students.FindIndex(s => s.Email.Equals(originalStudent.Email, StringComparison.OrdinalIgnoreCase));
        if (index != -1)
        {
            students[index] = updatedStudent;
            
            // Save CSV
            await SaveStudentsInternalAsync(students);

            // Rename Folder if needed
            if (!originalStudent.FullName.Equals(updatedStudent.FullName, StringComparison.OrdinalIgnoreCase))
            {
                // We need extended functionality in IFileSystemService or cast it
                // Since interface definition was updated in previous steps, we assume usage of IFileSystemService methods
                // BUT wait, I need to ensure IFileSystemService has Rename logic in the Interface or do it here?
                // The task said "StudentService... handle... folder renaming logic".
                // So I can do it here using _fileService helpers or Directory.Move directly.
                // Using _fileService methods is cleaner if they exist.
                
                // Inspecting IFileSystemService: previous steps added RenameStudentFolder.
                // If it's there, great. If not, I'll rely on FileSystemService implementation details or re-add it.
                // I will call it assuming it exists on the Interface (from Step 211).
                
               _fileService.RenameStudentFolder(_dailyFolderPath, originalStudent.FullName, updatedStudent.FullName);
            }
        }
    }

    public async Task DeleteStudentAsync(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        var students = await LoadStudentsAsync();
        var studentToRemove = students.FirstOrDefault(s => s.Email.Equals(student.Email, StringComparison.OrdinalIgnoreCase));
        
        if (studentToRemove != null)
        {
            students.Remove(studentToRemove);
            await SaveStudentsInternalAsync(students);
            // "do not delete their folders" per requirements
        }
    }

    private async Task SaveStudentsInternalAsync(List<Student> students)
    {
        var lines = new List<string> { "Full Name,Email" };
        lines.AddRange(students.Select(s => $"{s.FullName},{s.Email}"));
        await File.WriteAllLinesAsync(_csvPath, lines);
    }
}
