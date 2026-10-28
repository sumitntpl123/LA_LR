using PhoneticBengaliInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace bzer_data_entry
{
    public partial class AddUserControl : UserControl
    {

        private Language _CurrentLanguage;

        private List<LanguageItem> _PlotNumbers;

        private List<LanguageItem> _BataPlotNumbers;

        private List<LanguageItem> _Mesurement;

        private List<LanguageItem> _pt1;

        private List<LanguageItem> _pt2;

        private List<LanguageItem> _ptsr;

        private List<LanguageItem> _plottype;

        private List<LanguageItem> _plotnumber;

        private List<LanguageItem> _district;

        private List<LanguageItem> _block;

        private List<LanguageItem> _mauza;

        public List<LanguageItem> district
        {
            get => _district;
            set
            {
                _district = value;
                UpdateEntryCounter();
            }
        }
        public List<LanguageItem> block
        {
            get => _block;
            set
            {
                _block = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> mauza
        {
            get => _mauza;
            set
            {
                _mauza = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> pt1
        {
            get => _pt1;
            set
            {
                _pt1 = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> pt2
        {
            get => _pt2;
            set
            {
                _pt2 = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> PlotNumbers
        {
            get => _PlotNumbers;
            set
            {
                _PlotNumbers = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> ptsr
        {
            get => _ptsr;
            set
            {
                _ptsr = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> plottype
        {
            get => _plottype;
            set
            {
                _plottype = value;
                UpdateEntryCounter();
            }
        }
        public List<LanguageItem> Mesurement
        {
            get => _Mesurement;
            set
            {
                _Mesurement = value;
                UpdateEntryCounter();
            }
        }

        public List<LanguageItem> BataPlotNumbers
        {
            get => _BataPlotNumbers;
            set
            {
                _BataPlotNumbers = value;
                UpdateEntryCounter();
            }
        }

        public event EventHandler ItemsCommitted;

        public Language CurrentLanguage
        {
            get { return _CurrentLanguage; }
            set { _CurrentLanguage = value; }
        }

        public AddUserControl()
        {
            InitializeComponent();
            _district = new List<LanguageItem>();
            _block = new List<LanguageItem>();
            _mauza = new List<LanguageItem>();
            _plottype = new List<LanguageItem>();
            _PlotNumbers = new List<LanguageItem>();
            _BataPlotNumbers = new List<LanguageItem>();
            _ptsr = new List<LanguageItem>();
            _Mesurement = new List<LanguageItem>();
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEntry = new AddNewEntry(
                CurrentLanguage,
                district.ToList(),
                block.ToList(),
                mauza.ToList(),
                plottype.ToList(),
                PlotNumbers.ToList(),
                BataPlotNumbers.ToList(),
                ptsr.ToList(),
                Mesurement.ToList()
                );
            Form ownerForm = this.FindForm();
            var result = addEntry.ShowDialog(ownerForm);
            if (result == DialogResult.OK)
            {
                district = addEntry.District;
                block = addEntry.block;
                mauza = addEntry.mauza;
                plottype = addEntry.plotType;
                PlotNumbers = addEntry.PlotNumbers;
                BataPlotNumbers = addEntry.BataPlotNumbers;
                ptsr = addEntry.ptsr;
                Mesurement = addEntry.Measurement;
                UpdateEntryCounter();
                ItemsCommitted?.Invoke(sender, e);
            }
        }

        private void UpdateEntryCounter()
        {
            // Find digits, initially zero inside `(` and `)` and replace with current count value
            btnAdd.Text = Regex.Replace(btnAdd.Text, @"(.*)\((\d+)\)", $@"$1({PlotNumbers.Count})");
        }
    }
}
