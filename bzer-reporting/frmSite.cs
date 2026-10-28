using System;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using NovaNet.Utils;

namespace Reporting
{
    public partial class frmSite : Form
    {

        OdbcConnection sqlCon = null;

        public string stDate;
        public string endDate;
        public string stage = string.Empty;

        public Credentials crd = new Credentials();

        public frmSite(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;

        }

        private void frmSite_Load(object sender, EventArgs e)
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

            deComboBox1.SelectedIndex = 0;
            deComboBox1.Select();
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            //init();
            stage = deComboBox1.Text;
            grdStatus.DataSource = null;
            dgvBundle.DataSource = null;
            //Data Entry
            if (stage == "Data Entry")
            {
                stDate = dateTimePicker1.Text;
                endDate = dateTimePicker2.Text;

                initDE();

            }
            //scan
            if (stage == "Scan")
            {
                stDate = dateTimePicker1.Text;
                endDate = dateTimePicker2.Text;

                initScan();
            }
            //qc
            if (stage == "QC")
            {
                stDate = dateTimePicker1.Text;
                endDate = dateTimePicker2.Text;

                initQC();
            }
            //index
            if (stage == "Doc Type Association")
            {
                stDate = dateTimePicker1.Text;
                endDate = dateTimePicker2.Text;

                initIndex();
            }
            //fqc
            if (stage == "FQC")
            {
                stDate = dateTimePicker1.Text;
                endDate = dateTimePicker2.Text;

                initFQC();
            }
            //submission
            if (stage == "Submission")
            {
                stDate = dateTimePicker1.Text;
                endDate = dateTimePicker2.Text;

                initSub();
            }
            //cert
            if (stage == "Certification")
            {
                stDate = dateTimePicker1.Text;
                endDate = dateTimePicker2.Text;

                initCert();
            }
        }

