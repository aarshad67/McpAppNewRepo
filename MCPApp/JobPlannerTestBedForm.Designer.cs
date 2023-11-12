
namespace MCPApp
{
    partial class JobPlannerTestBedForm
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
            this.jobDGV = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.jobDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // jobDGV
            // 
            this.jobDGV.AllowUserToAddRows = false;
            this.jobDGV.AllowUserToDeleteRows = false;
            this.jobDGV.AllowUserToResizeRows = false;
            this.jobDGV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jobDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobDGV.Location = new System.Drawing.Point(13, 135);
            this.jobDGV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jobDGV.Name = "jobDGV";
            this.jobDGV.RowHeadersWidth = 62;
            this.jobDGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.jobDGV.Size = new System.Drawing.Size(2134, 845);
            this.jobDGV.TabIndex = 122;
            this.jobDGV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.jobDGV_CellFormatting);
            this.jobDGV.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.jobDGV_DataError);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 66);
            this.button1.TabIndex = 123;
            this.button1.Text = "CLOSE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // JobPlannerTestBedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2160, 994);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.jobDGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "JobPlannerTestBedForm";
            this.Text = "JobPlannerTestBedForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.JobPlannerTestBedForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.jobDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView jobDGV;
        private System.Windows.Forms.Button button1;
    }
}