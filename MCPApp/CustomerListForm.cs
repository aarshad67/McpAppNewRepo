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
    public partial class CustomerListForm : Form
    {
        DataTable customerDT = new DataTable();
        Logger logger = new Logger();
        MeltonData mcData = new MeltonData();

        private string customerKey = "";

        private string custCode;
        public string CustCode
        {
            get
            {
                return custCode;
            }
            set
            {
                custCode = value;
            }
        }


        public CustomerListForm()
        {
            InitializeComponent();
        }

        private void BuildCustomersDGV()
        {
            try
            {
                custDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn custCodeBoxColumn = new DataGridViewTextBoxColumn();
                custCodeBoxColumn.HeaderText = "Cust Code";
                custCodeBoxColumn.Width = 100;
                custCodeBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custCodeBoxColumn.ReadOnly = true;
                custDGV.Columns.Add(custCodeBoxColumn);

                //1
                DataGridViewTextBoxColumn custNameTextBoxColumn = new DataGridViewTextBoxColumn();
                custNameTextBoxColumn.HeaderText = "Customer Name";
                custNameTextBoxColumn.Width = 200;
                custNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custNameTextBoxColumn.ReadOnly = true;
                custDGV.Columns.Add(custNameTextBoxColumn);

                custDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                custDGV.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                string msg = ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite;
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("CustomerListForm.cs", "BuildCustomersDGV()", msg);
            }
        }

        private void PopulateDGV( DataTable dt)
        {
            MeltonData mcData = new MeltonData();
            int row = 0;
            try
            {
                custDGV.Rows.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    custDGV.Rows.Add();
                    custDGV[0, row].Value = dr["custCode"].ToString();
                    custDGV[1, row++].Value = dr["custName"].ToString();

                }
                custDGV.CurrentCell = custDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("CustomerListForm.cs", "PopulateDGV( DataTable dt)", msg);
                return;

            }


        }

        public CustomerListForm(string custKey,DataTable custDT)
        {
            InitializeComponent();
            customerKey = custKey;
            customerDT = custDT;
        }

        private void CustomerListForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Customers returned for search key [ {0} ]", customerKey);
            BuildCustomersDGV();
            PopulateDGV(customerDT);
        }

        private void custDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!custDGV.Focused) { return; }
            custCode = custDGV[0, e.RowIndex].Value.ToString();
            this.Dispose();
            this.Close();
        }
    }
}
