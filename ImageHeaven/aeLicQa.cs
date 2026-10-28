using bzer_data_entry;
using ImageHeaven.Logic;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageHeaven
{
    public partial class aeLicQa : Form
    {

        private ImageConfig config = null;
        private static string docType;

        OdbcConnection sqlCon = null;
        NovaNet.Utils.dbCon dbcon = null;
        CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        wfePolicy wPolicy = null;
        wfeImage wImage = null;
        private string boxNo = null;
        private string policyNumber = null;
        private string projCode = null;
        private string batchCode = null;
        private string picPath = null;
        private udtPolicy policyData = null;
        string policyPath = null;
        private int policyStatus = 0;
        private int clickedIndexValue;
        private CtrlBox pBox = null;
        private int selBoxNo;
        string[] imageName;
        int policyRowIndex;

        string imagePath = null;
        string photoPath = null;
        private Imagery img;
        private Imagery imgAll;
        private Credentials crd = new Credentials();
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private string imgFileName = string.Empty;
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed = 1;
        private DataTable gTable;
        private string selDocType = string.Empty;
        private int currntPg = 0;
        private bool firstDoc = true;
        private string prevDoc;
        private int policyLen = 0;

        private OdbcDataAdapter sqlAdap = null;

        public aeLicQa(OdbcConnection prmCon, Credentials prmCrd)
        {
            // 
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            this.Name = "Quality control";
            InitializeComponent();
            sqlCon = prmCon;
            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            imgAll = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            this.Text = "Quality control";
            crd = prmCrd;
            exMailLog.SetNextLogger(exTxtLog);

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            documentTypeListView.Init(sqlCon);
            SwitchToSelectionTab();
        }

        private void AeLicQa_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip otherToolTip = new System.Windows.Forms.ToolTip();
            this.WindowState = FormWindowState.Maximized;
            PopulateProjectCombo();
            rdoShowAll.Checked = true;
            cmdZoomIn.ForeColor = Color.Black;
            cmdZoomOut.ForeColor = Color.Black;
            chkRejectBatch.Visible = false;
            bttnToolTip.SetToolTip(cmdZoomIn, "Shortcut Key- (+)");
            bttnToolTip.SetToolTip(cmdZoomOut, "Shortcut Key- (-)");
        }

        private void PopulateProjectCombo()
        {
            DataSet ds = new DataSet();

            dbcon = new NovaNet.Utils.dbCon();

            wfeProject tmpProj = new wfeProject(sqlCon);
            ds = tmpProj.GetAllValues();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cmbProject.DataSource = ds.Tables[0];
                cmbProject.DisplayMember = ds.Tables[0].Columns[1].ToString();
                cmbProject.ValueMember = ds.Tables[0].Columns[0].ToString();
            }
        }

        private void CmbProject_Leave(object sender, EventArgs e)
        {
            PopulateBatchCombo();
        }
        public System.Data.DataSet GetAllValues(int prmProjectKey)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetAllValues(prmProjectKey);
        }
        public DataSet GetAllBox(int prmBatchKey)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetAllBox(prmBatchKey, projCode);
        }
        public DataSet GetReadyImageCount(eSTATES[] state, eSTATES[] prmPolicyState)
        {

            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetReadyImageCount(state, prmPolicyState, projCode, batchCode);
        }
        public int GetPolicyCount(eSTATES[] state)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            if (ob.GetPolicyCount(state, projCode, batchCode).Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ob.GetPolicyCount(state, projCode, batchCode).Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        private void PopulateBoxDetails()
        {
            string batchKey = null;
            DataSet ds = new DataSet();
            CtrlBox cBox = new CtrlBox((int)cmbProject.SelectedValue, (int)cmbBatch.SelectedValue, "0");
            dbcon = new NovaNet.Utils.dbCon();

            wfeBox tmpBox = new wfeBox(sqlCon, cBox);
            DataTable dt = new DataTable();
            DataSet imageCount = new DataSet();
            DataRow dr;
            int indexPolicyCont = 0;
            double avgSize;
            string totSize;
            string totPage;
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[5];
            NovaNet.wfe.eSTATES[] policyState = new NovaNet.wfe.eSTATES[5];

            dt.Columns.Add("Files");
            dt.Columns.Add("Ready");
            dt.Columns.Add("ScannedPages");
            dt.Columns.Add("Avg_Size");
            dt.Columns.Add("TotalSize");

            if (cmbBatch.SelectedValue != null)
            {
                batchKey = cmbBatch.SelectedValue.ToString();
                batchCode = batchKey;
                ds = GetAllBox(Convert.ToInt32(batchKey));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr["Files"] = ds.Tables[0].Rows[i]["files"].ToString();

                        pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), "1", "0");
                        wPolicy = new wfePolicy(sqlCon, pPolicy);

                        policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                        policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                        policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                        policyState[3] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                        policyState[4] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
                        indexPolicyCont = GetPolicyCount(policyState);

                        dr["Ready"] = indexPolicyCont;

                        pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), "1", "0", string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);

                        state[0] = eSTATES.PAGE_INDEXED;
                        state[1] = eSTATES.PAGE_FQC;
                        state[2] = eSTATES.PAGE_CHECKED;
                        state[3] = eSTATES.PAGE_EXCEPTION;
                        state[4] = eSTATES.PAGE_EXPORTED;
                        imageCount = GetReadyImageCount(state, policyState);
                        totPage = imageCount.Tables[0].Rows[0]["page_count"].ToString();
                        dr["ScannedPages"] = totPage;
                        totSize = imageCount.Tables[0].Rows[0]["index_size"].ToString();
                        if (totSize != string.Empty)
                        {
                            dr["TotalSize"] = Math.Round(Convert.ToDouble(totSize), 2);
                        }
                        else
                        {
                            dr["TotalSize"] = string.Empty;
                        }

                        if ((totSize != string.Empty) && (totPage != "0"))
                        {
                            avgSize = Math.Round(Convert.ToDouble(totSize) / Convert.ToDouble(totPage), 2);
                            dr["Avg_Size"] = avgSize.ToString();
                        }

                        dt.Rows.Add(dr);
                    }
                    grdBox.DataSource = dt;
                    grdBox.ForeColor = Color.Black;
                }
            }
        }
        public System.Data.DataTable BundDetails(int prmProjectKey, int prmBatch)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.BundDetails(prmProjectKey, prmBatch);
        }

        public System.Data.DataTable MainMetaDetails(int prmProjectKey, int prmBatch, string prmFilename)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.MainMetaDetails(prmProjectKey, prmBatch, prmFilename);
        }

        public System.Data.DataTable CounselMetaDetails(int prmProjectKey, int prmBatch, string prmFilename)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.CounselMetaDetails(prmProjectKey, prmBatch, prmFilename);
        }

        private void CheckBatchRejection(string pBatchKey)
        {
            wfeBatch wBatch = new wfeBatch(sqlCon);
            if (chkReadyUat.Checked == false)
            {

                if (wBatch.PolicyWithLICException(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(pBatchKey)) == true)
                {
                    chkRejectBatch.Visible = false;
                }
                else
                {
                    chkRejectBatch.Visible = false;
                }

            }
            else
            {
                chkRejectBatch.Visible = false;
            }
        }
        private void PopulateBatchCombo()
        {
            string projKey = null;
            DataSet ds = new DataSet();

            dbcon = new NovaNet.Utils.dbCon();
            NovaNet.wfe.eSTATES[] bState = new NovaNet.wfe.eSTATES[2];
            wfeBatch tmpBatch = new wfeBatch(sqlCon);
            if (cmbProject.SelectedValue != null)
            {
                projKey = cmbProject.SelectedValue.ToString();
                projCode = projKey;

                ds = GetAllValues(Convert.ToInt32(projKey));


                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbBatch.DataSource = ds.Tables[0];
                    cmbBatch.DisplayMember = ds.Tables[0].Columns[1].ToString();
                    cmbBatch.ValueMember = ds.Tables[0].Columns[0].ToString();
                }
                else
                {
                    cmbBatch.DataSource = ds.Tables[0];
                }
            }
        }
        public int GetTotalPolicies(eSTATES prmState)
        {

            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetTotalPolicies(prmState, projCode, batchCode).Tables[0].Rows.Count;
        }
        public int GetTotalPolicies(eSTATES[] prmState)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetTotalPolicies(prmState, projCode, batchCode).Tables[0].Rows.Count;
        }
        public double GetTotalBatchSize()
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetTotalBatchSize(projCode, batchCode);
        }

        public int GetTotalImageCount(eSTATES[] state, bool prmIsSignaturePage, eSTATES[] prmPolicyState)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return Convert.ToInt32(ob.GetTotalImageCount(state, prmIsSignaturePage, prmPolicyState, projCode, batchCode).Tables[0].Rows[0]["page_Count"].ToString());
        }
        public int GetBatchStatus(int prmBatchKey)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            int status = 0;
            if (ob.GetBatchStatus(prmBatchKey).Tables[0].Rows.Count > 0)
            {
                status = Convert.ToInt32(ob.GetBatchStatus(prmBatchKey).Tables[0].Rows[0]["status"].ToString());
            }
            return status;
        }
        private void CmbBatch_Leave(object sender, EventArgs e)
        {
            try
            {
                if ((cmbProject.SelectedValue != null) && (cmbBatch.SelectedValue != null))
                {
                    var projectID = Convert.ToInt32(cmbProject.SelectedValue.ToString());
                    var batchID = Convert.ToInt32(cmbBatch.SelectedValue.ToString());
                    uatBatchDetailsPanel.LoadBatchDetails(projectID, batchID, sqlCon);

                    PopulateBoxDetails();
                    if (GetBatchStatus(Convert.ToInt32(cmbBatch.SelectedValue.ToString())) == (int)eSTATES.BATCH_READY_FOR_UAT)
                    {
                        chkReadyUat.Enabled = false;
                        chkReadyUat.Checked = true;
                        cmdAccepted.Enabled = false;
                        cmdRejected.Enabled = false;
                    }
                    else
                    {
                        chkReadyUat.Enabled = true;
                        chkReadyUat.Checked = false;
                        cmdAccepted.Enabled = true;
                        cmdRejected.Enabled = true;
                    }
                    CheckBatchRejection(cmbBatch.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating bundle information........" + "  " + ex.Message);
            }
        }

        private void SwitchToSelectionTab()
        {
            tabControl1.SelectedIndex = 0;
            (tbImages as Control).Enabled = false;
        }

        private void SwitchToImagesTab()
        {
            tabControl1.SelectedIndex = 1;
            (tbImages as Control).Enabled = true;
        }

        private void GrdBox_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selBoxNo = Convert.ToInt32("1");
            PolicyDetails("1");
            grdPolicy.ForeColor = Color.Black;
        }
        public DataSet GetPolicyList(eSTATES[] prmState)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetPolicyList(prmState, projCode, batchCode);
        }
        void PolicyDetails(string prmBoxNo)
        {
            DataTable dt = new DataTable();
            eSTATES[] filterState = new eSTATES[1];

            dt.Columns.Add("SrlNo");
            dt.Columns.Add("Filename");
            dt.Columns.Add("ScannedPages");
            dt.Columns.Add("TotalSize");
            dt.Columns.Add("Avg_Size");
            dt.Columns.Add("STATUS");
            dt.Columns.Add("FILESTATUS");
            foreach (var docType in documentTypeListView.Store.GetDocumentTypes())
            {
                dt.Columns.Add(docType);
            }

            if (cmbBatch.Text != "")
            {
                if (!string.IsNullOrEmpty(prmBoxNo) && !string.IsNullOrEmpty(cmbProject.SelectedValue.ToString()) && !string.IsNullOrEmpty(cmbBatch.SelectedValue.ToString()))
                {
                    boxNo = prmBoxNo;
                    pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), prmBoxNo, "0");
                    wPolicy = new wfePolicy(sqlCon, pPolicy);

                    var projectKey = Convert.ToInt32(cmbProject.SelectedValue);
                    var batchKey = Convert.ToInt32(cmbBatch.SelectedValue);

                    var states = new eSTATES[0];
                    if (rdoChecked.Checked)
                    {
                        states = new eSTATES[] { eSTATES.POLICY_CHECKED };
                    }
                    if (rdoExceptions.Checked)
                    {
                        states = new eSTATES[] { eSTATES.POLICY_EXCEPTION };
                    }
                    if (rdoOnHold.Checked)
                    {
                        states = new eSTATES[] { eSTATES.POLICY_ON_HOLD };
                    }
                    if (rdoMissing.Checked)
                    {
                        states = new eSTATES[] { eSTATES.POLICY_MISSING };
                    }
                    var dsPolicy = GetPolicyList(states);
                    var fileRows = dsPolicy.Tables[0].Rows.Cast<DataRow>()
                        .Select(row =>
                        {
                            var fileName = row["filename"].ToString();
                            var ctrlImage = new CtrlImage(projectKey, batchKey, prmBoxNo, fileName, string.Empty, string.Empty);
                            var wImage = new wfeImage(sqlCon, ctrlImage);
                            var dsImage = wImage.GetPolicyWiseImageInfo(new eSTATES[]{
                                eSTATES.PAGE_EXCEPTION,
                                eSTATES.PAGE_INDEXED,
                                eSTATES.PAGE_CHECKED,
                                eSTATES.PAGE_FQC,
                                eSTATES.PAGE_EXPORTED,
                                eSTATES.PAGE_ON_HOLD
                            });
                            return
                            (
                                Row: row,
                                ImageSize: Convert.ToDouble(dsImage.Tables[0].Rows[0]["qc_size"]),
                                PageCount: Convert.ToInt32(dsImage.Tables[0].Rows[0]["page_count"])
                            );
                        })
                        .Where(details =>
                        {
                            if (rdo150.Checked)
                            {
                                var imageSize = details.ImageSize / 1024;
                                return imageSize > ihConstants._DOCKET_MAX_SIZE;
                            }
                            return true;
                        })
                        .Select((details, index) =>
                        {
                            var row = details.Row;
                            var imageSize = details.ImageSize;
                            var pageCount = details.PageCount;
                            var policyStatus = Convert.ToInt32(row["status"].ToString());
                            var invalidPolicyStates = new List<eSTATES> { eSTATES.POLICY_MISSING, eSTATES.POLICY_SCANNED, eSTATES.POLICY_QC, eSTATES.POLICY_ON_HOLD };
                            var unIndexedPolicyStates = new List<eSTATES> { eSTATES.BATCH_CREATED, eSTATES.BATCH_SCANNED, eSTATES.POLICY_MISSING, eSTATES.POLICY_ON_HOLD };
                            var totalPage = invalidPolicyStates.Any(status => (int)status == policyStatus) ? 0 : pageCount;
                            var totalSize = invalidPolicyStates.Any(status => (int)status == policyStatus) ? 0 : imageSize;
                            var avgSize = totalPage != 0 ? totalSize / totalPage : 0;

                            var dr = dt.NewRow();
                            dr["SrlNo"] = index + 1;
                            dr["Filename"] = row["filename"].ToString();
                            dr["ScannedPages"] = totalPage.ToString();
                            dr["TotalSize"] = totalSize.ToString(".2");
                            dr["Avg_Size"] = avgSize.ToString(".2");
                            dr["STATUS"] = policyStatus.ToString();
                            dr["FILESTATUS"] = new Func<int, string>(state =>
                            {
                                switch (state)
                                {
                                    case (int)eSTATES.POLICY_NOT_INDEXED:
                                        return "Not Indexed";
                                    case (int)eSTATES.POLICY_INDEXED:
                                        return "Indexed";
                                    case (int)eSTATES.POLICY_FQC:
                                        return "Vendor QC done";
                                    case (int)eSTATES.POLICY_ON_HOLD:
                                        return "On hold";
                                    case (int)eSTATES.POLICY_MISSING:
                                        return "Missing";
                                    case (int)eSTATES.POLICY_EXCEPTION:
                                        return "In exception";
                                    case (int)eSTATES.POLICY_CHECKED:
                                        return "Checked";
                                    case (int)eSTATES.POLICY_EXPORTED:
                                        return "Exported";
                                    default:
                                        return "Unknown";
                                }
                            })(policyStatus);

                            var fileName = row["filename"].ToString();
                            var counts = documentTypeListView.Store.GetDocumentTypeCountsForFile(projectKey, batchKey, fileName);
                            foreach (var docType in documentTypeListView.Store.GetDocumentTypes())
                            {
                                if (counts.TryGetValue(docType, out int count))
                                {
                                    dr[docType] = unIndexedPolicyStates.Any(state => (int)state == policyStatus) ? "0" : count.ToString();
                                }
                                else
                                {
                                    dr[docType] = "0";
                                }
                            }

                            return dr;
                        });
                    foreach (var row in fileRows)
                    {
                        dt.Rows.Add(row);
                    }

                    grdPolicy.DataSource = dt;

                    if (grdPolicy.Rows.Count > 0)
                    {
                        for (int l = 0; l < grdPolicy.Rows.Count; l++)
                        {
                            var policyStatus = Convert.ToInt32(dt.Rows[l]["status"].ToString());
                            if (policyStatus == (int)eSTATES.POLICY_CHECKED)
                            {
                                grdPolicy.Rows[l].DefaultCellStyle.ForeColor = Color.Black;
                                grdPolicy.Rows[l].DefaultCellStyle.BackColor = Color.Green;
                            }
                            if (policyStatus == (int)eSTATES.POLICY_EXCEPTION || policyStatus == (int)eSTATES.POLICY_EXCEPTION)
                            {
                                grdPolicy.Rows[l].DefaultCellStyle.ForeColor = Color.Black;
                                grdPolicy.Rows[l].DefaultCellStyle.BackColor = Color.Red;
                            }
                            if (policyStatus == (int)eSTATES.POLICY_ON_HOLD)
                            {
                                grdPolicy.Rows[l].DefaultCellStyle.ForeColor = Color.Black;
                                grdPolicy.Rows[l].DefaultCellStyle.BackColor = Color.Turquoise;
                            }
                            if (policyStatus == (int)eSTATES.POLICY_MISSING)
                            {
                                grdPolicy.Rows[l].DefaultCellStyle.ForeColor = Color.Black;
                                grdPolicy.Rows[l].DefaultCellStyle.BackColor = Color.Magenta;
                            }
                        }

                    }
                    foreach (var colIndex in grdPolicy.Columns.Cast<DataGridViewColumn>().Select((col, index) => (col, index)))
                    {
                        if (colIndex.index == grdPolicy.ColumnCount - 1)
                        {
                            colIndex.col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        else
                        {
                            colIndex.col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        }
                    }
                }
            }
        }

        private void RdoShowAll_Click(object sender, EventArgs e)
        {
            if ((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
                PolicyDetails(selBoxNo.ToString());
        }

        private void RdoChecked_Click(object sender, EventArgs e)
        {
            if ((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
                PolicyDetails(selBoxNo.ToString());
        }

        private void RdoExceptions_Click(object sender, EventArgs e)
        {
            if ((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
                PolicyDetails(selBoxNo.ToString());
        }

        private void RdoOnHold_Click(object sender, EventArgs e)
        {
            if ((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
                PolicyDetails(selBoxNo.ToString());
        }

        private void RdoMissing_Click(object sender, EventArgs e)
        {
            if ((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
                PolicyDetails(selBoxNo.ToString());
        }

        private void Rdo150_Click(object sender, EventArgs e)
        {
            if ((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
                PolicyDetails(selBoxNo.ToString());
        }
        void ClearPicBox()
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
        }
        private string GetPolicyPath(string policyNo)
        {
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = GetPath(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
            return batchPath + "\\" + policyNo;
        }
        public string GetPath(int prmProjKey, int prmBatchKey)
        {
            string Path;
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);

            if (ob.GetPath(prmProjKey, prmBatchKey).Tables[0].Rows.Count > 0)
            {
                Path = ob.GetPath(prmProjKey, prmBatchKey).Tables[0].Rows[0]["bundle_path"].ToString();
            }
            else
                Path = string.Empty;

            return Path;
        }

        private string GetFileNameFromSelectedRow()
        {
            if (grdPolicy.SelectedCells.Count > 0)
            {
                return grdPolicy.SelectedCells[0].OwningRow.Cells["Filename"].Value as string;
            }
            return null;
        }

        private void DisplayDocTypeCount()
        {
            if (grdPolicy.SelectedCells.Count > 0)
            {
                var selectedFileName = GetFileNameFromSelectedRow();
                if (selectedFileName != null)
                {
                    documentTypeListView.UpdateCounts(Convert.ToInt32(cmbProject.SelectedValue), Convert.ToInt32(cmbBatch.SelectedValue), selectedFileName);
                }
            }
        }

        public ArrayList GetImages(eITEMS item, string case_file_no)
        {
            OdbcDataAdapter wAdap;
            OdbcCommand oCom = new OdbcCommand();
            string strQuery = null;
            wItemControl wic = null;
            DataSet ds = new DataSet();
            string strQr = string.Empty;
            ArrayList arrItem = new ArrayList();

            if (item == eITEMS.LIC_QA_PAGE)
            {
                strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A,case_file_master B where A.proj_key=B.proj_code and A.batch_key = B.bundle_key  and A.policy_number = B.filename and A.photo <> 1 and A.proj_key=" + projCode + " and A.batch_key=" + batchCode + " and  A.policy_number='" + case_file_no + "' and a.status <> 29 and (b.status = 3 or b.status = 4 or b.status = 5 or b.status = 6 or b.status ='7' or b.status = '8' or b.status = '30' or b.status = '31' or b.status = '37' or b.status = '40' or b.status = '77') order by a.serial_no";

                oCom.Connection = sqlCon;
                oCom.CommandText = strQuery;
                wAdap = new OdbcDataAdapter(oCom);
                wAdap.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        wic = new CtrlImage(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_key"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[i]["batch_key"].ToString()), "1", ds.Tables[0].Rows[i]["policy_number"].ToString(), ds.Tables[0].Rows[i]["page_name"].ToString(), ds.Tables[0].Rows[i]["doc_type"].ToString());
                        arrItem.Add(wic);
                    }
                }
            }

            return arrItem;
        }
        private void GrdPolicy_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cmbBatch.Text != "")
            {
                try
                {
                    ClearPicBox();

                    firstDoc = true;
                    DataSet expDs = new DataSet();
                    clickedIndexValue = e.RowIndex;
                    picControl.Image = null;
                    lstImage.Items.Clear();
                    DisplayDocTypeCount();
                    policyNumber = grdPolicy.Rows[e.RowIndex].Cells["Filename"].Value.ToString();
                    policyLen = policyNumber.Length;

                    policyRowIndex = e.RowIndex;
                    if (Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells["ScannedPages"].Value.ToString()) > 0)
                    {
                        lblTotFiles.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells["ScannedPages"].Value.ToString()), 2));
                        lblAvgSize.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells["Avg_Size"].Value.ToString()), 2)) + " KB";
                        lblDock.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells["TotalSize"].Value.ToString()), 2)) + " KB";
                        policyStatus = Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells["STATUS"].Value.ToString());

                        if (policyStatus == (int)eSTATES.POLICY_EXPORTED)
                        {
                            cmdAccepted.Enabled = false;
                            cmdRejected.Enabled = false;
                        }
                        else
                        {
                            cmdAccepted.Enabled = true;
                            cmdRejected.Enabled = true;
                        }
                        pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo, policyNumber);
                        wfePolicy policy = new wfePolicy(sqlCon, pPolicy);
                        //policyData = (udtPolicy)policy.LoadValuesFromDB();
                        policyPath = GetPolicyPath(policyNumber); //policyData.policy_path;
                        expDs = policy.GetAllException();
                        if (expDs.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["missing_img_exp"].ToString()) == 1)
                            {
                                chkMissingImg.Checked = true;
                            }
                            else
                            {
                                chkMissingImg.Checked = false;
                            }

                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["crop_clean_exp"].ToString()) == 1)
                            {
                                chkCropClean.Checked = true;
                            }
                            else
                            {
                                chkCropClean.Checked = false;
                            }

                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["poor_scan_exp"].ToString()) == 1)
                            {
                                chkPoorScan.Checked = true;
                            }
                            else
                            {
                                chkPoorScan.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["wrong_indexing_exp"].ToString()) == 1)
                            {
                                chkIndexing.Checked = true;
                            }
                            else
                            {
                                chkIndexing.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["linked_policy_exp"].ToString()) == 1)
                            {
                                chkLinkedPolicy.Checked = true;
                            }
                            else
                            {
                                chkLinkedPolicy.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
                            {
                                chkDesicion.Checked = true;
                            }
                            else
                            {
                                chkDesicion.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["extra_page_exp"].ToString()) == 1)
                            {
                                chkExtraPage.Checked = true;
                            }
                            else
                            {
                                chkExtraPage.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
                            {
                                chkDesicion.Checked = true;
                            }
                            else
                            {
                                chkDesicion.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["rearrange_exp"].ToString()) == 1)
                            {
                                chkRearrange.Checked = true;
                            }
                            else
                            {
                                chkRearrange.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["other_exp"].ToString()) == 1)
                            {
                                chkOther.Checked = true;
                            }
                            else
                            {
                                chkOther.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["move_to_respective_policy_exp"].ToString()) == 1)
                            {
                                chkMove.Checked = true;
                            }
                            else
                            {
                                chkMove.Checked = false;
                            }
                            if (Convert.ToInt32(expDs.Tables[0].Rows[0]["metadata_exp"].ToString()) == 1)
                            {
                                chkMeta.Checked = true;
                            }
                            else
                            {
                                chkMeta.Checked = false;
                            }

                            txtComments.Text = expDs.Tables[0].Rows[0]["comments"].ToString() + "\r\n";
                            txtComments.SelectionStart = txtComments.Text.Length;
                            txtComments.ScrollToCaret();
                            txtComments.Refresh();
                        }
                        else
                        {
                            chkMissingImg.Checked = false;
                            chkCropClean.Checked = false;
                            chkPoorScan.Checked = false;
                            chkIndexing.Checked = false;
                            chkLinkedPolicy.Checked = false;
                            chkDesicion.Checked = false;
                            chkExtraPage.Checked = false;
                            chkDesicion.Checked = false;
                            chkRearrange.Checked = false;
                            chkOther.Checked = false;
                            chkMove.Checked = false;
                            chkMeta.Checked = false;
                            txtComments.Text = string.Empty;
                        }

                        ArrayList arrImage = new ArrayList();
                        eSTATES[] state = new eSTATES[5];
                        state[0] = eSTATES.POLICY_CHECKED;
                        state[1] = eSTATES.POLICY_FQC;
                        state[2] = eSTATES.POLICY_INDEXED;
                        state[3] = eSTATES.POLICY_EXCEPTION;
                        state[4] = eSTATES.POLICY_EXPORTED;
                        CtrlImage ctrlImage;
                        arrImage = GetImages(eITEMS.LIC_QA_PAGE, policyNumber);
                        for (int i = 0; i < arrImage.Count; i++)
                        {
                            ctrlImage = (CtrlImage)arrImage[i];
                            if (ctrlImage.DocType != string.Empty)
                            {
                                lstImage.Items.Add(ctrlImage.ImageName + "-" + ctrlImage.DocType);
                            }
                            else
                                lstImage.Items.Add(ctrlImage.ImageName);
                        }
                        DisplayDocTypeCount();
                        SwitchToImagesTab();
                        if (lstImage.Items.Count > 0)
                        {
                            lstImage.SelectedIndex = 0;
                            cmdAccepted.Enabled = true;
                            cmdRejected.Enabled = true;
                        }

                    }
                    else
                    {
                        cmdAccepted.Enabled = false;
                        cmdRejected.Enabled = false;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while getting the information of the selected policy.....");
                    exMailLog.Log(ex);
                }
            }
        }
        private bool GetThumbnailImageAbort()
        {
            return false;
        }

        private void ShowThumbImage(string pDocType)
        {
            DataSet ds = new DataSet();
            string imageFileName;
            Image imgNew = null;
            IContainerControl icc = tabControl2.GetContainerControl();


            System.Drawing.Image imgThumbNail = null;

            pImage = new CtrlImage(Convert.ToInt32(projCode), Convert.ToInt32(batchCode), boxNo, policyNumber, string.Empty, pDocType);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            ds = wImage.GetAllIndexedImageName();
            ClearPicBox();
            if (ds.Tables[0].Rows.Count > 0)
            {
                imageName = new string[ds.Tables[0].Rows.Count];
                if (ds.Tables[0].Rows.Count <= 6)
                {
                    pgOne.Visible = true;
                    pgTwo.Visible = false;
                    pgThree.Visible = false;
                }
                if ((ds.Tables[0].Rows.Count > 6) && (ds.Tables[0].Rows.Count <= 12))
                {
                    pgOne.Visible = true;
                    pgTwo.Visible = true;
                    pgThree.Visible = false;
                }
                if ((ds.Tables[0].Rows.Count > 12) && (ds.Tables[0].Rows.Count <= 14))
                {
                    pgOne.Visible = true;
                    pgTwo.Visible = true;
                    pgThree.Visible = true;
                }
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (picPath != null)
                    {
                        imageFileName = picPath + "\\" + ds.Tables[0].Rows[j][0].ToString();
                        imgAll.LoadBitmapFromFile(imageFileName);

                        if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                        {
                            try
                            {
                                imgAll.GetLZW("tmp.TIF");
                                imgNew = Image.FromFile("tmp.TIF");
                                imgThumbNail = imgNew;
                            }
                            catch (Exception ex)
                            {
                                string err = ex.Message;
                            }
                        }
                        else
                        {
                            imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                        }
                        imageName[j] = imageFileName;
                        if (!System.IO.File.Exists(imageFileName)) return;
                        double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                        double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                        double Scale = Math.Min(scaleX, scaleY);
                        int w = (int)(imgThumbNail.Width * Scale);
                        int h = (int)(imgThumbNail.Height * Scale);
                        w = w - 5;
                        imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                        if (j == 0)
                        {
                            pictureBox1.Image = imgThumbNail;
                            pictureBox1.Tag = imageFileName;
                        }
                        if (j == 1)
                        {
                            pictureBox2.Image = imgThumbNail;
                            pictureBox2.Tag = imageFileName;
                        }
                        if (j == 2)
                        {
                            pictureBox3.Image = imgThumbNail;
                            pictureBox3.Tag = imageFileName;
                        }
                        if (j == 3)
                        {
                            pictureBox4.Image = imgThumbNail;
                            pictureBox4.Tag = imageFileName;
                        }
                        if (j == 4)
                        {
                            pictureBox5.Image = imgThumbNail;
                            pictureBox5.Tag = imageFileName;
                        }
                        if (j == 5)
                        {
                            pictureBox6.Image = imgThumbNail;
                            pictureBox6.Tag = imageFileName;
                        }
                        if (imgNew != null)
                        {
                            imgNew.Dispose();
                            imgNew = null;
                            if (File.Exists("tmp.tif"))
                                File.Delete("tmp.TIF");
                        }
                    }
                }
            }
            else
            {
                ClearPicBox();
                imageName = null;
            }

        }
        private void documentTypeListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgOne.Visible = false;
            pgTwo.Visible = false;
            pgThree.Visible = false;
            currntPg = 0;

            if (documentTypeListView.SelectedIndex != -1)
            {
                ShowThumbImage(documentTypeListView.SelectedDocumentType);
                lstImage.SelectedIndex = lstImage.Items.Cast<string>()
                    .Select((item, index) => (item, index))
                    .Where(item => item.item.Contains(documentTypeListView.SelectedDocumentType))
                    .FirstOrDefault().index;
                lstImage.Focus();
            }
        }
        private void ChangeSize()
        {
            Image imgTot = null;
            try
            {
                if (img.IsValid() == true)
                {
                    if (img.GetBitmap().PixelFormat == PixelFormat.Format1bppIndexed)
                    {
                        picControl.Height = tabControl1.Height - 75;
                        picControl.Width = tabControl2.Width - 30;
                        if (!System.IO.File.Exists(imgFileName)) return;
                        Image newImage;
                        imgAll.LoadBitmapFromFile(imgFileName);
                        if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                        {
                            imgAll.GetLZW("tmp1.TIF");
                            imgTot = Image.FromFile("tmp1.TIF");
                            newImage = imgTot;
                        }
                        else
                        {
                            newImage = System.Drawing.Image.FromFile(imgFileName);
                        }

                        double scaleX = (double)picControl.Width / (double)newImage.Width;
                        double scaleY = (double)picControl.Height / (double)newImage.Height;
                        double Scale = Math.Min(scaleX, scaleY);
                        int w = (int)(newImage.Width * Scale);
                        int h = (int)(newImage.Height * Scale);
                        picControl.Width = w;
                        picControl.Height = h;
                        picControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                        newImage.Dispose();
                        picControl.Refresh();
                        if (imgTot != null)
                        {
                            imgTot.Dispose();
                            imgTot = null;
                            if (File.Exists("tmp1.tif"))
                                File.Delete("tmp1.TIF");
                        }
                    }
                    else
                    {
                        picControl.Height = tabControl1.Height - 75;
                        picControl.Width = tabControl2.Width - 100;
                        img.LoadBitmapFromFile(imgFileName);
                        picControl.Image = img.GetBitmap();
                        picControl.SizeMode = PictureBoxSizeMode.StretchImage;
                        picControl.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error ..." + ex.Message, "Error");
            }
        }
        private void lstImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pos;
            string changedImage = null;
            double fileSize;
            string currntDoc;
            wfeImage wImage = null;

            try
            {
                pos = lstImage.SelectedItem.ToString().IndexOf("-");
                if (pos < 0)
                {
                    changedImage = lstImage.SelectedItem.ToString();
                }
                else
                { changedImage = lstImage.SelectedItem.ToString().Substring(0, pos); }

                pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo, policyNumber, changedImage, string.Empty);
                wImage = new wfeImage(sqlCon, pImage);
                changedImage = wImage.GetIndexedImageName();

                if ((policyStatus == (int)4) || (policyStatus == (int)eSTATES.POLICY_CHECKED) || (policyStatus == (int)eSTATES.POLICY_EXCEPTION) || (policyStatus == (int)eSTATES.POLICY_EXPORTED))
                {
                    if (Directory.Exists(policyPath + "\\" + ihConstants._FQC_FOLDER))
                    {
                        picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                        imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                        if (changedImage.Substring(policyLen, 6) == "_000_A")
                        {
                            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                            if (File.Exists(imgFileName) == false)
                            {
                                imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                                picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                            }
                            photoPath = imagePath;
                        }
                        else
                        {
                            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                            if (File.Exists(imgFileName) == true)
                            {
                                imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                                picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                            }
                        }
                    }
                    else
                    {
                        imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                        picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                        if (changedImage.Substring(policyLen, 6) == "_000_A")
                        {
                            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                            img.LoadBitmapFromFile(imgFileName);
                            photoPath = imagePath;
                        }
                        else
                        {
                            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;

                        }
                    }

                }
                else
                {
                    picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                    imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                    if (changedImage.Substring(policyLen, 6) == "_000_A")
                    {
                        imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                        if (File.Exists(imgFileName) == true)
                        {
                            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                            picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                        }
                        photoPath = imagePath;
                    }
                    else
                    {
                        imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                        if (File.Exists(imgFileName) == false)
                        {
                            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                            picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                        }
                    }

                }
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);

                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImageSize.Text = Convert.ToString(Math.Round(fileSize, 2)) + " KB";
                img.LoadBitmapFromFile(imgFileName);
                int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;

                if (imgFileName != string.Empty)
                {
                    ChangeSize();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating the preview....");
                exMailLog.Log(ex);
            }
        }

        private void CmdNext_Click(object sender, EventArgs e)
        {
            ListViewItem lvwItem;
            if (tabControl2.SelectedIndex == 0)
            {
                if (lstImage.Items.Count > 0)
                {
                    if ((lstImage.Items.Count - 1) != lstImage.SelectedIndex)
                    {
                        lstImage.SelectedIndex = lstImage.SelectedIndex + 1;
                    }
                }
                if (tabControl2.SelectedIndex == 1)
                {
                    if (lstImage.SelectedIndex != 0)
                    {
                        int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                        string currntDoc = lstImage.Items[lstImage.SelectedIndex - 1].ToString().Substring(dashPos);
                        string prevDoc = lstImage.Items[lstImage.SelectedIndex].ToString().Substring(dashPos);
                        if (currntDoc != prevDoc)
                        {
                            documentTypeListView.SelectedDocumentType = prevDoc;
                            documentTypeListView.Focus();
                        }
                    }
                }
            }
        }
        private void ThumbnailChangeSize(string fName)
        {
            Image imgTot = null;
            try
            {
                //picBig.Height = tabControl1.Height - 75;
                //picBig.Width = tabControl2.Width - 30;
                //if (!System.IO.File.Exists(fName)) return;
                //Image newImage;
                //imgAll.LoadBitmapFromFile(fName);
                //if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                //{
                //    imgAll.GetLZW("tmp1.TIF");
                //    imgTot = Image.FromFile("tmp1.TIF");
                //    newImage = imgTot;
                //}
                //else
                //{
                //    newImage = System.Drawing.Image.FromFile(fName);
                //}
                //double scaleX = (double)picBig.Width / (double)newImage.Width;
                //double scaleY = (double)picBig.Height / (double)newImage.Height;
                //double Scale = Math.Min(scaleX, scaleY);
                //int w = (int)(newImage.Width * Scale);
                //int h = (int)(newImage.Height * Scale);
                //picBig.Width = w;
                //picBig.Height = h;
                //picBig.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                //newImage.Dispose();
                //picBig.Refresh();
                //if (imgTot != null)
                //{
                //    imgTot.Dispose();
                //    imgTot = null;
                //    if (File.Exists("tmp1.tif"))
                //        File.Delete("tmp1.TIF");
                //}
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error ..." + ex.Message, "Error");
            }
        }
        private void CmdPrevious_Click(object sender, EventArgs e)
        {
            ListViewItem lvwItem;
            if (tabControl2.SelectedIndex == 0)
            {
                if (lstImage.SelectedIndex != 0)
                {
                    lstImage.SelectedIndex = lstImage.SelectedIndex - 1;
                }
                if (tabControl2.SelectedIndex == 1)
                {
                    if (lstImage.SelectedIndex != 0)
                    {
                        int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                        string currntDoc = lstImage.Items[lstImage.SelectedIndex].ToString().Substring(dashPos);
                        string prevDoc = lstImage.Items[lstImage.SelectedIndex + 1].ToString().Substring(dashPos);
                        if (currntDoc != prevDoc)
                        {
                            documentTypeListView.SelectedDocumentType = prevDoc;
                            documentTypeListView.Focus();
                        }
                    }
                }
            }
        }

        private void CmdZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void CmdZoomOut_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }
        int ZoomIn()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    picControl.Dock = DockStyle.None;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height * (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width * (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    picControl.Width = Convert.ToInt32(Convert.ToDouble(picControl.Width) * 1.2);
                    picControl.Height = Convert.ToInt32(Convert.ToDouble(picControl.Height) * 1.2);
                    picControl.Refresh();
                    ChangeZoomSize();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
                exMailLog.Log(ex);
            }
            return 0;
        }

        private void ChangeZoomSize()
        {
            if (!System.IO.File.Exists(imgFileName)) return;
            Image newImage = Image.FromFile(imgFileName);
            double scaleX = (double)picControl.Width / (double)newImage.Width;
            double scaleY = (double)picControl.Height / (double)newImage.Height;
            double Scale = Math.Min(scaleX, scaleY);
            int w = (int)(newImage.Width * Scale);
            int h = (int)(newImage.Height * Scale);
            picControl.Width = w;
            picControl.Height = h;
            picControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
            picControl.Invalidate();
            newImage.Dispose();
        }

        private void TabControl2_TabIndexChanged(object sender, EventArgs e)
        {
            if (imgFileName != string.Empty)
            {
                if (tabControl2.SelectedIndex == 0)
                    ChangeSize();
                ThumbnailChangeSize(imgFileName);
            }
        }

        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem lvwItem;
            string currntDoc = string.Empty;
            if (tabControl2.SelectedIndex != 1)
            {
                ChangeSize();
            }
            documentTypeListView.Focus();
        }

        private void PictureBox1_DoubleClick(object sender, EventArgs e)
        {


            if (imageName != null)
            {
                if (imageName.Length >= 1)
                {

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 0 + GetDocTypePos();

                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();

                }
            }
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {

            if (imageName != null)
            {
                if (imageName.Length >= 2)
                {

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 1 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();

                }
            }
        }

        private void PictureBox3_DoubleClick(object sender, EventArgs e)
        {
            if (imageName != null)
            {
                if (imageName.Length >= 3)
                {

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 2 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();

                }
            }
        }

        private void PictureBox4_DoubleClick(object sender, EventArgs e)
        {

            if (imageName != null)
            {
                if (imageName.Length >= 4)
                {

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 3 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();

                }
            }
        }

        private void pictureBox5_DoubleClick(object sender, EventArgs e)
        {

            if (imageName != null)
            {
                if (imageName.Length >= 5)
                {

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 4 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();

                }
            }
        }
        private int GetDocTypePos()
        {
            if (documentTypeListView.SelectedIndex != -1)
            {
                return lstImage.Items.Cast<string>()
                    .Select((item, index) => (item, index))
                    .Where(item => item.item.Contains(documentTypeListView.SelectedDocumentType))
                    .FirstOrDefault().index;
            }
            return 0;
        }
        private void PictureBox6_DoubleClick(object sender, EventArgs e)
        {

            if (imageName != null)
            {
                if (imageName.Length >= 6)
                {

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 5 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    documentTypeListView.Focus();

                }
            }
        }

        private void PgOne_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            for (int i = 0; i < imageName.Length; i++)
            {
                imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);

                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                currntPg = 0;
                if (i == 0)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 1)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 2)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (i == 3)
                {
                    pictureBox4.Image = imgThumbNail;
                    pictureBox4.Tag = imageFileName;
                }
                if (i == 4)
                {
                    pictureBox5.Image = imgThumbNail;
                    pictureBox5.Tag = imageFileName;
                }
                if (i == 5)
                {
                    pictureBox6.Image = imgThumbNail;
                    pictureBox6.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void PgTwo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            for (int i = 6; i < imageName.Length; i++)
            {
                imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);
                currntPg = 1;
                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                if (i == 6)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 7)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 8)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (i == 9)
                {
                    pictureBox4.Image = imgThumbNail;
                    pictureBox4.Tag = imageFileName;
                }
                if (i == 10)
                {
                    pictureBox5.Image = imgThumbNail;
                    pictureBox5.Tag = imageFileName;
                }
                if (i == 11)
                {
                    pictureBox6.Image = imgThumbNail;
                    pictureBox6.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void PgThree_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;

            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            for (int i = 0; i < imageName.Length; i++)
            {
                imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);
                currntPg = 2;
                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                if (i == 12)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 13)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 14)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void ChkMissingImg_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkMissingImg.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Missing image \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Missing image \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkPoorScan_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkPoorScan.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Poor scan quality \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Poor scan quality \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkDesicion_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkDesicion.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Desicion misd \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Desicion misd \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkExtraPage_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkExtraPage.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Extra page \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Extra page \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkMove_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkMove.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Move to respective file \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Move to respective file \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkLinkedPolicy_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;

            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkLinkedPolicy.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Linked file problem \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Linked file problem \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkCropClean_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkCropClean.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Crop clean problem \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Crop clean problem \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkIndexing_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkIndexing.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Wrong doc type \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Wrong doc type \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkOther_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkOther.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Other \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Other \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void ChkRearrange_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkRearrange.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Rearrange error \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Rearrange error \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }
        public bool UpdateStatus(eSTATES state, Credentials prmCrd)
        {
            UpdateStatusLogic ob = new UpdateStatusLogic(sqlCon);
            return ob.UpdateStatus(projCode, batchCode, policyNumber, prmCrd, new eSTATES[] { }, new eSTATES[] { eSTATES.POLICY_EXPORTED }, state);
        }
        private void CmdAccepted_Click(object sender, EventArgs e)
        {
            string pageName;
            try
            {
                if (crd.role == ihConstants._LIC_ROLE)
                {
                    if (chkReadyUat.Checked == false)
                    {
                        if (lstImage.Items.Count > 0)
                        {
                            pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo, policyNumber);
                            wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);


                            if (wPolicy.GetAllExceptionCheck().Tables[0].Rows.Count > 0)
                            { wPolicy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_SOLVED, ihConstants._LIC_QA_POLICY_CHECKED); }
                            else
                            {
                                wPolicy.InitiateQaPolicyException(crd);
                                wPolicy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_SOLVED, ihConstants._LIC_QA_POLICY_CHECKED);
                            }
                            UpdateStatus(eSTATES.POLICY_CHECKED, crd);


                            CtrlImage exppImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo, policyNumber, string.Empty, string.Empty);
                            wfeImage expwImage = new wfeImage(sqlCon, exppImage);
                            expwImage.UpdateAllImageStatus(eSTATES.PAGE_CHECKED, crd);

                            PolicyDetails("1");
                            SwitchToSelectionTab();
                            CheckBatchRejection(cmbBatch.SelectedValue.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("This bundle is already marked as ready for UAT.....");
                    }
                }
                else
                {
                    MessageBox.Show("You are not authorized to do this.....");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public int GetPolicyStatus()
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return Convert.ToInt32(ob.GetPolicyStatus(projCode, batchCode, policyNumber).Tables[0].Rows[0]["status"]);
        }
        private void CmdRejected_Click(object sender, EventArgs e)
        {
            bool expBol = false;
            policyException udtExp = new policyException();
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
            string pageName = null;
            if (crd.role == ihConstants._LIC_ROLE)
            {
                if (chkReadyUat.Checked == false)
                {
                    if (lstImage.Items.Count > 0)
                    {
                        pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo, policyNumber);
                        wfePolicy policy = new wfePolicy(sqlCon, pPolicy);
                        if (chkCropClean.Checked == true)
                        {
                            udtExp.crop_clean_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.crop_clean_exp = 0;
                        }

                        if (chkDesicion.Checked == true)
                        {
                            udtExp.decision_misd_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.decision_misd_exp = 0;
                        }

                        if (chkExtraPage.Checked == true)
                        {
                            udtExp.extra_page_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.extra_page_exp = 0;
                        }

                        if (chkLinkedPolicy.Checked == true)
                        {
                            udtExp.linked_policy_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.linked_policy_exp = 0;
                        }

                        if (chkMissingImg.Checked == true)
                        {
                            udtExp.missing_img_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.missing_img_exp = 0;
                        }
                        if (chkMove.Checked == true)
                        {
                            udtExp.move_to_respective_policy_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.move_to_respective_policy_exp = 0;
                        }
                        if (chkOther.Checked == true)
                        {
                            udtExp.other_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.other_exp = 0;
                        }

                        if (chkPoorScan.Checked == true)
                        {
                            udtExp.poor_scan_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.poor_scan_exp = 0;
                        }
                        if (chkRearrange.Checked == true)
                        {
                            udtExp.rearrange_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.rearrange_exp = 0;
                        }
                        if (chkIndexing.Checked == true)
                        {
                            udtExp.wrong_indexing_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.wrong_indexing_exp = 0;
                        }
                        if (chkMeta.Checked == true)
                        {
                            udtExp.metadata_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.metadata_exp = 0;
                        }
                        udtExp.comments = txtComments.Text;
                        if (expBol == true)
                        {
                            udtExp.solved = ihConstants._POLICY_EXCEPTION_NOT_SOLVED;
                            if (policy.GetAllExceptionCheck().Tables[0].Rows.Count > 0)
                            {
                                if (policy.UpdateQaPolicyException(crd, udtExp) == true)
                                {
                                    if (policy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_NOT_SOLVED, ihConstants._LIC_QA_POLICY_EXCEPTION) == true)
                                    {
                                        UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);

                                        CtrlImage exppImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo, policyNumber, string.Empty, string.Empty);
                                        wfeImage expwImage = new wfeImage(sqlCon, exppImage);
                                        expwImage.UpdateAllImageStatus(eSTATES.PAGE_EXCEPTION, crd);

                                        PolicyDetails("1");
                                    }
                                }
                                SwitchToSelectionTab();
                                CheckBatchRejection(cmbBatch.SelectedValue.ToString());
                            }
                            else
                            {
                                policy.InitiateQaPolicyException(crd);
                                if (policy.UpdateQaPolicyException(crd, udtExp) == true)
                                {
                                    if (policy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_NOT_SOLVED, ihConstants._LIC_QA_POLICY_EXCEPTION) == true)
                                    {
                                        UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);

                                        CtrlImage exppImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo, policyNumber, string.Empty, string.Empty);
                                        wfeImage expwImage = new wfeImage(sqlCon, exppImage);
                                        expwImage.UpdateAllImageStatus(eSTATES.PAGE_EXCEPTION, crd);

                                        PolicyDetails("1");
                                    }
                                }
                                SwitchToSelectionTab();
                                CheckBatchRejection(cmbBatch.SelectedValue.ToString());
                            }

                        }
                        else
                        {
                            MessageBox.Show("Provide atleast one exception type", "B'Zer", MessageBoxButtons.OK);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("This bundle is already marked as ready for UAT.....");
                }
            }
            else
            {
                MessageBox.Show("You are not authorized to do this.....");
            }
        }

        public bool PolicyWithLICException(int prmProjKey, int prmBatchKey)
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);

            if (ob.PolicyWithLICException(prmProjKey, prmBatchKey).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool UpdateStatus(eSTATES state, int prmProjKey, int prmBatchKey)
        {
            UpdateStatusLogic ob = new UpdateStatusLogic(sqlCon);
            return ob.UpdateBundleStatus(prmProjKey.ToString(), prmBatchKey.ToString(), new eSTATES[] { }, new eSTATES[] { eSTATES.BATCH_READY_FOR_UAT }, state);
        }
        public DataTable getAllFiles()
        {
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetAllFiles(projCode, batchCode);
        }
        public DataSet GetAllException(string policy)
        {
            string project = cmbProject.SelectedValue.ToString(), batch = cmbBatch.SelectedValue.ToString();
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.GetAllException(policy, project, batch);
        }
        public bool QaExceptionStatus(int prmStatus, int prmExpStatus, string policy)
        {
            string project = cmbProject.SelectedValue.ToString(), batch = cmbBatch.SelectedValue.ToString();
            aeLicQaLogic ob = new aeLicQaLogic(sqlCon);
            return ob.UpdateQaExceptionStatus(prmStatus, prmExpStatus, policy, project, batch, crd);
        }
        private void chkReadyUat_Click(object sender, EventArgs e)
        {
            DialogResult dlg;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            ///changed in version 1.0.2
            if (crd.role == ihConstants._LIC_ROLE)
            {
                if ((cmbProject.SelectedValue != null) && (cmbBatch.SelectedValue != null))
                {
                    if ((grdBox.Rows.Count > 0) && (grdPolicy.Rows.Count > 0))
                    {
                        bool hasPolicyWithLICException = PolicyWithLICException(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                        var dsFiles = GetPolicyList(new[] { eSTATES.POLICY_CHECKED });
                        var isAnyFileChecked = dsFiles.Tables[0].Rows.Cast<DataRow>()
                                               .Any(row => Convert.ToInt32(row["status"]) == (int)eSTATES.POLICY_CHECKED);
                        if (!hasPolicyWithLICException && isAnyFileChecked && chkReadyUat.Checked == true)
                        {
                            dlg = MessageBox.Show(this, "Are you sure, this bundle is ready for UAT?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dlg == DialogResult.Yes)
                            {
                                var allFiles = getAllFiles();
                                allFiles.Rows.OfType<DataRow>().Where(row => GetAllException(row[0].ToString()).Tables[0].Rows.Count == 0)
                                                                .ToList()
                                                                .ForEach(row =>
                                                                 {
                                                                     wPolicy.InitiateQaPolicyException(crd, row[0].ToString());
                                                                     QaExceptionStatus(ihConstants._POLICY_EXCEPTION_SOLVED, ihConstants._LIC_QA_POLICY_CHECKED, row[0].ToString());
                                                                 });

                                UpdateStatus(eSTATES.BATCH_READY_FOR_UAT, Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                                chkReadyUat.Checked = true;
                                chkReadyUat.Enabled = false;
                                PopulateBatchCombo();
                            }
                            else
                            {
                                chkReadyUat.Checked = false;
                                chkReadyUat.Enabled = true;
                            }
                        }
                        else
                        {
                            if (hasPolicyWithLICException)
                            {
                                MessageBox.Show("One or more files is in exception stage, clear the exceptions before proceeding....");
                            }
                            else
                            {
                                MessageBox.Show("Atleast one file has to get accepted to accept the batch!!!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            chkReadyUat.Checked = false;
                            chkReadyUat.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Populate file details.....");
                    }
                }
            }
            else
            {
                MessageBox.Show("You are not authorized to do this.....");
                chkReadyUat.Checked = false;
            }
        }

        private void ChkMeta_CheckedChanged(object sender, EventArgs e)
        {
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkMeta.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Wrong metadata entry \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Wrong metadata entry \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }

        private void buttonFileDetails_Click(object sender, EventArgs e)
        {
            if (grdPolicy.SelectedCells.Count > 0)
            {
                var selectedFileName = GetFileNameFromSelectedRow();
                if (selectedFileName != null)
                {
                    var projKey = cmbProject.SelectedValue.ToString();
                    var bundleKey = cmbBatch.SelectedValue.ToString();
                    IEntryFormBuilder frmBuilder = new EntryFormBuilder();
                    var fm = frmBuilder.CreateEntryForm(sqlCon, projKey, bundleKey, DataLayerDefs.Mode._View, selectedFileName, crd);
                    fm.ShowDialog(this);
                }
            }
        }
    }
}
