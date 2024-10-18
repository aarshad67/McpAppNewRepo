namespace MCPApp
{
    partial class JobPlannerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobPlannerForm));
            this.jobDGV = new System.Windows.Forms.DataGridView();
            this.cancelButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.amendParentJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewPhaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recreateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.CustomerDetailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.siteDetailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.completeJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncompleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.whiteboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSupplierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobDateAuditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.totalInvoiceValueTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filter1Button = new System.Windows.Forms.Button();
            this.rbCompleted = new System.Windows.Forms.RadioButton();
            this.rbInProgress = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbNotDrawn = new System.Windows.Forms.RadioButton();
            this.rbNotOnShop = new System.Windows.Forms.RadioButton();
            this.rbNotApproved = new System.Windows.Forms.RadioButton();
            this.rbDrawn = new System.Windows.Forms.RadioButton();
            this.rbApproved = new System.Windows.Forms.RadioButton();
            this.rbOnShop = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbSupplyFix = new System.Windows.Forms.RadioButton();
            this.rbSupplyOnly = new System.Windows.Forms.RadioButton();
            this.rbBoth = new System.Windows.Forms.RadioButton();
            this.currentYearFiltersGroupBox = new System.Windows.Forms.GroupBox();
            this.decButton = new System.Windows.Forms.Button();
            this.junButton = new System.Windows.Forms.Button();
            this.novButton = new System.Windows.Forms.Button();
            this.mayButton = new System.Windows.Forms.Button();
            this.octButton = new System.Windows.Forms.Button();
            this.aprButton = new System.Windows.Forms.Button();
            this.sepButton = new System.Windows.Forms.Button();
            this.marButton = new System.Windows.Forms.Button();
            this.augButton = new System.Windows.Forms.Button();
            this.febButton = new System.Windows.Forms.Button();
            this.julButton = new System.Windows.Forms.Button();
            this.janButton = new System.Windows.Forms.Button();
            this.getAllJobsButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.totalSlabM2TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.totalBeamM2TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nextYearFiltersGroupBox = new System.Windows.Forms.GroupBox();
            this.dec2Button = new System.Windows.Forms.Button();
            this.jun2Button = new System.Windows.Forms.Button();
            this.nov2Button = new System.Windows.Forms.Button();
            this.may2Button = new System.Windows.Forms.Button();
            this.oct2Button = new System.Windows.Forms.Button();
            this.apr2Button = new System.Windows.Forms.Button();
            this.sep2Button = new System.Windows.Forms.Button();
            this.mar2Button = new System.Windows.Forms.Button();
            this.aug2Button = new System.Windows.Forms.Button();
            this.feb2Button = new System.Windows.Forms.Button();
            this.jul2Button = new System.Windows.Forms.Button();
            this.jan2Button = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.latestButton = new System.Windows.Forms.Button();
            this.earliestButton = new System.Windows.Forms.Button();
            this.excelButton = new System.Windows.Forms.Button();
            this.btnLockedJobs = new System.Windows.Forms.Button();
            this.txtTotalJobMgn = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.jobDGV)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.currentYearFiltersGroupBox.SuspendLayout();
            this.nextYearFiltersGroupBox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // jobDGV
            // 
            this.jobDGV.AllowUserToAddRows = false;
            this.jobDGV.AllowUserToDeleteRows = false;
            this.jobDGV.AllowUserToResizeRows = false;
            this.jobDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jobDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobDGV.Location = new System.Drawing.Point(18, 178);
            this.jobDGV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jobDGV.Name = "jobDGV";
            this.jobDGV.RowHeadersWidth = 62;
            this.jobDGV.Size = new System.Drawing.Size(2092, 883);
            this.jobDGV.TabIndex = 121;
            this.jobDGV.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.jobDGV_CellBeginEdit);
            this.jobDGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobDGV_CellClick);
            this.jobDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobDGV_CellDoubleClick);
            this.jobDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobDGV_CellEndEdit);
            this.jobDGV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.jobDGV_CellFormatting);
            this.jobDGV.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.jobDGV_CellMouseUp);
            this.jobDGV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobDGV_CellValueChanged);
            this.jobDGV.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.jobDGV_DataError);
            this.jobDGV.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.jobDGV_EditingControlShowing);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(202, 1068);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(166, 80);
            this.cancelButton.TabIndex = 145;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.amendParentJobToolStripMenuItem,
            this.addNewPhaseToolStripMenuItem1,
            this.cancelJobToolStripMenuItem,
            this.recreateToolStripMenuItem1,
            this.CustomerDetailToolStripMenuItem,
            this.siteDetailToolStripMenuItem,
            this.completeJobToolStripMenuItem,
            this.uncompleteToolStripMenuItem1,
            this.whiteboardToolStripMenuItem,
            this.removeSupplierToolStripMenuItem,
            this.jobDateAuditToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(342, 356);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // amendParentJobToolStripMenuItem
            // 
            this.amendParentJobToolStripMenuItem.Name = "amendParentJobToolStripMenuItem";
            this.amendParentJobToolStripMenuItem.Size = new System.Drawing.Size(341, 32);
            this.amendParentJobToolStripMenuItem.Text = "Amend Parent Job";
            this.amendParentJobToolStripMenuItem.Click += new System.EventHandler(this.amendParentJobToolStripMenuItem_Click);
            // 
            // addNewPhaseToolStripMenuItem1
            // 
            this.addNewPhaseToolStripMenuItem1.Name = "addNewPhaseToolStripMenuItem1";
            this.addNewPhaseToolStripMenuItem1.Size = new System.Drawing.Size(341, 32);
            this.addNewPhaseToolStripMenuItem1.Text = "Add a new phase";
            this.addNewPhaseToolStripMenuItem1.Click += new System.EventHandler(this.addNewPhaseToolStripMenuItem1_Click);
            // 
            // cancelJobToolStripMenuItem
            // 
            this.cancelJobToolStripMenuItem.Name = "cancelJobToolStripMenuItem";
            this.cancelJobToolStripMenuItem.Size = new System.Drawing.Size(341, 32);
            this.cancelJobToolStripMenuItem.Text = "CANCEL Phase Job";
            this.cancelJobToolStripMenuItem.Click += new System.EventHandler(this.cancelJobToolStripMenuItem_Click);
            // 
            // recreateToolStripMenuItem1
            // 
            this.recreateToolStripMenuItem1.Name = "recreateToolStripMenuItem1";
            this.recreateToolStripMenuItem1.Size = new System.Drawing.Size(341, 32);
            this.recreateToolStripMenuItem1.Text = "Re-create a deleted phase";
            this.recreateToolStripMenuItem1.Click += new System.EventHandler(this.recreateToolStripMenuItem1_Click);
            // 
            // CustomerDetailToolStripMenuItem
            // 
            this.CustomerDetailToolStripMenuItem.Name = "CustomerDetailToolStripMenuItem";
            this.CustomerDetailToolStripMenuItem.Size = new System.Drawing.Size(341, 32);
            this.CustomerDetailToolStripMenuItem.Text = "View Customer Details";
            this.CustomerDetailToolStripMenuItem.Click += new System.EventHandler(this.CustomerDetailToolStripMenuItem_Click);
            // 
            // siteDetailToolStripMenuItem
            // 
            this.siteDetailToolStripMenuItem.Name = "siteDetailToolStripMenuItem";
            this.siteDetailToolStripMenuItem.Size = new System.Drawing.Size(341, 32);
            this.siteDetailToolStripMenuItem.Text = "View Site Details";
            this.siteDetailToolStripMenuItem.Click += new System.EventHandler(this.siteDetailToolStripMenuItem_Click);
            // 
            // completeJobToolStripMenuItem
            // 
            this.completeJobToolStripMenuItem.Name = "completeJobToolStripMenuItem";
            this.completeJobToolStripMenuItem.Size = new System.Drawing.Size(341, 32);
            this.completeJobToolStripMenuItem.Text = "Complete the Job";
            this.completeJobToolStripMenuItem.Click += new System.EventHandler(this.completeJobToolStripMenuItem_Click);
            // 
            // uncompleteToolStripMenuItem1
            // 
            this.uncompleteToolStripMenuItem1.Name = "uncompleteToolStripMenuItem1";
            this.uncompleteToolStripMenuItem1.Size = new System.Drawing.Size(341, 32);
            this.uncompleteToolStripMenuItem1.Text = "Un-complete Job";
            this.uncompleteToolStripMenuItem1.Click += new System.EventHandler(this.uncompleteToolStripMenuItem1_Click);
            // 
            // whiteboardToolStripMenuItem
            // 
            this.whiteboardToolStripMenuItem.Name = "whiteboardToolStripMenuItem";
            this.whiteboardToolStripMenuItem.Size = new System.Drawing.Size(341, 32);
            this.whiteboardToolStripMenuItem.Text = "Go To Job in Whiteboard";
            this.whiteboardToolStripMenuItem.Click += new System.EventHandler(this.whiteboardToolStripMenuItem_Click);
            // 
            // removeSupplierToolStripMenuItem
            // 
            this.removeSupplierToolStripMenuItem.Name = "removeSupplierToolStripMenuItem";
            this.removeSupplierToolStripMenuItem.Size = new System.Drawing.Size(341, 32);
            this.removeSupplierToolStripMenuItem.Text = "Remove Supplier";
            this.removeSupplierToolStripMenuItem.Click += new System.EventHandler(this.removeSupplierToolStripMenuItem_Click);
            // 
            // jobDateAuditToolStripMenuItem
            // 
            this.jobDateAuditToolStripMenuItem.Name = "jobDateAuditToolStripMenuItem";
            this.jobDateAuditToolStripMenuItem.Size = new System.Drawing.Size(341, 32);
            this.jobDateAuditToolStripMenuItem.Text = "Audit of Job Date(s) Movements";
            this.jobDateAuditToolStripMenuItem.Click += new System.EventHandler(this.jobDateAuditToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1535, 1081);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 65);
            this.label1.TabIndex = 146;
            this.label1.Text = "Total Inv Value(£) :";
            // 
            // totalInvoiceValueTextBox
            // 
            this.totalInvoiceValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.totalInvoiceValueTextBox.BackColor = System.Drawing.Color.Aqua;
            this.totalInvoiceValueTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalInvoiceValueTextBox.Location = new System.Drawing.Point(1637, 1081);
            this.totalInvoiceValueTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.totalInvoiceValueTextBox.Name = "totalInvoiceValueTextBox";
            this.totalInvoiceValueTextBox.Size = new System.Drawing.Size(162, 30);
            this.totalInvoiceValueTextBox.TabIndex = 147;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.filter1Button);
            this.groupBox1.Controls.Add(this.rbCompleted);
            this.groupBox1.Controls.Add(this.rbInProgress);
            this.groupBox1.Controls.Add(this.rbAll);
            this.groupBox1.Controls.Add(this.rbNotDrawn);
            this.groupBox1.Controls.Add(this.rbNotOnShop);
            this.groupBox1.Controls.Add(this.rbNotApproved);
            this.groupBox1.Controls.Add(this.rbDrawn);
            this.groupBox1.Controls.Add(this.rbApproved);
            this.groupBox1.Controls.Add(this.rbOnShop);
            this.groupBox1.Location = new System.Drawing.Point(202, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(632, 151);
            this.groupBox1.TabIndex = 148;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter By";
            // 
            // filter1Button
            // 
            this.filter1Button.Location = new System.Drawing.Point(526, 23);
            this.filter1Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.filter1Button.Name = "filter1Button";
            this.filter1Button.Size = new System.Drawing.Size(92, 108);
            this.filter1Button.TabIndex = 151;
            this.filter1Button.Text = "SEARCH";
            this.filter1Button.UseVisualStyleBackColor = true;
            this.filter1Button.Click += new System.EventHandler(this.filter1Button_Click_1);
            // 
            // rbCompleted
            // 
            this.rbCompleted.AutoSize = true;
            this.rbCompleted.Location = new System.Drawing.Point(369, 29);
            this.rbCompleted.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbCompleted.Name = "rbCompleted";
            this.rbCompleted.Size = new System.Drawing.Size(132, 24);
            this.rbCompleted.TabIndex = 11;
            this.rbCompleted.TabStop = true;
            this.rbCompleted.Text = "COMPLETED";
            this.rbCompleted.UseVisualStyleBackColor = true;
            // 
            // rbInProgress
            // 
            this.rbInProgress.AutoSize = true;
            this.rbInProgress.Location = new System.Drawing.Point(369, 65);
            this.rbInProgress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbInProgress.Name = "rbInProgress";
            this.rbInProgress.Size = new System.Drawing.Size(147, 24);
            this.rbInProgress.TabIndex = 10;
            this.rbInProgress.TabStop = true;
            this.rbInProgress.Text = "IN-PROGRESS";
            this.rbInProgress.UseVisualStyleBackColor = true;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Enabled = false;
            this.rbAll.Location = new System.Drawing.Point(369, 95);
            this.rbAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(63, 24);
            this.rbAll.TabIndex = 9;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "ALL";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbNotDrawn
            // 
            this.rbNotDrawn.AutoSize = true;
            this.rbNotDrawn.Enabled = false;
            this.rbNotDrawn.Location = new System.Drawing.Point(178, 100);
            this.rbNotDrawn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbNotDrawn.Name = "rbNotDrawn";
            this.rbNotDrawn.Size = new System.Drawing.Size(131, 24);
            this.rbNotDrawn.TabIndex = 8;
            this.rbNotDrawn.TabStop = true;
            this.rbNotDrawn.Text = "NOT DRAWN";
            this.rbNotDrawn.UseVisualStyleBackColor = true;
            // 
            // rbNotOnShop
            // 
            this.rbNotOnShop.AutoSize = true;
            this.rbNotOnShop.Location = new System.Drawing.Point(178, 29);
            this.rbNotOnShop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbNotOnShop.Name = "rbNotOnShop";
            this.rbNotOnShop.Size = new System.Drawing.Size(142, 24);
            this.rbNotOnShop.TabIndex = 7;
            this.rbNotOnShop.TabStop = true;
            this.rbNotOnShop.Text = "NOT ON SHOP";
            this.rbNotOnShop.UseVisualStyleBackColor = true;
            // 
            // rbNotApproved
            // 
            this.rbNotApproved.AutoSize = true;
            this.rbNotApproved.Location = new System.Drawing.Point(178, 65);
            this.rbNotApproved.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbNotApproved.Name = "rbNotApproved";
            this.rbNotApproved.Size = new System.Drawing.Size(159, 24);
            this.rbNotApproved.TabIndex = 6;
            this.rbNotApproved.TabStop = true;
            this.rbNotApproved.Text = "NOT APPROVED";
            this.rbNotApproved.UseVisualStyleBackColor = true;
            // 
            // rbDrawn
            // 
            this.rbDrawn.AutoSize = true;
            this.rbDrawn.Enabled = false;
            this.rbDrawn.Location = new System.Drawing.Point(26, 100);
            this.rbDrawn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbDrawn.Name = "rbDrawn";
            this.rbDrawn.Size = new System.Drawing.Size(95, 24);
            this.rbDrawn.TabIndex = 5;
            this.rbDrawn.TabStop = true;
            this.rbDrawn.Text = "DRAWN";
            this.rbDrawn.UseVisualStyleBackColor = true;
            // 
            // rbApproved
            // 
            this.rbApproved.AutoSize = true;
            this.rbApproved.Location = new System.Drawing.Point(26, 65);
            this.rbApproved.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbApproved.Name = "rbApproved";
            this.rbApproved.Size = new System.Drawing.Size(123, 24);
            this.rbApproved.TabIndex = 4;
            this.rbApproved.TabStop = true;
            this.rbApproved.Text = "APPROVED";
            this.rbApproved.UseVisualStyleBackColor = true;
            // 
            // rbOnShop
            // 
            this.rbOnShop.AutoSize = true;
            this.rbOnShop.Location = new System.Drawing.Point(26, 29);
            this.rbOnShop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbOnShop.Name = "rbOnShop";
            this.rbOnShop.Size = new System.Drawing.Size(106, 24);
            this.rbOnShop.TabIndex = 3;
            this.rbOnShop.TabStop = true;
            this.rbOnShop.Text = "ON SHOP";
            this.rbOnShop.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbSupplyFix);
            this.groupBox2.Controls.Add(this.rbSupplyOnly);
            this.groupBox2.Controls.Add(this.rbBoth);
            this.groupBox2.Location = new System.Drawing.Point(18, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(162, 151);
            this.groupBox2.TabIndex = 149;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Supply Type";
            // 
            // rbSupplyFix
            // 
            this.rbSupplyFix.AutoSize = true;
            this.rbSupplyFix.Location = new System.Drawing.Point(46, 100);
            this.rbSupplyFix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSupplyFix.Name = "rbSupplyFix";
            this.rbSupplyFix.Size = new System.Drawing.Size(88, 24);
            this.rbSupplyFix.TabIndex = 2;
            this.rbSupplyFix.TabStop = true;
            this.rbSupplyFix.Text = "SF / XF";
            this.rbSupplyFix.UseVisualStyleBackColor = true;
            // 
            // rbSupplyOnly
            // 
            this.rbSupplyOnly.AutoSize = true;
            this.rbSupplyOnly.Location = new System.Drawing.Point(46, 65);
            this.rbSupplyOnly.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSupplyOnly.Name = "rbSupplyOnly";
            this.rbSupplyOnly.Size = new System.Drawing.Size(92, 24);
            this.rbSupplyOnly.TabIndex = 1;
            this.rbSupplyOnly.TabStop = true;
            this.rbSupplyOnly.Text = "SO / XO";
            this.rbSupplyOnly.UseVisualStyleBackColor = true;
            // 
            // rbBoth
            // 
            this.rbBoth.AutoSize = true;
            this.rbBoth.Location = new System.Drawing.Point(46, 29);
            this.rbBoth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbBoth.Name = "rbBoth";
            this.rbBoth.Size = new System.Drawing.Size(78, 24);
            this.rbBoth.TabIndex = 0;
            this.rbBoth.TabStop = true;
            this.rbBoth.Text = "BOTH";
            this.rbBoth.UseVisualStyleBackColor = true;
            // 
            // currentYearFiltersGroupBox
            // 
            this.currentYearFiltersGroupBox.Controls.Add(this.decButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.junButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.novButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.mayButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.octButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.aprButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.sepButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.marButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.augButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.febButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.julButton);
            this.currentYearFiltersGroupBox.Controls.Add(this.janButton);
            this.currentYearFiltersGroupBox.Location = new System.Drawing.Point(843, 18);
            this.currentYearFiltersGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.currentYearFiltersGroupBox.Name = "currentYearFiltersGroupBox";
            this.currentYearFiltersGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.currentYearFiltersGroupBox.Size = new System.Drawing.Size(482, 151);
            this.currentYearFiltersGroupBox.TabIndex = 151;
            this.currentYearFiltersGroupBox.TabStop = false;
            this.currentYearFiltersGroupBox.Text = "Filter by MONTH";
            // 
            // decButton
            // 
            this.decButton.Location = new System.Drawing.Point(399, 80);
            this.decButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.decButton.Name = "decButton";
            this.decButton.Size = new System.Drawing.Size(66, 57);
            this.decButton.TabIndex = 11;
            this.decButton.Text = "DEC";
            this.decButton.UseVisualStyleBackColor = true;
            this.decButton.Click += new System.EventHandler(this.decButton_Click);
            // 
            // junButton
            // 
            this.junButton.Location = new System.Drawing.Point(399, 29);
            this.junButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.junButton.Name = "junButton";
            this.junButton.Size = new System.Drawing.Size(66, 57);
            this.junButton.TabIndex = 10;
            this.junButton.Text = "JUN";
            this.junButton.UseVisualStyleBackColor = true;
            this.junButton.Click += new System.EventHandler(this.junButton_Click);
            // 
            // novButton
            // 
            this.novButton.Location = new System.Drawing.Point(324, 80);
            this.novButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.novButton.Name = "novButton";
            this.novButton.Size = new System.Drawing.Size(66, 57);
            this.novButton.TabIndex = 9;
            this.novButton.Text = "NOV";
            this.novButton.UseVisualStyleBackColor = true;
            this.novButton.Click += new System.EventHandler(this.novButton_Click);
            // 
            // mayButton
            // 
            this.mayButton.Location = new System.Drawing.Point(324, 29);
            this.mayButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mayButton.Name = "mayButton";
            this.mayButton.Size = new System.Drawing.Size(66, 57);
            this.mayButton.TabIndex = 8;
            this.mayButton.Text = "MAY";
            this.mayButton.UseVisualStyleBackColor = true;
            this.mayButton.Click += new System.EventHandler(this.mayButton_Click);
            // 
            // octButton
            // 
            this.octButton.Location = new System.Drawing.Point(249, 80);
            this.octButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.octButton.Name = "octButton";
            this.octButton.Size = new System.Drawing.Size(66, 57);
            this.octButton.TabIndex = 7;
            this.octButton.Text = "OCT";
            this.octButton.UseVisualStyleBackColor = true;
            this.octButton.Click += new System.EventHandler(this.octButton_Click);
            // 
            // aprButton
            // 
            this.aprButton.Location = new System.Drawing.Point(249, 29);
            this.aprButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.aprButton.Name = "aprButton";
            this.aprButton.Size = new System.Drawing.Size(66, 57);
            this.aprButton.TabIndex = 6;
            this.aprButton.Text = "APR";
            this.aprButton.UseVisualStyleBackColor = true;
            this.aprButton.Click += new System.EventHandler(this.aprButton_Click);
            // 
            // sepButton
            // 
            this.sepButton.Location = new System.Drawing.Point(170, 80);
            this.sepButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sepButton.Name = "sepButton";
            this.sepButton.Size = new System.Drawing.Size(70, 57);
            this.sepButton.TabIndex = 5;
            this.sepButton.Text = "SEP";
            this.sepButton.UseVisualStyleBackColor = true;
            this.sepButton.Click += new System.EventHandler(this.sepButton_Click);
            // 
            // marButton
            // 
            this.marButton.Location = new System.Drawing.Point(170, 29);
            this.marButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.marButton.Name = "marButton";
            this.marButton.Size = new System.Drawing.Size(70, 57);
            this.marButton.TabIndex = 4;
            this.marButton.Text = "MAR";
            this.marButton.UseVisualStyleBackColor = true;
            this.marButton.Click += new System.EventHandler(this.marButton_Click);
            // 
            // augButton
            // 
            this.augButton.Location = new System.Drawing.Point(90, 80);
            this.augButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.augButton.Name = "augButton";
            this.augButton.Size = new System.Drawing.Size(70, 57);
            this.augButton.TabIndex = 3;
            this.augButton.Text = "AUG";
            this.augButton.UseVisualStyleBackColor = true;
            this.augButton.Click += new System.EventHandler(this.augButton_Click);
            // 
            // febButton
            // 
            this.febButton.Location = new System.Drawing.Point(90, 29);
            this.febButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.febButton.Name = "febButton";
            this.febButton.Size = new System.Drawing.Size(70, 57);
            this.febButton.TabIndex = 2;
            this.febButton.Text = "FEB";
            this.febButton.UseVisualStyleBackColor = true;
            this.febButton.Click += new System.EventHandler(this.febButton_Click);
            // 
            // julButton
            // 
            this.julButton.Location = new System.Drawing.Point(12, 80);
            this.julButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.julButton.Name = "julButton";
            this.julButton.Size = new System.Drawing.Size(69, 57);
            this.julButton.TabIndex = 1;
            this.julButton.Text = "JUL";
            this.julButton.UseVisualStyleBackColor = true;
            this.julButton.Click += new System.EventHandler(this.julButton_Click);
            // 
            // janButton
            // 
            this.janButton.Location = new System.Drawing.Point(12, 29);
            this.janButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.janButton.Name = "janButton";
            this.janButton.Size = new System.Drawing.Size(69, 57);
            this.janButton.TabIndex = 0;
            this.janButton.Text = "JAN";
            this.janButton.UseVisualStyleBackColor = true;
            this.janButton.Click += new System.EventHandler(this.janButton_Click);
            // 
            // getAllJobsButton
            // 
            this.getAllJobsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.getAllJobsButton.Location = new System.Drawing.Point(1824, 25);
            this.getAllJobsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.getAllJobsButton.Name = "getAllJobsButton";
            this.getAllJobsButton.Size = new System.Drawing.Size(116, 145);
            this.getAllJobsButton.TabIndex = 152;
            this.getAllJobsButton.Text = "GET ALL JOBS";
            this.getAllJobsButton.UseVisualStyleBackColor = true;
            this.getAllJobsButton.Click += new System.EventHandler(this.getAllJobsButton_Click);
            // 
            // updateButton
            // 
            this.updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.updateButton.BackColor = System.Drawing.Color.Green;
            this.updateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateButton.ForeColor = System.Drawing.Color.White;
            this.updateButton.Location = new System.Drawing.Point(16, 1069);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(179, 80);
            this.updateButton.TabIndex = 152;
            this.updateButton.Text = "UPDATE AND CLOSE";
            this.updateButton.UseVisualStyleBackColor = false;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // totalSlabM2TextBox
            // 
            this.totalSlabM2TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.totalSlabM2TextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.totalSlabM2TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalSlabM2TextBox.Location = new System.Drawing.Point(1078, 1085);
            this.totalSlabM2TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.totalSlabM2TextBox.Name = "totalSlabM2TextBox";
            this.totalSlabM2TextBox.Size = new System.Drawing.Size(142, 30);
            this.totalSlabM2TextBox.TabIndex = 154;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(954, 1085);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 65);
            this.label2.TabIndex = 153;
            this.label2.Text = "Total SLAB M2";
            // 
            // totalBeamM2TextBox
            // 
            this.totalBeamM2TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.totalBeamM2TextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.totalBeamM2TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalBeamM2TextBox.Location = new System.Drawing.Point(1367, 1082);
            this.totalBeamM2TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.totalBeamM2TextBox.Name = "totalBeamM2TextBox";
            this.totalBeamM2TextBox.Size = new System.Drawing.Size(118, 30);
            this.totalBeamM2TextBox.TabIndex = 156;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1231, 1084);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 65);
            this.label3.TabIndex = 155;
            this.label3.Text = "Total BEAM M2";
            // 
            // nextYearFiltersGroupBox
            // 
            this.nextYearFiltersGroupBox.Controls.Add(this.dec2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.jun2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.nov2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.may2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.oct2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.apr2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.sep2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.mar2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.aug2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.feb2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.jul2Button);
            this.nextYearFiltersGroupBox.Controls.Add(this.jan2Button);
            this.nextYearFiltersGroupBox.Location = new System.Drawing.Point(1334, 18);
            this.nextYearFiltersGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nextYearFiltersGroupBox.Name = "nextYearFiltersGroupBox";
            this.nextYearFiltersGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nextYearFiltersGroupBox.Size = new System.Drawing.Size(482, 151);
            this.nextYearFiltersGroupBox.TabIndex = 158;
            this.nextYearFiltersGroupBox.TabStop = false;
            this.nextYearFiltersGroupBox.Text = "Filter by MONTH";
            // 
            // dec2Button
            // 
            this.dec2Button.Location = new System.Drawing.Point(399, 80);
            this.dec2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dec2Button.Name = "dec2Button";
            this.dec2Button.Size = new System.Drawing.Size(66, 57);
            this.dec2Button.TabIndex = 11;
            this.dec2Button.Text = "DEC";
            this.dec2Button.UseVisualStyleBackColor = true;
            this.dec2Button.Click += new System.EventHandler(this.dec2Button_Click);
            // 
            // jun2Button
            // 
            this.jun2Button.Location = new System.Drawing.Point(399, 29);
            this.jun2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jun2Button.Name = "jun2Button";
            this.jun2Button.Size = new System.Drawing.Size(66, 57);
            this.jun2Button.TabIndex = 10;
            this.jun2Button.Text = "JUN";
            this.jun2Button.UseVisualStyleBackColor = true;
            this.jun2Button.Click += new System.EventHandler(this.jun2Button_Click);
            // 
            // nov2Button
            // 
            this.nov2Button.Location = new System.Drawing.Point(324, 80);
            this.nov2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nov2Button.Name = "nov2Button";
            this.nov2Button.Size = new System.Drawing.Size(66, 57);
            this.nov2Button.TabIndex = 9;
            this.nov2Button.Text = "NOV";
            this.nov2Button.UseVisualStyleBackColor = true;
            this.nov2Button.Click += new System.EventHandler(this.nov2Button_Click);
            // 
            // may2Button
            // 
            this.may2Button.Location = new System.Drawing.Point(324, 29);
            this.may2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.may2Button.Name = "may2Button";
            this.may2Button.Size = new System.Drawing.Size(66, 57);
            this.may2Button.TabIndex = 8;
            this.may2Button.Text = "MAY";
            this.may2Button.UseVisualStyleBackColor = true;
            this.may2Button.Click += new System.EventHandler(this.may2Button_Click);
            // 
            // oct2Button
            // 
            this.oct2Button.Location = new System.Drawing.Point(249, 80);
            this.oct2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.oct2Button.Name = "oct2Button";
            this.oct2Button.Size = new System.Drawing.Size(66, 57);
            this.oct2Button.TabIndex = 7;
            this.oct2Button.Text = "OCT";
            this.oct2Button.UseVisualStyleBackColor = true;
            this.oct2Button.Click += new System.EventHandler(this.oct2Button_Click);
            // 
            // apr2Button
            // 
            this.apr2Button.Location = new System.Drawing.Point(249, 29);
            this.apr2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.apr2Button.Name = "apr2Button";
            this.apr2Button.Size = new System.Drawing.Size(66, 57);
            this.apr2Button.TabIndex = 6;
            this.apr2Button.Text = "APR";
            this.apr2Button.UseVisualStyleBackColor = true;
            this.apr2Button.Click += new System.EventHandler(this.apr2Button_Click);
            // 
            // sep2Button
            // 
            this.sep2Button.Location = new System.Drawing.Point(170, 80);
            this.sep2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sep2Button.Name = "sep2Button";
            this.sep2Button.Size = new System.Drawing.Size(70, 57);
            this.sep2Button.TabIndex = 5;
            this.sep2Button.Text = "SEP";
            this.sep2Button.UseVisualStyleBackColor = true;
            this.sep2Button.Click += new System.EventHandler(this.sep2Button_Click);
            // 
            // mar2Button
            // 
            this.mar2Button.Location = new System.Drawing.Point(170, 29);
            this.mar2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mar2Button.Name = "mar2Button";
            this.mar2Button.Size = new System.Drawing.Size(70, 57);
            this.mar2Button.TabIndex = 4;
            this.mar2Button.Text = "MAR";
            this.mar2Button.UseVisualStyleBackColor = true;
            this.mar2Button.Click += new System.EventHandler(this.mar2Button_Click);
            // 
            // aug2Button
            // 
            this.aug2Button.Location = new System.Drawing.Point(90, 80);
            this.aug2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.aug2Button.Name = "aug2Button";
            this.aug2Button.Size = new System.Drawing.Size(70, 57);
            this.aug2Button.TabIndex = 3;
            this.aug2Button.Text = "AUG";
            this.aug2Button.UseVisualStyleBackColor = true;
            this.aug2Button.Click += new System.EventHandler(this.aug2Button_Click);
            // 
            // feb2Button
            // 
            this.feb2Button.Location = new System.Drawing.Point(90, 29);
            this.feb2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.feb2Button.Name = "feb2Button";
            this.feb2Button.Size = new System.Drawing.Size(70, 57);
            this.feb2Button.TabIndex = 2;
            this.feb2Button.Text = "FEB";
            this.feb2Button.UseVisualStyleBackColor = true;
            this.feb2Button.Click += new System.EventHandler(this.feb2Button_Click);
            // 
            // jul2Button
            // 
            this.jul2Button.Location = new System.Drawing.Point(12, 80);
            this.jul2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jul2Button.Name = "jul2Button";
            this.jul2Button.Size = new System.Drawing.Size(69, 57);
            this.jul2Button.TabIndex = 1;
            this.jul2Button.Text = "JUL";
            this.jul2Button.UseVisualStyleBackColor = true;
            this.jul2Button.Click += new System.EventHandler(this.jul2Button_Click);
            // 
            // jan2Button
            // 
            this.jan2Button.Location = new System.Drawing.Point(12, 29);
            this.jan2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jan2Button.Name = "jan2Button";
            this.jan2Button.Size = new System.Drawing.Size(69, 57);
            this.jan2Button.TabIndex = 0;
            this.jan2Button.Text = "JAN";
            this.jan2Button.UseVisualStyleBackColor = true;
            this.jan2Button.Click += new System.EventHandler(this.jan2Button_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.latestButton);
            this.groupBox4.Controls.Add(this.earliestButton);
            this.groupBox4.Location = new System.Drawing.Point(1960, 25);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(148, 149);
            this.groupBox4.TabIndex = 159;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Date Order";
            // 
            // latestButton
            // 
            this.latestButton.Location = new System.Drawing.Point(18, 86);
            this.latestButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.latestButton.Name = "latestButton";
            this.latestButton.Size = new System.Drawing.Size(122, 43);
            this.latestButton.TabIndex = 1;
            this.latestButton.Text = "LATEST";
            this.latestButton.UseVisualStyleBackColor = true;
            this.latestButton.Click += new System.EventHandler(this.latestButton_Click_1);
            // 
            // earliestButton
            // 
            this.earliestButton.Location = new System.Drawing.Point(18, 37);
            this.earliestButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.earliestButton.Name = "earliestButton";
            this.earliestButton.Size = new System.Drawing.Size(122, 43);
            this.earliestButton.TabIndex = 0;
            this.earliestButton.Text = "EARLIEST";
            this.earliestButton.UseVisualStyleBackColor = true;
            this.earliestButton.Click += new System.EventHandler(this.earliestButton_Click_1);
            // 
            // excelButton
            // 
            this.excelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.excelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.excelButton.Location = new System.Drawing.Point(547, 1069);
            this.excelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(145, 75);
            this.excelButton.TabIndex = 160;
            this.excelButton.Text = "EXPORT TO EXCEL";
            this.excelButton.UseVisualStyleBackColor = false;
            this.excelButton.Click += new System.EventHandler(this.excelButton_Click);
            // 
            // btnLockedJobs
            // 
            this.btnLockedJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLockedJobs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnLockedJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLockedJobs.ForeColor = System.Drawing.Color.Black;
            this.btnLockedJobs.Location = new System.Drawing.Point(374, 1068);
            this.btnLockedJobs.Name = "btnLockedJobs";
            this.btnLockedJobs.Size = new System.Drawing.Size(166, 80);
            this.btnLockedJobs.TabIndex = 161;
            this.btnLockedJobs.Text = "LOCKED JOBS";
            this.btnLockedJobs.UseVisualStyleBackColor = false;
            this.btnLockedJobs.Click += new System.EventHandler(this.btnLockedJobs_Click);
            // 
            // txtTotalJobMgn
            // 
            this.txtTotalJobMgn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalJobMgn.BackColor = System.Drawing.Color.Aqua;
            this.txtTotalJobMgn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalJobMgn.Location = new System.Drawing.Point(1953, 1078);
            this.txtTotalJobMgn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalJobMgn.Name = "txtTotalJobMgn";
            this.txtTotalJobMgn.Size = new System.Drawing.Size(162, 30);
            this.txtTotalJobMgn.TabIndex = 163;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1833, 1079);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 65);
            this.label4.TabIndex = 162;
            this.label4.Text = "Total Job Mgn(£) :";
            // 
            // JobPlannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(2128, 1195);
            this.Controls.Add(this.txtTotalJobMgn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnLockedJobs);
            this.Controls.Add(this.excelButton);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.nextYearFiltersGroupBox);
            this.Controls.Add(this.getAllJobsButton);
            this.Controls.Add(this.totalBeamM2TextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.totalSlabM2TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.currentYearFiltersGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.totalInvoiceValueTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.jobDGV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "JobPlannerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JobPlannerForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JobPlannerForm_FormClosing);
            this.Load += new System.EventHandler(this.JobPlannerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.jobDGV)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.currentYearFiltersGroupBox.ResumeLayout(false);
            this.nextYearFiltersGroupBox.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView jobDGV;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CustomerDetailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem siteDetailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem completeJobToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox totalInvoiceValueTextBox;
        private System.Windows.Forms.ToolStripMenuItem addNewPhaseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cancelJobToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recreateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem uncompleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem whiteboardToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbNotDrawn;
        private System.Windows.Forms.RadioButton rbNotOnShop;
        private System.Windows.Forms.RadioButton rbNotApproved;
        private System.Windows.Forms.RadioButton rbDrawn;
        private System.Windows.Forms.RadioButton rbApproved;
        private System.Windows.Forms.RadioButton rbOnShop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbSupplyFix;
        private System.Windows.Forms.RadioButton rbSupplyOnly;
        private System.Windows.Forms.RadioButton rbBoth;
        private System.Windows.Forms.GroupBox currentYearFiltersGroupBox;
        private System.Windows.Forms.Button decButton;
        private System.Windows.Forms.Button junButton;
        private System.Windows.Forms.Button novButton;
        private System.Windows.Forms.Button mayButton;
        private System.Windows.Forms.Button octButton;
        private System.Windows.Forms.Button aprButton;
        private System.Windows.Forms.Button sepButton;
        private System.Windows.Forms.Button marButton;
        private System.Windows.Forms.Button augButton;
        private System.Windows.Forms.Button febButton;
        private System.Windows.Forms.Button julButton;
        private System.Windows.Forms.Button janButton;
        private System.Windows.Forms.Button getAllJobsButton;
        private System.Windows.Forms.RadioButton rbCompleted;
        private System.Windows.Forms.RadioButton rbInProgress;
        private System.Windows.Forms.ToolStripMenuItem amendParentJobToolStripMenuItem;
        private System.Windows.Forms.Button filter1Button;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.ToolStripMenuItem removeSupplierToolStripMenuItem;
        private System.Windows.Forms.TextBox totalSlabM2TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox totalBeamM2TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem jobDateAuditToolStripMenuItem;
        private System.Windows.Forms.GroupBox nextYearFiltersGroupBox;
        private System.Windows.Forms.Button dec2Button;
        private System.Windows.Forms.Button jun2Button;
        private System.Windows.Forms.Button nov2Button;
        private System.Windows.Forms.Button may2Button;
        private System.Windows.Forms.Button oct2Button;
        private System.Windows.Forms.Button apr2Button;
        private System.Windows.Forms.Button sep2Button;
        private System.Windows.Forms.Button mar2Button;
        private System.Windows.Forms.Button aug2Button;
        private System.Windows.Forms.Button feb2Button;
        private System.Windows.Forms.Button jul2Button;
        private System.Windows.Forms.Button jan2Button;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button latestButton;
        private System.Windows.Forms.Button earliestButton;
        private System.Windows.Forms.Button excelButton;
        private System.Windows.Forms.Button btnLockedJobs;
        private System.Windows.Forms.TextBox txtTotalJobMgn;
        private System.Windows.Forms.Label label4;
    }
}