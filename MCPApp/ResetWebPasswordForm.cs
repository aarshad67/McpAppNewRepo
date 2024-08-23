using System;
using System.Windows.Forms;
using System.Configuration;

namespace MCPApp
{
    public partial class ResetWebPasswordForm : Form
    {
        private string userID = "";
        private string userName = "";
        private string fullName = "";
        private bool confirmPwdFlag = false;
        private bool pwdConfirmedFlag = false;
        private bool stopFlag = false;
        MeltonData mcData = new MeltonData();
        Crypto c = new Crypto();

        public ResetWebPasswordForm()
        {
            InitializeComponent();
        }

        public ResetWebPasswordForm(string id,string uName,string fName)
        {
            InitializeComponent();
            userID = id;
            userName = uName;
            fullName = fName;
        }

        private void ResetWebPassword_Load(object sender, EventArgs e)
        {
            this.Text = $"Reset Password for {fullName.ToUpper()}";
            txtUserID.Text = userID;
            txtUserName.Text = userName;
            updateUserButton.Enabled = false;
            txtNewPassword.Focus();
            return;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (mcData.CheckSQLConnection() != 1)
                {
                    MessageBox.Show("There is NO database connection. Try restarting macine first. If still no joy then please contact FIRSTCALL IT support", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                confirmPwdFlag = true;
                if (confirmPwdFlag)
                {
                    lblConfirmPassword.Visible = true;
                    txtConfirmPassword.Visible = true;
                    txtConfirmPassword.Focus();
                }
                
            }
        }

        private void txtNewPassword_Enter(object sender, EventArgs e)
        {
            if (!stopFlag)
            {
                txtNewPassword.Select(0, txtNewPassword.Text.Length);
            }
            else
            {
                return;
            }
        }

        private void txtNewPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!txtNewPassword.Focused)
            {
                return;
            }
            confirmPwdFlag = true; 
            if (confirmPwdFlag)
            {
                lblConfirmPassword.Visible = true;
                txtConfirmPassword.Visible = true;
                txtConfirmPassword.Focus();
            }
            else
            {

                if (mcData.CheckSQLConnection() != 1)
                {
                    MessageBox.Show("There is NO database connection. Try restarting macine first. If still no joy then please contact FIRSTCALL IT support", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    stopFlag = true;
                    LoginForm.ActiveForm.Close();
                    return;
                }

                //validate password
                
            }
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtConfirmPassword.Text.ToUpper() != txtNewPassword.Text.ToUpper())
                {
                    updateUserButton.Enabled = false;
                    MessageBox.Show("Password Mismatch !! Please try again", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNewPassword.Focus();
                    return;
                }
                else
                {
                    updateUserButton.Enabled = true;
                }
            }
        }

        private void txtConfirmPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!txtConfirmPassword.Focused)
            {
                return;
            }
            if (txtConfirmPassword.Text.ToUpper() != txtNewPassword.Text.ToUpper())
            {
                updateUserButton.Enabled = false;
                MessageBox.Show("Password Mismatch !! Please try again", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
            }
            else
            {
                updateUserButton.Enabled = true;
            }
        }

        private void updateUserButton_Click(object sender, EventArgs e)
        {
            string encryptedPwd = c.WebEncrypt(txtConfirmPassword.Text.Trim());
            if (mcData.UpdateWebPassword(userName, encryptedPwd))
            {
                lblConfirmPassword.Visible = false;
                txtConfirmPassword.Visible = false;
                confirmPwdFlag = false;

                MessageBox.Show("Web Access Password successfully updated!!");
                this.Dispose();
                this.Close();
                return;
            }
        }

        private void btnResetUserPwd_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrWhiteSpace(txtUserName.Text)) { return; }
            if(MessageBox.Show($"Are you sure you wish to reset webapp password of user [{txtUserName.Text}] so they have to change it themselves on the web app ?","Confirm Password Reset",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string response = mcData.ResetPassword(txtUserID.Text);
                if(response == "OK")
                {
                    MessageBox.Show($"Password for user [{txtUserID.Text}] has successfully been reset. Next time they log into the web app, they will be prompted to enter a new password ");
                    this.Dispose();
                    this.Close();
                    return;
                }
            }
        }
    }
}
