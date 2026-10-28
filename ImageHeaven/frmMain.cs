using bzer_data_entry;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using Reporting;
using System;
using System.Data;
using System.Data.Odbc;
using System.Reflection;
//using System.Linq;
using System.Windows.Forms;

namespace ImageHeaven
{
    public partial class frmMain : Form
    {

        static wItem wi;
        //NovaNet.Utils.dbCon dbcon;
        frmMain mainForm;
        OdbcConnection sqlCon = null;
        public Credentials crd;
        static int colorMode;
        dbCon dbcon;

        //
        NovaNet.Utils.GetProfile pData;
        NovaNet.Utils.ChangePassword pCPwd;
        NovaNet.Utils.Profile p;
        public static NovaNet.Utils.IntrRBAC rbc;
        private short logincounter;
        //
        OdbcTransaction txn;

        public static string projKey;
        public static string bundleKey;
        public static string projectName = null;
        public static string batchName = null;
        public static string boxNumber = null;
        public static string projectVal = null;
        public static string batchVal = null;

        public static string name;

        public static int height;
        public static int width;

        public frmMain(OdbcConnection pCon)
        {
            InitializeComponent();

            sqlCon = pCon;

            logincounter = 0;

            ImageHeaven.Program.Logout = false;
        }

        public frmMain()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
            this.Text = "B'Zer - " + "           Version: " + assemName.ToString();
            InitializeComponent();

            logincounter = 0;
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            height = pictureBox1.Height;
            width = pictureBox1.Width;

