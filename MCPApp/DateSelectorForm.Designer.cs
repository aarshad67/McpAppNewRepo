namespace MCPApp
{
    partial class DateSelectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DateSelectorForm));
            this.mcCalendar = new System.Windows.Forms.MonthCalendar();
            this.cancelButton = new System.Windows.Forms.Button();
            this.selectBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mcCalendar
            // 
            this.mcCalendar.BackColor = System.Drawing.Color.White;
            this.mcCalendar.CalendarDimensions = new System.Drawing.Size(4, 3);
            this.mcCalendar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mcCalendar.Location = new System.Drawing.Point(10, 4);
            this.mcCalendar.Margin = new System.Windows.Forms.Padding(14);
            this.mcCalendar.Name = "mcCalendar";
            this.mcCalendar.ShowWeekNumbers = true;
            this.mcCalendar.TabIndex = 1;
            this.mcCalendar.TrailingForeColor = System.Drawing.Color.Gray;
            this.mcCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.mcCalendar_DateChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(29, 729);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(262, 51);
            this.cancelButton.TabIndex = 122;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // selectBtn
            // 
            this.selectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectBtn.Location = new System.Drawing.Point(1070, 725);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(298, 52);
            this.selectBtn.TabIndex = 123;
            this.selectBtn.Text = "SELECT DATE";
            this.selectBtn.UseVisualStyleBackColor = true;
            this.selectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // DateSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1402, 810);
            this.Controls.Add(this.selectBtn);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.mcCalendar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DateSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DateSelectorForm";
            this.Load += new System.EventHandler(this.DateSelectorForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MonthCalendar mcCalendar;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button selectBtn;
    }
}