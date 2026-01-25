using System.ComponentModel;
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

    private BindingList<Student> _students = new();
    private string _masterPdfPath = string.Empty;

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

        lstStudents.DataSource = _students;
        lstStudents.DisplayMember = "FullName";
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

    private void btnSelectFile_Click(object sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
        openFileDialog.Title = "Select Class Content PDF";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            _masterPdfPath = openFileDialog.FileName;
            lblSelectedFile.Text = Path.GetFileName(_masterPdfPath);
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
        if (lstStudents.SelectedItem is not Student selectedStudent) return;

        using var form = new StudentForm(selectedStudent);
        if (form.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var updatedStudent = new Student
                {
                    FullName = form.StudentFullName,
                    Email = form.StudentEmail
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

                _students.ResetBindings();
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
        if (lstStudents.SelectedItem is not Student selectedStudent) return;

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

    private void lstStudents_DoubleClick(object sender, EventArgs e)
    {
        btnEditFeedback_Click(sender, e);
    }

    private async void btnEditFeedback_Click(object sender, EventArgs e)
    {
        if (lstStudents.SelectedItem is not Student selectedStudent) return;

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
        if (string.IsNullOrEmpty(_masterPdfPath) || !File.Exists(_masterPdfPath))
        {
            MessageBox.Show("Please select a Class Content PDF first.", "Warning", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (_students.Count == 0)
        {
            MessageBox.Show("No students loaded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnGenerate.Enabled = false;
        lblStatus.Text = "Processing...";

        int successCount = 0;
        int errorCount = 0;

        foreach (var student in _students)
        {
            try
            {
                lblStatus.Text = $"Processing: {student.FullName}";
                Application.DoEvents(); // Keep UI responsive-ish without full async thread offloading complexity if strictly KISS, but async is better.
                // Since I used `async void`, I should ideally use Task.Run for heavy work, but iText is fast enough for small batches.

                // 1. Ensure/Find Student Folder
                // The folders should have been created manually or by app?
                // User said: "Inside [YYYYMMDD], create subfolders for each student listed in the CSV."
                // So we must create them now if they don't exist.
                string studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, student);

                // 2. Get Note Content
                string? content = _fileService.GetStudentNoteContent(studentFolder);
                content ??= "No specific notes found for this student.";

                // 3. Generate PDF
                string studentPdfPath = _pdfService.GenerateStudentPdf(student, content, studentFolder);

                // 4. Open Outlook
                _emailService.DraftEmail(student, _masterPdfPath, studentPdfPath);

                successCount++;
            }
            catch (Exception)
            {
                errorCount++;
            }
        }

        lblStatus.Text = $"Done. Success: {successCount}, Errors: {errorCount}";
        btnGenerate.Enabled = true;
        MessageBox.Show($"Process Completed.\nSuccess: {successCount}\nErrors: {errorCount}", "Finished",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}