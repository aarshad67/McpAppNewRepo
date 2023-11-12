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
    public partial class JobDateAuditForm : Form
    {
        private string jobNo = "";
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        public JobDateAuditForm()
        {
            InitializeComponent();
        }

        public JobDateAuditForm(string job)
        {
            jobNo = job;
            InitializeComponent();
        }

        private void JobDateAuditForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Job No. {0} - Audit of Job Date Movements", jobNo);
            BuildDGV();
            PopulateDGV();
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
                commentDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn jobNoColumn = new DataGridViewTextBoxColumn();
                jobNoColumn.HeaderText = "Job No";
                jobNoColumn.Width = 100;
                jobNoColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoColumn.ReadOnly = true;
                commentDGV.Columns.Add(jobNoColumn);

                //1
                DataGridViewTextBoxColumn commentDateColumn = new DataGridViewTextBoxColumn();
                commentDateColumn.HeaderText = "Job Date";
                commentDateColumn.Width = 150;
                commentDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                commentDateColumn.ReadOnly = true;
                commentDGV.Columns.Add(commentDateColumn);

                //2
                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
                dateColumn.HeaderText = "Audit Date";
                dateColumn.Width = 150;
                dateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dateColumn.ReadOnly = true;
                commentDGV.Columns.Add(dateColumn);

                //3
                DataGridViewTextBoxColumn userColumn = new DataGridViewTextBoxColumn();
                userColumn.HeaderText = " By";
                userColumn.Width = 20;
                userColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                userColumn.ReadOnly = true;
                commentDGV.Columns.Add(userColumn);

                commentDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                commentDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                commentDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                commentDGV.EnableHeadersVisualStyles = false;
                commentDGV.AllowUserToAddRows = false;



            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite;
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobDateAuditForm.cs", "BuildDGV()", msg);
            }
        }

        private void PopulateDGV()
        {
            MeltonData mcData = new MeltonData();
            int row = 0;
            string fullDayStr, dayStr = "";

            try
            {
                commentDGV.Rows.Clear();
                DataTable dt = mcData.GetJobDayAuditDT(jobNo);
                if (dt.Rows.Count < 1) { return; }
                foreach (DataRow dr in dt.Rows)
                {
                    dayStr = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().ToUpper().Substring(0, 3);
                    fullDayStr = dayStr + ", " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                    commentDGV.Rows.Add();
                    commentDGV[0, row].Value = dr["jobNo"].ToString();
                    commentDGV[1, row].Value = fullDayStr;
                    commentDGV[2, row].Value = dr["auditDate"].ToString();
                    commentDGV[3, row++].Value = dr["auditUser"].ToString();
                }
                commentDGV.CurrentCell = commentDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobDateAuditForm.cs", "PopulateDGV()", msg);
                return;

            }


        }
    }
}
