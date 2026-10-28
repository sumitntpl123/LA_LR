using bzer_data_entry.src;
using NovaNet.Utils;
using PhoneticBengaliInput;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace bzer_data_entry
{
    public partial class AddNewEntry : Form
    {
        private bool columnWidthAdjusting = false;

        private List<LanguageItem> _PlotNumbers;

        private List<LanguageItem> _measurement;

        private List<LanguageItem> _plotType;

        private List<LanguageItem> _district;

        private List<LanguageItem> _Block;

        private List<LanguageItem> _Mauza;

        private List<LanguageItem> _BataPlotNumbers;

        private List<LanguageItem> _plsr;

        private DataEntryNavigationController NavigationController;

        private List<PBITextBox> validationFields;

        private string TAG_FIELD = "code";


        public List<LanguageItem> PlotNumbers
        {
            get => _PlotNumbers;
            set
            {
                _PlotNumbers = value;
                UpdateListView();
            }
        }
        public List<LanguageItem> District
        {
            get => _district;
            set
            {
                _district = value;
                UpdateListView();
            }
        }
        public List<LanguageItem> block
        {
            get => _Block;
            set
            {
                _Block = value;
                UpdateListView();
            }
        }

        public List<LanguageItem> mauza
        {
            get => _Mauza;
            set
            {
                _Mauza = value;
                UpdateListView();
            }
        }

        public List<LanguageItem> Measurement
        {
            get => _measurement;
            set
            {
                _measurement = value;
                UpdateListView();
            }
        }

        public List<LanguageItem> plotType
        {
            get => _plotType;
            set
            {
                _plotType = value;
                UpdateListView();
            }
        }

        public List<LanguageItem> ptsr
        {
            get => _plsr;
            set
            {
                _plsr = value;
                UpdateListView();
            }
        }

        public List<LanguageItem> BataPlotNumbers
        {
            get => _BataPlotNumbers;
            set
            {
                _BataPlotNumbers = value;
                UpdateListView();
            }
        }
        NovaNet.Utils.dbCon dbcon;
        private OdbcConnection SqlConnection { get; set; }
        public AddNewEntry(Language ln)
        {
            dbcon = new NovaNet.Utils.dbCon();
            SqlConnection = dbcon.Connect();
            InitializeComponent();
            _district = new List<LanguageItem>();
            _Block = new List<LanguageItem>();
            _Mauza = new List<LanguageItem>();
            _plotType = new List<LanguageItem>();
            _PlotNumbers = new List<LanguageItem>();
            _BataPlotNumbers = new List<LanguageItem>();
            _plsr = new List<LanguageItem>();
            _measurement = new List<LanguageItem>();

            this.pbiComboBoxDistrict.CurrentLanguage = ln;
            this.cmbBlock.CurrentLanguage = ln;
            this.pbiComboBoxMouza.CurrentLanguage = ln;
            this.pbiTextBoxPlotNumber.CurrentLanguage = ln;
            this.pbiTextBoxBataPlotNumber.CurrentLanguage = ln;
            this.pbiTextBoxMesurement.CurrentLanguage = ln;
            this.txtPTSR.CurrentLanguage = ln;
            this.cmbPlot.CurrentLanguage = ln;
            this.pbiComboBoxDistrict.Tag = TAG_FIELD;
            this.cmbBlock.Tag = TAG_FIELD;

            this.pbiComboBoxMouza.Tag = TAG_FIELD;
            this.pbiTextBoxPlotNumber.Tag = TAG_FIELD;
            this.pbiTextBoxBataPlotNumber.Tag = TAG_FIELD;
            this.pbiTextBoxMesurement.Tag = TAG_FIELD;
            this.txtPTSR.Tag = TAG_FIELD;
            this.cmbPlot.Tag = TAG_FIELD;

            this.pbiComboBoxDistrict.InnerSelectedIndexChanged += this.pbiComboBoxDistrict_Leave;
            this.cmbBlock.InnerSelectedIndexChanged += this.cmbBlock_Leave;

            validationFields = new List<PBITextBox>();
            UpdateListView();
        }
        private List<MasterItem> LoadBlocks(Guid underDistrict)
        {
            var dataStore = new MetadataStore(SqlConnection);
            var blocks = dataStore.GetMasterList(underDistrict, new List<string> { MetadataStore.FIELD_BLOCK });

            cmbBlock.LanguageItems.Set(blocks.Select(block => (LanguageItem)block.Value));
            Blocks = blocks;
            return blocks;
        }
        //private List<MasterItem> LoadBlocks(Guid underDistrict)
        //{
        //    var dataStore = new MetadataStore(SqlConnection);

        //    var blocks = dataStore.GetMasterList(
        //        underDistrict,
        //        new List<string> { MetadataStore.FIELD_DISTRICT });

        //    cmbBlock.LanguageItems.Set(
        //        blocks.Select(block => (LanguageItem)block.Value));

        //    return blocks;
        //}

        //private List<MasterItem> LoadDistricts()
        //{
        //    var dataStore = new MetadataStore(SqlConnection);
        //    var districts = dataStore.GetMasterList(null, new List<string> { MetadataStore.FIELD_DISTRICT });
        //    pbiComboBoxDistrict.LanguageItems.Set(districts.Select(district => (LanguageItem)district.Value));
        //    return districts;
        //}
        private List<MasterItem> LoadMouzas(Guid underBlock)
        {
            var dataStore = new MetadataStore(SqlConnection);

            var mouzas = dataStore.GetMasterList(
                underBlock,
                new List<string> { MetadataStore.FIELD_MOUZA });

            pbiComboBoxMouza.LanguageItems.Set(
                mouzas.Select(mouza => (LanguageItem)mouza.Value));
            return mouzas;
        }
        private List<MasterItem> Districts {  get; set; }
        private List<MasterItem> Blocks { get; set; }
        private List<MasterItem> LoadDistricts()
        {
            var dataStore = new MetadataStore(SqlConnection);

            var districts = dataStore.GetMasterList(
                null,
                new List<string> { MetadataStore.FIELD_DISTRICT });

            pbiComboBoxDistrict.LanguageItems.Set(
                districts.Select(district => (LanguageItem)district.Value));
            Districts = districts;
            return districts;
        }

        //private List<MasterItem> LoadBlocks(Guid underDistrict)
        //{
        //    var dataStore = new MetadataStore(SqlConnection);

        //    var blocks = dataStore.GetMasterList(
        //        underDistrict,
        //        new List<string> { MetadataStore.FIELD_BLOCK });

        //    cmbBlock.LanguageItems.Set(
        //        blocks.Select(block => (LanguageItem)block.Value));

        //    return blocks;
        //}

        //private List<MasterItem> LoadMouzas(Guid underBlock)
        //{
        //    var dataStore = new MetadataStore(SqlConnection);

        //    var mouzas = dataStore.GetMasterList(
        //        underBlock,
        //        new List<string> { MetadataStore.FIELD_MOUZA });

        //    pbiComboBoxMouza.LanguageItems.Set(
        //        mouzas.Select(mouza => (LanguageItem)mouza.Value));

        //    return mouzas;
        //}
        public AddNewEntry(
            Language ln,
            List<LanguageItem> district,
            List<LanguageItem> block,
            List<LanguageItem> mauza,
            List<LanguageItem> plottype,
            List<LanguageItem> plotNumbers,
            List<LanguageItem> bataPlotNumbers,
            List<LanguageItem> ptsr = null,
            List<LanguageItem> measurement = null) : this(ln)
        {
            pbiComboBoxDistrict.Focus();
            LoadDistricts();

            _district = district ?? new List<LanguageItem>();
            _Block = block ?? new List<LanguageItem>();
            _Mauza = mauza ?? new List<LanguageItem>();
            _plotType = plottype ?? new List<LanguageItem>();
            _PlotNumbers = plotNumbers ?? new List<LanguageItem>();
            _BataPlotNumbers = bataPlotNumbers ?? new List<LanguageItem>();
            _plsr = ptsr ?? new List<LanguageItem>();
            _measurement = measurement ?? new List<LanguageItem>();

            UpdateListView();

            InitNavigationController();
        }

        public AddNewEntry(
            Language ln,
            List<LanguageItem> plottype,
            List<LanguageItem> plotNumbers,
            List<LanguageItem> bataPlotNumbers,
            List<LanguageItem> ptsr,
            List<LanguageItem> measurement) : this(ln, new List<LanguageItem>(), new List<LanguageItem>(), new List<LanguageItem>(), plottype, plotNumbers, bataPlotNumbers, ptsr, measurement)
        {
        }
        private void InitNavigationController()
        {
            var controls = new Control[]
            {
                pbiComboBoxDistrict,
                cmbBlock,
                pbiComboBoxMouza,
                cmbPlot,
                pbiTextBoxPlotNumber,
                pbiTextBoxBataPlotNumber,
                txtPTSR,
                pbiTextBoxMesurement,
                buttonAdd,
                buttonOk
            };

            string[] controlNames =
            {
        nameof(pbiComboBoxDistrict),
        nameof(cmbBlock),
        nameof(pbiComboBoxMouza),
        nameof(cmbPlot),
        nameof(pbiTextBoxPlotNumber),
        nameof(pbiTextBoxBataPlotNumber),
        nameof(txtPTSR),
        nameof(pbiTextBoxMesurement),
        nameof(buttonAdd),
        nameof(buttonOk)
    };

            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i] == null)
                {
                    MessageBox.Show(
                        $"Control is NULL: {controlNames[i]}",
                        "Navigation Initialization Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            var controllerBuilder =
                new DataEntryNavigationController.Builder(this);

            for (int i = 0; i < controls.Length; i++)
            {
                controllerBuilder.AddControl(
                    i,
                    controls[i].Name,
                    controls[i]);
            }

            NavigationController = controllerBuilder.Ready();
        }
        //private void InitNavigationController()
        //{
        //    var controllerBuilder = new DataEntryNavigationController.Builder(this);
        //    foreach (var controlIndexPair in new Control[] {
        //        pbiComboBoxDistrict,
        //        cmbBlock,
        //        pbiComboBoxMouza,
        //        cmbPlot,
        //        pbiTextBoxPlotNumber,
        //        pbiTextBoxBataPlotNumber,
        //        txtPTSR,
        //        pbiTextBoxMesurement,
        //        buttonAdd,
        //        buttonOk
        //    }.Select((control, index) => (control, index)))
        //    {
        //        controllerBuilder.AddControl(controlIndexPair.index, controlIndexPair.control.Name, controlIndexPair.control);
        //    }
        //    NavigationController = controllerBuilder.Ready();
        //}

        private LanguageItem GetSelectedLanguageItem(PBIComboBox comboBox)
        {
            if (comboBox == null) return new LanguageItem();
            if (comboBox.Value != null && (!string.IsNullOrEmpty(comboBox.Value.EnglishText) || !string.IsNullOrEmpty(comboBox.Value.BengaliText)))
            {
                return new LanguageItem(comboBox.Value);
            }
            if (comboBox.SelectedIndex >= 0 && comboBox.LanguageItems != null && comboBox.SelectedIndex < comboBox.LanguageItems.Count)
            {
                return new LanguageItem(comboBox.LanguageItems[comboBox.SelectedIndex]);
            }
            if (!string.IsNullOrEmpty(comboBox.EnglishText) && comboBox.LanguageItems != null)
            {
                var match = comboBox.LanguageItems.FirstOrDefault(item => item.EnglishText == comboBox.EnglishText);
                if (match != null) return new LanguageItem(match);
            }
            return comboBox.Value != null ? new LanguageItem(comboBox.Value) : new LanguageItem();
        }

        private string GetItemText(LanguageItem item)
        {
            if (item == null) return "";
            if (!string.IsNullOrEmpty(item.EnglishText)) return item.EnglishText;
            if (!string.IsNullOrEmpty(item.BengaliText)) return item.BengaliText;
            return item.BengaliPhoneticText ?? "";
        }

        private void UpdateListView()
        {
            if (listViewPlotBataPlotNumber == null)
                return;

            listViewPlotBataPlotNumber.Items.Clear();

            int count = new[]
            {
                District?.Count ?? 0,
                block?.Count ?? 0,
                mauza?.Count ?? 0,
                plotType?.Count ?? 0,
                PlotNumbers?.Count ?? 0,
                BataPlotNumbers?.Count ?? 0,
                ptsr?.Count ?? 0,
                Measurement?.Count ?? 0
            }.Max();

            for (int i = 0; i < count; i++)
            {
                string districtText = (District != null && i < District.Count) ? GetItemText(District[i]) : "";
                string blockText = (block != null && i < block.Count) ? GetItemText(block[i]) : "";
                string mauzaText = (mauza != null && i < mauza.Count) ? GetItemText(mauza[i]) : "";
                string plotTypeText = (plotType != null && i < plotType.Count) ? GetItemText(plotType[i]) : "";
                string plotText = (PlotNumbers != null && i < PlotNumbers.Count) ? GetItemText(PlotNumbers[i]) : "";
                string bataText = (BataPlotNumbers != null && i < BataPlotNumbers.Count) ? GetItemText(BataPlotNumbers[i]) : "";
                string ptsrText = (ptsr != null && i < ptsr.Count) ? GetItemText(ptsr[i]) : "";
                string measurementText = (Measurement != null && i < Measurement.Count) ? GetItemText(Measurement[i]) : "";

                string slashText =
                    string.IsNullOrWhiteSpace(bataText) || bataText.Trim() == MetadataStore.INVALID_PLOT_BATA_KHATIAN
                        ? ""
                        : "/";

                ListViewItem item = new ListViewItem(new[]
                {
                    districtText,
                    blockText,
                    mauzaText,
                    plotTypeText,
                    plotText,
                    slashText,
                    bataText,
                    ptsrText,
                    measurementText
                });

                listViewPlotBataPlotNumber.Items.Add(item);
            }

            AdjustColumnWidths();
        }

        private void buttonAdd_Click(object sender, System.EventArgs e)
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

                var distItem = GetSelectedLanguageItem(pbiComboBoxDistrict);
                var blockItem = GetSelectedLanguageItem(cmbBlock);
                var mauzaItem = GetSelectedLanguageItem(pbiComboBoxMouza);
                var plotTypeItem = GetSelectedLanguageItem(cmbPlot);
                var plotNumItem = new LanguageItem(pbiTextBoxPlotNumber.Value);
                var bataPlotItem = new LanguageItem(pbiTextBoxBataPlotNumber.Value);
                var ptsrItem = new LanguageItem(txtPTSR.Value);
                var measurementItem = new LanguageItem(pbiTextBoxMesurement.Value);

                if (listViewPlotBataPlotNumber.SelectedItems.Count > 0)
                {
                    int selectedIndex = listViewPlotBataPlotNumber.SelectedIndices[0];

                    if (selectedIndex < District.Count) District[selectedIndex] = distItem;
                    else District.Add(distItem);

                    if (selectedIndex < block.Count) block[selectedIndex] = blockItem;
                    else block.Add(blockItem);

                    if (selectedIndex < mauza.Count) mauza[selectedIndex] = mauzaItem;
                    else mauza.Add(mauzaItem);

                    if (selectedIndex < plotType.Count) plotType[selectedIndex] = plotTypeItem;
                    else plotType.Add(plotTypeItem);

                    if (selectedIndex < PlotNumbers.Count) PlotNumbers[selectedIndex] = plotNumItem;
                    else PlotNumbers.Add(plotNumItem);

                    if (selectedIndex < BataPlotNumbers.Count) BataPlotNumbers[selectedIndex] = bataPlotItem;
                    else BataPlotNumbers.Add(bataPlotItem);

                    if (selectedIndex < ptsr.Count) ptsr[selectedIndex] = ptsrItem;
                    else ptsr.Add(ptsrItem);

                    if (selectedIndex < Measurement.Count) Measurement[selectedIndex] = measurementItem;
                    else Measurement.Add(measurementItem);
                }
                else
                {
                    District.Add(distItem);
                    block.Add(blockItem);
                    mauza.Add(mauzaItem);
                    plotType.Add(plotTypeItem);
                    PlotNumbers.Add(plotNumItem);
                    BataPlotNumbers.Add(bataPlotItem);
                    ptsr.Add(ptsrItem);
                    Measurement.Add(measurementItem);
                }
                UpdateListView();
            }
            catch (DomainError ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NavigationController.Reset();
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonRemove_Click_1(object sender, System.EventArgs e)
        {
            if (listViewPlotBataPlotNumber.SelectedItems.Count > 0)
            {
                var selectedIndices = listViewPlotBataPlotNumber.SelectedIndices.Cast<int>().OrderByDescending(i => i).ToList();

                selectedIndices.ForEach(index =>
                {
                    if (index < District.Count) District.RemoveAt(index);
                    if (index < block.Count) block.RemoveAt(index);
                    if (index < mauza.Count) mauza.RemoveAt(index);
                    if (index < plotType.Count) plotType.RemoveAt(index);
                    if (index < PlotNumbers.Count) PlotNumbers.RemoveAt(index);
                    if (index < BataPlotNumbers.Count) BataPlotNumbers.RemoveAt(index);
                    if (index < ptsr.Count) ptsr.RemoveAt(index);
                    if (index < Measurement.Count) Measurement.RemoveAt(index);
                });

                UpdateListView();
            }
            else
            {
                MessageBox.Show("Please select atleast one item to remove.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void listViewPlotBataPlotNumber_Resize(object sender, System.EventArgs e)
        {
            if (!columnWidthAdjusting)
            {
                AdjustColumnWidths();
            }
        }

        private void AdjustColumnWidths()
        {
            columnWidthAdjusting = true;
            foreach (ColumnHeader column in listViewPlotBataPlotNumber.Columns)
            {
                column.Width = -2;
            }
            columnWidthAdjusting = false;
        }

        private void ValidateAdd()
        {
            if (string.IsNullOrEmpty(pbiTextBoxPlotNumber.EnglishText))
            {
                throw new DomainError("Please enter Plot number.");
            }
            if (string.IsNullOrEmpty(pbiTextBoxMesurement.EnglishText))
            {
                throw new DomainError("Please enter Measurement.");
            }
            GetValidationFields();
        }

        private void GetValidationFields()
        {
            validationFields.Clear();
            validationFields.Add(pbiTextBoxPlotNumber);
            if (!string.IsNullOrEmpty(pbiTextBoxBataPlotNumber.EnglishText))
            {
                validationFields.Add(pbiTextBoxBataPlotNumber);
            }
            validationFields.Add(pbiTextBoxMesurement);
        }

        private void listViewPlotBataPlotNumber_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listViewPlotBataPlotNumber.SelectedItems.Count > 0)
            {
                int selectedIndex = listViewPlotBataPlotNumber.SelectedIndices[0];
                if (selectedIndex < District.Count)
                {
                    pbiComboBoxDistrict.EnglishText = District[selectedIndex].EnglishText;
                    if (pbiComboBoxDistrict.SelectedIndex >= 0 && Districts != null && pbiComboBoxDistrict.SelectedIndex < Districts.Count)
                    {
                        LoadBlocks(Districts[pbiComboBoxDistrict.SelectedIndex].MasterID);
                    }
                }
                if (selectedIndex < block.Count)
                {
                    cmbBlock.EnglishText = block[selectedIndex].EnglishText;
                    if (cmbBlock.SelectedIndex >= 0 && Blocks != null && cmbBlock.SelectedIndex < Blocks.Count)
                    {
                        LoadMouzas(Blocks[cmbBlock.SelectedIndex].MasterID);
                    }
                }
                if (selectedIndex < mauza.Count)
                {
                    pbiComboBoxMouza.EnglishText = mauza[selectedIndex].EnglishText;
                }
                if (selectedIndex < plotType.Count)
                {
                    cmbPlot.EnglishText = plotType[selectedIndex].EnglishText;
                }
                if (selectedIndex < PlotNumbers.Count)
                {
                    pbiTextBoxPlotNumber.EnglishText = PlotNumbers[selectedIndex].EnglishText;
                }
                if (selectedIndex < BataPlotNumbers.Count)
                {
                    pbiTextBoxBataPlotNumber.EnglishText = BataPlotNumbers[selectedIndex].EnglishText;
                }
                if (selectedIndex < ptsr.Count)
                {
                    txtPTSR.EnglishText = ptsr[selectedIndex].EnglishText;
                }
                if (selectedIndex < Measurement.Count)
                {
                    pbiTextBoxMesurement.EnglishText = Measurement[selectedIndex].EnglishText;
                }
            }
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
        public bool flag = false;
        private void pbiTextBoxPlotNumber_Leave(object sender, System.EventArgs e)
        {
            var text = pbiTextBoxPlotNumber.Value.EnglishText;
            if(text == string.Empty)
            {
                return;
            }
            //if (pbiTextBoxPlotNumber.EnglishText == string.Empty)
            //{
            //    txtPTSR = pbiTextBoxPlotNumber.EnglishText.PadLeft(txtPTSR.ToString(), '-');
            //}


            if (!int.TryParse(text,out _))
            {
                
                MessageBox.Show("Please enter digits only.",
                "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                pbiTextBoxBataPlotNumber.Select();
                pbiTextBoxBataPlotNumber.Focus();
                flag = true;
                SendKeys.Send("+{TAB}");
                return;
            }
            else
            {
                flag = false;
            }
            

        }

        private void pbiTextBoxBataPlotNumber_Leave(object sender, System.EventArgs e)
        {
        
                var text = pbiTextBoxBataPlotNumber.Value.EnglishText;

            if (flag == false)
            {
                if (!int.TryParse(text, out _))
                {
                    MessageBox.Show("Please enter digits only.",
                "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pbiTextBoxBataPlotNumber.Select();
                    pbiTextBoxBataPlotNumber.Focus();
                    return;

                }
            } 
            else
            {
                flag = false;
            }
        }

        private void pbiTextBoxPlotNumber_KeyDown(object sender, KeyEventArgs e)
        {
            

        }

        private void pbiTextBoxBataPlotNumber_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void pbiTextBoxMesurement_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void pbiTextBoxPlotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tableLayoutPanelPlotAddRemove_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddNewEntry_Load(object sender, System.EventArgs e)
        {
            if (Districts == null || Districts.Count == 0)
            {
                LoadDistricts();
            }
            LoadHardcodedPlot();
        }
        private void LoadHardcodedPlot()
        {
            var items = new List<LanguageItem>
            {
                new LanguageItem
                {
                    EnglishText = "CS",
                    BengaliPhoneticText = "CS"
                },
                new LanguageItem
                {
                    EnglishText = "LR",
                    BengaliPhoneticText = "LR"
                },
                new LanguageItem
                {
                    EnglishText = "RS",
                    BengaliPhoneticText = "RS"
                }
            };
            cmbPlot.LanguageItems.Set(items);
            cmbPlot.SelectedIndex = 0;
        }

        private void pbiComboBoxDistrict_Leave(object sender, EventArgs e)
        {
            if (Districts != null && pbiComboBoxDistrict.SelectedIndex >= 0 && pbiComboBoxDistrict.SelectedIndex < Districts.Count)
            {
                LoadBlocks(Districts[pbiComboBoxDistrict.SelectedIndex].MasterID);
            }
        }

        private void cmbBlock_Leave(object sender, EventArgs e)
        {
            if (Blocks != null && cmbBlock.SelectedIndex >= 0 && cmbBlock.SelectedIndex < Blocks.Count)
            {
                LoadMouzas(Blocks[cmbBlock.SelectedIndex].MasterID);
            }
        }

        private void tableLayoutPanelPlot_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

