
namespace MCPApp
{
    partial class JobsMissingDataForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.totalSlabM2TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.jobDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // jobDGV
            // 
            this.jobDGV.AllowUserToAddRows = false;
            this.jobDGV.AllowUserToDeleteRows = false;
            this.jobDGV.AllowUserToResizeRows = false;
            this.jobDGV.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.jobDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobDGV.Location = new System.Drawing.Point(13, 66);
            this.jobDGV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jobDGV.Name = "jobDGV";
            this.jobDGV.RowHeadersWidth = 62;
            this.jobDGV.Size = new System.Drawing.Size(1603, 659);
            this.jobDGV.TabIndex = 122;
            this.jobDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobDGV_CellDoubleClick);
            this.jobDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobDGV_CellEndEdit);
            this.jobDGV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobDGV_CellValueChanged);
            this.jobDGV.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.jobDGV_EditingControlShowing);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(1334, 733);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(282, 59);
            this.cancelButton.TabIndex = 153;
            this.cancelButton.Text = "CLOSE";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // totalSlabM2TextBox
            // 
            this.totalSlabM2TextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.totalSlabM2TextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.totalSlabM2TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalSlabM2TextBox.ForeColor = System.Drawing.Color.Red;
            this.totalSlabM2TextBox.Location = new System.Drawing.Point(1258, 17);
            this.totalSlabM2TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.totalSlabM2TextBox.Name = "totalSlabM2TextBox";
            this.totalSlabM2TextBox.Size = new System.Drawing.Size(215, 39);
            this.totalSlabM2TextBox.TabIndex = 156;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1237, 39);
            this.label2.TabIndex = 155;
            this.label2.Text = "Total number of COMPLETED jobs with missing";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // JobsMissingDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1645, 804);
            this.Controls.Add(this.totalSlabM2TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.jobDGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "JobsMissingDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JobsMissingDataForm";
            this.Load += new System.EventHandler(this.JobsMissingDataForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.jobDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView jobDGV;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox totalSlabM2TextBox;
        private System.Windows.Forms.Label label2;
    }
}