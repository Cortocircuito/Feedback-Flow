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
        btnAssignMaterial = new Button();
        btnGenerate = new Button();
        statusStrip1 = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        btnAdd = new Button();
        btnUpdate = new Button();
        btnRemove = new Button();
        lblDayOfWeek = new Label();
        btnEditFeedback = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // dgvStudents
        // 
        dgvStudents.AllowUserToAddRows = false;
        dgvStudents.AllowUserToDeleteRows = false;
        dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvStudents.Location = new Point(12, 60);
        dgvStudents.MultiSelect = false;
        dgvStudents.Name = "dgvStudents";
        dgvStudents.ReadOnly = true;
        dgvStudents.RowHeadersVisible = false;
        dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStudents.Size = new Size(300, 255);
        dgvStudents.TabIndex = 2;
        dgvStudents.CellDoubleClick += dgvStudents_CellDoubleClick;
        dgvStudents.CellFormatting += dgvStudents_CellFormatting;
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
        // btnGenerate
        // 
        btnGenerate.Location = new Point(12, 321);
        btnGenerate.Name = "btnGenerate";
        btnGenerate.Size = new Size(426, 35);
        btnGenerate.TabIndex = 6;
        btnGenerate.Text = "Generate Feedback Emails";
        btnGenerate.UseVisualStyleBackColor = true;
        btnGenerate.Click += btnGenerate_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip1.Location = new Point(0, 365);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(450, 22);
        statusStrip1.TabIndex = 7;
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
        btnAdd.Location = new Point(318, 60);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(120, 40);
        btnAdd.TabIndex = 3;
        btnAdd.Text = "Add New";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;
        // 
        // btnUpdate
        // 
        btnUpdate.Location = new Point(318, 106);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new Size(120, 40);
        btnUpdate.TabIndex = 4;
        btnUpdate.Text = "Edit Selected";
        btnUpdate.UseVisualStyleBackColor = true;
        btnUpdate.Click += btnUpdate_Click;
        // 
        // btnRemove
        // 
        btnRemove.Location = new Point(318, 152);
        btnRemove.Name = "btnRemove";
        btnRemove.Size = new Size(120, 40);
        btnRemove.TabIndex = 5;
        btnRemove.Text = "Remove";
        btnRemove.UseVisualStyleBackColor = true;
        btnRemove.Click += btnRemove_Click;
        // 
        // lblDayOfWeek
        // 
        lblDayOfWeek.AutoSize = true;
        lblDayOfWeek.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDayOfWeek.ForeColor = SystemColors.HotTrack;
        lblDayOfWeek.Location = new Point(325, 35);
        lblDayOfWeek.Name = "lblDayOfWeek";
        lblDayOfWeek.Size = new Size(0, 15);
        lblDayOfWeek.TabIndex = 8;
        // 
        // btnEditFeedback
        // 
        btnEditFeedback.Location = new Point(138, 12);
        btnEditFeedback.Name = "btnEditFeedback";
        btnEditFeedback.Size = new Size(120, 42);
        btnEditFeedback.TabIndex = 1;
        btnEditFeedback.Text = "Edit Feedback";
        btnEditFeedback.UseVisualStyleBackColor = true;
        btnEditFeedback.Click += btnEditFeedback_Click;
        // 
        // MainDashboard
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(450, 387);
        Controls.Add(statusStrip1);
        Controls.Add(btnGenerate);
        Controls.Add(btnAssignMaterial);
        Controls.Add(btnEditFeedback);
        Controls.Add(btnRemove);
        Controls.Add(btnUpdate);
        Controls.Add(btnAdd);
        Controls.Add(dgvStudents);
        Controls.Add(lblDayOfWeek);
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
    private System.Windows.Forms.Button btnGenerate;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnUpdate;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Label lblDayOfWeek;
    private System.Windows.Forms.Button btnEditFeedback;

    #endregion
}