using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHeaven
{
    public partial class frmTargetIndexForMove : Form
    {
        public delegate void GetIndex(Nullable<int> idx);

        GetIndex callBack;

        public frmTargetIndexForMove(GetIndex idxFx)
        {
            InitializeComponent();
            callBack = idxFx;
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            bool parseResult = int.TryParse(deIndex.Text, out int parsedVal);
            Nullable<int> retVal = null;
            if (parseResult) {
                retVal = parsedVal;
            }

            this.Close();
            callBack(retVal);
        }

        private void frmTargetIndexForMove_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) {
                this.Close();
                callBack(null);
            }
        }
    }
}
