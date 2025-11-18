using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MCPApp
{
    public partial class SuppliersReportForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        
        DataTable sourceDT = new DataTable();
      //  DataRow dr;
        ExcelUtlity excel = new ExcelUtlity();

        public SuppliersReportForm()
        {
            InitializeComponent();
        }



        private void BuildSuppliersDGV()
        {
            try
            {
                suppDGV.Columns.Clear();

                //0
                DataGridViewCheckBoxColumn dgvCmbColumn = new DataGridViewCheckBoxColumn();
                dgvCmbColumn.ValueType = typeof(bool);
                dgvCmbColumn.Name = "Chk";
                dgvCmbColumn.Width = 30;
                dgvCmbColumn.HeaderText = "Tick";
                suppDGV.Columns.Add(dgvCmbColumn); 

                
                //1
                DataGridViewTextBoxColumn suppCodeBoxColumn = new DataGridViewTextBoxColumn();
                suppCodeBoxColumn.HeaderText = "Supp Code";
                suppCodeBoxColumn.Width = 100;
                suppCodeBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppCodeBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(suppCodeBoxColumn);

                //2
                DataGridViewTextBoxColumn suppNameTextBoxColumn = new DataGridViewTextBoxColumn();
                suppNameTextBoxColumn.HeaderText = "Supplier Name";
                suppNameTextBoxColumn.Width = 200;
                suppNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppNameTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(suppNameTextBoxColumn);

                //3
                DataGridViewTextBoxColumn shortnameTextBoxColumn = new DataGridViewTextBoxColumn();
                shortnameTextBoxColumn.HeaderText = "Shortname";
                shortnameTextBoxColumn.Width = 100;
                shortnameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                shortnameTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(shortnameTextBoxColumn);

                //4
                DataGridViewCheckBoxColumn excludeSiteAddressColumn = new DataGridViewCheckBoxColumn();
                excludeSiteAddressColumn.ValueType = typeof(bool);
                excludeSiteAddressColumn.Name = "NoAddress";
                excludeSiteAddressColumn.Width = 150;
                excludeSiteAddressColumn.HeaderText = "Exclude Site Address";
                suppDGV.Columns.Add(excludeSiteAddressColumn);

                //5
                DataGridViewCheckBoxColumn excludeCustColumn = new DataGridViewCheckBoxColumn();
                excludeCustColumn.ValueType = typeof(bool);
                excludeCustColumn.Name = "NoCust";
                excludeCustColumn.Width = 150;
                excludeCustColumn.HeaderText = "Exclude Customer";
                suppDGV.Columns.Add(excludeCustColumn); 

                suppDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                suppDGV.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MeltonData mcData = new MeltonData();
                MessageBox.Show("BuildSuppliersDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("SuppliersListForm.cs", "BuildSuppliersDGV()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                return;
            }
        }

        private void PopulateDGV()
        {
            MeltonData mcData = new MeltonData();
            int row = 0;
            int rgb1, rgb2, rgb3 = 255;
            try
            {
                suppDGV.Rows.Clear();
                DataTable dt = mcData.GetAllSuppliers();
                foreach (DataRow dr in dt.Rows)
                {
                    rgb1 = dr["rgb1"] == null ? 255 : Convert.ToInt16(dr["rgb1"].ToString());
                    rgb2 = dr["rgb2"] == null ? 255 : Convert.ToInt16(dr["rgb2"].ToString());
                    rgb3 = dr["rgb3"] == null ? 255 : Convert.ToInt16(dr["rgb3"].ToString());
                    suppDGV.Rows.Add();
                    suppDGV[1, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    suppDGV[1, row].Value = dr["suppCode"].ToString();
                    suppDGV[2, row].Value = dr["suppName"].ToString();
                    suppDGV[3, row].Value = dr["shortname"].ToString();
                    suppDGV[5, row++].Value = true;

                }
                suppDGV.CurrentCell = suppDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("SuppliersListForm.cs", "PopulateDGV()", ex.Message);
                return;

            }


        }

        private void SuppliersReportForm_Load(object sender, EventArgs e)
        {
            this.Text = "Supplier Reports for " + DateTime.Now.ToShortDateString();
            //pathTextBox.Text = @"C:\Users\Abby.Arshad\Documents\TEST";
            BuildSuppliersDGV();
            PopulateDGV();
        }

        private void allButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < suppDGV.Rows.Count; i++)
            {
                suppDGV.Rows[i].Cells[0].Value = true;
            }
        }

        private void noneButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < suppDGV.Rows.Count; i++)
            {
                suppDGV.Rows[i].Cells[0].Value = false;
            }
        }

        private void SelectBFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                pathTextBox.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }  
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(pathTextBox.Text))
            {
                MessageBox.Show(String.Format("[{0}] path does NOT exist", pathTextBox.Text));
                return;
            }
            int numSelected = 0;
            for (int i = 0; i < suppDGV.Rows.Count; i++)
            {
                if (suppDGV.Rows[i].Cells[0].Value == null) { continue; }
                if ((bool)suppDGV.Rows[i].Cells[0].Value == true) { numSelected++; }

            }

            if (numSelected == 0)
            {
                MessageBox.Show("No suppliers have been selected");
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            string suppCode = "";
            string suppName = "";
            string shortName = "";
            bool excludeAddressFlag = false;
            bool excludeCustFlag = false;
            string fileName = "";
            string fullFilePath = "";

            for (int i = 0; i < suppDGV.Rows.Count; i++)
            {
                if (suppDGV.Rows[i].Cells[0].Value == null) { continue; }
                if ((bool)suppDGV.Rows[i].Cells[0].Value == false) { continue; }
                if (suppDGV.Rows[i].Cells[4].Value == null)
                {
                    excludeAddressFlag = false;
                }
                else
                {
                    excludeAddressFlag = (bool)suppDGV.Rows[i].Cells[4].Value ? true : false;
                }

                if (suppDGV.Rows[i].Cells[5].Value == null)
                {
                    excludeCustFlag = false;
                }
                else
                {
                    excludeCustFlag = (bool)suppDGV.Rows[i].Cells[5].Value ? true : false;
                }
                suppCode = suppDGV.Rows[i].Cells[1].Value.ToString();
                suppName = suppDGV.Rows[i].Cells[2].Value.ToString();
                shortName = suppDGV.Rows[i].Cells[3].Value.ToString();
                fileName = $"{shortName.ToUpper()}_{suppCode.ToUpper()}_{DateTime.Now.ToString("ddMMMyyyyhhmmss")}.xlsx";
                fullFilePath = Path.Combine(pathTextBox.Text, fileName);
                sourceDT = mcData.GetOnShopNotOnShopJobsBySupplierDT(shortName);
                label3.Text = $"{shortName.ToUpper()} - A/C {suppCode.ToUpper()}"; 
                GenerateDataTable(excludeAddressFlag,excludeCustFlag, shortName, suppName, suppCode, fullFilePath);
            }
            label3.Text = "*** Supplier reports generated successfully ***";
            this.Cursor = Cursors.Default;
            string message = "Supplier reports successfully created in [" + pathTextBox.Text + "] location." + Environment.NewLine + "" + Environment.NewLine + "Do you wish to check the excel files now?";
            if (MessageBox.Show(message, "Open Excel Files ? ", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                OpenLocationFolder();
            }
            
            return;
        }

        private void OpenLocationFolder()
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", (String)pathTextBox.Text);
            }
            catch (Win32Exception win32Exception)
            {
                //The system cannot find the file specified...
                Console.WriteLine(String.Format("OpenLocationFolder() ERROR - {0}",win32Exception.Message));
            }
        }

        private void GenerateDataTable(bool excludeSiteAddressFlag, bool excludeCustFlag, string tabName, string supplierName,string supplierCode, string fullFilePath)
        {
            DataTable jobsDT = new DataTable();
            string jobNo = "";
            string custName = "";
            string custCode = "";
            string designStatus = "";
            DateTime reqDate;
            Double dateValue;
            jobsDT.Columns.Clear();
            jobsDT.Columns.Add("JobNo", typeof(string));
            jobsDT.Columns.Add("RequiredDate", typeof(Double)); 
            if (!excludeCustFlag)
            {
                jobsDT.Columns.Add("Customer", typeof(string));
            }
            jobsDT.Columns.Add("FloorLevel", typeof(string)); 
            
            if (!excludeSiteAddressFlag)
            {
                jobsDT.Columns.Add("SiteAddress", typeof(string)); 
            }
            jobsDT.Columns.Add("DesignStatus", typeof(string));
            jobsDT.Columns.Add("SupplyType", typeof(string));
            jobsDT.Columns.Add("SlabM2", typeof(string));
            jobsDT.Columns.Add("BeamLM", typeof(string));
            jobsDT.Columns.Add("BeamM2", typeof(string));
            jobsDT.Columns.Add("SupplierRef", typeof(string));
            jobsDT.Columns.Add("Comments", typeof(string));
           
            int counter = 0;
            
            foreach (DataRow row in sourceDT.Rows)
            {
                DataRow dr = jobsDT.NewRow();
                jobNo = row["jobNo"].ToString();
                if(mcData.IsJobCancelled(jobNo)) { continue; }
                custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                custName = mcData.GetCustName(custCode);
                designStatus = mcData.GetDesignStatusByJobNo(jobNo);
                reqDate = Convert.ToDateTime(row["requiredDate"]);
                dateValue = reqDate.ToOADate();
              //  var parsedDate = DateTime.ParseExact(reqDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            //    var formattedDate = reqDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                dr["JobNo"] = jobNo;
                dr["RequiredDate"] = dateValue;
                if (!excludeCustFlag)
                {
                    dr["Customer"] = custName;
                }
                dr["FloorLevel"] = row["floorLevel"].ToString();
                
                if (!excludeSiteAddressFlag)
                {
                    dr["SiteAddress"] = row["siteAddress"].ToString();
                }
                dr["DesignStatus"] = designStatus;
                dr["SupplyType"] = row["supplyType"].ToString();
                dr["SlabM2"] = row["slabM2"].ToString();
                dr["BeamLM"] = row["beamLm"].ToString();
                dr["BeamM2"] = row["beamM2"].ToString();
                dr["SupplierRef"] = row["supplierRef"].ToString();
                dr["Comments"] = row["lastComment"].ToString();
                jobsDT.Rows.Add(dr);
                counter++;

            }
            
            excel.WriteDataTableToExcelQuick(jobsDT, tabName, fullFilePath, $"Supplier : {supplierName} (A/C {supplierCode})" ,2);
            
        }

       
    }
}
