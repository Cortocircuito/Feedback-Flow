using FeedbackFlow.Core.Models;
using FluentAssertions;
using Xunit;

namespace Feedback_Flow.Tests.Models;

public class StudentTests
{
    // -------------------------------------------------------------------------
    // GetClassDays
    // -------------------------------------------------------------------------

    [Fact]
    public void GetClassDays_SingleDay_ReturnsSingleElement()
    {
        var student = new Student { ClassDay = "Monday" };

        var result = student.GetClassDays();

        result.Should().ContainSingle().Which.Should().Be("Monday");
    }

    [Fact]
    public void GetClassDays_MultipleDays_ReturnsAllTrimmed()
    {
        var student = new Student { ClassDay = "Monday, Wednesday, Friday" };

        var result = student.GetClassDays();

        result.Should().Equal("Monday", "Wednesday", "Friday");
    }

    [Fact]
    public void GetClassDays_EmptyString_ReturnsEmptyList()
    {
        var student = new Student { ClassDay = string.Empty };

        var result = student.GetClassDays();

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    public void GetClassDays_NullOrWhitespace_ReturnsEmptyList(string? classDay)
    {
        var student = new Student { ClassDay = classDay! };

        var result = student.GetClassDays();

        result.Should().BeEmpty();
    }

    // -------------------------------------------------------------------------
    // HasClassOnDay
    // -------------------------------------------------------------------------

    [Fact]
    public void HasClassOnDay_DayExists_ReturnsTrue()
    {
        var student = new Student { ClassDay = "Monday, Wednesday" };

        student.HasClassOnDay("Monday").Should().BeTrue();
    }

    [Fact]
    public void HasClassOnDay_DayMissing_ReturnsFalse()
    {
        var student = new Student { ClassDay = "Monday, Wednesday" };

        student.HasClassOnDay("Tuesday").Should().BeFalse();
    }

    [Fact]
    public void HasClassOnDay_CaseInsensitive_ReturnsTrue()
    {
        var student = new Student { ClassDay = "Monday" };

        student.HasClassOnDay("monday").Should().BeTrue();
    }

    // -------------------------------------------------------------------------
    // GetFolderName
    // -------------------------------------------------------------------------

    [Fact]
    public void GetFolderName_SpacesReplacedWithHyphens()
    {
        var student = new Student { FullName = "Jane Doe" };

        student.GetFolderName().Should().Be("Jane-Doe");
    }

    [Fact]
    public void GetFolderName_EmptyName_ReturnsUnknownStudent()
    {
        var student = new Student { FullName = string.Empty };

        student.GetFolderName().Should().Be("Unknown-Student");
    }

    [Fact]
    public void GetFolderName_TrimsWhitespace()
    {
        var student = new Student { FullName = "  Jane  " };

        student.GetFolderName().Should().Be("Jane");
    }
}
