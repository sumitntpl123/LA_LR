using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using bzer_data_entry;
using LItems;
using NovaNet.wfe;
using PhoneticBengaliInput;

namespace Reporting
{
    public partial class frmDashboard : Form
    {
        OdbcConnection sqlCon;

        string searchFilter = string.Empty;

        private readonly MetadataStore DataStore;

        List<MasterItem> Districts;

        List<MasterItem> Blocks;

        List<MasterItem> Mouzas;

        ihwQuery ihw;
        enum FilterFields
        {
            district,
            block,
            mouza
        }

        struct QueryData
        {
            private MasterItem item;
            private FilterFields? field;

            public FilterFields? Field
            {
                get { return field; }
                set { field = value; }
            }

            public MasterItem Item { get => item; set => item = value; }

            public QueryData(FilterFields ff, MasterItem itm)
            {
                item = itm;
                field = ff;
            }

            public string GetStatusText()
            {
                return $@"Applying filter {Field}: {Item.Value.BengaliText}";
            }
        }

        QueryData? parentGuid;
        public frmDashboard(OdbcConnection prmCon)
        {
            AvroBengaliPhoneticParser.Parser.Register();
            InitializeComponent();
            sqlCon = prmCon;
            DataStore = new MetadataStore(sqlCon);
            ihw = new ihwQuery(sqlCon);
            Districts = DataStore.GetMasterList(null, new List<string> { MetadataStore.FIELD_DISTRICT });
            init(ihw.GetAllBatches());
        }

        private void init(List<(string name, int status)> lstValues)
        {
            lvwBatchDetails.Clear();
            lvwSummary.Items.Clear();
            stsDetails.Items["batchCount"].Text = $@"Total batches: {Convert.ToString(lstValues.Count)}";
            pbiCmbDistrict.LanguageItems.Set(Districts.Select(district => (LanguageItem)district.Value));

            var enumStates = lstValues.GroupBy(e => (eSTATES)e.status).ToList();
            lvwBatchDetails.Columns.Add("SLNO", "Sl. No.");
            enumStates.ForEach(e =>
            {
                string strColumnHeader = $@"Undefined ({e.Key})";
                if (getNamedStates().ContainsKey(e.Key))
                {
                    strColumnHeader = getNamedStates()[e.Key];
                }

                lvwBatchDetails.Columns.Add(Convert.ToString(e.Key), $@"{strColumnHeader} ({e.Count()})");
                lvwSummary.Items.Add(strColumnHeader).SubItems.Add($@"{e.Count()}");
                lvwSummary.Items[lvwSummary.Items.Count - 1].BackColor = (lvwSummary.Items.Count - 1) % 2 == 0 ? Color.LightGray : Color.White;
            });

            var maxRowsByStateInLVW = enumStates.Select(group => new
            {
                Metric = group.Key,
                Count = group.Count()
            })
            .Max(e => e.Count as int?) ?? 0;

            var columnCount = enumStates.Count;
            for (int i = 0; i < maxRowsByStateInLVW; i++)
            {
                string[] strArrayForLVWItem = new string[columnCount + 1];
                strArrayForLVWItem[0] = Convert.ToString(i + 1);
                var lvwItems = new ListViewItem(strArrayForLVWItem);
                lvwItems.BackColor = i % 2 == 0 ? Color.LightGray : Color.White;
                lvwBatchDetails.Items.Add(lvwItems);
            }

            for (var i = 0; i < columnCount; i++)
            {
                var itemsOfGroup = enumStates.ElementAt(i).Select(e => e.name);
                for (var j = 0; j < itemsOfGroup.Count(); j++)
                {
                    lvwBatchDetails.Items[j].SubItems[i + 1].Text = itemsOfGroup.ElementAt(j);
                }
            }
            lvwBatchDetails.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvwBatchDetails.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private Dictionary<eSTATES, string> getNamedStates()
        {
            Dictionary<eSTATES, string> dicNamedList = new Dictionary<eSTATES, string>();
            dicNamedList.Add(eSTATES.BATCH_CREATED, "Created");
            dicNamedList.Add(eSTATES.BATCH_SCANNED, "Scanned");
            dicNamedList.Add(eSTATES.BATCH_QC, "QC");
            dicNamedList.Add(eSTATES.BATCH_INDEXED, "Indexed");
            dicNamedList.Add(eSTATES.BATCH_FQC, "FQC");
            dicNamedList.Add(eSTATES.BATCH_READY_FOR_UAT, "Ready for UAT");
            dicNamedList.Add(eSTATES.BATCH_EXPORTED, "Exported");
            dicNamedList.Add(eSTATES.BATCH_SUBMITTED, "Submitted");
            return dicNamedList;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.searchFilter = string.Empty;
            var searchStatusText = string.Empty;
            if (this.txtSearch.Text.Trim().Length > 0)
            {
                this.searchFilter = this.txtSearch.Text.Trim();
                searchStatusText = $@"Search applied {searchFilter}";
            }
            init(ihw.GetAllBatches(searchFilter));
            updateStatusText(searchStatusText);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtSearch.Text = string.Empty;
            this.btnSearch_Click(sender, e);
            updateStatusText(string.Empty);
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            if (parentGuid != null)
            {
                init(ihw.GetAllBatchesByParent(parentGuid.Value.Item.MasterID.ToString().Replace("-", "")));
                updateStatusText(parentGuid.Value.GetStatusText());
            }

        }

        private void pbiCmbDistrict_Leave(object sender, EventArgs e)
        {
            if (pbiCmbDistrict.SelectedIndex >= 0)
            {
                parentGuid = new QueryData(FilterFields.district, Districts.ElementAt(pbiCmbDistrict.SelectedIndex));
                Blocks = DataStore.GetMasterList(parentGuid.Value.Item.MasterID, new List<string> { MetadataStore.FIELD_BLOCK });
                pbiCmbBlock.LanguageItems.Set(Blocks.Select(block => (LanguageItem)block.Value));
            }
            else
            {
                pbiCmbBlock.LanguageItems.Set(new List<LanguageItem>());
            }

        }

        private void pbiCmbBlock_Leave(object sender, EventArgs e)
        {
            if (pbiCmbBlock.SelectedIndex >= 0)
            {
                parentGuid = new QueryData(FilterFields.block, Blocks.ElementAt(pbiCmbBlock.SelectedIndex));
                Mouzas = DataStore.GetMasterList(parentGuid.Value.Item.MasterID, new List<string> { MetadataStore.FIELD_MOUZA });
                pbiCmbMouza.LanguageItems.Set(Mouzas.Select(mouza => (LanguageItem)mouza.Value));
            }
            else
            {
                pbiCmbMouza.LanguageItems.Set(new List<LanguageItem>());
            }
        }
        private void pbiCmbMouza_Leave(object sender, EventArgs e)
        {
            if (pbiCmbMouza.SelectedIndex >= 0)
            {
                parentGuid = new QueryData(FilterFields.mouza, Mouzas.ElementAt(pbiCmbMouza.SelectedIndex));
            }
            else
            {
                pbiCmbMouza.LanguageItems.Set(new List<LanguageItem>());
            }
        }
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            pbiCmbDistrict.LanguageItems.Set(new List<LanguageItem>());
            pbiCmbBlock.LanguageItems.Set(new List<LanguageItem>());
            pbiCmbMouza.LanguageItems.Set(new List<LanguageItem>());
            init(ihw.GetAllBatches());
            updateStatusText(string.Empty);
        }

        private void updateStatusText(string text)
        {
            stsDetails.Items["filterField"].Text = text;
        }
    }
}
