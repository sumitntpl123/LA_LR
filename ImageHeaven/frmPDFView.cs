using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using NovaNet.Utils;
using NovaNet.wfe;
using LItems;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using DataLayerDefs;
using System.Drawing.Drawing2D;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;
using AcroPDFLib;
using AxAcroPDFLib;

namespace ImageHeaven
{
    public partial class frmPDFView : Form
    {
        OdbcConnection sqlCon;

        public frmPDFView(OdbcConnection prmCon)
        {
            
            InitializeComponent();
            sqlCon = prmCon;
            this.Text = "B'Zer -  Barcode Seperator PDF Viewer. . .";
        }


        private void frmPDFView_Load(object sender, EventArgs e)
        {
            string excePath = Path.GetDirectoryName(Application.ExecutablePath);

            if (File.Exists(excePath + "/Barcode_seperator.pdf"))
            {
                string filePdf = excePath + "\\" + "Barcode_seperator.pdf";
                string name = "Barcode_seperator.pdf";
                axAcroPDF1.src = name;
                axAcroPDF1.Name = name;
                axAcroPDF1.LoadFile(name);
            }
        }
    }
}
