using Feedback_Flow.Models;
using Feedback_Flow.Services;
using Feedback_Flow.Services.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Feedback_Flow.Tests.Services;

public class StudentServiceTests
{
    private readonly IDatabaseService _db;
    private readonly IFileSystemService _fs;
    private readonly StudentService _sut;

    public StudentServiceTests()
    {
        _db = Substitute.For<IDatabaseService>();
        _fs = Substitute.For<IFileSystemService>();
        _fs.InitializeDailyFolder(Arg.Any<DateTime>()).Returns("/test/daily-folder");
        _sut = new StudentService(_db, _fs);
    }

    // -------------------------------------------------------------------------
    // GetDayOfWeek
    // -------------------------------------------------------------------------

    [Fact]
    public void GetDayOfWeek_Monday_ReturnsMondayString()
    {
        var monday = new DateTime(2024, 1, 1); // Known Monday

        _sut.GetDayOfWeek(monday).Should().Be("Monday");
    }

    [Fact]
    public void GetDayOfWeek_Sunday_ReturnsSundayString()
    {
        var sunday = new DateTime(2024, 1, 7); // Known Sunday

        _sut.GetDayOfWeek(sunday).Should().Be("Sunday");
    }

    // -------------------------------------------------------------------------
    // AddStudentAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task AddStudentAsync_NullStudent_ThrowsArgumentNullException()
    {
        var act = () => _sut.AddStudentAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddStudentAsync_ValidStudent_CallsDbAddAndSetId()
    {
        var student = new Student { FullName = "Jane Doe" };
        _db.AddStudentAsync(student).Returns(42);

        await _sut.AddStudentAsync(student);

        student.Id.Should().Be(42);
        await _db.Received(1).AddStudentAsync(student);
    }

    [Fact]
    public async Task AddStudentAsync_ValidStudent_CreatesStudentFolder()
    {
        var student = new Student { FullName = "Jane Doe" };
        _db.AddStudentAsync(student).Returns(1);

        await _sut.AddStudentAsync(student);

        _fs.Received(1).CreateStudentFolder(Arg.Any<string>(), student);
    }

    // -------------------------------------------------------------------------
    // UpdateStudentAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateStudentAsync_NullOriginal_ThrowsArgumentNullException()
    {
        var act = () => _sut.UpdateStudentAsync(null!, new Student());

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateStudentAsync_NullUpdated_ThrowsArgumentNullException()
    {
        var act = () => _sut.UpdateStudentAsync(new Student(), null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateStudentAsync_SameName_DoesNotRenameFolder()
    {
        var original = new Student { Id = 1, FullName = "Jane Doe" };
        var updated = new Student { FullName = "jane doe" }; // same name, different case

        await _sut.UpdateStudentAsync(original, updated);

        _fs.DidNotReceive().RenameStudentFolder(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task UpdateStudentAsync_NameChanged_RenamesFolderCalled()
    {
        var original = new Student { Id = 1, FullName = "Jane Doe" };
        var updated = new Student { FullName = "Jane Smith" };

        await _sut.UpdateStudentAsync(original, updated);

        _fs.Received(1).RenameStudentFolder(Arg.Any<string>(), "Jane Doe", "Jane Smith");
    }

    // -------------------------------------------------------------------------
    // DeleteStudentAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task DeleteStudentAsync_NullStudent_ThrowsArgumentNullException()
    {
        var act = () => _sut.DeleteStudentAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteStudentAsync_ValidStudent_CallsDbDelete()
    {
        var student = new Student { Id = 7, FullName = "John Smith" };

        await _sut.DeleteStudentAsync(student);

        await _db.Received(1).DeleteStudentAsync(7);
    }

    // -------------------------------------------------------------------------
    // MarkAttendanceAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task MarkAttendanceAsync_DelegatesToDatabase()
    {
        await _sut.MarkAttendanceAsync(5, true);

        await _db.Received(1).UpdateSessionAttendanceAsync(5, true);
    }

    // -------------------------------------------------------------------------
    // UpdateSessionMaterialAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateSessionMaterialAsync_DelegatesToDatabase()
    {
        await _sut.UpdateSessionMaterialAsync(3, "/path/to/material.docx");

        await _db.Received(1).UpdateSessionMaterialAsync(3, "/path/to/material.docx");
    }

    // -------------------------------------------------------------------------
    // SaveNextClassPlanAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task SaveNextClassPlanAsync_EnsuresSessionThenUpdates()
    {
        var date = new DateTime(2024, 3, 15);
        _db.EnsureSessionForStudentAsync(1, date).Returns(10);

        await _sut.SaveNextClassPlanAsync(1, date, "material.pptx", "description");

        await _db.Received(1).EnsureSessionForStudentAsync(1, date);
        await _db.Received(1).UpdateSessionPlanAsync(10, "material.pptx", "description");
    }
}
