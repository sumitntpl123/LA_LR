using PhoneticBengaliInput;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace bzer_data_entry.src
{
    public static class ValidationHelper
    {
        #region regex patterns
        static readonly Regex InvalidPattern = new Regex(@"^\s|\s$|[^a-zA-Z0-9 ^\-:`,$.]");
        static readonly Regex InvalidCodePattern = new Regex(@"^\d+$.");
        static readonly Regex ValidNumberPattern = new Regex(@"^\d+(\.\d+)?$");
        #endregion

        #region constants
        private const string CODE_FIELD_TAG = "code";
        private const int MOUZA_CODE_LENGTH = 3;
        private const int DISTRICT_OR_BLOCK_CODE_LENGTH = 2;
        #endregion

        public static bool ValidateAllInputs(IEnumerable<PBITextBox> textBoxes)
        {
            return textBoxes.All(ValidateInput);
        }

        public static string GetText(PBITextBox textBox)
        {
            return (textBox.EnglishText == string.Empty) ? textBox.BengaliPhoneticText : textBox.EnglishText;
        }
        public static bool ValidateInput(PBITextBox textBox)
        {
            if (textBox.Tag is string tagString && tagString == CODE_FIELD_TAG ||
                textBox.Tag is CoustomTag customTag && customTag.identity == CODE_FIELD_TAG)
            {
                return ValidateCode(textBox);
            }

            string text = GetText(textBox);
            var match = InvalidPattern.Match(text);
            if (match.Success)
            {
                int index = match.Index;
                MessageBox.Show($"Invalid Character found at {index} in {textBox.Tag}: `{text[index]}`", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }

        public static bool ValidateCode(PBITextBox textBox)
        {
        
            var code = GetText(textBox);
            var isMatch = Regex.IsMatch(code, ValidNumberPattern.ToString());
            if (!isMatch)
            {
                MessageBox.Show("Integer field must contain only digits.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox.Tag is CoustomTag coustomTag)
            {
                int requiredDigits = (coustomTag.currentLevel == MetadataStore.FIELD_MOUZA) ? MOUZA_CODE_LENGTH : DISTRICT_OR_BLOCK_CODE_LENGTH;
                if (code.Length != requiredDigits)
                {
                    MessageBox.Show($"Code for {coustomTag.currentLevel} must be exactly {requiredDigits} digits.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

            }
            return true;
        }
    }
}
