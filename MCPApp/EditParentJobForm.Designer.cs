namespace MCPApp
{
    partial class EditParentJobForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditParentJobForm));
            this.label5 = new System.Windows.Forms.Label();
            this.custCombo = new System.Windows.Forms.ComboBox();
            this.siteAddressTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.custCodeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.continueButton = new System.Windows.Forms.Button();
            this.contactTelTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.contactEmailTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.siteContactTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.parentJobNoTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(52, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(292, 35);
            this.label5.TabIndex = 120;
            this.label5.Text = "Account :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // custCombo
            // 
            this.custCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custCombo.FormattingEnabled = true;
            this.custCombo.Location = new System.Drawing.Point(352, 100);
            this.custCombo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.custCombo.Name = "custCombo";
            this.custCombo.Size = new System.Drawing.Size(728, 37);
            this.custCombo.TabIndex = 106;
            this.custCombo.TabStop = false;
            this.custCombo.SelectedIndexChanged += new System.EventHandler(this.custCombo_SelectedIndexChanged);
            // 
            // siteAddressTextBox
            // 
            this.siteAddressTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteAddressTextBox.Location = new System.Drawing.Point(352, 222);
            this.siteAddressTextBox.MaxLength = 30;
            this.siteAddressTextBox.Multiline = true;
            this.siteAddressTextBox.Name = "siteAddressTextBox";
            this.siteAddressTextBox.Size = new System.Drawing.Size(728, 118);
            this.siteAddressTextBox.TabIndex = 108;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(146, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 35);
            this.label4.TabIndex = 119;
            this.label4.Text = "Site Address :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // custCodeTextBox
            // 
            this.custCodeTextBox.Enabled = false;
            this.custCodeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custCodeTextBox.Location = new System.Drawing.Point(352, 152);
            this.custCodeTextBox.MaxLength = 30;
            this.custCodeTextBox.Name = "custCodeTextBox";
            this.custCodeTextBox.Size = new System.Drawing.Size(300, 35);
            this.custCodeTextBox.TabIndex = 107;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(52, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(292, 35);
            this.label3.TabIndex = 118;
            this.label3.Text = "Customer :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(820, 508);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(262, 52);
            this.cancelButton.TabIndex = 113;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // continueButton
            // 
            this.continueButton.Location = new System.Drawing.Point(352, 508);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(298, 52);
            this.continueButton.TabIndex = 112;
            this.continueButton.Text = "Update";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // contactTelTextBox
            // 
            this.contactTelTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactTelTextBox.Location = new System.Drawing.Point(352, 394);
            this.contactTelTextBox.Name = "contactTelTextBox";
            this.contactTelTextBox.Size = new System.Drawing.Size(492, 35);
            this.contactTelTextBox.TabIndex = 110;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(146, 392);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(200, 35);
            this.label7.TabIndex = 117;
            this.label7.Text = "Contact Tel :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contactEmailTextBox
            // 
            this.contactEmailTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactEmailTextBox.Location = new System.Drawing.Point(352, 440);
            this.contactEmailTextBox.Name = "contactEmailTextBox";
            this.contactEmailTextBox.Size = new System.Drawing.Size(492, 35);
            this.contactEmailTextBox.TabIndex = 111;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(146, 438);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 35);
            this.label6.TabIndex = 116;
            this.label6.Text = "Contact Email :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // siteContactTextBox
            // 
            this.siteContactTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteContactTextBox.Location = new System.Drawing.Point(352, 348);
            this.siteContactTextBox.MaxLength = 30;
            this.siteContactTextBox.Name = "siteContactTextBox";
            this.siteContactTextBox.Size = new System.Drawing.Size(493, 35);
            this.siteContactTextBox.TabIndex = 109;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(146, 348);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 35);
            this.label2.TabIndex = 115;
            this.label2.Text = "Site Contact :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // parentJobNoTextBox
            // 
            this.parentJobNoTextBox.BackColor = System.Drawing.Color.Yellow;
            this.parentJobNoTextBox.Enabled = false;
            this.parentJobNoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parentJobNoTextBox.Location = new System.Drawing.Point(352, 32);
            this.parentJobNoTextBox.MaxLength = 10;
            this.parentJobNoTextBox.Name = "parentJobNoTextBox";
            this.parentJobNoTextBox.Size = new System.Drawing.Size(298, 35);
            this.parentJobNoTextBox.TabIndex = 105;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 35);
            this.label1.TabIndex = 114;
            this.label1.Text = "Parent Job No.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EditParentJobForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1142, 615);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.custCombo);
            this.Controls.Add(this.siteAddressTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.custCodeTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.contactTelTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.contactEmailTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.siteContactTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.parentJobNoTextBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "EditParentJobForm";
            this.Text = "EditParentJobForm";
            this.Load += new System.EventHandler(this.EditParentJobForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox custCombo;
        private System.Windows.Forms.TextBox siteAddressTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox custCodeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.TextBox contactTelTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox contactEmailTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox siteContactTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox parentJobNoTextBox;
        private System.Windows.Forms.Label label1;
    }
}