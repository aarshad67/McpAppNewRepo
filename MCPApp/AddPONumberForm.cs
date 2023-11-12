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
    public partial class AddPONumberForm : Form
    {
        MeltonData mData = new MeltonData();
        private string jobNumber = "";
        private string poNumber = "";
        public string PONumber
        {
            get
            {
                return poNumber;
            }
            set
            {
                poNumber = value;
            }
        }

        public AddPONumberForm()
        {
            InitializeComponent();
        }

        public AddPONumberForm(string jobNum)
        {
            InitializeComponent();
            jobNumber = jobNum;
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(poTextBox.Text)) { return; }

            if (poTextBox.Text.Length < 3) { return; }

            poNumber = poTextBox.Text;

            this.Dispose();
            this.Close();


        }

        private void AddPONumberForm_Load(object sender, EventArgs e)
        {

            this.Text = "Enter PO Number for Job No." + jobNumber;
            string suppRef = mData.GetSupplierRefFromJobNo(jobNumber);
            poTextBox.Text = String.IsNullOrWhiteSpace(suppRef) ? String.Empty : suppRef; 
            poTextBox.Focus();
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            poNumber = poTextBox.Text;
            this.Dispose();
            this.Close();
        }
    }
}
