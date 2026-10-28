using ImageHeaven.Logic;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Windows.Forms;

namespace ImageHeaven
{
    public enum TypeState
    {
        ImageQC,
        DocType
    }
    public partial class SelectionBox : Form
    {
        private readonly OdbcConnection SqlCon;
        private string ProjectKey = null;
        private Credentials Crd;
        public SelectionBox(OdbcConnection con, Credentials crd)
        {
            InitializeComponent();
            SqlCon = con;
            Crd = crd;
        }
        // Create a Dictionary to map enum values to display text
        private Dictionary<TypeState, string> typeStateMap = new Dictionary<TypeState, string>
        {
            { TypeState.ImageQC, "Image QC" },
            {TypeState.DocType, "Doc Type" }
        };
        private void SelectionBox_Load(object sender, System.EventArgs e)
        {
            comboBoxSType.DataSource = new BindingSource(typeStateMap, null);
            comboBoxSType.DisplayMember = "Value";
            comboBoxSType.ValueMember = "Key";
            loadProject();
        }
        private void loadProject()
        {
            try
            {
                comboBoxProject.Text = "";
                var proj = new wfeProject(SqlCon);
                var dt = proj.GetAllValues().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    comboBoxProject.DataSource = dt;
                    comboBoxProject.DisplayMember = "proj_code";
                    comboBoxProject.ValueMember = "proj_key";
                }
                else
                {
                    MessageBox.Show("Add one project first...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonSelect_Click(object sender, EventArgs e)
        {
            ProjectKey = comboBoxProject.SelectedValue.ToString();

            Dictionary<TypeState, eSTATES> mapState = new Dictionary<TypeState, eSTATES>()
            {
                {TypeState.ImageQC, eSTATES.BATCH_CREATED },
                {TypeState.DocType, eSTATES.BATCH_SCANNED }
            };
            var selectedTypeState = mapState[(TypeState)comboBoxSType.SelectedValue];
            var batch = new wfeBatch(SqlCon);
            var batches = batch.GetAllValues(Convert.ToInt32(ProjectKey), new eSTATES[] { selectedTypeState });
            if (batches.Tables.Count > 0 && batches.Tables[0].Rows.Count > 0)
            {
                frmBulkStateChange obj = null;
                switch (selectedTypeState)
                {
                    case eSTATES.BATCH_CREATED:
                        var aeImgQCObj = new aeImageQCLogic(SqlCon, Crd);
                        obj = new ImageQCStateChange(SqlCon, ProjectKey, Crd, selectedTypeState, aeImgQCObj);
                        break;

                    case eSTATES.BATCH_SCANNED:
                        var aeIndexObj = new aeIndexingLogic(SqlCon, Crd);
                        obj = new DocTypeStateChange(SqlCon, ProjectKey, Crd, selectedTypeState, aeIndexObj);
                        break;

                    default:
                        MessageBox.Show("Wrong input.");
                        break;
                }
                obj?.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("No data found.");
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
