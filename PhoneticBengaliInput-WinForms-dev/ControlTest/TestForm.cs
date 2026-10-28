using PhoneticBengaliInput;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ControlTest
{
    public partial class TestForm : Form
    {
        static TestForm()
        {
            AvroBengaliPhoneticParser.Parser.Register();
        }

        public TestForm()
        {
            InitializeComponent();
            foreach (var comboBox in new List<PBIComboBox>() { pbiComboBox1, pbiComboBox2, /*pbiComboBox3*/ })
            {
                comboBox.LanguageItems.Set(new List<LanguageItem>()
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
            }
            pbiTextBox3.Ready += () =>
            {
                pbiTextBox3.Select();
                pbiTextBox3.Focus();
            };
            pbiComboBox1.Ready += () =>
            {
                pbiComboBox1.Enabled = false;
            };
            pbiTextBox2.Suggestions.Set(new List<LanguageItem>
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
        }

        private void btnClearBn_Click(object sender, System.EventArgs e)
        {
            foreach (var control in new List<PBIControl>() { pbiTextBox1, pbiTextBox2, pbiTextBox3, pbiComboBox1, pbiComboBox2, pbiComboBox3 })
            {
                control.BengaliPhoneticText = txtBnFill.Text;
            }
        }

        private void btnClearEn_Click(object sender, System.EventArgs e)
        {
            foreach (var control in new List<PBIControl>() { pbiTextBox1, pbiTextBox2, pbiTextBox3, pbiComboBox1, pbiComboBox2, pbiComboBox3 })
            {
                control.EnglishText = txtEnFill.Text;
            }
        }

        private void pbiComboBox2_InnerSelectedIndexChanged(object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{pbiComboBox2.SelectedIndex} = {pbiComboBox2.BengaliPhoneticText}");
        }

        private void btnDialog_Click(object sender, System.EventArgs e)
        {
            new TestDialogForm().ShowDialog(this);
        }
    }
}
