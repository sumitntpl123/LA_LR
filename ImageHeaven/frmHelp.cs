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
    public partial class frmHelp : Form
    {
        public string currentStage = null;

        public frmHelp(string stage)
        {
            InitializeComponent();

            currentStage = stage;
        }

        private void frmHelp_Load(object sender, EventArgs e)
        {
            if(currentStage == "QC")
            {
                string[] row = { "File Move Up", "F11" };
                var listItem = new ListViewItem(row);
                listView6.Items.Add(listItem);

                string[] row1 = { "File Move Down", "F12" };
                var listItem1 = new ListViewItem(row1);
                listView6.Items.Add(listItem1);

                string[] row2 = { "Today's Production Count", "F9" };
                var listItem2 = new ListViewItem(row2);
                listView6.Items.Add(listItem2);

            }
            if(currentStage == "Index")
            {
                string[] row = { "File Move Up", "F11" };
                var listItem = new ListViewItem(row);
                listView6.Items.Add(listItem);

                string[] row1 = { "File Move Down", "F12" };
                var listItem1 = new ListViewItem(row1);
                listView6.Items.Add(listItem1);

                string[] row2 = { "Today's Production Count", "F9" };
                var listItem2 = new ListViewItem(row2);
                listView6.Items.Add(listItem2);

                string[] row3 = { "Add Image Exception", "Ctrl + C" };
                var listItem3 = new ListViewItem(row3);
                listView6.Items.Add(listItem3);

                string[] row5 = { "Drag Image", "F10" };
                var listItem5 = new ListViewItem(row5);
                listView6.Items.Add(listItem5);
                //string[] row4 = { "Show File Log", "Ctrl + L" };
                //var listItem4 = new ListViewItem(row4);
                //listView6.Items.Add(listItem4);
            }
            if(currentStage == "Fqc")
            {
                string[] row = { "File Move Up", "F11" };
                var listItem = new ListViewItem(row);
                listView6.Items.Add(listItem);

                string[] row1 = { "File Move Down", "F12" };
                var listItem1 = new ListViewItem(row1);
                listView6.Items.Add(listItem1);

                string[] row2 = { "Today's Production Count", "F9" };
                var listItem2 = new ListViewItem(row2);
                listView6.Items.Add(listItem2);

                string[] row3 = { "Add Image Exception", "Ctrl + C" };
                var listItem3 = new ListViewItem(row3);
                listView6.Items.Add(listItem3);

                string[] row4 = { "Show File Log", "Ctrl + L" };
                var listItem4 = new ListViewItem(row4);
                listView6.Items.Add(listItem4);

                string[] row5 = { "Drag Image", "F10" };
                var listItem5 = new ListViewItem(row5);
                listView6.Items.Add(listItem5);
            }

        }
    }
}
