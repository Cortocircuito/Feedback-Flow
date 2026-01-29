using System.Data;
using Dapper;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;
using Microsoft.Data.Sqlite;

namespace Feedback_Flow.Services;

public class SqliteDatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    private readonly string _dbPath;

    public SqliteDatabaseService()
    {
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string appFolder = Path.Combine(documents, "Feedback-Flow");
        
        if (!Directory.Exists(appFolder))
        {
            Directory.CreateDirectory(appFolder);
        }

        _dbPath = Path.Combine(appFolder, "feedbackflow.db");
        _connectionString = $"Data Source={_dbPath}";
    }

    private SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public async Task InitializeDatabaseAsync()
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Students (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                AssignedMaterial TEXT NULL,
                AttendedClass INTEGER NOT NULL DEFAULT 0,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL
            );";

        await connection.ExecuteAsync(createTableQuery);

        // Migration: Ensure AttendedClass column exists
        try 
        {
            await connection.ExecuteAsync("SELECT AttendedClass FROM Students LIMIT 1");
        }
        catch
        {
            await connection.ExecuteAsync("ALTER TABLE Students ADD COLUMN AttendedClass INTEGER NOT NULL DEFAULT 0;");
        }
    }

    public async Task<bool> UpdateAttendanceAsync(int studentId, bool attended)
    {
        using var connection = CreateConnection();
        string sql = "UPDATE Students SET AttendedClass = @Attended, UpdatedAt = @UpdatedAt WHERE Id = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Attended = attended ? 1 : 0, UpdatedAt = DateTime.UtcNow.ToString("o"), Id = studentId });
        return rows > 0;
    }

    public async Task<IEnumerable<Student>> GetStudentsWhoAttendedAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Student>("SELECT * FROM Students WHERE AttendedClass = 1 ORDER BY FullName");
    }

    public async Task<int> GetAttendanceCountAsync()
    {
        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Students WHERE AttendedClass = 1");
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        using var connection = CreateConnection();
        var students = await connection.QueryAsync<Student>("SELECT * FROM Students ORDER BY FullName");
        return students;
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Student>("SELECT * FROM Students WHERE Id = @Id", new { Id = id });
    }

    public async Task<Student?> GetStudentByEmailAsync(string email)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Student>("SELECT * FROM Students WHERE Email = @Email", new { Email = email });
    }

    public async Task<int> AddStudentAsync(Student student)
    {
        using var connection = CreateConnection();
        
        // Ensure audit fields are set if missing
        if (string.IsNullOrWhiteSpace(student.CreatedAt)) student.CreatedAt = DateTime.UtcNow.ToString("o");
        if (string.IsNullOrWhiteSpace(student.UpdatedAt)) student.UpdatedAt = DateTime.UtcNow.ToString("o");

        string sql = @"
            INSERT INTO Students (FullName, Email, AssignedMaterial, CreatedAt, UpdatedAt)
            VALUES (@FullName, @Email, @AssignedMaterial, @CreatedAt, @UpdatedAt);
            SELECT last_insert_rowid();";

        return await connection.ExecuteScalarAsync<int>(sql, student);
    }

    public async Task<bool> UpdateStudentAsync(Student student)
    {
        using var connection = CreateConnection();
        
        student.UpdatedAt = DateTime.UtcNow.ToString("o");

        string sql = @"
            UPDATE Students 
            SET FullName = @FullName, 
                Email = @Email, 
                AssignedMaterial = @AssignedMaterial, 
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id";

        var rows = await connection.ExecuteAsync(sql, student);
        return rows > 0;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        using var connection = CreateConnection();
        var rows = await connection.ExecuteAsync("DELETE FROM Students WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        using var connection = CreateConnection();
        string sql = "SELECT COUNT(1) FROM Students WHERE Email = @Email";
        
        if (excludeId.HasValue)
        {
            sql += " AND Id != @ExcludeId";
        }

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email, ExcludeId = excludeId });
        return count > 0;
    }
}
