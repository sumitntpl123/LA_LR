using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ImageHeaven.Logic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NovaNet.Utils;
using NovaNet.wfe;

namespace ImageHeaven
{
    public partial class frmBundleUpload : Form
    {

        Credentials crd = new Credentials();

        NovaNet.Utils.dbCon dbcon;
        OdbcConnection sqlCon = null;
        private OdbcDataAdapter sqlAdap = null;
        eSTATES[] state;
        DataSet dsdeed = null;
        public static string projKey;
        public static string bundleKey;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);

        iTextSharp.text.Image i2;
        System.Drawing.Image image3;
        int j;
        Paragraph para;
        Paragraph para1;
        Paragraph para2;
        int flag = 0;
        int flag1 = 0;
        int flag2 = 0;
        List<string> fileNames = new List<string>();
        frmBundleUploadLogic FrmBundleUploadLogic = null;
        UpdateStatusLogic updateStatusLogic = null;

        public Bitmap ConvertTextToImage(string txt, string fontname, int fontsize, Color bgcolor, Color fcolor, int width, int Height)
        {
            Bitmap bmp = new Bitmap(width, Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {

                System.Drawing.Font font = new System.Drawing.Font(fontname, fontsize);
                graphics.FillRectangle(new SolidBrush(bgcolor), 0, 0, bmp.Width, bmp.Height);
                graphics.DrawString(txt, font, new SolidBrush(fcolor), 0, 0);
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();


            }
            return bmp;
        }

        public frmBundleUpload(OdbcConnection prmCon, Credentials prmCrd)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            sqlCon = prmCon;
            crd = prmCrd;
            this.Text = "B'Zer -  - Bundle Upload";

            FrmBundleUploadLogic = new frmBundleUploadLogic(prmCon);
            updateStatusLogic = new UpdateStatusLogic(prmCon);

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private void FrmBundleUpload_Load(object sender, EventArgs e)
        {
            PopulateProject();
        }

        private void PopulateProject()
        {

            DataTable dt = new DataTable();

            dt = FrmBundleUploadLogic.PopulateProject();

            if (dt.Rows.Count > 0)
            {
                cmbProject.DataSource = dt;
                cmbProject.DisplayMember = "proj_code";
                cmbProject.ValueMember = "proj_key";

                PopulateBundle();
            }
            else
            {
                cmbProject.DataSource = null;
                MessageBox.Show("Add one project first...");
                this.Close();
            }


        }

        private void PopulateBundle()
        {
            DataTable dt = new DataTable();
            string cmdProjValue = cmbProject.SelectedValue.ToString();
            dt = FrmBundleUploadLogic.PopulateBundle(cmdProjValue);

            if (dt.Rows.Count > 0)
            {
                cmbBundle.DataSource = dt;
                cmbBundle.DisplayMember = "bundle_code";
                cmbBundle.ValueMember = "bundle_key";
            }
            else
            {
                cmbBundle.Text = string.Empty;
                cmbBundle.DataSource = null;
                cmbBundle.DisplayMember = "";
                cmbBundle.ValueMember = "";
                cmbProject.Select();
            }
        }

        private DataSet ReadDatabase()
        {
            DataSet ds = new DataSet();
            dsdeed = new DataSet();
            try
            {
                string cmdProjValue = cmbProject.SelectedValue.ToString();
                string cmdBundleValue = cmbBundle.SelectedValue.ToString();
                ds = FrmBundleUploadLogic.ReadDatabase(cmdProjValue, cmdBundleValue, dsdeed);

            }
            catch (Exception ex)
            {
                statusStrip1.Text = ex.Message.ToString();
            }
            return ds;
        }
        public string GetBundleName(int prmProjectKey, int prmBundleKey)
        {
            string projName = null;
            DataSet projDs = new DataSet();
            projDs = FrmBundleUploadLogic.GetBundleName(prmProjectKey, prmBundleKey);

            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["bundle_code"].ToString();
            }
            else
                projName = string.Empty;
            return projName;
        }
        private void Cmdsearch_Click(object sender, EventArgs e)
        {
            if (cmbProject.Text != "" && cmbBundle.Text != "")
            {
                grdCsv.DataSource = null;
                DataTable dt = ReadDatabase().Tables[0];
                grdCsv.DataSource = dt;
                if (grdCsv.Rows.Count > 0)
                {
                    cmdExport.Enabled = true;
                    for (int i = 0; i < grdCsv.Rows.Count; i++)
                    {
                        fileNames.Add(dt.Rows[i]["filename"].ToString());
                    }
                }
                else
                {
                    cmdExport.Enabled = false;
                }
            }
        }

        private bool InsertIntoDB(string pk, string bk, string bn)
        {
            return FrmBundleUploadLogic.InsertIntoDB(pk, bk, bn, crd);
        }

        public DataTable GetFileDetails(string proj, string bundle, string fileName)
        {
            return FrmBundleUploadLogic.GetFileDetails(proj, bundle, fileName);
        }

        public DataTable PendingBundle()
        {
            return FrmBundleUploadLogic.PendingBundle();
        }


        private void CmbProject_Leave(object sender, EventArgs e)
        {
            PopulateBundle();
        }

        public DataTable _GetGenCount(string proj, string bundle)
        {
            return FrmBundleUploadLogic._GetGenCount(proj, bundle);
        }

