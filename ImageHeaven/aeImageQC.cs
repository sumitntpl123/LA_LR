using bzer_data_entry;
using DockSample;
using ImageHeaven.Logic;
using Inlite.ClearImageNet;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;



namespace ImageHeaven
{
    public partial class aeImageQC : Form
    {
        public delegate void NextClickedHandler(object sender, EventArgs e);
        public delegate void PreviousClickedHandler(object sender, EventArgs e);
        public delegate void PolicyChangeHandler(object sender, EventArgs e);
        public delegate void ImageChangeHandler(object sender, EventArgs e);
        public delegate void BoxDetailsLoaded(object sender, EventArgs e);
        public delegate void LstDelImageKeyPress(object sender, KeyEventArgs e);
        public delegate void LstImageIndexKeyPress(object sender, KeyPressEventArgs e);
        public delegate void BoxDetailsMouseClick(object sender, MouseEventArgs e);
        public delegate void LstImageClick(object sender, EventArgs e);
        public delegate void LstDelImageClick(object sender, EventArgs e);
        public delegate void LstNextKeyPress(object sender, KeyEventArgs e);
        public delegate void ComboValueChanged(object sender, EventArgs e);


        System.Windows.Forms.Button prmButtonSkewLeft = new Button();

        System.Windows.Forms.Button prmButtonRescan = new Button();

        System.Windows.Forms.Button prmNext = null;
        System.Windows.Forms.Button prmPrevious = null;

        private OdbcConnection sqlCon = null;
        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private FloatToolbox m_toolbox = new FloatToolbox();
        private string imgFileName = null;
        NovaNet.Utils.ImageManupulation delImage;
        private wfeBox wBox = null;
        private CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        private CtrlBox pBox = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        private bool photoCropOperation = false;
        private bool getPhotoOperation = false;
        private string error = null;
        private Credentials crd = new Credentials();
        MemoryStream stateLog;
        byte[] tmpWrite;
        ///For key config
        string cropKey;
        string zoomInKey;
        string zoomOutKey;
        string autoCropKey;
        string rotateRKey;
        string rotateLKey;
        string skewRKey;
        string skewLKey;
        string noiseRemovalLKey;
        string cleanKey;
        string deleteKey;

        /// <summary>
        /// For drawing rectangle
        /// </summary>
        private int cropX;
        private int cropY;
        private int cropWidth;
        private int cropHeight;
        private double constRotateAngle;
        private int OperationInProgress;
        private bool hasPhotoBol;
        private bool delinsrtBol = false;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private Pen cropPen;
        private int cropPenSize = 1;
        private System.Drawing.Color cropPenColor = System.Drawing.Color.Blue;

        public static string projKey = null;
        public static string bundleKey = null;
        public static string boxNumber = null;
        OdbcDataAdapter sqlAdap;
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed = 1;
        private ImageConfig config = null;

        private ListBox policyLst = null;
        private ListBox delImgList = null;
        private Label lblImageSize = null;
        private Label lblinfo = null;
        private ListBox imageLst = null;
        private ListBox imageDelLst = null;
        private CtrlPolicy ctrlPolicy = null;
        private wfePolicy policy = null;
        private udtPolicy policyData = null;
        private FileorFolder fileMove = null;
        private string sourceFilePath = null;
        private string qcFolderName = null;
        private ToolTip tp = new ToolTip();
        private Imagery img;
        private string policy_Path = string.Empty;
        private int policyLen = 0;


        wfeBatch pBatch;
        wfeProject pProject;

        private int indexCount = 0;
        private eSTATES[] currState;
        private eSTATES[] imageCurrState;
        private int pLeft = 0;
        private int pTop = 0;

        string iniPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + "IhConfiguration.ini";
        INIFile ini = new INIFile();

        private aeImageQCLogic aeImgQCLogic;
        private UpdateStatusLogic updateStatusLogic;
        GetItemsLogic getItemsLogic = null;


        public aeImageQC(OdbcConnection prmCon, Credentials prmCrd)
        {
            sqlCon = prmCon;
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            crd = prmCrd;
            InitializeComponent();
            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            currState = new eSTATES[1];

            currState[0] = eSTATES.POLICY_SCANNED;
            Find_AutoQC_or_Not();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            this.Text = "Image Quality Control" + "Connection: " + sqlCon.ConnectionString.ToString();
            exMailLog.SetNextLogger(exTxtLog);

            aeImgQCLogic = new aeImageQCLogic(prmCon, prmCrd);
            updateStatusLogic = new UpdateStatusLogic(prmCon);
            getItemsLogic = new GetItemsLogic(prmCon);
        }

