using System.ComponentModel;
using Feedback_Flow.Helpers;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow;

public partial class MainDashboard : Form
{
    private readonly IStudentService _studentService;
    private readonly INoteService _noteService;
    private readonly IFileSystemService _fileService;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;

    private SortableBindingList<Student> _students;
    private string _dailyFolderPath = string.Empty;

    public MainDashboard(
        IStudentService studentService,
        IFileSystemService fileService,
        IPdfService pdfService,
        IEmailService emailService,
        INoteService noteService)
    {
        _studentService = studentService;
        _fileService = fileService;
        _pdfService = pdfService;
        _emailService = emailService;
        _noteService = noteService;

        InitializeComponent();
        InitializeDataGrid();
    }

    #region Initialization

    private void InitializeDataGrid()
    {
        _students = new SortableBindingList<Student>("FullName", ListSortDirection.Ascending);
        dgvStudents.AutoGenerateColumns = false;

        // Fix: Grid must not be globally ReadOnly for checkboxes to work
        dgvStudents.ReadOnly = false;

        dgvStudents.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "AttendedClass",
            HeaderText = "Attended",
            DataPropertyName = "AttendedClass",
            Width = 60,
            ReadOnly = false
        });
        dgvStudents.Columns.Add(CreateColumn("StudentName", "Student", "FullName"));
        dgvStudents.Columns.Add(CreateColumn("ClassDay", "Class Day(s)", "ClassDay"));
        dgvStudents.Columns.Add(CreateColumn("LearningMaterial", "Learning Material", "AssignedMaterial"));
        dgvStudents.DataSource = _students;

        // Wire up manually added event handlers
        dgvStudents.CellContentClick += dgvStudents_CellContentClick;
    }

    private static DataGridViewTextBoxColumn CreateColumn(string name, string headerText, string dataPropertyName)
    {
        return new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = headerText,
            DataPropertyName = dataPropertyName,
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.Automatic
        };
    }

    private async void MainDashboard_Load(object sender, EventArgs e)
    {
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            _dailyFolderPath = _fileService.InitializeDailyFolder();

            // Default: Load filtered by current day
            await ReloadGridAsync();

            UpdateStatus($"Ready. Showing students for {_studentService.GetCurrentDayOfWeek()}.");
            lblDayOfWeek.Text = DateTime.Now.ToString("dddd", System.Globalization.CultureInfo.InvariantCulture);
        }, "Startup Error");
    }

    private bool _showAllStudents = false;

    private async Task ReloadGridAsync()
    {
        _students.Clear();

        List<Student> loaded;
        if (_showAllStudents)
        {
            loaded = await _studentService.GetAllStudentsAsync();
            btnToggleFilter.Text = "Show Today Only";
        }
        else
        {
            string today = _studentService.GetCurrentDayOfWeek();
            loaded = await _studentService.GetStudentsByDayAsync(today);
            btnToggleFilter.Text = "Show All Students";
        }

        foreach (var student in loaded)
            _students.Add(student);

        dgvStudents.Refresh();
    }

    private async void btnToggleFilter_Click(object sender, EventArgs e)
    {
        _showAllStudents = !_showAllStudents;
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            await ReloadGridAsync();
            UpdateStatus(_showAllStudents
                ? "Showing all students."
                : $"Showing students for {_studentService.GetCurrentDayOfWeek()}.");
        }, "Error toggling filter");
    }

    #endregion

    #region Cell Formatting

    private void dgvStudents_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvStudents.Columns[e.ColumnIndex].Name != "LearningMaterial") return;

        try
        {
            e.Value = FormatMaterialCell(e.Value?.ToString());
            e.FormattingApplied = true;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Formatting error: {ex.Message}");
            e.Value = "Error";
            e.FormattingApplied = true;
        }
    }

    private static string FormatMaterialCell(string? fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
            return "Not assigned";

        return File.Exists(fullPath) ? Path.GetFileName(fullPath) : "File not found";
    }

    #endregion

    #region Material Management

    private async void btnAssignMaterial_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedStudent(out var student)) return;

        var filePath = ShowMaterialFileDialog(student.FullName);
        if (string.IsNullOrEmpty(filePath)) return;

        await UpdateStudentMaterialAsync(student, filePath, $"Assigned material to {student.FullName}");
    }

    private async void btnUnassignMaterial_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedStudent(out var student)) return;

        if (string.IsNullOrWhiteSpace(student.AssignedMaterial))
        {
            ShowInfo($"{student.FullName} has no material assigned.", "Information");
            return;
        }

        if (!ShowConfirmation($"Unassign material from {student.FullName}?", "Confirm Unassignment"))
            return;

        await UpdateStudentMaterialAsync(student, string.Empty, $"Unassigned material from {student.FullName}");
    }

    private async void btnViewMaterial_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedStudent(out var student)) return;

        if (string.IsNullOrWhiteSpace(student.AssignedMaterial))
        {
            ShowInfo($"{student.FullName} has no material assigned.", "No Material");
            return;
        }

        if (!File.Exists(student.AssignedMaterial))
        {
            ShowWarning($"Material file not found:\n{student.AssignedMaterial}", "File Not Found");
            return;
        }

        OpenFile(student.AssignedMaterial, $"Opened material for {student.FullName}");
    }

    private async Task UpdateStudentMaterialAsync(Student student, string materialPath, string successMessage)
    {
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            student.AssignedMaterial = materialPath;
            await _studentService.UpdateStudentAsync(student, student);
            RefreshStudentInGrid(student);
            UpdateStatus(successMessage);
        }, "Error updating material");
    }

    private static string? ShowMaterialFileDialog(string studentName)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "All Supported Files|*.pdf;*.docx;*.doc;*.odt;*.pptx;*.ppt;*.odp|" +
                     "PDF Files (*.pdf)|*.pdf|" +
                     "Word Documents (*.docx;*.doc)|*.docx;*.doc|" +
                     "LibreOffice Writer (*.odt)|*.odt|" +
                     "PowerPoint (*.pptx;*.ppt)|*.pptx;*.ppt|" +
                     "LibreOffice Impress (*.odp)|*.odp",
            Title = $"Select Material for {studentName}"
        };

        return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
    }

    #endregion

    #region Student CRUD Operations

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using var form = new StudentForm();
        if (form.ShowDialog() != DialogResult.OK) return;

        await ExecuteWithErrorHandlingAsync(async () =>
        {
            var newStudent = new Student
            {
                FullName = form.StudentFullName,
                Email = form.StudentEmail,
                ClassDay = form.ClassDays
            };

            if (IsEmailDuplicate(newStudent.Email))
            {
                ShowWarning("Student with this email already exists.", "Validation");
                return;
            }

            await _studentService.AddStudentAsync(newStudent);
            //_students.Add(newStudent); // Remove direct add, let reload handle it to respect filter
            await ReloadGridAsync();
            UpdateStatus($"Added {newStudent.FullName}");
        }, "Error adding student");
    }

    private async void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedStudent(out var selectedStudent)) return;

        using var form = new StudentForm(selectedStudent);
        if (form.ShowDialog() != DialogResult.OK) return;

        await ExecuteWithErrorHandlingAsync(async () =>
        {
            var updatedStudent = new Student
            {
                FullName = form.StudentFullName,
                Email = form.StudentEmail,
                ClassDay = form.ClassDays,
                AssignedMaterial = selectedStudent.AssignedMaterial
            };

            if (IsEmailChanged(selectedStudent, updatedStudent) && IsEmailDuplicate(updatedStudent.Email))
            {
                ShowWarning("Another student with this email already exists.", "Validation");
                return;
            }

            await _studentService.UpdateStudentAsync(selectedStudent, updatedStudent);

            selectedStudent.FullName = updatedStudent.FullName;
            selectedStudent.Email = updatedStudent.Email;
            selectedStudent.ClassDay = updatedStudent.ClassDay;

            // If we are in filtered mode and the day was changed to something *not* today, we should probably remove it?
            // But for simplicity, we just leave it until refresh. Or we can just ReloadGrid.
            // KISS: Just refresh grid to be safe and correct.
            await ReloadGridAsync();

            UpdateStatus($"Updated {selectedStudent.FullName}");
        }, "Error updating student");
    }

    private async void btnRemove_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedStudent(out var selectedStudent)) return;

        if (!ShowConfirmation($"Remove {selectedStudent.FullName}?", "Confirm"))
            return;

        await ExecuteWithErrorHandlingAsync(async () =>
        {
            await _studentService.DeleteStudentAsync(selectedStudent);
            _students.Remove(selectedStudent);
            UpdateStatus($"Removed {selectedStudent.FullName}");
        }, "Error removing student");
    }

    #endregion

    #region Feedback Management

    private void dgvStudents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            btnEditFeedback_Click(sender, e);
    }

    private async void btnEditFeedback_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedStudent(out var selectedStudent)) return;

        await ExecuteWithErrorHandlingAsync(async () =>
        {
            string studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, selectedStudent);
            await _noteService.OpenOrCreateNotesAsync(studentFolder, selectedStudent.FullName);
        }, "Error opening notes", ex =>
        {
            if (ex is DirectoryNotFoundException)
            {
                ShowWarning(
                    "The student's folder has not been created properly. Please ensure the daily workspace is initialized.",
                    "Folder Missing");
            }
        });
    }

    #endregion

    #region Email Generation

    private async void btnGenerate_Click(object sender, EventArgs e)
    {
        if (_students.Count == 0)
        {
            ShowWarning("No students loaded.", "Warning");
            return;
        }

        btnGenerate.Enabled = false;
        UpdateStatus("Processing...");

        var result = await ProcessAllStudentsAsync();

        btnGenerate.Enabled = true;
        UpdateStatus(
            $"Done. Success: {result.SuccessCount}, Skipped: {result.SkippedStudents.Count}, Errors: {result.ErrorCount}");
        ShowCompletionMessage(result);
    }

    private async Task<GenerationResult> ProcessAllStudentsAsync()
    {
        var result = new GenerationResult();

        foreach (var student in _students)
        {
            UpdateStatus($"Processing: {student.FullName}");
            Application.DoEvents();

            if (!ValidateStudentMaterial(student, result))
                continue;

            try
            {
                await ProcessStudentAsync(student);
                result.SuccessCount++;
            }
            catch (Exception)
            {
                result.ErrorCount++;
            }
        }

        return result;
    }

    private async void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || dgvStudents.Columns[e.ColumnIndex].Name != "AttendedClass") return;

        // Commit change immediately to capture new value
        dgvStudents.CommitEdit(DataGridViewDataErrorContexts.Commit);

        var student = _students[e.RowIndex];
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            await _studentService.MarkAttendanceAsync(student, student.AttendedClass);
            UpdateStatus($"Marked {student.FullName}: " + (student.AttendedClass ? "Present" : "Absent"));
        }, "Error updating attendance");
    }

    private bool ValidateStudentMaterial(Student student, GenerationResult result)
    {
        // 1. Check Attendance
        if (!student.AttendedClass)
        {
            UpdateStatus($"Skipping {student.FullName} (Absent)");
            return false; // Silently skip absent students
        }

        // 2. Validate Material (Optional now, but if path is set, it must exist)
        if (!string.IsNullOrEmpty(student.AssignedMaterial) && !File.Exists(student.AssignedMaterial))
        {
            // If assigned but missing, it's an error/warning scenario? 
            // Requirement says: "Material attachment is still included if AssignedMaterial is assigned"
            // I'll log it but proceed? Or skip? 
            // New flow: "Generate emails -> Only students who attended get emails (with or without material)"
            // So if file is missing but string is there, we should probably warn or clear it. 
            // To be safe: warn and skip attachment, or fail? 
            // Current logic skipped student. I will keep it strict: if path is assigned, file MUST exist.
            result.SkippedStudents.Add($"{student.FullName} (Material Missing)");
            UpdateStatus($"Skipping {student.FullName} (Missing Material File)");
            return false;
        }

        return true;
    }

    private async Task ProcessStudentAsync(Student student)
    {
        string studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, student);
        string content = _fileService.GetStudentNoteContent(studentFolder) ??
                         "No specific notes found for this student.";
        string studentPdfPath = _pdfService.GenerateStudentPdf(student, content, studentFolder);

        _emailService.DraftEmail(student, studentPdfPath);
    }

    private static void ShowCompletionMessage(GenerationResult result)
    {
        var message = $"Process Completed.\nSuccess: {result.SuccessCount}\nErrors: {result.ErrorCount}";

        if (result.SkippedStudents.Any())
        {
            message += $"\n\nSkipped ({result.SkippedStudents.Count}) - No Material:\n- " +
                       string.Join("\n- ", result.SkippedStudents);
        }

        MessageBox.Show(message, "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    #endregion

    #region Helper Methods

    private bool TryGetSelectedStudent(out Student student)
    {
        student = dgvStudents.CurrentRow?.DataBoundItem as Student;

        if (student != null) return true;

        ShowWarning("Please select a student first.", "Warning");
        return false;
    }

    private bool IsEmailDuplicate(string email) =>
        _students.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    private static bool IsEmailChanged(Student original, Student updated) =>
        !original.Email.Equals(updated.Email, StringComparison.OrdinalIgnoreCase);

    private void RefreshStudentInGrid(Student student)
    {
        var index = _students.IndexOf(student);
        if (index >= 0)
            _students.ResetItem(index);
    }

    private void UpdateStatus(string message) => lblStatus.Text = message;

    private async Task ExecuteWithErrorHandlingAsync(
        Func<Task> action,
        string errorTitle,
        Action<Exception>? customErrorHandler = null)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            customErrorHandler?.Invoke(ex);
            if (customErrorHandler == null)
                ShowError($"{errorTitle}: {ex.Message}", "Error");
        }
    }

    private static void OpenFile(string filePath, string successMessage)
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
        catch (Exception ex)
        {
            ShowError($"Error opening file: {ex.Message}", "Error");
        }
    }

    private static void ShowWarning(string message, string title) =>
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    private static void ShowInfo(string message, string title) =>
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

    private static void ShowError(string message, string title) =>
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

    private static bool ShowConfirmation(string message, string title) =>
        MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

    #endregion

    #region Result Class

    private class GenerationResult
    {
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> SkippedStudents { get; } = new();
    }

    #endregion
}