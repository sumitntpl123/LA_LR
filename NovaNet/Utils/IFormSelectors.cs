using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;
using NovaNet.wfe;

namespace NovaNet
{
    namespace Utils
    {
        /// <summary>
        /// Selects a bundle based on the provided states, database connection, and user credentials.
        /// </summary>
        /// <param name="prmState">An array of states used to determine the states.</param>
        /// <param name="prmCon">An ODBC connection to the database.</param>
        /// <param name="prmCrd">User credentials required for accessing the necessary resources.</param>
        /// <returns>A tuple containing the project key and the bundle key that match the selection criteria.</returns>
        public interface IBundleSelector
        {
            (string projKey, string bundleKey) Select(eSTATES[] prmState, OdbcConnection prmCon, Credentials prmCrd);
        }

        /// <summary>
        /// Interface for Metadata Entry Form
        /// B'Zer core uses this interface to invoke the entry form
        /// </summary>
        public interface IEntryFormBuilder
        {
            Form CreateEntryForm(OdbcConnection pCon, string projkey, string batchkey, DataLayerDefs.Mode mode, string casefilename, NovaNet.Utils.Credentials prmCrd);
        }

        public interface ISummaryForm
        {
            (string projKey, string bundleKey) GetBundleControl();
        }
        /// <summary>
        /// Interface for Summary/List form of metadata/records
        /// B'Zer core uses this interface to invoke the Summary/List Form
        /// </summary>
        public interface ISummaryFormBuilder : ISummaryForm
        {
            Form CreateSummaryForm(OdbcConnection pCon, Credentials pcrd, eSTATES[] prmState);

            DataSet GetSummaryUploadData(OdbcConnection pCon, string projKey, string bundleKey);
        }

        public interface IMasterUpdateFormBuilder
        {
            Form CreateMasterUpdateForm(OdbcConnection pCon, Credentials pcrd);
        }
    }
}
