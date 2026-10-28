using bzer_data_entry.src;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using PhoneticBengaliInput;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Windows.Forms;

namespace bzer_data_entry
{
    public partial class frmBatch : Form
    {
        protected int mode;
        MemoryStream stateLog;
        private udtBatch objBatch;
        private OdbcDataAdapter sqlAdap = null;
        private DataSet dsPath = null;
        private wfeProject objProj = null;
        private INIReader rd = null;
        private KeyValueStruct udtKeyValue;
        public string err = null;
        private int projCode;
        wfeBatch crtBatch = null;
        OdbcConnection sqlCon = null;
        byte[] tmpWrite;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);

        string name = string.Empty;

        DataLayerDefs.Mode _mode = DataLayerDefs.Mode._Edit;

        public string currentDate;
        public string handoverDate;
        public string inwardDate;

        string old_path;

        private MetadataStore Store;
        private List<MasterItem> Districts;
        private List<MasterItem> Blocks;
        private List<MasterItem> Mouzas;

        private DataEntryNavigationController NavigationController;


        public frmBatch(wItem prmCmd, OdbcConnection prmCon, DataLayerDefs.Mode mode)
        {
            AvroBengaliPhoneticParser.Parser.Register();
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            //this.Icon = 
            exMailLog.SetNextLogger(exTxtLog);
            _mode = mode;
            crtBatch = (wfeBatch)prmCmd;
            sqlCon = prmCon;
            if (crtBatch.GetMode() == Constants._ADDING)
                this.Text = "B'Zer -  (Add Bundle)";
            else
                this.Text = "B'Zer -  (Edit Bundle)";
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            Store = new MetadataStore(sqlCon);
            Districts = new List<MasterItem>();
            Blocks = new List<MasterItem>();
            Mouzas = new List<MasterItem>();

            InitNavigationController();
        }

        private void InitNavigationController()
        {
            var controllerBuilder = new DataEntryNavigationController.Builder(this);
            foreach (var controlIndexPair in new Control[] {
                pbiComboBoxDistrict,
                textBoxBundleNumber,
                txtCreateDate,
                dtHandover,
                dtInwarddate,
                buttonGenerateBundleCode,
                buttonSaveBundle
            }.Select((control, index) => (control, index)))
            {
                controllerBuilder.AddControl(controlIndexPair.index, controlIndexPair.control.Name, controlIndexPair.control);
            }
            NavigationController = controllerBuilder.Ready(enable: true);
        }


        private void frmBatch_Load(object sender, EventArgs e)
        {

            try
            {
                cmbFinancialYear.Items.Clear();

                for (int year = 1947; year <= 2025; year++)
                {
                    cmbFinancialYear.Items.Add($"{year}_{year + 1}");
                }

                //cmbFinancialYear.SelectedIndex = cmbFinancialYear.Items.Count - 1;
                {
                    var rbac = RBAC.GetInstance();
                    this.name = rbac.getCredentials(rbac.getProfile()).created_by;
                }
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            if (_mode == DataLayerDefs.Mode._Add)
            {
                populateProject();
                buttonSaveBundle.Enabled = false;

                currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                handoverDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                inwardDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                this.Text = "B'Zer -  (Edit Bundle)";
                groupBox3.Enabled = true;
                populateProject();

                cmbProject.Text = getProjectName(frmEntrySummary.projKey).Rows[0][0].ToString();
                groupBox2.Enabled = false;

                textBoxBundleNumber.Text = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][3].ToString();
                //txtCreateDate.Text = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][4].ToString();
                //dateTimePicker1.Text = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][5].ToString();

                textBox3.Text = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][0].ToString();
                textBox4.Text = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][1].ToString();

