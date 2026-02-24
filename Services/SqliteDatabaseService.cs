using System.Data;
using Dapper;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;
using Microsoft.Data.Sqlite;

namespace Feedback_Flow.Services;

public class SqliteDatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public SqliteDatabaseService()
    {
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string appFolder = Path.Combine(documents, "Feedback-Flow");

        if (!Directory.Exists(appFolder))
        {
            Directory.CreateDirectory(appFolder);
        }

        string dbPath = Path.Combine(appFolder, "feedbackflow.db");
        _connectionString = $"Data Source={dbPath}";
    }

    private SqliteConnection CreateConnection() => new SqliteConnection(_connectionString);

    // -------------------------------------------------------------------------
    // Schema Initialization
    // -------------------------------------------------------------------------

    public async Task InitializeDatabaseAsync()
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        // Students table — identity only, no transient session data
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS Students (
                Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName  TEXT NOT NULL,
                Email     TEXT NOT NULL UNIQUE,
                ClassDay  TEXT NOT NULL DEFAULT '',
                CreatedAt TEXT NOT NULL
            );");

        // Migration: Add ClassDay column to existing databases that predate this column
        try { await connection.ExecuteAsync("SELECT ClassDay FROM Students LIMIT 1"); }
        catch { await connection.ExecuteAsync("ALTER TABLE Students ADD COLUMN ClassDay TEXT NOT NULL DEFAULT '';"); }

        // ClassSessions table — one row per student per date
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS ClassSessions (
                Id               INTEGER PRIMARY KEY AUTOINCREMENT,
                StudentId        INTEGER NOT NULL,
                ClassDate        TEXT    NOT NULL,
                Attended         INTEGER NOT NULL DEFAULT 0,
                AssignedMaterial TEXT    NULL,
                FeedbackNotePath TEXT    NULL,
                FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE CASCADE
            );");

        // Unique index prevents duplicate sessions for the same student+date
        await connection.ExecuteAsync(@"
            CREATE UNIQUE INDEX IF NOT EXISTS IX_ClassSessions_Student_Date
                ON ClassSessions(StudentId, ClassDate);");
    }

    // -------------------------------------------------------------------------
    // Student CRUD
    // -------------------------------------------------------------------------

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Student>("SELECT * FROM Students ORDER BY FullName");
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Student>(
            "SELECT * FROM Students WHERE Id = @Id", new { Id = id });
    }

    public async Task<Student?> GetStudentByEmailAsync(string email)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Student>(
            "SELECT * FROM Students WHERE Email = @Email", new { Email = email });
    }

    public async Task<int> AddStudentAsync(Student student)
    {
        using var connection = CreateConnection();

        if (string.IsNullOrWhiteSpace(student.CreatedAt))
            student.CreatedAt = DateTime.UtcNow.ToString("o");

        const string sql = @"
            INSERT INTO Students (FullName, Email, ClassDay, CreatedAt)
            VALUES (@FullName, @Email, @ClassDay, @CreatedAt);
            SELECT last_insert_rowid();";

        return await connection.ExecuteScalarAsync<int>(sql, student);
    }

    public async Task<bool> UpdateStudentAsync(Student student)
    {
        using var connection = CreateConnection();

        const string sql = @"
            UPDATE Students
            SET FullName  = @FullName,
                Email     = @Email,
                ClassDay  = @ClassDay
            WHERE Id = @Id";

        var rows = await connection.ExecuteAsync(sql, student);
        return rows > 0;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        using var connection = CreateConnection();
        // ON DELETE CASCADE handles ClassSessions cleanup automatically
        var rows = await connection.ExecuteAsync("DELETE FROM Students WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        using var connection = CreateConnection();
        string sql = "SELECT COUNT(1) FROM Students WHERE Email = @Email";
        if (excludeId.HasValue) sql += " AND Id != @ExcludeId";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email, ExcludeId = excludeId });
        return count > 0;
    }

    // -------------------------------------------------------------------------
    // ClassSession Operations
    // -------------------------------------------------------------------------

    public async Task EnsureTodaySessionsAsync(string dayName, DateTime date)
    {
        using var connection = CreateConnection();
        // Insert a session row for every student whose ClassDay matches today.
        // INSERT OR IGNORE + unique index makes this fully idempotent.
        const string sql = @"
            INSERT OR IGNORE INTO ClassSessions (StudentId, ClassDate, Attended)
            SELECT Id, @Date, 0
            FROM Students
            WHERE ClassDay LIKE '%' || @DayName || '%';";

        await connection.ExecuteAsync(sql, new
        {
            Date    = date.ToString("yyyy-MM-dd"),
            DayName = dayName
        });
    }

    public async Task<IEnumerable<StudentSessionView>> GetTodaySessionViewsAsync(string dayName, DateTime date)
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT s.Id   AS StudentId,
                   s.FullName,
                   s.Email,
                   s.ClassDay,
                   cs.Id  AS SessionId,
                   cs.Attended,
                   cs.AssignedMaterial,
                   cs.FeedbackNotePath
            FROM   Students s
            INNER  JOIN ClassSessions cs ON cs.StudentId = s.Id
            WHERE  s.ClassDay LIKE '%' || @DayName || '%'
              AND  cs.ClassDate = @Date
            ORDER  BY s.FullName;";

        return await connection.QueryAsync<StudentSessionView>(sql, new
        {
            DayName = dayName,
            Date    = date.ToString("yyyy-MM-dd")
        });
    }

    public async Task<bool> UpdateSessionAttendanceAsync(int sessionId, bool attended)
    {
        using var connection = CreateConnection();
        const string sql = "UPDATE ClassSessions SET Attended = @Attended WHERE Id = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Attended = attended ? 1 : 0, Id = sessionId });
        return rows > 0;
    }

    public async Task<bool> UpdateSessionMaterialAsync(int sessionId, string? materialPath)
    {
        using var connection = CreateConnection();
        const string sql = "UPDATE ClassSessions SET AssignedMaterial = @Material WHERE Id = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Material = materialPath, Id = sessionId });
        return rows > 0;
    }
}
