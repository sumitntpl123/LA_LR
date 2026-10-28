using bzer_data_entry.src;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace bzer_data_entry
{
    public partial class frmEntrySummary : Form
    {
        //OdbcConnection sqlCon = null;
        NovaNet.Utils.GetProfile pData;
        NovaNet.Utils.ChangePassword pCPwd;
        NovaNet.Utils.Profile p;
        public static NovaNet.Utils.IntrRBAC rbc;
        //public Credentials crd;
        static wItem wi;
        public static OdbcTransaction txn;
        public static string projKey;
        public static string bundleKey;
        public static string bundleNo;
        public Credentials crd = new Credentials();
        private OdbcConnection sqlCon;
        public string batch_key;

        public static string delpath;
        eSTATES[] state;
        frmEntrySummary entrySum;

        public frmEntrySummary(OdbcConnection pCon, Credentials pcrd, eSTATES[] prmState)
        {
            InitializeComponent();

            sqlCon = pCon;

            crd = pcrd;

            //txn = _txn;

            state = prmState;

            init();
        }
        private void init(int selectedIndex = 0)
        {
            DataTable Dt = new DataTable();
            Dt = _GetEntries();
            dtGrdVol.DataSource = Dt;

            FormatDataGridView();

            this.dtGrdVol.Refresh();
            this.textBox2.Text = "";
            if (dtGrdVol.RowCount > 0 && selectedIndex < dtGrdVol.RowCount)
            {
                dtGrdVol.Rows[selectedIndex].Selected = true;
            }
            this.textBox2.Focus();

            ArrayList lst = GetTotalDaily(crd.created_by);
            for (int i = 0; i < lst.Count; i++)
            {
                deLabel1.Text = "Today You Have Entered: " + lst[0].ToString() + " Files";
            }
        }

        public DataTable _GetEntries()
        {
            DataTable dt = new DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',count(*) as 'Number of Files' from bundle_master a,case_file_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key group by a.proj_code,a.bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void FormatDataGridView()
        {
            //Format the Data Grid View
            dtGrdVol.Columns[0].Visible = false;
            dtGrdVol.Columns[1].Visible = false;

            //Set Autosize on for all the columns
            for (int i = 0; i < dtGrdVol.Columns.Count; i++)
            {
                dtGrdVol.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

        }

        private void frmEntrySummary_Load(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            this.textBox2.AutoCompleteCustomSource = GetSuggestions("bundle_master", "bundle_no");
            this.textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            ArrayList lst = GetTotalDaily(crd.created_by);
            for (int i = 0; i < lst.Count; i++)
            {
                deLabel1.Text = "Today You Have Entered: " + lst[0].ToString() + " Files";
            }
            watch.Stop();
            System.Diagnostics.Debug.Print("Time taken to load Entry Summary: " + watch.ElapsedMilliseconds.ToString());
        }

        public ArrayList GetTotalDaily(string name)
        {
            ArrayList totList = new ArrayList();
            //string sql = "Select proj_code from metadata_entry where date_format(created_DTTM,'%Y-%m-%d')=date_format(now(),'%Y-%m-%d') and created_by = '" + name + "'";
            string sql = "Select proj_code from metadata_entry where created_by = '" + name + "'" + " and date_format(created_DTTM,'%Y-%m-%d')=date_format(now(),'%Y-%m-%d')";
            //string sql = "Select district_code from deed_details where created_DTTM like now() and created_by = '" + crd.created_by + "'";
            DataSet ds = new DataSet();
            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(ds);
            if (ds.Tables.Count > 0)
            { totList.Add(ds.Tables[0].Rows.Count); }
            else { totList.Add("0"); }
            return totList;
        }

        public AutoCompleteStringCollection GetSuggestions(string tblName, string fldName)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct " + fldName + " from " + tblName;
            DataSet ds = new DataSet();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    x.Add(ds.Tables[0].Rows[i][0].ToString().Trim());
                }
            }
            //x.Add("Others");
            //x.Add("NA");
            return x;
        }

        public DataTable _GetResultBundle(string bundle_no)
        {
            DataTable dt = new DataTable();

            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number',count(*) as 'Number of Files' from bundle_master a,case_file_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and a.bundle_no like '%" + bundle_no + "%' group by a.proj_code,a.bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            string bundle_no = textBox2.Text;

            dtGrdVol.DataSource = null;
            DataTable Dt = new DataTable();

            if (bundle_no != null)
            {
                Dt = _GetResultBundle(bundle_no);
                dtGrdVol.DataSource = Dt;


                FormatDataGridView();

                this.dtGrdVol.Refresh();
                this.textBox2.Focus();

                if (dtGrdVol.Rows.Count > 0)
                {
                    dtGrdVol.Rows[0].Selected = true;
                    dtGrdVol.Focus();

                    return;
                }

            }
            else
            {
                //init();

                dtGrdVol.DataSource = null;
            }
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            init();
        }

        private void frmEntrySummary_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                init();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            if (e.Control == true && e.KeyCode == Keys.N)
            {
                cmdnew_Click(sender, e);
            }
        }

        public DataTable _GetBundleStatus(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select  status,bundle_key from bundle_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' and status = 0 ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void cmdnew_Click(object sender, EventArgs e)
        {
            //
            try
            {

                this.SetTopLevel(false);

                eSTATES[] state = new eSTATES[1] { NovaNet.wfe.eSTATES.METADATA_ENTRY };
                IBundleSelector bs = new BundleSelectorImpl();
                var bundleKeyValues = bs.Select(state, sqlCon, crd);

                projKey = bundleKeyValues.projKey;
                bundleKey = bundleKeyValues.bundleKey;

                if (projKey != null && bundleKey != null)
                {
                    if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "0")
                    {
                        this.SetTopLevel(true);
                        IEntryFormBuilder frmBuilder = new EntryFormBuilder();
                        var fm = frmBuilder.CreateEntryForm(sqlCon, projKey, bundleKey, DataLayerDefs.Mode._Add, bundleNo, crd);
                        fm.ShowDialog(this);
                        init();
                    }
                    else
                    {
                        MessageBox.Show(this, "This Bundle has been uploaded for the further process...", "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.SetTopLevel(true);
                        return;
                    }

                }
                else
                {
                    this.SetTopLevel(true);
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void dtGrdVol_DoubleClick(object sender, EventArgs e)
        {
            if (dtGrdVol.Rows.Count > 0)
            {
                projKey = dtGrdVol.SelectedRows[0].Cells[0].Value.ToString();
                bundleKey = dtGrdVol.SelectedRows[0].Cells[1].Value.ToString();
                if (crd.role == ihConstants._ADMINISTRATOR_ROLE || crd.role == "Supervisor" || crd.role == "Metadata Entry")
                {
                    int entryCount = Convert.ToInt32(dtGrdVol.SelectedRows[0].Cells[4].Value.ToString());
                    if (entryCount > 0)
                    {


                        if (projKey != null && bundleKey != null)
                        {

                            Form activeChild = this.ActiveMdiChild;
                            if (activeChild == null)
                            {
                                FilesEdit fm = new FilesEdit(sqlCon, DataLayerDefs.Mode._Edit, crd, projKey, bundleKey);
                                fm.ShowDialog(this);
                                cmdSearch_Click(this, EventArgs.Empty);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "No file is enterd for this Bundle...", "B'Zer -  - Entry Check !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(this, "You are not authorized to do so ...", "B'Zer - ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
        }

        private void dtGrdVol_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.O)
            {
                dtGrdVol_DoubleClick(sender, e);
            }
            if (e.KeyCode == Keys.Enter)
            {
                //dtGrdVol_DoubleClick(sender, e);
            }

        }

    }
}
