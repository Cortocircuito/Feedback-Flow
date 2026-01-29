namespace Feedback_Flow;

partial class StudentForm
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
        components = new System.ComponentModel.Container();
        lblFullName = new Label();
        txtFullName = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        errorProvider = new ErrorProvider(components);
        lblClassDays = new Label();
        checkedListBoxClassDays = new CheckedListBox();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblFullName
        // 
        lblFullName.AutoSize = true;
        lblFullName.Location = new Point(12, 15);
        lblFullName.Name = "lblFullName";
        lblFullName.Size = new Size(64, 15);
        lblFullName.TabIndex = 0;
        lblFullName.Text = "Full Name:";
        // 
        // txtFullName
        // 
        txtFullName.Location = new Point(12, 33);
        txtFullName.Name = "txtFullName";
        txtFullName.Size = new Size(260, 23);
        txtFullName.TabIndex = 1;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(12, 69);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(39, 15);
        lblEmail.TabIndex = 2;
        lblEmail.Text = "Email:";
        // 
        // txtEmail
        // 
        txtEmail.Location = new Point(12, 87);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(260, 23);
        txtEmail.TabIndex = 3;
        // 
        // lblClassDays
        // 
        lblClassDays.AutoSize = true;
        lblClassDays.Location = new Point(12, 125);
        lblClassDays.Name = "lblClassDays";
        lblClassDays.Size = new Size(66, 15);
        lblClassDays.TabIndex = 4;
        lblClassDays.Text = "Class Days:";
        // 
        // checkedListBoxClassDays
        // 
        checkedListBoxClassDays.CheckOnClick = true;
        checkedListBoxClassDays.FormattingEnabled = true;
        checkedListBoxClassDays.Items.AddRange(new object[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
        checkedListBoxClassDays.Location = new Point(12, 143);
        checkedListBoxClassDays.Name = "checkedListBoxClassDays";
        checkedListBoxClassDays.Size = new Size(260, 130);
        checkedListBoxClassDays.TabIndex = 5;
        // 
        // btnSave
        // 
        btnSave.DialogResult = DialogResult.OK;
        btnSave.Location = new Point(116, 290);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 23);
        btnSave.TabIndex = 6;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(197, 290);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 23);
        btnCancel.TabIndex = 7;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // StudentForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(296, 330);
        Controls.Add(checkedListBoxClassDays);
        Controls.Add(lblClassDays);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(txtEmail);
        Controls.Add(lblEmail);
        Controls.Add(txtFullName);
        Controls.Add(lblFullName);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "StudentForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Student Details";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label lblFullName;
    private System.Windows.Forms.TextBox txtFullName;
    private System.Windows.Forms.Label lblEmail;
    private System.Windows.Forms.TextBox txtEmail;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.Label lblClassDays;
    private System.Windows.Forms.CheckedListBox checkedListBoxClassDays;
}