        private void CmdExport_Click(object sender, EventArgs e)
        {
            cmdExport.Enabled = false;
            DialogResult dlg;
            if ((cmbProject.Text == "" || cmbProject.Text == null) || (cmbBundle.Text == "" || cmbBundle.Text == null))
            {
                MessageBox.Show("Please select proper Project or Bundle...");
                cmbProject.Focus();
                cmbProject.Select();
            }
            else
            {

                projKey = cmbProject.SelectedValue.ToString();

                bundleKey = cmbBundle.SelectedValue.ToString();

                if (projKey != "" || projKey != null || bundleKey != "" || bundleKey != null)
                {
                    statusStrip1.Items.Add("Status: Wait While Uploading the Database......");
                    bool updatebundle = UpdateBundle();
                    bool updatecasefile = UpdateCaseFile();

                    if (updatebundle == true && updatecasefile == true)
                    {
                        dlg = MessageBox.Show(this, "Are you sure, you want to generate barcode for this bundle?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlg == DialogResult.Yes)
                        {

                            if (_GetGenCount(projKey, bundleKey).Rows.Count > 0)
                            {
                                cmdExport.Enabled = false;

                                string expFolder = "C:\\";
                                bool isDeleted = false;
                                //check if folder is exists or not
                                if (Directory.Exists(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey))) && isDeleted == false)
                                {
                                    Directory.Delete(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)), true);
                                }

                                if (fileNames.Count > 0)
                                {
                                    Application.DoEvents();
                                    //bundle folder creation
                                    if (!Directory.Exists(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey))))
                                    {
                                        Directory.CreateDirectory(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)));
                                    }
                                    String[] imageList = new String[fileNames.Count];
                                    //checking each filename
                                    for (int x = 0; x < fileNames.Count; x++)
                                    {
                                        string filename = fileNames[x].ToString();

                                        string str1 = "HIGH COURT OF CALCUTTA";
                                        string casetype = GetFileDetails(projKey, bundleKey, filename).Rows[0][0].ToString();
                                        string caseno = GetFileDetails(projKey, bundleKey, filename).Rows[0][1].ToString();
                                        string caseyear = GetFileDetails(projKey, bundleKey, filename).Rows[0][2].ToString();
                                        string est = GetFileDetails(projKey, bundleKey, filename).Rows[0][3].ToString();
                                        string entity = null;
                                        if (est == "Appellate")
                                        {
                                            entity = "AS";
                                        }
                                        else
                                        {
                                            entity = "OS";
                                        }
                                        string str2 = "File No: " + casetype + "/" + caseno + "/" + caseyear + "/" + entity;
                                        string str3 = casetype + "/" + caseno + "/" + caseyear + "/" + entity;
                                        using (Graphics e1 = this.CreateGraphics())
                                        {
                                            System.Drawing.Font stringFont = new System.Drawing.Font("calibri", 9);
                                            SizeF stringSize2 = new SizeF();
                                            SizeF stringSize1 = new SizeF();
                                            stringSize2 = e1.MeasureString(str2, stringFont);
                                            stringSize1 = e1.MeasureString(str1, stringFont);
                                            System.Drawing.Image img1 = ConvertTextToImage(str2, "calibri", 9, Color.Transparent, Color.Black, Convert.ToInt32(Math.Round(stringSize2.Width)), Convert.ToInt32(Math.Round(stringSize2.Height)));
                                            System.Drawing.Image img4 = ConvertTextToImage(str1, "calibri", 9, Color.Transparent, Color.Black, Convert.ToInt32(Math.Round(stringSize1.Width)), Convert.ToInt32(Math.Round(stringSize1.Height + 5)));
                                            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                                            System.Drawing.Image img2 = barcode.Draw(str3, 50);

                                            int height = img1.Height + img2.Height + img4.Height;
                                            int width = Math.Max(img1.Width, img2.Width);
                                            Bitmap img3 = new Bitmap(width, height);

                                            Bitmap image1 = new Bitmap(img1);
                                            Bitmap image2 = new Bitmap(img2);
                                            Bitmap image4 = new Bitmap(img4);
                                            int X = image2.Width;

                                            using (Graphics g = Graphics.FromImage(img3))
                                            {


                                                g.DrawImage(image4, (image2.Width - image4.Width) / 2, 0);
                                                g.DrawImage(image2, 0, image4.Height);
                                                g.DrawImage(image1, (image2.Width - image1.Width) / 2, image2.Height + image4.Height);
                                            }

                                            Bitmap bitmap = new Bitmap(img3);

                                            image3 = (System.Drawing.Image)bitmap;
                                            image3.Save(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + "\\" + x + ".tiff", System.Drawing.Imaging.ImageFormat.Tiff);

                                            j = x;
                                            string imageURL = expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + "\\" + j + ".tiff";
                                            imageList[x] = imageURL;
                                        }

                                    }
                                    if (fileNames.Count > 36)
                                    {

                                        sfdUAT.Filter = "Pdf files (*.pdf)|*.pdf";
                                        sfdUAT.FilterIndex = 2;
                                        sfdUAT.RestoreDirectory = true;
                                        sfdUAT.FileName = cmbBundle.Text;
                                        sfdUAT.ShowDialog();

                                        FileStream fs = new FileStream(sfdUAT.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                                        Document doc = new Document();
                                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                                        doc.SetPageSize(iTextSharp.text.PageSize.A4);
                                        PdfPTable table = new PdfPTable(7);
                                        table.TotalWidth = 500f;
                                        table.LockedWidth = true;
                                        float[] widths = new float[] { 4f, 1.66f, 4f, 1.66f, 4f, 1.66f, 4f };

                                        table.SetWidths(widths);
                                        table.HorizontalAlignment = 0;

                                        table.SpacingBefore = 10f;
                                        table.SpacingAfter = 5f;
                                        PdfPTable table1 = new PdfPTable(7);
                                        table1.TotalWidth = 500f;
                                        table1.LockedWidth = true;
                                        float[] widths1 = new float[] { 4f, 1.66f, 4f, 1.66f, 4f, 1.66f, 4f };

                                        table1.SetWidths(widths1);
                                        table1.HorizontalAlignment = 0;

                                        table1.SpacingBefore = 10f;
                                        table1.SpacingAfter = 10f;
                                        doc.Open();
                                        BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                        iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL);
                                        para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + fileNames.Count + " ,Generated : " + _GetGenCount(projKey, bundleKey).Rows.Count + 1 + " times", font);

                                        para.Alignment = Element.ALIGN_RIGHT;


