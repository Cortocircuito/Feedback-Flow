using Feedback_Flow.Models;
using Feedback_Flow.Services.Interfaces;

namespace Feedback_Flow;

public partial class Form1 : Form
{
    private readonly IFileSystemService _fileService;
    private readonly IDataService _dataService;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;

    private List<Student> _students = new List<Student>();
    private string _masterPdfPath = string.Empty;
    private string _dailyFolderPath = string.Empty;

    // Constructor for DI
    public Form1(IFileSystemService fileService,
        IDataService dataService,
        IPdfService pdfService,
        IEmailService emailService)
    {
        _fileService = fileService;
        _dataService = dataService;
        _pdfService = pdfService;
        _emailService = emailService;

        InitializeComponent();
    }

    // Default constructor for Designer support (if needed, though DI prefers the above)
    // WinForms designer sometimes complains if no parameterless constructor exists, 
    // but runtime needs the one with params.

    private void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            // 1. Initialize Folders
            _dailyFolderPath = _fileService.InitializeDailyFolder();
            lblStatus.Text = $"Ready. Folder: {_dailyFolderPath}";

            // 2. Load Students
            // Assuming alumnos.csv is in the AppDirectory or Root Folder?
            // Plan: "Locate alumnos.csv in the root [of Documents/Feedback-Flow? or App?]"
            // User said: "Locate alumnos.csv in the root." usually means App Root or Documents Root.
            // Let's check Documents/Feedback-Flow/alumnos.csv first, then App Root.

            string docRoot = Path.GetDirectoryName(_dailyFolderPath); // ..
            string csvPath = Path.Combine(docRoot, "alumnos.csv");

            if (!File.Exists(csvPath))
            {
                // Fallback to app directory
                csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alumnos.csv");
            }

            if (File.Exists(csvPath))
            {
                _students = _dataService.LoadStudents(csvPath).ToList();
                lstStudents.DataSource = _students;
                lstStudents.DisplayMember = "FullName";
                lblStatus.Text = $"Loaded {_students.Count} students.";
            }
            else
            {
                MessageBox.Show("alumnos.csv not found in Documents/Feedback-Flow or App Directory.", "Missing CSV",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Missing alumnos.csv";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Startup Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnSelectFile_Click(object sender, EventArgs e)
    {
        using (var openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            openFileDialog.Title = "Select Class Content PDF";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _masterPdfPath = openFileDialog.FileName;
                lblSelectedFile.Text = Path.GetFileName(_masterPdfPath);
            }
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
                // Update Status
                lblStatus.Text = $"Processing: {student.FullName}";
                Application.DoEvents(); // Keep UI responsive-ish without full async thread offloading complexity if strictly KISS, but async is better.
                // Since I used `async void`, I should ideally use Task.Run for heavy work, but iText is fast enough for small batches.

                // 1. Ensure/Find Student Folder
                // The folders should have been created manually or by app?
                // User said: "Inside [YYYYMMDD], create subfolders for each student listed in the CSV."
                // So we must create them now if they don't exist.
                string studentFolder = _fileService.CreateStudentFolder(_dailyFolderPath, student);

                // 2. Get Note Content
                string content = _fileService.GetStudentNoteContent(studentFolder);
                if (content == null)
                {
                    // User said: "Handle missing .txt notes".
                    // Logic: Maybe Create a default "No specific notes" PDF?
                    // Or Skip?
                    // Plan said: "Ensure the PDF conversion doesn't lock files."
                    // Let's assume we proceed with "generic" content or skip.
                    // "The app must find the .txt file... convert... into PDF"
                    // I'll proceed with empty content warning or default text.
                    content = "No specific notes found for this student.";
                }

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