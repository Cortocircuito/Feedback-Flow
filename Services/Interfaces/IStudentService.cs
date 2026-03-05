using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IStudentService
{
    // --- Student Identity CRUD ---
    Task<List<Student>> LoadStudentsAsync();
    Task AddStudentAsync(Student student);
    Task UpdateStudentAsync(Student originalStudent, Student updatedStudent);
    Task DeleteStudentAsync(Student student);
    Task<List<Student>> GetAllStudentsAsync();

    // --- Session-Based Data Access ---

    /// <summary>
    /// Ensures session rows exist for students with class on the given date's weekday,
    /// then returns the joined view for grid binding.
    /// </summary>
    Task<List<StudentSessionView>> GetSessionViewsAsync(DateTime date);

    /// <summary>Marks attendance on a specific session row.</summary>
    Task MarkAttendanceAsync(int sessionId, bool attended);

    /// <summary>Sets or clears the material path on a specific session row.</summary>
    Task UpdateSessionMaterialAsync(int sessionId, string? materialPath);

    /// <summary>Returns the English day-of-week name for the given date.</summary>
    string GetDayOfWeek(DateTime date);

    /// <summary>
    /// Returns the existing ClassSession for the student on the given date, or null if none exists yet.
    /// </summary>
    Task<ClassSession?> GetNextClassSessionAsync(int studentId, DateTime nextDate);

    /// <summary>
    /// Creates the session row if needed, then saves material and class description to it.
    /// </summary>
    Task SaveNextClassPlanAsync(int studentId, DateTime nextDate, string? material, string? description);
}
