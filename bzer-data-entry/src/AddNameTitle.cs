using bzer_data_entry.src;
using PhoneticBengaliInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace bzer_data_entry
{
    public partial class AddNameTitle : Form
    {
        private LanguageItem KhatianDefaultValue;

        private LanguageItem BataKhatianDefaultValue;

        private List<LanguageItem> _NameList;

        private List<LanguageItem> _TitleList;

        private List<LanguageItem> _KhatianList;

        private List<LanguageItem> _BataKhatianList;

        private bool columnWidthAdjusting = false;

        private DataEntryNavigationController NavigationController;

        private string KHATIAN_BATAKHATIAN_TAG = "code";

        private List<PBITextBox> validationFields;

        public List<LanguageItem> NameList
        {
            get => _NameList;
            set
            {
                _NameList = value;
                UpdateListView();
            }
        }
        public List<LanguageItem> TitleList
        {
            get => _TitleList;
            set
            {
                _TitleList = value;
                UpdateListView();
            }
        }
        public List<LanguageItem> KhatianList
        {
            get => _KhatianList;
            set
            {
                _KhatianList = value;
                UpdateListView();
            }
        }
        public List<LanguageItem> BataKhatianList
        {
            get => _BataKhatianList;
            set
            {
                _BataKhatianList = value;
                UpdateListView();
            }
        }
        public AddNameTitle()
        {
            InitializeComponent();
            _NameList = new List<LanguageItem>();
            _TitleList = new List<LanguageItem>();
            _KhatianList = new List<LanguageItem>();
            _BataKhatianList = new List<LanguageItem>();
            this.pbiTextBoxName.CurrentLanguage = Language.Bn;
            this.pbiTextBoxTitle.CurrentLanguage = Language.Bn;
            this.pbiTextBoxKhatian.CurrentLanguage = Language.En;
            this.pbiTextBoxBataKhatian.CurrentLanguage = Language.En;

            this.pbiTextBoxName.Tag = $"{MetadataStore.FIELD_SATYA_NAME} or {MetadataStore.FIELD_PROJA_NAME}";
            this.pbiTextBoxTitle.Tag = $"{MetadataStore.FIELD_SATYA_TITLE} or {MetadataStore.FIELD_PROJA_TITLE}";
            this.pbiTextBoxKhatian.Tag = KHATIAN_BATAKHATIAN_TAG;
            this.pbiTextBoxBataKhatian.Tag = KHATIAN_BATAKHATIAN_TAG;
            validationFields = new List<PBITextBox>();
        }

        public AddNameTitle(List<LanguageItem> nameList, List<LanguageItem> titleList, List<LanguageItem> khatianList, List<LanguageItem> bataKhatianList, LanguageItem khatianDefault = null, LanguageItem bataKhatianDefault = null, List<LanguageItem> nameSuggestions = null, List<LanguageItem> titleSuggestions = null) : this()
        {
            SettingValues(nameList, titleList, khatianList, bataKhatianList, khatianDefault, bataKhatianDefault);
        }
        private void SettingValues(List<LanguageItem> nameList, List<LanguageItem> titleList, List<LanguageItem> khatianList, List<LanguageItem> bataKhatianList, LanguageItem khatianDefault = null, LanguageItem bataKhatianDefault = null)
        {
            KhatianDefaultValue = khatianDefault;
            BataKhatianDefaultValue = bataKhatianDefault;
            _NameList = nameList;
            _TitleList = titleList;
            _KhatianList = khatianList;
            _BataKhatianList = bataKhatianList;
            UpdateListView();
            UpdateKhatianandBataKhatianDefaultValue();

            InitNavigationController();
        }
        private void InitNavigationController()
        {
            var controllerBuilder = new DataEntryNavigationController.Builder(this);
            foreach (var controlIndexPair in new Control[] {
                pbiTextBoxName,
                pbiTextBoxTitle,
                pbiTextBoxKhatian,
                pbiTextBoxBataKhatian,
                buttonAdd,
                buttonOk
            }.Select((control, index) => (control, index)))
            {
                controllerBuilder.AddControl(controlIndexPair.index, controlIndexPair.control.Name, controlIndexPair.control);
            }
            NavigationController = controllerBuilder.Ready();
        }

        public void LoadNameTitleSuggestions(List<LanguageItem> nameSuggestions, List<LanguageItem> titleSuggestions)
        {
            if (nameSuggestions != null)
            {
                pbiTextBoxName.Suggestions.Set(nameSuggestions);
            }
            if (titleSuggestions != null)
            {
                pbiTextBoxTitle.Suggestions.Set(titleSuggestions);
            }
        }

        public void ToggleLabelLoadingSuggestions(bool visible)
        {
            labelLoadingSuggestions.Visible = visible;
        }

        private void UpdateKhatianandBataKhatianDefaultValue()
        {
            if (KhatianDefaultValue != null)
            {
                pbiTextBoxKhatian.EnglishText = KhatianDefaultValue.EnglishText;
            }
            if (BataKhatianDefaultValue != null)
            {
                pbiTextBoxBataKhatian.EnglishText = BataKhatianDefaultValue.EnglishText;
            }
        }

        private void UpdateListView()
        {
            listViewNameTitleKhatianBata.Items.Clear();

            var combinedList = NameList.Zip(TitleList, (name, title) => new { Name = name, Title = title })
                                       .Zip(KhatianList, (nt, khatian) => new { nt.Name, nt.Title, Khatian = khatian })
                                       .Zip(BataKhatianList, (ntk, bataKhatian) => new { ntk.Name, ntk.Title, ntk.Khatian, bataKhatian });

            listViewNameTitleKhatianBata.Items.AddRange(combinedList.Select(item => new ListViewItem(new[] { item.Name.BengaliText, item.Title.BengaliText, item.Khatian.EnglishText, item.bataKhatian.EnglishText.Trim() == MetadataStore.INVALID_PLOT_BATA_KHATIAN ? "" : "/", item.bataKhatian.EnglishText })).ToArray());
            AdjustColumnWidths();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateAdd();

                if (validationFields.Count > 0)
                {
                    bool IsValidInput = ValidationHelper.ValidateAllInputs(validationFields);
                    validationFields.Clear();
                    if (!IsValidInput)
                    {
                        return;
                    }
                }

                if (listViewNameTitleKhatianBata.SelectedIndices.Count > 0)
                {
                    int selectedIndex = listViewNameTitleKhatianBata.SelectedIndices[0];

                    NameList[selectedIndex] = new LanguageItem(pbiTextBoxName.Value);
                    TitleList[selectedIndex] = new LanguageItem(pbiTextBoxTitle.Value);
                    KhatianList[selectedIndex] = new LanguageItem(pbiTextBoxKhatian.Value ?? KhatianDefaultValue);
                    BataKhatianList[selectedIndex] = new LanguageItem(pbiTextBoxBataKhatian.Value ?? BataKhatianDefaultValue);
                }
                else
                {
                    NameList.Add(new LanguageItem(pbiTextBoxName.Value));
                    TitleList.Add(new LanguageItem(pbiTextBoxTitle.Value));
                    KhatianList.Add(new LanguageItem(pbiTextBoxKhatian.Value ?? KhatianDefaultValue));
                    BataKhatianList.Add(new LanguageItem(pbiTextBoxBataKhatian.Value ?? BataKhatianDefaultValue));
                }
                UpdateListView();
            }
            catch (DomainError ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NavigationController.Reset();
            UpdateKhatianandBataKhatianDefaultValue();
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            var unselectedItemsWithIndex = listViewNameTitleKhatianBata.Items.Cast<ListViewItem>()
                .Select((item, index) => new { item, index })
                .Where(element => !element.item.Selected).ToList();

            _NameList = unselectedItemsWithIndex.Select(element => NameList.ElementAt(element.index)).ToList();
            _TitleList = unselectedItemsWithIndex.Select(element => TitleList.ElementAt(element.index)).ToList();
            _KhatianList = unselectedItemsWithIndex.Select(element => KhatianList.ElementAt(element.index)).ToList();
            _BataKhatianList = unselectedItemsWithIndex.Select(element => BataKhatianList.ElementAt(element.index)).ToList();

            UpdateListView();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void listViewNameTitleKhatianBata_Resize(object sender, EventArgs e)
        {
            if (!columnWidthAdjusting)
            {
                AdjustColumnWidths();
            }
        }

        private void AdjustColumnWidths()
        {
            columnWidthAdjusting = true;
            foreach (ColumnHeader column in listViewNameTitleKhatianBata.Columns)
            {
                column.Width = -2;
            }
            columnWidthAdjusting = false;
        }

        private void ValidateAdd()
        {
            if (string.IsNullOrWhiteSpace(pbiTextBoxName.BengaliText))
            {
                throw new DomainError("Please enter Name.");
            }
            if (string.IsNullOrWhiteSpace(pbiTextBoxKhatian.EnglishText) && !string.IsNullOrWhiteSpace(pbiTextBoxBataKhatian.EnglishText))
            {
                throw new DomainError("Bata khatian number can't be present without khatian number");
            }
            GetValidationFields();
        }
        private void GetValidationFields()
        {
            var allPbiTextBoxes = new[]
            {
                pbiTextBoxTitle,
                pbiTextBoxKhatian,
                pbiTextBoxBataKhatian
            };
            validationFields.Add(pbiTextBoxName);
            validationFields.AddRange(
                allPbiTextBoxes.Where(x => !string.IsNullOrEmpty(x.EnglishText)
                    || !string.IsNullOrWhiteSpace(x.BengaliPhoneticText)
                )
            );
        }

        private void listViewNameTitleKhatianBata_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewNameTitleKhatianBata.SelectedIndices.Count > 0)
            {
                int selectedIndex = listViewNameTitleKhatianBata.SelectedIndices[0];

                pbiTextBoxName.BengaliPhoneticText = NameList[selectedIndex].BengaliPhoneticText;
                pbiTextBoxTitle.BengaliPhoneticText = TitleList[selectedIndex].BengaliPhoneticText;
                pbiTextBoxKhatian.EnglishText = KhatianList[selectedIndex].EnglishText;
                pbiTextBoxBataKhatian.EnglishText = BataKhatianList[selectedIndex].EnglishText;
            }
        }

        public void UpdateValues(List<LanguageItem> nameList, List<LanguageItem> titleList, List<LanguageItem> khatianList, List<LanguageItem> bataKhatianList, LanguageItem khatianDefault = null, LanguageItem bataKhatianDefault = null)
        {
            SettingValues(nameList, titleList, khatianList, bataKhatianList, khatianDefault, bataKhatianDefault);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var parentEntryForm = this.Owner as EntryForm;
            switch (keyData)
            {
                case Keys.Add:
                    parentEntryForm.zoomIn();
                    return true;

                case Keys.Subtract:
                    parentEntryForm.zoomOut();
                    return true;

                case Keys.Right:
                    parentEntryForm.moveNextImage();
                    return true;

                case Keys.Left:
                    parentEntryForm.moveBackImage();
                    return true;

                case Keys.Control | Keys.R:
                    parentEntryForm.RotateRight();
                    return true;

                case Keys.Control | Keys.L:
                    parentEntryForm.RotateLeft();
                    return true;

                case Keys.Control | Keys.Up:
                    parentEntryForm.ImageMoveUp();
                    return true;

                case Keys.Control | Keys.Down:
                    parentEntryForm.ImageMoveDown();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}