using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using NovaNet.Utils;
using nControls;
using NovaNet.wfe;
using System.Runtime.Remoting.Messaging;

namespace Reporting
{
    public partial class frmUAT : Form
    {
        OdbcConnection sqlCon = null;

        public string stDate;
        public string endDate;
        public string stage = string.Empty;
        public string userId = string.Empty;
        int i;
        int j;

        public Credentials crd = new Credentials();

        public frmUAT(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void populateUserType()
        {
            DataSet ds = new DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();

            string sql = "select role_id, role_description from ac_role where role_id = 7 ";

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
                deComboBox1.DataSource = dt;
                deComboBox1.DisplayMember = "user_id";
                deComboBox1.ValueMember = "role_id";
            }
            else
            {
                deComboBox1.DataSource = null;
            }
        }

        private void frmUAT_Load(object sender, EventArgs e)
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

            for (int i = 0; i < grdStatus.Columns.Count; i++)
            {
                grdStatus.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


        }
        public System.Data.DataTable _GetEntriesAudit(string userId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = $@"select distinct b.proj_code,b.bundle_key, 
                         date_format(a.created_dttm,'%Y-%m-%d') as 'Audit Date',
                         a.created_by as 'Audit User',
                         b.bundle_name as 'Batch Name',
                         count(a.policy_number) as 'Khatians',
                         (select count(im.policy_number) from image_master im where b.proj_code = im.proj_key 
                         and b.bundle_key = im.batch_key 
                         and im.status <> {Convert.ChangeType(eSTATES.PAGE_DELETED, eSTATES.PAGE_DELETED.GetTypeCode())}) 
                         as 'Image Count'
                         from lic_qa_log a, 


















b
                         where (date_format(a.created_dttm,'%Y-%m-%d') >= '{ stDate }' 
                         and date_format(a.created_dttm,'%Y-%m-%d') <= '{ endDate }') 
                         and (a.qa_status = 0 or a.qa_status = 1 or a.qa_status = 2) and
                         a.proj_key = b.proj_code and (b.status = 7 or b.status = 8) 
                         and a.batch_key = b.bundle_key and a.created_by = '{ userId }'
                         group by b.proj_code, b.bundle_key
                         order by 'Audit Date'";
            
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
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

        private void initAudit()
        {
            //DataSet ds = new DataSet();
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesAudit(userId);
            System.Data.DataTable DtTemp = new System.Data.DataTable();
            
            DtTemp.Columns.Add("Audit Date");
            DtTemp.Columns.Add("Audit User");
            DtTemp.Columns.Add("Batch Names");
            DtTemp.Columns.Add("Khatians");
            DtTemp.Columns.Add("Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                DtTemp.Rows.Add(Dt.Rows[i]["Audit Date"].ToString(), Dt.Rows[i]["Audit User"].ToString(), Dt.Rows[i]["Batch Name"].ToString(), Dt.Rows[i]["Khatians"].ToString(), Dt.Rows[i]["Image Count"].ToString());
            }
            
            grdStatus.DataSource = DtTemp;
            if (DtTemp.Rows.Count > 0)
            {
                deButton20.Enabled = true;
            }
            else
            {
                deButton20.Enabled = false;
            }

            FormatDataGridView();

            this.grdStatus.Refresh();

        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            grdStatus.DataSource = null;

            stage = comboBox1.Text;
            userId = deComboBox1.Text;

            stDate = dateTimePicker1.Text;
            endDate = dateTimePicker2.Text;

            if (stage == "Audit")
            {
                if (userId != null || userId != "")
                {
                    initAudit();
                }
            }
        }

        private void deButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deButton20_Click(object sender, EventArgs e)
        {
            deButton20.Enabled = false;
            try
            {
                if (grdStatus.Rows.Count > 0)
                {

                    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                    app.Visible = false;

                    worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];


                    worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

                    if (comboBox1.Text.Trim() == "Audit")
                    {
                        worksheet.Name = "UAT Certificate";

                        worksheet.Cells[1, 4] = "UAT Certificate";
                        Range range44 = worksheet.get_Range("D1");
                        range44.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray);

                        worksheet.Rows.AutoFit();
                        worksheet.Columns.AutoFit();

                        worksheet.Cells[2, 2] = "Customer Acceptance Certificate";
                        Range range4 = worksheet.get_Range("B2");
                        range4.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                        worksheet.Rows.AutoFit();
                        worksheet.Columns.AutoFit();

                        worksheet.Cells[3, 2] = "Digitization of Khatians @ WBLR";
                        Range range5 = worksheet.get_Range("B3");
                        range5.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                        worksheet.Rows.AutoFit();
                        worksheet.Columns.AutoFit();

                        worksheet.Cells[4, 2] = "Date Range : " + stDate + " - " + endDate;
                        Range range6 = worksheet.get_Range("B4");
                        range6.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        worksheet.Rows.AutoFit();
                        worksheet.Columns.AutoFit();

                        worksheet.Cells[5, 2] = "User Id : " + userId;
                        Range range7 = worksheet.get_Range("B5");
                        range7.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        worksheet.Rows.AutoFit();
                        worksheet.Columns.AutoFit();

                        Range range = worksheet.get_Range("B2", "B5");
                        range.Borders.Color = ColorTranslator.ToOle(Color.Black);


                        Range range1 = worksheet.get_Range("B7", "F7");
                        range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                        for (int i = 1; i < grdStatus.Columns.Count + 1; i++)
                        {


                            Range range2 = worksheet.get_Range("B7", "F7");
                            range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                            range2.EntireRow.AutoFit();
                            range2.EntireColumn.AutoFit();
                            worksheet.Cells[7, i + 1] = grdStatus.Columns[i - 1].HeaderText;
                            worksheet.Cells[7, i + 1].EntireRow.Font.Bold = true;
                        }
                        worksheet.Cells[9 + grdStatus.Rows.Count, 4] = "Total";
                        worksheet.Cells[9 + grdStatus.Rows.Count, 4].Font.Bold = true;

                        int imgcount = 0;
                        int filecount = 0;
                        // int bundleCount = 0;
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
                            // bundleCount = grdStatus.Rows.Count;
                            // filecount = filecount + Convert.ToInt32(grdStatus.Rows[i].Cells[3].Value.ToString());
                            filecount = grdStatus.Rows.Count;
                            imgcount = imgcount + Convert.ToInt32(grdStatus.Rows[i].Cells[4].Value.ToString());
                        }
                        worksheet.Cells[9 + grdStatus.Rows.Count, 5] = filecount.ToString();
                        worksheet.Cells[9 + grdStatus.Rows.Count, 6] = imgcount.ToString();
                        // worksheet.Cells[7 + grdStatus.Rows.Count, 2].Borders.Color = ColorTranslator.ToOle(Color.Black);
                        worksheet.Cells[9 + grdStatus.Rows.Count, 4].Borders.Color = ColorTranslator.ToOle(Color.Black);
                        worksheet.Cells[9 + grdStatus.Rows.Count, 5].Borders.Color = ColorTranslator.ToOle(Color.Black);
                        worksheet.Cells[9 + grdStatus.Rows.Count, 6].Borders.Color = ColorTranslator.ToOle(Color.Black);

                        worksheet.Cells[11 + grdStatus.Rows.Count, 2] = "The above files have been checked by WBLR officials and found satisfactory";

                        worksheet.Cells[16 + grdStatus.Rows.Count, 2] = "Signature of WBLR Calcutta";
                        worksheet.Cells[16 + grdStatus.Rows.Count, 6] = "Signature of NTPL Supervisor";

                        string namexls = $@"UAT_Report_{userId}_{stDate}_{endDate}.xls";
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
            catch (Exception ex) {
                var userResponse = MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (userResponse == DialogResult.Cancel) { this.Close(); }
            }
            deButton20.Enabled = true;
        }
    }
}
