using System.Windows.Forms;

namespace PhoneticBengaliInput
{
    public partial class PBIKeyboardLayoutHelp : Form
    {
        public PBIKeyboardLayoutHelp()
        {
            InitializeComponent();
        }

        private void PBIKeyboardLayoutHelp_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
