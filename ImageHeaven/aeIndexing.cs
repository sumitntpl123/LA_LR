using bzer_data_entry;
using DockSample;
using ImageHeaven.Logic;
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
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ImageHeaven
{
    public partial class aeIndexing : Form
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
        private static string docType;
        private CtrlBox pBox = null;
        private string indexFilePath = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        private string scanFolder = null;
        public static string projKey = null;
        public static string bundleKey = null;
        public static string boxNumber = null;
        OdbcDataAdapter sqlAdap;

        bool isOk;
        bool hasPhotoBol;
        bool pageDelInsrt;
        string policyPath = string.Empty;
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
        private long fileSize;
        private int OperationInProgress;
        private bool IndexingOperation = false;
        private ListBox delImgList = null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private Pen cropPen;
        private int cropPenSize = 1;
        private System.Drawing.Color cropPenColor = System.Drawing.Color.Blue;

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


        private short scanDpi;
        private short scangreyDpi;
        //Scanning
        private bool colorMode;
        private bool msgfilter;
        private int scanWhat = 0;
        //private Label lblBatch;

        private bool FlatBedScan = false;

        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed = 1;
        private ImageConfig config = null;
        MemoryStream stateLog;
        byte[] tmpWrite;
        //GD objects
        //private GdPicture.GdPictureImaging imgQc = new GdPicture.GdPictureImaging();
        //private int ino;
        private Label lblImageSize = null;
        private bool delinsrtBol = false;
        private ListBox policyLst = null;
        private ListBox imageLst = null;
        private ListBox imageDelLst = null;
        private CtrlPolicy ctrlPolicy = null;
        private wfePolicy policy = null;
        private udtPolicy policyData = null;
        private FileorFolder fileMove = null;
        private string sourceFilePath = null;
        private string indexFolderName = null;
        private Credentials crd = new Credentials();
        private int policyLen = 0;
        private Imagery img;
        private string qcFolderName = null;
        private bool photoCropOperation = false;
        private bool getPhotoOperation = false;

        private int indexCount = 0;
        private eSTATES[] currState;
        private eSTATES[] imageCurrState;
        private int pLeft = 0;
        private int pTop = 0;
        private string policy_Path = string.Empty;

        private aeIndexingLogic aeIndexingLogic;
        private UpdateStatusLogic updateStatusLogic;
        private GetItemsLogic getItemsLogic;

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        public aeIndexing(OdbcConnection prmCon, Credentials prmCrd)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            sqlCon = prmCon;
            crd = prmCrd;
            InitializeComponent();

            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);

            currState = new eSTATES[1];

            currState[0] = eSTATES.POLICY_QC;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            this.Text = "DOC Type Association";
            exMailLog.SetNextLogger(exTxtLog);

            aeIndexingLogic = new aeIndexingLogic(sqlCon, crd);
            updateStatusLogic = new UpdateStatusLogic(sqlCon);
            getItemsLogic = new GetItemsLogic(sqlCon);

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

            documentTypeListView.Init(sqlCon);
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
        void UpdateImageSize(long prmSize)
        {
            wfeImage img;

            policyLst = lstPolicy;
            imageLst = lstImage;
            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageLst.SelectedItem.ToString(), string.Empty);
            img = new wfeImage(sqlCon, pImage);
            img.UpdateImageSize(crd, eSTATES.PAGE_QC, prmSize);
        }
        int CropRegister()
        {
            OperationInProgress = ihConstants._CROP;
            pageDelInsrt = false;
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
                    //pictureControl.Image = img.GetBitmap();
                    img.LoadBitmapFromFile(imgFileName);
                }
                ChangeSize();
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImageSize = lblImgSize;
                lblImageSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
                pageDelInsrt = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping the image", "Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return 0;
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
                        pictureControl.Width = 525;
                        pictureControl.Height = 693;
                        img.LoadBitmapFromFile(imgFileName);
                        pictureControl.Image = img.GetBitmap();
                        pictureControl.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        pictureControl.Width = 525;
                        pictureControl.Height = 693;
                        img.LoadBitmapFromFile(imgFileName);
                        pictureControl.Image = img.GetBitmap();
                        pictureControl.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error ..." + ex.Message, "Error");
            }
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
                    img.LoadBitmapFromFile(imgFileName);
                    //Show the image back in picture box
                    //pictureControl.Image = img.GetBitmap();
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
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
                    img.LoadBitmapFromFile(imgFileName);
                    //Show the image back in picture box
                    //pictureControl.Image = img.GetBitmap();
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
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
                exMailLog.Log(ex);
            }
            return 0;
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
                    img.LoadBitmapFromFile(imgFileName);
                    //Show the image back in picture box
                    //pictureControl.Image = img.GetBitmap();
                    ChangeSize();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    fileSize = fileSize / 1024;
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
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();

                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
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
        int CleanImageRegister()
        {
            OperationInProgress = ihConstants._CLEAN;
            pageDelInsrt = false;
            return 0;
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
        private void ChangeSize(string fName)
        {
            Image imgTot = null;

            try
            {
                if (img.IsValid() == true)
                {
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

                    pictureControl.Width = 525;
                    pictureControl.Height = 693;
                    pictureControl.Image = CreateThumbnail(newImage, 525, 693); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    newImage.Dispose();
                    pictureControl.Refresh();
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
                        pictureControl.Width = 525;
                        pictureControl.Height = 693;
                        img.LoadBitmapFromFile(fName);
                        pictureControl.Image = img.GetBitmap();
                        pictureControl.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error ..." + ex.Message, "Error");
            }
        }
        private bool GetThumbnailImageAbort()
        {
            return false;
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
        void DisplayValues()
        {
            projKey = frmMain.projKey;
            bundleKey = frmMain.bundleKey;
            boxNumber = "1";
            lblProject.Text = "Project: " + GetProjectName(Convert.ToInt32(projKey));
            lblBatch.Text = "Bundle: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            lblProject.ForeColor = Color.RoyalBlue;
            lblBatch.ForeColor = Color.RoyalBlue;
            pendingLabelCount.ForeColor = Color.RoyalBlue;

        }
        public string GetProjectName(int prmProjectKey)
        {
            string projName = null;
            DataSet projDs = new DataSet();
            projDs = aeIndexingLogic.GetProjectName(prmProjectKey);
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
            DataSet projDs = new DataSet();
            string projName = null;
            projDs = aeIndexingLogic.GetBundleName(prmProjectKey, prmBundleKey);
            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["bundle_code"].ToString();
            }
            else
                projName = string.Empty;

            return projName;
        }
        bool UpdateState(eSTATES prmPageSate, string prmPageName, string prmPolicyNumber)
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
                    if (UpdateState(eSTATES.PAGE_DELETED, originalFile, policyLst.SelectedItem.ToString()) == true)
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
                scannedImagesLabel.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while doing the operation..." + ex.Message);
            }
            return 0;
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
            string Path = null;
            DataSet projDs = new DataSet();
            projDs = aeIndexingLogic.GetPath(prmProjKey, prmBatchKey);

            if (projDs.Tables[0].Rows.Count > 0)
            {
                Path = projDs.Tables[0].Rows[0]["bundle_path"].ToString();
            }
            else
                Path = string.Empty;

            return Path;
        }

        public int GetPolicyPhotoStatus()
        {
            DataSet policyDs = new DataSet();

            int ProjKey = ctrlPolicy.ProjectKey;
            int BatchKey = ctrlPolicy.BatchKey;
            string PolicyNumber = ctrlPolicy.PolicyNumber;

            policyDs = aeIndexingLogic.GetPolicyPhotoStatus(policyDs, ProjKey, BatchKey, PolicyNumber);

            return Convert.ToInt32(policyDs.Tables[0].Rows[0]["photo"].ToString());
        }

        void ShowImage(bool prmOverWrite)
        {
            string policyName;
            string changedImageName = string.Empty;
            string photoImageName = null;
            wfeImage wImage = null;

            int pos;
            try
            {
                policyLst = lstPolicy;
                imageLst = lstImage;
                if (policyLst.SelectedItem != null)
                {
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

                        //policyData = (udtPolicy)policy.LoadValuesFromDB();
                        policyPath = GetPolicyPath(); //policyData.policy_path;
                        fileMove = new FileorFolder();
                        string sourcePath = policyPath + "\\" + ihConstants._QC_FOLDER;
                        string destPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;

                        sourceFilePath = sourcePath;
                        indexFolderName = destPath;

                        if (FileorFolder.RenameFolder(sourceFilePath, destPath) == false)
                        {
                            if (pos <= 0)
                            {
                                //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                            }
                            imgFileName = destPath + "\\" + changedImageName;
                            //Open the source file
                            //img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();

                            prmButtonSkewRight.Enabled = true;
                            if (hasPhotoBol == true)
                            {
                                if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos <= 0))
                                {
                                    imgFileName = destPath + "\\" + changedImageName;
                                    //Open the source file
                                    img.LoadBitmapFromFile(imgFileName);
                                    //Show the image back in picture box
                                    //pictureControl.Image = img.GetBitmap();
                                }
                                else if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos > 0))
                                {
                                    photoImageName = imageLst.Items[0].ToString().Substring(0, pos);
                                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    changedImageName = wImage.GetIndexedImageName();
                                    if (pos > 0)
                                    {
                                        //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                                    }
                                    imgFileName = destPath + "\\" + changedImageName;
                                    //Open the source file
                                    img.LoadBitmapFromFile(imgFileName);
                                    //Show the image back in picture box
                                    //pictureControl.Image = img.GetBitmap();
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
                                            //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                                        }
                                        imgFileName = destPath + "\\" + changedImageName;
                                        //Open the source file
                                        img.LoadBitmapFromFile(imgFileName);
                                        //Show the image back in picture box
                                        //pictureControl.Image = img.GetBitmap();
                                    }
                                    else
                                    {
                                        imgFileName = destPath + "\\" + changedImageName;

                                        //Open the source file
                                        img.LoadBitmapFromFile(imgFileName);
                                        //Show the image back in picture box
                                        //pictureControl.Image = img.GetBitmap();
                                    }
                                }
                            }
                            else
                            {
                                if (pos > 0)
                                {
                                    photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), photoImageName, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    changedImageName = wImage.GetIndexedImageName();
                                }
                                imgFileName = destPath + "\\" + changedImageName;
                                img.LoadBitmapFromFile(imgFileName);
                            }
                            pictureControl.Refresh();
                            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                            long fileSize = info.Length;
                            fileSize = fileSize / 1024;
                            lblImgSize.Text = fileSize.ToString() + " KB";

                        }
                        else
                            MessageBox.Show("Error while creaing index folder");
                    }
                    ChangeSize();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error while showing the image", "Image error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
        }

        private void DisplayDocTypeCount()
        {
            documentTypeListView.UpdateCounts(Convert.ToInt32(frmMain.projKey), Convert.ToInt32(frmMain.bundleKey), lstPolicy.SelectedItem.ToString());
        }
        public bool DeleteNotify(int currIndex)
        {
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
            arrImage = wImage.GetDeletedPageList2(currState, imageState, policy);
            for (int i = 0; i < arrImage.Count; i++)
            {
                ctrlImage = (CtrlImage)arrImage[i];
                lstImageDel.Items.Add(ctrlImage.ImageName);
            }
        }

        private void aeIndexing_Load(object sender, EventArgs e)
        {

            ReadConfigKey();
            cmbDesValue.SelectedIndex = 0;
            DisplayValues();
            try
            {
                if (lblImgSize != null)
                    lblImgSize.ForeColor = Color.Black;

                System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();

                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
                ArrayList arrPolicy = new ArrayList();


                if (File.Exists(configFile))

                    delImage = new NovaNet.Utils.ImageManupulation(CropRegister);
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
                prmButtonNoiseRemove.AutoSize = true;
                prmButtonNoiseRemove.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(CleanImageRegister);
                bttnToolTip.SetToolTip(prmButtonCleanImg, "Shortcut Key-" + cleanKey);
                prmButtonCleanImg.Text = "Clean";
                prmButtonCleanImg.AutoSize = true;
                prmButtonCleanImg.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(ImageCopy);
                bttnToolTip.SetToolTip(prmButtonCopyImage, "Shortcut Key-(Control+z)");
                prmButtonCopyImage.Text = "Copy Original";
                prmButtonCopyImage.AutoSize = true;
                prmButtonCopyImage.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(ImageDelete);
                bttnToolTip.SetToolTip(prmButtonDelImage, "Shortcut Key-" + deleteKey);
                prmButtonDelImage.Text = "Delete";
                prmButtonDelImage.AutoSize = true;
                prmButtonDelImage.ForeColor = Color.Black;
                ShowPolicyDetails();
                this.WindowState = FormWindowState.Maximized;
                policyLst = lstPolicy;
                if (lstPolicy.Items.Count > 0)
                {
                    lstPolicy.SelectedIndex = 0;
                }

                System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
                toolTip.SetToolTip(lstImageDel, "Press (Insert) key to insert this deleted image");

                if (policyLst.SelectedItem != null)
                {
                    this.Text = this.Text;
                    PopulateDelList(policyLst.SelectedItem.ToString());
                    markNotReadyHoldPolicyToolStripMenuItem.Visible = false;
                    markReadyToolStripMenuItem.Visible = false;
                    lstImage.SelectionMode = SelectionMode.One;
                    DisplayDocTypeCount();

                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (lstImage.Items.Count == 0)
                    {
                        MessageBox.Show(this, "No image found for this file", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        pictureControl.Image = null;
                    }
                    else { ShowImage(false); }

                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    policyLen = policyLst.SelectedItem.ToString().Length;
                }

                scannedImagesLabel.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
                lstPolicy.Select();
                lstImage.Focus();
                lstImage.Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading indexing form... " + ex.Message.ToString());
            }
        }

        private void ShowPolicyDetails()
        {
            lstPolicy.Items.Clear();
            ArrayList arrPolicy = new ArrayList();
            CtrlPolicy ctrPol;

            arrPolicy = GetItems(eITEMS.POLICY, currState);
            for (int i = 0; i < arrPolicy.Count; i++)
            {
                ctrPol = (CtrlPolicy)arrPolicy[i];
                lstPolicy.Items.Add(ctrPol.PolicyNumber);
            }
            if (arrPolicy.Count > 0)
            {
                pendingLabelCount.Text = "Pending: " + GetItems(eITEMS.POLICY, currState).Count;
            }
            else
            {
                pendingLabelCount.Text = "Pending : 0";
                lstImageDel.Items.Clear();
            }
        }
        public ArrayList GetItems(eITEMS item, eSTATES[] state)
        {
            return getItemsLogic.GetItems(item, projKey, bundleKey, new eSTATES[] { eSTATES.BATCH_SCANNED });
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
                    lstImage.Items.Add(ctrlImage.ImageName + "-" + ctrlImage.DocType);
                }
                else
                    lstImage.Items.Add(ctrlImage.ImageName);
            }
        }

        public ArrayList GetImagesItems(eITEMS item, eSTATES[] state, string case_file_no)
        {
            wItemControl wic = null;
            string strQr = string.Empty;
            ArrayList arrItem = new ArrayList();

            if (item == eITEMS.PAGE)
            {
                var ds = aeIndexingLogic.GetImagesItems(item, state, projKey, bundleKey, case_file_no);

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
        }

        public DataTable getFileDetails(string projKey, string bundleKey, string file_no)
        {
            return aeIndexingLogic.GetFileDetails(projKey, bundleKey, file_no);
        }


        [Category("Action")]
        [Description("Fires when the Policy is changed.")]
        public event PolicyChangeHandler PolicyChanged;
        private void lstPolicy_SelectedIndexChanged(object sender, EventArgs e)
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

                LockPolicy(crd, sqlTrans);
            }
            else
            {
                OdbcTransaction sqlTrans = null;

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
                DisplayDocTypeCount();
            }

        }

        public bool LockPolicy(Credentials pCrd, OdbcTransaction pTrns)
        {
            return aeIndexingLogic.LockPolicy(pCrd, pTrns, ctrlPolicy);

        }

        public event LstImageClick LstImgClick;
        private void LstImage_Click(object sender, EventArgs e)
        {
            EnableDisbleControls(true);
            ShowImage(false);
            if (LstImgClick != null)
            {
                LstImgClick(this, e);
            }
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
                                    imageLst.Items.Insert(imageLst.SelectedIndex + 1, policyLst.SelectedItem.ToString() + fileCount + "_B.TIF-" + ihConstants.SIGNATUREPAGE_FILE);
                                }
                            }
                            indexFileName = propFileName;
                        }
                    }
                }
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
        void UpdateState(eSTATES prmPageSate, string prmPageName, string prmDocType, string prmIndexImageName, string prmPolicyNumber)
        {
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();

            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, prmPolicyNumber, prmPageName, string.Empty);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            wImage.UpdateStatusAndDockType(prmPageSate, prmDocType, prmIndexImageName, crd);

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
                    if (File.Exists(indexFolderName + "\\" + imageName) == false)
                    {
                        File.Copy(sourceFilePath + "\\" + imageName, indexFolderName + "\\" + imageName);
                    }
                    imgFileName = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
                }
                img.LoadBitmapFromFile(imgFileName);
                //Open the source file
                ChangeSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while showing image..." + ex.Message);
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
            DialogResult rlst;
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);

            int pos = 0;
            int index;
            bool bolKey = false;
            string selImageName = null;
            string originalFileName = null;
            string indexFileName = null;
            string docType = null;
            bool sigBol = false;
            DateTime st = DateTime.Now;

            bool indexedBol = false;
            policyLst = lstPolicy;

            string policyNo = policyLst.SelectedItem.ToString();

            string origDoctype = string.Empty;
            int tifPos = imageLst.SelectedItem.ToString().ToString().IndexOf("-") + 1;
            if (tifPos > 0)
            {
                origDoctype = imageLst.SelectedItem.ToString().Substring(tifPos);
            }

            if ((origDoctype == "Signature_page") && (char.ToUpper(e.KeyChar) != deleteKey[0]))
            {
                MessageBox.Show("You can not change signature page. You can only delete it...");
                return;
            }

            char upperKeyChar = char.ToUpper(e.KeyChar);

            var docTypeOpt = documentTypeListView.Store.GetDocumentTypeForShortcut(upperKeyChar);
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
                    UpdateState(eSTATES.PAGE_INDEXED, originalFileName, docType, indexFileName, policyLst.SelectedItem.ToString());
                }

                index = imageLst.SelectedIndex;
                imageLst.Items[index] = selImageName;
                imageLst.Refresh();
                if ((index + 1) != imageLst.Items.Count)
                {
                    if (sigBol == false)
                    {
                        imageLst.SelectedIndex = index + 1;
                        ShowIndexedImage();
                    }
                    if (sigBol == true)
                    {
                        if ((index + 2) != imageLst.Items.Count)
                        {
                            if ((imageLst.SelectedIndex + 1) != (imageLst.Items.Count))
                            {
                                imageLst.SelectedIndex = index + 2;
                                ShowIndexedImage();
                            }
                            else
                            {
                                if ((policyLst.SelectedIndex) != policyLst.Items.Count)
                                {
                                    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                }
                            }
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
                        if (imageLst.Items.Count == (imageLst.SelectedIndex + 1))
                        {
                            policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                        }

                    }
                }
            }
            DisplayDocTypeCount();
            imageLst.Refresh();
            DateTime end = DateTime.Now;
            TimeSpan duration = end - st;
        }

        [Category("Action")]
        [Description("Fires when the Image is changed.")]
        public event ImageChangeHandler ImageChanged;
        private void lstImage_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        }
        public event LstDelImageClick LstDelImgClick;
        private void lstImageDel_Click(object sender, EventArgs e)
        {
            string delFileName;

            delImgList = lstImageDel;
            if (lstImageDel.Items.Count > 0)
            {
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
            string scanFilePath;
            string indexPath;

            try
            {
                if (e.KeyCode == Keys.Insert)
                {
                    imageDelLst = lstImageDel;
                    policyLst = lstPolicy;
                    imageLst = lstImage;

                    pageDelInsrt = true;
                    if (imageDelLst.Items.Count > 0)
                    {
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageDelLst.SelectedItem.ToString(), string.Empty);
                        if (UpdateState(eSTATES.PAGE_QC, imageDelLst.SelectedItem.ToString(), policyLst.SelectedItem.ToString()) == true)
                        {

                            pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), imageDelLst.SelectedItem.ToString(), string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            string changedImageName = wImage.GetIndexedImageName();

                            delPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;

                            sourceFileName = sourceFilePath + "\\" + imageDelLst.SelectedItem.ToString();
                            delFileName = delPath + "\\" + imageDelLst.SelectedItem.ToString();
                            scanFilePath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + imageDelLst.SelectedItem.ToString();
                            if (changedImageName == string.Empty)
                            {
                                qcFilePath = indexFolderName + "\\" + imageDelLst.SelectedItem.ToString();
                            }
                            else
                            {
                                qcFilePath = indexFolderName + "\\" + changedImageName;
                            }
                            if (File.Exists(delFileName) == true)
                            {
                                File.Move(delFileName, qcFilePath);
                                File.Copy(qcFilePath, scanFilePath, true);
                            }

                            InsertNotify(imageLst.SelectedIndex);
                            EnableDisbleControls(true);
                            delImgList = lstImageDel;
                            if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                            {
                                delImgList.SetSelected(delImgList.SelectedIndex, false);
                            }
                        }
                    }
                    scannedImagesLabel.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

            if (LstDelIamgeInsert != null)
            {
                LstDelIamgeInsert(this, e);
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

        public bool MoveNext(bool prmMove)
        {
            if (prmMove == true)
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
            }
            return true;
        }
        private void cmdNext_Click(object sender, EventArgs e)
        {
            DialogResult rlst;
            DateTime stdt = DateTime.Now;
            imageLst = imageLst = lstImage;
            policyLst = lstPolicy;

            IndexingOperation = false;
            if (imageLst.Items.Count > 0)
            {
                EnableDisbleControls(true);
            }
            delImgList = lstImageDel;
            if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
            {
                delImgList.SetSelected(delImgList.SelectedIndex, false);
            }

            MoveNext(true);
            ShowImage(false);
            DateTime eddt = DateTime.Now;
            TimeSpan sp = eddt - stdt;

        }

        private void cmdPrevious_Click(object sender, EventArgs e)
        {
            imageLst = imageLst = lstImage;
            policyLst = lstPolicy;


            if ((indexCount > 0) && (lstImage.Items.Count > 0))
            {
                EnableDisbleControls(true);

                indexCount = indexCount - 1;
                lstImage.SelectedIndex = indexCount;

                delImgList = lstImageDel;
                if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                {
                    delImgList.SetSelected(delImgList.SelectedIndex, false);
                }
                ShowImage(false);
            }
            if ((indexCount == 0) && (lstImage.Items.Count > 0) && (lstPolicy.SelectedIndex != 0))
            {
                indexCount = 0;
                lstPolicy.SelectedIndex = lstPolicy.SelectedIndex - 1;
            }
        }
        public bool MoveUp()
        {
            ArrayList arr = new ArrayList();
            CtrlPolicy ctPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, lstPolicy.SelectedItem.ToString());
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
            CtrlPolicy ctPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, lstPolicy.SelectedItem.ToString());
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

        private int GetKeyVal(string key)
        {
            int cropKeyVal = 0;

            foreach (char c in key)
            {
                cropKeyVal = (int)c;
            }
            return cropKeyVal;
        }
        private void AeIndexing_KeyUp(object sender, KeyEventArgs e)
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

            if (!(this.ActiveControl is TextBox))
            {
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
                    MoveUp();
                }
                if (e.KeyCode == Keys.F12)
                {
                    MoveDown();
                }

                if (e.KeyCode == Keys.Right)
                {
                    IndexingOperation = false;

                    //MoveNext();
                    EnableDisbleControls(true);
                    ShowImage(false);
                    DisplayDocTypeCount();
                }
                if (e.KeyCode == Keys.Left)
                {
                    //MovePrevious();
                    EnableDisbleControls(true);
                    ShowImage(false);
                }
                if (e.KeyCode == Keys.Up)
                {
                    //MovePrevious();
                    EnableDisbleControls(true);
                    ShowImage(false);
                    ChangeSize();
                }
                if (e.KeyCode == Keys.Down)
                {
                    //MoveNext();
                    EnableDisbleControls(true);
                    ShowImage(false);
                    ChangeSize();
                    DisplayDocTypeCount();
                }

            }
        }

        public bool UpdateStatus(Credentials prmCrd)
        {
            eSTATES newState = eSTATES.BATCH_QC;
            return updateStatusLogic.UpdateStatus(ctrlPolicy.ProjectKey.ToString(), ctrlPolicy.BatchKey.ToString(), ctrlPolicy.PolicyNumber, prmCrd, new eSTATES[] { }, new eSTATES[] { eSTATES.POLICY_EXPORTED }, newState);
        }

        private void UpdateAllPolicyStatus()
        {
            try
            {
                var policies = policyLst.Items.Cast<object>().Select(item => item.ToString()).ToList();
                policies.ForEach(policy =>
                {
                    pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policy, string.Empty, string.Empty);
                    var wImage = new wfeImage(sqlCon, pImage);

                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policy);
                    var wPolicy = new wfePolicy(sqlCon, ctrlPolicy);

                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                    UpdateStatus(crd);
                    wPolicy.UpdateTransactionLog(eSTATES.POLICY_INDEXED, crd);
                });
                if (GetFileCount(projKey, bundleKey) == 0 && GetFileCountQC(projKey, bundleKey) == 0)
                {
                    UpdateBundleStatus(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error - " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Bundle-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
        }
        public bool UpdateBundleStatus(int prmProjKey, int prmBatchKey)
        {
            eSTATES state = eSTATES.BATCH_QC;
            return updateStatusLogic.UpdateBundleStatus(prmProjKey.ToString(), prmBatchKey.ToString(), new eSTATES[] { eSTATES.BATCH_SCANNED }, new eSTATES[] { eSTATES.BATCH_QC }, state);
        }
        public int GetFileCount(string projkey, string bundleKey)
        {
            return aeIndexingLogic.GetFileCount(projkey, bundleKey);
        }
        public int GetFileCountQC(string projkey, string bundleKey)
        {
            return aeIndexingLogic.GetFileCountQC(projkey, bundleKey);
        }
        public void RefreshNotify()
        {
            ShowPolicyDetails();
            if (lstPolicy.Items.Count > 0)
            {
                lstPolicy.SelectedIndex = 0;
            }
        }
        public bool CheckINDEXStatus(string policyList)
        {
            var filesInQC = aeIndexingLogic.CheckINDEXStatus(projKey, bundleKey, boxNumber, policyList).Tables[0].Rows.Count;
            bool checkImageIndexed = filesInQC <= 0;
            return checkImageIndexed;
        }
        private void AeIndexing_KeyDown(object sender, KeyEventArgs e)
        {
            DialogResult rlst;
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    frmHelp frm = new frmHelp("Index");
                    frm.ShowDialog(this);
                }
                if (e.KeyCode == Keys.F5)
                {
                    bool allFilesMarked = lstPolicy.Items.Cast<object>().All(item =>
                        CheckINDEXStatus(item.ToString())
                    );

                    if (!allFilesMarked)
                    {
                        MessageBox.Show(this, "Please mark document type for all images...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    UpdateAllPolicyStatus();

                    lstPolicy.Items.Clear();
                    imageLst.Items.Clear();
                    pendingLabelCount.Text = "Pending : 0";

                    if (GetFileCountQC(projKey, bundleKey) == 0)
                    {
                        DisableAllControls();
                        MessageBox.Show(this, "Indexing Completed for this Bundle ....", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    DisableAllControls();
                    return;
                }
                if ((e.KeyCode == Keys.Z) && (e.Control))
                {
                    ImageCopy();
                }
                ///For checking todays production count
                if ((e.KeyCode == Keys.F9))
                {
                    wfePolicy wPolicy = new wfePolicy(sqlCon);
                    int count = wPolicy.GetTransactionLogCount(bundleKey, dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_INDEXED);
                    frmProductionCount frmProd = new frmProductionCount(count);
                    frmProd.ShowDialog(this);
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
                char rslt;
                if (char.TryParse(e.KeyCode.ToString(), out rslt))
                {
                    if (Convert.ToChar(e.KeyCode.ToString().ToUpper()) == deleteKey[0])
                    {
                        ImageDelete();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DisableAllControls()
        {
            EnableDisbleControls(false);
            pictureControl.Image = null;
            scannedImagesLabel.Visible = false;
            prmButtonImportImage.Enabled = false;
        }
        int Crop(Rectangle udtRect)
        {

            Bitmap bmpImage = new Bitmap(pictureControl.Image);
            double htRatio = 0;
            double wdRatio = 0;
            DateTime st = DateTime.Now;
            DateTime end = DateTime.Now;
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
                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
                pageDelInsrt = false;
                end = DateTime.Now;
                TimeSpan duration = end - st;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error while cropping the image", "Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + projKey + " ,Batch-" + bundleKey + " ,Box-" + boxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
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
                if (img.IsValid() == true)
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
                }
                ChangeSize();

                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImgSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);

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
                    //string[] searchFileName = Directory.GetFiles(sourceFilePath, originalImage);
                    ////For the file in index folder
                    //if (searchFileName.Length >= 0)
                    //    sourcePath = searchFileName[0];
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
            ctrlImg = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), policyLst.SelectedItem.ToString(), prmIamgeName, string.Empty);

            img = new wfeImage(sqlCon, ctrlImg);
            img.Save(crd, eSTATES.PAGE_RESCANNED_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, prmSrlNo, prmIamgeName);

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            policy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
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
                    //Build string for supposed to be file in indexing folder
                    //sourcePath = sourceFilePath + "\\" + flName;
                    //Build string for file in FQC folder
                    desPath = indexFolderName + "\\" + flName;
                    //Build string for file in QC folder
                    scanPath = scanFolder + "\\" + flName;
                    //Build string for file in scan folder
                    //qcPath = qcFolder + "\\" + flName;
                    //ino = gdScanImg.CreateGdPictureImageFromDIB(prmHBmp);
                    pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber.ToString(), policyLst.SelectedItem.ToString());
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
                if (prmHBmp != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(prmHBmp);
                    prmHBmp = IntPtr.Zero;
                }
            }
        }

        private void PrmEndPhotoCrop_Click(object sender, EventArgs e)
        {
            EndPhotoCrop();
        }

        private void PrmGetPhoto_Click(object sender, EventArgs e)
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

        private void PictureControl_MouseClick_1(object sender, MouseEventArgs e)
        {
            Point pt = new Point();
            pt.X = e.X;
            pt.Y = e.Y;
            if (e.Button == MouseButtons.Right)
            {
                policyLst = lstPolicy;
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString());
                policy = new wfePolicy(sqlCon, ctrlPolicy);


            }
        }

        private void PictureControl_MouseDown_1(object sender, MouseEventArgs e)
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

        private void PictureControl_MouseMove_1(object sender, MouseEventArgs e)
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
                Cursor = Cursors.Cross;
                if ((pictureControl.Image != null))
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
            if (e.Button == MouseButtons.Right)
            {
                Cursor = Cursors.Default;
                OperationInProgress = ihConstants._NO_OPERATION;
            }
        }

        private void PictureControl_MouseUp_1(object sender, MouseEventArgs e)
        {

            cropWidth = Math.Abs(e.X - cropX);
            cropHeight = Math.Abs(e.Y - cropY);

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

        private void AeIndexing_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private string GetPolicyPath(string policyNo)
        {
            policyLst = lstPolicy;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = GetPath(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey));
            return batchPath + "\\" + policyNo;
        }
        public int GetPolicyStatus()
        {
            int projKey = ctrlPolicy.ProjectKey;
            int batchKey = ctrlPolicy.BatchKey;
            string policyNumber = ctrlPolicy.PolicyNumber;
            return aeIndexingLogic.GetPolicyStatus(projKey, batchKey, policyNumber);
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
                imageName = imageLst.SelectedItem.ToString();
                ///get the file name to copy
                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "B'Zer - TIF Files";
                fileDlg.ShowDialog();
                int countfromFiledlg = fileDlg.FileNames.Length;
                for (int i = 0; i < countfromFiledlg; i++)
                {
                    imageName = fileDlg.FileNames[i].ToString();
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
                        if ((GetPolicyStatus() == 3) || (GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED) || (GetPolicyStatus() == (int)eSTATES.POLICY_EXPORTED))
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
                        System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                        fileSize = info.Length;
                        fileSize = fileSize / 1024;
                        crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                        pImage = new CtrlImage(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber, policyLst.SelectedItem.ToString(), newImageName, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
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
                MessageBox.Show("Image has been imported successfully.....");


                scannedImagesLabel.Text = "Total Scanned Image : " + lstImage.Items.Count.ToString();
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
        public bool UpdateStatus(eSTATES state, Credentials prmCrd)
        {

            return updateStatusLogic.UpdateStatus(ctrlPolicy.ProjectKey.ToString(), ctrlPolicy.BatchKey.ToString(), ctrlPolicy.PolicyNumber, prmCrd, new eSTATES[] { }, new eSTATES[] { eSTATES.POLICY_EXPORTED }, state);
        }
        private void PrmButtonImportImage_Click(object sender, EventArgs e)
        {
            ImportImageFromDir();
        }

        private void LstImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (lstImage.AllowDrop == true)
            {
                if (this.lstImage.SelectedItem == null) return;
                this.lstImage.DoDragDrop(this.lstImage.SelectedItem, DragDropEffects.Move);
            }
        }

        private void LstImage_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void LstImage_DragDrop(object sender, DragEventArgs e)
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

        private void LstImage_MouseClick(object sender, MouseEventArgs e)
        {
            EnableDisbleControls(true);
            ShowImage(false);
            if (LstImgClick != null)
            {
                LstImgClick(this, e);
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
