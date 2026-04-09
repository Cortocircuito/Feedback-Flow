using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FeedbackFlow.Core.Models;
using FeedbackFlow.Core.Services.Interfaces;
using Avalonia.Controls;

namespace FeedbackFlow.App.ViewModels;

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
    private CancellationTokenSource? _statusCts;

    [ObservableProperty]
    private ObservableCollection<StudentSessionView> _sessions = new();

    [ObservableProperty]
    private ObservableCollection<Student> _allStudents = new();

    [ObservableProperty]
    private StudentSessionView? _selectedSession;

    [ObservableProperty]
    private Student? _selectedStudent;

    [ObservableProperty]
    private DateTime? _selectedDate = DateTime.Today;

    [ObservableProperty]
    private bool _showAllStudents;

    [ObservableProperty]
    private bool _isTodayMode = true;

    [ObservableProperty]
    private bool _isHistoryMode;

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

    // D1: Loading indicator
    [ObservableProperty]
    private bool _isLoading;

    // D2: Empty state message
    [ObservableProperty]
    private string _emptyStateMessage = string.Empty;

    public bool IsDayViewMode => IsTodayMode || IsHistoryMode;

    // D2: Computed empty state visibility
    public bool IsEmptyState => !IsLoading && IsDayViewMode && Sessions.Count == 0;

    public IBrush ModeBannerBrush
    {
        get
        {
            if (ShowAllStudents) return new SolidColorBrush(Color.Parse("#1565C0"));
            var date = SelectedDate?.Date ?? DateTime.Today;
            return date == DateTime.Today
                ? new SolidColorBrush(Color.Parse("#2E7D32"))
                : new SolidColorBrush(Color.Parse("#E65100"));
        }
    }

    public DateTime MaxSelectableDate => DateTime.Today;

    // D4: Theme preference file path
    private static string ThemeFilePath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Feedback-Flow", ".theme");

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

            // D4 + E: Load saved theme or sync with system theme
            LoadSavedTheme();

            await LoadDataAsync();
            UpdateModeDisplay();
            UpdateStatus($"Ready. Showing students for {_studentService.GetDayOfWeek(DateTime.Today)}.");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Startup error: {ex.Message}");
        }
    }

    // D4: Load persisted theme; fall back to actual system theme (E fix)
    private void LoadSavedTheme()
    {
        try
        {
            if (File.Exists(ThemeFilePath))
            {
                var saved = File.ReadAllText(ThemeFilePath).Trim();
                IsDarkMode = saved == "dark";
                if (Avalonia.Application.Current != null)
                    Avalonia.Application.Current.RequestedThemeVariant = IsDarkMode
                        ? Avalonia.Styling.ThemeVariant.Dark
                        : Avalonia.Styling.ThemeVariant.Light;
                return;
            }
        }
        catch { /* best-effort */ }

        // E: Sync IsDarkMode with the actual resolved theme so first toggle does something visible
        if (Avalonia.Application.Current != null)
            IsDarkMode = Avalonia.Application.Current.ActualThemeVariant ==
                         Avalonia.Styling.ThemeVariant.Dark;
    }

    // D4: Persist theme preference to disk
    private void PersistTheme()
    {
        try
        {
            var dir = Path.GetDirectoryName(ThemeFilePath)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(ThemeFilePath, IsDarkMode ? "dark" : "light");
        }
        catch { /* best-effort */ }
    }

    private async Task LoadDataAsync()
    {
        // D1: Show loading indicator
        IsLoading = true;
        try
        {
            if (ShowAllStudents)
            {
                _allStudentsCache = await _studentService.GetAllStudentsAsync();
                AllStudents = new ObservableCollection<Student>(_allStudentsCache);
                FilterBySearch();
            }
            else
            {
                var selectedDate = SelectedDate?.Date ?? DateTime.Today;
                _allSessions = await _studentService.GetSessionViewsAsync(selectedDate);
                Sessions = new ObservableCollection<StudentSessionView>(_allSessions);
                OnPropertyChanged(nameof(IsEmptyState));
                UpdateDescriptionPanel();
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    // D1+D2: Notify IsEmptyState when IsLoading or Sessions change
    partial void OnIsLoadingChanged(bool value) => OnPropertyChanged(nameof(IsEmptyState));
    partial void OnSessionsChanged(ObservableCollection<StudentSessionView> value) =>
        OnPropertyChanged(nameof(IsEmptyState));

    partial void OnSelectedDateChanged(DateTime? value)
    {
        OnPropertyChanged(nameof(ModeBannerBrush));
        if (!ShowAllStudents)
        {
            UpdateModeDisplay();
            _ = LoadDataAsync();
        }
    }

    partial void OnShowAllStudentsChanged(bool value)
    {
        OnPropertyChanged(nameof(IsDayViewMode));
        OnPropertyChanged(nameof(ModeBannerBrush));
        OnPropertyChanged(nameof(IsEmptyState));
        UpdateModeDisplay();
        _ = LoadDataAsync();
    }

    partial void OnIsTodayModeChanged(bool value)
    {
        OnPropertyChanged(nameof(IsDayViewMode));
        OnPropertyChanged(nameof(ModeBannerBrush));
        OnPropertyChanged(nameof(IsEmptyState));
    }

    partial void OnIsHistoryModeChanged(bool value)
    {
        OnPropertyChanged(nameof(IsDayViewMode));
        OnPropertyChanged(nameof(ModeBannerBrush));
        OnPropertyChanged(nameof(IsEmptyState));
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
        var date = SelectedDate?.Date ?? DateTime.Today;
        var isToday = date == DateTime.Today;

        if (ShowAllStudents)
        {
            ModeTitle = "All Students";
            ModeDescription = "Manage student records";
            EmptyStateMessage = string.Empty;
        }
        else
        {
            var dayName = _studentService.GetDayOfWeek(date);
            var dateLabel = isToday ? dayName : $"{dayName} ({date:dd MMM yyyy})";
            ModeTitle = $"Showing students for: {dateLabel}";
            ModeDescription = isToday ? "Ready to manage today's class" : "Viewing a previous class session";
            // D2: Context-appropriate empty state message
            EmptyStateMessage = $"No students scheduled for {dayName}";
        }
    }

    private void UpdateButtonStates()
    {
        var date = SelectedDate?.Date ?? DateTime.Today;
        var isToday = date == DateTime.Today;

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
            DescriptionText = SelectedSession.ClassDescription!;
        }
    }

    // D3: Auto-clear status bar after 5 seconds
    private void UpdateStatus(string message)
    {
        StatusMessage = message;
        _statusCts?.Cancel();
        _statusCts = new CancellationTokenSource();
        var token = _statusCts.Token;
        _ = Task.Delay(5000, token).ContinueWith(
            _ => Avalonia.Threading.Dispatcher.UIThread.Post(() => StatusMessage = string.Empty),
            TaskContinuationOptions.OnlyOnRanToCompletion);
    }

    [RelayCommand]
    private void NavigateToToday()
    {
        IsTodayMode = true;
        IsHistoryMode = false;
        ShowAllStudents = false;
        SelectedDate = DateTime.Today;
        SearchText = string.Empty;
    }

    [RelayCommand]
    private void NavigateToHistory()
    {
        IsTodayMode = false;
        IsHistoryMode = true;
        ShowAllStudents = false;
        SearchText = string.Empty;
        // OnShowAllStudentsChanged only fires if value changed; explicitly reload for same-mode transitions
        UpdateModeDisplay();
        _ = LoadDataAsync();
    }

    [RelayCommand]
    private void NavigateToAllStudents()
    {
        IsTodayMode = false;
        IsHistoryMode = false;
        ShowAllStudents = true; // OnShowAllStudentsChanged handles reload
    }

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

    // B: Confirm before removing a student
    [RelayCommand]
    private async Task RemoveStudent()
    {
        if (_mainWindow == null || SelectedStudent == null) return;

        var dialog = new Views.ConfirmDialog(
            $"Remove {SelectedStudent.FullName}?\n\nThis will also delete all their session history.");
        if (!await dialog.ShowDialog<bool>(_mainWindow)) return;

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
        if (!CanAssignMaterial || SelectedSession == null || _mainWindow == null) return;

        var options = new FilePickerOpenOptions
        {
            Title = "Select Learning Material",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("All Supported Files")
                {
                    Patterns = new[] { "*.pdf", "*.docx", "*.doc", "*.odt", "*.pptx", "*.ppt", "*.odp" }
                },
                new FilePickerFileType("PDF Files") { Patterns = new[] { "*.pdf" } },
                new FilePickerFileType("Word Documents") { Patterns = new[] { "*.docx", "*.doc" } },
                new FilePickerFileType("PowerPoint") { Patterns = new[] { "*.pptx", "*.ppt" } }
            }
        };

        var result = await _mainWindow.StorageProvider.OpenFilePickerAsync(options);
        if (result.Count > 0)
        {
            var path = result[0].Path.LocalPath;
            await _studentService.UpdateSessionMaterialAsync(SelectedSession.SessionId, path);
            SelectedSession.AssignedMaterial = path;
            UpdateStatus($"Assigned material to {SelectedSession.FullName}: {Path.GetFileName(path)}");
        }
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
            var date = SelectedDate?.Date ?? DateTime.Today;
            await _noteService.OpenOrCreateNotesAsync(studentFolder, SelectedSession.FullName, date);
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

        var date = SelectedDate?.Date ?? DateTime.Today;
        var referenceDate = date < DateTime.Today ? DateTime.Today : date;
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

    // D4 + E: Toggle theme, persist preference
    [RelayCommand]
    private void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        if (Avalonia.Application.Current != null)
            Avalonia.Application.Current.RequestedThemeVariant = IsDarkMode
                ? Avalonia.Styling.ThemeVariant.Dark
                : Avalonia.Styling.ThemeVariant.Light;
        PersistTheme();
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
