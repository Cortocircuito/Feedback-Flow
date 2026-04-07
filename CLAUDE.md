# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```bash
# Build all projects
dotnet build

# Run WinForms app (original)
dotnet run --project "Feedback Flow/Feedback Flow.csproj"

# Run Avalonia app (migration target)
dotnet run --project "FeedbackFlow.App/FeedbackFlow.App.csproj"

# Publish self-contained Windows x64
dotnet publish -c Release -r win-x64 --self-contained true
```

## Tests

```bash
# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~StudentServiceTests"

# Run a single test method
dotnet test --filter "FullyQualifiedName~StudentServiceTests.GetDayOfWeek_Monday_ReturnsMondayString"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

Test setup: `setup_test_env.ps1` creates a local test environment with dummy CSV and feedback notes.

## Project Structure

The solution (`Feedback Flow.slnx`) has four projects:

| Project | Description |
|---------|-------------|
| `Feedback Flow/` | Original WinForms app (.NET 8) — stable, Windows-only |
| `FeedbackFlow.Core/` | Shared business logic (services + models) — referenced by both UIs |
| `FeedbackFlow.App/` | New Avalonia app (migration target) — cross-platform |
| `Feedback Flow.Tests/` | xUnit test suite — tests Core services |

## Active Migration

The `migration-winforms-to-avalonia` branch is an ongoing migration. Core business logic was extracted to `FeedbackFlow.Core` so both UIs share the same services. The Avalonia app is the target; the WinForms app is kept stable until the migration is complete.

## Architecture

**Service Layer** (`FeedbackFlow.Core/Services/`): All business logic lives here, behind interfaces in `Services/Interfaces/`. Both apps register the same singletons via Microsoft.Extensions.DependencyInjection.

Key services:
- `IStudentService` / `StudentService` — student CRUD, session management
- `IDatabaseService` / `SqliteDatabaseService` — SQLite schema + CRUD (file: `~/Documents/Feedback-Flow/feedbackflow.db`)
- `IFileSystemService` — feedback note files and folder management
- `IPdfService` — PDF generation from text feedback (PDFsharp)
- `IEmailService` — `.eml` draft generation (MimeKit)
- `IMigrationService` — one-time CSV → SQLite migration

**Models** (`FeedbackFlow.Core/Models/`):
- `Student` — has `ClassDay` (comma-separated days), helper methods `GetClassDays()`, `HasClassOnDay()`
- `ClassSession` — attendance record for a student on a specific date
- `StudentSessionView` — read model joining Student + ClassSession for UI binding

**WinForms UI** (`Feedback Flow/UI/`): Direct service calls from forms. `MainDashboard` is the main window. `ThemeManager` handles dark mode via Windows 10/11 native APIs.

**Avalonia UI** (`FeedbackFlow.App/`): MVVM with CommunityToolkit.Mvvm source generators. `MainWindowViewModel` (500 lines) holds all state and commands. Dialogs: `StudentDialog`, `PrepareClassDialog`. Theme: Avalonia Fluent.

## Tech Stack

- .NET 8, C# with nullable enabled and file-scoped namespaces (in Core/Tests)
- SQLite via Dapper + Microsoft.Data.Sqlite
- PDF: PDFsharp 6.2.4
- Email: MimeKit 4.3.0
- Avalonia 11.3.12 with Fluent theme
- Tests: xUnit + FluentAssertions + NSubstitute
