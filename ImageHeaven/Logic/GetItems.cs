using LItems;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Data.Odbc;

namespace ImageHeaven.Logic
{
    public class GetItemsLogic
    {
        OdbcConnection sqlCon;
        public GetItemsLogic(OdbcConnection prmcon)
        {
            sqlCon = prmcon;
        }
        public ArrayList GetItems(eITEMS item, string projKey, string bundleKey, eSTATES[] withStates)
        {
            CtrlBox ctrtBox = new CtrlBox(Convert.ToInt32(projKey), Convert.ToInt32(bundleKey), null);
            wfeBox pBox = new wfeBox(sqlCon, ctrtBox);
            ihwQuery getItems = new ihwQuery(sqlCon);
            return getItems.GetItems(item, withStates, pBox);
        }



    }
}