using System.ComponentModel;
using Feedback_Flow.Helpers;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow.UI.Forms;

public sealed partial class MainDashboard : Form
{
    private readonly IStudentService _studentService;
    private readonly INoteService _noteService;
    private readonly IFileSystemService _fileService;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;

    // Day mode: binds StudentSessionView (Student + today's ClassSession)
    private SortableBindingList<StudentSessionView> _sessions;
    // Show-All mode: binds plain Student for CRUD
    private SortableBindingList<Student> _allStudentsList;
    private string _dailyFolderPath = string.Empty;

    // Search Functionality
    private Panel? _panelSearch;
    private TextBox? _txtSearch;
    private Button? _btnClearSearch;
    private Label? _lblSearchIcon;
    private List<Student> _allStudentsCache = new();

    // Layout State
    private bool _searchLayoutActive = false;

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
        this.MinimumSize = this.Size; // Set minimum size to current size

        InitializeDataGrid();
        InitializeSearchPanel();
        InitializeVersionLabel();
    }

    private void InitializeVersionLabel()
    {
        // Enable tooltips for the StatusStrip
        statusStrip1.ShowItemToolTips = true;

        // Add a spring label to push the version to the right
        var springLabel = new ToolStripStatusLabel
        {
            Spring = true
        };

        // Get version from assembly
        var rawVersion = Application.ProductVersion;
        var displayVersion = rawVersion;
        var tooltipText = $"Build: {rawVersion}";

        // Tries to split "1.0.7+abc123..."
        if (!string.IsNullOrEmpty(rawVersion) && rawVersion.Contains('+'))
        {
            var parts = rawVersion.Split('+');
            if (parts.Length == 2)
            {
                var ver = parts[0];
                var hash = parts[1];

                // Shorten hash to 7 chars if possible
                var shortHash = hash.Length >= 7 ? hash[0..7] : hash;

                displayVersion = $"{ver} ({shortHash})";
            }
        }

        // Create version label
        var versionLabel = new ToolStripStatusLabel
        {
            Text = $"v{displayVersion}",
            ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 9f),
            Margin = new Padding(0, 0, 10, 0),
            ToolTipText = tooltipText
        };

        statusStrip1.Items.Add(springLabel);
        statusStrip1.Items.Add(versionLabel);
    }

    #region Initialization

    protected override void OnDpiChanged(DpiChangedEventArgs e)
    {
        base.OnDpiChanged(e);
        PerformLayout();
    }

    private void InitializeDataGrid()
    {
        // Day-mode binding list
        _sessions = new SortableBindingList<StudentSessionView>("FullName", ListSortDirection.Ascending);
        // All-students-mode binding list
        _allStudentsList = new SortableBindingList<Student>("FullName", ListSortDirection.Ascending);

        dgvStudents.AutoGenerateColumns = false;
        dgvStudents.ReadOnly = false;

        // DataPropertyName maps to StudentSessionView.Attended
        dgvStudents.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "AttendedClass",
            HeaderText = "Attended",
            DataPropertyName = "Attended",
            Width = 60,
            ReadOnly = false
        });
        dgvStudents.Columns.Add(CreateColumn("StudentName", "Student", "FullName"));
        dgvStudents.Columns.Add(CreateColumn("ClassDay", "Class Day(s)", "ClassDay"));
        dgvStudents.Columns.Add(CreateColumn("LearningMaterial", "Learning Material", "AssignedMaterial"));

        // Start in day mode
        dgvStudents.DataSource = _sessions;

        dgvStudents.CellContentClick += dgvStudents_CellContentClick;
        InitializeTooltips();
    }

    private void InitializeSearchPanel()
    {
        // 1. Create Container Panel
        _panelSearch = new Panel
        {
            Name = "panelSearch",
            Visible = false, // Hidden by default
            BackColor = Color.FromArgb(250, 250, 250),
            BorderStyle = BorderStyle.FixedSingle,
            Height = 45,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // 2. Create Icon
        _lblSearchIcon = new Label
        {
            Text = "🔍",
            AutoSize = true,
            Location = new Point(10, 12),
            Font = new Font("Segoe UI Emoji", 10f)
        };

        // 3. Create TextBox
        _txtSearch = new TextBox
        {
            Name = "txtSearchStudent",
            Location = new Point(40, 10),
            Width = 300,
            Font = new Font("Segoe UI", 10f),
            PlaceholderText = "Type student name to search..."
        };
        _txtSearch.TextChanged += TxtSearch_TextChanged;
        _txtSearch.KeyDown += TxtSearch_KeyDown;

        // 4. Create Clear Button
        _btnClearSearch = new Button
        {
            Text = "✕",
            Location = new Point(350, 9),
            Size = new Size(40, 25),
            FlatStyle = FlatStyle.Flat,
            Enabled = false,
            BackColor = Color.White
        };
        _btnClearSearch.FlatAppearance.BorderSize = 0;
        _btnClearSearch.Click += BtnClearSearch_Click;

        // 5. Assemble
        _panelSearch.Controls.Add(_lblSearchIcon);
        _panelSearch.Controls.Add(_txtSearch);
        _panelSearch.Controls.Add(_btnClearSearch);

        // 6. Add to Mode Indicator Panel instead of Form
        _panelSearch.Location = new Point(10, 60); // Below the labels
        _panelSearch.Width = panelModeIndicator.Width - 20;
        _panelSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panelModeIndicator.Controls.Add(_panelSearch);
    }

    private void InitializeTooltips()
    {
        // Set initial tooltips for day-specific actions (will be updated based on mode)
        toolTip.SetToolTip(btnAssignMaterial, "Assign learning material to selected student");
        toolTip.SetToolTip(btnUnassignMaterial, "Remove assigned material from selected student");
        toolTip.SetToolTip(btnViewMaterial, "View the assigned material file");
        toolTip.SetToolTip(btnEditFeedback, "Edit feedback notes for selected student");
        toolTip.SetToolTip(btnGenerate, "Generate feedback emails for students who attended");

        // Set tooltips for CRUD operations
        toolTip.SetToolTip(btnAdd, "Add a new student to the system");
        toolTip.SetToolTip(btnUpdate, "Edit the selected student's information");
        toolTip.SetToolTip(btnRemove, "Remove the selected student from the system");

        // Set tooltip for filter toggle
        toolTip.SetToolTip(btnToggleFilter, "Switch between viewing all students or today's students only");
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
        await ExecuteWithErrorHandlingAsync(
            async () =>
            {
                _dailyFolderPath = _fileService.InitializeDailyFolder();

                // Initialize date picker at today (MaxDate prevents future dates)
                dtpClassDate.Value = DateTime.Today;

                // Default: Load filtered by current day
                await ReloadGridAsync();

                var currentDay = _studentService.GetDayOfWeek(DateTime.Today);
                lblModeTitle.Text = $"Showing students for: {currentDay}";
                UpdateStatus($"Ready. Showing students for {currentDay}.");
            }, "Startup Error");
    }

    private bool _showAllStudents = false;

    private async Task ReloadGridAsync()
    {
        if (_showAllStudents)
        {
            // All-students mode: show identity only, swap to Student binding list
            _allStudentsCache = await _studentService.GetAllStudentsAsync();
            dgvStudents.DataSource = _allStudentsList;
            FilterStudentsBySearch(_txtSearch?.Text ?? string.Empty);
        }
        else
        {
            // Day mode: load sessions for the date selected in the date picker
            var selectedDate = dtpClassDate.Value.Date;
            var dayName = _studentService.GetDayOfWeek(selectedDate);

            _sessions.Clear();
            dgvStudents.DataSource = _sessions;

            var sessions = await _studentService.GetSessionViewsAsync(selectedDate);
            foreach (var session in sessions)
                _sessions.Add(session);

            // Update mode indicator title to reflect the selected date
            bool isToday = selectedDate == DateTime.Today;
            string dateLabel = isToday ? dayName : $"{dayName} ({selectedDate:dd MMM yyyy})";
            lblModeTitle.Text = $"Showing students for: {dateLabel}";
            UpdateStatus($"Showing {dateLabel}'s students ({_sessions.Count} students)");
        }

        dgvStudents.Refresh();
    }

    private async void btnToggleFilter_Click(object sender, EventArgs e)
    {
        _showAllStudents = !_showAllStudents;
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            UpdateUIForViewMode();
            await ReloadGridAsync();
        }, "Error toggling filter");
    }

    private async void dtpClassDate_ValueChanged(object sender, EventArgs e)
    {
        // Only reload when in day mode; all-students mode ignores the date picker
        if (_showAllStudents) return;
        await ExecuteWithErrorHandlingAsync(ReloadGridAsync, "Error loading sessions for selected date");
    }

    private void UpdateUIForViewMode()
    {
        if (_showAllStudents)
        {
            SetAllStudentsMode();
        }
        else
        {
            SetCurrentDayMode();
        }
    }

    private void SetAllStudentsMode()
    {
        // Visual feedback
        UpdateModeIndicator(true);
        btnToggleFilter.Text = "Show Today Only";

        // Enable Search Mode Layout
        SetSearchLayout(true);

        // Disable day-specific buttons
        ToggleDaySpecificActions(false);

        // Hide day-specific columns
        SetDaySpecificColumnsVisible(false);

        // Disable and hide the date picker — it's meaningless in Show All mode
        dtpClassDate.Enabled = false;
        dtpClassDate.Visible = false;

        // Text will be updated by filtering logic
    }

    private void SetCurrentDayMode()
    {
        // Visual feedback
        UpdateModeIndicator(false);
        btnToggleFilter.Text = "Show All Students";

        // Disable Search Mode Layout
        SetSearchLayout(false);
        if (_txtSearch != null) _txtSearch.Text = string.Empty; // Clear search

        // Enable day-specific buttons
        ToggleDaySpecificActions(true);

        // Show day-specific columns
        SetDaySpecificColumnsVisible(true);

        // Re-enable the date picker
        dtpClassDate.Enabled = true;
        dtpClassDate.Visible = true;
    }

    private void SetSearchLayout(bool active)
    {
        if (_searchLayoutActive == active) return;
        if (_panelSearch == null) return;

        // Visual Toggle of Search Panel
        // Since it's inside the AutoSize ModeIndicator, the TableLayout will handle the resize
        _panelSearch.Visible = active;

        if (active) _txtSearch?.Focus();

        _searchLayoutActive = active;
    }

    private void SetDaySpecificColumnsVisible(bool visible)
    {
        // Null-safe column visibility toggling
        if (dgvStudents.Columns["AttendedClass"] is { } attendedCol)
            attendedCol.Visible = visible;

        if (dgvStudents.Columns["LearningMaterial"] is { } materialCol)
            materialCol.Visible = visible;
    }

    private void ToggleDaySpecificActions(bool enabled)
    {
        btnAssignMaterial.Enabled = enabled;
        btnUnassignMaterial.Enabled = enabled;
        btnViewMaterial.Enabled = enabled;
        btnEditFeedback.Enabled = enabled;
        btnGenerate.Enabled = enabled;

        btnAdd.Enabled = !enabled;
        btnUpdate.Enabled = !enabled;
        btnRemove.Enabled = !enabled;

        // Update tooltips only when buttons are enabled
        if (enabled)
        {
            toolTip.SetToolTip(btnAssignMaterial, "Assign learning material to selected student");
            toolTip.SetToolTip(btnUnassignMaterial, "Remove assigned material from selected student");
            toolTip.SetToolTip(btnViewMaterial, "View the assigned material file");
            toolTip.SetToolTip(btnEditFeedback, "Edit feedback notes for selected student");
            toolTip.SetToolTip(btnGenerate, "Generate feedback emails for students who attended");

            toolTip.SetToolTip(btnAdd, "Add a new student to the system");
            toolTip.SetToolTip(btnUpdate, "Edit the selected student's information");
            toolTip.SetToolTip(btnRemove, "Remove the selected student from the system");
        }
    }

    private void UpdateModeIndicator(bool showingAll)
    {
        if (showingAll)
        {
            panelModeIndicator.BackColor = Color.FromArgb(227, 242, 253); // Light Blue
            lblModeIcon.Text = "👥";
            lblModeTitle.Text = "Viewing ALL students (all days)";
            lblModeTitle.ForeColor = Color.FromArgb(21, 101, 192); // Dark Blue
            lblModeDescription.Text = "Day-specific actions are disabled";
            lblModeDescription.ForeColor = Color.FromArgb(21, 101, 192);
        }
        else
        {
            panelModeIndicator.BackColor = Color.FromArgb(232, 245, 233); // Light Green
            lblModeIcon.Text = "📅";
            string currentDay = _studentService.GetDayOfWeek(dtpClassDate.Value.Date).ToUpper();
            lblModeTitle.Text = $"Showing students for: {currentDay}";
            lblModeTitle.ForeColor = Color.FromArgb(46, 125, 50); // Dark Green
            lblModeDescription.Text = "Ready to manage today's class";
            lblModeDescription.ForeColor = Color.FromArgb(46, 125, 50);
        }
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
        if (!TryGetSelectedSession(out var session)) return;

        var filePath = ShowMaterialFileDialog(session.FullName);
        if (string.IsNullOrEmpty(filePath)) return;

        await UpdateSessionMaterialAsync(session, filePath, $"Assigned material to {session.FullName}");
    }

    private async void btnUnassignMaterial_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedSession(out var session)) return;

        if (string.IsNullOrWhiteSpace(session.AssignedMaterial))
        {
            ShowInfo($"{session.FullName} has no material assigned.", "Information");
            return;
        }

        if (!ShowConfirmation($"Unassign material from {session.FullName}?", "Confirm Unassignment"))
            return;

        await UpdateSessionMaterialAsync(session, null, $"Unassigned material from {session.FullName}");
    }

    private async void btnViewMaterial_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedSession(out var session)) return;

        if (string.IsNullOrWhiteSpace(session.AssignedMaterial))
        {
            ShowInfo($"{session.FullName} has no material assigned.", "No Material");
            return;
        }

        if (!File.Exists(session.AssignedMaterial))
        {
            ShowWarning($"Material file not found:\n{session.AssignedMaterial}", "File Not Found");
            return;
        }

        OpenFile(session.AssignedMaterial, $"Opened material for {session.FullName}");
    }

    private async Task UpdateSessionMaterialAsync(StudentSessionView session, string? materialPath, string successMessage)
    {
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            await _studentService.UpdateSessionMaterialAsync(session.SessionId, materialPath);
            session.AssignedMaterial = materialPath;
            RefreshStudentInGrid(session);
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
                Email    = form.StudentEmail,
                ClassDay = form.ClassDays
            };

            if (IsEmailChanged(selectedStudent, updatedStudent) && IsEmailDuplicate(updatedStudent.Email))
            {
                ShowWarning("Another student with this email already exists.", "Validation");
                return;
            }

            await _studentService.UpdateStudentAsync(selectedStudent, updatedStudent);

            selectedStudent.FullName  = updatedStudent.FullName;
            selectedStudent.Email     = updatedStudent.Email;
            selectedStudent.ClassDay  = updatedStudent.ClassDay;

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
            _allStudentsList.Remove(selectedStudent);
            UpdateStatus($"Removed {selectedStudent.FullName}");
        }, "Error removing student");
    }

    #endregion

    #region Feedback Management

    private void dgvStudents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (_showAllStudents)
            btnUpdate_Click(sender, e);
        else
            btnEditFeedback_Click(sender, e);
    }

    private async void btnEditFeedback_Click(object sender, EventArgs e)
    {
        if (!TryGetSelectedSession(out var session)) return;

        await ExecuteWithErrorHandlingAsync(async () =>
        {
            var student = new Student { Id = session.StudentId, FullName = session.FullName, Email = session.Email, ClassDay = session.ClassDay };
            string studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, student);
            await _noteService.OpenOrCreateNotesAsync(studentFolder, session.FullName);
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
        if (_sessions.Count == 0)
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

        foreach (var session in _sessions)
        {
            UpdateStatus($"Processing: {session.FullName}");
            Application.DoEvents();

            if (!ValidateStudentMaterial(session, result))
                continue;

            try
            {
                await ProcessStudentAsync(session);
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
        // Attendance toggling is only available in day mode
        if (_showAllStudents) return;

        if (e.RowIndex < 0 || dgvStudents.Columns[e.ColumnIndex].Name != "AttendedClass") return;

        dgvStudents.CommitEdit(DataGridViewDataErrorContexts.Commit);

        var session = _sessions[e.RowIndex];
        await ExecuteWithErrorHandlingAsync(async () =>
        {
            await _studentService.MarkAttendanceAsync(session.SessionId, session.Attended);
            UpdateStatus($"Marked {session.FullName}: " + (session.Attended ? "Present" : "Absent"));
        }, "Error updating attendance");
    }

    private bool ValidateStudentMaterial(StudentSessionView session, GenerationResult result)
    {
        // 1. Check Attendance
        if (!session.Attended)
        {
            UpdateStatus($"Skipping {session.FullName} (Absent)");
            return false; // Silently skip absent students
        }

        // 2. Validate Material (Optional)
        // If assigned but missing, we warn but DO NOT skip the student. They still get feedback.
        if (!string.IsNullOrEmpty(session.AssignedMaterial) && !File.Exists(session.AssignedMaterial))
        {
            result.SkippedStudents.Add($"{session.FullName} (Material File Last - Sending Feedback Only)");
            UpdateStatus($"Warning: Material missing for {session.FullName}. Sending feedback only.");
        }

        return true;
    }

    private async Task ProcessStudentAsync(StudentSessionView session)
    {
        // Build a temporary Student object for folder/file operations that still need it
        var student = new Student { Id = session.StudentId, FullName = session.FullName, Email = session.Email, ClassDay = session.ClassDay };
        string studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, student);
        string content = _fileService.GetStudentNoteContent(studentFolder) ??
                         "No specific notes found for this student.";
        string studentPdfPath = _pdfService.GenerateStudentPdf(session, content, studentFolder);

        _emailService.DraftEmail(session, studentPdfPath);
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

    #region Search Logic & Events

    private void TxtSearch_TextChanged(object? sender, EventArgs e)
    {
        if (_txtSearch == null || _btnClearSearch == null) return;

        string text = _txtSearch.Text;
        _btnClearSearch.Enabled = !string.IsNullOrWhiteSpace(text);

        // Real-time filtering
        if (_showAllStudents)
        {
            FilterStudentsBySearch(text);
        }
    }

    private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape && _txtSearch != null)
        {
            _txtSearch.Clear();
            e.Handled = true;
        }
    }

    private void BtnClearSearch_Click(object? sender, EventArgs e)
    {
        _txtSearch?.Clear();
        _txtSearch?.Focus();
    }

    private void FilterStudentsBySearch(string searchText)
    {
        _allStudentsList.Clear();

        var query = searchText.Trim();
        var isSearchActive = !string.IsNullOrWhiteSpace(query);

        var filtered = isSearchActive
            ? _allStudentsCache.Where(s => s.FullName.Contains(query, StringComparison.OrdinalIgnoreCase))
            : _allStudentsCache;

        foreach (var student in filtered)
            _allStudentsList.Add(student);

        if (_showAllStudents)
        {
            if (isSearchActive)
            {
                if (_allStudentsList.Count == 0)
                {
                    lblStatus.ForeColor = Color.OrangeRed;
                    UpdateStatus($"No students found matching '{query}'");
                }
                else
                {
                    lblStatus.ForeColor = SystemColors.ControlText;
                    UpdateStatus($"Found {_allStudentsList.Count} of {_allStudentsCache.Count} students");
                }
            }
            else
            {
                lblStatus.ForeColor = SystemColors.ControlText;
                UpdateStatus($"Viewing all students ({_allStudentsList.Count} total) - Day-specific columns hidden");
            }
        }
    }

    #endregion

    #region Helper Methods

    private bool TryGetSelectedSession(out StudentSessionView session)
    {
        session = dgvStudents.CurrentRow?.DataBoundItem as StudentSessionView;
        if (session == null && dgvStudents.SelectedRows.Count > 0)
            session = dgvStudents.SelectedRows[0].DataBoundItem as StudentSessionView;
        if (session != null) return true;
        ShowWarning("Please select a student first.", "Warning");
        return false;
    }

    // Used by CRUD (btnUpdate/btnRemove) — only active in Show-All mode where DataSource is _allStudentsList
    private bool TryGetSelectedStudent(out Student student)
    {
        student = dgvStudents.CurrentRow?.DataBoundItem as Student;
        if (student == null && dgvStudents.SelectedRows.Count > 0)
            student = dgvStudents.SelectedRows[0].DataBoundItem as Student;
        if (student != null) return true;
        ShowWarning("Please select a student first.", "Warning");
        return false;
    }

    private bool IsEmailDuplicate(string email) =>
        _allStudentsList.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    private static bool IsEmailChanged(Student original, Student updated) =>
        !original.Email.Equals(updated.Email, StringComparison.OrdinalIgnoreCase);

    private void RefreshStudentInGrid(StudentSessionView session)
    {
        var index = _sessions.IndexOf(session);
        if (index >= 0)
            _sessions.ResetItem(index);
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

}