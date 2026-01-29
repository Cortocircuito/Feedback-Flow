using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.Services;

public class StudentService : IStudentService
{
    private readonly IDatabaseService _db;
    private readonly IFileSystemService _fileService;
    private readonly string _dailyFolderPath;

    public StudentService(IDatabaseService db, IFileSystemService fileService)
    {
        _db = db;
        _fileService = fileService;

        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string dateFolder = DateTime.Now.ToString("yyyyMMdd");
        string expectedDailyPath = Path.Combine(documents, "Feedback-Flow", dateFolder);

        bool isNewDay = !Directory.Exists(expectedDailyPath);

        _dailyFolderPath = _fileService.InitializeDailyFolder();

        if (isNewDay)
        {
            Task.Run(async () => await PerformDailyResetAsync()).Wait();
        }
    }

    private async Task PerformDailyResetAsync()
    {
        try 
        {
            var students = await _db.GetAllStudentsAsync();
            foreach (var s in students)
            {
                if (!string.IsNullOrEmpty(s.AssignedMaterial) || s.AttendedClass)
                {
                    s.AssignedMaterial = string.Empty;
                    s.AttendedClass = false;
                    await _db.UpdateStudentAsync(s);
                }
            }
        }
        catch
        {
            // Ignore init errors
        }
    }

    public async Task<List<Student>> LoadStudentsAsync()
    {
        var students = await _db.GetAllStudentsAsync();
        return students.ToList();
    }

    public async Task AddStudentAsync(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        student.Id = await _db.AddStudentAsync(student);
        _fileService.CreateStudentFolder(_dailyFolderPath, student);
    }

    public async Task UpdateStudentAsync(Student originalStudent, Student updatedStudent)
    {
        ArgumentNullException.ThrowIfNull(originalStudent);
        ArgumentNullException.ThrowIfNull(updatedStudent);

        updatedStudent.Id = originalStudent.Id;
        await _db.UpdateStudentAsync(updatedStudent);

        if (!originalStudent.FullName.Equals(updatedStudent.FullName, StringComparison.OrdinalIgnoreCase))
        {
            _fileService.RenameStudentFolder(_dailyFolderPath, originalStudent.FullName, updatedStudent.FullName);
        }
    }

    public async Task DeleteStudentAsync(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        await _db.DeleteStudentAsync(student.Id);
    }

    public async Task MarkAttendanceAsync(Student student, bool attended)
    {
        ArgumentNullException.ThrowIfNull(student);
        student.AttendedClass = attended;
        await _db.UpdateAttendanceAsync(student.Id, attended);
    }

    public async Task<List<Student>> GetStudentsWhoAttendedAsync()
    {
        var attended = await _db.GetStudentsWhoAttendedAsync();
        return attended.ToList();
    }
}
