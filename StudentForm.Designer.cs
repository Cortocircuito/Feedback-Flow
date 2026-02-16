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
        mainLayout = new TableLayoutPanel();
        buttonPanel = new FlowLayoutPanel();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        mainLayout.SuspendLayout();
        buttonPanel.SuspendLayout();
        SuspendLayout();
        // 
        // mainLayout
        // 
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(lblFullName, 0, 0);
        mainLayout.Controls.Add(txtFullName, 0, 1);
        mainLayout.Controls.Add(lblEmail, 0, 2);
        mainLayout.Controls.Add(txtEmail, 0, 3);
        mainLayout.Controls.Add(lblClassDays, 0, 4);
        mainLayout.Controls.Add(checkedListBoxClassDays, 0, 5);
        mainLayout.Controls.Add(buttonPanel, 0, 6);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(10);
        mainLayout.RowCount = 7;
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.Size = new Size(350, 400);
        mainLayout.TabIndex = 0;
        // 
        // lblFullName
        // 
        lblFullName.AutoSize = true;
        lblFullName.Location = new Point(13, 10);
        lblFullName.Margin = new Padding(3, 0, 3, 5);
        lblFullName.Name = "lblFullName";
        lblFullName.Size = new Size(64, 15);
        lblFullName.TabIndex = 0;
        lblFullName.Text = "Full Name:";
        // 
        // txtFullName
        // 
        txtFullName.Dock = DockStyle.Fill;
        txtFullName.Location = new Point(13, 28);
        txtFullName.Margin = new Padding(3, 0, 3, 15);
        txtFullName.Name = "txtFullName";
        txtFullName.Size = new Size(324, 23);
        txtFullName.TabIndex = 1;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(13, 66);
        lblEmail.Margin = new Padding(3, 0, 3, 5);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(39, 15);
        lblEmail.TabIndex = 2;
        lblEmail.Text = "Email:";
        // 
        // txtEmail
        // 
        txtEmail.Dock = DockStyle.Fill;
        txtEmail.Location = new Point(13, 84);
        txtEmail.Margin = new Padding(3, 0, 3, 15);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(324, 23);
        txtEmail.TabIndex = 3;
        // 
        // lblClassDays
        // 
        lblClassDays.AutoSize = true;
        lblClassDays.Location = new Point(13, 122);
        lblClassDays.Margin = new Padding(3, 0, 3, 5);
        lblClassDays.Name = "lblClassDays";
        lblClassDays.Size = new Size(66, 15);
        lblClassDays.TabIndex = 4;
        lblClassDays.Text = "Class Days:";
        // 
        // checkedListBoxClassDays
        // 
        checkedListBoxClassDays.CheckOnClick = true;
        checkedListBoxClassDays.Dock = DockStyle.Fill;
        checkedListBoxClassDays.FormattingEnabled = true;
        checkedListBoxClassDays.Items.AddRange(new object[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
        checkedListBoxClassDays.Location = new Point(13, 140);
        checkedListBoxClassDays.Name = "checkedListBoxClassDays";
        checkedListBoxClassDays.Size = new Size(324, 196);
        checkedListBoxClassDays.TabIndex = 5;
        // 
        // buttonPanel
        // 
        buttonPanel.AutoSize = true;
        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Dock = DockStyle.Fill;
        buttonPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonPanel.Location = new Point(13, 342);
        buttonPanel.Margin = new Padding(0);
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Size = new Size(324, 48);
        buttonPanel.TabIndex = 6;
        // 
        // btnSave
        // 
        btnSave.DialogResult = DialogResult.OK;
        btnSave.Location = new Point(246, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 23);
        btnSave.TabIndex = 0;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(165, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 23);
        btnCancel.TabIndex = 1;
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
        ClientSize = new Size(350, 400);
        Controls.Add(mainLayout);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "StudentForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Student Details";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        buttonPanel.ResumeLayout(false);
        ResumeLayout(false);
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
    private System.Windows.Forms.TableLayoutPanel mainLayout;
    private System.Windows.Forms.FlowLayoutPanel buttonPanel;
}
