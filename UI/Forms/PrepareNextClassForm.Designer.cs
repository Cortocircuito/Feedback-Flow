namespace Feedback_Flow.UI.Forms;

partial class PrepareNextClassForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblStudentName = new Label();
        lblNextDate = new Label();
        lblMaterialPath = new Label();
        btnBrowse = new Button();
        btnClearMaterial = new Button();
        txtDescription = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();

        var mainLayout = new TableLayoutPanel();
        var materialLayout = new TableLayoutPanel();
        var lblMaterialHeader = new Label();
        var panelDescription = new Panel();
        var lblDescriptionHeader = new Label();
        var buttonPanel = new FlowLayoutPanel();

        mainLayout.SuspendLayout();
        materialLayout.SuspendLayout();
        panelDescription.SuspendLayout();
        buttonPanel.SuspendLayout();
        SuspendLayout();

        //
        // mainLayout — 6 rows: student name | next date | material | description | buttons
        //
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(lblStudentName, 0, 0);
        mainLayout.Controls.Add(lblNextDate, 0, 1);
        mainLayout.Controls.Add(materialLayout, 0, 2);
        mainLayout.Controls.Add(panelDescription, 0, 3);
        mainLayout.Controls.Add(buttonPanel, 0, 4);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(12);
        mainLayout.RowCount = 5;
        mainLayout.RowStyles.Add(new RowStyle());                           // lblStudentName
        mainLayout.RowStyles.Add(new RowStyle());                           // lblNextDate
        mainLayout.RowStyles.Add(new RowStyle());                           // materialLayout
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));     // description
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));     // buttons
        mainLayout.TabIndex = 0;

        //
        // lblStudentName
        //
        lblStudentName.AutoSize = true;
        lblStudentName.Dock = DockStyle.Fill;
        lblStudentName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblStudentName.Margin = new Padding(0, 0, 0, 4);
        lblStudentName.Name = "lblStudentName";
        lblStudentName.Text = "Preparing next class for: ...";

        //
        // lblNextDate
        //
        lblNextDate.AutoSize = true;
        lblNextDate.Dock = DockStyle.Fill;
        lblNextDate.Font = new Font("Segoe UI", 9F);
        lblNextDate.ForeColor = Color.FromArgb(80, 80, 80);
        lblNextDate.Margin = new Padding(0, 0, 0, 14);
        lblNextDate.Name = "lblNextDate";
        lblNextDate.Text = "Next class date: ...";

        //
        // materialLayout — 2 rows x 3 cols: [header (span 3)] / [path | browse | clear]
        //
        materialLayout.AutoSize = true;
        materialLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        materialLayout.ColumnCount = 3;
        materialLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        materialLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88F));
        materialLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 68F));
        materialLayout.Controls.Add(lblMaterialHeader, 0, 0);
        materialLayout.Controls.Add(lblMaterialPath, 0, 1);
        materialLayout.Controls.Add(btnBrowse, 1, 1);
        materialLayout.Controls.Add(btnClearMaterial, 2, 1);
        materialLayout.Dock = DockStyle.Fill;
        materialLayout.Margin = new Padding(0, 0, 0, 12);
        materialLayout.Name = "materialLayout";
        materialLayout.RowCount = 2;
        materialLayout.RowStyles.Add(new RowStyle());                       // header label
        materialLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F)); // path + buttons

        //
        // lblMaterialHeader
        //
        lblMaterialHeader.AutoSize = true;
        lblMaterialHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMaterialHeader.Margin = new Padding(0, 0, 0, 4);
        lblMaterialHeader.Name = "lblMaterialHeader";
        lblMaterialHeader.Text = "Learning Material";
        materialLayout.SetColumnSpan(lblMaterialHeader, 3);

        //
        // lblMaterialPath
        //
        lblMaterialPath.BorderStyle = BorderStyle.FixedSingle;
        lblMaterialPath.Dock = DockStyle.Fill;
        lblMaterialPath.Font = new Font("Segoe UI", 9F);
        lblMaterialPath.ForeColor = Color.Gray;
        lblMaterialPath.Margin = new Padding(0, 0, 6, 0);
        lblMaterialPath.Name = "lblMaterialPath";
        lblMaterialPath.Padding = new Padding(4, 0, 4, 0);
        lblMaterialPath.Text = "No material assigned";
        lblMaterialPath.TextAlign = ContentAlignment.MiddleLeft;

        //
        // btnBrowse
        //
        btnBrowse.Dock = DockStyle.Fill;
        btnBrowse.Margin = new Padding(0, 0, 6, 0);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.TabIndex = 0;
        btnBrowse.Text = "Browse...";
        btnBrowse.UseVisualStyleBackColor = true;
        btnBrowse.Click += btnBrowse_Click;

        //
        // btnClearMaterial
        //
        btnClearMaterial.Dock = DockStyle.Fill;
        btnClearMaterial.Enabled = false;
        btnClearMaterial.Margin = new Padding(0);
        btnClearMaterial.Name = "btnClearMaterial";
        btnClearMaterial.TabIndex = 1;
        btnClearMaterial.Text = "Clear";
        btnClearMaterial.UseVisualStyleBackColor = true;
        btnClearMaterial.Click += btnClearMaterial_Click;

        //
        // panelDescription — label docked Top, textbox fills remaining
        //
        panelDescription.Controls.Add(txtDescription);
        panelDescription.Controls.Add(lblDescriptionHeader);
        panelDescription.Dock = DockStyle.Fill;
        panelDescription.Name = "panelDescription";

        //
        // lblDescriptionHeader — must be added AFTER txtDescription so it docks on top
        //
        lblDescriptionHeader.AutoSize = false;
        lblDescriptionHeader.Dock = DockStyle.Top;
        lblDescriptionHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDescriptionHeader.Height = 24;
        lblDescriptionHeader.Name = "lblDescriptionHeader";
        lblDescriptionHeader.Text = "Class Description";

        //
        // txtDescription
        //
        txtDescription.Dock = DockStyle.Fill;
        txtDescription.Font = new Font("Segoe UI", 9.5F);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.PlaceholderText = "Describe what you plan to do in this class...";
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.TabIndex = 2;

        //
        // buttonPanel — FlowLayoutPanel, RightToLeft: [Cancel] [Save]
        // AutoSize omitted: Dock=Fill already fills the fixed 52px row.
        //
        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Dock = DockStyle.Fill;
        buttonPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonPanel.Margin = new Padding(0);
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Padding = new Padding(0, 8, 0, 0);
        buttonPanel.WrapContents = false;

        //
        // btnSave — 8px from right edge of buttonPanel
        //
        btnSave.DialogResult = DialogResult.OK;
        btnSave.Margin = new Padding(3, 3, 8, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(90, 36);
        btnSave.TabIndex = 3;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;

        //
        // btnCancel — same vertical margins as Save; 8px gap to its right (Save)
        //
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Margin = new Padding(3, 3, 8, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 36);
        btnCancel.TabIndex = 4;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;

        //
        // PrepareNextClassForm
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(520, 400);
        Controls.Add(mainLayout);
        FormBorderStyle = FormBorderStyle.Sizable;
        MinimumSize = new Size(480, 360);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PrepareNextClassForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Prepare Next Class";

        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        materialLayout.ResumeLayout(false);
        materialLayout.PerformLayout();
        panelDescription.ResumeLayout(false);
        buttonPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Label lblStudentName;
    private Label lblNextDate;
    private Label lblMaterialPath;
    private Button btnBrowse;
    private Button btnClearMaterial;
    private TextBox txtDescription;
    private Button btnSave;
    private Button btnCancel;
}
