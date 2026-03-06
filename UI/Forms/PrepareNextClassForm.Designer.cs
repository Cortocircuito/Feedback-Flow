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
        mainLayout = new TableLayoutPanel();
        materialLayout = new TableLayoutPanel();
        lblMaterialHeader = new Label();
        panelDescription = new Panel();
        lblDescriptionHeader = new Label();
        buttonPanel = new FlowLayoutPanel();
        mainLayout.SuspendLayout();
        materialLayout.SuspendLayout();
        panelDescription.SuspendLayout();
        buttonPanel.SuspendLayout();
        SuspendLayout();
        // 
        // lblStudentName
        // 
        lblStudentName.AutoSize = true;
        lblStudentName.Dock = DockStyle.Fill;
        lblStudentName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblStudentName.Location = new Point(12, 12);
        lblStudentName.Margin = new Padding(0, 0, 0, 4);
        lblStudentName.Name = "lblStudentName";
        lblStudentName.Size = new Size(496, 25);
        lblStudentName.TabIndex = 0;
        lblStudentName.Text = "Preparing next class for: ...";
        // 
        // lblNextDate
        // 
        lblNextDate.AutoSize = true;
        lblNextDate.Dock = DockStyle.Fill;
        lblNextDate.Font = new Font("Segoe UI", 9F);
        lblNextDate.Location = new Point(12, 41);
        lblNextDate.Margin = new Padding(0, 0, 0, 14);
        lblNextDate.Name = "lblNextDate";
        lblNextDate.Size = new Size(496, 20);
        lblNextDate.TabIndex = 1;
        lblNextDate.Text = "Next class date: ...";
        // 
        // lblMaterialPath
        // 
        lblMaterialPath.BorderStyle = BorderStyle.FixedSingle;
        lblMaterialPath.Dock = DockStyle.Fill;
        lblMaterialPath.Font = new Font("Segoe UI", 9F);
        lblMaterialPath.ForeColor = Color.Gray;
        lblMaterialPath.Location = new Point(0, 24);
        lblMaterialPath.Margin = new Padding(0, 0, 6, 0);
        lblMaterialPath.Name = "lblMaterialPath";
        lblMaterialPath.Padding = new Padding(4, 0, 4, 0);
        lblMaterialPath.Size = new Size(334, 32);
        lblMaterialPath.TabIndex = 1;
        lblMaterialPath.Text = "No material assigned";
        lblMaterialPath.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnBrowse
        // 
        btnBrowse.Dock = DockStyle.Fill;
        btnBrowse.Location = new Point(340, 24);
        btnBrowse.Margin = new Padding(0, 0, 6, 0);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(82, 32);
        btnBrowse.TabIndex = 0;
        btnBrowse.Text = "Browse...";
        btnBrowse.UseVisualStyleBackColor = true;
        btnBrowse.Click += btnBrowse_Click;
        // 
        // btnClearMaterial
        // 
        btnClearMaterial.Dock = DockStyle.Fill;
        btnClearMaterial.Enabled = false;
        btnClearMaterial.Location = new Point(428, 24);
        btnClearMaterial.Margin = new Padding(0);
        btnClearMaterial.Name = "btnClearMaterial";
        btnClearMaterial.Size = new Size(68, 32);
        btnClearMaterial.TabIndex = 1;
        btnClearMaterial.Text = "Clear";
        btnClearMaterial.UseVisualStyleBackColor = true;
        btnClearMaterial.Click += btnClearMaterial_Click;
        // 
        // txtDescription
        // 
        txtDescription.Dock = DockStyle.Fill;
        txtDescription.Font = new Font("Segoe UI", 9.5F);
        txtDescription.Location = new Point(0, 24);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.PlaceholderText = "Describe what you plan to do in this class...";
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.Size = new Size(490, 163);
        txtDescription.TabIndex = 2;
        // 
        // btnSave
        // 
        btnSave.DialogResult = DialogResult.OK;
        btnSave.Location = new Point(398, 11);
        btnSave.Margin = new Padding(3, 3, 8, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(90, 36);
        btnSave.TabIndex = 3;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(297, 11);
        btnCancel.Margin = new Padding(3, 3, 8, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 36);
        btnCancel.TabIndex = 4;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // mainLayout
        // 
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(lblStudentName, 0, 0);
        mainLayout.Controls.Add(lblNextDate, 0, 1);
        mainLayout.Controls.Add(materialLayout, 0, 2);
        mainLayout.Controls.Add(panelDescription, 0, 3);
        mainLayout.Controls.Add(buttonPanel, 0, 4);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(12);
        mainLayout.RowCount = 5;
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        mainLayout.Size = new Size(520, 400);
        mainLayout.TabIndex = 0;
        // 
        // materialLayout
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
        materialLayout.Location = new Point(12, 75);
        materialLayout.Margin = new Padding(0, 0, 0, 12);
        materialLayout.Name = "materialLayout";
        materialLayout.RowCount = 2;
        materialLayout.RowStyles.Add(new RowStyle());
        materialLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        materialLayout.Size = new Size(496, 56);
        materialLayout.TabIndex = 2;
        // 
        // lblMaterialHeader
        // 
        lblMaterialHeader.AutoSize = true;
        materialLayout.SetColumnSpan(lblMaterialHeader, 3);
        lblMaterialHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMaterialHeader.Location = new Point(0, 0);
        lblMaterialHeader.Margin = new Padding(0, 0, 0, 4);
        lblMaterialHeader.Name = "lblMaterialHeader";
        lblMaterialHeader.Size = new Size(132, 20);
        lblMaterialHeader.TabIndex = 0;
        lblMaterialHeader.Text = "Learning Material";
        // 
        // panelDescription
        // 
        panelDescription.Controls.Add(txtDescription);
        panelDescription.Controls.Add(lblDescriptionHeader);
        panelDescription.Dock = DockStyle.Fill;
        panelDescription.Location = new Point(15, 146);
        panelDescription.Name = "panelDescription";
        panelDescription.Size = new Size(490, 187);
        panelDescription.TabIndex = 3;
        // 
        // lblDescriptionHeader
        // 
        lblDescriptionHeader.Dock = DockStyle.Top;
        lblDescriptionHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDescriptionHeader.Location = new Point(0, 0);
        lblDescriptionHeader.Name = "lblDescriptionHeader";
        lblDescriptionHeader.Size = new Size(490, 24);
        lblDescriptionHeader.TabIndex = 3;
        lblDescriptionHeader.Text = "Class Description";
        // 
        // buttonPanel
        // 
        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Dock = DockStyle.Fill;
        buttonPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonPanel.Location = new Point(12, 336);
        buttonPanel.Margin = new Padding(0);
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Padding = new Padding(0, 8, 0, 0);
        buttonPanel.Size = new Size(496, 52);
        buttonPanel.TabIndex = 4;
        buttonPanel.WrapContents = false;
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
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(480, 360);
        Name = "PrepareNextClassForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Prepare Next Class";
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        materialLayout.ResumeLayout(false);
        materialLayout.PerformLayout();
        panelDescription.ResumeLayout(false);
        panelDescription.PerformLayout();
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
    private TableLayoutPanel mainLayout;
    private TableLayoutPanel materialLayout;
    private Label lblMaterialHeader;
    private Panel panelDescription;
    private Label lblDescriptionHeader;
    private FlowLayoutPanel buttonPanel;
}
