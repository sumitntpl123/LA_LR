using System;
using System.Data.Odbc;
using System.IO;
using System.Windows.Forms;
using NovaNet.Utils;
//using AForge.Imaging;
//using AForge;
//using AForge.Imaging.Filters;
//using System.Drawing.Bitmap;
//using System.Drawing.Graphics;
//using Graphics.DrawImage;


namespace ImageHeaven
{
    public partial class frmLocConfig : Form
    {
        OdbcConnection sqlCon = null;

        string iniPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + "IhConfiguration.ini";
        INIFile ini = new INIFile();

        public string location = string.Empty;

        public frmLocConfig(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void find_lPath_sPath()
        {
            if (File.Exists(iniPath) == true)
            {
                string loc = ini.ReadINI("LOCCONFIG", "LOC", string.Empty, iniPath).Trim();
                if (loc == string.Empty || loc == null || loc == "" || loc == "\0")
                {
                    deTextBox1.Text = string.Empty;
                    location = string.Empty;
                }
                else
                {
                    location = loc;
                    deTextBox1.Text = loc;
                }

                if (deTextBox1.Text == "")
                {
                    deButton20.Enabled = false;
                }
                else
                { deButton20.Enabled = true; }
            }
        }
        private void frmPathConfig_Load(object sender, EventArgs e)
        {
            find_lPath_sPath();
            if (deTextBox1.Text == "")
            {
                deButton20.Enabled = false;
            }
            else
            { deButton20.Enabled = true; }
        }

        private void deButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deButton20_Click(object sender, EventArgs e)
        {
            if (deTextBox1.Text != "" || deTextBox1.Text != null)
            {
                int local = ini.WriteINI("LOCCONFIG", "LOC", deTextBox1.Text, iniPath);
            }

            MessageBox.Show(this, "Location configured successfully ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void deTextBox1_Leave(object sender, EventArgs e)
        {
            if (deTextBox1.Text == "")
            {
                deButton20.Enabled = false;
            }
            else
            { deButton20.Enabled = true; }
        }

    }
}
