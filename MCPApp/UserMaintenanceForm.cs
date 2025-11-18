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
    public partial class UserMaintenanceForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        public UserMaintenanceForm()
        {
            InitializeComponent();
        }

        private void UserMaintenanceForm_Load(object sender, EventArgs e)
        {
            this.Text = "User Maintanance(DOUBLE CLICK user to update or remove)";
            BuildUsersDGV();
            PopulateDGV();
        }

        private void BuildUsersDGV()
        {
            try
            {


                usersDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn userIDBoxColumn = new DataGridViewTextBoxColumn();
                userIDBoxColumn.HeaderText = "User ID";
                userIDBoxColumn.Width = 80;
                userIDBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                userIDBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(userIDBoxColumn);

                //1
                DataGridViewTextBoxColumn userNameTextBoxColumn = new DataGridViewTextBoxColumn();
                userNameTextBoxColumn.HeaderText = "User Name";
                userNameTextBoxColumn.Width = 100;
                userNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                userNameTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(userNameTextBoxColumn);

                //2
                DataGridViewTextBoxColumn fullNameTextBoxColumn = new DataGridViewTextBoxColumn();
                fullNameTextBoxColumn.HeaderText = "Full Name";
                fullNameTextBoxColumn.Width = 150;
                fullNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                fullNameTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(fullNameTextBoxColumn);

                //3
                DataGridViewTextBoxColumn emailAddressTextBoxColumn = new DataGridViewTextBoxColumn();
                emailAddressTextBoxColumn.HeaderText = "Email Address";
                emailAddressTextBoxColumn.Width = 150;
                emailAddressTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                emailAddressTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(emailAddressTextBoxColumn);

                //4
                DataGridViewTextBoxColumn validUserTextBoxColumn = new DataGridViewTextBoxColumn();
                validUserTextBoxColumn.HeaderText = "Valid";
                validUserTextBoxColumn.Width = 55;
                validUserTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                validUserTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(validUserTextBoxColumn);

                //5
                DataGridViewTextBoxColumn telNoTextBoxColumn = new DataGridViewTextBoxColumn();
                telNoTextBoxColumn.HeaderText = "Tel No";
                telNoTextBoxColumn.Width = 100;
                telNoTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                telNoTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(telNoTextBoxColumn);

                //6
                DataGridViewTextBoxColumn contactTelTextBoxColumn = new DataGridViewTextBoxColumn();
                contactTelTextBoxColumn.HeaderText = "Last Logged";
                contactTelTextBoxColumn.Width = 180;
                contactTelTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                contactTelTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(contactTelTextBoxColumn);

                //7
                DataGridViewTextBoxColumn rptFlagTextBoxColumn = new DataGridViewTextBoxColumn();
                rptFlagTextBoxColumn.HeaderText = "Rpt Flag";
                rptFlagTextBoxColumn.Width = 80;
                rptFlagTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                rptFlagTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(rptFlagTextBoxColumn);

                //8
                DataGridViewTextBoxColumn lastUpdatedTextBoxColumn = new DataGridViewTextBoxColumn();
                lastUpdatedTextBoxColumn.HeaderText = "Last Updated";
                lastUpdatedTextBoxColumn.Width = 70;
                lastUpdatedTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                lastUpdatedTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(lastUpdatedTextBoxColumn);

                //9
                DataGridViewTextBoxColumn updateFlagTextBoxColumn = new DataGridViewTextBoxColumn();
                updateFlagTextBoxColumn.HeaderText = "Update Reqd";
                updateFlagTextBoxColumn.Width = 80;
                updateFlagTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                updateFlagTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(updateFlagTextBoxColumn);

                //10
                DataGridViewTextBoxColumn managerFlagTextBoxColumn = new DataGridViewTextBoxColumn();
                managerFlagTextBoxColumn.HeaderText = "Manager";
                managerFlagTextBoxColumn.Width = 80;
                managerFlagTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                managerFlagTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(managerFlagTextBoxColumn);

                //11
                DataGridViewTextBoxColumn guestTextBoxColumn = new DataGridViewTextBoxColumn();
                guestTextBoxColumn.HeaderText = "Guest";
                guestTextBoxColumn.Width = 80;
                guestTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                guestTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(guestTextBoxColumn);

                //12
                DataGridViewTextBoxColumn ipTextBoxColumn = new DataGridViewTextBoxColumn();
                ipTextBoxColumn.HeaderText = "IP Addr";
                ipTextBoxColumn.Width = 120;
                ipTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                ipTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(ipTextBoxColumn);

                //13
                DataGridViewTextBoxColumn reqDateFlagTextBoxColumn = new DataGridViewTextBoxColumn();
                reqDateFlagTextBoxColumn.HeaderText = "Move SF (On Shop) job Date";
                reqDateFlagTextBoxColumn.Width = 250;
                reqDateFlagTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                reqDateFlagTextBoxColumn.ReadOnly = true;
                usersDGV.Columns.Add(reqDateFlagTextBoxColumn);


                usersDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                usersDGV.EnableHeadersVisualStyles = false;
                usersDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                usersDGV.Columns[1].DefaultCellStyle.BackColor = Color.Yellow;
                usersDGV.Columns[2].DefaultCellStyle.BackColor = Color.Yellow;

                return;

            }
            catch (Exception ex)
            {
                string msg = String.Format("BuildUsersDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("UserMaintenanceForm.cs", "BuildUsersDGV()", ex.Message);
                return;
            }





        }

        private void GetUserCurrentRow(string userID)
        {
            if (!this.Text.Contains("Portal")) { return; }
            int index = -1;
            try
            {
                for (int i = 0; i < usersDGV.Rows.Count; i++)
                {
                    if (usersDGV.Rows[i].Cells[0].Value.ToString() == userID)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    usersDGV.CurrentCell = usersDGV.Rows[index].Cells[0];
                }
            }
            catch (Exception ex)
            {
                string msg = String.Format("GetUserCurrentRow() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("UserMaintenanceForm.cs", String.Format("GetUserCurrentRow({0})",userID), ex.Message);
                return;
            }
        }

        private void PopulateDGV()
        {

            int row = 0;
            try
            {
                usersDGV.Rows.Clear();
                DataTable dt = mcData.GetAllUsers();
                foreach (DataRow dr in dt.Rows)
                {
                    usersDGV.Rows.Add();
                    usersDGV[0, row].Value = dr["userID"].ToString();
                    usersDGV[1, row].Value = dr["userName"].ToString();
                    usersDGV[2, row].Value = dr["fullName"].ToString();
                    usersDGV[3, row].Value = dr["emailAddress"].ToString();
                    usersDGV[4, row].Value = dr["validUserFlag"].ToString();
                    usersDGV[5, row].Value = dr["telNo"].ToString();
                    usersDGV[6, row].Value = dr["lastLoggedOn"].ToString();
                    usersDGV[7, row].Value = dr["reportingFlag"].ToString();
                    usersDGV[8, row].Value = dr["mcpLastUpdated"].ToString();
                    usersDGV[9, row].Value = dr["updateRequired"].ToString();
                    usersDGV[10, row].Value = dr["managerFlag"].ToString();
                    usersDGV[11, row].Value = dr["guestFlag"].ToString();
                    usersDGV[12, row].Value = dr["ipAddress"].ToString();
                    usersDGV[13, row++].Value = dr["reqDateFlag"].ToString();
                }
                
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("UserMaintenanceForm.cs", "PopulateDGV()", ex.Message);
                return;
            }
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddUserForm userForm = new AddUserForm();

                userForm.ShowDialog();
                PopulateDGV();
                if (userForm.UserIDUpdated != "ERR" && !String.IsNullOrWhiteSpace(userForm.UserIDUpdated)) { GetUserCurrentRow(userForm.UserIDUpdated); }
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("addUserButton_Click() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("UserMaintenanceForm.cs", "addUserButton_Click()", ex.Message);
                return;
            }
            
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void usersDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string userID = usersDGV[0, e.RowIndex].Value.ToString();
                AddUserForm addForm = new AddUserForm(userID);
                addForm.ShowDialog();
                PopulateDGV();
                if (addForm.UserIDUpdated != "ERR" && addForm.UserIDUpdated != "DEL" && !String.IsNullOrWhiteSpace(addForm.UserIDUpdated)) { GetUserCurrentRow(addForm.UserIDUpdated); } else { PopulateDGV(); }
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("usersDGV_CellDoubleClick() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("UserMaintenanceForm.cs", "usersDGV_CellDoubleClick()", ex.Message);
                return;
            }
            
        }
    }
}
