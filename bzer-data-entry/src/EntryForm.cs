using bzer_data_entry.src;
using LItems;
using NovaNet.Utils;
using PhoneticBengaliInput;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Data;


namespace bzer_data_entry
{
    /// <summary>
    /// The additional database connection has been removed to avoid redundancy
    /// and improve stability. All database operations now use a single shared
    /// connection passed into the constructor. This change simplifies connection
    /// management, reduces resource overhead, and ensures consistency.
    ///
    /// The update is also part of ongoing testing to monitor and address
    /// intermittent connection problems and slowness observed during usage.
    /// </summary>
    public partial class EntryForm : Form
    {

        private readonly OdbcConnection SqlConnection = null;
        private readonly string ProjectKey;
        private readonly string BatchKey;
        private readonly Credentials Credentials;
        private readonly DataLayerDefs.Mode OpenMode = DataLayerDefs.Mode._Edit;

        private wfeBatch pBatch;

        private Metadata Data;

        private List<MasterItem> Districts;
        private List<MasterItem> Blocks;
        private List<MasterItem> Mouzas;
        private List<MasterItem> ProjaSotterTypes;

        private double currentZoom = 1.0;

        enum ImageOrientation
        {
            Portrait,
            Landscape
        }

        private List<PBITextBox> validationFields = null;

        private DataEntryNavigationController NavigationController;
        private static int i;
        private CancellationTokenSource taskController;
        private Task<Dictionary<string, IEnumerable<LanguageItem>>> taskFetchSuggestions;
        private const string ENTRY_FORM_TAG = "code";
        List<string> img_FullPath = new List<string>();
        Imagery img;
        private const int ScrollStep = 30;
        private const double ZoomFactor = 1.2;

        public EntryForm(OdbcConnection con, string projkey, string batchkey, DataLayerDefs.Mode mode, string fileName, NovaNet.Utils.Credentials prmCrd)
        {
            var v = new ValueItem();
            v.EnglishText = "LA";
            //var listItem = new List<ValueItem> {"CS" };
            AvroBengaliPhoneticParser.Parser.Register();
            InitializeComponent();
            SqlConnection = con;
            ProjectKey = projkey;
            BatchKey = batchkey;
            Credentials = prmCrd;
            OpenMode = mode;
            taskController = new CancellationTokenSource();
            LoadOffice();
            MakeDropdownOnly(txtOfficeCode);
            Data = new Metadata();

            InitNavigationController();

            if (!DesignMode)
            {
                LoadBatch(Convert.ToInt32(ProjectKey), Convert.ToInt32(BatchKey));
                if (OpenMode == DataLayerDefs.Mode._View)
                {
                    buttonSave.Enabled = buttonSave.Visible = false;
                    buttonAbort.Enabled = buttonAbort.Visible = false;
                }
                if (OpenMode != DataLayerDefs.Mode._Add)
                {
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        //using (var dbconn = new dbCon())
                        //{
                        //    var conn = dbconn.Connect();
                        var dataStore = new MetadataStore(SqlConnection);
                        var fileID = dataStore.GetFileFromFileName(fileName);
                        if (fileID.HasValue)
                        {
                            LoadFile(fileID.Value);
                            if (OpenMode == DataLayerDefs.Mode._View || OpenMode == DataLayerDefs.Mode._Rename)
                            {
                                LoadQCImagesForBatch(fileName);
                            }
                        }
                        else
                        {
                            throw new FetchError($"No file data found for file name {fileName}");
                        }
                        //}
                    }
                    if (OpenMode != DataLayerDefs.Mode._Rename)
                    {
                        DisableCSKhatianAndBataKhatianControls();
                    }
                }
            }
            validationFields = new List<PBITextBox>();

            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            this.KeyPreview = true;
        }

        //private void LoadFileType()
        //{
        //    DataTable ds = new DataTable();
        //    string str = "select syskeys,sysvalues from sysconfig";
        //    OdbcDataAdapter adpt = new OdbcDataAdapter(str, SqlConnection);
        //    adpt.Fill(ds);
        //    if(ds.Rows.Count >1)
        //    {
        //        cmbFileType.DataSource = ds;
        //        cmbFileType.DisplayMember = "sysvalues";
        //        cmbFileType.ValueMember = "syskeys";
        //        cmbFileType.SelectedIndex = 0;
        //    }
        //}
        //private void LoadPS()
        //{
        //    if (pbiComboBoxDistrict.Tag == null || pbiComboBoxBlock.Tag == null)
        //    {
        //        return;
        //    }
        //    DataTable ds = new DataTable();
        //    var metadataStore = new MetadataStore(SqlConnection);
        //    var district_code = metadataStore.GetMasterCode((Guid)pbiComboBoxDistrict.Tag);
        //    var Block_code = metadataStore.GetMasterCode((Guid)pbiComboBoxBlock.Tag);
        //    string str = "select ps_code,ps_name from tbl_lr_ps where district_code = '" + district_code + "' and block_code = '"+Block_code+"'";
        //    OdbcDataAdapter adpt = new OdbcDataAdapter(str, SqlConnection);
        //    adpt.Fill(ds);
        //    if (ds.Rows.Count > 0)
        //    {
        //        cmbPS.DataSource = ds;
        //        cmbPS.DisplayMember = "ps_name";
        //        cmbPS.ValueMember = "ps_code";
        //        cmbPS.SelectedIndex = 0;
        //    }
        //}
        private void Loadcaseno()
        {
            DataTable ds = new DataTable();
            string str = "select bundle_no,handover_date from bundle_master where proj_code = '"+Convert.ToInt32(ProjectKey)+ "' and bundle_key = '" + Convert.ToInt32(BatchKey) + "'";
           
            OdbcDataAdapter adpt = new OdbcDataAdapter(str, SqlConnection);
            adpt.Fill(ds);
            if (ds.Rows.Count > 0)
            {
                pbiTextBox1.EnglishText = ds.Rows[0][0].ToString();
                pbiTextBox2.EnglishText = ds.Rows[0][1].ToString();
                pbiTextBox1.Enabled = false;
            }
        }

