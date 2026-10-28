using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.InteropServices;
using DockSample.Customization;
using System.IO;
using DockSample;
using NovaNet.Utils;
using NovaNet.wfe;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using LItems;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImageHeaven
{
    public partial class frmDelivery : Form
    {
        OdbcConnection sqlCon = null;
        private INIReader rd = null;
        private KeyValueStruct udtKeyValue;
        public static string location = "";
        DataTable table = new DataTable();

        public frmDelivery(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
            INIFile ini = new INIFile();
            string iniPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + "IhConfiguration.ini";
            location = ini.ReadINI("LOCCONFIG", "LOC", string.Empty, iniPath).Trim().Replace("\0", "");
        }
        private void frmDelivery_Load(object sender, EventArgs e)
        {
            populateBundle();
            button2.Enabled = false;
            //deComboBox1.Items.Insert(0, "No");
            //deComboBox1.Items.Insert(1, "Yes");

            dateTimePicker1.Enabled = false;
            deTextBox1.Enabled = false;

            dateTimePicker1.Text = "";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            //dateTimePicker1.Value = Convert.ToDateTime(handoverDate.ToString());
            deTextBox1.Text = string.Empty;

            dateTimePicker2.Enabled = false;

            dateTimePicker2.Text = "";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            //dateTimePicker1.Value = Convert.ToDateTime(handoverDate.ToString());


            dateTimePicker3.Enabled = false;
            deTextBox2.Enabled = false;

            dateTimePicker3.Text = "";
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy-MM-dd";
            //dateTimePicker1.Value = Convert.ToDateTime(handoverDate.ToString());
            deTextBox2.Text = string.Empty;
        }
        private void populateBundle()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select distinct b.bundle_name,b.bundle_key from detailed_dashboard_master a, bundle_master b where a.bundle_name = b.bundle_name  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                cmbBundle.DataSource = dt;
                cmbBundle.DisplayMember = "bundle_name";
                cmbBundle.ValueMember = "bundle_key";
            }
            else
            {

                cmbBundle.Text = string.Empty;
                cmbBundle.DataSource = null;
                cmbBundle.DisplayMember = "";
                cmbBundle.ValueMember = "";
            }
        }
        public System.Data.DataTable _GetBundleEntries()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select * from detailed_dashboard_master where bundle_name = '" + cmbBundle.Text + "'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        private void cmbBundle_Leave(object sender, EventArgs e)
        {
            table = new DataTable();
            if (cmbBundle.Text != "")
            {
                table = _GetBundleEntries();
                if (table.Rows.Count > 0)
                {
                    groupBox2.Enabled = true;
                    checkBox1.Focus();
                    //dateTimePicker1.Text = "";
                    //dateTimePicker1.Format = DateTimePickerFormat.Custom;
                    //dateTimePicker1.CustomFormat = " ";
                    //dateTimePicker1.Value = Convert.ToDateTime(handoverDate.ToString());
                    deTextBox1.Text = string.Empty;
                    deTextBox2.Text = string.Empty;
                    button2.Enabled = true;

                }
                else
                {
                    groupBox2.Enabled = false;

                    //dateTimePicker1.Text = "";
                    //dateTimePicker1.Format = DateTimePickerFormat.Custom;
                    //dateTimePicker1.CustomFormat = " ";
                    ////dateTimePicker1.Value = Convert.ToDateTime(handoverDate.ToString());
                    deTextBox1.Text = string.Empty;
                    deTextBox1.Text = string.Empty;
                    deTextBox2.Text = string.Empty;
                    button2.Enabled = false;

                }
            }
        }

        private void checkBox1_Click_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                dateTimePicker1.Enabled = true;
                deTextBox1.Enabled = true;
            }
            else
            {
                dateTimePicker1.Enabled = false;
                deTextBox1.Enabled = false;
            }
        }

        private void checkBox2_Click_1(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                dateTimePicker2.Enabled = true;
            }
            else
            {
                dateTimePicker2.Enabled = false;
            }
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                dateTimePicker3.Enabled = true;
                deTextBox2.Enabled = true;
            }
            else
            {
                dateTimePicker2.Enabled = false;
                deTextBox2.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool updateIntoDB(string sl, 
            string delDMS, string delI, string delDate, string delTo, string upFile, string upIm, string upDate, string rInv,
            string challanNo, string challanDate)
        {
            bool commitBol = true;


            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();
            sqlStr = @"update detailed_dashboard_master set  " +
                      "deliveryDMS='" + delDMS + "',deliveredImage = '" + delI + "'," +
                      "deliveryDate='" + delDate + "',deliveredTo='" + delTo + "',uploadedFile='" + upFile + "'," +
                      "uploadedImage='" + upIm + "',uploadDate='" + upDate + "',raiseInvoice='" + rInv + "'," +
                      "challanNo='" + challanNo + "',challanDate='" + challanDate + "' where slno= '" + sl + "'";

            sqlCmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            sqlCmd.CommandText = sqlStr;
            int j = sqlCmd.ExecuteNonQuery();
            if (j > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            bool retVal = false;
            bool upLog = false;

            string dDMS = "";
            string dI = "";
            string dD = "";
            string dT = "";

            string uF = "";
            string uI = "";
            string uD = "";

            string rI = "";
            string cN = "";
            string cD = "";

            if (checkBox1.Checked == true)
            {

                if (dateTimePicker1.Text != "" && deTextBox1.Text != "")
                {
                    retVal = true;
                }
                else
                {
                    retVal = false;
                }

            }
            else { retVal = true; }
            if (checkBox2.Checked == true)
            {

                if (dateTimePicker2.Text != "") { retVal = true; }
                else
                {
                    retVal = false;
                }

            }
            else { retVal = true; }
            if (checkBox3.Checked == true)
            {

                if (dateTimePicker3.Text != "" && deTextBox1.Text != "") { retVal = true; }
                else { retVal = false; }

            }
            else { retVal = true; }
            if (retVal == true)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string sl = table.Rows[i][0].ToString();

                    if (checkBox1.Checked == true)
                    { 
                        dDMS = "Yes";
                        dI = table.Rows[i][16].ToString();
                        dD = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                        dT = deTextBox1.Text.Trim();
                    }
                    else {
                        dDMS = "No";
                        dI = "";
                        dD = "";
                        dT = "";
                    }
                    if (checkBox2.Checked == true)
                    {
                        uF = "Yes";
                        uI = table.Rows[i][16].ToString();
                        uD = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                    }
                    else {
                        uF = "No";
                        uI = "";
                        uD = "";
                    }
                    if (checkBox3.Checked == true)
                    {
                        rI = "Yes";
                        cN = deTextBox2.Text.Trim();
                        cD = dateTimePicker3.Value.ToString("yyyy-MM-dd");
                    }
                    else {
                        rI = "No";
                        cN = "";
                        cD = "";
                    }

                    //update
                    bool updatelog = updateIntoDB(sl, dDMS, dI, dD, dT, uF, uI, uD, rI, cN, cD);
                    if (updatelog == true)
                    {
                        upLog = true;
                        continue;
                    }
                    else
                    {
                        upLog = false;
                        return;
                    }
                }
                if (upLog == true)
                {
                    MessageBox.Show(this, "Status updated successfully...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                button2.Enabled = true;
                return;
            }
        }
    }
}
