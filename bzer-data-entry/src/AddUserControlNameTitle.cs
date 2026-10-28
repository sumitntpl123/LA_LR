using PhoneticBengaliInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace bzer_data_entry
{
    public partial class AddUserControlNameTitle : UserControl
    {
        private List<LanguageItem> _NameItems;

        private List<LanguageItem> _TitleItems;

        private List<LanguageItem> _KhatianItems;

        private List<LanguageItem> _BataKhatianItems;

        public List<LanguageItem> NameItems
        {
            get => _NameItems;
            set
            {
                _NameItems = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> TitleItems
        {
            get => _TitleItems;
            set
            {
                _TitleItems = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> KhatianItems
        {
            get => _KhatianItems;
            set
            {
                _KhatianItems = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> BataKhatianItems
        {
            get => _BataKhatianItems;
            set
            {
                _BataKhatianItems = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> NameSuggestions { get; set; }

        public List<LanguageItem> TitleSuggestions { get; set; }

        public LanguageItem KhatianDefaultValue { get; set; }
        public LanguageItem BataKhatianDefaultValue { get; set; }

        public event EventHandler ItemsCommitted;

        private AddNameTitle NameTitle = null;

        public AddUserControlNameTitle()
        {
            InitializeComponent();
            _NameItems = new List<LanguageItem>();
            _TitleItems = new List<LanguageItem>();
            _KhatianItems = new List<LanguageItem>();
            _BataKhatianItems = new List<LanguageItem>();
            NameSuggestions = new List<LanguageItem>();
            TitleSuggestions = new List<LanguageItem>();

            NameTitle = new AddNameTitle(
                NameItems.ToList(),
                TitleItems.ToList(),
                KhatianItems.ToList(),
                BataKhatianItems.ToList(),
                KhatianDefaultValue,
                BataKhatianDefaultValue
            );
        }

        public void PushSuggestions(List<LanguageItem> nameSuggestions, List<LanguageItem> titleSuggestions)
        {
            NameTitle.LoadNameTitleSuggestions(nameSuggestions, titleSuggestions);
        }
        public void ToggleLabelLoadingSuggestions(bool visible)
        {
            NameTitle.ToggleLabelLoadingSuggestions(visible);
        }
        private void addNameTitle_Click(object sender, EventArgs e)
        {
            NameTitle.UpdateValues(NameItems.ToList(),
                TitleItems.ToList(),
                KhatianItems.ToList(),
                BataKhatianItems.ToList(),
                KhatianDefaultValue,
                BataKhatianDefaultValue);
            Form ownerForm = this.FindForm();
            var result = NameTitle.ShowDialog(ownerForm);
            if (result == DialogResult.OK)
            {
                NameItems = NameTitle.NameList;
                TitleItems = NameTitle.TitleList;
                KhatianItems = NameTitle.KhatianList;
                BataKhatianItems = NameTitle.BataKhatianList;
                UpdateEntryCounter();
                ItemsCommitted?.Invoke(sender, e);
            }
        }

        private void UpdateEntryCounter()
        {
            // Find digits, initially zero inside `(` and `)` and replace with current count value
            addNameTitle.Text = Regex.Replace(addNameTitle.Text, @"(.*)\((\d+)\)", $@"$1({NameItems.Count})");
        }
    }
}

