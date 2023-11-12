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
    public partial class WhiteboardDayCommentForm : Form
    {
        MeltonData mcData = new MeltonData();
        private string dayComment = "";
        public string DayComment
        {
            get
            {
                return dayComment;
            }
            set
            {
                dayComment = value;
            }
        }

        string jobNo = "";
        DateTime requiredDate;
        public WhiteboardDayCommentForm()
        {
            InitializeComponent();
        }

        public WhiteboardDayCommentForm(string job,DateTime date,string comment)
        {
            InitializeComponent();
            jobNo = job;
            requiredDate = date;
            dayComment = comment;
        }

        private void WhiteboardDayCommentForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Add comment to job no.{0} on day [{1}, {2}]", jobNo, requiredDate.DayOfWeek.ToString().ToUpper(), requiredDate.ToShortDateString());
            commentTextBox.Text = dayComment != "N/A" ? dayComment : String.Empty;
          //  FillFixersCombo();
        }

        private void FillFixersCombo()
        {
            //DataTable dt = mcData.GetFixersDT();
            //fixerCombo.DataSource = dt;
            //fixerCombo.ValueMember = dt.Columns[0].ColumnName;
            //fixerCombo.DisplayMember = dt.Columns[1].ColumnName.ToString();
            //return;

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            
            dayComment = commentTextBox.Text;
            this.Dispose();
            this.Close();
            return;
            
        }
    }
}
