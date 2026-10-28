using System;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using NovaNet.Utils;

namespace Reporting
{
    public partial class frmDetailDashboard : Form
    {
        OdbcConnection sqlCon = null;
        private INIReader rd = null;
        private KeyValueStruct udtKeyValue;
        public string location = "";
        string totalImageScaned = "";
        string scanComplete = "";
        string indexComplete = "";
        string fqcComplete = "";

        string uatCom = "";
        string uatimg = "";
        string uatby = "";
        string uatdate = "";
        public string loc { get; set; }

        public frmDetailDashboard(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void frmDetailDashboard_Load(object sender, EventArgs e)
        {
            grdStatus.DataSource = null;
            deTextBox1.Visible = false;
            deTextBox29.Visible = false;
            deLabel29.Visible = false;
            deLabel1.Visible = false;
            updateDeedToolStripMenuItem.Visible = false;
            deLabel2.Text = "Maximum running serial no : " + _GetTotalBundleEntries().Rows[0][0];
            INIFile ini = new INIFile();
            string iniPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + "IhConfiguration.ini";
            location = ini.ReadINI("LOCCONFIG", "LOC", string.Empty, iniPath).Trim('\0').Replace("\0", "");
            // location = ini.ReadINI("LOCCONFIG", "LOC", string.Empty, iniPath).TrimEnd();
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            grdStatus.DataSource = null;
            init();
        }

        public System.Data.DataTable _GetTotalBundleEntries()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select count(*) " +
                         "from bundle_master a, metadata_entry b where " +
                         "a.proj_code = b.proj_code and  " +
                         " a.bundle_key = b.bundle_key order by b.running_serial";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _getTotalentries()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            loc = location.Replace("\0", "");
            string sql = "SELECT me2.running_serial AS 'SlNo', '" + loc + "' AS Location,'Nevaeh' AS Software, " +
                "DATE_FORMAT(a.created_dttm, '%d-%m-%Y') AS 'monthYear', " +
                "bm2.bundle_name, bm2.bundle_no, a.case_type, a.case_file_no, a.case_year,a.filename," +
                "(SELECT CONCAT(DATE_FORMAT(tl.scanned_dttm, '%d-%m-%Y'), " +
                "',', DATE_FORMAT(tl.Qc_DTTM, '%d-%m-%Y'),',', DATE_FORMAT(tl.Index_DTTM, '%d-%m-%Y')," +
                "',', DATE_FORMAT(tl.fqc_DTTM, '%d-%m-%Y')" +
                ") FROM transaction_log tl WHERE a.proj_code = tl.proj_key " +
                "AND a.bundle_key = tl.batch_key AND a.filename = tl.policy_number)" +
                " AS 'scanComplete,qcComplete,IndexComplete,fqcComplete'," +
                "(SELECT COUNT(*) FROM image_master im WHERE a.proj_code = im.proj_key AND a.bundle_key = im.batch_key " +
                "AND im.box_number = '1' AND a.filename = im.policy_number) AS imageCount," +
                "(SELECT CONCAT('Completed'," +
                "',', COUNT(*)," +
                "',', DATE_FORMAT(lql.created_dttm, '%d-%m-%Y')," +
                "',', lql.Created_by) " +
                "FROM lic_qa_log lql " +
                "INNER JOIN image_master im ON lql.proj_key = im.proj_key AND lql.batch_key = im.batch_key AND im.box_number = '1' " +
                "AND lql.policy_number = im.policy_number AND im.status = '33' " +
                "WHERE a.proj_code = lql.proj_key " +
                "AND a.bundle_key = lql.batch_key AND a.filename = lql.policy_number " +
                "GROUP BY im.proj_key, im.batch_key, im.box_number, im.policy_number " +
                "ORDER BY lql.created_by DESC LIMIT 1) AS 'uatComplete,uatImage,uatDate,uatDoneBy'," +
                " (SELECT CASE WHEN me.status = 'N' THEN 'Not Exported' ELSE 'Exported' END FROM metadata_entry me " +
                "WHERE a.proj_code = me.proj_code AND a.bundle_key = me.bundle_key AND a.filename = me.filename) AS OcrFile," +
                "(SELECT COUNT(*) FROM metadata_entry me INNER JOIN image_master im " +
                "ON me.proj_code = im.proj_key AND me.bundle_key = im.batch_key " +
                "AND me.filename = im.policy_number WHERE a.proj_code = me.proj_code" +
                " AND a.bundle_key = me.bundle_key " +
                "AND im.box_number = '1' " +
                "AND a.filename = me.filename AND im.status = '33' AND me.status <> 'N' " +
                "GROUP BY im.proj_key, im.batch_key, im.policy_number) AS OcrImage " +
                "FROM bundle_master bm2 INNER JOIN case_file_master a " +
                "ON a.proj_code = bm2.proj_code AND a.bundle_key = bm2.bundle_key " +
                "INNER JOIN metadata_entry me2 " +
                "ON a.proj_code = me2.proj_code AND a.bundle_key = me2.bundle_key AND me2.filename = a.filename " +
                "ORDER BY slno LIMIT 10000000";


            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;

        }
        public System.Data.DataTable _GetBundleEntries()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select a.proj_code, a.bundle_key, date_format(a.inward_date,'%M-%Y'), a.bundle_name, a.bundle_no, b.case_type,b.case_file_no,b.case_year,b.filename, " +
                         "date_format(a.outward_date,'%M-%Y') , b.status, b.running_serial " +
                         "from bundle_master a, metadata_entry b where " +
                         "a.proj_code = b.proj_code and b.running_serial >='" + deTextBox29.Text + "' and b.running_serial <= '" + deTextBox1.Text + "' and " +
                         " a.bundle_key = b.bundle_key order by b.running_serial";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void init()
        {
            System.Data.DataTable table = new System.Data.DataTable();
            TimeWatcher tw = new TimeWatcher();
            tw.start();
            table = _getTotalentries();

            grdStatus.DataSource = table;
            tw.stop();
            return;
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

        private string TableToString(System.Data.DataTable dt)
        {
            string strData = string.Empty;
            string sep = string.Empty;
            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn c in dt.Columns)
                {
                    if (c.DataType != typeof(System.Guid) &&
                    c.DataType != typeof(System.Byte[]))
                    {
                        strData += sep + c.ColumnName;
                        sep = ",";
                    }
                }
                strData += "\r\n";
                foreach (DataRow r in dt.Rows)
                {
                    sep = string.Empty;
                    foreach (DataColumn c in dt.Columns)
                    {
                        if (c.DataType != typeof(System.Guid) &&
                        c.DataType != typeof(System.Byte[]))
                        {
                            if (!Convert.IsDBNull(r[c.ColumnName]))

                                strData += sep +
                                    r[c.ColumnName].ToString().Trim().Replace("\n", " ");
                            /*'"' + r[c.ColumnName].ToString().Trim().Replace("\n", " ").Replace(",", "-") + '"';*/
                            else
                                strData += sep + "";
                            sep = ",";

                        }
                    }
                    strData += "\r\n";

                }
            }
            else
            {
                //strData += "\r\n---> Table was empty!";
                foreach (DataColumn c in dt.Columns)
                {
                    if (c.DataType != typeof(System.Guid) &&
                    c.DataType != typeof(System.Byte[]))
                    {
                        strData += sep + c.ColumnName;
                        sep = ",";
                    }
                }
                strData += "\r\n";
            }
            return strData;
        }

