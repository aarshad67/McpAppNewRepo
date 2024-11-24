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
using System.Configuration;
using System.Globalization;
//using Microsoft.Office.Interop.Excel;

namespace MCPApp
{
    public partial class JobPlannerForm : Form
    {
        MeltonData mcData = new MeltonData();
        DataTable sourceDT = new DataTable();
        DataTable jobDT = new DataTable();
        DataTable newJobDT = new DataTable();
        DataTable dgvDT = new DataTable();
        ExcelUtlity excel = new ExcelUtlity();
        int parentJobNo = 0;
        string jobPlannerMode = "ALL";

        Logger logger = new Logger();
        private string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

        private int rowIndex = 0;
        private int colIndex = 0;

        public JobPlannerForm(string mode)
        {
            InitializeComponent();
            jobPlannerMode = mode;
        }

        public JobPlannerForm(DataTable dt)
        {
            InitializeComponent();
            sourceDT = dt;
        }

        public JobPlannerForm(DataTable dt, int parentJob)
        {
            InitializeComponent();
            sourceDT = dt;
            parentJobNo = parentJob;
        }

        public JobPlannerForm(int parentJob)
        {
            InitializeComponent();
            parentJobNo = parentJob;
        }

        private void JobPlannerForm_Load(object sender, EventArgs e)
        {
            this.Text = "Job Planner (" + jobPlannerMode + ") - " + DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.ToLongDateString();
            currentYearFiltersGroupBox.Text = "Jobs in " + DateTime.Now.Year.ToString();
            nextYearFiltersGroupBox.Text = "Jobs in " + DateTime.Now.AddYears(1).Year.ToString();
            BuildDGV();
            if (sourceDT.Rows.Count > 0)
            {
                dgvDT = sourceDT;
                PopulateDGV(dgvDT);
                
            }
            else if (parentJobNo > 0)
            {
                dgvDT = mcData.GetJobPlannerDT();
                PopulateDGV(dgvDT);
                 
                int rowIndex = -1;
                foreach (DataGridViewRow row in jobDGV.Rows)
                {
                    if (row.Cells[0].Value.ToString().Contains(parentJobNo.ToString()))
                    {
                        rowIndex = row.Index;
                        break;
                    }
                }
                if (rowIndex >= 0)
                {
                    jobDGV.CurrentCell = jobDGV[0, rowIndex];
                }

            }
            else
            {
                if (jobPlannerMode == "BEAM")
                {
                    dgvDT = mcData.GetBeamJobPlannerDT();
                    PopulateDGV(dgvDT);
                }
                else if (jobPlannerMode == "SLAB")
                {
                    dgvDT = mcData.GetSlabJobPlannerDT();
                    PopulateDGV(dgvDT);
                }
                else
                {
                    dgvDT = mcData.GetJobPlannerDT();
                    PopulateDGV(dgvDT);
                }

            }
            //DisplayTotalInvValue();
            //DisplayTotalBeamM2();
            //DisplayTotalSlabM2();
            rbBoth.Checked = true;
            rbInProgress.Checked = true;
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

                DataTable designersDT = mcData.GetAllDesigners();
                BindingSource designerBindingSource = new BindingSource();
                designerBindingSource.DataSource = designersDT;

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

                DataGridViewComboBoxColumn levelColumn = new DataGridViewComboBoxColumn();
                levelColumn.DataPropertyName = "Levels";
                levelColumn.HeaderText = "Floor Levels";
                levelColumn.Width = 150;
                levelColumn.DataSource = levelsBindngSource;
                levelColumn.ValueMember = "level";
                levelColumn.DisplayMember = "level";
                levelColumn.Frozen = true;
                jobDGV.Columns.Add(levelColumn);

                //3
                DataGridViewTextBoxColumn reqDateTextBoxColumn = new DataGridViewTextBoxColumn();
                reqDateTextBoxColumn.HeaderText = "Required Date (Dbl Click)";
                reqDateTextBoxColumn.ValueType = typeof(DateTime);
                reqDateTextBoxColumn.Width = 120;
                reqDateTextBoxColumn.DefaultCellStyle.Format = "dd/MM/YYYY";
                reqDateTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                reqDateTextBoxColumn.ReadOnly = true;
                reqDateTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                reqDateTextBoxColumn.Frozen = true;
                jobDGV.Columns.Add(reqDateTextBoxColumn);

                //4
                DataGridViewTextBoxColumn designDateColumn = new DataGridViewTextBoxColumn();
                designDateColumn.HeaderText = "Design Date (Dbl Click)";
                designDateColumn.ValueType = typeof(DateTime);
                designDateColumn.Width = 80;
                designDateColumn.DefaultCellStyle.Format = "dd/MM/YYYY";
                designDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                designDateColumn.ReadOnly = true;
                designDateColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                designDateColumn.Frozen = false;
                jobDGV.Columns.Add(designDateColumn);

                //5
                DataGridViewTextBoxColumn siteAddressTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                siteAddressTextBoxColumn.HeaderText = "Site Address";
                siteAddressTextBoxColumn.Width = 300;
                siteAddressTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(siteAddressTextBoxColumn);

                //6
                DataGridViewComboBoxColumn designerColumn = new DataGridViewComboBoxColumn();
                designerColumn.Name = "Designer";
                designerColumn.Width = 80;
                designerColumn.HeaderText = "Designer";
                designerColumn.DataSource = designerBindingSource;
                designerColumn.ValueMember = "designer";
                designerColumn.DisplayMember = "designer";
                designerColumn.ReadOnly = false;
                jobDGV.Columns.Add(designerColumn);


                //7
                DataGridViewCheckBoxColumn approvedColumn = new DataGridViewCheckBoxColumn();
                approvedColumn.ValueType = typeof(bool);
                approvedColumn.Name = "Approved";
                approvedColumn.Width = 50;
                approvedColumn.HeaderText = "Apprvd";
                jobDGV.Columns.Add(approvedColumn);

                //8
                DataGridViewCheckBoxColumn onShopColumn = new DataGridViewCheckBoxColumn();
                onShopColumn.ValueType = typeof(bool);
                onShopColumn.Name = "OnShop";
                onShopColumn.Width = 50;
                onShopColumn.HeaderText = "On Shop";
                jobDGV.Columns.Add(onShopColumn);

                

                //9
                DataGridViewCheckBoxColumn stairsCmbColumn = new DataGridViewCheckBoxColumn();
                stairsCmbColumn.ValueType = typeof(bool);
                stairsCmbColumn.Name = "Stairs";
                stairsCmbColumn.Width = 50;
                stairsCmbColumn.HeaderText = "Stairs";
                jobDGV.Columns.Add(stairsCmbColumn);

                //10
                DataGridViewTextBoxColumn slabM2TextBoxColumn = new DataGridViewTextBoxColumn();  //0
                slabM2TextBoxColumn.HeaderText = "Slab M2";
                slabM2TextBoxColumn.Width = 50;
                slabM2TextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(slabM2TextBoxColumn);

                //11
                DataGridViewTextBoxColumn beamLmTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                beamLmTextBoxColumn.HeaderText = "Beam LM";
                beamLmTextBoxColumn.Width = 50;
                beamLmTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(beamLmTextBoxColumn);

                //12
                DataGridViewTextBoxColumn beamM2TextBoxColumn = new DataGridViewTextBoxColumn();  //0
                beamM2TextBoxColumn.HeaderText = "Beam M2";
                beamM2TextBoxColumn.Width = 50;
                beamM2TextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(beamM2TextBoxColumn);

                //13
                DataGridViewTextBoxColumn supplierTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                supplierTextBoxColumn.HeaderText = "Supplier (Dbl Click)";
                supplierTextBoxColumn.Width = 80;
                supplierTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(supplierTextBoxColumn);

                //suppTypeBindngSource
                //14
                DataGridViewComboBoxColumn suppTypeColumn = new DataGridViewComboBoxColumn();
                suppTypeColumn.DataPropertyName = "suppType";
                suppTypeColumn.HeaderText = "SuppType";
                suppTypeColumn.Width = 60;
                suppTypeColumn.DataSource = suppTypeBindngSource;
                suppTypeColumn.ValueMember = "suppType";
                suppTypeColumn.DisplayMember = "suppType";
                jobDGV.Columns.Add(suppTypeColumn);

                //15
                DataGridViewTextBoxColumn suppRefTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                suppRefTextBoxColumn.HeaderText = "Supplier Ref";
                suppRefTextBoxColumn.Width = 80;
                suppRefTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(suppRefTextBoxColumn);

                //16
                DataGridViewTextBoxColumn phasedValueTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                phasedValueTextBoxColumn.HeaderText = "Phase Value(£)";
                phasedValueTextBoxColumn.Width = 80;
                phasedValueTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                phasedValueTextBoxColumn.DefaultCellStyle.Format = "D2";
                phasedValueTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(phasedValueTextBoxColumn);

                //17
                DataGridViewTextBoxColumn jobMarginColumn = new DataGridViewTextBoxColumn();  //0
                jobMarginColumn.HeaderText = "Job Mgn(£)";
                jobMarginColumn.Width = 80;
                jobMarginColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                jobMarginColumn.DefaultCellStyle.Format = "D2";
                jobMarginColumn.ReadOnly = false;
                jobDGV.Columns.Add(jobMarginColumn);


                //18
                DataGridViewTextBoxColumn commentTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                commentTextBoxColumn.HeaderText = "Last Comment (DBL Click)";
                commentTextBoxColumn.Width = 200;
                commentTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(commentTextBoxColumn);

                //19
                DataGridViewTextBoxColumn modifiedDateTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                modifiedDateTextBoxColumn.HeaderText = "Date Created";
                modifiedDateTextBoxColumn.Width = 130;
                modifiedDateTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(modifiedDateTextBoxColumn);

                //20
                DataGridViewTextBoxColumn modifiedByTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                modifiedByTextBoxColumn.HeaderText = "Rep";
                modifiedByTextBoxColumn.Width = 30;
                modifiedByTextBoxColumn.ReadOnly = true;
                jobDGV.Columns.Add(modifiedByTextBoxColumn);

                jobDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                jobDGV.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                jobDGV.EnableHeadersVisualStyles = false;
                jobDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                jobDGV.Columns[3].DefaultCellStyle.BackColor = Color.LightGreen;
                jobDGV.Columns[16].DefaultCellStyle.BackColor = Color.Cyan;
                jobDGV.Columns[17].DefaultCellStyle.BackColor = Color.Cyan;
                jobDGV.Columns[10].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[11].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[12].DefaultCellStyle.Format = "D2";
                jobDGV.Columns[16].DefaultCellStyle.Format = "N2";
                jobDGV.Columns[17].DefaultCellStyle.Format = "N2";
                



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

        private void PopulateDGV(DataTable jobDT)
        {
            if (jobDT == null) { return; }
            if (jobDT.Rows.Count == 0) { return; }
            int row = 0;
            int rgb1, rgb2, rgb3 = 255;
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
                List<DataGridViewRow> rows = new List<DataGridViewRow>();
                foreach (DataRow dr in jobDT.Rows)
                {

                    jobNo = dr["jobNo"].ToString();
                    //drow.Cells[0].Value = dr["jobNo"].ToString();
                    custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                    custName = mcData.GetCustName(custCode);
                    approved = dr["approved"].ToString();
                    daysDiff = mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                    suppShortname = dr["productSupplier"].ToString();
                    mcData.GetSupplierColourByShortname(suppShortname, out rgb1, out rgb2, out rgb3);
                    //jobDGV.Rows.Add();
                    DataGridViewRow drow = new DataGridViewRow();
                    drow.CreateCells(jobDGV);
                    drow.Cells[0].Value = jobNo;
                    drow.Cells[1].Value = custName;
                    drow.Cells[2].Value = dr["floorLevel"].ToString();
                    drow.Cells[3].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().Substring(0, 3) + " " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                    drow.Cells[4].Value = Convert.ToDateTime(dr["designDate"].ToString()).ToShortDateString();
                    drow.Cells[5].Value = dr["siteAddress"].ToString();
                    drow.Cells[6].Value = dr["dman"].ToString();
                    drow.Cells[7].Value = dr["approved"].ToString() == "Y" ? true : false;
                    drow.Cells[8].Value = dr["onshop"].ToString() == "Y" ? true : false;
                    // drow.Cells[7].Value = dr["approved"].ToString() == "Y" ? 0 : mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                    // drow.Cells[7].Style.ForeColor = approved != "Y" && daysDiff > 0 ? Color.Red : Color.Black;

                    drow.Cells[9].Value = dr["stairsIncl"].ToString() == "Y" ? true : false;
                    drow.Cells[10].Value = dr["slabM2"].ToString();
                    drow.Cells[11].Value = dr["beamLm"].ToString();
                    drow.Cells[12].Value = dr["beamM2"].ToString();
                    drow.Cells[13].Value = dr["productSupplier"].ToString();
                    drow.Cells[13].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    drow.Cells[14].Value = dr["supplyType"].ToString();
                    drow.Cells[15].Value = dr["supplierRef"].ToString();
                    drow.Cells[16].Value = dr["phaseInvValue"].ToString();
                    drow.Cells[17].Value = dr["jobMgnValue"].ToString();
                    drow.Cells[18].Value = mcData.GetLastComment(dr["jobNo"].ToString());

                    drow.Cells[19].Value = Convert.ToDateTime(dr["jobCreatedDate"].ToString()).ToString("dd/MMM/yyyy hh:mm tt");
                    drow.Cells[20].Value = dr["jobCreatedBy"].ToString();
                    rows.Add(drow);

                }
                jobDGV.Rows.AddRange(rows.ToArray());
                foreach (DataGridViewRow jobRow in jobDGV.Rows)
                {
                    if (mcData.IsJobCompleted(jobRow.Cells[0].Value.ToString()))
                    {
                        jobRow.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    else
                    {
                        jobRow.DefaultCellStyle.ForeColor = Color.Black;
                    }

                    if (mcData.IsJobLockExistByOtherUser("JP", jobRow.Cells[0].Value.ToString(), loggedInUser))
                    {
                        jobRow.Frozen = true;
                        jobRow.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                jobDGV.CurrentCell = jobDGV.Rows[0].Cells[0];

                DisplayTotalInvValue(); 
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                this.Cursor = Cursors.Default;
                // MessageBox.Show(row.ToString());
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV(DataTable jobDT) Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "PopulateDGV(DataTable jobDT)", msg);
                this.Cursor = Cursors.Default;
                return;
            }


        }

        private void PopulateDGVByParentJob(int parentJobNo)
        {
            DataTable jobDT = mcData.GetJobPlannerDT(parentJobNo);
            if (jobDT.Rows.Count == 0) { return; }
            int row = 0;
            int rgb1, rgb2, rgb3 = 255;
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
                List<DataGridViewRow> rows = new List<DataGridViewRow>();
                foreach (DataRow dr in jobDT.Rows)
                {
                    jobNo = dr["jobNo"].ToString();
                    custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                    custName = mcData.GetCustName(custCode);
                    approved = dr["approved"].ToString();
                    daysDiff = mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                    suppShortname = dr["productSupplier"].ToString();
                    mcData.GetSupplierColourByShortname(suppShortname, out rgb1, out rgb2, out rgb3);
                    //jobDGV.Rows.Add();
                    DataGridViewRow drow = new DataGridViewRow();
                    drow.CreateCells(jobDGV);
                    drow.Cells[0].Value = jobNo;
                    drow.Cells[1].Value = custName;
                    drow.Cells[2].Value = dr["floorLevel"].ToString();
                    drow.Cells[3].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().Substring(0, 3) + " " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                    drow.Cells[4].Value = Convert.ToDateTime(dr["designDate"].ToString()).ToShortDateString();
                    drow.Cells[5].Value = dr["siteAddress"].ToString();
                    drow.Cells[6].Value = dr["dman"].ToString();
                    drow.Cells[7].Value = dr["approved"].ToString() == "Y" ? true : false;
                    drow.Cells[8].Value = dr["onshop"].ToString() == "Y" ? true : false;
                    //drow.Cells[7].Value = dr["approved"].ToString() == "Y" ? 0 : mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                    //drow.Cells[7].Style.ForeColor = approved != "Y" && daysDiff > 0 ? Color.Red : Color.Black;
                    drow.Cells[9].Value = dr["stairsIncl"].ToString() == "Y" ? true : false;
                    drow.Cells[10].Value = dr["slabM2"].ToString();
                    drow.Cells[11].Value = dr["beamLm"].ToString();
                    drow.Cells[12].Value = dr["beamM2"].ToString();
                    drow.Cells[13].Value = dr["productSupplier"].ToString();
                    drow.Cells[13].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    drow.Cells[14].Value = dr["supplyType"].ToString();
                    drow.Cells[15].Value = dr["supplierRef"].ToString();
                    drow.Cells[16].Value = dr["phaseInvValue"].ToString();
                    drow.Cells[17].Value = dr["jobMgnValue"].ToString();
                    drow.Cells[18].Value = mcData.GetLastComment(dr["jobNo"].ToString());
                    drow.Cells[19].Value = Convert.ToDateTime(dr["jobCreatedDate"].ToString()).ToString("dd/MMM/yyyy hh:mm tt");
                    drow.Cells[20].Value = dr["jobCreatedBy"].ToString();
                    rows.Add(drow);
                }
                jobDGV.Rows.AddRange(rows.ToArray());
                foreach (DataGridViewRow jobRow in jobDGV.Rows)
                {
                    if (mcData.IsJobCompleted(jobRow.Cells[0].Value.ToString()))
                    {
                        jobRow.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    else
                    {
                        jobRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                jobDGV.CurrentCell = jobDGV.Rows[0].Cells[0];

                DisplayTotalInvValue();
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                //   MessageBox.Show(row.ToString());
                this.Cursor = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", String.Format("PopulateDGVByParentJob({0})", parentJobNo.ToString()), msg);
                return;
            }


        }

        private void PopulateDGVByQry(string qry)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable jobDT = mcData.GetJobPlannerDTByQry(qry);
                if (jobDT.Rows.Count == 0)
                {
                    MessageBox.Show("No Jobs found");
                    this.Cursor = Cursors.Default;
                    return;
                }
                int row = 0;
                int rgb1, rgb2, rgb3 = 255;
                string suppShortname = "";
                int daysDiff = 0;
                string approved = "";
                string jobNo = "";
                string custName = "";
                string custCode = "";
                jobDGV.Rows.Clear();
                List<DataGridViewRow> rows = new List<DataGridViewRow>();
                foreach (DataRow dr in jobDT.Rows)
                {
                    jobNo = dr["jobNo"].ToString();
                    custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                    custName = mcData.GetCustName(custCode);
                    approved = dr["approved"].ToString();
                    daysDiff = mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                    suppShortname = dr["productSupplier"].ToString();
                    mcData.GetSupplierColourByShortname(suppShortname, out rgb1, out rgb2, out rgb3);
                    //jobDGV.Rows.Add();
                    DataGridViewRow drow = new DataGridViewRow();
                    drow.CreateCells(jobDGV);
                    drow.Cells[0].Value = jobNo;
                    drow.Cells[1].Value = custName;
                    drow.Cells[2].Value = dr["floorLevel"].ToString();
                    drow.Cells[3].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().Substring(0, 3) + " " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                    drow.Cells[4].Value = Convert.ToDateTime(dr["designDate"].ToString()).ToShortDateString();
                    drow.Cells[5].Value = dr["siteAddress"].ToString();
                    drow.Cells[6].Value = dr["dman"].ToString();
                    drow.Cells[7].Value = dr["approved"].ToString() == "Y" ? true : false;
                    drow.Cells[8].Value = dr["onshop"].ToString() == "Y" ? true : false;
                    //drow.Cells[7].Value = dr["approved"].ToString() == "Y" ? 0 : daysDiff;// mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["jobCreatedDate"].ToString()));
                    //drow.Cells[7].Style.ForeColor = approved != "Y" && daysDiff > 0 ? Color.Red : Color.Black;
                    drow.Cells[9].Value = dr["stairsIncl"].ToString() == "Y" ? true : false;
                    drow.Cells[10].Value = dr["slabM2"].ToString();
                    drow.Cells[11].Value = dr["beamLm"].ToString();
                    drow.Cells[12].Value = dr["beamM2"].ToString();
                    drow.Cells[13].Value = dr["productSupplier"].ToString();
                    drow.Cells[13].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    drow.Cells[14].Value = dr["supplyType"].ToString();
                    drow.Cells[15].Value = dr["supplierRef"].ToString();
                    drow.Cells[16].Value = dr["phaseInvValue"].ToString();
                    drow.Cells[17].Value = dr["jobMgnValue"].ToString();
                    drow.Cells[18].Value = mcData.GetLastComment(dr["jobNo"].ToString());
                    drow.Cells[19].Value = Convert.ToDateTime(dr["jobCreatedDate"].ToString()).ToString("dd/MMM/yyyy hh:mm tt");
                    drow.Cells[20].Value = dr["jobCreatedBy"].ToString();
                    rows.Add(drow);

                }
                jobDGV.Rows.AddRange(rows.ToArray());
                foreach (DataGridViewRow jobRow in jobDGV.Rows)
                {
                    if (mcData.IsJobCompleted(jobRow.Cells[0].Value.ToString()))
                    {
                        jobRow.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    else
                    {
                        jobRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                jobDGV.CurrentCell = jobDGV.Rows[0].Cells[0];

                DisplayTotalInvValue();
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                this.Cursor = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", String.Format("PopulateDGVByQry({0})", qry), msg);
                return;
            }


        }

        private void UpdatedJobsDGVToDB()
        {
            int parentJobNo;
            string jobNo;
            string phaseNo;
            string floorLevel;
            DateTime requiredDate;
            DateTime designDate;
            string siteAddress;
            string drawn;
            string approved;
            string onshop;
            string dman;
            string stairsIncl;
            int slabM2;
            int beamM2;
            int beamLm;
            string supplyType;
            string suppShortname;
            string supplierRef;
            string lastComment;
            decimal phaseInvValue;
            decimal jobMgnValue;
            string sortType;


            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[0].Value == null || jobDGV.Rows[i].Cells[0].Value.ToString().Length != 8)
                    {
                        break;
                    }
                    parentJobNo = Convert.ToInt32(jobDGV.Rows[i].Cells[0].Value.ToString().Substring(0, 5));
                    jobNo = jobDGV.Rows[i].Cells[0].Value.ToString();
                    if(mcData.IsJobCompleted(jobNo)) { continue; }
                    phaseNo = jobDGV.Rows[i].Cells[0].Value.ToString().Substring(6, 2);
                    floorLevel = jobDGV.Rows[i].Cells[2].Value.ToString();
                    requiredDate = Convert.ToDateTime(jobDGV.Rows[i].Cells[3].Value);
                    designDate = Convert.ToDateTime(jobDGV.Rows[i].Cells[4].Value);
                    siteAddress = jobDGV.Rows[i].Cells[5].Value.ToString();
                    dman = jobDGV.Rows[i].Cells[6].Value.ToString();
                    approved = (bool)jobDGV.Rows[i].Cells[7].Value ? "Y" : "N";
                    onshop = (bool)jobDGV.Rows[i].Cells[8].Value ? "Y" : "N";
                    stairsIncl = (bool)jobDGV.Rows[i].Cells[9].Value ? "Y" : "N";
                    slabM2 = Convert.ToInt32(jobDGV.Rows[i].Cells[10].Value);
                    beamLm = Convert.ToInt32(jobDGV.Rows[i].Cells[11].Value);
                    beamM2 = Convert.ToInt32(jobDGV.Rows[i].Cells[12].Value);
                    suppShortname = jobDGV.Rows[i].Cells[13].Value.ToString();
                    supplyType = jobDGV.Rows[i].Cells[14].Value.ToString();
                    sortType = "S" + supplyType.Substring(1, 1);

                    
                    supplierRef = jobDGV.Rows[i].Cells[15].Value.ToString();
                    
                    phaseInvValue = jobDGV.Rows[i].Cells[16].Value == null || jobDGV.Rows[i].Cells[16].Value == "" ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[16].Value);
                    jobMgnValue = jobDGV.Rows[i].Cells[17].Value == null || jobDGV.Rows[i].Cells[17].Value == "" ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[17].Value);
                    lastComment = jobDGV.Rows[i].Cells[18].Value.ToString();
                    string mon = "";
                    string tue = "";
                    string wed = "";
                    string thu = "";
                    string fri = "";
                    string sat = "";
                    string sun = "";
                    string dayValue = "";

                    mcData.GetWhiteboardDays(jobNo, out mon, out tue, out wed, out thu, out fri, out sat, out sun);

                    if (!String.IsNullOrWhiteSpace(mon))
                    {
                        dayValue = mon;
                    }
                    if (!String.IsNullOrWhiteSpace(tue))
                    {
                        dayValue = tue;
                    }
                    if (!String.IsNullOrWhiteSpace(wed))
                    {
                        dayValue = wed;
                    }
                    if (!String.IsNullOrWhiteSpace(thu))
                    {
                        dayValue = thu;
                    }
                    if (!String.IsNullOrWhiteSpace(fri))
                    {
                        dayValue = fri;
                    }
                    if (!String.IsNullOrWhiteSpace(sat))
                    {
                        dayValue = sat;
                    }
                    if (!String.IsNullOrWhiteSpace(sun))
                    {
                        dayValue = sun;
                    }


                    if (requiredDate.DayOfWeek.ToString().ToUpper() == "MONDAY")
                    {
                        mon = dayValue;
                        tue = "";
                        wed = "";
                        thu = "";
                        fri = "";
                        sat = "";
                        sun = "";
                    }
                    if (requiredDate.DayOfWeek.ToString().ToUpper() == "TUESDAY")
                    {
                        tue = dayValue;
                        mon = "";
                        wed = "";
                        thu = "";
                        fri = "";
                        sat = "";
                        sun = "";
                    }
                    if (requiredDate.DayOfWeek.ToString().ToUpper() == "WEDNESDAY")
                    {
                        wed = dayValue;
                        mon = "";
                        tue = "";
                        thu = "";
                        fri = "";
                        sat = "";
                        sun = "";
                    }
                    if (requiredDate.DayOfWeek.ToString().ToUpper() == "THURSDAY")
                    {
                        thu = dayValue;
                        mon = "";
                        tue = "";
                        wed = "";
                        fri = "";
                        sat = "";
                        sun = "";
                    }
                    if (requiredDate.DayOfWeek.ToString().ToUpper() == "FRIDAY")
                    {
                        fri = dayValue;
                        mon = "";
                        tue = "";
                        wed = "";
                        thu = "";
                        sat = "";
                        sun = "";
                    }
                    if (requiredDate.DayOfWeek.ToString().ToUpper() == "SATURDAY")
                    {
                        sat = dayValue;
                        mon = "";
                        tue = "";
                        wed = "";
                        thu = "";
                        fri = "";
                        sun = "";
                    }
                    if (requiredDate.DayOfWeek.ToString().ToUpper() == "SUNDAY")
                    {
                        sun = dayValue;
                        mon = "";
                        tue = "";
                        wed = "";
                        thu = "";
                        fri = "";
                        sat = "";
                    }

                    string err = mcData.UpdateJobPlanner(jobNo,  floorLevel, requiredDate, siteAddress,approved,onshop, 
                            stairsIncl, slabM2, beamM2, beamLm, supplyType,suppShortname,  supplierRef, lastComment, phaseInvValue, sortType, designDate, jobMgnValue,dman );
                    if (err != "OK")
                    {
                        MessageBox.Show(String.Format("UpdatedJobsDGVToDB() ERROR : {0}", err));
                        break;
                    }
                    string wbErr = mcData.UpdateWhiteboardViaJobPlanner(jobNo, floorLevel, requiredDate, siteAddress, slabM2, beamM2, supplyType, suppShortname, stairsIncl, lastComment, mon, tue, wed, thu, fri, sat, sun, phaseInvValue, sortType);
                    if (wbErr != "OK")
                    {
                        MessageBox.Show(String.Format("UpdatedJobsDGVToDB() ERROR : {0}", wbErr));
                        break;
                    }
                    string dbErr = mcData.UpdateDesignBoardJobFromJP(jobNo, designDate, floorLevel, suppShortname,
                                        supplierRef, stairsIncl, supplyType, slabM2, beamM2, beamLm,
                                        mon, tue, wed, thu, fri, sat, sun, sortType);
                }
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("UpdatedJobsDGVToDB() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "UpdatedJobsDGVToDB()", msg);
                return;
            }

        }

        private string JobsDGVToNewDatatable()
        {
            try
            {


                newJobDT.Columns.Clear();
                newJobDT.Columns.Add("jobNo", typeof(string)); //0
                newJobDT.Columns.Add("parentJobNo", typeof(int)); //--
                newJobDT.Columns.Add("phaseNo", typeof(string)); //--
                newJobDT.Columns.Add("floorLevel", typeof(string));//2
                newJobDT.Columns.Add("requiredDate", typeof(DateTime));//3
                newJobDT.Columns.Add("siteAddress", typeof(string));//4
                newJobDT.Columns.Add("Drawn", typeof(string));//5
                newJobDT.Columns.Add("Approved", typeof(string));//6
                newJobDT.Columns.Add("OnShop", typeof(string));//6
                newJobDT.Columns.Add("stairsIncl", typeof(string));//8
                newJobDT.Columns.Add("slabM2", typeof(int));//9
                newJobDT.Columns.Add("beamM2", typeof(int));//10
                newJobDT.Columns.Add("beamLm", typeof(int));//11
                newJobDT.Columns.Add("supplyType", typeof(string));//13
                newJobDT.Columns.Add("productSupplier", typeof(string));//12
                newJobDT.Columns.Add("supplierRef", typeof(string));//14
                newJobDT.Columns.Add("lastComment", typeof(string));//17
                newJobDT.Columns.Add("phaseInvValue", typeof(decimal));//15
                newJobDT.Columns.Add("completedFlag", typeof(string));//--
                newJobDT.Columns.Add("modifiedDate", typeof(DateTime));//18
                newJobDT.Columns.Add("modifiedBy", typeof(string));//19
                newJobDT.Columns.Add("jobCreatedDate", typeof(DateTime));//19
                newJobDT.Columns.Add("jobCreatedBy", typeof(string));//19
                newJobDT.Columns.Add("sortType", typeof(string));//15
                newJobDT.Columns.Add("designDate", typeof(DateTime));//15
                newJobDT.Columns.Add("jobMgnValue", typeof(decimal));//16
                newJobDT.Columns.Add("dman", typeof(string));//16
                
                
                DataRow dr = newJobDT.NewRow();
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[0].Value == null) { continue; }
                    dr = newJobDT.NewRow();
                    dr["jobNo"] = jobDGV.Rows[i].Cells[0].Value.ToString();
                    dr["parentJobNo"] = Convert.ToInt32(jobDGV.Rows[i].Cells[0].Value.ToString().Substring(0, 5));
                    dr["phaseNo"] = jobDGV.Rows[i].Cells[0].Value.ToString().Substring(6, 2);
                    dr["floorLevel"] = jobDGV.Rows[i].Cells[2].Value.ToString();
                    dr["requiredDate"] = Convert.ToDateTime(jobDGV.Rows[i].Cells[3].Value);
                    dr["designDate"] = Convert.ToDateTime(jobDGV.Rows[i].Cells[4].Value);
                    dr["siteAddress"] = jobDGV.Rows[i].Cells[5].Value.ToString();
                    dr["dman"] = jobDGV.Rows[i].Cells[6].Value.ToString();
                    dr["Approved"] = (bool)jobDGV.Rows[i].Cells[7].Value ? "Y" : "N";
                    dr["OnShop"] = (bool)jobDGV.Rows[i].Cells[8].Value ? "Y" : "N";
                    dr["stairsIncl"] = (bool)jobDGV.Rows[i].Cells[9].Value ? "Y" : "N";
                    dr["slabM2"] = Convert.ToInt32(jobDGV.Rows[i].Cells[10].Value);
                    dr["beamLm"] = Convert.ToInt32(jobDGV.Rows[i].Cells[11].Value);
                    dr["beamM2"] = Convert.ToInt32(jobDGV.Rows[i].Cells[12].Value);
                    dr["productSupplier"] = jobDGV.Rows[i].Cells[13].Value.ToString();
                    dr["supplyType"] = jobDGV.Rows[i].Cells[14].Value.ToString();
                    dr["supplierRef"] = jobDGV.Rows[i].Cells[15].Value.ToString();
                    dr["phaseInvValue"] = jobDGV.Rows[i].Cells[16].Value == null || jobDGV.Rows[i].Cells[16].Value == "" ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[16].Value);
                    dr["jobMgnValue"] = jobDGV.Rows[i].Cells[17].Value == null || jobDGV.Rows[i].Cells[17].Value == "" ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[17].Value);
                    dr["lastComment"] = jobDGV.Rows[i].Cells[8].Value.ToString();
                    