        public string _GetBundleCount(string getDate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select count(*) as 'Total Bundle' from bundle_master where date_format(created_dttm,'%Y-%m-%d') = '" + getDate + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        public string _GetEntryCount(string date)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select COUNT(*) from metadata_entry where date_format(created_dttm,'%Y-%m-%d') = '" + date + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }
        public string _GetScanCount(string date)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select COUNT(*) from image_master where date_format(created_dttm,'%Y-%m-%d') = '" + date + "'  ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        public string _GetImageCountQC(string projk, string batchK, string file)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select COUNT(*) from image_master where proj_key =  '" + projk + "' and batch_key = '" + batchK + "' and policy_number = '" + file + "'   ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }
        public string _GetImageCountIndex(string projk, string batchK, string file)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select COUNT(*) from image_master where proj_key =  '" + projk + "' and batch_key = '" + batchK + "' and policy_number = '" + file + "' and status <> 29  ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }
        public System.Data.DataTable _GetFileDetailsQC(string sdate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select proj_key, batch_key, policy_number from transaction_log where date_format(qc_dttm,'%Y-%m-%d') = '" + sdate + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetFileDetailsIndex(string sdate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select proj_key, batch_key, policy_number from transaction_log where date_format(index_dttm,'%Y-%m-%d') = '" + sdate + "'  ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetDECount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_code,bundle_key,filename from case_file_master where date_format(created_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(created_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }
        public System.Data.DataTable _GetDEBCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_code,bundle_key from case_file_master where date_format(created_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(created_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public System.Data.DataTable _GetScanCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key,policy_number from transaction_log where date_format(scanned_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(scanned_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }
        public System.Data.DataTable _GetScanBCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key from transaction_log where date_format(scanned_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(scanned_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public System.Data.DataTable _GetQCCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key,policy_number from transaction_log where date_format(qc_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(qc_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }
        public System.Data.DataTable _GetQCBCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key from transaction_log where date_format(qc_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(qc_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public System.Data.DataTable _GetIndexCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key,policy_number from transaction_log where date_format(index_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(index_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }
        public System.Data.DataTable _GetIndexBCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key from transaction_log where date_format(index_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(index_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public System.Data.DataTable _GetFQCCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key,policy_number from transaction_log where date_format(fqc_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(fqc_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }
        public System.Data.DataTable _GetFQCBCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key from transaction_log where date_format(fqc_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(fqc_dttm,'%Y-%m-%d') <= '" + end + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public System.Data.DataTable _GetCertCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key,policy_number from lic_qa_log where (date_format(created_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(created_dttm,'%Y-%m-%d') <= '" + end + "') or (date_format(modified_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(modified_dttm,'%Y-%m-%d') <= '" + end + "') and (qa_status = 0 or qa_status = 1 or qa_status = 2)";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }
        public System.Data.DataTable _GetCertBCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key from lic_qa_log where (date_format(created_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(created_dttm,'%Y-%m-%d') <= '" + end + "') or (date_format(modified_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(modified_dttm,'%Y-%m-%d') <= '" + end + "') and (qa_status = 0 or qa_status = 1 or qa_status = 2)";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public System.Data.DataTable _GetSubCount(string pk, string bk)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select Count(*) from case_file_master where proj_code = '" + pk + "' and bundle_key = '" + bk + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }
        public System.Data.DataTable _GetSubBCount(string st, string end)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_key,batch_key from tbl_submission_log where (date_format(created_dttm,'%Y-%m-%d') >= '" + st + "' and date_format(created_dttm,'%Y-%m-%d') <= '" + end + "') ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt;
        }

        public void initDE()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt.Columns.Add("Number of Bundles");
            Dt.Columns.Add("Number of Files");

            string couB = _GetDEBCount(stDate, endDate).Rows.Count.ToString();
            string cou = _GetDECount(stDate, endDate).Rows.Count.ToString();

            Dt.Rows.Add(couB, cou);//no of files


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();

        }

        public void initScan()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt.Columns.Add("Number of Bundles");
            Dt.Columns.Add("Number of Files");

            string couB = _GetScanBCount(stDate, endDate).Rows.Count.ToString();
            string cou = _GetScanCount(stDate, endDate).Rows.Count.ToString();

            Dt.Rows.Add(couB, cou);//no of files


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();


        }

        public void initQC()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt.Columns.Add("Number of Bundles");
            Dt.Columns.Add("Number of Files");

            string couB = _GetQCBCount(stDate, endDate).Rows.Count.ToString();
            string cou = _GetQCCount(stDate, endDate).Rows.Count.ToString();

            Dt.Rows.Add(couB, cou);//no of files


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();

        }

        public void initIndex()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt.Columns.Add("Number of Bundles");
            Dt.Columns.Add("Number of Files");

            string couB = _GetIndexBCount(stDate, endDate).Rows.Count.ToString();
            string cou = _GetIndexCount(stDate, endDate).Rows.Count.ToString();

            Dt.Rows.Add(couB, cou);//no of files


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();


        }

        public void initFQC()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt.Columns.Add("Number of Bundles");
            Dt.Columns.Add("Number of Files");

            string couB = _GetFQCBCount(stDate, endDate).Rows.Count.ToString();
            string cou = _GetFQCCount(stDate, endDate).Rows.Count.ToString();

            Dt.Rows.Add(couB, cou);//no of files


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();


        }

        public void initCert()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt.Columns.Add("Number of Bundles");
            Dt.Columns.Add("Number of Files");

            string couB = _GetCertBCount(stDate, endDate).Rows.Count.ToString();
            string cou = _GetCertCount(stDate, endDate).Rows.Count.ToString();

            Dt.Rows.Add(couB, cou);//no of files


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();


        }

        public void initSub()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt.Columns.Add("Number of Bundles");
            Dt.Columns.Add("Number of Files");

            string couB = _GetSubBCount(stDate, endDate).Rows.Count.ToString();

            int couFile = 0;

            for (int i = 0; i < _GetSubBCount(stDate, endDate).Rows.Count; i++)
            {
                string pk = _GetSubBCount(stDate, endDate).Rows[i][0].ToString();
                string bk = _GetSubBCount(stDate, endDate).Rows[i][1].ToString();

                couFile = couFile + Convert.ToInt32(_GetSubCount(pk, bk).Rows[0][0].ToString());
            }

            //string cou = _GetCertCount(stDate, endDate).Rows.Count.ToString();



            Dt.Rows.Add(couB, couFile);//no of files


            grdStatus.DataSource = Dt;


            FormatDataGridView();

            this.grdStatus.Refresh();


        }

        private void FormatDataGridView()
        {
            //Format the Data Grid View
            //grdStatus.Columns[0].Visible = false;
            //grdStatus.Columns[1].Visible = false;
            //grdStatus.Columns[2].Visible = false;
            //Format Colors


            //Set Autosize on for all the columns
            for (int i = 0; i < grdStatus.Columns.Count; i++)
            {
                grdStatus.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


        }

        private void FormatDataGridViewBundle()
        {
            //Format the Data Grid View
            //grdStatus.Columns[0].Visible = false;
            dgvBundle.Columns[0].Visible = false;
            dgvBundle.Columns[1].Visible = false;
            //Format Colors


            //Set Autosize on for all the columns
            for (int i = 0; i < dgvBundle.Columns.Count; i++)
            {
                dgvBundle.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


        }

        public System.Data.DataTable _GetBundleDetails(string sdate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select proj_code, bundle_key, bundle_no as 'Bundle Number', bundle_name as 'Bundle Name', establishment as 'Establishment', date_format(creation_date,'%Y-%m-%d') as 'Creation Date', date_format(handover_date,'%Y-%m-%d') as 'Handover Date' from bundle_master where date_format(created_dttm,'%Y-%m-%d') = '" + sdate + "'  ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetFileDetails(string projK, string bundleK)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select proj_code, bundle_key, case_file_no as 'Case File No', case_status as 'Case Status', case_nature as 'Case Nature', case_type as 'Case Type', case_year as 'Case Year', filename as 'Filename' from case_file_master where proj_code = '" + projK + "' and bundle_key = '" + bundleK + "'  ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetImageCount(string projK, string bundleK, string pol)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select Count(*) from image_master where proj_key = '" + projK + "' and batch_key = '" + bundleK + "' and policy_number = '" + pol + "' and status <> 29 ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetImageAddedCount(string projK, string bundleK, string pol)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select Count(*) from image_master where proj_key = '" + projK + "' and batch_key = '" + bundleK + "' and policy_number = '" + pol + "' and date_format(created_dttm,'%Y-%m-%d') >= '" + stDate + "' and date_format(created_dttm,'%Y-%m-%d') <= '" + endDate + "' and status <> 29 ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetImageRemovedCount(string projK, string bundleK, string pol)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select Count(*) from image_master where proj_key = '" + projK + "' and batch_key = '" + bundleK + "' and policy_number = '" + pol + "' and date_format(modified_dttm,'%Y-%m-%d') >= '" + stDate + "' and date_format(modified_dttm,'%Y-%m-%d') <= '" + endDate + "' and status = 29 ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        private void initDEFile()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt = _GetDEDetails(stDate, endDate);

            Dt.Columns.Add("Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string pk = Dt.Rows[i][0].ToString();
                string bk = Dt.Rows[i][1].ToString();
                string fileno = Dt.Rows[i][4].ToString();

                Dt.Rows[i]["Image Count"] = _GetImageCount(pk, bk, fileno).Rows[0][0].ToString();
            }

            dgvBundle.DataSource = Dt;


            FormatDataGridViewBundle();

            this.dgvBundle.Refresh();

        }
        private void initScanFile()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt = _GetScanDetails(stDate, endDate);

            Dt.Columns.Add("Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string pk = Dt.Rows[i][0].ToString();
                string bk = Dt.Rows[i][1].ToString();
                string fileno = Dt.Rows[i][4].ToString();

                Dt.Rows[i]["Image Count"] = _GetImageCount(pk, bk, fileno).Rows[0][0].ToString();
            }

            dgvBundle.DataSource = Dt;


            FormatDataGridViewBundle();

            this.dgvBundle.Refresh();

        }
        private void initQCFile()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt = _GetQCDetails(stDate, endDate);

            Dt.Columns.Add("Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string pk = Dt.Rows[i][0].ToString();
                string bk = Dt.Rows[i][1].ToString();
                string fileno = Dt.Rows[i][4].ToString();

                Dt.Rows[i]["Image Count"] = _GetImageCount(pk, bk, fileno).Rows[0][0].ToString();
            }

            dgvBundle.DataSource = Dt;


            FormatDataGridViewBundle();

            this.dgvBundle.Refresh();

        }
        private void initIndexFile()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt = _GetIndexDetails(stDate, endDate);

            Dt.Columns.Add("Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string pk = Dt.Rows[i][0].ToString();
                string bk = Dt.Rows[i][1].ToString();
                string fileno = Dt.Rows[i][4].ToString();

                Dt.Rows[i]["Image Count"] = _GetImageCount(pk, bk, fileno).Rows[0][0].ToString();
            }

            dgvBundle.DataSource = Dt;


            FormatDataGridViewBundle();

            this.dgvBundle.Refresh();

        }
        private void initFQCFile()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt = _GetFQCDetails(stDate, endDate);

            Dt.Columns.Add("Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string pk = Dt.Rows[i][0].ToString();
                string bk = Dt.Rows[i][1].ToString();
                string fileno = Dt.Rows[i][4].ToString();

                Dt.Rows[i]["Image Count"] = _GetImageCount(pk, bk, fileno).Rows[0][0].ToString();
            }

            dgvBundle.DataSource = Dt;


            FormatDataGridViewBundle();

            this.dgvBundle.Refresh();

        }
        private void initCertFile()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt = _GetCertDetails(stDate, endDate);

            Dt.Columns.Add("Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string pk = Dt.Rows[i][0].ToString();
                string bk = Dt.Rows[i][1].ToString();
                string fileno = Dt.Rows[i][4].ToString();

                Dt.Rows[i]["Image Count"] = _GetImageCount(pk, bk, fileno).Rows[0][0].ToString();
            }

            dgvBundle.DataSource = Dt;


            FormatDataGridViewBundle();

            this.dgvBundle.Refresh();

        }
        private void initSubFile()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();

            Dt = _GetSubDetails(stDate, endDate);


            Dt.Columns.Add("Image Count");
            Dt.Columns.Add("Increasing / Decreasing Image Count");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string pk = Dt.Rows[i][0].ToString();
                string bk = Dt.Rows[i][1].ToString();
                string fileno = Dt.Rows[i][4].ToString();

                Dt.Rows[i]["Image Count"] = _GetImageCount(pk, bk, fileno).Rows[0][0].ToString();

                int add = Convert.ToInt32(_GetImageAddedCount(pk, bk, fileno).Rows[0][0].ToString());
                int removed = Convert.ToInt32(_GetImageRemovedCount(pk, bk, fileno).Rows[0][0].ToString());

                int delta = add - removed;

                Dt.Rows[i]["Increasing / Decreasing Image Count"] = delta;
            }

            dgvBundle.DataSource = Dt;


            FormatDataGridViewBundle();

            this.dgvBundle.Refresh();

        }
        public System.Data.DataTable _GetDEDetails(string sdate, string edate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',b.filename as 'File Name' from bundle_master a,case_file_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and date_format(b.created_dttm,'%Y-%m-%d') >= '" + sdate + "' and date_format(b.created_dttm,'%Y-%m-%d') <= '" + edate + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetScanDetails(string sdate, string edate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',b.policy_number as 'File Name' from bundle_master a,transaction_log b where a.proj_code = b.proj_key and a.bundle_key = b.batch_key and date_format(b.scanned_dttm,'%Y-%m-%d') >= '" + sdate + "' and date_format(b.scanned_dttm,'%Y-%m-%d') <= '" + edate + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetQCDetails(string sdate, string edate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',b.policy_number as 'File Name' from bundle_master a,transaction_log b where a.proj_code = b.proj_key and a.bundle_key = b.batch_key and date_format(b.qc_dttm,'%Y-%m-%d') >= '" + sdate + "' and date_format(b.qc_dttm,'%Y-%m-%d') <= '" + edate + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetIndexDetails(string sdate, string edate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',b.policy_number as 'File Name' from bundle_master a,transaction_log b where a.proj_code = b.proj_key and a.bundle_key = b.batch_key and date_format(b.index_dttm,'%Y-%m-%d') >= '" + sdate + "' and date_format(b.index_dttm,'%Y-%m-%d') <= '" + edate + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetFQCDetails(string sdate, string edate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',b.policy_number as 'File Name' from bundle_master a,transaction_log b where a.proj_code = b.proj_key and a.bundle_key = b.batch_key and date_format(b.fqc_dttm,'%Y-%m-%d') >= '" + sdate + "' and date_format(b.fqc_dttm,'%Y-%m-%d') <= '" + edate + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetCertDetails(string sdate, string edate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',b.policy_number as 'File Name' from bundle_master a,lic_qa_log b where a.proj_code = b.proj_key and a.bundle_key = b.batch_key and (date_format(b.created_dttm,'%Y-%m-%d') >= '" + sdate + "' and date_format(b.created_dttm,'%Y-%m-%d') <= '" + edate + "') ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public System.Data.DataTable _GetSubDetails(string sdate, string edate)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',c.filename as 'File Name' from bundle_master a,tbl_submission_log b,case_file_master c where a.proj_code = b.proj_key and a.bundle_key = b.batch_key and date_format(b.created_dttm,'%Y-%m-%d') >= '" + sdate + "' and date_format(b.created_dttm,'%Y-%m-%d') <= '" + edate + "' and b.proj_key = c.proj_code and b.batch_key = c.bundle_key ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void grdStatus_DoubleClick(object sender, EventArgs e)
        {
            if (grdStatus.SelectedRows.Count > 0)
            {
                //string stDt = grdStatus.SelectedRows[0].Cells[0].Value.ToString();
                ////string endDt = grdStatus.SelectedRows[0].Cells[2].Value.ToString();

                //if(stDt != null)
                //{
                //    initBundle(stDt);
                //}
                dgvBundle.DataSource = null;

                if (stage == "Data Entry")
                {
                    initDEFile();
                }
                if (stage == "Scan")
                {
                    initScanFile();
                }
                if (stage == "QC")
                {
                    initQCFile();
                }
                if (stage == "Doc Type Association")
                {
                    initIndexFile();
                }
                if (stage == "FQC")
                {
                    initFQCFile();
                }
                if (stage == "Submission")
                {
                    initSubFile();
                }
                if (stage == "Certification")
                {
                    initCertFile();
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

        private void dgvBundle_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgvBundle.Rows.Count > 0)
                {
                    contextMenuStrip1.Show(Cursor.Position);
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

                worksheet.Name = "Site Report";

                worksheet.Cells[1, 3] = "Site Report";
                Range range44 = worksheet.get_Range("C1");
                range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);

                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();


                worksheet.Cells[3, 1] = "Report for : " + stage;
                Range range43 = worksheet.get_Range("A3");
                range43.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[4, 1] = "Date From : " + stDate + " - " + endDate;
                Range range33 = worksheet.get_Range("A4");
                range33.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                Range range = worksheet.get_Range("A3", "A4");
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);


                Range range1 = worksheet.get_Range("A6", "B6");
                range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                for (int i = 1; i < grdStatus.Columns.Count + 1; i++)
                {


                    Range range2 = worksheet.get_Range("A6", "B6");
                    range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                    range2.EntireRow.AutoFit();
                    range2.EntireColumn.AutoFit();
                    worksheet.Cells[6, i] = grdStatus.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < grdStatus.Rows.Count; i++)
                {
                    for (int j = 0; j < grdStatus.Columns.Count; j++)
                    {
                        Range range3 = worksheet.Cells;
                        //range3.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range3.EntireRow.AutoFit();
                        range3.EntireColumn.AutoFit();
                        worksheet.Cells[i + 7, j + 1] = grdStatus.Rows[i].Cells[j].Value.ToString();
                        worksheet.Cells[i + 7, j + 1].Borders.Color = ColorTranslator.ToOle(Color.Black);

                    }

                }

                string namexls = "Site_Summary_Report" + ".xls";
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvBundle.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                app.Visible = false;

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];


                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

                worksheet.Name = "Site Report";

                worksheet.Cells[1, 3] = "Site Report";
                Range range44 = worksheet.get_Range("C1");
                range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);

                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();


                worksheet.Cells[3, 1] = "Report for : " + stage;
                Range range43 = worksheet.get_Range("A3");
                range43.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[4, 1] = "Date From : " + stDate + " - " + endDate;
                Range range33 = worksheet.get_Range("A4");
                range33.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                Range range = worksheet.get_Range("A3", "A4");
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);



                if (stage != "Submission")
                {
                    Range range1 = worksheet.get_Range("A6", "D6");
                    range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                    for (int i = 3; i < dgvBundle.Columns.Count + 1; i++)
                    {


                        Range range2 = worksheet.get_Range("A6", "D6");
                        range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range2.EntireRow.AutoFit();
                        range2.EntireColumn.AutoFit();
                        worksheet.Cells[6, i - 2] = dgvBundle.Columns[i - 1].HeaderText;
                    }
                }
                else
                {
                    Range range1 = worksheet.get_Range("A6", "E6");
                    range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                    for (int i = 3; i < dgvBundle.Columns.Count + 1; i++)
                    {


                        Range range2 = worksheet.get_Range("A6", "E6");
                        range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range2.EntireRow.AutoFit();
                        range2.EntireColumn.AutoFit();
                        worksheet.Cells[6, i - 2] = dgvBundle.Columns[i - 1].HeaderText;
                    }
                }


                for (int i = 0; i < dgvBundle.Rows.Count; i++)
                {
                    for (int j = 2; j < dgvBundle.Columns.Count; j++)
                    {
                        Range range3 = worksheet.Cells;
                        //range3.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range3.EntireRow.AutoFit();
                        range3.EntireColumn.AutoFit();
                        worksheet.Cells[i + 7, j - 2 + 1] = dgvBundle.Rows[i].Cells[j].Value.ToString();
                        worksheet.Cells[i + 7, j - 2 + 1].Borders.Color = ColorTranslator.ToOle(Color.Black);

                    }

                }

                string namexls = "Site_Summary_Report_" + stage + ".xls";
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
