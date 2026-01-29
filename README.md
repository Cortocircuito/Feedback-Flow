# Feedback Flow

**Feedback Flow** is a desktop application developed in C# WinForms (.NET 10) designed to automate the distribution of personalized feedback to students.

## 📑 Changelog

See `CHANGELOG.md` for release notes and version history.

## 🚀 Features

- **Student Management (CRUD)**: Add, edit, and remove students with a dedicated dialog form
- **Class Day Assignment**: Assign one or more class days per student
- **Attendance Tracking**: Mark attendance directly in the grid for the current day
- **Material Assignment**: Assign optional learning materials (PDF, Word, PowerPoint, LibreOffice)
- **Direct Note Editing**: One-click access to student feedback notes
- **Automated Organization**: Daily folders (`YYYYMMDD`) within `Documents/Feedback-Flow`
- **SQLite Storage**: Reliable local database with automatic CSV migration
- **PDF Generation**: Converts notes to PDFs using iText 9
- **Email Integration**: Generates `.eml` drafts with optional materials attached
- **Modern Architecture**: Dependency Injection and service-oriented design
- **Day Display**: Shows current weekday on the dashboard

## 🛠️ Development

## 🏗️ Architecture Highlights

- **SOLID Principles**: Service-based architecture
- **Dependency Injection**: All services registered via DI
- **Async Operations**: Responsive UI with async DB access
- **SQLite + Dapper**: Lightweight, fast data access
- **Automatic Migration**: One-time CSV import

## 📝 Notes

- Student folders are **never deleted** when removing students
- CSV is migrated and renamed to `students.csv.migrated`
- Note files follow: `feedback-john-doe-20260125.txt`
- Email drafts are saved in `Documents/Feedback-Flow/TempEmails`

---

Developed to simplify the teaching workflow and streamline student feedback distribution.
