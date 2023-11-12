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
    public partial class WhiteboardForm : Form
    {
        Logger logger = new Logger();
        private DateTime wbStartDate = DateTime.MinValue;
        private DateTime wbEndDate = DateTime.MinValue;
        private string wbCompCode = "";
        private string wbParentJob = "";
        private string wbSupplyType = "";
        MeltonData mcData = new MeltonData();
        private DateTime tabPageDate;

        private System.Data.DataTable wbDT = new System.Data.DataTable();
        private System.Data.DataTable wbDatesDT = new System.Data.DataTable();
        private int wcNumWeeks = 0;
        private DateTime wbStartTime = DateTime.MinValue;
        private DataGridView wbDataGridView = new DataGridView();
        private const string netsValue = "(NETS)";
        private string selectedJob = "";

        private int rowIndex = 0;
        private int colIndex = 0;

        private List<int> chkBoxColumns = new List<int>(new[] { 10, 13, 14, 15, 24, 36, 37, 39 });
        private List<int> editColumns = new List<int>(new[] { 7, 8, 25, 27, 32, 33, 34, 38, 42 });

        private string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];


        public WhiteboardForm()
        {
            InitializeComponent();
        }



        public WhiteboardForm(DateTime startDate, DateTime endDate, DataTable datesDT, int numWeeks)
        {
            InitializeComponent();
            wbStartDate = startDate;
            wbEndDate = endDate;

            wbDatesDT = datesDT;
            wcNumWeeks = numWeeks;

        }

        public WhiteboardForm(string job, DateTime startDate, DateTime endDate, DataTable datesDT, int numWeeks)
        {
            InitializeComponent();
            wbStartDate = startDate;
            wbEndDate = endDate;

            wbDatesDT = datesDT;
            wcNumWeeks = numWeeks;
            selectedJob = job;

        }

        private void BuildTabs()
        {
            this.weeksTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.weeksTabControl_DrawItem);
            Cursor.Current = Cursors.WaitCursor;
            DateTime wcDate = DateTime.MinValue;
            int numTabsCreated = 0;

            for (int i = 0; i < wcNumWeeks; i++)
            {
                foreach (DataRow dr in wbDatesDT.Rows)
                {
                    if (Convert.ToInt16(dr["tabNo"].ToString()) == i + 1 || Convert.ToInt16(dr["tabNo"].ToString()) == 0)
                    {
                        wcDate = Convert.ToDateTime(dr["wcDate"].ToString());
                        break;
                    }
                }

                // if (!mcData.IsValidWhiteboardJobsOnDate(wcDate)) continue;
                //  {
                weeksTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
                //  weeksTabControl.TabPages.Add(wcDate.ToString("dd-MMM-yy"));
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
                //  }
                //  else
                //  {
                //      continue;
                //  }

            }

            // MessageBox.Show(String.Format("Number of tabs created = {0}",numTabsCreated.ToString()));

            foreach (Control thisControl in weeksTabControl.Controls)
            {
                if (thisControl.GetType() == typeof(TabPage))
                {
                    foreach (Control dgv in thisControl.Controls)
                    {
                        if (dgv.GetType() == typeof(DataGridView))
                        {
                            BuildWhiteboardDGV((DataGridView)dgv, Convert.ToDateTime(thisControl.Text).Date);
                            PopulateWhiteboardDGV((DataGridView)dgv, Convert.ToDateTime(thisControl.Text).Date, Convert.ToDateTime(thisControl.Text).Date.AddDays(6));
                            AddContextMenu((DataGridView)dgv);
                            wbDataGridView = (DataGridView)dgv;
                            wbDataGridView.CellMouseUp += new DataGridViewCellMouseEventHandler(wbDataGridView_CellMouseUp);
                            wbDataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(wbDataGridView_EditingControlShowing);
                            // wbDataGridView.CellClick += new DataGridViewCellEventHandler(wbDataGridView_CellClick);
                            // wbDataGridView.CellContentClick += new DataGridViewCellEventHandler(wbDataGridView_CellContentClick);
                            wbDataGridView.DataError += new DataGridViewDataErrorEventHandler(wbDataGridView_DataError);
                            //wbDataGridView.CellValueChanged += new DataGridViewCellEventHandler(wbDataGridView_CellValueChanged);
                            ((DataGridView)dgv).CellValueChanged += new DataGridViewCellEventHandler(wbDataGridView_CellValueChanged);
                            ((DataGridView)dgv).CellClick += new DataGridViewCellEventHandler(wbDataGridView_CellClick);
                            ((DataGridView)dgv).CellBeginEdit += new DataGridViewCellCancelEventHandler(wbDataGridView_CellBeginEdit);
                            ((DataGridView)dgv).CellEndEdit += new DataGridViewCellEventHandler(wbDataGridView_CellEndEdit);

                        }
                    }

                }
            }




            Cursor.Current = Cursors.Hand;
        }

        private void wbDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!wbDataGridView.Focused) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "WB");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot update");
                return;

            }
            if (mcData.IsJobLockExist("WB", jobNo, "wbDataGridView_CellEndEdit", loggedInUser)) { return; }
            string result = mcData.CreateJobLock("WB", jobNo, "wbDataGridView_CellEndEdit");
            return;
        }

        private void wbDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(!wbDataGridView.Focused) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "WB");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot update");
                return;

            }
            if (mcData.IsJobLockExist("WB", jobNo, "wbDataGridView_CellBeginEdit", loggedInUser)) { return; }
            string result = mcData.CreateJobLock("WB", jobNo, "wbDataGridView_CellBeginEdit");
            return;
        }



        private void wbDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!wbDataGridView.Focused) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "WB");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot update");
                return;

            }
            if (mcData.IsJobLockExist("WB", jobNo, "wbDataGridView_CellClick", loggedInUser)) { return; }
            string result = mcData.CreateJobLock("WB", jobNo, "wbDataGridView_CellClick");
            return;
        }



        private void wbDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
             if (!wbDataGridView.Focused) { return; }

            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "WB");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot update");
                return;

            }
            if (mcData.IsJobLockExist("WB", jobNo, "wbDataGridView_CellValueChanged", loggedInUser)) { return; }
            string result = mcData.CreateJobLock("WB", jobNo, "wbDataGridView_CellValueChanged");
            return;
        }

        private void wbDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!wbDataGridView.Focused) { return; }
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (wbDataGridView.CurrentCell.ColumnIndex == 7) //Desired Column
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
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        //private void wbDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (!wbDataGridView.Focused) { return; }
        //    if (e.ColumnIndex != 25) { return; }

        //    DataGridViewCheckBoxCell cell = this.wbDataGridView.CurrentCell as DataGridViewCheckBoxCell;

        //    if (cell != null && !cell.ReadOnly)
        //    {
        //        cell.Value = cell.Value == null || !((bool)cell.Value);
        //        this.wbDataGridView.RefreshEdit();
        //        this.wbDataGridView.NotifyCurrentCellDirty(true);
        //        string jobNo = wbDataGridView[0, e.RowIndex].Value.ToString();
        //        if ((bool)cell.Value == true)
        //        {

        //            DateTime reqDate = Convert.ToDateTime(wbDataGridView[1, e.RowIndex].Value.ToString());
        //            string siteAddr = wbDataGridView[2, e.RowIndex].Value.ToString();
        //            string products = wbDataGridView[5, e.RowIndex].Value.ToString();
        //            int totalM2 = Convert.ToInt16(wbDataGridView[6, e.RowIndex].Value.ToString());
        //            string suppShortname = wbDataGridView[10, e.RowIndex].Value.ToString();
        //            string stairsSupplier = wbDataGridView[12, e.RowIndex].Value.ToString();
        //            MessageBox.Show(String.Format("RAM for Job No.{0} confirmed as sent !!",jobNo));
        //            //create new record in JobPurchaseNo with RAMS sent details
        //            if (mcData.IsJobPoExists(jobNo))
        //            {
        //                int num = mcData.DeleteJobPO(jobNo);
        //            }
        //            string err = mcData.CreateJobPO(jobNo, reqDate, siteAddr, products, totalM2, suppShortname, stairsSupplier);
        //            return;
        //        }
        //        else
        //        {
        //            MessageBox.Show(String.Format("RAM for Job No.{0} confirmed as NOT sent !!", jobNo));
        //            //delete record in JobPurchaseNo with RAMS sent details
        //            int num = mcData.DeleteJobPO(jobNo);
        //            return;
        //        }
        //    }
        //}


        //private void wbDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (!wbDataGridView.Focused) { return; }
        //    if (e.ColumnIndex != 25) { return; }
        //    if (e.ColumnIndex == 25)
        //    {
        //        this.wbDataGridView.RefreshEdit();
        //    }
        //}



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
            DateTime wbDate = Convert.ToDateTime(tp.Text);
            Color fontColor = mcData.IsCurrentWeek(wbDate) ? Color.Blue : Color.Black;
            g.DrawString(tp.Text, weeksTabControl.Font, new SolidBrush(fontColor), headerRect, sf);
        }

        private void wbDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.RowIndex != -1) // added this and it now works
            {
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.wbDataGridView = (DataGridView)sender;
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
            {
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.wbDataGridView = (DataGridView)sender;
                return;
            }
            //tabPageDate = this.weeksTabControl.TabPages[
        }

        private void wbDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            return;
        }

        private void BuildWhiteboardDGV(DataGridView myDGV, DateTime wcDate)
        {
            try
            {
                wbDataGridView = myDGV;

                wbDataGridView.Columns.Clear();
                //0
                DataGridViewTextBoxColumn quoteNoColumn = new DataGridViewTextBoxColumn();//0
                quoteNoColumn.DataPropertyName = "JobNo";
                quoteNoColumn.HeaderText = "Job No.";
                //quoteNoColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                quoteNoColumn.Width = 90;
                quoteNoColumn.ReadOnly = true;
                quoteNoColumn.Frozen = true;
                quoteNoColumn.DefaultCellStyle.ForeColor = Color.Black;
                quoteNoColumn.DefaultCellStyle.BackColor = Color.Cyan;
                wbDataGridView.Columns.Add(quoteNoColumn);
                //1
                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();//1
                dateColumn.DataPropertyName = "date";
                dateColumn.HeaderText = "Required Date";
                //dateColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dateColumn.Width = 120;
                dateColumn.ReadOnly = true;
                dateColumn.Frozen = true;
                dateColumn.DefaultCellStyle.BackColor = Color.Yellow;
                dateColumn.DefaultCellStyle.ForeColor = Color.Blue;
                wbDataGridView.Columns.Add(dateColumn);
                //2
                DataGridViewTextBoxColumn custColumn = new DataGridViewTextBoxColumn();//2
                custColumn.DataPropertyName = "CustName";
                custColumn.HeaderText = "Customer";
                //custColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                custColumn.Width = 150;
                custColumn.Frozen = true;
                custColumn.ReadOnly = true;
                //custColumn.DefaultCellStyle.BackColor = Color.LightGray;
                //custColumn.DefaultCellStyle.ForeColor = Color.Black;
                //custColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                wbDataGridView.Columns.Add(custColumn);
                //3
                DataGridViewTextBoxColumn siteColumn = new DataGridViewTextBoxColumn();//3
                siteColumn.DataPropertyName = "site";
                siteColumn.HeaderText = "Site";
                siteColumn.Width = 250;
                siteColumn.Frozen = true;
                siteColumn.ReadOnly = true;
                siteColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                wbDataGridView.Columns.Add(siteColumn);
                //4
                DataGridViewTextBoxColumn supplyTypeColumn = new DataGridViewTextBoxColumn();//4
                supplyTypeColumn.DataPropertyName = "type";
                supplyTypeColumn.HeaderText = "TYPE";
                supplyTypeColumn.Width = 50;
                supplyTypeColumn.ReadOnly = true;
                wbDataGridView.Columns.Add(supplyTypeColumn);
                //5
                DataGridViewTextBoxColumn productsColumn = new DataGridViewTextBoxColumn();//5
                productsColumn.DataPropertyName = "products";
                productsColumn.HeaderText = "Products (Right Clk)";
                productsColumn.Width = 60;
                productsColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(productsColumn);
                //6
                DataGridViewTextBoxColumn totalM2Column = new DataGridViewTextBoxColumn();//6
                totalM2Column.DataPropertyName = "totalM2";
                totalM2Column.HeaderText = "Total M²";
                totalM2Column.Width = 60;
                totalM2Column.ReadOnly = true;
                wbDataGridView.Columns.Add(totalM2Column);
                //7
                DataGridViewTextBoxColumn fixDaysColumn = new DataGridViewTextBoxColumn();//7
                fixDaysColumn.DataPropertyName = "fixDays";
                fixDaysColumn.HeaderText = "Num Fixing Days";
                fixDaysColumn.Width = 70;
                fixDaysColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(fixDaysColumn);
                //8
                DataGridViewTextBoxColumn salesPriceColumn = new DataGridViewTextBoxColumn();//8
                salesPriceColumn.DataPropertyName = "salesPrice";
                salesPriceColumn.HeaderText = "Sales Price";
                salesPriceColumn.Width = 70;
                salesPriceColumn.ReadOnly = true;
                salesPriceColumn.DefaultCellStyle.BackColor = Color.Cyan;
                salesPriceColumn.DefaultCellStyle.ForeColor = Color.Black;
                wbDataGridView.Columns.Add(salesPriceColumn);
                //9
                DataGridViewCheckBoxColumn invoicedColumn = new DataGridViewCheckBoxColumn();//9
                invoicedColumn.DataPropertyName = "invoiced";
                invoicedColumn.ValueType = typeof(bool);
                invoicedColumn.HeaderText = "invoiced";
                invoicedColumn.Width = 70;
                invoicedColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(invoicedColumn);
                //10
                DataGridViewTextBoxColumn productSupplierColumn = new DataGridViewTextBoxColumn();//10
                productSupplierColumn.DataPropertyName = "productSupplier";
                productSupplierColumn.HeaderText = "Product Supplier (Right Click)";
                productSupplierColumn.Width = 70;
                productSupplierColumn.ReadOnly = true;
                wbDataGridView.Columns.Add(productSupplierColumn);
                //11
                DataGridViewCheckBoxColumn stairsColumn = new DataGridViewCheckBoxColumn();//11
                stairsColumn.DataPropertyName = "stairs";
                stairsColumn.ValueType = typeof(bool);
                stairsColumn.HeaderText = "Stairs";
                stairsColumn.Width = 70;
                stairsColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(stairsColumn);
                //12
                DataGridViewTextBoxColumn stairsSupplierColumn = new DataGridViewTextBoxColumn();//12
                stairsSupplierColumn.DataPropertyName = "stairsSupplier";
                stairsSupplierColumn.HeaderText = "Stairs Supplier (Right Click)";
                stairsSupplierColumn.Width = 70;
                stairsSupplierColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(stairsSupplierColumn);
                //13
                DataGridViewTextBoxColumn levelColumn = new DataGridViewTextBoxColumn();//13
                levelColumn.DataPropertyName = "Level";
                levelColumn.HeaderText = "Floor Level";
                levelColumn.Width = 100;
                levelColumn.DefaultCellStyle.ForeColor = Color.Blue;
                levelColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                levelColumn.ReadOnly = true;
                wbDataGridView.Columns.Add(levelColumn);
                //14
                DataGridViewCheckBoxColumn prevWeekColumn = new DataGridViewCheckBoxColumn();//14
                prevWeekColumn.DataPropertyName = "contFromPrevWeek";
                prevWeekColumn.ValueType = typeof(bool);
                prevWeekColumn.HeaderText = "Job Cont Prev Week";
                prevWeekColumn.Width = 70;
                prevWeekColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(prevWeekColumn);
                //15
                DataGridViewCheckBoxColumn cowSentColumn = new DataGridViewCheckBoxColumn();//15
                cowSentColumn.DataPropertyName = "cowSent";
                cowSentColumn.ValueType = typeof(bool);
                cowSentColumn.HeaderText = "COW Sent";
                cowSentColumn.Width = 30;
                cowSentColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(cowSentColumn);
                //16
                DataGridViewTextBoxColumn cowRcvdColumn = new DataGridViewTextBoxColumn();//16
                cowRcvdColumn.DataPropertyName = "cowRcvd";
                cowRcvdColumn.HeaderText = "COW Rcvd Signed";
                cowRcvdColumn.Width = 120;
                cowRcvdColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(cowRcvdColumn);
                //17
                DataGridViewTextBoxColumn monColumn = new DataGridViewTextBoxColumn();//17
                monColumn.Name = "mon";
                monColumn.HeaderText = "MON " + wcDate.Day.ToString() + "/" + wcDate.ToString("MMM");
                monColumn.Width = 80;
                monColumn.DefaultCellStyle.BackColor = Color.Yellow;
                monColumn.DefaultCellStyle.ForeColor = Color.Blue;
                monColumn.ReadOnly = true;
                wbDataGridView.Columns.Add(monColumn);
                //18
                DataGridViewTextBoxColumn tueColumn = new DataGridViewTextBoxColumn();//18
                tueColumn.DataPropertyName = "tue";
                tueColumn.HeaderText = "TUE " + wcDate.AddDays(1).Day.ToString() + "/" + wcDate.AddDays(1).ToString("MMM");
                tueColumn.Width = 80;
                tueColumn.DefaultCellStyle.BackColor = Color.Yellow;
                tueColumn.DefaultCellStyle.ForeColor = Color.Blue;
                tueColumn.ReadOnly = true;
                wbDataGridView.Columns.Add(tueColumn);
                //19
                DataGridViewTextBoxColumn wedColumn = new DataGridViewTextBoxColumn();//19
                wedColumn.DataPropertyName = "wed";
                wedColumn.HeaderText = "WED " + wcDate.AddDays(2).Day.ToString() + "/" + wcDate.AddDays(2).ToString("MMM");
                wedColumn.Width = 80;
                wedColumn.DefaultCellStyle.BackColor = Color.Yellow;
                wedColumn.DefaultCellStyle.ForeColor = Color.Blue;
                wedColumn.ReadOnly = true;
                wbDataGridView.Columns.Add(wedColumn);
                //20
                DataGridViewTextBoxColumn thuColumn = new DataGridViewTextBoxColumn();//20
                thuColumn.DataPropertyName = "thu";
                thuColumn.HeaderText = "THU " + wcDate.AddDays(3).Day.ToString() + "/" + wcDate.AddDays(3).ToString("MMM");
                thuColumn.Width = 80;
                thuColumn.DefaultCellStyle.BackColor = Color.Yellow;
                thuColumn.DefaultCellStyle.ForeColor = Color.Blue;
                thuColumn.ReadOnly = true;
                wbDataGridView.Columns.Add(thuColumn);
                //21
                DataGridViewTextBoxColumn friColumn = new DataGridViewTextBoxColumn();//21
                friColumn.DataPropertyName = "fri";
                friColumn.HeaderText = "FRI " + wcDate.AddDays(4).Day.ToString() + "/" + wcDate.AddDays(4).ToString("MMM");
                friColumn.Width = 80;
                friColumn.DefaultCellStyle.BackColor = Color.Yellow;
                friColumn.DefaultCellStyle.ForeColor = Color.Blue;
                friColumn.ReadOnly = true;
                wbDataGridView.Columns.Add(friColumn);
                //22
                DataGridViewTextBoxColumn satColumn = new DataGridViewTextBoxColumn();//22
                satColumn.DataPropertyName = "sat";
                satColumn.HeaderText = "SAT " + wcDate.AddDays(5).Day.ToString() + "/" + wcDate.AddDays(5).ToString("MMM");
                satColumn.Width = 80;
                satColumn.DefaultCellStyle.ForeColor = Color.Blue;
                satColumn.ReadOnly = true;
                satColumn.DefaultCellStyle.BackColor = Color.Gray;
                wbDataGridView.Columns.Add(satColumn);
                //23
                DataGridViewTextBoxColumn sunColumn = new DataGridViewTextBoxColumn();//23
                sunColumn.DataPropertyName = "sun";
                sunColumn.HeaderText = "SUN " + wcDate.AddDays(6).Day.ToString() + "/" + wcDate.AddDays(6).ToString("MMM");
                sunColumn.Width = 80;
                sunColumn.DefaultCellStyle.ForeColor = Color.Blue;
                sunColumn.ReadOnly = true;
                sunColumn.DefaultCellStyle.BackColor = Color.Gray;
                wbDataGridView.Columns.Add(sunColumn);
                //24
                DataGridViewTextBoxColumn contractsColumn = new DataGridViewTextBoxColumn();//24
                contractsColumn.DataPropertyName = "contracts";
                contractsColumn.HeaderText = "Contracts";
                contractsColumn.Width = 70;
                contractsColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(contractsColumn);
                //25
                DataGridViewCheckBoxColumn ramsColumn = new DataGridViewCheckBoxColumn();//25
                ramsColumn.DataPropertyName = "ramUploaded";
                ramsColumn.ValueType = typeof(bool);
                ramsColumn.HeaderText = "RAMS Uploaded";
                ramsColumn.Width = 70;
                ramsColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(ramsColumn);
                //26
                DataGridViewCheckBoxColumn ramsSentColumn = new DataGridViewCheckBoxColumn();//26
                ramsSentColumn.DataPropertyName = "ramsSent";
                ramsSentColumn.ValueType = typeof(bool);
                ramsSentColumn.HeaderText = "RAMS Sent";
                ramsSentColumn.Width = 70;
                ramsSentColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(ramsSentColumn);
                //27
                DataGridViewCheckBoxColumn ramsCompRtndColumn = new DataGridViewCheckBoxColumn();//27
                ramsCompRtndColumn.DataPropertyName = "ramsRtnd";
                ramsCompRtndColumn.ValueType = typeof(bool);
                ramsCompRtndColumn.HeaderText = "Signed RAMS Rtnd";
                ramsCompRtndColumn.Width = 70;
                ramsCompRtndColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(ramsCompRtndColumn);
                //28
                DataGridViewTextBoxColumn lorryColumn = new DataGridViewTextBoxColumn();//28
                lorryColumn.DataPropertyName = "lorry";
                lorryColumn.HeaderText = "Lorry";
                lorryColumn.Width = 70;
                lorryColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(lorryColumn);
                //29
                DataGridViewTextBoxColumn craneSizeColumn = new DataGridViewTextBoxColumn();//29
                craneSizeColumn.DataPropertyName = "craneSize";
                craneSizeColumn.HeaderText = "Crane Size";
                craneSizeColumn.Width = 70;
                craneSizeColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(craneSizeColumn);
                //30
                DataGridViewTextBoxColumn craneSupplierColumn = new DataGridViewTextBoxColumn();//30
                craneSupplierColumn.DataPropertyName = "craneSupplier";
                craneSupplierColumn.HeaderText = "Crane Supplier";
                craneSupplierColumn.Width = 70;
                craneSupplierColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(craneSupplierColumn);
                //31
                DataGridViewCheckBoxColumn spreaderMatsColumn = new DataGridViewCheckBoxColumn();//31
                spreaderMatsColumn.DataPropertyName = "spreaderMats";
                spreaderMatsColumn.ValueType = typeof(bool);
                spreaderMatsColumn.HeaderText = "Spreader Mats";
                spreaderMatsColumn.Width = 70;
                spreaderMatsColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(spreaderMatsColumn);
                //32
                DataGridViewTextBoxColumn hireContractRcvdColumn = new DataGridViewTextBoxColumn();//32
                hireContractRcvdColumn.DataPropertyName = "hireContractRcvd ";
                hireContractRcvdColumn.HeaderText = "Hire Contract Rcvd";
                hireContractRcvdColumn.Width = 70;
                hireContractRcvdColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(hireContractRcvdColumn);
                //33
                DataGridViewTextBoxColumn fallArrestColumn = new DataGridViewTextBoxColumn();//33
                fallArrestColumn.DataPropertyName = "fallarrest";
                fallArrestColumn.HeaderText = "Fall Arrest";
                fallArrestColumn.Width = 70;
                fallArrestColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(fallArrestColumn);
                //34
                DataGridViewTextBoxColumn gangColumn = new DataGridViewTextBoxColumn();//34
                gangColumn.DataPropertyName = "gang";
                gangColumn.HeaderText = "Gang";
                gangColumn.Width = 70;
                gangColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(gangColumn);
                //35
                DataGridViewCheckBoxColumn onHireColumn = new DataGridViewCheckBoxColumn();//35
                onHireColumn.DataPropertyName = "onHire";
                onHireColumn.ValueType = typeof(bool);
                onHireColumn.HeaderText = "On Hire";
                onHireColumn.Width = 70;
                onHireColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(onHireColumn);
                //36
                DataGridViewCheckBoxColumn extrasColumn = new DataGridViewCheckBoxColumn();//36
                extrasColumn.DataPropertyName = "extras";
                extrasColumn.ValueType = typeof(bool);
                extrasColumn.HeaderText = "Extras";
                extrasColumn.Width = 50;
                extrasColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(extrasColumn);
                //37
                DataGridViewTextBoxColumn concreteColumn = new DataGridViewTextBoxColumn();//37
                concreteColumn.DataPropertyName = "concrete";
                concreteColumn.HeaderText = "Concrete";
                concreteColumn.Width = 70;
                concreteColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(concreteColumn);
                //38
                DataGridViewTextBoxColumn blocksColumn = new DataGridViewTextBoxColumn();//38
                blocksColumn.DataPropertyName = "blocks";
                blocksColumn.HeaderText = "Blocks";
                blocksColumn.Width = 90;
                blocksColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(blocksColumn);
                //39
                DataGridViewCheckBoxColumn drawingsEmailedColumn = new DataGridViewCheckBoxColumn();//39
                drawingsEmailedColumn.DataPropertyName = "dwgEmailed";
                drawingsEmailedColumn.ValueType = typeof(bool);
                drawingsEmailedColumn.HeaderText = "Drwg Emailed";
                drawingsEmailedColumn.Width = 50;
                // drawingsEmailedColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(drawingsEmailedColumn);
                //40
                DataGridViewTextBoxColumn draughtsmanColumn = new DataGridViewTextBoxColumn();//40
                draughtsmanColumn.DataPropertyName = "Draftsman";
                draughtsmanColumn.HeaderText = "Draftsman";
                draughtsmanColumn.Width = 90;
                draughtsmanColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(draughtsmanColumn);
                //41
                DataGridViewTextBoxColumn salesmanColumn = new DataGridViewTextBoxColumn();//41
                salesmanColumn.DataPropertyName = "salesman";
                salesmanColumn.HeaderText = "Salesman";
                salesmanColumn.Width = 90;
                salesmanColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(salesmanColumn);
                //42
                DataGridViewTextBoxColumn commentColumn = new DataGridViewTextBoxColumn();//42
                commentColumn.DataPropertyName = "commentColumn";
                commentColumn.HeaderText = "Last Comment";
                commentColumn.Width = 300;
                commentColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(commentColumn);
                //43
                DataGridViewTextBoxColumn modDateColumn = new DataGridViewTextBoxColumn();//43
                modDateColumn.DataPropertyName = "createdDateColumn";
                modDateColumn.HeaderText = "Created";
                modDateColumn.Width = 100;
                modDateColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(modDateColumn);
                //44
                DataGridViewTextBoxColumn modByColumn = new DataGridViewTextBoxColumn();//44
                modByColumn.DataPropertyName = "repColumn";
                modByColumn.HeaderText = "Rep";
                modByColumn.Width = 30;
                modByColumn.ReadOnly = false;
                wbDataGridView.Columns.Add(modByColumn);

                wbDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                wbDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
                wbDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                wbDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                wbDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                wbDataGridView.Dock = DockStyle.Fill;
                wbDataGridView.AllowUserToAddRows = false;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("BuildWhiteboardDGV() ERROR - " + ex.Message.ToString());
                string audit = mcData.CreateErrorAudit("WhiteboardForm.cs", "BuildWhiteboardDGV(...)", ex.Message);
                return;
            }

        }

        private void PopulateWhiteboardDGV(DataGridView myDGV, DateTime myStartDate, DateTime myEndDate)
        {
            int row = 0;

            int rgb1 = 255;
            int rgb2 = 255;
            int rgb3 = 255;
            int srgb1 = 255;
            int srgb2 = 255;
            int srgb3 = 255;
            string suppShortname = "";
            string stairsSupplier = "";
            string monFlag = "";
            string tueFlag = "";
            string wedFlag = "";
            string thuFlag = "";
            string friFlag = "";
            string satFlag = "";
            string sunFlag = "";
            string dateStr = "";
            string ramsFlag = "";
            string custName = "";
            string isInvoiced = "";
            string jobNo = "";
            string dateCreated = "";

            this.Cursor = Cursors.WaitCursor;
            DataTable dt = mcData.GetWhiteboardByDateRangeDT(myStartDate, myEndDate);
            if (dt == null)
            {
                //MessageBox.Show(String.Format("Cannot find whiteboard jobs between [{0}] and [{1}]", myStartDate.ToShortDateString(), myEndDate.ToShortDateString()));
                return;
            }
            if (dt.Rows.Count < 1)
            {
                //MessageBox.Show(String.Format("Cannot find whiteboard jobs between [{0}] and [{1}]",myStartDate.ToShortDateString(),myEndDate.ToShortDateString()));
                return;
            }

            myDGV.Rows.Clear();
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            foreach (DataRow dr in dt.Rows)
            {
                
                jobNo = dr["jobNo"].ToString();
                if (!mcData.IsValidWhiteboardJob(jobNo)) { continue; }


                isInvoiced = dr["isInvoiced"].ToString();
                ramsFlag = dr["ramsFlag"].ToString();
                suppShortname = dr["suppShortname"].ToString();
                stairsSupplier = dr["stairsSupplier"].ToString();
                mcData.GetSupplierColourByShortname(suppShortname, out rgb1, out rgb2, out rgb3);
                mcData.GetSupplierColourByShortname(stairsSupplier, out srgb1, out srgb2, out srgb3);
                custName = mcData.GetCustName(dr["custCode"].ToString());
                dateCreated = mcData.GetJobCreatedDate(jobNo).ToString("dd/MMM/yyyy hh:mm tt");

                dateStr = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().ToUpper().Substring(0, 3);
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
                drow.Cells[0].Style.BackColor = ramsFlag == "Y" ? Color.LightGreen : Color.Cyan;
                drow.Cells[1].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                drow.Cells[2].Value = custName;
                drow.Cells[3].Value = dr["siteAddress"].ToString();
                drow.Cells[4].Value = dr["supplyType"].ToString();
                drow.Cells[5].Value = dr["products"].ToString();
                drow.Cells[6].Value = Convert.ToInt32(dr["totalM2"].ToString());
                drow.Cells[7].Value = Convert.ToInt32(dr["fixingDaysAllowance"].ToString());
                drow.Cells[8].Value = Convert.ToDecimal(dr["salesPrice"].ToString());
                drow.Cells[9].Value = dr["isInvoiced"].ToString().Contains("Y") ? true : false;
                drow.Cells[10].Value = dr["suppShortname"].ToString();
                drow.Cells[10].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                drow.Cells[11].Value = dr["stairsIncl"].ToString().Contains("Y") ? true : false;
                drow.Cells[12].Value = dr["stairsSupplier"].ToString();
                drow.Cells[12].Style.BackColor = Color.FromArgb(srgb1, srgb2, srgb3);
                drow.Cells[13].Value = dr["floorLevel"].ToString();
                drow.Cells[14].Value = dr["continuedFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[15].Value = dr["cowSentFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[16].Value = dr["cowReceived"].ToString();
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
                drow.Cells[24].Value = dr["contracts"].ToString();
                drow.Cells[25].Value = dr["ramsFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[26].Value = dr["ramsSignedReturnedFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[27].Value = dr["ramsCompleteReturnedFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[28].Value = dr["lorry"].ToString();
                drow.Cells[29].Value = dr["craneSize"].ToString();
                drow.Cells[30].Value = dr["craneSupplier"].ToString();
                drow.Cells[31].Value = dr["spreaderMatFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[32].Value = dr["hireContractRcvd"].ToString();
                drow.Cells[33].Value = dr["fallArrest"].ToString();
                drow.Cells[34].Value = dr["fixingGang"].ToString();
                drow.Cells[35].Value = dr["onHireFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[36].Value = dr["extrasFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[37].Value = dr["concrete"].ToString();
                drow.Cells[38].Value = dr["blocks"].ToString();
                drow.Cells[39].Value = dr["drawingsEmailedFlag"].ToString().Contains("Y") ? true : false;
                drow.Cells[40].Value = dr["draughtsman"].ToString();
                drow.Cells[41].Value = dr["salesman"].ToString();
                drow.Cells[42].Value = dr["lastComment"].ToString();
                drow.Cells[43].Value = dateCreated;// Convert.ToDateTime(dr["modifiedDate"].ToString()).ToLongDateString();
                drow.Cells[44].Value = dr["jobCreatedBy"].ToString();
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
                }
                else if(mcData.IsJobCustomerOnStop(custCode))
                {
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                }
                if (mcData.IsJobLockExistByOtherUser("WB", dgvRow.Cells[0].Value.ToString(),loggedInUser))
                {
                    dgvRow.Frozen = true;
                    dgvRow.DefaultCellStyle.ForeColor = Color.MediumPurple;
                }
            }
        }

        private void PopulateWhiteboardDGV_BACKUP(DataGridView myDGV, DateTime myStartDate, DateTime myEndDate)
        {
            int row = 0;

            int rgb1 = 255;
            int rgb2 = 255;
            int rgb3 = 255;
            int srgb1 = 255;
            int srgb2 = 255;
            int srgb3 = 255;
            string suppShortname = "";
            string stairsSupplier = "";
            string monFlag = "";
            string tueFlag = "";
            string wedFlag = "";
            string thuFlag = "";
            string friFlag = "";
            string satFlag = "";
            string sunFlag = "";
            string dateStr = "";
            string ramsFlag = "";
            string custName = "";
            string isInvoiced = "";
            string jobNo = "";
            string dateCreated = "";


            DataTable dt = mcData.GetWhiteboardByDateRangeDT(myStartDate, myEndDate);
            if (dt == null)
            {
                //MessageBox.Show(String.Format("Cannot find whiteboard jobs between [{0}] and [{1}]", myStartDate.ToShortDateString(), myEndDate.ToShortDateString()));
                return;
            }
            if (dt.Rows.Count < 1)
            {
                //MessageBox.Show(String.Format("Cannot find whiteboard jobs between [{0}] and [{1}]",myStartDate.ToShortDateString(),myEndDate.ToShortDateString()));
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {

                jobNo = dr["jobNo"].ToString();
                if (!mcData.IsValidWhiteboardJob(jobNo)) { continue; }


                isInvoiced = dr["isInvoiced"].ToString();
                ramsFlag = dr["ramsFlag"].ToString();
                suppShortname = dr["suppShortname"].ToString();
                stairsSupplier = dr["stairsSupplier"].ToString();
                mcData.GetSupplierColourByShortname(suppShortname, out rgb1, out rgb2, out rgb3);
                mcData.GetSupplierColourByShortname(stairsSupplier, out srgb1, out srgb2, out srgb3);
                custName = mcData.GetCustName(dr["custCode"].ToString());
                dateCreated = mcData.GetJobCreatedDate(jobNo).ToString("dd/MMM/yyyy hh:mm tt");

                dateStr = Convert.ToDateTime(dr["requiredDate"].ToString()).DayOfWeek.ToString().ToUpper().Substring(0, 3);
                monFlag = dateStr.Substring(0, 3).ToUpper() == "MON" ? "Y" : String.Empty;
                tueFlag = dateStr.Substring(0, 3).ToUpper() == "TUE" ? "Y" : String.Empty;
                wedFlag = dateStr.Substring(0, 3).ToUpper() == "WED" ? "Y" : String.Empty;
                thuFlag = dateStr.Substring(0, 3).ToUpper() == "THU" ? "Y" : String.Empty;
                friFlag = dateStr.Substring(0, 3).ToUpper() == "FRI" ? "Y" : String.Empty;
                satFlag = dateStr.Substring(0, 3).ToUpper() == "SAT" ? "Y" : String.Empty;
                sunFlag = dateStr.Substring(0, 3).ToUpper() == "SUN" ? "Y" : String.Empty;
                myDGV.Rows.Add();
                myDGV[0, row].Value = dr["jobNo"].ToString();
                myDGV[0, row].Style.BackColor = ramsFlag == "Y" ? Color.LightGreen : Color.Cyan;
                myDGV[1, row].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                myDGV[2, row].Value = custName;
                myDGV[3, row].Value = dr["siteAddress"].ToString();
                myDGV[4, row].Value = dr["supplyType"].ToString();
                myDGV[5, row].Value = dr["products"].ToString();
                myDGV[6, row].Value = Convert.ToInt32(dr["totalM2"].ToString());
                myDGV[7, row].Value = Convert.ToInt32(dr["fixingDaysAllowance"].ToString());
                myDGV[8, row].Value = Convert.ToDecimal(dr["salesPrice"].ToString());
                myDGV[9, row].Value = dr["isInvoiced"].ToString().Contains("Y") ? true : false;
                myDGV[10, row].Value = dr["suppShortname"].ToString();
                myDGV[10, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                myDGV[11, row].Value = dr["stairsIncl"].ToString().Contains("Y") ? true : false;
                myDGV[12, row].Value = dr["stairsSupplier"].ToString();
                myDGV[12, row].Style.BackColor = Color.FromArgb(srgb1, srgb2, srgb3);
                myDGV[13, row].Value = dr["floorLevel"].ToString();
                myDGV[14, row].Value = dr["continuedFlag"].ToString().Contains("Y") ? true : false;
                myDGV[15, row].Value = dr["cowSentFlag"].ToString().Contains("Y") ? true : false;
                myDGV[16, row].Value = dr["cowReceived"].ToString();
                myDGV[17, row].Value = dr["wcMonday"].ToString();
                myDGV[18, row].Value = dr["wcTuesday"].ToString();
                myDGV[19, row].Value = dr["wcWednesday"].ToString();
                myDGV[20, row].Value = dr["wcThursday"].ToString();
                myDGV[21, row].Value = dr["wcFriday"].ToString();
                myDGV[22, row].Value = dr["wcSaturday"].ToString();
                myDGV[23, row].Value = dr["wcSunday"].ToString();
                myDGV[17, row].Style.BackColor = monFlag == "Y" ? Color.Yellow : Color.White;
                myDGV[18, row].Style.BackColor = tueFlag == "Y" ? Color.Yellow : Color.White;
                myDGV[19, row].Style.BackColor = wedFlag == "Y" ? Color.Yellow : Color.White;
                myDGV[20, row].Style.BackColor = thuFlag == "Y" ? Color.Yellow : Color.White;
                myDGV[21, row].Style.BackColor = friFlag == "Y" ? Color.Yellow : Color.White;
                myDGV[22, row].Style.BackColor = satFlag == "Y" ? Color.Yellow : Color.White;
                myDGV[23, row].Style.BackColor = sunFlag == "Y" ? Color.Yellow : Color.White;
                myDGV[24, row].Value = dr["contracts"].ToString();
                myDGV[25, row].Value = dr["ramsFlag"].ToString().Contains("Y") ? true : false;
                myDGV[26, row].Value = dr["ramsSignedReturnedFlag"].ToString().Contains("Y") ? true : false;
                myDGV[27, row].Value = dr["ramsCompleteReturnedFlag"].ToString().Contains("Y") ? true : false;
                myDGV[28, row].Value = dr["lorry"].ToString();
                myDGV[29, row].Value = dr["craneSize"].ToString();
                myDGV[30, row].Value = dr["craneSupplier"].ToString();
                myDGV[31, row].Value = dr["spreaderMatFlag"].ToString().Contains("Y") ? true : false;
                myDGV[32, row].Value = dr["hireContractRcvd"].ToString();
                myDGV[33, row].Value = dr["fallArrest"].ToString();
                myDGV[34, row].Value = dr["fixingGang"].ToString();
                myDGV[35, row].Value = dr["onHireFlag"].ToString().Contains("Y") ? true : false;
                myDGV[36, row].Value = dr["extrasFlag"].ToString().Contains("Y") ? true : false;
                myDGV[37, row].Value = dr["concrete"].ToString();
                myDGV[38, row].Value = dr["blocks"].ToString();
                myDGV[39, row].Value = dr["drawingsEmailedFlag"].ToString().Contains("Y") ? true : false;
                myDGV[40, row].Value = dr["draughtsman"].ToString();
                myDGV[41, row].Value = dr["salesman"].ToString();
                myDGV[42, row].Value = dr["lastComment"].ToString();
                myDGV[43, row].Value = dateCreated;// Convert.ToDateTime(dr["modifiedDate"].ToString()).ToLongDateString();
                myDGV[44, row++].Value = dr["jobCreatedBy"].ToString();

            }

            foreach (DataGridViewRow dgvRow in myDGV.Rows)
            {

                string unsuffixedJobNo = dgvRow.Cells[0].Value.ToString().Substring(0, 8);
                string custCode = mcData.GetCustomerCodeByJobNo(unsuffixedJobNo);
                if (mcData.IsJobCompleted(unsuffixedJobNo))
                {
                    dgvRow.DefaultCellStyle.ForeColor = Color.Gray;
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

        private void WhiteboardForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Whiteboard spanning period between {0} and {1}", wbStartDate.ToString("dd/MMM/yyyy"), wbEndDate.ToString("dd/MMM/yyyy"));
            BuildTabs();
            FillSuppliersStatusStrip();

            if (!String.IsNullOrWhiteSpace(selectedJob) && mcData.IsWhiteboardJobExists(selectedJob))
            {
                if (wbDataGridView.Rows.Count < 1) { return; }
                for (int i = 0; i < wbDataGridView.Rows.Count; i++)
                {
                    if (wbDataGridView.Rows[i].Cells[0].Value.ToString().Equals(selectedJob))
                    {
                        wbDataGridView.CurrentCell = wbDataGridView.Rows[i].Cells[0];
                        break;
                        Cursor.Current = Cursors.Hand;
                        return;
                    }
                }
            }
        }

        //private void LoadFixersLegendListView()
        //{
        //    DataTable fxDT = mcData.GetFixersDT();
        //    int seq = 0;
        //    int rgb1 = 0;
        //    int rgb2 = 0;
        //    int rgb3 = 0;
        //    foreach (DataRow dr in fxDT.Rows)
        //    {
        //        rgb1 = Convert.ToInt16(dr["wbRGB_1"].ToString());
        //        rgb2 = Convert.ToInt16(dr["wbRGB_2"].ToString());
        //        rgb3 = Convert.ToInt16(dr["wbRGB_3"].ToString());
        //        listView1.Items.Add(dr["wbFixingGang"].ToString());
        //        listView1.Items[seq].BackColor = Color.FromArgb(rgb1, rgb2, rgb3);

        //        listView1.View = View.Tile;
        //        seq++;
        //    }
        //}

        private void FillSuppliersStatusStrip()
        {
            statusStrip1.Items.Add("SUPPLIERS :");
            DataTable dt = mcData.GetAllSuppliers();
            int i = 0;
            int rgb1, rgb2, rgb3 = 0;
            string shortname = "";
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                shortname = dr["shortname"].ToString();
                mcData.GetSupplierColourByShortname(shortname, out rgb1, out rgb2, out rgb3);
                statusStrip1.Items.Add(shortname);
                statusStrip1.Items[i].BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
            }
        }

        //private void closeButton_Click(object sender, EventArgs e)
        //{

        //    string loggedInUser = mcData.GetLoggedInUser();
        //    string databaseName = mcData.GetDatabaseName().ToUpper();
        //    string message = $"Given your current logged in details : {Environment.NewLine} {Environment.NewLine} User ID : {loggedInUser} {Environment.NewLine} Database Name : {databaseName} ";
        //    string fullMessage = $"{message} {Environment.NewLine} {Environment.NewLine} You are NOT permitted to update a LIVE database";
        //    // string serverName = mcData.GetServerName().ToUpper();
        //    if (loggedInUser == "aa" && !databaseName.Contains("TEST"))
        //    {
        //        MessageBox.Show(fullMessage, "WARNING");
        //        return;
        //    }
        //    SaveAllWhiteboardDGVsToDB();
        //    //Here I need to check that all invoiced/completed non suffixed jobs also have there SUFFIXED invoiced and completed
        //    mcData.SyncCompletedNonSuffixedJobsWithTheirExtendedJobs("Y");
        //    mcData.SyncCompletedNonSuffixedJobsWithTheirExtendedJobs("N");
        //    if (MessageBox.Show("Do you wish to exit the Whiteboard ?", "Whiteboard", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        //    {
        //        int num = mcData.EmptyCurrentInvoicedWhiteboardJobList();
        //        this.Dispose();
        //        this.Close();
        //    }
        //}

        //private void selectFixerMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (this.colIndex >= 15 && this.colIndex <= 21)
        //        {
        //            if (wbDataGridView[this.colIndex,this.rowIndex].Style.BackColor != Color.White)
        //            {
        //                LoadBuildFixersCombo(true);
        //                toolStripFixersComboBox.ComboBox.Refresh();
        //            }

        //        }
        //        toolStripFixersComboBox.ComboBox.Text = String.Empty;
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //         MessageBox.Show("selectFixerMenuItem_Click() ERROR - " + ex.Message + " --> " + ex.InnerException.ToString());
        //        return;
        //    }
        //}

        //private void LoadBuildFixersCombo(bool dailyFlag)
        //{
        //    try
        //    {
        //        System.Data.DataTable fixingDT = mcData.GetFixersDT();
        //        toolStripFixersComboBox.Items.Clear();
        //        foreach (DataRow dr in fixingDT.Rows)
        //        {
        //            toolStripFixersComboBox.Items.Add(dr["wbFixingGang"].ToString());
        //        }
        //        toolStripFixersComboBox.ComboBox.DisplayMember = fixingDT.Columns["wbFixingGang"].ToString();
        //        toolStripFixersComboBox.ComboBox.ValueMember = fixingDT.Columns["wbFixingCode"].ToString();
        //        toolStripFixersComboBox.ComboBox.Refresh();
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("LoadBuildFixersCombo() ERROR - " + ex.Message);
        //        return;
        //    }
        //}

        //private void toolStripFixersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!toolStripFixersComboBox.Focused)
        //    {
        //        return;
        //    }
        //    int rgb1 = 0;
        //    int rgb2 = 0;
        //    int rgb3 = 0;
        //    string gang = toolStripFixersComboBox.SelectedItem.ToString();
        //    mcData.GetFixerRGB(gang, out rgb1, out rgb2, out rgb3);
        //    if (this.colIndex >= 15 && this.colIndex <= 21)
        //    {
        //        if (wbDataGridView[this.colIndex, this.rowIndex].Style.BackColor != Color.White)
        //        {
        //            wbDataGridView[this.colIndex, this.rowIndex].Value = gang;
        //          //  wbDataGridView[this.colIndex, this.rowIndex].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
        //        }

        //    }

        //    toolStripFixersComboBox.ComboBox.Text = String.Empty;
        //    return;

        //}

        private void AddContextMenu()
        {


            foreach (DataGridViewColumn column in wbDataGridView.Columns)
            {
                if (column.Index >= 17 && column.Index <= 23)
                {
                    column.ContextMenuStrip = wbDailyContextMenuStrip1;

                }

                if (column.Index == 0)
                {
                    column.ContextMenuStrip = wbJobContextMenuStrip;

                }

                if (column.Index == 5)
                {
                    column.ContextMenuStrip = productContextMenuStrip;

                }

                if (column.Index == 10 || column.Index == 12)
                {
                    column.ContextMenuStrip = supplierContextMenuStrip;

                }


            }
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
                    column.ContextMenuStrip = wbJobContextMenuStrip;

                }

                if (column.Index == 5)
                {
                    column.ContextMenuStrip = productContextMenuStrip;

                }

                if (column.Index == 10 || column.Index == 12)
                {
                    column.ContextMenuStrip = supplierContextMenuStrip;

                }
            }
        }

        private void SaveAllWhiteboardDGVsToDB()
        {
            try
            {
                int totalUpdated = 0;
                this.Cursor = Cursors.WaitCursor;
                foreach (Control thisControl in weeksTabControl.Controls)
                {
                    //   logger.LogLine(String.Format("SaveAllWhiteboardDGVsToDB() --> Tab Name : {0} / {1}", thisControl.Name.ToString(), thisControl.Text));
                    if (thisControl.GetType() == typeof(TabPage))
                    {
                        foreach (Control dgv in thisControl.Controls)
                        {
                            if (dgv.GetType() == typeof(DataGridView))
                            {
                                // logger.LogLine(String.Format("SaveAllWhiteboardDGVsToDB() --> Is DGV therefore --> Call WhiteBoardDGVToDB({0})", thisControl.Text));
                                totalUpdated += WhiteBoardDGVToDB(thisControl.Text, dgv);
                            }
                        }

                    }
                }
                MessageBox.Show(String.Format("{0} in-progress WHITEBOARD jobs successfully updated", totalUpdated));
                this.Cursor = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("SaveAllWhiteboardDGVsToDB() ERROR - {0}", ex.Message + Environment.NewLine + ex.TargetSite + Environment.NewLine + ex.InnerException));
                string audit = mcData.CreateErrorAudit("WhiteboardForm.cs", "SaveAllWhiteboardDGVsToDB()", ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }

        }

        private void GetDayColourRGB(DataGridViewRow row, int rowNo, out int rgb1, out int rgb2, out int rgb3, out int rgbColNo)
        {
            rgb1 = 0;
            rgb2 = 0;
            rgb3 = 0;
            rgbColNo = 0;

            System.Drawing.Color color;

            if (row.Cells[15].Style.BackColor != Color.White)
            {
                color = row.Cells[15].Style.BackColor;
                rgb1 = Convert.ToInt16(color.R.ToString());
                rgb2 = Convert.ToInt16(color.G.ToString());
                rgb3 = Convert.ToInt16(color.B.ToString());
                rgbColNo = 15;
                return;
            }
            if (row.Cells[16].Style.BackColor != Color.White)
            {
                color = row.Cells[16].Style.BackColor;
                rgb1 = Convert.ToInt16(color.R.ToString());
                rgb2 = Convert.ToInt16(color.G.ToString());
                rgb3 = Convert.ToInt16(color.B.ToString());
                rgbColNo = 16;
                return;
            }
            if (row.Cells[17].Style.BackColor != Color.White)
            {
                color = row.Cells[17].Style.BackColor;
                rgb1 = Convert.ToInt16(color.R.ToString());
                rgb2 = Convert.ToInt16(color.G.ToString());
                rgb3 = Convert.ToInt16(color.B.ToString());
                rgbColNo = 17;
                return;
            }
            if (row.Cells[18].Style.BackColor != Color.White)
            {
                color = row.Cells[18].Style.BackColor;
                rgb1 = Convert.ToInt16(color.R.ToString());
                rgb2 = Convert.ToInt16(color.G.ToString());
                rgb3 = Convert.ToInt16(color.B.ToString());
                rgbColNo = 18;
                return;
            }
            if (row.Cells[19].Style.BackColor != Color.White)
            {
                color = row.Cells[19].Style.BackColor;
                rgb1 = Convert.ToInt16(color.R.ToString());
                rgb2 = Convert.ToInt16(color.G.ToString());
                rgb3 = Convert.ToInt16(color.B.ToString());
                rgbColNo = 19;
                return;
            }
            if (row.Cells[20].Style.BackColor != Color.White)
            {
                color = row.Cells[20].Style.BackColor;
                rgb1 = Convert.ToInt16(color.R.ToString());
                rgb2 = Convert.ToInt16(color.G.ToString());
                rgb3 = Convert.ToInt16(color.B.ToString());
                rgbColNo = 20;
                return;
            }
            if (row.Cells[21].Style.BackColor != Color.White)
            {
                color = row.Cells[21].Style.BackColor;
                rgb1 = Convert.ToInt16(color.R.ToString());
                rgb2 = Convert.ToInt16(color.G.ToString());
                rgb3 = Convert.ToInt16(color.B.ToString());
                rgbColNo = 21;
                return;
            }

            return;
        }

        private Color GetDayColor(DataRow row, DateTime date, int rgb1, int rgb2, int rgb3)
        {

            if (date.DayOfWeek.ToString().ToUpper().Contains("MON"))
            {
                if (row["wcMonday"].ToString().Length > 0)
                {
                    return Color.FromArgb(rgb1, rgb2, rgb3);
                }
                else
                {
                    return Color.Yellow;
                }
            }
            if (date.DayOfWeek.ToString().ToUpper().Contains("TUE"))
            {
                if (row["wcTuesday"].ToString().Length > 0)
                {
                    return Color.FromArgb(rgb1, rgb2, rgb3);
                }
                else
                {
                    return Color.Yellow;
                }
            }
            if (date.DayOfWeek.ToString().ToUpper().Contains("WED"))
            {
                if (row["wcWednesday"].ToString().Length > 0)
                {
                    return Color.FromArgb(rgb1, rgb2, rgb3);
                }
                else
                {
                    return Color.Yellow;
                }
            }
            if (date.DayOfWeek.ToString().ToUpper().Contains("THU"))
            {
                if (row["wcThursday"].ToString().Length > 0)
                {
                    return Color.FromArgb(rgb1, rgb2, rgb3);
                }
                else
                {
                    return Color.Yellow;
                }
            }
            if (date.DayOfWeek.ToString().ToUpper().Contains("FRI"))
            {
                if (row["wcFriday"].ToString().Length > 0)
                {
                    return Color.FromArgb(rgb1, rgb2, rgb3);
                }
                else
                {
                    return Color.Yellow;
                }
            }
            if (date.DayOfWeek.ToString().ToUpper().Contains("SAT"))
            {
                if (row["wcSaturday"].ToString().Length > 0)
                {
                    return Color.FromArgb(rgb1, rgb2, rgb3);
                }
                else
                {
                    return Color.Yellow;
                }
            }
            if (date.DayOfWeek.ToString().ToUpper().Contains("SUN"))
            {
                if (row["wcSunday"].ToString().Length > 0)
                {
                    return Color.FromArgb(rgb1, rgb2, rgb3);
                }
                else
                {
                    return Color.Yellow;
                }
            }
            return Color.White;

        }

        private int WhiteBoardDGVToDB(string tabPageName, Control sourceDgv)
        {
            DataGridView dgv = (DataGridView)sourceDgv;
            string jobNo = "";
            DateTime requiredDate;
            string custCode = "";
            string custName = "";
            string siteAddress = "";
            string supplyType = "";
            string sortType = "";
            string products = "";
            int totalM2 = 0;
            int fixingDaysAllowance = 0;
            decimal salesPrice = 0;
            string isInvoiced = "";
            string suppShortname = "";
            string stairsIncl = "";
            string stairsSupplier = "";
            string floorlevel = "";
            string continuedFlag = "";
            string cowSentFlag = "";
            string cowReceived = "";
            string wcMonday = "";
            string wcTuesday = "";
            string wcWednesday = "";
            string wcThursday = "";
            string wcFriday = "";
            string wcSaturday = "";
            string wcSunday = "";
            string contracts = "";
            string ramsFlag = "";
            string ramsSignedReturnedFlag = "";
            string ramsCompleteReturnedFlag = "";
            string lorry = "";
            string craneSize = "";
            string craneSupplier = "";
            string spreaderMatFlag = "";
            string hireContractRcvd = "";
            string fallArrest = "";
            string fixingGang = "";
            string onHireFlag = "";
            string extrasFlag = "";
            string concrete = "";
            string blocks = "";
            string drawingsEmailedFlag = "";
            string draughtsman = "";
            string salesman = "";
            string lastComment = "";
            string modifiedDate = "";
            string modifiedBy = "";
            int rgb1 = 0;
            int rgb2 = 0;
            int rgb3 = 0;
            string jpSuppShortName = "";



            //int rgb1 = 0;
            //int rgb2 = 0;
            //int rgb3 = 0;
            //int rgbColNo = 0;

            int numInProgressJobsUpdated = 0;


            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[0].Value == null || dgv.Rows[i].Cells[0].Value.ToString().Length < 8)
                {
                    //  logger.LogLine(String.Format("WhiteBoardDGVToDB({0}) --> Job No. {1}", tabPageName, jobNo));
                    break;
                }
                jobNo = dgv.Rows[i].Cells[0].Value.ToString();
                jpSuppShortName = mcData.GetJobPlannerSupplier(jobNo);
                //if (jobNo == "30598.04")
                //{
                //    MessageBox.Show("Convert from cell value to field value");
                //    string startHere = "";
                //}
                if (mcData.IsJobCompleted(jobNo)) { continue; }
                //  logger.LogLine(String.Format("WhiteBoardDGVToDB({0}) --> Job No. {1}", tabPageName,jobNo));
                requiredDate = Convert.ToDateTime(dgv.Rows[i].Cells[1].Value.ToString());
                custName = dgv.Rows[i].Cells[2].Value == null ? "" : dgv.Rows[i].Cells[2].Value.ToString();
                custCode = mcData.GetCustCodeByCustName(custName);
                siteAddress = dgv.Rows[i].Cells[3].Value == null ? "" : dgv.Rows[i].Cells[3].Value.ToString();
                supplyType = dgv.Rows[i].Cells[4].Value == null ? "" : dgv.Rows[i].Cells[4].Value.ToString();
                products = dgv.Rows[i].Cells[5].Value == null ? "" : dgv.Rows[i].Cells[5].Value.ToString();
                totalM2 = dgv.Rows[i].Cells[6].Value == null ? 0 : Convert.ToInt16(dgv.Rows[i].Cells[6].Value.ToString());
                fixingDaysAllowance = dgv.Rows[i].Cells[7].Value == null ? 0 : Convert.ToInt16(dgv.Rows[i].Cells[7].Value.ToString());
                salesPrice = dgv.Rows[i].Cells[8].Value == null ? 0 : Convert.ToDecimal(dgv.Rows[i].Cells[8].Value.ToString());
                isInvoiced = (bool)dgv.Rows[i].Cells[9].Value ? "Y" : "N";
                suppShortname = jpSuppShortName;// dgv.Rows[i].Cells[10].Value == null ? "" : dgv.Rows[i].Cells[10].Value.ToString();
                stairsIncl = (bool)dgv.Rows[i].Cells[11].Value ? "Y" : "N";
                stairsSupplier = dgv.Rows[i].Cells[12].Value == null ? "" : dgv.Rows[i].Cells[12].Value.ToString();
                floorlevel = dgv.Rows[i].Cells[13].Value == null ? "" : dgv.Rows[i].Cells[13].Value.ToString();
                continuedFlag = (bool)dgv.Rows[i].Cells[14].Value ? "Y" : "N";
                cowSentFlag = (bool)dgv.Rows[i].Cells[15].Value ? "Y" : "N";
                cowReceived = dgv.Rows[i].Cells[16].Value == null ? "" : dgv.Rows[i].Cells[16].Value.ToString();
                wcMonday = dgv.Rows[i].Cells[17].Value == null ? "" : dgv.Rows[i].Cells[17].Value.ToString();
                wcTuesday = dgv.Rows[i].Cells[18].Value == null ? "" : dgv.Rows[i].Cells[18].Value.ToString();
                wcWednesday = dgv.Rows[i].Cells[19].Value == null ? "" : dgv.Rows[i].Cells[19].Value.ToString();
                wcThursday = dgv.Rows[i].Cells[20].Value == null ? "" : dgv.Rows[i].Cells[20].Value.ToString();
                wcFriday = dgv.Rows[i].Cells[21].Value == null ? "" : dgv.Rows[i].Cells[21].Value.ToString();
                wcSaturday = dgv.Rows[i].Cells[22].Value == null ? "" : dgv.Rows[i].Cells[22].Value.ToString();
                wcSunday = dgv.Rows[i].Cells[23].Value == null ? "" : dgv.Rows[i].Cells[23].Value.ToString();
                contracts = dgv.Rows[i].Cells[24].Value == null ? "" : dgv.Rows[i].Cells[24].Value.ToString();
                ramsFlag = (bool)dgv.Rows[i].Cells[25].Value ? "Y" : "N";
                ramsSignedReturnedFlag = (bool)dgv.Rows[i].Cells[26].Value ? "Y" : "N";
                ramsCompleteReturnedFlag = (bool)dgv.Rows[i].Cells[27].Value ? "Y" : "N";
                lorry = dgv.Rows[i].Cells[28].Value == null ? "" : dgv.Rows[i].Cells[28].Value.ToString();
                craneSize = dgv.Rows[i].Cells[29].Value == null ? "" : dgv.Rows[i].Cells[29].Value.ToString();
                craneSupplier = dgv.Rows[i].Cells[30].Value == null ? "" : dgv.Rows[i].Cells[30].Value.ToString();
                spreaderMatFlag = (bool)dgv.Rows[i].Cells[31].Value ? "Y" : "N";
                hireContractRcvd = dgv.Rows[i].Cells[32].Value == null ? "" : dgv.Rows[i].Cells[32].Value.ToString();
                fallArrest = dgv.Rows[i].Cells[33].Value == null ? "" : dgv.Rows[i].Cells[33].Value.ToString();
                fixingGang = dgv.Rows[i].Cells[34].Value == null ? "" : dgv.Rows[i].Cells[34].Value.ToString();
                onHireFlag = (bool)dgv.Rows[i].Cells[35].Value ? "Y" : "N";
                extrasFlag = (bool)dgv.Rows[i].Cells[36].Value ? "Y" : "N";
                concrete = dgv.Rows[i].Cells[37].Value == null ? "" : dgv.Rows[i].Cells[37].Value.ToString();
                blocks = dgv.Rows[i].Cells[38].Value == null ? "" : dgv.Rows[i].Cells[38].Value.ToString();
                drawingsEmailedFlag = (bool)dgv.Rows[i].Cells[39].Value ? "Y" : "N";
                draughtsman = dgv.Rows[i].Cells[40].Value == null ? "" : dgv.Rows[i].Cells[40].Value.ToString();
                salesman = dgv.Rows[i].Cells[41].Value == null ? "" : dgv.Rows[i].Cells[41].Value.ToString();
                lastComment = dgv.Rows[i].Cells[42].Value == null ? "" : dgv.Rows[i].Cells[42].Value.ToString();
                sortType = supplyType.Substring(1, 1);


                string testline = "Tab = " + tabPageName + Environment.NewLine +
                    "JobNo = " + jobNo + Environment.NewLine +
                    "custCode = " + custCode + Environment.NewLine +
                    "siteAddress = " + siteAddress + Environment.NewLine +
                    "supplyType = " + supplyType + Environment.NewLine +
                    "products = " + products + Environment.NewLine +
                    "totalM2 = " + totalM2 + Environment.NewLine +
                    "fixingDaysAllowance = " + fixingDaysAllowance + Environment.NewLine +
                    "salesPrice = " + salesPrice + Environment.NewLine +
                    "isInvoiced = " + isInvoiced + Environment.NewLine +
                    "suppShortname = " + suppShortname + Environment.NewLine +
                    "stairsIncl = " + stairsIncl + Environment.NewLine +
                    "stairsSupplier = " + stairsSupplier + Environment.NewLine +
                    "floorlevel = " + floorlevel + Environment.NewLine +
                    "continuedFlag = " + continuedFlag + Environment.NewLine +
                    "cowSentFlag = " + cowSentFlag + Environment.NewLine +
                    "cowReceived = " + cowReceived + Environment.NewLine +
                    "wcMonday = " + wcMonday + Environment.NewLine +
                    "wcTuesday = " + wcTuesday + Environment.NewLine +
                    "wcWednesday = " + wcWednesday + Environment.NewLine +
                    "wcThursday = " + wcThursday + Environment.NewLine +
                    "wcFriday = " + wcFriday + Environment.NewLine +
                    "wcSaturday = " + wcSaturday + Environment.NewLine +
                    "wcSunday = " + wcSunday + Environment.NewLine +
                    "contracts = " + contracts + Environment.NewLine +
                    "ramsFlag = " + ramsFlag + Environment.NewLine +
                    "ramsSignedReturnedFlag = " + ramsSignedReturnedFlag + Environment.NewLine +
                    "ramsCompleteReturnedFlag = " + ramsCompleteReturnedFlag + Environment.NewLine +
                    "lorry = " + lorry + Environment.NewLine +
                    "craneSize = " + craneSize + Environment.NewLine +
                    "craneSupplier = " + craneSupplier + Environment.NewLine +
                    "spreaderMatFlag = " + spreaderMatFlag + Environment.NewLine +
                    "hireContractRcvd = " + hireContractRcvd + Environment.NewLine +
                    "fallArrest = " + fallArrest + Environment.NewLine +
                    "fixingGang = " + fixingGang + Environment.NewLine +
                    "onHireFlag = " + onHireFlag + Environment.NewLine +
                    "extrasFlag = " + extrasFlag + Environment.NewLine +
                    "concrete = " + concrete + Environment.NewLine +
                    "blocks = " + blocks + Environment.NewLine +
                    "drawingsEmailedFlag = " + drawingsEmailedFlag + Environment.NewLine +
                    "draughtsman = " + draughtsman + Environment.NewLine +
                    "salesman = " + salesman + Environment.NewLine +
                    "lastComment = " + lastComment;

                //if (jobNo == "30598.04")
                //{
                //    MessageBox.Show(testline);
                //}

                //if (tabPageName == "07/11/2022")
                //{
                //    MessageBox.Show(testline);
                //}

                // MessageBox.Show(String.Format("Tab[{0}] : JobNo = {1} --> [{2}]/[{3}]/[{4}]/[{5}]/[{6}]/[{7}]/[{8}]", tabPageName, dgv.Rows.Count.ToString(),wcMonday,wcTuesday,wcWednesday,wcThursday,wcFriday,wcSaturday,wcSunday));

                string err = mcData.UpdateWhiteBoard(jobNo, /*requiredDate,*/ custCode, siteAddress, supplyType, products, totalM2, fixingDaysAllowance, salesPrice, isInvoiced, suppShortname, stairsIncl,
                                                        stairsSupplier, floorlevel, continuedFlag, cowSentFlag, cowReceived,/*wcMonday,wcTuesday,wcWednesday,wcThursday,wcFriday,wcSaturday,wcSunday,*/
                                                        contracts, ramsFlag, ramsSignedReturnedFlag, ramsCompleteReturnedFlag, lorry, craneSize, craneSupplier, spreaderMatFlag, hireContractRcvd,
                                                        fallArrest, fixingGang, onHireFlag, extrasFlag, concrete, blocks, drawingsEmailedFlag, draughtsman, salesman, lastComment,sortType);
                if (err == "OK")
                {
                    numInProgressJobsUpdated++;
                    //  logger.LogLine(String.Format("WhiteBoardDGVToDB({0}) --> mcData.UpdateWhiteBoard({1},......) --> OK ", tabPageName, jobNo));
                    continue;
                }
                else
                {
                    mcData.CreateErrorAudit("WhiteboardForm.cs", "WhiteBoardDGVToDB(tab " + tabPageName + " on Job[" + jobNo + "])", err);
                    MessageBox.Show(String.Format("Error updating whiteboard DGV on Tab {0} : {1} ", tabPageName, err));
                    //   logger.LogLine(String.Format("WhiteBoardDGVToDB({0}) --> mcData.UpdateWhiteBoard({1},......) --> ERROR : {2} ", tabPageName, jobNo,err));
                    return numInProgressJobsUpdated;

                }
            }
            //logger.LogLine(String.Format("TAB[{0}] : Num of inprogress jobs = {1}", tabPageName, numInProgressJobsUpdated));
            return numInProgressJobsUpdated;
        }

        private void addCommenttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }
            /*
            // test code lines ==============================
            DateTime requiredDate = Convert.ToDateTime(wbDataGridView[1, this.rowIndex].Value.ToString()).Date;
            int diff = colIndex - 17;
            DateTime commDate = mcData.GetMonday(requiredDate).AddDays(diff).AddDays(0);
            string message = "Row = " + rowIndex.ToString() + Environment.NewLine +
                             "Col = " + colIndex.ToString() + Environment.NewLine +
                             "Req Date = " + requiredDate.ToShortDateString() + Environment.NewLine +
                             "Days Diff = " + diff.ToString() + Environment.NewLine +
                             "Comment Date = " + commDate.ToShortDateString();

            MessageBox.Show(message); return;
            */

            //   if (wbDataGridView[this.colIndex, this.rowIndex].Style.BackColor != Color.Yellow) { return; }
            if (colIndex != 17 && colIndex != 18 && colIndex != 19 && colIndex != 20 && colIndex != 21 && colIndex != 22 && colIndex != 23) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "WB");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot update the comment");
                return;

            }
            DateTime reqDate = Convert.ToDateTime(wbDataGridView[1, this.rowIndex].Value.ToString()).Date;
            string comment = wbDataGridView[this.colIndex, this.rowIndex].Value == null ? "N/A" : wbDataGridView[this.colIndex, this.rowIndex].Value.ToString();
            int diffDays = colIndex - 17;
            DateTime commentDate = mcData.GetMonday(reqDate).AddDays(diffDays).AddDays(0);
            WhiteboardDayCommentForm commentForm = new WhiteboardDayCommentForm(jobNo, commentDate, comment);
            commentForm.ShowDialog();
            wbDataGridView[this.colIndex, this.rowIndex].Value = commentForm.DayComment;
            UpdateWBJobDayCommentOnly(jobNo, commentForm.DayComment, colIndex, commentDate);



            return;
        }

        private void UpdateWBJobDayCommentOnly(string jobNo, string comment, int dayColIndex, DateTime commentDate)
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
            string err = mcData.UpdateWhiteBoardJobDayComment(jobNo, fieldName, comment);
            if (err != "OK")
            {
                MessageBox.Show(String.Format("Error updating day comment for whiteboard Job No {0} : {1}", jobNo, err));
                return;
            }
            else
            {
                string err2 = mcData.CreateWBDayCommentAudit(jobNo, comment, commentDate);
                return;
            }
        }

        private void jobPlannerStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            int parentJob = Convert.ToInt32(jobNo.Substring(0, 5));
            DataTable dt = mcData.GetJobPlannerDT(parentJob);
            JobPlannerForm plannerForm = new JobPlannerForm(dt);
            plannerForm.ShowDialog();
            int num = mcData.EmptyCurrentInvoicedWhiteboardJobList();
            this.Dispose();
            this.Close();
            return;
        }

        private void selectSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuppliersListForm myForm;
            int rgb1, rgb2, rgb3 = 255;

            if (wbDataGridView[0, rowIndex].Value == null) { return; }

            if (wbDataGridView[10, rowIndex].Value == null && wbDataGridView[10, rowIndex].Value == null) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            string suppShortName = wbDataGridView[10, this.rowIndex].Value.ToString();
            if (colIndex == 10)
            {
                if (wbDataGridView[10, rowIndex].Value == null)
                {
                    myForm = new SuppliersListForm();
                }
                else
                {
                    myForm = new SuppliersListForm(wbDataGridView.Rows[rowIndex].Cells[10].Value.ToString());
                }
                myForm.ShowDialog();
                suppShortName = myForm.Shortname;
                wbDataGridView[10, rowIndex].Value = suppShortName;
                mcData.GetSupplierColourByShortname(suppShortName, out rgb1, out rgb2, out rgb3);
                wbDataGridView[10, rowIndex].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                if (!String.IsNullOrWhiteSpace(suppShortName))
                {
                    string err1 = mcData.UpdateWhiteBoardSupplierShortName(jobNo, suppShortName);
                    string err2 = mcData.UpdateJobPlannerSupplierShortName(jobNo, suppShortName);

                }

                return;
            }
            if (colIndex == 12)
            {
                if (wbDataGridView[12, rowIndex].Value == null)
                {
                    myForm = new SuppliersListForm();
                }
                else
                {
                    myForm = new SuppliersListForm(wbDataGridView.Rows[rowIndex].Cells[12].Value.ToString());
                }
                myForm.ShowDialog();
                wbDataGridView[12, rowIndex].Value = myForm.Shortname;
                mcData.GetSupplierColourByShortname(myForm.Shortname, out rgb1, out rgb2, out rgb3);
                wbDataGridView[12, rowIndex].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                return;
            }


        }

        private void selectProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductSelectorForm myForm = new ProductSelectorForm();
            myForm.ShowDialog();
            wbDataGridView[5, rowIndex].Value = myForm.Product;
            return;
        }

        //private void outstandingRamsButton_Click(object sender, EventArgs e)
        //{
        //    OutstandingRamsForm ramsForm = new OutstandingRamsForm();
        //    ramsForm.ShowDialog();
        //    return;
        //}

        private void raisePOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            RaisePOForm poForm = new RaisePOForm(jobNo);
            poForm.ShowDialog();

        }

        private void completeJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            if (MessageBox.Show(String.Format("Confirm INVOICED status and Completion of Job No.{0}", jobNo), "Confirm INVOICED status and Job Completion", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string joblist = mcData.GetExtendedJobsList(jobNo);
                string err = mcData.CompleteJobPlanner(jobNo, "Y");
                string err2 = mcData.CompleteWhiteboardJob(jobNo, "Y");
                if (err == "OK" && err2 == "OK")
                {
                    string response = mcData.AddWhiteboardJobToCurrentInvoicedList(jobNo.Substring(0, 8));
                    string message = $"Following job(s) now marked as INVOICED and complete : '{Environment.NewLine} {Environment.NewLine} {joblist}";
                    wbDataGridView[9, this.rowIndex].Value = true;
                    mcData.CreateJobComment(jobNo, String.Format("*** Job No.{0} is now marked as INVOICED and complete ***", jobNo));
                    wbDataGridView.Rows[rowIndex].ReadOnly = true;
                    wbDataGridView.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Gray;
                    MessageBox.Show($"{message}{Environment.NewLine}{Environment.NewLine} When you exit the whiteboard and return to it, all extensions of job {jobNo.Substring(0, 8)} will be greyed out", "Job(s) Completed");
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            int num = mcData.EmptyCurrentInvoicedWhiteboardJobList();
            int numLocksDeleted = mcData.DeleteUserLocks("WB", loggedInUser);
            this.Dispose();
            this.Close();
        }

        private void weeksTabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) // added this and it now works
            {
                tabPageDate = Convert.ToDateTime(weeksTabControl.SelectedTab.Text);
                return;
            }
        }

        private void jobCommentsAudittoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            WhiteboardDayCommentAuditForm auditForm = new WhiteboardDayCommentAuditForm(jobNo);
            auditForm.ShowDialog();
            return;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show($"Please ensure you save any of your UNSAVED jobs using the [SAVE JOB LINE] option before continuing with the REFRESH option otherwise you will lose your changes.{Environment.NewLine} Do you wish to continue ?","Confirm REFRESH",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int numLocksDeleted = mcData.DeleteUserLocks("WB", loggedInUser);
                weeksTabControl.TabPages.Clear();
                BuildTabs();
                FillSuppliersStatusStrip();
            }
            return;
        }

        private void jobDateAuditMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            JobDateAuditForm auditForm = new JobDateAuditForm(jobNo);
            auditForm.ShowDialog();
            return;
        }

        private void spanJobMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }

            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();

            if (mcData.IsJobCompleted(jobNo))
            {
                MessageBox.Show(String.Format("Job No. {0} is a COMPLETED job and so cannot be spanned to multiple weeks", jobNo));
                return;
            }
            string msg = "Spanning this job over another week will mean a new copy of this job will sit in that new week with EMPTY comments which you can update "
                                + Environment.NewLine + Environment.NewLine +
                        "This job itself will remain in this week. "
                                + Environment.NewLine + Environment.NewLine +
                                "Are you happy to continue with selecting the NEW date?";
            if (MessageBox.Show(msg, String.Format("Confirm spanning Job No {0} over another week", jobNo), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                //MessageBox.Show(mcData.GetNextJobSuffix(jobNo));
                DateTime requiredDate = Convert.ToDateTime(wbDataGridView.Rows[this.rowIndex].Cells[1].Value.ToString());
                DateSelectorForm dateForm = new DateSelectorForm(requiredDate);
                dateForm.ShowDialog();
                DateTime newRequiredDate = dateForm.RequiredDate;
                DataTable wbJobDT = mcData.GetWhiteboardByJobDT(jobNo);
                String latestJobNo = mcData.GetLastJobNo(jobNo);
                String newJobNo = mcData.GetNextJobSuffix(latestJobNo);
                string err = mcData.CreateWhiteBoardSpannedJobCopy(newJobNo, newRequiredDate, wbJobDT);
                if (err == "OK")
                {
                    string err2a = mcData.CreateJobDayAudit(newJobNo, newRequiredDate.Date);
                    DateTime monDate = mcData.GetMonday(newRequiredDate);
                    string msg2 = String.Format("Job No. {0} is now spanning across multiple weeks and has been extended to new job No. {1}", jobNo, newJobNo) + Environment.NewLine + Environment.NewLine +
                                    String.Format("Simply click on the week commencing [{0}] tab to find your extended job No. {1} ", monDate.ToShortDateString(), newJobNo);
                    MessageBox.Show(msg2);
                    weeksTabControl.TabPages.Clear();
                    BuildTabs();
                    FillSuppliersStatusStrip();
                    return;
                }
                return;
            }

        }

        private void removedExtendedJobMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }

            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();

            if (mcData.IsJobCompleted(jobNo))
            {
                MessageBox.Show(String.Format("Job No. {0} is a COMPLETED job and so cannot be deleted", jobNo));
                return;
            }

            if (!mcData.IsContainLetter(jobNo)) { return; }

            string msg = String.Format("Are you sure you wish to remove extended job [{0}] ?", jobNo);

            if (MessageBox.Show(msg, "Confirm Removal of Extended Job", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int num = mcData.DeleteWhiteboardByJobNo(jobNo);
                weeksTabControl.TabPages.Clear();
                BuildTabs();
                FillSuppliersStatusStrip();
            }

            return;
        }

        private void saveJobLineMenuItem_Click(object sender, EventArgs e)
        {
            wbDataGridView.NotifyCurrentCellDirty(true);
            int i = this.rowIndex;
            if (wbDataGridView.Rows[i].Cells[0].Value == null || wbDataGridView.Rows[i].Cells[0].Value.ToString().Length < 8) { return; }

            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            string response = mcData.GetJobLockedUser(jobNo, "WB");
            if (!response.Equals("n/a") && !response.Equals(loggedInUser))
            {
                string userName = mcData.GetUserFullNameFromUserID(response);
                MessageBox.Show($"Job {jobNo} islocked by {userName}. Cannot save this job");
                return;

            }
            
            int numJobLockDeleted = mcData.DeleteJobLocks("WB", jobNo);
            if (numJobLockDeleted.Equals(0))
            {
                MessageBox.Show($"Unable to remove lock(s) on Job {jobNo}. Cannot continue");
                return;
            }
            DateTime requiredDate = Convert.ToDateTime(wbDataGridView.Rows[i].Cells[1].Value.ToString());
            string custName = wbDataGridView.Rows[i].Cells[2].Value == null ? "" : wbDataGridView.Rows[i].Cells[2].Value.ToString();
            string custCode = mcData.GetCustCodeByCustName(custName);
            string siteAddress = wbDataGridView.Rows[i].Cells[3].Value == null ? "" : wbDataGridView.Rows[i].Cells[3].Value.ToString();
            string supplyType = wbDataGridView.Rows[i].Cells[4].Value == null ? "" : wbDataGridView.Rows[i].Cells[4].Value.ToString();
            string products = wbDataGridView.Rows[i].Cells[5].Value == null ? "" : wbDataGridView.Rows[i].Cells[5].Value.ToString();
            int totalM2 = wbDataGridView.Rows[i].Cells[6].Value == null ? 0 : Convert.ToInt16(wbDataGridView.Rows[i].Cells[6].Value.ToString());
            int fixingDaysAllowance = wbDataGridView.Rows[i].Cells[7].Value == null ? 0 : Convert.ToInt16(wbDataGridView.Rows[i].Cells[7].Value.ToString());
            decimal salesPrice = wbDataGridView.Rows[i].Cells[8].Value == null ? 0 : Convert.ToDecimal(wbDataGridView.Rows[i].Cells[8].Value.ToString());
            string isInvoiced = (bool)wbDataGridView.Rows[i].Cells[9].Value ? "Y" : "N";
            string suppShortname = wbDataGridView.Rows[i].Cells[10].Value == null ? "" : wbDataGridView.Rows[i].Cells[10].Value.ToString();
            string stairsIncl = (bool)wbDataGridView.Rows[i].Cells[11].Value ? "Y" : "N";
            string stairsSupplier = wbDataGridView.Rows[i].Cells[12].Value == null ? "" : wbDataGridView.Rows[i].Cells[12].Value.ToString();
            string floorlevel = wbDataGridView.Rows[i].Cells[13].Value == null ? "" : wbDataGridView.Rows[i].Cells[13].Value.ToString();
            string continuedFlag = (bool)wbDataGridView.Rows[i].Cells[14].Value ? "Y" : "N";
            string cowSentFlag = (bool)wbDataGridView.Rows[i].Cells[15].Value ? "Y" : "N";
            string cowReceived = wbDataGridView.Rows[i].Cells[16].Value == null ? "" : wbDataGridView.Rows[i].Cells[16].Value.ToString();
            string wcMonday = wbDataGridView.Rows[i].Cells[17].Value == null ? "" : wbDataGridView.Rows[i].Cells[17].Value.ToString();
            string wcTuesday = wbDataGridView.Rows[i].Cells[18].Value == null ? "" : wbDataGridView.Rows[i].Cells[18].Value.ToString();
            string wcWednesday = wbDataGridView.Rows[i].Cells[19].Value == null ? "" : wbDataGridView.Rows[i].Cells[19].Value.ToString();
            string wcThursday = wbDataGridView.Rows[i].Cells[20].Value == null ? "" : wbDataGridView.Rows[i].Cells[20].Value.ToString();
            string wcFriday = wbDataGridView.Rows[i].Cells[21].Value == null ? "" : wbDataGridView.Rows[i].Cells[21].Value.ToString();
            string wcSaturday = wbDataGridView.Rows[i].Cells[22].Value == null ? "" : wbDataGridView.Rows[i].Cells[22].Value.ToString();
            string wcSunday = wbDataGridView.Rows[i].Cells[23].Value == null ? "" : wbDataGridView.Rows[i].Cells[23].Value.ToString();
            string contracts = wbDataGridView.Rows[i].Cells[24].Value == null ? "" : wbDataGridView.Rows[i].Cells[24].Value.ToString();
            string ramsFlag = (bool)wbDataGridView.Rows[i].Cells[25].Value ? "Y" : "N";
            string ramsSignedReturnedFlag = (bool)wbDataGridView.Rows[i].Cells[26].Value ? "Y" : "N";
            string ramsCompleteReturnedFlag = (bool)wbDataGridView.Rows[i].Cells[27].Value ? "Y" : "N";
            string lorry = wbDataGridView.Rows[i].Cells[28].Value == null ? "" : wbDataGridView.Rows[i].Cells[28].Value.ToString();
            string craneSize = wbDataGridView.Rows[i].Cells[29].Value == null ? "" : wbDataGridView.Rows[i].Cells[29].Value.ToString();
            string craneSupplier = wbDataGridView.Rows[i].Cells[30].Value == null ? "" : wbDataGridView.Rows[i].Cells[30].Value.ToString();
            string spreaderMatFlag = (bool)wbDataGridView.Rows[i].Cells[31].Value ? "Y" : "N";
            string hireContractRcvd = wbDataGridView.Rows[i].Cells[32].Value == null ? "" : wbDataGridView.Rows[i].Cells[32].Value.ToString();
            string fallArrest = wbDataGridView.Rows[i].Cells[33].Value == null ? "" : wbDataGridView.Rows[i].Cells[33].Value.ToString();
            string fixingGang = wbDataGridView.Rows[i].Cells[34].Value == null ? "" : wbDataGridView.Rows[i].Cells[34].Value.ToString();
            string onHireFlag = (bool)wbDataGridView.Rows[i].Cells[35].Value ? "Y" : "N";
            string extrasFlag = (bool)wbDataGridView.Rows[i].Cells[36].Value ? "Y" : "N";
            string concrete = wbDataGridView.Rows[i].Cells[37].Value == null ? "" : wbDataGridView.Rows[i].Cells[37].Value.ToString();
            string blocks = wbDataGridView.Rows[i].Cells[38].Value == null ? "" : wbDataGridView.Rows[i].Cells[38].Value.ToString();
            string drawingsEmailedFlag = (bool)wbDataGridView.Rows[i].Cells[39].Value ? "Y" : "N";
            string draughtsman = wbDataGridView.Rows[i].Cells[40].Value == null ? "" : wbDataGridView.Rows[i].Cells[40].Value.ToString();
            string salesman = wbDataGridView.Rows[i].Cells[41].Value == null ? "" : wbDataGridView.Rows[i].Cells[41].Value.ToString();
            string lastComment = wbDataGridView.Rows[i].Cells[42].Value == null ? "" : wbDataGridView.Rows[i].Cells[42].Value.ToString();
            string sortType = "S" + supplyType.Substring(1, 1);

            string testline =
                    "JobNo = " + jobNo + Environment.NewLine +
                    "custName = " + custName + Environment.NewLine +
                    "siteAddress = " + siteAddress + Environment.NewLine +
                    "supplyType = " + supplyType + Environment.NewLine +
                    "sortType = " + sortType + Environment.NewLine +
                    "products = " + products + Environment.NewLine +
                    "totalM2 = " + totalM2 + Environment.NewLine +
                    "fixingDaysAllowance = " + fixingDaysAllowance + Environment.NewLine +
                    "salesPrice = " + salesPrice + Environment.NewLine +
                    "isInvoiced = " + isInvoiced + Environment.NewLine +
                    "suppShortname = " + suppShortname + Environment.NewLine +
                    "stairsIncl = " + stairsIncl + Environment.NewLine +
                    "stairsSupplier = " + stairsSupplier + Environment.NewLine +
                    "floorlevel = " + floorlevel + Environment.NewLine +
                    "continuedFlag = " + continuedFlag + Environment.NewLine +
                    "cowSentFlag = " + cowSentFlag + Environment.NewLine +
                    "cowReceived = " + cowReceived + Environment.NewLine +
                    "wcMonday = " + wcMonday + Environment.NewLine +
                    "wcTuesday = " + wcTuesday + Environment.NewLine +
                    "wcWednesday = " + wcWednesday + Environment.NewLine +
                    "wcThursday = " + wcThursday + Environment.NewLine +
                    "wcFriday = " + wcFriday + Environment.NewLine +
                    "wcSaturday = " + wcSaturday + Environment.NewLine +
                    "wcSunday = " + wcSunday + Environment.NewLine +
                    "contracts = " + contracts + Environment.NewLine +
                    "ramsFlag = " + ramsFlag + Environment.NewLine +
                    "ramsSignedReturnedFlag = " + ramsSignedReturnedFlag + Environment.NewLine +
                    "ramsCompleteReturnedFlag = " + ramsCompleteReturnedFlag + Environment.NewLine +
                    "lorry = " + lorry + Environment.NewLine +
                    "craneSize = " + craneSize + Environment.NewLine +
                    "craneSupplier = " + craneSupplier + Environment.NewLine +
                    "spreaderMatFlag = " + spreaderMatFlag + Environment.NewLine +
                    "hireContractRcvd = " + hireContractRcvd + Environment.NewLine +
                    "fallArrest = " + fallArrest + Environment.NewLine +
                    "fixingGang = " + fixingGang + Environment.NewLine +
                    "onHireFlag = " + onHireFlag + Environment.NewLine +
                    "extrasFlag = " + extrasFlag + Environment.NewLine +
                    "concrete = " + concrete + Environment.NewLine +
                    "blocks = " + blocks + Environment.NewLine +
                    "drawingsEmailedFlag = " + drawingsEmailedFlag + Environment.NewLine +
                    "draughtsman = " + draughtsman + Environment.NewLine +
                    "salesman = " + salesman + Environment.NewLine +
                    "lastComment = " + lastComment;

            if (MessageBox.Show(testline, "Confirm you want save Job No.[" + jobNo + "] line", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string err = mcData.UpdateWhiteBoardLine(jobNo, /*requiredDate,*/ custCode, siteAddress, supplyType, products, totalM2, fixingDaysAllowance, salesPrice, isInvoiced, suppShortname, stairsIncl,
                                                        stairsSupplier, floorlevel, continuedFlag, cowSentFlag, cowReceived, wcMonday, wcTuesday, wcWednesday, wcThursday, wcFriday, wcSaturday, wcSunday,
                                                        contracts, ramsFlag, ramsSignedReturnedFlag, ramsCompleteReturnedFlag, lorry, craneSize, craneSupplier, spreaderMatFlag, hireContractRcvd,
                                                        fallArrest, fixingGang, onHireFlag, extrasFlag, concrete, blocks, drawingsEmailedFlag, draughtsman, salesman, lastComment,sortType);

                if (err == "OK")
                {
                    MessageBox.Show("JobNo[" + jobNo + "] line saved successfully");
                    return;
                }
                else
                {
                    mcData.CreateErrorAudit("WhiteboardForm.cs", "saveJobLineMenuItem_Click on Job[" + jobNo + "]", err);
                    MessageBox.Show(String.Format("Error saving JobNo[{0}] : {1} ", jobNo, err));
                    return;

                }
            }
            return;
        }



        private void GenerateWhiteBoardReport()
        {
            // **************** NOT READY YET ***********************
            this.Cursor = Cursors.WaitCursor;

            var dgvList = new List<DataGridView>();
            DataGridView[] dgvArray = new DataGridView[] { };
            String[] tabNameArray = new String[] { };
            var tabNameList = new List<String>();
            string tabName = "";
            foreach (Control thisControl in weeksTabControl.Controls)
            {
                if (thisControl.GetType() == typeof(TabPage))
                {
                    TabPage tb = (TabPage)thisControl;
                    tabName = tb.Text.Replace("/", "_");
                    foreach (Control dgv in thisControl.Controls)
                    {
                        if (dgv.GetType() == typeof(DataGridView))
                        {
                            DataGridView mydgv = (DataGridView)dgv;
                            dgvList.Add(mydgv);
                            tabNameList.Add(tabName);
                        }
                    }

                }
            }
            dgvArray = dgvList.ToArray();
            tabNameArray = tabNameList.ToArray();
            string filename = String.Format("Whiteboard{0}_to_{1}", wbStartDate.ToString("ddMMMyyyy"), wbEndDate.ToString("ddMMMyyyy"));
            DataGridviewImportToExcel(dgvArray, tabNameArray, filename);

            this.Cursor = Cursors.Default;
            return;
        }




        private bool DataGridviewImportToExcel(DataGridView[] dgv, String[] tabName, string fileName)
        {
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xlsx";
            saveDialog.Filter = "Excel file|*.xlsx";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0)
                return false;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("can not create Excel");
                return false;
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            //   worksheet.Tab.ToString() = dgv[0].Columns[0].Tag;

            for (int index = 0; index < dgv.Length; index++)
            {
                worksheet.Name = tabName[index];

                for (int i = 0; i < dgv[index].ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dgv[index].Columns[i].HeaderText;
                }

                for (int r = 0; r < dgv[index].Rows.Count; r++)
                {
                    for (int i = 0; i < dgv[index].ColumnCount; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rg = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[r + 2, 2];
                        rg.EntireColumn.NumberFormat = "dd mmm yyyy";
                        worksheet.Cells[r + 2, i + 1] = dgv[index].Rows[r].Cells[i].Value;
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
                worksheet.Columns.EntireColumn.AutoFit();
                if (index < dgv.Length - 1)
                    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add();
            }

            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error,file maybe is opening！\n" + ex.Message);
                    return false;
                }
            }
            xlApp.Quit();
            GC.Collect();
            MessageBox.Show("File： " + fileName + ".xls save Successfully", "tip ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        private void copyAlltoClipboard()
        {
            //  return;
            wbDataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            wbDataGridView.MultiSelect = true;
            wbDataGridView.SelectAll();
            DataObject dataObj = wbDataGridView.GetClipboardContent();
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

        private void GenerateWhiteboardExcelReport(DataTable sourceDT, DateTime wcDate)
        {

            DataTable wbDT = new DataTable();
            wbDT.Columns.Clear();
            wbDT.Columns.Add("jobNo", typeof(string));
            wbDT.Columns.Add("requiredDate", typeof(DateTime));
            wbDT.Columns.Add("custName", typeof(string));
            wbDT.Columns.Add("siteAddress", typeof(string));
            // wbDT.Columns.Add("customerCode", typeof(string));
            wbDT.Columns.Add("supplyType", typeof(string));
            wbDT.Columns.Add("products", typeof(string));
            wbDT.Columns.Add("totalM2", typeof(int));
            wbDT.Columns.Add("fixingDaysAllowance", typeof(int));
            wbDT.Columns.Add("salesPrice", typeof(decimal));
            wbDT.Columns.Add("isInvoiced", typeof(string));
            wbDT.Columns.Add("suppShortname", typeof(string));
            wbDT.Columns.Add("stairsIncl", typeof(string));
            wbDT.Columns.Add("stairsSupplier", typeof(string));
            wbDT.Columns.Add("floorlevel", typeof(string));
            wbDT.Columns.Add("continuedFlag", typeof(string));
            wbDT.Columns.Add("cowSentFlag", typeof(string));
            wbDT.Columns.Add("cowReceived", typeof(string));
            wbDT.Columns.Add("wcMonday", typeof(string));
            wbDT.Columns.Add("wcTuesday", typeof(string));
            wbDT.Columns.Add("wcWednesday", typeof(string));
            wbDT.Columns.Add("wcThursday", typeof(string));
            wbDT.Columns.Add("wcFriday", typeof(string));
            wbDT.Columns.Add("wcSaturday", typeof(string));
            wbDT.Columns.Add("wcSunday", typeof(string));
            wbDT.Columns.Add("contracts", typeof(string));
            wbDT.Columns.Add("ramsFlag", typeof(string));
            wbDT.Columns.Add("ramsSignedReturnedFlag", typeof(string));
            wbDT.Columns.Add("ramsCompleteReturnedFlag", typeof(string));
            wbDT.Columns.Add("lorry", typeof(string));
            wbDT.Columns.Add("craneSize", typeof(string));
            wbDT.Columns.Add("craneSupplier", typeof(string));
            wbDT.Columns.Add("spreaderMatFlag", typeof(string));
            wbDT.Columns.Add("hireContractRcvd", typeof(string));
            wbDT.Columns.Add("fallArrest", typeof(string));
            wbDT.Columns.Add("fixingGang", typeof(string));
            wbDT.Columns.Add("onHireFlag", typeof(string));
            wbDT.Columns.Add("extrasFlag", typeof(string));
            wbDT.Columns.Add("concrete", typeof(string));
            wbDT.Columns.Add("blocks", typeof(string));
            wbDT.Columns.Add("drawingsEmailedFlag", typeof(string));
            wbDT.Columns.Add("draughtsman", typeof(string));
            wbDT.Columns.Add("salesman", typeof(string));
            wbDT.Columns.Add("lastComment", typeof(string));
            wbDT.Columns.Add("dateCreated", typeof(string));
            wbDT.Columns.Add("jobCreatedBy", typeof(string));

            string dateCreated = "";
            string custName = "";
            DateTime reqDate;
            Double dateValue;
            string jobNo = "";
            foreach (DataRow row in sourceDT.Rows)
            {
                DataRow dr = wbDT.NewRow();
                jobNo = row["jobNo"].ToString();
                dr["jobNo"] = jobNo;
                dateCreated = mcData.GetJobCreatedDate(jobNo).ToString("dd/MMM/yyyy hh:mm tt");
                reqDate = Convert.ToDateTime(row["requiredDate"]);
                dateValue = reqDate.ToOADate();
                dr["requiredDate"] = dateValue;
                custName = mcData.GetCustName(dr["custCode"].ToString());
                dr["custName"] = custName;

                dr["siteAddress"] = row["siteAddress"].ToString();
                //   dr["customerCode"] = row["customerCode"].ToString();
                dr["supplyType"] = row["supplyType"].ToString();
                dr["products"] = row["products"].ToString();
                dr["totalM2"] = Convert.ToInt16(row["totalM2"].ToString());
                dr["fixingDaysAllowance"] = Convert.ToInt16(row["fixingDaysAllowance"].ToString());
                dr["salesPrice"] = Convert.ToDecimal(row["salesPrice"].ToString());
                dr["isInvoiced"] = row["isInvoiced"].ToString();
                dr["suppShortname"] = row["suppShortname"].ToString();
                dr["stairsIncl"] = row["stairsIncl"].ToString();
                dr["stairsSupplier"] = row["stairsSupplier"].ToString();
                dr["floorlevel"] = row["floorlevel"].ToString();
                dr["continuedFlag"] = row["continuedFlag"].ToString();
                dr["cowSentFlag"] = row["cowSentFlag"].ToString();
                dr["cowReceived"] = row["cowReceived"].ToString();
                dr["wcMonday"] = row["wcMonday"].ToString();
                dr["wcTuesday"] = row["wcTuesday"].ToString();
                dr["wcWednesday"] = row["wcWednesday"].ToString();
                dr["wcThursday"] = row["wcThursday"].ToString();
                dr["wcFriday"] = row["wcFriday"].ToString();
                dr["wcSaturday"] = row["wcSaturday"].ToString();
                dr["wcSunday"] = row["wcSunday"].ToString();
                dr["contracts"] = row["contracts"].ToString();
                dr["ramsFlag"] = row["ramsFlag"].ToString();
                dr["ramsSignedReturnedFlag"] = row["ramsSignedReturnedFlag"].ToString();
                dr["ramsCompleteReturnedFlag"] = row["ramsCompleteReturnedFlag"].ToString();
                dr["craneSize"] = row["craneSize"].ToString();
                dr["craneSupplier"] = row["craneSupplier"].ToString();
                dr["spreaderMatFlag"] = row["spreaderMatFlag"].ToString();
                dr["hireContractRcvd"] = row["hireContractRcvd"].ToString();
                dr["fallArrest"] = row["fallArrest"].ToString();
                dr["fixingGang"] = row["fixingGang"].ToString();
                dr["onHireFlag"] = row["onHireFlag"].ToString();
                dr["extrasFlag"] = row["extrasFlag"].ToString();
                dr["concrete"] = row["concrete"].ToString();
                dr["blocks"] = row["blocks"].ToString();
                dr["drawingsEmailedFlag"] = row["drawingsEmailedFlag"].ToString();
                dr["draughtsman"] = row["draughtsman"].ToString();
                dr["salesman"] = row["salesman"].ToString();
                dr["lastComment"] = row["lastComment"].ToString();
                dr["dateCreated"] = dateCreated;
                dr["jobCreatedBy"] = row["jobCreatedBy"].ToString();
                wbDT.Rows.Add(dr);

            }


        }



        private void exportToEXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            wbDataGridView.NotifyCurrentCellDirty(true);
            int i = this.rowIndex;
            if (wbDataGridView.Rows[i].Cells[0].Value == null || wbDataGridView.Rows[i].Cells[0].Value.ToString().Length < 8) { return; }
            DateTime requiredDate = Convert.ToDateTime(wbDataGridView.Rows[i].Cells[1].Value.ToString());
            DateTime wcDate = mcData.GetMonday(requiredDate);
            this.Cursor = Cursors.Default;
            WhiteboardRptForm wbRptForm = new WhiteboardRptForm(wcDate);
            wbRptForm.ShowDialog();

        }

        private void btnJobLocks_Click(object sender, EventArgs e)
        {
            if(!mcData.IsJobLockExistByType("WB"))
            {
                MessageBox.Show("There are currently no locked jobs on the Whiteboard");
                return;
            }

            LockedJobsForm lockForm = new LockedJobsForm("WB");
            lockForm.ShowDialog();
            return;
        }

        private void WhiteboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            int num = mcData.EmptyCurrentInvoicedWhiteboardJobList();
            int numLocksDeleted = mcData.DeleteUserLocks("WB", loggedInUser);
            this.Dispose();
            this.Close();
            return;

            //if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.WindowsShutDown)
            //{
            //    int num = mcData.EmptyCurrentInvoicedWhiteboardJobList();
            //    int numLocksDeleted = mcData.DeleteUserLocks("WB", loggedInUser);
            //    this.Dispose();
            //    this.Close();
            //}
        }

        private void putJobCustomerONSTOPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wbDataGridView[0, rowIndex].Value == null) { return; }
            string jobNo = wbDataGridView[0, this.rowIndex].Value.ToString();
            string custName = wbDataGridView[2, this.rowIndex].Value.ToString();
            string msg = $"Any job from customer [{custName.ToUpper()}] will be flagged as RED and customer put on [ON STOP] status. {Environment.NewLine} {Environment.NewLine} Are you sure you wish to put customer [{custName.ToUpper()}] for Job No.{jobNo} ON STOP ?";
            if (MessageBox.Show(msg, "Job Customer ON STOP Confirmation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                string result = mcData.UpdateCustomerOnStopFlag(custCode, "Y");
                if (result == "OK")
                {
                    wbDataGridView.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    MessageBox.Show($"All jobs for customer [{custName}] are now highlighted in RED");
                    return;
                }
            }
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
