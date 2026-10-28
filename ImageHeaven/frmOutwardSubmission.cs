using System;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;
using bzer_data_entry;
using LItems;
using NovaNet.Utils;

namespace ImageHeaven
{
    public partial class frmOutwardSubmission : Form
    {
        wfeProject tmpProj = null;
        wfePolicy tmpPolicy = null;
        OdbcConnection sqlCon = new OdbcConnection();
        OdbcTransaction txn = null;
        Credentials crd = new Credentials();
        string RunNo = string.Empty;
        private OdbcDataAdapter sqlAdap = null;
        byte[] tmpWrite;
        System.IO.MemoryStream stateLog;
        public string err = null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        public static string proj = string.Empty;

        public frmOutwardSubmission(OdbcConnection prmCon, Credentials prmCrd)
        {
            InitializeComponent();
            sqlCon = prmCon;
            crd = prmCrd;
        }

        public void init()
        {
            //dtpDateFrom.CustomFormat = " ";
            //dtpDateFrom.Text = string.Empty;
            //dtpDateTo.CustomFormat = " ";
            //dtpDateTo.Text = string.Empty;
            cmbBundle.Enabled = false;
            cmbBundle.Text = string.Empty;
            dtGrdVol.DataSource = null;
            dtpOutDate.CustomFormat = " ";
            dtpOutDate.Enabled = false;
            dtpOutDate.Text = string.Empty;
            btnSave.Enabled = false;

            populateBundle();
        }

        private void frmOutwardSubmission_Load(object sender, EventArgs e)
        {
            init();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void populateBundle()
        {
            DataTable dt = new DataTable();

            string sql = "select bundle_key,bundle_code from bundle_master where ISNULL(outward_date) and status >= 7 ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                cmbBundle.DataSource = dt;
                cmbBundle.DisplayMember = "bundle_code";
                cmbBundle.ValueMember = "bundle_key";
                cmbBundle.Enabled = true;
            }
            else
            {
                cmbBundle.DataSource = null;
                cmbBundle.Text = string.Empty;
                cmbBundle.Enabled = false;
                dtGrdVol.DataSource = null;
                dtpOutDate.Enabled = false;
                dtpOutDate.CustomFormat = " ";
                dtpOutDate.Text = string.Empty;
                btnSave.Enabled = false;
                return;
            }
        }

        public void populateGrid()
        {
            ISummaryFormBuilder summaryFormBuilder = new SummaryFormBuilder();
            proj = getProjKey(cmbBundle.Text);
            var ds = summaryFormBuilder.GetSummaryUploadData(sqlCon, proj, cmbBundle.SelectedValue.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                var colSlNo = "Sl No";
                var imgCount = "Image Count";
                dt.Columns.Add(colSlNo);
                dt.Columns.Add(imgCount);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][colSlNo] = i + 1;
                    dt.Rows[i][imgCount] = getimageCount(dt.Rows[i]["filename"].ToString(), proj, cmbBundle.SelectedValue.ToString());
                }

                dtGrdVol.DataSource = dt;
                FormatDataGridView();
                //out date enable
                dtpOutDate.Enabled = true;
            }
            else
            {
                dtpOutDate.Enabled = false;
            }
            dtpOutDate.CustomFormat = " ";
            dtpOutDate.Text = string.Empty;
        }

        private void FormatDataGridView()
        {

            //Set Autosize on for all the columns
            for (int i = 0; i < dtGrdVol.Columns.Count; i++)
            {
                dtGrdVol.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

        }

        public string getProjKey(string batch)
        {
            DataTable dt = new DataTable();

            string sql = "select proj_code from bundle_master where bundle_code = '" + batch + "' and bundle_key = '" + cmbBundle.SelectedValue.ToString() + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt.Rows[0][0].ToString();
        }

        public string getimageCount(string filename, string proj, string batch)
        {
            DataTable dt = new DataTable();

            string sql = "select count(*) from image_master where policy_number = '" + filename + "' and status <> 29";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);

            return dt.Rows[0][0].ToString();
        }

        private void cmbBundle_Leave(object sender, EventArgs e)
        {
            populateGrid();
        }

        private void cmbBundle_MouseLeave(object sender, EventArgs e)
        {
            cmbBundle_Leave(sender, e);
        }

        private void dtpOutDate_ValueChanged(object sender, EventArgs e)
        {
            dtpOutDate.CustomFormat = "yyyy-MM-dd";
        }

        private void dtpOutDate_Leave(object sender, EventArgs e)
        {
            if ((dtpOutDate.Text != null || dtpOutDate.Text != string.Empty))
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                MessageBox.Show(this, "Please select correct outward date", "Warning !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpOutDate.Focus();
                return;
            }
        }

        private void dtpOutDate_MouseLeave(object sender, EventArgs e)
        {
            dtpOutDate_Leave(sender, e);
        }

        public bool updateCaseFile(string filename, string imgCount)
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateCaseFile(proj, cmbBundle.SelectedValue.ToString(), filename, imgCount);

                ret = true;
            }
            return ret;
        }

        public bool updateBundle()
        {
            bool ret = false;
            if (ret == false)
            {
                _UpdateBundle(proj, cmbBundle.SelectedValue.ToString());

                ret = true;
            }
            return ret;
        }

        public bool _UpdateCaseFile(string projKey, string bundleKey, string filename, string imgCount)
        {
            string sqlStr = null;

            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;

            sqlStr = "UPDATE case_file_master SET outward_image_count = '" + imgCount + "' WHERE proj_code = '" + projKey + "' AND bundle_key = '" + bundleKey + "' and filename = '" + filename + "' ";

            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon, txn);
            if (cmd.ExecuteNonQuery() >= 0)
            {
                retVal = true;
            }

            return retVal;
        }

        public bool _UpdateBundle(string projKey, string bundleKey)
        {
            string sqlStr = null;

            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;

            sqlStr = "UPDATE bundle_master SET outward_date = '" + dtpOutDate.Text + "',outward_by = '" + crd.created_by + "' WHERE proj_code = '" + projKey + "' AND bundle_key = '" + bundleKey + "' ";

            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon, txn);
            if (cmd.ExecuteNonQuery() >= 0)
            {
                retVal = true;
            }

            return retVal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            OdbcTransaction sqlTrans = null;

            if (sqlCon.State == ConnectionState.Closed || sqlCon.State == ConnectionState.Broken)
            {
                sqlCon.Open();
            }

            DialogResult dr = MessageBox.Show(this, "Do you want to Save ? ", "B'Zer -  ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    for (int i = 0; i < dtGrdVol.Rows.Count; i++)
                    {
                        updateCaseFile(dtGrdVol.Rows[i].Cells[1].Value.ToString(), dtGrdVol.Rows[i].Cells[7].Value.ToString());
                    }
                    updateBundle();
                    if (sqlTrans == null)
                    {
                        sqlTrans = sqlCon.BeginTransaction();
                    }
                    sqlTrans.Commit();
                    sqlTrans = null;
                    MessageBox.Show(this, "Record Saved Successfully...", "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    populateBundle();

                }
                catch (Exception)
                {
                    MessageBox.Show(this, "Ooops!!! There is an Error - Record not Saved...", "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlTrans.Rollback();
                    sqlTrans = null;
                    return;
                }
                dtGrdVol.DataSource = null;
                dtpOutDate.Enabled = false;
                dtpOutDate.CustomFormat = " ";
                dtpOutDate.Text = string.Empty;
            }
            else
            {
                return;
            }
        }

    }
}
