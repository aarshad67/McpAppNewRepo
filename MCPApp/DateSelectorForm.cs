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
    public partial class DateSelectorForm : Form
    {
        private DateTime requiredDate = DateTime.MinValue;
        public DateTime RequiredDate
        {
            get
            {
                return requiredDate;
            }
            set
            {
                requiredDate = value;
            }
        }
        public DateSelectorForm()
        {
            InitializeComponent();
        }

        public DateSelectorForm(DateTime reqDate)
        {
            InitializeComponent();
            requiredDate = reqDate;
        }

        private void DateSelectorForm_Load(object sender, EventArgs e)
        {
            this.Text = "Select REQUIRED/DELIVERY date";
            mcCalendar.CalendarDimensions = new System.Drawing.Size(4, 3);
            mcCalendar.MaxSelectionCount = 1;
            mcCalendar.SetDate(requiredDate);
        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void mcCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            requiredDate = e.Start;
        }
    }
}
