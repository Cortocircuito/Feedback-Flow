using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.Services;

public class CsvDataService : IDataService
{
    public IEnumerable<Student> LoadStudents(string csvPath)
    {
        var students = new List<Student>();

        if (!File.Exists(csvPath))
        {
            return students;
        }

        // Simple parsing logic (KISS)
        // Expecting: Full Name, Email
        // Skipping header if looks like header

        var lines = File.ReadAllLines(csvPath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(',');
            if (parts.Length < 2) continue;

            var name = parts[0].Trim();
            var email = parts[1].Trim();

            // Basic header check
            if (name.Equals("Full Name", StringComparison.OrdinalIgnoreCase) &&
                email.Equals("Email", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            students.Add(new Student { FullName = name, Email = email });
        }

        return students.OrderBy(s => s.FullName).ToList();
    }
}