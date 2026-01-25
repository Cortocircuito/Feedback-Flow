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
        lstStudents = new ListBox();
        btnAssignMaterial = new Button();
        lblSelectedFile = new Label();
        btnGenerate = new Button();
        statusStrip1 = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        btnAdd = new Button();
        btnUpdate = new Button();
        btnRemove = new Button();
        lblDayOfWeek = new Label();
        btnEditFeedback = new Button();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // lstStudents
        // 
        lstStudents.FormattingEnabled = true;
        lstStudents.Location = new Point(12, 50);
        lstStudents.Name = "lstStudents";
        lstStudents.Size = new Size(300, 244);
        lstStudents.TabIndex = 0;
        lstStudents.DoubleClick += lstStudents_DoubleClick;
        // 
        // btnAssignMaterial
        // 
        btnAssignMaterial.Location = new Point(12, 12);
        btnAssignMaterial.Name = "btnAssignMaterial";
        btnAssignMaterial.Size = new Size(120, 23);
        btnAssignMaterial.TabIndex = 1;
        btnAssignMaterial.Text = "Assign Material";
        btnAssignMaterial.UseVisualStyleBackColor = true;
        btnAssignMaterial.Click += btnAssignMaterial_Click;
        // 
        // lblSelectedFile
        // 
        lblSelectedFile.AutoSize = true;
        lblSelectedFile.Location = new Point(140, 16);
        lblSelectedFile.Name = "lblSelectedFile";
        lblSelectedFile.Size = new Size(88, 15);
        lblSelectedFile.TabIndex = 2;
        lblSelectedFile.Text = "No file selected";
        // 
        // btnGenerate
        // 
        btnGenerate.Location = new Point(12, 305);
        btnGenerate.Name = "btnGenerate";
        btnGenerate.Size = new Size(436, 35);
        btnGenerate.TabIndex = 3;
        btnGenerate.Text = "Generate Feedback Emails";
        btnGenerate.UseVisualStyleBackColor = true;
        btnGenerate.Click += btnGenerate_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip1.Location = new Point(0, 350);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(458, 22);
        statusStrip1.TabIndex = 4;
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
        btnAdd.Location = new Point(328, 50);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(120, 40);
        btnAdd.TabIndex = 5;
        btnAdd.Text = "Add New";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;
        // 
        // btnUpdate
        // 
        btnUpdate.Location = new Point(328, 100);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new Size(120, 40);
        btnUpdate.TabIndex = 6;
        btnUpdate.Text = "Edit Selected";
        btnUpdate.UseVisualStyleBackColor = true;
        btnUpdate.Click += btnUpdate_Click;
        // 
        // btnRemove
        // 
        btnRemove.Location = new Point(328, 150);
        btnRemove.Name = "btnRemove";
        btnRemove.Size = new Size(120, 40);
        btnRemove.TabIndex = 7;
        btnRemove.Text = "Remove";
        btnRemove.UseVisualStyleBackColor = true;
        btnRemove.Click += btnRemove_Click;
        // 
        // lblDayOfWeek
        // 
        lblDayOfWeek.AutoSize = true;
        lblDayOfWeek.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDayOfWeek.ForeColor = SystemColors.HotTrack;
        lblDayOfWeek.Location = new Point(328, 250);
        lblDayOfWeek.Name = "lblDayOfWeek";
        lblDayOfWeek.Size = new Size(0, 15);
        lblDayOfWeek.TabIndex = 8;
        // 
        // btnEditFeedback
        // 
        btnEditFeedback.Location = new Point(328, 200);
        btnEditFeedback.Name = "btnEditFeedback";
        btnEditFeedback.Size = new Size(120, 40);
        btnEditFeedback.TabIndex = 9;
        btnEditFeedback.Text = "Edit Feedback";
        btnEditFeedback.UseVisualStyleBackColor = true;
        btnEditFeedback.Click += btnEditFeedback_Click;
        // 
        // MainDashboard
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(458, 372);
        Controls.Add(statusStrip1);
        Controls.Add(btnGenerate);
        Controls.Add(lblSelectedFile);
        Controls.Add(btnAssignMaterial);
        Controls.Add(btnEditFeedback);
        Controls.Add(btnRemove);
        Controls.Add(btnUpdate);
        Controls.Add(btnAdd);
        Controls.Add(lstStudents);
        Controls.Add(lblDayOfWeek);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "MainDashboard";
        Text = "Feedback Flow - Teacher Assistant";
        Load += MainDashboard_Load;
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();

    }

    #endregion

    #region Controls

    private System.Windows.Forms.ListBox lstStudents;
    private System.Windows.Forms.Button btnAssignMaterial;
    private System.Windows.Forms.Label lblSelectedFile;
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