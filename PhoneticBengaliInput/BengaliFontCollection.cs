using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.IO;

namespace PhoneticBengaliInput
{
    public class BengaliFontCollection
    {
        private static readonly PrivateFontCollection BengaliFonts;

        private static readonly FontFamily BengaliFontFamily;

        static BengaliFontCollection()
        {
            BengaliFonts = new PrivateFontCollection();
            BengaliFonts.AddFontFile(Path.Combine("Fonts", "NotoSerifBengali-Regular.ttf"));
            if (BengaliFonts.Families.Length > 0)
            {
                BengaliFontFamily = BengaliFonts.Families.First();
            }
        }

        public static Font CreateFont(float sizeInEm, FontStyle style = FontStyle.Regular)
        {
            return new Font(BengaliFontFamily, sizeInEm, style);
        }
    }
}
