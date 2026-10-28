using System;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ImageHeaven.Logic;

namespace ImageHeaven
{
    public partial class DocumentTypeListView : UserControl
    {
        #region constants
        const string COL_DOC_TYPE = "DocumentType";

        const string COL_SHORTCUT = "ShortcutKey";

        const string COL_COUNT = "Count";
        #endregion

        private DocumentTypeStore _Store;

        private bool SuppressIndexEvent;

        public event EventHandler SelectedIndexChanged;

        public DocumentTypeStore Store { get => _Store; }

        public int SelectedIndex
        {
            get => mainListView.SelectedIndices.Count > 0 ? mainListView.SelectedIndices[0] : -1;

            set
            {
                SuppressIndexEvent = value != -1;
                mainListView.SelectedIndices.Clear();
                SuppressIndexEvent = false;
                if (value != -1)
                {
                    mainListView.SelectedIndices.Add(value);
                }
            }
        }

        public string SelectedDocumentType
        {
            get => mainListView.SelectedItems.Count > 0 ? mainListView.SelectedItems[0].Text : null;

            set
            {
                var matchIndex = mainListView.Items.IndexOfKey(value);
                if (matchIndex != -1)
                {
                    SelectedIndex = matchIndex;
                }
            }
        }

        public DocumentTypeListView()
        {
            InitializeComponent();
            SuppressIndexEvent = false;
            foreach (ColumnHeader column in mainListView.Columns)
            {
                column.Width = -2;
            }
            mainListView.SelectedIndexChanged += this.MainListView_SelectedIndexChanged;
        }

        public void Init(OdbcConnection sqlCon, OdbcTransaction sqlTran = null)
        {
            _Store = new DocumentTypeStore(sqlCon, sqlTran);
            mainListView.Items.AddRange(_Store.GetDocumentTypes().Select(docType =>
            {
                var item = new ListViewItem(docType);
                var shortcutItem = item.SubItems.Add(_Store.GetDocumentTypeShortcut(docType).ToString());
                var countItem = item.SubItems.Add("0");
                item.Name = COL_DOC_TYPE;
                shortcutItem.Name = COL_SHORTCUT;
                countItem.Name = COL_COUNT;
                return item;
            }).ToArray());
        }

        public void UpdateCounts(int projectKey, int batchKey, string fileName)
        {
            foreach (ListViewItem item in mainListView.Items)
            {
                item.SubItems[COL_COUNT].Text = "0";
                item.ForeColor = SystemColors.ControlText;
            }

            foreach (var docTypeCount in _Store.GetDocumentTypeCountsForFile(projectKey, batchKey, fileName))
            {
                var item = mainListView.FindItemWithText(docTypeCount.Key);
                if (item != null)
                {
                    item.SubItems[COL_COUNT].Text = docTypeCount.Value.ToString();
                    item.ForeColor = Color.Blue;
                }
            }
        }

        private void MainListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SuppressIndexEvent)
            {
                SelectedIndexChanged?.Invoke(sender, e);
            }
        }
    }
}
