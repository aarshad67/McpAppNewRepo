using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace MCPApp
{
    public partial class CancelledJobsForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        private string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

        public CancelledJobsForm()
        {
            InitializeComponent();
        }

        private void CancelledJobsForm_Load(object sender, EventArgs e)
        {
            this.Text = "Current Canncelled Jobs ( Select job(s) to UNCANCEL )";
            BuildDGV();
            PopulateDGV();
        }

        private void BuildDGV()
        {
            try
            {
                dgv.Columns.Clear();
                //0
                DataGridViewCheckBoxColumn tickColumn = new DataGridViewCheckBoxColumn();
                tickColumn.Name = "chk";
                tickColumn.HeaderText = "";
                tickColumn.Width = 30;
                tickColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                tickColumn.ReadOnly = false;
                dgv.Columns.Add(tickColumn);

                //1
                DataGridViewTextBoxColumn jobNoColumn = new DataGridViewTextBoxColumn();
                jobNoColumn.HeaderText = "Job";
                jobNoColumn.Width = 60;
                jobNoColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoColumn.DefaultCellStyle.BackColor = Color.LightPink;
                jobNoColumn.ReadOnly = true;
                dgv.Columns.Add(jobNoColumn);

                //1
                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
                dateColumn.HeaderText = "Date";
                dateColumn.Width = 100;
                dateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dateColumn.DefaultCellStyle.BackColor = Color.LightPink;
                dateColumn.ReadOnly = true;
                dgv.Columns.Add(dateColumn);

                //2
                DataGridViewTextBoxColumn custColumn = new DataGridViewTextBoxColumn();
                custColumn.HeaderText = "Customer";
                custColumn.Width = 200;
                custColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custColumn.ReadOnly = true;
                dgv.Columns.Add(custColumn);

                //3
                DataGridViewTextBoxColumn siteAddrColumn = new DataGridViewTextBoxColumn();
                siteAddrColumn.HeaderText = "Site Address";
                siteAddrColumn.Width = 200;
                siteAddrColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                siteAddrColumn.ReadOnly = true;
                dgv.Columns.Add(siteAddrColumn);

                //4
                DataGridViewTextBoxColumn valueColumn = new DataGridViewTextBoxColumn();
                valueColumn.HeaderText = "Inv Value(£)";
                valueColumn.Width = 70;
                valueColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                valueColumn.ReadOnly = true;
                dgv.Columns.Add(valueColumn);

                //5
                DataGridViewTextBoxColumn commentColumn = new DataGridViewTextBoxColumn();
                commentColumn.HeaderText = "Last Comment";
                commentColumn.Width = 300;
                commentColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                commentColumn.ReadOnly = true;
                dgv.Columns.Add(commentColumn);

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
                string audit = mcData.CreateErrorAudit("CancelledJobsForm.cs", "BuildDGV()", msg);
            }
        }

        private void PopulateDGV()
        {        

            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable dt = mcData.GetCancelledJobsDT();
                dgv.Rows.Clear();
                List<DataGridViewRow> rows = new List<DataGridViewRow>();
                foreach (DataRow dr in dt.Rows)
                {
                    DataGridViewRow drow = new DataGridViewRow();
                    drow.CreateCells(dgv);
                    drow.Cells[1].Value = dr["jobNum"].ToString();
                    drow.Cells[2].Value = Convert.ToDateTime(dr["reqDate"]).ToShortDateString();
                    drow.Cells[3].Value = dr["customer"].ToString();
                    drow.Cells[4].Value = dr["site"].ToString();
                    drow.Cells[5].Value = dr["value"].ToString();
                    drow.Cells[6].Value = dr["note"].ToString();
                    rows.Add(drow);
                }
                dgv.Rows.AddRange(rows.ToArray());
                this.Cursor = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("CancelledJobsForm.cs", "PopulateDGV()", msg);
                this.Cursor = Cursors.Default;
                return;

            }


        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void uncancelBtn_Click(object sender, EventArgs e)
        {
            List<string> myLIst = new List<string>();
            if(dgv.Rows.Count < 1) { return; }

            try
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv.Rows[i].Cells[0].Value == null) { continue; }
                    if ((bool)dgv.Rows[i].Cells[0].Value)
                    {
                        myLIst.Add(dgv.Rows[i].Cells[1].Value.ToString());
                    }
                }
                if (myLIst.Count < 1) { return; }
                string result = string.Join(Environment.NewLine, myLIst);
                if (MessageBox.Show($"Confirm jobs [{result}] are to be UNCANCELLED (Y/N) ?", "Un-Cancel Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (string job in myLIst)
                    {
                        int num = mcData.DeleteCancelledJob(job);
                        if(num == 1)
                        {
                            mcData.CreateJobComment(job, $"Job No.{job} UN-CANCELLED by [{loggedInUser}] at {DateTime.Now.ToShortTimeString()} on {DateTime.Now.ToShortDateString()}");
                        }
                    }
                    PopulateDGV();
                }
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("uncancelBtn_Click() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("CancelledJobsForm.cs", "uncancelBtn_Click()", msg);
                return;
            }

            

        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            List<string> myLIst = new List<string>();
            if (dgv.Rows.Count < 1) { return; }

            try
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv.Rows[i].Cells[0].Value == null) { continue; }
                    if ((bool)dgv.Rows[i].Cells[0].Value)
                    {
                        myLIst.Add(dgv.Rows[i].Cells[1].Value.ToString());
                    }
                }
                if (myLIst.Count < 1) { return; }
                string result = string.Join(Environment.NewLine, myLIst);
                if (MessageBox.Show($"Confirm you wish to PERMANENTLY DELETE jobs : [{result}] (Y/N) ?", "Delete Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DateTime reqDate = DateTime.MinValue;
                    string custName = "";
                    string siteAddress = "";
                    decimal invValue = 0;
                    string lastComment = "";

                    foreach (string job in myLIst)
                    {
                        mcData.GetCancelledJobDetails(job, out reqDate, out custName, out siteAddress, out invValue, out lastComment);
                        int num = mcData.DeleteCancelledJob(job);
                        int num2 = mcData.DeleteWhiteboardByJobNo(job);
                        int num3 = mcData.DeleteJobPlannerByJobNo(job);
                        int num4 = mcData.DeleteDesignBoardByJobNo(job);
                        int num5 = mcData.DeleteJobLocks("WB", job);
                        int num6 = mcData.DeleteJobLocks("JP", job);
                        if(num > 0 && num2 > 0 && num3 > 0)
                        {
                            
                            string err = mcData.CreateJobDeletionAudit(job,reqDate,custName,siteAddress,invValue,lastComment,"Deleted from teh CANCELLED JOBS screen");
                        }
                    }
                    PopulateDGV();
                }
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("uncancelBtn_Click() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("CancelledJobsForm.cs", "uncancelBtn_Click()", msg);
                return;
            }
        }
    }
}
