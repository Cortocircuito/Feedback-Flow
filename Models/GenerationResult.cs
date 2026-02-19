namespace Feedback_Flow.Models;

/// <summary>
/// Holds the outcome of a batch email-generation run.
/// </summary>
internal sealed class GenerationResult
{
    public int SuccessCount { get; set; }
    public int ErrorCount { get; set; }
    public List<string> SkippedStudents { get; } = new();
}
