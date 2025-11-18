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
    public partial class DesignBoardForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        private DateTime dbStartDate = DateTime.MinValue;
        private DateTime dbEndDate = DateTime.MinValue;
        private System.Data.DataTable dbDT = new System.Data.DataTable();
        private System.Data.DataTable dbDatesDT = new System.Data.DataTable();
        private int wcNumWeeks = 0;
        private DateTime dbStartTime = DateTime.MinValue;
        private DataGridView dbDataGridView = new DataGridView();
        private string selectedJob = "";
        private int rowIndex = 0;
        private int colIndex = 0;
        private string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
        private string selectedDesigner = "";
        BindingSource suppTypeBindngSource = new BindingSource();
        BindingSource salesmanBindngSource = new BindingSource();
        private bool _singleJobSearchOnly = false;


        public DesignBoardForm()
        {
            InitializeComponent();
        }

        public DesignBoardForm(DateTime startDate, DateTime endDate, DataTable datesDT, int numWeeks)
        {
            InitializeComponent();
            dbStartDate = startDate;
            dbEndDate = endDate;
            dbDatesDT = datesDT;
            wcNumWeeks = numWeeks;
        }

        public DesignBoardForm(string jobNo,DateTime startDate, DateTime endDate, DataTable datesDT, int numWeeks)
        {
            InitializeComponent();
            selectedJob = jobNo;
            dbStartDate = startDate;
            dbEndDate = endDate;
            dbDatesDT = datesDT;
            wcNumWeeks = numWeeks;
            _singleJobSearchOnly = true;
        }

        private void BuildTabs()
        {
            this.weeksTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.weeksTabControl_DrawItem);
            Cursor.Current = Cursors.WaitCursor;
            DateTime wcDate = DateTime.MinValue;
            int numTabsCreated = 0;

            for (int i = 0; i < wcNumWeeks; i++)
            {
                foreach (DataRow dr in dbDatesDT.Rows)
                {
                    if (Convert.ToInt16(dr["tabNo"].ToString()) == i + 1 || Convert.ToInt16(dr["tabNo"].ToString()) == 0)
                    {
                        wcDate = Convert.ToDateTime(dr["wcDate"].ToString());
                        break;
                    }
                }
                weeksTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
                weeksTabControl.TabPages.Add(wcDate.ToShortDateString());
                weeksTabControl.TabPages[i].ForeColor = mcData.IsCurrentWeek(wcDate) ? Color.Blue : Color.Black;
                weeksTabControl.TabPages[i].AutoScroll = true;
                weeksTabControl.TabPages[i].Width = 1600;
                weeksTabControl.TabPages[i].Height = 700;
                weeksTabControl.TabPages[i].Controls.Add(new DataGridView()
                {
                    Name = "dataGridView" + (i + 1).ToString(),
                    Dock = DockStyle.Fill,
                    Width = 1450,
                    Height = 650,
                    Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right),
                    ScrollBars = System.Windows.Forms.ScrollBars.Both,
                    AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                });
                numTabsCreated++;
            }

            foreach (Control thisControl in weeksTabControl.Controls)
            {
                if (thisControl.GetType() == typeof(TabPage))
                {

                    foreach (Control dgv in thisControl.Controls)
                    {
                        if (dgv.GetType() == typeof(DataGridView))
                        {

                            var vScrollbar = dgv.Controls.OfType<VScrollBar>().FirstOrDefault();




                            BuildDesignBoardDGV((DataGridView)dgv, Convert.ToDateTime(thisControl.Text).Date);
                            PopulateDesignBoardDGV((DataGridView)dgv, Convert.ToDateTime(thisControl.Text).Date, Convert.ToDateTime(thisControl.Text).Date.AddDays(6));
                           // PopulateDesignBoardDGV((DataGridView)dgv, dbStartDate, dbEndDate);
                            dgv.SuspendLayout();
                            dgv.ResumeLayout();
                            AddContextMenu((DataGridView)dgv);
                            dbDataGridView = (DataGridView)dgv;
                            dbDataGridView.CellMouseUp += new DataGridViewCellMouseEventHandler(dbDataGridView_CellMouseUp);
                            dbDataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dbDataGridView_EditingControlShowing);
                            //  dbDataGridView.DataError += new DataGridViewDataErrorEventHandler(dbDataGridView_DataError);
                            dbDataGridView.DataError += DbDataGridView_DataError;
                            dbDataGridView.CellValueChanged += new DataGridViewCellEventHandler(dbDataGridView_CellValueChanged);
                            dbDataGridView.CurrentCellDirtyStateChanged += DbDataGridView_CurrentCellDirtyStateChanged;
                            dbDataGridView.CellClick += new DataGridViewCellEventHandler(dbDataGridView_CellClick);
                            dbDataGridView.CellBeginEdit += new DataGridViewCellCancelEventHandler(dbDataGridView_CellBeginEdit);
                            dbDataGridView.CellEndEdit += new DataGridViewCellEventHandler(dbDataGridView_CellEndEdit);
                            dbDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(dbDataGridView_CellFormatting);

                            
                        }
                    }

                }
            }




            Cursor.Current = Cursors.Hand;
        }

        private void DbDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void DbDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if(dgv.IsCurrentCellDirty)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dbDataGridView_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void dbDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(!this.dbDataGridView.Focused) { return; }
            if(e.ColumnIndex == 1 || e.ColumnIndex == 7)
            {
                DataGridViewCell cell = this.dbDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DateTime cellDate = Convert.ToDateTime(this.dbDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString());
                cell.ToolTipText = $"{cellDate.DayOfWeek},{cellDate.ToString("dd MMM yyyy")}";
            }
            return;
        }

        private void ResetDayColour(DataGridView dgv,int rowIndex)
        {
            int monColIndex = 16;
            int tueColIndex = 17;
            int wedColIndex = 18;
            int thuColIndex = 19;
            int friColIndex = 20;

            dgv[monColIndex, rowIndex].Style.BackColor = Color.Yellow;
            dgv[tueColIndex, rowIndex].Style.BackColor = Color.White;
            dgv[wedColIndex, rowIndex].Style.BackColor = Color.White;
            dgv[thuColIndex, rowIndex].Style.BackColor = Color.White;
            dgv[friColIndex, rowIndex].Style.BackColor = Color.White;
        }

        private void dbDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!dbDataGridView.Focused) { return; }

            string jobNo = dbDataGridView[0, e.RowIndex].Value.ToString();

            if (e.ColumnIndex == 2)
            {
                if (dbDataGridView[0, e.RowIndex].Value == null) { return; }
                if (dbDataGridView[2, e.RowIndex].Value == null) { return; }

                ResetDayColour(dbDataGridView, e.RowIndex);
                DateTime designDate = Convert.ToDateTime(dbDataGridView[1, e.RowIndex].Value.ToString()).Date;
                int dow = (int)designDate.DayOfWeek;
                string inputStr = dbDataGridView[0, e.RowIndex].Value == null ? "" : dbDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
                int detailDayCount = Convert.ToInt16(inputStr);

                int numRemaining = mcData.GetDesignDateRemainingDays(dow, detailDayCount);
                dbDataGridView[e.ColumnIndex, e.RowIndex].Value = numRemaining;
                dbDataGridView.CurrentCell = this.dbDataGridView[e.ColumnIndex, e.RowIndex];
                mcData.UpdateDesignBoardColourCodeDayFlags(jobNo, designDate, numRemaining, dow);
                ColourCodeDayCells(dbDataGridView, e.RowIndex, numRemaining, dow);
                return;

                
            }
            if (e.ColumnIndex == 13 || e.ColumnIndex == 14 || e.ColumnIndex == 15)
            {
                int slabM2 = 0;
                int beamLM = 0;
                int beamM2 = 0;
                int totalM2 = 0;
                if (e.ColumnIndex == 14) //BeamLM
                {
                    slabM2 = Convert.ToInt32(dbDataGridView[13, e.RowIndex].Value);
                    beamLM = Convert.ToInt32(dbDataGridView[14, e.RowIndex].Value.ToString());
                    decimal halvedBeamLM = beamLM * 0.5m;
                    beamM2 = (int)Math.Round(halvedBeamLM, 0);
                    dbDataGridView[15, e.RowIndex].Value = beamM2;
                    string err1 = mcData.UpdateDesignBoardQuantities(jobNo, slabM2, beamLM, beamM2);
                    string err2 = mcData.UpdateJobPlannerQuantities(jobNo, slabM2, beamLM, beamM2);
                    totalM2 = slabM2 + beamM2;
                    string err3 = mcData.UpdateWhiteboardTotalM2(jobNo, totalM2);
                    return;
                }
                slabM2 = Convert.ToInt32(dbDataGridView[13, e.RowIndex].Value);
                beamLM = Convert.ToInt32(dbDataGridView[14, e.RowIndex].Value);
                beamM2 = Convert.ToInt32(dbDataGridView[15, e.RowIndex].Value);
                string err4 = mcData.UpdateDesignBoardQuantities(jobNo, slabM2, beamLM, beamM2);
                string err5 = mcData.UpdateJobPlannerQuantities(jobNo, slabM2, beamLM, beamM2);
                totalM2 = slabM2 + beamM2;
                string err6 = mcData.UpdateWhiteboardTotalM2(jobNo, totalM2);
                return;
            }

        }

        private void ColourCodeDayCells(DataGridView dgv, int rowNo, int ddaysCount, int dow)
        {
            string monFlag = "N";
            string tueFlag = "N";
            string wedFlag = "N";
            string thuFlag = "N";
            string friFlag = "N";
            int monColIndex = 16;
            int tueColIndex = 17;
            int wedColIndex = 18;
            int thuColIndex = 19;
            int friColIndex = 20;


            mcData.GetDesignBoardDaysColourFlags(dow, ddaysCount, out monFlag, out tueFlag, out wedFlag, out thuFlag, out friFlag);
            dgv[monColIndex, rowNo].Style.BackColor = monFlag == "Y" ? Color.Yellow : Color.White;
            dgv[tueColIndex, rowNo].Style.BackColor = tueFlag == "Y" ? Color.Yellow : Color.White;
            dgv[wedColIndex, rowNo].Style.BackColor = wedFlag == "Y" ? Color.Yellow : Color.White;
            dgv[thuColIndex, rowNo].Style.BackColor = thuFlag == "Y" ? Color.Yellow : Color.White;
            dgv[friColIndex, rowNo].Style.BackColor = friFlag == "Y" ? Color.Yellow : Color.White;
            return;
        }

        private void dbDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            return;
        }

        private void dbDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            return;
        }

        private void dbDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int designerCboColIndex = 5;
            int suppTypeCboIndex = 11;
            int salesmanCboColIndex = 6;
            string dman = "";
            string job = "";
            string suppType = "";
            string salesman = "";
            DataGridViewComboBoxCell cb1 = (DataGridViewComboBoxCell)dbDataGridView.Rows[e.RowIndex].Cells[designerCboColIndex];
            if (cb1.Value != null)
            {
                dman = cb1.Value.ToString();
                job = dbDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                string err1 = mcData.UpdateDesignboardDesigner(job, dman);
                string err2 = mcData.UpdateJobPlannerDesigner(job, dman);
                string err3 = mcData.UpdateWhiteboardDesigner(job, dman);
            }
            DataGridViewComboBoxCell cb2 = (DataGridViewComboBoxCell)dbDataGridView.Rows[e.RowIndex].Cells[suppTypeCboIndex];
            if (cb2.Value != null)
            {
                suppType = cb2.Value.ToString();
                job = dbDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                string err4 = mcData.UpdateDesignboardSupplyType(job, suppType);
                string err5 = mcData.UpdateJobPlannerSupplyType(job, suppType);
                string err6 = mcData.UpdateWhiteboardSupplyType(job, suppType);
            }
            DataGridViewComboBoxCell cb3 = (DataGridViewComboBoxCell)dbDataGridView.Rows[e.RowIndex].Cells[salesmanCboColIndex];
            if (cb3.Value != null)
            {
                salesman = cb3.Value.ToString();
                job = dbDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                string err7 = mcData.UpdateDesignboardSalesman(job, salesman);
                string err8 = mcData.UpdateWhiteboardSalesman(job, salesman);
            }

            return;
        }

        private void dbDataGridView_DataError_Old(object sender, DataGridViewDataErrorEventArgs e)
        {
            //e.Cancel = true;
            //return;

            if (e.Exception is ArgumentException && e.RowIndex >= 0 && ( e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 11))
            {
                var cell = this.dbDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;

                if (cell != null)
                {
                    // You can handle the error, e.g., set a default value or show a message
                    MessageBox.Show("Invalid value in ComboBox column, setting to default.");

                    // You can set a default value if needed
                    cell.Value = cell.Items[0]; // Or any valid default item

                    // Mark the error as handled
                    e.ThrowException = false;
                }
            }
        }

        private void dbDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            return;
        }

        

        private void dbDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.RowIndex != -1) // added this and it now works
            {
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.dbDataGridView = (DataGridView)sender;
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
            {
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.dbDataGridView = (DataGridView)sender;
                return;
            }
        }

        public void GetDesignDayColourFlags(DateTime dwcdate, int detailDaysCount, DateTime dsgnDate,
            out bool isMonYellow, out bool IsTueYellow, out bool IsWedYellow, out bool IsThuYellow, out bool IsFriYellow)
        {
            string error;
            int dayCount = 0;
            isMonYellow = false;
            IsTueYellow = false;
            IsWedYellow = false;
            IsThuYellow = false;
            IsFriYellow = false;


            int weekDayNo = (int)dsgnDate.DayOfWeek;
            if(weekDayNo > 0)
            {
                if(detailDaysCount > 5)
                switch (weekDayNo)
                {
                    case 1:
                        dayCount = (detailDaysCount + weekDayNo) - 1;

                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    default:
                        break;
                }
                
            }

            return;

        }
    

    private void PopulateDesignBoardDGV(DataGridView myDGV, DateTime myStartDate, DateTime myEndDate)
        {
            int row = 0;

            int rgb1 = 255;
            int rgb2 = 255;
            int rgb3 = 255;
            int srgb1 = 255;
            int srgb2 = 255;
            int srgb3 = 255;
            string productSupplier = "";
            string stairsSupplier = "";
            string monFlag = "";
            string tueFlag = "";
            string wedFlag = "";
            string thuFlag = "";
            string friFlag = "";
            string satFlag = "";
            string sunFlag = "";
            string designDateStr = "";
            string dateStr = "";
            string ramsFlag = "";
            string custName = "";
            string siteAddr = "";
            string isInvoiced = "";
            string jobNo = "";
            string dateCreated = "";
            string designStatus = "";
            string detailingDays = "";
            int designDateDayNo = 0;
            int daysDiff = 0;
            DateTime dateJobCreated;
            DateTime ddate;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                DataTable dt = mcData.GetDesignboardByDateRangeDT(myStartDate, myEndDate);
                if (dt == null)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count < 1)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                myDGV.Rows.Clear();
                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                foreach (DataRow dr in dt.Rows)
                {

                    jobNo = dr["jobNo"].ToString();
                    if(_singleJobSearchOnly)
                    {
                        if(!jobNo.Trim().Equals(selectedJob.Trim())) { continue; }
                    }
                    productSupplier = dr["productSupplier"].ToString();
                    // stairsSupplier = dr["stairsSupplier"].ToString();
                    designStatus = dr["designStatus"].ToString();
                    if (!mcData.IsDesignBoardJobExists(jobNo)) { continue; }
                    ddate = Convert.ToDateTime(dr["designDate"].ToString());
                    designDateStr = ddate.DayOfWeek.ToString().ToUpper().Substring(0, 3);
                    designDateDayNo = (int)Convert.ToDateTime(dr["designDate"].ToString()).DayOfWeek;
                    dateStr = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().ToUpper().Substring(0, 3);
                    custName = mcData.GetCustomerNameByJobNo(jobNo);
                    siteAddr = mcData.GetSiteAddressFromJobNo(jobNo);
                    mcData.GetSupplierColourByShortname(productSupplier, out rgb1, out rgb2, out rgb3);
                    if (designStatus.ToUpper().Contains("APPROVED") || designStatus.ToUpper() == "ON SHOP")
                    {
                        daysDiff = 0;
                    }
                    else
                    {
                        dateJobCreated = mcData.GetJobCreatedDateByJobNo(jobNo);
                        daysDiff = mcData.GetDaysDiffBetweenTwDates(dateJobCreated.Date);
                    }
                    detailingDays = dr["detailingDays"] == null ? "" : dr["detailingDays"].ToString();

                    //mcData.GetSupplierColourByShortname(stairsSupplier, out srgb1, out srgb2, out srgb3);
                    //    dateCreated = mcData.GetJobCreatedDate(jobNo).ToString("dd/MMM/yyyy hh:mm tt");

                    monFlag = dr["monFlag"].ToString();
                    tueFlag = dr["tueFlag"].ToString();
                    wedFlag = dr["wedFlag"].ToString();
                    thuFlag = dr["thuFlag"].ToString();
                    friFlag = dr["friFlag"].ToString();
                    //   satFlag = dateStr.Substring(0, 3).ToUpper() == "SAT" ? "Y" : String.Empty;
                    //   sunFlag = dateStr.Substring(0, 3).ToUpper() == "SUN" ? "Y" : String.Empty;

                    DataGridViewRow drow = new DataGridViewRow();
                    drow.CreateCells(myDGV);
                    drow.Cells[0].Value = dr["jobNo"].ToString();
                    drow.Cells[1].Value = Convert.ToDateTime(dr["designDate"].ToString()).ToShortDateString();
                    drow.Cells[2].Value = detailingDays;
                    drow.Cells[3].Value = dr["designStatus"].ToString();
                    drow.Cells[3].Style.ForeColor = designStatus.Contains("NOT DRAWN") ? Color.Black : Color.Red;
                    drow.Cells[4].Value = daysDiff.ToString();
                    drow.Cells[5].Value = dr["dman"].ToString();
                    drow.Cells[6].Value = dr["salesman"].ToString();
                    drow.Cells[7].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString(); ;
                    drow.Cells[8].Value = custName;
                    drow.Cells[9].Value = siteAddr;
                    drow.Cells[10].Value = dr["floorlevel"].ToString();
                    drow.Cells[11].Value = dr["supplyType"].ToString();
                    drow.Cells[12].Value = dr["productSupplier"].ToString();
                    drow.Cells[12].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    drow.Cells[13].Value = Convert.ToInt32(dr["slabM2"].ToString());
                    drow.Cells[14].Value = Convert.ToInt32(dr["beamM2"].ToString());
                    drow.Cells[15].Value = Convert.ToInt32(dr["beamLM"].ToString());
                    drow.Cells[16].Value = dr["wcMonday"].ToString();
                    drow.Cells[17].Value = dr["wcTuesday"].ToString();
                    drow.Cells[18].Value = dr["wcWednesday"].ToString();
                    drow.Cells[19].Value = dr["wcThursday"].ToString();
                    drow.Cells[20].Value = dr["wcFriday"].ToString();
                    drow.Cells[16].Style.BackColor = monFlag == "Y" ? Color.Yellow : Color.White;
                    drow.Cells[17].Style.BackColor = tueFlag == "Y" ? Color.Yellow : Color.White;
                    drow.Cells[18].Style.BackColor = wedFlag == "Y" ? Color.Yellow : Color.White;
                    drow.Cells[19].Style.BackColor = thuFlag == "Y" ? Color.Yellow : Color.White;
                    drow.Cells[20].Style.BackColor = friFlag == "Y" ? Color.Yellow : Color.White;
                    drow.Cells[21].Value = dr["additionalNotes"].ToString();
                    rows.Add(drow);
                }
                myDGV.Rows.AddRange(rows.ToArray());
                this.Cursor = Cursors.Default;
                foreach (DataGridViewRow dgvRow in myDGV.Rows)
                {

                    string unsuffixedJobNo = dgvRow.Cells[0].Value.ToString().Substring(0, 8);
                    string custCode = mcData.GetCustomerCodeByJobNo(unsuffixedJobNo);
                    if (mcData.IsJobCompleted(unsuffixedJobNo))
                    {
                        dgvRow.DefaultCellStyle.ForeColor = Color.Gray;
                        // dgvRow.Frozen = true;
                    }
                    else if (mcData.IsJobCustomerOnStop(custCode))
                    {
                        dgvRow.DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    //if (mcData.IsJobLockExistByOtherUser("WB", dgvRow.Cells[0].Value.ToString(), loggedInUser))
                    //{
                    //    dgvRow.Frozen = true;
                    //    dgvRow.DefaultCellStyle.ForeColor = Color.MediumPurple;
                    //}
                }
            }
            catch (Exception ex)
            {
                var errLineNo = new System.Diagnostics.StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                MessageBox.Show($"PopulateDesignBoardDGV(DataGridView myDGV, {myStartDate.ToShortDateString()}, {myEndDate.ToShortDateString()}) ERROR(Line {errLineNo}) - " + ex.Message.ToString());
                string audit = mcData.CreateErrorAudit("DesignBoardForm.cs", $"PopulateDesignBoardDGV(DataGridView myDGV, {myStartDate.ToShortDateString()}, {myEndDate.ToShortDateString()})", ex.Message);
                return;
            }
            
        }

        private void BuildDesignBoardDGV(DataGridView myDGV, DateTime wcDate)
        {
            try
            {
                DataTable designersDT = mcData.GetAllDesigners();
                BindingSource designerBindngSource = new BindingSource();
                designerBindngSource.DataSource = designersDT;


                dbDataGridView = myDGV;
                dbDataGridView.Dock = DockStyle.None;
                dbDataGridView.Controls[1].Enabled = true;
                dbDataGridView.Controls[1].Visible = true;
                dbDataGridView.ScrollBars = ScrollBars.Both;
                dbDataGridView.Columns.Clear();
                dbDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                //0
                DataGridViewTextBoxColumn jobNoColumn = new DataGridViewTextBoxColumn();//0
                jobNoColumn.DataPropertyName = "JobNo";
                jobNoColumn.HeaderText = "Job No.";
                jobNoColumn.Width = 70;
                jobNoColumn.ReadOnly = true;
                jobNoColumn.Frozen = true;
                jobNoColumn.DefaultCellStyle.ForeColor = Color.Black;
                jobNoColumn.DefaultCellStyle.BackColor = Color.Cyan;
                dbDataGridView.Columns.Add(jobNoColumn);

                //1
                DataGridViewTextBoxColumn designDateColumn = new DataGridViewTextBoxColumn();
                designDateColumn.HeaderText = "Design Date (Right Click)";
                designDateColumn.Width = 50;
                designDateColumn.ReadOnly = true;
                designDateColumn.Frozen = true;
                designDateColumn.DefaultCellStyle.BackColor = Color.Yellow;
                designDateColumn.DefaultCellStyle.ForeColor = Color.Blue;
                dbDataGridView.Columns.Add(designDateColumn);

                

                //2
                DataGridViewTextBoxColumn detailingDaysColumn = new DataGridViewTextBoxColumn();
                detailingDaysColumn.DataPropertyName = "detailingDays";
                detailingDaysColumn.HeaderText = "Detail Days";
                detailingDaysColumn.Width = 40;
                detailingDaysColumn.ReadOnly = false;
                detailingDaysColumn.Frozen = true;
                dbDataGridView.Columns.Add(detailingDaysColumn);

                //3
                DataGridViewTextBoxColumn designStatusColumn = new DataGridViewTextBoxColumn();
                designStatusColumn.HeaderText = "Design Status (Right Click)";
                designStatusColumn.Width = 120;
                designStatusColumn.Frozen = true;
                designStatusColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(designStatusColumn);

                //4
                DataGridViewTextBoxColumn daysUnapprovedColumn = new DataGridViewTextBoxColumn();
                daysUnapprovedColumn.HeaderText = "Days UnApprvd";
                daysUnapprovedColumn.Width = 40;
                daysUnapprovedColumn.Frozen = true;
                daysUnapprovedColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(daysUnapprovedColumn);

                //5
                DataGridViewComboBoxColumn designerColumn = new DataGridViewComboBoxColumn();
                designerColumn.DataPropertyName = "Designer";
                designerColumn.HeaderText = "Designer";
                designerColumn.Width = 90;
                designerColumn.DataSource = designerBindngSource;
                designerColumn.ValueMember = "designer";
                designerColumn.DisplayMember = "designer";
                designerColumn.ReadOnly = false;
                designerColumn.Frozen = true;
                dbDataGridView.Columns.Add(designerColumn);

                //6
                DataGridViewComboBoxColumn salesmanColumn = new DataGridViewComboBoxColumn();
                salesmanColumn.DataPropertyName = "Salesman";
                salesmanColumn.HeaderText = "Salesman";
                salesmanColumn.Width = 90;
                salesmanColumn.DataSource = salesmanBindngSource;
                salesmanColumn.ValueMember = "userID";
                salesmanColumn.DisplayMember = "userID";
                salesmanColumn.ReadOnly = false;
                salesmanColumn.Frozen = true;
                dbDataGridView.Columns.Add(salesmanColumn);

                //DataGridViewTextBoxColumn salesmanColumn = new DataGridViewTextBoxColumn();//41
                //salesmanColumn.DataPropertyName = "salesman";
                //salesmanColumn.HeaderText = "Salesman";
                //salesmanColumn.Width = 90;
                //salesmanColumn.ReadOnly = false;
                //dbDataGridView.Columns.Add(salesmanColumn);                

                //7
                DataGridViewTextBoxColumn reqDateColumn = new DataGridViewTextBoxColumn();
                reqDateColumn.HeaderText = "Req Date (Right Click)";
                reqDateColumn.Width = 50;
                reqDateColumn.ReadOnly = true;
             //   reqDateColumn.Frozen = true;
                reqDateColumn.DefaultCellStyle.ForeColor = Color.Blue;
                dbDataGridView.Columns.Add(reqDateColumn);

                //8
                DataGridViewTextBoxColumn custColumn = new DataGridViewTextBoxColumn();
                custColumn.DataPropertyName = "CustName";
                custColumn.HeaderText = "Customer";
                custColumn.Width = 150;
              //  custColumn.Frozen = true;
                custColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(custColumn);

                //9
                DataGridViewTextBoxColumn siteColumn = new DataGridViewTextBoxColumn();
                siteColumn.DataPropertyName = "site";
                siteColumn.HeaderText = "Site Address";
                siteColumn.Width = 250;
             //   siteColumn.Frozen = true;
                siteColumn.ReadOnly = true;
                siteColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dbDataGridView.Columns.Add(siteColumn);

                //10
                DataGridViewTextBoxColumn levelColumn = new DataGridViewTextBoxColumn();
                levelColumn.DataPropertyName = "Level";
                levelColumn.HeaderText = "Floor Level";
                levelColumn.Width = 100;
                levelColumn.DefaultCellStyle.ForeColor = Color.Blue;
                levelColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                levelColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(levelColumn);

                //11
                DataGridViewComboBoxColumn suppTypeColumn = new DataGridViewComboBoxColumn();
                suppTypeColumn.DataPropertyName = "suppType";
                suppTypeColumn.HeaderText = "SuppType";
                suppTypeColumn.Width = 60;
                suppTypeColumn.DataSource = suppTypeBindngSource;
                suppTypeColumn.ValueMember = "suppType";
                suppTypeColumn.DisplayMember = "suppType";
                dbDataGridView.Columns.Add(suppTypeColumn);



                ////10
                //DataGridViewTextBoxColumn productsColumn = new DataGridViewTextBoxColumn();//5
                //productsColumn.DataPropertyName = "products";
                //productsColumn.HeaderText = "Product (Right Clk)";
                //productsColumn.Width = 60;
                //productsColumn.ReadOnly = false;
                //dbDataGridView.Columns.Add(productsColumn);

                //12
                DataGridViewTextBoxColumn productSupplierColumn = new DataGridViewTextBoxColumn();
                productSupplierColumn.DataPropertyName = "productSupplier";
                productSupplierColumn.HeaderText = "Supplier (Right Click)";
                productSupplierColumn.Width = 70;
                productSupplierColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(productSupplierColumn);

                ////12
                //DataGridViewCheckBoxColumn stairsColumn = new DataGridViewCheckBoxColumn();
                //stairsColumn.DataPropertyName = "stairs";
                //stairsColumn.ValueType = typeof(bool);
                //stairsColumn.HeaderText = "Stairs";
                //stairsColumn.Width = 70;
                //stairsColumn.ReadOnly = false;
                //dbDataGridView.Columns.Add(stairsColumn);

                ////13
                //DataGridViewTextBoxColumn stairSupplierColumn = new DataGridViewTextBoxColumn();
                //stairSupplierColumn.DataPropertyName = "stairSupplier";
                //stairSupplierColumn.HeaderText = "StairsSupplier (Right Click)";
                //stairSupplierColumn.Width = 70;
                //stairSupplierColumn.ReadOnly = true;
                //dbDataGridView.Columns.Add(stairSupplierColumn);

                //13
                DataGridViewTextBoxColumn slabM2Column = new DataGridViewTextBoxColumn();
                slabM2Column.DataPropertyName = "slabM2";
                slabM2Column.HeaderText = "Slab M²";
                slabM2Column.Width = 60;
                slabM2Column.ReadOnly = false;
                dbDataGridView.Columns.Add(slabM2Column);

                //14
                DataGridViewTextBoxColumn beamLMColumn = new DataGridViewTextBoxColumn();
                beamLMColumn.DataPropertyName = "beamLM";
                beamLMColumn.HeaderText = "Beam LM";
                beamLMColumn.Width = 60;
                beamLMColumn.ReadOnly = false;
                dbDataGridView.Columns.Add(beamLMColumn);

                //15
                DataGridViewTextBoxColumn beamM2Column = new DataGridViewTextBoxColumn();
                beamM2Column.DataPropertyName = "beamM2";
                beamM2Column.HeaderText = "Beam M²";
                beamM2Column.Width = 60;
                beamM2Column.ReadOnly = false;
                dbDataGridView.Columns.Add(beamM2Column);

                

                //16
                DataGridViewTextBoxColumn monColumn = new DataGridViewTextBoxColumn();
                monColumn.Name = "mon";
                monColumn.HeaderText = "MON " + wcDate.Day.ToString() + "/" + wcDate.ToString("MMM");
                monColumn.Width = 80;
                monColumn.DefaultCellStyle.BackColor = Color.Yellow;
                monColumn.DefaultCellStyle.ForeColor = Color.Blue;
                monColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(monColumn);

                //17
                DataGridViewTextBoxColumn tueColumn = new DataGridViewTextBoxColumn();
                tueColumn.DataPropertyName = "tue";
                tueColumn.HeaderText = "TUE " + wcDate.AddDays(1).Day.ToString() + "/" + wcDate.AddDays(1).ToString("MMM");
                tueColumn.Width = 80;
                tueColumn.DefaultCellStyle.BackColor = Color.Yellow;
                tueColumn.DefaultCellStyle.ForeColor = Color.Blue;
                tueColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(tueColumn);

                //18
                DataGridViewTextBoxColumn wedColumn = new DataGridViewTextBoxColumn();
                wedColumn.DataPropertyName = "wed";
                wedColumn.HeaderText = "WED " + wcDate.AddDays(2).Day.ToString() + "/" + wcDate.AddDays(2).ToString("MMM");
                wedColumn.Width = 80;
                wedColumn.DefaultCellStyle.BackColor = Color.Yellow;
                wedColumn.DefaultCellStyle.ForeColor = Color.Blue;
                wedColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(wedColumn);

                //19
                DataGridViewTextBoxColumn thuColumn = new DataGridViewTextBoxColumn();
                thuColumn.DataPropertyName = "thu";
                thuColumn.HeaderText = "THU " + wcDate.AddDays(3).Day.ToString() + "/" + wcDate.AddDays(3).ToString("MMM");
                thuColumn.Width = 80;
                thuColumn.DefaultCellStyle.BackColor = Color.Yellow;
                thuColumn.DefaultCellStyle.ForeColor = Color.Blue;
                thuColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(thuColumn);

                //20
                DataGridViewTextBoxColumn friColumn = new DataGridViewTextBoxColumn();
                friColumn.DataPropertyName = "fri";
                friColumn.HeaderText = "FRI " + wcDate.AddDays(4).Day.ToString() + "/" + wcDate.AddDays(4).ToString("MMM");
                friColumn.Width = 80;
                friColumn.DefaultCellStyle.BackColor = Color.Yellow;
                friColumn.DefaultCellStyle.ForeColor = Color.Blue;
                friColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(friColumn);

                //21
                DataGridViewTextBoxColumn notesColumn = new DataGridViewTextBoxColumn();
                notesColumn.DataPropertyName = "additionalNotes";
                notesColumn.HeaderText = "Additonal Notes (Right Click)";
                notesColumn.Width = 200;
                notesColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(notesColumn);

                
                dbDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dbDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dbDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                //wbDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                dbDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                dbDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dbDataGridView.Dock = DockStyle.Fill;
                dbDataGridView.AllowUserToAddRows = false;
                return;

                
            }
            catch (Exception ex)
            {
                var errLineNo = new System.Diagnostics.StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                MessageBox.Show($"BuildDesignBoardDGV() ERROR(Line {errLineNo}) - " + ex.Message.ToString());
                string audit = mcData.CreateErrorAudit("DesignBoardForm.cs", "BuildDesignBoardDGV(...)", ex.Message);
                return;
            }

        }

        private void weeksTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabPage tp = weeksTabControl.TabPages[e.Index];

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;  //optional

            // This is the rectangle to draw "over" the tabpage title
            RectangleF headerRect = new RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);

            // This is the default colour to use for the non-selected tabs
            SolidBrush sb = new SolidBrush(TabPage.DefaultBackColor);

            // This changes the colour if we're trying to draw the selected tabpage
            if (weeksTabControl.SelectedIndex == e.Index)
                sb.Color = Color.Yellow;

            // Colour the header of the current tabpage based on what we did above
            g.FillRectangle(sb, e.Bounds);

            //Remember to redraw the text - I'm always using black for title text
            DateTime dbDate = Convert.ToDateTime(tp.Text);
            Color fontColor = mcData.IsCurrentWeek(dbDate) ? Color.Blue : Color.Black;
            g.DrawString(tp.Text, weeksTabControl.Font, new SolidBrush(fontColor), headerRect, sf); ;
        }

        private void AddContextMenu(DataGridView myDGV)
        {


            foreach (DataGridViewColumn column in myDGV.Columns)
            {

                if (column.Index >= 16 && column.Index <= 20)
                {
                    column.ContextMenuStrip = wbDailyContextMenuStrip1;

                }

                if (column.Index == 0)
                {
                   column.ContextMenuStrip = jobContextMenuStrip1;

                }

                if (column.Index == 1 || column.Index == 7)
                {
                    column.ContextMenuStrip = dateChangeContextMenuStrip1;

                }

                if (column.Index == 3)
                {
                    column.ContextMenuStrip = statusContextMenuStrip1;

                }

                if (column.Index == 10)
                {
                    column.ContextMenuStrip = productContextMenuStrip;

                }

                if (column.Index == 12)
                {
                    column.ContextMenuStrip = supplierContextMenuStrip;

                }

                if (column.Index == 21)
                {
                    column.ContextMenuStrip = AdditionalCommentContextMenuStrip;
                }
            }
        }

        private void DesignBoardForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Design Board spanning period between {0} and {1}", dbStartDate.ToString("dd/MMM/yyyy"), dbEndDate.ToString("dd/MMM/yyyy"));
            label1.Text = "NOTE : Right click on the DAYS (and Additional Comments) to add notes";
            
            suppTypeBindngSource.DataSource = mcData.GetSupplyTypeDT();
            salesmanBindngSource.DataSource = mcData.GetSalesmanDT();
            BuildTabs();

            if (!String.IsNullOrWhiteSpace(selectedJob) && mcData.IsWhiteboardJobExists(selectedJob))
            {
                if (dbDataGridView.Rows.Count < 1) { return; }
                for (int i = 0; i < dbDataGridView.Rows.Count; i++)
                {
                    if (dbDataGridView.Rows[i].Cells[0].Value.ToString().Equals(selectedJob))
                    {
                        dbDataGridView.CurrentCell = dbDataGridView.Rows[i].Cells[0];
                        break;
                        Cursor.Current = Cursors.Hand;
                        return;
                    }
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
            return;
        }

        private void UpdateDBJobDayCommentOnly(string jobNo, string comment, int dayColIndex, DateTime commentDate)
        {
            string fieldName = "";
            switch (dayColIndex)
            {
                case 16:
                    fieldName = "wcMonday";
                    break;
                case 17:
                    fieldName = "wcTuesday";
                    break;
                case 18:
                    fieldName = "wcWednesday";
                    break;
                case 19:
                    fieldName = "wcThursday";
                    break;
                case 20:
                    fieldName = "wcFriday";
                    break;
                default:
                    break;
            }


            string err = mcData.UpdateDesignBoardJobDayComment(jobNo, fieldName, comment);
            if (err != "OK")
            {
                MessageBox.Show(String.Format("Error updating day comment for design board Job No {0} : {1}", jobNo, err));
                return;
            }
            else
            {
                string err2 = mcData.CreateDBDayCommentAudit(jobNo, comment, commentDate);
                return;
            }

        }

        private void addCommenttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbDataGridView[0, rowIndex].Value == null) { return; }
            if (colIndex != 16 && colIndex != 17 && colIndex != 18 && colIndex != 19 && colIndex != 20) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            DateTime designDate = Convert.ToDateTime(dbDataGridView[1, this.rowIndex].Value.ToString()).Date;
            string comment = dbDataGridView[this.colIndex, this.rowIndex].Value == null ? "N/A" : dbDataGridView[this.colIndex, this.rowIndex].Value.ToString();
            int diffDays = colIndex - 16;
            DateTime commentDate = mcData.GetMonday(designDate).AddDays(diffDays).AddDays(0);
            WhiteboardDayCommentForm commentForm = new WhiteboardDayCommentForm(jobNo, commentDate, comment);
            commentForm.ShowDialog();
            dbDataGridView[this.colIndex, this.rowIndex].Value = commentForm.DayComment;
            UpdateDBJobDayCommentOnly(jobNo, commentForm.DayComment, colIndex, commentDate);
        }

        private void selectProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            if (mcData.IsJobCompleted(jobNo)) { return; }
            ProductSelectorForm myForm = new ProductSelectorForm();
            myForm.ShowDialog();
            if (!String.IsNullOrWhiteSpace(myForm.Product))
            {
                dbDataGridView[10, rowIndex].Value = myForm.Product;
                string err = mcData.UpdateDesignBoardJobProduct(jobNo, myForm.Product);
            }
            return;
        }

        private void selectSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuppliersListForm myForm;
            int rgb1, rgb2, rgb3 = 255;
            int supplierColIndex = 12;

            if (dbDataGridView[0, rowIndex].Value == null) { return; }

            if (dbDataGridView[supplierColIndex, rowIndex].Value == null && dbDataGridView[supplierColIndex, rowIndex].Value == null) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            if (mcData.IsJobCompleted(jobNo)) { return; }
            string suppShortName = dbDataGridView[supplierColIndex, this.rowIndex].Value.ToString();
            if (colIndex == supplierColIndex)
            {
                if (dbDataGridView[supplierColIndex, rowIndex].Value == null)
                {
                    myForm = new SuppliersListForm();
                }
                else
                {
                    myForm = new SuppliersListForm(dbDataGridView.Rows[rowIndex].Cells[supplierColIndex].Value.ToString());
                }
                myForm.ShowDialog();
                suppShortName = myForm.Shortname;
                dbDataGridView[supplierColIndex, rowIndex].Value = suppShortName;
                mcData.GetSupplierColourByShortname(suppShortName, out rgb1, out rgb2, out rgb3);
                dbDataGridView[supplierColIndex, rowIndex].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                if (!String.IsNullOrWhiteSpace(suppShortName))
                {
                    string err1 = mcData.UpdateWhiteBoardSupplierShortName(jobNo, suppShortName, rgb1, rgb2, rgb3);
                    string err2 = mcData.UpdateJobPlannerSupplierShortName(jobNo, suppShortName);
                    string err3 = mcData.UpdateDesignBoardSupplierShortName(jobNo, suppShortName);
                }

                return;
            }
            
            return;
        }

            
        //private void sAVEJobLineToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    dbDataGridView.NotifyCurrentCellDirty(true);
        //    int i = this.rowIndex;
        //    if (dbDataGridView.Rows[i].Cells[0].Value == null || dbDataGridView.Rows[i].Cells[0].Value.ToString().Length < 8) { return; }
        //    string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
        //    if (mcData.IsJobCompleted(jobNo)) { return; }
        //    DateTime designDate = Convert.ToDateTime(dbDataGridView.Rows[i].Cells[1].Value.ToString());
        //    int detailingDays = dbDataGridView.Rows[i].Cells[2].Value == null ? 0 : Convert.ToInt16(dbDataGridView.Rows[i].Cells[2].Value.ToString());
        //    string designStatus = dbDataGridView[3, this.rowIndex].Value.ToString();
        //    string dman = dbDataGridView[5, this.rowIndex].Value.ToString();
        //    string salesman = dbDataGridView[6, this.rowIndex].Value.ToString();
        //    DateTime requiredDate = Convert.ToDateTime(dbDataGridView.Rows[i].Cells[7].Value.ToString());
        //    string floorLevel = dbDataGridView[10, this.rowIndex].Value.ToString();
        //    string supplyType = dbDataGridView[11, this.rowIndex].Value.ToString();
        //    //string product = dbDataGridView[10, this.rowIndex].Value.ToString();
        //    string productSupplier = dbDataGridView[12, this.rowIndex].Value.ToString();
        //    //string stairsIncluded = (bool)dbDataGridView.Rows[i].Cells[12].Value ? "Y" : "N";
        //    //string stairsSupplier = dbDataGridView[13, this.rowIndex].Value.ToString();
        //    string supplierRef = mcData.GetSupplierRefByJobNo(jobNo);
            
        //    int slabM2 = dbDataGridView.Rows[i].Cells[13].Value == null ? 0 : Convert.ToInt16(dbDataGridView.Rows[i].Cells[13].Value.ToString());
        //    int beamM2 = dbDataGridView.Rows[i].Cells[14].Value == null ? 0 : Convert.ToInt16(dbDataGridView.Rows[i].Cells[14].Value.ToString());
        //    int beamLM = dbDataGridView.Rows[i].Cells[15].Value == null ? 0 : Convert.ToInt16(dbDataGridView.Rows[i].Cells[15].Value.ToString());
        //    string wcMonday = dbDataGridView[16, this.rowIndex].Value.ToString();
        //    string wcTuesday = dbDataGridView[17, this.rowIndex].Value.ToString();
        //    string wcWednesday = dbDataGridView[18, this.rowIndex].Value.ToString();
        //    string wcThursday = dbDataGridView[19, this.rowIndex].Value.ToString();
        //    string wcFriday = dbDataGridView[20, this.rowIndex].Value.ToString();
        //    string additionalNotes = dbDataGridView[21, this.rowIndex].Value.ToString();
        //    //string wcSaturday = dbDataGridView[22, this.rowIndex].Value.ToString();
        //    //string wcSunday = dbDataGridView[23, this.rowIndex].Value.ToString();
        //    //string drawingsEmailedFlag = (bool)dbDataGridView.Rows[i].Cells[24].Value ? "Y" : "N";
        //    //string draughtsman = dbDataGridView[25, this.rowIndex].Value.ToString();
        //    //DateTime createdDate = mcData.GetJobCreatedDateByJobNo(jobNo);
        //    //string completedFlag = mcData.GetCompletedFlagFromJob(jobNo);
        //    //DateTime requiredDate = mcData.GetPlannerDateByJobNo(jobNo);
        //    string sortType = "S" + supplyType.Substring(1, 1);

        //    string testline =
        //            "JobNo = " + jobNo + Environment.NewLine +
        //            "designDate = " + designDate.ToShortDateString() + Environment.NewLine +
        //            "detailingDays = " + detailingDays + Environment.NewLine +
        //            "designStatus = " + designStatus + Environment.NewLine +
        //            "draughtsman = " + dman + Environment.NewLine +
        //            "salesman = " + salesman + Environment.NewLine +
        //            "requiredDate = " + requiredDate.ToShortDateString() + Environment.NewLine +
        //            "floorLevel = " + floorLevel + Environment.NewLine +
        //            "supplyType = " + supplyType + Environment.NewLine +
        //            // "product = " + product + Environment.NewLine +
        //            "productSupplier = " + productSupplier + Environment.NewLine +
        //         //   "stairsIncluded = " + stairsIncluded + Environment.NewLine + //
        //         //   "stairsSupplier = " + stairsSupplier + Environment.NewLine +
        //            "supplierRef = " + supplierRef + Environment.NewLine +
                    
        //            "slabM2 = " + slabM2 + Environment.NewLine +
        //            "beamM2 = " + beamM2 + Environment.NewLine +
        //            "beamLM = " + beamLM + Environment.NewLine + 
        //            "wcMonday = " + wcMonday + Environment.NewLine +
        //            "wcTuesday = " + wcTuesday + Environment.NewLine +
        //            "wcWednesday = " + wcWednesday + Environment.NewLine +
        //            "wcThursday = " + wcThursday + Environment.NewLine +
        //            "wcFriday = " + wcFriday + Environment.NewLine +
        //            "additionalNotes = " + additionalNotes + Environment.NewLine;
        //    // "wcSaturday = " + wcSaturday + Environment.NewLine +
        //    // "wcSunday = " + wcSunday + Environment.NewLine +
        //    //// "drawingsEmailedFlag = " + drawingsEmailedFlag + Environment.NewLine +
        //    // "draughtsman = " + draughtsman + Environment.NewLine +
        //    //  "dateJobCreated = " + createdDate.ToShortDateString() + Environment.NewLine +
        //    //  "completedFlag = " + completedFlag + Environment.NewLine;

        //    //if (MessageBox.Show(testline, "Confirm you want save Design Board Job No.[" + jobNo + "] line ? ", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        //    //{
        //    //    string err = mcData.UpdateDesignBoardLine(jobNo, designDate, designStatus, detailingDays, requiredDate,dman,floorLevel, productSupplier,
        //    //                            supplierRef, salesman, supplyType, slabM2, beamM2, beamLM,
        //    //                            wcMonday, wcTuesday, wcWednesday, wcThursday, wcFriday,additionalNotes,sortType);
        //    //    if (err == "OK")
        //    //    {
        //    //        string jpDesignStatus = mcData.GetJobPlannerStatusFromDesignerJob(jobNo);
        //    //        string err1 = mcData.UpdateJobPlannerFromDesignBoardJob(jobNo, designDate, jpDesignStatus, requiredDate);
        //    //        string err2 = mcData.UpdateWhiteBoardFromDesignBoardJob(jobNo, requiredDate,supplyType, productSupplier, dman, salesman);
        //    //        MessageBox.Show("Design Board JobNo[" + jobNo + "] line saved successfully");
        //    //        return;
        //    //    }
        //    //    else
        //    //    {
        //    //        mcData.CreateErrorAudit("DesignboardForm.cs", "sAVEJobLineToolStripMenuItem_Click on Job[" + jobNo + "]", err);
        //    //        MessageBox.Show($"Error saving JobNo[{jobNo}] : {err} ");
        //    //        return;

        //    //    }
        //    //}

        //    string err = mcData.UpdateDesignBoardLine(jobNo, designDate, designStatus, detailingDays, requiredDate, dman, floorLevel, productSupplier,
        //                                supplierRef, salesman, supplyType, slabM2, beamM2, beamLM,
        //                                wcMonday, wcTuesday, wcWednesday, wcThursday, wcFriday, additionalNotes, sortType);
        //    if (err == "OK")
        //    {
        //        string jpDesignStatus = mcData.GetJobPlannerStatusFromDesignerJob(jobNo);
        //        string err1 = mcData.UpdateJobPlannerFromDesignBoardJob(jobNo, designDate, jpDesignStatus, requiredDate);
        //        string err2 = mcData.UpdateWhiteBoardFromDesignBoardJob(jobNo, requiredDate, supplyType, productSupplier, dman, salesman);
        //        MessageBox.Show("Design Board JobNo[" + jobNo + "] line saved successfully");
        //        return;
        //    }
        //    else
        //    {
        //        mcData.CreateErrorAudit("DesignboardForm.cs", "sAVEJobLineToolStripMenuItem_Click on Job[" + jobNo + "]", err);
        //        MessageBox.Show($"Error saving JobNo[{jobNo}] : {err} ");
        //        return;

        //    }
        //    return;
        //}

        private void jobCommentsAuditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            return;
            // this actually a GO TO WHITEBOARD JOB menu item option
            //if (dbDataGridView[0, rowIndex].Value == null) { return; }
            //string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            //if (!mcData.IsValidWhiteboardJob(jobNo))
            //{
            //    MessageBox.Show(String.Format("Job [{0}] is not [ON SHOP] or [APPROVED]. Cannot continue to Whiteboard", jobNo));
            //    return;
            //}

            //if (mcData.IsJobCompleted(jobNo))
            //{
            //    MessageBox.Show(String.Format("Job [{0}] is flagged as COMPLETED. Cannot continue to Whiteboard", jobNo));
            //    return;
            //}
            //// int numWeeks = 1;
            //DataTable dt = mcData.WhiteboardDatesDT(jobNo);
            //DateTime jobDate = mcData.GetPlannerDateByJobNo(jobNo);
            //DateTime startDate = mcData.GetMonday(jobDate);
            //DateTime lastDate = startDate.AddDays(6);
            //TimeSpan ts = lastDate - startDate;
            //int dateDiff = ts.Days;
            //decimal numWeeks = dateDiff / 7m;
            //int roundedNumWeeks = (int)Decimal.Round(numWeeks, 1) + 1;
            //WhiteboardForm wbForm = new WhiteboardForm(jobNo, startDate, lastDate, dt, roundedNumWeeks);
            //wbForm.ShowDialog();
        } // Go To WB menu item option

        private void selectDesignStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            DateTime designDate = Convert.ToDateTime(dbDataGridView[1, this.rowIndex].Value.ToString());
            if(mcData.IsJobCompleted(jobNo)) { return; }
            DesignStatusSelectorForm form = new DesignStatusSelectorForm();
            form.ShowDialog();
            if(!String.IsNullOrWhiteSpace(form.Status))
            {
                dbDataGridView[3, rowIndex].Value = form.Status;
                string err = mcData.UpdateDesignStatus(jobNo, form.Status);
                string auditErr = mcData.CreateDesignStatusAudit(jobNo, designDate, form.Status, "Design Status updated in DesignBoard via right click SELECT DESIGN STATUS option");
                if (form.Status.Contains("APPROVED (NOT ON SHOP)"))
                {
                    string err2 = mcData.UpdateJobPlannerApprovedFlag(jobNo, "Y");
                    string err3 = mcData.UpdateJobPlannerOnShopFlag(jobNo, "N");
                }
                else
                {
                    string err4 = mcData.UpdateJobPlannerApprovedFlag(jobNo, "N");
                    string err5 = mcData.UpdateJobPlannerOnShopFlag(jobNo, "N");
                }
            }
            return;
        }

        private void canToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (dbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            int detailingDays = String.IsNullOrWhiteSpace(dbDataGridView[2, this.rowIndex].Value.ToString()) ? 0 : Convert.ToInt16(dbDataGridView[2, this.rowIndex].Value.ToString());
            if (mcData.IsJobCompleted(jobNo)) { return; }
            DateTime selectedDate = DateTime.MinValue;

            if (colIndex == 1)
            {
                string warning = $@"Changing DESIGN DATE for Job [{jobNo}] will mean resetting DETAILING DAYS to 1.{Environment.NewLine} 
                                    Any notes belonging to the old design date week will remain but NEW notes will be required for the new design date{Environment.NewLine}
                                    Do you wish to continue ?";
                if( MessageBox.Show(warning,"Changing Design Date",MessageBoxButtons.YesNo) == DialogResult.No) { return; }
                if (dbDataGridView[1, rowIndex].Value == null)
                {
                    DateSelectorForm dateForm1 = new DateSelectorForm();
                    dateForm1.ShowDialog();
                    selectedDate = dateForm1.RequiredDate;
                    dbDataGridView[1, rowIndex].Value = selectedDate.Date.ToShortDateString();
                    
                }
                else
                {
                    DateTime currDate = Convert.ToDateTime(dbDataGridView[1, rowIndex].Value.ToString());
                    DateSelectorForm dateForm = new DateSelectorForm(currDate);
                    dateForm.ShowDialog();
                    selectedDate = dateForm.RequiredDate;
                    dbDataGridView[1, rowIndex].Value = selectedDate.Date.ToShortDateString();
                    
                }
                mcData.UpdateDesignBoardColourCodeDayFlags(jobNo, selectedDate, 1, (int)selectedDate.DayOfWeek);
                mcData.UpdateJobPlannerDesignDate(jobNo, selectedDate);
                ColourCodeDayCells(dbDataGridView, rowIndex, 5, 0);


                // ****** Mike wanted this pop up message removed  - 14/11/2025 so I am commenting out ******
                //if(!mcData.IsDateWithinDateRange(selectedDate,dbStartDate,dbEndDate))
                //{
                //    MessageBox.Show($@"New selected design date [{selectedDate.ToShortDateString()}] sits outside this Design Board's 
                //        date range period [{dbStartDate.ToShortDateString()} - {dbEndDate.ToShortDateString()}] {Environment.NewLine}{Environment.NewLine}.
                //        This will require you to close and open the Design Board again in order to the access the moved job ");
                //    return;
                //}
                //else
                //{
                //    //commented out due to each date change taking 2 mins because of refresh
                //    //this.Text = String.Format("Design Board spanning period between {0} and {1}", dbStartDate.ToString("dd/MMM/yyyy"), dbEndDate.ToString("dd/MMM/yyyy"));
                //    //suppTypeBindngSource.DataSource = mcData.GetSupplyTypeDT();
                //    //salesmanBindngSource.DataSource = mcData.GetSalesmanDT();
                //    //BuildTabs();
                //    return;
                //}
               // return;
            }

            if (colIndex == 7)
            {
                if (dbDataGridView[7, rowIndex].Value == null)
                {
                    DateSelectorForm dateForm1 = new DateSelectorForm();
                    dateForm1.ShowDialog();
                    selectedDate = dateForm1.RequiredDate;
                    dbDataGridView[7, rowIndex].Value = selectedDate.Date.ToShortDateString();
                }
                else
                {
                    DateTime currDate = Convert.ToDateTime(dbDataGridView[7, rowIndex].Value.ToString());
                    DateSelectorForm dateForm = new DateSelectorForm(currDate);
                    dateForm.ShowDialog();
                    selectedDate = dateForm.RequiredDate;
                    dbDataGridView[7, rowIndex].Value = selectedDate.Date.ToShortDateString();
                }
                string err1 = mcData.UpdateJobPlannerJobDate(jobNo, selectedDate);
                string err2 = mcData.UpdateWhiteBoardJobDate(jobNo, selectedDate);
                string err3 = mcData.UpdateDesignBoardJobDate(jobNo, selectedDate);
                return;
            }


        }


        private void AdditionalCommentContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (dbDataGridView[0, rowIndex].Value == null) { return; }
            if (colIndex != 21) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            string existingNote = dbDataGridView[21, this.rowIndex].Value.ToString();
            WhiteboardDayCommentForm frm = new WhiteboardDayCommentForm(jobNo, existingNote, "DB");
            frm.ShowDialog();
            dbDataGridView[21, this.rowIndex].Value = frm.DayComment; // re-using WB day comment for designboard additionl comments
            string result = mcData.UpdateDesignBoardAdditionalComment(jobNo, frm.DayComment);
            return;
        }

        private void AddAdditionalCommentMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void GoToJobPlannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = this.dbDataGridView[0, this.rowIndex].Value.ToString();
            int parentJob = Convert.ToInt32(jobNo.Substring(0, 5));
            DataTable dt = mcData.GetJobPlannerDT(parentJob);
            bool isSingleJobSearchFlag = true;
            JobPlannerForm plannerForm = new JobPlannerForm(isSingleJobSearchFlag,jobNo);
            plannerForm.ShowDialog();
            
            return;
        }

        private void exportToEXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.dbDataGridView.NotifyCurrentCellDirty(true);
            int i = this.rowIndex;
            if (this.dbDataGridView.Rows[i].Cells[0].Value == null || this.dbDataGridView.Rows[i].Cells[0].Value.ToString().Length < 8) { return; }
            DateTime designDate = Convert.ToDateTime(this.dbDataGridView.Rows[i].Cells[1].Value.ToString());
            DateTime wcDate = mcData.GetMonday(designDate);
            this.Cursor = Cursors.Default;
            MyBoardReportForm rptForm = new MyBoardReportForm("DB", wcDate);
            rptForm.ShowDialog();
        }

        private async void RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshBtn.Text = "Pls Wait.Refreshing ....";
            await Task.Delay(2000);
            this.Text = String.Format("Design Board spanning period between {0} and {1}", dbStartDate.ToString("dd/MMM/yyyy"), dbEndDate.ToString("dd/MMM/yyyy"));
            suppTypeBindngSource.DataSource = mcData.GetSupplyTypeDT();
            salesmanBindngSource.DataSource = mcData.GetSalesmanDT();
            BuildTabs();
            MessageBox.Show("DesignBoard has refreshed successfully");
            RefreshBtn.Text = "Refresh the Design Board ";
            return;
        }

        private void designStatusAuditForJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = this.dbDataGridView[0, this.rowIndex].Value.ToString();
            DataTable dt = mcData.GetJobDesignStatusAuditDT(jobNo);
            if(dt.Rows.Count < 1)
            {
                MessageBox.Show($"There are no audit entries for job no. {jobNo}");
                return;
            }
            JobDesignStatusAuditForm auditForm = new JobDesignStatusAuditForm(jobNo,dt);
            auditForm.ShowDialog();
        }
    }
}
