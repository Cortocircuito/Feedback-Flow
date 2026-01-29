namespace Feedback_Flow;

sealed partial class MainDashboard
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDashboard));
        dgvStudents = new DataGridView();
        btnToggleFilter = new Button();
        btnAssignMaterial = new Button();
        btnUnassignMaterial = new Button();
        btnGenerate = new Button();
        statusStrip1 = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        btnAdd = new Button();
        btnUpdate = new Button();
        btnRemove = new Button();
        btnEditFeedback = new Button();
        btnViewMaterial = new Button();
        panelModeIndicator = new Panel();
        lblModeDescription = new Label();
        lblModeTitle = new Label();
        lblModeIcon = new Label();
        toolTip = new ToolTip(components);
        ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
        statusStrip1.SuspendLayout();
        panelModeIndicator.SuspendLayout();
        SuspendLayout();
        // 
        // dgvStudents
        // 
        dgvStudents.AllowUserToAddRows = false;
        dgvStudents.AllowUserToDeleteRows = false;
        dgvStudents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvStudents.Location = new Point(12, 130);
        dgvStudents.MultiSelect = false;
        dgvStudents.Name = "dgvStudents";
        dgvStudents.ReadOnly = true;
        dgvStudents.RowHeadersVisible = false;
        dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStudents.Size = new Size(634, 347);
        dgvStudents.TabIndex = 3;
        dgvStudents.CellDoubleClick += dgvStudents_CellDoubleClick;
        dgvStudents.CellFormatting += dgvStudents_CellFormatting;
        // 
        // btnToggleFilter
        // 
        btnToggleFilter.Location = new Point(516, 82);
        btnToggleFilter.Name = "btnToggleFilter";
        btnToggleFilter.Size = new Size(130, 42);
        btnToggleFilter.TabIndex = 11;
        btnToggleFilter.Text = "Show All Students";
        btnToggleFilter.UseVisualStyleBackColor = true;
        btnToggleFilter.Click += btnToggleFilter_Click;
        // 
        // btnAssignMaterial
        // 
        btnAssignMaterial.Location = new Point(12, 82);
        btnAssignMaterial.Name = "btnAssignMaterial";
        btnAssignMaterial.Size = new Size(120, 42);
        btnAssignMaterial.TabIndex = 0;
        btnAssignMaterial.Text = "Assign Material";
        btnAssignMaterial.UseVisualStyleBackColor = true;
        btnAssignMaterial.Click += btnAssignMaterial_Click;
        // 
        // btnUnassignMaterial
        // 
        btnUnassignMaterial.Location = new Point(138, 82);
        btnUnassignMaterial.Name = "btnUnassignMaterial";
        btnUnassignMaterial.Size = new Size(120, 42);
        btnUnassignMaterial.TabIndex = 1;
        btnUnassignMaterial.Text = "Unassign Material";
        btnUnassignMaterial.UseVisualStyleBackColor = true;
        btnUnassignMaterial.Click += btnUnassignMaterial_Click;
        // 
        // btnGenerate
        // 
        btnGenerate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        btnGenerate.Location = new Point(12, 483);
        btnGenerate.Name = "btnGenerate";
        btnGenerate.Size = new Size(760, 35);
        btnGenerate.TabIndex = 7;
        btnGenerate.Text = "Generate Feedback Emails";
        btnGenerate.UseVisualStyleBackColor = true;
        btnGenerate.Click += btnGenerate_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip1.Location = new Point(0, 527);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(784, 22);
        statusStrip1.TabIndex = 8;
        statusStrip1.Text = "statusStrip1";
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(39, 17);
        lblStatus.Text = "Ready";
        // 
        // btnAdd
        // 
        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.Location = new Point(652, 130);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(120, 40);
        btnAdd.TabIndex = 4;
        btnAdd.Text = "Add New";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;
        // 
        // btnUpdate
        // 
        btnUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnUpdate.Location = new Point(652, 176);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new Size(120, 40);
        btnUpdate.TabIndex = 5;
        btnUpdate.Text = "Edit Selected";
        btnUpdate.UseVisualStyleBackColor = true;
        btnUpdate.Click += btnUpdate_Click;
        // 
        // btnRemove
        // 
        btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRemove.Location = new Point(652, 222);
        btnRemove.Name = "btnRemove";
        btnRemove.Size = new Size(120, 40);
        btnRemove.TabIndex = 6;
        btnRemove.Text = "Remove";
        btnRemove.UseVisualStyleBackColor = true;
        btnRemove.Click += btnRemove_Click;
        // 
        // btnEditFeedback
        // 
        btnEditFeedback.Location = new Point(390, 82);
        btnEditFeedback.Name = "btnEditFeedback";
        btnEditFeedback.Size = new Size(120, 42);
        btnEditFeedback.TabIndex = 2;
        btnEditFeedback.Text = "Edit Feedback";
        btnEditFeedback.UseVisualStyleBackColor = true;
        btnEditFeedback.Click += btnEditFeedback_Click;
        // 
        // btnViewMaterial
        // 
        btnViewMaterial.Location = new Point(264, 82);
        btnViewMaterial.Name = "btnViewMaterial";
        btnViewMaterial.Size = new Size(120, 42);
        btnViewMaterial.TabIndex = 10;
        btnViewMaterial.Text = "View Material";
        btnViewMaterial.UseVisualStyleBackColor = true;
        btnViewMaterial.Click += btnViewMaterial_Click;
        // 
        // panelModeIndicator
        // 
        panelModeIndicator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panelModeIndicator.BackColor = Color.FromArgb(232, 245, 233);
        panelModeIndicator.BorderStyle = BorderStyle.FixedSingle;
        panelModeIndicator.Controls.Add(lblModeDescription);
        panelModeIndicator.Controls.Add(lblModeTitle);
        panelModeIndicator.Controls.Add(lblModeIcon);
        panelModeIndicator.Location = new Point(12, 12);
        panelModeIndicator.Name = "panelModeIndicator";
        panelModeIndicator.Size = new Size(760, 60);
        panelModeIndicator.TabIndex = 12;
        // 
        // lblModeDescription
        // 
        lblModeDescription.AutoSize = true;
        lblModeDescription.Font = new Font("Segoe UI", 9F);
        lblModeDescription.ForeColor = Color.FromArgb(46, 125, 50);
        lblModeDescription.Location = new Point(60, 35);
        lblModeDescription.Name = "lblModeDescription";
        lblModeDescription.Size = new Size(168, 15);
        lblModeDescription.TabIndex = 2;
        lblModeDescription.Text = "Ready to manage today's class";
        // 
        // lblModeTitle
        // 
        lblModeTitle.AutoSize = true;
        lblModeTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblModeTitle.ForeColor = Color.FromArgb(46, 125, 50);
        lblModeTitle.Location = new Point(60, 10);
        lblModeTitle.Name = "lblModeTitle";
        lblModeTitle.Size = new Size(244, 21);
        lblModeTitle.TabIndex = 1;
        lblModeTitle.Text = "Showing students for: MONDAY";
        // 
        // lblModeIcon
        // 
        lblModeIcon.AutoSize = true;
        lblModeIcon.Font = new Font("Segoe UI Emoji", 20F);
        lblModeIcon.Location = new Point(10, 10);
        lblModeIcon.Name = "lblModeIcon";
        lblModeIcon.Size = new Size(46, 36);
        lblModeIcon.TabIndex = 0;
        lblModeIcon.Text = "📅";
        // 
        // MainDashboard
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 550);
        Controls.Add(panelModeIndicator);
        Controls.Add(btnViewMaterial);
        Controls.Add(statusStrip1);
        Controls.Add(btnGenerate);
        Controls.Add(btnToggleFilter);
        Controls.Add(btnUnassignMaterial);
        Controls.Add(btnAssignMaterial);
        Controls.Add(btnEditFeedback);
        Controls.Add(btnRemove);
        Controls.Add(btnUpdate);
        Controls.Add(btnAdd);
        Controls.Add(dgvStudents);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "MainDashboard";
        Text = "Feedback Flow - Teacher Assistant";
        Load += MainDashboard_Load;
        ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        panelModeIndicator.ResumeLayout(false);
        panelModeIndicator.PerformLayout();
        ResumeLayout(false);
        PerformLayout();

    }

    #endregion

    #region Controls

    private System.Windows.Forms.DataGridView dgvStudents;
    private System.Windows.Forms.Button btnAssignMaterial;
    private System.Windows.Forms.Button btnUnassignMaterial;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnUpdate;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnEditFeedback;
    private System.Windows.Forms.Button btnGenerate;
    private System.Windows.Forms.Button btnToggleFilter;
    private System.Windows.Forms.Button btnViewMaterial;
    private System.Windows.Forms.Panel panelModeIndicator;
    private System.Windows.Forms.Label lblModeIcon;
    private System.Windows.Forms.Label lblModeTitle;
    private System.Windows.Forms.Label lblModeDescription;
    private System.Windows.Forms.ToolTip toolTip;

    #endregion
}