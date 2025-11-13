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
    public partial class SearchJobForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        private int rowIndex = 0;
        private int colIndex = 0;

        public SearchJobForm()
        {
            InitializeComponent();
        }

        private void SearchJobForm_Load(object sender, EventArgs e)
        {
            this.Text = "Search for Parent Jobs";
            BuildDGV();
            jobNoKeyTextBox.Focus();
        }

        private void BuildDGV()
        {
            try
            {

                jobDGV.Columns.Clear();

                //0
                DataGridViewTextBoxColumn parentJobBoxColumn = new DataGridViewTextBoxColumn();
                parentJobBoxColumn.HeaderText = "Parent Job";
                parentJobBoxColumn.Width = 60;
                parentJobBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                parentJobBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(parentJobBoxColumn);

                //1
                DataGridViewTextBoxColumn custCodeTextBoxColumn = new DataGridViewTextBoxColumn();
                custCodeTextBoxColumn.HeaderText = "Cust Code";
                custCodeTextBoxColumn.Width = 80;
                custCodeTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custCodeTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(custCodeTextBoxColumn);

                //2
                DataGridViewTextBoxColumn siteAddressTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                siteAddressTextBoxColumn.HeaderText = "Site Address";
                siteAddressTextBoxColumn.Width = 350;
                siteAddressTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(siteAddressTextBoxColumn);

                //3
                DataGridViewTextBoxColumn contactTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                contactTextBoxColumn.HeaderText = "Site Contact";
                contactTextBoxColumn.Width = 120;
                contactTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(contactTextBoxColumn);

                //4
                DataGridViewTextBoxColumn contactTelTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                contactTelTextBoxColumn.HeaderText = "Site Contact Tel";
                contactTelTextBoxColumn.Width = 100;
                contactTelTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(contactTelTextBoxColumn);

                //5
                DataGridViewTextBoxColumn emailTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                emailTextBoxColumn.HeaderText = "Site Contact Email";
                emailTextBoxColumn.Width = 180;
                emailTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(emailTextBoxColumn);

                jobDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                jobDGV.EnableHeadersVisualStyles = false;
                jobDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                
            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("SearchJobForm.cs", "BuildDGV()", ex.Message);
                return;
            }
        }

        private void PopulateDGV(DataTable parentDT)
        {
            int row = 0;
            try
            {
                jobDGV.Rows.Clear();
                foreach (DataRow dr in parentDT.Rows)
                {
                    jobDGV.Rows.Add();
                    jobDGV[0, row].Value = dr["parentJobNo"].ToString();
                    jobDGV[1, row].Value = dr["custCode"].ToString();
                    jobDGV[2, row].Value = dr["siteAddress"].ToString();
                    jobDGV[3, row].Value = dr["siteContact"].ToString();
                    jobDGV[4, row].Value = dr["siteContactTel"].ToString();
                    jobDGV[5, row++].Value = dr["siteContactEmail"].ToString();
                   // break;
                }
                jobDGV.CurrentCell = jobDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("SearchJobForm.cs", "PopulateDGV(DataTable parentDT)", ex.Message);
                return;
            }

        }

        private void searchBySiteButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(siteKeyTextBox.Text)) { return; }
            DataTable parentDT = mcData.GetParentJobsBySiteKeyDT(siteKeyTextBox.Text);
            PopulateDGV(parentDT);
            siteKeyTextBox.Text = "";
            siteKeyTextBox.Focus();

        }

        private void searchByCustButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(custKeyTextBox.Text)) { return; }

            DataTable custDT = mcData.GetCustomersByKeyDT(custKeyTextBox.Text);
            CustomerListForm custForm = new CustomerListForm(custKeyTextBox.Text, custDT);
            custForm.ShowDialog();
            DataTable parentDT = mcData.GetParentJobsByCustCodeDT(custForm.CustCode);
            PopulateDGV(parentDT);
            custKeyTextBox.Text = "";
            custKeyTextBox.Focus();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void jobDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int parentJobNo = Convert.ToInt32(jobDGV[0, e.RowIndex].Value);
            DataTable dt = mcData.GetJobPlannerDT(parentJobNo);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(String.Format("There are no phased jobs for parent job no.{0}", parentJobNo.ToString()));
                return;
            }
            JobPlannerForm jobForm = new JobPlannerForm(dt, parentJobNo);
            jobForm.ShowDialog();
            this.Dispose();
            this.Close();
        }

        private void allButton_Click(object sender, EventArgs e)
        {
            DataTable dt = mcData.GetAllParentJobsDT();
            PopulateDGV(dt);
            return;
        }

        private void jobDGV_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!jobDGV.Focused) { return; }
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
            {
                this.jobDGV.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.jobDGV.CurrentCell = this.jobDGV.Rows[e.RowIndex].Cells[0];
                this.contextMenuStrip1.Show(this.jobDGV, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
                return;

            }
        }

        private void editParentMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            string phaseJob = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();
            int parentJobNo = Convert.ToInt32(phaseJob.Substring(0, 5));
            bool updateParentJobFlag = true;
            CreateParentJobForm pjForm = new CreateParentJobForm(parentJobNo, updateParentJobFlag);
            pjForm.ShowDialog();
            DataTable parentDT = mcData.GetParentJobsDTByJobKey(parentJobNo.ToString());
            PopulateDGV(parentDT);
            // DataTable dt = mcData.GetAllParentJobsDT();
            // PopulateDGV(dt);
            return;




            //PopulateDGV(dt);
        }

        private void addPhaseJobToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is no longer available. If you wish to add an additional phase to a job, you can do it from the right click [ADD A NEW PHASE] option in Job Planner");
            return;
            //try
            //{
            //    if (!jobDGV.Focused) { return; }
            //    if (jobDGV[colIndex, rowIndex].Value == null) { return; }
            //    int parentJobNo = Convert.ToInt32(jobDGV[colIndex, rowIndex].Value.ToString());
            //    DataTable parentDT = mcData.GetParentJobsDT(parentJobNo);
            //    bool updatePhaseJobOnly = true;
            //    JobEntryForm jobForm = new JobEntryForm(parentDT, updatePhaseJobOnly);
            //    jobForm.ShowDialog();
            //    if (jobForm.CancelJob)
            //    {
            //        this.Dispose();
            //        this.Close();
            //    }
            //    return;
            //}
            //catch (Exception ex)
            //{
            //    string msg = String.Format("addPhaseJobToolStripMenuItem1_Click() Error : {0}", ex.Message.ToString());
            //    logger.LogLine(msg);
            //    string audit = mcData.CreateErrorAudit("SearchJobForm.cs", "addPhaseJobToolStripMenuItem1_Click()", ex.Message);
            //    return;
            //}
        }

        private void searchByJobNoButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(jobNoKeyTextBox.Text)) { return; }
            
            //if(!mcData.AreDigitsOnly(jobNoKeyTextBox.Text)) 
            //{
            //    MessageBox.Show("THere are no PARENT jobs found");
            //    jobNoKeyTextBox.Text = String.Empty;
            //    jobNoKeyTextBox.Focus();
                
            //    return; 
            //}

            DataTable parentDT = mcData.GetParentJobsDTByJobKey(jobNoKeyTextBox.Text);
            if (parentDT == null) 
            {
                MessageBox.Show(String.Format("[{0}] is an INVALID parent job number", jobNoKeyTextBox.Text));
                return; 
            }
            if (parentDT.Rows.Count < 1)
            {
                MessageBox.Show("THere are no PARENT jobs found");
                jobDGV.Rows.Clear();
                jobNoKeyTextBox.Text = String.Empty;
                jobNoKeyTextBox.Focus();

                return; 
            }
            PopulateDGV(parentDT);
            custKeyTextBox.Text = "";
            custKeyTextBox.Focus();
            return;
        }

        
        
    }
}
