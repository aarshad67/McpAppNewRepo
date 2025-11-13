using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MCPApp
{
    public partial class JobDesignStatusAuditForm : Form
    {
        private MeltonData mcData = new MeltonData();   
        private Logger logger = new Logger();   
        private string _jobNo = string.Empty;
        private DataTable _auditDT = new DataTable();

        public JobDesignStatusAuditForm()
        {
            InitializeComponent();
        }

        public JobDesignStatusAuditForm(string jobNumber, DataTable myDT)
        {
            InitializeComponent();
            _jobNo = jobNumber;
            _auditDT = myDT;
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
                auditDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn jobNoColumn = new DataGridViewTextBoxColumn();
                jobNoColumn.HeaderText = "Job No";
                jobNoColumn.Width = 100;
                jobNoColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoColumn.ReadOnly = true;
                jobNoColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;// | DataGridViewAutoSizeColumnMode.None;
                auditDGV.Columns.Add(jobNoColumn);

                //1
                DataGridViewTextBoxColumn commentDateColumn = new DataGridViewTextBoxColumn();
                commentDateColumn.HeaderText = "Design Date";
                commentDateColumn.Width = 250;
                commentDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                commentDateColumn.ReadOnly = true;
                commentDateColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;// | DataGridViewAutoSizeColumnMode.None;
                auditDGV.Columns.Add(commentDateColumn);

                //2
                DataGridViewTextBoxColumn statusColumn = new DataGridViewTextBoxColumn();
                statusColumn.HeaderText = "Design Status";
                statusColumn.Width = 200;
                statusColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                statusColumn.ReadOnly = true;
                statusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;// | DataGridViewAutoSizeColumnMode.None;
                auditDGV.Columns.Add(statusColumn);

                //3
                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
                dateColumn.HeaderText = "Audit Date";
                dateColumn.Width = 150;
                dateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dateColumn.ReadOnly = true;
                dateColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;// | DataGridViewAutoSizeColumnMode.None;
                auditDGV.Columns.Add(dateColumn);

                //4
                DataGridViewTextBoxColumn userColumn = new DataGridViewTextBoxColumn();
                userColumn.HeaderText = " By";
                userColumn.Width = 30;
                userColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                userColumn.ReadOnly = true;
                userColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;// | DataGridViewAutoSizeColumnMode.None;
                auditDGV.Columns.Add(userColumn);

                //5
                DataGridViewTextBoxColumn sourceColumn = new DataGridViewTextBoxColumn();
                sourceColumn.HeaderText = " Source";
                sourceColumn.Width = 500;
                sourceColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                sourceColumn.ReadOnly = true;
                sourceColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;//| DataGridViewAutoSizeColumnMode.None;   
                auditDGV.Columns.Add(sourceColumn);

                auditDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                auditDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                auditDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                auditDGV.EnableHeadersVisualStyles = false;
                auditDGV.AllowUserToAddRows = false;



            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite;
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobDateAuditForm.cs", "BuildDGV()", msg);
            }
        }

        private void PopulateDGV(DataTable dt)
        {
           
            int row = 0;
            string fullDayStr, dayStr = "";

            try
            {
                auditDGV.Rows.Clear();
                
                if (dt.Rows.Count < 1) { return; }
                foreach (DataRow dr in dt.Rows)
                {
                    dayStr = Convert.ToDateTime(dr["designDate"].ToString()).DayOfWeek.ToString().ToUpper().Substring(0, 3);
                    fullDayStr = dayStr + ", " + Convert.ToDateTime(dr["designDate"].ToString()).ToShortDateString();
                    auditDGV.Rows.Add();
                    auditDGV[0, row].Value = dr["jobNo"].ToString();
                    auditDGV[1, row].Value = fullDayStr;
                    auditDGV[2, row].Value = dr["designStatus"].ToString();
                    auditDGV[3, row].Value = dr["auditDate"].ToString();
                    auditDGV[4, row].Value = dr["auditUser"].ToString();
                    auditDGV[5, row++].Value = dr["source"].ToString();
                }
                auditDGV.CurrentCell = auditDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobDesignStatusAuditForm.cs", "PopulateDGV()", msg);
                return;

            }


        }

        private void JobDesignStatusAuditForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Audit of DESIGN STATUS movements for Job [{_jobNo}]";
            BuildDGV();
            PopulateDGV(_auditDT);
        }
    }
}
