using bzer_data_entry;
using ImageHeaven.Logic;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using Reporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageHeaven
{
    public partial class frmMouzaExport : Form
    {
        private readonly OdbcConnection SqlCon;

        private readonly Credentials Credential;

        private IExporter Exporter;

        private int MouzaCount;

        private int BundleCount;

        private string lastClickedButtonName;

        private bool columnWidthAdjusting;

        public frmMouzaExport(OdbcConnection prmCon, Credentials prmCrd)
        {
            InitializeComponent();
            SqlCon = prmCon;
            Credential = prmCrd;
            init();
            AttachClickHandlers();
            ShowBlock();
            Reset();

        }
        private void init()
        {
            MouzaCount = 0;
            BundleCount = 0;
            lastClickedButtonName = null;
            columnWidthAdjusting = false;
        }
        private void AttachClickHandlers()
        {
            buttonValidate.Click += Button_Click;
            buttonExport.Click += Button_Click;
        }
        private void Button_Click(object sender, EventArgs e)
        {
            lastClickedButtonName = (sender as Button)?.Text.Substring(1);
        }
        private void Reset()
        {
            comboBoxBlockSelection.SelectedIndex = -1;
            listViewMouza.Items.Clear();
            listViewBundle.Items.Clear();
            buttonExport.Enabled = false;
            btnCancel.Enabled = false;
            btnClose.Enabled = true;
            checkBoxSelectAll.Checked = false;
        }
        private Dictionary<int, string> ProjectKeyValueDict = new Dictionary<int, string>();
        private void LoadProject()
        {
            try
            {
                var proj = new wfeProject(SqlCon);
                var projects = proj.GetAllValues().Tables.Cast<DataTable>()
                    .SelectMany(tab => tab.Rows.Cast<DataRow>().Select(row => new IDName { ID = row.Field<int>("proj_key"), Name = row.Field<string>("proj_code") }))
                    .ToList();
                projects.ForEach(x =>
                {
                    ProjectKeyValueDict[x.ID] = x.Name;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmMouzaExport_Load(object sender, System.EventArgs e)
        {
            LoadProject();
        }
        private DataTable GetMouzaForExport(bool reExport)
        {
            var blockMasterId = (Guid)comboBoxBlockSelection.SelectedValue;
            var batchStates = reExport ?
                            new eSTATES[] { eSTATES.BATCH_EXPORTED } :
                            new eSTATES[] { eSTATES.BATCH_READY_FOR_UAT };
            var store = new frmMouzaExportLogic(SqlCon);
            return store.GetMouzaForExport(batchStates, blockMasterId);
        }
        private void DisplayMouzaForExport(bool reExport)
        {
            try
            {
                var res = GetMouzaForExport(reExport);
                listViewMouza.Items.Clear();
                foreach (DataRow row in res.Rows)
                {
                    var mouzaName = row[0].ToString();
                    var mouzaMasterId = row[1].ToString();
                    ListViewItem item = new ListViewItem(mouzaName);
                    item.SubItems.Add(mouzaMasterId);
                    listViewMouza.Items.Add(item);
                }
                AdjustListView(listViewMouza);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to display mouza name : " + ex.Message);
            }
        }
        private DataTable GetBundleDetailsByMouzaMasterId(List<string> master_ids)
        {
            var store = new frmMouzaExportLogic(SqlCon);
            return store.GetBundleDetailsByMouzaMasterId(master_ids);
        }
        public void DisplayBundleName(List<string> master_ids)
        {
            try
            {
                var dt = GetBundleDetailsByMouzaMasterId(master_ids);
                listViewBundle.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    var bundleKey = row[0].ToString();
                    var bundleName = row[1].ToString();
                    var projCode = row[2].ToString();
                    var mouzaName = row[3].ToString();
                    var displayName = $"{bundleName} ({mouzaName})";
                    ListViewItem item = new ListViewItem(displayName);
                    item.Tag = (BundleName: bundleName, BundleKey: bundleKey, ProjCode: projCode);
                    listViewBundle.Items.Add(item);
                }
                AdjustListView(listViewBundle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to display Bundle name. " + ex.Message);
            }
        }
        public DataSet GetAllExportedImageAll(string proj_key, string batch_key, string filename)
        {
            string indexPageName = string.Empty;
            frmExportLogic ob = new frmExportLogic(SqlCon);
            return ob.GetAllExportedImageAll(proj_key, batch_key, filename);
        }
        public string GetPath(int prmProjKey, int prmBatchKey)
        {
            var wBatch = new wfeBatch(SqlCon);
            return wBatch.GetPath(prmProjKey, prmBatchKey);
        }
        private async void buttonValidate_Click(object sender, System.EventArgs e)
        {
            try
            {

                var frmMouzaExportLogic = new frmMouzaExportLogic(SqlCon);
                var anyChecked = listViewMouza.Items.Cast<ListViewItem>().Any(item => item.Checked);
                if (!anyChecked)
                {
                    MessageBox.Show("Please select at least one item to proceed.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Exporter = new Exporter(SqlCon, ExportState.Mouza);
                foreach (ListViewItem bundle in listViewBundle.Items)
                {
                    var (bundleName, bundleKey, ProjectKey) = ((string BundleName, string BundleKey, string ProjCode))bundle.Tag;

                    var allFileStatus = frmMouzaExportLogic.GetAllPolicyStatus(ProjectKey, bundleKey);
                    var restrictedStatuses = new List<int> { (int)eSTATES.POLICY_EXCEPTION, (int)eSTATES.POLICY_NOT_INDEXED };
                    var invalidFiles = allFileStatus.Where(x => restrictedStatuses.Contains(x.status)).ToList();
                    if (invalidFiles.Any())
                    {
                        var invalidFileNames = string.Join(", ", invalidFiles.Select(f => $"'{f.filename}' (Status: {f.status})"));
                        throw new Exception($"Files with invalid status found: {invalidFileNames}");
                    }
                    var file = new wfePolicy(SqlCon, new CtrlPolicy(int.Parse(ProjectKey), Convert.ToInt32(bundleKey), ""));
                    var files = file.GetPolicyListImport(ProjectKey, bundleKey);
                    foreach (DataRow row in files.Tables[0].Rows)
                    {
                        var filename = row[0].ToString();
                        var dsImages = GetAllExportedImageAll(ProjectKey, bundleKey.ToString(), filename);
                        Debug.Assert(dsImages.Tables.Count > 0);
                        var exportableImagesTbl = dsImages.Tables[0];

                        foreach (DataRow exportableImage in exportableImagesTbl.Rows)
                        {
                            var imageFileName = exportableImage.Field<string>("page_index_name");
                            var imageFilePath = Path.Combine(GetPath(Convert.ToInt32(ProjectKey), int.Parse(bundleKey)), row[0].ToString(), "QC", imageFileName);
                            var docType = exportableImage.Field<string>("doc_type");
                            var projectName = GetProjectName(ProjectKey);
                            Exporter.AddImage(projectName, int.Parse(ProjectKey), bundleName, Convert.ToInt32(bundleKey), filename, imageFilePath, docType);
                        }
                    }
                }
                var postValidationResult = await Exporter.ValidatePostconditions();
                if (IsExportValid(postValidationResult))
                {
                    UpdateExportProgress("Validation success", 1, 1, 1, 1);
                    SetExportValidateButtons(true);
                    buttonExport.Focus();
                    btnCancel.Enabled = false;

                    buttonValidate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to validate : " + ex.Message);
            }
        }
        private static bool IsExportValid(ExportValidationResult validationResult)
        {
            if (validationResult.Errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", validationResult.Errors), "Error: Can't continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (validationResult.Warnings.Count > 0)
            {
                var cont = MessageBox.Show(string.Join("\n", validationResult.Warnings), "Warning: Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return cont == DialogResult.Yes;
            }
            return true;
        }
        private void SetExportValidateButtons(bool isEnable)
        {
            buttonExport.Enabled = isEnable;
            buttonValidate.Enabled = isEnable;
        }
        private async void buttonExport_Click(object sender, EventArgs e)
        {
            var exportTasks = Exporter.Export(Path.Combine(@"C:\", "Nevaeh"), Credential);
            try
            {
                while (!exportTasks.IsEmpty())
                {
                    SetExportValidateButtons(false);
                    var (groupName, finishedTask) = await exportTasks.WhenAny();
                    btnCancel.Enabled = (groupName.Equals("PDF generation")) ? false : true;
                    await finishedTask;
                    UpdateExportProgress(
                        groupName, exportTasks.CurrentStage.GetFinishedTaskCount(groupName),
                        exportTasks.CurrentStage.GetTotalTaskCount(groupName),
                        exportTasks.GetFinishedTaskCount(),
                        exportTasks.GetTotalTaskCount());
                }

                foreach (ListViewItem batchName in listViewBundle.Items)
                {
                    var (bundleName, bundleKey, ProjectKey) = ((string BundleName, string BundleKey, string ProjCode))batchName.Tag;
                    string projectName = GetProjectName(ProjectKey);
                    var exportedFiles = Exporter.GetFiles(projectName, bundleName);
                    foreach (var fileName in exportedFiles)
                    {
                        UpdateFileStatus(projectName, bundleName, fileName, Credential);
                    }
                    UpdateBatchStatus(projectName, bundleName, Credential);
                }
                SetExportValidateButtons(true);

                btnCancel.Enabled = false;
                buttonExport.Enabled = false;
                btnClose.Enabled = true;
                btnClose.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetProjectName(string projectKey)
        {
            var store = new frmMouzaExportLogic(SqlCon);
            var dt = store.getProjectName(projectKey);
            string projectName = dt.Rows[0]["proj_code"].ToString();
            return projectName;
        }
        private void UpdateBatchStatus(string projectName, string batchName, Credentials cred)
        {
            var projectKey = Exporter.GetProjectKey(projectName);
            var batchKey = Exporter.GetBatchKey(projectName, batchName);
            var ctlPolicy = new CtrlPolicy(projectKey, batchKey, "");
            var file = new wfePolicy(SqlCon, ctlPolicy);
            file.UpdateBatchStatus(eSTATES.BATCH_EXPORTED, cred);
            file.UpdateExportLog(projectKey, batchKey, cred, Exporter.GetFiles(projectName, batchName).Count());
        }
        private void UpdateFileStatus(string projectName, string batchName, string fileName, Credentials cred)
        {
            var ctlPolicy = new CtrlPolicy(Exporter.GetProjectKey(projectName), Exporter.GetBatchKey(projectName, batchName), "", fileName);
            var file = new wfePolicy(SqlCon, ctlPolicy);
            file.UpdateStatus(eSTATES.POLICY_EXPORTED, cred);
        }
        private void UpdateExportProgress(string groupName, int groupFinishedCount, int groupTotalCount, int finishedCount, int totalCount)
        {
            labelBundle.Text = $"{groupName}: ({groupFinishedCount}/{groupTotalCount})";
            labelFinal.Text = $"{finishedCount}/{totalCount}";
            progressBarBundle.Value = (int)Math.Round((double)groupFinishedCount * 100 / groupTotalCount);
            progressBarFinal.Value = (int)Math.Round((double)finishedCount * 100 / totalCount);
        }
        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = checkBoxSelectAll.Checked;
            foreach (ListViewItem item in listViewMouza.Items)
            {
                item.Checked = isChecked;
            }
        }

        private void listViewMouza_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var master_ids = listViewMouza.Items.Cast<ListViewItem>()
                .Where(x => x.Checked)
                .Select(item => item.SubItems[1].Text)
                .ToList();
            if (master_ids.Count > 0)
            {
                DisplayBundleName(master_ids);
            }
            else
            {
                listViewBundle.Items.Clear();
            }
            DisplayItemsCount();
        }
        private void DisplayItemsCount()
        {
            MouzaCount = listViewMouza.Items.Cast<ListViewItem>().Count();
            BundleCount = listViewBundle.Items.Cast<ListViewItem>().Count();
            var checkedMouzaCount = listViewMouza.Items.Cast<ListViewItem>()
                .Where(x => x.Checked)
                .Count();
            tSSLMouzaCount.Text = checkedMouzaCount > 0 ? $"Selected mouza : {checkedMouzaCount}/{MouzaCount}" : $"Mouza Count : {MouzaCount}";
            tSSLBundleCount.Text = $"Bundle count : {BundleCount}";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var diaResult = MessageBox.Show($"Do you really want to cancel {lastClickedButtonName}?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (diaResult == DialogResult.Yes)
            {
                Exporter.CancelTasks();
                SetExportValidateButtons(true);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void frmMouzaExport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!btnCancel.Enabled)
            {
                DialogResult = DialogResult.Abort;
            }
            else
            {
                var diaResult = MessageBox.Show("If you close, running task will be cancelled!!!\nDo you want to close?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (diaResult == DialogResult.Yes)
                {
                    DialogResult = CancelandAbort();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        private DialogResult CancelandAbort()
        {
            Exporter.CancelTasks();
            return DialogResult.Abort;
        }

        private void chkReExport_CheckedChanged(object sender, EventArgs e)
        {
            Reset();
            DisplayItemsCount();
        }

        private void comboBoxBlockSelection_Leave(object sender, EventArgs e)
        {
            if (comboBoxBlockSelection.SelectedIndex != -1)
            {
                DisplayMouzaForExport(chkReExport.Checked);
                DisplayItemsCount();
            }
        }
        private void ShowBlock()
        {
            MetadataStore metadatastore = new MetadataStore(SqlCon);
            var districts = metadatastore.GetMasterList(null, new List<string> { MetadataStore.FIELD_DISTRICT });
            foreach (var district in districts)
            {
                var blocks = metadatastore.GetMasterList(district.MasterID, new List<string> { MetadataStore.FIELD_BLOCK });
                var blockArrLists = blocks.Select(x =>
                     new { MasterId = x.MasterID, Value = x.Value.BengaliText }
                     )
                .ToList();

                comboBoxBlockSelection.DataSource = blockArrLists;
                comboBoxBlockSelection.DisplayMember = "Value";
                comboBoxBlockSelection.ValueMember = "MasterId";
            }
        }
        private void AdjustListView(ListView listview)
        {
            listview.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

    }
}