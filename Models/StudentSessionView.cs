namespace Feedback_Flow.Models;

/// <summary>
/// Flat DTO that combines Student identity with a single ClassSession's data.
/// Used as the DataGridView binding source so grid columns remain unchanged.
/// </summary>
public class StudentSessionView
{
    // From Student
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ClassDay { get; set; } = string.Empty;

    // From ClassSession
    public int SessionId { get; set; }
    public bool Attended { get; set; }
    public string? AssignedMaterial { get; set; }
    public string? FeedbackNotePath { get; set; }

    /// <summary>
    /// Sanitized folder name matching Student.GetFolderName() convention.
    /// E.g. "Jane Doe" -> "Jane-Doe"
    /// </summary>
    public string GetFolderName()
    {
        if (string.IsNullOrWhiteSpace(FullName)) return "Unknown-Student";
        return FullName.Trim().Replace(" ", "-");
    }
}
