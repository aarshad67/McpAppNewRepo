using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace MCPApp
{
    public partial class LoginForm : Form
    {
        Crypto c = new Crypto(Crypto.CryptoTypes.encTypeTripleDES);
        MeltonData mcData = new MeltonData();
        private bool stopFlag = false;
        private bool confirmPwdFlag = false;
        public bool openMainForm = false;
        public string mcpUserID = "";
        public string mcpFullName = "";
        private string userIDUpdated = "";
        

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.Width = 790;
            this.Text = mcData.GetConnectionString().Contains("TestMeltonConcreteDB") ? "Welcome to Melton Concrete Portal (MCP) *** TEST VERSION ***" : "Welcome to Melton Concrete Portal (MCP)";
            UserFullNameLabel.Text = "";
            ConfirmPasswordLabel.Visible = false;
            ConfirmPasswordTextBox.Visible = false;
            button1.Visible = false;
            UserIDTextBox.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddUserForm userForm = new AddUserForm();

            userForm.ShowDialog();
        }

        private void UserIDTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string pwd = "";
            if (!UserIDTextBox.Focused)
            {
                return;
            }

            if (e.KeyChar == (char)13)
            {
                
                if (mcData.CheckSQLConnection() != 1)
                {
                    MessageBox.Show("There is NO database connection. Try restarting macine first. If still no joy then please contact FIRSTCALL IT support", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    stopFlag = true;
                    LoginForm.ActiveForm.Close();
                    return;
                }

                if (!mcData.UserNameExists(UserIDTextBox.Text))
                {
                    MessageBox.Show("You are not a valid user. Please contact IT support", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UserIDTextBox.Focus();
                    UserIDTextBox.SelectAll();
                    stopFlag = true;
                    LoginForm.ActiveForm.Close();
                    return;

                }
                else
                {
                    UserFullNameLabel.Text = mcData.GetUserFullNameFromUserName(UserIDTextBox.Text);
                    pwd = mcData.GetPassword(UserIDTextBox.Text);
                    if (String.IsNullOrWhiteSpace(pwd))
                    {
                        MessageBox.Show(String.Format("Welcome {0}, either you are logging in for the first time or your password has been reset. Please enter your new password", UserFullNameLabel.Text.ToUpper()));
                        confirmPwdFlag = true;
                        PasswordTextBox.Focus();
                    }
                    else
                    {
                        PasswordTextBox.Focus();
                        e.Handled = true;
                    }
                }
            }
        }

        private void UserIDTextBox_Validated(object sender, EventArgs e)
        {
            //string pwd = "";
            if (!UserIDTextBox.Focused)
            {
                return;
            }

            if (mcData.CheckSQLConnection() != 1)
            {
                MessageBox.Show("There is NO database connection. Try restarting macine first. If still no joy then please contact FIRSTCALL IT support", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                stopFlag = true;
                LoginForm.ActiveForm.Close();
                return;
            }

            if (!mcData.UserNameExists(UserIDTextBox.Text))
            {
                MessageBox.Show("You are not a valid user. Please contact IT support", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UserIDTextBox.Focus();
                UserIDTextBox.SelectAll();
                stopFlag = true;
                LoginForm.ActiveForm.Close();
                return;

            }
            else
            {
                UserFullNameLabel.Text = mcData.GetUserFullNameFromUserName(UserIDTextBox.Text);
                //pwd = mcData.GetPassword(UserIDTextBox.Text);
                if (String.IsNullOrWhiteSpace(mcData.GetPassword(UserIDTextBox.Text)))
                {
                    MessageBox.Show(String.Format("Welcome {0}, either you are logging in for the first time or your password has been reset. Please enter your new password", UserFullNameLabel.Text.ToUpper()));
                    confirmPwdFlag = true;
                    PasswordTextBox.Focus();
                }
                else
                {
                    PasswordTextBox.Focus();
                }
            }


        }

        private void PasswordTextBox_Enter(object sender, EventArgs e)
        {
            if (!stopFlag)
            {
                PasswordTextBox.Select(0, PasswordTextBox.Text.Length);
            }
            else
            {
                LoginForm.ActiveForm.Close();
            }
        }

        private void PasswordTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!PasswordTextBox.Focused)
            {
                return;
            }

            if (confirmPwdFlag)
            {
                ConfirmPasswordLabel.Visible = true;
                ConfirmPasswordTextBox.Visible = true;
                ConfirmPasswordTextBox.Focus();
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
                string decryptedPwd = c.Decrypt(mcData.GetPassword(UserIDTextBox.Text));
                if (PasswordTextBox.Text == decryptedPwd)
                {
                    mcpFullName = mcData.GetUserFullNameFromUserName(UserIDTextBox.Text);
                    mcpUserID = mcData.GetUserIDFromUserName(UserIDTextBox.Text);
                    openMainForm = true;
                    //MessageBox.Show(ConfigurationManager.AppSettings["LoggedInUser"] + " - " + ConfigurationManager.AppSettings["LoggedInUserFullName"]);
                    this.Close();
                }
            }
        }

        private void PasswordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (mcData.CheckSQLConnection() != 1)
                {
                    MessageBox.Show("There is NO database connection. Try restarting macine first. If still no joy then please contact FIRSTCALL IT support", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    stopFlag = true;
                    LoginForm.ActiveForm.Close();
                    return;
                }


                if (confirmPwdFlag)
                {
                    ConfirmPasswordLabel.Visible = true;
                    ConfirmPasswordTextBox.Visible = true;
                    ConfirmPasswordTextBox.Focus();
                }
                else
                {
                    //validate password
                    //CharconSecurity security = new CharconSecurity();
                    string decryptedPwd = c.Decrypt(mcData.GetPassword(UserIDTextBox.Text));
                    
                    if (PasswordTextBox.Text == decryptedPwd)
                    {
                        openMainForm = true;
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        //config.GetSection(
                        string configPath = Application.ExecutablePath.ToString();
                        config.AppSettings.Settings.Remove("LoggedInUser");
                        config.AppSettings.Settings.Add("LoggedInUser", mcData.GetUserIDFromUserName(UserIDTextBox.Text));
                        config.AppSettings.Settings.Remove("LoggedInUserFullName");
                        config.AppSettings.Settings.Add("LoggedInUserFullName", mcData.GetUserFullNameFromUserName(UserIDTextBox.Text));
                        


                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                        //MessageBox.Show(ConfigurationManager.AppSettings["LoggedInUser"] + " - " + ConfigurationManager.AppSettings["LoggedInUserFullName"]);
                        this.Close();
                        //MessageBox.Show("Welcome to the new .NET verion of the Q2O");
                    }
                    else
                    {
                        MessageBox.Show("Invalid Password", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Focus();
                        PasswordTextBox.Select(0, PasswordTextBox.Text.Length);
                        return;

                    }
                }
            }
        }

        private void ConfirmPasswordTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!ConfirmPasswordTextBox.Focused)
            {
                return;
            }
            if (ConfirmPasswordTextBox.Text.ToUpper() != PasswordTextBox.Text.ToUpper())
            {
                MessageBox.Show("Password Mismatch !! Please try again", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PasswordTextBox.Focus();
            }
            else
            {
                //write password to DB
                //CharconSecurity security = new CharconSecurity();
                string encrypPwd = c.Encrypt(ConfirmPasswordTextBox.Text);// security.HashSHA1(ConfirmPasswordTextBox.Text);
                if (mcData.UpdatePassword(UserIDTextBox.Text, encrypPwd))
                {
                    ConfirmPasswordLabel.Visible = false;
                    ConfirmPasswordTextBox.Visible = false;
                    confirmPwdFlag = false;
                    MessageBox.Show("Please login with your new password");
                    PasswordTextBox.Text = String.Empty;
                    PasswordTextBox.Focus();
                }
            }
        }

        private void ConfirmPasswordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (ConfirmPasswordTextBox.Text.ToUpper() != PasswordTextBox.Text.ToUpper())
                {
                    MessageBox.Show("Password Mismatch !! Please try again", "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PasswordTextBox.Focus();
                }
                else
                {
                    //write password to DB
                    
                    if (mcData.UpdatePassword(UserIDTextBox.Text, ConfirmPasswordTextBox.Text))
                    {
                        ConfirmPasswordLabel.Visible = false;
                        ConfirmPasswordTextBox.Visible = false;
                        confirmPwdFlag = false;
                        MessageBox.Show("Please login with your new password");
                        PasswordTextBox.Focus();
                    }
                }
            }
        }

        private void CancelPhaseButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
            Application.Exit();
        }

        private void UserIDTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    Parent.SelectNextControl(UserIDTextBox, true, true, true, true);
            //    e.Handled = e.SuppressKeyPress = true;
            //}
        }
    }
}
