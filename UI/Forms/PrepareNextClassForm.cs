using Feedback_Flow.Models;

namespace Feedback_Flow.UI.Forms;

public partial class PrepareNextClassForm : Form
{
    public string? SelectedMaterial { get; private set; }
    public string? ClassDescription
    {
        get
        {
            var text = txtDescription.Text?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
    }

    public PrepareNextClassForm(string studentName, DateTime nextClassDate, ClassSession? existingSession)
    {
        InitializeComponent();

        lblStudentName.Text = $"Preparing next class for: {studentName}";
        lblNextDate.Text = $"Next class date: {nextClassDate:dddd, dd MMM yyyy}";

        if (existingSession != null)
        {
            SelectedMaterial = existingSession.AssignedMaterial;
            txtDescription.Text = existingSession.ClassDescription ?? string.Empty;
        }

        UpdateMaterialLabel();
    }

    protected override void OnDpiChanged(DpiChangedEventArgs e)
    {
        base.OnDpiChanged(e);
        PerformLayout();
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "All Supported Files|*.pdf;*.docx;*.doc;*.odt;*.pptx;*.ppt;*.odp|" +
                     "PDF Files (*.pdf)|*.pdf|" +
                     "Word Documents (*.docx;*.doc)|*.docx;*.doc|" +
                     "LibreOffice Writer (*.odt)|*.odt|" +
                     "PowerPoint (*.pptx;*.ppt)|*.pptx;*.ppt|" +
                     "LibreOffice Impress (*.odp)|*.odp",
            Title = "Select Learning Material"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            SelectedMaterial = dialog.FileName;
            UpdateMaterialLabel();
        }
    }

    private void btnClearMaterial_Click(object sender, EventArgs e)
    {
        SelectedMaterial = null;
        UpdateMaterialLabel();
    }

    private void UpdateMaterialLabel()
    {
        if (string.IsNullOrEmpty(SelectedMaterial))
        {
            lblMaterialPath.Text = "No material assigned";
            lblMaterialPath.ForeColor = Color.Gray;
            btnClearMaterial.Enabled = false;
        }
        else
        {
            lblMaterialPath.Text = Path.GetFileName(SelectedMaterial);
            lblMaterialPath.ForeColor = SystemColors.ControlText;
            btnClearMaterial.Enabled = true;
        }
    }

}
