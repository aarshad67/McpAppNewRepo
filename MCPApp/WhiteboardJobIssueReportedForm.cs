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
    public partial class WhiteboardJobIssueReportedForm : Form
    {
        Logger logger = new Logger();
        private string _jobNo = "";
        public WhiteboardJobIssueReportedForm(string jobNo)
        {
            InitializeComponent();
            _jobNo = jobNo;
        }

        public WhiteboardJobIssueReportedForm()
        {
            InitializeComponent();
        }

        private void WhiteboardJobIssueReportedForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Issues Reported on Site for Job {_jobNo}";
            BuildDGV();
            PopulateDGV();
        }

        private void BuildDGV()
        {
            try
            {
                dgv.Columns.Clear();
                //0
                DataGridViewTextBoxColumn suppCodeBoxColumn = new DataGridViewTextBoxColumn();
                suppCodeBoxColumn.HeaderText = "Date Raised";
                suppCodeBoxColumn.Width = 100;
                suppCodeBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppCodeBoxColumn.ReadOnly = true;
                dgv.Columns.Add(suppCodeBoxColumn);

                //1
                DataGridViewTextBoxColumn suppNameTextBoxColumn = new DataGridViewTextBoxColumn();
                suppNameTextBoxColumn.HeaderText = "By";
                suppNameTextBoxColumn.Width = 60;
                suppNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppNameTextBoxColumn.ReadOnly = true;
                dgv.Columns.Add(suppNameTextBoxColumn);

                //2
                DataGridViewTextBoxColumn shortnameTextBoxColumn = new DataGridViewTextBoxColumn();
                shortnameTextBoxColumn.HeaderText = "Issue";
                shortnameTextBoxColumn.Width = 500;
                shortnameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                shortnameTextBoxColumn.ReadOnly = true;
                dgv.Columns.Add(shortnameTextBoxColumn);

                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.EnableHeadersVisualStyles = false;
                dgv.AllowUserToAddRows = false;


            }
            catch (Exception ex)
            {
                MeltonData mcData = new MeltonData();
                MessageBox.Show("BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("WhiteboardJobIssueReportedForm.cs", "BuildDGV()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                return;
            }
        }

        private void PopulateDGV()
        {
            MeltonData mcData = new MeltonData();

            DataTable dt = mcData.GetWBJobIssuesDT(_jobNo);
            int row = 0;
            
            try
            {
                dgv.Rows.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    dgv.Rows.Add();
                    dgv[0, row].Value = dr["jobIssueDate"].ToString();
                    dgv[1, row].Value = dr["jobIssueUser"].ToString();
                    dgv[2, row++].Value = dr["jobIssue"].ToString();
                }
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = $"PopulateDGV Error : {ex.Message}";
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("WhiteboardJobIssueReportedForm.cs", "PopulateDGV()", ex.Message);
                return;

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
