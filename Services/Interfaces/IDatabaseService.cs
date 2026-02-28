using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IDatabaseService
{
    Task InitializeDatabaseAsync();

    // --- Student CRUD ---
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student?> GetStudentByEmailAsync(string email);
    Task<int> AddStudentAsync(Student student);
    Task<bool> UpdateStudentAsync(Student student);
    Task<bool> DeleteStudentAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);

    // --- ClassSession Operations ---

    /// <summary>
    /// Ensures a ClassSession row exists for every student with class on <paramref name="dayName"/>
    /// for the given <paramref name="date"/>. Safe to call multiple times (uses INSERT OR IGNORE).
    /// </summary>
    Task EnsureTodaySessionsAsync(string dayName, DateTime date);

    /// <summary>
    /// Returns a joined view of Students + today's ClassSessions for the given day/date.
    /// </summary>
    Task<IEnumerable<StudentSessionView>> GetTodaySessionViewsAsync(string dayName, DateTime date);

    Task<bool> UpdateSessionAttendanceAsync(int sessionId, bool attended);
    Task<bool> UpdateSessionMaterialAsync(int sessionId, string? materialPath);
}
