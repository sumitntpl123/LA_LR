using NovaNet.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Windows.Forms;

namespace bzer_data_entry.src
{
    public partial class FilesEdit : Form
    {
        Credentials Crd = new Credentials();
        OdbcConnection SqlCon = null;
        DataLayerDefs.Mode Mode;

        MetadataStore store;
        BatchDataStore batchStore;

        public static string ProjKey;
        public static string BundleKey;

        private List<string> _Filenames;

        private DataEntryNavigationController NavigationController;

        public List<string> Filenames
        {
            get => _Filenames;
            set
            {
                _Filenames = value;
            }
        }
        public FilesEdit(OdbcConnection sqlCon, DataLayerDefs.Mode mode, Credentials crd, string projKey, string bundleKey)
        {
            AvroBengaliPhoneticParser.Parser.Register();
            InitializeComponent();
            SqlCon = sqlCon;

            store = new MetadataStore(SqlCon);
            batchStore = new BatchDataStore(SqlCon);

            Mode = mode;
            Crd = crd;
            ProjKey = projKey;
            BundleKey = bundleKey;

            DisplayBundleDetails();
            InitNavigationController();
        }

        private void InitNavigationController()
        {
            var controllerBuilder = new DataEntryNavigationController.Builder(this);
            foreach (var controlIndexPair in new Control[]{
                pbiTextBoxSearchFileNames
            }.Select((control, index) => (control, index)))
            {
                controllerBuilder.AddControl(controlIndexPair.index, controlIndexPair.control.Name, controlIndexPair.control);
            }
            NavigationController = controllerBuilder.Ready();
        }
        private void DisplayBundleDetails()
        {
            //var batchMetaData = GetBatchMetadata();
            //SetBatchLabels(batchMetaData);

            //var batchKeyData = GetBatchKeyData(batchMetaData.Mouza.MasterID);
            //SetBatchKeyLabels(batchKeyData);

            //DisplayAllFilenamesUnderBundle();
            var batchMetaData = GetBatchMetadata();
            if (batchMetaData != null)
            {
                SetBatchLabels(batchMetaData);
                if (batchMetaData.Mouza != null && batchMetaData.Mouza.MasterID != Guid.Empty)
                {
                    var batchKeyData = GetBatchKeyData(batchMetaData.Mouza.MasterID);
                    SetBatchKeyLabels(batchKeyData);
                }
            }
            DisplayAllFilenamesUnderBundle();
        }

        private BatchMetadata GetBatchMetadata()
        {
            var projectCode = int.Parse(ProjKey);
            var bundleCode = int.Parse(BundleKey);

            var batchMetaData = batchStore.GetBatchMetadata(projectCode, bundleCode);
            return batchMetaData;
        }
        private void SetBatchLabels(BatchMetadata batchMetaData)
        {
            labelDistrict.Text = batchMetaData.District.Value.BengaliText;
            labelBlock.Text = batchMetaData.Block.Value.BengaliText;
            labelMouza.Text = batchMetaData.Mouza.Value.BengaliText;
        }

        private BatchKeyData GetBatchKeyData(Guid masterID)
        {
            var batchInfo = batchStore.GetBatchKeyData(masterID);
            var batchKeyData = batchInfo.FirstOrDefault(batch => batch.ProjectCode == ProjKey && batch.BundleKey == BundleKey);
            return batchKeyData;
        }
        private void SetBatchKeyLabels(BatchKeyData batchKeyData)
        {
            labelBundleName.Text = batchKeyData.BundleName;
            labelBundleNumber.Text = batchKeyData.BundleNo;
            //labelHandoverDate.Text = batchKeyData.HandoverDate.ToString("MMMM dd, yyyy");
        }
        private void DisplayAllFilenamesUnderBundle()
        {
            Filenames = store.GetAllFilesFromBatch(int.Parse(ProjKey), int.Parse(BundleKey))
                             .Select(item => item.FileName.EnglishText).ToList();

            listViewFileNames.Items.AddRange(Filenames.Select(file => new ListViewItem(file)).ToArray());
        }

        private void pbiTextBoxSearchFileNames_InnerTextChanged(object sender, System.EventArgs e)
        {
            string searchText = pbiTextBoxSearchFileNames.EnglishText.ToUpper();
            FilterFileNames(searchText);
        }

        private void FilterFileNames(string searchText)
        {
            listViewFileNames.Items.Clear();

            listViewFileNames.Items.AddRange(Filenames
                .Where(file => GetFilenameWithoutBatchName(file).Contains(searchText))
                .Select(file => new ListViewItem(file)).ToArray());
        }
        private void listViewFileNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenFileEntryForm(getFilenameOfSelectedItem());
            }
        }
        private void listViewFileNames_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OpenFileEntryForm(getFilenameOfSelectedItem());
            }
        }

        private string getFilenameOfSelectedItem()
        {
            int index = listViewFileNames.FocusedItem.Index;
            return listViewFileNames.Items[index].SubItems[0].Text;
        }
        private void OpenFileEntryForm(string fileName)
        {
            IEntryFormBuilder formBuilder = new EntryFormBuilder();
            var form = formBuilder.CreateEntryForm(SqlCon, ProjKey, BundleKey, Mode, fileName, Crd);
            form.ShowDialog(this);
        }
        private string GetFilenameWithoutBatchName(string filename)
        {
            const char Underscore = '_';
            int underscoreIndex = filename.IndexOf(Underscore);
            var filenameAfterUnderscore = filename.Substring(++underscoreIndex);
            return filenameAfterUnderscore;
        }
    }
}
