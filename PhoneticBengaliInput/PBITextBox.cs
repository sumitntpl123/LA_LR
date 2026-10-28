using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneticBengaliInput
{
    public partial class PBITextBox : PBIControl
    {
        private const char AutoCompleteDelim = '\u00a0';

        private bool _ShowLanguageSwitcherButton = true;

        private bool _ShowSuggestions = false;

        public event System.EventHandler InnerTextChanged;

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

        public bool ShowSuggestions
        {
            get => _ShowSuggestions;

            set
            {
                _ShowSuggestions = value;
                SetupSuggestions(value);
            }
        }

        public LanguageItemCollection Suggestions { get; }

        public PBITextBox()
        {
            InitializeComponent();
            Suggestions = new LanguageItemCollection();
            phoneticPreviewTxt.Visible = CurrentLanguage != Language.En;
            LanguageChangedInternal += this.PBITextBox_LanguageChanged;
            TextSetInternal += this.PBITextBox_TextSetInternal;
            PhoneticChangedInternal += this.PBITextBox_PhoneticChanged;
            TranslatedInternal += this.PBITextBox_TranslatedInternal;
        }

        private void SetupSuggestions(bool showSuggestions = false)
        {
            if (showSuggestions)
            {
                mainTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                mainTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                Suggestions.ItemsChanged += PopulateSuggestions;
                LanguageChanged += PopulateSuggestions;
                Ready += PopulateSuggestions;
            }
            else
            {
                mainTextBox.AutoCompleteSource = AutoCompleteSource.None;
                mainTextBox.AutoCompleteMode = AutoCompleteMode.None;
                Suggestions.ItemsChanged -= PopulateSuggestions;
                LanguageChanged -= PopulateSuggestions;
                Ready -= PopulateSuggestions;
            }
        }

        private async void PopulateSuggestions()
        {
            await PopulateSuggestionsHelper(CurrentLanguage);
        }

        private async void PopulateSuggestions(Language currentLang)
        {
            await PopulateSuggestionsHelper(currentLang);
        }

        private async Task PopulateSuggestionsHelper(Language currentLang = Language.En)
        {
            await EnsureLanguageItemsTransalated();
            var suggestions = new AutoCompleteStringCollection();
            suggestions.AddRange(Suggestions
                .Select(sug => currentLang == Language.En ? sug.EnglishText : $"{sug.BengaliPhoneticText}{AutoCompleteDelim}({sug.BengaliText})")
                .ToArray());
            mainTextBox.AutoCompleteCustomSource = suggestions;
        }

        private async Task EnsureLanguageItemsTransalated()
        {
            foreach (var languageItem in Suggestions)
            {
                if (string.IsNullOrEmpty(languageItem.BengaliText))
                {
                    languageItem.BengaliText = await TranslateToBn(languageItem.BengaliPhoneticText);
                }
            }
        }

        private void PBITextBox_Load(object sender, System.EventArgs e)
        {
            if (!DesignMode)
            {
                phoneticPreviewTxt.Font = BengaliFont;
            }
        }

        private void languageSwitcherBtn_Click(object sender, System.EventArgs e)
        {
            ToggleLanguage();
        }

        private void mainTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TriggerInnerKeyDown(sender, e);
        }

        private void mainTextBox_KeyUp(object sender, KeyEventArgs e)
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

        private void mainTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if (CurrentLanguage == Language.En)
            {
                EnglishText = mainTextBox.Text;
            }
            else
            {
                BengaliPhoneticText = mainTextBox.Text.Split(AutoCompleteDelim)[0];
            }
            InnerTextChanged?.Invoke(this, e);
        }

        private async void PBITextBox_LanguageChanged(Language lang)
        {
            Debug.Assert(lang == CurrentLanguage);
            languageSwitcherBtn.Text = lang.ToString();
            if (lang == Language.Bn)
            {
                var (literalInput, _) = await EnsureTranslated();
                mainTextBox.Text = literalInput;
                BengaliPhoneticText = literalInput;
                phoneticPreviewTxt.Visible = true;
            }
            else
            {
                mainTextBox.Text = EnglishText;
                phoneticPreviewTxt.Visible = false;
            }
            TriggerLanguageChanged(lang);
        }

        private void PBITextBox_TextSetInternal(Language lang, string oldText, string newText)
        {
            if (lang == CurrentLanguage)
            {
                mainTextBox.Text = newText;
            }
        }

        private async void PBITextBox_PhoneticChanged(string phonetic)
        {
            var (_, translatedBn) = await EnsureTranslated();
            TriggerPhoneticChanged(phonetic);
        }

        private void PBITextBox_TranslatedInternal(string translatedBn)
        {
            phoneticPreviewTxt.Text = translatedBn;
            TriggerTranslated(translatedBn);
        }
    }
}
