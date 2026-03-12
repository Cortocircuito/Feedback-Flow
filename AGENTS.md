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

---

## Migration: WinForms to Avalonia

This section documents the migration from WinForms to Avalonia for cross-platform support (Windows, macOS, Linux).

### Migration Overview

| Aspect | Value |
|--------|-------|
| Target Framework | .NET 8 LTS |
| MVVM Framework | Community Toolkit MVVM |
| Theming | Native FluentTheme (built-in dark/light) |
| Timeline | ~6-8 weeks |
| Approach | Full rewrite (not incremental) |

### Prerequisites

- **.NET 8 SDK** (or .NET 9)
- **Avalonia templates**: `dotnet new install Avalonia.Templates`
- **IDE**: Rider (recommended) or Visual Studio with Avalonia extension
- **Platform**: Development on Linux supported (Ubuntu 24.04 tested)

### Phase 1: Preparation (Week 1)

#### 1.1 Upgrade .NET Version
- [ ] Upgrade from .NET 10 to .NET 8 LTS (or .NET 9)
- [ ] Update all NuGet packages to latest cross-platform compatible versions
- [ ] Verify build: `dotnet build`

#### 1.2 Extract Business Logic
Create a class library `FeedbackFlow.Core`:
```
FeedbackFlow.slnx
├── FeedbackFlow.Core/          # Business logic (Services + Models)
├── FeedbackFlow/               # Existing WinForms (to be replaced)
└── FeedbackFlow.Tests/        # Existing tests
```

- [ ] Move `Services/` to Core library
- [ ] Move `Models/` to Core library
- [ ] Keep only UI code in WinForms project
- [ ] Verify tests still pass

#### 1.3 Verify Dependencies
All existing packages are cross-platform compatible:
- `Microsoft.Data.Sqlite` ✅
- `Dapper` ✅
- `PDFsharp` ✅
- `MimeKit` ✅
- `Microsoft.Extensions.DependencyInjection` ✅

### Phase 2: Avalonia Project Setup (Week 1-2)

#### 2.1 Create New Solution Structure
```bash
# Create Avalonia MVVM project
dotnet new avalonia.mvvm -o FeedbackFlow.Avalonia -n FeedbackFlow.Avalonia
```

```
FeedbackFlow.slnx
├── FeedbackFlow.Core/              # Business logic (from Phase 1)
├── FeedbackFlow.Avalonia/         # New Avalonia UI
└── FeedbackFlow.Tests/            # Existing tests
```

#### 2.2 Configure Project
- [ ] Add reference to FeedbackFlow.Core
- [ ] Target platforms: Windows, macOS, Linux (Desktop)
- [ ] Set .NET 8+ in csproj
- [ ] Install Community Toolkit MVVM (if not included)

#### 2.3 Test Empty Shell
- [ ] Run on Ubuntu: `dotnet run`
- [ ] Verify window appears
- [ ] Commit baseline

### Phase 3: UI Migration (Weeks 2-4)

#### 3.1 Control Mapping

| WinForms Control | Avalonia Control |
|-----------------|------------------|
| `Form` | `Window` / `UserControl` |
| `DataGridView` | `DataGrid` |
| `Button` | `Button` |
| `TextBox` | `TextBox` |
| `Label` | `TextBlock` |
| `DateTimePicker` | `CalendarDatePicker` |
| `Panel` | `Border` / `Panel` |
| `CheckBox` | `CheckBox` |
| `ComboBox` | `ComboBox` |
| `ToolStrip` | `NativeMenuBar` |
| `StatusStrip` | Custom status bar |

#### 3.2 Migrate Forms (in order)

**MainDashboard → MainWindow** (most complex)
- [ ] DataGrid with attendance tracking
- [ ] Date picker (CalendarDatePicker)
- [ ] Search panel
- [ ] Theme toggle button
- [ ] Mode indicator panels
- [ ] All button commands

**StudentForm → StudentDialog**
- [ ] Student name field
- [ ] Email field
- [ ] Class day selection
- [ ] Save/Cancel buttons

**PrepareNextClassForm → PrepareClassDialog**
- [ ] Material assignment
- [ ] Description text area
- [ ] Date display

#### 3.3 Convert Code-Behind to MVVM

**Before (WinForms):**
```csharp
private void btnAdd_Click(object sender, EventArgs e)
{
    var student = new Student { FullName = txtName.Text };
    _service.AddStudent(student);
}
```

**After (Avalonia MVVM):**
```csharp
// ViewModel
[RelayCommand]
private async Task AddStudent()
{
    var student = new Student { FullName = Name };
    await StudentService.AddStudentAsync(student);
}

// View (XAML)
<Button Command="{Binding AddStudentCommand}">Add Student</Button>
```

### Phase 4: Theming (Week 3)

#### 4.1 Use Native FluentTheme
Avalonia includes built-in theming - no need to port ThemeManager!

```xml
<!-- App.axaml -->
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Styles>
        <FluentTheme />
    </Application.Styles>
</Application>
```

#### 4.2 Dark/Light Mode Toggle
```csharp
// Simple theme toggle (2 lines!)
public void ToggleTheme()
{
    Application.Current!.RequestedThemeVariant =
        Application.Current.RequestedThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;
}
```

Options:
- `ThemeVariant.Default` - Follow system preference
- `ThemeVariant.Light` - Force light mode
- `ThemeVariant.Dark` - Force dark mode

#### 4.3 Persist User Preference
- [ ] Store preference in settings (same as current UserPreferences)
- [ ] Apply on startup
- [ ] Toggle button in UI

#### 4.4 Optional: FluentAvalonia
For more WinUI-like controls, consider adding `FluentAvalonia` package.

### Phase 5: Testing & Build (Week 4)

#### 5.1 Platform Testing
- [ ] Test on Linux (Ubuntu 24.04 - development machine)
- [ ] Test on Windows
- [ ] Test on macOS

#### 5.2 Build Commands
```bash
# Windows
dotnet publish -c Release -r win-x64 --self-contained true

# macOS
dotnet publish -c Release -r osx-x64 --self-contained true
dotnet publish -c Release -r osx-arm64 --self-contained true

# Linux
dotnet publish -c Release -r linux-x64 --self-contained true
```

#### 5.3 Final Verification
- [ ] All features work on all platforms
- [ ] Dark/light mode works
- [ ] Database operations work
- [ ] PDF generation works
- [ ] Email draft generation works

### Key Differences: WinForms → Avalonia

| WinForms | Avalonia |
|----------|----------|
| Designer files (.Designer.cs) | XAML files (.axaml) |
| Code-behind events | Commands (ICommand) |
| `this.Controls.Add()` | XAML `<Panel>` with bindings |
| `DataGridView` | `DataGrid` |
| `MessageBox.Show()` | `DialogService` |
| Custom ThemeManager | Built-in FluentTheme |

### Migration Status

- [ ] Phase 1: Preparation
- [ ] Phase 2: Avalonia Setup
- [ ] Phase 3: UI Migration
- [ ] Phase 4: Theming
- [ ] Phase 5: Testing & Build
