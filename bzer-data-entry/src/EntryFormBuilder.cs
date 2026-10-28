using System.Data.Odbc;
using System.Windows.Forms;
using NovaNet.Utils;
namespace bzer_data_entry
{
    public class EntryFormBuilder : IEntryFormBuilder
    {
        public Form CreateEntryForm(OdbcConnection pCon, string projkey, string batchkey, DataLayerDefs.Mode mode, string casefilename, NovaNet.Utils.Credentials prmCrd)
        {
            return new EntryForm(pCon, projkey, batchkey, mode, casefilename, prmCrd);
        }
    }
}
