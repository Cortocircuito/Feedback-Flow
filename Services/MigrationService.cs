using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.Services;

public class MigrationService : IMigrationService
{
    private readonly IDatabaseService _databaseService;
    private readonly string _csvPath;

    public MigrationService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string appFolder = Path.Combine(documents, "Feedback-Flow");
        _csvPath = Path.Combine(appFolder, "students.csv");
    }

    public async Task<MigrationResult> MigrateFromCsvAsync()
    {
        var result = new MigrationResult();

        if (!File.Exists(_csvPath))
        {
            result.Success = true;
            return result; // No CSV to migrate, not an error
        }

        // Check if DB is already empty? Or rely on unique constraints.
        // Prompt said: "If students.csv exists and database is empty or doesn't exist" -> we handle "empty or doesn't exist" via InitializeDatabaseAsync called before this.
        // We will check if DB has students to avoid double migration if file wasn't renamed for some reason.
        var existingStudents = await _databaseService.GetAllStudentsAsync();
        if (existingStudents.Any())
        {
            result.Errors.Add("Database is not empty. Migration skipped.");
            return result;
        }

        try
        {
            var lines = await File.ReadAllLinesAsync(_csvPath);
            result.TotalRecords = lines.Length; // Rough count, includes header

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length < 2) continue;

                var name = parts[0].Trim();
                var email = parts[1].Trim();
                var assignedMaterial = parts.Length > 2 ? parts[2].Trim() : string.Empty;

                // Skip header
                if (name.Equals("Full Name", StringComparison.OrdinalIgnoreCase) &&
                    email.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (await _databaseService.EmailExistsAsync(email))
                {
                    result.SkippedRecords++;
                    continue; // Skip duplicates
                }

                var student = new Student
                {
                    FullName = name,
                    Email = email,
                    AssignedMaterial = assignedMaterial,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    UpdatedAt = DateTime.UtcNow.ToString("o")
                };

                await _databaseService.AddStudentAsync(student);
                result.ImportedRecords++;
            }

            // Rename CSV after successful migration
            string migratedPath = _csvPath + ".migrated";
            if (File.Exists(migratedPath)) File.Delete(migratedPath);
            File.Move(_csvPath, migratedPath);

            result.Success = true;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Errors.Add($"Migration failed: {ex.Message}");
        }

        return result;
    }
}
