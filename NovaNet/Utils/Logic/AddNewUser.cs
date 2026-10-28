using System;
using System.Data.Odbc;
using System.Data;

namespace NovaNet.Logic
{
    internal class AddNewUserLogic
    {
        OdbcConnection sqlCon;

        public AddNewUserLogic(OdbcConnection prmcon)
        {
            sqlCon = prmcon;
        }
        public DataSet AddNewUserLoad()
        {
            String sSelectSQL = "select role_id,role_description from ac_role where role_id <> '8'";
            OdbcCommand oCommand = new OdbcCommand(sSelectSQL, sqlCon);
            OdbcDataAdapter objDataAdapter = new OdbcDataAdapter(oCommand);
            DataSet ds = new DataSet();
            objDataAdapter.Fill(ds);
            return ds;
        }


    }
}


