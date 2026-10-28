using ImageHeaven.Logic;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Data.Odbc;
using System.IO;
using System.Windows.Forms;


namespace ImageHeaven
{
    internal class DocTypeStateChange : frmBulkStateChange
    {
        protected aeIndexingLogic aeIndexingLogic;
        public DocTypeStateChange(OdbcConnection con, string projkey, Credentials prmcrd, eSTATES tState, aeIndexingLogic obj)
            : base(con, projkey, prmcrd, tState, obj)
        {
            aeIndexingLogic = obj;
        }

        protected override void UpdateAllPolicyStatus(string bundleKey)
        {
            try
            {
                var arrPolicy = GetItems(eITEMS.POLICY, typeState, bundleKey);
                for (int i = 0; i < arrPolicy.Count; i++)
                {
                    var policyNumber = ((CtrlPolicy)arrPolicy[i]).PolicyNumber;
                    //Images status update
                    var arrImage = GetImagesItems(eITEMS.PAGE, currState, policyNumber, bundleKey);
                    if (arrImage.Count > 0)
                    {
                        UpdateImageStatus(arrImage, bundleKey, policyNumber, eSTATES.BATCH_SCANNED);
                    }
                    else
                    {
                        MessageBox.Show("Image not found.");
                    }
                    pImage = new CtrlImage(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey), boxNumber, policyNumber, string.Empty, string.Empty);
                    var wImage = new wfeImage(SqlCon, pImage);
                    var ctrlPolicy = new CtrlPolicy(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey), boxNumber, policyNumber);
                    var wPolicy = new wfePolicy(SqlCon, ctrlPolicy);
                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, SqlCon);
                    UpdateStatus(crd, bundleKey, policyNumber, eSTATES.BATCH_QC);
                    wPolicy.UpdateTransactionLog(eSTATES.POLICY_INDEXED, crd);
                }
                if (GetFileCount(ProjectKey, bundleKey) == 0 && GetFileCountQC(ProjectKey, bundleKey) == 0)
                {
                    UpdateBundleStatus(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey), typeState, eSTATES.BATCH_QC);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public int GetFileCountQC(string projkey, string bundleKey)
        {
            return aeIndexingLogic.GetFileCountQC(projkey, bundleKey);
        }
        void UpdateState(eSTATES prmPageSate, string prmPageName, string prmDocType, string prmIndexImageName, string prmPolicyNumber, string bundleKey)
        {
            pImage = new CtrlImage(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey), boxNumber, prmPolicyNumber, prmPageName, string.Empty);
            wfeImage wImage = new wfeImage(SqlCon, pImage);
            wImage.UpdateStatusAndDockType(prmPageSate, prmDocType, prmIndexImageName, crd);
        }
        protected override void UpdateImageStatus(ArrayList arrImage, string bundleKey, string fileName, eSTATES state)
        {
            for (int i = 0; i < arrImage.Count; i++)
            {
                var ctrlImage = (CtrlImage)arrImage[i];
                IndexingOperation(ctrlImage.ImageName, bundleKey, fileName);
            }
        }
        private void IndexingOperation(string imageName, string bundleKey, string policyName)
        {
            int pos = 0;
            bool bolKey = false;
            string selImageName = null;
            string docType = null;
            DateTime st = DateTime.Now;
            bool indexedBol = false;
            var docTypeOpt = "NA";
            if (docTypeOpt != null)
            {
                docType = docTypeOpt;
                indexedBol = ChangeAndMoveFile(imageName, docType, bundleKey, policyName);
                bolKey = true;
            }
            if (bolKey == true)
            {
                pos = imageName.IndexOf("-");
                if (pos <= 0)
                {
                    selImageName = imageName + "-";
                    pos = imageName.IndexOf(".TIF") + 5;
                    selImageName = selImageName.Insert(pos, docType);
                }
                if (indexedBol == true)
                {
                    var originalFileName = GetFileName(imageName, docType);
                    pos = originalFileName.ToString().IndexOf(".TIF");
                    var indexFileName = selImageName.Substring(0, pos) + "-" + docType + ".TIF";

                    UpdateState(eSTATES.PAGE_INDEXED, originalFileName, docType, indexFileName, policyName, bundleKey);
                }
            }
            DateTime end = DateTime.Now;
            TimeSpan duration = end - st;
        }
        private string GetFileName(string prmSelFileName, string prmDocType)
        {
            string modFileName = null;
            int pos;

            pos = prmSelFileName.IndexOf("-");
            if (pos <= 0)
            {
                modFileName = prmSelFileName;
            }
            else
            {
                modFileName = prmSelFileName.Substring(0, pos);
            }
            return modFileName;
        }

        private bool ChangeAndMoveFile(string prmSourceFileName, string prmDocType, string bundleKey, string policyName)
        {
            try
            {
                string indexFileName = null;
                string sourceFile = null;
                int pos;
                int tifPos;
                string origDoctype = string.Empty;
                string indexFilePath = string.Empty;
                var policyPath = GetPolicyPath(bundleKey, policyName); //policyData.policy_path;
                var fileMove = new FileorFolder();
                string sourcePath = policyPath + "\\" + ihConstants._QC_FOLDER;
                string destPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;

                var sourceFilePath = sourcePath;
                var indexFolderName = destPath;

                pos = prmSourceFileName.ToString().IndexOf("-");
                tifPos = prmSourceFileName.ToString().IndexOf("-") + 1;
                if (tifPos > 0)
                {
                    origDoctype = prmSourceFileName.Substring(tifPos);
                }
                if (pos <= 0)
                {
                    pos = prmSourceFileName.ToString().IndexOf("TIF") - 1;
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + prmDocType + ".TIF";
                        sourceFile = indexFolderName + "\\" + prmSourceFileName;
                        indexFilePath = indexFileName;
                    }
                }
                if ((File.Exists(indexFileName) == false))
                {
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        if (origDoctype != prmDocType)
                        {
                            File.Copy(sourceFile, indexFileName, true);
                            File.Delete(sourceFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while changing the index name......" + ex.Message);
            }
            return true;
        }
    }
}
