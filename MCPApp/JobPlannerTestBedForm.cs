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
    public partial class JobPlannerTestBedForm : Form
    {
        MeltonData mcData = new MeltonData();
        DataTable dt = new DataTable();
        Logger logger = new Logger();
        private string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

        public JobPlannerTestBedForm()
        {
            InitializeComponent();
        }

        private void JobPlannerTestBedForm_Load(object sender, EventArgs e)
        {
           
            dt = mcData.GetJobPlannerDT();
            DataTable newDT = FormatJobPlannerDT(dt);
            BuildDGV();
            jobDGV.DataSource = dt;
            
            //PopulateDGV(dt);
        }

        private DataTable GetSupplyTypeDT()
        {
            DataTable suppTypeDT = new DataTable();
            suppTypeDT.Columns.Clear();
            suppTypeDT.Columns.Add("suppType", typeof(string));

            List<String> typeList = new List<String>() { "SF", "XF", "SO", "XO" };

            DataRow dr;

            foreach (var type in typeList)
            {
                dr = suppTypeDT.NewRow();
                dr["suppType"] = type;
                suppTypeDT.Rows.Add(dr);
            }
            /*
            DataRow dr = suppTypeDT.NewRow();
            dr["suppType"] = "SO";
            suppTypeDT.Rows.Add(dr);
            dr = suppTypeDT.NewRow();
            dr["suppType"] = "SF";
            suppTypeDT.Rows.Add(dr);
            */
            return suppTypeDT;
        }

        private void BuildDGV()
        {
            try
            {
                DataTable levelsDT = mcData.GetAllFloorLevels();
                BindingSource levelsBindngSource = new BindingSource();
                levelsBindngSource.DataSource = levelsDT;

                BindingSource suppTypeBindngSource = new BindingSource();
                suppTypeBindngSource.DataSource = GetSupplyTypeDT();

                jobDGV.Columns.Clear();

                //0
                DataGridViewTextBoxColumn jobNoBoxColumn = new DataGridViewTextBoxColumn();
                jobNoBoxColumn.Name = "jobNo";
                jobNoBoxColumn.HeaderText = "JobNo";
                jobNoBoxColumn.Width = 70;
                jobNoBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoBoxColumn.ReadOnly = true;
                jobNoBoxColumn.Frozen = true;
                jobDGV.Columns.Add(jobNoBoxColumn);
                


                //1
                DataGridViewTextBoxColumn custBoxColumn = new DataGridViewTextBoxColumn();
                custBoxColumn.Name = "custName";
                custBoxColumn.HeaderText = "Customer";
                custBoxColumn.Width = 150;
                custBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custBoxColumn.ReadOnly = true;
                custBoxColumn.Frozen = true;
                jobDGV.Columns.Add(custBoxColumn);

                //2

                DataGridViewComboBoxColumn levelColumn = new DataGridViewComboBoxColumn();
                levelColumn.Name = "floorLevel";
                levelColumn.DataPropertyName = "Levels";
                levelColumn.HeaderText = "Floor Levsls";
                levelColumn.Width = 150;
                levelColumn.DataSource = levelsBindngSource;
                levelColumn.ValueMember = "level";
                levelColumn.DisplayMember = "level";
                levelColumn.Frozen = true;
                jobDGV.Columns.Add(levelColumn);

                //3
                DataGridViewTextBoxColumn reqDateTextBoxColumn = new DataGridViewTextBoxColumn();
                reqDateTextBoxColumn.Name = "requiredDate";
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
                siteAddressTextBoxColumn.Name = "siteAddress";
                siteAddressTextBoxColumn.HeaderText = "Site Address";
                siteAddressTextBoxColumn.Width = 300;
                siteAddressTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(siteAddressTextBoxColumn);


                //5
                DataGridViewCheckBoxColumn drawnCmbColumn = new DataGridViewCheckBoxColumn();
                drawnCmbColumn.Name = "approved";
                drawnCmbColumn.ValueType = typeof(bool);
                drawnCmbColumn.Name = "Approved";
                drawnCmbColumn.Width = 50;
                drawnCmbColumn.HeaderText = "Apprvd";
                jobDGV.Columns.Add(drawnCmbColumn);

                //6
                DataGridViewCheckBoxColumn approvedCmbColumn = new DataGridViewCheckBoxColumn();
                approvedCmbColumn.Name = "onShop";
                approvedCmbColumn.ValueType = typeof(bool);
                approvedCmbColumn.Name = "OnShop";
                approvedCmbColumn.Width = 50;
                approvedCmbColumn.HeaderText = "On Shop";
                jobDGV.Columns.Add(approvedCmbColumn);

                //7
                DataGridViewTextBoxColumn diffCmbColumn = new DataGridViewTextBoxColumn();
                diffCmbColumn.Name = "daysUnapproved";
                diffCmbColumn.Width = 60;
                diffCmbColumn.HeaderText = "Days Unapproved";
                diffCmbColumn.DefaultCellStyle.ForeColor = Color.Red;
                diffCmbColumn.ReadOnly = true;
                jobDGV.Columns.Add(diffCmbColumn);

                ////8
                //DataGridViewCheckBoxColumn onShopCmbColumn = new DataGridViewCheckBoxColumn();
                //onShopCmbColumn.ValueType = typeof(bool);
                //onShopCmbColumn.Name = "On Shop";
                //onShopCmbColumn.Width = 50;
                //onShopCmbColumn.HeaderText = "On Shop";
                //jobDGV.Columns.Add(onShopCmbColumn);

                //8
                DataGridViewCheckBoxColumn stairsCmbColumn = new DataGridViewCheckBoxColumn();
                stairsCmbColumn.ValueType = typeof(bool);
                stairsCmbColumn.Name = "stairsIncl";
                stairsCmbColumn.Width = 50;
                stairsCmbColumn.HeaderText = "Stairs";
                jobDGV.Columns.Add(stairsCmbColumn);

                //9
                DataGridViewTextBoxColumn slabM2TextBoxColumn = new DataGridViewTextBoxColumn();  //0
                slabM2TextBoxColumn.Name = "slabM2";
                slabM2TextBoxColumn.HeaderText = "Slab M2";
                slabM2TextBoxColumn.Width = 50;
                slabM2TextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(slabM2TextBoxColumn);

                //10
                DataGridViewTextBoxColumn beamM2TextBoxColumn = new DataGridViewTextBoxColumn();  //0
                beamM2TextBoxColumn.Name = "beamM2";
                beamM2TextBoxColumn.HeaderText = "Beam M2";
                beamM2TextBoxColumn.Width = 50;
                beamM2TextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(beamM2TextBoxColumn);

                //11
                DataGridViewTextBoxColumn beamLmTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                beamLmTextBoxColumn.Name = "beamLm";
                beamLmTextBoxColumn.HeaderText = "Beam LM";
                beamLmTextBoxColumn.Width = 50;
                beamLmTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(beamLmTextBoxColumn);

                //12
                DataGridViewTextBoxColumn supplierTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                supplierTextBoxColumn.Name = "productSupplier";
                supplierTextBoxColumn.HeaderText = "Supplier (Dbl Click)";
                supplierTextBoxColumn.Width = 80;
                supplierTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(supplierTextBoxColumn);

                //suppTypeBindngSource
                //13
                DataGridViewComboBoxColumn suppTypeColumn = new DataGridViewComboBoxColumn();
                suppTypeColumn.Name = "supplyType";
                suppTypeColumn.DataPropertyName = "suppType";
                suppTypeColumn.HeaderText = "SuppType";
                suppTypeColumn.Width = 60;
                suppTypeColumn.DataSource = suppTypeBindngSource;
                suppTypeColumn.ValueMember = "suppType";
                suppTypeColumn.DisplayMember = "suppType";
                jobDGV.Columns.Add(suppTypeColumn);

                //14
                DataGridViewTextBoxColumn suppRefTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                suppRefTextBoxColumn.Name = "supplierRef";
                suppRefTextBoxColumn.HeaderText = "Supplier Ref";
                suppRefTextBoxColumn.Width = 80;
                suppRefTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(suppRefTextBoxColumn);

                //15
                DataGridViewTextBoxColumn phasedValueTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                phasedValueTextBoxColumn.Name = "phaseInvValue";
                phasedValueTextBoxColumn.HeaderText = "Phase Value(£)";
                phasedValueTextBoxColumn.Width = 80;
                phasedValueTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                phasedValueTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(phasedValueTextBoxColumn);


                //16
                DataGridViewTextBoxColumn commentTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                commentTextBoxColumn.Name = "lastComment";
                commentTextBoxColumn.HeaderText = "Last Comment (DBL Click)";
                commentTextBoxColumn.Width = 200;
                commentTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(commentTextBoxColumn);

                //17
                DataGridViewTextBoxColumn modifiedDateTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                modifiedDateTextBoxColumn.Name = "dateCreated";
                modifiedDateTextBoxColumn.HeaderText = "Date Created";
                modifiedDateTextBoxColumn.Width = 130;
                modifiedDateTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(modifiedDateTextBoxColumn);

                //18
                DataGridViewTextBoxColumn modifiedByTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                modifiedByTextBoxColumn.Name = "jobCreatedBy";
                modifiedByTextBoxColumn.HeaderText = "Rep";
                modifiedByTextBoxColumn.Width = 30;
                modifiedByTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(modifiedByTextBoxColumn);

                jobDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                jobDGV.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                jobDGV.EnableHeadersVisualStyles = false;
                jobDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                jobDGV.Columns[3].DefaultCellStyle.BackColor = Color.LightGreen;
                jobDGV.Columns[15].DefaultCellStyle.BackColor = Color.Cyan;
                jobDGV.Columns[9].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[10].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[11].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[15].DefaultCellStyle.Format = "D2";

                jobDGV.Columns[0].DataPropertyName = "jobNo";
                jobDGV.Columns[1].DataPropertyName = "custName";
                jobDGV.Columns[2].DataPropertyName = "floorLevel";
                jobDGV.Columns[3].DataPropertyName = "requiredDate";
                jobDGV.Columns[4].DataPropertyName = "siteAddress";
                jobDGV.Columns[5].DataPropertyName = "approved";
                jobDGV.Columns[6].DataPropertyName = "onShop";
                jobDGV.Columns[7].DataPropertyName = "daysUnapproved";
                jobDGV.Columns[8].DataPropertyName = "stairsIncl";
                jobDGV.Columns[9].DataPropertyName = "slabM2";
                jobDGV.Columns[10].DataPropertyName = "beamM2";
                jobDGV.Columns[11].DataPropertyName = "beamLm";
                jobDGV.Columns[12].DataPropertyName = "productSupplier";
                jobDGV.Columns[13].DataPropertyName = "supplyType";
                jobDGV.Columns[14].DataPropertyName = "supplierRef";
                jobDGV.Columns[15].DataPropertyName = "phaseInvValue";
                jobDGV.Columns[16].DataPropertyName = "lastComment";
                jobDGV.Columns[17].DataPropertyName = "dateCreated";
                jobDGV.Columns[18].DataPropertyName = "jobCreatedBy";


                jobDGV.AutoGenerateColumns = false;



            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "BuildDGV()", msg);
                return;
            }





        }

        private DataTable FormatJobPlannerDT(DataTable sourceDT)
        {
            string jobNo = "";
            string custName = "";
            string custCode = "";

            DataTable dt = new DataTable();
            dt.Columns.Clear();
            dt.Columns.Add("jobNo", typeof(string)); //0
            dt.Columns.Add("custName", typeof(string));//1
            dt.Columns.Add("floorLevel", typeof(string));//2
            dt.Columns.Add("requiredDate", typeof(string));//3
            dt.Columns.Add("siteAddress", typeof(string));//4
            dt.Columns.Add("approved", typeof(bool));//5
            dt.Columns.Add("onShop", typeof(bool));//6
            dt.Columns.Add("daysUnapproved", typeof(int));//7
            dt.Columns.Add("stairsIncl", typeof(bool));//8
            dt.Columns.Add("slabM2", typeof(string));//9
            dt.Columns.Add("beamM2", typeof(string));//10
            dt.Columns.Add("beamLm", typeof(string));//11
            dt.Columns.Add("productSupplier", typeof(string));//12
            dt.Columns.Add("supplyType", typeof(string));//13
            dt.Columns.Add("supplierRef", typeof(string));//14
            dt.Columns.Add("phaseInvValue", typeof(string));//15
            dt.Columns.Add("lastComment", typeof(string));//16
            dt.Columns.Add("dateCreated", typeof(string));//17
            dt.Columns.Add("jobCreatedBy", typeof(string));//18

            DataRow row = dt.NewRow();
            foreach (DataRow dr in sourceDT.Rows)
            {
                row = dt.NewRow();
                jobNo = dr["jobNo"].ToString();
                custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                custName = mcData.GetCustName(custCode);
                row["jobNo"] = dr["jobNo"].ToString();
                row["custName"] = custName;
                row["floorLevel"] = dr["floorLevel"].ToString();
                row["requiredDate"] = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().Substring(0, 3) + " " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                row["siteAddress"] = dr["siteAddress"].ToString();
                row["approved"] = dr["approved"].ToString() == "Y" ? true : false;
                row["onshop"] = dr["onshop"].ToString() == "Y" ? true : false;
                row["daysUnapproved"] = dr["approved"].ToString() == "Y" ? 0 : mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                row["stairsIncl"] = dr["stairsIncl"].ToString() == "Y" ? true : false;
                row["slabM2"] = dr["slabM2"].ToString();
                row["beamM2"] = dr["beamM2"].ToString();
                row["beamLm"] = dr["beamLm"].ToString();
                row["productSupplier"] = dr["productSupplier"].ToString();
                row["supplyType"] = dr["supplyType"].ToString();
                row["supplierRef"] = dr["supplierRef"].ToString();
                row["phaseInvValue"] = dr["phaseInvValue"].ToString();
                row["lastComment"] = mcData.GetLastComment(jobNo);
                row["dateCreated"] = Convert.ToDateTime(dr["jobCreatedDate"].ToString()).ToString("dd/MMM/yyyy hh:mm tt");
                row["jobCreatedBy"] = dr["jobCreatedBy"].ToString();
                dt.Rows.Add(row);
            }
            return dt;
        }

        private void PopulateDGV(DataTable jobDT)
        {
            if (jobDT == null) { return; }
            if (jobDT.Rows.Count == 0) { return; }
            int row = 0;
            int rgb1 = 255;
            int rgb2 = 255;
            int rgb3 = 255;
            string suppShortname = "";
            int daysDiff = 0;
            string approved = "";
            string jobNo = "";
            string custName = "";
            string custCode = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                jobDGV.Rows.Clear();
                foreach (DataRow dr in jobDT.Rows)
                {
                    jobNo = dr["jobNo"].ToString();
                    custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                    custName = mcData.GetCustName(custCode);
                    approved = dr["approved"].ToString();
                    daysDiff = mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                    suppShortname = dr["productSupplier"].ToString();
                    mcData.GetSupplierColourByShortname(suppShortname, out rgb1, out rgb2, out rgb3);
                    jobDGV.Rows.Add();
                    jobDGV[0, row].Value = jobNo;
                    jobDGV[1, row].Value = custName;
                    jobDGV[2, row].Value = dr["floorLevel"].ToString();
                    jobDGV[3, row].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().Substring(0, 3) + " " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                    jobDGV[4, row].Value = dr["siteAddress"].ToString();
                    jobDGV[5, row].Value = dr["approved"].ToString() == "Y" ? true : false;
                    jobDGV[6, row].Value = dr["onshop"].ToString() == "Y" ? true : false;
                    jobDGV[7, row].Value = dr["approved"].ToString() == "Y" ? 0 : mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                    jobDGV[7, row].Style.ForeColor = approved != "Y" && daysDiff > 0 ? Color.Red : Color.Black;
                    //   jobDGV[8, row].Value = dr["OnShop"].ToString() == "Y" ? true : false;
                    jobDGV[8, row].Value = dr["stairsIncl"].ToString() == "Y" ? true : false;
                    jobDGV[9, row].Value = dr["slabM2"].ToString();
                    jobDGV[10, row].Value = dr["beamM2"].ToString();
                    jobDGV[11, row].Value = dr["beamLm"].ToString();
                    jobDGV[12, row].Value = dr["productSupplier"].ToString();
                    jobDGV[12, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    jobDGV[13, row].Value = dr["supplyType"].ToString();
                    jobDGV[14, row].Value = dr["supplierRef"].ToString();
                    jobDGV[15, row].Value = dr["phaseInvValue"].ToString();
                    jobDGV[16, row].Value = mcData.GetLastComment(dr["jobNo"].ToString());

                    jobDGV[17, row].Value = Convert.ToDateTime(dr["jobCreatedDate"].ToString()).ToString("dd/MMM/yyyy hh:mm tt");
                    jobDGV[18, row++].Value = dr["jobCreatedBy"].ToString();


                }
                //foreach (DataGridViewRow jobRow in jobDGV.Rows)
                //{
                //    if (mcData.IsJobCompleted(jobRow.Cells[0].Value.ToString()))
                //    {
                //        jobRow.DefaultCellStyle.ForeColor = Color.Gray;
                //    }
                //    else
                //    {
                //        jobRow.DefaultCellStyle.ForeColor = Color.Black;
                //    }

                //    if (mcData.IsJobLockExistByOtherUser("JP", jobRow.Cells[0].Value.ToString(), loggedInUser))
                //    {
                //        jobRow.Frozen = true;
                //        jobRow.DefaultCellStyle.ForeColor = Color.Red;
                //    }
                //}
                jobDGV.CurrentCell = jobDGV.Rows[0].Cells[0];

                //DisplayTotalInvValue();
                //DisplayTotalBeamM2();
                //DisplayTotalSlabM2();
                this.Cursor = Cursors.Default;
                // MessageBox.Show(row.ToString());
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "PopulateDGV(DataTable jobDT)", msg);
                this.Cursor = Cursors.Default;
                return;
            }


        }

        private void jobDGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                BuildDGV();
                PopulateDGV(dt);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void jobDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
    }
}
