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
    public partial class AddNewPhaseForm : Form
    {
        private Logger logger = new Logger();
        private MeltonData mcData = new MeltonData();
        private string parentJobNo = "";
        private string newPhaseJob = "";

        public String NewPhaseJob 
        {
            get { return newPhaseJob; }
            set { newPhaseJob = value; }
        }

        public AddNewPhaseForm()
        {
            InitializeComponent();
        }

        public AddNewPhaseForm(string parentJob)
        {
            InitializeComponent();
            parentJobNo = parentJob;
        }

        private void AddNewPhaseForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Create Phase for Parent Job.{parentJobNo}";
            txtParentJob.Text = parentJobNo;
            FillExistingPhasesDGV();
            txtPhaseNo.Focus();

        }

        private void BuildDGV()
        {
            try
            {
                

                phasesDGV.Columns.Clear();

                //0
                DataGridViewTextBoxColumn jobNoBoxColumn = new DataGridViewTextBoxColumn();
                jobNoBoxColumn.HeaderText = "JobNo";
                jobNoBoxColumn.Width = 70;
                jobNoBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoBoxColumn.ReadOnly = true;
                phasesDGV.Columns.Add(jobNoBoxColumn);

                //1
                DataGridViewTextBoxColumn reqDateTextBoxColumn = new DataGridViewTextBoxColumn();
                reqDateTextBoxColumn.HeaderText = "Required Date";
                reqDateTextBoxColumn.ValueType = typeof(DateTime);
                reqDateTextBoxColumn.Width = 120;
                reqDateTextBoxColumn.DefaultCellStyle.Format = "dd/MM/YYYY";
                reqDateTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                reqDateTextBoxColumn.ReadOnly = true;
                phasesDGV.Columns.Add(reqDateTextBoxColumn);

                //2
                DataGridViewTextBoxColumn siteAddressTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                siteAddressTextBoxColumn.HeaderText = "Site Address";
                siteAddressTextBoxColumn.Width = 400;
                siteAddressTextBoxColumn.ReadOnly = true;
                phasesDGV.Columns.Add(siteAddressTextBoxColumn);

                //3
                DataGridViewTextBoxColumn levelColumn = new DataGridViewTextBoxColumn();
                levelColumn.HeaderText = "Floor Levsls";
                levelColumn.Width = 150;
                levelColumn.ReadOnly = true;
                phasesDGV.Columns.Add(levelColumn);

                //4
                DataGridViewTextBoxColumn completedColumn = new DataGridViewTextBoxColumn();
                completedColumn.HeaderText = "Completed";
                completedColumn.Width = 150;
                completedColumn.ReadOnly = true;
                phasesDGV.Columns.Add(completedColumn);







                phasesDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                phasesDGV.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                phasesDGV.EnableHeadersVisualStyles = false;
                

                




            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "BuildDGV()", msg);
                return;
            }





        }

        private void PopulateDGV()
        {
            int row = 0;
            string jobNo = "";
            string completedFlag = "";
           
            DataTable jobDT = mcData.GetJobPlannerDT(Convert.ToInt32(parentJobNo));
            if(jobDT.Rows.Count == 0) { return; }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                phasesDGV.Rows.Clear();
                foreach (DataRow dr in jobDT.Rows)
                {
                    jobNo = dr["jobNo"].ToString();
                    completedFlag = mcData.IsJobCompleted(jobNo) == true ? "YES" : "NO";
                    phasesDGV.Rows.Add();
                    phasesDGV[0, row].Value = jobNo;
                    phasesDGV[1, row].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().Substring(0, 3) + " " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString(); ;
                    phasesDGV[2, row].Value = dr["siteAddress"].ToString();
                    phasesDGV[3, row].Value = dr["floorLevel"].ToString();
                    phasesDGV[4, row++].Value = completedFlag;
                }

                phasesDGV.CurrentCell = phasesDGV.Rows[0].Cells[0];

                
                this.Cursor = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "PopulateDGV(DataTable jobDT)", msg);
                this.Cursor = Cursors.Default;
                return;
            }


        }

        private void FillExistingPhasesDGV()
        {
            BuildDGV(); PopulateDGV();
        }

        private void txtPhaseNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            
            
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt16(txtPhaseNo.Text) < 10 && txtPhaseNo.Text.Trim().Length < 2)
            {
                MessageBox.Show("Phase number should be 2 digits. Please re-enter");
                txtPhaseNo.Clear();
                txtPhaseNo.Focus();
                return;
            }

            string phasedJob = txtParentJob.Text.Trim() + "." + txtPhaseNo.Text.Trim();
            if(mcData.IsJobExists(phasedJob))
            {
                MessageBox.Show($"Phased Job {phasedJob} already exists. Please re-enter");
                txtPhaseNo.Clear();
                txtPhaseNo.Focus();
                return;
            }

            newPhaseJob = phasedJob;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            newPhaseJob = "";
            this.Close();
        }
    }
}
