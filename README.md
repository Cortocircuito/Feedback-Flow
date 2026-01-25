# Feedback Flow

**Feedback Flow** is a desktop application developed in C# WinForms (.NET 10) designed to automate the distribution of personalized feedback to students.

## 🚀 Features

- **Student Management (CRUD)**: Add, edit, and remove students directly from the application with a dedicated dialog form
- **Direct Note Editing**: Instant access to student feedback files with a built-in "Smart" editor launcher
- **Automated Organization**: Organizes feedback in daily folders (`YYYYMMDD`) within `My Documents/Feedback-Flow`
- **Data Loading**: Imports student list from an `students.csv` file with automatic persistence
- **PDF Generation**: Converts individual text notes (`.txt`) into professional PDF documents using iText 9
- **Email Integration**: Generates personalized email drafts (`.eml`) with student-specific subject lines, class content and feedback attached, ready to be reviewed and sent from Outlook or another email client
- **Modern Architecture**: Uses Dependency Injection and service-oriented design for clean, maintainable code
- **Folder Synchronization**: Automatically creates and renames student folders when managing student data
- **Day Display**: Shows the current day of the week in English on the main dashboard

## 📋 Requirements

- **Environment**: Windows OS
- **Runtime**: .NET 10.0 SDK/Runtime
- **Software**: Email client (Outlook recommended) to open `.eml` files
- **Main Libraries**:
  - `iText 9`: For PDF manipulation and generation
  - `MimeKit`: For email message creation
  - `Microsoft.Extensions.DependencyInjection`: For service management

## 🛠️ Project Structure

```text
Feedback Flow/
├── Models/                    # Data models (Student)
├── Services/                  # Business logic
│   ├── Interfaces/            # Service contracts
│   │   ├── IStudentService.cs
│   │   ├── IFileSystemService.cs
│   │   ├── IDataService.cs
│   │   ├── IPdfService.cs
│   │   ├── IEmailService.cs
│   │   └── INoteService.cs
│   ├── StudentService.cs      # Student CRUD and CSV management
│   ├── CsvDataService.cs      # CSV file operations
│   ├── FileSystemService.cs   # Folder management
│   ├── OutlookEmailService.cs # Email draft generation
│   ├── PdfGenerationService.cs # PDF creation
│   └── NoteService.cs         # Note file management & editor launching
├── MainDashboard.cs           # Main application form
├── StudentForm.cs             # Student add/edit dialog
├── students.csv               # Student data file (example)
└── Program.cs                 # Entry point and DI configuration
```

## 📖 How to Use

### 1. Initial Setup
- Ensure you have an `students.csv` file in the project root or application folder with the format: `Full Name,Email`
- The file will be automatically created in `Documents/Feedback-Flow/` if it doesn't exist

### 2. Student Management
- **Add New Student**: Click "Add New" button, enter name and email in the dialog, click "Save"
- **Edit Student**: Select a student from the list, click "Edit Selected", modify details, click "Save"
- **Remove Student**: Select a student, click "Remove", confirm deletion (folders are preserved)
- All changes are automatically saved to the CSV file

### 3. Editing Feedback Notes
- **Access**: Select a student and click **"Edit Feedback"** OR simply **Double-Click** the student in the list.
- **Smart Creation**: The app checks for an existing note file. If none exists, it automatically creates one named `feedback-[student-name]-[yyyyMMdd].txt`.
- **Content**: New files are pre-filled with the header: `[Student Name] Feedback Notes:`.
- **Editor**: Opens immediately in your system's default text editor (e.g., Notepad).

### 4. Feedback Generation
- **Select PDF**: Click "Select Master PDF" and choose the class content PDF file
- **Prepare Notes**: (If you haven't used the "Edit Feedback" feature) Place `.txt` files with individual student notes in their corresponding folders.
- **Generate**: Click "Generate Feedback Emails" to create PDFs and email drafts
- **Email Format**: Each draft is personalized with:
  - **Subject**: `Feedback for [Student Name] - YYYY-MM-DD`
  - **Body**: Personalized greeting with student's name
  - **Attachments**: Class content PDF and student-specific feedback PDF
- **Send**: Review the drafts opened in your email client and send them

### 5. Daily Workflow
- The application displays the current day of the week at the top
- Each day gets its own folder (e.g., `20260125`)
- Student folders are created automatically when you add students or generate feedback

## 🛠️ Development

To compile the project locally:

```powershell
dotnet restore
dotnet build
dotnet run
```

## 🏗️ Architecture Highlights

- **SOLID Principles**: Separation of concerns with dedicated services
- **Dependency Injection**: All services are registered and injected via DI container
- **Service Layer**: 
  - `StudentService` handles all student-related operations (CRUD + file sync)
  - `NoteService` encapsulates file creation and system process execution
- **Validation**: `ErrorProvider` for real-time input validation in forms
- **Async Operations**: File I/O operations are asynchronous to keep UI responsive
- **Data Binding**: `BindingList<Student>` for automatic UI updates

## 📝 Notes

- Student folders are **never deleted** when removing students from the list (data safety)
- Email validation ensures proper format before saving
- Duplicate email addresses are prevented
- Folder names use hyphenated format (e.g., "John Doe" → "John-Doe")
- Note files follow the convention: `feedback-john-doe-20260125.txt`
- Email drafts are saved temporarily in `Documents/Feedback-Flow/TempEmails`

---

Developed to simplify the teaching workflow and streamline student feedback distribution.
