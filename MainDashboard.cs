using System.ComponentModel;
using Feedback_Flow.Helpers;
using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow;

public partial class MainDashboard : Form
{
    private readonly IStudentService _studentService;
    private readonly INoteService _noteService;

    // We still need these for Generation
    private readonly IFileSystemService _fileService;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;

    private SortableBindingList<Student> _students;
    // Removed global masterPdfPath

    private string
        _dailyFolderPath = string.Empty; // Still needed for context in Generation? Or maybe PDF service handles it? 
    // Wait, Generation logic uses fileService.CreateStudentFolder... 
    // StudentService handles Creation on Add. 
    // Generation assumes folders exist or ensures them? 
    // Standard FileService CreateStudentFolder ensures existence. So we can keep using it for Generation.

    public MainDashboard(IStudentService studentService,
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

        // Initialize sortable binding list with alphabetical sorting by FullName
        _students = new SortableBindingList<Student>("FullName", ListSortDirection.Ascending);

        // Configure DataGridView columns
        dgvStudents.AutoGenerateColumns = false;

        var studentNameColumn = new DataGridViewTextBoxColumn
        {
            Name = "StudentName",
            HeaderText = "Student",
            DataPropertyName = "FullName",
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.Automatic
        };

        var learningMaterialColumn = new DataGridViewTextBoxColumn
        {
            Name = "LearningMaterial",
            HeaderText = "Learning Material",
            DataPropertyName = "LearningMaterialPath",
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.Automatic
        };

        dgvStudents.Columns.Add(studentNameColumn);
        dgvStudents.Columns.Add(learningMaterialColumn);

        // Bind data source
        dgvStudents.DataSource = _students;
    }

    private async void MainDashboard_Load(object sender, EventArgs e)
    {
        try
        {
            // Initialize view by loading from service
            var loaded = await _studentService.LoadStudentsAsync();
            foreach (var s in loaded) _students.Add(s);

            // Just for status display - we can ask fileService for today's folder path if needed
            // or just say Ready.
            _dailyFolderPath = _fileService.InitializeDailyFolder();
            lblStatus.Text = $"Ready. Loaded {_students.Count} students.";

            // Display current day of week in English
            lblDayOfWeek.Text = DateTime.Now.ToString("dddd", System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Startup Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Formats cells in the DataGridView for better display.
    /// Extracts file names from full paths and shows "Not assigned" for empty materials.
    /// </summary>
    private void dgvStudents_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        try
        {
            // Only format the Learning Material column
            if (dgvStudents.Columns[e.ColumnIndex].Name == "LearningMaterial")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Value = "Not assigned";
                    e.FormattingApplied = true;
                }
                else
                {
                    string fullPath = e.Value.ToString()!;

                    // Check if the file exists to avoid displaying invalid paths
                    if (File.Exists(fullPath))
                    {
                        e.Value = Path.GetFileName(fullPath);
                    }
                    else
                    {
                        e.Value = "File not found";
                    }

                    e.FormattingApplied = true;
                }
            }
        }
        catch (Exception ex)
        {
            // Log error to status bar instead of crashing
            lblStatus.Text = $"Formatting error: {ex.Message}";
            e.Value = "Error";
            e.FormattingApplied = true;
        }
    }

