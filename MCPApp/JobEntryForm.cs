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
    public partial class JobEntryForm : Form
    {
        MeltonData mcData = new MeltonData();
        DataTable pjobDetailDT = new DataTable();
        Logger logger = new Logger();
        
        int parentJobNo = 0;
        string custCode = "";
        string custName = "";
        string siteAddress = "";
        string siteContact = "";
        string siteContactTel = "";
        string siteContactEmail = "";
        bool updatePhaseJobsOnly = false;
        private int rowIndex = 0;
        private int colIndex = 0;

        private bool cancelJob = false;
        public bool CancelJob
        {
            get
            {
                return cancelJob;
            }
            set
            {
                cancelJob = value;
            }
        }

        public JobEntryForm()
        {
            InitializeComponent();
        }

        public JobEntryForm(DataTable headerDT)
        {
            InitializeComponent();
            pjobDetailDT = headerDT;
            foreach (DataRow dr in pjobDetailDT.Rows)
            {
                parentJobNo = Convert.ToInt32(dr["parentJobNo"].ToString());
                custCode = dr["custCode"].ToString();
                custName = dr["custName"].ToString();
                siteAddress = dr["siteAddress"].ToString();
                siteContact = dr["siteContact"].ToString();
                siteContactTel = dr["siteContactTel"].ToString();
                siteContactEmail = dr["siteContactEmail"].ToString();
            }
        }

        public JobEntryForm(DataTable headerDT,bool phaseOnly)
        {
            InitializeComponent();
            updatePhaseJobsOnly = phaseOnly;
            pjobDetailDT = headerDT;
            foreach (DataRow dr in pjobDetailDT.Rows)
            {
                parentJobNo = Convert.ToInt32(dr["parentJobNo"].ToString());
                custCode = dr["custCode"].ToString();
                custName = mcData.GetCustName(custCode);
                //custName = dr["custName"].ToString();
                siteAddress = dr["siteAddress"].ToString();
                siteContact = dr["siteContact"].ToString();
                siteContactTel = dr["siteContactTel"].ToString();
                siteContactEmail = dr["siteContactEmail"].ToString();
            }
        }

        private void JobEntryForm_Load(object sender, EventArgs e)
        {
            this.Text = "Create phases for Job No." + pjobDetailDT;
            parentJobNoTextBox.Text = parentJobNo.ToString();
            custNameTextBox.Text = custName;
            custCodeTextBox.Text = custCode;
            siteAddressTextBox.Text = siteAddress;
            siteContactTextBox.Text = siteContact;
            contactTelTextBox.Text = siteContactTel;
            contactEmailTextBox.Text = siteContactEmail;
            label8.Text = "NOTE - Please do not use UPPERCASE for the full site address. Only use it for the postcode";
            BuildDGV();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you want to cancel job creation", "Cancel Job", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                cancelJob = true;
                int parentJobNo = Convert.ToInt32(parentJobNoTextBox.Text);
                DataTable jobsDT = mcData.GetJobPlannerDT(parentJobNo);

                int numDeleted = mcData.DeleteJobPlannerByParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                if(numDeleted > 0)
                {
                    string job = "";

                    if (jobsDT.Rows.Count > 1)
                    {
                        foreach (DataRow dr in jobsDT.Rows)
                        {
                            if (dr["jobNo"] == null) { continue; }
                            job = dr["jobNo"].ToString();

                            string err = mcData.CreateJobDeletionAudit(job, DateTime.MinValue, "n/a", "n/a", 0, "n/a", "cancelButton_Click from JobEntryForm.cs");
                        }
                    }
                }
                int num = mcData.DeleteParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                jobsDT.Clear();
                cancelJob = true;
                this.Dispose();
                this.Close();
            }
        }


        

        private void BuildDGV()
        {
            try
            {
                DataTable levelsDT = mcData.GetAllFloorLevels();
                BindingSource levelsBindngSource = new BindingSource();
                levelsBindngSource.DataSource = levelsDT;

                DataTable designersDT = mcData.GetAllDesigners();
                BindingSource designerBindngSource = new BindingSource();
                designerBindngSource.DataSource = designersDT;

                BindingSource suppTypeBindngSource = new BindingSource();
                suppTypeBindngSource.DataSource = mcData.GetSupplyTypeDT();

                jobDGV.Columns.Clear();
                
                //0
                DataGridViewCheckBoxColumn dgvCmbColumn = new DataGridViewCheckBoxColumn();
                dgvCmbColumn.ValueType = typeof(bool);
                dgvCmbColumn.Name = "Chk";
                dgvCmbColumn.Width = 30;
                dgvCmbColumn.HeaderText = "Tick";
                jobDGV.Columns.Add(dgvCmbColumn); 


                //1
                DataGridViewTextBoxColumn seqBoxColumn = new DataGridViewTextBoxColumn();
                seqBoxColumn.HeaderText = "No";
                seqBoxColumn.Width = 30;
                seqBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                seqBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(seqBoxColumn);

                //2
                DataGridViewTextBoxColumn jobNoBoxColumn = new DataGridViewTextBoxColumn();
                jobNoBoxColumn.HeaderText = "JobNo";
                jobNoBoxColumn.Width = 70;
                jobNoBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(jobNoBoxColumn);

                //3
                
                DataGridViewComboBoxColumn levelColumn = new DataGridViewComboBoxColumn();
                levelColumn.DataPropertyName = "Levels";
                levelColumn.HeaderText = "Floor Levels";
                levelColumn.Width = 150;
                levelColumn.DataSource = levelsBindngSource;
                levelColumn.ValueMember = "level";
                levelColumn.DisplayMember = "level";
                jobDGV.Columns.Add(levelColumn);
                
                //4
                DataGridViewTextBoxColumn reqDateTextBoxColumn = new DataGridViewTextBoxColumn();
                reqDateTextBoxColumn.HeaderText = "Req Date (Dbl Click)";
                reqDateTextBoxColumn.Width = 80;
                reqDateTextBoxColumn.DefaultCellStyle.Format = "dd/MM/YYYY";
                reqDateTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                reqDateTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(reqDateTextBoxColumn);

                //5
                DataGridViewTextBoxColumn designDateColumn = new DataGridViewTextBoxColumn();
                designDateColumn.HeaderText = "Design Date (Dbl Click)";
                designDateColumn.Width = 80;
                designDateColumn.DefaultCellStyle.Format = "dd/MM/YYYY";
                designDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                designDateColumn.ReadOnly = false;
                jobDGV.Columns.Add(designDateColumn);

                //6
                DataGridViewTextBoxColumn detailingDaysColumn = new DataGridViewTextBoxColumn();  //0
                detailingDaysColumn.HeaderText = "Detailing Days";
                detailingDaysColumn.Width = 50;
                detailingDaysColumn.ReadOnly = false;
                jobDGV.Columns.Add(detailingDaysColumn);

                //7
                DataGridViewComboBoxColumn designerColumn = new DataGridViewComboBoxColumn();
                designerColumn.DataPropertyName = "Designer";
                designerColumn.HeaderText = "Designer";
                designerColumn.Width = 90;
                designerColumn.DataSource = designerBindngSource;
                designerColumn.ValueMember = "designer";
                designerColumn.DisplayMember = "designer";
                jobDGV.Columns.Add(designerColumn);

                //8
                DataGridViewTextBoxColumn siteAddressTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                siteAddressTextBoxColumn.HeaderText = "Site Address";
                siteAddressTextBoxColumn.Width = 250;
                siteAddressTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(siteAddressTextBoxColumn);

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
                supplierTextBoxColumn.Width = 70;
                supplierTextBoxColumn.ReadOnly = false;
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
                DataGridViewTextBoxColumn commentTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                commentTextBoxColumn.HeaderText = "Comment";
                commentTextBoxColumn.Width = 200;
                commentTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(commentTextBoxColumn);

                //17
                DataGridViewTextBoxColumn phasedValueTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                phasedValueTextBoxColumn.HeaderText = "Phase Value(£)";
                phasedValueTextBoxColumn.Width = 80;
                phasedValueTextBoxColumn.ReadOnly = false;
                jobDGV.Columns.Add(phasedValueTextBoxColumn);

                //18
                DataGridViewTextBoxColumn jobMgnValueColumn = new DataGridViewTextBoxColumn();  //0
                jobMgnValueColumn.HeaderText = "Job Mgn Value(£)";
                jobMgnValueColumn.Width = 80;
                jobMgnValueColumn.ReadOnly = false;
                jobDGV.Columns.Add(jobMgnValueColumn);


                jobDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                jobDGV.EnableHeadersVisualStyles = false;
                jobDGV.Columns[2].DefaultCellStyle.BackColor = Color.Yellow;
                jobDGV.Columns[4].DefaultCellStyle.BackColor = Color.Yellow;
                jobDGV.Columns[5].DefaultCellStyle.BackColor = Color.Yellow;
                jobDGV.Columns[17].DefaultCellStyle.BackColor = Color.Cyan;
                jobDGV.Columns[18].DefaultCellStyle.BackColor = Color.Cyan;




            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobEntryForm.cs", "BuildDGV()", msg);
                return;
            }





        }

        private void jobDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)//set your checkbox column index instead of 2
                {
                    if (Convert.ToBoolean(jobDGV.Rows[e.RowIndex].Cells[0].EditedFormattedValue) == true)
                    {
                        jobDGV.Rows[e.RowIndex].Cells[1].Value = e.RowIndex > 8 ? (e.RowIndex + 1).ToString() : "0" + (e.RowIndex + 1).ToString();
                        jobDGV.Rows[e.RowIndex].Cells[2].Value = e.RowIndex > 8 ? parentJobNo.ToString() + "." + (e.RowIndex + 1).ToString() : parentJobNo.ToString() + "." + "0" + (e.RowIndex + 1).ToString();
                        jobDGV.Rows[e.RowIndex].Cells[8].Value = siteAddress;
                    }
                    else
                    {
                        jobDGV.Rows[e.RowIndex].Cells[1].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[2].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[3].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[4].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[5].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[6].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[7].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[8].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[9].Value = false;
                        jobDGV.Rows[e.RowIndex].Cells[10].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[11].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[12].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[13].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[14].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[15].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[16].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[17].Value = "";
                        jobDGV.Rows[e.RowIndex].Cells[18].Value = "";
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = String.Format("jobDGV_CellContentClick() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobEntryForm.cs", "jobDGV_CellContentClick()", msg);
                return;
            }
            
        }

        private void jobDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!jobDGV.Focused) { return; }

                if (jobDGV.Rows[0].Cells[2].Value == null)
                {
                    return;
                }

                if (String.IsNullOrWhiteSpace(jobDGV.Rows[0].Cells[2].Value.ToString()) || jobDGV.Rows[0].Cells[2].Value.ToString().Length != 8)
                {
                    return;
                }
                DateTime myDate;
                if (e.ColumnIndex == 4 || e.ColumnIndex == 5)//set your checkbox column index instead of 2
                {
                    if (jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null) { return; }
                    try
                    {
                        if (!DateTime.TryParse(jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out myDate))
                        {
                            MessageBox.Show("Invalid Date format. Please re-enter date");
                            jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                            jobDGV.CurrentCell = jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
                            jobDGV.CurrentCell.Selected = true;


                        }
                    }
                    catch (Exception err)
                    {
                        string msg = $"jobDGV_CellEndEdit() ColIndex[{e.ColumnIndex}] Error : {err.Message.ToString()}{Environment.NewLine} Please try again";
                        logger.LogLine(msg);
                        string audit = mcData.CreateErrorAudit("JobEntryForm.cs", $"jobDGV_CellEndEdit() ColIndex[{e.ColumnIndex}] Error", msg);
                        MessageBox.Show(msg, $"ColIndex[{e.ColumnIndex}] error");
                        return;
                    }
                    
                }

              //  if (!jobDGV.Focused) { return; }

                string jobNo = jobDGV[2, e.RowIndex].Value.ToString();

                if (e.ColumnIndex == 6)
                {
                   // if (jobDGV[0, e.RowIndex].Value == null) { return; }
                    DateTime designDate = Convert.ToDateTime(jobDGV[5, e.RowIndex].Value.ToString()).Date;
                    int dow = (int)designDate.DayOfWeek;
                    string inputStr = jobDGV[0, e.RowIndex].Value == null ? "" : jobDGV[e.ColumnIndex, e.RowIndex].Value.ToString();
                    int detailDayCount = Convert.ToInt16(inputStr);
                    int numRemaining = mcData.GetDesignDateRemainingDays(dow, detailDayCount);
                    jobDGV[e.ColumnIndex, e.RowIndex].Value = numRemaining;
                    
                }



                if (e.ColumnIndex == 11) //BeamLM
                {
                    int beamLM = jobDGV[e.ColumnIndex, e.RowIndex].Value == null ? 0 : Convert.ToInt32(jobDGV[e.ColumnIndex, e.RowIndex].Value.ToString());
                    decimal halvedBeamLM = beamLM * 0.5m;
                    int beamM2 = (int)Math.Round(halvedBeamLM, 0);
                    jobDGV[12, e.RowIndex].Value = beamM2;
                    
                }
            }
            catch (Exception ex)
            {
                string msg = String.Format("jobDGV_CellEndEdit() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobEntryForm.cs", "jobDGV_CellEndEdit()", msg);
                return;
            }

        }

        



        private void jobDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!jobDGV.Focused) { return; }

                if (jobDGV.Rows[0].Cells[2].Value == null)
                {
                    return;
                }

                if (String.IsNullOrWhiteSpace(jobDGV.Rows[0].Cells[2].Value.ToString()) || jobDGV.Rows[0].Cells[2].Value.ToString().Length != 8)
                {
                    return; 
                }

                if (e.ColumnIndex == 13)
                {
                    SuppliersListForm suppForm = new SuppliersListForm();
                    suppForm.ShowDialog();
                    jobDGV.Rows[e.RowIndex].Cells[13].Value = suppForm.Shortname;
                    jobDGV.CurrentCell = jobDGV.Rows[e.RowIndex].Cells[13];
                    jobDGV.CurrentCell.Selected = true;
                }

                if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
                {
                    DateSelectorForm dateForm = new DateSelectorForm();
                    dateForm.ShowDialog();
                    jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dateForm.RequiredDate.ToShortDateString();
                    jobDGV.CurrentCell = jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    jobDGV.CurrentCell.Selected = true;
                }

                
            }
            catch (Exception ex)
            {
                string msg = String.Format("jobDGV_CellDoubleClick() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobEntryForm.cs", "jobDGV_CellDoubleClick()", msg);
                return;
            }
            
        }

        private void createJobBtn_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (updatePhaseJobsOnly)
                {
                    string result = JobsDGVToDB(parentJobNo);
                    if (result != "OK")
                    {
                        string msg = "Errors creating the phased jobs have cancelled this operation : " + Environment.NewLine + Environment.NewLine + result;
                        MessageBox.Show(msg);
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Parent Job No.{0} jobs have been created successfully", parentJobNoTextBox.Text));
                        this.Cursor = Cursors.Default;
                        cancelJob = true;
                        this.Dispose();
                        this.Close();
                        return;
                    }
                }

                if (String.IsNullOrWhiteSpace(parentJobNoTextBox.Text) || parentJobNoTextBox.Text.Length < 5)
                {
                    MessageBox.Show("Invalid parent job number");
                    this.Cursor = Cursors.Default;
                    return;
                }

                if (mcData.IsParentJobExists(Convert.ToInt32(parentJobNoTextBox.Text)))
                {
                    MessageBox.Show($"Parent job number [{parentJobNoTextBox.Text}] already exists. Cannot continue");
                    this.Cursor = Cursors.Default;
                    return;
                }

                if (jobDGV.Rows.Count > 0)
                {
                    if (!String.IsNullOrWhiteSpace(jobDGV.Rows[0].Cells[2].Value.ToString()) && jobDGV.Rows[0].Cells[2].Value.ToString().Length == 8)
                    {
                        
                        string err = mcData.CreateParentJob(Convert.ToInt32(parentJobNoTextBox.Text), custCodeTextBox.Text, siteAddressTextBox.Text, siteContactTextBox.Text, contactTelTextBox.Text, contactEmailTextBox.Text);
                        if (err != "OK")
                        {
                            MessageBox.Show("ERROR Creating Parent Job : " + err);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else
                        {
                            if (!ValidateDGV())
                            {
                                MessageBox.Show("ERROR Missing job fields - Floor Level, Site Address and Supply Type (SO / SF) must be specified for a job");
                                this.Cursor = Cursors.Default;
                                int numDeleted = mcData.DeleteParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                                return;
                            }
                            string result = JobsDGVToDB();
                            if (result != "OK")
                            {
                                string msg = "Errors creating the phased jobs have cancelled this operation : " + Environment.NewLine + Environment.NewLine + result;
                                MessageBox.Show(msg);
                                this.Cursor = Cursors.Default;
                                return;
                            }
                            else
                            {
                                MessageBox.Show(String.Format("Parent Job No.{0} jobs have been created successfully", parentJobNoTextBox.Text));
                                this.Cursor = Cursors.Default;
                                cancelJob = true;
                                JobPlannerForm jobForm = new JobPlannerForm(Convert.ToInt32(parentJobNoTextBox.Text));

                                jobForm.ShowDialog();
                                this.Dispose();
                                this.Close();
                                return;
                            }

                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid job number(s). Cannot create jobs");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("createJobBtn_Click() Error : {0}", ex.Message.ToString()) + Environment.NewLine + Environment.NewLine +
                            ex.InnerException.ToString();
                logger.LogLine(msg);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobEntryForm.cs", "createJobBtn_Click()", msg);
                this.Cursor = Cursors.Default;
                return;
            }
            
        }

        private bool ValidateDGV()
        {
            bool result = false;
            string supplyType,floorLevel,siteAddr = "";
            bool failFlag = false;


            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[2].Value == null || jobDGV.Rows[i].Cells[2].Value.ToString().Length != 8)
                    {
                        break;
                    }
                    
                    floorLevel = jobDGV.Rows[i].Cells[3].Value == null ? "" : jobDGV.Rows[i].Cells[3].Value.ToString();
                    siteAddr = jobDGV.Rows[i].Cells[8].Value == null ? "" : jobDGV.Rows[i].Cells[8].Value.ToString();
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
                String msg = String.Format("ValidateDGV() ERROR - {0}", ex.Message);
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("JobEntryForm.cs", "ValidateDGV()", msg);
                return false;
            }
        }

        private string JobsDGVToDB()
        {
            int parentJobNo;
            string jobNo;
            string phaseNo;
            string floorLevel;
            DateTime requiredDate;
            int detailingDays;
            string dman;
            DateTime designDate;
            string siteAddress;
            string drawn;
            string approved;
            string OnShop;
            string stairsIncl;
            int slabM2;
            int beamM2;
            int beamLm;
            string supplyType;
            string sortType;
            string shortname;
            string supplierRef;
            string lastComment;
            decimal phaseInvValue;
            decimal jobMgnValue;
            bool errFound = false;
            string err = "";
            string wbErr = "";
            string dbErr = "";

            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[2].Value == null || jobDGV.Rows[i].Cells[2].Value.ToString().Length != 8)
                    {
                        break;
                    }
                    parentJobNo = Convert.ToInt32(parentJobNoTextBox.Text);
                    jobNo = jobDGV.Rows[i].Cells[2].Value.ToString();
                    phaseNo = jobDGV.Rows[i].Cells[1].Value.ToString();
                    floorLevel = jobDGV.Rows[i].Cells[3].Value == null ? "" : jobDGV.Rows[i].Cells[3].Value.ToString();
                    requiredDate = jobDGV.Rows[i].Cells[4].Value == null ? DateTime.Now.AddYears(10) : Convert.ToDateTime(jobDGV.Rows[i].Cells[4].Value);
                    designDate = jobDGV.Rows[i].Cells[5].Value == null ? DateTime.Now.AddYears(10) : Convert.ToDateTime(jobDGV.Rows[i].Cells[5].Value);
                    detailingDays = jobDGV.Rows[i].Cells[6].Value == null ? 0 : Convert.ToInt32(jobDGV.Rows[i].Cells[6].Value.ToString());
                    dman = jobDGV.Rows[i].Cells[7].Value == null ? "" : jobDGV.Rows[i].Cells[7].Value.ToString();
                    siteAddress = jobDGV.Rows[i].Cells[8].Value == null ? "" : jobDGV.Rows[i].Cells[8].Value.ToString();

                    stairsIncl = jobDGV.Rows[i].Cells[9].Value == null ? "N" : "Y";
                    slabM2 = jobDGV.Rows[i].Cells[10].Value == null ? 0 : Convert.ToInt32(jobDGV.Rows[i].Cells[10].Value.ToString());
                    beamLm = jobDGV.Rows[i].Cells[11].Value == null ? 0 : Convert.ToInt32(jobDGV.Rows[i].Cells[11].Value.ToString());
                    beamM2 = jobDGV.Rows[i].Cells[12].Value == null ? 0 : Convert.ToInt32(jobDGV.Rows[i].Cells[12].Value.ToString());
                    shortname = jobDGV.Rows[i].Cells[13].Value == null ? "" : jobDGV.Rows[i].Cells[13].Value.ToString();
                    supplyType = jobDGV.Rows[i].Cells[14].Value == null ? "" : jobDGV.Rows[i].Cells[14].Value.ToString();
                    supplierRef = jobDGV.Rows[i].Cells[15].Value == null ? "" : jobDGV.Rows[i].Cells[15].Value.ToString();
                    lastComment = jobDGV.Rows[i].Cells[16].Value == null ? "" : jobDGV.Rows[i].Cells[16].Value.ToString();
                    phaseInvValue = jobDGV.Rows[i].Cells[17].Value == null ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[17].Value.ToString());
                    jobMgnValue = jobDGV.Rows[i].Cells[18].Value == null ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[18].Value.ToString());
                    sortType = "S" + supplyType.Substring(1, 1);

                    if (!mcData.IsJobExists(jobNo))
                    {
                        err = mcData.CreateJobPlanner(parentJobNo, jobNo, phaseNo, floorLevel, requiredDate, designDate, siteAddress, "N", "N", stairsIncl, slabM2, beamM2, beamLm, shortname, supplyType, supplierRef, lastComment, phaseInvValue, jobMgnValue, sortType,dman);
                        if (err != "OK")
                        {
                            MessageBox.Show(String.Format("CreateJobPlanner ERROR ( Job {0} ) : {1}", jobNo, err));
                            errFound = true;
                            break;
                        }
                    }

                    if (!mcData.IsWhiteboardJobExists(jobNo))
                    {
                        wbErr = mcData.CreateWhiteBoard(jobNo, requiredDate, custCode, siteAddress, supplyType, "", beamM2 + slabM2, 0, phaseInvValue, "N", shortname, stairsIncl, "", floorLevel, "", "N", "", "", "", "", "", "", "", "",
                                                            "", "N", "N", "N", "", "", "", "N", "", "", "", "N", "N", "", "", "N", dman, "",lastComment, sortType);

                        if (wbErr != "OK")
                        {
                            MessageBox.Show(String.Format("CreateWhiteBoard ERROR ( Job {0} ) : {1}", jobNo, wbErr));
                            errFound = true;
                            break;
                        }
                        else
                        {
                            //string auditErr = mcData.CreateJobDayAudit(jobNo, requiredDate, $"CreateWhiteBoard(....{requiredDate.ToShortDateString()}......)");
                        }
                    }

                    if (!mcData.IsDesignBoardJobExists(jobNo))
                    {
                        dbErr = mcData.CreateDesignBoardJob(jobNo, designDate, "NOT DRAWN", requiredDate, detailingDays, dman, floorLevel, shortname, supplierRef, supplyType, slabM2, beamM2, beamLm, sortType);
                        if (dbErr != "OK")
                        {
                            MessageBox.Show(String.Format("CreateDesignBoardJob ERROR ( Job {0} ) : {1}", jobNo, dbErr));
                            errFound = true;
                            break;
                        }
                        mcData.UpdateDesignBoardColourCodeDayFlags(jobNo, designDate, detailingDays, (int)designDate.DayOfWeek);
                    }


                }


                if (errFound)
                {
                    int dbNum = mcData.DeleteDesignBoardByParentJob(parentJobNoTextBox.Text);
                    int numDeleted = mcData.DeleteJobPlannerByParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                    int wbNum = mcData.DeleteWhiteboardByParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                    int num = mcData.DeleteParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                    
                    MessageBox.Show($"[{parentJobNoTextBox.Text}] Jobs could NOT be created due to errors. This job creation is cancelled");
                    cancelJob = true;
                    this.Dispose();
                    this.Close();

                    string errorMessage = "JOB PLANNER Error : " + Environment.NewLine + err + Environment.NewLine + Environment.NewLine +
                                            "WHITEBOARD error : " + Environment.NewLine + wbErr;

                    return errorMessage;
                }


                return "OK";
            }
            catch (Exception ex)
            {
                string msg = String.Format("JobsDGVToDB() Error : {0}", ex.Message.ToString()) + Environment.NewLine + 
                    Environment.NewLine + 
                    "JOB PLANNER Error : " + Environment.NewLine + err + Environment.NewLine + Environment.NewLine +
                                            "WHITEBOARD error : " + Environment.NewLine + wbErr;
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobEntryForm.cs", "JobsDGVToDB()", msg);
                return msg;
            }
            
        }

        private string JobsDGVToDB(int parentJob)
        {
            int parentJobNo;
            string jobNo;
            string phaseNo;
            string floorLevel;
            DateTime requiredDate;
            DateTime designDate;
            int detailingDays;
            string dman;
            string siteAddress;
            string drawn;
            string approved;
            string OnShop;
            string stairsIncl;
            int slabM2;
            int beamM2;
            int beamLm;
            string supplyType;
            string sortType;
            string shortname;
            string supplierRef;
            string lastComment;
            decimal phaseInvValue;
            decimal jobMgnValue;
            bool errFound = false;
            string err = "";
            string wbErr = "";
            string dbErr = "";
            try
            {
                for (int i = 0; i < jobDGV.Rows.Count; i++)
                {
                    if (jobDGV.Rows[i].Cells[2].Value == null || jobDGV.Rows[i].Cells[2].Value.ToString().Length != 8)
                    {
                        break;
                    }
                    parentJobNo = parentJob;
                    jobNo = jobDGV.Rows[i].Cells[2].Value.ToString();
                    phaseNo = jobDGV.Rows[i].Cells[1].Value.ToString();
                    floorLevel = jobDGV.Rows[i].Cells[3].Value == null ? "" : jobDGV.Rows[i].Cells[3].Value.ToString();
                    requiredDate = jobDGV.Rows[i].Cells[4].Value == null ? DateTime.Now.AddYears(10) : Convert.ToDateTime(jobDGV.Rows[i].Cells[4].Value);
                    designDate = jobDGV.Rows[i].Cells[5].Value == null ? DateTime.Now.AddYears(10) : Convert.ToDateTime(jobDGV.Rows[i].Cells[5].Value);
                    detailingDays = jobDGV.Rows[i].Cells[6].Value == null ? 0 : Convert.ToInt32(jobDGV.Rows[i].Cells[6].Value.ToString());
                    dman = jobDGV.Rows[i].Cells[7].Value == null ? "" : jobDGV.Rows[i].Cells[7].Value.ToString();
                    siteAddress = jobDGV.Rows[i].Cells[8].Value == null ? "" : jobDGV.Rows[i].Cells[8].Value.ToString();
      
                    stairsIncl = jobDGV.Rows[i].Cells[9].Value == null ? "N" : "Y";
                    slabM2 = jobDGV.Rows[i].Cells[10].Value == null ? 0 : Convert.ToInt32(jobDGV.Rows[i].Cells[10].Value.ToString());
                    beamLm = jobDGV.Rows[i].Cells[11].Value == null ? 0 : Convert.ToInt32(jobDGV.Rows[i].Cells[11].Value.ToString());
                    beamM2 = jobDGV.Rows[i].Cells[12].Value == null ? 0 : Convert.ToInt32(jobDGV.Rows[i].Cells[12].Value.ToString());
                    shortname = jobDGV.Rows[i].Cells[13].Value == null ? "" : jobDGV.Rows[i].Cells[13].Value.ToString();
                    supplyType = jobDGV.Rows[i].Cells[14].Value == null ? "" : jobDGV.Rows[i].Cells[14].Value.ToString();
                    supplierRef = jobDGV.Rows[i].Cells[15].Value == null ? "" : jobDGV.Rows[i].Cells[15].Value.ToString();
                    lastComment = jobDGV.Rows[i].Cells[16].Value == null ? "" : jobDGV.Rows[i].Cells[16].Value.ToString();
                    phaseInvValue = jobDGV.Rows[i].Cells[17].Value == null ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[17].Value.ToString());
                    jobMgnValue = jobDGV.Rows[i].Cells[18].Value == null ? 0 : Convert.ToDecimal(jobDGV.Rows[i].Cells[18].Value.ToString());
                    sortType = "S" + supplyType.Substring(1, 1);

                    if (!mcData.IsJobExists(jobNo))
                    {
                        err = mcData.CreateJobPlanner(parentJobNo, jobNo, phaseNo, floorLevel, requiredDate, designDate, siteAddress, "N", "N", stairsIncl, slabM2, beamM2, beamLm, shortname, supplyType, supplierRef, lastComment, phaseInvValue, jobMgnValue, sortType,dman);
                        if (err != "OK")
                        {
                            MessageBox.Show(String.Format("CreateJobPlanner ERROR ( Job {0} ) : {1}", jobNo, err));
                            errFound = true;
                            break;
                        }
                    }

                    if (!mcData.IsWhiteboardJobExists(jobNo))
                    {
                        wbErr = mcData.CreateWhiteBoard(jobNo, requiredDate, custCode, siteAddress, supplyType, "", beamM2 + slabM2, 0, phaseInvValue, "N", shortname, stairsIncl, "", floorLevel, "", "N", "", "", "", "", "", "", "", "",
                                                                "", "N", "N", "N", "", "", "", "N", "", "", "", "N", "N", "", "", "N", dman,"", lastComment, sortType);
                        if (wbErr != "OK")
                        {
                            MessageBox.Show(String.Format("CreateWhiteBoard ERROR ( Job {0} ) : {1}", jobNo, wbErr));
                            errFound = true;
                            break;
                        }
                    }

                    if(!mcData.IsDesignBoardJobExists(jobNo))
                    {
                        dbErr = mcData.CreateDesignBoardJob(jobNo, designDate, "NOT DRAWN", requiredDate, detailingDays,dman, floorLevel, shortname, supplierRef, supplyType, slabM2, beamM2, beamLm, sortType);
                        if (dbErr != "OK")
                        {
                            MessageBox.Show(String.Format("CreateDesignBoardJob ERROR ( Job {0} ) : {1}", jobNo, dbErr));
                            errFound = true;
                            break;
                        }
                    }
                }


                if (errFound)
                {
                    int numDeleted = mcData.DeleteJobPlannerByParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                    int wbNum = mcData.DeleteWhiteboardByParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                    int num = mcData.DeleteParentJob(Convert.ToInt32(parentJobNoTextBox.Text));
                    MessageBox.Show(String.Format("[{0}] Jobs could NOT be created due to errors. This job creation is cancelled", parentJobNoTextBox.Text));
                    cancelJob = true;
                    this.Dispose();
                    this.Close();
                    string errorMessage = "JOB PLANNER Error : " + Environment.NewLine + err + Environment.NewLine + Environment.NewLine +
                                            "WHITEBOARD error : " + Environment.NewLine + wbErr;

                    return errorMessage;
                }


                return "OK";
            }
            catch (Exception ex)
            {
                string msg = String.Format("JobsDGVToDB() Error : {0}", ex.Message.ToString()) + Environment.NewLine +
                    Environment.NewLine +
                    "JOB PLANNER Error : " + Environment.NewLine + err + Environment.NewLine + Environment.NewLine +
                                            "WHITEBOARD error : " + Environment.NewLine + wbErr;
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobEntryForm.cs", String.Format("JobsDGVToDB({0}", parentJob.ToString()), msg);
                return msg;
            }

        }

        private void jobDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!jobDGV.Focused) { return; }

            if (jobDGV.Rows[0].Cells[2].Value == null)
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(jobDGV.Rows[0].Cells[2].Value.ToString()) || jobDGV.Rows[0].Cells[2].Value.ToString().Length != 8)
            {
                return;
            }
        }

        private void completeJobToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void jobDGV_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1 & e.ColumnIndex < 4)
            {
                this.jobDGV.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.jobDGV.CurrentCell = this.jobDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
                this.contextMenuStrip1.Show(this.jobDGV, e.Location);
                contextMenuStrip1.Show(Cursor.Position);


            }
        }

        private void jobDGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (jobDGV.CurrentCell.ColumnIndex == 10 || jobDGV.CurrentCell.ColumnIndex == 11 || jobDGV.CurrentCell.ColumnIndex == 12 || jobDGV.CurrentCell.ColumnIndex == 17 || jobDGV.CurrentCell.ColumnIndex == 18) //Desired Column
            {
                System.Windows.Forms.TextBox tb = e.Control as System.Windows.Forms.TextBox;
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
                && e.KeyChar == '.') // only allows integers and no decimals
            {
                e.Handled = true; 
            }
        }

        private void jobDGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Don't throw an exception when we're done.
            e.ThrowException = false;

            // Display an error message.
            string txt = "Error with " +
                jobDGV.Columns[e.ColumnIndex].HeaderText +
                "\n\n" + e.Exception.Message;
            MessageBox.Show(txt, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            // If this is true, then the user is trapped in this cell.
            e.Cancel = false;
        }
    }
}
