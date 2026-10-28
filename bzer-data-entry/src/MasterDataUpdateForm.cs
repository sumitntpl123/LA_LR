using bzer_data_entry.src;
using NovaNet.Utils;
using PhoneticBengaliInput;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Utils;

namespace bzer_data_entry
{
    public partial class MasterDataUpdateForm : Form
    {
        private readonly LanguageItem LastEntry;

        private PBITextBox CurrentTextBox;

        private OdbcConnection SqlCon;

        private Credentials Credentials;

        private DataEntryNavigationController DistrictController;

        private DataEntryNavigationController BlockController;

        private DataEntryNavigationController MouzaController;

        private DataEntryNavigationController ProjaStController;

        private NavigationControllerSet ControllerSet;

        private ViewSelector viewSelector;

        #region regex patterns
        static readonly Regex InvalidPattern = new Regex(@"^\s|\s$|[^a-zA-Z0-9 ^\-:`,$.]");
        static readonly Regex InvalidCodePattern = new Regex(@"^\d+$");
        #endregion

        #region constants
        private const string CODE_FIELD_TAG = "code";
        private const int MOUZA_CODE_LENGTH = 3;
        private const int DISTRICT_OR_BLOCK_CODE_LENGTH = 2;
        #endregion

        private static string CurrentLevel { get; set; }
        private CoustomTag customTag = null;

        public enum ViewType
        {
            District,
            Block,
            Mouza,
            RemoveMouza
        }
        public MasterDataUpdateForm()
        {
            AvroBengaliPhoneticParser.Parser.Register();
            InitializeComponent();

            treeViewMasterData.ExpandAll();

            // Wire up MouseDown event for treeViewMasterData
            treeViewMasterData.MouseDown += treeViewMasterData_MouseDown;

            LastEntry = new LanguageItem();

            pbiTextBoxDistrict.Tag = MetadataStore.FIELD_DISTRICT;
            pbiTextBoxBlock.Tag = MetadataStore.FIELD_BLOCK;
            pbiTextBoxMouza.Tag = MetadataStore.FIELD_MOUZA;
            pbiTextBoxCode.Tag = "code";
        }
        public MasterDataUpdateForm(OdbcConnection SqlCon, Credentials cred) : this()
        {
            this.SqlCon = SqlCon;
            Credentials = cred;
            PopulateTreeView();

            InitNavigationController();

            viewSelector = new ViewSelector(this);
            viewSelector.SelectView(ViewType.District);
            customTag = new CoustomTag() { identity = CODE_FIELD_TAG };
        }

        private void InitNavigationController()
        {
            var bcBuilder = new DataEntryNavigationController.Builder(this);
            foreach (var controlIndexPair in new Control[] {
                pbiTextBoxBlock,
                pbiTextBoxCode,
                buttonAddBlock
            }.Select((control, index) => (control, index)))
            {
                bcBuilder.AddControl(controlIndexPair.index, controlIndexPair.control.Name, controlIndexPair.control);
            }
            BlockController = bcBuilder.Ready(enable: false);

            var mcBuilder = new DataEntryNavigationController.Builder(this);
            foreach (var controlIndexPair in new Control[] {
                pbiTextBoxMouza,
                pbiTextBoxCode,
                buttonAddMouza
            }.Select((control, index) => (control, index)))
            {
                mcBuilder.AddControl(controlIndexPair.index, controlIndexPair.control.Name, controlIndexPair.control);
            }
            MouzaController = mcBuilder.Ready(enable: false);

            var dcBuilder = new DataEntryNavigationController.Builder(this);
            foreach (var controlIndexPair in new Control[] {
                pbiTextBoxDistrict,
                pbiTextBoxCode,
                buttonAddDistrict
            }.Select((control, index) => (control, index)))
            {
                dcBuilder.AddControl(controlIndexPair.index, controlIndexPair.control.Name, controlIndexPair.control);
            }
            DistrictController = dcBuilder.Ready(enable: false);

            ControllerSet = new NavigationControllerSet(DistrictController, BlockController, MouzaController);
        }

        private TreeNode CreateNode(string text, object tag)
        {
            return new TreeNode(text) { Tag = tag };
        }

