using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneticBengaliInput
{
    public enum Language
    {
        Bn = 1 << 0,
        En = 1 << 1,
        BnPhonetic = 1 << 2
    }

    [Serializable]
    public class LanguageItem
    {
        private readonly Dictionary<Language, string> Values;

        public string EnglishText
        {
            get
            {
                return Values[Language.En];
            }

            set
            {
                Values[Language.En] = value;
            }
        }

        public string BengaliText
        {
            get
            {
                return Values[Language.Bn];
            }

            set
            {
                Values[Language.Bn] = value;
            }
        }

        public string BengaliPhoneticText
        {
            get
            {
                return Values[Language.BnPhonetic];
            }

            set
            {
                Values[Language.BnPhonetic] = value;
            }
        }

        public LanguageItem()
        {
            Values = new Dictionary<Language, string>()
            {
                { Language.En, ""},
                { Language.Bn, ""},
                { Language.BnPhonetic, ""}
            };
        }

        public LanguageItem(LanguageItem other)
        {
            Values = new Dictionary<Language, string>();
            foreach (var item in other.Values)
            {
                Values.Add(item.Key, item.Value);
            }
        }
    }

    [Serializable]
    public class LanguageItemCollection : IEnumerable<LanguageItem>
    {
        private List<LanguageItem> Items;

        public delegate void ItemsOperation();

        public event ItemsOperation ItemsChanged;

        public LanguageItemCollection()
        {
            Items = new List<LanguageItem>();
        }

        public LanguageItemCollection(IEnumerable<LanguageItem> items) : this()
        {
            Set(items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator<LanguageItem> IEnumerable<LanguageItem>.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public LanguageItem this[int index]
        {
            get
            {
                return Items[index];
            }
        }

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }
        public int IndexOf(LanguageItem item)
        {
            return Items.IndexOf(item);
        }

        public void Set(IEnumerable<LanguageItem> items)
        {
            Items.Clear();
            Items.AddRange(items);
            ItemsChanged?.Invoke();
        }
    }

    public class PBIControl : UserControl
    {
        private static readonly Form KeyboardLayoutHelpForm;

        private static bool UnregisteredPhoneticParserError = false;

        private IBengaliPhoneticParser Parser;

        private readonly LanguageItem _Value;

        private Language _CurrentLanguage = Language.En;

        private Font _BengaliFont;

        public delegate void OnLanguageChanged(Language lang);

        protected event OnLanguageChanged LanguageChangedInternal;

        public event OnLanguageChanged LanguageChanged;

        public delegate void OnPhoneticChanged(string phonetic);

        protected event OnPhoneticChanged PhoneticChangedInternal;

        public event OnPhoneticChanged PhoneticChanged;

        public delegate void OnTextSet(Language lang, string oldText, string newText);

        protected event OnTextSet TextSetInternal;

        public event KeyEventHandler InnerKeyUp;

        public event KeyEventHandler InnerKeyDown;

        public event OnPhoneticChanged TranslatedInternal;

        public event OnPhoneticChanged Translated;

        public delegate void OnReady();

        public event OnReady Ready;

        protected Font BengaliFont
        {
            get
            {
                if (_BengaliFont == null)
                {
                    _BengaliFont = BengaliFontCollection.CreateFont(Font == null ? 10 : Font.Size);
                }
                return _BengaliFont;
            }
        }

        public Language CurrentLanguage
        {
            get
            {
                return _CurrentLanguage;
            }

            set
            {
                if (_CurrentLanguage != value && value != Language.BnPhonetic)
                {
                    _CurrentLanguage = value;
                    LanguageChangedInternal(value);
                }
            }
        }

        public LanguageItem Value
        {
            get
            {
                return _Value;
            }
        }

        public string EnglishText
        {
            get
            {
                return _Value.EnglishText;
            }

            set
            {
                var oldText = _Value.EnglishText;
                _Value.EnglishText = value;
                TextSetInternal?.Invoke(Language.En, oldText, value);
            }
        }

        public string BengaliText
        {
            get
            {
                return _Value.BengaliText;
            }
        }

        public string BengaliPhoneticText
        {
            get
            {
                return _Value.BengaliPhoneticText;
            }

            set
            {
                var oldText = _Value.BengaliPhoneticText;
                _Value.BengaliPhoneticText = value;
                TextSetInternal?.Invoke(Language.Bn, oldText, value);
                PhoneticChangedInternal?.Invoke(_Value.BengaliPhoneticText);
            }
        }

        public Shortcut LanguageSwitchShortcut { get; set; }

        public Shortcut KeyboardLayoutHelpShortcut { get; set; }

        static PBIControl()
        {
            KeyboardLayoutHelpForm = new PBIKeyboardLayoutHelp();
        }

        public PBIControl()
        {
            _Value = new LanguageItem();
            LanguageSwitchShortcut = Shortcut.CtrlE;
            KeyboardLayoutHelpShortcut = Shortcut.F1;
            Load += this.PBIControl_Load;
            Enabled = false;
        }

        protected void ToggleLanguage()
        {
            _CurrentLanguage = _CurrentLanguage == Language.En ? Language.Bn : Language.En;
            LanguageChangedInternal(_CurrentLanguage);
        }

        public void ShowKeyboardLayoutHelp()
        {
            if (CurrentLanguage == Language.Bn)
            {
                KeyboardLayoutHelpForm.ShowDialog(ParentForm);
            }
        }

        protected async Task<(string, string)> EnsureTranslated()
        {
            var literalInput = string.IsNullOrEmpty(_Value.BengaliPhoneticText) ?
                    _Value.EnglishText : _Value.BengaliPhoneticText;
            var translatedBn = await TranslateToBn(literalInput);
            TranslatedInternal?.Invoke(translatedBn);
            return (literalInput, translatedBn);
        }

        protected async Task<string> TranslateToBn(string literalInput)
        {
            if (Parser != null)
            {
                var translatedBn = await Parser.TranslateToBengali(literalInput);
                _Value.BengaliText = translatedBn;
                return translatedBn;
            }
            return "";
        }

        protected virtual async Task OnParserLoaded() { }

        protected void TriggerLanguageChanged(Language lang)
        {
            LanguageChanged?.Invoke(lang);
        }

        protected void TriggerPhoneticChanged(string phonetic)
        {
            PhoneticChanged?.Invoke(phonetic);
        }

        protected void TriggerTranslated(string translation)
        {
            Translated?.Invoke(translation);
        }

        protected void TriggerInnerKeyUp(object sender, KeyEventArgs evt)
        {
            InnerKeyUp?.Invoke(sender, evt);
        }

        protected void TriggerInnerKeyDown(object sender, KeyEventArgs evt)
        {
            InnerKeyDown?.Invoke(sender, evt);
        }

        private async void PBIControl_Load(object sender, EventArgs e)
        {
            // Avoid crash trying to load webbrowser at design time
            if (!DesignMode)
            {
                try
                {
                    Parser = await BengaliPhoneticParser.GetInstance();
                    await EnsureTranslated();
                    await OnParserLoaded();
                    Enabled = true;
                    Ready?.Invoke();
                }
                catch (Exception ex)
                {
                    if (!UnregisteredPhoneticParserError)
                    {
                        UnregisteredPhoneticParserError = true;
                        MessageBox.Show(ex.Message, "Error loading phonetic input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