            int k;
            dbcon = new NovaNet.Utils.dbCon();
            try
            {
                string dllPaths = string.Empty;

                menuStrip1.Visible = false;


                if (sqlCon.State == ConnectionState.Open)
                {
                    pData = getData;
                    pCPwd = getCPwd;
                    rbc = NovaNet.Utils.RBAC.GetInstance(sqlCon, dbcon, pData, pCPwd);
                    GetChallenge gc = new GetChallenge(getData);

                    gc.ShowDialog(this);

                    crd = rbc.getCredentials(p);
                    AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
                    this.Text = "B'Zer - " + "           Version: " + assemName.Version.ToString() + "-" + ImageHeaven.BuildVersion.Version + "    Database name: " + sqlCon.Database.ToString() + "    Logged in user: " + crd.userName;

                    name = crd.userName;


                    if (crd.role == ihConstants._ADMINISTRATOR_ROLE || crd.role == "Supervisor")
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Visible = true;
                        newToolStripMenuItem.Enabled = true;
                        toolsToolStripMenuItem.Visible = true;
                        dataEntryToolStripMenuItem.Visible = true;
                        exportToolStripMenuItem.Visible = true;
                        newToolStripMenuItem.Visible = true;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newUserToolStripMenuItem.Visible = true;
                        onlineUsersToolStripMenuItem.Visible = true;
                        dataEntryToolStripMenuItem.Visible = true;
                        imageImportToolStripMenuItem.Visible = true;
                        exportToolStripMenuItem.Visible = true;
                        batchUploadToolStripMenuItem.Visible = true;
                        batchUploadToolStripMenuItem.Enabled = true;
                        toolStrip1.Visible = true;
                        imageQualityControlToolStripMenuItem.Visible = true;
                        toolStripButton3.Visible = true;
                        indexingToolStripMenuItem.Visible = true;
                        toolStripButton2.Visible = true;
                        qualityControlFinalToolStripMenuItem.Visible = true;
                        toolStripButton4.Visible = true;
                        toolStripMenuItem1.Visible = true;
                        outwardSubmissionToolStripMenuItem.Visible = true;
                        reportToolStripMenuItem.Visible = true;
                        reportToolStripMenuItem.Enabled = true;
                        dashboardToolStripMenuItem1.Visible = true;
                        dashboardToolStripMenuItem1.Enabled = true;
                        productionReportToolStripMenuItem.Visible = true;
                        productionReportToolStripMenuItem.Enabled = true;
                        siteReportToolStripMenuItem.Visible = true;
                        siteReportToolStripMenuItem.Enabled = true;
                        masterUpdateToolStripMenuItem.Visible = true;
                        masterUpdateToolStripMenuItem.Enabled = true;

                        generateBarcodeToolStripMenuItem1.Visible = true;
                        caseFileCreationToolStripMenuItem.Visible = false;
                        caseFileCreationToolStripMenuItem.Enabled = false;

                        configurationToolStripMenuItem.Visible = true;
                        helpToolStripMenuItem.Visible = true;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = true;
                        locationConfigurationToolStripMenuItem.Visible = true;
                        aboutToolStripMenuItem.Visible = true;
                        auditToolStripMenuItem.Visible = true;
                        detailDashboardToolStripMenuItem.Visible = true;
                        detailedDashboardToolStripMenuItem.Visible = true;
                        userWiseReportToolStripMenuItem.Visible = true;
                        uATCertificateToolStripMenuItem.Visible = true;
                        outwardReportToolStripMenuItem.Visible = true;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                    }
                    else if (crd.role == "Metadata Entry")
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Enabled = true;
                        newToolStripMenuItem.Visible = true;
                        projectToolStripMenuItem.Enabled = false;
                        batchToolStripMenuItem.Enabled = false;
                        caseFileCreationToolStripMenuItem.Visible = false;
                        caseFileCreationToolStripMenuItem.Enabled = false;
                        masterUpdateToolStripMenuItem.Visible = false;
                        masterUpdateToolStripMenuItem.Enabled = false;
                        generateBarcodeToolStripMenuItem1.Enabled = false;
                        exitToolStripMenuItem.Enabled = true;
                        exitToolStripMenuItem.Visible = true;
                        transactionsToolStripMenuItem.Enabled = true;
                        transactionsToolStripMenuItem.Visible = true;
                        dataEntryToolStripMenuItem.Visible = true;
                        batchUploadToolStripMenuItem.Enabled = false;
                        batchUploadToolStripMenuItem.Visible = false;
                        imageImportToolStripMenuItem.Enabled = false;
                        imageImportToolStripMenuItem.Visible = false;
                        imageQualityControlToolStripMenuItem.Enabled = false;
                        imageQualityControlToolStripMenuItem.Visible = false;
                        indexingToolStripMenuItem.Enabled = false;
                        indexingToolStripMenuItem.Visible = false;
                        qualityControlFinalToolStripMenuItem.Enabled = false;
                        qualityControlFinalToolStripMenuItem.Visible = false;
                        toolStripMenuItem1.Enabled = false;
                        toolStripMenuItem1.Visible = false;
                        exportToolStripMenuItem.Enabled = false;
                        exportToolStripMenuItem.Visible = false;
                        toolsToolStripMenuItem.Enabled = true;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Enabled = true;
                        newUserToolStripMenuItem.Visible = false;
                        newUserToolStripMenuItem.Enabled = false;
                        onlineUsersToolStripMenuItem.Visible = false;
                        onlineUsersToolStripMenuItem.Enabled = false;
                        reportToolStripMenuItem.Visible = true;
                        reportToolStripMenuItem.Enabled = true;
                        dashboardToolStripMenuItem1.Visible = false;
                        dashboardToolStripMenuItem1.Enabled = false;
                        productionReportToolStripMenuItem.Visible = true;
                        productionReportToolStripMenuItem.Enabled = true;
                        siteReportToolStripMenuItem.Visible = false;
                        siteReportToolStripMenuItem.Enabled = false;
                        logoutToolStripMenuItem.Visible = true;
                        toolStrip1.Visible = false;
                        toolStrip1.Enabled = false;
                        configurationToolStripMenuItem.Visible = false;
                        outwardSubmissionToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                        helpToolStripMenuItem.Visible = true;
                        locationConfigurationToolStripMenuItem.Visible = false;
                        aboutToolStripMenuItem.Visible = true;
                        detailDashboardToolStripMenuItem.Visible = false;
                        detailedDashboardToolStripMenuItem.Visible = false;
                        auditToolStripMenuItem.Visible = false;
                        userWiseReportToolStripMenuItem.Visible = false;
                        uATCertificateToolStripMenuItem.Visible = false;
                        outwardReportToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                    }
                    else if (crd.role == "Scan")
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Enabled = false;
                        newToolStripMenuItem.Visible = false;
                        projectToolStripMenuItem.Enabled = false;
                        batchToolStripMenuItem.Enabled = false;
                        caseFileCreationToolStripMenuItem.Visible = false;
                        caseFileCreationToolStripMenuItem.Enabled = false;

                        masterUpdateToolStripMenuItem.Visible = false;
                        masterUpdateToolStripMenuItem.Enabled = false;

