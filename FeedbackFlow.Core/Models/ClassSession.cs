namespace FeedbackFlow.Core.Models;

/// <summary>
/// Represents a single class session record for a student on a specific date.
/// One-to-Many: one Student has many ClassSessions (one per class date).
/// </summary>
public class ClassSession
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public DateTime ClassDate { get; set; }
    public bool Attended { get; set; }
    public string? AssignedMaterial { get; set; }
    public string? FeedbackNotePath { get; set; }
    public string? ClassDescription { get; set; }
}
