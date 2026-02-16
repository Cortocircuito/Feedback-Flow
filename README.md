# Feedback Flow

<div align="center">

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D4?logo=windows)
![License](https://img.shields.io/badge/License-MIT-green.svg)
![C#](https://img.shields.io/badge/C%23-WinForms-239120?logo=csharp)
[![GitHub release](https://img.shields.io/github/v/release/Cortocircuito/Feedback-Flow?include_prereleases)](https://github.com/Cortocircuito/Feedback-Flow/releases)

**A modern desktop application for automating personalized student feedback distribution**

[Quick Start](#-quick-start) • [Features](#-features) • [Use Cases](#-use-cases) • [Installation](#-installation) • [Architecture](#️-architecture) • [FAQ](#-faq)

</div>

---

## 📖 Overview

**Feedback Flow** is a desktop application developed in C# WinForms (.NET 10) designed to streamline the teaching workflow by automating the distribution of personalized feedback to students. It combines student management, attendance tracking, material assignment, and automated email generation into a single, intuitive interface.

## 📣 Recent Updates (v1.0.7)

- **🖥️ High DPI Support**: Fully optimized for 4K monitors and scaling settings (100% - 200%).
- **🔍 Smart Search**: Instantly filter students by name with the new search bar in "Show All" mode.
- **✨ Enhanced UI**: Modernized layout with responsive grids, cleaner buttons, and improved spacing.
- **📄 Pro Reports**: Redesigned PDF feedback reports for a more professional look.

## 🚀 Quick Start

1. **Download** the latest release from [GitHub Releases](https://github.com/Cortocircuito/Feedback-Flow/releases)
2. **Extract** and run `Feedback Flow.exe`
3. **Add students** with their assigned class days
4. **Mark attendance** for today's class
5. **Write feedback** by clicking "Open Note" for each student
6. **Generate emails** to send feedback automatically

> 💡 **Tip**: The app automatically creates folders and organizes files in `Documents/Feedback-Flow`

## 💼 Use Cases

Feedback Flow is perfect for:

- **Language Teachers**: Manage multiple classes across different days with personalized feedback
- **Tutors**: Track student progress and send weekly feedback reports
- **Workshop Instructors**: Organize materials and attendance for recurring sessions
- **Educational Coordinators**: Maintain records and streamline communication with students
- **Online Educators**: Combine digital materials with personalized feedback delivery

## ✨ Features

### 👥 Student Management
- **Complete CRUD Operations**: Add, edit, and remove students with a dedicated dialog form
- **Class Day Assignment**: Assign students to specific weekdays (Monday-Friday) with multi-day support
- **Smart Filtering**: Dashboard automatically displays only students assigned to the current day
- **Instant Search**: Real-time filtering by name in "View All" mode
- **High DPI Ready**: Scales perfectly on high-resolution displays
- **View All Mode**: Toggle to view all students across all days for planning and administration
- **Column Sorting**: Alphabetic sorting for "Student Name" and "Learning Material" columns

### 📅 Attendance & Tracking
- **One-Click Attendance**: Mark attendance directly in the grid with visual checkmarks
- **Day-Specific Views**: Context-aware interface that adapts based on current day vs. all-students view
- **Visual Mode Indicators**: Color-coded panels showing current view mode
  - 📅 **Green panel**: Current day view (active teaching mode)
  - 👥 **Blue panel**: All students view (planning/admin mode)

### 📚 Learning Materials
- **Material Assignment**: Assign optional learning materials (PDF, Word, PowerPoint, LibreOffice)
- **Flexible Workflow**: Send feedback emails with or without materials
- **Smart Attachments**: Materials automatically attached when available

### 📝 Feedback Management
- **Direct Note Editing**: One-click access to student feedback notes in your default text editor
- **Automated Organization**: Daily folders (`YYYYMMDD`) created within `Documents/Feedback-Flow`
- **PDF Generation**: Converts feedback notes to professional PDFs using iText 9
- **Persistent Storage**: Student folders are **never deleted** when removing students

### 📧 Email Integration
- **Automated Email Drafts**: Generates `.eml` files with feedback and materials attached
- **Attendance-Based Sending**: Emails generated for all students who attended class
- **Customizable Templates**: Pre-filled subject and body with student-specific information
- **Temp Email Storage**: Drafts saved in `Documents/Feedback-Flow/TempEmails`

### 🗄️ Data Management
- **SQLite Database**: Reliable local storage with automatic CSV migration
- **Automatic Migration**: Seamless one-time migration from legacy `students.csv` files
- **Data Integrity**: Enhanced validation and referential integrity with database constraints
- **Schema Versioning**: Automatic database migrations with idempotent update logic

## 🖼️ Screenshots

> **Note**: Add screenshots of the main dashboard, student form, and email generation features here.

## 💾 Data Storage

All data is stored locally on your machine:

| Data Type | Location | Format |
|-----------|----------|--------|
| **Student Database** | `Documents/Feedback-Flow/feedbackflow.db` | SQLite |
| **Feedback Notes** | `Documents/Feedback-Flow/{student-name}/` | `.txt` files |
| **PDF Feedback** | `Documents/Feedback-Flow/{student-name}/` | `.pdf` files |
| **Email Drafts** | `Documents/Feedback-Flow/TempEmails/` | `.eml` files |
| **Learning Materials** | `Documents/Feedback-Flow/{student-name}/` | Various formats |

> 🔒 **Privacy**: All data remains on your local machine. No cloud storage or external services are used.

## 📦 Installation

### System Requirements
- **Operating System**: Windows 10/11 (64-bit)
- **Runtime**: .NET 10 Runtime (included in self-contained builds)
- **Disk Space**: ~100 MB for application + storage for student data
- **Memory**: 512 MB RAM minimum
- **Display**: 1280x720 minimum resolution recommended

### Download & Run
1. Download the latest release from the [Releases](../../releases) page
2. Extract the ZIP file to your desired location
3. Run `Feedback Flow.exe`

### First-Time Setup
On first launch, the application will:
- Create the `Documents/Feedback-Flow` directory structure
- Automatically migrate any existing `students.csv` file to SQLite
- Rename the original CSV to `students.csv.migrated`

## 🚀 Usage

### Basic Workflow
1. **Add Students**: Click "Add Student" to create student records with assigned class days
2. **Mark Attendance**: Select students and click "Mark Attendance" for the current day
3. **Assign Materials**: (Optional) Assign learning materials to students
4. **Write Feedback**: Click "Open Note" to write personalized feedback in your text editor
5. **Generate Emails**: Click "Generate Emails" to create draft emails with PDF feedback

### View Modes
- **Current Day View**: Shows only students assigned to today's class
- **All Students View**: Click "Show All Students" to view and manage all students

### File Naming Conventions
- **Feedback Notes**: `feedback-john-doe-20260129.txt`
- **PDF Files**: `feedback-john-doe-20260129.pdf`
- **Student Folders**: `john-doe/`

## 🏗️ Architecture

### Technology Stack
- **Framework**: .NET 10 (Windows Forms)
- **Database**: SQLite with Dapper ORM
- **PDF Generation**: iText 9
- **Email**: MimeKit for `.eml` generation
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection

### Design Principles
- ✅ **SOLID Principles**: Service-based architecture with clear separation of concerns
- ✅ **Dependency Injection**: All services registered via DI container
- ✅ **Async/Await**: Non-blocking database operations for responsive UI
- ✅ **Repository Pattern**: Abstracted data access through service interfaces

### Project Structure
```
Feedback Flow/
├── Services/
│   ├── Interfaces/           # Service contracts
│   ├── FileSystemService.cs  # File/folder operations
│   ├── SqliteDatabaseService.cs  # Database access
│   ├── StudentService.cs     # Student CRUD operations
│   ├── MigrationService.cs   # CSV to SQLite migration
│   ├── NoteService.cs        # Feedback note management
│   ├── PdfGenerationService.cs  # PDF conversion
│   └── OutlookEmailService.cs   # Email draft generation
├── Models/                   # Data models
├── Helpers/                  # Utility classes
├── MainDashboard.cs          # Main application window
├── StudentForm.cs            # Student add/edit dialog
└── Program.cs                # Application entry point
```

### Key Services
| Service | Responsibility |
|---------|---------------|
| `IStudentService` | Student CRUD operations and queries |
| `IDatabaseService` | SQLite database initialization and schema management |
| `IMigrationService` | Legacy CSV to SQLite migration |
| `IFileSystemService` | File and folder operations |
| `INoteService` | Feedback note creation and management |
| `IPdfGenerationService` | PDF conversion from text notes |
| `IOutlookEmailService` | Email draft generation with attachments |

## 💻 Development

### Prerequisites
- **Visual Studio 2022** or later
- **.NET 10 SDK**
- **Git**

### Setup
```bash
# Clone the repository
git clone https://github.com/Cortocircuito/Feedback-Flow.git
cd Feedback-Flow

# Open in Visual Studio
start "Feedback Flow.slnx"

# Or build from command line
dotnet build

# Run the application
dotnet run
```

### Dependency Injection Setup

The application uses Microsoft.Extensions.DependencyInjection. All services are registered in `Program.cs`:

```csharp
services.AddSingleton<IDatabaseService, SqliteDatabaseService>();
services.AddSingleton<IStudentService, StudentService>();
services.AddSingleton<IPdfService, PdfGenerationService>();
// ... and more
```

This architecture ensures:
- **Testability**: Easy to mock services for unit testing
- **Maintainability**: Clear separation of concerns
- **Flexibility**: Simple to swap implementations

### Building for Release
```bash
# Self-contained Windows x64 build
dotnet publish -c Release -r win-x64 --self-contained true
```

### Testing
A PowerShell script is provided for setting up a test environment:
```powershell
.\setup_test_env.ps1
```

## 📑 Changelog

See [CHANGELOG.md](CHANGELOG.md) for detailed release notes and version history.

## ❓ FAQ

<details>
<summary><strong>Can I use this on macOS or Linux?</strong></summary>

Currently, Feedback Flow is Windows-only due to WinForms. A cross-platform version using Avalonia or MAUI could be considered for future releases.
</details>

<details>
<summary><strong>Where is my data stored?</strong></summary>

All data is stored locally in <code>Documents/Feedback-Flow</code>. The SQLite database contains student records, while feedback notes and materials are organized in student-specific folders.
</details>

<details>
<summary><strong>What happens to student data when I remove a student?</strong></summary>

Student records are removed from the database, but their folders and files are <strong>never deleted</strong>. This ensures you never lose historical feedback or materials.
</details>

<details>
<summary><strong>Can I customize the email templates?</strong></summary>

Currently, email templates are pre-defined in the code. Future versions may include customizable templates. You can modify the <code>OutlookEmailService.cs</code> file to customize the email format.
</details>

<details>
<summary><strong>What file formats are supported for learning materials?</strong></summary>

The application supports PDF, Word (.doc, .docx), PowerPoint (.ppt, .pptx), and LibreOffice formats (.odt, .odp). Any file can be stored in student folders, but these formats are specifically recognized.
</details>

<details>
<summary><strong>How do I backup my data?</strong></summary>

Simply copy the entire <code>Documents/Feedback-Flow</code> folder to your backup location. This includes the database and all student files.
</details>

<details>
<summary><strong>Can multiple teachers use this on the same computer?</strong></summary>

Yes, each Windows user account will have its own separate <code>Documents/Feedback-Flow</code> folder with independent data.
</details>

## 🐛 Troubleshooting

### Database Issues
- **Migration Failed**: Check that `students.csv` is properly formatted and not locked by another application
- **Database Locked**: Ensure only one instance of the application is running

### Email Generation
- **Emails Not Generated**: Verify that students have attendance marked for the current day
- **Attachments Missing**: Check that material files exist in the student's folder

### PDF Generation
- **PDF Creation Failed**: Ensure feedback note file exists and is not empty
- **Encoding Issues**: Save feedback notes in UTF-8 encoding

## 📝 Notes & Best Practices

- Student folders are **never deleted** when removing students from the database
- Original CSV file is migrated and renamed to `students.csv.migrated` after successful import
- Note files follow the naming convention: `feedback-john-doe-20260129.txt`
- Email drafts are saved in `Documents/Feedback-Flow/TempEmails`
- Always mark attendance before generating emails for the day

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## 🙏 Acknowledgments

### Libraries & Frameworks
- **[iText 9](https://itextpdf.com/)**: PDF generation library
- **[MimeKit](https://github.com/jstedfast/MimeKit)**: Email message creation
- **[Dapper](https://github.com/DapperLib/Dapper)**: Lightweight ORM for .NET
- **[SQLite](https://www.sqlite.org/)**: Embedded database engine
- **[Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection)**: Dependency injection container

### Design Resources
- **[Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons)**: Application icon by Microsoft (MIT License)

---

<div align="center">

**Developed to simplify the teaching workflow and streamline student feedback distribution**

Made with ❤️ for educators

</div>
