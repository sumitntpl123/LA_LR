using bzer_data_entry;
using DockSample;
using ImageHeaven.Logic;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ImageHeaven
{
    public partial class aeFQC : Form
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

        private int indexCount = 0;
        private bool indexingOn = false;
        //private DummyToolbox m_toolbox = new DummyToolbox();
        private OdbcConnection sqlCon = null;
        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private FloatToolbox m_toolbox = new FloatToolbox();
        //private MagickNet.Image imgQc;
        private string imgFileName = null;
        //private ImageQC objQc=new ImageQC();
        NovaNet.Utils.ImageManupulation delImage;
        private wfeBox wBox = null;
        private CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        private static string docType;
        private CtrlBox pBox = null;
        private string indexFilePath = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        long fileSize;
        Credentials crd = new Credentials();
        int firstImage = 0;
        int lastImage = 0;
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
        private const int THUMBNAIL_DATA = 0x501B;
        /// <summary>
        /// For drawing rectangle
        /// </summary>
        private int cropX;
        private int cropY;
        private int cropWidth;
        private int cropHeight;
        private double constRotateAngle;
        private double skewXAngle;
        private double skewYAngle;
        bool hasPhotoBol;
        private int OperationInProgress;
        string[] imageName;
        //private Bitmap cropBitmap;
        delegate void ch();
        private Pen cropPen;
        private int cropPenSize = 1;
        private System.Drawing.Color cropPenColor = System.Drawing.Color.Blue;

        private Label lblImageSize = null;
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed = 1;
        private ImageConfig config = null;
        private string policyPath = string.Empty;
        private ListBox delImgList = null;
        //GD objects
        //private GdPicture.GdPictureImaging imgQc = new GdPicture.GdPictureImaging();
        //private int ino;
        MemoryStream stateLog;
        byte[] tmpWrite;
        private ListBox policyLst = null;
        private ListBox imageLst = null;
        private ListBox imageDelLst = null;
        private CtrlPolicy ctrlPolicy = null;
        private wfePolicy policy = null;
        private udtPolicy policyData = null;
        private FileorFolder fileMove = null;
        private string sourceFilePath = null;
        private string indexFolderName = null;
        private string qcFolderName = null;
        private bool getPhotoOperation = false;
        private string scanFolder = null;
        private string qcFolder = null;
        private string error;
        private bool insertFlag = false;
        private int lvwIndex;
        //Scanning
        private bool colorMode;
        private bool msgfilter;
        private int scanWhat = 0;
        private Label lblBatch;
        //private long Page2=0;

        private Imagery img;
        private Imagery imgAll;
        private Imagery imgBond;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private string selDocType = string.Empty;
        private int currntPg = 0;
        private bool firstDoc = true;
        private string prevDoc;
        private int policyLen = 0;
        System.Windows.Forms.Button prmButtonSkewLeft = new Button();
        System.Windows.Forms.Button prmButtonCopyTo = new Button();
        System.Windows.Forms.Button prmButtonMoveTo = new Button();
        System.Windows.Forms.Button prmButtonCopyImageTo = new Button();
        System.Windows.Forms.Button prmButtonCopyProposalForm = new Button();
        System.Windows.Forms.Button prmButtonCopyProposalReviewSlip = new Button();
        System.Windows.Forms.Button prmNext = null;
        System.Windows.Forms.Button prmPrevious = null;
        Nullable<int> moveTarget = 0;
        bool pageDelInsrt;
        private bool photoCropOperation = false;
        public static string projKey = null;
        public static string bundleKey = null;
        public static string boxNumber = null;
        public static string filename;

        public static DataLayerDefs.Mode _mode = DataLayerDefs.Mode._Edit;

        public static DataLayerDefs.Mode mode;

        private bool delinsrtBol = false;

        ToolTip _toolTip;
        System.EventHandler _imageChangedHandler;
        NovaNet.Utils.TimeWatcher tw;

        private int filenameSerial;

        public aeFQC(OdbcConnection prmCon, Credentials prmCrd)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            sqlCon = prmCon;
            crd = prmCrd;
            InitializeComponent();

            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            imgAll = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            imgBond = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            this.Text = "Final QC";
            exMailLog.SetNextLogger(exTxtLog);
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

            _mode = mode;
            _toolTip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 1000,
                ReshowDelay = 500,
                ShowAlways = true
            };

            _imageChangedHandler = new System.EventHandler(this.lstImage_SelectedIndexChanged);
            this.lstImage.SelectedIndexChanged += _imageChangedHandler;
            documentTypeListView.Init(sqlCon);
            LoadImgCountCondition();
            tw = new TimeWatcher();
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            return m_toolbox;
        }

        private void ChangeSize()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    if (!System.IO.File.Exists(imgFileName)) return;
                    Image newImage = img.GetBitmap();
                    if (newImage.PixelFormat == PixelFormat.Format1bppIndexed)
                    {
                        pictureControl.Image = null;
                        pictureControl.Width = 565;
                        pictureControl.Height = 662;
                        img.LoadBitmapFromFile(imgFileName);
                        pictureControl.Image = img.GetBitmap();
                        pictureControl.Image = CreateThumbnail(newImage, 565, 662); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                        newImage.Dispose();
                        if (Convert.ToDouble(fileSize) < 60)
                        {
                            lblImgSize.ForeColor = Color.Black;
                        }
                        else
                        {
                            lblImgSize.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        pictureControl.Width = 565;
                        pictureControl.Height = 662;
                        img.LoadBitmapFromFile(imgFileName);
                        pictureControl.Image = img.GetBitmap();
                        pictureControl.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureControl.Refresh();
                        if (Convert.ToDouble(fileSize) < 20)
                        {
                            lblImgSize.ForeColor = Color.Black;
                        }
                        else
                        {
                            lblImgSize.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
            }
        }
        public void GetImageNew(System.IntPtr prmHBmp)
        {
            char leftPad = Convert.ToChar("0");

            string sourcePath = null;
            string desPath = null;
            string qcPath = null;
            string scanPath = null;
            int imgCount = 0;
            int pageCount = 0;
            string flName;
            string scanDate;
            bool success = false;
            try
            {
                policyLst = lstPolicy;
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);

                imgCount = wImage.GetImageCount();
                pageCount = wImage.GetMaxPageCount();

                if (imgCount >= 0)
                {
                    imgCount = imgCount + 1;
                    pageCount = pageCount + 1;
                    flName = policyLst.SelectedItem.ToString() + "_" + pageCount.ToString().PadLeft(5, '0') + "_A.TIF";
                    desPath = indexFolderName + "\\" + flName;
                    scanPath = scanFolder + "\\" + flName;
                    pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                    scanDate = dbcon.GetCurrenctDTTM(1, sqlCon);
                    wPolicy.UpdateScanDetails(scanDate, ihConstants.SCAN_SUCCESS_FLAG);

                    img.LoadBitmapFromDIB(prmHBmp);
                    if (Directory.Exists(indexFolderName))
                    {
                        if (img.SaveFile(desPath) == IGRStatus.Success)
                        {
                            if (Directory.Exists(scanFolder))
                            {
                                if (img.SaveFile(scanPath) == IGRStatus.Success)
                                {
                                    success = true;
                                }
                            }
                        }
                    }
                    if (success == true)
                    {
                        //pictureControl.Image = img.GetBitmap();
                        ChangeSize();
                        SetListboxValue(flName, imgCount);
                        if (prmHBmp != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(prmHBmp);
                            prmHBmp = IntPtr.Zero;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error while saving....");
                        if (prmHBmp != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(prmHBmp);
                            prmHBmp = IntPtr.Zero;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error - " + ex.Message);
                //exMailLog.Log(ex);
                if (prmHBmp != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(prmHBmp);
                    prmHBmp = IntPtr.Zero;
                }
            }
        }
        void SetListboxValue(string prmIamgeName, int prmSrlNo)
        {
            CtrlImage ctrlImg;

            long fileSize;
            System.IO.FileInfo info = new System.IO.FileInfo(indexFolderName + "\\" + prmIamgeName);

            fileSize = info.Length;
            fileSize = fileSize / 1024;

            wfeImage img;
            imageLst = lstImage;
            policyLst = lstPolicy;
            imageLst.Items.Add(prmIamgeName);
            ctrlImg = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmIamgeName, string.Empty);

            img = new wfeImage(sqlCon, ctrlImg);
            img.Save(crd, eSTATES.PAGE_RESCANNED_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, prmSrlNo, prmIamgeName);

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
        }
        public void GetImage(System.IntPtr prmHBmp)
        {
            char leftPad = Convert.ToChar("0");
            string sourcePath = null;
            string desPath = null;
            int pos = 0;
            string fileName;
            try
            {
                imageLst = lstImage;
                if (imageLst.SelectedItem.ToString() != string.Empty)
                {
                    //Build string for supposed to be file in indexing folder
                    sourcePath = sourceFilePath + "\\" + imageLst.SelectedItem.ToString();
                    //Build string for file in FQC folder
                    desPath = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
                }
                //Wild card search: To save in the indexing folder
                fileName = imageLst.SelectedItem.ToString();
                pos = fileName.IndexOf("-");
                if (pos > 0)
                {
                    string originalImage = fileName.Substring(0, pos - 4) + "*" + ".TIF";
                    //For the file in FQC folder
                    string[] searchFileName = Directory.GetFiles(indexFolderName, originalImage);
                    if (searchFileName.Length >= 0)
                        desPath = searchFileName[0];
                }
                //End: Wild card search
                img.LoadBitmapFromDIB(prmHBmp);
                img.SaveFile(desPath);
                //img.SaveFile(sourcePath);
                ChangeSize(desPath);
                if (prmHBmp != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(prmHBmp);
                    prmHBmp = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error - " + ex.Message);
                exMailLog.Log(ex);
            }
        }
        private bool GetThumbnailImageAbort()
        {
            return false;
        }

        private void ChangeSize(string fName)
        {
            Image imgTot = null;
            try
            {
                if (img.IsValid() == true)
                {
                    if (!System.IO.File.Exists(fName)) return;
                    Image newImage;
                    imgAll.LoadBitmapFromFile(fName);
                    if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        imgAll.GetLZW("tmp1.TIF");
                        imgTot = Image.FromFile("tmp1.TIF");
                        newImage = imgTot;
                        //File.Delete("tmp1.TIF");
                    }
                    else
                    {
                        newImage = System.Drawing.Image.FromFile(fName);
                    }
                    pictureControl.Width = 565;
                    pictureControl.Height = 662;
                    pictureControl.Image = CreateThumbnail(newImage, 565, 662); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    newImage.Dispose();
                    pictureControl.Refresh();
                    if (imgTot != null)
                    {
                        imgTot.Dispose();
                        imgTot = null;
                        if (File.Exists("tmp1.tif"))
                            File.Delete("tmp1.TIF");
                    }
                    if (newImage != null)
                    {
                        newImage.Dispose();
                        newImage = null;
                        pictureControl.Width = 565;
                        pictureControl.Height = 662;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
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
            DisplayMissingKhatian();
        }
        public string GetProjectName(int prmProjectKey)
        {
            string projName = null;

            DataSet projDs = new DataSet();
            aeFQCLogic ob = new aeFQCLogic(sqlCon);
            projDs = ob.GetProjectName(prmProjectKey);
            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["proj_code"].ToString();
            }
            else
                projName = string.Empty;
            return projName;
        }
        public string GetBundleName(int prmProjectKey, int prmBundleKey)
        {
            string projName = null;
            DataSet projDs = new DataSet();
            aeFQCLogic ob = new aeFQCLogic(sqlCon);
            projDs = ob.GetBundleName(prmProjectKey, prmBundleKey);
            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["bundle_code"].ToString();
            }
            else
                projName = string.Empty;
            return projName;
        }

        private void aeFQC_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            //wfePolicy wPolicy = new wfePolicy(sqlCon);
            //int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
            this.Text = this.Text;// +"                                       Today you have done " + count + " ";

            System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();
            ReadConfigKey();
            DisplayValues();
            ArrayList arrPolicy = new ArrayList();


            delImage = new NovaNet.Utils.ImageManupulation(CropRegister);
            prmButtonCrop.Text = "Crop";
            bttnToolTip.SetToolTip(prmButtonCrop, "Shortcut Key-" + cropKey);
            //m_toolbox.AddButton(prmButtonCrop, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(AutoCrop);
            prmButtonAutoCrp.Text = "Auto-Crop";
            bttnToolTip.SetToolTip(prmButtonAutoCrp, "Shortcut Key-" + autoCropKey);
            //m_toolbox.AddButton(prmButtonAutoCrp, delImage);

            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            constRotateAngle = Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.ROTATE_ANGLE_KEY).Replace("\0", ""));
            delImage = new NovaNet.Utils.ImageManupulation(RotateRight);
            bttnToolTip.SetToolTip(prmButtonRotateRight, "Shortcut Key-" + rotateRKey);
            prmButtonRotateRight.Text = "Rotate Right";
            //m_toolbox.AddButton(prmButtonRotateRight, delImage);

            //delImage = ZoomOut;
            delImage = new NovaNet.Utils.ImageManupulation(RotateLeft);
            prmButtonRotateLeft.Text = "Rotate Left";
            bttnToolTip.SetToolTip(prmButtonRotateLeft, "Shortcut Key-" + rotateLKey);
            //m_toolbox.AddButton(prmButtonRotateLeft, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ZoomIn);
            prmButtonZoomIn.Text = "Zoom In";
            bttnToolTip.SetToolTip(prmButtonZoomIn, "Shortcut Key-" + zoomInKey);
            //m_toolbox.AddButton(prmButtonZoomIn, delImage);

            //delImage = ZoomOut;
            delImage = new NovaNet.Utils.ImageManupulation(ZoomOut);
            prmButtonZoomOut.Text = "Zoom Out";
            bttnToolTip.SetToolTip(prmButtonZoomOut, "Shortcut Key-" + zoomOutKey);
            //m_toolbox.AddButton(prmButtonZoomOut, delImage);

            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            //skewXAngle =Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.SKEW_X_KEY).Replace("\0", ""));
            //skewYAngle=Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.SKEW_Y_KEY).Replace("\0", ""));
            delImage = new NovaNet.Utils.ImageManupulation(SkewRight);
            prmButtonSkewRight.Text = "DeSkew";
            bttnToolTip.SetToolTip(prmButtonSkewRight, "Shortcut Key-" + skewRKey);
            //m_toolbox.AddButton(prmButtonSkewRight, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(NoiseRemove);
            prmButtonNoiseRemove.Text = "Despacle";
            bttnToolTip.SetToolTip(prmButtonNoiseRemove, "Shortcut Key-" + noiseRemovalLKey);
            prmButtonNoiseRemove.AutoSize = true;
            //m_toolbox.AddButton(prmButtonNoiseRemove, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(CleanImageRegister);
            prmButtonCleanImg.Text = "Clean";
            bttnToolTip.SetToolTip(prmButtonCleanImg, "Shortcut Key-" + cleanKey);
            prmButtonCleanImg.AutoSize = true;
            //m_toolbox.AddButton(prmButtonCleanImg, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImageCopy);
            bttnToolTip.SetToolTip(prmButtonCopyImage, "Shortcut Key-(Control+z)");
            prmButtonCopyImage.Text = "Copy Original";
            prmButtonCopyImage.AutoSize = true;
            //m_toolbox.AddButton(prmButtonCopyImage, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImageDelete);
            bttnToolTip.SetToolTip(prmButtonDelImage, "Shortcut Key-" + deleteKey);
            prmButtonDelImage.Text = "Delete";
            prmButtonDelImage.AutoSize = true;
            //m_toolbox.AddButton(prmButtonDelImage, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImportImage);
            prmButtonCopyTo.Text = "Import Bond";
            prmButtonCopyTo.AutoSize = true;
            //prmButtonCopyTo.Enabled = false;
            //m_toolbox.AddButton(prmButtonCopyTo, delImage);
            //prmButtonCopyTo.Visible = false;

            delImage = new NovaNet.Utils.ImageManupulation(ImportImageFromDir);
            prmButtonImportImage.Text = "Import Image";
            prmButtonImportImage.AutoSize = true;
            //m_toolbox.AddButton(prmButtonImportImage, delImage);
            prmButtonImportImage.Visible = true;

            delImage = new NovaNet.Utils.ImageManupulation(MoveImage);
            deButton1.Text = "Move To";
            deButton1.AutoSize = true;
            //m_toolbox.AddButton(deButton1, delImage);
            deButton1.Visible = true;

            delImage = new NovaNet.Utils.ImageManupulation(ReplaceImage);
            deReplace.Text = "Replace";
            deReplace.AutoSize = true;
            //m_toolbox.AddButton(deButton1, delImage);
            deReplace.Visible = true;
            //prmButtonMoveTo.Visible = false;
            ShowPolicyDetails();
            PopulatePolicyCombo();
            //this.WindowState = FormWindowState.Maximized;
            policyLst = lstPolicy;
            if (lstPolicy.Items.Count > 0)
            {
                lstPolicy.SelectedIndex = 0;
            }

            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
            toolTip.SetToolTip(lstImageDel, "Press (Insert) key to insert this deleted image");

            imageLst = lstImage;
            imageDelLst = lstImageDel;
            policyLst = lstPolicy;
            Label delLabel = label3;

            PopulateListView();
            imageLst.Enabled = true;
            policyLst.Enabled = true;

            ShowAllException();
            //this.WindowState = FormWindowState.Maximized;
            PopulateDelList(policyLst.SelectedItem.ToString());
            ///changed in version 1.0.0.1
            DisplayDocTypeCount();
            cmbDocType.SelectedIndex = 0;
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();

            if (crd.role == ihConstants._ADMINISTRATOR_ROLE)
            {
                cmdValidatefiles.Enabled = true;
                buttonUpdate.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                cmdValidatefiles.Enabled = false;
                buttonUpdate.Enabled = false;
                button2.Enabled = false;
            }

        }
        //private void GetNotification(string pNotification)
        //{
        //    groupBox3.Enabled = true;
        //}
        private void PopulatePolicyCombo()
        {
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[6];

            pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1");
            wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);


            state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            state[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            state[3] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[4] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
            state[5] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

            ds = wPolicy.GetPolicyListCombo(state);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cmbPolicy.DataSource = ds.Tables[0];
                cmbPolicy.DisplayMember = ds.Tables[0].Columns["filename"].ToString();
                cmbPolicy.ValueMember = ds.Tables[0].Columns["status"].ToString();
            }
        }
        void ShowAllException()
        {
            policyLst = lstPolicy;
            DataSet expDs = new DataSet();
            pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            wfePolicy policy = new wfePolicy(sqlCon, pPolicy);

            txtExceptionType.Text = string.Empty;
            expDs = policy.GetAllException();
            if (expDs.Tables[0].Rows.Count > 0)
            {

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["missing_img_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = "Missing image";
                }
                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["crop_clean_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Crop clean exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["poor_scan_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Poor quality of scan";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["wrong_indexing_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Wrong indexing";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["linked_policy_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Linked policy exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Decision misd Exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["extra_page_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Extra page exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["rearrange_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Rearrange exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["other_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Other exception";
                }

                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["move_to_respective_policy_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Move to respective file exception";
                }
                if (Convert.ToInt32(expDs.Tables[0].Rows[0]["metadata_exp"].ToString()) == 1)
                {
                    txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Metadata entry exception";
                }

                txtComments.Text = expDs.Tables[0].Rows[0]["comments"].ToString();
                //MessageBox.Show ("This policy has some exception, details are given at right side","Policy Exception",MessageBoxButtons.OK);
            }
            else
            {
                txtExceptionType.Text = string.Empty;
                txtComments.Text = string.Empty;
            }
        }
        private void PopulateListView()
        {
            DataSet ds = new DataSet();
            string imageName = string.Empty;
            policyLst = lstPolicy;
            imageLst = lstImage;

            /* Changed by Rahul: 29 May, 09
             * To avoid Null Reference in imageLst.SelectedItem during swapping items within the list
             * Occurs when user requests rearrangement of items
             * */
            //int pos = imageLst.SelectedItem.ToString().IndexOf("-");
            int pos = imageLst.Text.ToString().IndexOf("-");
            if (pos > 0)
            {
                //imageName=imageLst.SelectedItem.Substring(0,pos);
                imageName = imageLst.Text.Substring(0, pos);
            }
            else
            {
                //imageName = imageLst.SelectedItem.ToString();
                imageName = imageLst.Text.ToString();
            }
            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageName, string.Empty);
            wfeImage wImage = new wfeImage(sqlCon, pImage);

            ds = wImage.GetCustomException(ihConstants._NOT_RESOLVED);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string custExcp = string.Empty;
                listView1.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ListViewItem lvwItem = listView1.Items.Add(ds.Tables[0].Rows[i]["problem_type"].ToString());
                    //listView1.Items.Add(ds.Tables[0].Rows[i]["problem_type"].ToString());
                    lvwItem.SubItems.Add(ds.Tables[0].Rows[i]["Remarks"].ToString());
                }
                //txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
            else
            {
                listView1.Items.Clear();
                //txtRemarks.Text = string.Empty;
            }
        }
        private int MoveImage()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            string newImageName = string.Empty;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
            wfeImage wImage = null;
            int pos = 0;
            string originalFileName = string.Empty;
            System.IO.FileInfo info = null;
            string origDoctype = null;
            string toMoveOriginalFileN;
            //policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            //imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            bool imagemoved = false;
            int movedImgCount = 0;
            try
            {
                if (cmbPolicy.SelectedValue != null)
                {
                    if (policyLst.SelectedItem.ToString() != cmbPolicy.Text)
                    {
                        for (int i = 0; i < imageLst.Items.Count; i++)
                        {
                            if (imageLst.GetSelected(i))
                            {
                                movedImgCount++;
                                imageName = imageLst.Items[i].ToString();
                                pos = imageName.IndexOf("-");
                                if (pos > 0)
                                {
                                    //imageName = imageName.Substring(0);
                                    originalFileName = imageName.Substring(0, pos);
                                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString(), originalFileName, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    imageName = wImage.GetIndexedImageName();
                                    origDoctype = imageLst.Items[i].ToString().Substring(pos + 1);
                                }
                                else
                                {
                                    imageName = imageLst.Items[i].ToString();
                                    originalFileName = imageName;
                                }
                                //MessageBox.Show(cmbPolicy.Text);
                                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString());
                                wPolicy = new wfePolicy(sqlCon, pPolicy);
                                string fromPolicyPath = GetPolicyPath(policyLst.SelectedItem.ToString()); //wPolicy.GetPolicyPath();

                                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", cmbPolicy.Text, string.Empty, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                int imgCount = wImage.GetImageCount();
                                imgCount++;
                                int pageCount = wImage.GetMaxPageCount();
                                pageCount = pageCount + 1;
                                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", cmbPolicy.Text);
                                wPolicy = new wfePolicy(sqlCon, pPolicy);
                                string toPolicyPath = GetPolicyPath(cmbPolicy.Text); //wPolicy.GetPolicyPath();
                                if (pos <= 0)
                                {
                                    newImageName = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(5, '0') + "_A.TIF";
                                    toMoveOriginalFileN = newImageName;
                                }
                                else
                                {
                                    newImageName = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(5, '0') + "_A-" + origDoctype + ".TIF";
                                    toMoveOriginalFileN = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(5, '0') + "_A.TIF";
                                }


                                //if ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_FQC) || ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_CHECKED)) || ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_EXCEPTION)))
                                //{
                                if (Directory.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                {
                                    if (Directory.Exists(toPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                    {
                                        if (File.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName))
                                        {
                                            File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                            //File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                            //File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._QC_FOLDER + "\\" + toMoveOriginalFileN, true);
                                            File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                                            File.Delete(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName);
                                            info = new System.IO.FileInfo(toPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName);
                                            imagemoved = true;
                                        }
                                    }
                                    else
                                    {
                                        if (File.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName))
                                        {
                                            File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                            File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                                            info = new System.IO.FileInfo(toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName);
                                            //File.Delete(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName);
                                            imagemoved = true;
                                        }
                                    }
                                }
                                if (imagemoved == true)
                                {
                                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString(), originalFileName, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    if (wImage.DeletePage())
                                    {
                                        imagemoved = true;
                                        UpdateAllStatus();
                                    }
                                    else
                                    {
                                        imagemoved = false;
                                    }
                                }
                                fileSize = info.Length;
                                fileSize = fileSize / 1024;
                                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", cmbPolicy.Text, toMoveOriginalFileN, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                if (imagemoved == true)
                                {
                                    if (pos <= 0)
                                    {
                                        if (wImage.Save(crd, eSTATES.PAGE_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, imgCount, string.Empty))
                                        {
                                            UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                                        }
                                    }
                                    else
                                    {
                                        if (img.GetBitmap().PixelFormat != PixelFormat.Format24bppRgb)
                                        {
                                            if (wImage.Save(crd, eSTATES.PAGE_FQC, origDoctype, newImageName, fileSize, imgCount))
                                            {
                                                if (wPolicy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                                                {
                                                    UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                                                }
                                                else if ((wPolicy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                                                {
                                                    UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                                                }
                                                else
                                                {
                                                    UpdateStatus(eSTATES.POLICY_FQC, crd);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DataSet imageDs = wImage.GetIndexedImageName(origDoctype);
                                            int maxSrl = imgCount - 1;
                                            if (wImage.RearrangePoposalDoctype(imageDs, maxSrl))
                                            {
                                                if (wImage.Save(crd, eSTATES.PAGE_FQC, origDoctype, newImageName, fileSize, imgCount))
                                                {
                                                    if (wPolicy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                                                    {
                                                        UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                                                    }
                                                    else if ((wPolicy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                                                    {
                                                        UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                                                    }
                                                    else
                                                    {
                                                        UpdateStatus(eSTATES.POLICY_FQC, crd);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Image move to same file is not possible.....");

                    }
                }
                else
                {
                    MessageBox.Show("Given file is not a valid file.....");

                }
                if (imagemoved == true)
                {
                    if (isExists(projKey, bundleKey, "1", cmbPolicy.Text) == false)
                    {
                        UpdateTransactionLog(eSTATES.POLICY_SCANNED, crd, cmbPolicy.Text);
                    }
                    //policy.ctrlPolicy.PolicyNumber = cmbPolicy.Text;

                    MessageBox.Show(movedImgCount + " Images moved successfully.....");
                    //if ((policyLst.SelectedIndex) != (policyLst.Items.Count - 1))
                    //{
                    //    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                    //    policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                    //}
                    //else
                    //{
                    //    policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                    //    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                    //}
                    int selImageIndx = imageLst.SelectedIndex;
                    imageLst.Items.RemoveAt(imageLst.SelectedIndex);
                    if ((selImageIndx) != imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = selImageIndx;
                    }
                    else
                    {
                        imageLst.SelectedIndex = selImageIndx - 1;
                    }
                    ShowImage(false);
                    DisplayDocTypeCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while move the image to the destination file..... " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Bundle-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 1;
        }

        public bool isExists(string pr, string bu, string box, string pol)
        {
            bool retval = false;
            aeFQCLogic ob = new aeFQCLogic(sqlCon);

            DataTable dt = ob.IsTransactionExists(pr, bu, pol);

            if (dt.Rows.Count > 0)
            {
                retval = true;
            }
            else
            {
                retval = false;
            }
            return retval;
        }

        public bool UpdateTransactionLog(eSTATES state, Credentials prmCrd, string filename, OdbcTransaction sqlTrans = null)
        {

            aeFQCLogic ob = new aeFQCLogic(sqlCon);
            return ob.UpdateTransactionLog(state, prmCrd, filename, ctrlPolicy, sqlTrans);
        }
        private string GetPolicyPath(string policyNo)
        {
            policyLst = lstPolicy;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = GetPath(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            return batchPath + "\\" + policyNo;
        }
        public DataTable FilterFilesByImageCount(string imgCount, imgCountConStatus status)
        {
            aeFQCLogic obj = new aeFQCLogic(sqlCon);
            return obj.GetFileNamesByImageCount(projKey, bundleKey, imgCount, status);
        }
        private void ShowPolicyDetails()
        {
            lstPolicy.Items.Clear();
            ArrayList arrPolicy = new ArrayList();
            CtrlPolicy ctrPol;

            arrPolicy = GetItems(eITEMS.POLICY);
            for (int i = 0; i < arrPolicy.Count; i++)
            {
                ctrPol = (CtrlPolicy)arrPolicy[i];
                lstPolicy.Items.Add(ctrPol.PolicyNumber);
                //string[] row = { ctrPol.PolicyNumber, ctrPol.ItemNo };
                //var listItem = new ListViewItem(row);

                //lstPolicy1.Items.Add(listItem.SubItems[0].Text,listItem.SubItems[1].Text);
            }
            SortListPolicy();
            if (arrPolicy.Count > 0)
            {
                lblCount.Text = "Total File: " + arrPolicy.Count;
            }
            else
            {
                lblCount.Text = "Total File: 0";
                lstImageDel.Items.Clear();
            }
        }
        private void SortListPolicy()
        {
            try
            {
                var sortPolicyItems = lstPolicy.Items.Cast<string>().Select(item =>
                {
                    var khatianWithBata = item.Substring(item.IndexOf('_') + 1);
                    var khatianORKhatianWithBataParts = khatianWithBata.Split('_');
                    var khatian = int.Parse(khatianORKhatianWithBataParts[0]);
                    var bataKhatian = khatianORKhatianWithBataParts.Length > 1 ? int.Parse(khatianORKhatianWithBataParts[1]) : 0;
                    return new { Item = item, Khatian = khatian, BataKhatian = bataKhatian };
                })
                    .OrderBy(x => x.Khatian)
                    .ThenBy(y => y.BataKhatian)
                    .Select(i => i.Item)
                    .ToArray();
                lstPolicy.Items.Clear();
                lstPolicy.Items.AddRange(sortPolicyItems);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to sort lstPolicy items. " + ex.Message);
            }
        }
        private void ShowPolicyDetailsIncomplete()
        {
            lstPolicy.Items.Clear();
            ArrayList arrPolicy = new ArrayList();
            CtrlPolicy ctrPol;

            arrPolicy = GetItemsIncomplete(eITEMS.POLICY);
            for (int i = 0; i < arrPolicy.Count; i++)
            {
                ctrPol = (CtrlPolicy)arrPolicy[i];
                lstPolicy.Items.Add(ctrPol.PolicyNumber);
            }
            if (arrPolicy.Count > 0)
            {
                lblCount.Text = "Total File: " + arrPolicy.Count;
            }
            else
            {
                lblCount.Text = "Total File: 0";
                lstImageDel.Items.Clear();
            }
        }

        private void ShowPolicyDetailsImageIncomplete()
        {
            lstPolicy.Items.Clear();
            ArrayList arrPolicy = new ArrayList();
            CtrlPolicy ctrPol;

            arrPolicy = GetItemsImageIncomplete(eITEMS.POLICY);
            for (int i = 0; i < arrPolicy.Count; i++)
            {
                ctrPol = (CtrlPolicy)arrPolicy[i];
                lstPolicy.Items.Add(ctrPol.PolicyNumber);
            }
            if (arrPolicy.Count > 0)
            {
                lblCount.Text = "Total File: " + arrPolicy.Count;
            }
            else
            {
                lblCount.Text = "Total File: 0";
                lstImageDel.Items.Clear();
            }
        }

        public ArrayList GetItems(eITEMS item)
        {
            GetItemsLogic ob = new GetItemsLogic(sqlCon);
            return ob.GetItems(item, projKey, bundleKey, new eSTATES[] { eSTATES.BATCH_QC, eSTATES.BATCH_INDEXED, eSTATES.BATCH_FQC, eSTATES.BATCH_READY_FOR_UAT, eSTATES.BATCH_EXPORTED, eSTATES.POLICY_EXCEPTION, eSTATES.POLICY_CHECKED, eSTATES.POLICY_ON_HOLD, eSTATES.POLICY_NOT_INDEXED, eSTATES.POLICY_SUBMITTED, eSTATES.POLICY_EXPORTED, eSTATES.POLICY_FQC,eSTATES.BATCH_SCANNED,eSTATES.POLICY_SCANNED,eSTATES.BATCH_INDEXED,eSTATES.BATCH_QC });
        }
        public ArrayList GetItemsIncomplete(eITEMS item)
        {
            GetItemsLogic ob = new GetItemsLogic(sqlCon);
            return ob.GetItems(item, projKey, bundleKey, new eSTATES[] { eSTATES.INCOMPLETE });
        }

        public ArrayList GetItemsImageIncomplete(eITEMS item)
        {
            GetItemsLogic ob = new GetItemsLogic(sqlCon);
            return ob.GetItems(item, projKey, bundleKey, new eSTATES[] { eSTATES.INCOMPLETE });
        }

        private int ReplaceImage()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[4];
            wfeImage wImage = null;
            string newImageName = string.Empty;
            string origImageName = string.Empty;
            int pos;
            try
            {
                imageLst = lstImage;
                policyLst = lstPolicy;
                //imageName = imageLst.SelectedItem.ToString();
                ///get the file name to copy
                fileDg.Filter = "TIF File|*.TIF";
                fileDg.FileName = string.Empty;
                fileDg.Title = "B'Zer - TIF Files";
                fileDg.ShowDialog();
                int countfromFiledlg = fileDg.FileNames.Length;
                for (int i = 0; i < countfromFiledlg; i++)
                {
                    imageName = fileDg.FileNames[i].ToString();
                    if ((imageName != null) && ((imageName != string.Empty)))
                    {
                        //copy to
                        pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        string newPolicyPath = GetPolicyPath(policyLst.SelectedItem.ToString()); //wPolicy.GetPolicyPath();

                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);


                        newImageName = imageLst.SelectedItem.ToString();

                        pos = imageLst.SelectedItem.ToString().IndexOf("-");

                        if (pos > 0)
                        {
                            newImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), newImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            newImageName = wImage.GetIndexedImageName();
                        }

                        //newImageName = lstImage.SelectedItem.ToString();
                        //newImageName = policyLst.SelectedItem.ToString() + "_" + pageCount.ToString().PadLeft(5, '0') + "_A.TIF";
                        if ((GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED) || (GetPolicyStatus() == (int)eSTATES.POLICY_EXPORTED))
                        {
                            if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                            {
                                File.Delete(newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName);
                                File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                //File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + newImageName, true);
                            }

                        }
                        else
                        {
                            if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                            {
                                File.Delete(newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName);
                                File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                //File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + newImageName, true);
                            }

                        }
                        System.IO.FileInfo info = new System.IO.FileInfo(fileDg.FileName.ToString());
                        fileSize = info.Length;
                        fileSize = fileSize / 1024;
                        ShowImage(false);
                    }
                }
                MessageBox.Show("Image has been replaced successfully.....");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while replacing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 1;
        }

        private int ImportImageFromDir()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[4];
            wfeImage wImage = null;
            string newImageName = string.Empty;
            string origImageName = string.Empty;
            try
            {
                imageLst = lstImage;
                policyLst = lstPolicy;
                //imageName = imageLst.SelectedItem.ToString();
                ///get the file name to copy
                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "B'Zer - TIF Files";
                fileDlg.ShowDialog();
                int countfromFiledlg = fileDlg.FileNames.Length;
                //string xxxxxx= fileDlg.FileNames[0].ToString();
                for (int i = 0; i < countfromFiledlg; i++)
                {
                    imageName = fileDlg.FileNames[i].ToString();
                    //status = Convert.ToInt32(cmbPolicy.SelectedValue.ToString());
                    if ((imageName != null) && ((imageName != string.Empty)))
                    {

                        //copy to
                        pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        string newPolicyPath = GetPolicyPath(policyLst.SelectedItem.ToString()); //wPolicy.GetPolicyPath();

                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        int imgCount = wImage.GetImageCount();
                        imgCount++;
                        int pageCount = wImage.GetMaxPageCount();
                        pageCount++;
                        newImageName = policyLst.SelectedItem.ToString() + "_" + pageCount.ToString().PadLeft(5, '0') + "_A.TIF";
                        if ((GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED) || (GetPolicyStatus() == (int)eSTATES.POLICY_EXPORTED))
                        {
                            if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                            {
                                File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + newImageName, true);
                            }

                        }
                        else
                        {
                            if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                            {
                                File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + newImageName, true);
                            }

                        }
                        //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());
                        System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                        fileSize = info.Length;
                        fileSize = fileSize / 1024;
                        //crd.created_by = "ADMIN";
                        crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), newImageName, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        //wImage.Save(crd, eSTATES.PAGE_FQC, fileSize, ihConstants._NORMAL_PAGE, imgCount,string.Empty);
                        if (GetPolicyStatus() == 6)
                        {
                            wImage.Save(crd, eSTATES.PAGE_EXPORTED, fileSize, ihConstants._NORMAL_PAGE, imgCount, newImageName);
                        }
                        else
                        {
                            if (wImage.Save(crd, eSTATES.PAGE_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, imgCount, newImageName) == true)
                            {
                                UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                            }
                        }

                        lstImage.Items.Add(newImageName);

                    }
                }
                if (isExists(projKey, bundleKey, "1", lstPolicy.SelectedItem.ToString()) == false)
                {
                    policy.UpdateTransactionLog(eSTATES.POLICY_SCANNED, crd);
                }
                MessageBox.Show("Image has been imported successfully.....");

                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while importing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 1;
        }

        private int ImportImage()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[5];
            wfeImage wImage = null;
            string newImageName = string.Empty;
            string origImageName = string.Empty;
            string policyNo = string.Empty;
            string batchName = string.Empty;
            string boxNumber = string.Empty;
            string tmpBondPath = Path.GetDirectoryName(Application.ExecutablePath);
            try
            {
                ihwQuery wQ = new ihwQuery(sqlCon);

                wfeBatch pBatch = new wfeBatch(sqlCon);
                policyLst = lstPolicy;
                imageLst = lstImage;

                state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                state[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                state[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                state[3] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
                state[4] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;

                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "B'Zer - TIF Files";
                fileDlg.ShowDialog();
                imageName = fileDlg.FileName.ToString();

                if ((imageName != null) && ((imageName != string.Empty)))
                {
                    int j = 0;
                    for (int i = 0; i < policyLst.Items.Count; i++)
                    {
                        //MessageBox.Show(cmbPolicy.DisplayMember);
                        //copy to
                        policyNo = policyLst.Items[i].ToString();
                        batchName = pBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
                        boxNumber = wBox.ctrlBox.BoxNumber.ToString();

                        if (wQ.GetSysConfigValue(ihConstants.SPECIALBOND_KEY) == ihConstants.SPECIALBOND_VALUE)
                        {
                            ///This is for auto text add
                            Image image = Image.FromFile(fileDlg.FileName.ToString());
                            Bitmap bmp = new Bitmap(image);

                            bmp.SetResolution(200, 200);
                            Graphics g = Graphics.FromImage(bmp);
                            g.DrawString(batchName, new Font("Calibri", 11), Brushes.Black, new PointF(348, 395));
                            g.DrawString(boxNumber, new Font("Calibri", 11), Brushes.Black, new PointF(781, 395));
                            g.DrawString(policyNo, new Font("Calibri", 11), Brushes.Black, new PointF(1099, 395));
                            bmp.Save(tmpBondPath + "\\tempBond.tif", ImageFormat.Tiff);
                            imgBond.LoadBitmapFromFile(tmpBondPath + "\\tempBond.TIF");
                            imgBond.ConvertTo1Bpp(tmpBondPath + "\\tempBond.TIF", tmpBondPath + "\\tempBond.TIF");
                            imageName = tmpBondPath + "\\tempBond.TIF";
                        }
                        pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo);
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        string newPolicyPath = GetPolicyPath(policyNo); //wPolicy.GetPolicyPath();

                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo, string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        int imgCount = wImage.GetImageCount();
                        int pageCount = wImage.GetMaxPageCount();
                        pageCount++;
                        imgCount++;
                        newImageName = policyNo + "_" + pageCount.ToString().PadLeft(5, '0') + "_A-Policy_bond.TIF";
                        origImageName = policyNo + "_" + pageCount.ToString().PadLeft(5, '0') + "_A.TIF";
                        string status = GetPolicyStatus().ToString();
                        if ((Convert.ToInt32(status) != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            if ((Convert.ToInt32(status) == (int)eSTATES.POLICY_FQC) || ((Convert.ToInt32(status) == (int)eSTATES.POLICY_CHECKED)) || (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED))
                            {
                                if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                {
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                }
                                else
                                {
                                    if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                    {
                                        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                    }
                                }
                            }
                            else
                            {
                                if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                {
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                }
                            }
                            //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());
                            System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                            fileSize = info.Length;
                            fileSize = fileSize / 1024;
                            //crd.created_by = "ADMIN";
                            crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo, origImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            //wImage.Save(crd, eSTATES.PAGE_FQC, fileSize, ihConstants._NORMAL_PAGE, imgCount,string.Empty);
                            if (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED)
                            {
                                wImage.Save(crd, eSTATES.PAGE_EXPORTED, ihConstants.POLICYBOND_FILE, newImageName, fileSize, imgCount);
                            }
                            else
                            {
                                wImage.Save(crd, eSTATES.PAGE_FQC, ihConstants.POLICYBOND_FILE, newImageName, fileSize, imgCount);
                            }
                            //wPolicy.UpdateStatus(eSTATES.POLICY_INDEXED,crd);
                            j = j + 1;
                        }
                        //Refresh the image list
                        if (File.Exists(tmpBondPath + "\\tempBond.TIF"))
                        {
                            File.Delete(tmpBondPath + "\\tempBond.TIF");
                        }
                    }
                    if (j > 0)
                    {
                        if (policyLst.Items.Count != 1)
                        {
                            if ((policyLst.SelectedIndex) != (policyLst.Items.Count + 1))
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                            }
                            else
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                        }
                        else
                        {
                            imageLst.Items.Add(origImageName + "-" + ihConstants.POLICYBOND_FILE);
                        }

                        MessageBox.Show("Image has been imported successfully in " + j + " policies.....");

                        prmButtonCopyTo.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while importing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 1;
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
            aeFQCLogic ob = new aeFQCLogic(sqlCon);
            projDs = ob.GetPath(prmProjKey, prmBatchKey);

            if (projDs.Tables[0].Rows.Count > 0)
            {
                Path = projDs.Tables[0].Rows[0]["bundle_path"].ToString();
            }
            else
                Path = string.Empty;

            return Path;
        }
        bool UpdateState(eSTATES prmPageSate, string prmPageName)
        {
            double fileSize;
            bool success = false;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
            try
            {
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmPageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                if (wImage.UpdateStatusFQC(prmPageSate, crd))
                {
                    success = true;
                }
                else
                {
                    success = false;
                }
                imageLst = lstImage;
                if ((prmPageSate != eSTATES.PAGE_DELETED) && (insertFlag != true))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                    fileSize = info.Length;
                    fileSize = fileSize / 1024;

                    wImage.UpdateImageSize(crd, eSTATES.PAGE_FQC, fileSize);
                }
                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_EXPORTED) == false) && (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
                {
                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                    if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                    {
                        UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                    }
                    else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                    {
                        UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                    }
                    else
                    {
                        UpdateStatus(eSTATES.POLICY_FQC, crd);
                    }
                    ///update into transaction log
                    wPolicy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                }
                pBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber);
                wfeBox box = new wfeBox(sqlCon, pBox);
                NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
                state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                state[1] = NovaNet.wfe.eSTATES.POLICY_QC;
                state[2] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

                if (wPolicy.GetPolicyCount(state) == 0)
                {
                    //box.UpdateStatus(eSTATES.BOX_FQC);
                }

                if (GetFileCount(projKey, bundleKey) == 0)
                {
                    ///Update the batch status
                    //wBatch.UpdateStatus(eSTATES.BATCH_SCANNED, wBox.ctrlBox.BatchKey);
                    UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updaing the status " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Policy-" + policyLst.SelectedItem.ToString() + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                success = false;
            }
            insertFlag = false;
            return success;
        }
        public bool UpdateStatus(eSTATES state, Credentials prmCrd, bool pLock)
        {

            bool commitBol = true;
            UpdateStatusLogic ob = new UpdateStatusLogic(sqlCon);
            commitBol = ob.UpdateStatus(Convert.ToString(ctrlPolicy.ProjectKey), Convert.ToString(ctrlPolicy.BatchKey), Convert.ToString(ctrlPolicy.PolicyNumber), prmCrd, new eSTATES[] { }, new eSTATES[] { eSTATES.BATCH_SCANNED, eSTATES.POLICY_EXPORTED }, state);
            return commitBol;
        }
        void UpdateAllStatus()
        {
            string imageName;
            int pos = 0;
            wfeImage wImage = null;
            eSTATES pageState;
            DateTime stDt = DateTime.Now;
            imageLst = lstImage;
            policyLst = lstPolicy;
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            policy = new wfePolicy(sqlCon, ctrlPolicy);

            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
            wImage = new wfeImage(sqlCon, pImage);

            if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (GetPolicyStatus() != 6) && (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
            {
                if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                {
                    UpdateStatus(eSTATES.POLICY_CHECKED, crd, true);
                    pageState = eSTATES.PAGE_CHECKED;
                }
                else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                {
                    UpdateStatus(eSTATES.POLICY_EXCEPTION, crd, true);
                    pageState = eSTATES.PAGE_EXCEPTION;
                }
                else
                {
                    UpdateStatus(eSTATES.POLICY_FQC, crd, true);
                    pageState = eSTATES.PAGE_FQC;
                    UpdateFileStatusForNoImage(wImage);
                }
                if (crd.role != ihConstants._ADMINISTRATOR_ROLE)
                {
                    policy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                }
                //wfePolicy wPolicy = new wfePolicy(sqlCon);
                //int count =  wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
                this.Text = "Final QC";
                this.Text = this.Text;// +"                       Today you have done " + count + " ";
                for (int i = 0; i < imageLst.Items.Count; i++)
                {
                    imageName = imageLst.Items[i].ToString();
                    pos = imageName.IndexOf("-");
                    if (pos > 0)
                    {
                        imageName = imageName.Substring(0, pos);
                    }
                    //To get the index file name from del list selected file name.
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    wImage.UpdateStatusFQC(pageState, crd);
                }
                if (GetFileCount(projKey, bundleKey) == 0)
                {
                    ///Update the batch status
                    //wBatch.UpdateStatus(eSTATES.BATCH_SCANNED, wBox.ctrlBox.BatchKey);
                    UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                }
            }
            DateTime endDt = DateTime.Now;
            TimeSpan tpm = endDt - stDt;
            //MessageBox.Show(tpm.Milliseconds.ToString());
        }
        bool UpdateStateDel(eSTATES prmPageSate, string prmPageName, string prmPolicyNumber)
        {
            bool delBol = false;
            double fileSize;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();

            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, prmPolicyNumber, prmPageName, string.Empty);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            if (wImage.UpdateStatus(prmPageSate, crd) == true)
            {
                delBol = true;
            }

            imageLst = lstImage;
            if (pageDelInsrt == false)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                fileSize = info.Length;
                fileSize = fileSize / 1024;
                wImage.UpdateImageSize(crd, eSTATES.PAGE_INDEXED, fileSize);

            }

            return delBol;
        }

        bool UpdateState(eSTATES prmPageSate, string prmPageName, string prmDocType, string prmIndexImageName)
        {
            double fileSize;
            bool success = false;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
            try
            {
                policyLst = lstPolicy;
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);

                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmPageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                if (wImage.UpdateStatusAndDockType(prmPageSate, prmDocType, prmIndexImageName, crd))
                {
                    if (prmPageSate == eSTATES.PAGE_DELETED)
                    {
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);

                        if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (GetPolicyStatus() != 6) && (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            tw.current("After retrieving Image Count: ");
                            if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                            {
                                UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                            }
                            else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                            {
                                UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                            }
                            else
                            {
                                UpdateStatus(eSTATES.POLICY_FQC, crd);
                            }
                            policy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                        }
                        success = true;
                    }
                }
                else
                {
                    success = false;
                }
                imageLst = lstImage;
                if ((prmPageSate != eSTATES.PAGE_DELETED) && (insertFlag != true))
                {
                    if ((indexFilePath != null) && (indexingOn == false))
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                        fileSize = info.Length;
                        fileSize = fileSize / 1024;

                        wImage.UpdateImageSize(crd, eSTATES.PAGE_FQC, fileSize);
                    }
                    pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                    UpdateAllStatus();

                    pBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber);
                    NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
                    state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                    state[1] = NovaNet.wfe.eSTATES.POLICY_QC;
                    state[2] = NovaNet.wfe.eSTATES.POLICY_SCANNED;



                    if (GetFileCount(projKey, bundleKey) == 0)
                    {
                        UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updaing the status " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Policy-" + policyLst.SelectedItem.ToString() + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                success = false;
            }

            insertFlag = false;
            return success;
        }

        public int GetFileCount(string projkey, string bundleKey)
        {
            DataSet dsImage = new DataSet();
            aeFQCLogic ob = new aeFQCLogic(sqlCon);
            dsImage = ob.GetFileCount(projkey, bundleKey);
            return dsImage.Tables[0].Rows.Count;
        }
        public bool UpdateBundleStatus(int prmProjKey, int prmBatchKey)
        {
            UpdateStatusLogic ob = new UpdateStatusLogic(sqlCon);
            return ob.UpdateBundleStatus(prmProjKey.ToString(), prmBatchKey.ToString(), new eSTATES[] { eSTATES.BATCH_CREATED, eSTATES.BATCH_SCANNED, eSTATES.BATCH_QC }, new eSTATES[] { eSTATES.BATCH_INDEXED }, eSTATES.BATCH_INDEXED);
        }
        public bool UpdateStatus(eSTATES state, Credentials prmCrd)
        {
            bool commitBol = true;
            UpdateStatusLogic ob = new UpdateStatusLogic(sqlCon);
            commitBol = ob.UpdateStatus(Convert.ToString(ctrlPolicy.ProjectKey), Convert.ToString(ctrlPolicy.BatchKey), Convert.ToString(ctrlPolicy.PolicyNumber), prmCrd, new eSTATES[] { }, new eSTATES[] { eSTATES.POLICY_EXPORTED }, state);
            return commitBol;
        }
        int ImageDeleteFromScanning()
        {
            try
            {
                string qcDelPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                int pos = imageLst.SelectedItem.ToString().Contains("-") ? pos = imageLst.SelectedItem.ToString().IndexOf(".TIF") : imageLst.SelectedItem.ToString().IndexOf("-");

                string destFileName = pos <= 0 ? imageLst.SelectedItem.ToString() : imageLst.SelectedItem.ToString().Substring(0, pos) + imageLst.SelectedItem.ToString().Substring(pos + 4) + ".TIF";
                string originalFile = pos <= 0 ? destFileName : imageLst.SelectedItem.ToString().Substring(0, pos) + ".TIF";

                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), originalFile, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);

                string scanPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + originalFile;

                string sourceFileName = indexFolderName + "\\" + destFileName;
                destFileName = qcDelPath + "\\" + originalFile;
                string qcFilePath = sourceFilePath + "\\" + originalFile;
                indexFilePath = sourceFileName;
                if (UpdateStateDel(eSTATES.PAGE_DELETED, originalFile, policyLst.SelectedItem.ToString()) == true)
                {
                    if (FileorFolder.CreateFolder(qcDelPath) == true)
                    {
                        if (File.Exists(destFileName) == false)
                        {
                            File.Move(sourceFileName, destFileName);
                            File.Delete(scanPath);
                        }
                    }
                }
                DeleteNotify(imageLst.SelectedIndex);
                DisplayDocTypeCount();
                ShowImage(true);
                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while doing the operation..." + ex.Message);
            }

            return 0;
        }
        int ImageDelete()
        {
            string qcDelPath = null;
            string sourceFileName = null;
            string destFileName = null;
            string qcFilePath = null;
            string originalFile = null;
            string scanPath = null;
            int pos;
            wfeImage wImage;

            try
            {
                qcDelPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;

                imageLst = lstImage;
                delImgList = lstImageDel;
                pageDelInsrt = true;
                pos = imageLst.SelectedItem.ToString().IndexOf("-");
                if (pos <= 0)
                {
                    destFileName = imageLst.SelectedItem.ToString();
                    originalFile = destFileName;
                }
                else
                {
                    pos = imageLst.SelectedItem.ToString().IndexOf(".TIF");
                    destFileName = imageLst.SelectedItem.ToString().Substring(0, pos) + imageLst.SelectedItem.ToString().Substring(pos + 4) + ".TIF";
                    originalFile = imageLst.SelectedItem.ToString().Substring(0, pos) + ".TIF";
                }
                scanPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + originalFile;
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageLst.SelectedItem.ToString(), string.Empty);
                if (imageLst.SelectedItem.ToString().IndexOf("Signature_page") <= 0)
                {
                    if (UpdateStateDel(eSTATES.PAGE_DELETED, originalFile, policyLst.SelectedItem.ToString()) == true)
                    {

                        sourceFileName = indexFolderName + "\\" + destFileName;
                        destFileName = qcDelPath + "\\" + originalFile;
                        qcFilePath = sourceFilePath + "\\" + originalFile;
                        if (FileorFolder.CreateFolder(qcDelPath) == true)
                        {
                            if (File.Exists(destFileName) == false)
                            {
                                File.Move(sourceFileName, destFileName);

                                File.Delete(scanPath);
                            }
                        }
                    }
                }
                else
                {
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), originalFile, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    if (wImage.DeleteImage() == true)
                    {
                        sourceFileName = indexFolderName + "\\" + destFileName;
                        destFileName = qcDelPath + "\\" + originalFile;
                        qcFilePath = sourceFilePath + "\\" + originalFile;
                        if (FileorFolder.CreateFolder(qcDelPath) == true)
                        {
                            if (File.Exists(destFileName) == false)
                            {
                                File.Delete(sourceFileName);
                                File.Delete(scanPath);
                            }
                        }
                    }
                }
                DeleteNotify(imageLst.SelectedIndex);
                DisplayDocTypeCount();
                ShowImage(true);
                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while doing the operation..." + ex.Message);
            }
            return 0;
        }

        private void DisplayDocTypeCount()
        {
            documentTypeListView.UpdateCounts(Convert.ToInt32(frmMain.projKey), Convert.ToInt32(frmMain.bundleKey), lstPolicy.SelectedItem.ToString());
        }
        public int GetPolicyPhotoStatus()
        {
            aeFQCLogic ob = new aeFQCLogic(sqlCon);
            return Convert.ToInt32(ob.GetPolicyPhotoStatus(ctrlPolicy).Tables[0].Rows[0]["photo"].ToString());
        }

        void ShowImage(bool prmOverWrite)
        {

            string policyName;
            string changedImageName = string.Empty;
            wfeImage wImage = null;
            string photoImageName = null;

            int pos;
            //((ListBox)BoxDtls.Controls["lstPolicy"]).GetItemText();
            try
            {
                policyLst = lstPolicy;
                imageLst = lstImage;
                policyName = policyLst.SelectedItem.ToString();
                if (imageLst.SelectedIndex >= 0)
                {
                    changedImageName = imageLst.SelectedItem.ToString();

                    pos = imageLst.SelectedItem.ToString().IndexOf("-");

                    if (pos > 0)
                    {
                        changedImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), changedImageName, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        changedImageName = wImage.GetIndexedImageName();
                    }
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                    {
                        hasPhotoBol = true;
                    }
                    else
                    {
                        hasPhotoBol = false;
                    }
                    //policyData=(udtPolicy)policy.LoadValuesFromDB();
                    policyPath = GetPolicyPath(); //policyData.policy_path;
                    fileMove = new FileorFolder();
                    string sourcePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                    string destPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                    sourceFilePath = sourcePath;
                    indexFolderName = destPath;
                    if (Directory.Exists(destPath) == false)
                    {
                        //Directory.CreateDirectory(destPath);
                        //FileorFolder.MoveFiles(sourcePath, destPath);
                        FileorFolder.RenameFolder(sourcePath, destPath);
                    }
                    if (pos <= 0)
                    {
                        //fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
                    }
                    //prmButtonRescan.Enabled = true;
                    //prmButtonSkewRight.Enabled = true;
                    imgFileName = destPath + "\\" + changedImageName;
                    if (hasPhotoBol == true)
                    {
                        if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos <= 0))
                        {
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                            prmButtonSkewRight.Enabled = false;
                        }
                        else if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos > 0))
                        {
                            photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                            if (pos > 0)
                            {
                                //fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
                            }
                            imgFileName = destPath + "\\" + changedImageName;
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                            prmButtonSkewRight.Enabled = false;
                        }
                        else
                        {
                            if (pos > 0)
                            {
                                photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                changedImageName = wImage.GetIndexedImageName();
                                if (pos > 0)
                                {
                                    //fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
                                }
                                imgFileName = destPath + "\\" + changedImageName;
                            }
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                        }
                    }
                    else
                    {
                        if (pos > 0)
                        {
                            photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            //fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                        }
                        imgFileName = destPath + "\\" + changedImageName;
                        //Open the source file
                        img.LoadBitmapFromFile(imgFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                    }
                    pictureControl.Refresh();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    long fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImgSize.Text = fileSize.ToString() + " KB";
                }
                ChangeSize();
            }

            catch (Exception ex)
            {

                MessageBox.Show("Error while showing the image", "Image error");
                //stateLog = new MemoryStream();
                //tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                //stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                //exMailLog.Log(ex);
            }
        }
        public bool DeleteNotify(int currIndex)
        {
            lstImage.Items.RemoveAt(currIndex);
            //PopulateImageList((int)lstPolicy.SelectedItem);
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
            //			//MoveNext();
            //			lstImage.Refresh();
            //			lstImageDel.Refresh();
            return true;
        }
        public void PopulateDelList(string prmPolicyNo)
        {
            lstImageDel.Items.Clear();
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", prmPolicyNo);
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            //ListViewItem lstView;
            ArrayList arrImage = new ArrayList();
            wfeImage wImage = new wfeImage(sqlCon);
            //eSTATES[] policyState=new eSTATES[1];
            eSTATES[] imageState = new eSTATES[1];
            //            policyState[0]=currState;
            imageState[0] = eSTATES.PAGE_DELETED;
            //state[1]=eSTATES.POLICY_SCANNED;
            CtrlImage ctrlImage;
            arrImage = wImage.GetDeletedPageList3(imageState, policy);
            for (int i = 0; i < arrImage.Count; i++)
            {
                ctrlImage = (CtrlImage)arrImage[i];
                //lstView=lstImage.Items.Add(ctrlImage.ImageName);
                lstImageDel.Items.Add(ctrlImage.ImageName);
            }
        }

        int ImageCopy()
        {
            string path;
            string changedImageName = string.Empty;
            string scanImageName = string.Empty;
            wfeImage wImage = null;
            int pos;
            string sPath;
            string iPath;
            policyLst = lstPolicy;
            imageLst = lstImage;

            if (imageLst.SelectedIndex >= 0)
            {
                changedImageName = imageLst.SelectedItem.ToString();
                scanImageName = changedImageName;
                pos = imageLst.SelectedItem.ToString().IndexOf("-");

                if (pos > 0)
                {
                    changedImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                    scanImageName = changedImageName;
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), changedImageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    changedImageName = wImage.GetIndexedImageName();
                }
            }

            path = policyPath + "\\" + ihConstants._SCAN_FOLDER;
            sPath = path + "\\" + scanImageName;

            path = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            iPath = path + "\\" + changedImageName;
            if (File.Exists(sPath))
            {
                File.Copy(sPath, iPath, true);
                img.LoadBitmapFromFile(iPath);
                ChangeSize(iPath);
            }
            else
            {
                MessageBox.Show("This image is not present in the folder......");
            }
            return 0;
        }

        int AutoCrop()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Crop
                    img.AutoCrop();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                }
                //ChangeSize();
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping the image", "Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                error = ex.Message;
            }
            return 0;
        }
        int RotateLeft()
        {
            long fileSize;

            OperationInProgress = ihConstants._OTHER_OPERATION;

            try
            {
                if (img.IsValid() == true)
                {
                    //Rotate right -90
                    img.RotateLeft();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                    //delinsrtBol = false;
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    UpdateImageSize(fileSize);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            //imgQc.Write(imgFileName);

            return 0;

        }
        int CropRegister()
        {
            OperationInProgress = ihConstants._CROP;
            return 0;
        }
        int CleanImageRegister()
        {
            OperationInProgress = ihConstants._CLEAN;
            //ChangeSize();
            return 0;
        }
        int ZoomIn()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    pictureControl.Dock = DockStyle.None;
                    //OperationInProgress = ihConstants._OTHER_OPERATION;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height * (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width * (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) * 1.2);
                    pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) * 1.2);
                    pictureControl.Refresh();
                    ChangeZoomSize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping " + ex.Message, "Auto Crop Error");

                error = ex.Message;
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
            //pictureControl.Width = w;
            //pictureControl.Height = h;
            pictureControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
            newImage.Dispose();
        }
        int ZoomOut()
        {
            try
            {
                if (keyPressed > 0)
                {
                    pictureControl.Dock = DockStyle.None;
                    //OperationInProgress = ihConstants._OTHER_OPERATION;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height / (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width / (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) / 1.2);
                    pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) / 1.2);
                    pictureControl.Refresh();
                    ChangeZoomSize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping " + ex.Message, "Auto Crop Error");
                error = ex.Message;
            }
            return 0;
        }

        int RotateRight()
        {
            long fileSize;

            OperationInProgress = ihConstants._OTHER_OPERATION;

            try
            {
                if (img.IsValid() == true)
                {
                    //Rotate right +90
                    img.RotateRight();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();

                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //delinsrtBol = false;
                    UpdateImageSize(fileSize);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,BUndle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }
        void UpdateImageSize(long prmSize)
        {
            //string photoName;
            wfeImage img;
            //long fileSize;

            policyLst = lstPolicy;
            imageLst = lstImage;

            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageLst.SelectedItem.ToString(), string.Empty);
            img = new wfeImage(sqlCon, pImage);
            img.UpdateImageSize(crd, eSTATES.PAGE_QC, prmSize);
        }
        int SkewRight()
        {
            long fileSize;
            OperationInProgress = ihConstants._OTHER_OPERATION;
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Deskew
                    img.AutoDeSkew();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImgSize.Text = fileSize.ToString() + " KB";
                    UpdateImageSize(fileSize);
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

        int SkewLeft()
        {
            OperationInProgress = ihConstants._OTHER_OPERATION;

            //            //rotateAngle = rotateAngle + constRotateAngle;
            //            imgQc = objQc.Skew(imgQc,(-skewXAngle),(-skewYAngle));
            //            pictureControl.Image = MagickNet.Image.ToBitmap(imgQc);
            //            pictureControl.Refresh();
            //imgQc.Write(imgFileName);

            return 0;
        }

        int NoiseRemove()
        {
            long fileSize;
            try
            {
                if (img.IsValid() == true)
                {

                    img.Despeckle(5);
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //lblImageSize = lblImgSize;
                    lblImgSize.Text = fileSize.ToString() + " KB";
                    UpdateImageSize(fileSize);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotating the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
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
            //prmButtonSkewLeft.Enabled = prmControl;
            prmButtonNoiseRemove.Enabled = prmControl;
            prmButtonCleanImg.Enabled = prmControl;
            prmButtonCopyImage.Enabled = prmControl;
            prmButtonDelImage.Enabled = prmControl;
            //prmButtonCopyTo.Enabled = prmControl;
            prmButtonMoveTo.Enabled = prmControl;
            prmButtonCopyImageTo.Enabled = prmControl;
            deReplace.Enabled = prmControl;
        }

        private void ShowPolicyDetails(string file_no)
        {

        }
        public DataTable getFileDetails(string projKey, string bundleKey, string file_no)
        {
            aeFQCLogic ob = new aeFQCLogic(sqlCon);
            return ob.GetFileDetails(projKey, bundleKey, file_no);
        }
        void CreateAllFolders()
        {
            policyLst = lstPolicy;
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                //policyData = (udtPolicy)policy.LoadValuesFromDB();
                policyPath = GetPolicyPath();
                indexFolderName = policyPath + "\\" + ihConstants._FQC_FOLDER;
                sourceFilePath = policyPath + "\\" + ihConstants._QC_FOLDER;
                scanFolder = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                qcFolder = policyPath + "\\" + ihConstants._QC_FOLDER;
                if (Directory.Exists(policyPath) == false)
                {
                    Directory.CreateDirectory(policyPath);
                    if (!Directory.Exists(scanFolder))
                    {
                        Directory.CreateDirectory(scanFolder);
                    }
                    if (Directory.Exists(indexFolderName) == false)
                    {
                        Directory.CreateDirectory(indexFolderName);
                    }
                }
                else
                {
                    if (!Directory.Exists(scanFolder))
                    {
                        Directory.CreateDirectory(scanFolder);
                    }
                    if (Directory.Exists(indexFolderName) == false)
                    {
                        Directory.CreateDirectory(indexFolderName);
                    }
                }
            }
        }
        public int GetPolicyStatus()
        {
            aeFQCLogic ob = new aeFQCLogic(sqlCon);
            return Convert.ToInt32(ob.GetPolicyStatus(ctrlPolicy).Tables[0].Rows[0]["status"]);
        }
        private void PopulateImageList(string prmPolicyNo)
        {
            lstImage.Items.Clear();
            CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", prmPolicyNo);
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            //ListViewItem lstView;
            ArrayList arrImage = new ArrayList();
            CtrlImage ctrlImage;
            arrImage = GetImagesItems(eITEMS.PAGE, prmPolicyNo);
            for (int i = 0; i < arrImage.Count; i++)
            {
                ctrlImage = (CtrlImage)arrImage[i];

                if (ctrlImage.DocType != string.Empty)
                {
                    lstImage.Items.Add(ctrlImage.ImageName + "-" + ctrlImage.DocType);
                }
                else
                {
                    lstImage.Items.Add(ctrlImage.ImageName);
                }
            }
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
        }

        public ArrayList GetImagesItems(eITEMS item, string case_file_no)
        {

            wItemControl wic = null;
            DataSet ds = new DataSet();
            string strQr = string.Empty;
            //wfePolicy queryPolicy = (wfePolicy)wi;
            ArrayList arrItem = new ArrayList();

            if (item == eITEMS.PAGE)
            {
                aeFQCLogic ob = new aeFQCLogic(sqlCon);
                ds = ob.GetImagesItems(item, case_file_no, projKey, bundleKey);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        wic = new CtrlImage(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_key"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[i]["batch_key"].ToString()), "1", ds.Tables[0].Rows[i]["policy_number"].ToString(), ds.Tables[0].Rows[i]["page_name"].ToString(), ds.Tables[0].Rows[i]["doc_type"].ToString());
                        arrItem.Add(wic);
                    }
                }
            }

            return arrItem;
        }

        [Category("Action")]
        [Description("Fires when the Policy is changed.")]
        public event PolicyChangeHandler PolicyChanged;

        private void lstPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            tw.start();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
            DateTime stdt = new DateTime();
            DateTime enddt = new DateTime();
            stdt = DateTime.Now;

            PopulateImageList(lstPolicy.SelectedItem.ToString());
            tw.current("After populating images: ");
            DataSet dsVol = new DataSet();
            //policyLst = lstPolicy;
            //imageLst = lstImage;
            /*
            tw.current("Before creating controls: ");
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString());
            tw.current("Before creating wfePolicy: ");
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            tw.current("Before showing policy details: ");
            */
            if (lstImage.Items.Count > 0)
            {
                lstImage.SelectedIndex = 0;
            }
            tw.current("Before showing policy details: ");
            if (PolicyChanged != null)
            {
                PolicyChanged(this, e);
                ShowPolicyDetails(lstPolicy.SelectedItem.ToString());
            }
            else
            {
                ShowPolicyDetails(lstPolicy.SelectedItem.ToString());
            }
            tw.current("After showing policy details: ");

            if (lstPolicy.SelectedItem != null)
            {
                ShowAllException();
                if (tabControl1.SelectedIndex == 1)
                {
                    PopulateListView();
                }
                string policy_no = lstPolicy.SelectedItem.ToString();
                //string deed_no = policy_no.Split(new char[] { '[', ']' })[1];
                //txtOldDeedno.Text = deed_no;
                lnkPage1.Visible = true;
                lnkPage2.Visible = true;
                lnkPage3.Visible = true;
                EnableDisbleControls(true);
                tw.current("After populating list view: ");
                try
                {
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, lstPolicy.SelectedItem.ToString());
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
                    {
                        EnableDisbleControls(true);
                        //policyData = (udtPolicy)policy.LoadValuesFromDB();
                        string policyPath = GetPolicyPath();
                        indexFolderName = policyPath + "\\" + ihConstants._FQC_FOLDER;
                        sourceFilePath = policyPath + "\\" + ihConstants._QC_FOLDER;
                        scanFolder = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                        qcFolder = policyPath + "\\" + ihConstants._QC_FOLDER;

                        if (lstImage.Items.Count == 0)
                        {
                            pictureControl.Image = null;
                        }

                        ShowPolicyDetails(lstPolicy.SelectedItem.ToString());
                        policyLen = lstPolicy.SelectedItem.ToString().Length;
                        UpdateAllStatus();
                        policy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);

                    }
                    else
                    {
                        Image img = null;
                        pictureControl.Image = img;
                        EnableDisbleControls(false);
                    }
                    //}
                    if (imageLst.Items.Count > 0)
                    {
                        PopulateDelList(lstPolicy.SelectedItem.ToString());
                        DisplayDocTypeCount();
                        ShowImage(false);

                    }
                    else
                    {
                        DisplayDocTypeCount();
                        pictureControl.Image = null;
                    }
                    CreateAllFolders();
                    if (tabControl2.SelectedIndex == 1)
                    {
                        documentTypeListView.SelectedIndex = lvwIndex;
                        documentTypeListView.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while copying all images from Index folder " + ex.Message);
                    //stateLog = new MemoryStream();
                    //tmpWrite = new System.Text.ASCIIEncoding().GetBytes(wBox.ctrlBox.ProjectCode.ToString());
                    //stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                    //exMailLog.Log(ex);
                }
                enddt = DateTime.Now;
                TimeSpan ts = enddt - stdt;

            }
            tw.stop();
        }

        [Category("Action")]
        [Description("Fires when the Image is changed.")]
        public event ImageChangeHandler ImageChanged;

        private void lstImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime st = DateTime.Now;
            indexCount = lstImage.SelectedIndex;

            ShowImage(false);

            PopulateListView();
            if ((ImageChanged != null) && (indexCount >= 0))
            {
                ImageChanged(this, e);
            }
            if (tabControl1.SelectedIndex == 1)
                PopulateListView();
            DateTime end = DateTime.Now;
            TimeSpan duration = end - st;
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
        }

        public event LstImageClick LstImgClick;
        private void lstImage_Click(object sender, EventArgs e)
        {
            DateTime stdt = DateTime.Now;
            tabControl2.SelectedIndex = 0;
            ShowImage(false);
            DateTime enddt = DateTime.Now;
            TimeSpan tp = enddt - stdt;
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
        }
        private bool ChangeAndMoveFile(string prmSourceFileName, string prmDocType)
        {
            string indexFileName = null;
            string sourceFile = null;
            CtrlImage ctrlImg;
            wfeImage wimg;
            int pos;
            int tifPos;
            string origDoctype = string.Empty;
            try
            {
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
                    else
                    {
                        indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + ihConstants.PROPOSALFORM_FILE + ".TIF";
                        sourceFile = indexFolderName + "\\" + prmSourceFileName;
                        indexFilePath = indexFileName;
                    }
                }
                else
                {
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        prmSourceFileName = imageLst.SelectedItem.ToString().Substring(0, pos);
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmSourceFileName, string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        prmSourceFileName = wImage.GetIndexedImageName();
                        sourceFile = indexFolderName + "\\" + prmSourceFileName;
                        indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos - 3) + prmDocType + ".TIF";
                        indexFilePath = indexFileName;
                    }
                    else
                    {
                        if (origDoctype != ihConstants.SIGNATUREPAGE_FILE)
                        {
                            prmSourceFileName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), prmSourceFileName, string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            prmSourceFileName = wImage.GetIndexedImageName();
                            sourceFile = indexFolderName + "\\" + prmSourceFileName;
                            indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos - 3) + ihConstants.PROPOSALFORM_FILE + ".TIF";
                            indexFilePath = indexFileName;
                        }
                    }
                }
                string fileCount = imageLst.SelectedItem.ToString().Substring(policyLen, 4);
                string propFileName = indexFolderName + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";
                if ((File.Exists(propFileName) == false) || (File.Exists(indexFileName) == false))
                {
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        if (origDoctype != prmDocType)
                        {
                            File.Copy(sourceFile, indexFileName, true);
                            File.Delete(sourceFile);
                        }
                    }
                    else
                    {
                        if (origDoctype != ihConstants.SIGNATUREPAGE_FILE)
                        {
                            if (File.Exists(indexFileName) == false)
                            {
                                File.Copy(sourceFile, indexFileName, true);
                                File.Delete(sourceFile);
                            }
                            policyLst = lstPolicy;
                            fileCount = imageLst.SelectedItem.ToString().Substring(policyLen, 4);
                            propFileName = indexFolderName + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";
                            if (File.Exists(propFileName) == false)
                            {
                                ctrlImg = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), policyLst.SelectedItem.ToString() + fileCount + "_B" + ".TIF", string.Empty);
                                wimg = new wfeImage(sqlCon, ctrlImg);
                                int imgCount = wimg.GetImageCount();
                                imgCount++;
                                System.IO.FileInfo info = new System.IO.FileInfo(indexFileName);
                                double fileSize = info.Length;
                                fileSize = fileSize / 1024;

                                if (wimg.Save(crd, eSTATES.PAGE_INDEXED, ihConstants.SIGNATUREPAGE_FILE, policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF", fileSize, imgCount) == true)
                                {
                                    File.Copy(indexFileName, propFileName, true);
                                    //File.Copy(indexFileName, sourceFilePath + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B" + ".TIF", true);
                                    imageLst.Items.Insert(imageLst.SelectedIndex + 1, policyLst.SelectedItem.ToString() + fileCount + "_B.TIF-" + ihConstants.SIGNATUREPAGE_FILE);
                                }
                            }
                            indexFileName = propFileName;
                        }
                    }
                }
                //img.LoadBitmapFromFile(indexFileName);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while changing the index name......" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + sourceFile + "Doc type-" + prmDocType + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                return false;
            }
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


        void ShowIndexedImage()
        {
            string imageName;
            int pos = 0;
            string docType = string.Empty;

            imageLst = lstImage;
            try
            {
                imageName = imageLst.SelectedItem.ToString();
                pos = imageName.IndexOf("-");
                if (pos > 0)
                {
                    docType = imageName.Substring(pos);
                    pos = imageName.IndexOf(".TIF");
                    imageName = imageName.Substring(0, pos) + docType + ".TIF";
                    imgFileName = indexFolderName + "\\" + imageName;
                }
                else
                {
                    imgFileName = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
                }

                //Open the source file
                img.LoadBitmapFromFile(imgFileName);
                ChangeSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while showing image..." + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
        }

        [Category("Action")]
        [Description("Fires when the key Pressed for indexing")]
        public event LstImageIndexKeyPress LstImageIndex;
        private void lstImage_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (LstImageIndex != null)
            {
                LstImageIndex(this, e);
            }
            char DELETE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
            int pos = 0;
            int index;
            bool bolKey = false;
            string selImageName = null;
            string originalFileName = null;
            string indexFileName = null;
            string docType = null;
            bool indexedBol = false;
            bool sigBol = false;

            policyLst = lstPolicy;

            string policyNo = policyLst.SelectedItem.ToString();
            string origDoctype = string.Empty;
            int tifPos = imageLst.SelectedItem.ToString().ToString().IndexOf("-") + 1;
            if (tifPos > 0)
            {
                origDoctype = imageLst.SelectedItem.ToString().Substring(tifPos);
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DELETE)
            {
                ImageDeleteFromScanning();
                return;
            }
            if ((origDoctype == "Signature_page") && (Convert.ToChar(e.KeyChar.ToString().ToUpper()) != DELETE))
            {
                MessageBox.Show("You can not change signature page. You can only delete it...");
                return;
            }

            char keyPressed = Convert.ToChar(e.KeyChar.ToString().ToUpper());
            var docTypeOpt = documentTypeListView.Store.GetDocumentTypeForShortcut(keyPressed);
            if (docTypeOpt != null)
            {
                docType = docTypeOpt;
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), docType);
                bolKey = true;
            }

            if (bolKey == true)
            {
                pos = imageLst.SelectedItem.ToString().IndexOf("-");
                if (pos <= 0)
                {
                    selImageName = imageLst.SelectedItem.ToString() + "-";
                    pos = imageLst.SelectedItem.ToString().IndexOf(".TIF") + 5;
                    selImageName = selImageName.Insert(pos, docType);
                }
                else
                {
                    selImageName = imageLst.SelectedItem.ToString().Substring(0, (pos + 1));
                    selImageName = selImageName + docType;
                }
                if (indexedBol == true)
                {
                    originalFileName = GetFileName(imageLst.SelectedItem.ToString(), docType);
                    pos = originalFileName.ToString().IndexOf(".TIF");
                    indexFileName = selImageName.Substring(0, pos) + "-" + docType + ".TIF";
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo);
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    indexingOn = true;
                    int policyStatus = GetPolicyStatus();

                    if ((policyStatus == 5) || (policyStatus == 4) || (policyStatus == 3) || (policyStatus == (int)eSTATES.POLICY_NOT_INDEXED) || (policyStatus == (int)eSTATES.POLICY_EXCEPTION) || (policyStatus == (int)eSTATES.POLICY_CHECKED))
                    {
                        UpdateState(eSTATES.PAGE_FQC, originalFileName, docType, indexFileName);
                    }
                    else if (policyStatus == (int)eSTATES.POLICY_ON_HOLD)
                    {
                        UpdateState(eSTATES.PAGE_ON_HOLD, originalFileName, docType, indexFileName);
                    }
                    else if (policyStatus == 6 || policyStatus == 7 || policyStatus == 8)
                    {
                        UpdateState(eSTATES.PAGE_EXPORTED, originalFileName, docType, indexFileName);
                    }
                    indexingOn = false;
                }
                index = imageLst.SelectedIndex;
                imageLst.Items[index] = selImageName;
                imageLst.Refresh();



                if ((index + 1) != imageLst.Items.Count)
                {
                    if (docType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        imageLst.SelectedIndex = index + 1;
                        ShowIndexedImage();
                    }
                    if (sigBol == true)
                    {
                        if ((index + 2) != imageLst.Items.Count)
                        {
                            imageLst.SelectedIndex = index + 2;
                            ShowIndexedImage();
                        }
                        else
                        {
                            if ((policyLst.SelectedIndex + 1) != policyLst.Items.Count)
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                        }
                    }
                }
                else
                {
                    if ((policyLst.SelectedIndex + 1) != (policyLst.Items.Count))
                    {
                        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                    }
                }
                if (GetFileCount(projKey, bundleKey) == 0)
                {
                    UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                }
            }
            DisplayDocTypeCount();
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();

        }


        private void lstImageDel_SelectedIndexChanged(object sender, EventArgs e)
        {
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
        }

        public event LstDelImageClick LstDelImgClick;
        private void lstImageDel_Click(object sender, EventArgs e)
        {
            string delFileName;

            delImgList = lstImageDel;
            if (lstImageDel.Items.Count > 0)
            {
                //statusStrip1.Visible = true;
                toolStripStatusLabel1.Text = "";
                toolStripStatusLabel1.Text = "Press 'Insert' to remove image from deleted list";
            }
            else
            {
                statusStrip1.Visible = false;
                toolStripStatusLabel1.Text = "";
            }
            if (delImgList.Items.Count > 0)
            {
                if (delImgList.SelectedIndex >= 0)
                {
                    EnableDisbleControls(false);
                    if (File.Exists(policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER + "\\" + delImgList.SelectedItem.ToString()))
                    {
                        delFileName = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                    }
                    else
                    {
                        delFileName = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                    }

                    string[] searchFileName = Directory.GetFiles(delFileName, delImgList.SelectedItem.ToString());
                    //For searching deleted file in deleted folder.
                    if (searchFileName.Length >= 0)
                    {
                        delFileName = searchFileName[0];

                        img.LoadBitmapFromFile(delFileName);
                        ChangeSize(delFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                    }
                }
            }
            if (LstDelImgClick != null)
            {
                LstDelImgClick(this, e);
            }
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
        }

        [Category("Action")]
        [Description("Fires when the image from deleted list inserted.")]
        public event LstDelImageKeyPress LstDelIamgeInsert;
        private void lstImageDel_KeyDown(object sender, KeyEventArgs e)
        {
            string delPath = null;
            string sourceFileName = null;
            string qcFilePath = null;
            string sourcePath = null;
            string[] searchFileName;
            try
            {
                if (e.KeyCode == Keys.Insert)
                {

                    delPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                    imageDelLst = lstImageDel;
                    policyLst = lstPolicy;
                    imageLst = lstImage;
                    if (imageDelLst.Items.Count > 0)
                    {
                        imageLst = lstImage;
                        ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                        policy = new wfePolicy(sqlCon, ctrlPolicy);
                        policyPath = GetPolicyPath(); //policyData.policy_path;
                        //To get the index file name from del list selected file name.
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageDelLst.SelectedItem.ToString(), string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        string changedImageName = wImage.GetIndexedImageName();

                        if (changedImageName == string.Empty)
                        {
                            sourceFileName = sourceFilePath + "\\" + imageDelLst.SelectedItem.ToString();
                            qcFilePath = indexFolderName + "\\" + imageDelLst.SelectedItem.ToString();
                        }
                        else
                        {
                            sourceFileName = sourceFilePath + "\\" + changedImageName;
                            //Rahul: To change image 
                            qcFilePath = indexFolderName + "\\" + changedImageName;
                        }
                        int pos = imageDelLst.SelectedItem.ToString().IndexOf(".TIF");
                        searchFileName = Directory.GetFiles(delPath, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                        //For searching deleted file in deleted folder.
                        if (searchFileName.Length <= 0)
                        {
                            delPath = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                            searchFileName = Directory.GetFiles(delPath, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                            if (searchFileName.Length > 0)
                            {
                                sourcePath = searchFileName[0];
                            }
                        }
                        else
                        {
                            sourcePath = searchFileName[0];
                        }
                        string scanFilePath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + imageDelLst.SelectedItem.ToString();
                        if (sourcePath != string.Empty)
                        {
                            if (File.Exists(sourcePath) == true)
                            {
                                File.Move(sourcePath, qcFilePath);
                                if (File.Exists(scanFilePath) == false)
                                    File.Copy(qcFilePath, scanFilePath);
                            }
                            insertFlag = true;
                            int dashedPos = changedImageName.IndexOf("-");
                            if (dashedPos > 0)
                            {
                                string docType = changedImageName.Substring(dashedPos + 1);
                                int tifPos = docType.IndexOf(".TIF");
                                if (tifPos > 0)
                                {
                                    docType = docType.Substring(0, tifPos);
                                    UpdateState(eSTATES.PAGE_FQC, imageDelLst.SelectedItem.ToString(), docType, changedImageName);
                                }
                            }
                            else
                            {
                                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                                wPolicy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                                UpdateState(eSTATES.PAGE_NOT_INDEXED, imageDelLst.SelectedItem.ToString());
                            }
                            InsertNotify(imageLst.SelectedIndex);
                        }
                    }
                    deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
                }
                DisplayDocTypeCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while doing the operation.... " + ex.Message);
            }
        }
        public bool InsertNotify(int currIndex)
        {
            //lstImage.Items.Remove(delValue);
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
            //			//MoveNext();
            //			lstImage.Refresh();
            //			lstImageDel.Refresh();
            return true;
        }

        private void cmdPrevious_Click(object sender, EventArgs e)
        {
            policyLst = lstPolicy;

            string policyNo = policyLst.SelectedItem.ToString();

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo);
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                EnableDisbleControls(true);
            }
            else
            {
                EnableDisbleControls(false);
            }
            //BoxDtls.SetCurrentSelection(imageLst.SelectedIndex);
            //ClearSelection();
            MovePrevious();
            ShowImage(false);
        }
        public bool MovePrevious()
        {
            if ((indexCount > 0) && (lstImage.Items.Count > 0))
            {
                //ClearSelection();
                indexCount = indexCount - 1;
                lstImage.SelectedIndex = indexCount;
            }
            if ((indexCount == 0) && (lstImage.Items.Count > 0) && (lstPolicy.SelectedIndex != 0))
            {
                //ClearSelection();
                indexCount = 0;
                lstPolicy.SelectedIndex = lstPolicy.SelectedIndex - 1;
            }
            return true;
        }
        private void cmdNext_Click(object sender, EventArgs e)
        {
            policyLst = lstPolicy;

            string policyNo = policyLst.SelectedItem.ToString();

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyNo);
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                EnableDisbleControls(true);
            }
            else
            {
                EnableDisbleControls(false);
            }

            MoveNext();
            ShowImage(false);
            //ChangeSize();
            //DisplayDocTypeCount();
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
                        lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                    }
                    if (lstImage.Items.Count > 0)
                    {
                        //ClearSelection();
                        lstImage.SelectedIndex = indexCount;
                    }
                    //lstImage.EnsureVisible(indexCount);
                }
                else
                {
                    //					lstImage.Items[indexCount].Selected=true;
                    //					lstImage.EnsureVisible(indexCount);
                    //ClearSelection();
                    lstImage.SelectedIndex = indexCount;
                }
            }
            else
            {
                if ((lstPolicy.Items.Count - 1) > (lstPolicy.SelectedIndex))
                {
                    lstPolicy.SelectedIndex = lstPolicy.SelectedIndex + 1;
                }
            }
            return true;
        }

        private void pictureControl_MouseUp(object sender, MouseEventArgs e)
        {
            cropWidth = Math.Abs(e.X - cropX);
            cropHeight = Math.Abs(e.Y - cropY);


            Cursor = Cursors.Default;
            if (OperationInProgress == ihConstants._CROP || (OperationInProgress == ihConstants._CLEAN))
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

        }
        int Crop(Rectangle rect)
        {
            Bitmap bmpImage = new Bitmap(pictureControl.Image);
            double htRatio = 0;
            double wdRatio = 0;

            try
            {
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
                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error while cropping the image", "Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber.ToString() + "Image name-" + imageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
        }
        int Clean(Rectangle rect)
        {
            //Rectangle rect  = new Rectangle(e.X,e.Y, 6,6);

            double htRatio = 0;
            double wdRatio = 0;
            long fileSize;
            try
            {
                if (img.IsValid() == true)
                {
                    Bitmap bmpImage = img.GetBitmap();

                    htRatio = Convert.ToDouble(bmpImage.Size.Height) / Convert.ToDouble(pictureControl.Height);
                    rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
                    wdRatio = (Convert.ToDouble(bmpImage.Size.Width) / Convert.ToDouble(pictureControl.Width));
                    rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width) * wdRatio);

                    rect.X = Convert.ToInt32(rect.X * wdRatio);
                    rect.Y = Convert.ToInt32(rect.Y * htRatio);
                    if (rect.X <= 0)
                    {
                        rect.Width = rect.Width + rect.X;
                        rect.X = 0;
                    }
                    if (rect.X + rect.Width >= rect.Width)
                    {
                        rect.Width = rect.Width;
                    }
                    if (rect.Y <= 0)
                    {
                        rect.Height = rect.Height + rect.Y;
                        rect.Y = 0;
                    }
                    if (rect.Y + rect.Height >= rect.Height)
                    {
                        rect.Height = rect.Height;
                    }
                    //MessageBox.Show("Before filling " + gdi.GetBitDepth(imageNo).ToString());
                    img.Clean(rect);
                    img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
                }
                //Change the size of the image in relation to the canvas
                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
                //bmpCrop.Dispose();
            }
            catch (Exception ex)
            {
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber.ToString() + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                MessageBox.Show("Error while cleaning the image" + ex.Message, "Crop error");
                return 0;
            }
            return 1;
        }
        private void pictureControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (OperationInProgress == ihConstants._CROP)
            {
                Cursor = Cursors.Cross;
                if ((pictureControl.Image != null) && (cropPen != null))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        pictureControl.Refresh();
                        cropWidth = Math.Abs(e.X - cropX);
                        cropHeight = Math.Abs(e.Y - cropY);
                        pictureControl.CreateGraphics().DrawRectangle(cropPen, ((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
                    }
                }
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

        private void pictureControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (OperationInProgress == ihConstants._CROP)
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
            if (OperationInProgress == ihConstants._CLEAN)
            {
                if (e.Button == MouseButtons.Left)
                {
                    cropX = e.X;
                    cropY = e.Y;

                    //MessageBox.Show("X-" + cropX + "Y-" + cropY);
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
        private int GetDocTypePos()
        {
            if (documentTypeListView.SelectedIndex != -1)
            {
                return lstImage.Items.Cast<string>()
                    .Select((item, index) => (item, index))
                    .Where(item => item.item.Contains(documentTypeListView.SelectedDocumentType))
                    .FirstOrDefault().index;
            }
            return 0;
        }


        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 1)
                {
                    //ThumbnailChangeSize(pictureBox1.Tag.ToString());

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 0 + GetDocTypePos();

                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 2)
                {

                    //ThumbnailChangeSize(pictureBox2.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 1 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 3)
                {

                    //ThumbnailChangeSize(pictureBox3.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 2 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 4)
                {

                    //ThumbnailChangeSize(pictureBox4.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 3 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox5_DoubleClick(object sender, EventArgs e)
        {
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 5)
                {

                    //ThumbnailChangeSize(pictureBox5.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 4 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = lstImage;
            string imageName = imageLst.SelectedItem.ToString();
            if (imageName != null)
            {
                if (imageName.Length >= 6)
                {

                    //ThumbnailChangeSize(pictureBox6.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 5 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void lnkPage1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            imageLst = lstImage;
            imageName = new String[lstImage.Items.Count];
            for (int i = 0; i < imageName.Length; i++)
            {
                imageName[i] = lstImage.Items[i].ToString();
                imageFileName = policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageName[i].Remove(imageName[i].IndexOf("."), 4) + ".TIF";
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);

                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                currntPg = 0;
                if (i == 0)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 1)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 2)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (i == 3)
                {
                    pictureBox4.Image = imgThumbNail;
                    pictureBox4.Tag = imageFileName;
                }
                if (i == 4)
                {
                    pictureBox5.Image = imgThumbNail;
                    pictureBox5.Tag = imageFileName;
                }
                if (i == 5)
                {
                    pictureBox6.Image = imgThumbNail;
                    pictureBox6.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void lnkPage2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            imageLst = lstImage;
            imageName = new String[lstImage.Items.Count];
            for (int i = 6; i < imageName.Length; i++)
            {
                imageName[i] = lstImage.Items[i].ToString();
                imageFileName = policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageName[i].Remove(imageName[i].IndexOf("."), 4) + ".TIF";
                //imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);
                currntPg = 1;
                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                if (i == 6)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 7)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 8)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (i == 9)
                {
                    pictureBox4.Image = imgThumbNail;
                    pictureBox4.Tag = imageFileName;
                }
                if (i == 10)
                {
                    pictureBox5.Image = imgThumbNail;
                    pictureBox5.Tag = imageFileName;
                }
                if (i == 11)
                {
                    pictureBox6.Image = imgThumbNail;
                    pictureBox6.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void lnkPage3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            imageLst = lstImage;
            imageName = new String[lstImage.Items.Count];
            for (int i = 0; i < imageName.Length; i++)
            {
                imageName[i] = lstImage.Items[i].ToString();
                imageFileName = policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageName[i].Remove(imageName[i].IndexOf("."), 4) + ".TIF";
                //imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);
                currntPg = 2;
                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                if (i == 12)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 13)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 14)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        void ClearPicBox()
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
        }

        private int GetKeyVal(string key)
        {
            int cropKeyVal = 0;

            foreach (char c in key)
            {
                cropKeyVal = (int)c;
            }
            return cropKeyVal;
        }

        private void aeFQC_KeyUp(object sender, KeyEventArgs e)
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
            imageLst = lstImage;
            policyLst = lstPolicy;
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
            if (e.KeyCode == Keys.Subtract)
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
            if (e.KeyCode == Keys.F10)
            {
                DialogResult dl = MessageBox.Show(this, "Do you want to drag image from one position to another ? ", "Confirm ! ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dl == DialogResult.Yes)
                {
                    lstImage.AllowDrop = true;
                }
                else
                {
                    lstImage.AllowDrop = false;
                    return;
                }
            }
            if (e.KeyCode == Keys.F11)
            {
                tw.start();
                MoveRemainAtSource();
                tw.stop();
            }
            if (e.KeyCode == Keys.F12)
            {
                tw.start();
                MoveAndGoToTarget();
                tw.stop();
            }
            if (e.KeyCode == Keys.Right)
            {
                //MoveNext();
                ShowImage(false);
                //ChangeSize();
                DisplayDocTypeCount();
            }
            if (e.KeyCode == Keys.Left)
            {
                //MovePrevious();
                ShowImage(false);
                //ChangeSize();
            }
            if (e.KeyCode == Keys.Up)
            {
                //BoxDtls.MovePrevious();
                tw.start();
                ShowImage(false);
                ChangeSize();
                tw.stop();
            }
            if (e.KeyCode == Keys.Down)
            {
                tw.start();
                ShowImage(false);
                ChangeSize();
                tw.stop();
            }
            if (e.KeyCode == Keys.F8)
            {
                Background();
            }
        }

        public bool MoveRemainAtSource()
        {
            var posToStayAt = this.lstImage.SelectedIndex;
            if (MoveImageToPos())
            {
                this.lstImage.SelectedIndex = posToStayAt;
                _imageChangedHandler.Invoke(this, EventArgs.Empty);
                this.moveTarget = null;
            }
            return true;
        }

        public bool MoveAndGoToTarget()
        {
            if (MoveImageToPos())
            {
                this.lstImage.SelectedIndex = this.moveTarget.Value;
                this.moveTarget = null;
            }
            return true;
        }

        int Background()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Crop
                    img.BackGround();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                }
                ChangeSize();
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                //lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping the image", "Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber.ToString() + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
                error = ex.Message;
            }
            return 0;
        }

        private void GetTarget(Nullable<int> x)
        {
            moveTarget = x;
            if (x.HasValue)
            {
                int totalImages = lstImage.Items.Count;
                moveTarget = Math.Max((Math.Min(moveTarget.Value, totalImages) - 1), 0);
            }
        }

        public bool MoveImageToPos()
        {
            //Can only move up when there are records in the list box of images
            //and the currently selected element is less than count of items-1

            frmTargetIndexForMove frmForTargetIndex = new frmTargetIndexForMove(this.GetTarget);
            frmForTargetIndex.ShowDialog();

            int totalImages = lstImage.Items.Count;
            if (!moveTarget.HasValue)
            {
                return false;
            }

            int source = lstImage.SelectedIndex;
            int target = moveTarget.Value;

            string swp = lstImage.SelectedItem.ToString();

            var index = source;
            int direction = target > source ? 1 : -1; //Determine the direction to move

            this.lstImage.SelectedIndexChanged -= _imageChangedHandler;

            while (index != target)
            {
                lstImage.Items[index] = lstImage.Items[index + direction];
                index += direction;
            }
            lstImage.SelectedIndexChanged += _imageChangedHandler;

            lstImage.Items[target] = swp;

            ArrayList arr = new ArrayList();
            for (int i = 0; i < lstImage.Items.Count; i++)
            {
                arr.Add(lstImage.Items[i]);
            }

            CtrlPolicy ctPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, lstPolicy.SelectedItem.ToString());
            wfePolicy wp = new wfePolicy(sqlCon, ctPolicy);
            wp.UpdateSrl(arr);
            return true;
        }

        private void aeFQC_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F1)
            {
                frmHelp frm = new frmHelp("Fqc");
                frm.ShowDialog(this);
            }

            if (e.KeyCode == Keys.Space)
            {
                if ((this.ActiveControl == null) || (this.ActiveControl.Name != "cmdNext" && this.ActiveControl.Name != "cmdPrevious"))
                {
                    object o = new object();
                    EventArgs a = new KeyEventArgs(Keys.None);
                    cmdNext_Click(o, a);
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                //if (dg != null)
                //{
                //    dg.Close();
                //}
            }
            //    if (picBig.Visible == true)
            //    {
            //        picBig.Visible = false;
            //        panelBig.Visible = false;
            //        picBig.Image = null;
            //    }
            //}
            ///For checking todays production count
            if ((e.KeyCode == Keys.F9))
            {
                wfePolicy wPolicy = new wfePolicy(sqlCon);
                int count = wPolicy.GetTransactionLogCount(bundleKey, dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
                frmProductionCount frmProd = new frmProductionCount(count);
                frmProd.ShowDialog(this);
            }
            if ((e.KeyCode == Keys.L) && (e.Control))
            {
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                DataSet pDs = policy.GetPolicyLog();

                DataSet pDs1 = policy.GetPolicyLog1();

                if (pDs.Tables.Count > 0 && pDs1.Tables.Count > 0)
                {
                    if (pDs.Tables[0].Rows.Count > 0)
                    {
                        frmPolicyLog log = new frmPolicyLog(pDs, pDs1, ctrlPolicy.PolicyNumber.ToString());
                        log.Show();
                    }
                }
                else
                {
                    MessageBox.Show("No log information available for this file....");
                }
            }
            if ((e.KeyCode == Keys.C) && (e.Control))
            {
                string imageName;
                policyLst = lstPolicy;
                imageLst = lstImage;
                int pos = imageLst.SelectedItem.ToString().IndexOf("-");
                if (pos > 0)
                {
                    imageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                }
                else
                {
                    imageName = imageLst.SelectedItem.ToString();
                }
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                aeCustomExp frmExp = new aeCustomExp(wImage, sqlCon, crd);
                frmExp.ShowDialog(this);

            }
            if ((e.KeyCode == Keys.Z) && (e.Control))
            {
                ImageCopy();
            }
        }

        private void cmdDeedDetails_Click(object sender, EventArgs e)
        {
            string filename = lstPolicy.SelectedItem.ToString();
            IEntryFormBuilder frmBuilder = new EntryFormBuilder();
            var fm = frmBuilder.CreateEntryForm(sqlCon, projKey, bundleKey, DataLayerDefs.Mode._View, filename, crd);
            fm.ShowDialog(this);
        }

        private void prmButtonCrop_Click(object sender, EventArgs e)
        {
            CropRegister();
        }

        private void prmButtonImportImage_Click(object sender, EventArgs e)
        {
            ImportImageFromDir();
        }

        private void prmButtonAutoCrp_Click(object sender, EventArgs e)
        {
            AutoCrop();
        }

        private void prmButtonRotateRight_Click(object sender, EventArgs e)
        {
            RotateRight();
        }

        private void prmButtonRotateLeft_Click(object sender, EventArgs e)
        {
            RotateLeft();
        }

        private void prmButtonZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void prmButtonZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void prmButtonSkewRight_Click(object sender, EventArgs e)
        {
            SkewRight();
        }

        private void prmButtonNoiseRemove_Click(object sender, EventArgs e)
        {
            NoiseRemove();
        }

        private void prmButtonCleanImg_Click(object sender, EventArgs e)
        {
            CleanImageRegister();
        }

        private void prmButtonCopyImage_Click(object sender, EventArgs e)
        {
            ImageCopy();
        }

        private void prmButtonDelImage_Click(object sender, EventArgs e)
        {
            ImageDelete();
        }

        private void prmPhotoCrop_Click(object sender, EventArgs e)
        {
            PhotoCropRegister();
        }

        private void prmEndPhotoCrop_Click(object sender, EventArgs e)
        {
            EndPhotoCrop();
        }

        private void prmGetPhoto_Click(object sender, EventArgs e)
        {
            GetPhoto();
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
                //pictureControl.Image = img.GetBitmap();

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
                prmGetPhoto.Enabled = false;
                //prmButtonSkewRight.Enabled = true;
            }
            //ChangeSize();
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
                prmButtonCopyImage.Enabled = false;
                prmPhotoCrop.Enabled = false;
                //imageLst = (ListBox)BoxDtls.Controls["lstImage"];

                //img.UpdateImageSize(crd, eSTATES.PAGE_QC,fileSize);
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";
                if (File.Exists(imgFileName))
                {
                    /*
                    if (ino > 0)
                        imgQc.ReleaseGdPictureImage(ino);
	    		
                    ino=imgQc.CreateGdPictureImageFromFile(imgFileName);
                    imgQc.SaveAsTIFF(ino,imgFileName,GdPicture.TiffCompression.TiffCompressionJPEG);
                    Bitmap newBmp = new Bitmap(pictureControl.Width,pictureControl.Height);
                    newBmp = imgQc.GetBitmapFromGdPictureImage(ino);                           
                    pictureControl.Image = newBmp;//imgQc.GetBitmapFromGdPictureImage(ino);
                    */
                    //img.LoadBitmapFromFile(imgFileName);
                    //img.SaveAsTiff(imgFileName, IGRComressionTIFF.JPEG);
                    ChangeSize(imgFileName);
                    //pictureControl.Image=img.GetBitmap();
                    getPhotoOperation = true;
                    prmNext = cmdNext;
                    prmPrevious = cmdPrevious;
                    prmNext.Enabled = false;
                    prmPrevious.Enabled = false;
                    //prmButtonSkewRight.Enabled = false;
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

        private void pictureControl_MouseClick(object sender, MouseEventArgs e)
        {
            Point pt = new Point();
            pt.X = e.X;
            pt.Y = e.Y;
            if (e.Button == MouseButtons.Right)
            {
                policyLst = lstPolicy;
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);

                //int polStatus = policy.GetPolicyStatus();
                //if (polStatus == (int)eSTATES.POLICY_ON_HOLD)
                //{
                //    markNotReadyHoldPolicyToolStripMenuItem.Enabled = false;
                //    markReadyToolStripMenuItem.Enabled = true;
                //}
                //else
                //{
                //    markNotReadyHoldPolicyToolStripMenuItem.Enabled = true;
                //    markReadyToolStripMenuItem.Enabled = false;
                //}
                //contextMenuStrip1.Show(BoxDtls, pt);
            }
        }

        private void aeFQC_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void cmdFetch_Click(object sender, EventArgs e)
        {
            int i;

            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[8];

            state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[1] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            state[2] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[3] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            state[4] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            state[5] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
            state[6] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
            state[7] = NovaNet.wfe.eSTATES.POLICY_QC;
            if (cmbDocType.Text == "All")
            {
                ShowPolicyDetails();
                //policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                lblRecordCount.Text = policyLst.Items.Count.ToString();
                if (policyLst.Items.Count <= 0)
                {
                    MessageBox.Show("No file found for this search....");
                    //rdoAll.Checked = false;
                    //rdoAll.Checked = true;
                    lstImage.Items.Clear();
                    lstImageDel.Items.Clear();
                    pictureControl.Image = null;
                    deLabel1.Visible = false;
                    deLabel2.Visible = false;
                    //lblImageSize.Visible = false;
                    prmButtonCopyTo.Enabled = false;
                    prmButtonCopyProposalForm.Enabled = false;
                    prmButtonCopyProposalReviewSlip.Enabled = false;
                }
                else
                {
                    if (lstPolicy.Items.Count > 0)
                    {
                        lstPolicy.SelectedIndex = 0;
                        deLabel1.Visible = true;
                        deLabel2.Visible = true;
                        //lblImageSize.Visible = true;
                    }

                    if ((textBox1.Text.Trim() == "0") || (textBox1.Text.Trim() == "1"))
                    {
                        prmButtonCopyTo.Enabled = true;
                        prmButtonCopyProposalForm.Enabled = true;
                        prmButtonCopyProposalReviewSlip.Enabled = true;
                    }
                    else
                    {
                        prmButtonCopyTo.Enabled = false;
                        prmButtonCopyProposalForm.Enabled = false;
                        prmButtonCopyProposalReviewSlip.Enabled = false;
                    }
                }
                return;

            }
            if (cmbDocType.Text == "Metadata Missing")
            {
                ShowPolicyDetailsIncomplete();
                //policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                lblRecordCount.Text = policyLst.Items.Count.ToString();
                if (policyLst.Items.Count <= 0)
                {
                    MessageBox.Show("No file found for this search....");
                    //rdoAll.Checked = false;
                    //rdoAll.Checked = true;
                    lstImage.Items.Clear();
                    lstImageDel.Items.Clear();
                    pictureControl.Image = null;
                    deLabel1.Visible = false;
                    deLabel2.Visible = false;
                    //lblImageSize.Visible = false;
                    prmButtonCopyTo.Enabled = false;
                    prmButtonCopyProposalForm.Enabled = false;
                    prmButtonCopyProposalReviewSlip.Enabled = false;
                }
                else
                {
                    if (lstPolicy.Items.Count > 0)
                    {
                        lstPolicy.SelectedIndex = 0;
                        deLabel1.Visible = true;
                        deLabel2.Visible = true;
                        //lblImageSize.Visible = true;
                    }

                    if ((textBox1.Text.Trim() == "0") || (textBox1.Text.Trim() == "1"))
                    {
                        prmButtonCopyTo.Enabled = true;
                        prmButtonCopyProposalForm.Enabled = true;
                        prmButtonCopyProposalReviewSlip.Enabled = true;
                    }
                    else
                    {
                        prmButtonCopyTo.Enabled = false;
                        prmButtonCopyProposalForm.Enabled = false;
                        prmButtonCopyProposalReviewSlip.Enabled = false;
                    }

                }
                return;

            }
            if (cmbDocType.Text == "Image Missing")
            {
                ShowPolicyDetailsImageIncomplete();
                //policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                lblRecordCount.Text = policyLst.Items.Count.ToString();
                if (policyLst.Items.Count <= 0)
                {
                    MessageBox.Show("No file found for this search....");
                    //rdoAll.Checked = false;
                    //rdoAll.Checked = true;
                    lstImage.Items.Clear();
                    lstImageDel.Items.Clear();
                    pictureControl.Image = null;
                    deLabel1.Visible = false;
                    deLabel2.Visible = false;
                    //lblImageSize.Visible = false;
                    prmButtonCopyTo.Enabled = false;
                    prmButtonCopyProposalForm.Enabled = false;
                    prmButtonCopyProposalReviewSlip.Enabled = false;
                }
                else
                {
                    if (lstPolicy.Items.Count > 0)
                    {
                        lstPolicy.SelectedIndex = 0;
                        deLabel1.Visible = true;
                        deLabel2.Visible = true;
                        //lblImageSize.Visible = true;
                    }

                    if ((textBox1.Text.Trim() == "0") || (textBox1.Text.Trim() == "1"))
                    {
                        prmButtonCopyTo.Enabled = true;
                        prmButtonCopyProposalForm.Enabled = true;
                        prmButtonCopyProposalReviewSlip.Enabled = true;
                    }
                    else
                    {
                        prmButtonCopyTo.Enabled = false;
                        prmButtonCopyProposalForm.Enabled = false;
                        prmButtonCopyProposalReviewSlip.Enabled = false;
                    }

                }
                return;

            }
        }


        private void deButton1_Click_1(object sender, EventArgs e)
        {
            MoveImage();
        }

        private void cmdValidatefiles_Click(object sender, EventArgs e)
        {
            if (projKey != null && bundleKey != null)
            {

                eSTATES[] state = new eSTATES[1];
                state[0] = NovaNet.wfe.eSTATES.POLICY_FQC;

                IEntryFormBuilder frmBuilder = new EntryFormBuilder();
                var fm = frmBuilder.CreateEntryForm(sqlCon, projKey, bundleKey, DataLayerDefs.Mode._Add, null, crd);
                fm.ShowDialog(this);
                UpdateStatusForNewlyAddedFile(eSTATES.POLICY_EXCEPTION);
                ShowPolicyDetails();
                PopulatePolicyCombo();
                policyLst = lstPolicy;
                if (lstPolicy.Items.Count > 0)
                {
                    lstPolicy.SelectedIndex = 0;
                }

                System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
                toolTip.SetToolTip(lstImageDel, "Press (Insert) key to insert this deleted image");

                imageLst = lstImage;
                imageDelLst = lstImageDel;
                policyLst = lstPolicy;
                Label delLabel = label3;

                PopulateListView();
                imageLst.Enabled = true;
                policyLst.Enabled = true;

                ShowAllException();
                //this.WindowState = FormWindowState.Maximized;
                PopulateDelList(policyLst.SelectedItem.ToString());
                ///changed in version 1.0.0.1
                DisplayDocTypeCount();

                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
                DisplayMissingKhatian();
            }
            else
            {

                return;
            }
        }
        /// <summary>
        /// Replaces the portion of the filename before the last underscore with a new prefix.
        /// </summary>
        /// <param name="input">Original filename.</param>
        /// <param name="newPrefix">New prefix to apply.</param>
        /// <returns>
        /// A new filename with the prefix replaced. If no underscore is found,
        /// returns the original filename.
        /// </returns>
        public string ReplaceBeforeLastUnderscore(string input, string newPrefix)
        {
            if (string.IsNullOrEmpty(input)) return input;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (input[i] == '_' && i + 1 <= input.Length && char.IsDigit(input[i + 1]))
                {
                    return newPrefix + input.Substring(i);
                }
            }
            int lastUnderscoreIndex = input.LastIndexOf('_');
            return lastUnderscoreIndex == -1
                ? input
                : $"{newPrefix}{input.Substring(lastUnderscoreIndex)}";
        }
        /// <summary>
        /// Renames the parent bundle named folder and process files in Scan & QC subfolders.
        /// </summary>
        private void RenameFoldersandImages()
        {
            aeFQCLogic logic = new aeFQCLogic(sqlCon);
            var newFilename = logic.GetFilenameFromSerial(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), filenameSerial);

            var newParentFolderName = GetPolicyPath(newFilename);
            if (newParentFolderName == policyPath)
            {
                return;
            }
            string[] subfolders = { "Scan", "QC" };

            RenameFolder(policyPath, newParentFolderName);

            subfolders
                .Select(subFolder => Path.Combine(newParentFolderName, subFolder))
                .Where(Directory.Exists)
                .ToList()
                .ForEach(folder => ProcessFiles(folder, newFilename));
        }
        /// <summary>
        /// Renames the parent bundle named folder if it exists
        /// and the target name does not already exist.
        /// </summary>
        /// <param name="oldPath">Current path of the folder to rename.</param>
        /// <param name="newPath">New name/path for the folder.</param>
        private void RenameFolder(string oldPath, string newPath)
        {
            if (Directory.Exists(oldPath))
            {
                Directory.Move(oldPath, newPath);
            }
        }
        /// <summary>
        /// Process all files in a given folder and renames them using the specified prefix.
        /// </summary>
        /// <param name="folderPath">Path of the folder containing files to process.</param>
        /// <param name="newPrefix">New prefix to apply to filenames.</param>
        private void ProcessFiles(string folderPath, string newPrefix)
        {
            Directory.GetFiles(folderPath)
                     .ToList()
                     .ForEach(filePath => RenameFile(filePath, newPrefix));
        }
        /// <summary>
        /// Renames a single file by replacing the portion before
        /// the last underscore with a new prefix.
        /// </summary>
        /// <param name="filePath">Full path of the file to rename.</param>
        /// <param name="newPrefix">New prefix to apply to the file's name.</param>
        private void RenameFile(string filePath, string newPrefix)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string newFileName = ReplaceBeforeLastUnderscore(fileName, newPrefix);
            string newFilePath = Path.Combine(directory, newFileName);

            if (filePath != newFilePath)
            {
                File.Move(filePath, newFilePath);
            }
        }
        private void Update_Click(object sender, EventArgs e)
        {
            if (projKey != null && bundleKey != null)
            {
                filename = lstPolicy.SelectedItem.ToString();

                string casefilename = lstPolicy.SelectedItem.ToString();
                aeFQCLogic logic = new aeFQCLogic(sqlCon);
                filenameSerial = logic.GetFilenameSerial(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), filename);
                IEntryFormBuilder frmBuilder = new EntryFormBuilder();
                var fm = frmBuilder.CreateEntryForm(sqlCon, projKey, bundleKey, DataLayerDefs.Mode._Rename, casefilename, crd);
                var entryFormDialogResult = fm.ShowDialog(this);
                if (entryFormDialogResult.Equals(DialogResult.OK))
                {
                    try
                    {
                        RenameFoldersandImages();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($@"The target name doesn't exist already {ex.Message} ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                ShowPolicyDetails();
                PopulatePolicyCombo();
                //this.WindowState = FormWindowState.Maximized;
                policyLst = lstPolicy;
                if (lstPolicy.Items.Count > 0)
                {
                    lstPolicy.SelectedIndex = 0;
                }

                System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
                toolTip.SetToolTip(lstImageDel, "Press (Insert) key to insert this deleted image");

                imageLst = lstImage;
                imageDelLst = lstImageDel;
                policyLst = lstPolicy;
                Label delLabel = label3;

                PopulateListView();
                imageLst.Enabled = true;
                policyLst.Enabled = true;

                ShowAllException();
                //this.WindowState = FormWindowState.Maximized;
                PopulateDelList(policyLst.SelectedItem.ToString());
                ///changed in version 1.0.0.1
                DisplayDocTypeCount();

                deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
                DisplayMissingKhatian();
            }
            else
            {
                return;
            }
        }

        public bool deleteMeta(string proj, string bundle, string fileName, OdbcTransaction sqlTran)
        {
            bool ret = false;
            if (ret == false)
            {
                _deleteMeta(fileName, sqlTran);

                ret = true;
            }
            return ret;
        }

        public bool _deleteMeta(string fileName, OdbcTransaction sqlTran)
        {
            aeFQCLogic ob = new aeFQCLogic(sqlCon, sqlTran);
            return ob.DeleteMeta(fileName, projKey, bundleKey);
        }

        public bool deleteCaseFile(string proj, string bundle, string fileName, OdbcTransaction sqlTran)
        {
            bool ret = false;
            if (ret == false)
            {
                _deleteCaseFile(fileName, sqlTran);

                ret = true;
            }
            return ret;
        }

        public bool deleteImage(string proj, string bundle, string fileName, OdbcTransaction sqlTran)
        {
            bool ret = false;
            if (ret == false)
            {
                _deleteImage(fileName, sqlTran);

                ret = true;
            }
            return ret;
        }
        public bool deleteTrans(string proj, string bundle, string fileName, OdbcTransaction sqlTran)
        {
            bool ret = false;
            if (ret == false)
            {
                _deleteTrans(fileName, sqlTran);

                ret = true;
            }
            return ret;
        }
        public bool deleteCusEx(string proj, string bundle, string fileName, OdbcTransaction sqlTran)
        {
            bool ret = false;
            if (ret == false)
            {
                _deleteCusEx(fileName, sqlTran);

                ret = true;
            }
            return ret;
        }
        public bool deleteQa(string proj, string bundle, string fileName, OdbcTransaction sqlTran)
        {
            bool ret = false;
            if (ret == false)
            {
                _deleteQa(fileName, sqlTran);

                ret = true;
            }
            return ret;
        }
        public bool _deleteImage(string fileName, OdbcTransaction sqlTran)
        {

            aeFQCLogic ob = new aeFQCLogic(sqlCon, sqlTran);
            return ob.DeleteImage(fileName, projKey, bundleKey);
        }
        public bool _deleteTrans(string fileName, OdbcTransaction sqlTran)
        {
            aeFQCLogic ob = new aeFQCLogic(sqlCon, sqlTran);
            return ob.DeleteTrans(fileName, projKey, bundleKey);
        }
        public bool _deleteCusEx(string fileName, OdbcTransaction sqlTran)
        {

            aeFQCLogic ob = new aeFQCLogic(sqlCon, sqlTran);
            return ob.DeleteCusEx(fileName, projKey, bundleKey);
        }
        public bool _deleteQa(string fileName, OdbcTransaction sqlTran)
        {
            aeFQCLogic ob = new aeFQCLogic(sqlCon, sqlTran);
            return ob.DeleteQa(fileName, projKey, bundleKey);
        }
        public bool _deleteCaseFile(string fileName, OdbcTransaction sqlTran)
        {
            aeFQCLogic ob = new aeFQCLogic(sqlCon, sqlTran);
            return ob.DeleteCaseFile(fileName, projKey, bundleKey);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var SqlTransaction = TransactionManager.GetInstance(sqlCon);
            try
            {
                if (projKey != null && bundleKey != null)
                {

                    DialogResult dr = MessageBox.Show(this, "Do you want to delete this file ? ", "B'Zer -  ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        SqlTransaction.OpenTransaction();
                        bool deletemeta = deleteMeta(projKey, bundleKey, lstPolicy.SelectedItem.ToString(), SqlTransaction.Txn);
                        bool deletecasefile = deleteCaseFile(projKey, bundleKey, lstPolicy.SelectedItem.ToString(), SqlTransaction.Txn);

                        if (deletemeta == true && deletecasefile == true)
                        {
                            bool delImg = deleteImage(projKey, bundleKey, lstPolicy.SelectedItem.ToString(), SqlTransaction.Txn);
                            bool delTran = deleteTrans(projKey, bundleKey, lstPolicy.SelectedItem.ToString(), SqlTransaction.Txn);
                            bool delCusEx = deleteCusEx(projKey, bundleKey, lstPolicy.SelectedItem.ToString(), SqlTransaction.Txn);
                            bool delQa = deleteQa(projKey, bundleKey, lstPolicy.SelectedItem.ToString(), SqlTransaction.Txn);
                            var metadata = new MetadataStore(sqlCon, SqlTransaction.Txn);
                            var fileId = metadata.GetFileFromFileName(lstPolicy.SelectedItem.ToString());
                            if (fileId.HasValue)
                            {
                                var delFileIdFromMVC = metadata.DeleteMetadataValue(fileId.Value);
                                var delFileIdFromMRC = metadata.DeleteMetadataRecord(fileId.Value);
                            }
                            if (delImg == true && delTran == true && delCusEx == true && delQa == true)
                            {
                                SqlTransaction.CommitTransaction();
                                string path = GetPolicyPath();

                                if (Directory.Exists(path))
                                {
                                    Directory.Delete(path, true);
                                }
                            }
                            else
                            {
                                SqlTransaction.AbortTransaction();
                                MessageBox.Show("Transaction Failed.");
                                return;
                            }

                            ShowPolicyDetails();
                            PopulatePolicyCombo();
                            //this.WindowState = FormWindowState.Maximized;
                            policyLst = lstPolicy;
                            if (lstPolicy.Items.Count > 0)
                            {
                                lstPolicy.SelectedIndex = 0;
                            }

                            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
                            toolTip.SetToolTip(lstImageDel, "Press (Insert) key to insert this deleted image");

                            imageLst = lstImage;
                            imageDelLst = lstImageDel;
                            policyLst = lstPolicy;
                            Label delLabel = label3;

                            PopulateListView();
                            imageLst.Enabled = true;
                            policyLst.Enabled = true;

                            ShowAllException();
                            //this.WindowState = FormWindowState.Maximized;
                            PopulateDelList(policyLst.SelectedItem.ToString());
                            ///changed in version 1.0.0.1
                            DisplayDocTypeCount();

                            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();

                            MessageBox.Show(this, "Case file deleted successfully ...", "B'Zer -  !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        DisplayMissingKhatian();
                    }
                    else
                    {
                        return;
                    }


                }
                else
                { return; }
            }
            catch (Exception ex)
            {
                SqlTransaction.AbortTransaction();
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();

            //imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            //policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            if (imageLst.Items.Count > 0)
            {
                pPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1", policyLst.SelectedItem.ToString());
                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                if (GetPolicyStatus() == (int)eSTATES.POLICY_EXCEPTION)
                {
                    if (wPolicy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_RECTIFIED, ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED) == true)
                    {
                        //UpdateStatus(eSTATES.POLICY_FQC, crd);
                        MessageBox.Show("Successfully updated....");
                    }
                }
                ShowAllException();
            }
        }

        private void cmdResolved_Click(object sender, EventArgs e)
        {
            if (imageLst.Items.Count > 0)
            {
                //pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), wBox.ctrlBox.BoxNumber.ToString(), policyLst.SelectedItem.ToString());
                //wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);

                string imageName;
                policyLst = lstPolicy;
                imageLst = lstImage;
                int pos = imageLst.SelectedItem.ToString().IndexOf("-");
                if (pos > 0)
                {
                    imageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                }
                else
                {
                    imageName = imageLst.SelectedItem.ToString();
                }
                pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);

                if (listView1.Items.Count > 0)
                {
                    if (listView1.CheckedItems.Count > 0)
                    {
                        for (int i = 0; i < listView1.CheckedItems.Count; i++)
                        {
                            string prb = listView1.CheckedItems[i].Text;
                            wImage.UpdateCustomException(ihConstants._RESOLVED, prb, crd);
                        }
                    }
                }

                ShowAllException();
                PopulateListView();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmCustExc frmCust = new frmCustExc(sqlCon, projKey, bundleKey);
            frmCust.ShowDialog(this);
        }

        private void deReplace_Click(object sender, EventArgs e)
        {
            ReplaceImage();
        }

        private void lstImage_DragDrop(object sender, DragEventArgs e)
        {
            ArrayList arr = new ArrayList();
            CtrlPolicy ctPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, lstPolicy.SelectedItem.ToString());
            wfePolicy wp = new wfePolicy(sqlCon, ctPolicy);

            Point point = lstImage.PointToClient(new Point(e.X, e.Y));
            int index = this.lstImage.IndexFromPoint(point);
            if (index < 0) index = this.lstImage.Items.Count - 1;
            object data = lstImage.SelectedItem.ToString();
            this.lstImage.Items.Remove(data);
            this.lstImage.Items.Insert(index, data);

            lstImage.SelectedIndex = index;

            lstImage.AllowDrop = false;

            for (int i = 0; i < lstImage.Items.Count; i++)
            {
                arr.Add(lstImage.Items[i]);
            }
            wp.UpdateSrl(arr);
        }

        private void lstImage_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lstImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (lstImage.AllowDrop == true)
            {
                if (this.lstImage.SelectedItem == null) return;
                this.lstImage.DoDragDrop(this.lstImage.SelectedItem, DragDropEffects.Move);
            }

        }

        private void lstImage_MouseClick(object sender, MouseEventArgs e)
        {
            DateTime stdt = DateTime.Now;
            tabControl2.SelectedIndex = 0;
            ShowImage(false);
            DateTime enddt = DateTime.Now;
            TimeSpan tp = enddt - stdt;
            deLabel2.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
        }

        private void lstImage_MouseMove(object sender, MouseEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox == null) { return; }
            var strTip = string.Empty;
            var index = listbox.IndexFromPoint(e.Location);
            if ((index >= 0) && (index < listbox.Items.Count))
                strTip = listbox.Items[index].ToString() + " @[" + Convert.ToString(index + 1) + "]";
            if (_toolTip.GetToolTip(listbox) != strTip)
            {
                _toolTip.SetToolTip(listbox, strTip);
            }
        }
        /// <summary>
        /// Update the status of file in case-file-master for not containing any images
        /// </summary>
        /// <param name="wImage"></param>
        private void UpdateFileStatusForNoImage(wfeImage wImage)
        {
            int imgCount = wImage.GetImageCount();
            if (imgCount == 0)
            {
                UpdateStatus(eSTATES.POLICY_EXCEPTION, crd, true);
            }
        }
        /// <summary>
        /// Update status of newly added files to POLICY_EXCEPTION in case-file-master for not conatining any image initially
        /// </summary>
        /// <param name="newState"></param>
        private void UpdateStatusForNewlyAddedFile(eSTATES newState)
        {
            UpdateStatusLogic logic = new UpdateStatusLogic(sqlCon);
            logic.UpdateStatus(projKey, bundleKey, new eSTATES[] { }, new eSTATES[] { eSTATES.BATCH_QC, eSTATES.POLICY_FQC, eSTATES.POLICY_EXCEPTION, eSTATES.POLICY_NOT_INDEXED }, newState);
        }
        public enum imgCountConStatus
        {
            lessThan,
            greaterThanEqualTo
        }

        Dictionary<string, imgCountConStatus> mapImgCountCondition = new Dictionary<string, imgCountConStatus>()
        {
            { "Less Than (<)", imgCountConStatus.lessThan },
            { "Atleast (>=)", imgCountConStatus.greaterThanEqualTo }
        };

        private void LoadImgCountCondition()
        {
            cmbImgCountCondition.DataSource = new BindingSource(mapImgCountCondition, null);
            cmbImgCountCondition.DisplayMember = "Key";
            cmbImgCountCondition.ValueMember = "Value";
        }
        private void HandleEmptyImageCountSearch()
        {
            ShowPolicyDetails();
            toolStripStatusLabelTotalKhatianCount.Text = string.Empty;
            toolStripStatusLabelTotalKhatianCount.Enabled = false;

            if (lstPolicy.Items.Count > 0)
            {
                lstPolicy.SelectedIndex = 0;
                deLabel1.Visible = true;
                deLabel2.Visible = true;
            }
        }
        private void PopulatePolicyList(DataTable policyData)
        {
            lstPolicy.Items.Clear();
            foreach (DataRow item in policyData.Rows)
            {
                lstPolicy.Items.Add(item["policy_number"]);
            }
        }
        private void HandleNoResultsFound()
        {
            MessageBox.Show("No file found for this search....");
            lstImage.Items.Clear();
            lstImageDel.Items.Clear();
            pictureControl.Image = null;
            deLabel1.Visible = false;
            deLabel2.Visible = false;
            prmButtonCopyTo.Enabled = false;
            prmButtonCopyProposalForm.Enabled = false;
            prmButtonCopyProposalReviewSlip.Enabled = false;
        }
        private void buttonFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbImgCountValue.Text))
            {
                HandleEmptyImageCountSearch();
                return;
            }
            var arr = FilterFilesByImageCount(txbImgCountValue.Text, mapImgCountCondition[cmbImgCountCondition.Text]);
            toolStripStatusLabelTotalKhatianCount.Enabled = true;
            toolStripStatusLabelTotalKhatianCount.Text = $"Total number of khatian: {arr.Rows.Count.ToString()}";
            PopulatePolicyList(arr);
            if (lstPolicy.Items.Count > 0)
            {
                lstPolicy.SelectedIndex = 0;
                deLabel1.Visible = true;
                deLabel2.Visible = true;
                DisplayDocTypeCount();
            }
            else
            {
                HandleNoResultsFound();
            }
        }
        private void DisplayMissingKhatian()
        {
            //var bundleName = GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            //khatianMismatchUserControl1.GetKhatianValidationResult(sqlCon, projKey, bundleKey, bundleName);
        }

        private void textBoxSearchKhatian_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var searchValue = textBoxSearchKhatian.Text.Trim();
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        var matchedItem = lstPolicy.Items.Cast<string>()
                            .FirstOrDefault(item => item.Contains(searchValue));
                        if (matchedItem != null)
                        {
                            lstPolicy.SelectedItem = matchedItem;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to find lstPolicy item. " + ex.Message);
            }
        }

        private void khatianMismatchUserControl1_Load(object sender, EventArgs e)
        {

        }
    }
}