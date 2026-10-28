using ImageHeaven.Logic;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Windows.Forms;

namespace ImageHeaven
{
    public class ImageQCStateChange : frmBulkStateChange
    {
        public ImageQCStateChange(OdbcConnection con, string projkey, Credentials prmcrd, eSTATES tState, aeImageQCLogic obj)
            : base(con, projkey, prmcrd, tState, obj)
        {
        }
        protected override void UpdateAllPolicyStatus(string bundleKey)
        {
            wfePolicy wPolicy;
            var arrPolicy = GetItems(eITEMS.POLICY, typeState, bundleKey);
            if (arrPolicy.Count > 0)
            {
                for (int i = 0; i < arrPolicy.Count; i++)
                {
                    var policyNumber = ((CtrlPolicy)arrPolicy[i]).PolicyNumber;
                    var arrImage = GetImagesItems(eITEMS.PAGE, currState, policyNumber, bundleKey);
                    if (arrImage.Count > 0)
                    {
                        UpdateImageStatus(arrImage, bundleKey, policyNumber, eSTATES.PAGE_QC);
                    }
                    else
                    {
                        MessageBox.Show("Image not found.");
                    }
                    pImage = new CtrlImage(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey), boxNumber, policyNumber, string.Empty, string.Empty);
                    pPolicy = new CtrlPolicy(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey), boxNumber, policyNumber);
                    wPolicy = new wfePolicy(SqlCon, pPolicy);
                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, SqlCon);
                    UpdateStatus(crd, bundleKey, policyNumber, eSTATES.BATCH_SCANNED);
                    wPolicy.UpdateTransactionLog(eSTATES.POLICY_QC, crd);
                }
            }
            if (GetFileCount(ProjectKey, bundleKey) == 0)
            {
                UpdateBundleStatus(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey), typeState, eSTATES.BATCH_SCANNED);
            }
        }

        void CreateQcFolder(string bundleKey, string policyName, string imgName)
        {
            TimeWatcher tw = new TimeWatcher();
            tw.start();
            string policyPath;
            string changedImageName = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (policyName != null)
                {
                    policyPath = GetPolicyPath(bundleKey, policyName);
                    changedImageName = imgName;

                    FileorFolder fileMove = new FileorFolder();
                    string sourcePath = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                    string destPath = policyPath + "\\" + ihConstants._QC_FOLDER;
                    var sourceFilePath = sourcePath;
                    var qcFolderName = destPath;
                    if (policyPath != null || policyPath != "")
                    {
                        if (FileorFolder.CreateFolder(destPath) == true)
                        {
                            if (File.Exists(destPath + "\\" + changedImageName) == false)
                            {
                                File.Delete(destPath + "\\" + changedImageName);
                                fileMove.MoveFile(sourcePath, destPath, changedImageName, true);
                            }

                            imgFileName = destPath + "\\" + changedImageName;
                            if (File.Exists(destPath + "\\" + "0_" + changedImageName))
                            {
                                File.Delete(destPath + "\\" + "0_" + changedImageName);
                            }
                        }
                        else
                            MessageBox.Show("Error while creaing QC folder");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            tw.stop("Image loading took: ");
        }
        protected override void UpdateImageStatus(ArrayList arrImage, string bundleKey, string fileName, eSTATES state)
        {
            for (int i = 0; i < arrImage.Count; i++)
            {
                var ctrlImage = (CtrlImage)arrImage[i];
                UpdateState(state, ctrlImage.ImageName, bundleKey, fileName, eSTATES.PAGE_QC);
                CreateQcFolder(bundleKey, fileName, ctrlImage.ImageName);
            }
        }
        protected bool UpdateState(eSTATES prmPageSate, string prmPageName, string bundleKey, string fileName, eSTATES imageState)
        {
            bool delBol = false;
            double fileSize;

            var wBatch = new wfeBatch(SqlCon);
            pImage = new CtrlImage(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey), boxNumber, fileName, prmPageName, string.Empty);
            wfeImage wImage = new wfeImage(SqlCon, pImage);
            if (wImage.UpdateStatus(prmPageSate, crd) == true)
            {
                delBol = true;
            }
            if (imgFileName == null)
            {
                imgFileName = Path.Combine(wBatch.GetPath(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey)), fileName, "Scan", pImage.ImageName);
            }

            var info = new System.IO.FileInfo(imgFileName);
            fileSize = info.Length;
            fileSize = fileSize / 1024;

            wImage.UpdateImageSize(crd, imageState, fileSize);
            return delBol;
        }

    }
}