                    dr["completedFlag"] = mcData.GetCompletedFlagFromJob(jobDGV.Rows[i].Cells[0].Value.ToString());
                    dr["modifiedDate"] = DateTime.Now;// Convert.ToDateTime(jobDGV.Rows[i].Cells[16].Value);
                    dr["modifiedBy"] = loggedInUser;// jobDGV.Rows[i].Cells[17].Value.ToString();
                    newJobDT.Rows.Add(dr);
                }
                return "OK";
            }
            catch (Exception ex)
            {
                string msg = $"JobsDGVToDB() Error : {ex.Message.ToString()} -- Inner Exception : {ex.InnerException.ToString()}";
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "JobsDGVToNewDatatable()", msg);
                return msg;
            }

        }

        private DataTable GetDTFromDGV()
        {
            DataTable dt = new DataTable();
            Double reqDate;
            int dd = 0;
            int mm = 0;
            int yyyy = 0;
            string jobNo = "";
            try
            {


                dt.Columns.Clear();
                dt.Columns.Add("jobNo", typeof(string)); //0
                dt.Columns.Add("parentJobNo", typeof(string)); //1
                dt.Columns.Add("phaseNo", typeof(string)); //2
                dt.Columns.Add("floorLevel", typeof(string));//3
                dt.Columns.Add("requiredDate", typeof(Double));//4
                dt.Columns.Add("custName", typeof(string)); //5
                dt.Columns.Add("siteAddress", typeof(string));//6
                // newJobDT.Columns.Add("drawn", typeof(string));//6
                dt.Columns.Add("approved", typeof(string));//7
                dt.Columns.Add("OnShop", typeof(string));//8
                dt.Columns.Add("stairsIncl", typeof(string));//9
                dt.Columns.Add("slabM2", typeof(string));//10

                dt.Columns.Add("beamLm", typeof(string));//11
                dt.Columns.Add("beamM2", typeof(string));//12
                dt.Columns.Add("supplyType", typeof(string));//13
                dt.Columns.Add("productSupplier", typeof(string));//14
                dt.Columns.Add("supplierRef", typeof(string));//15
                dt.Columns.Add("lastComment", typeof(string));//16
                dt.Columns.Add("phaseInvValue", typeof(string));//17
                dt.Columns.Add("jobMgnValue", typeof(string));//17
                dt.Columns.Add("completed", typeof(string));//18
                dt.Columns.Add("modifiedDate", typeof(string));//19
                dt.Columns.Add("modifiedBy", typeof(string));//20

                DataRow dr = dt.NewRow();
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[0].Value == null) { continue; }
                    //  jobNo = jobDGV.Rows[i].Cells[0].Value.ToString();
                    dr = dt.NewRow();
                    dr["jobNo"] = jobDGV.Rows[i].Cells[0].Value.ToString();
                    dr["parentJobNo"] = Convert.ToInt32(jobDGV.Rows[i].Cells[0].Value.ToString().Substring(0, 5));
                    dr["phaseNo"] = jobDGV.Rows[i].Cells[0].Value.ToString().Substring(6, 2);
                    dr["floorLevel"] = jobDGV.Rows[i].Cells[2].Value.ToString();
                    reqDate = mcData.GetCorrectDate(Convert.ToDateTime(jobDGV.Rows[i].Cells[3].Value)).ToOADate();
                    dr["requiredDate"] = reqDate;
                    dr["custName"] = jobDGV.Rows[i].Cells[1].Value.ToString();
                    dr["siteAddress"] = jobDGV.Rows[i].Cells[4].Value.ToString();
                    //   dr["drawn"] =  (bool)jobDGV.Rows[i].Cells[5].Value ? "Y" : "N";
                    dr["approved"] = (bool)jobDGV.Rows[i].Cells[6].Value ? "Y" : "N";
                    dr["OnShop"] = (bool)jobDGV.Rows[i].Cells[6].Value ? "Y" : "N";
                    dr["stairsIncl"] = (bool)jobDGV.Rows[i].Cells[8].Value ? "Y" : "N";
                    dr["slabM2"] = Convert.ToInt32(jobDGV.Rows[i].Cells[9].Value);
                    dr["beamLm"] = Convert.ToInt32(jobDGV.Rows[i].Cells[10].Value);
                    dr["beamM2"] = Convert.ToInt32(jobDGV.Rows[i].Cells[11].Value);

                    dr["supplyType"] = jobDGV.Rows[i].Cells[13].Value.ToString();
                    dr["supplierRef"] = jobDGV.Rows[i].Cells[14].Value.ToString();
                    dr["productSupplier"] = jobDGV.Rows[i].Cells[12].Value.ToString();
                    dr["lastComment"] = jobDGV.Rows[i].Cells[16].Value.ToString();
                    dr["phaseInvValue"] = Convert.ToDecimal(jobDGV.Rows[i].Cells[15].Value);
                    dr["jobMgnValue"] = Convert.ToDecimal(jobDGV.Rows[i].Cells[16].Value);
                    dr["completed"] = mcData.GetCompletedFlagFromJob(jobDGV.Rows[i].Cells[0].Value.ToString());
                    dr["modifiedDate"] = Convert.ToDateTime(jobDGV.Rows[i].Cells[18].Value);
                    dr["modifiedBy"] = jobDGV.Rows[i].Cells[19].Value.ToString();

                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                string msg = String.Format("GetDTFromDGV() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "GetDTFromDGV()", msg);
                return null;
            }

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            int numLocksDeleted = mcData.DeleteUserLocks("JP", loggedInUser);
            this.Dispose();
            this.Close();
        }

        private bool ValidateDGV()
        {
            bool result = false;
            string supplyType, floorLevel, siteAddr = "";
            bool failFlag = false;
            ;

            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[0].Value == null || jobDGV.Rows[i].Cells[0].Value.ToString().Length != 8)
                    {
                        break;
                    }

                    floorLevel = jobDGV.Rows[i].Cells[2].Value == null ? "" : jobDGV.Rows[i].Cells[2].Value.ToString();
                    siteAddr = jobDGV.Rows[i].Cells[5].Value == null ? "" : jobDGV.Rows[i].Cells[5].Value.ToString();
                    supplyType = jobDGV.Rows[i].Cells[14].Value == null ? "" : jobDGV.Rows[i].Cells[14].Value.ToString();

                    if (String.IsNullOrWhiteSpace(floorLevel) || String.IsNullOrWhiteSpace(siteAddr) || String.IsNullOrWhiteSpace(supplyType))
                    {
                        MessageBox.Show("Floor Level, Site Address and Supply Type (SO / SF) must be specified for a job to be created");
                        failFlag = true;
                        break;

                    }

                }
                if (failFlag) { return false; }
                return true;

            }
            catch (Exception ex)
            {
                string msg = String.Format("ValidateDGV() ERROR - {0}", ex.Message);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "ValidateDGV()", msg);
                return false;
            }
        }

        private void CustomerDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            string jobNo = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();
            int parentJob = Convert.ToInt32(jobNo.Substring(0, 5));
            string custCode = mcData.GetCustomerCodeByParentJob(parentJob);
            AddCustomerForm custForm = new AddCustomerForm(custCode, true);
            custForm.ShowDialog();
            return;
        }

        private void completeJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            string phaseJob = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();
            if (MessageBox.Show(String.Format("Confirm completion of Job No.{0}", phaseJob), "Confirm Completion", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string err = mcData.CompleteJobPlanner(phaseJob, "Y");

                if (err == "OK")
                {
                    string err2 = mcData.CompleteWhiteboardJob(phaseJob, "Y");
                    jobDGV.Rows[rowIndex].Cells[15].Value = String.Format("*** Job No.{0} is now complete ***", phaseJob);
                    mcData.CreateJobComment(phaseJob, String.Format("*** Job No.{0} is now complete ***", phaseJob));
                    jobDGV.Rows[rowIndex].ReadOnly = true;
                    jobDGV.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    MessageBox.Show(String.Format("Unable to complete job no.{0} : {1}", phaseJob, err));
                    return;
                }
            }
        }

        private void siteDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            string jobNo = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();
            string siteAddr = jobDGV.Rows[rowIndex].Cells[5].Value == null ? "" : jobDGV.Rows[rowIndex].Cells[5].Value.ToString();
            int parentJob = Convert.ToInt32(jobNo.Substring(0, 5));
            string siteContact = "";
            string siteContactTel = "";
            string siteContactEmail = "";
            mcData.GetSiteDetailFromParentJob(parentJob, out siteContact, out siteContactTel, out siteContactEmail);


            CreateParentJobForm siteForm = new CreateParentJobForm(parentJob, siteAddr, siteContact, siteContactTel, siteContactEmail);
            siteForm.ShowDialog();
            jobDGV.Rows[rowIndex].Cells[5].Value = siteForm.SiteAddress;
            return;
        }

        private void jobDGV_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!jobDGV.Focused) { return; }
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
            {
                this.jobDGV.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.jobDGV.CurrentCell = this.jobDGV.Rows[e.RowIndex].Cells[0];
                this.contextMenuStrip1.Show(this.jobDGV, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
                return;

            }
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

                string job = jobDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (mcData.IsJobCompleted(job) /*&& e.ColumnIndex != 15*/) { return; }

                if (String.IsNullOrWhiteSpace(jobDGV.Rows[e.RowIndex].Cells[0].Value.ToString()) || jobDGV.Rows[e.RowIndex].Cells[0].Value.ToString().Length != 8)
                {
                    return;
                }

                if (e.ColumnIndex == 13)
                {
                    //   if (MessageBox.Show("Do you have a PO number for supplier", "Confirm PO Number", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) { return; }
                    AddPONumberForm poForm = new AddPONumberForm(job);
                    poForm.ShowDialog();
                    jobDGV.Rows[e.RowIndex].Cells[15].Value = poForm.PONumber;


                    if (jobDGV.Rows[e.RowIndex].Cells[13].Value != null)
                    {
                        suppForm = new SuppliersListForm(jobDGV.Rows[e.RowIndex].Cells[13].Value.ToString());
                    }
                    else
                    {
                        suppForm = new SuppliersListForm();
                    }

                    suppForm.ShowDialog();
                    string suppShortName = suppForm.Shortname;
                    string productType = mcData.GetSupplierProductTypeFromShortname(suppShortName);
                    string result = mcData.UpdateWhiteBoardJobProductWithSupplierProductType(job, productType);
                    jobDGV.Rows[e.RowIndex].Cells[13].Value = suppShortName;
                    int rgb1, rgb2, rgb3 = 0;

                    mcData.GetSupplierColourByShortname(suppShortName, out rgb1, out rgb2, out rgb3);
                    if (!String.IsNullOrWhiteSpace(suppShortName))
                    {
                        string err1 = mcData.UpdateJobPlannerSupplierShortName(job, suppShortName);
                        string err2 = mcData.UpdateWhiteBoardSupplierShortName(job, suppShortName, rgb1, rgb2, rgb3);
                    }
                    jobDGV.CurrentCell = jobDGV.Rows[e.RowIndex].Cells[13];
                    jobDGV.Rows[e.RowIndex].Cells[13].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    jobDGV.CurrentCell.Selected = true;
                }

                if (e.ColumnIndex == 3)
                {
                    string msg = "Changing the REQUIRED DATE of this job will mean any existing comments in the Whiteboard held against the original required date for this job will be CLEARED."
                                + Environment.NewLine + Environment.NewLine +
                                "You can always view the history of any comments made for this job by going to the whiteboard and right clicking on the job and selecting [Job Comments Audit] option"

                                + Environment.NewLine + Environment.NewLine +
                                "Are you happy to continue ?";
                    if (MessageBox.Show(msg, String.Format("Confirm Job REQUIRED DATE change for Job No {0}", job), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string fullDate = jobDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
                        string datePart = fullDate.Substring(4, 10);
                        DateTime reqDate = Convert.ToDateTime(datePart);
                        DateSelectorForm dateForm = new DateSelectorForm(reqDate);
                        dateForm.ShowDialog();
                        //jobDGV[2, row].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().Substring(0,3) + " " + Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                        jobDGV.Rows[e.RowIndex].Cells[3].Value = dateForm.RequiredDate.DayOfWeek.ToString().Substring(0, 3) + " " + dateForm.RequiredDate.ToShortDateString();
                        string err = mcData.UpdateJobPlannerJobDate(job, dateForm.RequiredDate);
                        if (err == "OK")
                        {
                            string err2 = mcData.UpdateWhiteBoardJobDate(job, dateForm.RequiredDate);
                            if (err2 == "OK")
                            {
                                // string err2a = mcData.CreateJobDayAudit(job, dateForm.RequiredDate, $"UpdateWhiteBoardJobDate(....{dateForm.RequiredDate.ToShortDateString()}......)");
                            }
                            string err3 = mcData.ClearWhiteboardJobDayComments(job);
                        }
                        jobDGV.CurrentCell = jobDGV.Rows[e.RowIndex].Cells[3];
                        jobDGV.CurrentCell.Selected = true;
                    }
                }

                if (e.ColumnIndex == 4)
                {
                    string dateStr = jobDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
                    //string datePart = fullDate.Substring(4, 10);
                    DateTime dateValue = Convert.ToDateTime(dateStr);
                    DateSelectorForm dateForm = new DateSelectorForm(dateValue);
                    dateForm.ShowDialog();
                    jobDGV.Rows[e.RowIndex].Cells[4].Value = dateForm.RequiredDate.ToShortDateString();
                    string err = mcData.UpdateJobPlannerDesignDate(job, dateForm.RequiredDate);
                    string err2 = mcData.UpdateDesignDate(job, dateForm.RequiredDate);
                }

                if (e.ColumnIndex == 18)
                {
                    //jobDGV.Rows[0].Cells[0].Value
                    JobCommentForm commentForm = new JobCommentForm(jobDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
                    commentForm.ShowDialog();
                    jobDGV.Rows[e.RowIndex].Cells[18].Value = mcData.GetLastComment(jobDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
                    jobDGV.CurrentCell = jobDGV.Rows[e.RowIndex].Cells[18];
                    jobDGV.CurrentCell.Selected = true;
                }
            }
            catch (Exception ex)
            {
                string msg = String.Format("jobDGV_CellDoubleClick() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "jobDGV_CellDoubleClick(...)", msg);
                return;
            }
        }

        private void DisplayTotalInvValue()
        {
            decimal total = 0;
            decimal value = 0;
            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[0].Value == null) { continue; }
                    value = jobDGV.Rows[i].Cells[16].Value == null || jobDGV.Rows[i].Cells[16].Value == "" ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[16].Value.ToString());
                   // if (jobDGV.Rows[i].Cells[15].Value == null) { continue; }
                    total += value;
                }

                totalInvoiceValueTextBox.Text = total.ToString("#,#");
            }
            catch (Exception ex)
            {
                string msg = String.Format("DisplayTotalInvValue() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "DisplayTotalInvValue()", msg);
                return;
            }
        }

        private void DisplayTotalJobMgnValue()
        {
            decimal total = 0;
            decimal value = 0;
            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[0].Value == null) { continue; }
                    value = jobDGV.Rows[i].Cells[17].Value == null || jobDGV.Rows[i].Cells[17].Value == "" ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[17].Value.ToString());
                    //   if (jobDGV.Rows[i].Cells[16].Value == null) { continue; }value
                    total += value;
                }

                txtTotalJobMgn.Text = total.ToString("#,#");
            }
            catch (Exception ex)
            {
                string msg = String.Format("DisplayTotalInvValue() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "DisplayTotalInvValue()", msg);
                return;
            }
        }

        private void DisplayTotalSlabM2()
        {
            decimal total = 0;
            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[0].Value == null) { continue; }
                    if (jobDGV.Rows[i].Cells[10].Value == null) { continue; }
                    total += Convert.ToDecimal(jobDGV.Rows[i].Cells[10].Value.ToString());
                }

                totalSlabM2TextBox.Text = total.ToString("#,#");
            }
            catch (Exception ex)
            {
                string msg = String.Format("DisplayTotalSlabM2() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "DisplayTotalSlabM2()", msg);
                return;
            }
        }

        private void DisplayTotalBeamM2()
        {
            decimal total = 0;
            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[0].Value == null) { continue; }
                    if (jobDGV.Rows[i].Cells[12].Value == null) { continue; }
                    total += Convert.ToDecimal(jobDGV.Rows[i].Cells[12].Value.ToString());
                }

                totalBeamM2TextBox.Text = total.ToString("#,#");
            }
            catch (Exception ex)
            {
                string msg = String.Format("DisplayTotalBeamM2() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobPlannerForm.cs", "DisplayTotalBeamM2()", msg);
                return;
            }
        }

        private void jobDGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                BuildDGV();
                PopulateDGV(mcData.GetJobPlannerDT());
                return;
            }
        }

        private void jobDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!jobDGV.Focused) { return; }

            string jobNo = jobDGV[0, e.RowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "JP");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot continue");
                return;

            }

            if (e.ColumnIndex == 6) // designer
            {
                string designer = jobDGV[e.ColumnIndex, e.RowIndex].Value.ToString();
            }

            if (e.ColumnIndex != 10 && e.ColumnIndex != 11 && e.ColumnIndex != 12 && e.ColumnIndex != 16 && e.ColumnIndex != 17) { return; }

            if (e.ColumnIndex == 10)
            {
                DisplayTotalSlabM2();
                //return;
            }

            if (e.ColumnIndex == 11) //BeamLM
            {
                int beamLM = Convert.ToInt32(jobDGV[e.ColumnIndex, e.RowIndex].Value.ToString());
                decimal halvedBeamLM = beamLM * 0.5m;
                int beamM2 = (int)Math.Round(halvedBeamLM, 0);
                jobDGV[12, e.RowIndex].Value = beamM2;
                DisplayTotalBeamM2();
                //return;
            }

            if (e.ColumnIndex == 12)
            {
                
                DisplayTotalBeamM2();
           //     return;
            }

            if (e.ColumnIndex == 16)
            {
                string newValue = this.jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                // Normalize input to use a period as the decimal separator
                newValue = newValue.Replace(',', '.');

                decimal parsedValue;

                // Try parsing the value again to ensure it's a decimal
                if (decimal.TryParse(newValue, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedValue))
                {
                    // Format to 2 decimal places
                    this.jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = parsedValue.ToString("F2");
                }
                DisplayTotalInvValue();
              //  return;
            }

            if(e.ColumnIndex == 17)
            {
                string newValue = this.jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                // Normalize input to use a period as the decimal separator
                newValue = newValue.Replace(',', '.');

                decimal parsedValue;

                // Try parsing the value again to ensure it's a decimal
                if (decimal.TryParse(newValue, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedValue))
                {
                    // Format to 2 decimal places
                    this.jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = parsedValue.ToString("F2");
                }
                DisplayTotalJobMgnValue();
              //  return;
            }

            if (mcData.IsJobLockExist("JP", jobNo, "jobDGV_CellEndEdit", loggedInUser)) { return; }
            string result = mcData.CreateJobLock("JP", jobNo, "jobDGV_CellEndEdit");
            return;
        }

        private void jobDGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!jobDGV.Focused) { return; }
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);

            string jobNo = jobDGV[jobDGV.CurrentCell.ColumnIndex, jobDGV.CurrentCell.RowIndex].Value.ToString();
            if (jobDGV.CurrentCell.ColumnIndex == 10 || jobDGV.CurrentCell.ColumnIndex == 11 || jobDGV.CurrentCell.ColumnIndex == 12 || jobDGV.CurrentCell.ColumnIndex == 16 || jobDGV.CurrentCell.ColumnIndex == 17) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
                return;
            }

            if (jobDGV.CurrentCell.ColumnIndex == 6) // designer
            {
                ComboBox combo = e.Control as ComboBox;
                if (combo != null)
                {
                    combo.SelectedIndexChanged -=
            new EventHandler(ComboBox_SelectedIndexChanged);

                    // Add the event handler. 
                    combo.SelectedIndexChanged +=
                        new EventHandler(ComboBox_SelectedIndexChanged);
                }
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           // string designer = 
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

        private void addNewPhaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            int parentJob = Convert.ToInt32(jobDGV.Rows[rowIndex].Cells[0].Value.ToString().Substring(0, 5));
            /*
            string message = "This option is currently DISABLED as it only creates the next incremented phase i.e. 123456.01 to 123456.02, 123456.09 to 123456.10 etc"
                    + Environment.NewLine + "You currently cannot skip phase numbers i.e. 123456.05 --> 123456.09";
            MessageBox.Show(message, "Add New Phase Number currently Disabled");
            return;



            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }

            int parentJob = Convert.ToInt32(jobDGV.Rows[rowIndex].Cells[0].Value.ToString().Substring(0,5));

            */

            // 21mAY2023 - New Code --------------------------------------------------------------------
            /*
            AddNewPhaseForm addNewPhaseFrm = new AddNewPhaseForm(parentJob.ToString());
            addNewPhaseFrm.ShowDialog();
            string nextJobNo = addNewPhaseFrm.NewPhaseJob;

            if(String.IsNullOrWhiteSpace(nextJobNo)) { return; }

            */
            //------------------------------------------------------------------------------------------




            string phaseStr = mcData.GetLastPhaseFromJobPlanner(parentJob);
            string nextJobNo = mcData.GetNextPhaseNumber(parentJob, phaseStr);
            string siteAddress = mcData.GetSiteAddressFromParentJob(parentJob);
            string customerCode = mcData.GetCustomerCodeByParentJob(parentJob);
            string custName = mcData.GetCustName(customerCode);
            string err = "OK"; // initialised to "OK" so that whether WB job exists or not we ca get past check and populate DGV
            string wbErr = "OK"; // initialised to "OK" so that whether WB job exists or not we ca get past check and populate DGV
            string dbErr = "OK";

            if (!mcData.IsJobExists(nextJobNo))
            {
                err = mcData.CreateJobPlanner(parentJob, nextJobNo, nextJobNo.Substring(6, 2), "", DateTime.Now.AddYears(1), DateTime.Now.AddYears(1), siteAddress, "N", "N", "N", 0, 0, 0, "", "", "", "", 0,0, "","");
            }

            if (!mcData.IsWhiteboardJobExists(nextJobNo))
            {
                wbErr = mcData.CreateWhiteBoard(nextJobNo, DateTime.Now.AddYears(1), customerCode, siteAddress, "", "", 0, 0, 0, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",

                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            }

            if(!mcData.IsDesignBoardJobExists(nextJobNo))
            {
                dbErr = mcData.CreateDesignBoardJob(nextJobNo, DateTime.Now.AddYears(1), "NOT DRAWN", DateTime.Now.AddYears(1), 1, "", "", "", "", "", 0, 0, 0, "");
            }

            this.Cursor = Cursors.Default;


            if (err == "OK" && wbErr == "OK" && dbErr == "OK")
            {
                this.Cursor = Cursors.WaitCursor;
                if (sourceDT != null)
                {
                    PopulateDGVByParentJob(parentJob);
                }
                else
                {
                    PopulateDGV(mcData.GetJobPlannerDT());
                }
                jobDGV.Refresh();
                this.Cursor = Cursors.Default;
            }
            else
            {
                if (err != "OK")
                {
                    MessageBox.Show(String.Format("Error Creating Job Planner Record : {0}", err));
                    return;
                }

                if (wbErr != "OK")
                {
                    MessageBox.Show(String.Format("Error Creating Whiteboard Record : {0}", wbErr));
                    int num = mcData.DeleteJobPlannerByJobNo(nextJobNo);
                    string response = mcData.CreateJobDeletionAudit(nextJobNo, DateTime.MinValue, "n/a", "n/a", 0, "n/a", "addNewPhaseToolStripMenuItem1_Click errored out in JobPlanner.cs");
                    return;
                }

            }
        }

        private void cancelJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            string phaseJob = jobDGV.Rows[rowIndex].Cells[0].Value.ToString(); //
            if (mcData.IsJobCancelled(phaseJob))
            {
                MessageBox.Show(String.Format("Job [{0}] is already flagged as CANCELLED. Cannot continue.", phaseJob));
                this.Cursor = Cursors.Default;
                return;
            }
            if (mcData.IsJobCompleted(phaseJob))
            {
                MessageBox.Show(String.Format("Job [{0}] is flagged as COMPLETED. Cannot cancel a completed job.", phaseJob));
                this.Cursor = Cursors.Default;
                return;
            }
            this.Cursor = Cursors.Default;
            if (MessageBox.Show(String.Format("Confirm CANCELLATION of Job No.{0}", phaseJob), "Confirm Job Cancellation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                //string result = JobsDGVToNewDatatable();
                //if (result == "OK")
                //{
                //    if (!mcData.AreTablesTheSame(jobDT, newJobDT))
                //    {
                //        UpdatedJobsDGVToDB();
                //    }
                //}

                string err1 = mcData.CreateCancelledLock(phaseJob);
                //int num1 = mcData.DeleteJobPlannerByJobNo(phaseJob);
                //int num2 = mcData.DeleteWhiteboardByJobNo(phaseJob);

                if (err1 == "OK")
                {
                    mcData.CreateJobComment(phaseJob, $"Job No.{phaseJob} CANCELLED by [{loggedInUser}] at {DateTime.Now.ToShortTimeString()} on {DateTime.Now.ToShortDateString()}");

                    MessageBox.Show($"Job {phaseJob} is now cancelled.");
                    if (parentJobNo > 0)
                    {
                        PopulateDGVByParentJob(parentJobNo);
                    }
                    else
                    {
                        PopulateDGV(mcData.GetJobPlannerDT());
                    }
                }
                this.Cursor = Cursors.Default;
                jobDGV.Refresh();
                return;
            }
        }

        private void recreateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            int parentJob = Convert.ToInt32(jobDGV.Rows[rowIndex].Cells[0].Value.ToString().Substring(0, 5));
            RecreatePhaseForm rcForm = new RecreatePhaseForm(parentJob);
            rcForm.ShowDialog();
            if (rcForm.IsPhaseCreated)
            {
                PopulateDGV(mcData.GetJobPlannerDT());
                jobDGV.Refresh();
                return;
            }
        }

        private void jobDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!jobDGV.Focused) { return; }

            if (e.ColumnIndex == 16 || e.ColumnIndex == 17)
            {
                double value;
                if (double.TryParse(e.Value.ToString(), out value))
                {
                    // Format to two decimal places
                    e.Value = value.ToString("F2");
                }
            }
            //if (!jobDGV.Focused) { return; }

                //if (jobDGV.Rows[e.RowIndex].Cells[0].Value == null)
                //{
                //    return;
                //}

                //string job = jobDGV[0, e.RowIndex].Value.ToString();


                //if (mcData.IsJobCompleted(job))
                //{
                //    jobDGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //    jobDGV.Rows[e.RowIndex].ReadOnly = true;
                //}
                //else
                //{
                //    jobDGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

                //}
        }

        private void uncompleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            string phaseJob = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();
            if (MessageBox.Show(String.Format("Confirm UN-completion of Job No.{0}", phaseJob), "Confirm Un-Completion", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string err = mcData.CompleteJobPlanner(phaseJob, "N");

                if (err == "OK")
                {
                    string err2 = mcData.CompleteWhiteboardJob(phaseJob, "N");
                    MessageBox.Show(String.Format("*** Job No.{0} is now back in progress ***", phaseJob));
                    jobDGV.Rows[rowIndex].Cells[15].Value = String.Format("*** Job No.{0} is now back in progress ***", phaseJob);
                    mcData.CreateJobComment(phaseJob, String.Format("*** Job No.{0} is now back in progress ***", phaseJob));
                    jobDGV.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    jobDGV.Rows[rowIndex].ReadOnly = false;
                    return;
                }
                else
                {
                    MessageBox.Show(String.Format("Unable to un-complete job no.{0} : {1}", phaseJob, err));
                    return;
                }
            }
        }

        private void whiteboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }

            /*
             DateTime firstDate = mcData.GetFirstPlannerDate();
            DateTime startDate = mcData.GetMonday(firstDate);
            DateTime lastDate = mcData.GetLastPlannerDate();
            DataTable dt = mcData.WhiteboardDatesDT();

            TimeSpan ts = lastDate - startDate;
            int dateDiff = ts.Days;
            decimal numWeeks = dateDiff / 7m;
            int roundedNumWeeks = (int)Decimal.Round(numWeeks, 1) + 1;
            //int roundedNumWeeks = (int)Math.Ceiling(numWeeks);
            WhiteboardForm wbForm = new WhiteboardForm(startDate, lastDate, dt, roundedNumWeeks);
            wbForm.ShowDialog();
             */





            string phaseJob = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();

            if (!mcData.IsValidWhiteboardJob(phaseJob))
            {
                MessageBox.Show(String.Format("Job [{0}] is not [ON SHOP] or [APPROVED]. Cannot continue to Whiteboard", phaseJob));
                return;
            }

            if (mcData.IsJobCompleted(phaseJob))
            {
                MessageBox.Show(String.Format("Job [{0}] is flagged as COMPLETED. Cannot continue to Whiteboard", phaseJob));
                return;
            }
            // int numWeeks = 1;
            DataTable dt = mcData.WhiteboardDatesDT(phaseJob);
            DateTime jobDate = mcData.GetPlannerDateByJobNo(phaseJob);
            DateTime startDate = mcData.GetMonday(jobDate);
            DateTime lastDate = startDate.AddDays(6);
            TimeSpan ts = lastDate - startDate;
            int dateDiff = ts.Days;
            decimal numWeeks = dateDiff / 7m;
            int roundedNumWeeks = (int)Decimal.Round(numWeeks, 1) + 1;
            WhiteboardForm wbForm = new WhiteboardForm(phaseJob, startDate, lastDate, dt, roundedNumWeeks);
            wbForm.ShowDialog();

        }

        public void OrderJobsByEarliestDate()
        {
            this.Cursor = Cursors.WaitCursor;
            string qry = "SELECT * FROM dbo.JobPlanner WHERE LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate";

            if (jobPlannerMode == "SLAB")
            {
                qry = @"SELECT * FROM dbo.JobPlanner 
                        WHERE completedFlag != 'Y' AND
                        LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) AND
                        ( slabM2 > 0 OR stairsIncl = 'Y' )  ORDER BY requiredDate";
            }

            if (jobPlannerMode == "BEAM")
            {
                qry = @"SELECT * FROM dbo.JobPlanner 
                        WHERE completedFlag != 'Y' AND
                        LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                        AND ( beamM2 > 0 OR beamLm > 0 ) ORDER BY requiredDate";
            }


            PopulateDGVByQry(qry);
            DisplayTotalInvValue();
            DisplayTotalJobMgnValue();
            DisplayTotalBeamM2();
            DisplayTotalSlabM2();
            this.Cursor = Cursors.Default;
            return;
        }

        public void OrderJobsByLatestDate()
        {
            this.Cursor = Cursors.WaitCursor;
            string qry = "SELECT * FROM dbo.JobPlanner ORDER BY requiredDate DESC";

            if (jobPlannerMode == "SLAB")
            {
                qry = @"SELECT * FROM dbo.JobPlanner 
                        WHERE completedFlag != 'Y' AND 
                        LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) AND
                        ( slabM2 > 0 OR stairsIncl = 'Y' )  ORDER BY requiredDate DESC";
            }

            if (jobPlannerMode == "BEAM")
            {
                qry = @"SELECT * FROM dbo.JobPlanner 
                        WHERE completedFlag != 'Y' AND 
                        LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) AND
                        ( beamM2 > 0 OR beamLm > 0 ) ORDER BY requiredDate DESC";
            }
            PopulateDGVByQry(qry);
            DisplayTotalInvValue();
            DisplayTotalJobMgnValue();
            DisplayTotalBeamM2();
            DisplayTotalSlabM2();
            this.Cursor = Cursors.Default;
            return;
        }

        public void FilterByMonth(string month, string year)
        {
            if (jobPlannerMode == "ALL")
            {
                string qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                if (rbBoth.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE OnShop = 'Y' AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE OnShop = 'N'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Approved = 'Y'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Approved = 'N'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Drawn = 'Y'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Drawn = 'N'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE completedFlag = 'Y'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}'  
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE completedFlag != 'Y'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                }
                if (rbSupplyFix.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE OnShop = 'Y' 
                            AND supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE OnShop = 'N' 
                            AND supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Approved = 'Y' 
                            AND supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Approved = 'N' 
                            AND supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Drawn = 'Y' 
                            AND supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Drawn = 'N' 
                            AND supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE completedFlag = 'Y' 
                            AND supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE completedFlag != 'Y' 
                            AND supplyType = 'SF'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                }
                if (rbSupplyOnly.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE OnShop = 'Y' 
                            AND supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE OnShop = 'N' 
                            AND supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Approved = 'Y' 
                            AND supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) 
                            ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Approved = 'N' 
                            AND supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Drawn = 'Y' 
                            AND supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Drawn = 'N' 
                            AND supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE completedFlag = 'Y' 
                            AND supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE completedFlag != 'Y' 
                            AND supplyType = 'SO'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                }

                PopulateDGVByQry(qry);
                DisplayTotalInvValue();
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                return;
            }

            if (jobPlannerMode == "SLAB")
            {
                string qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                if (rbBoth.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                    WHERE OnShop = 'Y' 
                                    AND DATENAME(month,requiredDate) = '{month}' 
                                    AND DATENAME(year,requiredDate) = '{year}' 
                                    AND completedFlag != 'Y' 
                                    AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                    AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                    ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'N'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'Y'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'N'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'Y'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE Drawn = 'N'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND completedFlag != 'Y' 
                            AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag = 'Y'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}'  
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag != 'Y'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                }
                if (rbSupplyFix.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'N' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'N' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'N' 
                                AND supplyType = 'SF' 
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag = 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag != 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                }
                if (rbSupplyOnly.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'N' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'N' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'N' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag = 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag != 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( slabM2 > 0 OR stairsIncl = 'Y' )  
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                }

                PopulateDGVByQry(qry);
                DisplayTotalInvValue();
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                return;
            }

            if (jobPlannerMode == "BEAM")
            {
                string qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                if (rbBoth.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'Y' 
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'N'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'Y'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'N'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'Y'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'N'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag = 'Y'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}'  
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                            WHERE completedFlag != 'Y'  
                            AND DATENAME(month,requiredDate) = '{month}' 
                            AND DATENAME(year,requiredDate) = '{year}' 
                            AND ( beamM2 > 0 OR beamLm > 0 ) 
                            AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                            ORDER BY requiredDate";
                    }
                }
                if (rbSupplyFix.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'N' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'N' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'N' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag = 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag != 'Y' 
                                AND supplyType = 'SF'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                }
                if (rbSupplyOnly.Checked)
                {
                    if (rbOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotOnShop.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE OnShop = 'N' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotApproved.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Approved = 'N' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbNotDrawn.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE Drawn = 'N' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND completedFlag != 'Y' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbAll.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbCompleted.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag = 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = '{month}' 
                                AND DATENAME(year,requiredDate) = '{year}' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                    if (rbInProgress.Checked)
                    {
                        qry = $@"SELECT * FROM dbo.JobPlanner 
                                WHERE completedFlag != 'Y' 
                                AND supplyType = 'SO'  
                                AND DATENAME(month,requiredDate) = 'month' 
                                AND DATENAME(year,requiredDate) = 'year' 
                                AND ( beamM2 > 0 OR beamLm > 0 ) 
                                AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )
                                ORDER BY requiredDate";
                    }
                }

                PopulateDGVByQry(qry);
                DisplayTotalInvValue();
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                return;
            }



        }

        private void janButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("January", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void febButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("February", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void marButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("March", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void aprButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("April", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void mayButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("May", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void junButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("June", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void julButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("July", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void augButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("August", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void sepButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("September", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void octButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("October", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void novButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("November", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void decButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("December", DateTime.Now.Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void getAllJobsButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY supplyType,requiredDate";
            if (jobPlannerMode == "ALL")
            {
                qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY supplyType,requiredDate";
            }
            if (jobPlannerMode == "BEAM")
            {
                qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) AND ( beamM2 > 0 OR beamLm > 0 ) ORDER BY supplyType,requiredDate";
            }
            if (jobPlannerMode == "SLAB")
            {
                qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) AND ( slabM2 > 0 OR stairsIncl = 'Y' )  ORDER BY supplyType,requiredDate";
            }

            PopulateDGVByQry(qry);
            DisplayTotalInvValue();
            DisplayTotalJobMgnValue();
            DisplayTotalBeamM2();
            DisplayTotalSlabM2();
            this.Cursor = Cursors.Default;
            return;
        }

        private void amendParentJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }

            string phaseJob = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();
            if (mcData.IsJobCompleted(phaseJob))
            {
                MessageBox.Show(String.Format("Job [{0}] is flagged as COMPLETED. Cannot continue.", phaseJob));
                return;
            }
            int parentJobNo = Convert.ToInt32(phaseJob.Substring(0, 5));
            bool updateParentJobFlag = true;
            CreateParentJobForm pjForm = new CreateParentJobForm(parentJobNo, updateParentJobFlag);
            pjForm.ShowDialog();
            PopulateDGV(mcData.GetJobPlannerDT());
            return;
        }

        private void filter1Button_Click_1(object sender, EventArgs e)
        {
            if (jobPlannerMode == "ALL")
            {
                this.Cursor = Cursors.WaitCursor;
                string qry = "SELECT * FROM dbo.JobPlanner WHERE LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY jobNo";
                if (rbBoth.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                }
                if (rbSupplyFix.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE sortType = 'SF' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND sortType = 'SF' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND sortType = 'SF' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                }
                if (rbSupplyOnly.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND sortType = 'SO' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND sortType = 'SO' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND sortType = 'SO' AND completedFlag != 'Y' ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE sortType = 'SO' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND sortType = 'SO' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND sortType = 'SO' AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                }
                PopulateDGVByQry(qry);
                DisplayTotalInvValue();
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                this.Cursor = Cursors.Default;
                return;
            }

            if (jobPlannerMode == "BEAM")
            {
                this.Cursor = Cursors.WaitCursor;
                string qry = "SELECT * FROM dbo.JobPlanner WHERE ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY jobNo";
                if (rbBoth.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                }
                if (rbSupplyFix.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE sortType = 'SF' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND sortType = 'SF' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND sortType = 'SF' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                }
                if (rbSupplyOnly.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND sortType = 'SO' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND sortType = 'SO' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND sortType = 'SO' AND completedFlag != 'Y' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE sortType = 'SO' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND sortType = 'SO' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND sortType = 'SO' AND ( beamM2 > 0 OR beamLm > 0 ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob ) ORDER BY requiredDate"; }
                }
                PopulateDGVByQry(qry);
                DisplayTotalInvValue();
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                this.Cursor = Cursors.Default;
                return;
            }

            if (jobPlannerMode == "SLAB")
            {
                this.Cursor = Cursors.WaitCursor;
                string qry = "SELECT * FROM dbo.JobPlanner AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY jobNo";
                if (rbBoth.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner AND ( slabM2 > 0 OR stairsIncl = 'Y' )   AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                }
                if (rbSupplyFix.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND sortType = 'SF' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND sortType = 'SF' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE sortType = 'SF' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND sortType = 'SF' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND sortType = 'SF' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                }
                if (rbSupplyOnly.Checked)
                {
                    if (rbOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotOnShop.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE OnShop = 'N' AND sortType = 'SO' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotApproved.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Approved = 'N' AND sortType = 'SO' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'Y' AND sortType = 'SO' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbNotDrawn.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE Drawn = 'N' AND sortType = 'SO' AND completedFlag != 'Y' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbAll.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE sortType = 'SO' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbCompleted.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag = 'Y' AND sortType = 'SO' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                    if (rbInProgress.Checked) { qry = "SELECT * FROM dbo.JobPlanner WHERE completedFlag != 'Y' AND sortType = 'SO' AND ( slabM2 > 0 OR stairsIncl = 'Y' ) AND LEFT(jobNo,8) NOT in ( SELECT LEFT(jobNo, 8) FROM dbo.CancelledJob )  ORDER BY requiredDate"; }
                }
                PopulateDGVByQry(qry);
                DisplayTotalInvValue();
                DisplayTotalJobMgnValue();
                DisplayTotalBeamM2();
                DisplayTotalSlabM2();
                this.Cursor = Cursors.Default;
                return;
            }

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            string loggedInUser = mcData.GetLoggedInUser();
            string databaseName = mcData.GetDatabaseName().ToUpper();
            string message = $"Given your current logged in details : {Environment.NewLine} {Environment.NewLine} User ID : {loggedInUser} {Environment.NewLine} Database Name : {databaseName} ";
            string fullMessage = $"{message} {Environment.NewLine} {Environment.NewLine} You are NOT permitted to update a LIVE database";
            // string serverName = mcData.GetServerName().ToUpper();
            if ((loggedInUser == "aa" || loggedInUser == "test") && !databaseName.Contains("TEST"))
            {
                MessageBox.Show(fullMessage, "WARNING");
                return;
            }
            this.Cursor = Cursors.WaitCursor;

            if (!ValidateDGV())
            {
                MessageBox.Show("Cannot update. Missing job fields - Floor Level, Site Address and Supply Type (SO / SF) must be specified for a job to be created");
                this.Cursor = Cursors.Default;
                return;
            }
            string result = JobsDGVToNewDatatable();
            if (result == "OK")
            {
                if (!mcData.AreTablesTheSame(dgvDT, newJobDT))
                {
                    UpdatedJobsDGVToDB();
                }
            }

            int numLocksDeleted = mcData.DeleteUserLocks("JP", loggedInUser);

            this.Cursor = Cursors.Default;
            this.Dispose();
            this.Close();
        }

        private void removeSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            string phaseJob = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();
            jobDGV.Rows[rowIndex].Cells[10].Value = String.Empty;
            jobDGV.Rows[rowIndex].Cells[10].Style.BackColor = Color.White;
            string err = mcData.RemoveSupplierFromJobPlanner(phaseJob);
            string err2 = mcData.RemoveSupplierFromWB(phaseJob);
            return;


        }

        private void jobDateAuditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (jobDGV[0, rowIndex].Value == null) { return; }
            string jobNo = jobDGV[0, this.rowIndex].Value.ToString();
            JobDateAuditForm auditForm = new JobDateAuditForm(jobNo);
            auditForm.ShowDialog();
            return;
        }

        private void jan2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("January", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void feb2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("February", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void mar2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("March", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void apr2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("April", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void may2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("May", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void jun2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("June", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void jul2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("July", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void aug2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("August", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void sep2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("September", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void oct2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("October", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void nov2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("November", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void dec2Button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FilterByMonth("December", DateTime.Now.AddYears(1).Year.ToString());
            this.Cursor = Cursors.Default;
            return;
        }

        private void earliestButton_Click(object sender, EventArgs e)
        {
            return;
        }

        private void latestButton_Click(object sender, EventArgs e)
        {
            return;
        }

        private void earliestButton_Click_1(object sender, EventArgs e)
        {
            OrderJobsByEarliestDate();
            return;
        }

        private void latestButton_Click_1(object sender, EventArgs e)
        {
            OrderJobsByLatestDate();
            return;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void copyAlltoClipboard()
        {
            //  return;
            jobDGV.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            jobDGV.MultiSelect = true;
            jobDGV.SelectAll();
            DataObject dataObj = jobDGV.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void GenerateExcelWorkbook()
        {

            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Rows.AutoFit();
            xlWorkSheet.Columns.AutoFit();
            //Microsoft.Office.Tools.Excel.NamedRange rng = this.Controls.AddNamedRange(this.Range["A1"], "NamedRange1");
            Microsoft.Office.Interop.Excel.Range range1 = xlWorkSheet.get_Range("B1", "AB1");
            range1.Cells.Interior.Color = Color.Yellow;

            //freeze top row
            xlWorkSheet.Activate();
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

            //apply filters
            Microsoft.Office.Interop.Excel.Range firstRow = xlWorkSheet.Rows[1];
            firstRow.Activate();
            firstRow.Select();
            firstRow.AutoFilter(1,
                                Type.Missing,
                                Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd,
                                Type.Missing,
                                true);
            xlWorkSheet.Cells.Font.Size = 8;
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            //delete blank column 1
            Microsoft.Office.Interop.Excel.Range objRange = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.get_Range("A1", Type.Missing);
            objRange.EntireColumn.Delete(Type.Missing);
            //autosize all the columns
            Microsoft.Office.Interop.Excel.Range fullRange = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.get_Range("A1", "IV65536");
            fullRange.EntireColumn.WrapText = true;
            fullRange.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            fullRange.Columns.AutoFit();
            /*
            xlWorkBook.SaveAs(@"C:\test1\absBook1.xlsx", Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xlWorkBook.Close();
            xlexcel.Quit();
            */
            return;
        }

        private void excelButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable dt = GetDTFromDGV();
            //string result = JobsDGVToNewDatatable();

            if (dt != null)
            {
                JobPlannerRptForm jpRptForm = new JobPlannerRptForm(dt);
                jpRptForm.ShowDialog();
            }
            this.Cursor = Cursors.Default;
            return;
        }

        private void jobDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!jobDGV.Focused) { return; }


            
            string jobNo = jobDGV[0, e.RowIndex].Value.ToString();

            if (e.ColumnIndex == 6) // designer
            {
                string designer = jobDGV[e.ColumnIndex, e.RowIndex].Value.ToString();
            }

            string response = mcData.GetJobLockedUser(jobNo, "JP");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot continue");
                return;

            }
            if (mcData.IsJobLockExist("JP", jobNo, "jobDGV_CellValueChanged", loggedInUser)) { return; }
            string result = mcData.CreateJobLock("JP", jobNo, "jobDGV_CellValueChanged");
            return;
        }

        private void jobDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!jobDGV.Focused) { return; }
            if (e.RowIndex < 0) { return; }
            string jobNo = jobDGV[0, e.RowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "JP");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot continue");
                return;

            }
            if (mcData.IsJobLockExist("JP", jobNo, "jobDGV_CellClick", loggedInUser)) { return; }
            string result = mcData.CreateJobLock("JP", jobNo, "jobDGV_CellClick");
            return;
        }

        private void jobDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!jobDGV.Focused) { return; }
            string jobNo = jobDGV[0, e.RowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "JP");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot continue");
                return;

            }
            if (mcData.IsJobLockExist("JP", jobNo, "jobDGV_CellBeginEdit", loggedInUser)) { return; }
            string result = mcData.CreateJobLock("JP", jobNo, "jobDGV_CellBeginEdit");
            return;
        }

        private void btnLockedJobs_Click(object sender, EventArgs e)
        {
            if (!mcData.IsJobLockExistByType("JP"))
            {
                MessageBox.Show("There are currently no locked jobs on the Job Planner");
                return;
            }

            LockedJobsForm lockForm = new LockedJobsForm("JP");
            lockForm.ShowDialog();
            return;
        }

        private void JobPlannerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.WindowsShutDown)
            {
                int num = mcData.EmptyCurrentInvoicedWhiteboardJobList();
                int numLocksDeleted = mcData.DeleteUserLocks("JP", loggedInUser);
                this.Dispose();
                this.Close();
            }
        }

        private void goToJobInDesignBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colIndex < 0 || jobDGV.Rows[rowIndex].Cells[0].Value == null)
            {
                return;
            }
            string phaseJob = jobDGV.Rows[rowIndex].Cells[0].Value.ToString();
            if(!mcData.IsDesignBoardJob(phaseJob))
            {
                MessageBox.Show($"Job [{phaseJob}] does NOT exist in the Design Board");
                return;
            }
            if(mcData.IsDesignBoardJobOnShop(phaseJob))
            {
                MessageBox.Show($"Job [{phaseJob}] is ON SHOP and will not NOT exist in the Design Board");
                return;
            }
            DataTable dt = mcData.DesignBoardDatesDT(phaseJob);
            DateTime jobDate = mcData.GetDesignDateByJobNo(phaseJob);
            DateTime startDate = mcData.GetMonday(jobDate);
            DateTime lastDate = startDate.AddDays(6);
            TimeSpan ts = lastDate - startDate;
            int dateDiff = ts.Days;
            decimal numWeeks = dateDiff / 7m;
            int roundedNumWeeks = (int)Decimal.Round(numWeeks, 1) + 1;
            DesignBoardForm dbForm = new DesignBoardForm(phaseJob, startDate, lastDate, dt, roundedNumWeeks);
            dbForm.ShowDialog();
           // PopulateDGV(mcData.GetJobPlannerDTByJob(phaseJob));
            return;
        }

        private void viewIssuesReportedAtSiteForJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mcData.IsFeatureToggleEnabled("WB_JOB_ISSUE"))
            {
                MessageBox.Show($"This option is NOT yet available in the version of MCP");
                return;
            }

            if (jobDGV[0, rowIndex].Value == null) { return; }
            string jobNo = jobDGV[0, this.rowIndex].Value.ToString();
            string siteAddr = jobDGV[3, this.rowIndex].Value.ToString();
            DataTable issuesDT = mcData.GetWBJobIssuesDT(jobNo);
            if (issuesDT != null && issuesDT.Rows.Count > 0)
            {
                WhiteboardJobIssueReportedForm issueForm = new WhiteboardJobIssueReportedForm(jobNo, siteAddr, issuesDT);
                issueForm.ShowDialog();
                return;
            }
            MessageBox.Show($"There were no issues raised at site for Job [{jobNo}]");
            return;
        }

        private void jobDGV_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!jobDGV.Focused) { return; }

            if (e.ColumnIndex == 16 || e.ColumnIndex == 17)
            {
                string newValue = e.FormattedValue.ToString();

                // Normalize input to use a period as the decimal separator
                newValue = newValue.Replace(',', '.');

                decimal parsedValue;

                // Try to parse the input as a decimal (using period as separator)
                if (decimal.TryParse(newValue, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedValue))
                {
                    // Allow the value to pass through if it's valid
                    e.Cancel = false;
                }
                else
                {
                    // If the value is invalid (not a number), cancel the edit and show an error message
                    e.Cancel = true;
                    //MessageBox.Show("Please enter a valid decimal number.");
                }
            }
        }

        private void jobDGV_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            //    if (!char.IsControl(e.KeyChar)
            //    && !char.IsDigit(e.KeyChar)
            //    && e.KeyChar == '.')
            //{
            //    e.Handled = true;
            //}
        }
    }
}
