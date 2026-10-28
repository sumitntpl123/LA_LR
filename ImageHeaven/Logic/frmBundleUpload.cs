using System;
using System.Data;
using System.Data.Odbc;
using bzer_data_entry;
using NovaNet.Utils;

namespace ImageHeaven.Logic
{
    internal class frmBundleUploadLogic
    {
        private OdbcConnection sqlCon = null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);

        public frmBundleUploadLogic(OdbcConnection prmCon)
        {
            sqlCon = prmCon;
        }
        public DataTable PopulateProject()
        {

            DataTable dt = new DataTable();

            string sql = "select proj_key, proj_code from project_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;
        }
        public DataTable PopulateBundle(string cmdProjValue)
        {
            DataTable dt = new DataTable();

            string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.proj_code = '" + cmdProjValue + "' and a.status = '0'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;
        }
        public DataSet ReadDatabase(string cmdProjValue, string cmdBundleValue, DataSet dsdeed)
        {
            ISummaryFormBuilder summaryFormBuilder = new SummaryFormBuilder();
            var ds = summaryFormBuilder.GetSummaryUploadData(sqlCon, cmdProjValue, cmdBundleValue);

            return ds;
        }
        public DataSet GetBundleName(int prmProjectKey, int prmBundleKey)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter();
            try
            {
                sqlStr = "select bundle_code,bundle_no from bundle_master where proj_code= '" + prmProjectKey + "' and bundle_key = '" + prmBundleKey + "' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }

            return projDs;
        }
        public bool InsertIntoDB(string pk, string bk, string bn, Credentials crd)
        {
            bool commitBol = true;

            string sqlStr = string.Empty;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"insert into barcode_log(proj_key,bundle_key,bundle_no,created_dttm,created_by) values('" + pk + "','" + bk + "','" + bn + "','" + crd.created_dttm + "','" + crd.created_by + "')";

            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandText = sqlStr;
            int j = sqlCmd.ExecuteNonQuery();
            if (j > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }

            return commitBol;
        }
        public DataTable GetFileDetails(string proj, string bundle, string fileName)
        {
            string sql = "select b.case_type,b.case_file_no,b.case_year,a.establishment from bundle_master a, case_file_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.proj_code = '" + proj + "' and b.bundle_key = '" + bundle + "' and b.filename = '" + fileName + "'";
            DataTable dt = new DataTable();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }
        public DataTable PendingBundle()
        {
            DataTable dt = new DataTable();
            string sql = "select distinct bundle_key, bundle_code from bundle_master where status <> '0' and bundle_key NOT IN (select distinct bundle_key from barcode_log)";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;
        }
        public DataTable _GetGenCount(string proj, string bundle)
        {
            string sql = "select * from barcode_log  where proj_key = '" + proj + "' and bundle_key = '" + bundle + "' ";
            DataTable dt = new DataTable();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

    }
}