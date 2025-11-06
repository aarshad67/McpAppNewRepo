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
    public partial class LockedJobsForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        private int rowIndex, colIndex = 0;
        private bool rowReadOnly = false;
        private string lockType = "";

        public LockedJobsForm()
        {
            InitializeComponent();
        }

        public LockedJobsForm(string source)
        {
            InitializeComponent();
            if(!String.IsNullOrWhiteSpace(source))
            {
                rowReadOnly = true;
                lockType = source;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void BuildDGV()
        {
            try
            {
                dgv.Columns.Clear();
                //0
                DataGridViewTextBoxColumn lockTypeColumn = new DataGridViewTextBoxColumn();
                lockTypeColumn.HeaderText = "Lock Type";
                lockTypeColumn.Width = 100;
                lockTypeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                lockTypeColumn.ReadOnly = true;
                dgv.Columns.Add(lockTypeColumn);

                //1
                DataGridViewTextBoxColumn jobNoColumn = new DataGridViewTextBoxColumn();
                jobNoColumn.HeaderText = "JobNo";
                jobNoColumn.Width = 100;
                jobNoColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoColumn.ReadOnly = true;
                dgv.Columns.Add(jobNoColumn);

                //2
                DataGridViewTextBoxColumn custCodeColumn = new DataGridViewTextBoxColumn();
                custCodeColumn.HeaderText = "Cust Code";
                custCodeColumn.Width = 120;
                custCodeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custCodeColumn.ReadOnly = true;
                dgv.Columns.Add(custCodeColumn);

                //3
                DataGridViewTextBoxColumn siteAddrColumn = new DataGridViewTextBoxColumn();
                siteAddrColumn.HeaderText = "Site Address";
                siteAddrColumn.Width = 200;
                siteAddrColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                siteAddrColumn.ReadOnly = true;
                dgv.Columns.Add(siteAddrColumn);

                //4
                DataGridViewTextBoxColumn userColumn = new DataGridViewTextBoxColumn();
                userColumn.HeaderText = "Locked By";
                userColumn.Width = 150;
                userColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                userColumn.ReadOnly = true;
                dgv.Columns.Add(userColumn);

                //5
                DataGridViewTextBoxColumn datetimeColumn = new DataGridViewTextBoxColumn();
                datetimeColumn.HeaderText = " At";
                datetimeColumn.Width = 150;
                datetimeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                datetimeColumn.ReadOnly = true;
                dgv.Columns.Add(datetimeColumn);

                //6
                DataGridViewTextBoxColumn triggerColumn = new DataGridViewTextBoxColumn();
                triggerColumn.HeaderText = " Trigger Event";
                triggerColumn.Width = 250;
                triggerColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                triggerColumn.ReadOnly = true;
                dgv.Columns.Add(triggerColumn);

                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.EnableHeadersVisualStyles = false;
                dgv.AllowUserToAddRows = false;



            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite;
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("LockedJobsForm.cs", "BuildDGV()", msg);
            }
        }

        private void PopulateDGV()
        {
            MeltonData mcData = new MeltonData();
            int row = 0;
            string fullName,custCode,siteAddress = "";
            

            try
            {
                dgv.Rows.Clear();
                DataTable dt = String.IsNullOrWhiteSpace(lockType) ? mcData.GetJobLocksDT() : mcData.GetJobLocksDT(lockType);
                if (dt.Rows.Count < 1) { return; }
                foreach (DataRow dr in dt.Rows)
                {
                    fullName = mcData.GetUserFullNameFromUserID(dr["lockUser"].ToString());
                    mcData.GetCusomerCodeAndSiteAddrFromWBJob(dr["jobNo"].ToString(),out custCode,out siteAddress);
                    dgv.Rows.Add();
                    dgv[0, row].Value = dr["jobLockType"].ToString();
                    dgv[1, row].Value = dr["jobNo"].ToString();
                    dgv[2, row].Value = custCode;
                    dgv[3, row].Value = siteAddress;
                    dgv[4, row].Value = fullName;
                    dgv[5, row].Value = dr["lockDateTime"].ToString();
                    dgv[6, row++].Value = dr["lockTrigger"].ToString();
                }
                
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("LockedJobsForm.cs", "PopulateDGV()", msg);
                return;

            }


        }

        private void LockedJobsForm_Load(object sender, EventArgs e)
        {
            this.Text = "Current Locked Jobs";

            if(!rowReadOnly)
            {
                label1.Text = "To unlock a job, simply select the job row then RIGHT-CLICK on the job row and click UNLOCK JOB option to unlock the job";
                label2.Text = "NOTE - If there are multiple locks showing for the same job number, unlocking one of the instances wil unlock all the instances of that job";
            }
            else
            {
                label1.Text = "";
                label2.Text = "";
            }
            this.Cursor = Cursors.WaitCursor;
            BuildDGV();
            PopulateDGV();
            this.Cursor = Cursors.Default;
        }

        private void dgv_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            if (!dgv.Focused) { return; }
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
            {
               // this.dgv.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                //this.dgv.CurrentCell = this.dgv.Rows[e.RowIndex].Cells[0];
                this.contextMenuStrip1.Show(this.dgv, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
                return;

            }
        }

        private void toolStripMenuItemUnlock_Click(object sender, EventArgs e)
        {
            string lockType = dgv.Rows[rowIndex].Cells[0].Value.ToString();
            string jobNo = dgv.Rows[rowIndex].Cells[1].Value.ToString();
            if(String.IsNullOrWhiteSpace(lockType) || String.IsNullOrEmpty(jobNo)) {  return; }
            
            if(MessageBox.Show($"Are you sure you wish to the REMOVE [{lockType}] lock for job {jobNo}?","Remove Job Lock Confirmation",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int numJobLockDeleted = mcData.DeleteJobLocks(lockType, jobNo);
                if (numJobLockDeleted > 0)
                {
                    MessageBox.Show($"Job No. {jobNo} lock successfully removed");
                    dgv.Rows.Clear();
                    PopulateDGV();
                    return;
                }
                else
                {
                    MessageBox.Show($"Failed to remove lock(s) on Job No. {jobNo}");
                    return;
                }
            }

            return;
        }
    }
}
