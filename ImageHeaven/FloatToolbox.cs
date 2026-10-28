using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class FloatToolbox : ToolWindow
    {

    	Button[] btns = new Button[30];
    	NovaNet.Utils.ImageManupulation[] delTest = new NovaNet.Utils.ImageManupulation[30];
    	int btnIndex = 0;
        public FloatToolbox()
        {
            InitializeComponent();
        }

    }
}