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
    public partial class JobCommentForm : Form
    {
        string jobNo = "";
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        public JobCommentForm(string jobNumber)
        {
            InitializeComponent();
            jobNo = jobNumber;
        }

        private void JobCommentForm_Load(object sender, EventArgs e)
        {
            this.Text = "Add new comment for Job No." + jobNo;
            BuildDGV();
            PopulateDGV();
            commentTextBox.Focus();
        }

        private void BuildDGV()
        {
            try
            {
                commentDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn seqColumn = new DataGridViewTextBoxColumn();
                seqColumn.HeaderText = "Seq";
                seqColumn.Width = 30;
                seqColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                seqColumn.ReadOnly = true;
                commentDGV.Columns.Add(seqColumn);

                //1
                DataGridViewTextBoxColumn commentColumn = new DataGridViewTextBoxColumn();
                commentColumn.HeaderText = "Comment";
                commentColumn.Width = 950;
                commentColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                commentColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                commentColumn.ReadOnly = true;
                commentDGV.Columns.Add(commentColumn);

                //2
                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
                dateColumn.HeaderText = "Date Created";
                dateColumn.Width = 150;
                dateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dateColumn.ReadOnly = true;
                commentDGV.Columns.Add(dateColumn);

                //3
                DataGridViewTextBoxColumn userColumn = new DataGridViewTextBoxColumn();
                userColumn.HeaderText = "Created By";
                userColumn.Width = 60;
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
                string audit = mcData.CreateErrorAudit("JobCommentForm.cs", "BuildDGV()", msg);
            }
        }

        private void PopulateDGV()
        {
            MeltonData mcData = new MeltonData();
            int row = 0;
            try
            {
                commentDGV.Rows.Clear();
                DataTable dt = mcData.GetJobCommentsDT(jobNo);
                if (dt.Rows.Count < 1) { return; }
                foreach (DataRow dr in dt.Rows)
                {
                    commentDGV.Rows.Add();
                    commentDGV[0, row].Value = dr["seq"].ToString();
                    commentDGV[1, row].Value = dr["comment"].ToString();
                    commentDGV[2, row].Value = dr["dateCreated"].ToString();
                    commentDGV[3, row++].Value = dr["createdBy"].ToString();
                }
                commentDGV.CurrentCell = commentDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobCommentForm.cs", "PopulateDGV()", msg);
                return;

            }


        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(commentTextBox.Text)) { return; }
            string err = mcData.CreateJobComment(jobNo, commentTextBox.Text);
            if (err != "OK")
            {
                MessageBox.Show(String.Format("saveButton_Click() ERROR : {0}", err));
                return;
            }
            PopulateDGV();
            commentDGV.Refresh();
            commentTextBox.Text = string.Empty;
            commentTextBox.Focus();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void cancelButton_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void commentDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void saveButton_Click_1(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(commentTextBox.Text)) { return; }
            string err = mcData.CreateJobComment(jobNo, commentTextBox.Text);
            if (err != "OK")
            {
                MessageBox.Show(String.Format("saveButton_Click() ERROR : {0}", err));
                return;
            }
            PopulateDGV();
            commentDGV.Refresh();
            commentTextBox.Text = string.Empty;
            commentTextBox.Focus();
            return;
        }
    }
}
