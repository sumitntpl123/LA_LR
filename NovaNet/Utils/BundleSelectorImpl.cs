using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;


namespace NovaNet
{
    namespace Utils
    {
        public class BundleSelectorImpl : IBundleSelector
        {
            // Implementation of the selection logic based on prmState, prmCon, and prmCrd.
            public (string projKey, string bundleKey) Select(eSTATES[] prmState, OdbcConnection prmCon, Credentials prmCrd)
            {
                frmBundleSelect bundleSelect = new frmBundleSelect(prmState, prmCon, prmCrd);
                bundleSelect.ShowDialog();
                return (projKey: frmBundleSelect.projKey, bundleKey: frmBundleSelect.bundleKey);
            }
        }
    }
}
