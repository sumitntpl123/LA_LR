using ImageHeaven.Logic;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
    public partial class frmExport : Form
    {
        public static NovaNet.Utils.exLog.Logger ExMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);

        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);

        private readonly OdbcConnection SqlCon;

        private readonly Credentials Credential;

        private IExporter Exporter;

        private string lastClickedButtonName = null;

        public frmExport(OdbcConnection prmCon, Credentials prmCrd)
        {
            Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            ExMailLog.SetNextLogger(exTxtLog);
            InitializeComponent();
            SqlCon = prmCon;
            Credential = prmCrd;
            AttachClickHandlers();
        }

        private void AttachClickHandlers()
        {
            btnValidatefiles.Click += Button_Click;
            btnExport.Click += Button_Click;
        }
        private void Button_Click(object sender, EventArgs e)
        {
            lastClickedButtonName = (sender as Button)?.Text.Substring(1);
        }

        private void LoadProject()
        {
            var proj = new wfeProject(SqlCon);
            var projects = proj.GetAllValues().Tables.Cast<DataTable>()
                .SelectMany(tab => tab.Rows.Cast<DataRow>().Select(row => new IDName { ID = row.Field<int>("proj_key"), Name = row.Field<string>("proj_code") }))
                .ToList();
            cmbProject.DataSource = projects;
            cmbProject.DisplayMember = "Name";
            cmbProject.ValueMember = "ID";

            cmbProject.Select();
        }

        private void LoadBatch(int projectKey, bool reExport)
        {
            var batchStates = reExport ?
                new eSTATES[] { eSTATES.BATCH_EXPORTED } :
                new eSTATES[] { eSTATES.BATCH_READY_FOR_UAT };
            var batch = new wfeBatch(SqlCon);
            var batches = batch.GetAllValues(projectKey, batchStates).Tables.Cast<DataTable>()
                .SelectMany(tab => tab.Rows.Cast<DataRow>().Select(row => new IDName { ID = row.Field<int>("bundle_key"), Name = row.Field<string>("bundle_name") }))
                .ToList();
            cmbBatch.DataSource = batches;
            cmbBatch.DisplayMember = "Name";
            cmbBatch.ValueMember = "ID";

            cmbBatch.Select();
        }

        private void LoadFile(int projectKey, int batchKey, bool reExport)
        {
            var states = reExport ? new eSTATES[] { eSTATES.POLICY_EXPORTED } : new eSTATES[] { eSTATES.POLICY_FQC, eSTATES.BATCH_QC, eSTATES.POLICY_CHECKED };
            var file = new wfePolicy(SqlCon, new CtrlPolicy(projectKey, batchKey, ""));
            var files = file.GetFileList(states);

            Debug.Assert(files.Tables.Count == 1);
            dgvbatch.DataSource = files.Tables[0];
            UpdateCounters(makeVisible: true);
        }

        private void LoadFilesToExport(int projectKey, int batchKey, List<string> fileNames, DataTable tab)
        {
            var file = new wfePolicy(SqlCon);
            foreach (var fileName in fileNames)
            {
                var fileStatus = file.GetFileStatus(projectKey, batchKey, fileName);
                tab.Rows.Add(fileName, fileStatus == (int)eSTATES.POLICY_EXPORTED ? "Y" : "N");
            }

            dgvexport.DataSource = tab;
        }

        private void Reset()
        {
            cmbProject.SelectedIndex = -1;
            cmbBatch.SelectedIndex = -1;
            dgvbatch.DataSource = null;
            dgvexport.DataSource = null;
            btnExport.Enabled = false;
            btnCancel.Enabled = false;
            btnClose.Enabled = true;

            UpdateCounters(makeVisible: false);

            LoadProject();
        }

        private void UpdateCounters(bool makeVisible)
        {
            lbldeedCount.Text = dgvbatch.RowCount.ToString();
            lblBatchSelected.Text = dgvexport.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Count().ToString();
            lblTotal.Visible = makeVisible;
            lblSelected.Visible = makeVisible;
            lbldeedCount.Visible = makeVisible;
            lblBatchSelected.Visible = makeVisible;
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

        private void UpdateExportProgress(string groupName, int groupFinishedCount, int groupTotalCount, int finishedCount, int totalCount)
        {
            lblFileStatus.Text = $"{groupName}: ({groupFinishedCount}/{groupTotalCount})";
            lblFinalStatus.Text = $"{finishedCount}/{totalCount}";
            progressBarFile.Value = (int)Math.Round((double)groupFinishedCount * 100 / groupTotalCount);
            progressBarFinal.Value = (int)Math.Round((double)finishedCount * 100 / totalCount);
        }

        private void UpdateFileStatus(string projectName, string batchName, string fileName, Credentials cred)
        {
            var ctlPolicy = new CtrlPolicy(Exporter.GetProjectKey(projectName), Exporter.GetBatchKey(projectName, batchName), "", fileName);
            var file = new wfePolicy(SqlCon, ctlPolicy);
            file.UpdateStatus(eSTATES.POLICY_EXPORTED, cred);
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

        private void frmExport_Load(object sender, EventArgs e)
        {
            Reset();
            chkReExport.Checked = false;
        }

        private void cmbProject_Leave(object sender, EventArgs e)
        {
            if (cmbProject.SelectedIndex != -1)
            {
                LoadBatch((int)cmbProject.SelectedValue, chkReExport.Checked);
            }
        }

        private void cmbBatch_Leave(object sender, EventArgs e)
        {
            if (cmbProject.SelectedIndex != -1 && cmbBatch.SelectedIndex != -1)
            {
                LoadFile((int)cmbProject.SelectedValue, (int)cmbBatch.SelectedValue, chkReExport.Checked);
            }
        }

        private void chkReExport_CheckedChanged(object sender, EventArgs e)
        {
            Reset();
        }
        private void dgvbatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                SelectedExpotableFilenames();
            }
        }

        private void dgvbatch_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            SelectedExpotableFilenames();
        }

        private void SelectedExpotableFilenames()
        {
            if (cmbProject.SelectedIndex != -1 && cmbBatch.SelectedIndex != -1 && dgvbatch.SelectedCells.Count > 0)
            {
                var tab = new DataTable();
                tab.Columns.Add("File Name");
                tab.Columns.Add("Exported");

                var exportFileNames = dgvbatch.SelectedCells
                    .Cast<DataGridViewCell>()
                    .Select(cell => cell.OwningRow)
                    .Distinct()
                    .Select(row => row.Cells["filename"].Value as string)
                    .ToList();
                LoadFilesToExport((int)cmbProject.SelectedValue, (int)cmbBatch.SelectedValue, exportFileNames, tab);
            }
        }

        private void dgvexport_DataSourceOrSelectionChanged(object sender, EventArgs e)
        {
            UpdateCounters(makeVisible: true);
            btnExport.Enabled = false;
        }

        //private async void cmdValidatefiles_Click(object sender, EventArgs e)
        //{
        //    btnClose.Enabled = false;
        //    btnCancel.Enabled = true;
        //    SetExportValidateButtons(false);

        //    var projectName = cmbProject.Text;
        //    var batchName = cmbBatch.Text;
        //    var projectKey = (int)cmbProject.SelectedValue;
        //    var batchKey = (int)cmbBatch.SelectedValue;
        //    Exporter = new Exporter(SqlCon, ExportState.Bundle);

        //    var preValidationResult = await Exporter.ValidatePreconditions();
        //    if (IsExportValid(preValidationResult))
        //    {
        //        foreach (DataGridViewRow exportableFile in dgvexport.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow))
        //        {
        //            var fileName = exportableFile.Cells["File Name"].Value as string;
        //            var dsImages = GetAllExportedImageAll(projectKey.ToString(), batchKey.ToString(), fileName);
        //            Debug.Assert(dsImages.Tables.Count > 0);
        //            var exportableImagesTbl = dsImages.Tables[0];

        //            foreach (DataRow exportableImage in exportableImagesTbl.Rows)
        //            {
        //                var imageFileName = exportableImage.Field<string>("page_index_name");
        //                // TODO: move this into a application-wide function
        //                var imageFilePath = Path.Combine(GetPath(projectKey, batchKey), fileName, "QC", imageFileName);
        //                var docType = exportableImage.Field<string>("doc_type");

        //                Exporter.AddImage(projectName, projectKey, batchName, batchKey, fileName, imageFilePath, docType);
        //            }
        //        }

        //        var postValidationResult = await Exporter.ValidatePostconditions();
        //        if (IsExportValid(postValidationResult))
        //        {
        //            UpdateExportProgress("Validation success", 1, 1, 1, 1);
        //            SetExportValidateButtons(true);
        //            btnExport.Focus();
        //            btnClose.Enabled = true;
        //            btnCancel.Enabled = false;
        //        }
        //    }
        //}
        private async void cmdValidatefiles_Click(object sender, EventArgs e)
        {
            btnClose.Enabled = false;
            btnCancel.Enabled = true;

            SetExportValidateButtons(false);

            var projectName = cmbProject.Text;
            var batchName = cmbBatch.Text;

            var projectKey = (int)cmbProject.SelectedValue;
            var batchKey = (int)cmbBatch.SelectedValue;

            Exporter = new Exporter(SqlCon, ExportState.Bundle);

            var preValidationResult = await Exporter.ValidatePreconditions();

            if (!IsExportValid(preValidationResult))
                return;

            foreach (DataGridViewRow exportableFile in dgvexport.SelectedCells
                         .Cast<DataGridViewCell>()
                         .Select(cell => cell.OwningRow)
                         .Distinct())
            {
                var fileName = exportableFile.Cells["File Name"].Value as string;

                var dsImages = GetAllExportedImageAll(
                    projectKey.ToString(),
                    batchKey.ToString(),
                    fileName);

                Debug.Assert(dsImages.Tables.Count > 0);

                var exportableImagesTbl = dsImages.Tables[0];

                // For combined PDF
                List<string> allImages = new List<string>();

                // For doc type wise PDF
                Dictionary<string, List<string>> docTypeImages =
                    new Dictionary<string, List<string>>();

                foreach (DataRow exportableImage in exportableImagesTbl.Rows)
                {
                    var imageFileName =
                        exportableImage.Field<string>("page_index_name");

                    var imageFilePath = Path.Combine(
                        GetPath(projectKey, batchKey),
                        fileName,
                        "QC",
                        imageFileName);

                    var docType =
                        exportableImage.Field<string>("doc_type");

                    // Add into exporter
                    Exporter.AddImage(
                        projectName,
                        projectKey,
                        batchName,
                        batchKey,
                        fileName,
                        imageFilePath,
                        docType);

                    // Single PDF
                    allImages.Add(imageFilePath);

                    // DocType PDF
                    if (!docTypeImages.ContainsKey(docType))
                    {
                        docTypeImages[docType] = new List<string>();
                    }

                    docTypeImages[docType].Add(imageFilePath);
                }

                // Store all images
                Exporter.AllPdfImages[fileName] = allImages;

                // Store doc type wise images
                Exporter.DocTypePdfImages[fileName] = docTypeImages;
            }

            var postValidationResult = await Exporter.ValidatePostconditions();

            if (IsExportValid(postValidationResult))
            {
                UpdateExportProgress(
                    "Validation success",
                    1,
                    1,
                    1,
                    1);

                SetExportValidateButtons(true);

                btnExport.Focus();

                btnClose.Enabled = true;
                btnCancel.Enabled = false;
            }
        }

        private async void btnExport_Click_1(object sender, EventArgs e)
        {
            btnClose.Enabled = false;
            btnCancel.Enabled = false;

            var projectName = cmbProject.Text;
            var batchName = cmbBatch.Text;

            DeletePreviousOrCorruptedBundle();

            // TODO: move this into a application-wide function
            var exportTasks = Exporter.Export(Path.Combine(@"C:\", "Nevaeh", projectName), Credential);

            try
            {
                while (!exportTasks.IsEmpty())
                {
                    SetExportValidateButtons(false);
                    var (groupName, finishedTask) = await exportTasks.WhenAny();
                    btnCancel.Enabled = (groupName.Equals("PDF generation")) ? false : true;
                    await finishedTask;
                    foreach (var fileItem in Exporter.DocTypePdfImages)
                    {
                        var fileName = fileItem.Key;
                        var docTypes = fileItem.Value;

                        foreach (var docTypeItem in docTypes)
                        {
                            var docType = docTypeItem.Key;
                            var images = docTypeItem.Value;

                            var outputPdf = Path.Combine(
                                @"C:\Nevaeh\", projectName+"\\" + batchName,
                                fileName + "_" + docType + ".pdf");

                            CreatePdf(images, outputPdf);
                        }
                    }
                    UpdateExportProgress(
                        groupName, exportTasks.CurrentStage.GetFinishedTaskCount(groupName),
                        exportTasks.CurrentStage.GetTotalTaskCount(groupName),
                        exportTasks.GetFinishedTaskCount(),
                        exportTasks.GetTotalTaskCount());
                }

                var exportedFiles = Exporter.GetFiles(projectName, batchName);
                foreach (var fileName in exportedFiles)
                {
                    UpdateFileStatus(projectName, batchName, fileName, Credential);
                }
                if (dgvbatch.RowCount == exportedFiles.Count())
                {
                    UpdateBatchStatus(projectName, batchName, Credential);
                }
                SetExportValidateButtons(true);

                btnCancel.Enabled = false;
                btnClose.Enabled = true;
                btnClose.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExMailLog.Log($"ERROR EXPORT [{projectName}/{batchName}]: {ex.Message}");
            }
        }
        private void CreatePdf(List<string> imagePaths, string outputPdfPath)
        {
            if (imagePaths == null || imagePaths.Count == 0)
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(outputPdfPath));

            using (FileStream stream = new FileStream(outputPdfPath, FileMode.Create))
            {
                Document document = new Document(PageSize.A4);

                PdfWriter.GetInstance(document, stream);

                document.Open();

                foreach (var imagePath in imagePaths)
                {
                    if (!File.Exists(imagePath))
                        continue;

                    Image image = Image.GetInstance(imagePath);

                    image.ScaleToFit(
                        document.PageSize.Width - 20,
                        document.PageSize.Height - 20);

                    image.Alignment = Element.ALIGN_CENTER;

                    document.Add(image);

                    document.NewPage();
                }

                document.Close();
            }
        }
        //private async void btnExport_Click_1(object sender, EventArgs e)
        //{
        //    btnClose.Enabled = false;
        //    btnCancel.Enabled = false;

        //    var projectName = cmbProject.Text;
        //    var batchName = cmbBatch.Text;

        //    DeletePreviousOrCorruptedBundle();

        //    var exportTasks = Exporter.Export(
        //        Path.Combine(@"C:\", "Nevaeh", projectName),
        //        Credential);

        //    try
        //    {
        //        while (!exportTasks.IsEmpty())
        //        {
        //            SetExportValidateButtons(false);

        //            var (groupName, finishedTask) =
        //                await exportTasks.WhenAny();

        //            btnCancel.Enabled =
        //                groupName.Equals("PDF generation")
        //                    ? false
        //                    : true;

        //            await finishedTask;

        //            UpdateExportProgress(
        //                groupName,
        //                exportTasks.CurrentStage.GetFinishedTaskCount(groupName),
        //                exportTasks.CurrentStage.GetTotalTaskCount(groupName),
        //                exportTasks.GetFinishedTaskCount(),
        //                exportTasks.GetTotalTaskCount());
        //        }

        //        // Create Single PDFs
        //        foreach (var fileItem in Exporter.GetFiles(projectName, batchName);)
        //        {
        //            var fileName = fileItem.Key;
        //            var images = fileItem.Value;

        //            var outputPdf = Path.Combine(
        //                @"C:\PDF",
        //                fileName + "_ALL.pdf");

        //            CreatePdf(images, outputPdf);
        //        }

        //        // Create DocType Wise PDFs
        //        foreach (var fileItem in Exporter.DocTypePdfImages)
        //        {
        //            var fileName = fileItem.Key;
        //            var docTypes = fileItem.Value;

        //            foreach (var docTypeItem in docTypes)
        //            {
        //                var docType = docTypeItem.Key;
        //                var images = docTypeItem.Value;

        //                var outputPdf = Path.Combine(
        //                    @"C:\PDF",
        //                    fileName + "_" + docType + ".pdf");

        //                CreatePdf(images, outputPdf);
        //            }
        //        }

        //        var exportedFiles =
        //            Exporter.GetFiles(projectName, batchName);

        //        foreach (var fileName in exportedFiles)
        //        {
        //            UpdateFileStatus(
        //                projectName,
        //                batchName,
        //                fileName,
        //                Credential);
        //        }

        //        if (dgvbatch.RowCount == exportedFiles.Count())
        //        {
        //            UpdateBatchStatus(
        //                projectName,
        //                batchName,
        //                Credential);
        //        }

        //        SetExportValidateButtons(true);

        //        btnCancel.Enabled = false;

        //        btnClose.Enabled = true;
        //        btnClose.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            ex.Message,
        //            "Error in Export",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);

        //        ExMailLog.Log(
        //            $"ERROR EXPORT [{projectName}/{batchName}]: {ex.Message}");
        //    }
        //}
        private void SetExportValidateButtons(bool isEnable)
        {
            btnExport.Enabled = isEnable;
            btnValidatefiles.Enabled = isEnable;
        }

        private void DeletePreviousOrCorruptedBundle()
        {
            var exportPath = Path.Combine(@"C:\", "Nevaeh", cmbProject.Text, cmbBatch.Text);
            if (Directory.Exists(exportPath))
            {
                Directory.Delete(exportPath, true);
            }
        }

        private void cmdCancel_Click_1(object sender, EventArgs e)
        {
            var diaResult = MessageBox.Show($"Do you really want to cancel {lastClickedButtonName}?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (diaResult == DialogResult.Yes)
            {
                Exporter.CancelTasks();
                SetExportValidateButtons(true);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void frmExport_FormClosing(object sender, FormClosingEventArgs e)
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
    }
    internal class IDName
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}