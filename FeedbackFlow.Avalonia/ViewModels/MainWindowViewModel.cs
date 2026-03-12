using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;
using Avalonia.Controls;

namespace FeedbackFlow.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IStudentService _studentService;
    private readonly IFileSystemService _fileService;
    private readonly INoteService _noteService;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;

    private List<Student> _allStudentsCache = new();
    private List<StudentSessionView> _allSessions = new();
    private string _dailyFolderPath = string.Empty;
    private Window? _mainWindow;

    [ObservableProperty]
    private ObservableCollection<StudentSessionView> _sessions = new();

    [ObservableProperty]
    private ObservableCollection<Student> _allStudents = new();

    [ObservableProperty]
    private StudentSessionView? _selectedSession;

    [ObservableProperty]
    private Student? _selectedStudent;

    [ObservableProperty]
    private DateTimeOffset _selectedDate = DateTimeOffset.Now;

    [ObservableProperty]
    private bool _showAllStudents;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    [ObservableProperty]
    private string _modeTitle = string.Empty;

    [ObservableProperty]
    private string _modeDescription = string.Empty;

    [ObservableProperty]
    private bool _isDescriptionPanelVisible;

    [ObservableProperty]
    private string _descriptionTitle = string.Empty;

    [ObservableProperty]
    private string _descriptionText = string.Empty;

    [ObservableProperty]
    private bool _isDarkMode;

    [ObservableProperty]
    private string _versionText = string.Empty;

    [ObservableProperty]
    private bool _canEditAttendance;

    [ObservableProperty]
    private bool _canEditFeedback;

    [ObservableProperty]
    private bool _canAssignMaterial;

    public MainWindowViewModel(
        IStudentService studentService,
        IFileSystemService fileService,
        INoteService noteService,
        IPdfService pdfService,
        IEmailService emailService)
    {
        _studentService = studentService;
        _fileService = fileService;
        _noteService = noteService;
        _pdfService = pdfService;
        _emailService = emailService;

        InitializeAsync();
    }

    public void SetMainWindow(Window window)
    {
        _mainWindow = window;
    }

    private async void InitializeAsync()
    {
        try
        {
            _dailyFolderPath = _fileService.InitializeDailyFolder(DateTime.Today);
            
            var version = GetType().Assembly.GetName().Version;
            VersionText = $"v{version?.ToString(3) ?? "1.0.0"}";
            
            await LoadDataAsync();
            UpdateModeDisplay();
            UpdateStatus($"Ready. Showing students for {_studentService.GetDayOfWeek(DateTime.Today)}.");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Startup error: {ex.Message}");
        }
    }

    private async Task LoadDataAsync()
    {
        if (ShowAllStudents)
        {
            _allStudentsCache = await _studentService.GetAllStudentsAsync();
            AllStudents = new ObservableCollection<Student>(_allStudentsCache);
            FilterBySearch();
        }
        else
        {
            var selectedDate = SelectedDate.DateTime.Date;
            _allSessions = await _studentService.GetSessionViewsAsync(selectedDate);
            Sessions = new ObservableCollection<StudentSessionView>(_allSessions);
            UpdateDescriptionPanel();
        }
    }

    partial void OnSelectedDateChanged(DateTimeOffset value)
    {
        if (!ShowAllStudents)
        {
            _ = LoadDataAsync();
        }
    }

    partial void OnShowAllStudentsChanged(bool value)
    {
        UpdateModeDisplay();
        _ = LoadDataAsync();
    }

    partial void OnSearchTextChanged(string value)
    {
        if (ShowAllStudents)
        {
            FilterBySearch();
        }
    }

    partial void OnSelectedSessionChanged(StudentSessionView? value)
    {
        UpdateDescriptionPanel();
        UpdateButtonStates();
    }

    private void FilterBySearch()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            AllStudents = new ObservableCollection<Student>(_allStudentsCache);
            UpdateStatus($"Viewing all students ({AllStudents.Count} total)");
        }
        else
        {
            var filtered = _allStudentsCache
                .Where(s => s.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
            AllStudents = new ObservableCollection<Student>(filtered);
            UpdateStatus(filtered.Count == 0 
                ? $"No students found matching '{SearchText}'" 
                : $"Found {filtered.Count} of {_allStudentsCache.Count} students");
        }
    }

    private void UpdateModeDisplay()
    {
        var isToday = SelectedDate.DateTime.Date == DateTime.Today;
        
        if (ShowAllStudents)
        {
            ModeTitle = "Viewing ALL students (all days)";
            ModeDescription = "Day-specific actions are disabled";
        }
        else
        {
            var dayName = _studentService.GetDayOfWeek(SelectedDate.DateTime.Date);
            var dateLabel = isToday ? dayName : $"{dayName} ({SelectedDate.DateTime.Date:dd MMM yyyy})";
            ModeTitle = $"Showing students for: {dateLabel}";
            ModeDescription = isToday ? "Ready to manage today's class" : "Viewing a previous class session";
        }
    }

    private void UpdateButtonStates()
    {
        var isToday = SelectedDate.DateTime.Date == DateTime.Today;
        
        if (ShowAllStudents)
        {
            CanEditAttendance = false;
            CanEditFeedback = false;
            CanAssignMaterial = false;
        }
        else
        {
            CanEditAttendance = isToday;
            CanEditFeedback = true;
            CanAssignMaterial = true;
        }
    }

    private void UpdateDescriptionPanel()
    {
        if (ShowAllStudents || SelectedSession == null)
        {
            IsDescriptionPanelVisible = false;
            return;
        }

        var hasDescription = !string.IsNullOrWhiteSpace(SelectedSession.ClassDescription);
        IsDescriptionPanelVisible = hasDescription;

        if (hasDescription)
        {
            DescriptionTitle = $"Class description — {SelectedSession.FullName}:";
            DescriptionText = SelectedSession.ClassDescription;
        }
    }

    private void UpdateStatus(string message) => StatusMessage = message;

    [RelayCommand]
    private Task ToggleFilter()
    {
        ShowAllStudents = !ShowAllStudents;
        SearchText = string.Empty;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task AddStudent()
    {
        if (_mainWindow == null) return;

        var dialog = new Views.StudentDialog();
        var result = await dialog.ShowDialog<bool>(_mainWindow);
        
        if (result)
        {
            var newStudent = new Student
            {
                FullName = dialog.StudentFullName,
                Email = dialog.StudentEmail,
                ClassDay = dialog.ClassDays
            };

            if (IsEmailDuplicate(newStudent.Email))
            {
                UpdateStatus("Student with this email already exists.");
                return;
            }

            await _studentService.AddStudentAsync(newStudent);
            await LoadDataAsync();
            UpdateStatus($"Added {newStudent.FullName}");
        }
    }

    [RelayCommand]
    private async Task UpdateStudent()
    {
        if (_mainWindow == null || SelectedStudent == null) return;

        var dialog = new Views.StudentDialog(SelectedStudent);
        var result = await dialog.ShowDialog<bool>(_mainWindow);
        
        if (result)
        {
            var updatedStudent = new Student
            {
                FullName = dialog.StudentFullName,
                Email = dialog.StudentEmail,
                ClassDay = dialog.ClassDays
            };

            if (IsEmailChanged(SelectedStudent, updatedStudent) && IsEmailDuplicate(updatedStudent.Email))
            {
                UpdateStatus("Another student with this email already exists.");
                return;
            }

            await _studentService.UpdateStudentAsync(SelectedStudent, updatedStudent);
            await LoadDataAsync();
            UpdateStatus($"Updated {updatedStudent.FullName}");
        }
    }

    [RelayCommand]
    private async Task RemoveStudent()
    {
        if (SelectedStudent == null) return;

        await _studentService.DeleteStudentAsync(SelectedStudent);
        await LoadDataAsync();
        UpdateStatus($"Removed {SelectedStudent.FullName}");
    }

    [RelayCommand]
    private async Task ToggleAttendance(StudentSessionView session)
    {
        await _studentService.MarkAttendanceAsync(session.SessionId, session.Attended);
        UpdateStatus($"Marked {session.FullName}: {(session.Attended ? "Present" : "Absent")}");
    }

    [RelayCommand]
    private async Task AssignMaterial()
    {
        if (!CanAssignMaterial || SelectedSession == null) return;
        UpdateStatus("Use file picker to select material (feature coming soon)");
    }

    [RelayCommand]
    private async Task UnassignMaterial()
    {
        if (!CanAssignMaterial || SelectedSession == null) return;

        if (string.IsNullOrWhiteSpace(SelectedSession.AssignedMaterial))
        {
            UpdateStatus($"{SelectedSession.FullName} has no material assigned.");
            return;
        }

        await _studentService.UpdateSessionMaterialAsync(SelectedSession.SessionId, null);
        SelectedSession.AssignedMaterial = null;
        UpdateStatus($"Unassigned material from {SelectedSession.FullName}");
    }

    [RelayCommand]
    private async Task ViewMaterial()
    {
        if (SelectedSession == null || string.IsNullOrWhiteSpace(SelectedSession.AssignedMaterial))
        {
            UpdateStatus("No material assigned.");
            return;
        }

        if (!File.Exists(SelectedSession.AssignedMaterial))
        {
            UpdateStatus($"File not found: {SelectedSession.AssignedMaterial}");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = SelectedSession.AssignedMaterial,
                UseShellExecute = true
            });
            UpdateStatus($"Opened material for {SelectedSession.FullName}");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error opening file: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task EditFeedback()
    {
        if (SelectedSession == null) return;

        try
        {
            var student = new Student
            {
                Id = SelectedSession.StudentId,
                FullName = SelectedSession.FullName,
                Email = SelectedSession.Email,
                ClassDay = SelectedSession.ClassDay
            };
            var studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, student);
            await _noteService.OpenOrCreateNotesAsync(studentFolder, SelectedSession.FullName, SelectedDate.DateTime.Date);
        }
        catch (DirectoryNotFoundException)
        {
            UpdateStatus("The student's folder has not been created properly.");
        }
    }

    [RelayCommand]
    private async Task PrepareNextClass()
    {
        if (SelectedSession == null || _mainWindow == null) return;

        var referenceDate = SelectedDate.DateTime.Date < DateTime.Today ? DateTime.Today : SelectedDate.DateTime.Date;
        var nextDate = ComputeNextClassDate(SelectedSession.ClassDay, referenceDate);
        
        if (nextDate == null)
        {
            UpdateStatus($"The next class date for {SelectedSession.FullName} could not be determined.");
            return;
        }

        var existingSession = await _studentService.GetNextClassSessionAsync(SelectedSession.StudentId, nextDate.Value);

        var dialog = new Views.PrepareClassDialog(SelectedSession.FullName, nextDate.Value, existingSession);
        var result = await dialog.ShowDialog<bool>(_mainWindow);

        if (result)
        {
            await _studentService.SaveNextClassPlanAsync(
                SelectedSession.StudentId, 
                nextDate.Value, 
                dialog.SelectedMaterial, 
                dialog.ClassDescription);
            UpdateStatus($"Next class prepared for {SelectedSession.FullName} ({nextDate.Value:dd MMM yyyy}).");
        }
    }

    [RelayCommand]
    private async Task GenerateEmail()
    {
        if (SelectedSession == null) return;

        if (!SelectedSession.Attended)
        {
            UpdateStatus($"{SelectedSession.FullName} did not attend this class.");
            return;
        }

        try
        {
            var student = new Student
            {
                Id = SelectedSession.StudentId,
                FullName = SelectedSession.FullName,
                Email = SelectedSession.Email,
                ClassDay = SelectedSession.ClassDay
            };
            var studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, student);
            var content = _fileService.GetStudentNoteContent(studentFolder) ?? "No specific notes found.";
            var pdfPath = _pdfService.GenerateStudentPdf(SelectedSession, content, studentFolder);
            _emailService.DraftEmail(SelectedSession, pdfPath);
            UpdateStatus($"Email drafted for {SelectedSession.FullName}.");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error generating email: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = string.Empty;
    }

    private bool IsEmailDuplicate(string email) =>
        _allStudentsCache.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    private static bool IsEmailChanged(Student original, Student updated) =>
        !original.Email.Equals(updated.Email, StringComparison.OrdinalIgnoreCase);

    private static DateTime? ComputeNextClassDate(string classDays, DateTime fromDate)
    {
        var days = classDays.Split(',')
            .Select(d => d.Trim())
            .Select(d => Enum.TryParse<DayOfWeek>(d, true, out var day) ? (DayOfWeek?)day : null)
            .Where(d => d.HasValue)
            .Select(d => d!.Value)
            .ToHashSet();

        if (days.Count == 0) return null;

        var candidate = fromDate.AddDays(1);
        for (int i = 0; i < 7; i++, candidate = candidate.AddDays(1))
        {
            if (days.Contains(candidate.DayOfWeek))
                return candidate;
        }
        return null;
    }
}