        private void updateDeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdStatus.Rows.Count > 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.Windows.Forms.Application.DoEvents();

                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                app.Visible = false;

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];


                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

                worksheet.Name = "Detailed Dashboard Report";

                worksheet.Cells[1, 9] = "Detailed Dashboard Report";
                Range range44 = worksheet.get_Range("I1");
                range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);

                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();




                Range range1 = worksheet.get_Range("B3", "AF3");
                range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                for (int i = 1; i < grdStatus.Columns.Count + 1; i++)
                {


                    Range range2 = worksheet.get_Range("B3", "AF3");
                    range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                    range2.EntireRow.AutoFit();
                    range2.EntireColumn.AutoFit();
                    worksheet.Cells[3, i + 1] = grdStatus.Columns[i - 1].HeaderText;
                }
                using (Process p = Process.GetCurrentProcess())
                {
                    for (int i = 0; i < grdStatus.Rows.Count; i++)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        System.Windows.Forms.Application.DoEvents();

                        for (int j = 0; j < grdStatus.Columns.Count; j++)
                        {
                            Range range3 = worksheet.Cells;
                            //range3.Borders.Color = ColorTranslator.ToOle(Color.Black);
                            range3.EntireRow.AutoFit();
                            range3.EntireColumn.AutoFit();
                            worksheet.Cells[i + 4, j + 2] = grdStatus.Rows[i].Cells[j].Value.ToString();
                            worksheet.Cells[i + 4, j + 2].Borders.Color = ColorTranslator.ToOle(Color.Black);

                        }
                        System.Windows.Forms.Application.DoEvents();

                    }
                }

                string namexls = "Detailed_Dashboard_Report" + ".csv";
                string path = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                sfdUAT.Filter = "CSV files (*.csv)|*.csv";
                sfdUAT.FilterIndex = 2;
                sfdUAT.RestoreDirectory = true;
                sfdUAT.FileName = namexls;
                sfdUAT.ShowDialog();

                workbook.SaveAs(sfdUAT.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //tabTextFile(grdStatus, sfdUAT.FileName);

                app.Quit();
            }
        }
        public bool validateFrom()
        {
            bool retval = false;
            if (deTextBox29.Text != "")
            {

                bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox29.Text, "[^0-9]");
                if (res != true && deTextBox29.Text.Substring(0, 1) != "0")
                {
                    retval = true;
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please input Valid no...");
                    deTextBox29.Focus();
                    return retval;
                }
            }
            else
            {
                retval = false;
                MessageBox.Show("Please input Valid no...");
                deTextBox29.Focus();
                return retval;
            }
            return retval;
        }
        public bool validateTo()
        {
            bool retval = false;
            if (deTextBox1.Text != "")
            {

                bool res = System.Text.RegularExpressions.Regex.IsMatch(deTextBox1.Text, "[^0-9]");
                if (res != true && deTextBox1.Text.Substring(0, 1) != "0")
                {
                    retval = true;
                }
                else
                {
                    retval = false;
                    MessageBox.Show("Please input Valid no...");
                    deTextBox1.Focus();
                    return retval;
                }
            }
            else
            {
                retval = false;
                MessageBox.Show("Please input Valid no...");
                deTextBox1.Focus();
                return retval;
            }
            return retval;
        }
        private void deTextBox29_Leave(object sender, EventArgs e)
        {
            validateFrom();
        }

        private void deTextBox1_Leave(object sender, EventArgs e)
        {
            validateTo();
        }

        private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string date = DateTime.UtcNow.ToString("dd-MM-yyyy");
            tabTextFile(grdStatus, "D:\\" + "Dashboard Report_" + date + ".csv");
            MessageBox.Show("CSV Generated......");
        }
        public void tabTextFile(DataGridView dg, string filename)
        {

            DataSet ds = new DataSet();
            System.Data.DataTable dtSource = null;
            System.Data.DataTable dt = new System.Data.DataTable();
            DataRow dr;
            if (dg.DataSource != null)
            {
                if (dg.DataSource.GetType() == typeof(DataSet))
                {
                    DataSet dsSource = (DataSet)dg.DataSource;
                    if (dsSource.Tables.Count > 0)
                    {
                        string strTables = string.Empty;
                        foreach (System.Data.DataTable dt1 in dsSource.Tables)
                        {
                            strTables += TableToString(dt1);
                            strTables += "\r\n\r\n";
                            //strTables += "\r\n";
                        }
                        if (strTables != string.Empty)
                            SaveDataGridData(strTables, filename);
                    }
                }
                else
                {
                    if (dg.DataSource.GetType() == typeof(System.Data.DataTable))
                        dtSource = (System.Data.DataTable)dg.DataSource;
                    if (dtSource != null)
                        TableToString1(dtSource, filename);
                    //SaveDataGridData(, filename);
                }
            }

        }
        private void SaveDataGridData(string strData, string strFileName)
        {
            FileStream fs;
            TextWriter tw = null;
            try
            {
                if (File.Exists(strFileName))
                {
                    fs = new FileStream(strFileName, FileMode.Open);
                }
                else
                {
                    fs = new FileStream(strFileName, FileMode.Create);
                }
                tw = new StreamWriter(fs);
                tw.Write(strData);
            }
            finally
            {
                if (tw != null)
                {
                    tw.Flush();
                    tw.Close();
                }
            }
        }
        private string TableToString1(System.Data.DataTable dt, string strFileName)
        {

            FileStream fs;
            TextWriter tw = null;
            string strData = string.Empty;
            try
            {
                if (File.Exists(strFileName))
                {
                    fs = new FileStream(strFileName, FileMode.Open);
                }
                else
                {
                    fs = new FileStream(strFileName, FileMode.Create);
                }

                tw = new StreamWriter(fs);

                string sep = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn c in dt.Columns)
                    {
                        if (c.DataType != typeof(System.Guid) &&
                        c.DataType != typeof(System.Byte[]))
                        {
                            strData += sep + c.ColumnName;
                            sep = ",";
                        }
                    }
                    strData += "\r\n";
                    tw.Write(strData);
                    int exportCount = 0;
                    dt.Rows.OfType<DataRow>().ToList().ForEach(row =>
                    {
                        strData = string.Empty;
                        sep = string.Empty;
                        dt.Columns.OfType<DataColumn>().ToList().ForEach(col =>
                        {
                            if (col.DataType != typeof(System.Guid) &&
                            col.DataType != typeof(System.Byte[]))
                            {
                                if (!Convert.IsDBNull(row[col.ColumnName]))
                                    strData += sep
                                        + row[col.ColumnName.Trim()].ToString().Replace("\n", " ");
                                else
                                {
                                    strData += sep + "";
                                    var countOfCommas = col.ColumnName.ToList().FindAll(e => e == ',').Count();
                                    for (int i = 0; i < countOfCommas; i++)
                                    {
                                        strData += sep + "";
                                    }
                                }
                                sep = ",";
                            }
                        });
                        strData += "\r\n";
                        exportCount++;
                        if (exportCount % 100 == 0)
                        {
                            System.Diagnostics.Debug.Print(Convert.ToString(exportCount) + " images computed");
                        }
                        tw.Write(strData);
                    });
                    /*
                    foreach (DataRow r in dt.Rows)
                    {
                        strData = string.Empty;
                        sep = string.Empty;
                        foreach (DataColumn c in dt.Columns)
                        {
                            if (c.DataType != typeof(System.Guid) &&
                            c.DataType != typeof(System.Byte[]))
                            {
                                try
                                {
                                    if (!Convert.IsDBNull(r[c.ColumnName]))
                                        strData += sep
                                            + r[c.ColumnName.Trim()].ToString().Replace("\n", " ");
                                    else
                                    {
                                        strData += sep + "";
                                        var countOfCommas = c.ColumnName.ToList().FindAll(e => e == ',').Count();
                                        for (int i = 0; i < countOfCommas; i++)
                                        {
                                            strData += sep + "";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.Print(ex.Message);
                                }
                                sep = ",";
                            }
                        }
                        strData += "\r\n";
                        exportCount++;
                        if (exportCount % 100 == 0)
                        {
                            System.Diagnostics.Debug.Print(Convert.ToString(exportCount) + " images computed");
                        }
                        tw.Write(strData);
                    }
                    */
                }
                else
                {
                    //strData += "\r\n---> Table was empty!";
                    foreach (DataColumn c in dt.Columns)
                    {
                        if (c.DataType != typeof(System.Guid) &&
                        c.DataType != typeof(System.Byte[]))
                        {
                            strData += sep + c.ColumnName;
                            sep = ",";
                        }
                    }
                    strData += "\r\n";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally
            {
                if (tw != null)
                {
                    tw.Flush();
                    tw.Close();
                }
            }

            return strData;
        }
    }
}