                        generateBarcodeToolStripMenuItem1.Enabled = false;
                        exitToolStripMenuItem.Enabled = false;
                        transactionsToolStripMenuItem.Enabled = true;
                        transactionsToolStripMenuItem.Visible = true;
                        dataEntryToolStripMenuItem.Visible = false;
                        batchUploadToolStripMenuItem.Enabled = false;
                        batchUploadToolStripMenuItem.Visible = false;
                        imageImportToolStripMenuItem.Enabled = true;
                        imageImportToolStripMenuItem.Visible = true;
                        imageQualityControlToolStripMenuItem.Enabled = false;
                        imageQualityControlToolStripMenuItem.Visible = false;
                        indexingToolStripMenuItem.Enabled = false;
                        indexingToolStripMenuItem.Visible = false;
                        qualityControlFinalToolStripMenuItem.Enabled = true;
                        qualityControlFinalToolStripMenuItem.Visible = true;
                        toolStripMenuItem1.Enabled = false;
                        toolStripMenuItem1.Visible = false;
                        exportToolStripMenuItem.Enabled = false;
                        exportToolStripMenuItem.Visible = false;
                        toolsToolStripMenuItem.Enabled = true;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Enabled = true;
                        newUserToolStripMenuItem.Visible = false;
                        newUserToolStripMenuItem.Enabled = false;
                        onlineUsersToolStripMenuItem.Visible = false;
                        onlineUsersToolStripMenuItem.Enabled = false;
                        reportToolStripMenuItem.Visible = true;
                        reportToolStripMenuItem.Enabled = true;
                        dashboardToolStripMenuItem1.Visible = false;
                        dashboardToolStripMenuItem1.Enabled = false;
                        productionReportToolStripMenuItem.Visible = true;
                        productionReportToolStripMenuItem.Enabled = true;
                        siteReportToolStripMenuItem.Visible = false;
                        siteReportToolStripMenuItem.Enabled = false;
                        logoutToolStripMenuItem.Visible = true;
                        toolStrip1.Visible = true;
                        toolStrip1.Enabled = true;
                        toolStripButton3.Visible = false;
                        toolStripButton2.Visible = false;
                        toolStripButton4.Visible = true;

                        configurationToolStripMenuItem.Visible = false;
                        outwardSubmissionToolStripMenuItem.Visible = false;

                        helpToolStripMenuItem.Visible = true;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                        locationConfigurationToolStripMenuItem.Visible = false;
                        aboutToolStripMenuItem.Visible = true;
                        detailDashboardToolStripMenuItem.Visible = false;
                        detailedDashboardToolStripMenuItem.Visible = false;
                        auditToolStripMenuItem.Visible = false;
                        userWiseReportToolStripMenuItem.Visible = false;
                        outwardReportToolStripMenuItem.Visible = false;
                        uATCertificateToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                    }
                    else if (crd.role == "QC")
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Enabled = false;
                        newToolStripMenuItem.Visible = false;
                        projectToolStripMenuItem.Enabled = false;
                        batchToolStripMenuItem.Enabled = false;
                        caseFileCreationToolStripMenuItem.Visible = false;
                        caseFileCreationToolStripMenuItem.Enabled = false;

                        masterUpdateToolStripMenuItem.Visible = false;
                        masterUpdateToolStripMenuItem.Enabled = false;

                        generateBarcodeToolStripMenuItem1.Enabled = false;
                        exitToolStripMenuItem.Enabled = false;
                        transactionsToolStripMenuItem.Enabled = true;
                        transactionsToolStripMenuItem.Visible = true;
                        dataEntryToolStripMenuItem.Enabled = false;
                        dataEntryToolStripMenuItem.Visible = false;
                        batchUploadToolStripMenuItem.Enabled = false;
                        batchUploadToolStripMenuItem.Visible = false;
                        imageImportToolStripMenuItem.Enabled = false;
                        imageImportToolStripMenuItem.Visible = false;
                        imageQualityControlToolStripMenuItem.Enabled = true;
                        imageQualityControlToolStripMenuItem.Visible = true;
                        indexingToolStripMenuItem.Enabled = false;
                        indexingToolStripMenuItem.Visible = false;
                        qualityControlFinalToolStripMenuItem.Enabled = false;
                        qualityControlFinalToolStripMenuItem.Visible = false;
                        toolStripMenuItem1.Enabled = false;
                        toolStripMenuItem1.Visible = false;
                        exportToolStripMenuItem.Enabled = false;
                        exportToolStripMenuItem.Visible = false;
                        toolsToolStripMenuItem.Enabled = true;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Enabled = true;
                        newUserToolStripMenuItem.Visible = false;
                        newUserToolStripMenuItem.Enabled = false;
                        onlineUsersToolStripMenuItem.Visible = false;
                        onlineUsersToolStripMenuItem.Enabled = false;
                        reportToolStripMenuItem.Visible = true;
                        reportToolStripMenuItem.Enabled = true;
                        dashboardToolStripMenuItem1.Visible = false;
                        dashboardToolStripMenuItem1.Enabled = false;
                        productionReportToolStripMenuItem.Visible = true;
                        productionReportToolStripMenuItem.Enabled = true;
                        siteReportToolStripMenuItem.Visible = false;
                        siteReportToolStripMenuItem.Enabled = false;
                        logoutToolStripMenuItem.Visible = true;
                        toolStrip1.Visible = true;
                        toolStrip1.Enabled = true;
                        toolStripButton3.Visible = true;
                        toolStripButton2.Visible = false;
                        toolStripButton4.Visible = false;

