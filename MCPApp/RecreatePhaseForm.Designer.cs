namespace MCPApp
{
    partial class RecreatePhaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecreatePhaseForm));
            this.label1 = new System.Windows.Forms.Label();
            this.phaseNoTextBox = new System.Windows.Forms.TextBox();
            this.recreateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(82, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(399, 32);
            this.label1.TabIndex = 99;
            this.label1.Text = "Enter Deleted Phase Number :";
            // 
            // phaseNoTextBox
            // 
            this.phaseNoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phaseNoTextBox.Location = new System.Drawing.Point(506, 29);
            this.phaseNoTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.phaseNoTextBox.MaxLength = 2;
            this.phaseNoTextBox.Name = "phaseNoTextBox";
            this.phaseNoTextBox.Size = new System.Drawing.Size(112, 39);
            this.phaseNoTextBox.TabIndex = 0;
            this.phaseNoTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.phaseNoTextBox_KeyPress);
            // 
            // recreateButton
            // 
            this.recreateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recreateButton.Location = new System.Drawing.Point(676, 29);
            this.recreateButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.recreateButton.Name = "recreateButton";
            this.recreateButton.Size = new System.Drawing.Size(180, 45);
            this.recreateButton.TabIndex = 100;
            this.recreateButton.Text = "RE-CREATE";
            this.recreateButton.UseVisualStyleBackColor = true;
            this.recreateButton.Click += new System.EventHandler(this.recreateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(676, 83);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(180, 58);
            this.cancelButton.TabIndex = 123;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // RecreatePhaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(964, 158);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.recreateButton);
            this.Controls.Add(this.phaseNoTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RecreatePhaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RecreatePhaseForm";
            this.Load += new System.EventHandler(this.RecreatePhaseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox phaseNoTextBox;
        private System.Windows.Forms.Button recreateButton;
        private System.Windows.Forms.Button cancelButton;
    }
}