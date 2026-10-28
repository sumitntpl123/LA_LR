using System;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace Reporting
{
    public partial class frmOutwardReport : Form
    {
        OdbcConnection sqlCon = null;

        public string stDate;
        public string endDate;

        public frmOutwardReport(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void frmOutwardReport_Load(object sender, EventArgs e)
        {
            stDate = DateTime.Now.ToString("yyyy-MM-dd");
            endDate = DateTime.Now.ToString("yyyy-MM-dd");

            dateTimePicker1.Text = stDate;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker1.Value = Convert.ToDateTime(stDate.ToString());

            dateTimePicker2.Text = endDate;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            dateTimePicker2.Value = Convert.ToDateTime(endDate.ToString());

            grdStatus.DataSource = null;
        }
        private void init()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntries();

            System.Data.DataTable DtTemp = new System.Data.DataTable();
            DtTemp.Columns.Add("Sl No");
            DtTemp.Columns.Add("Bundle Name");
            DtTemp.Columns.Add("Inward Date");
            DtTemp.Columns.Add("Outward Date");
            DtTemp.Columns.Add("File Count");
            DtTemp.Columns.Add("Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                int imgCount = 0;

                for (int j = 0; j < _GetFileCount(Dt.Rows[i][0].ToString(), Dt.Rows[i][1].ToString()).Rows.Count; j++)
                {
                    imgCount = imgCount + Convert.ToInt32(_GetImageCount(Dt.Rows[i][0].ToString(), Dt.Rows[i][1].ToString(), _GetFileCount(Dt.Rows[i][0].ToString(), Dt.Rows[i][1].ToString()).Rows[j][0].ToString()).Rows[0][0].ToString());
                }
                DtTemp.Rows.Add(i + 1, Dt.Rows[i][2].ToString(), Dt.Rows[i][3].ToString(), Dt.Rows[i][4].ToString(), _GetFileCount(Dt.Rows[i][0].ToString(), Dt.Rows[i][1].ToString()).Rows.Count.ToString(), imgCount);
            }



            grdStatus.DataSource = DtTemp;

            FormatDataGridView();

            this.grdStatus.Refresh();

        }
        public System.Data.DataTable _GetImageCount(string proj, string batch, string file)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select count(*) from image_master where proj_key = '" + proj + "' and batch_key = '" + batch + "' and policy_number = '" + file + "' and status <> 29";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetFileCount(string proj, string batch)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct filename from case_file_master where proj_code = '" + proj + "' and bundle_key = '" + batch + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        private void FormatDataGridView()
        {
            //Format the Data Grid View
            //grdStatus.Columns[0].Visible = false;
            //grdStatus.Columns[1].Visible = false;
            //dtGrdVol.Columns[2].Visible = false;
            //Format Colors


            //Set Autosize on for all the columns
            for (int i = 0; i < grdStatus.Columns.Count; i++)
            {
                grdStatus.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


        }
        public System.Data.DataTable _GetEntries()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select proj_code, bundle_key, bundle_name, date_format(inward_date, '%Y-%m-%d'), date_format(outward_date, '%Y-%m-%d') from bundle_master where " +
                         "date_format(outward_date, '%Y-%m-%d') >= '" + stDate + "' and date_format(outward_date, '%Y-%m-%d') <= '" + endDate + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            grdStatus.DataSource = null;


            stDate = dateTimePicker1.Text;
            endDate = dateTimePicker2.Text;

            init();
        }

        private void grdStatus_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (grdStatus.Rows.Count > 0)
                {
                    cmsDeeds.Show(Cursor.Position);
                }
            }
        }

        private void updateDeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdStatus.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                app.Visible = false;

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];


                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

                worksheet.Name = "Outward Report";

                worksheet.Cells[1, 4] = "Outward Report";
                Range range44 = worksheet.get_Range("D1");
                range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);

                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();




                worksheet.Cells[3, 2] = "Date From : " + stDate;
                Range range33 = worksheet.get_Range("B3");
                range33.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[4, 2] = "Date To : " + endDate;
                Range range34 = worksheet.get_Range("B4");
                range34.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                Range range = worksheet.get_Range("B3", "B4");
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);


                Range range1 = worksheet.get_Range("B7", "G7");
                range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                for (int i = 1; i < grdStatus.Columns.Count + 1; i++)
                {


                    Range range2 = worksheet.get_Range("B7", "G7");
                    range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                    range2.EntireRow.AutoFit();
                    range2.EntireColumn.AutoFit();
                    worksheet.Cells[7, i + 1] = grdStatus.Columns[i - 1].HeaderText;
                }

                worksheet.Cells[9 + grdStatus.Rows.Count, 5] = "Total";

                int filecount = 0;
                int imgcount = 0;

                for (int i = 0; i < grdStatus.Rows.Count; i++)
                {
                    for (int j = 0; j < grdStatus.Columns.Count; j++)
                    {
                        Range range3 = worksheet.Cells;
                        //range3.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range3.EntireRow.AutoFit();
                        range3.EntireColumn.AutoFit();
                        worksheet.Cells[i + 8, j + 2] = grdStatus.Rows[i].Cells[j].Value.ToString();
                        worksheet.Cells[i + 8, j + 2].Borders.Color = ColorTranslator.ToOle(Color.Black);

                    }

                    filecount = filecount + Convert.ToInt32(grdStatus.Rows[i].Cells[4].Value);
                    imgcount = imgcount + Convert.ToInt32(grdStatus.Rows[i].Cells[5].Value);
                }

                worksheet.Cells[9 + grdStatus.Rows.Count, 6] = filecount.ToString();
                worksheet.Cells[9 + grdStatus.Rows.Count, 7] = imgcount.ToString();
                worksheet.Cells[9 + grdStatus.Rows.Count, 5].Borders.Color = ColorTranslator.ToOle(Color.Black);
                worksheet.Cells[9 + grdStatus.Rows.Count, 6].Borders.Color = ColorTranslator.ToOle(Color.Black);
                worksheet.Cells[9 + grdStatus.Rows.Count, 7].Borders.Color = ColorTranslator.ToOle(Color.Black);



                worksheet.Cells[15 + grdStatus.Rows.Count, 2] = "Checked & Verified";
                worksheet.Cells[16 + grdStatus.Rows.Count, 2] = "Seal & Signature by Nevaeh Technology";
                worksheet.Cells[15 + grdStatus.Rows.Count, 7] = "Checked & Verified";
                worksheet.Cells[16 + grdStatus.Rows.Count, 7] = "Seal & Signature by ";

                string namexls = "Outward_Report" + ".xls";
                string path = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                sfdUAT.Filter = "Xls files (*.xls)|*.xls";
                sfdUAT.FilterIndex = 2;
                sfdUAT.RestoreDirectory = true;
                sfdUAT.FileName = namexls;
                sfdUAT.ShowDialog();

                workbook.SaveAs(sfdUAT.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                app.Quit();
            }
        }
    }
}
