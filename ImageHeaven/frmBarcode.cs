using DockSample;
//using AForge.Imaging;
//using AForge;
//using AForge.Imaging.Filters;
//using System.Drawing.Bitmap;
//using System.Drawing.Graphics;
//using Graphics.DrawImage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ImageHeaven
{
    public partial class frmBarcode : Form
    {
        wfePolicy wPolicy = null;
        public static string projKey = null;
        public static string bundleKey = null;
        public static string boxNumber = null;
        OdbcDataAdapter sqlAdap;
        private Credentials crd = new Credentials();
        MemoryStream stateLog;
        byte[] tmpWrite;
        private OdbcConnection sqlCon = null;
        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private FloatToolbox m_toolbox = new FloatToolbox();
        //private MagickNet.Image imgQc;
        private string imgFileName = null;
        NovaNet.Utils.ImageManupulation delImage;
        private wfeBox wBox = null;
        private CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        private CtrlBox pBox = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        private int indexCount = 0;
        private eSTATES[] currState;
        private eSTATES[] imageCurrState;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        //public static string projKey = null;
        //public static string bundleKey = null;
        //public static string boxNumber = null;
        //OdbcDataAdapter sqlAdap;




        iTextSharp.text.Image i2;
        System.Drawing.Image image3;
        int j;
        Paragraph para;
        Paragraph para1;
        Paragraph para2;
        int flag = 0;
        int flag1 = 0;
        int flag2 = 0;
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

        public frmBarcode(OdbcConnection prmCon, Credentials prmCrd)
        {
            sqlCon = prmCon;
            //wBox = prmBox;
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            crd = prmCrd;
            InitializeComponent();
            //img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            currState = new eSTATES[1];

            currState[0] = eSTATES.BARCODE_ENTRY;

            //img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            this.Text = "Barcode Generation ";
            exMailLog.SetNextLogger(exTxtLog);
        }
        private void frmBarcode_Load(object sender, EventArgs e)
        {
            //DisplayValues();
            populateProject();
            deCheckBox1.Checked = false;
            button1.Enabled = false;
            deButton20.Enabled = false;

            int pendBundle = pendingBundle().Rows.Count;

            if (pendBundle == 0)
            {
                deLabel1.Text = "No bundle is pending for barcode generate";
            }
            else { deLabel1.Text = "Pending bundle : " + pendBundle; }
        }
        public DataTable pendingBundle()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = "select distinct bundle_key, bundle_code from bundle_master where status <> '0' and bundle_key NOT IN (select distinct bundle_key from barcode_log)";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;
        }

        private void populateProject()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select proj_key, proj_code from project_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox1.DataSource = dt;
                deComboBox1.DisplayMember = "proj_code";
                deComboBox1.ValueMember = "proj_key";
                deComboBox1.Select();
                if (deCheckBox1.Checked == false)
                {
                    populateBatch();
                }
                else
                {
                    populateBatchReGen();
                }
            }
            else
            {
                deComboBox1.Text = "";
                MessageBox.Show("Add one project first...");

            }

        }

        private void populateBatchReGen()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select distinct a.bundle_key, a.bundle_code from bundle_master a, barcode_log b where a.proj_code = b.proj_key and (a.status <> '0') and a.proj_code = '" + deComboBox1.SelectedValue.ToString() + "'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox2.DataSource = dt;
                deComboBox2.DisplayMember = "bundle_code";
                deComboBox2.ValueMember = "bundle_key";

            }
            else
            {
                //cmbBatch.Text = "";
                //MessageBox.Show("No Batch found for this project...","Add Batch");
                deComboBox2.Text = string.Empty;
                deComboBox2.DataSource = null;
                deComboBox2.DisplayMember = "";
                deComboBox2.ValueMember = "";
                deComboBox1.Select();


            }

        }

        private void populateBatch()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.status <> '0'  and a.proj_code = '" + deComboBox1.SelectedValue.ToString() + "' and a.bundle_key NOT IN (select distinct bundle_key from barcode_log where proj_key = '" + deComboBox1.SelectedValue.ToString() + "')";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox2.DataSource = dt;
                deComboBox2.DisplayMember = "bundle_code";
                deComboBox2.ValueMember = "bundle_key";

            }
            else
            {
                //cmbBatch.Text = "";
                //MessageBox.Show("No Batch found for this project...","Add Batch");
                deComboBox2.Text = string.Empty;
                deComboBox2.DataSource = null;
                deComboBox2.DisplayMember = "";
                deComboBox2.ValueMember = "";
                deComboBox1.Select();

            }

        }

        private void PopulatePolicyList()
        {
            lstImage.Items.Clear();
            ArrayList arrPolicy = new ArrayList();
            CtrlPolicy ctrPol;

            arrPolicy = GetItems(eITEMS.POLICY, currState);
            for (int i = 0; i < arrPolicy.Count; i++)
            {
                ctrPol = (CtrlPolicy)arrPolicy[i];
                lstImage.Items.Add(ctrPol.PolicyNumber);
                if (lstImage.SelectedIndex == 0)
                {
                    //lstPolicy.Text = ctrPol.PolicyNumber;
                }
            }
            if (arrPolicy.Count > 0)
            {
                button1.Enabled = true;
                deButton20.Enabled = true;
                //lblCount.Text = "Pending: " + GetItems1(eITEMS.POLICY, currState).Count;
            }
            else
            {
                button1.Enabled = false;
                deButton20.Enabled = false;
                //lblCount.Text = "0";
                //lstImageDel.Items.Clear();
            }
            //CtrlPolicy ctrPolCurrent = null;
            //lblCurrentPolicy.Text = ctrPol.PolicyNumber.ToString();
            //if(lstPolicy.Items.Count >0)
            //{
            //    GetFileDetails(lstPolicy.SelectedIndices[0].ToString());
            //}
        }
        public ArrayList GetItems(eITEMS item, eSTATES[] state)
        {
            OdbcDataAdapter wAdap;
            OdbcTransaction trns = null;
            OdbcCommand oCom = new OdbcCommand();
            string strQuery = null;
            wItemControl wic = null;
            DataSet ds = new DataSet();
            ArrayList arrItem = new ArrayList();
            if (item == eITEMS.POLICY)
            {
                strQuery = "select proj_code,bundle_key,filename from case_file_master where proj_code= '" + projKey + "' and bundle_key='" + bundleKey + "' ";

                oCom.Connection = sqlCon;
                oCom.CommandText = strQuery;
                wAdap = new OdbcDataAdapter(oCom);
                wAdap.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string p = ds.Tables[0].Rows[i]["proj_code"].ToString();
                        string b = ds.Tables[0].Rows[i]["bundle_key"].ToString();

                        string pp = ds.Tables[0].Rows[i]["filename"].ToString();
                        wic = new CtrlPolicy(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_code"]), Convert.ToInt32(ds.Tables[0].Rows[i]["bundle_key"]), "1", ds.Tables[0].Rows[i]["filename"].ToString());
                        arrItem.Add(wic);

                    }
                }
            }
            return arrItem;
        }

        public string GetBundleName(int prmProjectKey, int prmBundleKey)
        {
            string sqlStr = null;
            string projName = null;

            DataSet projDs = new DataSet();

            try
            {
                sqlStr = "select bundle_code,bundle_no from bundle_master where proj_code= '" + prmProjectKey + "' and bundle_key = '" + prmBundleKey + "' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            if (projDs.Tables[0].Rows.Count > 0)
            {
                projName = projDs.Tables[0].Rows[0]["bundle_code"].ToString();
            }
            else
                projName = string.Empty;
            return projName;
        }

        private bool insertIntoDB(string pk, string bk, string bn)
        {
            bool commitBol = true;




            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"insert into barcode_log(proj_key,bundle_key,bundle_no,created_dttm,created_by) values('" + pk + "','" + bk + "','" + bn + "','" + crd.created_dttm + "','" + crd.created_by + "')";

            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int j = sqlCmd.ExecuteNonQuery();
            if (j > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
        }

        public DataTable GetFileDetails(string proj, string bundle, string fileName)
        {
            string sql = "select b.case_type,b.case_file_no,b.case_year,a.establishment from bundle_master a, case_file_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.proj_code = '" + proj + "' and b.bundle_key = '" + bundle + "' and b.filename = '" + fileName + "'";
            DataTable ds = new DataTable();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            return ds;
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            return m_toolbox;
        }

        public DataTable _GetGenCount(string proj, string bundle)
        {
            string sql = "select * from barcode_log  where proj_key = '" + proj + "' and bundle_key = '" + bundle + "' ";
            DataTable ds = new DataTable();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            return ds;
        }


        private void deButton1_Click(object sender, EventArgs e)
        {
            if (_GetGenCount(projKey, bundleKey).Rows.Count > 0)
            {
                button1.Enabled = false;
                deButton20.Enabled = false;
                string expFolder = "C:\\";
                bool isDeleted = false;
                //check if folder is exists or not
                if (Directory.Exists(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey))) && isDeleted == false)
                {
                    Directory.Delete(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)), true);
                }

                if (lstImage.Items.Count > 0)
                {
                    Application.DoEvents();
                    //bundle folder creation
                    if (!Directory.Exists(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey))))
                    {
                        Directory.CreateDirectory(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)));
                    }
                    String[] imageList = new String[lstImage.Items.Count];
                    //checking each filename
                    for (int x = 0; x < lstImage.Items.Count; x++)
                    {
                        string filename = lstImage.Items[x].ToString();


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
                        label2.Text = str2;
                        label1.Text = str1;
                        System.Drawing.Image img1 = ConvertTextToImage(str2, "calibri", 9, Color.Transparent, Color.Black, label2.Width, label2.Height);
                        //Image img4 = ConvertTextToImage("Nevaeh Technology", "calibri", 10, Color.Transparent, Color.Black, textBox1.Width, textBox1.Height);
                        System.Drawing.Image img4 = ConvertTextToImage(str1, "calibri", 9, Color.Transparent, Color.Black, label1.Width, label1.Height + 5);
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
                    if (lstImage.Items.Count > 36)
                    {

                        sfdUAT.Filter = "Pdf files (*.pdf)|*.pdf";
                        sfdUAT.FilterIndex = 2;
                        sfdUAT.RestoreDirectory = true;
                        sfdUAT.FileName = deComboBox2.Text;
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
                        //DirectoryInfo dir_info = new DirectoryInfo(folderPath);
                        //string directory = dir_info.Name;
                        //para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + lstImage.Items.Count + " ,Generated : " + _GetGenCount(projKey,bundleKey).Rows.Count +1 + " times", font);
                        para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + lstImage.Items.Count + " ,Generated : " + Convert.ToString(Convert.ToInt32(_GetGenCount(projKey, bundleKey).Rows.Count) + 1) + " times", font);

                        para.Alignment = Element.ALIGN_RIGHT;


                        if (lstImage.Items.Count % 4 == 0)
                        {
                            int k = 0;
                            for (int i = 0; i < lstImage.Items.Count / 4; i++)
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

                                //PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                //empty14.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty14);

                                //PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                //empty15.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty15);

                                //PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                //empty16.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty16);


                                //PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                //empty3.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty3);


                                //PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                //empty4.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty4);

                                //PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                //empty5.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty5);

                                //PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                //empty10.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty10);


                                //PdfPCell empty11 = new PdfPCell(new Phrase("  "));
                                //empty11.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty11);

                                //PdfPCell empty12 = new PdfPCell(new Phrase("  "));
                                //empty12.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty12);

                                //PdfPCell empty13 = new PdfPCell(new Phrase("  "));
                                //empty13.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty13);

                                //PdfPCell empty21 = new PdfPCell(new Phrase("  "));
                                //empty21.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty21);

                                //PdfPCell empty22 = new PdfPCell(new Phrase("  "));
                                //empty22.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty22);

                                //PdfPCell empty23 = new PdfPCell(new Phrase("  "));
                                //empty23.Border = PdfPCell.NO_BORDER;
                                //table.AddCell(empty23);
                            }
                        }
                        else if (lstImage.Items.Count % 4 == 1)
                        {
                            int k = 0;
                            int i;
                            for (i = 0; i < lstImage.Items.Count / 4; i++)
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
                        else if (lstImage.Items.Count % 4 == 2)
                        {
                            int k = 0;
                            for (int i = 0; i < lstImage.Items.Count / 4; i++)
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
                        else if (lstImage.Items.Count % 4 == 3)
                        {

                            int k = 0;
                            int i;
                            for (i = 0; i < lstImage.Items.Count / 4; i++)
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
                        else if (lstImage.Items.Count == 1)
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
                            for (int i = 0; i < (lstImage.Items.Count) / 2; i++)
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
                        if (lstImage.Items.Count > 32 && lstImage.Items.Count <= 36)
                        {
                            doc.Add(new Paragraph("\n"));

                            table.SpacingAfter = 70f;
                        }
                        doc.Add(table);

                        //DirectoryInfo dir_info1 = new DirectoryInfo(folderPath1);
                        //string directory1 = dir_info1.Name;
                        //para1 = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + lstImage.Items.Count + " ,Generated : " + Convert.ToString(Convert.ToInt32(_GetGenCount(projKey, bundleKey).Rows.Count) + 1) + " times", font);

                        //para1.Alignment = Element.ALIGN_RIGHT;
                        //doc.Add(para1);

                        doc.Add(table1);
                        doc.Close();


                        Directory.Delete(expFolder + "\\Barcode", true);
                        bool insertlog = insertIntoDB(projKey, bundleKey, deComboBox2.Text);
                        if (insertlog == true)
                        {

                            MessageBox.Show(this, "Barcode Generated Successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button1.Enabled = true;
                            deButton20.Enabled = true;
                            int pendBundle = pendingBundle().Rows.Count;

                            if (pendBundle == 0)
                            {
                                deLabel1.Text = "No bundle is pending for barcode generate";
                            }
                            else { deLabel1.Text = "Pending bundle : " + pendBundle; }
                        }


                    }
                    else
                    {
                        if ((lstImage.Items.Count) > 0)
                        {
                            sfdUAT.Filter = "Pdf files (*.pdf)|*.pdf";
                            sfdUAT.FilterIndex = 2;
                            sfdUAT.RestoreDirectory = true;
                            sfdUAT.FileName = deComboBox2.Text;
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



                                //DirectoryInfo dir_info = new DirectoryInfo(folderPath);
                                //string directory = dir_info.Name;
                                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL);
                                para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + lstImage.Items.Count + " ,Generated : " + Convert.ToString(Convert.ToInt32(_GetGenCount(projKey, bundleKey).Rows.Count) + 1) + " times", font);
                                para.Alignment = Element.ALIGN_RIGHT;


                                if (lstImage.Items.Count % 4 == 0)
                                {
                                    int k = 0;
                                    for (int i = 0; i < lstImage.Items.Count / 4; i++)
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

                                        //PdfPCell empty14 = new PdfPCell(new Phrase("  "));
                                        //empty14.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty14);

                                        //PdfPCell empty15 = new PdfPCell(new Phrase("  "));
                                        //empty15.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty15);

                                        //PdfPCell empty16 = new PdfPCell(new Phrase("  "));
                                        //empty16.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty16);


                                        //PdfPCell empty3 = new PdfPCell(new Phrase("  "));
                                        //empty3.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty3);


                                        //PdfPCell empty4 = new PdfPCell(new Phrase("  "));
                                        //empty4.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty4);

                                        //PdfPCell empty5 = new PdfPCell(new Phrase("  "));
                                        //empty5.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty5);

                                        //PdfPCell empty10 = new PdfPCell(new Phrase("  "));
                                        //empty10.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty10);


                                        //PdfPCell empty11 = new PdfPCell(new Phrase("  "));
                                        //empty11.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty11);

                                        //PdfPCell empty12 = new PdfPCell(new Phrase("  "));
                                        //empty12.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty12);

                                        //PdfPCell empty13 = new PdfPCell(new Phrase("  "));
                                        //empty13.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty13);

                                        //PdfPCell empty21 = new PdfPCell(new Phrase("  "));
                                        //empty21.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty21);

                                        //PdfPCell empty22 = new PdfPCell(new Phrase("  "));
                                        //empty22.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty22);

                                        //PdfPCell empty23 = new PdfPCell(new Phrase("  "));
                                        //empty23.Border = PdfPCell.NO_BORDER;
                                        //table.AddCell(empty23);
                                    }
                                }
                                else if (lstImage.Items.Count % 4 == 1)
                                {
                                    int k = 0;
                                    int i;
                                    for (i = 0; i < lstImage.Items.Count / 4; i++)
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
                                else if (lstImage.Items.Count % 4 == 2)
                                {
                                    int k = 0;
                                    for (int i = 0; i < lstImage.Items.Count / 4; i++)
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
                                else if (lstImage.Items.Count % 4 == 3)
                                {

                                    int k = 0;
                                    int i;
                                    for (i = 0; i < lstImage.Items.Count / 4; i++)
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

                                else if (lstImage.Items.Count == 1)
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
                                    for (int i = 0; i < (lstImage.Items.Count) / 2; i++)
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
                                //DirectoryInfo dir_info1 = new DirectoryInfo(folderPath1);
                                //string directory1 = dir_info1.Name;
                                //para1 = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + "- No. of Barcode: " + lstImage.Items.Count, font);
                                //para1.Alignment = Element.ALIGN_RIGHT;
                                //doc.Add(para1);

                                doc.Add(table1);
                                doc.Close();


                                Directory.Delete(expFolder + "\\Barcode", true);
                                bool insertlog = insertIntoDB(projKey, bundleKey, deComboBox2.Text);
                                if (insertlog == true)
                                {
                                    MessageBox.Show(this, "Barcode Generated Successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    button1.Enabled = true;
                                    deButton20.Enabled = true;
                                    int pendBundle = pendingBundle().Rows.Count;

                                    if (pendBundle == 0)
                                    {
                                        deLabel1.Text = "No bundle is pending for barcode generate";
                                    }
                                    else { deLabel1.Text = "Pending bundle : " + pendBundle; }
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
                button1.Enabled = false;
                deButton20.Enabled = false;
                string expFolder = "C:\\";
                bool isDeleted = false;
                //check if folder is exists or not
                if (Directory.Exists(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey))) && isDeleted == false)
                {
                    Directory.Delete(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)), true);
                }

                if (lstImage.Items.Count > 0)
                {
                    Application.DoEvents();
                    //bundle folder creation
                    if (!Directory.Exists(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey))))
                    {
                        Directory.CreateDirectory(expFolder + "\\Barcode\\" + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)));
                    }
                    String[] imageList = new String[lstImage.Items.Count];
                    //checking each filename
                    for (int x = 0; x < lstImage.Items.Count; x++)
                    {
                        string filename = lstImage.Items[x].ToString();

                        string str1 = "HIGH COURT OF CALCUTTA";
                        string casetype = GetFileDetails(projKey, bundleKey, filename).Rows[0][0].ToString();
                        string caseno = GetFileDetails(projKey, bundleKey, filename).Rows[0][1].ToString();
                        string caseyear = GetFileDetails(projKey, bundleKey, filename).Rows[0][2].ToString();
                        string est = GetFileDetails(projKey, bundleKey, filename).Rows[0][3].ToString();
                        //string entity = null;
                        //if (est == "Appellate")
                        //{
                        //    entity = "AS";
                        //}
                        //else
                        //{
                        //    entity = "OS";
                        //}
                        string str2 = "File No:" + casetype + "/" + caseno + "/" + caseyear + "/" + est;
                        string str3 = casetype + "/" + caseno + "/" + caseyear + "/" + est;
                        label2.Text = str2;
                        label1.Text = str1;
                        System.Drawing.Image img1 = ConvertTextToImage(str2, "calibri", 9, Color.Transparent, Color.Black, label2.Width, label2.Height);
                        //Image img4 = ConvertTextToImage("Nevaeh Technology", "calibri", 10, Color.Transparent, Color.Black, textBox1.Width, textBox1.Height);
                        System.Drawing.Image img4 = ConvertTextToImage(str1, "calibri", 9, Color.Transparent, Color.Black, label1.Width, label1.Height + 5);
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
                    if (lstImage.Items.Count > 36)
                    {

                        sfdUAT.Filter = "Pdf files (*.pdf)|*.pdf";
                        sfdUAT.FilterIndex = 2;
                        sfdUAT.RestoreDirectory = true;
                        sfdUAT.FileName = deComboBox2.Text;
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
                        //DirectoryInfo dir_info = new DirectoryInfo(folderPath);
                        //string directory = dir_info.Name;
                        para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + lstImage.Items.Count, font);

                        para.Alignment = Element.ALIGN_RIGHT;


                        if (lstImage.Items.Count % 4 == 0)
                        {
                            int k = 0;
                            for (int i = 0; i < lstImage.Items.Count / 4; i++)
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
                        else if (lstImage.Items.Count % 4 == 1)
                        {
                            int k = 0;
                            int i;
                            for (i = 0; i < lstImage.Items.Count / 4; i++)
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
                        else if (lstImage.Items.Count % 4 == 2)
                        {
                            int k = 0;
                            for (int i = 0; i < lstImage.Items.Count / 4; i++)
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
                        else if (lstImage.Items.Count % 4 == 3)
                        {

                            int k = 0;
                            int i;
                            for (i = 0; i < lstImage.Items.Count / 4; i++)
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
                        else if (lstImage.Items.Count == 1)
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
                            for (int i = 0; i < (lstImage.Items.Count) / 2; i++)
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
                        if (lstImage.Items.Count > 32 && lstImage.Items.Count <= 36)
                        {
                            doc.Add(new Paragraph("\n"));

                            table.SpacingAfter = 70f;
                        }
                        doc.Add(table);

                        //DirectoryInfo dir_info1 = new DirectoryInfo(folderPath1);
                        //string directory1 = dir_info1.Name;
                        para1 = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + lstImage.Items.Count, font);

                        para1.Alignment = Element.ALIGN_RIGHT;
                        doc.Add(para1);

                        doc.Add(table1);
                        doc.Close();


                        Directory.Delete(expFolder + "\\Barcode", true);

                        bool insertlog = insertIntoDB(projKey, bundleKey, deComboBox2.Text);
                        if (insertlog == true)
                        {
                            MessageBox.Show(this, "Barcode Generated Successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button1.Enabled = true;
                            deButton20.Enabled = true;

                            int pendBundle = pendingBundle().Rows.Count;

                            if (pendBundle == 0)
                            {
                                deLabel1.Text = "No bundle is pending for barcode generate";
                            }
                            else { deLabel1.Text = "Pending bundle : " + pendBundle; }
                        }

                    }
                    else
                    {
                        if ((lstImage.Items.Count) > 0)
                        {
                            sfdUAT.Filter = "Pdf files (*.pdf)|*.pdf";
                            sfdUAT.FilterIndex = 2;
                            sfdUAT.RestoreDirectory = true;
                            sfdUAT.FileName = deComboBox2.Text;
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



                                //DirectoryInfo dir_info = new DirectoryInfo(folderPath);
                                //string directory = dir_info.Name;
                                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL);
                                para = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + ", No. of Barcode: " + lstImage.Items.Count, font);
                                para.Alignment = Element.ALIGN_RIGHT;


                                if (lstImage.Items.Count % 4 == 0)
                                {
                                    int k = 0;
                                    for (int i = 0; i < lstImage.Items.Count / 4; i++)
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
                                else if (lstImage.Items.Count % 4 == 1)
                                {
                                    int k = 0;
                                    int i;
                                    for (i = 0; i < lstImage.Items.Count / 4; i++)
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
                                else if (lstImage.Items.Count % 4 == 2)
                                {
                                    int k = 0;
                                    for (int i = 0; i < lstImage.Items.Count / 4; i++)
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
                                else if (lstImage.Items.Count % 4 == 3)
                                {

                                    int k = 0;
                                    int i;
                                    for (i = 0; i < lstImage.Items.Count / 4; i++)
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

                                else if (lstImage.Items.Count == 1)
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
                                    for (int i = 0; i < (lstImage.Items.Count) / 2; i++)
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
                                //DirectoryInfo dir_info1 = new DirectoryInfo(folderPath1);
                                //string directory1 = dir_info1.Name;
                                //para1 = new Paragraph("Bundle name: " + GetBundleName(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey)) + "- No. of Barcode: " + lstImage.Items.Count, font);
                                //para1.Alignment = Element.ALIGN_RIGHT;
                                //doc.Add(para1);

                                doc.Add(table1);
                                doc.Close();


                                Directory.Delete(expFolder + "\\Barcode", true);
                                bool insertlog = insertIntoDB(projKey, bundleKey, deComboBox2.Text);
                                if (insertlog == true)
                                {
                                    MessageBox.Show(this, "Barcode Generated Successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    button1.Enabled = true;
                                    deButton20.Enabled = true;

                                    int pendBundle = pendingBundle().Rows.Count;

                                    if (pendBundle == 0)
                                    {
                                        deLabel1.Text = "No bundle is pending for barcode generate";
                                    }
                                    else { deLabel1.Text = "Pending bundle : " + pendBundle; }
                                }
                            }
                            catch (Exception ex)
                            { }
                        }
                    }

                }
            }



        }

        private void deComboBox1_Leave(object sender, EventArgs e)
        {
            if (deCheckBox1.Checked == false)
            {
                populateBatch();
            }
            else
            {
                populateBatchReGen();
            }
        }

        private void deButton1_Click_1(object sender, EventArgs e)
        {
            button1.Enabled = false;
            deButton20.Enabled = false;
            if (deComboBox1.Text != "" && deComboBox2.Text != "")
            {
                projKey = deComboBox1.SelectedValue.ToString();
                bundleKey = deComboBox2.SelectedValue.ToString();
            }
            else
            {
                projKey = null;
                bundleKey = null;
            }

            if (projKey != null && bundleKey != null)
            {
                int boxNumber = 1;
                PopulateView(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber);
                PopulatePolicyList();
                if (lstImage.Items.Count > 0)
                { button1.Enabled = true; deButton20.Enabled = true; }
                else
                { button1.Enabled = false; deButton20.Enabled = false; }
            }
            else
            {
                button1.Enabled = false;
                deButton20.Enabled = false;
                grdStatus.DataSource = null;
            }
        }

        private void PopulateView(int prmProjKey, int prmBatchKey, int prmBox)
        {
            DataSet dsAwtJobCrt = new DataSet();
            DataSet dsAwtAdf = new DataSet();
            DataSet dsSub = new DataSet();
            DataSet dsCert = new DataSet();
            DataSet dsAwtQc = new DataSet();
            DataSet dsAwtIndex = new DataSet();
            DataSet dsIndexed = new DataSet();
            DataSet dslicExcp = new DataSet();
            DataSet dsOnHold = new DataSet();
            DataSet dsMissing = new DataSet();
            //DataSet dsAwtQc = new DataSet();
            DataSet dsIndexIncom = new DataSet();
            DataSet dsExport = new DataSet();
            DataSet dsIncomplete = new DataSet();
            DataSet dsImgIncomplete = new DataSet();
            DataSet dsAwtExport = new DataSet();
            CtrlPolicy pPolicy = null;
            wfeBox tmpBox = new wfeBox(sqlCon);
            DataTable dt = new DataTable();
            DataSet imageCount = new DataSet();
            DataRow dr;
            NovaNet.wfe.eSTATES[] policyState;
            NovaNet.wfe.eSTATES[] policyStateIndexed = new NovaNet.wfe.eSTATES[2];
            dt.Columns.Add("Srl Number");
            dt.Columns.Add("Awt_Upload");
            dt.Columns.Add("Awt_Scan");
            dt.Columns.Add("Awt_QC");
            dt.Columns.Add("Awt_Index");
            dt.Columns.Add("Awt_Submission");
            dt.Columns.Add("Awt_Certification");
            dt.Columns.Add("CHCException");
            dt.Columns.Add("Entry_Incomplete");
            dt.Columns.Add("Image_Incomplete");
            dt.Columns.Add("Awt_Export");
            //dt.Columns.Add("Index incomplete");

            //dt.Columns.Add("On Hold");
            //dt.Columns.Add("Missing");

            dt.Columns.Add("Exported");
            try
            {
                //pPolicy = new CtrlPolicy(Convert.ToInt32(prmProjKey), Convert.ToInt32(prmBatchKey), prmBox, 0);
                pPolicy = new CtrlPolicy(Convert.ToInt32(prmProjKey), Convert.ToInt32(prmBatchKey), "1");
                wPolicy = new wfePolicy(sqlCon, pPolicy);
                //populate dataset for job creation
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_INITIALIZED;
                dsAwtJobCrt = wPolicy.GetPolicyList1(policyState);

                //populate dataset for awt adf
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_CREATED;
                dsAwtAdf = wPolicy.GetPolicyList2(policyState);


                //populate dataset for awt qc
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_SCANNED;
                dsAwtQc = wPolicy.GetPolicyList3(policyState);


                //populate dataset for awt index
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_QC;
                dsAwtIndex = wPolicy.GetPolicyList4(policyState);

                //awt submission
                policyState = new NovaNet.wfe.eSTATES[3];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                dsSub = wPolicy.GetPolicyList11(policyState);

                //populate dataset for certification
                policyState = new NovaNet.wfe.eSTATES[3];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                dsCert = wPolicy.GetPolicyList5(policyState);

                //populate dataset for chc Exception
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                dslicExcp = wPolicy.GetPolicyList6(policyState);

                //populate dataset for on hold
                //policyState = new NovaNet.wfe.eSTATES[1];
                //policyState[0] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
                //dsOnHold = wPolicy.GetPolicyList7(policyState);

                //policyState = new NovaNet.wfe.eSTATES[1];
                //policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
                //dsMissing = wPolicy.GetPolicyList8(policyState);

                //entry incomplete
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
                dsIncomplete = wPolicy.GetPolicyList12(policyState);

                //image incomplete
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
                dsImgIncomplete = wPolicy.GetPolicyList13(policyState);

                // awt export
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
                dsAwtExport = wPolicy.GetPolicyList9(policyState);

                //exported
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
                dsExport = wPolicy.GetPolicyList10(policyState);



                for (int i = 0; i < wPolicy.GetPolicyCount().Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["Srl Number"] = i + 1;



                    if ((i + 1) <= dsAwtJobCrt.Tables[0].Rows.Count)
                    {
                        dr["Awt_Upload"] = dsAwtJobCrt.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_Upload"] = string.Empty;
                    }

                    if ((i + 1) <= dsAwtAdf.Tables[0].Rows.Count)
                    {
                        dr["Awt_Scan"] = dsAwtAdf.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_Scan"] = string.Empty;
                    }
                    if ((i + 1) <= dsAwtQc.Tables[0].Rows.Count)
                    {
                        dr["Awt_QC"] = dsAwtQc.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_QC"] = string.Empty;
                    }
                    if ((i + 1) <= dsAwtIndex.Tables[0].Rows.Count)
                    {
                        dr["Awt_Index"] = dsAwtIndex.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_Index"] = string.Empty;
                    }
                    if ((i + 1) <= dsSub.Tables[0].Rows.Count)
                    {
                        dr["Awt_Submission"] = dsSub.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_Submission"] = string.Empty;
                    }
                    if ((i + 1) <= dsCert.Tables[0].Rows.Count)
                    {
                        dr["Awt_Certification"] = dsCert.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_Certification"] = string.Empty;
                    }
                    if ((i + 1) <= dslicExcp.Tables[0].Rows.Count)
                    {
                        dr["CHCException"] = dslicExcp.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["CHCException"] = string.Empty;
                    }
                    if ((i + 1) <= dsIncomplete.Tables[0].Rows.Count)
                    {
                        dr["Entry_Incomplete"] = dsIncomplete.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Entry_Incomplete"] = string.Empty;
                    }
                    if ((i + 1) <= dsImgIncomplete.Tables[0].Rows.Count)
                    {
                        dr["Image_Incomplete"] = dsImgIncomplete.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Image_Incomplete"] = string.Empty;
                    }
                    if ((i + 1) <= dsAwtExport.Tables[0].Rows.Count)
                    {
                        dr["Awt_Export"] = dsAwtExport.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_Export"] = string.Empty;
                    }

                    //if((i+1)<=dsOnHold.Tables[0].Rows.Count)
                    //{
                    //    dr["On Hold"] = dsOnHold.Tables[0].Rows[i][0].ToString();
                    //}
                    //else
                    //{
                    //    dr["On Hold"] = string.Empty;
                    //}
                    //if((i+1)<=dsMissing.Tables[0].Rows.Count)
                    //{
                    //    dr["Missing"] = dsMissing.Tables[0].Rows[i][0].ToString();
                    //}
                    //else
                    //{
                    //    dr["Missing"] = string.Empty;
                    //}

                    if ((i + 1) <= dsExport.Tables[0].Rows.Count)
                    {
                        dr["Exported"] = dsExport.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Exported"] = string.Empty;
                    }
                    dt.Rows.Add(dr);
                }
                grdStatus.DataSource = dt;
                grdStatus.ForeColor = Color.Black;

                //Set Autosize on for all the columns
                for (int i = 0; i < grdStatus.Columns.Count; i++)
                {
                    grdStatus.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                MessageBox.Show("Error while populating the file list......" + err);
            }
        }

        private void deCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (deCheckBox1.Checked == false)
            {
                populateBatch();
            }
            else
            {
                populateBatchReGen();
            }
        }

        private void deButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deButton20_Click(object sender, EventArgs e)
        {
            deButton1_Click(sender, e);
        }

    }
}
