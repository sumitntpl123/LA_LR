using bzer_data_entry.src;
using LItems;
using NovaNet.wfe;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Reporting
{
    public partial class UATBatchDetailsPanel : UserControl
    {
        public UATBatchDetailsPanel()
        {
            InitializeComponent();
        }

        public void LoadBatchDetails(int projectID, int batchID, OdbcConnection sqlConn, OdbcTransaction sqlTran = null)
        {
            var rptDataStore = new ReportingDataStore(sqlConn, sqlTran);
            var btDataStore = new BatchDataStore(sqlConn, sqlTran);
            //var (_, minKhatian, minBataKhatian) = rptDataStore.GetMinimumKhatianEntry(projectID, batchID);
            //var (_, maxKhatian, maxBataKhatian) = rptDataStore.GetMaximumKhatianEntry(projectID, batchID);
            //lblStartKhatianVal.Text = GetKhatian(minKhatian, minBataKhatian);
            //lblEndKhatianVal.Text = GetKhatian(maxKhatian, maxBataKhatian);
            var batchDetails = btDataStore.GetBatchMetadata(projectID, batchID);
            //var missingKhatians = rptDataStore.GetMissingKhatianNumbers(batchDetails.Mouza.MasterID);
            //lblKhMissingCountVal.Text = missingKhatians.Count.ToString();
            //lsMissingKhatians.Items.Clear();
            //lsMissingKhatians.Items.AddRange(missingKhatians.Select(khNo => new ListViewItem(khNo.ToString())).ToArray());
            //lblTotalKhatiansVal.Text = rptDataStore.GetFileCountByStatus(new List<eSTATES> { }, projectID, batchID).ToString();
            var scannedKhatianCount = rptDataStore.GetFileCountByStatus(new List<eSTATES> {
                eSTATES.POLICY_SCANNED,
                eSTATES.POLICY_QC,
                eSTATES.POLICY_INDEXED,
                eSTATES.POLICY_FQC,
                eSTATES.POLICY_EXPORTED,
                eSTATES.POLICY_NOT_INDEXED,
                eSTATES.POLICY_CONFLICT,
                eSTATES.POLICY_EXCEPTION,
                eSTATES.POLICY_CHECKED
            }, projectID, batchID);
            //lblKhScanCountVal.Text = scannedKhatianCount.ToString();
            lblKhCheckedVal.Text = rptDataStore.GetFileCountByStatus(new List<eSTATES> {
                eSTATES.POLICY_CHECKED,
                eSTATES.POLICY_EXPORTED
            }, projectID, batchID).ToString();
            var (numImages, batchSize) = GetImageSizeForBatch(projectID, batchID, sqlConn, sqlTran);
            //lblBatchSzVal.Text = $"{batchSize / 1024: 0.##} KB";
            lblAvgKhSzVal.Text = $"{batchSize / scannedKhatianCount / 1024: 0.##} KB";
            lblAvgImgSzVal.Text = $"{batchSize / numImages / 1024: 0.##} KB";
            lblScanDPIVal.Text = "200";
        }

        private (int numImages, double totalImageSize) GetImageSizeForBatch(int projectID, int batchID, OdbcConnection sqlConn, OdbcTransaction sqlTran = null)
        {
            var rptDataStore = new ReportingDataStore(sqlConn, sqlTran);
            var batchPath = new wfeBatch(sqlConn, sqlTran).GetPath(projectID, batchID);
            var imagePaths = rptDataStore.GetImages(projectID, batchID, states: new List<eSTATES> {
                eSTATES.PAGE_CREATED,
                eSTATES.PAGE_SCANNED,
                eSTATES.PAGE_QC,
                eSTATES.PAGE_INDEXED,
                eSTATES.PAGE_FQC,
                eSTATES.PAGE_EXCEPTION,
                eSTATES.PAGE_CHECKED
            });
            double totalImageSize = imagePaths.Select(path => Path.Combine(batchPath, path.FileName, "QC", path.ImagePath)).Sum(imagePath => new FileInfo(imagePath).Length);
            return (numImages: imagePaths.Count, totalImageSize);
        }

        private static string GetKhatian(string khatian, string bataKhatian)
        {
            return $"{khatian}{(!string.IsNullOrEmpty(bataKhatian) ? $"/{bataKhatian}" : "")}";
        }
    }
}