        private void PopulateTreeView()
        {
            treeViewMasterData.Nodes.Clear();

            MetadataStore metadatastore = new MetadataStore(SqlCon);
            var districts = metadatastore.GetMasterList(null, new List<string> { MetadataStore.FIELD_DISTRICT });
            Console.WriteLine(districts);
            foreach (var district in districts)
            {
                var districtNode = CreateNode(district.Value.EnglishText, district);      
                treeViewMasterData.Nodes.Add(districtNode);

                var blocks = metadatastore.GetMasterList(district.MasterID, new List<string> { MetadataStore.FIELD_BLOCK });
                foreach (var block in blocks)
                {
                    var blockNode = CreateNode(block.Value.EnglishText, block);
                    districtNode.Nodes.Add(blockNode);

                    var mouzas = metadatastore.GetMasterList(block.MasterID, new List<string> { MetadataStore.FIELD_MOUZA });
                    foreach (var mouza in mouzas)
                    {
                        var mouzaNode = CreateNode(mouza.Value.EnglishText, mouza);
                        blockNode.Nodes.Add(mouzaNode);
                    }
                }
            }
            UpdateNodeCounts();
            treeViewMasterData.ExpandAll();
        }

        public void UpdateNodeCounts()
        {
            MetadataStore metadataStore = new MetadataStore(SqlCon);
            foreach (TreeNode districtNode in treeViewMasterData.Nodes)
            {
                var blockCount = districtNode.Nodes.Count;
                var districtCode = metadataStore.GetMasterCode(((MasterItem)districtNode.Tag).MasterID);
                districtNode.Text = $"[{districtCode}]{((MasterItem)districtNode.Tag).Value.EnglishText} ({blockCount})";
                foreach (TreeNode blockNode in districtNode.Nodes)
                {
                    var mouzaCount = blockNode.Nodes.Count;
                    var blockCode = metadataStore.GetMasterCode(((MasterItem)blockNode.Tag).MasterID);
                    blockNode.Text = $"[{blockCode}]{((MasterItem)blockNode.Tag).Value.EnglishText} ({mouzaCount})";
                    foreach (TreeNode mouzaNode in blockNode.Nodes)
                    {
                        var mouzaCode = metadataStore.GetMasterCode(((MasterItem)mouzaNode.Tag).MasterID);
                        mouzaNode.Text = $"[{mouzaCode}]{((MasterItem)mouzaNode.Tag).Value.EnglishText}";
                    }
                }
            }
        }

        private void ReadProjaSotterTypes()
        {
            if (treeViewMasterData.SelectedNode != null && treeViewMasterData.SelectedNode.Tag is MasterItem selectedDistrict)
            {
                listViewProjaSt.Items.Clear();
                var store = new MetadataStore(SqlCon);
                var projaStTypes = store.GetMasterList(selectedDistrict.MasterID, new List<string> { MetadataStore.FIELD_PROJA_ST_TYPE });
                var regularFont = new Font(listViewProjaSt.Font, FontStyle.Regular);
                listViewProjaSt.Items.AddRange(projaStTypes.Select(projaStType => new ListViewItem(projaStType.Value.EnglishText)
                {
                    Tag = projaStType,
                    Font = regularFont
                }).ToArray());
                foreach (var columnHeader in listViewProjaSt.Columns.Cast<ColumnHeader>())
                {
                    columnHeader.Width = -2;
                }
            }
        }

        private void ShowProjaSotterTypeControls(bool visible = true)
        {
            tableLayoutPanelProjaSt.Visible = visible;
            if (visible)
            {
                ReadProjaSotterTypes();
            }
        }

        private void UpdateControls()
        {
            if (treeViewMasterData.SelectedNode == null)
            {
                viewSelector.SelectView(ViewType.District);
                buttonRemove.Enabled = false;
            }
            else if (treeViewMasterData.SelectedNode.Tag is MasterItem value)
            {
                // Check if referenced
                int count = GetMasterDataCountById(value.MasterID);
                EnableDisableButtonRemoved(count);

                if (treeViewMasterData.SelectedNode.Parent == null)
                {
                    viewSelector.SelectView(ViewType.Block);
                }
                else if (treeViewMasterData.SelectedNode.Parent != null && treeViewMasterData.SelectedNode.Parent.Parent == null)
                {
                    viewSelector.SelectView(ViewType.Mouza);
                }
                else
                {
                    viewSelector.SelectView(ViewType.RemoveMouza);
                }
            }
        }

