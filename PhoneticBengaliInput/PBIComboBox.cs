using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneticBengaliInput
{
    public partial class PBIComboBox : PBIControl
    {
        private LanguageItemCollection DisplayLanguageItems;

        public LanguageItemCollection LanguageItems { get; }

        public event EventHandler InnerSelectedIndexChanged;

        public event EventHandler InnerTextChanged;

        public int _SelectedIndex = -1;

        private bool _ShowLanguageSwitcherButton = true;

        private bool TranslationDone = false;

        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }

            set
            {
                _SelectedIndex = value;
                SetSelectedIndex(LanguageItems, value);
            }
        }

        public LanguageItem SelectedLanguageItem
        {
            get
            {
                return LanguageItems[_SelectedIndex];
            }
        }
        public bool ShowLanguageSwitcherButton
        {
            get
            {
                return _ShowLanguageSwitcherButton;
            }

            set
            {
                _ShowLanguageSwitcherButton = value;
                languageSwitcherBtn.Visible = value;
            }
        }

        public PBIComboBox()
        {
            InitializeComponent();
            phoneticPreviewTxt.Visible = CurrentLanguage != Language.En;
            LanguageItems = new LanguageItemCollection();
            LanguageChangedInternal += this.PBIComboBox_LanguageChanged;
            TextSetInternal += this.PBIComboBox_TextSetInternal;
            PhoneticChangedInternal += this.PBIComboBox_PhoneticChanged;
            TranslatedInternal += this.PBIComboBox_TranslatedInternal;
            LanguageItems.ItemsChanged += async () => await SetDropdownSource(forceTranslate: true);
        }

        private async Task SetDropdownSource(LanguageItemCollection languageItems = null, bool forceTranslate = false)
        {
            await EnsureLanguageItemsTransalated(forceTranslate);
            var langItems = languageItems ?? LanguageItems;
            DisplayLanguageItems = langItems;
            if (CurrentLanguage == Language.En)
            {
                mainComboBox.BeginUpdate();
                mainComboBox.Items.Clear();
                mainComboBox.Items.AddRange(langItems.Select(langItem => langItem.EnglishText).ToArray());
                mainComboBox.EndUpdate();
            }
            else
            {
                mainComboBox.BeginUpdate();
                mainComboBox.Items.Clear();
                mainComboBox.Items.AddRange(langItems.Select(langItem => langItem.BengaliText).ToArray());
                mainComboBox.EndUpdate();
            }
        }

        private async Task SetSelectedIndex(LanguageItemCollection languageItems, int index)
        {
            if (index >= 0 && index < languageItems.Count)
            {
                EnglishText = languageItems[index].EnglishText;
                BengaliPhoneticText = languageItems[index].BengaliPhoneticText;
                await SetDropdownSource();
                if (CurrentLanguage == Language.En)
                {
                    mainComboBox.SelectedIndex = LanguageItems.IndexOf(languageItems[index]);
                }
                else
                {
                    AddTempPhoneticBengaliText();
                    mainComboBox.SelectedIndex = mainComboBox.Items.Count - 1;
                }
            }
            else if (index == -1)
            {
                RemoveTempPhoneticBengaliText();
                Value.EnglishText = string.Empty;
                Value.BengaliPhoneticText = string.Empty;
                mainComboBox.SelectedIndex = -1;
            }
        }

        private async Task EnsureLanguageItemsTransalated(bool forceTranslate = false)
        {
            if (forceTranslate || !TranslationDone)
            {
                foreach (var languageItem in LanguageItems)
                {
                    if (string.IsNullOrEmpty(languageItem.BengaliText))
                    {
                        languageItem.BengaliText = await TranslateToBn(languageItem.BengaliPhoneticText);
                    }
                }
                TranslationDone = true;
            }
        }

        private void AddTempPhoneticBengaliText()
        {
            if (CurrentLanguage == Language.Bn && mainComboBox.FindString(BengaliPhoneticText) == -1)
            {
                mainComboBox.Items.Add(BengaliPhoneticText);
            }
        }

        private void RemoveTempPhoneticBengaliText()
        {
            if (CurrentLanguage == Language.Bn && mainComboBox.FindString(BengaliPhoneticText) != -1)
            {
                mainComboBox.Items.Remove(BengaliPhoneticText);
            }
        }

        private void languageSwitcherBtn_Click(object sender, EventArgs e)
        {
            ToggleLanguage();
        }

        private async void mainComboBox_TextUpdate(object sender, EventArgs e)
        {
            var cursorPosition = mainComboBox.SelectionStart;
            var matchWith = mainComboBox.Text;
            var matchList = CurrentLanguage == Language.En ?
                LanguageItems.Where(matchItem => matchItem.EnglishText.StartsWith(matchWith, StringComparison.InvariantCultureIgnoreCase)) :
                LanguageItems.Where(matchItem => matchItem.BengaliPhoneticText.StartsWith(matchWith, StringComparison.InvariantCultureIgnoreCase));
            await SetDropdownSource(new LanguageItemCollection(matchList));
            mainComboBox.SelectionStart = cursorPosition;
            mainComboBox.DroppedDown = true;
            InnerTextChanged?.Invoke(sender, e);
        }

        private async void mainComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (mainComboBox.SelectedIndex == -1)
            {
                _SelectedIndex = -1;
            }
            else if (mainComboBox.SelectedItem.ToString() != BengaliPhoneticText) // ignore the temporary phonetic item
            {
                _SelectedIndex = LanguageItems.IndexOf(DisplayLanguageItems[mainComboBox.SelectedIndex]);
            }
            await SetSelectedIndex(DisplayLanguageItems, mainComboBox.SelectedIndex);
            InnerSelectedIndexChanged?.Invoke(sender, e);
        }

        private void mainComboBox_DropDown(object sender, EventArgs e)
        {
            RemoveTempPhoneticBengaliText();
        }

        private void mainComboBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == (Keys)LanguageSwitchShortcut)
            {
                ToggleLanguage();
            }
            else if (e.KeyData == (Keys)KeyboardLayoutHelpShortcut)
            {
                ShowKeyboardLayoutHelp();
            }
            else
            {
                TriggerInnerKeyUp(sender, e);
            }
        }

        private void PBIComboBox_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                phoneticPreviewTxt.Font = BengaliFont;
            }
        }

        private async void PBIComboBox_LanguageChanged(Language lang)
        {
            Debug.Assert(lang == CurrentLanguage);
            languageSwitcherBtn.Text = lang.ToString();
            if (CurrentLanguage == Language.En)
            {
                mainComboBox.Text = EnglishText;
                phoneticPreviewTxt.Visible = false;
            }
            else
            {
                AddTempPhoneticBengaliText();
                mainComboBox.Text = BengaliPhoneticText;
                phoneticPreviewTxt.Visible = true;
            }
            await SetDropdownSource();
            TriggerLanguageChanged(lang);
        }

        private void PBIComboBox_TextSetInternal(Language lang, string oldText, string newText)
        {
            if (lang == CurrentLanguage)
            {
                if (LanguageItems.Where(langItem =>
                {
                    switch (lang)
                    {
                        case Language.En:
                            return langItem.EnglishText == newText;
                        case Language.Bn:
                        case Language.BnPhonetic:
                            return langItem.BengaliPhoneticText == newText;
                        default:
                            return false;
                    }
                }).Count() == 0)
                {
                    Value.BengaliPhoneticText = oldText;
                    SelectedIndex = -1;
                }
                else
                {
                    mainComboBox.Text = newText;
                }
            }
        }

        private async void PBIComboBox_PhoneticChanged(string phonetic)
        {
            await EnsureTranslated();
            TriggerPhoneticChanged(phonetic);
        }

        private void PBIComboBox_TranslatedInternal(string translatedBn)
        {
            phoneticPreviewTxt.Text = translatedBn;
            TriggerTranslated(translatedBn);
        }

        protected override async Task OnParserLoaded()
        {
            await SetDropdownSource(forceTranslate: true);
        }

        private async void mainComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up && !mainComboBox.DroppedDown)
            {
                if (_SelectedIndex != -1)
                {
                    _SelectedIndex = _SelectedIndex == 0 ? LanguageItems.Count - _SelectedIndex - 1 : _SelectedIndex - 1;
                    await SetSelectedIndex(LanguageItems, _SelectedIndex);
                    InnerSelectedIndexChanged?.Invoke(sender, e);
                }
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Down && !mainComboBox.DroppedDown)
            {
                if (LanguageItems.Count != 0)
                {
                    _SelectedIndex = (_SelectedIndex + 1) % LanguageItems.Count;
                    await SetSelectedIndex(LanguageItems, _SelectedIndex);
                    InnerSelectedIndexChanged?.Invoke(sender, e);
                }
                e.Handled = true;
            }
            else
            {
                TriggerInnerKeyDown(sender, e);
            }
        }
    }
}
