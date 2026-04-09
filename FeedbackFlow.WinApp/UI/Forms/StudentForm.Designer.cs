namespace Feedback_Flow.UI.Forms;

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
        // lblFullName
        // 
        lblFullName.AutoSize = true;
        lblFullName.Location = new Point(14, 13);
        lblFullName.Margin = new Padding(3, 0, 3, 7);
        lblFullName.Name = "lblFullName";
        lblFullName.Size = new Size(79, 20);
        lblFullName.TabIndex = 0;
        lblFullName.Text = "Full Name:";
        // 
        // txtFullName
        // 
        txtFullName.Dock = DockStyle.Fill;
        txtFullName.Location = new Point(14, 40);
        txtFullName.Margin = new Padding(3, 0, 3, 20);
        txtFullName.Name = "txtFullName";
        txtFullName.Size = new Size(372, 27);
        txtFullName.TabIndex = 1;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(14, 87);
        lblEmail.Margin = new Padding(3, 0, 3, 7);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(49, 20);
        lblEmail.TabIndex = 2;
        lblEmail.Text = "Email:";
        // 
        // txtEmail
        // 
        txtEmail.Dock = DockStyle.Fill;
        txtEmail.Location = new Point(14, 114);
        txtEmail.Margin = new Padding(3, 0, 3, 20);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(372, 27);
        txtEmail.TabIndex = 3;
        // 
        // btnSave
        // 
        btnSave.DialogResult = DialogResult.OK;
        btnSave.Location = new Point(289, 4);
        btnSave.Margin = new Padding(3, 4, 3, 4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(86, 31);
        btnSave.TabIndex = 1;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(197, 4);
        btnCancel.Margin = new Padding(3, 4, 3, 4);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(86, 31);
        btnCancel.TabIndex = 0;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // lblClassDays
        // 
        lblClassDays.AutoSize = true;
        lblClassDays.Location = new Point(14, 161);
        lblClassDays.Margin = new Padding(3, 0, 3, 7);
        lblClassDays.Name = "lblClassDays";
        lblClassDays.Size = new Size(81, 20);
        lblClassDays.TabIndex = 4;
        lblClassDays.Text = "Class Days:";
        // 
        // checkedListBoxClassDays
        // 
        checkedListBoxClassDays.CheckOnClick = true;
        checkedListBoxClassDays.Dock = DockStyle.Fill;
        checkedListBoxClassDays.FormattingEnabled = true;
        checkedListBoxClassDays.Items.AddRange(new object[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
        checkedListBoxClassDays.Location = new Point(14, 192);
        checkedListBoxClassDays.Margin = new Padding(3, 4, 3, 4);
        checkedListBoxClassDays.Name = "checkedListBoxClassDays";
        checkedListBoxClassDays.Size = new Size(372, 285);
        checkedListBoxClassDays.TabIndex = 5;
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
        mainLayout.Margin = new Padding(3, 4, 3, 4);
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(11, 13, 11, 13);
        mainLayout.RowCount = 7;
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.Size = new Size(400, 533);
        mainLayout.TabIndex = 0;
        // 
        // buttonPanel
        // 
        buttonPanel.AutoSize = true;
        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Dock = DockStyle.Fill;
        buttonPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonPanel.Location = new Point(11, 481);
        buttonPanel.Margin = new Padding(0);
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Size = new Size(378, 39);
        buttonPanel.TabIndex = 6;
        // 
        // StudentForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(400, 533);
        Controls.Add(mainLayout);
        Margin = new Padding(3, 4, 3, 4);
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
