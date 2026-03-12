namespace Feedback_Flow.Services.Interfaces;

public interface IMigrationService
{
    Task<MigrationResult> MigrateFromCsvAsync();
}

public class MigrationResult
{
    public bool Success { get; set; }
    public int TotalRecords { get; set; }
    public int ImportedRecords { get; set; }
    public int SkippedRecords { get; set; }
    public List<string> Errors { get; set; } = new();
}
