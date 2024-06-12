using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;


namespace MCPApp
{
    public partial class JobsMissingDataForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        private string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
        private string _mode = "";
        private int rowIndex = 0;
        private int colIndex = 0;
        int parentJobNo = 0;

        public JobsMissingDataForm()
        {
            InitializeComponent();
        }

        public JobsMissingDataForm(string product) //BEAMZERO , SLABZERO , ALLZERO, MISSINGSUPPLIER
        {
            _mode = product;
            InitializeComponent();
        }

        private void JobsMissingDataForm_Load(object sender, EventArgs e)
        {
            string prompt = "";
            switch (_mode)
            {
                case "BEAMZERO":
                    prompt = "Completed BEAM jobs with Missing LM or M²:";
                    break;
                case "SLABZERO":
                    prompt = "Completed SLAB jobs with Missing M²:";
                    break;
                case "ALLZERO":
                    prompt = "COMPLETED jobs where Beam LM/M² and Slab M² are all ZERO:";
                    break;
                case "MISSINGSUPPLIER":
                    prompt = "Completed jobs where SUPPLIERS are Missing:";
                    break;
                default:
                    break;
            }
            this.Text = prompt;
            label2.Text = $"Number of {prompt}";
            this.Cursor = Cursors.WaitCursor;
            BuildDGV();
            DataTable missingDT = mcData.GetJobPlannerMIssingDataDT(_mode);
            int missingCount = mcData.GetNumMissingData(_mode);
            totalSlabM2TextBox.Text = missingCount.ToString();
            PopulateDGV(missingDT);
            this.Cursor = Cursors.Default;
            return;
        }

        private void BuildDGV()
        {
            try
            {
                

                jobDGV.Columns.Clear();

                //0
                DataGridViewTextBoxColumn jobNoBoxColumn = new DataGridViewTextBoxColumn();
                jobNoBoxColumn.HeaderText = "JobNo";
                jobNoBoxColumn.Width = 70;
                jobNoBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoBoxColumn.ReadOnly = true;
                jobNoBoxColumn.Frozen = true;
                jobDGV.Columns.Add(jobNoBoxColumn);

                //1
                DataGridViewTextBoxColumn custBoxColumn = new DataGridViewTextBoxColumn();
                custBoxColumn.HeaderText = "Customer";
                custBoxColumn.Width = 150;
                custBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custBoxColumn.ReadOnly = true;
                custBoxColumn.Frozen = true;
                jobDGV.Columns.Add(custBoxColumn);

                //2

                DataGridViewTextBoxColumn levelColumn = new DataGridViewTextBoxColumn();
                levelColumn.HeaderText = "Floor Levels";
                levelColumn.Width = 80;
                levelColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                levelColumn.ReadOnly = true;
                levelColumn.Frozen = true;
                jobDGV.Columns.Add(levelColumn);

                //3
                DataGridViewTextBoxColumn reqDateTextBoxColumn = new DataGridViewTextBoxColumn();
                reqDateTextBoxColumn.HeaderText = "Required Date";
                reqDateTextBoxColumn.ValueType = typeof(DateTime);
                reqDateTextBoxColumn.Width = 120;
                reqDateTextBoxColumn.DefaultCellStyle.Format = "dd/MM/YYYY";
                reqDateTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                reqDateTextBoxColumn.ReadOnly = true;
                reqDateTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                reqDateTextBoxColumn.Frozen = true;
                jobDGV.Columns.Add(reqDateTextBoxColumn);

                //4
                DataGridViewTextBoxColumn siteAddressTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                siteAddressTextBoxColumn.HeaderText = "Site Address";
                siteAddressTextBoxColumn.Width = 150;
                siteAddressTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(siteAddressTextBoxColumn);

                //5
                DataGridViewTextBoxColumn slabM2TextBoxColumn = new DataGridViewTextBoxColumn();  //0
                slabM2TextBoxColumn.HeaderText = "Slab M2";
                slabM2TextBoxColumn.Width = 50;
                slabM2TextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(slabM2TextBoxColumn);

                //6
                DataGridViewTextBoxColumn beamM2TextBoxColumn = new DataGridViewTextBoxColumn();  //0
                beamM2TextBoxColumn.HeaderText = "Beam M2";
                beamM2TextBoxColumn.Width = 50;
                beamM2TextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(beamM2TextBoxColumn);

                //7
                DataGridViewTextBoxColumn beamLmTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                beamLmTextBoxColumn.HeaderText = "Beam LM";
                beamLmTextBoxColumn.Width = 50;
                beamLmTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(beamLmTextBoxColumn);

                //8
                DataGridViewTextBoxColumn supplierTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                supplierTextBoxColumn.HeaderText = "Supplier (Dbl Click)";
                supplierTextBoxColumn.Width = 80;
                supplierTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(supplierTextBoxColumn);

                //suppTypeBindngSource
                //9
                DataGridViewTextBoxColumn suppTypeColumn = new DataGridViewTextBoxColumn();
                suppTypeColumn.HeaderText = "SuppType";
                suppTypeColumn.Width = 60;
                suppTypeColumn.ReadOnly = true;
                jobDGV.Columns.Add(suppTypeColumn);

                //10
                DataGridViewTextBoxColumn suppRefTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                suppRefTextBoxColumn.HeaderText = "Supplier Ref";
                suppRefTextBoxColumn.Width = 80;
                suppRefTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(suppRefTextBoxColumn);

                //11
                DataGridViewTextBoxColumn phasedValueTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                phasedValueTextBoxColumn.HeaderText = "Phase Value(£)";
                phasedValueTextBoxColumn.Width = 80;
                phasedValueTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                phasedValueTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(phasedValueTextBoxColumn);


                jobDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                jobDGV.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                jobDGV.EnableHeadersVisualStyles = false;
                jobDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                jobDGV.Columns[3].DefaultCellStyle.BackColor = Color.LightGreen;
                jobDGV.Columns[11].DefaultCellStyle.BackColor = Color.Cyan;
                jobDGV.Columns[5].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[6].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[7].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[11].DefaultCellStyle.Format = "D2";
                jobDGV.RowHeadersVisible = false;



            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobsMissingDataForm.cs", "BuildDGV()", msg);
                return;
            }





        }

