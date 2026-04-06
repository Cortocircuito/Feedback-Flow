using FeedbackFlow.Core.Models;

namespace FeedbackFlow.Core.Services.Interfaces;

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

    /// <summary>
    /// Returns an existing ClassSession for a specific student and date, or null if none exists.
    /// </summary>
    Task<ClassSession?> GetSessionByStudentAndDateAsync(int studentId, DateTime date);

    /// <summary>
    /// Inserts a blank session row for the student on the given date if one doesn't exist yet,
    /// then returns the session Id.
    /// </summary>
    Task<int> EnsureSessionForStudentAsync(int studentId, DateTime date);

    /// <summary>
    /// Sets the material path and class description on a session row.
    /// </summary>
    Task<bool> UpdateSessionPlanAsync(int sessionId, string? material, string? description);
}
