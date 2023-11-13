using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCPApp
{
    public partial class DeletedJobsForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        private string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

        public DeletedJobsForm()
        {
            InitializeComponent();
        }

        private void DeletedJobsForm_Load(object sender, EventArgs e)
        {
            this.Text = "Jobs Permanently Deleted";
            BuildDGV();
            PopulateDGV();
        }

        private void BuildDGV()
        {
            try
            {
                dgv.Columns.Clear();
                //0
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
                dateColumn.Width = 80;
                dateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dateColumn.DefaultCellStyle.BackColor = Color.LightPink;
                dateColumn.ReadOnly = true;
                dgv.Columns.Add(dateColumn);

                //2
                DataGridViewTextBoxColumn custColumn = new DataGridViewTextBoxColumn();
                custColumn.HeaderText = "Customer";
                custColumn.Width = 100;
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
                commentColumn.Width = 200;
                commentColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                commentColumn.ReadOnly = true;
                dgv.Columns.Add(commentColumn);

                //6
                DataGridViewTextBoxColumn auditUserColumn = new DataGridViewTextBoxColumn();
                auditUserColumn.HeaderText = "Deleted On";
                auditUserColumn.Width = 120;
                auditUserColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                auditUserColumn.ReadOnly = true;
                dgv.Columns.Add(auditUserColumn);


                //7
                DataGridViewTextBoxColumn auditDateColumn = new DataGridViewTextBoxColumn();
                auditDateColumn.HeaderText = "By";
                auditDateColumn.Width = 20;
                auditDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                auditDateColumn.ReadOnly = true;
                dgv.Columns.Add(auditDateColumn);

                //8
                DataGridViewTextBoxColumn triggerColumn = new DataGridViewTextBoxColumn();
                triggerColumn.HeaderText = "Delete Triggered By";
                triggerColumn.Width = 300;
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
                string audit = mcData.CreateErrorAudit("DeletedJobsForm.cs", "BuildDGV()", msg);
            }
        }

        private void PopulateDGV()
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable dt = mcData.GetDeletedJobsAuditDT();
                dgv.Rows.Clear();
                List<DataGridViewRow> rows = new List<DataGridViewRow>();
                foreach (DataRow dr in dt.Rows)
                {
                    DataGridViewRow drow = new DataGridViewRow();
                    drow.CreateCells(dgv);
                    drow.Cells[0].Value = dr["jobNo"].ToString();
                    drow.Cells[1].Value = Convert.ToDateTime(dr["reqDate"]).ToShortDateString();
                    drow.Cells[2].Value = dr["custName"].ToString();
                    drow.Cells[3].Value = dr["siteAddress"].ToString();
                    drow.Cells[4].Value = dr["invValue"].ToString();
                    drow.Cells[5].Value = dr["lastComment"].ToString();
                    drow.Cells[6].Value = dr["auditDate"].ToString();
                    drow.Cells[7].Value = dr["auditUser"].ToString();
                    drow.Cells[8].Value = dr["deleteTrigger"].ToString();
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
                string audit = mcData.CreateErrorAudit("DeletedJobsForm.cs", "PopulateDGV()", msg);
                this.Cursor = Cursors.Default;
                return;

            }


        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
            return;
        }
    }
}
