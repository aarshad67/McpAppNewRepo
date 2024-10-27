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
                            AddContextMenu((DataGridView)dgv);
                            dbDataGridView = (DataGridView)dgv;
                            dbDataGridView.CellMouseUp += new DataGridViewCellMouseEventHandler(dbDataGridView_CellMouseUp);
                            dbDataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dbDataGridView_EditingControlShowing);
                            dbDataGridView.DataError += new DataGridViewDataErrorEventHandler(dbDataGridView_DataError);
                            ((DataGridView)dgv).CellValueChanged += new DataGridViewCellEventHandler(dbDataGridView_CellValueChanged);
                            ((DataGridView)dgv).CellClick += new DataGridViewCellEventHandler(dbDataGridView_CellClick);
                            ((DataGridView)dgv).CellBeginEdit += new DataGridViewCellCancelEventHandler(dbDataGridView_CellBeginEdit);
                            ((DataGridView)dgv).CellEndEdit += new DataGridViewCellEventHandler(dbDataGridView_CellEndEdit);
                            ((DataGridView)dgv).CellFormatting += new DataGridViewCellFormattingEventHandler(dbDataGridView_CellFormatting);
                        }
                    }

                }
            }




            Cursor.Current = Cursors.Hand;
        }

        private void dbDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            return;
        }

        private void dbDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
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
            return;
        }

        private void dbDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            return;
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

            this.Cursor = Cursors.WaitCursor;
            DataTable dt = mcData.GeDesignboardByDateRangeDT(myStartDate, myEndDate);
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
                productSupplier = dr["productSupplier"].ToString();
                stairsSupplier = dr["stairsSupplier"].ToString();
                if (!mcData.IsValidWhiteboardJob(jobNo)) { continue; }
                designDateStr = Convert.ToDateTime(dr["designDate"].ToString()).DayOfWeek.ToString().ToUpper().Substring(0, 3);
                dateStr = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().ToUpper().Substring(0, 3);
                custName = mcData.GetCustomerNameByJobNo(jobNo);
                siteAddr = mcData.GetSiteAddressFromJobNo(jobNo);
                mcData.GetSupplierColourByShortname(productSupplier, out rgb1, out rgb2, out rgb3);
                mcData.GetSupplierColourByShortname(stairsSupplier, out srgb1, out srgb2, out srgb3);
                dateCreated = mcData.GetJobCreatedDate(jobNo).ToString("dd/MMM/yyyy hh:mm tt");

                monFlag = dateStr.Substring(0, 3).ToUpper() == "MON" ? "Y" : String.Empty;
                tueFlag = dateStr.Substring(0, 3).ToUpper() == "TUE" ? "Y" : String.Empty;
                wedFlag = dateStr.Substring(0, 3).ToUpper() == "WED" ? "Y" : String.Empty;
                thuFlag = dateStr.Substring(0, 3).ToUpper() == "THU" ? "Y" : String.Empty;
                friFlag = dateStr.Substring(0, 3).ToUpper() == "FRI" ? "Y" : String.Empty;
                satFlag = dateStr.Substring(0, 3).ToUpper() == "SAT" ? "Y" : String.Empty;
                sunFlag = dateStr.Substring(0, 3).ToUpper() == "SUN" ? "Y" : String.Empty;
                //   myDGV.Rows.Add();
                DataGridViewRow drow = new DataGridViewRow();
                drow.CreateCells(myDGV);
                drow.Cells[0].Value = dr["jobNo"].ToString();
                //drow.Cells[0].Style.BackColor = Color.Cyan;
                drow.Cells[1].Value = Convert.ToDateTime(dr["designDate"].ToString()).ToShortDateString();
                drow.Cells[2].Value = dr["designStatus"].ToString();
                drow.Cells[3].Value = dr["designStatus"].ToString().Contains("APPROVED") || dr["designStatus"].ToString() == "ON SHOP" ? 0 : mcData.GetDaysDiffBetweenTwDates(Convert.ToDateTime(dr["dateJobCreated"].ToString()));
                drow.Cells[4].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString(); ;
                drow.Cells[5].Value = custName;
                drow.Cells[6].Value = siteAddr;
                drow.Cells[7].Value = dr["floorlevel"].ToString();
                drow.Cells[8].Value = dr["supplyType"].ToString();
                drow.Cells[9].Value = dr["salesman"].ToString();
                drow.Cells[10].Value = dr["product"].ToString();
                drow.Cells[11].Value = dr["productSupplier"].ToString();
                drow.Cells[11].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                drow.Cells[12].Value = dr["stairsIncluded"].ToString().Contains("Y") ? true : false;
                drow.Cells[13].Value = dr["stairsSupplier"].ToString();
                drow.Cells[13].Style.BackColor = Color.FromArgb(srgb1, srgb2, srgb3);
                drow.Cells[14].Value = Convert.ToInt32(dr["slabM2"].ToString());
                drow.Cells[15].Value = Convert.ToInt32(dr["beamM2"].ToString());
                drow.Cells[16].Value = Convert.ToInt32(dr["beamLM"].ToString());
                drow.Cells[17].Value = dr["wcMonday"].ToString();
                drow.Cells[18].Value = dr["wcTuesday"].ToString();
                drow.Cells[19].Value = dr["wcWednesday"].ToString();
                drow.Cells[20].Value = dr["wcThursday"].ToString();
                drow.Cells[21].Value = dr["wcFriday"].ToString();
                drow.Cells[22].Value = dr["wcSaturday"].ToString();
                drow.Cells[23].Value = dr["wcSunday"].ToString();
                drow.Cells[17].Style.BackColor = monFlag == "Y" ? Color.Yellow : Color.White;
                drow.Cells[18].Style.BackColor = tueFlag == "Y" ? Color.Yellow : Color.White;
                drow.Cells[19].Style.BackColor = wedFlag == "Y" ? Color.Yellow : Color.White;
                drow.Cells[20].Style.BackColor = thuFlag == "Y" ? Color.Yellow : Color.White;
                drow.Cells[21].Style.BackColor = friFlag == "Y" ? Color.Yellow : Color.White;
                drow.Cells[22].Style.BackColor = satFlag == "Y" ? Color.Yellow : Color.White;
                drow.Cells[23].Style.BackColor = sunFlag == "Y" ? Color.Yellow : Color.White;
                drow.Cells[24].Value = dr["drawingsEmailedFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[25].Value = dr["draughtsman"].ToString();
                drow.Cells[26].Value = dateCreated;
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
                    dgvRow.Frozen = true;
                }
                else if (mcData.IsJobCustomerOnStop(custCode))
                {
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                }
                if (mcData.IsJobLockExistByOtherUser("WB", dgvRow.Cells[0].Value.ToString(), loggedInUser))
                {
                    dgvRow.Frozen = true;
                    dgvRow.DefaultCellStyle.ForeColor = Color.MediumPurple;
                }
            }
        }

        private void BuildDesignBoardDGV(DataGridView myDGV, DateTime wcDate)
        {
            try
            {
                dbDataGridView = myDGV;
                dbDataGridView.Dock = DockStyle.None;
                dbDataGridView.Controls[1].Enabled = true;
                dbDataGridView.Controls[1].Visible = true;
                dbDataGridView.ScrollBars = ScrollBars.Both;
                dbDataGridView.Columns.Clear();
                //0
                DataGridViewTextBoxColumn jobNoColumn = new DataGridViewTextBoxColumn();//0
                jobNoColumn.DataPropertyName = "JobNo";
                jobNoColumn.HeaderText = "Job No.";
                jobNoColumn.Width = 70;
                jobNoColumn.ReadOnly = true;
                //jobNoColumn.Frozen = true;
                jobNoColumn.DefaultCellStyle.ForeColor = Color.Black;
                jobNoColumn.DefaultCellStyle.BackColor = Color.Cyan;
                dbDataGridView.Columns.Add(jobNoColumn);

                //1
                DataGridViewTextBoxColumn designDateColumn = new DataGridViewTextBoxColumn();
                designDateColumn.HeaderText = "Design Date";
                designDateColumn.Width = 80;
                designDateColumn.ReadOnly = true;
                //designDateColumn.Frozen = true;
                designDateColumn.DefaultCellStyle.BackColor = Color.Yellow;
                designDateColumn.DefaultCellStyle.ForeColor = Color.Blue;
                dbDataGridView.Columns.Add(designDateColumn);

                //2
                DataGridViewTextBoxColumn designStatusColumn = new DataGridViewTextBoxColumn();
                designStatusColumn.HeaderText = "Design Status";
                designStatusColumn.Width = 120;
                //designStatusColumn.Frozen = false;
                designStatusColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(designStatusColumn);

                //3
                DataGridViewTextBoxColumn daysUnapprovedColumn = new DataGridViewTextBoxColumn();
                daysUnapprovedColumn.HeaderText = "Days UnApprvd";
                daysUnapprovedColumn.Width = 70;
              // daysUnapprovedColumn.Frozen = true;
                daysUnapprovedColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(daysUnapprovedColumn);

                //4
                DataGridViewTextBoxColumn reqDateColumn = new DataGridViewTextBoxColumn();
                reqDateColumn.HeaderText = "Required Date";
                reqDateColumn.Width = 80;
                reqDateColumn.ReadOnly = true;
                //reqDateColumn.Frozen = false;
                reqDateColumn.DefaultCellStyle.ForeColor = Color.Blue;
                dbDataGridView.Columns.Add(reqDateColumn);

                

                

                //5
                DataGridViewTextBoxColumn custColumn = new DataGridViewTextBoxColumn();
                custColumn.DataPropertyName = "CustName";
                custColumn.HeaderText = "Customer";
                custColumn.Width = 150;
               // custColumn.Frozen = false;
                custColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(custColumn);

                //6
                DataGridViewTextBoxColumn siteColumn = new DataGridViewTextBoxColumn();
                siteColumn.DataPropertyName = "site";
                siteColumn.HeaderText = "Site Address";
                siteColumn.Width = 250;
              //  siteColumn.Frozen = false;
                siteColumn.ReadOnly = true;
                siteColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dbDataGridView.Columns.Add(siteColumn);

                //7
                DataGridViewTextBoxColumn levelColumn = new DataGridViewTextBoxColumn();
                levelColumn.DataPropertyName = "Level";
                levelColumn.HeaderText = "Floor Level";
                levelColumn.Width = 100;
                levelColumn.DefaultCellStyle.ForeColor = Color.Blue;
                levelColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                levelColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(levelColumn);

                //8
                DataGridViewTextBoxColumn supplyTypeColumn = new DataGridViewTextBoxColumn();
                supplyTypeColumn.DataPropertyName = "type";
                supplyTypeColumn.HeaderText = "TYPE";
                supplyTypeColumn.Width = 50;
                supplyTypeColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(supplyTypeColumn);

                //9
                DataGridViewTextBoxColumn salesmanColumn = new DataGridViewTextBoxColumn();//41
                salesmanColumn.DataPropertyName = "salesman";
                salesmanColumn.HeaderText = "Salesman";
                salesmanColumn.Width = 90;
                salesmanColumn.ReadOnly = false;
                dbDataGridView.Columns.Add(salesmanColumn);

                //10
                DataGridViewTextBoxColumn productsColumn = new DataGridViewTextBoxColumn();//5
                productsColumn.DataPropertyName = "products";
                productsColumn.HeaderText = "Product (Right Clk)";
                productsColumn.Width = 60;
                productsColumn.ReadOnly = false;
                dbDataGridView.Columns.Add(productsColumn);

                //11
                DataGridViewTextBoxColumn productSupplierColumn = new DataGridViewTextBoxColumn();
                productSupplierColumn.DataPropertyName = "productSupplier";
                productSupplierColumn.HeaderText = "Supplier (Right Click)";
                productSupplierColumn.Width = 70;
                productSupplierColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(productSupplierColumn);

                //12
                DataGridViewCheckBoxColumn stairsColumn = new DataGridViewCheckBoxColumn();
                stairsColumn.DataPropertyName = "stairs";
                stairsColumn.ValueType = typeof(bool);
                stairsColumn.HeaderText = "Stairs";
                stairsColumn.Width = 70;
                stairsColumn.ReadOnly = false;
                dbDataGridView.Columns.Add(stairsColumn);

                //13
                DataGridViewTextBoxColumn stairSupplierColumn = new DataGridViewTextBoxColumn();
                stairSupplierColumn.DataPropertyName = "stairSupplier";
                stairSupplierColumn.HeaderText = "StairsSupplier (Right Click)";
                stairSupplierColumn.Width = 70;
                stairSupplierColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(stairSupplierColumn);

                //14
                DataGridViewTextBoxColumn slabM2Column = new DataGridViewTextBoxColumn();
                slabM2Column.DataPropertyName = "slabM2";
                slabM2Column.HeaderText = "Slab M²";
                slabM2Column.Width = 60;
                slabM2Column.ReadOnly = false;
                dbDataGridView.Columns.Add(slabM2Column);

                //15
                DataGridViewTextBoxColumn beamM2Column = new DataGridViewTextBoxColumn();
                beamM2Column.DataPropertyName = "slabM2";
                beamM2Column.HeaderText = "Beam M²";
                beamM2Column.Width = 60;
                beamM2Column.ReadOnly = false;
                dbDataGridView.Columns.Add(beamM2Column);

                //16
                DataGridViewTextBoxColumn beamLMColumn = new DataGridViewTextBoxColumn();
                beamLMColumn.DataPropertyName = "beamLM";
                beamLMColumn.HeaderText = "Beam LM";
                beamLMColumn.Width = 60;
                beamLMColumn.ReadOnly = false;
                dbDataGridView.Columns.Add(beamLMColumn);

                //17
                DataGridViewTextBoxColumn monColumn = new DataGridViewTextBoxColumn();
                monColumn.Name = "mon";
                monColumn.HeaderText = "MON " + wcDate.Day.ToString() + "/" + wcDate.ToString("MMM");
                monColumn.Width = 80;
                monColumn.DefaultCellStyle.BackColor = Color.Yellow;
                monColumn.DefaultCellStyle.ForeColor = Color.Blue;
                monColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(monColumn);

                //18
                DataGridViewTextBoxColumn tueColumn = new DataGridViewTextBoxColumn();
                tueColumn.DataPropertyName = "tue";
                tueColumn.HeaderText = "TUE " + wcDate.AddDays(1).Day.ToString() + "/" + wcDate.AddDays(1).ToString("MMM");
                tueColumn.Width = 80;
                tueColumn.DefaultCellStyle.BackColor = Color.Yellow;
                tueColumn.DefaultCellStyle.ForeColor = Color.Blue;
                tueColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(tueColumn);

                //19
                DataGridViewTextBoxColumn wedColumn = new DataGridViewTextBoxColumn();
                wedColumn.DataPropertyName = "wed";
                wedColumn.HeaderText = "WED " + wcDate.AddDays(2).Day.ToString() + "/" + wcDate.AddDays(2).ToString("MMM");
                wedColumn.Width = 80;
                wedColumn.DefaultCellStyle.BackColor = Color.Yellow;
                wedColumn.DefaultCellStyle.ForeColor = Color.Blue;
                wedColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(wedColumn);

                //20
                DataGridViewTextBoxColumn thuColumn = new DataGridViewTextBoxColumn();
                thuColumn.DataPropertyName = "thu";
                thuColumn.HeaderText = "THU " + wcDate.AddDays(3).Day.ToString() + "/" + wcDate.AddDays(3).ToString("MMM");
                thuColumn.Width = 80;
                thuColumn.DefaultCellStyle.BackColor = Color.Yellow;
                thuColumn.DefaultCellStyle.ForeColor = Color.Blue;
                thuColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(thuColumn);

                //21
                DataGridViewTextBoxColumn friColumn = new DataGridViewTextBoxColumn();
                friColumn.DataPropertyName = "fri";
                friColumn.HeaderText = "FRI " + wcDate.AddDays(4).Day.ToString() + "/" + wcDate.AddDays(4).ToString("MMM");
                friColumn.Width = 80;
                friColumn.DefaultCellStyle.BackColor = Color.Yellow;
                friColumn.DefaultCellStyle.ForeColor = Color.Blue;
                friColumn.ReadOnly = true;
                dbDataGridView.Columns.Add(friColumn);

                //22
                DataGridViewTextBoxColumn satColumn = new DataGridViewTextBoxColumn();
                satColumn.DataPropertyName = "sat";
                satColumn.HeaderText = "SAT " + wcDate.AddDays(5).Day.ToString() + "/" + wcDate.AddDays(5).ToString("MMM");
                satColumn.Width = 80;
                satColumn.DefaultCellStyle.ForeColor = Color.Blue;
                satColumn.ReadOnly = true;
                satColumn.DefaultCellStyle.BackColor = Color.Gray;
                dbDataGridView.Columns.Add(satColumn);

                //23
                DataGridViewTextBoxColumn sunColumn = new DataGridViewTextBoxColumn();
                sunColumn.DataPropertyName = "sun";
                sunColumn.HeaderText = "SUN " + wcDate.AddDays(6).Day.ToString() + "/" + wcDate.AddDays(6).ToString("MMM");
                sunColumn.Width = 80;
                sunColumn.DefaultCellStyle.ForeColor = Color.Blue;
                sunColumn.ReadOnly = true;
                sunColumn.DefaultCellStyle.BackColor = Color.Gray;
                dbDataGridView.Columns.Add(sunColumn);
  
                //24
                DataGridViewCheckBoxColumn drawingsEmailedColumn = new DataGridViewCheckBoxColumn();
                drawingsEmailedColumn.DataPropertyName = "dwgEmailed";
                drawingsEmailedColumn.ValueType = typeof(bool);
                drawingsEmailedColumn.HeaderText = "Drwg Emailed";
                drawingsEmailedColumn.Width = 50;
                drawingsEmailedColumn.ReadOnly = false;
                dbDataGridView.Columns.Add(drawingsEmailedColumn);

                //25
                DataGridViewTextBoxColumn draughtsmanColumn = new DataGridViewTextBoxColumn();
                draughtsmanColumn.DataPropertyName = "Draughtsman";
                draughtsmanColumn.HeaderText = "Draughtsman";
                draughtsmanColumn.Width = 90;
                draughtsmanColumn.ReadOnly = false;
                dbDataGridView.Columns.Add(draughtsmanColumn);
                
                //26
                DataGridViewTextBoxColumn dateCreatedColumn = new DataGridViewTextBoxColumn();
                dateCreatedColumn.DataPropertyName = "createdDateColumn";
                dateCreatedColumn.HeaderText = "Created";
                dateCreatedColumn.Width = 100;
                dateCreatedColumn.ReadOnly = false;
                dbDataGridView.Columns.Add(dateCreatedColumn);
                



                dbDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dbDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dbDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                dbDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                dbDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dbDataGridView.Dock = DockStyle.Fill;
                dbDataGridView.AllowUserToAddRows = false;

                dbDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.Cyan;
                dbDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.Yellow;
                dbDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.Yellow;
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

                if (column.Index >= 17 && column.Index <= 23)
                {
                    column.ContextMenuStrip = wbDailyContextMenuStrip1;

                }

                if (column.Index == 0)
                {
                    column.ContextMenuStrip = jobContextMenuStrip1;

                }

                if (column.Index == 10)
                {
                    column.ContextMenuStrip = productContextMenuStrip;

                }

                if (column.Index == 11 || column.Index == 13)
                {
                    column.ContextMenuStrip = supplierContextMenuStrip;

                }
            }
        }

        private void DesignBoardForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Design Board spanning period between {0} and {1}", dbStartDate.ToString("dd/MMM/yyyy"), dbEndDate.ToString("dd/MMM/yyyy"));
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
                case 17:
                    fieldName = "wcMonday";
                    break;
                case 18:
                    fieldName = "wcTuesday";
                    break;
                case 19:
                    fieldName = "wcWednesday";
                    break;
                case 20:
                    fieldName = "wcThursday";
                    break;
                case 21:
                    fieldName = "wcFriday";
                    break;
                case 22:
                    fieldName = "wcSaturday";
                    break;
                case 23:
                    fieldName = "wcSunday";
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
            if (colIndex != 17 && colIndex != 18 && colIndex != 19 && colIndex != 20 && colIndex != 21 && colIndex != 22 && colIndex != 23) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            DateTime reqDate = Convert.ToDateTime(dbDataGridView[4, this.rowIndex].Value.ToString()).Date;
            string comment = dbDataGridView[this.colIndex, this.rowIndex].Value == null ? "N/A" : dbDataGridView[this.colIndex, this.rowIndex].Value.ToString();
            int diffDays = colIndex - 17;
            DateTime commentDate = mcData.GetMonday(reqDate).AddDays(diffDays).AddDays(0);
            WhiteboardDayCommentForm commentForm = new WhiteboardDayCommentForm(jobNo, commentDate, comment);
            commentForm.ShowDialog();
            dbDataGridView[this.colIndex, this.rowIndex].Value = commentForm.DayComment;
            UpdateDBJobDayCommentOnly(jobNo, commentForm.DayComment, colIndex, commentDate);
        }

        private void selectProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
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

            if (dbDataGridView[0, rowIndex].Value == null) { return; }

            if (dbDataGridView[11, rowIndex].Value == null && dbDataGridView[11, rowIndex].Value == null) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            string suppShortName = dbDataGridView[11, this.rowIndex].Value.ToString();
            if (colIndex == 11)
            {
                if (dbDataGridView[11, rowIndex].Value == null)
                {
                    myForm = new SuppliersListForm();
                }
                else
                {
                    myForm = new SuppliersListForm(dbDataGridView.Rows[rowIndex].Cells[11].Value.ToString());
                }
                myForm.ShowDialog();
                suppShortName = myForm.Shortname;
                string productType = mcData.GetSupplierProductTypeFromShortname(suppShortName);
                string result = mcData.UpdateWhiteBoardJobProductWithSupplierProductType(jobNo, productType);
                string wbProduct = mcData.GetWhiteboardProductFromSupplierProductType(productType);
                dbDataGridView[10, rowIndex].Value = wbProduct;
                dbDataGridView[11, rowIndex].Value = suppShortName;
                mcData.GetSupplierColourByShortname(suppShortName, out rgb1, out rgb2, out rgb3);
                dbDataGridView[11, rowIndex].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                if (!String.IsNullOrWhiteSpace(suppShortName))
                {
                    string err1 = mcData.UpdateWhiteBoardSupplierShortName(jobNo, suppShortName, rgb1, rgb2, rgb3);
                    string err2 = mcData.UpdateJobPlannerSupplierShortName(jobNo, suppShortName);
                    string err3 = mcData.UpdateDesignBoardSupplierShortName(jobNo, suppShortName);
                }

                return;
            }
    }

        private void sAVEJobLineToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void jobCommentsAuditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = dbDataGridView[0, this.rowIndex].Value.ToString();
            WhiteboardDayCommentAuditForm auditForm = new WhiteboardDayCommentAuditForm(jobNo);
            auditForm.ShowDialog();
            return;
        }
    }
