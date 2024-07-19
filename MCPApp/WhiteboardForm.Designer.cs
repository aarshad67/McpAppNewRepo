namespace MCPApp
{
    partial class WhiteboardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhiteboardForm));
            this.jobPlannerStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wbDailyContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addCommenttoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weeksTabControl = new System.Windows.Forms.TabControl();
            this.wbJobContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.spanJobMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removedExtendedJobMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.raisePOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobPOsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobCommentsAudittoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobDateAuditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.completeJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveJobLineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToEXCELToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.putJobCustomerONSTOPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAnyIssuesReportedOnSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateCOMPLETEDJobDetailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.supplierContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectSupplierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectProductToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.cancelButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.btnJobLocks = new System.Windows.Forms.Button();
            this.viewJobsQUANTITIESToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wbDailyContextMenuStrip1.SuspendLayout();
            this.wbJobContextMenuStrip.SuspendLayout();
            this.supplierContextMenuStrip.SuspendLayout();
            this.productContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // jobPlannerStripMenuItem
            // 
            this.jobPlannerStripMenuItem.Name = "jobPlannerStripMenuItem";
            this.jobPlannerStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.jobPlannerStripMenuItem.Text = "Go to Job Planner";
            this.jobPlannerStripMenuItem.Click += new System.EventHandler(this.jobPlannerStripMenuItem_Click);
            // 
            // wbDailyContextMenuStrip1
            // 
            this.wbDailyContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.wbDailyContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCommenttoolStripMenuItem});
            this.wbDailyContextMenuStrip1.Name = "wbDailyContextMenuStrip1";
            this.wbDailyContextMenuStrip1.Size = new System.Drawing.Size(203, 36);
            // 
            // addCommenttoolStripMenuItem
            // 
            this.addCommenttoolStripMenuItem.Name = "addCommenttoolStripMenuItem";
            this.addCommenttoolStripMenuItem.Size = new System.Drawing.Size(202, 32);
            this.addCommenttoolStripMenuItem.Text = "Add Comment";
            this.addCommenttoolStripMenuItem.Click += new System.EventHandler(this.addCommenttoolStripMenuItem_Click);
            // 
            // weeksTabControl
            // 
            this.weeksTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.weeksTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.weeksTabControl.Location = new System.Drawing.Point(16, 17);
            this.weeksTabControl.Name = "weeksTabControl";
            this.weeksTabControl.SelectedIndex = 0;
            this.weeksTabControl.Size = new System.Drawing.Size(1654, 863);
            this.weeksTabControl.TabIndex = 7;
            this.weeksTabControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.weeksTabControl_MouseUp);
            // 
            // wbJobContextMenuStrip
            // 
            this.wbJobContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.wbJobContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spanJobMenuItem,
            this.removedExtendedJobMenuItem,
            this.jobPlannerStripMenuItem,
            this.raisePOToolStripMenuItem,
            this.jobPOsToolStripMenuItem,
            this.jobCommentsAudittoolStripMenuItem,
            this.jobDateAuditMenuItem,
            this.completeJobToolStripMenuItem,
            this.saveJobLineMenuItem,
            this.exportToEXCELToolStripMenuItem,
            this.putJobCustomerONSTOPToolStripMenuItem,
            this.viewAnyIssuesReportedOnSiteToolStripMenuItem,
            this.updateCOMPLETEDJobDetailToolStripMenuItem,
            this.viewJobsQUANTITIESToolStripMenuItem});
            this.wbJobContextMenuStrip.Name = "wbContextMenuStrip";
            this.wbJobContextMenuStrip.Size = new System.Drawing.Size(375, 485);
            this.wbJobContextMenuStrip.Text = "JOB Option(s)";
            // 
            // spanJobMenuItem
            // 
            this.spanJobMenuItem.Name = "spanJobMenuItem";
            this.spanJobMenuItem.Size = new System.Drawing.Size(374, 32);
            this.spanJobMenuItem.Text = "Extend Job To Next Week";
            this.spanJobMenuItem.Click += new System.EventHandler(this.spanJobMenuItem_Click);
            // 
            // removedExtendedJobMenuItem
            // 
            this.removedExtendedJobMenuItem.Name = "removedExtendedJobMenuItem";
            this.removedExtendedJobMenuItem.Size = new System.Drawing.Size(374, 32);
            this.removedExtendedJobMenuItem.Text = "Remove EXTENDED Job";
            this.removedExtendedJobMenuItem.Click += new System.EventHandler(this.removedExtendedJobMenuItem_Click);
            // 
            // raisePOToolStripMenuItem
            // 
            this.raisePOToolStripMenuItem.Name = "raisePOToolStripMenuItem";
            this.raisePOToolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.raisePOToolStripMenuItem.Text = "Raise PO";
            this.raisePOToolStripMenuItem.Click += new System.EventHandler(this.raisePOToolStripMenuItem_Click);
            // 
            // jobPOsToolStripMenuItem
            // 
            this.jobPOsToolStripMenuItem.Name = "jobPOsToolStripMenuItem";
            this.jobPOsToolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.jobPOsToolStripMenuItem.Text = "View PO(s) for Job";
            // 
            // jobCommentsAudittoolStripMenuItem
            // 
            this.jobCommentsAudittoolStripMenuItem.Name = "jobCommentsAudittoolStripMenuItem";
            this.jobCommentsAudittoolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.jobCommentsAudittoolStripMenuItem.Text = "Job Comments Audit";
            this.jobCommentsAudittoolStripMenuItem.Click += new System.EventHandler(this.jobCommentsAudittoolStripMenuItem_Click);
            // 
            // jobDateAuditMenuItem
            // 
            this.jobDateAuditMenuItem.Name = "jobDateAuditMenuItem";
            this.jobDateAuditMenuItem.Size = new System.Drawing.Size(374, 32);
            this.jobDateAuditMenuItem.Text = "Audit of Job Date(s) Movement";
            this.jobDateAuditMenuItem.Click += new System.EventHandler(this.jobDateAuditMenuItem_Click);
            // 
            // completeJobToolStripMenuItem
            // 
            this.completeJobToolStripMenuItem.Name = "completeJobToolStripMenuItem";
            this.completeJobToolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.completeJobToolStripMenuItem.Text = "Set as INVOICED and COMPLETE job";
            this.completeJobToolStripMenuItem.Click += new System.EventHandler(this.completeJobToolStripMenuItem_Click);
            // 
            // saveJobLineMenuItem
            // 
            this.saveJobLineMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.saveJobLineMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.saveJobLineMenuItem.Name = "saveJobLineMenuItem";
            this.saveJobLineMenuItem.Size = new System.Drawing.Size(374, 32);
            this.saveJobLineMenuItem.Text = "SAVE Job Line";
            this.saveJobLineMenuItem.Click += new System.EventHandler(this.saveJobLineMenuItem_Click);
            // 
            // exportToEXCELToolStripMenuItem
            // 
            this.exportToEXCELToolStripMenuItem.Name = "exportToEXCELToolStripMenuItem";
            this.exportToEXCELToolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.exportToEXCELToolStripMenuItem.Text = "Export to EXCEL";
            this.exportToEXCELToolStripMenuItem.Click += new System.EventHandler(this.exportToEXCELToolStripMenuItem_Click);
            // 
            // putJobCustomerONSTOPToolStripMenuItem
            // 
            this.putJobCustomerONSTOPToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.putJobCustomerONSTOPToolStripMenuItem.Name = "putJobCustomerONSTOPToolStripMenuItem";
            this.putJobCustomerONSTOPToolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.putJobCustomerONSTOPToolStripMenuItem.Text = "Put Job Customer ON STOP";
            this.putJobCustomerONSTOPToolStripMenuItem.Click += new System.EventHandler(this.putJobCustomerONSTOPToolStripMenuItem_Click);
            // 
            // viewAnyIssuesReportedOnSiteToolStripMenuItem
            // 
            this.viewAnyIssuesReportedOnSiteToolStripMenuItem.Name = "viewAnyIssuesReportedOnSiteToolStripMenuItem";
            this.viewAnyIssuesReportedOnSiteToolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.viewAnyIssuesReportedOnSiteToolStripMenuItem.Text = "View Any Issues Reported on Site";
            this.viewAnyIssuesReportedOnSiteToolStripMenuItem.Click += new System.EventHandler(this.viewAnyIssuesReportedOnSiteToolStripMenuItem_Click);
            // 
            // updateCOMPLETEDJobDetailToolStripMenuItem
            // 
            this.updateCOMPLETEDJobDetailToolStripMenuItem.Name = "updateCOMPLETEDJobDetailToolStripMenuItem";
            this.updateCOMPLETEDJobDetailToolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.updateCOMPLETEDJobDetailToolStripMenuItem.Text = "Update COMPLETED Job Detail";
            this.updateCOMPLETEDJobDetailToolStripMenuItem.Click += new System.EventHandler(this.updateCOMPLETEDJobDetailToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.BackColor = System.Drawing.Color.Yellow;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(26, 938);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(390, 28);
            this.label1.TabIndex = 8;
            this.label1.Text = "Right click yellow DAY cell to add notes";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(26, 900);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(390, 28);
            this.label2.TabIndex = 9;
            this.label2.Text = "Right click on Job No to go to JOB PLANNER";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // supplierContextMenuStrip
            // 
            this.supplierContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.supplierContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectSupplierToolStripMenuItem});
            this.supplierContextMenuStrip.Name = "wbContextMenuStrip";
            this.supplierContextMenuStrip.Size = new System.Drawing.Size(201, 36);
            this.supplierContextMenuStrip.Text = "JOB Option(s)";
            // 
            // selectSupplierToolStripMenuItem
            // 
            this.selectSupplierToolStripMenuItem.Name = "selectSupplierToolStripMenuItem";
            this.selectSupplierToolStripMenuItem.Size = new System.Drawing.Size(200, 32);
            this.selectSupplierToolStripMenuItem.Text = "Select Supplier";
            this.selectSupplierToolStripMenuItem.Click += new System.EventHandler(this.selectSupplierToolStripMenuItem_Click);
            // 
            // productContextMenuStrip
            // 
            this.productContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.productContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectProductToolStripMenuItem});
            this.productContextMenuStrip.Name = "wbContextMenuStrip";
            this.productContextMenuStrip.Size = new System.Drawing.Size(198, 36);
            this.productContextMenuStrip.Text = "Select Product";
            // 
            // selectProductToolStripMenuItem
            // 
            this.selectProductToolStripMenuItem.Name = "selectProductToolStripMenuItem";
            this.selectProductToolStripMenuItem.Size = new System.Drawing.Size(197, 32);
            this.selectProductToolStripMenuItem.Text = "Select Product";
            this.selectProductToolStripMenuItem.Click += new System.EventHandler(this.selectProductToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.BackColor = System.Drawing.Color.LightGreen;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(448, 900);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 67);
            this.label3.TabIndex = 13;
            this.label3.Text = "RAM Sent and Rdy For PO";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.BackColor = System.Drawing.Color.Cyan;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(646, 900);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 67);
            this.label4.TabIndex = 12;
            this.label4.Text = "RAM not sent";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Location = new System.Drawing.Point(0, 993);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1688, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(1252, 900);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(153, 69);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "CLOSE";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Location = new System.Drawing.Point(1125, 900);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(120, 69);
            this.refreshButton.TabIndex = 16;
            this.refreshButton.Text = "REFRESH";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // btnJobLocks
            // 
            this.btnJobLocks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJobLocks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnJobLocks.Location = new System.Drawing.Point(997, 900);
            this.btnJobLocks.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnJobLocks.Name = "btnJobLocks";
            this.btnJobLocks.Size = new System.Drawing.Size(120, 69);
            this.btnJobLocks.TabIndex = 17;
            this.btnJobLocks.Text = "LOCKED JOBS";
            this.btnJobLocks.UseVisualStyleBackColor = false;
            this.btnJobLocks.Click += new System.EventHandler(this.btnJobLocks_Click);
            // 
            // viewJobsQUANTITIESToolStripMenuItem
            // 
            this.viewJobsQUANTITIESToolStripMenuItem.Name = "viewJobsQUANTITIESToolStripMenuItem";
            this.viewJobsQUANTITIESToolStripMenuItem.Size = new System.Drawing.Size(374, 32);
            this.viewJobsQUANTITIESToolStripMenuItem.Text = "View Job\'s QUANTITIES";
            this.viewJobsQUANTITIESToolStripMenuItem.Click += new System.EventHandler(this.viewJobsQUANTITIESToolStripMenuItem_Click);
            // 
            // WhiteboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1688, 1015);
            this.Controls.Add(this.btnJobLocks);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.weeksTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WhiteboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WhiteboardForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WhiteboardForm_FormClosing);
            this.Load += new System.EventHandler(this.WhiteboardForm_Load);
            this.wbDailyContextMenuStrip1.ResumeLayout(false);
            this.wbJobContextMenuStrip.ResumeLayout(false);
            this.supplierContextMenuStrip.ResumeLayout(false);
            this.productContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem jobPlannerStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip wbDailyContextMenuStrip1;
        private System.Windows.Forms.TabControl weeksTabControl;
        private System.Windows.Forms.ContextMenuStrip wbJobContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addCommenttoolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip supplierContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectSupplierToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip productContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectProductToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem raisePOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobPOsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem completeJobToolStripMenuItem;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ToolStripMenuItem jobCommentsAudittoolStripMenuItem;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ToolStripMenuItem jobDateAuditMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spanJobMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removedExtendedJobMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveJobLineMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToEXCELToolStripMenuItem;
        private System.Windows.Forms.Button btnJobLocks;
        private System.Windows.Forms.ToolStripMenuItem putJobCustomerONSTOPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewAnyIssuesReportedOnSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateCOMPLETEDJobDetailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewJobsQUANTITIESToolStripMenuItem;
    }
}