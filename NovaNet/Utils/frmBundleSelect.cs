using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Windows.Forms;

namespace Utils
{
    public partial class frmBundleSelect : Form
    {
        NovaNet.Utils.dbCon dbcon;
        OdbcConnection sqlCon = null;
        eSTATES[] state;
        public static string projKey;
        public static string bundleKey;
        //Transaction
        OdbcTransaction txn;
        //The credentials with which the user has logged on...
        Credentials crd = new Credentials();
        //The control object which will act as the key to retrieving records
        Dictionary<int, string> bundle_list;


        public frmBundleSelect(eSTATES[] prmState, OdbcConnection prmCon, Credentials prmCrd)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            sqlCon = prmCon;
            this.Text = "B'Zer -  - Fetch Docket";
            state = prmState;

            crd = prmCrd;

        }

        private void frmBundleSelect_Load(object sender, EventArgs e)
        {
            populateProject();
            bundle_list = new Dictionary<int, string>();

        }
        private void populateProject()
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select proj_key, proj_code from project_master  ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbProject.DataSource = dt;
                cmbProject.DisplayMember = "proj_code";
                cmbProject.ValueMember = "proj_key";
                //TransactionManager tm = TransactionManager.GetInstance(sqlCon);

                populateBundle();
            }
            else
            {
                cmbProject.DataSource = null;
                // cmbProject.Text = "";
                MessageBox.Show("Add one project first...");
                this.Close();
            }


        }

        private DataTable populateBundle()
        {
            if (state[0] == eSTATES.METADATA_ENTRY || state[0] == eSTATES.BARCODE_ENTRY || state[0] == eSTATES.POLICY_CREATED || state[0] == eSTATES.POLICY_SCANNED || state[0] == eSTATES.POLICY_QC || state[0] == eSTATES.POLICY_INDEXED)
            {
                DataTable dt = new DataTable();
                string sql = string.Empty;
                switch (state[0])
                {
                    case eSTATES.METADATA_ENTRY:
                        sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '0' order by a.bundle_key";
                        break;
                    case eSTATES.BARCODE_ENTRY:
                        sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' order by a.bundle_key";
                        break;
                    case eSTATES.POLICY_CREATED:
                        sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '1' order by a.bundle_key";
                        break;

                    case eSTATES.POLICY_SCANNED:
                        sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '2' order by a.bundle_key";
                        break;

                    case eSTATES.POLICY_QC:
                        sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '2' order by a.bundle_key";
                        break;

                    case eSTATES.POLICY_INDEXED:
                        sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' order by a.bundle_key";
                        break;

                }
                OdbcCommand cmd = new OdbcCommand(sql, sqlCon, txn);
                OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
                odap.Fill(dt);
                return dt;
            }
            else if (state[0] == eSTATES.POLICY_QC || state[0] == eSTATES.POLICY_ON_HOLD)
            {
                DataTable dt = new DataTable();

                string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and a.status = '3' order by a.bundle_key";
                OdbcCommand cmd = new OdbcCommand(sql, sqlCon, txn);
                OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
                odap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    DataTable dt1 = new DataTable();
                    string sql1 = "select distinct a.bundle_key, a.bundle_code from bundle_master a, case_file_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key  and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and b.status = '3' order by a.bundle_key";
                    OdbcCommand cmd1 = new OdbcCommand(sql1, sqlCon, txn);
                    OdbcDataAdapter odap1 = new OdbcDataAdapter(cmd1);
                    odap1.Fill(dt1);

                    return dt1;
                }
            }
            else if (state[0] == eSTATES.POLICY_INDEXED || state[0] == eSTATES.POLICY_ON_HOLD)
            {
                DataTable dt = new DataTable();

                string sql = "select distinct a.bundle_key, a.bundle_code from bundle_master a, project_master b, case_file_master c  where a.proj_code = b.proj_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and c.proj_code = a.proj_code and c.bundle_key = a.bundle_key and (a.status = '4' or a.status = '5' or a.status = '6' or a.status = '7' or a.status = '8' or a.status = '30' or a.status = '31' or a.status = '37' or a.status = '40' or a.status = '77' or c.status = '4' or c.status = '5' or c.status = '30' or c.status = '31' or c.status = '37' or c.status = '40' or c.status = '77') order by a.bundle_key";
                OdbcCommand cmd = new OdbcCommand(sql, sqlCon, txn);
                OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
                odap.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    DataTable dt1 = new DataTable();
                    string sql1 = "select distinct a.bundle_key, a.bundle_code from bundle_master a, case_file_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and a.proj_code = '" + cmbProject.SelectedValue.ToString() + "' and (b.status = '4' or b.status = '5' or b.status = '30' or b.status = '31' or b.status = '37' or b.status = '40' or b.status = '77')";
                    OdbcCommand cmd1 = new OdbcCommand(sql1, sqlCon, txn);
                    OdbcDataAdapter odap1 = new OdbcDataAdapter(cmd1);
                    odap1.Fill(dt1);
                    return dt1;
                }
            }
            return new DataTable();
        }

        private void cmbProject_Leave(object sender, EventArgs e)
        {
            var dt = populateBundle();
            populateBundleCombo(dt);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            projKey = null;
            bundleKey = null;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (state[0] == eSTATES.METADATA_ENTRY)
            {


                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {
                    cmbBundle.DropDownStyle = ComboBoxStyle.DropDown;

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    cmbBundle.DropDownStyle = ComboBoxStyle.DropDown;
                    bundleKey = Convert.ToString(bundle_list.First(val => { return val.Value == cmbBundle.Text; }).Key);
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "B'Zer -  - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (state[0] == eSTATES.BARCODE_ENTRY)
            {


                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(bundle_list.First(val => { return val.Value == cmbBundle.Text; }).Key);
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "B'Zer -  - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (state[0] == eSTATES.POLICY_CREATED)
            {
                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(bundle_list.First(val => { return val.Value == cmbBundle.Text; }).Key);
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "B'Zer -  - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            if (state[0] == eSTATES.POLICY_SCANNED)
            {
                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(bundle_list.First(val => { return val.Value == cmbBundle.Text; }).Key);
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "B'Zer -  - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (state[0] == eSTATES.POLICY_QC || state[0] == eSTATES.POLICY_ON_HOLD)
            {
                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(bundle_list.First(val => { return val.Value == cmbBundle.Text; }).Key);
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "B'Zer -  - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (state[0] == eSTATES.POLICY_INDEXED || state[0] == eSTATES.POLICY_ON_HOLD)
            {
                if ((cmbProject.Text.ToString() != string.Empty) && (cmbBundle.Text.ToString() != string.Empty))
                {

                    projKey = Convert.ToString(cmbProject.SelectedValue.ToString());
                    bundleKey = Convert.ToString(bundle_list.First(val => { return val.Value == cmbBundle.Text; }).Key);
                    this.Close();

                }
                else
                {
                    MessageBox.Show(this, "Please select Project and proper Bundle from the dropdown", "B'Zer -  - Selection Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void populateBundleCombo(DataTable dt)
        {
            cmbBundle.Items.Clear();
            bundle_list.Clear();

            List<KeyValuePair<string, string>> listItems = new List<KeyValuePair<string, string>>();
            foreach (DataRow dtRow in dt.Rows)
            {
                string bundle_code = dtRow.Field<string>("bundle_code");
                int bundle_key = dtRow.Field<int>("bundle_key");
                if (bundle_code.ToLower().Trim().Contains(this.cmbBundle.Text.Trim().ToLower()))
                {
                    listItems.Add(new KeyValuePair<string, string>(Convert.ToString(bundle_key), bundle_code));
                    bundle_list.Add(bundle_key, bundle_code);
                    int pos = cmbBundle.Items.Add(bundle_code);
                }
            }
            this.cmbBundle.SelectionStart = this.cmbBundle.Text.Length;
            Cursor = Cursors.Default;
            // Automatically pop up drop-down
            this.cmbBundle.DroppedDown = true;
        }

        private void cmbBundle_TextUpdate(object sender, EventArgs e)
        {
            var dt = populateBundle();
            populateBundleCombo(dt);
        }
    }
}
