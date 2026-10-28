using System;
using System.Data.Odbc;
using System.Windows.Forms;
using NovaNet.Utils;

namespace bzer_data_entry
{
    public class MasterUpdateFormBuilder : IMasterUpdateFormBuilder
    {
        public Form CreateMasterUpdateForm(OdbcConnection pCon, Credentials pcrd)
        {
            return new MasterDataUpdateForm(pCon, pcrd);
        }
    }
}
