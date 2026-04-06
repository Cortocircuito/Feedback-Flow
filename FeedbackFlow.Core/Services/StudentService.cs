using FeedbackFlow.Core.Models;
using FeedbackFlow.Core.Services.Interfaces;

namespace FeedbackFlow.Core.Services;

public class StudentService : IStudentService
{
    private readonly IDatabaseService _db;
    private readonly IFileSystemService _fileService;
    private readonly string _dailyFolderPath;

    public StudentService(IDatabaseService db, IFileSystemService fileService)
    {
        _db = db;
        _fileService = fileService;
        _dailyFolderPath = _fileService.InitializeDailyFolder(DateTime.Today);
        // Daily reset logic removed — session data is preserved historically.
    }

    // -------------------------------------------------------------------------
    // Student Identity CRUD
    // -------------------------------------------------------------------------

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
        // ClassSessions are cleaned up automatically via ON DELETE CASCADE
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await LoadStudentsAsync();
    }

    // -------------------------------------------------------------------------
    // Session-Based Data Access
    // -------------------------------------------------------------------------

    public async Task<List<StudentSessionView>> GetSessionViewsAsync(DateTime date)
    {
        string dayName = GetDayOfWeek(date);

        // Only auto-create sessions for the current day to avoid mutating historical data.
        if (date.Date == DateTime.Today)
        {
            await _db.EnsureTodaySessionsAsync(dayName, date);
        }

        var views = await _db.GetTodaySessionViewsAsync(dayName, date);
        return views.ToList();
    }

    public async Task MarkAttendanceAsync(int sessionId, bool attended)
    {
        await _db.UpdateSessionAttendanceAsync(sessionId, attended);
    }

    public async Task UpdateSessionMaterialAsync(int sessionId, string? materialPath)
    {
        await _db.UpdateSessionMaterialAsync(sessionId, materialPath);
    }

    public string GetDayOfWeek(DateTime date)
    {
        return date.ToString("dddd", System.Globalization.CultureInfo.InvariantCulture);
    }

    public async Task<ClassSession?> GetNextClassSessionAsync(int studentId, DateTime nextDate)
    {
        return await _db.GetSessionByStudentAndDateAsync(studentId, nextDate);
    }

    public async Task SaveNextClassPlanAsync(int studentId, DateTime nextDate, string? material, string? description)
    {
        int sessionId = await _db.EnsureSessionForStudentAsync(studentId, nextDate);
        await _db.UpdateSessionPlanAsync(sessionId, material, description);
    }
}
