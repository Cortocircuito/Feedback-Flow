# Changelog

All notable changes to Feedback Flow are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
versioning follows [Semantic Versioning](https://semver.org/).

---

## [Unreleased]

## [1.0.8] - 2026-02-19

### Changed
- Moved `MainDashboard` and `StudentForm` from the project root into `UI/Forms/` with namespace `Feedback_Flow.UI.Forms`
- Split monolithic `ServiceInterfaces.cs` into individual files: `IFileSystemService.cs`, `IPdfService.cs`, `IEmailService.cs`
- Extracted `GenerationResult` inner class from `MainDashboard.cs` into `Models/GenerationResult.cs`
- Cleaned up `Program.cs`: removed dead comments and aligned using directives with new namespace layout

### Removed
- `ServiceInterfaces.cs` — replaced by three individual interface files
- Dead `IDataService` interface (no implementation existed)

## [1.0.7] - 2026-02-16

### Changed
- Improved `MainDashboard` layout: reduced button heights, removed excessive whitespace, white background for data grid
- Updated README with latest features and file storage details

## [1.0.6] - 2026-02-09

### Fixed
- Corrected student count displayed in "trip" status when "Show Today Only" filter was active

## [1.0.5] - 2026-01-29

### Added
- Mode indicator panel with color-coded visual feedback (green = today's view, blue = all students view)
- Search panel in "Show All" mode to filter students by name in real time
- Tooltips on disabled buttons to guide users when day-specific actions are unavailable

### Changed
- Day-specific action buttons (Attend, Material, Generate) are now disabled in "Show All" mode to prevent accidental operations
- CRUD buttons (Add, Edit, Remove) are disabled in "Today" mode and enabled only in "Show All" mode
- Double-clicking a row opens Edit form in "Show All" mode and feedback notes in "Today" mode

## [1.0.4] - 2026-01-27

### Added
- Alphabetic column sorting for "Student Name" and "Learning Material" columns via custom `SortableBindingList<T>` helper

### Changed
- `btnShowAll` toggle replaced by `btnToggleFilter` for clearer intent

## [1.0.3] - 2026-01-26

### Added
- `AttendedClass` column in `Students` table (INTEGER 0/1) for daily attendance tracking
- "Mark Attendance" flow: checkbox column in grid, persisted via `StudentService.MarkAttendanceAsync()`
- "Show All Students" toggle to switch between day-filtered and full student list views
- Smart column visibility: Attendance and Learning Material columns hidden in "Show All" mode
- Feedback email generation now based on attendance rather than material assignment
- "Unassign Material" button to clear an assigned learning material
- "View Material" button to open the assigned file directly from the dashboard

### Changed
- `StudentForm` updated with `CheckedListBox` for multi-day class day selection
- `GetStudentsWhoAttendedAsync()` added to `IStudentService` and `IDatabaseService`
- Daily reset now clears both `AssignedMaterial` and `AttendedClass` on new day detection

## [1.0.2] - 2026-01-25

### Added
- `ClassDay` column in `Students` table (comma-separated, e.g. `"Monday,Wednesday"`)
- Day-of-week filtering: dashboard shows only students assigned to today's weekday on load
- `HasClassOnDay()` and `GetClassDays()` helper methods on the `Student` model
- `GetStudentsByDayAsync()` on `IDatabaseService` and `IStudentService`
- Daily reset logic: new teaching day detection clears `AssignedMaterial` and resets attendance state
- Version label in status bar showing short git hash

### Changed
- `StudentService.PerformDailyResetAsync()` checks if today's folder exists to detect day changes
- Schema migration with idempotent `ALTER TABLE` for adding `ClassDay` column to existing databases

## [1.0.1] - 2026-01-25

### Added
- SQLite database replacing CSV file storage (`feedbackflow.db` in `Documents/Feedback-Flow/`)
- `SqliteDatabaseService` implementing full CRUD and query operations via Dapper ORM
- `MigrationService` for one-time automatic import of existing `students.csv` into the database
- Dependency Injection container (`Microsoft.Extensions.DependencyInjection`) wired up in `Program.cs`
- `IStudentService`, `IDatabaseService`, `IMigrationService`, `IFileSystemService`, `IPdfService`, `IEmailService`, `INoteService` interfaces
- PDF generation from feedback notes via iText 9 (`PdfGenerationService`)
- `.eml` draft email generation via MimeKit (`OutlookEmailService`) with Outlook-safe delays
- DPI awareness (`PerMonitorV2`) and responsive layout via `TableLayoutPanel`
- Application icon (`fluent-color-person-edit-32.ico`)

### Removed
- CSV-based data storage and the old `CsvDataService`

## [1.0.0] - 2026-01-25

### Added
- Initial release of Feedback Flow
- `MainDashboard` form with student grid (Name, Email, Learning Material columns)
- `StudentForm` dialog for adding and editing students
- Assign / view learning material per student via file picker
- Open or create per-student feedback notes in default text editor
- Generate feedback emails as `.eml` drafts opening in Outlook
- File system directory structure: `Documents/Feedback-Flow/YYYYMMDD/student-name/`
- Self-contained Windows x64 publish target

---

## 📦 Database Structure

**Table**: `Students`

| Column | Type | Description |
|---|---|---|
| `Id` | `INTEGER PRIMARY KEY` | Auto-increment unique identifier |
| `FullName` | `TEXT NOT NULL` | Student's full name |
| `Email` | `TEXT NOT NULL UNIQUE` | Student's email address |
| `ClassDay` | `TEXT` | Comma-separated class days (e.g. `"Monday,Wednesday"`) |
| `AssignedMaterial` | `TEXT` | Absolute path to the assigned learning material file |
| `AttendedClass` | `INTEGER` | Daily attendance flag (`0` = absent, `1` = present) |
| `CreatedAt` | `TEXT` | ISO 8601 creation timestamp |
| `UpdatedAt` | `TEXT` | ISO 8601 last-modified timestamp |