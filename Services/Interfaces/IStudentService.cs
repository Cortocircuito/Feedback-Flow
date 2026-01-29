using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IStudentService
{
    Task<List<Student>> LoadStudentsAsync();
    Task AddStudentAsync(Student student);
    Task UpdateStudentAsync(Student originalStudent, Student updatedStudent);
    Task DeleteStudentAsync(Student student);
    Task MarkAttendanceAsync(Student student, bool attended);
    Task<List<Student>> GetStudentsWhoAttendedAsync();
}
