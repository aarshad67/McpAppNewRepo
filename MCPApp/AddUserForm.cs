using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCPApp
{
    public partial class AddUserForm : Form
    {
        private string userIDUpdated = "";
        public string UserIDUpdated
        {
            get
            {
                return userIDUpdated;
            }
            set
            {
                userIDUpdated = value;
            }
        }

        MeltonData mcData = new MeltonData();
        private DataTable userDT = new DataTable();
        private string userID = "";
        private string username = "";
        private string fullName = "";
        private string emailAddress = "";
        private string telNo = "";
        private string managerFlag = "";
        private string guestFlag = "";
        private string ipAddress = "";
        Logger logger = new Logger();
        

        public AddUserForm()
        {
            InitializeComponent();
        }

        public AddUserForm(string userIDKey)
        {
            InitializeComponent();
            userID = userIDKey;
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            this.Text = "Edit User";
            if (String.IsNullOrWhiteSpace(userID))
            {
                EmptyDetails();
                userIDTextBox.ReadOnly = false;
            }
            else
            {
                FillUserDetails(userID);
                userIDTextBox.ReadOnly = true;
            }



            userIDTextBox.ReadOnly = false;
            userIDTextBox.Focus();
        }

        private void updateUserButton_Click(object sender, EventArgs e)
        {
            MeltonData mcData = new MeltonData();
            userID = userIDTextBox.Text;
            username = userNameTextBox.Text;
            fullName = fullNameTextBox.Text;
            emailAddress = emailAddressTextBox.Text;
            telNo = telNoTextBox.Text;
            managerFlag = managerCheckBox.Checked ? "Y" : "N";
            guestFlag = guestCheckBox.Checked ? "Y" : "N";
            ipAddress = ipTextBox.Text;

            if (mcData.IsUserExists(userIDTextBox.Text.ToUpper()))
            {
                string err = mcData.UpdateMCPUser(userID, username, fullName, emailAddress, telNo, managerFlag, guestFlag,ipAddress);
                if (err == "OK")
                {
                    MessageBox.Show(String.Format("User [{0} - {1}] updated successfully", userID, fullName));
                    userIDUpdated = userID; 
                    this.Close();
                }
                else
                {
                    MessageBox.Show(String.Format("Error updating  [{0} - {1}] error : {2}", userID, fullName, err));
                    userIDUpdated = "ERR";
                    this.Close();
                }
            }
            else
            {
                string err = mcData.CreateMCPUser(userID, username, fullName, emailAddress, telNo, managerFlag, guestFlag,ipAddress);
                if (err == "OK")
                {
                    MessageBox.Show(String.Format("User [{0} - {1}] created successfully", userID, fullName));
                    userIDUpdated = userID;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(String.Format("Error created [{0} - {1}] error : {2}", userID, fullName, err));
                    userIDUpdated = "ERR";
                    this.Close();
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void fullNameTextBox_TextChanged(object sender, EventArgs e)
        {
            return;
        }

        private void removeUserButton_Click(object sender, EventArgs e)
        {
            
        }

        private void EmptyDetails()
        {
            userIDTextBox.Text = String.Empty;
            userNameTextBox.Text = String.Empty;
            fullNameTextBox.Text = String.Empty;
            emailAddressTextBox.Text = String.Empty;
            telNoTextBox.Text = String.Empty;
            managerCheckBox.Checked = false;
            guestCheckBox.Checked = false;
            ipTextBox.Text = String.Empty;
        }

        private void FillUserDetails(string userID)
        {
            EmptyDetails();
            try
            {
                DataTable dt = mcData.GetUserByUserID(userID);
                foreach (DataRow dr in dt.Rows)
                {
                    userIDTextBox.Text = dr["userID"].ToString();
                    userNameTextBox.Text = dr["username"].ToString();
                    fullNameTextBox.Text = dr["fullName"].ToString();
                    emailAddressTextBox.Text = dr["emailAddress"].ToString();
                    telNoTextBox.Text = dr["telNo"].ToString();
                    managerCheckBox.Checked = dr["managerFlag"].ToString() == "Y" ? true : false;
                    guestCheckBox.Checked = dr["guestFlag"].ToString() == "Y" ? true : false;
                    ipTextBox.Text = dr["ipAddress"].ToString();
                }
            }
            catch (Exception ex)
            {
                string msg = String.Format("FillUserDetails() Error : {0}", ex.Message.ToString());
                string audit = mcData.CreateErrorAudit("AddUserForm.cs", String.Format("FillUserDetails(string {0})", userID), msg);
                logger.LogLine(msg);
                return;
            }
            
        }

        private void ipTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            string userID = userIDTextBox.Text.Trim();
            string response = mcData.ResetPassword(userID);
            if (response == "OK")
            {
                MessageBox.Show($"Password for User [{userID}] has successfully been reset. Log out and then log back in agan where user will be prompted to enter new password");
                this.Dispose();
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show(response);
                this.Dispose();
                this.Close();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetWebPasswordForm resetForm = new ResetWebPasswordForm(userIDTextBox.Text.Trim(),userNameTextBox.Text.Trim(),fullNameTextBox.Text.Trim());
            resetForm.ShowDialog();
            return;

        }
    }
}