    private async void btnAssignMaterial_Click(object sender, EventArgs e)
    {
        // Get selected student from DataGridView
        if (dgvStudents.CurrentRow?.DataBoundItem is not Student selectedStudent)
        {
            MessageBox.Show("Please select a student first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "All Supported Files|*.pdf;*.docx;*.doc;*.odt;*.pptx;*.ppt;*.odp|" +
                                "PDF Files (*.pdf)|*.pdf|" +
                                "Word Documents (*.docx;*.doc)|*.docx;*.doc|" +
                                "LibreOffice Writer (*.odt)|*.odt|" +
                                "PowerPoint (*.pptx;*.ppt)|*.pptx;*.ppt|" +
                                "LibreOffice Impress (*.odp)|*.odp";
        openFileDialog.Title = $"Select Material for {selectedStudent.FullName}";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            selectedStudent.LearningMaterialPath = openFileDialog.FileName;

            // Persist change
            try
            {
                await _studentService.UpdateStudentAsync(selectedStudent, selectedStudent);

                // Trigger DataGridView refresh to show updated material
                var index = _students.IndexOf(selectedStudent);
                if (index >= 0)
                {
                    _students.ResetItem(index);
                }

                lblStatus.Text = $"Assigned material to {selectedStudent.FullName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error assigning material: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Unassigns the learning material from the selected student.
    /// Clears the LearningMaterialPath and updates the CSV file.
    /// </summary>
    private async void btnUnassignMaterial_Click(object sender, EventArgs e)
    {
        // Get selected student from DataGridView
        if (dgvStudents.CurrentRow?.DataBoundItem is not Student selectedStudent)
        {
            MessageBox.Show("Please select a student first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Check if student has material assigned
        if (string.IsNullOrWhiteSpace(selectedStudent.LearningMaterialPath))
        {
            MessageBox.Show($"{selectedStudent.FullName} has no material assigned.", "Information", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Confirm unassignment
        var result = MessageBox.Show(
            $"Unassign material from {selectedStudent.FullName}?",
            "Confirm Unassignment",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                // Clear the material path
                selectedStudent.LearningMaterialPath = string.Empty;

                // Persist change to CSV
                await _studentService.UpdateStudentAsync(selectedStudent, selectedStudent);

                // Trigger DataGridView refresh to show "Not assigned"
                var index = _students.IndexOf(selectedStudent);
                if (index >= 0)
                {
                    _students.ResetItem(index);
                }

                lblStatus.Text = $"Unassigned material from {selectedStudent.FullName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error unassigning material: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using var form = new StudentForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var newStudent = new Student
                {
                    FullName = form.StudentFullName,
                    Email = form.StudentEmail
                };

                // Check duplicate email in UI list first for quick feedback? 
                // Service could also handle it, but UI check prevents service call.
                if (_students.Any(s => s.Email.Equals(newStudent.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Student with this email already exists.", "Validation", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                await _studentService.AddStudentAsync(newStudent);
                _students.Add(newStudent);
                lblStatus.Text = $"Added {newStudent.FullName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding student: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    private async void btnUpdate_Click(object sender, EventArgs e)
    {
        if (dgvStudents.CurrentRow?.DataBoundItem is not Student selectedStudent) return;

        using var form = new StudentForm(selectedStudent);
        if (form.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var updatedStudent = new Student
                {
                    FullName = form.StudentFullName,
                    Email = form.StudentEmail,
                    // Preserve existing material path
                    LearningMaterialPath = selectedStudent.LearningMaterialPath
                };

                // Check duplicate if email changed
                if (!selectedStudent.Email.Equals(updatedStudent.Email, StringComparison.OrdinalIgnoreCase) &&
                    _students.Any(s => s.Email.Equals(updatedStudent.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Another student with this email already exists.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await _studentService.UpdateStudentAsync(selectedStudent, updatedStudent);

                // Update UI object directly to reflect changes in BindingList
                // (Or reload all, but updating object is efficient)
                // Note: UpdateStudentAsync uses original property to find, so we must update AFTER service call success
                selectedStudent.FullName = updatedStudent.FullName;
                selectedStudent.Email = updatedStudent.Email;

                // Trigger re-sort since name may have changed
                _students.Sort();
                lblStatus.Text = $"Updated {selectedStudent.FullName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating student: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    private async void btnRemove_Click(object sender, EventArgs e)
    {
        if (dgvStudents.CurrentRow?.DataBoundItem is not Student selectedStudent) return;

        if (MessageBox.Show($"Remove {selectedStudent.FullName}?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                await _studentService.DeleteStudentAsync(selectedStudent);
                _students.Remove(selectedStudent);
                lblStatus.Text = $"Removed {selectedStudent.FullName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing student: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Handles double-click on DataGridView rows to open feedback editor.
    /// </summary>
    private void dgvStudents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        // Ignore header row clicks
        if (e.RowIndex >= 0)
        {
            btnEditFeedback_Click(sender, e);
        }
    }

    private async void btnEditFeedback_Click(object sender, EventArgs e)
    {
        if (dgvStudents.CurrentRow?.DataBoundItem is not Student selectedStudent) return;

        try
        {
            // Resolve Path logic -> Documented requirement: Documents/Feedback-Flow/[YYYYMMDD]/[Student-Name]/
            // We can get the daily folder from _fileService (cached or re-init)
            // But _dailyFolderPath is already cached in field.

            // IMPORTANT: If user just started app, selected student, click edit... 
            // folder might not exist if they never clicked "Add" or "Generate". 
            // BUT StudentService creates folder on Add.
            // Requirement says: "Search for existing... If no .txt... create one"

            // We need the student folder path. _fileService.CreateStudentFolder is idempotent and returns the path.
            // Using CreateStudentFolder ensures we have the path even if we don't strictly "create" it redundantly.
            string studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, selectedStudent);

            await _noteService.OpenOrCreateNotesAsync(studentFolder, selectedStudent.FullName);
        }
        catch (DirectoryNotFoundException)
        {
            MessageBox.Show(
                "The student's folder has not been created properly. Please ensure the daily workspace is initialized.",
                "Folder Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening notes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnGenerate_Click(object sender, EventArgs e)
    {
        // Removed global PDF check logic
        // logic moved to per-student loop

        if (_students.Count == 0)
        {
            MessageBox.Show("No students loaded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnGenerate.Enabled = false;
        lblStatus.Text = "Processing...";

        var skippedStudents = new List<string>();
        int successCount = 0; // <-- Declare and initialize successCount
        int errorCount = 0; // <-- Declare and initialize errorCount

        foreach (var student in _students)
        {
            try
            {
                lblStatus.Text = $"Processing: {student.FullName}";
                Application.DoEvents();

                // Guard: Check if material assigned
                if (string.IsNullOrEmpty(student.LearningMaterialPath) || !File.Exists(student.LearningMaterialPath))
                {
                    skippedStudents.Add(student.FullName);
                    // Update status briefly
                    lblStatus.Text = $"Skipping {student.FullName} (No Material)";
                    Application.DoEvents();
                    continue;
                }

                // 1. Ensure/Find Student Folder
                string studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, student);

                // 2. Get Note Content
                string? content = _fileService.GetStudentNoteContent(studentFolder);
                content ??= "No specific notes found for this student.";

                // 3. Generate PDF
                string studentPdfPath = _pdfService.GenerateStudentPdf(student, content, studentFolder);

                // 4. Open Outlook
                _emailService.DraftEmail(student, studentPdfPath);

                successCount++;
            }
            catch (Exception)
            {
                errorCount++;
            }
        }

        lblStatus.Text = $"Done. Success: {successCount}, Skipped: {skippedStudents.Count}, Errors: {errorCount}";
        btnGenerate.Enabled = true;

        string msg = $"Process Completed.\nSuccess: {successCount}\nErrors: {errorCount}";
        if (skippedStudents.Any())
        {
            msg += $"\n\nSkipped ({skippedStudents.Count}) - No Material:\n- " + string.Join("\n- ", skippedStudents);
        }

        MessageBox.Show(msg, "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}