        private void Find_AutoQC_or_Not()
        {
            if (File.Exists(iniPath) == true)
            {
                string aqc = ini.ReadINI("AUTOQC", "AUTOQC", string.Empty, iniPath);
                if (aqc.ToString().Trim().ToLower().Contains("true"))
                {
                    deButton1.Enabled = true;
                }
                else
                {
                    deButton1.Enabled = false;
                }
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            return m_toolbox;
        }

        void ReadConfigKey()
        {
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            cropKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.CROP_KEY).Remove(1, 1).Trim();
            zoomInKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ZOOM_IN_KEY).Remove(1, 1).Trim();
            zoomOutKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ZOOM_OUT_KEY).Remove(1, 1).Trim();
            autoCropKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.AUTO_CROP_KEY).Remove(1, 1).Trim();
            rotateRKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ROTATE_RIGHT_KEY).Remove(1, 1).Trim();
            rotateLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ROTATE_LEFT_KEY).Remove(1, 1).Trim();
            skewRKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.SKEW_RIGHT_KEY).Remove(1, 1).Trim();
            skewLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.SKEW_LEFT_KEY).Remove(1, 1).Trim();
            noiseRemovalLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.NOISE_REMOVE_KEY).Remove(1, 1).Trim();
            cleanKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.CLEAN_KEY).Remove(1, 1).Trim();
            deleteKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim();
        }
        public string GetProjectName(int prmProjectKey)
        {
            string projName = null;
            DataSet projDs = new DataSet();

            projDs = aeImgQCLogic.GetProjectName(prmProjectKey);
            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["proj_code"].ToString();
            }
            else
                projName = string.Empty;
            return projName;
        }

        void DisplayValues()
        {
            projKey = frmMain.projKey;
            bundleKey = frmMain.bundleKey;
            boxNumber = "1";
            lblProject.Text = "Project: " + GetProjectName(Convert.ToInt32(projKey));
            lblBatch.Text = "Bundle: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            lblProject.ForeColor = Color.RoyalBlue;
            lblBatch.ForeColor = Color.RoyalBlue;
            lblCount.ForeColor = Color.RoyalBlue;

        }
        public string GetBundleName(int prmProjectKey, int prmBundleKey)
        {
            string projName = null;

            DataSet projDs = new DataSet();

            projDs = aeImgQCLogic.GetBundleName(prmProjectKey, prmBundleKey);
            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["bundle_code"].ToString();
            }
            else
                projName = string.Empty;
            return projName;
        }

        public DataTable GetFileDetails(string projKey, string bundleKey, string file_no)
        {
            DataTable dt = new DataTable();
            dt = aeImgQCLogic.GetFileDetails(projKey, bundleKey, file_no);
            return dt;
        }

        private void GetFileDetails(string file_no)
        {

        }

        bool UpdateState(eSTATES prmPageSate, string prmPageName)
        {
            double fileSize;
            bool delBol = false;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();

            var projectID = Convert.ToInt32(frmMain.projKey);
            var batchID = Convert.ToInt32(frmMain.bundleKey);
            var fileName = policyLst.SelectedItem.ToString();
            var wBatch = new wfeBatch(sqlCon);
            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, fileName, prmPageName, string.Empty);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            if (wImage.UpdateStatus(prmPageSate, crd) == true)
            {
                delBol = true;
            }
            if (imgFileName == null)
            {
                imgFileName = Path.Combine(wBatch.GetPath(projectID, batchID), fileName, "Scan", pImage.ImageName);
            }
            if (delinsrtBol == false)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);

                fileSize = info.Length;
                fileSize = fileSize / 1024;

                wImage.UpdateImageSize(crd, eSTATES.PAGE_QC, fileSize);
            }
            return delBol;
        }

        int CropRegister()
        {
            OperationInProgress = ihConstants._CROP;
            delinsrtBol = false;
            return 0;
        }

        int CleanImageRegister()
        {
            OperationInProgress = ihConstants._CLEAN;
            delinsrtBol = false;
            return 0;
        }
        int PhotoCropRegister()
        {
            delinsrtBol = false;
            imageLst = lstImage;
            if ((hasPhotoBol == true) && ((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION))
            {
                if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()) == false)
                {
                    File.Copy(sourceFilePath + "\\" + imageLst.SelectedItem.ToString(), qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                }
                imgFileName = qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString();

                img.LoadBitmapFromFile(imgFileName);

                ChangeSize();
                OperationInProgress = ihConstants._PHOTO_CROP;
                prmButtonCrop.Enabled = false;
                prmButtonAutoCrp.Enabled = false;
                prmButtonRotateRight.Enabled = false;
                prmButtonRotateLeft.Enabled = false;
                prmButtonZoomIn.Enabled = false;
                prmButtonZoomOut.Enabled = false;
                prmButtonSkewRight.Enabled = false;
                prmButtonSkewLeft.Enabled = false;
                prmButtonNoiseRemove.Enabled = false;
                prmButtonCleanImg.Enabled = false;
                prmButtonCopyImage.Enabled = false;
                prmButtonDelImage.Enabled = false;
                prmButtonRescan.Enabled = false;
                prmGetPhoto.Enabled = false;
            }
            return 0;
        }
        int EndPhotoCrop()
        {
            string photoName;
            wfeImage wImg;
            long fileSize;

            try
            {
                System.IO.FileInfo info = null;
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";
                if (File.Exists(imgFileName))
                {
                    if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
                    {
                        File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                    }
                    OperationInProgress = ihConstants._NO_OPERATION;
                    if (photoCropOperation == true)
                    {
                        info = new System.IO.FileInfo(imgFileName);
                        fileSize = info.Length;
                        fileSize = fileSize / 1024;

                        policyLst = lstPolicy;
                        imageLst = lstImage;
                        photoName = imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";

                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoName, string.Empty);

                        wImg = new wfeImage(sqlCon, pImage);
                        if (wImg.DeletePage() == true)
                        {
                            wImg.Save(crd, eSTATES.PAGE_QC, fileSize, ihConstants._PHOTO_PAGE, 0, photoName);
                        }
                        //Save the colour photograph into 000
                        //img.SaveAsTiff(imgFileName,IGRComressionTIFF.JPEG);
                    }
                    //Now load the black and white image	
                }
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString();

                ChangeSize(imgFileName);
                if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                }
                if (File.Exists(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString());
                }
                prmButtonCrop.Enabled = true;
                prmButtonAutoCrp.Enabled = true;
                prmButtonRotateRight.Enabled = true;
                prmButtonRotateLeft.Enabled = true;
                prmButtonZoomIn.Enabled = true;
                prmButtonZoomOut.Enabled = true;
                prmButtonSkewRight.Enabled = true;
                prmButtonSkewLeft.Enabled = true;
                prmButtonNoiseRemove.Enabled = true;
                prmButtonCleanImg.Enabled = true;
                prmButtonCopyImage.Enabled = true;
                prmButtonDelImage.Enabled = true;
                prmButtonRescan.Enabled = true;
                photoCropOperation = false;
                getPhotoOperation = false;
                prmPhotoCrop.Enabled = true;
                prmGetPhoto.Enabled = true;
                prmButtonSkewRight.Enabled = true;
                prmNext = cmdNext;
                prmPrevious = cmdPrevious;
                prmNext.Enabled = true;
                prmPrevious.Enabled = true;
                delinsrtBol = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }
        int GetPhoto()
        {

            try
            {
                prmButtonRescan.Enabled = false;
                prmButtonCopyImage.Enabled = false;
                prmPhotoCrop.Enabled = false;
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";
                if (File.Exists(imgFileName))
                {
                    ChangeSize(imgFileName);
                    getPhotoOperation = true;
                    prmNext = cmdNext;
                    prmPrevious = cmdPrevious;
                    prmNext.Enabled = false;
                    prmPrevious.Enabled = false;
                    prmButtonDelImage.Enabled = false;
                    delinsrtBol = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return 0;
        }
        private void AeImageQC_Load(object sender, EventArgs e)
        {
            TimeWatcher tw = new TimeWatcher();
            tw.start();
            ReadConfigKey();
            cmbDesValue.SelectedIndex = 0;
            DisplayValues();

            if (lblImageSize != null)
                lblImageSize.ForeColor = Color.Black;
            System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            ArrayList arrPolicy = new ArrayList();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(aeImageQC));


            delImage = new NovaNet.Utils.ImageManupulation(CropRegister);
            tp.SetToolTip(prmButtonCrop, "Crop");
            prmButtonCrop.Text = "Crop";
            bttnToolTip.SetToolTip(prmButtonCrop, "Shortcut Key-" + cropKey);

            prmButtonCrop.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(AutoCrop);
            bttnToolTip.SetToolTip(prmButtonAutoCrp, "Shortcut Key-" + autoCropKey);
            prmButtonAutoCrp.Text = "Auto-Crop";
            prmButtonAutoCrp.ForeColor = Color.Black;

            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            delImage = new NovaNet.Utils.ImageManupulation(RotateRight);
            bttnToolTip.SetToolTip(prmButtonRotateRight, "Shortcut Key-" + rotateRKey);
            prmButtonRotateRight.Text = "Rotate Right";
            prmButtonRotateRight.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(RotateLeft);
            bttnToolTip.SetToolTip(prmButtonRotateLeft, "Shortcut Key-" + rotateLKey);
            prmButtonRotateLeft.Text = "Rotate Left";
            prmButtonRotateLeft.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(ZoomIn);
            bttnToolTip.SetToolTip(prmButtonZoomIn, "Shortcut Key-" + zoomInKey);
            prmButtonZoomIn.Text = "Zoom In";
            prmButtonZoomIn.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(ZoomOut);
            bttnToolTip.SetToolTip(prmButtonZoomOut, "Shortcut Key-" + zoomOutKey);
            prmButtonZoomOut.Text = "Zoom Out";
            prmButtonZoomOut.ForeColor = Color.Black;

            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            delImage = new NovaNet.Utils.ImageManupulation(SkewRight);
            bttnToolTip.SetToolTip(prmButtonSkewRight, "Shortcut Key-" + skewRKey);
            prmButtonSkewRight.Text = "Deskew";
            prmButtonSkewRight.ForeColor = Color.Black;


            delImage = new NovaNet.Utils.ImageManupulation(NoiseRemove);
            bttnToolTip.SetToolTip(prmButtonNoiseRemove, "Shortcut Key-" + noiseRemovalLKey);
            prmButtonNoiseRemove.Text = "Despacle";
            prmButtonNoiseRemove.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(CleanImageRegister);
            bttnToolTip.SetToolTip(prmButtonCleanImg, "Shortcut Key-" + cleanKey);
            prmButtonCleanImg.Text = "Clean";
            prmButtonCleanImg.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(ImageCopy);
            bttnToolTip.SetToolTip(prmButtonCopyImage, "Shortcut Key-(Control+z)");
            prmButtonCopyImage.Text = "Copy Original";
            prmButtonCopyImage.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(ImageDelete);
            bttnToolTip.SetToolTip(prmButtonDelImage, "Shortcut Key-" + deleteKey);
            prmButtonDelImage.Text = "Delete";
            prmButtonDelImage.ForeColor = Color.Black;
            delImage = new NovaNet.Utils.ImageManupulation(PhotoCropRegister);
            prmPhotoCrop.Text = "Photo Crop";
            prmPhotoCrop.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(EndPhotoCrop);
            prmEndPhotoCrop.Text = "End Edit(Photo)";
            prmEndPhotoCrop.ForeColor = Color.Black;

            delImage = new NovaNet.Utils.ImageManupulation(GetPhoto);
            prmGetPhoto.Text = "Get Photo";
            prmGetPhoto.ForeColor = Color.Black;

            bttnToolTip.SetToolTip(deButton1, "Shortcut Key-" + "F4");
            deButton1.Text = "Auto-QC";
            deButton1.ForeColor = Color.Black;

            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
            toolTip.SetToolTip(lstImageDel, "Press (Insert) key to insert this deleted image");

            PopulatePolicyList();


            if (lstPolicy.Items.Count > 0)
            {
                this.lstPolicy.SelectedIndex = 0;
                policyLst = lstPolicy;
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                policy_Path = GetPolicyPath();

                if (lstImage.Items.Count == 0)
                {
                    MessageBox.Show(this, "No image found for this file", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pictureControl.Image = null;
                }
                policyLen = policyLst.SelectedItem.ToString().Length;
            }
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            lstImage.Focus();
            lstImage.Select();
            tw.stop("Loading took");
        }
        void ShowImage(bool prmOverWrite)
        {
            TimeWatcher tw = new TimeWatcher();
            tw.start();
            string policyPath;
            string policyName;
            string changedImageName = string.Empty;
            DataSet ds = new DataSet();
            int photoImageName;
            try
            {
                policyLst = lstPolicy;
                imageLst = lstImage;
                if (policyLst.SelectedItem != null)
                {
                    //Check for policy has photo or not
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString());
                    policy = new wfePolicy(sqlCon, ctrlPolicy);


                    policyPath = policy_Path;
                    if (GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                    {
                        if ((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION)
                        {
                            prmPhotoCrop.Enabled = true;
                            prmEndPhotoCrop.Enabled = true;
                            hasPhotoBol = true;
                            if (File.Exists(policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == false)
                            {
                                lblNote.Text = "Warning: This policy contains photo, crop it with photo crop button.";
                                lblNote.Visible = true;
                                lblNote.ForeColor = Color.Red;
                            }
                            else
                            {
                                lblNote.Visible = true;
                                lblNote.Text = "Photo crop has been done, select GetPhoto to check it";
                                lblNote.ForeColor = Color.Green;
                            }
                        }
                        else
                        {
                            prmPhotoCrop.Enabled = false;
                            prmEndPhotoCrop.Enabled = false;
                            hasPhotoBol = false;
                            lblNote.Visible = false;
                        }
                    }
                    else
                    {
                        prmPhotoCrop.Enabled = false;
                        prmEndPhotoCrop.Enabled = false;
                        hasPhotoBol = false;
                        lblNote.Visible = false;
                    }
                    policyName = policyLst.SelectedItem.ToString();
                    changedImageName = imageLst.SelectedItem.ToString();


                    fileMove = new FileorFolder();
                    string sourcePath = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                    string destPath = policyPath + "\\" + ihConstants._QC_FOLDER;
                    sourceFilePath = sourcePath;
                    qcFolderName = destPath;
                    if (policyPath == null || policyPath == "")
                    {

                    }
                    else
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
                            photoImageName = 100; //Convert.ToInt32(changedImageName.Substring(10, 3));
                            if (((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION) && (hasPhotoBol == true) && ihConstants._PHOTO_PAGE_POSITION == photoImageName)
                            {
                                if ((File.Exists(destPath + "\\" + "1_" + changedImageName) == false) && (File.Exists(destPath + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == false))
                                {
                                    File.Copy(imgFileName, destPath + "\\" + "0_" + changedImageName);
                                    img.ConvertTo1Bpp(destPath + "\\" + "0_" + changedImageName, destPath + "\\" + "1_" + changedImageName);
                                    File.Move(imgFileName, destPath + "\\" + "2_" + changedImageName);
                                    File.Move(destPath + "\\" + "1_" + changedImageName, imgFileName);
                                    File.Move(destPath + "\\" + "2_" + changedImageName, destPath + "\\" + "1_" + changedImageName);
                                    prmGetPhoto.Enabled = true;
                                    prmPhotoCrop.Enabled = true;
                                    prmEndPhotoCrop.Enabled = true;
                                }
                                if (File.Exists(destPath + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == true)
                                {
                                    prmGetPhoto.Enabled = true;
                                }
                            }
                            else
                            {
                                prmGetPhoto.Enabled = false;
                                prmPhotoCrop.Enabled = false;
                                prmEndPhotoCrop.Enabled = false;
                            }
                            img.LoadBitmapFromFile(imgFileName);
                            ChangeSize();
                            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                            long fileSize = info.Length;
                            fileSize = fileSize / 1024;
                            lblImgSize.Text = fileSize.ToString() + " KB";
                            lblinformation.Text = "Press F5 to move to the next File";

                        }
                        else
                            MessageBox.Show("Error while creaing QC folder");
                    }
                }
            }
            catch (Exception ex)
            {

                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + Convert.ToInt32(projKey) + " ,Batch-" + Convert.ToInt32(bundleKey) + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            tw.stop("Image loading took: ");
        }
        public int GetPolicyPhotoStatus()
        {
            DataSet policyDs = new DataSet();
            int projKey = ctrlPolicy.ProjectKey;
            int batchKey = ctrlPolicy.BatchKey;
            string policyNumber = ctrlPolicy.PolicyNumber;

            policyDs = aeImgQCLogic.GetPolicyPhotoStatus(projKey, batchKey, policyNumber);
            return Convert.ToInt32(policyDs.Tables[0].Rows[0]["photo"].ToString());
        }
        private void ChangeSize()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    if (pictureControl.Dock != DockStyle.Fill)
                    {
                        //pictureControl.Dock = DockStyle.Fill;
                    }
                    if (!System.IO.File.Exists(imgFileName)) return;
                    Image newImage = img.GetBitmap();
                    if (newImage.PixelFormat == PixelFormat.Format1bppIndexed)
                    {
                        pictureControl.Image = null;
                        pictureControl.Width = 559;
                        pictureControl.Height = 680;
                        if (!System.IO.File.Exists(imgFileName)) return;
                        double scaleX = (double)pictureControl.Width / (double)newImage.Width;
                        double scaleY = (double)pictureControl.Height / (double)newImage.Height;
                        double Scale = Math.Min(scaleX, scaleY);
                        int w = (int)(newImage.Width * Scale);
                        int h = (int)(newImage.Height * Scale);
                        pictureControl.Width = 559;
                        pictureControl.Height = 680;
                        pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                        newImage.Dispose();
                    }
                    else
                    {
                        img.LoadBitmapFromFile(imgFileName);
                        pictureControl.Image = img.GetBitmap();
                        pictureControl.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error while" + ex.Message, "Error");
            }
        }

        public Image CreateThumbnail(Image pImage, int lnWidth, int lnHeight)
        {

            Bitmap bmp = new Bitmap(lnWidth, lnHeight);
            try
            {

                DateTime stdt = DateTime.Now;

                //create a new Bitmap the size of the new image

                //create a new graphic from the Bitmap
                Graphics graphic = Graphics.FromImage((Image)bmp);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //draw the newly resized image
                graphic.DrawImage(pImage, 0, 0, lnWidth, lnHeight);
                //dispose and free up the resources
                graphic.Dispose();
                DateTime dt = DateTime.Now;
                TimeSpan tp = dt - stdt;
                //MessageBox.Show(tp.Milliseconds.ToString());
                //return the image

            }
            catch
            {
                return null;
            }
            return (Image)bmp;
        }

        private string GetPolicyPath()
        {
            policyLst = lstPolicy;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = GetPath(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            return batchPath + "\\" + policyLst.SelectedItem.ToString();
        }
        public string GetPath(int prmProjKey, int prmBatchKey)
        {
            DataSet projDs = new DataSet();
            string Path;

            projDs = aeImgQCLogic.GetPath(prmProjKey, prmBatchKey);
            if (projDs.Tables[0].Rows.Count > 0)
            {
                Path = projDs.Tables[0].Rows[0]["bundle_path"].ToString();
            }
            else
                Path = string.Empty;

            return Path;
        }

        public ArrayList GetItems(eITEMS item, eSTATES[] state)
        {
            return getItemsLogic.GetItems(item, projKey, bundleKey, new eSTATES[] { eSTATES.BATCH_CREATED });
        }
        private void PopulatePolicyList()
        {
            this.lstPolicy.SelectedIndexChanged -= this.LstPolicy_SelectedIndexChanged;
            lstPolicy.Items.Clear();
            ArrayList arrPolicy = new ArrayList();
            CtrlPolicy ctrPol;

            arrPolicy = GetItems(eITEMS.POLICY, currState);
            for (int i = 0; i < arrPolicy.Count; i++)
            {
                ctrPol = (CtrlPolicy)arrPolicy[i];
                lstPolicy.Items.Add(ctrPol.PolicyNumber);
                if (lstPolicy.SelectedIndex == 0)
                {
                    //lstPolicy.Text = ctrPol.PolicyNumber;
                }
            }
            this.lstPolicy.SelectedIndexChanged += this.LstPolicy_SelectedIndexChanged;
            if (arrPolicy.Count > 0)
            {
                lblCount.Text = "Pending: " + GetItems(eITEMS.POLICY, currState).Count;
            }
            else
            {
                lblCount.Text = "Pending : 0";
                lstImageDel.Items.Clear();
            }
        }

        private void PopulateImageList(string prmPolicyNo)
        {
            lstImage.Items.Clear();
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", prmPolicyNo);
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            ArrayList arrImage = new ArrayList();
            CtrlImage ctrlImage;
            arrImage = GetImagesItems(eITEMS.PAGE, currState, prmPolicyNo);
            for (int i = 0; i < arrImage.Count; i++)
            {
                ctrlImage = (CtrlImage)arrImage[i];

                if (ctrlImage.DocType != string.Empty)
                {

                    lstImage.Items.Add(ctrlImage.ImageName.ToString().Replace(".TIF", "") + "-" + ctrlImage.DocType + ".TIF");
                }
                else
                    lstImage.Items.Add(ctrlImage.ImageName);
            }
        }

        public ArrayList GetImagesItems(eITEMS item, eSTATES[] state, string fileName)
        {
            wItemControl wic = null;
            string strQr = string.Empty;
            ArrayList arrItem = new ArrayList();

            if (item == eITEMS.PAGE)
            {
                var ds = aeImgQCLogic.GetImagesItems(item, state, projKey, bundleKey, fileName);

                if (ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {

                        wic = new CtrlImage(Convert.ToInt32(ds.Rows[i]["proj_key"].ToString()), Convert.ToInt32(ds.Rows[i]["batch_key"].ToString()), "1", ds.Rows[i]["policy_number"].ToString(), ds.Rows[i]["page_name"].ToString(), ds.Rows[i]["doc_type"].ToString());
                        arrItem.Add(wic);
                    }
                }
            }

            return arrItem;
        }


        void EnableDisbleControls(bool prmControl)
        {
            prmButtonCrop.Enabled = prmControl;
            prmButtonAutoCrp.Enabled = prmControl;
            prmButtonRotateRight.Enabled = prmControl;
            prmButtonRotateLeft.Enabled = prmControl;
            prmButtonZoomIn.Enabled = prmControl;
            prmButtonZoomOut.Enabled = prmControl;
            prmButtonSkewRight.Enabled = prmControl;
            prmButtonSkewLeft.Enabled = prmControl;
            prmButtonNoiseRemove.Enabled = prmControl;
            prmButtonCleanImg.Enabled = prmControl;
            prmButtonCopyImage.Enabled = prmControl;
            prmButtonDelImage.Enabled = prmControl;
            prmButtonRescan.Enabled = prmControl;
        }



        [Category("Action")]
        [Description("Fires when the Policy is changed.")]
        public event PolicyChangeHandler PolicyChanged;
        private void LstPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageList(lstPolicy.SelectedItem.ToString());
            DataSet dsVol = new DataSet();
            CtrlPolicy ctrPol;
            policyLst = lstPolicy;
            imageLst = lstImage;
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString());
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (lstImage.Items.Count > 0)
            {
                lstImage.SelectedIndex = 0;
            }

            if (PolicyChanged != null)
            {
                PolicyChanged(this, e);
                OdbcTransaction sqlTrans = null;
                GetFileDetails(lstPolicy.SelectedItem.ToString());
                LockPolicy(crd, sqlTrans);
            }
            else
            {
                GetFileDetails(lstPolicy.SelectedItem.ToString());
                OdbcTransaction sqlTrans = null;
                GetFileDetails(lstPolicy.SelectedItem.ToString());
                LockPolicy(crd, sqlTrans);
            }


            string policyPath;

            if (policyLst.SelectedItem != null)
            {
                EnableDisbleControls(true);
                delImgList = lstImageDel;
                if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                {
                    delImgList.SetSelected(delImgList.SelectedIndex, false);
                }
                policyLst = lstPolicy;
                imageLst = lstImage;
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                policy_Path = GetPolicyPath();
                PopulateDelList(policyLst.SelectedItem.ToString());
                if (lstImage.Items.Count == 0)
                {
                    MessageBox.Show(this, "No image found for this file", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pictureControl.Image = null;
                }
                else { ShowImage(false); }

                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                policyLen = policyLst.SelectedItem.ToString().Length;
            }




        }

        public void PopulateDelList(string prmPolicyNo)
        {
            lstImageDel.Items.Clear();
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", prmPolicyNo);
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            ArrayList arrImage = new ArrayList();
            wfeImage wImage = new wfeImage(sqlCon);
            eSTATES[] imageState = new eSTATES[1];
            imageState[0] = eSTATES.PAGE_DELETED;
            CtrlImage ctrlImage;
            arrImage = wImage.GetDeletedPageList1(currState, imageState, policy);
            for (int i = 0; i < arrImage.Count; i++)
            {
                ctrlImage = (CtrlImage)arrImage[i];
                lstImageDel.Items.Add(ctrlImage.ImageName);
            }
        }

        [Category("Action")]
        [Description("Fires when the Image is changed.")]
        public event ImageChangeHandler ImageChanged;
        private void LstImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            imageLst = imageLst = lstImage;
            DataSet dsVol = new DataSet();
            DateTime st = DateTime.Now;
            indexCount = lstImage.SelectedIndex;
            ShowImage(false);
            if ((ImageChanged != null) && (indexCount >= 0))
            {
                ImageChanged(this, e);
            }
            DateTime end = DateTime.Now;
            TimeSpan duration = end - st;
            if ((imageLst.SelectedIndex + 1) == imageLst.Items.Count)
            {
                if (imageLst.Items.Count > 0)
                {
                    UpdateState(eSTATES.PAGE_QC, imageLst.SelectedItem.ToString());
                }
            }


        }


        public event LstImageClick LstImgClick;
        private void LstImage_Click(object sender, EventArgs e)
        {
            ShowImage(false);
            if (LstImgClick != null)
            {
                LstImgClick(this, e);
            }
        }

        [Category("Action")]
        [Description("Fires when the key Pressed for indexing")]
        public event LstImageIndexKeyPress LstImageIndex;
        private void LstImageKeyPress(object sender, KeyPressEventArgs e)
        {
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            if (LstImageIndex != null)
            {
                LstImageIndex(this, e);
            }
            char DELETE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DELETE)
            {
                ImageDelete();
            }
            if ((policyLst.SelectedIndex + 1) != (policyLst.Items.Count))
            {
                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
            }

        }

        int ZoomIn()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    pictureControl.Dock = DockStyle.None;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height * (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width * (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) * 1.2);
                    pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) * 1.2);
                    pictureControl.Refresh();
                    ChangeZoomSize();
                    delinsrtBol = false;
                    cmdNext.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
                exMailLog.Log(ex);
            }
            return 0;
        }

        private void ChangeZoomSize()
        {
            if (!System.IO.File.Exists(imgFileName)) return;
            Image newImage = Image.FromFile(imgFileName);
            double scaleX = (double)pictureControl.Width / (double)newImage.Width;
            double scaleY = (double)pictureControl.Height / (double)newImage.Height;
            double Scale = Math.Min(scaleX, scaleY);
            int w = (int)(newImage.Width * Scale);
            int h = (int)(newImage.Height * Scale);
            pictureControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
            newImage.Dispose();
        }
        private bool GetThumbnailImageAbort()
        {
            return false;
        }
        int ZoomOut()
        {
            try
            {
                if (keyPressed > 0)
                {
                    pictureControl.Dock = DockStyle.None;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height / (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width / (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) / 1.2);
                    pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) / 1.2);
                    pictureControl.Refresh();
                    ChangeZoomSize();
                    delinsrtBol = false;
                    cmdNext.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
                error = ex.Message;
            }
            return 0;
        }
        int RotateRight()
        {
            long fileSize;


            try
            {
                //Rotate right +90
                img.RotateRight();
                //Call the save routine
                img.SaveFile(imgFileName);
                img.LoadBitmapFromFile(imgFileName);
                //Show the image back in picture box
                //pictureControl.Image = img.GetBitmap();
                ChangeSize();
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImgSize.Text = fileSize.ToString() + " KB";
                delinsrtBol = false;
                UpdateImageSize(fileSize);
                cmdNext.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }

            return 0;
        }

        int RotateLeft()
        {
            long fileSize;


            try
            {
                if (img.IsValid() == true)
                {
                    //Rotate right -90
                    img.RotateLeft();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
                    //Show the image back in picture box
                    //pictureControl.Image = img.GetBitmap();
                    ChangeSize();
                    delinsrtBol = false;
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    lblImgSize.Text = fileSize.ToString() + " KB";
                    UpdateImageSize(fileSize);
                    cmdNext.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }

            return 0;
        }
        int AutoCrop()
        {
            long fileSize;
            try
            {
                //Auto Crop
                img.AutoCrop();
                //Call the save routine
                img.SaveFile(imgFileName);
                //Show the image back in picture box
                img.LoadBitmapFromFile(imgFileName);

                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                // lblImageSize = lblImageSize;
                lblImgSize.Text = fileSize.ToString() + " KB";
                delinsrtBol = false;

                UpdateImageSize(fileSize);
                cmdNext.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping " + ex.Message, "Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }

        int SkewRight()
        {
            long fileSize;
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Deskew
                    img.AutoDeSkew(true);
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
                    //Show the image back in picture box
                    //pictureControl.Image = img.GetBitmap();
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImgSize.Text = fileSize.ToString() + " KB";
                    UpdateImageSize(fileSize);
                    cmdNext.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }

        void UpdateImageSize(long prmSize)
        {
            wfeImage img;
            policyLst = lstPolicy;
            imageLst = lstImage;

            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString(), imageLst.SelectedItem.ToString(), string.Empty);
            img = new wfeImage(sqlCon, pImage);
            img.UpdateImageSize(crd, eSTATES.PAGE_QC, prmSize);
        }
        int SkewLeft()
        {
            //            OperationInProgress = ihConstants._OTHER_OPERATION;
            //
            //            //rotateAngle = rotateAngle + constRotateAngle;
            //            imgQc = objQc.Skew(imgQc,(-skewXAngle),(-skewYAngle));
            //            pictureControl.Image = MagickNet.Image.ToBitmap(imgQc);
            //            pictureControl.Refresh();
            //            //imgQc.Write(imgFileName);

            return 0;
        }
        int NoiseRemove()
        {
            long fileSize;
            ComboBox noiseVal = cmbDesValue;
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Deskew
                    //img.Despeckle();
                    img.Despeckle(Convert.ToInt32(Convert.ToInt32(noiseVal.Text)));
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
                    //Show the image back in picture box
                    //pictureControl.Image = img.GetBitmap();
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    lblImgSize.Text = fileSize.ToString() + " KB";
                    UpdateImageSize(fileSize);
                    cmdNext.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }
        private void aeImageQC_KeyUp(object sender, KeyEventArgs e)
        {
            int cropKeyVal = GetKeyVal(cropKey.ToUpper());
            int zoomInKeyVal = GetKeyVal(zoomInKey.ToUpper());
            int zoomOutKeyVal = GetKeyVal(zoomOutKey.ToUpper());
            int autoCropKeyVal = GetKeyVal(autoCropKey.ToUpper());
            int rotateRKeyVal = GetKeyVal(rotateRKey.ToUpper());
            int rotateLKeyVal = GetKeyVal(rotateLKey.ToUpper());
            int skewRKeyVal = GetKeyVal(skewRKey.ToUpper());
            int skewLKeyVal = GetKeyVal(skewLKey.ToUpper());
            int noiseRemovalLKeyVal = GetKeyVal(noiseRemovalLKey.ToUpper());
            int cleanKeyVal = GetKeyVal(cleanKey.ToUpper());

            if ((int)e.KeyData == cropKeyVal)
            {
                OperationInProgress = ihConstants._CROP;
            }
            if ((int)e.KeyData == (zoomInKeyVal + 64))
            {
                ZoomIn();
            }
            if ((int)e.KeyData == (zoomOutKeyVal))
            {
                ZoomOut();
            }

            if ((int)e.KeyData == (autoCropKeyVal))
            {
                AutoCrop();
            }

            if ((int)e.KeyData == (rotateRKeyVal))
            {
                RotateRight();
            }
            if ((int)e.KeyData == (rotateLKeyVal))
            {
                RotateLeft();
            }

            if ((int)e.KeyData == (skewRKeyVal))
            {
                SkewRight();
            }
            if ((int)e.KeyData == (skewLKeyVal))
            {
                SkewLeft();
            }

            if ((int)e.KeyData == (noiseRemovalLKeyVal))
            {
                NoiseRemove();
            }
            if ((int)e.KeyData == (cleanKeyVal))
            {
                OperationInProgress = ihConstants._CLEAN;
            }


            if (e.KeyCode == Keys.Escape)
            {
                OperationInProgress = ihConstants._NO_OPERATION;
                Cursor = Cursors.No;
                Cursor = Cursors.Default;
                pictureControl.Cursor = Cursors.Default;
            }
            if (e.KeyCode == Keys.F11)
            {
                MoveUp();
            }
            if (e.KeyCode == Keys.F12)
            {
                MoveDown();
            }
            if (e.KeyCode == Keys.Right)
            {
                imageLst = imageLst = lstImage;
                policyLst = lstPolicy;
                if (imageLst.Items.Count > 0)
                {
                    UpdateState(eSTATES.PAGE_QC, imageLst.SelectedItem.ToString());
                }
                MoveNext();
                ShowImage(false);
            }
            if (e.KeyCode == Keys.Left)
            {
                imageLst = imageLst = lstImage;
                policyLst = lstPolicy;
                if (imageLst.Items.Count > 0)
                {
                    UpdateState(eSTATES.PAGE_QC, imageLst.SelectedItem.ToString());
                }
                MovePrevious();
                ShowImage(false);
            }
            if ((e.KeyCode == Keys.Z) && (e.Control == true))
            {
                ImageCopy();
            }
            if (e.KeyCode == Keys.Subtract)
            {
                ZoomOut();
            }
        }

        public int GetKeyVal(string key)
        {
            int cropKeyVal = 0;

            foreach (char c in key)
            {
                cropKeyVal = (int)c;
            }
            return cropKeyVal;
        }


        public bool MovePrevious()
        {
            if ((indexCount > 0) && (lstImage.Items.Count > 0))
            {
                indexCount = indexCount - 1;
                lstImage.SelectedIndex = indexCount;
            }
            if ((indexCount == 0) && (lstImage.Items.Count > 0) && (lstPolicy.SelectedIndex != 0))
            {
                indexCount = 0;
                lstPolicy.SelectedIndex = lstPolicy.SelectedIndex - 1;
            }
            return true;
        }
        public bool MoveUp()
        {
            ArrayList arr = new ArrayList();
            CtrlPolicy ctPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lstPolicy.SelectedItem.ToString());
            wfePolicy wp = new wfePolicy(sqlCon, ctPolicy);
            //Can only move up when there are records in the list box of images
            //and the currently selected element is greater than 0
            if (lstImage.Items.Count > 0 && lstImage.SelectedIndex > 0)
            {
                indexCount = indexCount - 1;
                object swp = lstImage.Items[lstImage.SelectedIndex];
                lstImage.Items[lstImage.SelectedIndex] = lstImage.Items[lstImage.SelectedIndex - 1]; ;
                lstImage.Items[lstImage.SelectedIndex - 1] = swp;
                lstImage.SelectedIndex = lstImage.SelectedIndex - 1;
                for (int i = 0; i < lstImage.Items.Count; i++)
                {
                    arr.Add(lstImage.Items[i]);
                }
                wp.UpdateSrl(arr);
            }

            return true;
        }
        public bool MoveDown()
        {
            ArrayList arr = new ArrayList();
            CtrlPolicy ctPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", lstPolicy.SelectedItem.ToString());
            wfePolicy wp = new wfePolicy(sqlCon, ctPolicy);
            //Can only move up when there are records in the list box of images
            //and the currently selected element is less than count of items-1
            if (lstImage.Items.Count > 0 && lstImage.SelectedIndex < (lstImage.Items.Count - 1))
            {
                string swp = lstImage.SelectedItem.ToString();
                lstImage.Items[lstImage.SelectedIndex] = lstImage.Items[lstImage.SelectedIndex + 1];
                lstImage.Items[lstImage.SelectedIndex + 1] = swp;
                lstImage.SelectedIndex = lstImage.SelectedIndex + 1;
                indexCount = indexCount + 1;
                for (int i = 0; i < lstImage.Items.Count; i++)
                {
                    arr.Add(lstImage.Items[i]);
                }
                wp.UpdateSrl(arr);

            }
            return true;
        }
        public bool MoveNext()
        {
            if (lstImage.Items.Count > 0)
            {
                indexCount = indexCount + 1;
                if ((lstImage.Items.Count - 1) < indexCount)
                {
                    indexCount = 0;
                    if ((lstPolicy.Items.Count - 1) > (lstPolicy.SelectedIndex))
                    {
                        lstPolicy.SelectedIndex = lstPolicy.SelectedIndex;
                    }
                    if (lstImage.Items.Count > 0)
                    {
                        lstImage.SelectedIndex = indexCount;
                    }
                }
                else
                {
                    lstImage.SelectedIndex = indexCount;
                }
            }
            else
            {
                if ((lstPolicy.Items.Count - 1) > (lstPolicy.SelectedIndex))
                {
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex;
                }
            }
            return true;
        }

        [Category("Action")]
        [Description("Fires when the Next button is clicked.")]
        public event NextClickedHandler NextClicked;
        private void CmdNext_Click(object sender, EventArgs e)
        {

            if (NextClicked != null)
            {
                NextClicked(this, e);
            }
            imageLst = imageLst = lstImage;
            policyLst = lstPolicy;


            if (imageLst.Items.Count > 0)
            {
                EnableDisbleControls(true);
            }
            delImgList = lstImageDel;
            if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
            {
                delImgList.SetSelected(delImgList.SelectedIndex, false);
            }

            if (imageLst.Items.Count > 0)
            {
                UpdateState(eSTATES.PAGE_QC, imageLst.SelectedItem.ToString());
            }
            MoveNext();
            ShowImage(false);//added in version 1.0.0.1
            //ChangeSize();//added in version 1.0.0.1
        }

        [Category("Action")]
        [Description("Fires when the Previous button is clicked.")]
        public event PreviousClickedHandler PreviousClicked;
        private void CmdPrevious_Click(object sender, EventArgs e)
        {
            MovePrevious();

            if (PreviousClicked != null)
            {
                PreviousClicked(this, e);
            }

            imageLst = imageLst = lstImage;
            policyLst = lstPolicy;


            if (imageLst.Items.Count > 0)
            {
                EnableDisbleControls(true);
                ShowImage(false);

                delImgList = lstImageDel;
                if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                {
                    delImgList.SetSelected(delImgList.SelectedIndex, false);
                }

                if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                }
                if (File.Exists(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString());
                }
            }
        }
        private void ChangeSize(string fName)
        {
            Image imgTot = null;
            try
            {
                if (img.IsValid() == true)
                {
                    if (pictureControl.Dock != DockStyle.Fill)
                    {
                        //pictureControl.Dock = DockStyle.Fill;
                    }
                    pictureControl.Width = 559;
                    pictureControl.Height = 680;
                    if (!System.IO.File.Exists(fName)) return;
                    Image newImage;
                    img.LoadBitmapFromFile(fName);
                    if (img.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        img.GetLZW("tmp.TIF");
                        imgTot = Image.FromFile("tmp.TIF");
                        newImage = imgTot;
                    }
                    else
                    {
                        newImage = System.Drawing.Image.FromFile(fName);
                    }
                    double scaleX = (double)pictureControl.Width / (double)newImage.Width;
                    double scaleY = (double)pictureControl.Height / (double)newImage.Height;
                    double Scale = Math.Min(scaleX, scaleY);
                    int w = (int)(newImage.Width * Scale);
                    int h = (int)(newImage.Height * Scale);
                    pictureControl.Width = 559;
                    pictureControl.Height = 680;
                    pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    if (imgTot != null)
                    {
                        imgTot.Dispose();
                        imgTot = null;
                        if (File.Exists("tmp.tif"))
                            File.Delete("tmp.TIF");
                    }
                    if (newImage != null)
                    {
                        newImage.Dispose();
                        newImage = null;
                    }
                }
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
            }
        }

        public event LstDelImageClick LstDelImgClick;
        private void lstImageDel_Click(object sender, EventArgs e)
        {
            string delFileName;

            delImgList = lstImageDel;
            if (lstImageDel.Items.Count > 0)
            {
                toolStripStatusLabel2.Text = "";
                toolStripStatusLabel2.Text = "Press 'Insert' to remove image from deleted list";
            }
            else
            {
                statusStrip1.Visible = false;
                toolStripStatusLabel2.Text = "";
            }
            if (delImgList.Items.Count > 0)
            {
                if (delImgList.SelectedIndex >= 0)
                {
                    EnableDisbleControls(false);
                    delFileName = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                    string[] searchFileName = Directory.GetFiles(delFileName, delImgList.SelectedItem.ToString());
                    //For searching deleted file in deleted folder.
                    if (searchFileName.Length >= 0)
                    {
                        delFileName = searchFileName[0];

                        img.LoadBitmapFromFile(delFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                        ChangeSize(delFileName);
                    }
                }
            }
            if (LstDelImgClick != null)
            {
                LstDelImgClick(this, e);
            }
        }

        public bool InsertNotify(int currIndex)
        {
            PopulateImageList(lstPolicy.SelectedItem.ToString());
            if (lstImage.Items.Count > 0)
            {
                if (currIndex != lstImage.Items.Count)
                {
                    lstImage.SelectedIndex = currIndex;
                    indexCount = lstImage.SelectedIndex;
                }
                else
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
            }
            else
            {
                if ((lstPolicy.SelectedIndex) != (lstPolicy.Items.Count - 1))
                {
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                }
            }

            PopulateDelList(lstPolicy.SelectedItem.ToString());
            return true;
        }

        [Category("Action")]
        [Description("Fires when the image from deleted list inserted.")]
        public event LstDelImageKeyPress LstDelIamgeInsert;
        private void lstImageDel_KeyDown(object sender, KeyEventArgs e)
        {
            string delPath = null;
            string sourceFileName = null;
            string delFileName = null;
            string qcFilePath = null;
            int photoPageCount;

            if (e.KeyCode == Keys.Insert)
            {
                imageDelLst = lstImageDel;
                policyLst = lstPolicy;
                imageLst = lstImage;
                delinsrtBol = true;
                if (imageDelLst.Items.Count > 0)
                {
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString(), imageDelLst.SelectedItem.ToString(), string.Empty);
                    if (UpdateState(eSTATES.PAGE_QC, imageDelLst.SelectedItem.ToString()) == true)
                    {
                        delPath = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                        imageDelLst = lstImageDel;
                        sourceFileName = sourceFilePath + "\\" + imageDelLst.SelectedItem.ToString();
                        delFileName = delPath + "\\" + imageDelLst.SelectedItem.ToString();
                        qcFilePath = qcFolderName + "\\" + imageDelLst.SelectedItem.ToString();
                        photoPageCount = 100; //Convert.ToInt32(imageDelLst.SelectedItem.ToString().Substring(10, 3));
                        if (File.Exists(delFileName) == true)
                        {
                            File.Move(delFileName, sourceFileName);
                            if (ihConstants._PHOTO_PAGE_POSITION == photoPageCount)
                            {
                                img.ConvertTo1Bpp(sourceFileName, qcFilePath);
                            }
                            else
                            {
                                File.Copy(sourceFileName, qcFilePath);
                            }
                        }

                        InsertNotify(imageLst.SelectedIndex);
                        EnableDisbleControls(true);

                        if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                        {
                            delImgList.SetSelected(delImgList.SelectedIndex, false);
                        }
                    }
                }
                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }

            if (LstDelIamgeInsert != null)
            {
                LstDelIamgeInsert(this, e);
            }

        }
        public bool CheckQCStatus()
        {
            wfeImage wImage;
            bool flag = false;
            DataSet ds = new DataSet();
            System.IO.FileInfo info;
            wfeBox box;
            policyLst = lstPolicy;
            if (policyLst.Items.Count > 0)
            {
                string sql = "select * from image_master where proj_key = '" + Convert.ToInt32(projKey) + "' and batch_key = '" + Convert.ToInt32(bundleKey) + "' and box_number ='" + boxNumber + "' and policy_number = '" + policyLst.SelectedItem.ToString() + "' and status = '24'";
                OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
                odap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    flag = true;
                }


            }
            return flag;
        }
        public bool UpdateStatus(eSTATES state, Credentials prmCrd, bool pLock)
        {
            eSTATES newState = eSTATES.BATCH_SCANNED;
            return updateStatusLogic.UpdateStatus(ctrlPolicy.ProjectKey.ToString(), ctrlPolicy.BatchKey.ToString(), ctrlPolicy.PolicyNumber, prmCrd, new eSTATES[] { }, new eSTATES[] { eSTATES.POLICY_EXPORTED }, newState);
        }
        void UpdateAllPolicyStatus()
        {
            wfeImage wImage;
            System.IO.FileInfo info;
            wfePolicy wPolicy;
            wfeBox box;

            policyLst = lstPolicy;


            if (policyLst.Items.Count > 0)
            {
                for (int i = 0; i < policyLst.Items.Count; i++)
                {
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.Items[i].ToString(), string.Empty, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.Items[i].ToString());
                    wPolicy = new wfePolicy(sqlCon, pPolicy);
                    if (wImage.GetImageCount(eSTATES.PAGE_SCANNED) == false)
                    {
                        crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                        //policy update status
                        // case_file_master status update
                        UpdateStatus(eSTATES.POLICY_QC, crd, true);
                        // wPolicy.UnLockPolicy();
                        ///update into transaction log
                        wPolicy.UpdateTransactionLog(eSTATES.POLICY_QC, crd);
                    }

                    pBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber);
                    box = new wfeBox(sqlCon, pBox);
                    NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
                    state[0] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

                    if (wPolicy.GetPolicyCount(state) == 0)
                    {
                        //  box.UpdateStatus(eSTATES.BOX_QC);
                    }
                    if (GetFileCount(projKey, bundleKey) == 0)
                    {
                        ///Update the batch status
                        //wBatch.UpdateStatus(eSTATES.BATCH_SCANNED, wBox.ctrlBox.BatchKey);
                        UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                    }
                }

            }
        }

        public bool UpdateBundleStatus(int prmProjKey, int prmBatchKey)
        {
            eSTATES state = eSTATES.BATCH_SCANNED;
            return updateStatusLogic.UpdateBundleStatus(prmProjKey.ToString(), prmBatchKey.ToString(), new eSTATES[] { eSTATES.BATCH_CREATED }, new eSTATES[] { eSTATES.BATCH_SCANNED }, state);
        }

        public int GetFileCount(string projkey, string bundleKey)
        {
            var dsImage = aeImgQCLogic.GetFileCount(projkey, bundleKey);
            return dsImage;
        }
        public void RefreshNotify()
        {
            PopulatePolicyList();
            if (lstPolicy.Items.Count > 0)
            {
                lstPolicy.SelectedIndex = 0;
            }
            //PopulateImageList((int)lstPolicy.SelectedItem);
        }
        int ImageDelete()
        {
            string qcDelPath = null;
            string sourceFileName = null;
            string destFileName = null;
            string qcFilePath = null;

            delinsrtBol = true;
            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageLst.SelectedItem.ToString(), string.Empty);
            if (UpdateState(eSTATES.PAGE_DELETED, imageLst.SelectedItem.ToString()) == true)
            {
                qcDelPath = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                imageLst = lstImage;
                sourceFileName = sourceFilePath + "\\" + imageLst.SelectedItem.ToString();
                destFileName = qcDelPath + "\\" + imageLst.SelectedItem.ToString();
                qcFilePath = qcFolderName + "\\" + imageLst.SelectedItem.ToString();
                if (FileorFolder.CreateFolder(qcDelPath) == true)
                {
                    if (File.Exists(destFileName) == false)
                    {
                        File.Move(sourceFileName, destFileName);
                        File.Delete(qcFilePath);
                    }
                }
                DeleteNotify(imageLst.SelectedIndex);
                ShowImage(true);
                cmdNext.Focus();
                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }
            return 0;
        }
        public bool DeleteNotify(int currIndex)
        {
            lstImage.SelectedIndex = Math.Max(currIndex - 1, 0);
            lstImage.Items.RemoveAt(currIndex);
            if (lstImage.Items.Count > 0)
            {
                if (currIndex != lstImage.Items.Count)
                {
                    lstImage.SelectedIndex = currIndex;
                    indexCount = lstImage.SelectedIndex;
                }
                else
                {
                    if ((lstPolicy.SelectedIndex) != (lstPolicy.Items.Count - 1))
                    {
                        lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                    }
                    else
                    {
                        lstImage.SelectedIndex = currIndex - 1;
                    }
                }
            }
            else
            {
                if ((lstPolicy.SelectedIndex) != (lstPolicy.Items.Count - 1))
                {
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                }
            }
            PopulateDelList(lstPolicy.SelectedItem.ToString());

            return true;
        }

        int ImageCopy()
        {
            string policyPath;
            string policyName;
            string changedImageName = string.Empty;
            DataSet ds = new DataSet();

            try
            {
                delinsrtBol = false;
                policyLst = lstPolicy;
                imageLst = lstImage;

                //Check for policy has photo or not
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                if (GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                {
                    if ((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION)
                    {
                        prmPhotoCrop.Enabled = true;
                        prmEndPhotoCrop.Enabled = true;
                        hasPhotoBol = true;
                    }
                    else
                    {
                        prmPhotoCrop.Enabled = false;
                        prmEndPhotoCrop.Enabled = false;
                        hasPhotoBol = false;
                    }
                }
                else
                {
                    prmPhotoCrop.Enabled = false;
                    prmEndPhotoCrop.Enabled = false;
                    hasPhotoBol = false;
                }

                policyName = policyLst.SelectedItem.ToString();
                changedImageName = imageLst.SelectedItem.ToString();
                //policyData=(udtPolicy)policy.LoadValuesFromDB();
                policyPath = GetPolicyPath(); //policyData.policy_path;
                fileMove = new FileorFolder();
                string sourcePath = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                string destPath = policyPath + "\\" + ihConstants._QC_FOLDER;
                sourceFilePath = sourcePath;
                qcFolderName = destPath;


                if (((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION) && (hasPhotoBol == true))
                {
                    img.LoadBitmapFromFile(sourcePath + "\\" + changedImageName);

                    if (img.GetBitmap().PixelFormat != PixelFormat.Format1bppIndexed)
                    {
                        img.ToBitonal();
                    }
                    pictureControl.Image = img.GetBitmap();
                    img.SaveFile(destPath + "\\" + changedImageName);
                }
                else
                {
                    img.LoadBitmapFromFile(sourcePath + "\\" + changedImageName);
                    img.SaveFile(destPath + "\\" + changedImageName);
                    img.LoadBitmapFromFile(imgFileName);
                }
                ChangeSize();
                ds.Dispose();
                pictureControl.Refresh();
                cmdNext.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while copying the original image", "Copy Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }



        public bool LockPolicy(Credentials pCrd, OdbcTransaction pTrns)
        {
            return aeImgQCLogic.LockPolicy(pCrd, pTrns, ctrlPolicy);
        }

        private void aeImageQC_KeyDown(object sender, KeyEventArgs e)
        {
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            int cropKeyVal = GetKeyVal(cropKey.ToUpper());
            int zoomInKeyVal = GetKeyVal(zoomInKey.ToUpper());
            int zoomOutKeyVal = GetKeyVal(zoomOutKey.ToUpper());
            int autoCropKeyVal = GetKeyVal(autoCropKey.ToUpper());
            int rotateRKeyVal = GetKeyVal(rotateRKey.ToUpper());
            int rotateLKeyVal = GetKeyVal(rotateLKey.ToUpper());
            int skewRKeyVal = GetKeyVal(skewRKey.ToUpper());
            int skewLKeyVal = GetKeyVal(skewLKey.ToUpper());
            int noiseRemovalLKeyVal = GetKeyVal(noiseRemovalLKey.ToUpper());
            int cleanKeyVal = GetKeyVal(cleanKey.ToUpper());
            if (e.KeyCode == Keys.Escape)
            {
                //dg.Close();
            }
            if (e.KeyCode == Keys.F4)
            {
                if (deButton1.Enabled == true)
                {
                    DeButton1_Click(sender, e);
                }

            }
            if (e.KeyCode == Keys.F1)
            {
                frmHelp frm = new frmHelp("QC");
                frm.ShowDialog(this);
            }
            if (e.KeyCode == Keys.F5)
            {
                TimeWatcher tw = new TimeWatcher();
                tw.start();

                if (CheckQCStatus() == true)
                {
                    tw.current("Before UpdateAllPolicyStatus");
                    UpdateAllPolicyStatus();
                    tw.current("After UpdateAllPolicyStatus");

                    RefreshNotify();

                    if (policyLst.Items.Count <= 0)
                    {
                        imageLst.Items.Clear();
                        if (policyLst.Items.Count == 0)
                        {
                            EnableDisbleControls(false);
                            prmEndPhotoCrop.Enabled = false;
                            prmGetPhoto.Enabled = false;
                            prmPhotoCrop.Enabled = false;
                            pictureControl.Image = null;
                            deLabel2.Visible = false;
                            MessageBox.Show(this, "QC Completed for this Bundle ....", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    tw.stop();
                }
                else
                {
                    MessageBox.Show(this, "Please visit all images", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }
            ///For checking todays production count
            if ((e.KeyCode == Keys.F9))
            {
                wfePolicy wPolicy = new wfePolicy(sqlCon);
                int count = wPolicy.GetTransactionLogCount(bundleKey, dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_QC);
                frmProductionCount frmProd = new frmProductionCount(count);
                frmProd.ShowDialog(this);
            }
            if (e.KeyCode == Keys.Space)
            {
                if ((this.ActiveControl == null) || (this.ActiveControl.Name != "cmdNext" && this.ActiveControl.Name != "cmdPrevious"))
                {
                    object o = new object();
                    EventArgs a = new KeyEventArgs(Keys.None);
                    CmdNext_Click(o, a);
                }
            }

            char DELETE = Convert.ToChar(config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
            char rslt;
            if (char.TryParse(e.KeyCode.ToString(), out rslt))
            {
                if (Convert.ToChar(e.KeyCode.ToString().ToUpper()) == DELETE)
                {
                    ImageDelete();
                    return;
                }
            }
        }

        private void PictureControl_MouseDown(object sender, MouseEventArgs e)
        {
            if ((OperationInProgress == ihConstants._CROP) || (OperationInProgress == ihConstants._PHOTO_CROP))
            {
                if (e.Button == MouseButtons.Left)
                {

                    cropX = e.X;
                    cropY = e.Y;

                    cropPen = new Pen(cropPenColor, cropPenSize);
                    cropPen.DashStyle = DashStyle.Solid;
                    Cursor = Cursors.Cross;
                    pictureControl.Refresh();
                }
            }
            else
            {
                Cursor = Cursors.Default;
            }
            if (OperationInProgress == ihConstants._CLEAN)
            {
                if (e.Button == MouseButtons.Left)
                {
                    cropX = e.X;
                    cropY = e.Y;

                    cropPen = new Pen(cropPenColor, cropPenSize);
                    cropPen.DashStyle = DashStyle.Solid;
                    Cursor = Cursors.Cross;
                    pictureControl.Refresh();
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                OperationInProgress = ihConstants._NO_OPERATION;
                Cursor = Cursors.No;
                Cursor = Cursors.Default;
                pictureControl.Cursor = Cursors.Default;
            }
        }

        private void PictureControl_MouseMove(object sender, MouseEventArgs e)
        {
            if ((OperationInProgress == ihConstants._CROP) || (OperationInProgress == ihConstants._PHOTO_CROP))
            {

                if ((pictureControl.Image != null) && (cropPen != null))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        Cursor = Cursors.Cross;
                        pictureControl.Refresh();
                        cropWidth = Math.Abs(e.X - cropX);
                        cropHeight = Math.Abs(e.Y - cropY);
                        pictureControl.CreateGraphics().DrawRectangle(cropPen, ((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
                    }
                }
            }
            else
            {
                Cursor = Cursors.Default;
            }
            if (OperationInProgress == ihConstants._CLEAN)
            {
                if ((pictureControl.Image != null))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        Cursor = Cursors.Cross;
                        pictureControl.Refresh();
                        cropWidth = Math.Abs(e.X - cropX);
                        cropHeight = Math.Abs(e.Y - cropY);
                        pictureControl.CreateGraphics().DrawRectangle(cropPen, ((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
                    }
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                Cursor = Cursors.Default;
                OperationInProgress = ihConstants._NO_OPERATION;
            }
        }
        int Crop(Rectangle udtRect)
        {
            double htRatio = 0;
            double wdRatio = 0;
            long fileSize;
            Bitmap bmpImage = null;
            try
            {
                Rectangle rect = udtRect;
                if (img.IsValid() == true)
                {
                    bmpImage = img.GetBitmap();

                    htRatio = Convert.ToDouble(bmpImage.Size.Height) / Convert.ToDouble(pictureControl.Height);
                    rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
                    wdRatio = (Convert.ToDouble(bmpImage.Size.Width) / Convert.ToDouble(pictureControl.Width));
                    rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width) * wdRatio);

                    rect.X = Convert.ToInt32(rect.X * wdRatio);
                    rect.Y = Convert.ToInt32(rect.Y * htRatio);

                    img.Crop(rect);
                    img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
                }
                //Change the size of the image in relation to the canvas
                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImageSize = lblImgSize;

                lblImageSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
                cmdNext.Focus();
                delinsrtBol = false;
            }
            catch (Exception ex)
            {
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
            }
            return 0;
        }
        int Clean(Rectangle rect)
        {

            double htRatio = 0;
            double wdRatio = 0;
            long fileSize;
            try
            {
                Bitmap bmpImage = img.GetBitmap();
                htRatio = Convert.ToDouble(bmpImage.Size.Height) / Convert.ToDouble(pictureControl.Height);
                rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
                wdRatio = (Convert.ToDouble(bmpImage.Size.Width) / Convert.ToDouble(pictureControl.Width));
                rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width) * wdRatio);

                rect.X = Convert.ToInt32(rect.X * wdRatio);
                rect.Y = Convert.ToInt32(rect.Y * htRatio);

                img.Clean(rect);
                img.SaveFile(imgFileName);
                img.LoadBitmapFromFile(imgFileName);
                //pictureControl.Image = img.GetBitmap();
                //Change the size of the image in relation to the canvas
                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImageSize = lblImgSize;
                lblImageSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
                delinsrtBol = false;
                cmdNext.Focus();
            }
            catch (Exception ex)
            {
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                MessageBox.Show("Error while cleaning the image" + ex.Message, "Crop error");
                return 0;
            }
            return 1;
        }

        int CropPhoto(Rectangle udtRect)
        {
            Bitmap bmpImage;
            double htRatio = 0;
            double wdRatio = 0;
            imageLst = lstImage;
            long fileSize;

            foreach (ImageCodecInfo ice in ImageCodecInfo.GetImageEncoders())
            {
                if (ice.MimeType == "image/tiff")
                {
                    //info = ice;
                    break;
                }
            }
            try
            {
                if (OperationInProgress == ihConstants._PHOTO_CROP)
                {
                    Rectangle rect = udtRect;
                    bmpImage = img.GetBitmap();
                    //Calculate the ratio for Picturebox to actual image
                    htRatio = Convert.ToDouble(bmpImage.Size.Height) / Convert.ToDouble(pictureControl.Height);
                    rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
                    wdRatio = (Convert.ToDouble(bmpImage.Size.Width) / Convert.ToDouble(pictureControl.Width));
                    rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width) * wdRatio);
                    rect.X = Convert.ToInt32(rect.X * wdRatio);
                    rect.Y = Convert.ToInt32(rect.Y * htRatio);

                    //Crop
                    img.Crop(rect);

                    //Set the new image file name
                    imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";

                    //Call the save routine
                    img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
                    pictureControl.Image = img.GetBitmap();
                    //Show the image back in picture box
                    //					pictureControl.Width = panel1.Width - 2;
                    //	                pictureControl.Height = panel1.Height - 2;
                    //	                if (!System.IO.File.Exists(imgFileName)) return 0;
                    //	                Image newImage = Image.FromFile(imgFileName);
                    //	                
                    //	                pictureControl.Image = newImage.GetThumbnailImage(pictureControl.Width, pictureControl.Height, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    //	                newImage.Dispose();

                    //Change the size of the image in relation to the canvas
                    ChangeSize();
                    //panel1.Left = BoxDtls.Right + 4;
                    //panel1.Width = (dockPanel.Width - BoxDtls.Right) - 6;
                    //panel1.Top = panel2.Top;
                    // panel1.Height = this.ClientSize.Height - 20;
                    //Calculate and show file info
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    lblImageSize = lblImgSize;
                    lblImageSize.Text = fileSize.ToString() + " KB";
                    //Update file info in db
                    UpdateImageSize(fileSize);
                    delinsrtBol = false;
                    cmdNext.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while cropping the photo", "Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            photoCropOperation = true;
            return 0;
        }

        private void pictureControl_MouseUp(object sender, MouseEventArgs e)
        {
            cropWidth = Math.Abs(e.X - cropX);
            cropHeight = Math.Abs(e.Y - cropY);


            Cursor = Cursors.Default;

            if ((OperationInProgress == ihConstants._CROP) || (OperationInProgress == ihConstants._PHOTO_CROP) || (OperationInProgress == ihConstants._CLEAN))
            {
                //Create the rectangle on which to operate
                if ((cropWidth > 1) && (e.Button == MouseButtons.Left))
                {
                    //Works both ways
                    Rectangle rect = new Rectangle(((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
                    if (OperationInProgress == ihConstants._CROP)
                    {
                        Crop(rect);
                    }
                    if (OperationInProgress == ihConstants._PHOTO_CROP)
                    {
                        CropPhoto(rect);
                    }
                    if (OperationInProgress == ihConstants._CLEAN)
                    {
                        Clean(rect);
                    }
                }
                else
                    Cursor = Cursors.Default;
            }

            if (e.Button == MouseButtons.Right)
            {
                Cursor = Cursors.Default;
                OperationInProgress = ihConstants._NO_OPERATION;
            }
            pictureControl.Refresh();
        }

        private void PrmButtonCrop_Click(object sender, EventArgs e)
        {
            CropRegister();
        }

        private void PrmButtonAutoCrp_Click(object sender, EventArgs e)
        {
            AutoCrop();
        }

        private void PrmButtonRotateRight_Click(object sender, EventArgs e)
        {
            RotateRight();
        }

        private void PrmButtonRotateLeft_Click(object sender, EventArgs e)
        {
            RotateLeft();
        }

        private void PrmButtonZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void PrmButtonZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void PrmButtonSkewRight_Click(object sender, EventArgs e)
        {
            SkewRight();
        }

        private void PrmButtonNoiseRemove_Click(object sender, EventArgs e)
        {
            NoiseRemove();
        }

        private void PrmButtonCleanImg_Click(object sender, EventArgs e)
        {
            CleanImageRegister();
        }

        private void PrmButtonCopyImage_Click(object sender, EventArgs e)
        {
            ImageCopy();
        }

        private void PrmButtonDelImage_Click(object sender, EventArgs e)
        {
            ImageDelete();
        }

        private void PrmPhotoCrop_Click(object sender, EventArgs e)
        {
            PhotoCropRegister();
        }

        private void PrmEndPhotoCrop_Click(object sender, EventArgs e)
        {
            EndPhotoCrop();
        }

        private void PrmGetPhoto_Click(object sender, EventArgs e)
        {
            GetPhoto();
        }

        private void AeImageQC_AutoSizeChanged(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void AeImageQC_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void DeButton1_Click(object sender, EventArgs e)
        {
            if (deButton1.Enabled == true)
            {
                if (lstImage.Items.Count > 0)
                {
                    for (int i = 0; i < lstImage.Items.Count; i++)
                    {
                        string changedImageName = lstImage.SelectedItem.ToString();

                        imgFileName = qcFolderName + "\\" + changedImageName;

                        try
                        {
                            ImageEditor editor = new ImageEditor();
                            editor.Image.Open(imgFileName, 1);
                            editor.AutoDeskew();
                            editor.ClearBackground(7.0);
                            editor.BorderExtract(BorderExtractMode.deskewCrop);
                            editor.RemovePunchHoles();
                            editor.SmoothCharacters();
                            editor.CleanNoise(7);
                            if (editor.Image.IsBitonal())
                            { editor.ReconstructLines(LineDirection.horzAndVert); }
                            editor.Image.SaveAs(imgFileName, Inlite.ClearImage.EFileFormat.ciTIFF);

                            CmdNext_Click(sender, e);
                        }
                        catch (Exception ex)
                        {
                            CmdNext_Click(sender, e);
                            continue;
                        }


                    }
                    if (CheckQCStatus() == true)
                    {
                        UpdateAllPolicyStatus();
                        RefreshNotify();

                        if (policyLst.Items.Count > 0)
                        {
                            //ShowImage(false);
                            // GetIndexDetails(policyLst.SelectedItem.ToString());
                        }
                        else
                        {
                            imageLst.Items.Clear();
                            if (policyLst.Items.Count == 0)
                            {
                                EnableDisbleControls(false);
                                prmEndPhotoCrop.Enabled = false;
                                prmGetPhoto.Enabled = false;
                                prmPhotoCrop.Enabled = false;
                                pictureControl.Image = null;
                                deLabel2.Visible = false;
                                //dg.Close();
                                MessageBox.Show(this, "QC Completed for this Bundle ....", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Please visit all images", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
                }
            }

        }

        private void buttonFileDetails_Click(object sender, EventArgs e)
        {
            if (lstPolicy.SelectedItems.Count > 0)
            {
                string filename = lstPolicy.SelectedItem.ToString();
                IEntryFormBuilder frmBuilder = new EntryFormBuilder();
                var fm = frmBuilder.CreateEntryForm(sqlCon, projKey, bundleKey, DataLayerDefs.Mode._View, filename, crd);
                fm.ShowDialog(this);
            }
        }
    }
}