        private void PopulateDGV(DataTable jobDT)
        {
            if (jobDT == null) { return; }
            if (jobDT.Rows.Count == 0) { return; }
            int row = 0;
            int rgb1, rgb2, rgb3 = 255;
            string suppShortname = "";
            
            string jobNo = "";
            string custName = "";
            string custCode = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                jobDGV.Rows.Clear();
                List<DataGridViewRow> rows = new List<DataGridViewRow>();
                foreach (DataRow dr in jobDT.Rows)
                {

                    jobNo = dr["jobNo"].ToString();
                    custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                    custName = mcData.GetCustName(custCode);
                    suppShortname = dr["productSupplier"].ToString();
                    mcData.GetSupplierColourByShortname(suppShortname, out rgb1, out rgb2, out rgb3);
                    DataGridViewRow drow = new DataGridViewRow();
                    drow.CreateCells(jobDGV);
                    drow.Cells[0].Value = jobNo;
                    drow.Cells[1].Value = custName;
                    drow.Cells[2].Value = dr["floorLevel"].ToString();
                    drow.Cells[3].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().Substring(0, 3) + " " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                    drow.Cells[4].Value = dr["siteAddress"].ToString();
                    drow.Cells[5].Value = dr["slabM2"].ToString();
                   // drow.Cells[5].ReadOnly = dr["slabM2"].ToString().Length > 0 ? true : false;
                    drow.Cells[6].Value = dr["beamM2"].ToString();
                  //  drow.Cells[6].ReadOnly = dr["beamM2"].ToString().Length > 0 ? true : false;
                    drow.Cells[7].Value = dr["beamLm"].ToString();
                  //  drow.Cells[7].ReadOnly = dr["beamLm"].ToString().Length > 0 ? true : false;
                    drow.Cells[8].Value = dr["productSupplier"].ToString();
                 //   drow.Cells[8].ReadOnly = dr["productSupplier"].ToString().Length > 0 ? true : false;
                    drow.Cells[8].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    drow.Cells[9].Value = dr["supplyType"].ToString();
                    drow.Cells[10].Value = dr["supplierRef"].ToString();
                    drow.Cells[11].Value = dr["phaseInvValue"].ToString();
                    rows.Add(drow);

                }
                jobDGV.Rows.AddRange(rows.ToArray());
                foreach (DataGridViewRow jobRow in jobDGV.Rows)
                {
                    if (jobRow.Cells[5].Value.ToString().Trim() == "0")
                    {
                        jobRow.Cells[5].ReadOnly = false;
                    }
                    else
                    {
                        jobRow.Cells[5].ReadOnly = true;
                    }

                    if (jobRow.Cells[6].Value.ToString().Trim() == "0")
                    {
                        jobRow.Cells[6].ReadOnly = false;
                    }
                    else
                    {
                        jobRow.Cells[6].ReadOnly = true;
                    }

                    if (jobRow.Cells[7].Value.ToString().Trim() == "0")
                    {
                        jobRow.Cells[7].ReadOnly = false;
                    }
                    else
                    {
                        jobRow.Cells[7].ReadOnly = true;
                    }

                    if (String.IsNullOrWhiteSpace(jobRow.Cells[8].Value.ToString().Trim()))
                    {
                        jobRow.Cells[8].ReadOnly = false;
                    }
                    else
                    {
                        jobRow.Cells[8].ReadOnly = true;
                    }
                    

                    if (mcData.IsJobLockExistByOtherUser("JP", jobRow.Cells[0].Value.ToString(), loggedInUser))
                    {
                        jobRow.Frozen = true;
                        jobRow.DefaultCellStyle.ForeColor = Color.Red;
                    }

                }
                jobDGV.CurrentCell = jobDGV.Rows[0].Cells[0];

               
                this.Cursor = Cursors.Default;
                // MessageBox.Show(row.ToString());
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobsMissingDataForm.cs", "PopulateDGV(DataTable jobDT)", msg);
                this.Cursor = Cursors.Default;
                return;
            }


        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void jobDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!jobDGV.Focused) { return; }

           
            if (e.ColumnIndex != 5 && e.ColumnIndex != 6 && e.ColumnIndex != 7) { return; }

           
        }

        private void jobDGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!jobDGV.Focused) { return; }
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (jobDGV.CurrentCell.ColumnIndex == 5 || jobDGV.CurrentCell.ColumnIndex == 6 || jobDGV.CurrentCell.ColumnIndex == 7) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar == '.')
            {
                e.Handled = true;
            }
        }

        private void jobDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string jobNo = jobDGV[0, e.RowIndex].Value.ToString();
            int qty = 0;
            string supplier = "";
            if (e.ColumnIndex == 5)//slab M2
            {
                qty = Convert.ToInt32(jobDGV[5, e.RowIndex].Value.ToString());
                string err = mcData.UpdateMissingJobPlannerSlabM2(jobNo, qty);
               // return;
            }
            if (e.ColumnIndex == 6)//beam M2
            {
                qty = Convert.ToInt32(jobDGV[6, e.RowIndex].Value.ToString());
                string err = mcData.UpdateMissingJobPlannerBeamM2(jobNo, qty);
               // return;
            }
            if (e.ColumnIndex == 7)//beam LM
            {
                qty = Convert.ToInt32(jobDGV[7, e.RowIndex].Value.ToString());
                string err = mcData.UpdateMissingJobPlannerBeamLM(jobNo, qty);
              //  return;
            }
            if (e.ColumnIndex == 8)//supplier
            {
                supplier = jobDGV[8, e.RowIndex].Value.ToString();
                string err = mcData.UpdateMissingJobPlannerSupplier(jobNo, supplier);
              //  return;
            }
            int missingCount = mcData.GetNumMissingData(_mode);
            totalSlabM2TextBox.Text = missingCount.ToString();


            return;
        }

        private void jobDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void jobDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void JobsMissingDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void jobDGV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //if((int)jobDGV.Rows[e.RowIndex].Cells[5].Value > 0)
            //{
            //    jobDGV.Rows[e.RowIndex].ReadOnly = true;
            //    jobDGV.Rows[e.RowIndex].Cells[5].ReadOnly = false;

            //}
            //if ((int)jobDGV.Rows[e.RowIndex].Cells[6].Value > 0)
            //{
            //    jobDGV.Rows[e.RowIndex].ReadOnly = true;
            //    jobDGV.Rows[e.RowIndex].Cells[6].ReadOnly = false;

            //}
            //if ((int)jobDGV.Rows[e.RowIndex].Cells[7].Value > 0)
            //{
            //    jobDGV.Rows[e.RowIndex].ReadOnly = true;
            //    jobDGV.Rows[e.RowIndex].Cells[7].ReadOnly = false;

            //}
            //if (!String.IsNullOrWhiteSpace(jobDGV.Rows[e.RowIndex].Cells[8].Value.ToString().Trim()))
            //{
            //    jobDGV.Rows[e.RowIndex].ReadOnly = true;
            //    jobDGV.Rows[e.RowIndex].Cells[8].ReadOnly = false;

            //}
        }

        private void jobDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SuppliersListForm suppForm;

            try
            {
                if (!jobDGV.Focused) { return; }

                if (jobDGV.Rows[e.RowIndex].Cells[0].Value == null)
                {
                    return;
                }

                if(!String.IsNullOrWhiteSpace(jobDGV.Rows[e.RowIndex].Cells[8].Value.ToString()))
                {
                    return;
                }

                if (e.ColumnIndex == 8)
                {
                    //   if (MessageBox.Show("Do you have a PO number for supplier", "Confirm PO Number", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) { return; }
                    string job = jobDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //AddPONumberForm poForm = new AddPONumberForm(job);
                    //poForm.ShowDialog();
                    //jobDGV.Rows[e.RowIndex].Cells[14].Value = poForm.PONumber;

                    suppForm = new SuppliersListForm();
                    

                    suppForm.ShowDialog();
                    string suppShortName = suppForm.Shortname;
                    jobDGV.Rows[e.RowIndex].Cells[8].Value = suppShortName;
                    int rgb1, rgb2, rgb3 = 0;

                    mcData.GetSupplierColourByShortname(suppShortName, out rgb1, out rgb2, out rgb3);
                    if (!String.IsNullOrWhiteSpace(suppShortName))
                    {
                        string err1 = mcData.UpdateJobPlannerSupplierShortName(job, suppShortName);
                        string err2 = mcData.UpdateWhiteBoardSupplierShortName(job, suppShortName);
                    }
                    jobDGV.CurrentCell = jobDGV.Rows[e.RowIndex].Cells[8];
                    jobDGV.Rows[e.RowIndex].Cells[8].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    int missingCount = mcData.GetNumMissingData(_mode);
                    totalSlabM2TextBox.Text = missingCount.ToString();
                    jobDGV.CurrentCell.Selected = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