        private void LoadOffice()
        {
            if (pbiComboBoxDistrict.Tag == null)
            {
                return;
            }
            DataTable ds = new DataTable();
            //var metadataStore = new MetadataStore(SqlConnection);
            //var district_code = metadataStore.GetMasterCode((Guid)pbiComboBoxDistrict.Tag);
            string str = "select code,officename from tbloffice ";
            OdbcDataAdapter adpt = new OdbcDataAdapter(str, SqlConnection);
            adpt.Fill(ds);
            if (ds.Rows.Count > 0)
            {
                txtOfficeCode.DataSource = ds;
                txtOfficeCode.DisplayMember = "officename";
                txtOfficeCode.ValueMember = "code";
                txtOfficeCode.ValueMember = "code";
                txtOfficeCode.SelectedIndex = 0;
            }
        }
        private void LoadQCImagesForBatch(string filename)
        {
            var wBatch = new wfeBatch(SqlConnection);
            var batchPath = wBatch.GetPath(Convert.ToInt32(ProjectKey), Convert.ToInt32(BatchKey));
            var imgPath = batchPath + "\\" + filename + "\\QC";
            LoadImagesFromFolder(imgPath);
        }
        private void MakeDropdownOnly(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is ComboBox combo)
                {
                    combo.DropDownStyle = ComboBoxStyle.DropDownList; // ?? disables typing
                }
                else
                {
                    MakeDropdownOnly(ctrl); // recursive (because it's inside panel)
                }
            }
        }
        private void BrowseAndExportImages()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select folder containing images";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    img_FullPath.Clear();
                    listViewImageList.Items.Clear();
                    DirectoryInfo dInfo = new DirectoryInfo(fbd.SelectedPath);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                    var imgPath = fbd.SelectedPath;
                    LoadImagesFromFolder(imgPath);
                }
            }
        }
        private void DisableCSKhatianAndBataKhatianControls()
        {
            //pbiTextBoxCSKhatianNo.Ready += () => pbiTextBoxCSKhatianNo.Enabled = false;
            //pbiTextBoxCSBataKhatianNo.Ready += () => pbiTextBoxCSBataKhatianNo.Enabled = false;
        }
        private void InitNavigationController()
        {
            pbiComboBoxDistrict.Ready += () => pbiComboBoxDistrict.Enabled = false;
            pbiComboBoxBlock.Ready += () => pbiComboBoxBlock.Enabled = false;
            pbiComboBoxMouza.Ready += () => pbiComboBoxMouza.Enabled = false;

            var controllerBuilder = new DataEntryNavigationController.Builder(this);
            foreach (var controlIndexPair in new Control[] {
                txtOfficeCode,
                txtActNo,
                txtRequringbody,

                add_UserControlCSPlot,
                buttonSave,
                buttonAbort
            }.Select((control, index) => (control, index)))
            {
                controllerBuilder.AddControl(controlIndexPair.index, controlIndexPair.control.Name, controlIndexPair.control);
            }
            NavigationController = controllerBuilder.Ready();
        }
        private void LoadHardcodedPlot()
        {
            var items = new List<LanguageItem>
    {
        new LanguageItem
        {
            EnglishText = "CS",
            BengaliPhoneticText = "CS"
        },
        new LanguageItem
        {
            EnglishText = "LR",
            BengaliPhoneticText = "LR"
        },
        new LanguageItem
        {
            EnglishText = "RS",
            BengaliPhoneticText = "RS"
        }
    };

           // cmbPlot_Type.LanguageItems.Set(items);
        }
        private List<MasterItem> LoadDistricts()
        {
            var dataStore = new MetadataStore(SqlConnection);
            var districts = dataStore.GetMasterList(null, new List<string> { MetadataStore.FIELD_DISTRICT });
            pbiComboBoxDistrict.LanguageItems.Set(districts.Select(district => (LanguageItem)district.Value));
            return districts;
        }

        private List<MasterItem> LoadBlocks(Guid underDistrict)
        {
            var dataStore = new MetadataStore(SqlConnection);
            var blocks = dataStore.GetMasterList(underDistrict, new List<string> { MetadataStore.FIELD_BLOCK });
            pbiComboBoxBlock.LanguageItems.Set(blocks.Select(block => (LanguageItem)block.Value));
            return blocks;
        }

        private List<MasterItem> LoadMouzas(Guid underBlock)
        {
            var dataStore = new MetadataStore(SqlConnection);
            var mouzas = dataStore.GetMasterList(underBlock, new List<string> { MetadataStore.FIELD_MOUZA });
            pbiComboBoxMouza.LanguageItems.Set(mouzas.Select(mouza => (LanguageItem)mouza.Value));
            return mouzas;
        }

        private List<MasterItem> LoadProjaSotterTypes(Guid districtID)
        {
            var dataStore = new MetadataStore(SqlConnection);
            var projaSotterTypes = dataStore.GetMasterList(districtID, new List<string> { MetadataStore.FIELD_PROJA_ST_TYPE });
            //pbiComboBoxProjasottertype.LanguageItems.Set(projaSotterTypes.Select(pst => (LanguageItem)pst.Value));
            return projaSotterTypes;
        }

        private void LoadBatch(int projectKey, int batchKey)
        {
            pBatch = new wfeBatch(SqlConnection);
            //labelBundleNumber.Text = pBatch.GetBundleName(projectKey, batchKey);

            var batchStore = new BatchDataStore(SqlConnection);
            var batchData = batchStore.GetBatchMetadata(projectKey, batchKey);
            Districts = LoadDistricts(); 
            pbiComboBoxDistrict.SelectedIndex = Districts.FindIndex(district => district.MasterID == batchData.District.MasterID);
            pbiComboBoxDistrict.Tag = Districts.Find(district => district.MasterID == batchData.District.MasterID)?.MasterID ?? batchData.District.MasterID;
            Blocks = LoadBlocks(batchData.District.MasterID);
            pbiComboBoxBlock.SelectedIndex = Blocks.FindIndex(block => block.MasterID == batchData.Block.MasterID);
            pbiComboBoxBlock.Tag = Blocks.Find(block => block.MasterID == batchData.Block.MasterID)?.MasterID ?? batchData.Block.MasterID;
            Mouzas = LoadMouzas(batchData.Block.MasterID);
            pbiComboBoxMouza.SelectedIndex = Mouzas.FindIndex(mouza => mouza.MasterID == batchData.Mouza.MasterID);
            pbiComboBoxMouza.Tag = Mouzas.Find(mouza => mouza.MasterID == batchData.Mouza.MasterID)?.MasterID ?? batchData.Mouza.MasterID;
            Data.District = batchData.District;
            Data.Block = batchData.Block;
            Data.Mouza = batchData.Mouza;
            ProjaSotterTypes = LoadProjaSotterTypes(batchData.District.MasterID);

            Loadcaseno();
           // GenerateRecordNo();
            //LoadSubDivision();
            LoadOffice();
        }

        public Task<Dictionary<string, IEnumerable<LanguageItem>>> GetNameTitleSuggestions(BatchMetadata batchData = null)
        {
            taskFetchSuggestions = Task.Run(() =>
            {
                Dictionary<string, IEnumerable<LanguageItem>> fieldValue = null;
                using (var dbConSuggestion = new dbCon())
                {
                    var sqlConSuggestion = dbConSuggestion.Connect();
                    var metaStore = new MetadataStore(sqlConSuggestion);

                    string parentType = null;
                    Guid? parentId = null;

                    if (batchData != null)
                    {
                        parentType = MetadataStore.FIELD_MOUZA;
                        parentId = batchData.Mouza.MasterID;
                    }
                    fieldValue = metaStore
                        .GetFieldValue(
                            new List<string> { MetadataStore.FIELD_SATYA_NAME, MetadataStore.FIELD_SATYA_TITLE, MetadataStore.FIELD_PROJA_NAME, MetadataStore.FIELD_PROJA_TITLE },
                            taskController.Token,
                            parentType,
                            parentId);
                    return fieldValue;
                }
            }, taskController.Token);
            return taskFetchSuggestions;
        }



        private void LoadFile(Guid fileID)
        {
            //using (var dbconn = new dbCon())
            //{
            //    var conn = dbconn.Connect();
            var dataStore = new MetadataStore(SqlConnection);
            (_, _, Data) = dataStore.GetFile(fileID);
            pbiComboBoxDistrict.SelectedIndex = Districts.FindIndex(district => district.MasterID == Data.District.MasterID);
            pbiComboBoxDistrict.Tag = Data.District?.MasterID;
            Blocks = LoadBlocks(Data.District.MasterID);
            pbiComboBoxBlock.SelectedIndex = Blocks.FindIndex(block => block.MasterID == Data.Block.MasterID);
            pbiComboBoxBlock.Tag = Data.Block?.MasterID;
            Mouzas = LoadMouzas(Data.Block.MasterID);
            pbiComboBoxMouza.SelectedIndex = Mouzas.FindIndex(mouza => mouza.MasterID == Data.Mouza.MasterID);
            pbiComboBoxMouza.Tag = Data.Mouza?.MasterID;
            add_UserControlCSPlot.plottype = Data.plottype.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.district = Data.district.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.block = Data.block.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.mauza = Data.mauza.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.PlotNumbers = Data.Plots.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.BataPlotNumbers = Data.BataPlots.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.ptsr = Data.ptsr.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.Mesurement = Data.mesurment.Select(data => (LanguageItem) data).ToList();
            add_UserControlCSPlot.pt1 = Data.pt1.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.pt2 = Data.pt2.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.pt2 = Data.pt2.Select(data => (LanguageItem)data).ToList();
            add_UserControlCSPlot.ptsr = Data.ptsr.Select(data => (LanguageItem)data).ToList();
            pbiTextBox1.EnglishText = Data.CaseYear.EnglishText;
            txtPurpose.EnglishText = Data.purpose.EnglishText;
            txtActNo.EnglishText = Data.actno.EnglishText;
            txtRequringbody.EnglishText = Data.runningbody.EnglishText; 

        }

        private void Reset()
        {
            Data = new Metadata();
            NavigationController.Reset();
            Districts = LoadDistricts();
            add_UserControlCSPlot.plottype = new List<LanguageItem>();
            add_UserControlCSPlot.PlotNumbers = new List<LanguageItem>();
            add_UserControlCSPlot.BataPlotNumbers = new List<LanguageItem>();
            add_UserControlCSPlot.ptsr = new List<LanguageItem>();
            add_UserControlCSPlot.Mesurement = new List<LanguageItem>();
            add_UserControlCSPlot.district = new List<LanguageItem>();
            add_UserControlCSPlot.block = new List<LanguageItem>();
            add_UserControlCSPlot.mauza = new List<LanguageItem>();

        }

        private void pbiComboBoxDistrict_InnerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (pbiComboBoxDistrict.SelectedIndex >= 0 && Districts != null && pbiComboBoxDistrict.SelectedIndex < Districts.Count)
            {
                Data.District = Districts[pbiComboBoxDistrict.SelectedIndex];
                var selectedDistrictID = Data.District.MasterID;
                pbiComboBoxDistrict.Tag = selectedDistrictID;
                Blocks = LoadBlocks(selectedDistrictID);
                ProjaSotterTypes = LoadProjaSotterTypes(selectedDistrictID);
            }
        }

        private void pbiComboBoxBlock_InnerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (pbiComboBoxBlock.SelectedIndex >= 0 && Blocks != null && pbiComboBoxBlock.SelectedIndex < Blocks.Count)
            {
                Data.Block = Blocks[pbiComboBoxBlock.SelectedIndex];
                var selectedBlockID = Data.Block.MasterID;
                pbiComboBoxBlock.Tag = selectedBlockID;
                Mouzas = LoadMouzas(selectedBlockID);
            }
        }

        private void pbiComboBoxMouza_InnerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (pbiComboBoxMouza.SelectedIndex >= 0 && Mouzas != null && pbiComboBoxMouza.SelectedIndex < Mouzas.Count)
            {
                Data.Mouza = Mouzas[pbiComboBoxMouza.SelectedIndex];
                pbiComboBoxMouza.Tag = Data.Mouza.MasterID;
            }
        }

        private void pbiTextBoxCSKhatianNo_InnerTextChanged(object sender, EventArgs e)
        {
            //Data.KhatianNo = pbiTextBoxCSKhatianNo.Value;
        }

        private void pbiTextBoxCSBataKhatianNo_InnerTextChanged(object sender, EventArgs e)
        {
           // Data.BataKhatianNo = pbiTextBoxCSBataKhatianNo.Value;
        }

        private void add_UserControlCSPlot_ItemsCommitted(object sender, EventArgs e)
        {
            //Data.district.Clear();
            //Data.Plots.AddRange(add_UserControlCSPlot.district.Select(district => (ValueItem)district));
            //Data.block.Clear();
            //Data.Plots.AddRange(add_UserControlCSPlot.block.Select(block => (ValueItem)block));
            //Data.mauza.Clear();
            //Data.Plots.AddRange(add_UserControlCSPlot.mauza.Select(mauza => (ValueItem)mauza));
            Data.district.Clear();
            Data.district.AddRange(add_UserControlCSPlot.district.Select(district => (ValueItem)district));
            Data.block.Clear();
            Data.block.AddRange(add_UserControlCSPlot.block.Select(block => (ValueItem)block));
            Data.mauza.Clear();
            Data.mauza.AddRange(add_UserControlCSPlot.mauza.Select(mauza => (ValueItem)mauza));
            Data.Plots.Clear();
            Data.Plots.AddRange(add_UserControlCSPlot.PlotNumbers.Select(plot => (ValueItem)plot));
            Data.BataPlots.Clear();
            Data.BataPlots.AddRange(add_UserControlCSPlot.BataPlotNumbers.Select(bataPlot => (ValueItem)bataPlot));
            Data.mesurment.Clear();
            Data.mesurment.AddRange(add_UserControlCSPlot.Mesurement.Select(mesure => (ValueItem)mesure));
            Data.ptsr.Clear();
            Data.ptsr.AddRange(add_UserControlCSPlot.ptsr.Select(mesure => (ValueItem)mesure));
            Data.plottype.Clear();
            Data.plottype.AddRange(add_UserControlCSPlot.plottype.Select(mesure => (ValueItem)mesure));
            

            NavigationController.ControlEntryCompleted(add_UserControlCSPlot.Name);
        }
         

        //private void ValidateAdd()
        //{
        //    var allPbiTextBoxes = new[]
        //    {
        //        pbiTextBoxCSKhatianNo,
        //        pbiTextBoxCSBataKhatianNo
        //    };
        //    validationFields.AddRange(
        //        allPbiTextBoxes.Where(x => !string.IsNullOrEmpty(x.EnglishText))
        //        );
        //}
        private void buttonSave_Click(object sender, EventArgs e)
        {

            // TODO: the following block must be handled by triggering selected index change event from PBI combo box when selected index is changed from program
            // right now we just poll the values before save :(
            //Data.District = Districts[pbiComboBoxDistrict.SelectedIndex];
            //Data.Block = Blocks[pbiComboBoxBlock.SelectedIndex];
            //Data.Mouza = Mouzas[pbiComboBoxMouza.SelectedIndex];

            var transactionMgr = TransactionManager.GetInstance(SqlConnection);
            try
            {
                //ValidateAdd();
                if (validationFields.Count > 0)
                {
                    bool IsValidInput = ValidationHelper.ValidateAllInputs(validationFields);
                    validationFields.Clear();
                    if (!IsValidInput)
                    {
                        return;
                    }
                }

                transactionMgr.OpenTransaction();
                var sqlTransaction = transactionMgr.Txn;
                var dataStore = new MetadataStore(SqlConnection, sqlTransaction);
                //Data.FileName.EnglishText = dataStore.GenerateFileName(Data.District.MasterID, Data.Block.MasterID, Data.Mouza.MasterID, Data.FileName.EnglishText);

                //Data.FileName.EnglishText = labelBundleNumber.Text;
                if (string.IsNullOrEmpty(Data.FileName.EnglishText))
                {
                    Data.FileName.EnglishText = pbiTextBox1.EnglishText;
                }
                //Data.PsCode.EnglishText= cmbPS.SelectedValue.ToString();
                //Data.officecode.EnglishText = txtOfficeCode.SelectedValue.ToString();
                //Data.CaseType.EnglishText = txtCaseType.EnglishText;
                Data.CaseYear.EnglishText = pbiTextBox1.EnglishText;
                //Data.LAPlotType = (ValueItem)cmbPlot_Type.Value;
                //Data.pt1 = (ValueItem)txtPTR1.Value;
                //Data.pt2 = (ValueItem)txtPTR2.Value;
                //Data.ptsr = (ValueItem)txtPTSR.Value;
                Data.purpose = (ValueItem)txtPurpose.Value;
                Data.actno = (ValueItem)txtActNo.Value;
                Data.runningbody = (ValueItem)txtRequringbody.Value;
                //Data.officecode = txtOfficeCode.ValueMember;
                string officecode = txtOfficeCode.ValueMember;

                Data.officecode = new ValueItem()
                {
                    EnglishText = officecode
                };
                string record = txtOfficeCode.ValueMember;

                Data.record = new ValueItem()
                {
                    EnglishText = officecode+officecode
                };
                //Data.Validate();
                dataStore.Validate(Data);
                string truncatedUserName = Credentials.userName.Substring(0, Math.Min(Credentials.userName.Length, 20));
                dataStore.SaveFile(int.Parse(ProjectKey), int.Parse(BatchKey), truncatedUserName, Data);
                transactionMgr.CommitTransaction();
                MessageBox.Show("Data saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (OpenMode == DataLayerDefs.Mode._Add)
                {
                    DialogResult = DialogResult.None;
                    Reset();
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
            catch (StoreError ex)
            {
                transactionMgr.AbortTransaction();
                MessageBox.Show(ex.Message, "Error: Data wasn't saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;

               // NavigationController.ControlError(pbiTextBoxCSKhatianNo.Name);
            }
            catch (DomainError ex)
            {
                transactionMgr.AbortTransaction();
                MessageBox.Show(ex.Message, "Error: Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;

               // NavigationController.ControlError(pbiTextBoxCSKhatianNo.Name);
            }
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void EntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OpenMode != DataLayerDefs.Mode._View && !taskFetchSuggestions.IsCompleted)
            {
                CancelTasks();
            }
            if (e.CloseReason == CloseReason.UserClosing)
            {
                buttonAbort_Click(sender, e);
            }
        }

        private void CancelTasks()
        {
            taskController.Token.WaitHandle.WaitOne(1000);
            taskController.Cancel();
        }

        private void EntryForm_Load(object sender, EventArgs e)
        {
            if (OpenMode != DataLayerDefs.Mode._View)
            {
                LoadNameTitleSuggestionsAsync();
            }
            if (OpenMode == DataLayerDefs.Mode._Add || OpenMode == DataLayerDefs.Mode._Edit)
            {
                BrowseAndExportImages();
            }
        }

        private async void LoadNameTitleSuggestionsAsync()
        {
            ToggleLabelLoadingSuggestions(true);
            var allNamesTitles = await GetNameTitleSuggestions();
            ToggleLabelLoadingSuggestions(false);

            if (allNamesTitles == null || allNamesTitles.Keys.Count == 0)
            {
                PushSuggestions();
                return;
            }
            else
            {
                var allNames = allNamesTitles["name"].ToList();
                var allTitles = allNamesTitles["title"].ToList();
                PushSuggestions(allNames, allTitles);
            }
        }

        private void PushSuggestions(List<LanguageItem> allNames = null, List<LanguageItem> allTitles = null)
        {
            //addSatyadhikariNameTitle.PushSuggestions(allNames, allTitles);
            //addProjaNameTitle.PushSuggestions(allNames, allTitles);
        }

        private void ToggleLabelLoadingSuggestions(bool visible)
        {
           // addSatyadhikariNameTitle.ToggleLabelLoadingSuggestions(visible);
            //addProjaNameTitle.ToggleLabelLoadingSuggestions(visible);
        }
        private void pbiTextBoxCSKhatianNo_Leave(object sender, EventArgs e)
        {
            Data.KhatianNo = new LanguageItem()
            {
                //EnglishText = pbiTextBoxCSKhatianNo.EnglishText
            };
        }

        private void pbiTextBoxCSBataKhatianNo_Leave(object sender, EventArgs e)
        {
            Data.BataKhatianNo = new LanguageItem()
            {
               // EnglishText = pbiTextBoxCSBataKhatianNo.EnglishText
            };
        }

        private void LoadImagesFromFolder(string folderPath)
        {
            setpath(img_FullPath, folderPath);
            if (img_FullPath.Count == 0)
            {
                MessageBox.Show("No images found in the selected folder.",
                       "No Images Found",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning);
                return;
            }
            foreach (string imagePath in img_FullPath)
            {
                var imageName = Path.GetFileName(imagePath);
                ListViewItem item = new ListViewItem(imageName);
                item.Tag = imagePath;
                listViewImageList.Items.Add(item);
            }
            listViewImageList.Items[0].Selected = true;
        }
        void setpath(List<string> imPath, string folder)
        {
            Directory.GetFiles(folder, "*.*")
                .Where(m => (m.ToLower().EndsWith(".tif") || (m.ToLower().EndsWith(".tiff"))))
                .ToList()
                .ForEach(x => imPath.Add(x));
        }
        private void listViewImageList_Resize(object sender, EventArgs e)
        {
            listViewImageList.BeginUpdate();
            listViewImageList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewImageList.EndUpdate();
        }
        private void displayImage()
        {
            try
            {
                if (listViewImageList.SelectedItems.Count == 0)
                    return;
                var imgFileName = listViewImageList.SelectedItems[0].Tag?.ToString();
                if (string.IsNullOrEmpty(imgFileName))
                    return;
                if (!File.Exists(imgFileName))
                {
                    MessageBox.Show("Image not found at: " + imgFileName);
                    return;
                }
                pictureBoxImage.Width = panelImage.Width;
                pictureBoxImage.Height = panelImage.Height;
                img.Close();
                img.LoadBitmapFromFile(imgFileName);
                pictureBoxImage.Image = img.GetBitmap();
                this.groupBoxImage.Text = listViewImageList.SelectedItems[0].Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to display image.\n\nDetails: {ex.Message}",
                                "Display Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        private void listViewImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayImage();
            ResetCurrentZoom();
        }

        public void zoomIn()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    currentZoom *= ZoomFactor;
                    ZoomImage(currentZoom);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Zoom in failed: {ex.Message}", "Zoom Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void zoomOut()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    currentZoom *= 1.0 / ZoomFactor;
                    ZoomImage(currentZoom);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Zoom out failed: {ex.Message}", "Zoom Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            zoomIn();
        }

        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            zoomOut();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Add:
                    zoomIn();
                    return true;

                case Keys.Subtract:
                    zoomOut();
                    return true;

                case Keys.Right:
                    moveNextImage();
                    return true;

                case Keys.Left:
                    moveBackImage();
                    return true;

                case Keys.Control | Keys.R:
                    RotateRight();
                    return true;

                case Keys.Control | Keys.L:
                    RotateLeft();
                    return true;

                case Keys.Control | Keys.Up:
                    ImageMoveUp();
                    return true;

                case Keys.Control | Keys.Down:
                    ImageMoveDown();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void moveBackImage()
        {
            if (listViewImageList.Items.Count == 0)
                return;

            int currentIndex = -1;
            if (listViewImageList.SelectedItems.Count > 0)
                currentIndex = listViewImageList.SelectedItems[0].Index;

            int prevIndex = currentIndex - 1;
            if (prevIndex < 0)
                prevIndex = listViewImageList.Items.Count - 1;

            listViewImageList.SelectedItems.Clear();
            listViewImageList.Items[prevIndex].Selected = true;
            listViewImageList.Items[prevIndex].Focused = true;
            listViewImageList.EnsureVisible(prevIndex);
        }

        public void moveNextImage()
        {
            if (listViewImageList.Items.Count == 0)
                return;

            int currentIndex = -1;
            if (listViewImageList.SelectedItems.Count > 0)
                currentIndex = listViewImageList.SelectedItems[0].Index;

            int nextIndex = currentIndex + 1;
            if (nextIndex >= listViewImageList.Items.Count)
                nextIndex = 0;

            listViewImageList.SelectedItems.Clear();
            listViewImageList.Items[nextIndex].Selected = true;
            listViewImageList.Items[nextIndex].Focused = true;
            listViewImageList.EnsureVisible(nextIndex);

        }
        public void RotateRight()
        {
            try
            {
                if (img.IsValid())
                {
                    ResetCurrentZoom();
                    img.RotateRight();
                    ChangeSize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to rotate image right: {ex.Message}", "Rotate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void RotateLeft()
        {
            try
            {
                if (img.IsValid())
                {
                    ResetCurrentZoom();
                    img.RotateLeft();
                    ChangeSize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to rotate image left: {ex.Message}", "Rotate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangeSize()
        {
            if (img.IsValid() == true)
            {
                pictureBoxImage.Image = null;
                pictureBoxImage.Width = panelImage.Width;
                pictureBoxImage.Height = panelImage.Height;
                pictureBoxImage.Image = img.GetBitmap();
            }
        }

        private void buttonRotateLeft_Click(object sender, EventArgs e)
        {
            RotateLeft();
        }

        private void buttonRotateRight_Click(object sender, EventArgs e)
        {
            RotateRight();
        }

        public void ImageMoveUp()
        {
            var cur = panelImage.AutoScrollPosition;
            int curX = -cur.X;
            int curY = -cur.Y;
            panelImage.AutoScrollPosition = new Point(curX, Math.Max(0, curY - ScrollStep));
        }
        public void ImageMoveDown()
        {
            var cur = panelImage.AutoScrollPosition;
            int curX = -cur.X;
            int curY = -cur.Y;
            panelImage.AutoScrollPosition = new Point(curX, curY + ScrollStep);
        }

        private ImageOrientation GetImageOrientation(Bitmap bmp)
        {
            return GetAspectRatio(bmp) <= 1.0 ? ImageOrientation.Portrait : ImageOrientation.Landscape;
        }

        private double GetAspectRatio(Bitmap bmp)
        {
            return (double)bmp.Width / (double)bmp.Height;
        }


        private void ZoomImage(double factor)
        {
            if (GetImageOrientation(img.GetBitmap()) == ImageOrientation.Portrait)
            {
                var newZoom = ((Control)(pictureBoxImage.GetContainerControl())).ClientRectangle.Height * factor;
                pictureBoxImage.Size = new Size((int)(newZoom * GetAspectRatio(img.GetBitmap())), (int)newZoom);
            }
            else
            {
                var containerRect = ((Control)(pictureBoxImage.GetContainerControl())).ClientRectangle;
                var baseSize = Math.Min(containerRect.Width, containerRect.Height);
                var newZoom = baseSize * factor;
                pictureBoxImage.Size = new Size((int)newZoom, (int)(newZoom / GetAspectRatio(img.GetBitmap())));
            }
            pictureBoxImage.Refresh();

        }
        private void ResetCurrentZoom()
        {
            currentZoom = 1.0;
        }

        private void add_UserControlCSPlot_Load(object sender, EventArgs e)
        {

        }

        private void pbiComboBoxBlock_Load(object sender, EventArgs e)
        {

        }
        //public static string GetText(PBITextBox textBox)
        //{
        //    return (textBox.EnglishText == string.Empty) ? textBox.BengaliPhoneticText : textBox.EnglishText;
        //}
        //static readonly Regex InvalidCodePattern = new Regex(@"^\d+$");
        //public static bool ValidateCode(PBITextBox textBox)
        //{
        //    var code = GetText(textBox);
        //    var isMatch = Regex.IsMatch(code, InvalidCodePattern.ToString());
        //    if (!isMatch)
        //    {
        //        MessageBox.Show("Integer field must contain only digits.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        return false;
        //    }
        //    if (textBox.Tag is CoustomTag coustomTag)
        //    {
        //        int requiredDigits = (coustomTag.currentLevel == MetadataStore.FIELD_MOUZA) ? MOUZA_CODE_LENGTH : DISTRICT_OR_BLOCK_CODE_LENGTH;
        //        if (code.Length != requiredDigits)
        //        {
        //            MessageBox.Show($"Code for {coustomTag.currentLevel} must be exactly {requiredDigits} digits.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            return false;
        //        }

        //    }
        //    return true;
        //}
        private void pbiTextBox1_Leave(object sender, EventArgs e)
        {
            if (ValidateCode(pbiTextBox1) == true)
            {
                
            }

            else
            {
                pbiTextBox1.Select();
                pbiTextBox1.Focus();
                return;
            }
                

        }
        //private bool ValidateCode(PBITextBox textBox)
        //{
        //    //var text = pbiTextBox1.EnglishText;
        //    var text = textBox.EnglishText;
        //    if (!int.TryParse(text, out int year))
        //    {
        //        MessageBox.Show("Enter a valid number.", "Alert",
        //            MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        return false;
        //    }

        //    if (text.Length != 4)
        //    {
        //        MessageBox.Show("Year must be exactly 4 digits.", "Alert",
        //            MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        return false;
        //    }

        //    if (year < 1900 || year > 3000)
        //    {
        //        MessageBox.Show("Year must be between 1900 and 3000.", "Alert",
        //            MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        return false;
        //    }

        //    return true;
        //}
        private bool ValidateCode(PBITextBox textBox)
                
                {
            //var text = textBox.EnglishText;

            //if (text.Length != 4)
            //{
            //    MessageBox.Show("Year must be exactly 4 digits.", "Alert",
            //        MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return false;
            //}

            //if (!int.TryParse(text, out int year))
            //{
            //    MessageBox.Show("Enter a valid number.", "Alert",
            //        MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return false;
            //}

            //if (year < 1900 || year > 3000)
            //{
            //    MessageBox.Show("Year must be between 1900 and 3000.", "Alert",
            //        MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return false;
            //}

            return true;
        }

        private void pbiTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            bool isValid = ValidateCode(pbiTextBox1);

            if (!isValid)
            {
                pbiTextBox1.Select();
                pbiTextBox1.Focus();
            }
        }

        private void cmbPS_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    e.SuppressKeyPress = true; // Prevent the "ding" sound
            //    this.SelectNextControl(
            //        (Control)sender,  // current control
            //        true,             // move forward
            //        true,             // tab stop only
            //        true,             // nested controls
            //        true              // wrap around
            //    );
            //}
        }

        private void txtCaseType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void pbiTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void cmbPlot_Type_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txtPTR1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txtPTR2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txtPTSR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txtPurpose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txtActNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txtRequringbody_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txtJLNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void pbiTextBox2_Leave(object sender, EventArgs e)
        {
            if(txtOfficeCode.Text.Length >14)
            {
                //MessageBox.Show(this, "Office Code Must be Exact Five Digits", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                //txtOfficeCode.Select();
                //txtOfficeCode.Focus();
                //return;
            }
        }

        private void cmbFileType_Leave(object sender, EventArgs e)
        {
            //var metadataStore = new MetadataStore(SqlConnection);

            //Guid? districtId = pbiComboBoxDistrict.Tag as Guid?;
            //if (!districtId.HasValue && pbiComboBoxDistrict.SelectedIndex >= 0 && Districts != null && pbiComboBoxDistrict.SelectedIndex < Districts.Count)
            //{
            //    districtId = Districts[pbiComboBoxDistrict.SelectedIndex].MasterID;
            //    pbiComboBoxDistrict.Tag = districtId.Value;
            //}
            //else if (!districtId.HasValue && Data?.District != null)
            //{
            //    districtId = Data.District.MasterID;
            //    pbiComboBoxDistrict.Tag = districtId.Value;
            //}

            //Guid? blockId = pbiComboBoxBlock.Tag as Guid?;
            //if (!blockId.HasValue && pbiComboBoxBlock.SelectedIndex >= 0 && Blocks != null && pbiComboBoxBlock.SelectedIndex < Blocks.Count)
            //{
            //    blockId = Blocks[pbiComboBoxBlock.SelectedIndex].MasterID;
            //    pbiComboBoxBlock.Tag = blockId.Value;
            //}
            //else if (!blockId.HasValue && Data?.Block != null)
            //{
            //    blockId = Data.Block.MasterID;
            //    pbiComboBoxBlock.Tag = blockId.Value;
            //}

            //Guid? mouzaId = pbiComboBoxMouza.Tag as Guid?;
            //if (!mouzaId.HasValue && pbiComboBoxMouza.SelectedIndex >= 0 && Mouzas != null && pbiComboBoxMouza.SelectedIndex < Mouzas.Count)
            //{
            //    mouzaId = Mouzas[pbiComboBoxMouza.SelectedIndex].MasterID;
            //    pbiComboBoxMouza.Tag = mouzaId.Value;
            //}
            //else if (!mouzaId.HasValue && Data?.Mouza != null)
            //{
            //    mouzaId = Data.Mouza.MasterID;
            //    pbiComboBoxMouza.Tag = mouzaId.Value;
            //}

            //var district_code = districtId.HasValue ? metadataStore.GetMasterCode(districtId.Value) : "";
            //var Block_code = blockId.HasValue ? metadataStore.GetMasterCode(blockId.Value) : "";
            //var mauza_code = mouzaId.HasValue ? metadataStore.GetMasterCode(mouzaId.Value) : "";
            string running_srl = GetSerial().ToString().PadLeft(5, '0');
            string officecode = txtOfficeCode.Text ;
            txtOfficeCode.Text = txtOfficeCode + "_" +  running_srl;
        }

        public int GetSerial()
        {
            DataTable ds = new DataTable();
             i = 1;
            try
            {
                string str = "select max(running_serial) from metadata_entry";
                OdbcDataAdapter adpt = new OdbcDataAdapter(str, SqlConnection);
                adpt.Fill(ds);
                if (ds.Rows.Count > 0 && ds.Rows[0][0] != DBNull.Value && !string.IsNullOrWhiteSpace(ds.Rows[0][0].ToString()))
                {
                    i = Convert.ToInt32(ds.Rows[0][0]) + 1;
                }
                else
                {
                    i = 1;
                }
            }
            catch
            {
                i = 1;
            }

            return i;
        }
    }
}
