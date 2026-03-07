# AGENTS.md - Developer Guide for Feedback Flow

This document provides guidance for agentic coding agents working on the Feedback Flow codebase.

## Project Overview

Feedback Flow is a C# .NET 10 Windows Forms desktop application for automating personalized student feedback distribution. It uses SQLite for data storage, PDFSharp for PDF generation, and MimeKit for email draft creation.

## Build and Test Commands

### Building the Project

```bash
# Build the solution
dotnet build

# Build in Release mode
dotnet build -c Release

# Run the application
dotnet run

# Publish self-contained Windows x64 build
dotnet publish -c Release -r win-x64 --self-contained true
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test -v n

# Run a single test by method name
dotnet test --filter "FullyQualifiedName~StudentServiceTests.GetDayOfWeek_Monday_ReturnsMondayString"

# Run tests in a specific file
dotnet test --filter "FullyQualifiedName~StudentServiceTests"

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Project References

- Main project: `Feedback Flow.csproj`
- Test project: `Feedback Flow.Tests/Feedback Flow.Tests.csproj`
- Test framework: **xUnit** with **FluentAssertions** and **NSubstitute** for mocking

## Code Style Guidelines

### General Principles

- **SOLID Principles**: Use service-based architecture with clear separation of concerns
- **Dependency Injection**: All services registered via `Microsoft.Extensions.DependencyInjection`
- **Async/Await**: Use for all I/O-bound operations (database, file system)
- **No comments on implementation details**: Only use XML doc comments for public APIs

### File Organization

- **File-scoped namespaces**: Use `namespace X.Y;` instead of bracketed namespaces
- **Using statements**: Place at the top of files, sorted alphabetically
- **Region blocks**: Organize code with `#region` and `#endregion` for logical grouping

```csharp
using System.ComponentModel;
using Feedback_Flow.Helpers;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.UI.Forms;

public sealed partial class MainDashboard : Form
{
    #region Initialization
    
    // Constructor and setup code here
    
    #endregion

    #region Event Handlers
    
    // Button clicks, form events here
    
    #endregion
    
    #region Helper Methods
    
    // Private utility methods here
    
    #endregion
}
```

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes/Models | PascalCase | `Student`, `StudentService` |
| Interfaces | PascalCase with `I` prefix | `IStudentService`, `IDatabaseService` |
| Methods | PascalCase | `LoadStudentsAsync`, `GetDayOfWeek` |
| Properties | PascalCase | `FullName`, `ClassDay` |
| Private fields | PascalCase | `_studentService`, `_dailyFolderPath` |
| Local variables | camelCase | `students`, `selectedDate` |
| Parameters | camelCase | `studentId`, `materialPath` |
| Constants | PascalCase | `DefaultFolderName` |

### Type Guidelines

- **Nullable**: Disabled in main project (`<Nullable>disable</Nullable>`), enabled in tests
- **Properties**: Use `{ get; set; }` with default values where appropriate
- **String defaults**: Use `string.Empty` instead of `""`
- **Collections**: Prefer `List<T>` for mutable collections, return `IReadOnlyList<T>` where appropriate

```csharp
public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ClassDay { get; set; } = string.Empty;
}
```

### Error Handling

- **Argument validation**: Use `ArgumentNullException.ThrowIfNull(param)` for null checks
- **Service errors**: Wrap async operations in try-catch with user-friendly error messages
- **UI error handling**: Use `ExecuteWithErrorHandlingAsync` helper pattern for async UI operations

```csharp
public async Task AddStudentAsync(Student student)
{
    ArgumentNullException.ThrowIfNull(student);
    student.Id = await _db.AddStudentAsync(student);
    _fileService.CreateStudentFolder(_dailyFolderPath, student);
}
```

### Dependency Injection

All services are registered in `Program.cs` using the `ServiceCollection` pattern:

