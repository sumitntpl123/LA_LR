using System;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using NovaNet.Utils;

namespace Reporting
{
    public partial class frmUserReport : Form
    {
        OdbcConnection sqlCon = null;

        public string stDate;
        public string endDate;
        public string stage = string.Empty;
        public string userId = string.Empty;

        public Credentials crd = new Credentials();

        public frmUserReport(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void populateUserType()
        {
            DataSet ds = new DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();

            string sql = "select role_id, role_description from ac_role where role_id = 1 or role_id = 3 or role_id = 5 or role_id = 7 or role_id = 9 or role_id = 10";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "role_description";
                comboBox1.ValueMember = "role_id";
            }

        }

        private void populateUserId()
        {
            DataSet ds = new DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();

            string sql = "select user_id, role_id from ac_user_role_map a where role_id = '" + comboBox1.SelectedValue.ToString() + "'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                deComboBox2.DataSource = dt;
                deComboBox2.DisplayMember = "user_id";
                deComboBox2.ValueMember = "role_id";
            }
            else
            {
                deComboBox2.DataSource = null;
            }
        }

        private void frmUserReport_Load(object sender, EventArgs e)
        {
            populateUserType();

            //populateAllUserType();

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

            comboBox1.SelectedIndex = 0;
            comboBox1.Select();
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            populateUserId();
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

        private void init()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntries(userId);

            grdStatus.DataSource = Dt;

            FormatDataGridView();

            this.grdStatus.Refresh();

        }
        public System.Data.DataTable _GetEntries(string userId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct date_format(a.created_dttm,'%Y-%m-%d') as 'Entry Date',a.created_by as 'Entry User',b.bundle_name as 'Bundle Name',a.filename as 'Filename' from metadata_entry a,bundle_master b " +
                         "where date_format(a.created_dttm,'%Y-%m-%d') >= '" + stDate + "' and date_format(a.created_dttm,'%Y-%m-%d') <= '" + endDate + "' and " +
                         " a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and a.created_by = '" + userId + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void initScan()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesScan(userId);

            grdStatus.DataSource = Dt;

            FormatDataGridView();

            this.grdStatus.Refresh();
        }
        public System.Data.DataTable _GetEntriesScan(string userId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct date_format(a.scanned_dttm,'%Y-%m-%d') as 'Scanned Date',a.scanned_user as 'Scanned User',b.bundle_name as 'Bundle Name',a.policy_number as 'Filename' from transaction_log a, bundle_master b " +
                         " where date_format(a.scanned_dttm,'%Y-%m-%d') >= '" + stDate + "' and date_format(a.scanned_dttm,'%Y-%m-%d') <= '" + endDate + "' and " +
                         " a.proj_key = b.proj_code and a.batch_key = b.bundle_key and a.scanned_user = '" + userId + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void initQC()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesQC(userId);

            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();

        }
        public System.Data.DataTable _GetEntriesQC(string userId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct date_format(a.qc_dttm,'%Y-%m-%d') as 'QC Date',a.qc_user as 'QC User',b.bundle_name as 'Bundle Name',a.policy_number as 'Filename' from transaction_log a, bundle_master b " +
                         " where date_format(a.qc_dttm,'%Y-%m-%d') >= '" + stDate + "' and date_format(a.qc_dttm,'%Y-%m-%d') <= '" + endDate + "' and " +
                          " a.proj_key = b.proj_code and a.batch_key = b.bundle_key and a.qc_user = '" + userId + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void initIndex()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesIndex(userId);


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();

        }
        public System.Data.DataTable _GetEntriesIndex(string userId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct date_format(a.index_dttm,'%Y-%m-%d') as 'DOC Type Association Date',a.index_user as 'DOC Type Association User',b.bundle_name as 'Bundle Name',a.policy_number as 'Filename' from transaction_log a, bundle_master b " +
                         " where date_format(a.index_dttm,'%Y-%m-%d') >= '" + stDate + "' and date_format(a.index_dttm,'%Y-%m-%d') <= '" + endDate + "' and " +
                         " a.proj_key = b.proj_code and a.batch_key = b.bundle_key and a.index_user = '" + userId + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void initFqc()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesFqc(userId);


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();

        }
        public System.Data.DataTable _GetEntriesFqc(string userId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct date_format(a.fqc_dttm,'%Y-%m-%d') as 'Fqc Date',a.fqc_user as 'Fqc User',b.bundle_name as 'Bundle Name',a.policy_number  as 'Filename' from transaction_log a, bundle_master b " +
                         " where date_format(a.fqc_dttm,'%Y-%m-%d') >= '" + stDate + "' and date_format(a.fqc_dttm,'%Y-%m-%d') <= '" + endDate + "' and " +
                         " a.proj_key = b.proj_code and a.batch_key = b.bundle_key and a.fqc_user = '" + userId + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void initAudit()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesAudit(userId);


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();

        }
        public System.Data.DataTable _GetEntriesAudit(string userId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct date_format(a.created_dttm,'%Y-%m-%d') as 'Audit Date',a.created_by as 'Audit User',b.bundle_name as 'Bundle Name',a.policy_number  as 'Filename' from lic_qa_log a, bundle_master b " +
                         " where (date_format(a.created_dttm,'%Y-%m-%d') >= '" + stDate + "' and date_format(a.created_dttm,'%Y-%m-%d') <= '" + endDate + "') and (a.qa_status = 0 or a.qa_status = 1 or a.qa_status = 2) and " +
                         " a.proj_key = b.proj_code and a.batch_key = b.bundle_key and a.created_by = '" + userId + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            grdStatus.DataSource = null;

            stage = comboBox1.Text;
            userId = deComboBox2.Text;

            stDate = dateTimePicker1.Text;
            endDate = dateTimePicker2.Text;

            if (stage == "Metadata Entry")
            {
                if (userId != null || userId != "")
                {
                    init();
                }
            }
            else if (stage == "Scan")
            {
                if (userId != null || userId != "")
                {
                    initScan();
                }
            }
            else if (stage == "QC")
            {
                if (userId != null || userId != "")
                {
                    initQC();
                }
            }
            else if (stage == "DOC Type Association")
            {
                if (userId != null || userId != "")
                {
                    initIndex();
                }
            }
            else if (stage == "Fqc")
            {
                if (userId != null || userId != "")
                {
                    initFqc();
                }
            }
            else if (stage == "Audit")
            {
                if (userId != null || userId != "")
                {
                    initAudit();
                }
            }
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

                worksheet.Name = "User Wise Report";

                worksheet.Cells[1, 3] = "User Wise Report";
                Range range44 = worksheet.get_Range("C1");
                range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);

                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();