                currentDate = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][4].ToString();
                //handoverDate = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][5].ToString();

                old_path = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][6].ToString();
                inwardDate = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][7].ToString();

                txtCreateDate.Text = currentDate;
               // txtHandoverDate.Text = handoverDate;
                //dtHandover.Text = handoverDate;
                //dtHandover.Format = DateTimePickerFormat.Custom;
                //dtHandover.CustomFormat = "yyyy-MM-dd";
                //dtHandover.Value = Convert.ToDateTime(handoverDate.ToString());
                //dtHandover.Enabled = true;

                textBoxBundleNumber.Focus();
                textBoxBundleNumber.Select();

                buttonGenerateBundleCode.Enabled = true;
                buttonSaveBundle.Enabled = false;
            }
            LoadDistricts();
        }

        private void LoadDistricts()
        {
            Districts = Store.GetMasterList(null, new List<string> { MetadataStore.FIELD_DISTRICT });
            var districtNameList = Districts.Select(data => (LanguageItem)data.Value);
            pbiComboBoxDistrict.LanguageItems.Set(districtNameList);
        }

        private void pbiComboBoxDistrict_InnerSelectedIndexChanged(object sender, EventArgs e)
        {
            //ClearComboBoxItems(pbiComboBoxBlock);
            //ClearComboBoxItems(pbiComboBoxMouza);
            //pbiComboBoxMouza.LanguageItems.Set(new List<LanguageItem>());
           // LoadBlocks(Districts[pbiComboBoxDistrict.SelectedIndex].MasterID);

            NavigationController.ControlEntryCompleted(pbiComboBoxDistrict.Name);
        }

        private void LoadBlocks(Guid districtId)
        {
            Blocks = Store.GetMasterList(districtId, new List<string> { MetadataStore.FIELD_BLOCK });
            var blockNameList = Blocks.Select(data => (LanguageItem)data.Value);
            pbiComboBoxBlock.LanguageItems.Set(blockNameList);
        }

        private void pbiComboBoxBlock_InnerSelectedTextChanged(object sender, EventArgs e)
        {
            ClearComboBoxItems(pbiComboBoxMouza);
            LoadMouzas(Blocks[pbiComboBoxBlock.SelectedIndex].MasterID);

            NavigationController.ControlEntryCompleted(pbiComboBoxBlock.Name);
        }

        private void LoadMouzas(Guid blockId)
        {
            //Mouzas = Store.GetMasterList(blockId, new List<string> { MetadataStore.FIELD_MOUZA });
            //var mouzaNameList = Mouzas.Select(data => (LanguageItem)data.Value);
            //pbiComboBoxMouza.LanguageItems.Set(mouzaNameList);
        }

        private void pbiComboBoxMouza_InnerSelectedIndexChanged(object sender, EventArgs e)
        {
            NavigationController.ControlEntryCompleted(pbiComboBoxMouza.Name);
        }
        private void ClearComboBoxItems(PBIComboBox comboBox)
        {
            comboBox.Value.BengaliText = string.Empty;
            comboBox.BengaliPhoneticText = string.Empty;
        }
        private void populateProject()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select proj_key, proj_code from project_master ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbProject.DataSource = dt;
                cmbProject.DisplayMember = "proj_code";
                cmbProject.ValueMember = "proj_key";
            }
            else
            {
                MessageBox.Show("Add one project first...");
            }

        }

        public DataTable getProjectName(string pcode)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_key,proj_code from project_master where proj_key = '" + pcode + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public DataTable getBundleDetails(string pcode, string bcode)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct bundle_code,bundle_name,establishment,bundle_no,date_format(creation_date,'%Y-%m-%d'),date_format(handover_date,'%Y-%m-%d'),bundle_path,inward_date from bundle_master where proj_code = '" + pcode + "' and bundle_key = '" + bcode + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void ClearAllField()
        {
            textBoxBundleNumber.Text = string.Empty;
            txtCreateDate.Text = string.Empty;
            txtHandoverDate.Text = string.Empty;
            dtHandover.Format = DateTimePickerFormat.Custom;
            dtHandover.CustomFormat = " ";
            //dateTimePicker1.Value = Convert.ToDateTime("");
            dtInwarddate.Format = DateTimePickerFormat.Custom;
            dtInwarddate.CustomFormat = " ";
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            pbiComboBoxDistrict.SelectedIndex = -1;
            pbiComboBoxBlock.SelectedIndex = -1;
            pbiComboBoxMouza.SelectedIndex = -1;
            buttonSaveBundle.Enabled = false;
            cmbProject.Focus();
        }

        private void cmbProject_Leave_1(object sender, EventArgs e)
        {

            if (cmbProject.Text == "" || cmbProject.Text == null)
            {
                MessageBox.Show("Please select a project name");
                cmbProject.Focus();
                cmbProject.Select();
            }
            else
            {

                groupBox3.Enabled = true;

                txtCreateDate.Text = currentDate;
                txtHandoverDate.Text = handoverDate;
                //dtHandover.Text = handoverDate;
                //dtHandover.Format = DateTimePickerFormat.Custom;
                //dtHandover.CustomFormat = "yyyy-MM-dd";
                //dtHandover.Value = Convert.ToDateTime(handoverDate.ToString());

                txtInwardDate.Text = inwardDate;
                dtInwarddate.Text = inwardDate;
                dtInwarddate.Format = DateTimePickerFormat.Custom;
                dtInwarddate.CustomFormat = "yyyy-MM-dd";
                dtInwarddate.Value = Convert.ToDateTime(inwardDate.ToString());
            }
        }

        private void cmbProject_MouseLeave(object sender, EventArgs e)
        {

            if (cmbProject.Text == "" || cmbProject.Text == null)
            {
                MessageBox.Show("Please select a project name");
                cmbProject.Focus();
                cmbProject.Select();
            }
            else
            {
                groupBox3.Enabled = true;

                txtCreateDate.Text = currentDate;
                //txtHandoverDate.Text = handoverDate;
                //dtHandover.Text = handoverDate;
                //dtHandover.Format = DateTimePickerFormat.Custom;
                //dtHandover.CustomFormat = "yyyy-MM-dd";
                //dtHandover.Value = Convert.ToDateTime(handoverDate.ToString());

                txtInwardDate.Text = inwardDate;
                dtInwarddate.Text = inwardDate;
                dtInwarddate.Format = DateTimePickerFormat.Custom;
                dtInwarddate.CustomFormat = "yyyy-MM-dd";
                dtInwarddate.Value = Convert.ToDateTime(inwardDate.ToString());
            }
        }

        private void buttonGenerateBundleCode_Click(object sender, EventArgs e)
        {
            if (_mode == DataLayerDefs.Mode._Add)
            {
                DateTime temp;
                //string isDate = dtHandover.Text;
                string inDate = dtInwarddate.Text;
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                //if (DateTime.TryParse(isDate, out temp))
                //{
                if (textBoxBundleNumber.Text == "" || textBoxBundleNumber.Text == null)
                {
                    MessageBox.Show("You cannot left Case Number field blank....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxBundleNumber.Focus();
                    textBoxBundleNumber.Select();
                    return;
                }
                if (textBoxBundleNumber.Text != "" || textBoxBundleNumber.Text != null)
                {
                    try
                    {
                        Guid GetSelectedId(PBIComboBox pBIComboBox, List<MasterItem> items, string fieldName)
                        {
                            if (pBIComboBox.SelectedIndex == -1)
                            {
                                throw new DomainError($"{fieldName} doesn't correspond to any valid value from the master data");
                            }
                            return items[pBIComboBox.SelectedIndex].MasterID;
                        }

                        Guid selectedDistrictId = GetSelectedId(pbiComboBoxDistrict, Districts, MetadataStore.FIELD_DISTRICT);


                        try
                        {
                            var store = new BatchDataStore(sqlCon);
                            //var batchInfo = store.GetBatchKeyData(selectedMouzaId);

                            //foreach (var batchDetail in batchInfo)
                            //{
                            //    if (batchDetail.BundleNo == textBoxBundleNumber.Text)
                            //    {
                            //        throw new Exception($"Duplicate Volume Entry for mouza : {pbiComboBoxMouza.BengaliPhoneticText}");
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            NavigationController.ControlError(textBoxBundleNumber.Name);
                            return;
                        }
                        if(cmbFinancialYear.Text == "")
                        {
                            MessageBox.Show(this,"PLease Select Year","",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                            cmbFinancialYear.Focus();
                            return;
                        }

                        string volumeName = textBoxBundleNumber.Text.ToUpper();
                        string batchName = Store.GenerateBatchName(selectedDistrictId, volumeName,cmbFinancialYear.Text);
                        textBox3.Text = batchName;
                        textBox4.Text = batchName;
                        buttonSaveBundle.Enabled = true;

                        NavigationController.ControlEntryCompleted(buttonGenerateBundleCode.Name);
                    }
                    catch (DomainError ex)
                    {
                        MessageBox.Show(ex.Message, "Error: Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        
            
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                DateTime temp;
                string isDate = dtHandover.Text;
                string inDate = dtInwarddate.Text;
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (DateTime.TryParse(isDate, out temp) && (DateTime.Parse(isDate) > DateTime.Parse(currDate) || DateTime.Parse(isDate) <= DateTime.Parse(currDate)) && DateTime.Parse(isDate) > DateTime.Parse(currentDate))
                {
                    if (textBoxBundleNumber.Text == "" || textBoxBundleNumber.Text == null)
                    {
                        MessageBox.Show("You cannot left Bundle Number field blank....", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxBundleNumber.Focus();
                        textBoxBundleNumber.Select();
                        return;
                    }
                    if (textBoxBundleNumber.Text != "" || textBoxBundleNumber.Text != null)
                    {

                        string bundleCount = getBundleDetails(frmEntrySummary.projKey, frmEntrySummary.bundleKey).Rows[0][0].ToString();



                        string bundleCode = bundleCount.Substring(0, 8) + "_" + textBoxBundleNumber.Text.ToUpper();

                        textBox3.Text = bundleCode;
                        textBox4.Text = bundleCode;

                        buttonSaveBundle.Enabled = true;


                    }
                }
                else if (!DateTime.TryParse(inDate, out temp) && !(DateTime.Parse(inDate) > DateTime.Parse(currDate) || DateTime.Parse(inDate) <= DateTime.Parse(currDate)) && !(DateTime.Parse(inDate) > DateTime.Parse(currentDate)))
                {
                    //retval = false;
                    MessageBox.Show("Please select a valid Inward Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    dtInwarddate.Select();
                    //validateBol = false;
                }
                else
                {
                    //retval = false;
                    MessageBox.Show("Please select a valid Handover Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                   // dtHandover.Select();
                    //validateBol = false;

                }
            }

        }

        private string getBundleCount()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select Count(*) from bundle_master";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            int count = Convert.ToInt32(dt.Rows[0][0].ToString());

            string getCount = Convert.ToString(count + 1);

            return getCount;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed || sqlCon.State == ConnectionState.Broken)
            {
                sqlCon.Open();
            }
            this.Close();
        }
        public bool KeyCheck(string prmValue, OdbcTransaction sqlTran = null)
        {
            string sqlStr = null;
            OdbcCommand cmd = null;
            bool existsBol = true;

            sqlStr = "select bundle_code from bundle_master where bundle_code='" + prmValue.ToUpper() + "'";
            cmd = new OdbcCommand(sqlStr, sqlCon);
            cmd.Transaction = sqlTran;
            existsBol = cmd.ExecuteReader().HasRows;

            return existsBol;
        }
        private bool Validate(udtBatch cmd)
        {
            bool validateBol = true;
            //errList = new Hashtable();
            if (cmd.batch_code == string.Empty || KeyCheck(cmd.batch_code) == true)
            {
                validateBol = false;
                //errList.Add("Code", Constants.NOT_VALID);
            }

            if (cmd.batch_name == string.Empty)
            {
                validateBol = false;
                //errList.Add("Name", Constants.NOT_VALID);
            }

            if (cmd.Created_By == string.Empty && mode == Constants._ADDING)
            {
                validateBol = false;
                //errList.Add("Created_By", Constants.NOT_VALID);
            }

            if (cmd.Created_DTTM == string.Empty && mode == Constants._ADDING)
            {
                validateBol = false;
                // errList.Add("Created_DTTM", Constants.NOT_VALID);
            }

            ///Required at the time of editing
            if (cmd.Modified_By == string.Empty && mode == Constants._EDITING)
            {
                validateBol = false;
                //errList.Add("Modified_By", Constants.NOT_VALID);
            }

            if (cmd.Modified_DTTM == string.Empty && mode == Constants._EDITING)
            {
                validateBol = false;
                //errList.Add("Modified_DTTM", Constants.NOT_VALID);
            }

            if (_mode == DataLayerDefs.Mode._Add)
            {
                DateTime temp;
                string isDate = dtHandover.Text;
                string inDate = dtInwarddate.Text;
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                //if (DateTime.TryParse(isDate, out temp))
                //{
                    if (DateTime.TryParse(inDate, out temp))
                    {
                        validateBol = true;
                    }
                    else
                    {
                        //retval = false;
                        MessageBox.Show("Please select a valid Inward Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        dtInwarddate.Select();
                        validateBol = false;

                    }
                //}
                //else
                //{
                //    //retval = false;
                //    //MessageBox.Show("Please select a valid Handover Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //    //dtHandover.Select();
                //    //validateBol = false;

                //}
            }
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                DateTime temp;
                string isDate = dtHandover.Text;
                string inDate = dtHandover.Text;
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                //if (DateTime.TryParse(isDate, out temp))
                //{
                //    if (DateTime.TryParse(inDate, out temp))
                //    {
                //        validateBol = true;
                //    }
                //    else
                //    {
                //        //retval = false;
                //        MessageBox.Show("Please select a valid Inward Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //        dtInwarddate.Select();
                //        validateBol = false;

                //    }
                //}
                //else
                //{
                //    //retval = false;
                //    MessageBox.Show("Please select a valid Handover Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //    dtHandover.Select();
                //    validateBol = false;

                //}
            }


            return validateBol;
        }

        public int? Commit_Bundle_Edit(string bundle_no, string createDt, string handoverDt, string inwardDt, OdbcTransaction sqlTrans)
        {
            string sqlStr = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            string scanbatchPath = null;

            //errList = new Hashtable();
            objProj = new wfeProject(sqlCon, sqlTrans);

            dsPath = objProj.GetPath(objBatch.proj_code);

            if (dsPath.Tables[0].Rows.Count > 0)
            {
                scanbatchPath = dsPath.Tables[0].Rows[0]["project_Path"] + "\\" + objBatch.batch_code;
            }

            sqlStr = @"update bundle_master set bundle_code = '" + objBatch.batch_code.ToUpper() + "', bundle_name = '" + objBatch.batch_name + "',establishment='NA',bundle_no='" + bundle_no + "',creation_date='" + createDt + "',handover_date='" + handoverDt + "',bundle_path='" + scanbatchPath.Replace("\\", "\\\\") + "',inward_date = '" + inwardDt + "',modified_by = '" + objBatch.Created_By + "',modified_dttm = '" + objBatch.Created_DTTM + "' where proj_code = '" + frmEntrySummary.projKey + "' and bundle_key = '" + frmEntrySummary.bundleKey + "'";

            if (KeyCheck(objBatch.batch_code, sqlTrans) == false)
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();

                if (mode == Constants._ADDING)
                {
                    if (FileorFolder.RenameFolder(old_path, scanbatchPath) == true)
                    {
                        commitBol = true;
                    }
                    else
                    {
                        commitBol = false;
                        throw new CreateFolderException($"Could not create folder {scanbatchPath}");
                    }
                }
                else
                {
                    commitBol = true;
                }
            }
            else
            {
                commitBol = false;
            };
            if (commitBol)
            {
                var getBundleKeyCmd = new OdbcCommand($"select bundle_key from bundle_master where bundle_code = '{objBatch.batch_code.ToUpper()}'", sqlCon, sqlTrans);
                var bundleKey = getBundleKeyCmd.ExecuteScalar();
                return bundleKey != null ? (int?)Convert.ToInt32(bundleKey) : null;
            }
            return null;
        }

        public int? Commit_Bundle(string bundle_no, string createDt, string handoverDt, string inwardDt, OdbcTransaction sqlTrans)
        {
            string sqlStr = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            string scanbatchPath = null;

            //errList = new Hashtable();
            objProj = new wfeProject(sqlCon, sqlTrans);

            dsPath = objProj.GetPath(objBatch.proj_code);

            if (dsPath.Tables[0].Rows.Count > 0)
            {
                scanbatchPath = dsPath.Tables[0].Rows[0]["project_Path"] + "\\" + objBatch.batch_code;
            }

            sqlStr = @"insert into bundle_master(proj_code,bundle_code,bundle_name,created_by" +
                ",Created_DTTM,establishment,bundle_no,creation_date,handover_date,bundle_path,inward_date) values(" +
                objBatch.proj_code + ",'" + objBatch.batch_code.ToUpper() + "','" + objBatch.batch_name + "'," +
                "'" + objBatch.Created_By + "','" + objBatch.Created_DTTM + "','NA','" + bundle_no + "','" + createDt + "','" + handoverDt + "','" +
                scanbatchPath.Replace("\\", "\\\\") + "','" + inwardDt + "')";
            if (KeyCheck(objBatch.batch_code, sqlTrans) == false)
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();

                if (mode == Constants._ADDING)
                {
                    if (FileorFolder.CreateFolder(scanbatchPath) == true)
                    {
                        commitBol = true;
                    }
                    else
                    {
                        commitBol = false;
                        throw new CreateFolderException($"Could not create folder {scanbatchPath}");
                    }
                }
                else
                {
                    commitBol = true;
                }
            }
            else
            {
                commitBol = false;
            }
            if (commitBol)
            {
                var getBundleKeyCmd = new OdbcCommand($"select bundle_key from bundle_master where bundle_code = '{objBatch.batch_code.ToUpper()}'", sqlCon, sqlTrans);
                var bundleKey = getBundleKeyCmd.ExecuteScalar();
                return bundleKey != null ? (int?)Convert.ToInt32(bundleKey) : null;
            }
            return null;
        }

        private void AddBatchExtraData(int projectKey, int batchKey, OdbcTransaction sqlTrans)
        {
            var batchData = new BatchMetadata();
            batchData.District = Districts[pbiComboBoxDistrict.SelectedIndex];


            var store = new BatchDataStore(sqlCon, sqlTrans);
            store.SaveBatchMetadata(projectKey, batchKey, batchData);
        }

        public bool _bundleCodeExists(string bundlecode)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            bool retval = false;

            string sql = "select bundle_code from bundle_master where  bundle_code = '" + bundlecode + "' ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                MessageBox.Show(this, "This Bundle Already Exists ", "Bundle Add", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                retval = false;
                textBoxBundleNumber.Focus();
                return retval;
            }
            else
            {
                retval = true;
            }

            return retval;
        }

        public bool _bundleExistsEdit(string bundleno)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            bool retval = false;

            string sql = "select bundle_no from bundle_master where  bundle_no = '" + bundleno + "' ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                MessageBox.Show(this, "This Bundle Already Exists ", "Bundle Edit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                retval = false;
                textBoxBundleNumber.Focus();
                return retval;
            }
            else
            {
                retval = true;
            }

            return retval;
        }
        public bool TransferValuesBundle(udtCmd cmd, string bundle, string creatdate, string handdate, string inwarddate,string year)
        {

            objBatch = (udtBatch)(cmd);
            if (KeyCheck(objBatch.batch_code) == false)
            {
                if (Validate(objBatch) == true)
                {
                    var transMgr = TransactionManager.GetInstance(sqlCon);
                    try
                    {
                        transMgr.OpenTransaction();
                        var batchKey = Commit_Bundle(bundle, creatdate, handdate, inwarddate, transMgr.Txn);
                        if (batchKey.HasValue)
                        {
                            AddBatchExtraData(objBatch.proj_code, batchKey.Value, transMgr.Txn);
                            transMgr.CommitTransaction();
                            return true;
                        }
                        else
                        {
                            throw new DbCommitException($"Batch key was not generated for {bundle}");
                        }
                    }
                    catch (Exception ex)
                    {
                        transMgr.AbortTransaction();
                        throw new DbCommitException(ex.Message);
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new KeyCheckException($"Bundle {objBatch.batch_code} already present");
            }
        }

        public bool TransferValuesBundleEdit(udtCmd cmd, string bundle, string creatdate, string handdate, string inwarddate)
        {

            objBatch = (udtBatch)(cmd);
            if (KeyCheck(objBatch.batch_code) == false)
            {
                if (Validate(objBatch) == true)
                {
                    var transMgr = TransactionManager.GetInstance(sqlCon);
                    try
                    {
                        transMgr.OpenTransaction();
                        var batchKey = Commit_Bundle_Edit(bundle, creatdate, handdate, inwarddate, transMgr.Txn);
                        if (batchKey.HasValue)
                        {
                            AddBatchExtraData(objBatch.proj_code, batchKey.Value, transMgr.Txn);
                            transMgr.CommitTransaction();
                            return true;
                        }
                        else
                        {
                            throw new DbCommitException($"Batch key was not generated for {bundle}");
                        }
                    }
                    catch (Exception ex)
                    {
                        transMgr.AbortTransaction();
                        throw new DbCommitException(ex.Message);
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new KeyCheckException($"Bundle {objBatch.batch_code} already present");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed || sqlCon.State == ConnectionState.Broken)
            {
                sqlCon.Open();
            }
            if (_mode == DataLayerDefs.Mode._Add)
            {
                if (textBox3.Text == null || textBox3.Text == "")
                {
                    MessageBox.Show("Please generate a Bundle Code...");
                    textBoxBundleNumber.Focus();
                    textBoxBundleNumber.Select();
                }
                //else
                //{
                //    int len = textBox3.Text.Length;
                //    int startInd = textBox3.Text.IndexOf('_') + 1;
                //    int length = len - startInd;
                //    string bundleNo = textBox3.Text.Substring(startInd, length);

                    //if (bundleNo == textBoxBundleNumber.Text.Replace("-", "_"))
                    //{

                        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
                        udtBatch objBatch = new udtBatch();
                        try
                        {
                            statusStrip1.Items.Clear();
                            crtBatch = new wfeBatch(sqlCon);

                            objBatch.proj_code = Convert.ToInt32(cmbProject.SelectedValue);
                            objBatch.batch_code = textBox3.Text;
                            objBatch.batch_name = textBox4.Text;
                            objBatch.Created_By = name;
                            objBatch.Created_DTTM = dbcon.GetCurrenctDTTM(1, sqlCon);

                            if (TransferValuesBundle(objBatch, textBoxBundleNumber.Text, txtCreateDate.Text, cmbFinancialYear.Text, dtInwarddate.Text,cmbFinancialYear.Text) == true)
                            {
                                MessageBox.Show("Bundle SucessFully Created");
                                statusStrip1.Items.Add("Status: Data SucessFully Saved");
                                statusStrip1.ForeColor = System.Drawing.Color.Black;
                                ClearAllField();
                                NavigationController.Reset();
                            }
                            else
                            {
                                MessageBox.Show(this, "Data Cannot be Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                statusStrip1.Items.Add("Status: Data Cannot be Saved");
                                statusStrip1.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        catch (KeyCheckException ex)
                        {
                            MessageBox.Show(ex.Message, "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            stateLog = new MemoryStream();
                            tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Batch Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                            stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                            //exMailLog.Log(ex, this);
                        }
                        catch (DbCommitException dbex)
                        {
                            MessageBox.Show(dbex.Message, "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            stateLog = new MemoryStream();
                            tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while Commit" + "Batch Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                            stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                            // exMailLog.Log(dbex, this);
                        }
                        catch (CreateFolderException folex)
                        {
                            MessageBox.Show(folex.Message, "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            stateLog = new MemoryStream();
                            tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while Create Folder" + "Batch Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                            stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                            // exMailLog.Log(folex, this);
                        }
                        catch (DBConnectionException conex)
                        {
                            MessageBox.Show(conex.Message, "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            stateLog = new MemoryStream();
                            tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while Connection error" + "Batch Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                            stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                            //exMailLog.Log(conex, this);
                        }
                        catch (INIFileException iniex)
                        {
                            MessageBox.Show(iniex.Message, "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            stateLog = new MemoryStream();
                            tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while INI read error" + "Batch Key-" + objBatch.batch_key + "\n" + "project Key-" + objBatch.proj_code + "\n");
                            stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                            //exMailLog.Log(iniex, this);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Bundle Number Mismatch", "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxBundleNumber.Focus();
                        textBoxBundleNumber.Select();
                        return;
                    }


                }
            
        

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            if (_mode == DataLayerDefs.Mode._Add)
            {
                DateTime temp;
                string isDate = dtHandover.Text;
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (DateTime.TryParse(isDate, out temp))
                {
                    //validateBol = true;
                }
                else
                {
                    //retval = false;
                    MessageBox.Show("Please select a valid Handover Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    dtHandover.Select();
                    //validateBol = false;

                }
            }
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                DateTime temp;
                string isDate = dtHandover.Text;
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (DateTime.TryParse(isDate, out temp))
                {
                    //validateBol = true;
                }
                else
                {
                    //retval = false;
                    MessageBox.Show("Please select a valid Handover Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    dtHandover.Select();
                    //validateBol = false;

                }
            }

        }

        private void frmBatch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.S)
            {
                button2_Click(sender, e);
            }
            if (e.KeyCode == Keys.Escape)
            {
                //this.Close();
            }

        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            if (_mode == DataLayerDefs.Mode._Add)
            {
                DateTime temp;
                string isDate = dtInwarddate.Text;
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (DateTime.TryParse(isDate, out temp))
                {
                    //validateBol = true;
                }
                else
                {
                    //retval = false;
                    MessageBox.Show("Please select a valid Inward Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    dtInwarddate.Select();
                    //validateBol = false;

                }
            }
            if (_mode == DataLayerDefs.Mode._Edit)
            {
                DateTime temp;
                string isDate = dtInwarddate.Text;
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (DateTime.TryParse(isDate, out temp))
                {
                    //validateBol = true;
                }
                else
                {
                    //retval = false;
                    MessageBox.Show("Please select a valid Inward Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    dtInwarddate.Select();
                    //validateBol = false;

                }
            }
        }

        private void dtHandover_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                NavigationController.ControlEntryCompleted(dtHandover.Name);
            }
        }

        private void dtInwarddate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                NavigationController.ControlEntryCompleted(dtInwarddate.Name);
            }
        }

        private void textBoxBundleNumber_Leave(object sender, EventArgs e)
        {
            textBoxBundleNumber.Text = textBoxBundleNumber.Text.Trim().Replace('/', '_').Replace('\\', '_').ToString();
        }

        private void groupBoxBundleInformation_Enter(object sender, EventArgs e)
        {

        }
    }
}
