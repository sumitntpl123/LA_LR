using System.Collections.Generic;
using System.Windows.Forms;
using PhoneticBengaliInput;

namespace ControlTest
{
    public partial class TestDialogForm : Form
    {
        public TestDialogForm()
        {
            InitializeComponent();
            pbiComboBox1.LanguageItems.Set(new List<LanguageItem>()
                {
                    new LanguageItem()
                    {
                        EnglishText = "North 24 Pgs.",
                        BengaliText = "",
                        BengaliPhoneticText = "uttor 24 porgona"
                    },
                    new LanguageItem()
                    {
                        EnglishText = "North Dinajpur",
                        BengaliText = "",
                        BengaliPhoneticText = "uttor dinajpur"
                    },
                    new LanguageItem()
                    {
                        EnglishText = "Bankura",
                        BengaliText = "",
                        BengaliPhoneticText = "ba^kuRa"
                    },
                    new LanguageItem()
                    {
                        EnglishText = "Bardwan",
                        BengaliText = "",
                        BengaliPhoneticText = "borrdhoman"
                    },
                });

            pbiTextBox1.BengaliPhoneticText = "shyamol";
            pbiComboBox1.SelectedIndex = 1;

            pbiComboBox1.Ready += () =>
            {
                pbiComboBox1.Select();
                pbiComboBox1.Focus();
            };
        }
    }
}
