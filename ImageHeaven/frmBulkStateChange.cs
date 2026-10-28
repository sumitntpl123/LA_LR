using ImageHeaven.Logic;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Windows.Forms;

namespace ImageHeaven
{
    public abstract partial class frmBulkStateChange : Form
    {
        protected readonly OdbcConnection SqlCon;
        protected readonly string ProjectKey;
        protected const string boxNumber = "1";
        protected string imgFileName = null;
        protected eSTATES[] currState;
        protected eSTATES typeState;
        protected CtrlBox pBox = null;
        protected CtrlImage pImage = null;
        protected CtrlPolicy pPolicy = null;
        protected Credentials crd = new Credentials();
        protected UpdateStatusLogic updateStatusLogic;
        protected GetItemsLogic getItemsLogic = null;
        protected aeImageQCLogic aeImgQCLogic = null;
        private Imagery img;
        private int BundleCount = 0;
        protected NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        protected BaseLogic baseLogicObj = null;

        protected abstract void UpdateAllPolicyStatus(string bundleKey);
        protected abstract void UpdateImageStatus(ArrayList arrImage, string bundleKey, string fileName, eSTATES state);
        public frmBulkStateChange(OdbcConnection con, string projkey, Credentials prmcrd, eSTATES tState, BaseLogic obj)
        {
            InitializeComponent();
            SqlCon = con;
            ProjectKey = projkey;
            crd = prmcrd;
            updateStatusLogic = new UpdateStatusLogic(SqlCon);
            getItemsLogic = new GetItemsLogic(SqlCon);
            aeImgQCLogic = new aeImageQCLogic(SqlCon, crd);
            currState = new eSTATES[1];
            currState[0] = eSTATES.POLICY_SCANNED;
            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            typeState = tState;
            baseLogicObj = obj;

            loadBundle();
        }
        protected ArrayList GetImagesItems(eITEMS item, eSTATES[] state, string fileName, string bundleKey)
        {
            wItemControl wic = null;
            string strQr = string.Empty;
            ArrayList arrItem = new ArrayList();

            if (item == eITEMS.PAGE)
            {
                var dt = baseLogicObj.GetImagesItems(item, state, ProjectKey, bundleKey, fileName);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        wic = new CtrlImage(Convert.ToInt32(dt.Rows[i]["proj_key"].ToString()), Convert.ToInt32(dt.Rows[i]["batch_key"].ToString()), boxNumber, dt.Rows[i]["policy_number"].ToString(), dt.Rows[i]["page_name"].ToString(), dt.Rows[i]["doc_type"].ToString());
                        arrItem.Add(wic);
                    }
                }
            }
            return arrItem;
        }

        private void loadBundle()
        {
            listViewBundle.Clear();
            var batch = new wfeBatch(SqlCon);
            var ds = batch.GetAllValues(Convert.ToInt32(ProjectKey), new eSTATES[] { typeState });
            var bundles = ds.Tables
                .Cast<System.Data.DataTable>()
                .SelectMany(tab => tab.Rows.Cast<DataRow>().Select(row => new BundleInfo
                {
                    BundleName = row.Field<string>("bundle_name"),
                    BundleKey = row.Field<int>("bundle_key")
                }))
                .ToList();
            if (bundles.Count == 0)
            {
                listViewBundle.Clear();
                MessageBox.Show("No bundle found.");
                this.Dispose();
            }
            if (bundles.Count > 0)
            {
                listViewBundle.View = View.Details;
                listViewBundle.Columns.Add("Bundle Name");
                listViewBundle.Columns[0].Width = -2;
                listViewBundle.Columns[0].Width = listViewBundle.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;

                foreach (var item in bundles)
                {
                    var listViewItem = new ListViewItem(item.BundleName);

                    listViewItem.Tag = item.BundleKey;

                    listViewBundle.Items.Add(listViewItem);
                }
                BundleCount = bundles.Count;
                toolStripStatusLabel1.Text = "Bundle: " + BundleCount;
            }
        }
        private class BundleInfo
        {
            public string BundleName { get; set; }
            public int BundleKey { get; set; }
        }
        private void ProcessSelectedBundles()
        {
            // Check if at least one item is checked
            var anyChecked = listViewBundle.Items.Cast<ListViewItem>().Any(item => item.Checked);

            if (!anyChecked)
            {
                MessageBox.Show("Please select at least one item to proceed.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (ListViewItem item in listViewBundle.Items)
            {
                if (item.Checked && item.Tag != null)
                {
                    UpdateAllPolicyStatus(item.Tag.ToString());
                }
            }
            checkBoxSelectAll.Checked = false;
            loadBundle();
        }
        public int GetFileCount(string projkey, string bundleKey)
        {
            return baseLogicObj.GetFileCount(projkey, bundleKey);
        }
        protected ArrayList GetItems(eITEMS item, eSTATES state, string bundleKey)
        {
            return getItemsLogic.GetItems(item, ProjectKey, bundleKey, new eSTATES[] { state });
        }

        protected string GetPolicyPath(string bundleKey, string policyName)
        {
            wfeBatch wBatch = new wfeBatch(SqlCon);
            string batchPath = GetPath(Convert.ToInt32(ProjectKey), Convert.ToInt32(bundleKey));
            return batchPath + "\\" + policyName;
        }
        private string GetPath(int prmProjKey, int prmBatchKey)
        {
            DataSet projDs = new DataSet();
            var Path = string.Empty;

            projDs = aeImgQCLogic.GetPath(prmProjKey, prmBatchKey);
            if (projDs.Tables[0].Rows.Count > 0)
            {
                Path = projDs.Tables[0].Rows[0]["bundle_path"].ToString();
            }
            return Path;
        }
        protected bool UpdateBundleStatus(int prmProjKey, int prmBatchKey, eSTATES typeState, eSTATES state)
        {
            return updateStatusLogic.UpdateBundleStatus(prmProjKey.ToString(), prmBatchKey.ToString(), new eSTATES[] { typeState }, new eSTATES[] { state }, state);
        }

        protected bool UpdateStatus(Credentials prmCrd, string bundleKey, string PolicyNumber, eSTATES newState)
        {
            return updateStatusLogic.UpdateStatus(ProjectKey, bundleKey, PolicyNumber, prmCrd, new eSTATES[] { }, new eSTATES[] { eSTATES.POLICY_EXPORTED }, newState);
        }
        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = checkBoxSelectAll.Checked;

            foreach (ListViewItem item in listViewBundle.Items)
            {
                item.Checked = isChecked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessSelectedBundles();
        }

        private void listViewBundle_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var Count = listViewBundle.Items.Cast<ListViewItem>().Any(item => item.Checked);
            if (!Count)
            {
                toolStripStatusLabel1.Text = "Bundle: " + BundleCount;
            }
            else
            {
                var bundleCount = listViewBundle.Items.Cast<ListViewItem>().Count(item => item.Checked);
                toolStripStatusLabel1.Text = "Bundle selected: " + bundleCount + "/" + BundleCount;
            }
        }
    }
}
