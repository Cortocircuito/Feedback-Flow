using Feedback_Flow.Models;

namespace Feedback_Flow.Services.Interfaces;

public interface IDatabaseService
{
    Task InitializeDatabaseAsync();
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student?> GetStudentByEmailAsync(string email);
    Task<int> AddStudentAsync(Student student);
    Task<bool> UpdateStudentAsync(Student student);
    Task<bool> DeleteStudentAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
}
