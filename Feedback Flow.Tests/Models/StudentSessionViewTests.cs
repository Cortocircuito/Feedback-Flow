using FeedbackFlow.Core.Models;
using FluentAssertions;
using Xunit;

namespace Feedback_Flow.Tests.Models;

public class StudentSessionViewTests
{
    [Fact]
    public void GetFolderName_SpacesReplacedWithHyphens()
    {
        var view = new StudentSessionView { FullName = "Jane Doe" };

        view.GetFolderName().Should().Be("Jane-Doe");
    }

    [Fact]
    public void GetFolderName_EmptyName_ReturnsUnknownStudent()
    {
        var view = new StudentSessionView { FullName = string.Empty };

        view.GetFolderName().Should().Be("Unknown-Student");
    }
}