                                        if (fileNames.Count % 4 == 0)
                                        {
                                            int k = 0;
                                            for (int i = 0; i < fileNames.Count / 4; i++)
                                            {
                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[i]);
                                                jpg1.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;


                                                PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);


                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[i]);
                                                jpg2.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;


                                                PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                empty8.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty8);


                                                iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[i]);
                                                jpg3.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar2 = new PdfPCell(jpg3);
                                                bar2.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar2);
                                                k++;


                                                PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                empty9.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty9);

                                                iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[i]);
                                                jpg4.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar3 = new PdfPCell(jpg4);
                                                bar3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar3);
                                                k++;

                                                PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                empty14.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty14);

                                                PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                empty15.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty15);

                                                PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                empty16.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty16);


                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);


                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);

                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);

                                                PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                empty10.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty10);


                                                PdfPCell empty11 = new PdfPCell(new Phrase("  "));
                                                empty11.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty11);

                                                PdfPCell empty12 = new PdfPCell(new Phrase("  "));
                                                empty12.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty12);

                                                PdfPCell empty13 = new PdfPCell(new Phrase("  "));
                                                empty13.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty13);

                                                PdfPCell empty21 = new PdfPCell(new Phrase("  "));
                                                empty21.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty21);

                                                PdfPCell empty22 = new PdfPCell(new Phrase("  "));
                                                empty22.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty22);

                                                PdfPCell empty23 = new PdfPCell(new Phrase("  "));
                                                empty23.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty23);
                                            }
                                        }
                                        else if (fileNames.Count % 4 == 1)
                                        {
                                            int k = 0;
                                            int i;
                                            for (i = 0; i < fileNames.Count / 4; i++)
                                            {
                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg1.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;


                                                PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);


                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg2.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;


                                                PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                empty8.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty8);


                                                iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg3.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar2 = new PdfPCell(jpg3);
                                                bar2.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar2);
                                                k++;


                                                PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                empty9.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty9);

                                                iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg4.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar3 = new PdfPCell(jpg4);
                                                bar3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar3);
                                                k++;

                                                PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                empty14.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty14);

                                                PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                empty15.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty15);

                                                PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                empty16.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty16);


                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);


                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);

                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);

                                                PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                empty10.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty10);


                                            }
                                            iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg5.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar4 = new PdfPCell(jpg5);
                                            bar4.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar4);

                                            PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                            empty31.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty31);

                                            PdfPCell empty32 = new PdfPCell(new Phrase("  "));
                                            empty32.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty32);

                                            PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                            empty33.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty33);

                                            PdfPCell empty34 = new PdfPCell(new Phrase("  "));
                                            empty34.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty34);

                                            PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                            empty35.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty35);

                                            PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                            empty36.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty36);
                                        }
                                        else if (fileNames.Count % 4 == 2)
                                        {
                                            int k = 0;
                                            for (int i = 0; i < fileNames.Count / 4; i++)
                                            {
                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg1.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;


                                                PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);


                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg2.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;


                                                PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                empty8.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty8);


                                                iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg3.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar2 = new PdfPCell(jpg3);
                                                bar2.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar2);
                                                k++;


                                                PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                empty9.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty9);

                                                iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg4.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar3 = new PdfPCell(jpg4);
                                                bar3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar3);
                                                k++;

                                                PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                empty14.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty14);

                                                PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                empty15.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty15);

                                                PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                empty16.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty16);


                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);


                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);

                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);

                                                PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                empty10.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty10);



                                            }
                                            iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg5.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar4 = new PdfPCell(jpg5);
                                            bar4.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar4);

                                            k++;

                                            PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                            empty31.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty31);

                                            iTextSharp.text.Image jpg6 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg6.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar6 = new PdfPCell(jpg6);
                                            bar6.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar6);

                                            PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                            empty33.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty33);

                                            PdfPCell empty34 = new PdfPCell(new Phrase("  "));
                                            empty34.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty34);

                                            PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                            empty35.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty35);

                                            PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                            empty36.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty36);
                                        }
                                        else if (fileNames.Count % 4 == 3)
                                        {

                                            int k = 0;
                                            int i;
                                            for (i = 0; i < fileNames.Count / 4; i++)
                                            {

                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg1.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;


                                                PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);


                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg2.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;


                                                PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                empty8.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty8);


                                                iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg3.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar2 = new PdfPCell(jpg3);
                                                bar2.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar2);

                                                k++;


                                                PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                empty9.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty9);

                                                iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg4.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar3 = new PdfPCell(jpg4);
                                                bar3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar3);

                                                k++;

                                                PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                empty14.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty14);

                                                PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                empty15.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty15);

                                                PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                empty16.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty16);


                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);


                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);

                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);

                                                PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                empty10.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty10);



                                            }
                                            iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg5.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar4 = new PdfPCell(jpg5);
                                            bar4.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar4);

                                            k++;

                                            PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                            empty31.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty31);

                                            iTextSharp.text.Image jpg99 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg99.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar99 = new PdfPCell(jpg99);
                                            bar99.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar99);

                                            k++;

                                            PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                            empty33.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty33);

                                            iTextSharp.text.Image jpg54 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg54.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar54 = new PdfPCell(jpg54);
                                            bar54.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar54);

                                            PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                            empty35.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty35);

                                            PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                            empty36.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty36);
                                        }
                                        else if (fileNames.Count == 1)
                                        {
                                            int k = 0;
                                            iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            PdfPCell bar = new PdfPCell(jpg1);
                                            bar.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar);

                                            PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                            empty3.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty3);
                                            PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                            empty4.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty4);

                                        }
                                        else
                                        {
                                            int k = 0;
                                            for (int i = 0; i < (fileNames.Count) / 2; i++)
                                            {




                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg1.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;

                                                PdfPCell empty = new PdfPCell();
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);
                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;

                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);
                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);
                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);





                                            }
                                            iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            PdfPCell bar3 = new PdfPCell(jpg3);
                                            bar3.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar3);
                                            PdfPCell empty1 = new PdfPCell();
                                            empty1.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty1);
                                            PdfPCell empty2 = new PdfPCell();
                                            empty2.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty2);
                                        }

                                        doc.Add(para);
                                        if (fileNames.Count > 32 && fileNames.Count <= 36)
                                        {
                                            doc.Add(new Paragraph("\n"));

                                            table.SpacingAfter = 70f;
                                        }
                                        doc.Add(table);


                                        para1 = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + fileNames.Count + " ,Generated : " + Convert.ToString(Convert.ToInt32(_GetGenCount(projKey, bundleKey).Rows.Count) + 1) + " times", font);

                                        para1.Alignment = Element.ALIGN_RIGHT;
                                        doc.Add(para1);

                                        doc.Add(table1);
                                        doc.Close();


                                        Directory.Delete(expFolder + "\\Barcode", true);
                                        bool insertlog = InsertIntoDB(projKey, bundleKey, cmbBundle.Text);
                                        if (insertlog == true)
                                        {

                                            MessageBox.Show(this, "Barcode Generated Successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            cmdExport.Enabled = true;

                                            int pendBundle = PendingBundle().Rows.Count;


                                        }


                                    }
                                    else
                                    {
                                        if ((fileNames.Count) > 0)
                                        {
                                            sfdUAT.Filter = "Pdf files (*.pdf)|*.pdf";
                                            sfdUAT.FilterIndex = 2;
                                            sfdUAT.RestoreDirectory = true;
                                            sfdUAT.FileName = cmbBundle.Text;
                                            sfdUAT.ShowDialog();
                                            try
                                            {
                                                FileStream fs = new FileStream(sfdUAT.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                                                Document doc = new Document();
                                                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                                                doc.SetPageSize(iTextSharp.text.PageSize.A4);
                                                PdfPTable table = new PdfPTable(7);
                                                table.TotalWidth = 500f;
                                                table.LockedWidth = true;
                                                float[] widths = new float[] { 4f, 1.66f, 4f, 1.66f, 4f, 1.66f, 4f };

                                                table.SetWidths(widths);
                                                table.HorizontalAlignment = 0;

                                                table.SpacingBefore = 10f;
                                                table.SpacingAfter = 5f;
                                                PdfPTable table1 = new PdfPTable(7);
                                                table1.TotalWidth = 500f;
                                                table1.LockedWidth = true;
                                                float[] widths1 = new float[] { 4f, 1.66f, 4f, 1.66f, 4f, 1.66f, 4f };

                                                table1.SetWidths(widths1);
                                                table1.HorizontalAlignment = 0;

                                                table1.SpacingBefore = 5f;
                                                table1.SpacingAfter = 10f;
                                                doc.Open();




                                                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                                                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL);
                                                para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + fileNames.Count + " ,Generated : " + Convert.ToString(Convert.ToInt32(_GetGenCount(projKey, bundleKey).Rows.Count) + 1) + " times", font);
                                                para.Alignment = Element.ALIGN_RIGHT;


                                                if (fileNames.Count % 4 == 0)
                                                {
                                                    int k = 0;
                                                    for (int i = 0; i < fileNames.Count / 4; i++)
                                                    {
                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;


                                                        PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);


                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg2.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;


                                                        PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                        empty8.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty8);


                                                        iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg3.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar2 = new PdfPCell(jpg3);
                                                        bar2.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar2);

                                                        k++;


                                                        PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                        empty9.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty9);

                                                        iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg4.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar3 = new PdfPCell(jpg4);
                                                        bar3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar3);

                                                        k++;

                                                        PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                        empty14.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty14);

                                                        PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                        empty15.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty15);

                                                        PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                        empty16.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty16);


                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);


                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);

                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);

                                                        PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                        empty10.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty10);


                                                        PdfPCell empty11 = new PdfPCell(new Phrase("  "));
                                                        empty11.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty11);

                                                        PdfPCell empty12 = new PdfPCell(new Phrase("  "));
                                                        empty12.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty12);

                                                        PdfPCell empty13 = new PdfPCell(new Phrase("  "));
                                                        empty13.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty13);

                                                        PdfPCell empty21 = new PdfPCell(new Phrase("  "));
                                                        empty21.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty21);

                                                        PdfPCell empty22 = new PdfPCell(new Phrase("  "));
                                                        empty22.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty22);

                                                        PdfPCell empty23 = new PdfPCell(new Phrase("  "));
                                                        empty23.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty23);
                                                    }
                                                }
                                                else if (fileNames.Count % 4 == 1)
                                                {
                                                    int k = 0;
                                                    int i;
                                                    for (i = 0; i < fileNames.Count / 4; i++)
                                                    {
                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;


                                                        PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);


                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg2.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;


                                                        PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                        empty8.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty8);


                                                        iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg3.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar2 = new PdfPCell(jpg3);
                                                        bar2.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar2);

                                                        k++;


                                                        PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                        empty9.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty9);

                                                        iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg4.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar3 = new PdfPCell(jpg4);
                                                        bar3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar3);

                                                        k++;

                                                        PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                        empty14.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty14);

                                                        PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                        empty15.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty15);

                                                        PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                        empty16.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty16);


                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);


                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);

                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);

                                                        PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                        empty10.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty10);


                                                    }
                                                    iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg5.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar4 = new PdfPCell(jpg5);
                                                    bar4.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar4);

                                                    PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                                    empty31.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty31);

                                                    PdfPCell empty32 = new PdfPCell(new Phrase("  "));
                                                    empty32.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty32);

                                                    PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                                    empty33.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty33);

                                                    PdfPCell empty34 = new PdfPCell(new Phrase("  "));
                                                    empty34.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty34);

                                                    PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                                    empty35.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty35);

                                                    PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                                    empty36.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty36);
                                                }
                                                else if (fileNames.Count % 4 == 2)
                                                {
                                                    int k = 0;
                                                    for (int i = 0; i < fileNames.Count / 4; i++)
                                                    {
                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;


                                                        PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);


                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg2.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;


                                                        PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                        empty8.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty8);


                                                        iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg3.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar2 = new PdfPCell(jpg3);
                                                        bar2.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar2);

                                                        k++;


                                                        PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                        empty9.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty9);

                                                        iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg4.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar3 = new PdfPCell(jpg4);
                                                        bar3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar3);

                                                        k++;

                                                        PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                        empty14.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty14);

                                                        PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                        empty15.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty15);

                                                        PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                        empty16.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty16);


                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);


                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);

                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);

                                                        PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                        empty10.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty10);


                                                    }
                                                    iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg5.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar4 = new PdfPCell(jpg5);
                                                    bar4.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar4);

                                                    k++;

                                                    PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                                    empty31.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty31);

                                                    iTextSharp.text.Image jpg6 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg6.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar6 = new PdfPCell(jpg6);
                                                    bar6.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar6);

                                                    PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                                    empty33.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty33);

                                                    PdfPCell empty34 = new PdfPCell(new Phrase("  "));
                                                    empty34.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty34);

                                                    PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                                    empty35.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty35);

                                                    PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                                    empty36.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty36);
                                                }
                                                else if (fileNames.Count % 4 == 3)
                                                {

                                                    int k = 0;
                                                    int i;
                                                    for (i = 0; i < fileNames.Count / 4; i++)
                                                    {

                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;


                                                        PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);


                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg2.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;


                                                        PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                        empty8.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty8);


                                                        iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg3.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar2 = new PdfPCell(jpg3);
                                                        bar2.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar2);

                                                        k++;


                                                        PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                        empty9.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty9);

                                                        iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg4.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar3 = new PdfPCell(jpg4);
                                                        bar3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar3);

                                                        k++;

                                                        PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                        empty14.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty14);

                                                        PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                        empty15.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty15);

                                                        PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                        empty16.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty16);


                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);


                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);

                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);

                                                        PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                        empty10.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty10);

                                                    }
                                                    iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg5.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar4 = new PdfPCell(jpg5);
                                                    bar4.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar4);

                                                    k++;

                                                    PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                                    empty31.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty31);

                                                    iTextSharp.text.Image jpg99 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg99.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar99 = new PdfPCell(jpg99);
                                                    bar99.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar99);

                                                    k++;

                                                    PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                                    empty33.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty33);

                                                    iTextSharp.text.Image jpg54 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg54.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar54 = new PdfPCell(jpg54);
                                                    bar54.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar54);


                                                    PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                                    empty35.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty35);

                                                    PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                                    empty36.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty36);
                                                }

                                                else if (fileNames.Count == 1)
                                                {
                                                    int k = 0;
                                                    iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    PdfPCell bar = new PdfPCell(jpg1);
                                                    bar.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar);

                                                    PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                    empty3.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty3);
                                                    PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                    empty4.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty4);

                                                }
                                                else
                                                {
                                                    int k = 0;
                                                    for (int i = 0; i < (fileNames.Count) / 2; i++)
                                                    {

                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;

                                                        PdfPCell empty = new PdfPCell();
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);
                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;

                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);
                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);
                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);





                                                    }
                                                    iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    PdfPCell bar3 = new PdfPCell(jpg3);
                                                    bar3.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar3);
                                                    PdfPCell empty1 = new PdfPCell();
                                                    empty1.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty1);
                                                    PdfPCell empty2 = new PdfPCell();
                                                    empty2.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty2);
                                                }

                                                doc.Add(para);
                                                doc.Add(table);


                                                doc.Add(table1);
                                                doc.Close();


                                                Directory.Delete(expFolder + "\\Barcode", true);
                                                bool insertlog = InsertIntoDB(projKey, bundleKey, cmbBundle.Text);
                                                if (insertlog == true)
                                                {
                                                    MessageBox.Show(this, "Barcode Generated Successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    cmdExport.Enabled = true;

                                                    int pendBundle = PendingBundle().Rows.Count;


                                                }
                                            }
                                            catch (Exception ex)
                                            { }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                cmdExport.Enabled = false;

                                string expFolder = "C:\\";
                                bool isDeleted = false;
                                //check if folder is exists or not
                                if (Directory.Exists(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey))) && isDeleted == false)
                                {
                                    Directory.Delete(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)), true);
                                }

                                if (fileNames.Count > 0)
                                {
                                    Application.DoEvents();
                                    //bundle folder creation
                                    if (!Directory.Exists(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey))))
                                    {
                                        Directory.CreateDirectory(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)));
                                    }
                                    String[] imageList = new String[fileNames.Count];
                                    //checking each filename
                                    for (int x = 0; x < fileNames.Count; x++)
                                    {
                                        string filename = fileNames[x].ToString();

                                        string str1 = "HIGH COURT OF CALCUTTA";
                                        string casetype = GetFileDetails(projKey, bundleKey, filename).Rows[0][0].ToString();
                                        string caseno = GetFileDetails(projKey, bundleKey, filename).Rows[0][1].ToString();
                                        string caseyear = GetFileDetails(projKey, bundleKey, filename).Rows[0][2].ToString();
                                        string est = GetFileDetails(projKey, bundleKey, filename).Rows[0][3].ToString();
                                        string entity = null;
                                        if (est == "Appellate")
                                        {
                                            entity = "AS";
                                        }
                                        else
                                        {
                                            entity = "OS";
                                        }
                                        string str2 = "File No: " + casetype + "/" + caseno + "/" + caseyear + "/" + entity;
                                        string str3 = casetype + "/" + caseno + "/" + caseyear + "/" + entity;
                                        using (Graphics e1 = this.CreateGraphics())
                                        {
                                            System.Drawing.Font stringFont = new System.Drawing.Font("calibri", 9);
                                            SizeF stringSize2 = new SizeF();
                                            SizeF stringSize1 = new SizeF();
                                            stringSize2 = e1.MeasureString(str2, stringFont);
                                            stringSize1 = e1.MeasureString(str1, stringFont);
                                            System.Drawing.Image img1 = ConvertTextToImage(str2, "calibri", 9, Color.Transparent, Color.Black, Convert.ToInt32(Math.Round(stringSize2.Width)), Convert.ToInt32(Math.Round(stringSize2.Height)));
                                            //Image img4 = ConvertTextToImage("Nevaeh Technology", "calibri", 10, Color.Transparent, Color.Black, textBox1.Width, textBox1.Height);
                                            System.Drawing.Image img4 = ConvertTextToImage(str1, "calibri", 9, Color.Transparent, Color.Black, Convert.ToInt32(Math.Round(stringSize1.Width)), Convert.ToInt32(Math.Round(stringSize1.Height + 5)));
                                            //Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                                            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                                            //pictureBox1.Image = barcode.Draw(dt1.Rows[i][0].ToString(), 60);
                                            System.Drawing.Image img2 = barcode.Draw(str3, 50);

                                            int height = img1.Height + img2.Height + img4.Height;
                                            int width = Math.Max(img1.Width, img2.Width);
                                            Bitmap img3 = new Bitmap(width, height);

                                            Bitmap image1 = new Bitmap(img1);
                                            Bitmap image2 = new Bitmap(img2);
                                            Bitmap image4 = new Bitmap(img4);
                                            int X = image2.Width;

                                            using (Graphics g = Graphics.FromImage(img3))
                                            {


                                                g.DrawImage(image4, (image2.Width - image4.Width) / 2, 0);
                                                g.DrawImage(image2, 0, image4.Height);
                                                g.DrawImage(image1, (image2.Width - image1.Width) / 2, image2.Height + image4.Height);
                                            }

                                            Bitmap bitmap = new Bitmap(img3);

                                            image3 = (System.Drawing.Image)bitmap;
                                            //image3.Save(@"D\image5.tiff", System.Drawing.Imaging.ImageFormat.Tiff);
                                            image3.Save(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + "\\" + x + ".tiff", System.Drawing.Imaging.ImageFormat.Tiff);

                                            j = x;
                                            string imageURL = expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + "\\" + j + ".tiff";
                                            imageList[x] = imageURL;
                                        }
                                    }
                                    if (fileNames.Count > 36)
                                    {

                                        sfdUAT.Filter = "Pdf files (*.pdf)|*.pdf";
                                        sfdUAT.FilterIndex = 2;
                                        sfdUAT.RestoreDirectory = true;
                                        sfdUAT.FileName = cmbBundle.Text;
                                        sfdUAT.ShowDialog();

                                        FileStream fs = new FileStream(sfdUAT.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                                        Document doc = new Document();
                                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                                        doc.SetPageSize(iTextSharp.text.PageSize.A4);
                                        PdfPTable table = new PdfPTable(7);
                                        table.TotalWidth = 500f;
                                        table.LockedWidth = true;
                                        float[] widths = new float[] { 4f, 1.66f, 4f, 1.66f, 4f, 1.66f, 4f };

                                        table.SetWidths(widths);
                                        table.HorizontalAlignment = 0;

                                        table.SpacingBefore = 10f;
                                        table.SpacingAfter = 5f;
                                        PdfPTable table1 = new PdfPTable(7);
                                        table1.TotalWidth = 500f;
                                        table1.LockedWidth = true;
                                        float[] widths1 = new float[] { 4f, 1.66f, 4f, 1.66f, 4f, 1.66f, 4f };

                                        table1.SetWidths(widths1);
                                        table1.HorizontalAlignment = 0;

                                        table1.SpacingBefore = 10f;
                                        table1.SpacingAfter = 10f;
                                        doc.Open();
                                        BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                        iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL);

                                        para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + fileNames.Count, font);

                                        para.Alignment = Element.ALIGN_RIGHT;


                                        if (fileNames.Count % 4 == 0)
                                        {
                                            int k = 0;
                                            for (int i = 0; i < fileNames.Count / 4; i++)
                                            {
                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[i]);
                                                jpg1.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;


                                                PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);


                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[i]);
                                                jpg2.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;


                                                PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                empty8.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty8);


                                                iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[i]);
                                                jpg3.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar2 = new PdfPCell(jpg3);
                                                bar2.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar2);
                                                k++;


                                                PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                empty9.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty9);

                                                iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[i]);
                                                jpg4.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar3 = new PdfPCell(jpg4);
                                                bar3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar3);
                                                k++;

                                                PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                empty14.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty14);

                                                PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                empty15.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty15);

                                                PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                empty16.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty16);


                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);


                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);

                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);

                                                PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                empty10.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty10);


                                                PdfPCell empty11 = new PdfPCell(new Phrase("  "));
                                                empty11.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty11);

                                                PdfPCell empty12 = new PdfPCell(new Phrase("  "));
                                                empty12.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty12);

                                                PdfPCell empty13 = new PdfPCell(new Phrase("  "));
                                                empty13.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty13);

                                                PdfPCell empty21 = new PdfPCell(new Phrase("  "));
                                                empty21.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty21);

                                                PdfPCell empty22 = new PdfPCell(new Phrase("  "));
                                                empty22.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty22);

                                                PdfPCell empty23 = new PdfPCell(new Phrase("  "));
                                                empty23.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty23);
                                            }
                                        }
                                        else if (fileNames.Count % 4 == 1)
                                        {
                                            int k = 0;
                                            int i;
                                            for (i = 0; i < fileNames.Count / 4; i++)
                                            {
                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg1.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;


                                                PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);


                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg2.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;


                                                PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                empty8.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty8);


                                                iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg3.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar2 = new PdfPCell(jpg3);
                                                bar2.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar2);
                                                k++;


                                                PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                empty9.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty9);

                                                iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg4.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar3 = new PdfPCell(jpg4);
                                                bar3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar3);
                                                k++;

                                                PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                empty14.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty14);

                                                PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                empty15.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty15);

                                                PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                empty16.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty16);


                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);


                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);

                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);

                                                PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                empty10.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty10);


                                            }
                                            iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg5.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar4 = new PdfPCell(jpg5);
                                            bar4.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar4);

                                            PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                            empty31.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty31);

                                            PdfPCell empty32 = new PdfPCell(new Phrase("  "));
                                            empty32.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty32);

                                            PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                            empty33.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty33);

                                            PdfPCell empty34 = new PdfPCell(new Phrase("  "));
                                            empty34.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty34);

                                            PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                            empty35.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty35);

                                            PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                            empty36.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty36);
                                        }
                                        else if (fileNames.Count % 4 == 2)
                                        {
                                            int k = 0;
                                            for (int i = 0; i < fileNames.Count / 4; i++)
                                            {
                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg1.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;


                                                PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);


                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg2.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;


                                                PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                empty8.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty8);


                                                iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg3.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar2 = new PdfPCell(jpg3);
                                                bar2.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar2);
                                                k++;


                                                PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                empty9.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty9);

                                                iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg4.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar3 = new PdfPCell(jpg4);
                                                bar3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar3);
                                                k++;

                                                PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                empty14.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty14);

                                                PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                empty15.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty15);

                                                PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                empty16.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty16);


                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);


                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);

                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);

                                                PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                empty10.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty10);



                                            }
                                            iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg5.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar4 = new PdfPCell(jpg5);
                                            bar4.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar4);

                                            k++;

                                            PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                            empty31.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty31);

                                            iTextSharp.text.Image jpg6 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg6.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar6 = new PdfPCell(jpg6);
                                            bar6.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar6);

                                            PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                            empty33.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty33);

                                            PdfPCell empty34 = new PdfPCell(new Phrase("  "));
                                            empty34.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty34);

                                            PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                            empty35.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty35);

                                            PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                            empty36.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty36);
                                        }
                                        else if (fileNames.Count % 4 == 3)
                                        {

                                            int k = 0;
                                            int i;
                                            for (i = 0; i < fileNames.Count / 4; i++)
                                            {

                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg1.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;


                                                PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);


                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg2.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;


                                                PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                empty8.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty8);


                                                iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg3.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar2 = new PdfPCell(jpg3);
                                                bar2.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar2);

                                                k++;


                                                PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                empty9.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty9);

                                                iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg4.ScaleAbsolute(110f, 60f);
                                                PdfPCell bar3 = new PdfPCell(jpg4);
                                                bar3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar3);

                                                k++;

                                                PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                empty14.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty14);

                                                PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                empty15.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty15);

                                                PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                empty16.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty16);


                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);


                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);

                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);

                                                PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                empty10.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty10);



                                            }
                                            iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg5.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar4 = new PdfPCell(jpg5);
                                            bar4.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar4);

                                            k++;

                                            PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                            empty31.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty31);

                                            iTextSharp.text.Image jpg99 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg99.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar99 = new PdfPCell(jpg99);
                                            bar99.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar99);

                                            k++;

                                            PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                            empty33.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty33);

                                            iTextSharp.text.Image jpg54 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            jpg54.ScaleAbsolute(110f, 60f);
                                            PdfPCell bar54 = new PdfPCell(jpg54);
                                            bar54.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar54);

                                            PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                            empty35.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty35);

                                            PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                            empty36.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty36);
                                        }
                                        else if (fileNames.Count == 1)
                                        {
                                            int k = 0;
                                            iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            PdfPCell bar = new PdfPCell(jpg1);
                                            bar.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar);

                                            PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                            empty3.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty3);
                                            PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                            empty4.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty4);

                                        }
                                        else
                                        {
                                            int k = 0;
                                            for (int i = 0; i < (fileNames.Count) / 2; i++)
                                            {




                                                iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                jpg1.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                                                PdfPCell bar = new PdfPCell(jpg1);
                                                bar.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar);

                                                k++;

                                                PdfPCell empty = new PdfPCell();
                                                empty.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty);
                                                iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                PdfPCell bar1 = new PdfPCell(jpg2);
                                                bar1.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(bar1);

                                                k++;

                                                PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                empty3.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty3);
                                                PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                empty4.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty4);
                                                PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                empty5.Border = PdfPCell.NO_BORDER;
                                                table.AddCell(empty5);





                                            }
                                            iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                            PdfPCell bar3 = new PdfPCell(jpg3);
                                            bar3.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(bar3);
                                            PdfPCell empty1 = new PdfPCell();
                                            empty1.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty1);
                                            PdfPCell empty2 = new PdfPCell();
                                            empty2.Border = PdfPCell.NO_BORDER;
                                            table.AddCell(empty2);
                                        }

                                        doc.Add(para);
                                        if (fileNames.Count > 32 && fileNames.Count <= 36)
                                        {
                                            doc.Add(new Paragraph("\n"));

                                            table.SpacingAfter = 70f;
                                        }
                                        doc.Add(table);


                                        para1 = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + fileNames.Count, font);

                                        para1.Alignment = Element.ALIGN_RIGHT;
                                        doc.Add(para1);

                                        doc.Add(table1);
                                        doc.Close();


                                        Directory.Delete(expFolder + "\\Barcode", true);

                                        bool insertlog = InsertIntoDB(projKey, bundleKey, cmbBundle.Text);
                                        if (insertlog == true)
                                        {
                                            MessageBox.Show(this, "Barcode Generated Successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            cmdExport.Enabled = true;


                                            int pendBundle = PendingBundle().Rows.Count;

                                        }

                                    }
                                    else
                                    {
                                        if ((fileNames.Count) > 0)
                                        {
                                            sfdUAT.Filter = "Pdf files (*.pdf)|*.pdf";
                                            sfdUAT.FilterIndex = 2;
                                            sfdUAT.RestoreDirectory = true;
                                            sfdUAT.FileName = cmbBundle.Text;
                                            sfdUAT.ShowDialog();
                                            try
                                            {
                                                FileStream fs = new FileStream(sfdUAT.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                                                Document doc = new Document();
                                                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                                                doc.SetPageSize(iTextSharp.text.PageSize.A4);
                                                PdfPTable table = new PdfPTable(7);
                                                table.TotalWidth = 500f;
                                                table.LockedWidth = true;
                                                float[] widths = new float[] { 4f, 1.66f, 4f, 1.66f, 4f, 1.66f, 4f };

                                                table.SetWidths(widths);
                                                table.HorizontalAlignment = 0;

                                                table.SpacingBefore = 10f;
                                                table.SpacingAfter = 5f;
                                                PdfPTable table1 = new PdfPTable(7);
                                                table1.TotalWidth = 500f;
                                                table1.LockedWidth = true;
                                                float[] widths1 = new float[] { 4f, 1.66f, 4f, 1.66f, 4f, 1.66f, 4f };

                                                table1.SetWidths(widths1);
                                                table1.HorizontalAlignment = 0;

                                                table1.SpacingBefore = 5f;
                                                table1.SpacingAfter = 10f;
                                                doc.Open();



                                                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                                                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL);
                                                para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + fileNames.Count, font);
                                                para.Alignment = Element.ALIGN_RIGHT;


                                                if (fileNames.Count % 4 == 0)
                                                {
                                                    int k = 0;
                                                    for (int i = 0; i < fileNames.Count / 4; i++)
                                                    {
                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;


                                                        PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);


                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg2.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;


                                                        PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                        empty8.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty8);


                                                        iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg3.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar2 = new PdfPCell(jpg3);
                                                        bar2.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar2);

                                                        k++;


                                                        PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                        empty9.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty9);

                                                        iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg4.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar3 = new PdfPCell(jpg4);
                                                        bar3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar3);

                                                        k++;

                                                        PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                        empty14.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty14);

                                                        PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                        empty15.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty15);

                                                        PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                        empty16.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty16);


                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);


                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);

                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);

                                                        PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                        empty10.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty10);


                                                        PdfPCell empty11 = new PdfPCell(new Phrase("  "));
                                                        empty11.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty11);

                                                        PdfPCell empty12 = new PdfPCell(new Phrase("  "));
                                                        empty12.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty12);

                                                        PdfPCell empty13 = new PdfPCell(new Phrase("  "));
                                                        empty13.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty13);

                                                        PdfPCell empty21 = new PdfPCell(new Phrase("  "));
                                                        empty21.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty21);

                                                        PdfPCell empty22 = new PdfPCell(new Phrase("  "));
                                                        empty22.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty22);

                                                        PdfPCell empty23 = new PdfPCell(new Phrase("  "));
                                                        empty23.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty23);
                                                    }
                                                }
                                                else if (fileNames.Count % 4 == 1)
                                                {
                                                    int k = 0;
                                                    int i;
                                                    for (i = 0; i < fileNames.Count / 4; i++)
                                                    {
                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;


                                                        PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);


                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg2.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;


                                                        PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                        empty8.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty8);


                                                        iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg3.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar2 = new PdfPCell(jpg3);
                                                        bar2.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar2);

                                                        k++;


                                                        PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                        empty9.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty9);

                                                        iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg4.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar3 = new PdfPCell(jpg4);
                                                        bar3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar3);

                                                        k++;

                                                        PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                        empty14.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty14);

                                                        PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                        empty15.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty15);

                                                        PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                        empty16.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty16);


                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);


                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);

                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);

                                                        PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                        empty10.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty10);


                                                    }
                                                    iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg5.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar4 = new PdfPCell(jpg5);
                                                    bar4.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar4);

                                                    PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                                    empty31.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty31);

                                                    PdfPCell empty32 = new PdfPCell(new Phrase("  "));
                                                    empty32.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty32);

                                                    PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                                    empty33.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty33);

                                                    PdfPCell empty34 = new PdfPCell(new Phrase("  "));
                                                    empty34.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty34);

                                                    PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                                    empty35.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty35);

                                                    PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                                    empty36.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty36);
                                                }
                                                else if (fileNames.Count % 4 == 2)
                                                {
                                                    int k = 0;
                                                    for (int i = 0; i < fileNames.Count / 4; i++)
                                                    {
                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;


                                                        PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);


                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg2.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;


                                                        PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                        empty8.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty8);


                                                        iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg3.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar2 = new PdfPCell(jpg3);
                                                        bar2.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar2);

                                                        k++;


                                                        PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                        empty9.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty9);

                                                        iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg4.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar3 = new PdfPCell(jpg4);
                                                        bar3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar3);

                                                        k++;

                                                        PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                        empty14.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty14);

                                                        PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                        empty15.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty15);

                                                        PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                        empty16.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty16);


                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);


                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);

                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);

                                                        PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                        empty10.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty10);


                                                    }
                                                    iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg5.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar4 = new PdfPCell(jpg5);
                                                    bar4.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar4);

                                                    k++;

                                                    PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                                    empty31.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty31);

                                                    iTextSharp.text.Image jpg6 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg6.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar6 = new PdfPCell(jpg6);
                                                    bar6.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar6);

                                                    PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                                    empty33.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty33);

                                                    PdfPCell empty34 = new PdfPCell(new Phrase("  "));
                                                    empty34.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty34);

                                                    PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                                    empty35.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty35);

                                                    PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                                    empty36.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty36);
                                                }
                                                else if (fileNames.Count % 4 == 3)
                                                {

                                                    int k = 0;
                                                    int i;
                                                    for (i = 0; i < fileNames.Count / 4; i++)
                                                    {

                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;


                                                        PdfPCell empty = new PdfPCell(new Phrase("  "));
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);


                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg2.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;


                                                        PdfPCell empty8 = new PdfPCell(new Phrase("  "));
                                                        empty8.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty8);


                                                        iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg3.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar2 = new PdfPCell(jpg3);
                                                        bar2.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar2);

                                                        k++;


                                                        PdfPCell empty9 = new PdfPCell(new Phrase("  "));
                                                        empty9.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty9);

                                                        iTextSharp.text.Image jpg4 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg4.ScaleAbsolute(110f, 60f);
                                                        PdfPCell bar3 = new PdfPCell(jpg4);
                                                        bar3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar3);

                                                        k++;

                                                        PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                                        empty14.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty14);

                                                        PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                                        empty15.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty15);

                                                        PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                                        empty16.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty16);


                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);


                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);

                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);

                                                        PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                                        empty10.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty10);

                                                    }
                                                    iTextSharp.text.Image jpg5 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg5.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar4 = new PdfPCell(jpg5);
                                                    bar4.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar4);

                                                    k++;

                                                    PdfPCell empty31 = new PdfPCell(new Phrase("  "));
                                                    empty31.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty31);

                                                    iTextSharp.text.Image jpg99 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg99.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar99 = new PdfPCell(jpg99);
                                                    bar99.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar99);

                                                    k++;

                                                    PdfPCell empty33 = new PdfPCell(new Phrase("  "));
                                                    empty33.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty33);

                                                    iTextSharp.text.Image jpg54 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    jpg54.ScaleAbsolute(110f, 60f);
                                                    PdfPCell bar54 = new PdfPCell(jpg54);
                                                    bar54.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar54);


                                                    PdfPCell empty35 = new PdfPCell(new Phrase("  "));
                                                    empty35.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty35);

                                                    PdfPCell empty36 = new PdfPCell(new Phrase("  "));
                                                    empty36.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty36);
                                                }

                                                else if (fileNames.Count == 1)
                                                {
                                                    int k = 0;
                                                    iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    PdfPCell bar = new PdfPCell(jpg1);
                                                    bar.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar);

                                                    PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                    empty3.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty3);
                                                    PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                    empty4.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty4);

                                                }
                                                else
                                                {
                                                    int k = 0;
                                                    for (int i = 0; i < (fileNames.Count) / 2; i++)
                                                    {

                                                        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        jpg1.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                                                        PdfPCell bar = new PdfPCell(jpg1);
                                                        bar.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar);

                                                        k++;

                                                        PdfPCell empty = new PdfPCell();
                                                        empty.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty);
                                                        iTextSharp.text.Image jpg2 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                        PdfPCell bar1 = new PdfPCell(jpg2);
                                                        bar1.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(bar1);

                                                        k++;

                                                        PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                                        empty3.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty3);
                                                        PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                                        empty4.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty4);
                                                        PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                                        empty5.Border = PdfPCell.NO_BORDER;
                                                        table.AddCell(empty5);





                                                    }
                                                    iTextSharp.text.Image jpg3 = iTextSharp.text.Image.GetInstance(imageList[k]);
                                                    PdfPCell bar3 = new PdfPCell(jpg3);
                                                    bar3.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(bar3);
                                                    PdfPCell empty1 = new PdfPCell();
                                                    empty1.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty1);
                                                    PdfPCell empty2 = new PdfPCell();
                                                    empty2.Border = PdfPCell.NO_BORDER;
                                                    table.AddCell(empty2);
                                                }

                                                doc.Add(para);
                                                doc.Add(table);


                                                doc.Add(table1);
                                                doc.Close();


                                                Directory.Delete(expFolder + "\\Barcode", true);
                                                bool insertlog = InsertIntoDB(projKey, bundleKey, cmbBundle.Text);
                                                if (insertlog == true)
                                                {
                                                    MessageBox.Show(this, "Barcode Generated Successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    cmdExport.Enabled = true;

                                                    int pendBundle = PendingBundle().Rows.Count;


                                                }
                                            }
                                            catch (Exception ex)
                                            { }
                                        }
                                    }

                                }
                            }






                            statusStrip1.Items.Clear();
                            statusStrip1.Items.Add("Status: Bundle Sucessfully Uploaded");
                            MessageBox.Show("Bundle Sucessfully Uploaded");
                            PopulateBundle();
                        }
                        else
                        {
                            statusStrip1.Items.Clear();
                            statusStrip1.Items.Add("Status: Bundle Sucessfully Uploaded");
                            MessageBox.Show("Bundle Sucessfully Uploaded");
                            PopulateBundle();
                            return;
                        }
                    }
                    else
                    {
                        statusStrip1.Items.Clear();
                        statusStrip1.Items.Add("Status: Uploading Cannot be Completed");
                        MessageBox.Show(this, "Uploading Cannot be Completed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
        }

        public bool UpdateBundle()
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateBundle();

                ret = true;
            }
            return ret;
        }

        public bool _UpdateBundle()
        {
            eSTATES state = eSTATES.PROJECT_INITIALIZED;
            return updateStatusLogic.UpdateBundleStatus(projKey, bundleKey, new eSTATES[] { }, new eSTATES[] { }, state);
        }

        public bool UpdateCaseFile()
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateCaseFile();

                ret = true;
            }
            return ret;
        }

        public bool _UpdateCaseFile()
        {
            eSTATES state = eSTATES.PROJECT_INITIALIZED;
            return updateStatusLogic.UpdateFileStatus(projKey, bundleKey, state);
        }
    }
}