        private void treeViewMasterData_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the click is on an empty area of the TreeView
            if (e.Button == MouseButtons.Left)
            {
                var node = treeViewMasterData.GetNodeAt(e.X, e.Y);
                if (node == null)
                {
                    treeViewMasterData.SelectedNode = null; // Clear selection
                    UpdateControls(); // Update controls accordingly
                }
            }
        }

        private void treeViewMasterData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateControls();
            //if (!string.IsNullOrWhiteSpace(LastEntry.BengaliPhoneticText) && CurrentTextBox != null)
            //{
                //CurrentTextBox.EnglishText=LastEntry.EnglishText;
            //}
        }

        private void buttonAddDistrict_Click(object sender, EventArgs e)
        {
            customTag.currentLevel = MetadataStore.FIELD_DISTRICT;
            pbiTextBoxCode.Tag = customTag;
            //if (!string.IsNullOrWhiteSpace(pbiTextBoxDistrict.BengaliText) && !string.IsNullOrWhiteSpace(pbiTextBoxCode.EnglishText))
            //{
                try
                {
                    bool IsValidInput = ValidationHelper.ValidateAllInputs(new[] { pbiTextBoxDistrict, pbiTextBoxCode });
                    if (!IsValidInput)
                    {
                        return;
                    }

                    TransactionManager.GetInstance(SqlCon).OpenTransaction();
                    var sqlTran = TransactionManager.GetInstance(SqlCon).Txn;
                    var district = new MasterItem
                    {
                        MasterID = Guid.NewGuid(),
                        Value = new ValueItem
                        {
                            BengaliText = pbiTextBoxDistrict.BengaliText,
                            EnglishText = pbiTextBoxDistrict.EnglishText,
                            BengaliPhoneticText = pbiTextBoxDistrict.BengaliPhoneticText
                        }
                    };
                    MasterDataStore masterDataStore = new MasterDataStore(SqlCon, sqlTran);
                    masterDataStore.AddMasterItem(MetadataStore.FIELD_DISTRICT, district);
                    masterDataStore.AddMasterCode(MetadataStore.FIELD_DISTRICT, district.MasterID, pbiTextBoxCode.EnglishText);
                    TransactionManager.GetInstance(SqlCon).CommitTransaction();
                    var newNode = CreateNode(district.Value.EnglishText, district);
                    treeViewMasterData.Nodes.Insert(0, newNode);
                    treeViewMasterData.ExpandAll();
                    UpdateControls();
                    UpdateNodeCounts();

                    DistrictController.Reset();
                }
                catch (Exception ex)
                {
                    TransactionManager.GetInstance(SqlCon).AbortTransaction();
                    MessageBox.Show(ex.Message);
                }
            //}
        }

        private void buttonAddBlock_Click(object sender, EventArgs e)
        {
            customTag.currentLevel = MetadataStore.FIELD_BLOCK;
            pbiTextBoxCode.Tag = customTag;
            if (treeViewMasterData.SelectedNode != null && !string.IsNullOrWhiteSpace(pbiTextBoxBlock.EnglishText) && !string.IsNullOrWhiteSpace(pbiTextBoxCode.EnglishText) && treeViewMasterData.SelectedNode.Tag is MasterItem selectedDistrict)
            {
                try
                {
                    bool IsValidInput = ValidationHelper.ValidateAllInputs(new[] { pbiTextBoxBlock, pbiTextBoxCode });
                    if (!IsValidInput)
                    {
                        return;
                    }

                    TransactionManager.GetInstance(SqlCon).OpenTransaction();
                    var sqlTran = TransactionManager.GetInstance(SqlCon).Txn;
                    var block = new MasterItem
                    {
                        MasterID = Guid.NewGuid(),
                        Value = new ValueItem
                        {
                            BengaliText = pbiTextBoxBlock.BengaliText,
                            EnglishText = pbiTextBoxBlock.EnglishText,
                            BengaliPhoneticText = pbiTextBoxBlock.BengaliPhoneticText
                        }
                    };
                    MasterDataStore masterDataStore = new MasterDataStore(SqlCon, sqlTran);
                    masterDataStore.AddMasterItem(MetadataStore.FIELD_BLOCK, block, selectedDistrict.MasterID);
                    masterDataStore.AddMasterCode(MetadataStore.FIELD_BLOCK, block.MasterID, pbiTextBoxCode.EnglishText, selectedDistrict.MasterID);
                    TransactionManager.GetInstance(SqlCon).CommitTransaction();
                    var newNode = CreateNode(block.Value.EnglishText, block);
                    treeViewMasterData.SelectedNode.Nodes.Insert(0, newNode);
                    treeViewMasterData.ExpandAll();
                    UpdateControls();
                    UpdateNodeCounts();

                    BlockController.Reset();
                }
                catch (Exception ex)
                {
                    TransactionManager.GetInstance(SqlCon).AbortTransaction();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonAddMouza_Click(object sender, EventArgs e)
        {
            customTag.currentLevel = MetadataStore.FIELD_MOUZA;
            pbiTextBoxCode.Tag = customTag;
            if (treeViewMasterData.SelectedNode != null && !string.IsNullOrWhiteSpace(pbiTextBoxMouza.EnglishText) && !string.IsNullOrWhiteSpace(pbiTextBoxCode.EnglishText) && treeViewMasterData.SelectedNode.Tag is MasterItem selectedBlock)
            {
                try
                {
                    bool IsValidInput = ValidationHelper.ValidateAllInputs(new[] { pbiTextBoxMouza, pbiTextBoxCode });

                    if (!IsValidInput)
                    {
                        return;
                    }

                    TransactionManager.GetInstance(SqlCon).OpenTransaction();
                    var sqlTran = TransactionManager.GetInstance(SqlCon).Txn;
                    var mouza = new MasterItem
                    {
                        MasterID = Guid.NewGuid(),
                        Value = new ValueItem
                        {
                            BengaliText = pbiTextBoxMouza.BengaliText,
                            EnglishText = pbiTextBoxMouza.EnglishText,
                            BengaliPhoneticText = pbiTextBoxMouza.BengaliPhoneticText
                        }
                    };
                    MasterDataStore masterDataStore = new MasterDataStore(SqlCon, sqlTran);
                    masterDataStore.AddMasterItem(MetadataStore.FIELD_MOUZA, mouza, selectedBlock.MasterID);
                    masterDataStore.AddMasterCode(MetadataStore.FIELD_MOUZA, mouza.MasterID, pbiTextBoxCode.EnglishText, selectedBlock.MasterID);
                    TransactionManager.GetInstance(SqlCon).CommitTransaction();
                    var newNode = CreateNode(mouza.Value.EnglishText, mouza);
                    treeViewMasterData.SelectedNode.Nodes.Insert(0, newNode);
                    treeViewMasterData.ExpandAll();
                    UpdateControls();
                    UpdateNodeCounts();

                    MouzaController.Reset();
                }
                catch (Exception ex)
                {
                    TransactionManager.GetInstance(SqlCon).AbortTransaction();
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private int GetMasterDataCountById(Guid masterId)
        {
            var store = new BatchDataStore(SqlCon);
            var batchDataList = store.GetBatchKeyData(masterId);
            return batchDataList.Count;
        }
        private void EnableDisableButtonRemoved(int count)
        {
            buttonRemove.Enabled = (count == 0);
        }
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeViewMasterData.SelectedNode != null)
                {
                    if (treeViewMasterData.SelectedNode.Tag is MasterItem item)
                    {
                        // Confirm deletion
                        var dr = MessageBox.Show(
                            this,
                            "Are you sure you want to delete this value?",
                            "B'Zer - Confirm Deletion",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );

                        if (dr != DialogResult.Yes)
                        {
                            return;
                        }

                        TransactionManager.GetInstance(SqlCon).OpenTransaction();
                        var sqlTran = TransactionManager.GetInstance(SqlCon).Txn;
                        MasterDataStore masterDataStore = new MasterDataStore(SqlCon, sqlTran);
                        masterDataStore.DeleteMasterCodeItem(item.MasterID,
                            new List<string> { MetadataStore.FIELD_DISTRICT, MetadataStore.FIELD_BLOCK, MetadataStore.FIELD_MOUZA });
                        masterDataStore.DeleteMasterItem(item.MasterID);
                        TransactionManager.GetInstance(SqlCon).CommitTransaction();

                    }

                    TreeNode parentNode = treeViewMasterData.SelectedNode.Parent;
                    treeViewMasterData.SelectedNode.Remove();
                    if (parentNode != null)
                    {
                        parentNode.BackColor = SystemColors.Highlight;
                        parentNode.ForeColor = SystemColors.HighlightText;
                        treeViewMasterData.SelectedNode = parentNode;
                    }
                    UpdateControls();
                    UpdateNodeCounts();
                }
            }
            catch (Exception ex)
            {
                TransactionManager.GetInstance(SqlCon).AbortTransaction();
                MessageBox.Show(ex.Message);
            }
        }

        private void pbiTextBoxDistrict_PhoneticChanged(string phonetic)
        {
            LastEntry.BengaliPhoneticText = phonetic;
        }

        private void pbiTextBoxBlock_PhoneticChanged(string phonetic)
        {
            LastEntry.BengaliPhoneticText = phonetic;
        }

        private void pbiTextBoxMouza_PhoneticChanged(string phonetic)
        {
            LastEntry.BengaliPhoneticText = phonetic;
        }

        public class ViewSelector
        {
            private readonly Dictionary<ViewType, Action> viewActions;
            private readonly MasterDataUpdateForm form;

            public ViewType CurrentView
            {
                get;
                private set;
            }

            public ViewSelector(MasterDataUpdateForm form)
            {
                this.form = form;
                viewActions = new Dictionary<ViewType, Action>
                {
                    {ViewType.District, ()=>ConfigureView(showDistrict:true, showCode:true, currentTextBox:form.pbiTextBoxDistrict, controller: form.DistrictController) },
                    {ViewType.Block, ()=>ConfigureView(showBlock:true, showCode:true, showRemove:true, showProjaSt:true, removeText:"Remove District", currentTextBox:form.pbiTextBoxBlock, controller:form.BlockController) },
                    {ViewType.Mouza, ()=>ConfigureView(showMouza:true, showCode:true, showRemove:true, removeText:"Remove Block", currentTextBox:form.pbiTextBoxMouza, controller:form.MouzaController) },
                    {ViewType.RemoveMouza,()=>ConfigureView(showRemove:true, removeText:"Remove Mouza") }
                };
            }
            public void SelectView(ViewType viewType)
            {
                if (!Enum.IsDefined(typeof(ViewType), viewType))
                {
                    throw new ArgumentException($"Invalid view type: {viewType}", nameof(viewType));
                }

                HideAllControls();

                if (viewActions.TryGetValue(viewType, out var action))
                {
                    CurrentView = viewType;
                    action.Invoke();
                }
                else
                {
                    throw new InvalidOperationException($"No action defined for view type: {viewType}");
                }
            }

            private void HideAllControls()
            {
                form.tableLayoutPanelDistrict.Visible = false;
                form.tableLayoutPanelBlock.Visible = false;
                form.tableLayoutPanelMouza.Visible = false;
                form.tableLayoutPanelCode.Visible = false;
                form.buttonRemove.Visible = false;
                form.ShowProjaSotterTypeControls(false);
                form.ControllerSet.DisableAll();
            }

            private void ConfigureView(
            bool showDistrict = false,
            bool showBlock = false,
            bool showMouza = false,
            bool showCode = false,
            bool showRemove = false,
            bool showProjaSt = false,
            string removeText = "",
            PBITextBox currentTextBox = null,
            DataEntryNavigationController controller = null)
            {
                form.tableLayoutPanelDistrict.Visible = showDistrict;
                form.tableLayoutPanelBlock.Visible = showBlock;
                form.tableLayoutPanelMouza.Visible = showMouza;
                form.tableLayoutPanelCode.Visible = showCode;
                form.buttonRemove.Visible = showRemove;
                form.ShowProjaSotterTypeControls(showProjaSt);

                if (showRemove)
                {
                    form.buttonRemove.Text = removeText;
                }

                form.CurrentTextBox = currentTextBox;

                if (controller != null)
                {
                    form.ControllerSet.Enable(controller);
                }

            }

        }

    }
    public class CoustomTag
    {
        public string identity { get; set; }
        public string currentLevel { get; set; }
    }
}
