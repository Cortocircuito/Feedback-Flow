namespace Feedback_Flow.Models;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AssignedMaterial { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty;

    // Helper to get sanitize name for folders (e.g. "Jane Doe" -> "Jane-Doe")
    // Implementation rule: Replace spaces with hyphens.
    public string GetFolderName()
    {
        if (string.IsNullOrWhiteSpace(FullName)) return "Unknown-Student";
        return FullName.Trim().Replace(" ", "-");
    }
}