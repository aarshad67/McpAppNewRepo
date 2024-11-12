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
    public partial class DesignBoardParametersForm : Form
    {
        MeltonData mcData = new MeltonData();
        DateTime startDate;
        DateTime lastDate;

        public DesignBoardParametersForm()
        {
            InitializeComponent();
        }

        private void DesignBoardParametersForm_Load(object sender, EventArgs e)
        {
            this.Text = "Design Board Parameters";
            var now = DateTime.Now;
            var first = new DateTime(now.Year, now.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);

            startDate = first;// mcData.GetFirstPlannerDate();
            lastDate = last;// mcData.GetLastPlannerDate();
            startDateDTP.Value = startDate;
            endDateDTP.Value = lastDate;
        }

        private void wbButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DateTime d1 = startDateDTP.Value.Date;
            DateTime d2 = endDateDTP.Value.Date;

            DateTime firstDate = d1;
            DateTime startDate = mcData.GetMonday(d1);
            DateTime lastDate = d2;
            DataTable dt = mcData.DatesDT(d1, d2); ;
            this.Cursor = Cursors.Default;
            TimeSpan ts = lastDate - startDate;
            int dateDiff = ts.Days;
            decimal numWeeks = dateDiff / 7m;
            int roundedNumWeeks = (int)Decimal.Round(numWeeks, 1) + 1;
           
            DesignBoardForm dbForm = new DesignBoardForm(startDate, lastDate, dt, roundedNumWeeks);
            dbForm.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void startDateDTP_ValueChanged(object sender, EventArgs e)
        {
            DateTime d1 = startDateDTP.Value.Date;
            endDateDTP.Value = d1.AddDays(31);
            return;
        }
    }
}
