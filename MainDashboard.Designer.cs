namespace Feedback_Flow;

partial class MainDashboard
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
        lblDayOfWeek = new Label();
        btnViewMaterial = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // dgvStudents
        // 
        dgvStudents.AllowUserToAddRows = false;
        dgvStudents.AllowUserToDeleteRows = false;
        dgvStudents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvStudents.Location = new Point(12, 60);
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
        btnToggleFilter.Location = new Point(516, 12);
        btnToggleFilter.Name = "btnToggleFilter";
        btnToggleFilter.Size = new Size(130, 42);
        btnToggleFilter.TabIndex = 11;
        btnToggleFilter.Text = "Show All Students";
        btnToggleFilter.UseVisualStyleBackColor = true;
        btnToggleFilter.Click += btnToggleFilter_Click;
        // 
        // btnAssignMaterial
        // 
        btnAssignMaterial.Location = new Point(12, 12);
        btnAssignMaterial.Name = "btnAssignMaterial";
        btnAssignMaterial.Size = new Size(120, 42);
        btnAssignMaterial.TabIndex = 0;
        btnAssignMaterial.Text = "Assign Material";
        btnAssignMaterial.UseVisualStyleBackColor = true;
        btnAssignMaterial.Click += btnAssignMaterial_Click;
        // 
        // btnUnassignMaterial
        // 
        btnUnassignMaterial.Location = new Point(138, 12);
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
        btnGenerate.Location = new Point(12, 413);
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
        statusStrip1.Location = new Point(0, 457);
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
        btnAdd.Location = new Point(652, 60);
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
        btnUpdate.Location = new Point(652, 106);
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
        btnRemove.Location = new Point(652, 152);
        btnRemove.Name = "btnRemove";
        btnRemove.Size = new Size(120, 40);
        btnRemove.TabIndex = 6;
        btnRemove.Text = "Remove";
        btnRemove.UseVisualStyleBackColor = true;
        btnRemove.Click += btnRemove_Click;
        // 
        // btnEditFeedback
        // 
        btnEditFeedback.Location = new Point(390, 12);
        btnEditFeedback.Name = "btnEditFeedback";
        btnEditFeedback.Size = new Size(120, 42);
        btnEditFeedback.TabIndex = 2;
        btnEditFeedback.Text = "Edit Feedback";
        btnEditFeedback.UseVisualStyleBackColor = true;
        btnEditFeedback.Click += btnEditFeedback_Click;
        // 
        // lblDayOfWeek
        // 
        lblDayOfWeek.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDayOfWeek.AutoSize = true;
        lblDayOfWeek.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDayOfWeek.ForeColor = SystemColors.HotTrack;
        lblDayOfWeek.Location = new Point(652, 18);
        lblDayOfWeek.Name = "lblDayOfWeek";
        lblDayOfWeek.Size = new Size(86, 25);
        lblDayOfWeek.TabIndex = 9;
        lblDayOfWeek.Text = "Monday";
        // 
        // btnViewMaterial
        // 
        btnViewMaterial.Location = new Point(264, 12);
        btnViewMaterial.Name = "btnViewMaterial";
        btnViewMaterial.Size = new Size(120, 42);
        btnViewMaterial.TabIndex = 10;
        btnViewMaterial.Text = "View Material";
        btnViewMaterial.UseVisualStyleBackColor = true;
        btnViewMaterial.Click += btnViewMaterial_Click;
        // 
        // MainDashboard
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 479);
        Controls.Add(btnViewMaterial);
        Controls.Add(lblDayOfWeek);
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
    private System.Windows.Forms.Label lblDayOfWeek;
    private System.Windows.Forms.Button btnGenerate;
    private System.Windows.Forms.Button btnToggleFilter;
    private System.Windows.Forms.Button btnViewMaterial;

    #endregion
}