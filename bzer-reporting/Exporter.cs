using bzer_data_entry;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NovaNet.Utils.ExporterUtils;

namespace Reporting
{
    public class Exporter : AbstractExporter
    {
        const int PDF_IMAGE_QUALITY = 80;

        private readonly OdbcConnection SqlCon;

        private readonly object SqlConLock;

        private Dictionary<string, CancellationTokenSource> taskControllers;

        private ExportState exportState;

        public Dictionary<string, List<string>> AllPdfImages { get; set; }
            = new Dictionary<string, List<string>>();

        public Dictionary<string, Dictionary<string, List<string>>> DocTypePdfImages { get; set; }
            = new Dictionary<string, Dictionary<string, List<string>>>();
        public Exporter(OdbcConnection sqlCon, ExportState eState)
        {
            SqlCon = sqlCon;
            SqlConLock = new object();
            taskControllers = new Dictionary<string, CancellationTokenSource>();
            exportState = eState;
        }

        public override Task<ExportValidationResult> ValidatePostconditions()
        {
            var taskControllerVPost = new CancellationTokenSource();
            taskControllers.Add("ValidatePost", taskControllerVPost);
            return Task.Run(() =>
            {
                var result = new ExportValidationResult();
                var store = new MetadataStore(SqlCon);
                foreach (var projectName in GetProjects())
                {
                    foreach (var batchName in GetBatches(projectName))
                    {
                        var batch = new wfeBatch(SqlCon);
                        var batchStatus = batch.GetBundleStatus(GetProjectKey(projectName), GetBatchKey(projectName, batchName));
                        var batchState = (eSTATES)batchStatus;
                        if (batchState == eSTATES.BATCH_EXPORTED)
                        {
                            result.Warnings.Add($"Bundle {batchName} is already exported, Re-exporting");
                        }
                        foreach (var fileName in GetFiles(projectName, batchName))
                        {
                            var fileID = store.GetFileFromFileName(fileName);
                            if (!fileID.HasValue)
                            {
                                result.Errors.Add($"Missing file data for file {fileName}");
                            }
                            foreach (var imagePath in GetImages(projectName, batchName, fileName))
                            {

                                if (!File.Exists(imagePath))
                                {
                                    result.Errors.Add($"Missing image {imagePath} for batch {batchName}");
                                }

                            }
                            if (taskControllerVPost.IsCancellationRequested)
                            {
                                break;
                            }
                        }
                    }
                }
                return result;
            }, taskControllerVPost.Token);
        }

        public override Task<ExportValidationResult> ValidatePreconditions()
        {
            return Task.Run(() =>
            {
                var result = new ExportValidationResult();
                return result;
            });
        }