                        configurationToolStripMenuItem.Visible = false;
                        outwardSubmissionToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                        helpToolStripMenuItem.Visible = true;
                        locationConfigurationToolStripMenuItem.Visible = false;
                        aboutToolStripMenuItem.Visible = true;
                        detailDashboardToolStripMenuItem.Visible = false;
                        detailedDashboardToolStripMenuItem.Visible = false;
                        auditToolStripMenuItem.Visible = false;
                        userWiseReportToolStripMenuItem.Visible = false;
                        outwardReportToolStripMenuItem.Visible = false;
                        uATCertificateToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                    }
                    else if (crd.role == "DOC Type Association")
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Enabled = false;
                        newToolStripMenuItem.Visible = false;
                        projectToolStripMenuItem.Enabled = false;
                        batchToolStripMenuItem.Enabled = false;
                        caseFileCreationToolStripMenuItem.Visible = false;
                        caseFileCreationToolStripMenuItem.Enabled = false;

                        masterUpdateToolStripMenuItem.Visible = false;
                        masterUpdateToolStripMenuItem.Enabled = false;

                        generateBarcodeToolStripMenuItem1.Enabled = false;
                        exitToolStripMenuItem.Enabled = false;
                        transactionsToolStripMenuItem.Enabled = true;
                        transactionsToolStripMenuItem.Visible = true;
                        dataEntryToolStripMenuItem.Enabled = false;
                        dataEntryToolStripMenuItem.Visible = false;
                        batchUploadToolStripMenuItem.Enabled = false;
                        batchUploadToolStripMenuItem.Visible = false;
                        imageImportToolStripMenuItem.Enabled = false;
                        imageImportToolStripMenuItem.Visible = false;
                        imageQualityControlToolStripMenuItem.Enabled = false;
                        imageQualityControlToolStripMenuItem.Visible = false;
                        indexingToolStripMenuItem.Enabled = true;
                        indexingToolStripMenuItem.Visible = true;
                        qualityControlFinalToolStripMenuItem.Enabled = true;
                        qualityControlFinalToolStripMenuItem.Visible = true;
                        toolStripMenuItem1.Enabled = false;
                        toolStripMenuItem1.Visible = false;
                        exportToolStripMenuItem.Enabled = false;
                        exportToolStripMenuItem.Visible = false;
                        toolsToolStripMenuItem.Enabled = true;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Enabled = true;
                        newUserToolStripMenuItem.Visible = false;
                        newUserToolStripMenuItem.Enabled = false;
                        onlineUsersToolStripMenuItem.Visible = false;
                        onlineUsersToolStripMenuItem.Enabled = false;
                        reportToolStripMenuItem.Visible = true;
                        reportToolStripMenuItem.Enabled = true;
                        dashboardToolStripMenuItem1.Visible = false;
                        dashboardToolStripMenuItem1.Enabled = false;
                        productionReportToolStripMenuItem.Visible = true;
                        productionReportToolStripMenuItem.Enabled = true;
                        siteReportToolStripMenuItem.Visible = false;
                        siteReportToolStripMenuItem.Enabled = false;
                        logoutToolStripMenuItem.Visible = true;
                        toolStrip1.Visible = true;
                        toolStrip1.Enabled = true;
                        toolStripButton3.Visible = false;
                        toolStripButton2.Visible = true;
                        toolStripButton4.Visible = true;
                        outwardSubmissionToolStripMenuItem.Visible = false;
                        configurationToolStripMenuItem.Visible = false;

                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                        helpToolStripMenuItem.Visible = true;
                        locationConfigurationToolStripMenuItem.Visible = false;
                        aboutToolStripMenuItem.Visible = true;
                        detailDashboardToolStripMenuItem.Visible = false;
                        detailedDashboardToolStripMenuItem.Visible = false;
                        auditToolStripMenuItem.Visible = false;
                        userWiseReportToolStripMenuItem.Visible = false;
                        outwardReportToolStripMenuItem.Visible = false;
                        uATCertificateToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                    }
                    else if (crd.role == "InventoryIn")
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Enabled = true;
                        newToolStripMenuItem.Visible = true;
                        projectToolStripMenuItem.Enabled = false;
                        batchToolStripMenuItem.Enabled = true;
                        batchToolStripMenuItem.Visible = true;
                        caseFileCreationToolStripMenuItem.Visible = false;
                        caseFileCreationToolStripMenuItem.Enabled = false;


                        masterUpdateToolStripMenuItem.Visible = false;
                        masterUpdateToolStripMenuItem.Enabled = false;

                        generateBarcodeToolStripMenuItem1.Enabled = true;
                        generateBarcodeToolStripMenuItem1.Visible = true;
                        exitToolStripMenuItem.Enabled = true;
                        exitToolStripMenuItem.Visible = true;
                        transactionsToolStripMenuItem.Enabled = false;
                        transactionsToolStripMenuItem.Visible = false;
                        dataEntryToolStripMenuItem.Enabled = false;
                        dataEntryToolStripMenuItem.Visible = false;
                        batchUploadToolStripMenuItem.Enabled = false;
                        batchUploadToolStripMenuItem.Visible = false;
                        imageImportToolStripMenuItem.Enabled = false;
                        imageImportToolStripMenuItem.Visible = false;
                        imageQualityControlToolStripMenuItem.Enabled = false;
                        imageQualityControlToolStripMenuItem.Visible = false;
                        indexingToolStripMenuItem.Enabled = false;
                        indexingToolStripMenuItem.Visible = false;
                        qualityControlFinalToolStripMenuItem.Enabled = false;
                        qualityControlFinalToolStripMenuItem.Visible = false;
                        toolStripMenuItem1.Enabled = false;
                        toolStripMenuItem1.Visible = false;
                        exportToolStripMenuItem.Enabled = false;
                        exportToolStripMenuItem.Visible = false;
                        toolsToolStripMenuItem.Enabled = true;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Enabled = true;
                        newUserToolStripMenuItem.Visible = false;
                        newUserToolStripMenuItem.Enabled = false;
                        onlineUsersToolStripMenuItem.Visible = false;
                        onlineUsersToolStripMenuItem.Enabled = false;
                        reportToolStripMenuItem.Visible = false;
                        reportToolStripMenuItem.Enabled = false;
                        dashboardToolStripMenuItem1.Visible = false;
                        dashboardToolStripMenuItem1.Enabled = false;
                        productionReportToolStripMenuItem.Visible = false;
                        productionReportToolStripMenuItem.Enabled = false;
                        siteReportToolStripMenuItem.Visible = false;
                        siteReportToolStripMenuItem.Enabled = false;
                        logoutToolStripMenuItem.Visible = true;
                        toolStrip1.Visible = false;
                        toolStrip1.Enabled = false;

                        locationConfigurationToolStripMenuItem.Visible = false;
                        configurationToolStripMenuItem.Visible = false;
                        outwardSubmissionToolStripMenuItem.Visible = false;

                        helpToolStripMenuItem.Visible = true;

                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                        aboutToolStripMenuItem.Visible = true;
                        auditToolStripMenuItem.Visible = false;
                        detailDashboardToolStripMenuItem.Visible = false;
                        detailedDashboardToolStripMenuItem.Visible = false;
                        userWiseReportToolStripMenuItem.Visible = false;
                        outwardReportToolStripMenuItem.Visible = false;
                        uATCertificateToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                    }
                    else if (crd.role == ihConstants._LIC_ROLE)
                    {
                        toolStrip1.Visible = false;
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Visible = false;
                        newToolStripMenuItem.Enabled = false;
                        transactionsToolStripMenuItem.Visible = false;
                        transactionsToolStripMenuItem.Enabled = false;
                        auditToolStripMenuItem.Visible = true;
                        masterUpdateToolStripMenuItem.Visible = false;
                        masterUpdateToolStripMenuItem.Enabled = false;
                        toolsToolStripMenuItem.Visible = true;
                        configurationToolStripMenuItem.Visible = false;
                        outwardSubmissionToolStripMenuItem.Visible = false;
                        newPasswordToolStripMenuItem.Visible = true;
                        newUserToolStripMenuItem.Visible = false;
                        onlineUsersToolStripMenuItem.Visible = false;
                        reportToolStripMenuItem.Visible = true;
                        dashboardToolStripMenuItem1.Visible = false;
                        productionReportToolStripMenuItem.Visible = true;
                        siteReportToolStripMenuItem.Visible = false;
                        userWiseReportToolStripMenuItem.Visible = false;
                        helpToolStripMenuItem.Visible = true;
                        logoutToolStripMenuItem.Visible = true;
                        uATCertificateToolStripMenuItem.Visible = true;
                        outwardReportToolStripMenuItem.Visible = false;
                        detailDashboardToolStripMenuItem.Visible = false;
                        detailedDashboardToolStripMenuItem.Visible = false;
                        locationConfigurationToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        menuStrip1.Visible = true;
                        newToolStripMenuItem.Visible = false;
                        toolsToolStripMenuItem.Visible = true;
                        newPasswordToolStripMenuItem.Visible = true;
                        newUserToolStripMenuItem.Visible = false;
                        onlineUsersToolStripMenuItem.Visible = false;
                        dataEntryToolStripMenuItem.Visible = true;
                        imageImportToolStripMenuItem.Visible = true;
                        exportToolStripMenuItem.Visible = false;
                        batchUploadToolStripMenuItem.Visible = false;
                        toolStrip1.Visible = false;
                        toolStrip1.Enabled = false;
                        caseFileCreationToolStripMenuItem.Visible = false;
                        caseFileCreationToolStripMenuItem.Enabled = false;
                        reportToolStripMenuItem.Visible = false;
                        reportToolStripMenuItem.Enabled = false;
                        dashboardToolStripMenuItem1.Visible = false;
                        dashboardToolStripMenuItem1.Enabled = false;
                        productionReportToolStripMenuItem.Visible = false;
                        productionReportToolStripMenuItem.Enabled = false;
                        siteReportToolStripMenuItem.Visible = false;
                        siteReportToolStripMenuItem.Enabled = false;
                        outwardSubmissionToolStripMenuItem.Visible = false;

                        masterUpdateToolStripMenuItem.Visible = false;
                        masterUpdateToolStripMenuItem.Enabled = false;

                        configurationToolStripMenuItem.Visible = false;
                        userWiseReportToolStripMenuItem.Visible = false;

                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                        helpToolStripMenuItem.Visible = true;
                        locationConfigurationToolStripMenuItem.Visible = false;
                        aboutToolStripMenuItem.Visible = true;
                        detailDashboardToolStripMenuItem.Visible = false;
                        detailedDashboardToolStripMenuItem.Visible = false;
                        auditToolStripMenuItem.Visible = true;
                        uATCertificateToolStripMenuItem.Visible = true;
                        outwardReportToolStripMenuItem.Visible = false;
                        additaionalDetailsUpdateToolStripMenuItem.Visible = false;
                    }
                    exportToolStripMenuItem.Visible = true;
                    exportmouzaToolStripMenuItem.Visible = false;
                    imageQualityControlToolStripMenuItem.Visible = false ;
                    indexingToolStripMenuItem.Visible=false;
                    iQCToolStripMenuItem.Visible = false;
                    outwardSubmissionToolStripMenuItem.Visible = false;
                }
            }
            catch (DBConnectionException dbex)
            {
                string err = dbex.Message;
                this.Close();
            }
        }
        void getData(ref NovaNet.Utils.Profile prmp)
        {
            int i;
            p = prmp;
            for (i = 1; i <= 2; i++)
            {
                if (rbc.authenticate(p.UserId, p.Password) == false)
                {
                    if (logincounter == 2)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        logincounter++;
                        GetChallenge ogc = new GetChallenge(getData);
                        ogc.ShowDialog(this);
                    }
                }
                else
                {
                    if (rbc.CheckUserIsLogged(p.UserId))
                    {

                        p = rbc.getProfile();
                        crd = rbc.getCredentials(p);
                        if (crd.role != ihConstants._ADMINISTRATOR_ROLE)
                        {
                            rbc.LockedUser(p.UserId, crd.created_dttm);
                        }
                        break;
                    }
                    else
                    {
                        p.UserId = null;
                        p.UserName = null;
                        GetChallenge ogc = new GetChallenge(getData);
                        AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
                        this.Text = "B'Zer - " + "           Version: " + assemName.Version.ToString() + "    Database name: " + sqlCon.Database.ToString() + "    Logged in user: " + crd.userName;
                        ogc.ShowDialog(this);
                    }
                }
            }
        }
        void getCPwd(ref NovaNet.Utils.Profile prmpwd)
        {
            p = prmpwd;
            rbc.changePassword(p.UserId, p.UserName, p.Password);
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmProject dispProject;
                wi = new wfeProject(sqlCon);
                dispProject = new frmProject(wi, sqlCon);
                dispProject.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rbc.UnLockedUser(crd.created_by.ToString());
            this.Close();
        }

        private void batchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmBatch dispProject;
                wi = new wfeBatch(sqlCon);
                dispProject = new frmBatch(wi, sqlCon, DataLayerDefs.Mode._Add);
                dispProject.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void newPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PwdChange pwdCh = new PwdChange(ref p, getCPwd);
            pwdCh.ShowDialog(this);
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewUser nwUsr = new AddNewUser(getnwusrData, sqlCon);
            nwUsr.ShowDialog(this);
        }
        void getnwusrData(ref NovaNet.Utils.Profile prmp)
        {
            p = prmp;
            if (rbc.addUser(p.UserId, p.UserName, p.Role_des, p.Password) == false)
            {
                AddNewUser nwUsr = new AddNewUser(getnwusrData, sqlCon);
                nwUsr.ShowDialog(this);
            }
        }

        private void onlineUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoggedUser loged = new frmLoggedUser(rbc, crd);
            loged.ShowDialog(this);
        }

        public string _GetFileCount(string proj_code, string bundle_key)
        {
            DataTable dt = new DataTable();
            string sql = "select COUNT(*) from case_file_master where proj_code = '" + proj_code + "' and bundle_key = '" + bundle_key + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        public string _GetEntryCount(string proj_code, string bundle_key)
        {
            DataTable dt = new DataTable();
            string sql = "select COUNT(*) from metadata_entry where proj_code = '" + proj_code + "' and bundle_key = '" + bundle_key + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        public DataTable _GetBundleStatus(string proj, string bundle)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct status from bundle_master where proj_code = '" + proj + "' and bundle_key = '" + bundle + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        private void dataEntryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[2];
            state[0] = NovaNet.wfe.eSTATES.METADATA_ENTRY;
            ISummaryFormBuilder frmBuilder = new SummaryFormBuilder();
            var fm = frmBuilder.CreateSummaryForm(sqlCon, crd, state);
            fm.ShowDialog(this);
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
            this.Text = "B'Zer - " + "           Version: " + assemName.Version.ToString() + "    Database name: " + sqlCon.Database.ToString();
            sqlCon.Close();
            sqlCon.Open();
            menuStrip1.Visible = false;
            rbc.UnLockedUser(crd.created_by.ToString());
            frmMain_Load(sender, e);
        }

        private void imageImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDataImport data = new frmDataImport(sqlCon, crd);
            data.ShowDialog(this);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExport export = new frmExport(sqlCon, crd);
            export.ShowDialog(this);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nevaehtech.com/");
        }

        private void batchUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBundleUpload frm = new frmBundleUpload(sqlCon, crd);
            frm.ShowDialog(this);
        }

        private void caseFileCreationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBundleSummary frm = new frmBundleSummary(sqlCon, crd, name);
            frm.ShowDialog();
        }

        private void imageQualityControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[1];
            state[0] = eSTATES.POLICY_SCANNED;
            Utils.frmBundleSelect box = new Utils.frmBundleSelect(state, sqlCon, crd);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = Utils.frmBundleSelect.projKey;
            bundleKey = Utils.frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "2")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeImageQC frmQc = new aeImageQC(sqlCon, crd);
                        frmQc.ShowDialog(this);
                    }
                }
            }
        }

        private void indexingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[2];
            state[0] = eSTATES.POLICY_QC;
            state[1] = eSTATES.POLICY_ON_HOLD;

            Utils.frmBundleSelect box = new Utils.frmBundleSelect(state, sqlCon, crd);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = Utils.frmBundleSelect.projKey;
            bundleKey = Utils.frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                var bundleStatusTable = _GetBundleStatus(projKey, bundleKey);
                var bundleStatus = bundleStatusTable.Rows[0][0].ToString();
                if (bundleStatus == "3" || bundleStatus == "2")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeIndexing frmIndex = new aeIndexing(sqlCon, crd);

                        frmIndex.ShowDialog(this);
                    }
                }

            }


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[1];
            state[0] = eSTATES.POLICY_SCANNED;
            Utils.frmBundleSelect box = new Utils.frmBundleSelect(state, sqlCon, crd);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = Utils.frmBundleSelect.projKey;
            bundleKey = Utils.frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "2")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeImageQC frmQc = new aeImageQC(sqlCon, crd);

                        frmQc.ShowDialog(this);
                    }
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            eSTATES[] state = new eSTATES[2];
            state[0] = eSTATES.POLICY_QC;
            state[1] = eSTATES.POLICY_ON_HOLD;

            Utils.frmBundleSelect box = new Utils.frmBundleSelect(state, sqlCon, crd);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = Utils.frmBundleSelect.projKey;
            bundleKey = Utils.frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "3" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "2")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeIndexing frmIndex = new aeIndexing(sqlCon, crd);

                        frmIndex.ShowDialog(this);
                    }
                }
            }
        }

        private void qualityControlFinalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[4];
            //state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            state[2] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[1] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            state[3] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

            Utils.frmBundleSelect box = new Utils.frmBundleSelect(state, sqlCon, crd);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = Utils.frmBundleSelect.projKey;
            bundleKey = Utils.frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "3" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "2" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "4" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "5" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "6" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "7" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "8" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "30" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "31" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "37" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "40" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "77")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeFQC frm = new aeFQC(sqlCon, crd);
                        frm.ShowDialog(this);
                    }
                }

            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[2];
            //state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            //state[2] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[1] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            Utils.frmBundleSelect box = new Utils.frmBundleSelect(state, sqlCon, crd);
            box.chkPhotoScan.Visible = false;
            box.ShowDialog(this);

            projKey = Utils.frmBundleSelect.projKey;
            bundleKey = Utils.frmBundleSelect.bundleKey;

            if (projKey != null && bundleKey != null)
            {
                //status check
                if (_GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "3" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "2" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "4" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "5" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "6" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "7" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "8" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "30" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "31" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "37" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "40" || _GetBundleStatus(projKey, bundleKey).Rows[0][0].ToString() == "77")
                {
                    if (Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) > 0 && Convert.ToInt32(_GetFileCount(projKey, bundleKey).ToString()) == Convert.ToInt32(_GetEntryCount(projKey, bundleKey).ToString()))
                    {
                        aeFQC frm = new aeFQC(sqlCon, crd);
                        frm.ShowDialog(this);
                    }
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmBundleSubmit frm = new frmBundleSubmit(sqlCon, crd);
            frm.ShowDialog(this);
        }

        private void generateBarcodeToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            frmBarcode frmB = new frmBarcode(sqlCon, crd);

            frmB.ShowDialog(this);

        }

        private void dashboardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDashboard dashboard = new frmDashboard(sqlCon);
            dashboard.Show();
        }

        private void productionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProduction frm = new frmProduction(sqlCon, crd);
            frm.ShowDialog(this);
        }

        private void siteReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reporting.frmSite frm = new Reporting.frmSite(sqlCon);
            frm.ShowDialog(this);
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aeConfiguration csvUploader = new aeConfiguration();
            mainForm = new frmMain();
            csvUploader.ShowDialog(mainForm);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About frm = new About();
            frm.ShowDialog(this);
        }

        private void auditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aeLicQa frm = new aeLicQa(sqlCon, crd);
            frm.ShowDialog(this);
        }

        private void userWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserReport frm = new frmUserReport(sqlCon);
            frm.ShowDialog(this);
        }

        private void uATCertificateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUAT frm = new frmUAT(sqlCon);
            frm.ShowDialog(this);
        }

        private void outwardSubmissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOutwardSubmission frm = new frmOutwardSubmission(sqlCon, crd);
            frm.ShowDialog(this);
        }

        private void outwardReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOutwardReport frm = new frmOutwardReport(sqlCon);
            frm.ShowDialog(this);
        }

        private void detailDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reporting.frmDashboard frm = new Reporting.frmDashboard(sqlCon);
            frm.ShowDialog(this);
        }

        private void detailedDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reporting.frmDetailDashboard frm = new Reporting.frmDetailDashboard(sqlCon);
            frm.ShowDialog(this);
        }

        private void locationConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocConfig frm = new frmLocConfig(sqlCon);
            frm.ShowDialog(this);
        }

        private void additaionalDetailsUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDelivery frm = new frmDelivery(sqlCon);
            frm.ShowDialog(this);
        }

        private void masterUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMasterUpdateFormBuilder masterFormBuilder = new MasterUpdateFormBuilder();
            var masterUpdateForm = masterFormBuilder.CreateMasterUpdateForm(sqlCon, crd);
            masterUpdateForm.ShowDialog(this);
        }

        private void iQCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = new SelectionBox(sqlCon, crd);
            obj.ShowDialog(this);
        }

        private void exportmouzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var export = new frmMouzaExport(sqlCon, crd);
            export.ShowDialog(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