                worksheet.Cells[3, 1] = "User Report for : " + stage;
                Range range43 = worksheet.get_Range("A3");
                range43.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[4, 1] = "Date From : " + stDate + " - " + endDate;
                Range range33 = worksheet.get_Range("A4");
                range33.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[5, 1] = "User Id : " + userId;
                Range range34 = worksheet.get_Range("A5");
                range34.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                Range range = worksheet.get_Range("A3", "A5");
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);


                Range range1 = worksheet.get_Range("A7", "D7");
                range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                for (int i = 1; i < grdStatus.Columns.Count + 1; i++)
                {


                    Range range2 = worksheet.get_Range("A7", "D7");
                    range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                    range2.EntireRow.AutoFit();
                    range2.EntireColumn.AutoFit();
                    worksheet.Cells[7, i] = grdStatus.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < grdStatus.Rows.Count; i++)
                {
                    for (int j = 0; j < grdStatus.Columns.Count; j++)
                    {
                        Range range3 = worksheet.Cells;
                        //range3.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range3.EntireRow.AutoFit();
                        range3.EntireColumn.AutoFit();
                        worksheet.Cells[i + 8, j + 1] = grdStatus.Rows[i].Cells[j].Value.ToString();
                        worksheet.Cells[i + 8, j + 1].Borders.Color = ColorTranslator.ToOle(Color.Black);

                    }

                }

                string namexls = "User_Wise_Report_" + userId + ".xls";
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