        public override ExportTaskResult Export(string exportPath, Credentials cred)
        {

            var store = new MetadataStore(SqlCon);
            var taskControllerKeystoRemove = taskControllers.Where(item => !item.Key.StartsWith("Validate")).ToList();
            if (taskControllerKeystoRemove.Count > 0)
            {
                taskControllerKeystoRemove.ForEach(item => taskControllers.Remove(item.Key));
            }

            var result = new ExportTaskResult();
            var artifactGenStage = result.AddStage();
            foreach (var projectName in GetProjects())
            {
                foreach (var batchName in GetBatches(projectName))
                {
                    string IDN = null;
                    if (exportState == ExportState.Mouza)
                    {
                        var fileNames = GetFiles(projectName, batchName);
                        var firstFile = fileNames.FirstOrDefault();
                        if (firstFile == null)
                        {
                            throw new Exception($"No files found for project '{projectName}' and batch '{batchName}' in Mouza export.");
                        }
                        IDN = GenerateIdnFromFilename(store, firstFile);
                    }
                    foreach (var fileName in GetFiles(projectName, batchName))
                    {

                        artifactGenStage.AddTask("PDF generation", Task.Run(() => CombinePDFs(exportPath, projectName, batchName, fileName, IDN)));

                    }
                }
            }

            //var taskControllerProjaGen = new CancellationTokenSource();
            //taskControllers.Add("Proja Type CSV generation", taskControllerProjaGen);
            //artifactGenStage.AddTask("Proja Type CSV generation", Task.Run(() => GenerateProjaTypeCSV(exportPath, taskControllerProjaGen.Token), taskControllerProjaGen.Token));

            var taskControllerCSVGen = new CancellationTokenSource();
            taskControllers.Add("CSV generation", taskControllerCSVGen);
            artifactGenStage.AddTask("CSV generation", Task.Run(() => GenerateCSV(exportPath, taskControllerCSVGen.Token), taskControllerCSVGen.Token));
            return result;

        }
        private string GenerateIdnFromFilename(MetadataStore store, string filename)
        {
            var fileID = store.GetFileFromFileName(filename);
            var (_, _, fileData) = store.GetFile(fileID.Value);
            return GenerateFileName(store, fileData.District.MasterID, fileData.Block.MasterID, fileData.Mouza.MasterID);
        }
        private void CombinePDFs(string exportDir, string projectName, string batchName, string fileName, string IDN = null)
        {
            var folderName = IDN != null ? IDN : batchName;
            var plainPDFPath = Path.Combine(exportDir, folderName, $"{fileName}.pdf");
            Directory.CreateDirectory(Path.GetDirectoryName(plainPDFPath));
            if (File.Exists(plainPDFPath))
            {
                File.Delete(plainPDFPath);
            }
            var t = GetImages(projectName, batchName, fileName).ToArray();
            var imgLib = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            if (!imgLib.TifToPdf(GetImages(projectName, batchName, fileName).ToArray(), PDF_IMAGE_QUALITY, plainPDFPath))
            {
                throw new Exception($"Error combining batch images to plain PDF: {plainPDFPath}");
            }
        }
        private bool AllFilesExist(params string[] paths)
        {
            return paths.All(File.Exists);
        }
        private void GenerateProjaTypeCSV(string exportPath, CancellationToken token)
        {
            lock (SqlConLock)
            {
                using (dbCon dbConProjaCSV = new dbCon())
                {
                    var sqlConProjaCSV = dbConProjaCSV.Connect();
                    var store = new MetadataStore(sqlConProjaCSV);
                    foreach (var projectName in GetProjects())
                    {
                        var data = GetBatches(projectName);
                        foreach (var batchName in GetBatches(projectName))
                        {
                            string IDN = null;
                            if (exportState == ExportState.Mouza)
                            {
                                var fileNames = GetFiles(projectName, batchName);
                                var firstFile = fileNames.FirstOrDefault();

                                if (firstFile == null)
                                {
                                    return;
                                }
                                IDN = GenerateIdnFromFilename(store, firstFile);
                            }
                            var folderName = IDN != null ? IDN : batchName;
                            var csProjaTypeCSVPath = Path.Combine(exportPath, folderName, "cs_ProjaType.csv");
                            Directory.CreateDirectory(Path.GetDirectoryName(csProjaTypeCSVPath));

                            bool fileExists = AllFilesExist(csProjaTypeCSVPath);

                            using (var csProjaTypeCSVFile = File.Open(csProjaTypeCSVPath, FileMode.Append, FileAccess.Write))
                            using (var csProjaTypeCSV = new StreamWriter(csProjaTypeCSVFile, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true)))
                            {
                                if (!fileExists)
                                {
                                    WriteCSVRow(csProjaTypeCSV, "projaCD", "projaName_beng", "projaName_Eng", "projaName_Dev");
                                }
                                foreach (var fileName in GetFiles(projectName, batchName))
                                {
                                    var fileID = store.GetFileFromFileName(fileName);
                                    var (_, _, fileData) = store.GetFile(fileID.Value);
                                    var projaCD = store.GetMasterCode(fileData.ProjaSotterType.MasterID);
                                    var projaNameBeng = fileData.ProjaSotterType.Value.BengaliText;
                                    var projaNameEng = fileData.ProjaSotterType.Value.BengaliPhoneticText;
                                    var projaNameHindi = "";

                                    WriteCSVRow(csProjaTypeCSV, projaCD, projaNameBeng, projaNameEng, projaNameHindi);

                                    if (token.IsCancellationRequested)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void GenerateCSV(string exportPath, CancellationToken token)
        {
            lock (SqlConLock)
            {
                using (dbCon dbConNameCSV = new dbCon())
                {
                    var sqlConNameCSV = dbConNameCSV.Connect();
                    var store = new MetadataStore(sqlConNameCSV);

                    foreach (var projectName in GetProjects())
                    {
                        foreach (var batchName in GetBatches(projectName))
                        {
                            string IDN = null;
                            var khatianIndex = batchName.IndexOf('_') + 1;
                            var fileNames = GetFiles(projectName, batchName)
                                            .OrderBy(file =>
                                            {
                                                try
                                                {
                                                    var khatianWithBataKhatian = file.Substring(khatianIndex);
                                                    var khatians = khatianWithBataKhatian.Split('_');
                                                    return (int.Parse(khatians[0]), int.Parse(khatians.ElementAtOrDefault(1) ?? "-1"));
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw new Exception($"Error while parsing file '{file}'\n{ex.Message}");
                                                }
                                            });
                            var firstFile = fileNames.FirstOrDefault();

                            if (firstFile == null)
                            {
                                return;
                            }
                            IDN = GenerateIdnFromFilename(store, firstFile);

                            var folderName = exportState == ExportState.Mouza ? IDN : batchName;
                            var batchPath = Path.Combine(exportPath, folderName);
                            //var csNameCSVPath = Path.Combine(exportPath, folderName, "cs_Name.csv");
                            var csTF1CSVPath = Path.Combine(exportPath, folderName, "LA_Identification.csv");
                            var csTF2CSVPath = Path.Combine(exportPath, folderName, "LA_Plot_schedule.csv");
                            var csTF2FilePath = Path.Combine(exportPath, folderName, "LA_File_Details.csv");
                            var csTF2FileUpload = Path.Combine(exportPath, folderName, "LA_fiile_upload.csv");
                            Directory.CreateDirectory(batchPath);

                            bool allFilesExist = AllFilesExist(csTF1CSVPath, csTF2CSVPath, csTF2FilePath);

                            using (var csTF1CSVFile = File.Open(csTF1CSVPath, FileMode.Append, FileAccess.Write))
                            using (var csTF2CSVFile = File.Open(csTF2CSVPath, FileMode.Append, FileAccess.Write))
                            using (var csTF2FilePath1 = File.Open(csTF2FilePath, FileMode.Append, FileAccess.Write))
                            using (var csTF2FilePathUpload = File.Open(csTF2FileUpload, FileMode.Append, FileAccess.Write))
                            using (var csTF1CSV = new StreamWriter(csTF1CSVFile, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true)))
                            using (var csTF2CSV = new StreamWriter(csTF2CSVFile, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true)))
                            using (var csTF2File = new StreamWriter(csTF2FilePath1, new UTF8Encoding(true)))
                            using (var csTF2Upload = new StreamWriter(csTF2FilePathUpload, new UTF8Encoding(true)))
                            {
                                if (!allFilesExist)
                                {
                                    //WriteCSVRow(csTF1CSV, "running_serial", "actno", "case_no", "Case_year","IDN","District_code","Block_code","Mouza_code","JL_NO","ps_code","Purpose","Requiring_Body","Office_code", "user_id", "log_date");

                                    //WriteCSVRow(csTF2CSV, "IDN","running_serial","case_no","plot_type",  "pt1", "pt2", "ptsr", "Acquired_area", "user_id", "log_date");

                                    //WriteCSVRow(csTF2File, "IDN", "running_serial", "case_no", "file_name", "file_path", "File_type", "user_id", "log_date");

                                    //WriteCSVRow(csTF2Upload, "File_name","File_details" ,"running_serial", "user_id", "log_date");
                                }


                                foreach (var fileName in fileNames)
                                {
                                    var fileQAData = new wfePolicy(sqlConNameCSV, new CtrlPolicy(GetProjectKey(projectName), GetBatchKey(projectName, batchName), batchName, fileName))
                                        .GetQAUserActivity();
                                    var userID = string.IsNullOrEmpty(fileQAData.ModifiedBy) ? fileQAData.CreatedBy : fileQAData.ModifiedBy;
                                    var logDate = (string.IsNullOrEmpty(fileQAData.ModifiedBy) ? fileQAData.CreatedOn : fileQAData.ModifiedOn)?.ToString("dd/MM/yyyy HH:mm:ss");
                                    var fileID = store.GetFileFromFileName(fileName);
                                    var (_, _, fileData) = store.GetFile(fileID.Value);
                                    var ps = fileData.PsCode.EnglishText;
                                    var casetype = fileData.CaseType.EnglishText;
                                    var caseyear = fileData.CaseYear.EnglishText;
                                    var plottype = fileData.LAPlotType.EnglishText;
                                    var purpose = fileData.purpose.EnglishText;
                                    var actno  = fileData.actno.EnglishText;
                                    var requiringbody = fileData.runningbody.EnglishText;
                                    var jlno = fileData.jlno.EnglishText;
                                    //var recodid = fileData.recordid.EnglishText;
                                    var caseno = fileData.case_no.EnglishText;
                                    var index = fileName.IndexOf("_");
                                    string case_no = fileName.Substring(index + 1);
                                    var slNo = 1;
                                   // var projaCD = store.GetMasterCode(fileData.ProjaSotterType.MasterID);
                                    var nOpt = fileData.Plots.Count;
                                    //WriteCSVRow(csTF2Upload, fileName+".pdf", "File_details", recod_id, userID, logDate);
                                    //WriteCSVRow(csTF2File, IDN, recod_id, case_no, fileName, "FilePath",".pdf", userID, logDate);
                                    //WriteCSVRow(csTF1CSV, recod_id, actno, case_no, caseyear,IDN,IDN.Substring(0,2), IDN.Substring(2, 2), IDN.Substring(4), jlno, ps, purpose, requiringbody,"Office_code",  userID, logDate);

                                    var index1 = fileName.IndexOf("_");
                                    string case_no1 = fileName.Substring(index1+1);
                                    foreach (var item in fileData.Plots
                                            .Zip(fileData.BataPlots, (p, b) => new { p, b })
                                            .Zip(fileData.mesurment, (pb, m) => new { pb.p, pb.b, m }))
                                    {
                                        var pt1 = item.p.EnglishText;
                                        var pt2 = item.b.EnglishText;
                                        var MM = item.m.EnglishText;
                                        //var recodid = IDN + "001";
                                        
                                        WriteCSVRow(csTF2CSV, IDN,case_no, plottype, pt1, pt2,
                                            GeneratePlotSerial(pt1, pt2).Trim(),MM, userID, logDate);
                                    }

                                    if (token.IsCancellationRequested)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public override void CancelTasks()
        {
            foreach (var taskController in taskControllers)
            {
                taskController.Value.Token.WaitHandle.WaitOne(1000);
                taskController.Value.Cancel();
            }
        }

        private static string GenerateKhatianSerial(string kt1, string kt2)
        {
            return $"{kt1,5}{(kt2.Trim() == MetadataStore.INVALID_PLOT_BATA_KHATIAN ? "  " : kt2.PadLeft(2, '0'))}";
        }

        private static string GeneratePlotSerial(string pt1, string pt2)
        {
            return $"{(pt2.Trim() == MetadataStore.INVALID_PLOT_BATA_KHATIAN ? pt1 : pt2),5}";
        }

        private static string GenerateFileName(MetadataStore store, Guid districtID, Guid blockID, Guid mouzaID)
        {
            var districtCode = store.GetMasterCode(districtID);
            var blockCode = store.GetMasterCode(blockID);
            var mouzaCode = store.GetMasterCode(mouzaID);
            return $"{districtCode.PadLeft(2, '0')}{blockCode.PadLeft(2, '0')}{mouzaCode.PadLeft(3, '0')}";
        }
    }
    public enum ExportState
    {
        Bundle,
        Mouza
    }
}
