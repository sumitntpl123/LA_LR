using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Windows.Forms;

namespace ImageHeaven.Logic
{
    public class aeIndexingLogic : BaseLogic
    {
        private Credentials crd = new Credentials();
        private OdbcConnection sqlCon = null;
        private OdbcDataAdapter sqlAdap = null;
        private MemoryStream stateLog;
        byte[] tmpWrite;

        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);

        public aeIndexingLogic(OdbcConnection con, Credentials prmCrd)
        {
            sqlCon = con;
            crd = prmCrd;
        }

        public DataSet GetProjectName(int prmProjectKey)
        {
            DataSet projDs = new DataSet();
            string sqlStr = null;
            try
            {
                sqlStr = "select proj_key,proj_code from project_master where proj_key=" + prmProjectKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return projDs;
        }

        public DataSet GetBundleName(int prmProjectKey, int prmBundleKey)
        {
            DataSet projDs = new DataSet();
            string sqlStr = null;
            try
            {
                sqlStr = "select bundle_code,bundle_no from bundle_master where proj_code= '" + prmProjectKey + "' and bundle_key = '" + prmBundleKey + "' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return projDs;
        }

        public DataSet GetPath(int prmProjKey, int prmBatchKey)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();

            try
            {
                sqlStr = @"select bundle_path from bundle_master where proj_code=" + prmProjKey + " and bundle_key=" + prmBatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }

            return projDs;
        }

        public DataSet GetPolicyPhotoStatus(DataSet ds, int ProjKey, int BatchKey, string PolicyNumber)
        {
            string sqlStr = null;

            try
            {
                sqlStr = "select photo from case_file_master where proj_code=" + ProjKey + " and bundle_key=" + BatchKey + "  and filename='" + PolicyNumber + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(ds);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return ds;
        }

        public bool LockPolicy(Credentials pCrd, OdbcTransaction pTrns, CtrlPolicy ctrlPolicy)
        {
            string sqlStr = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            DataSet ds = new DataSet();
            long time = 0;
            try
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = pTrns;
                sqlStr = "select SYSVALUES from SYSCONFIG  where SYSKEYS='LOCK_EXPIRES_TIME'";
                sqlCmd.CommandText = sqlStr;
                sqlAdap = new OdbcDataAdapter(sqlCmd);
                sqlAdap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    time = Convert.ToInt32(ds.Tables[0].Rows[0]["SYSVALUES"].ToString());
                }

                sqlStr = @"update case_file_master set expires_dttm = Now()+interval " + time + " SECOND,Locked_uid ='" + pCrd.created_by + "',invalid=1 where proj_code= " + ctrlPolicy.ProjectKey + " and bundle_key=" + ctrlPolicy.BatchKey + " and filename='" + ctrlPolicy.PolicyNumber + "'";
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return commitBol;
        }

        public override int GetFileCount(string projkey, string bundleKey)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select filename from case_file_master " +
                    " where proj_code=" + projkey +
                " and bundle_key=" + bundleKey + " and status = '3'";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                MessageBox.Show(ex.Message);
            }
            return dsImage.Tables[0].Rows.Count;
        }

        public int GetFileCountQC(string projkey, string bundleKey)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select filename from case_file_master " +
                    " where proj_code=" + projkey +
                " and bundle_key=" + bundleKey + " and status = '2'";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                MessageBox.Show(ex.Message);
            }
            return dsImage.Tables[0].Rows.Count;
        }

        public DataSet CheckINDEXStatus(string projKey, string bundleKey, string boxNumber, string policyList)
        {
            DataSet ds = new DataSet();
            string sql = "select * from image_master where proj_key = '" + Convert.ToInt32(projKey) + "' and batch_key = '" + Convert.ToInt32(bundleKey) + "' and box_number ='" + boxNumber + "' and policy_number = '" + policyList + "' and status = '25'  ";
            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(ds);
            return ds;
        }

        public int GetPolicyStatus(int projKey, int batchKey, string policyNumber)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select status from case_file_master " +
                    " where proj_code=" + projKey +
                " and bundle_key=" + batchKey + " and filename ='" + policyNumber + "'";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return Convert.ToInt32(dsImage.Tables[0].Rows[0]["status"]);
        }
        public override DataTable GetImagesItems(eITEMS item, eSTATES[] state, string projKey, string bundleKey, string case_file_no)
        {
            OdbcDataAdapter wAdap;
            OdbcTransaction trns = null;
            OdbcCommand oCom = new OdbcCommand();
            string strQuery = null;
            DataSet ds = new DataSet();
            strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A,case_file_master B where A.proj_key=B.proj_code and A.batch_key = B.bundle_key  and A.policy_number = B.filename and A.photo <> 1 and A.proj_key=" + projKey + " and A.batch_key=" + bundleKey + " and  A.policy_number='" + case_file_no + "' and a.status <> 29 and b.status = 3 order by a.serial_no";

            oCom.Connection = sqlCon;
            oCom.CommandText = strQuery;
            wAdap = new OdbcDataAdapter(oCom);
            wAdap.Fill(ds);

            return ds.Tables[0];
        }

        public DataTable GetFileDetails(string projKey, string bundleKey, string file_no)
        {
            DataTable dt = new DataTable();

            string sql = "select case_file_no, case_status,case_nature, case_type,case_year from case_file_master where proj_code = '" + projKey + "' and bundle_key = '" + bundleKey + "' and filename = '" + file_no + "'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);

            return dt;
        }
    }
}