```csharp
services.AddSingleton<IDatabaseService, SqliteDatabaseService>();
services.AddSingleton<IStudentService, StudentService>();
services.AddTransient<MainDashboard>();
```

Services are injected via constructor injection:

```csharp
public MainDashboard(
    IStudentService studentService,
    IFileSystemService fileService,
    IPdfService pdfService)
{
    _studentService = studentService;
    // ...
}
```

### Database Access

- Use **Dapper** for lightweight ORM with raw SQL
- Async methods for all database operations
- Follow repository pattern through service interfaces

### XML Documentation

Add `<summary>` doc comments for public API methods:

```csharp
/// <summary>
/// Ensures session rows exist for students with class on the given date's weekday,
/// then returns the joined view for grid binding.
/// </summary>
Task<List<StudentSessionView>> GetSessionViewsAsync(DateTime date);
```

### Testing Guidelines

- Test file location: `Feedback Flow.Tests/Services/` or `Feedback Flow.Tests/Models/`
- Test class naming: `{ServiceName}Tests`
- Test method naming: `{MethodName}_{Scenario}_{ExpectedResult}`
- Use `FluentAssertions` for assertions: `result.Should().Be(expected)`
- Use `NSubstitute` for mocking: `Substitute.For<IInterface>()`

```csharp
[Fact]
public async Task AddStudentAsync_ValidStudent_CallsDbAddAndSetId()
{
    var student = new Student { FullName = "Jane Doe" };
    _db.AddStudentAsync(student).Returns(42);

    await _sut.AddStudentAsync(student);

    student.Id.Should().Be(42);
    await _db.Received(1).AddStudentAsync(student);
}
```

### UI Development (WinForms)

- Use `partial class` with Designer.cs companion files
- Theme support via `ThemeManager` in `UI/Theme/`
- Use `SortableBindingList<T>` for DataGridView binding
- Programmatic control creation for dynamic UI elements

## Project Structure

```
Feedback Flow/
├── Services/
│   ├── Interfaces/           # Service contracts (IStudentService, etc.)
│   ├── StudentService.cs     # Student CRUD and session management
│   ├── SqliteDatabaseService.cs  # Database operations
│   ├── FileSystemService.cs  # File/folder operations
│   ├── PdfGenerationService.cs  # PDF creation
│   ├── NoteService.cs        # Feedback note management
│   └── OutlookEmailService.cs   # Email draft generation
├── UI/
│   ├── Forms/                # WinForms dialogs and main window
│   │   ├── MainDashboard.cs
│   │   ├── StudentForm.cs
│   │   └── PrepareNextClassForm.cs
│   └── Theme/                # Dark mode theming
├── Models/                   # Data models (Student, ClassSession, etc.)
├── Helpers/                  # Utility classes (SortableBindingList)
├── Assets/                   # Icons and images
├── Program.cs                # Entry point with DI setup
└── Feedback Flow.csproj     # Project file
```

## Data Storage

| Data Type | Location | Format |
|-----------|----------|--------|
| Student Database | `Documents/Feedback-Flow/feedbackflow.db` | SQLite |
| Feedback Notes | `Documents/Feedback-Flow/{student-name}/` | `.txt` files |
| PDF Feedback | `Documents/Feedback-Flow/{student-name}/` | `.pdf` files |
| Email Drafts | `Documents/Feedback-Flow/TempEmails/` | `.eml` files |

## Common Tasks

### Adding a New Service

1. Create interface in `Services/Interfaces/`
2. Create implementation in `Services/`
3. Register in `Program.cs` ConfigureServices method
4. Inject into consumers via constructor

### Adding a New Model

1. Create in `Models/` directory
2. Add corresponding database queries in `SqliteDatabaseService`
3. Add service methods if CRUD needed

### Running a Single Test

```bash
dotnet test --filter "FullyQualifiedName~StudentServiceTests.GetDayOfWeek_Monday_ReturnsMondayString"
```
