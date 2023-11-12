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
    public partial class WhiteboardDayCommentAuditForm : Form
    {
        private string jobNo = "";
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        public WhiteboardDayCommentAuditForm()
        {
            InitializeComponent();
        }

        public WhiteboardDayCommentAuditForm(string job)
        {
            jobNo = job;
            InitializeComponent();
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
                DataGridViewTextBoxColumn commentDateColumn = new DataGridViewTextBoxColumn();
                commentDateColumn.HeaderText = "WB Comment Date";
                commentDateColumn.Width = 100;
                commentDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                commentDateColumn.ReadOnly = true;
                commentDGV.Columns.Add(commentDateColumn);

                //0
                DataGridViewTextBoxColumn commentColumn = new DataGridViewTextBoxColumn();
                commentColumn.HeaderText = "Comment";
                commentColumn.Width = 250;
                commentColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                commentColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                commentColumn.ReadOnly = true;
                commentDGV.Columns.Add(commentColumn);

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
                userColumn.Width = 30;
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

                MessageBox.Show("BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("WhiteboardDayCommentAuditForm.cs", "BuildDGV()", ex.Message);
                return;
            }
        }

        private void PopulateDGV()
        {
            MeltonData mcData = new MeltonData();
            int row = 0;
            string fullDayStr,dayStr = "";

            try
            {
                commentDGV.Rows.Clear();
                DataTable dt = mcData.GetWBCommentsAuditDT(jobNo);
                if (dt.Rows.Count < 1) { return; }
                foreach (DataRow dr in dt.Rows)
                {
                    dayStr = Convert.ToDateTime(dr["commentDate"].ToString()).DayOfWeek.ToString().ToUpper().Substring(0, 3);
                    fullDayStr = dayStr + " " + Convert.ToDateTime(dr["commentDate"].ToString()).ToShortDateString();
                    commentDGV.Rows.Add();
                    commentDGV[0, row].Value = fullDayStr;
                    commentDGV[1, row].Value = dr["comment"].ToString();
                    commentDGV[2, row].Value = dr["dateModified"].ToString();
                    commentDGV[3, row++].Value = dr["modifiedBy"].ToString();
                }
                commentDGV.CurrentCell = commentDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("WhiteboardDayCommentAuditForm.cs", "PopulateDGV()", ex.Message);
                return;

            }


        }

        private void WhiteboardDayCommentAuditForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Job No. {0} - Day Comments Audit", jobNo);
            BuildDGV();
            PopulateDGV();
        }
    }
}
