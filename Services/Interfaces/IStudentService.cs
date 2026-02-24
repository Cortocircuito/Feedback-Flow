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
    /// Ensures today's session rows exist for all students with class on the current day,
    /// then returns the joined view for grid binding.
    /// </summary>
    Task<List<StudentSessionView>> GetTodaySessionViewsAsync();

    /// <summary>Marks attendance on a specific session row.</summary>
    Task MarkAttendanceAsync(int sessionId, bool attended);

    /// <summary>Sets or clears the material path on a specific session row.</summary>
    Task UpdateSessionMaterialAsync(int sessionId, string? materialPath);

    string GetCurrentDayOfWeek();
}
