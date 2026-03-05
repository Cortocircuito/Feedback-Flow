namespace Feedback_Flow.UI.Forms;

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
        btnAssignMaterial = new Button();
        btnUnassignMaterial = new Button();
        btnGenerateEmail = new Button();
        statusStrip1 = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        btnAdd = new Button();
        btnUpdate = new Button();
        btnRemove = new Button();
        btnEditFeedback = new Button();
        btnViewMaterial = new Button();
        flowLayoutPanelActions = new FlowLayoutPanel();
        panelModeIndicator = new Panel();
        lblModeDescription = new Label();
        lblModeTitle = new Label();
        lblModeIcon = new Label();
        toolTip = new ToolTip(components);
        rootLayout = new TableLayoutPanel();
        actionsPanel = new TableLayoutPanel();
        flowLayoutPanel1 = new FlowLayoutPanel();
        btnToggleFilter = new Button();
        dtpClassDate = new DateTimePicker();
        mainContentPanel = new TableLayoutPanel();
        sideButtonPanel = new FlowLayoutPanel();
        flowLayoutPanel2 = new FlowLayoutPanel();
        btnPrepareNextClass = new Button();
        panelClassDescription = new Panel();
        lblDescriptionTitle = new Label();
        txtDescriptionView = new TextBox();
        ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
        statusStrip1.SuspendLayout();
        flowLayoutPanelActions.SuspendLayout();
        panelModeIndicator.SuspendLayout();
        rootLayout.SuspendLayout();
        actionsPanel.SuspendLayout();
        flowLayoutPanel1.SuspendLayout();
        mainContentPanel.SuspendLayout();
        sideButtonPanel.SuspendLayout();
        flowLayoutPanel2.SuspendLayout();
        panelClassDescription.SuspendLayout();
        SuspendLayout();
        // 
        // dgvStudents
        // 
        dgvStudents.AllowUserToAddRows = false;
        dgvStudents.AllowUserToDeleteRows = false;
        dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStudents.BackgroundColor = Color.White;
        dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvStudents.Dock = DockStyle.Fill;
        dgvStudents.Location = new Point(3, 4);
        dgvStudents.Margin = new Padding(3, 4, 3, 4);
        dgvStudents.MultiSelect = false;
        dgvStudents.Name = "dgvStudents";
        dgvStudents.ReadOnly = true;
        dgvStudents.RowHeadersVisible = false;
        dgvStudents.RowHeadersWidth = 51;
        dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStudents.Size = new Size(627, 335);
        dgvStudents.TabIndex = 0;
        dgvStudents.CellDoubleClick += dgvStudents_CellDoubleClick;
        dgvStudents.CellFormatting += dgvStudents_CellFormatting;
        // 
        // btnAssignMaterial
        // 
        btnAssignMaterial.Location = new Point(3, 4);
        btnAssignMaterial.Margin = new Padding(3, 4, 3, 4);
        btnAssignMaterial.Name = "btnAssignMaterial";
        btnAssignMaterial.Size = new Size(137, 40);
        btnAssignMaterial.TabIndex = 0;
        btnAssignMaterial.Text = "Assign Material";
        btnAssignMaterial.UseVisualStyleBackColor = true;
        btnAssignMaterial.Click += btnAssignMaterial_Click;
        // 
        // btnUnassignMaterial
        // 
        btnUnassignMaterial.Location = new Point(146, 4);
        btnUnassignMaterial.Margin = new Padding(3, 4, 3, 4);
        btnUnassignMaterial.Name = "btnUnassignMaterial";
        btnUnassignMaterial.Size = new Size(137, 40);
        btnUnassignMaterial.TabIndex = 1;
        btnUnassignMaterial.Text = "Unassign Material";
        btnUnassignMaterial.UseVisualStyleBackColor = true;
        btnUnassignMaterial.Click += btnUnassignMaterial_Click;
        // 
        // btnGenerateEmail
        // 
        btnGenerateEmail.Location = new Point(146, 4);
        btnGenerateEmail.Margin = new Padding(3, 4, 3, 4);
        btnGenerateEmail.Name = "btnGenerateEmail";
        btnGenerateEmail.Size = new Size(137, 40);
        btnGenerateEmail.TabIndex = 3;
        btnGenerateEmail.Text = "Generate Email";
        btnGenerateEmail.UseVisualStyleBackColor = true;
        btnGenerateEmail.Click += btnGenerate_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.ImageScalingSize = new Size(20, 20);
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip1.Location = new Point(0, 594);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Padding = new Padding(1, 0, 16, 0);
        statusStrip1.Size = new Size(801, 26);
        statusStrip1.TabIndex = 8;
        statusStrip1.Text = "statusStrip1";
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(50, 20);
        lblStatus.Text = "Ready";
        // 
        // btnAdd
        // 
        btnAdd.Enabled = false;
        btnAdd.Location = new Point(3, 0);
        btnAdd.Margin = new Padding(3, 0, 0, 8);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(137, 40);
        btnAdd.TabIndex = 0;
        btnAdd.Text = "Add New";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;
        // 
        // btnUpdate
        // 
        btnUpdate.Enabled = false;
        btnUpdate.Location = new Point(3, 48);
        btnUpdate.Margin = new Padding(3, 0, 0, 8);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new Size(137, 40);
        btnUpdate.TabIndex = 1;
        btnUpdate.Text = "Edit Selected";
        btnUpdate.UseVisualStyleBackColor = true;
        btnUpdate.Click += btnUpdate_Click;
        // 
        // btnRemove
        // 
        btnRemove.Enabled = false;
        btnRemove.Location = new Point(3, 96);
        btnRemove.Margin = new Padding(3, 0, 0, 0);
        btnRemove.Name = "btnRemove";
        btnRemove.Size = new Size(137, 40);
        btnRemove.TabIndex = 2;
        btnRemove.Text = "Remove";
        btnRemove.UseVisualStyleBackColor = true;
        btnRemove.Click += btnRemove_Click;
        // 
        // btnEditFeedback
        // 
        btnEditFeedback.Location = new Point(3, 4);
        btnEditFeedback.Margin = new Padding(3, 4, 3, 4);
        btnEditFeedback.Name = "btnEditFeedback";
        btnEditFeedback.Size = new Size(137, 40);
        btnEditFeedback.TabIndex = 3;
        btnEditFeedback.Text = "Edit Feedback";
        btnEditFeedback.UseVisualStyleBackColor = true;
        btnEditFeedback.Click += btnEditFeedback_Click;
        // 
        // btnViewMaterial
        // 
        btnViewMaterial.Location = new Point(289, 4);
        btnViewMaterial.Margin = new Padding(3, 4, 3, 4);
        btnViewMaterial.Name = "btnViewMaterial";
        btnViewMaterial.Size = new Size(137, 40);
        btnViewMaterial.TabIndex = 2;
        btnViewMaterial.Text = "View Material";
        btnViewMaterial.UseVisualStyleBackColor = true;
        btnViewMaterial.Click += btnViewMaterial_Click;
        // 
        // flowLayoutPanelActions
        // 
        flowLayoutPanelActions.AutoSize = true;
        flowLayoutPanelActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flowLayoutPanelActions.Controls.Add(btnAssignMaterial);
        flowLayoutPanelActions.Controls.Add(btnUnassignMaterial);
        flowLayoutPanelActions.Controls.Add(btnViewMaterial);
        flowLayoutPanelActions.Dock = DockStyle.Fill;
        flowLayoutPanelActions.Location = new Point(3, 4);
        flowLayoutPanelActions.Margin = new Padding(3, 4, 3, 4);
        flowLayoutPanelActions.Name = "flowLayoutPanelActions";
        flowLayoutPanelActions.Size = new Size(485, 52);
        flowLayoutPanelActions.TabIndex = 0;
        // 
        // panelModeIndicator
        // 
        panelModeIndicator.AutoSize = true;
        panelModeIndicator.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        panelModeIndicator.BackColor = Color.FromArgb(232, 245, 233);
        panelModeIndicator.BorderStyle = BorderStyle.FixedSingle;
        panelModeIndicator.Controls.Add(lblModeDescription);
        panelModeIndicator.Controls.Add(lblModeTitle);
        panelModeIndicator.Controls.Add(lblModeIcon);
        panelModeIndicator.Dock = DockStyle.Fill;
        panelModeIndicator.Location = new Point(14, 17);
        panelModeIndicator.Margin = new Padding(3, 4, 3, 13);
        panelModeIndicator.Name = "panelModeIndicator";
        panelModeIndicator.Size = new Size(773, 69);
        panelModeIndicator.TabIndex = 0;
        // 
        // lblModeDescription
        // 
        lblModeDescription.AutoSize = true;
        lblModeDescription.Font = new Font("Segoe UI", 9F);
        lblModeDescription.ForeColor = Color.FromArgb(46, 125, 50);
        lblModeDescription.Location = new Point(69, 47);
        lblModeDescription.Name = "lblModeDescription";
        lblModeDescription.Size = new Size(212, 20);
        lblModeDescription.TabIndex = 2;
        lblModeDescription.Text = "Ready to manage today's class";
        // 
        // lblModeTitle
        // 
        lblModeTitle.AutoSize = true;
        lblModeTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblModeTitle.ForeColor = Color.FromArgb(46, 125, 50);
        lblModeTitle.Location = new Point(69, 13);
        lblModeTitle.Name = "lblModeTitle";
        lblModeTitle.Size = new Size(303, 28);
        lblModeTitle.TabIndex = 1;
        lblModeTitle.Text = "Showing students for: Monday";
        // 
        // lblModeIcon
        // 
        lblModeIcon.AutoSize = true;
        lblModeIcon.Font = new Font("Segoe UI Emoji", 20F);
        lblModeIcon.Location = new Point(11, 13);
        lblModeIcon.Name = "lblModeIcon";
        lblModeIcon.Size = new Size(67, 46);
        lblModeIcon.TabIndex = 0;
        lblModeIcon.Text = "📅";
        // 
        // rootLayout
        // 
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(panelModeIndicator, 0, 0);
        rootLayout.Controls.Add(actionsPanel, 0, 1);
        rootLayout.Controls.Add(mainContentPanel, 0, 2);
        rootLayout.Controls.Add(panelClassDescription, 0, 3);
        rootLayout.Controls.Add(flowLayoutPanel2, 0, 4);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Margin = new Padding(3, 4, 3, 4);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(11, 13, 11, 13);
        rootLayout.RowCount = 5;
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));   // description panel (hidden initially)
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
        rootLayout.Size = new Size(801, 594);
        rootLayout.TabIndex = 0;
        // 
        // actionsPanel
        // 
        actionsPanel.AutoSize = true;
        actionsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        actionsPanel.ColumnCount = 2;
        actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        actionsPanel.ColumnStyles.Add(new ColumnStyle());
        actionsPanel.Controls.Add(flowLayoutPanel1, 1, 0);
        actionsPanel.Controls.Add(flowLayoutPanelActions, 0, 0);
        actionsPanel.Dock = DockStyle.Fill;
        actionsPanel.Location = new Point(11, 99);
        actionsPanel.Margin = new Padding(0, 0, 0, 13);
        actionsPanel.Name = "actionsPanel";
        actionsPanel.RowCount = 1;
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
        actionsPanel.Size = new Size(779, 60);
        actionsPanel.TabIndex = 1;
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.AutoSize = true;
        flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flowLayoutPanel1.Controls.Add(btnToggleFilter);
        flowLayoutPanel1.Controls.Add(dtpClassDate);
        flowLayoutPanel1.Dock = DockStyle.Fill;
        flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
        flowLayoutPanel1.Location = new Point(494, 3);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new Size(282, 54);
        flowLayoutPanel1.TabIndex = 3;
        flowLayoutPanel1.WrapContents = false;
        // 
        // btnToggleFilter
        // 
        btnToggleFilter.Anchor = AnchorStyles.Right;
        btnToggleFilter.Location = new Point(130, 4);
        btnToggleFilter.Margin = new Padding(3, 4, 3, 4);
        btnToggleFilter.Name = "btnToggleFilter";
        btnToggleFilter.Size = new Size(149, 40);
        btnToggleFilter.TabIndex = 1;
        btnToggleFilter.Text = "Show All Students";
        btnToggleFilter.UseVisualStyleBackColor = true;
        btnToggleFilter.Click += btnToggleFilter_Click;
        // 
        // dtpClassDate
        // 
        dtpClassDate.Anchor = AnchorStyles.Right;
        dtpClassDate.Format = DateTimePickerFormat.Short;
        dtpClassDate.Location = new Point(3, 13);
        dtpClassDate.Margin = new Padding(3, 9, 3, 4);
        dtpClassDate.Name = "dtpClassDate";
        dtpClassDate.Size = new Size(121, 27);
        dtpClassDate.TabIndex = 2;
        dtpClassDate.ValueChanged += dtpClassDate_ValueChanged;
        // 
        // mainContentPanel
        // 
        mainContentPanel.ColumnCount = 2;
        mainContentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainContentPanel.ColumnStyles.Add(new ColumnStyle());
        mainContentPanel.Controls.Add(dgvStudents, 0, 0);
        mainContentPanel.Controls.Add(sideButtonPanel, 1, 0);
        mainContentPanel.Dock = DockStyle.Fill;
        mainContentPanel.Location = new Point(11, 172);
        mainContentPanel.Margin = new Padding(0, 0, 0, 13);
        mainContentPanel.Name = "mainContentPanel";
        mainContentPanel.RowCount = 1;
        mainContentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainContentPanel.Size = new Size(779, 343);
        mainContentPanel.TabIndex = 2;
        // 
        // sideButtonPanel
        // 
        sideButtonPanel.AutoSize = true;
        sideButtonPanel.Controls.Add(btnAdd);
        sideButtonPanel.Controls.Add(btnUpdate);
        sideButtonPanel.Controls.Add(btnRemove);
        sideButtonPanel.Dock = DockStyle.Fill;
        sideButtonPanel.FlowDirection = FlowDirection.TopDown;
        sideButtonPanel.Location = new Point(636, 4);
        sideButtonPanel.Margin = new Padding(3, 4, 3, 4);
        sideButtonPanel.Name = "sideButtonPanel";
        sideButtonPanel.Size = new Size(140, 335);
        sideButtonPanel.TabIndex = 1;
        sideButtonPanel.WrapContents = false;
        // 
        // flowLayoutPanel2
        // 
        flowLayoutPanel2.AutoSize = true;
        flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flowLayoutPanel2.Controls.Add(btnEditFeedback);
        flowLayoutPanel2.Controls.Add(btnGenerateEmail);
        flowLayoutPanel2.Controls.Add(btnPrepareNextClass);
        flowLayoutPanel2.Location = new Point(14, 532);
        flowLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
        flowLayoutPanel2.Name = "flowLayoutPanel2";
        flowLayoutPanel2.Size = new Size(449, 45);
        flowLayoutPanel2.TabIndex = 3;
        // 
        // btnPrepareNextClass
        // 
        btnPrepareNextClass.Location = new Point(289, 4);
        btnPrepareNextClass.Margin = new Padding(3, 4, 3, 4);
        btnPrepareNextClass.Name = "btnPrepareNextClass";
        btnPrepareNextClass.Size = new Size(157, 40);
        btnPrepareNextClass.TabIndex = 4;
        btnPrepareNextClass.Text = "Prepare Next Class";
        btnPrepareNextClass.UseVisualStyleBackColor = true;
        btnPrepareNextClass.Click += btnPrepareNextClass_Click;
        //
        // panelClassDescription
        //
        panelClassDescription.BackColor = Color.FromArgb(232, 244, 253);
        panelClassDescription.BorderStyle = BorderStyle.FixedSingle;
        panelClassDescription.Controls.Add(txtDescriptionView);
        panelClassDescription.Controls.Add(lblDescriptionTitle);
        panelClassDescription.Dock = DockStyle.Fill;
        panelClassDescription.Margin = new Padding(0, 0, 0, 4);
        panelClassDescription.Name = "panelClassDescription";
        panelClassDescription.Padding = new Padding(8, 4, 8, 4);
        panelClassDescription.Visible = false;
        //
        // lblDescriptionTitle
        //
        lblDescriptionTitle.AutoSize = false;
        lblDescriptionTitle.Dock = DockStyle.Top;
        lblDescriptionTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDescriptionTitle.ForeColor = Color.FromArgb(13, 71, 161);
        lblDescriptionTitle.Height = 20;
        lblDescriptionTitle.Name = "lblDescriptionTitle";
        lblDescriptionTitle.Text = "Class description:";
        //
        // txtDescriptionView
        //
        txtDescriptionView.BackColor = Color.FromArgb(232, 244, 253);
        txtDescriptionView.BorderStyle = BorderStyle.None;
        txtDescriptionView.Dock = DockStyle.Fill;
        txtDescriptionView.Font = new Font("Segoe UI", 9.5F);
        txtDescriptionView.Multiline = true;
        txtDescriptionView.Name = "txtDescriptionView";
        txtDescriptionView.ReadOnly = true;
        txtDescriptionView.ScrollBars = ScrollBars.Vertical;
        txtDescriptionView.TabStop = false;
        //
        // MainDashboard
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(801, 620);
        Controls.Add(rootLayout);
        Controls.Add(statusStrip1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(3, 4, 3, 4);
        Name = "MainDashboard";
        Text = "Feedback Flow - Teacher Assistant";
        Load += MainDashboard_Load;
        ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        flowLayoutPanelActions.ResumeLayout(false);
        panelModeIndicator.ResumeLayout(false);
        panelModeIndicator.PerformLayout();
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        actionsPanel.ResumeLayout(false);
        actionsPanel.PerformLayout();
        flowLayoutPanel1.ResumeLayout(false);
        mainContentPanel.ResumeLayout(false);
        mainContentPanel.PerformLayout();
        sideButtonPanel.ResumeLayout(false);
        flowLayoutPanel2.ResumeLayout(false);
        panelClassDescription.ResumeLayout(false);
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
    private System.Windows.Forms.Button btnGenerateEmail;
    private System.Windows.Forms.Button btnViewMaterial;
    private System.Windows.Forms.Panel panelModeIndicator;
    private System.Windows.Forms.Label lblModeIcon;
    private System.Windows.Forms.Label lblModeTitle;
    private System.Windows.Forms.Label lblModeDescription;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelActions;
    private System.Windows.Forms.TableLayoutPanel rootLayout;
    private System.Windows.Forms.TableLayoutPanel actionsPanel;
    private System.Windows.Forms.TableLayoutPanel mainContentPanel;
    private System.Windows.Forms.FlowLayoutPanel sideButtonPanel;

    #endregion

    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnToggleFilter;
    private DateTimePicker dtpClassDate;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnPrepareNextClass;
    private System.Windows.Forms.Panel panelClassDescription;
    private System.Windows.Forms.Label lblDescriptionTitle;
    private System.Windows.Forms.TextBox txtDescriptionView;
}