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
    public partial class CustomersOverviewForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        private int rowIndex = 0;
        private int colIndex = 0;

        public CustomersOverviewForm()
        {
            InitializeComponent();
        }

        private void CustomersOverviewForm_Load(object sender, EventArgs e)
        {
            this.Text = "Customer Maintanance Screen ( Customers ON STOP are shown in RED )";
            this.Cursor = Cursors.WaitCursor;
            BuildCutomersDGV();
            PopulateDGV();
            AddContextMenu();
            this.Cursor = Cursors.Default;
            return;
        }

        private void BuildCutomersDGV()
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

                //2
                DataGridViewTextBoxColumn contactNameTextBoxColumn = new DataGridViewTextBoxColumn();
                contactNameTextBoxColumn.HeaderText = "Contact Name";
                contactNameTextBoxColumn.Width = 180;
                contactNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                contactNameTextBoxColumn.ReadOnly = true;
                custDGV.Columns.Add(contactNameTextBoxColumn);

                //3
                DataGridViewTextBoxColumn contactEmailTextBoxColumn = new DataGridViewTextBoxColumn();
                contactEmailTextBoxColumn.HeaderText = "Contact Email";
                contactEmailTextBoxColumn.Width = 180;
                contactEmailTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                contactEmailTextBoxColumn.ReadOnly = true;
                custDGV.Columns.Add(contactEmailTextBoxColumn);

                //4
                DataGridViewTextBoxColumn contactTelTextBoxColumn = new DataGridViewTextBoxColumn();
                contactTelTextBoxColumn.HeaderText = "Contact Tel";
                contactTelTextBoxColumn.Width = 120;
                contactTelTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                contactTelTextBoxColumn.ReadOnly = true;
                custDGV.Columns.Add(contactTelTextBoxColumn);

                //5
                DataGridViewTextBoxColumn contactMobileTextBoxColumn = new DataGridViewTextBoxColumn();
                contactMobileTextBoxColumn.HeaderText = "Contaact Mobile";
                contactMobileTextBoxColumn.Width = 120;
                contactMobileTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                contactMobileTextBoxColumn.ReadOnly = true;
                custDGV.Columns.Add(contactMobileTextBoxColumn);

                //6
                DataGridViewTextBoxColumn tempCustCodeTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                tempCustCodeTextBoxColumn.HeaderText = "Temp";
                tempCustCodeTextBoxColumn.Width = 80;
                tempCustCodeTextBoxColumn.ReadOnly = false;
                custDGV.Columns.Add(tempCustCodeTextBoxColumn);

                //7
                DataGridViewTextBoxColumn nonExistingTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                nonExistingTextBoxColumn.HeaderText = "Non Existing";
                nonExistingTextBoxColumn.Width = 80;
                nonExistingTextBoxColumn.ReadOnly = false;
                custDGV.Columns.Add(nonExistingTextBoxColumn);

                



                custDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                custDGV.EnableHeadersVisualStyles = false;
                custDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                custDGV.Columns[1].DefaultCellStyle.BackColor = Color.Yellow;

                return;


            }
            catch (Exception ex)
            {
                string msg = String.Format("LoadProductDataGridViewColumns() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("CustomerOverviewForm.cs", "BuildCutomersDGV()", msg);
                return;
            }





        }

        private void PopulateDGV()
        {
            int row = 0;
            string custCode = "";
            try
            {
                custDGV.Rows.Clear();
                DataTable dt = mcData.GetAllCustomer();
                foreach (DataRow dr in dt.Rows)
                {
                    custCode = dr["custCode"].ToString();
                   
                    custDGV.Rows.Add();
                    custDGV[0, row].Value = dr["custCode"].ToString();
                    custDGV[1, row].Value = dr["custName"].ToString();
                    custDGV[2, row].Value = dr["contactName"].ToString();
                    custDGV[3, row].Value = dr["contactEmail"].ToString();
                    custDGV[4, row].Value = dr["contactTel"].ToString();
                    custDGV[5, row].Value = dr["contactMobile"].ToString();
                    custDGV[6, row].Value = dr["tempCustCode"].ToString();
                    custDGV[7, row++].Value = dr["nonExistingCustomer"].ToString();
                }

                foreach (DataGridViewRow dgvRow in custDGV.Rows)
                {
                    if (dgvRow.Cells[0].Value == null) { continue; }
                    string customerCode = dgvRow.Cells[0].Value.ToString();
                    
                    if (mcData.IsJobCustomerOnStop(customerCode))
                    {
                        dgvRow.DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    
                }
                custDGV.CurrentCell = custDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("CustomerOverviewForm.cs", "PopulateDGV()", msg);
                return;
            }
            

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {

            this.Dispose();
            this.Close();
        }

        private void custDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (custDGV.Rows[e.RowIndex].Cells[0].Value == null)
            //{
            //    return;
            //}
            //string custCode = custDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
            //if (mcData.IsCustomerTempOrNonExisting(custCode))
            //{
            //    custDGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            //}
            //else
            //{
            //    custDGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            //}
        }

        private void AddCustomerBtn_Click(object sender, EventArgs e)
        {
            AddCustomerForm addForm = new AddCustomerForm();
            addForm.ShowDialog();
            PopulateDGV();
            if (addForm.CustCodeUpdated != "ERR" && !String.IsNullOrWhiteSpace(addForm.CustCodeUpdated)) { GetCustCurrentRow(addForm.CustCodeUpdated); }
            return;
        }

        private void GetCustCurrentRow(string custCode)
        {
            if (!this.Text.Contains("Customer")) { return; }
            int index = -1;
            try
            {
                for (int i = 0; i < custDGV.Rows.Count; i++)
                {
                    if (custDGV.Rows[i].Cells[0].Value.ToString() == custCode)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    custDGV.CurrentCell = custDGV.Rows[index].Cells[0];
                }
            }
            catch (Exception ex)
            {
                string msg = "GetCustCodeCurrentRow()  ERROR : " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite;
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("CustomerOverviewForm.cs", String.Format("GetCustCurrentRow({0}",custCode), msg);
            }
        }

        private void custDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string custCode = custDGV[0, e.RowIndex].Value.ToString();
            AddCustomerForm addForm = new AddCustomerForm(custCode);
            addForm.ShowDialog();
            PopulateDGV();
            if (addForm.CustCodeUpdated != "ERR" && addForm.CustCodeUpdated != "DEL" && !String.IsNullOrWhiteSpace(addForm.CustCodeUpdated)) { GetCustCurrentRow(addForm.CustCodeUpdated); } else { PopulateDGV(); }
            return;
        }

        private void toolStripMenuItemOn_Click(object sender, EventArgs e)
        {
            if (custDGV[0, rowIndex].Value == null) { return; }
            string custCode = custDGV[0, this.rowIndex].Value.ToString();
            string custName = custDGV[1, this.rowIndex].Value.ToString();
            
            string msg = $"Put [{custCode} - {custName.ToUpper()}] ON STOP ? ";
            if (MessageBox.Show(msg, "Customer ON STOP Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string result = mcData.UpdateCustomerOnStopFlag(custCode, "Y");
                if (result == "OK")
                {
                    MessageBox.Show($"Customer [{custName}] is now ON STOP");
                    custDGV.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    return;
                }
            }
        }

        private void toolStripMenuItemOff_Click(object sender, EventArgs e)
        {
            if (custDGV[0, rowIndex].Value == null) { return; }
            string custCode = custDGV[0, this.rowIndex].Value.ToString();
            string custName = custDGV[1, this.rowIndex].Value.ToString();

            string msg = $"Are you sure you wish to take customer [{custCode} - {custName.ToUpper()}] OFF STOP ? ";
            if (MessageBox.Show(msg, "Customer OFF STOP Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string result = mcData.UpdateCustomerOnStopFlag(custCode, "N");
                if (result == "OK")
                {
                    MessageBox.Show($"Customer [{custName}] is now no longer ON STOP");
                    custDGV.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    return;
                }
            }
        }

        private void custDGV_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.RowIndex != -1) // added this and it now works
            {
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.custDGV = (DataGridView)sender;
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
            {
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.custDGV = (DataGridView)sender;
                return;
            }
        }

        private void AddContextMenu()
        {


            foreach (DataGridViewColumn column in custDGV.Columns)
            {
                if (column.Index == 0)
                {
                    column.ContextMenuStrip = contextMenuStrip1;

                }
            }
        }
    }
}
