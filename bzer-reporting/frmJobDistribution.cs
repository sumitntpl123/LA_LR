using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NovaNet.Utils;
using NovaNet.wfe;
using LItems;
using System.Data.Odbc;
using Microsoft;
using Microsoft.Office;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace Reporting
{
    public partial class frmJobDistribution : Form
    {
        wfePolicy wPolicy = null;
        OdbcConnection sqlCon;
        public static string projKey = null;
        public static string bundleKey = null;
        public static string boxNumber = null;

        public OdbcDataAdapter sqlAdap = null;

        public string err = null;
        private int projCode;
        //MemoryStream stateLog;
        byte[] tmpWrite;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, NovaNet.Utils.Constants._MAIL_TO, NovaNet.Utils.Constants._MAIL_FROM, NovaNet.Utils.Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	

        public frmJobDistribution(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
            init();
        }

        public AutoCompleteStringCollection GetSuggestions(string tblName, string fldName)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct " + fldName + " from " + tblName;
            DataSet ds = new DataSet();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    x.Add(ds.Tables[0].Rows[i][0].ToString().Trim());
                }
            }
            //x.Add("Others");
            //x.Add("NA");
            return x;
        }
        private void init()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntries();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();
            
            this.textBox2.Text = "";
            this.textBox2.Focus();


            label1.Text = "Total Bundle : " + Dt.Rows.Count;
        }

        public string checkCount(int status)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select Count(*) from bundle_master where status = '"+status+"'";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }


        public void initSummary()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Metadata Entry");
            dt.Columns.Add("Upload");
            dt.Columns.Add("Scan");
            dt.Columns.Add("QC");
            dt.Columns.Add("Doc Type Association");
            dt.Columns.Add("FQC");
            dt.Columns.Add("Submission");
            dt.Columns.Add("UAT");
            dt.Columns.Add("Export");


            dt.Rows.Add(checkCount(0), checkCount(1), checkCount(2), checkCount(3), checkCount(4), checkCount(5), checkCount(6), checkCount(7), checkCount(8));


            grdAll.DataSource = dt;

            FormatDataGridViewSummary();

            this.grdAll.Refresh();

        }


        private void initMeta()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesMeta();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Pending for Metadata Entry : " + Dt.Rows.Count;
        }

        private void initUpload()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesUpload();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Pending for Upload : " + Dt.Rows.Count;
        }

        private void initScan()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesScan();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Pending for Scan : " + Dt.Rows.Count;
        }

        private void initQc()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesQc();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Pending for QC : " + Dt.Rows.Count;

        }

        private void initIndex()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesIndex();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Pending for DOC Type Association :" + Dt.Rows.Count;
        }

        private void initSubmission()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesSubmission();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Pending for Submission : " + Dt.Rows.Count;

        }

        private void initCert()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesCert();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Pending for Certification : " + Dt.Rows.Count;
        }

        private void initPeExport()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesPeExport();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Pending for Export : " + Dt.Rows.Count;
        }

        private void initExport()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt = _GetEntriesExport();


            dtGrdVol.DataSource = Dt;


            FormatDataGridView();

            this.dtGrdVol.Refresh();

            this.textBox2.Text = "";
            this.textBox2.Focus();

            label1.Text = "Total Bundle Exported : " + Dt.Rows.Count;

        }

        private void FormatDataGridView()
        {
            //Format the Data Grid View
            dtGrdVol.Columns[0].Visible = false;
            dtGrdVol.Columns[1].Visible = false;
            //dtGrdVol.Columns[2].Visible = false;
            //Format Colors


            //Set Autosize on for all the columns
            for (int i = 0; i < dtGrdVol.Columns.Count; i++)
            {
                dtGrdVol.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


        }

        private void FormatDataGridViewSummary()
        {
            //Format the Data Grid View
            //grdAll.Columns[0].Visible = false;
            //grdAll.Columns[1].Visible = false;
            //dtGrdVol.Columns[2].Visible = false;
            //Format Colors


            //Set Autosize on for all the columns
            for (int i = 0; i < grdAll.Columns.Count; i++)
            {
                grdAll.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


        }


        public System.Data.DataTable _GetEntries()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesMeta()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where status = 0 and bundle_key not in (select bundle_key from metadata_entry) group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesUpload()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where status = 0 and bundle_key in (select bundle_key from metadata_entry) group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesScan()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where status = 1 group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesQc()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where status = 2 group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesIndex()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where status = 3 group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesSubmission()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where (status = 5 or status = 4) group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesCert()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where status = 6 group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesPeExport()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where status = 7 group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetEntriesExport()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select  distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where status = 8 group by proj_code,bundle_key";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void frmJobDistribution_Load(object sender, EventArgs e)
        {
            //PopulateProjectCombo();
            //PopulateBatchCombo();
            deButton20.Enabled = false;

            this.deTextBox1.AutoCompleteCustomSource = GetSuggestions("case_file_master", "case_file_no");
            this.deTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.textBox2.AutoCompleteCustomSource = GetSuggestions("bundle_master", "bundle_no");
            this.textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            deComboBox1.Focus();
            deComboBox1.Select();

            deComboBox1.SelectedIndex = 0;


            //dtGrdVol_DoubleClick(sender, e);
            //GenTreeView();

            initSummary();
        }
     
        private void PopulateView(int prmProjKey,int prmBatchKey,int prmBox)
        {
            DataSet dsAwtJobCrt = new DataSet();
            DataSet dsAwtAdf = new DataSet();
            DataSet dsSub = new DataSet();
            DataSet dsCert = new DataSet();
            DataSet dsAwtQc = new DataSet();
            DataSet dsAwtIndex = new DataSet();
            DataSet dsIndexed = new DataSet();
            DataSet dslicExcp = new DataSet();
            DataSet dsOnHold = new DataSet();
            DataSet dsMissing = new DataSet();
            //DataSet dsAwtQc = new DataSet();
            DataSet dsIndexIncom = new DataSet();
            DataSet dsExport = new DataSet();
            DataSet dsIncomplete = new DataSet();
            DataSet dsImgIncomplete = new DataSet();
            DataSet dsAwtExport = new DataSet();
            CtrlPolicy pPolicy = null;
            wfeBox tmpBox = new wfeBox(sqlCon);
            System.Data.DataTable dt = new System.Data.DataTable();
            DataSet imageCount = new DataSet();
            DataRow dr;
            NovaNet.wfe.eSTATES[] policyState;
            NovaNet.wfe.eSTATES[] policyStateIndexed = new NovaNet.wfe.eSTATES[2];
            dt.Columns.Add("Sl No");
            dt.Columns.Add("Awt_Upload");
            dt.Columns.Add("Awt_Scan");
            dt.Columns.Add("Awt_QC");
            dt.Columns.Add("Awt_Doc_Association");
            dt.Columns.Add("Awt_Submission");
            dt.Columns.Add("Awt_Certification");
            dt.Columns.Add("CHCException");
            dt.Columns.Add("Entry_Incomplete");
            dt.Columns.Add("Image_Incomplete");
            dt.Columns.Add("Awt_Export");
            //dt.Columns.Add("Index incomplete");
            
            //dt.Columns.Add("On Hold");
            //dt.Columns.Add("Missing");
            
            dt.Columns.Add("Exported");
			try
			{
                //pPolicy = new CtrlPolicy(Convert.ToInt32(prmProjKey), Convert.ToInt32(prmBatchKey), prmBox, 0);
                pPolicy = new CtrlPolicy(Convert.ToInt32(prmProjKey), Convert.ToInt32(prmBatchKey), "1");
	            wPolicy = new wfePolicy(sqlCon, pPolicy);
                //populate dataset for job creation
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_INITIALIZED;
                dsAwtJobCrt = wPolicy.GetPolicyList1(policyState);

                //populate dataset for awt adf
                policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_CREATED;
                dsAwtAdf = wPolicy.GetPolicyList2(policyState);

               
	            //populate dataset for awt qc
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_SCANNED;
                dsAwtQc = wPolicy.GetPolicyList3(policyState);

	            
	            //populate dataset for awt index
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_QC;
                dsAwtIndex = wPolicy.GetPolicyList4(policyState);

                //awt submission
                policyState = new NovaNet.wfe.eSTATES[3];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                dsSub = wPolicy.GetPolicyList11(policyState);

                //populate dataset for certification
                policyState = new NovaNet.wfe.eSTATES[3];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                dsCert = wPolicy.GetPolicyList5(policyState);

                //populate dataset for chc Exception
                policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                dslicExcp = wPolicy.GetPolicyList6(policyState);
	            
	            //populate dataset for on hold
                //policyState = new NovaNet.wfe.eSTATES[1];
                //policyState[0] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
                //dsOnHold = wPolicy.GetPolicyList7(policyState);
	
                //policyState = new NovaNet.wfe.eSTATES[1];
                //policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
                //dsMissing = wPolicy.GetPolicyList8(policyState);

                //entry incomplete
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
                dsIncomplete = wPolicy.GetPolicyList12(policyState);

                //image incomplete
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
                dsImgIncomplete = wPolicy.GetPolicyList13(policyState);

                // awt export
                policyState = new NovaNet.wfe.eSTATES[1];
                policyState[0] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
                dsAwtExport = wPolicy.GetPolicyList9(policyState);

                //exported
                policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
                dsExport = wPolicy.GetPolicyList10(policyState);
	            
                

                for (int i = 0; i < wPolicy.GetPolicyCount().Tables[0].Rows.Count; i++)
	            {
                    deButton20.Enabled = true;

	                dr = dt.NewRow();
	                dr["Sl No"] = i+1;
	
	                
	
	                if((i+1)<=dsAwtJobCrt.Tables[0].Rows.Count)
	                {
                        dr["Awt_Upload"] = dsAwtJobCrt.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
                        dr["Awt_Upload"] = string.Empty;
	                }
	                
	                if((i+1)<=dsAwtAdf.Tables[0].Rows.Count)
	                {
                        dr["Awt_Scan"] = dsAwtAdf.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
                        dr["Awt_Scan"] = string.Empty;
	                }
					if((i+1)<=dsAwtQc.Tables[0].Rows.Count)
	                {
	                	dr["Awt_QC"] = dsAwtQc.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Awt_QC"] = string.Empty;
	                }
	                if((i+1)<=dsAwtIndex.Tables[0].Rows.Count)
	                {
	                	dr["Awt_Doc_Association"] = dsAwtIndex.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Awt_Doc_Association"] = string.Empty;
	                }
	                if((i+1)<= dsSub.Tables[0].Rows.Count)
	                {
                        dr["Awt_Submission"] = dsSub.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
                        dr["Awt_Submission"] = string.Empty;
	                }
                    if ((i + 1) <= dsCert.Tables[0].Rows.Count)
                    {
                        dr["Awt_Certification"] = dsCert.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_Certification"] = string.Empty;
                    }
                    if ((i + 1) <= dslicExcp.Tables[0].Rows.Count)
                    {
                        dr["CHCException"] = dslicExcp.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["CHCException"] = string.Empty;
                    }
                    if ((i + 1) <= dsIncomplete.Tables[0].Rows.Count)
                    {
                        dr["Entry_Incomplete"] = dsIncomplete.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Entry_Incomplete"] = string.Empty;
                    }
                    if ((i + 1) <= dsImgIncomplete.Tables[0].Rows.Count)
                    {
                        dr["Image_Incomplete"] = dsImgIncomplete.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Image_Incomplete"] = string.Empty;
                    }
                    if ((i + 1) <= dsAwtExport.Tables[0].Rows.Count)
                    {
                        dr["Awt_Export"] = dsAwtExport.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {
                        dr["Awt_Export"] = string.Empty;
                    }
	                

	                
	                if((i+1)<=dsExport.Tables[0].Rows.Count)
	                {
	                	dr["Exported"] = dsExport.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Exported"] = string.Empty;
	                }
	                dt.Rows.Add(dr);
	            }
	            grdStatus.DataSource = dt;
	            grdStatus.ForeColor = Color.Black;

                //Set Autosize on for all the columns
                for (int i = 0; i < grdStatus.Columns.Count; i++)
                {
                    grdStatus.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
			catch(Exception ex)
			{
                string err = ex.Message;
				MessageBox.Show("Error while populating the file list......"+err);
			}
        }
        
        private void frmJobDistribution_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //PopulateView();
                init();
                initSummary();
            }
        }

        void TrvNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int batchKey =0;
            int projectKey=0;
            try
            {
                DataSet ds = new DataSet();
                if (e.Node.Tag.ToString() != "Box")
                {
                    batchKey = Convert.ToInt32(e.Node.Tag.ToString());
                    projectKey = Convert.ToInt32(e.Node.Parent.Tag.ToString());
                }
                NovaNet.wfe.eSTATES[] policyStateIndexed = new NovaNet.wfe.eSTATES[2];

                if (e.Node.Tag.ToString() == "Box")
                {
                    batchKey = Convert.ToInt32(e.Node.Parent.Tag.ToString());
                    projectKey = Convert.ToInt32(e.Node.Parent.Parent.Tag.ToString());
                    int boxNumber = Convert.ToInt32(e.Node.Text);
                    PopulateView(projectKey, batchKey, boxNumber);
                }
                CtrlBox ctrlBox = new CtrlBox(projectKey,batchKey, "1");
                wfeBox box = new wfeBox(sqlCon, ctrlBox);
                lblTotalImageScanned.Text = box.GetTotalImageCount().ToString();
            }
            catch
            { }
        }

        public System.Data.DataTable _GetResultBundle(string bundle_no)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where bundle_no like '%" + bundle_no + "%' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetResultBundle(string proj, string bundle)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct proj_code,bundle_key,bundle_name as 'Bundle Name',bundle_no as 'Bundle Number' from bundle_master where proj_code = '"+proj+"' and bundle_key = '"+bundle+"' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public System.Data.DataTable _GetResultBundleFile(string fileno)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string sql = "select distinct a.proj_code,a.bundle_key,a.bundle_name as 'Bundle Name',a.bundle_no as 'Bundle Number' from bundle_master a, case_file_master b where b.case_file_no like '%" + fileno + "%' and a.proj_code = b.proj_code and a.bundle_key = b.bundle_key ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            
            string bundle_no = textBox2.Text;

            dtGrdVol.DataSource = null;
            System.Data.DataTable Dt = new System.Data.DataTable();

            if (bundle_no != null)
            {
                Dt = _GetResultBundle(bundle_no);

                dtGrdVol.DataSource = Dt;

                FormatDataGridView();

                this.dtGrdVol.Refresh();
                this.textBox2.Focus();
            }
            else
            {
                init();
            }
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            string bundle_status = deComboBox1.Text;

            if(bundle_status == "" || bundle_status == string.Empty || string.IsNullOrEmpty(bundle_status) || bundle_status == "All Bundle")
            {
                init();
            }
            if(bundle_status == "Pending for Metadata Entry")
            {
                initMeta();
            }
            if (bundle_status == "Pending for Upload")
            {
                initUpload();
            }
            if(bundle_status == "Pending for Scan")
            {
                initScan();
            }
            if(bundle_status == "Pending for QC")
            {
                initQc();
            }
            if (bundle_status == "Pending for DOC Type Association")
            {
                initIndex();
            }
            if (bundle_status == "Pending for Submission")
            {
                initSubmission();
            }
            if (bundle_status == "Pending for Certification")
            {
                initCert();
            }
            if (bundle_status == "Pending for Export")
            {
                initPeExport();
            }
            if (bundle_status == "Exported")
            {
                initExport();
            }

        }
        public int GetTotalImageCount()
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();
            int count;

            try
            {
                sqlStr = @"select count(*) from image_master where proj_key=" + projKey + " and batch_key=" + bundleKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                //stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                //stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            if (projDs.Tables[0].Rows.Count > 0)
            {
                count = Convert.ToInt32(projDs.Tables[0].Rows[0][0].ToString());
            }
            else
                count = 0;

            return count;
        }
        private void dtGrdVol_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                deButton20.Enabled = false;

                if (dtGrdVol.SelectedRows.Count > 0)
                {
                    projKey = dtGrdVol.SelectedRows[0].Cells[0].Value.ToString();
                    bundleKey = dtGrdVol.SelectedRows[0].Cells[1].Value.ToString();

                    if(projKey != null && bundleKey != null)
                    {
                       
                        int boxNumber = 1;
                        PopulateView(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), boxNumber);

                        CtrlBox ctrlBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), "1");
                        wfeBox box = new wfeBox(sqlCon, ctrlBox);
                        lblTotalImageScanned.Text = box.GetTotalImageCount().ToString();
                    }
                    else
                    {
                        lblTotalImageScanned.Text = "0";
                    }
                }
            }
            catch
            { }
            
        }

        private void deButton2_Click(object sender, EventArgs e)
        {
            string file_no = deTextBox1.Text;

            dtGrdVol.DataSource = null;
            System.Data.DataTable Dt = new System.Data.DataTable();

            if (file_no != null)
            {
                Dt = _GetResultBundleFile(file_no);

                dtGrdVol.DataSource = Dt;

                FormatDataGridView();

                this.dtGrdVol.Refresh();
                this.textBox2.Focus();
            }
            else
            {
                init();
            }
        }

        private void deButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deButton20_Click(object sender, EventArgs e)
        {
            if(projKey != null && bundleKey != null)
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                app.Visible = false;

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];


                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

                worksheet.Name = "Dashboard Report";

                worksheet.Cells[1, 3] = "File Status Report";
                Range range44 = worksheet.get_Range("C1");
                range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);

                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();


                worksheet.Cells[3, 1] = "Bundle Name : " + _GetResultBundle(projKey, bundleKey).Rows[0][2].ToString();
                Range range43 = worksheet.get_Range("A3");
                range43.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[4, 1] = "Total Image Count : " + GetTotalImageCount().ToString();
                Range range33 = worksheet.get_Range("A4");
                range33.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                Range range = worksheet.get_Range("A3", "A4");
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);


                Range range1 = worksheet.get_Range("A6", "L6");
                range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                for (int i = 1; i < grdStatus.Columns.Count + 1; i++)
                {


                    Range range2 = worksheet.get_Range("A6", "L6");
                    range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                    range2.EntireRow.AutoFit();
                    range2.EntireColumn.AutoFit();
                    worksheet.Cells[6, i] = grdStatus.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < grdStatus.Rows.Count; i++)
                {
                    for (int j = 0; j < grdStatus.Columns.Count; j++)
                    {
                        Range range3 = worksheet.Cells;
                        //range3.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range3.EntireRow.AutoFit();
                        range3.EntireColumn.AutoFit();
                        worksheet.Cells[i + 7, j + 1] = grdStatus.Rows[i].Cells[j].Value.ToString();
                        worksheet.Cells[i + 7, j + 1].Borders.Color = ColorTranslator.ToOle(Color.Black);

                    }

                }

                string namexls = "File_Status_Report" + ".xls";
                string path = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                sfdUAT.Filter = "Xls files (*.xls)|*.xls";
                sfdUAT.FilterIndex = 2;
                sfdUAT.RestoreDirectory = true;
                sfdUAT.FileName = namexls;
                sfdUAT.ShowDialog();

                workbook.SaveAs(sfdUAT.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                app.Quit();
            }

           
        }

    }
}