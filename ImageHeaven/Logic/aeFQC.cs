using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Windows.Forms;
using static ImageHeaven.aeFQC;

namespace ImageHeaven.Logic
{
    internal class aeFQCLogic
    {
        OdbcConnection sqlCon;
        OdbcDataAdapter sqlAdap;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        MemoryStream stateLog;
        byte[] tmpWrite;
        OdbcTransaction SqlTrans;
        public aeFQCLogic(OdbcConnection prmcon, OdbcTransaction sqlTrans = null)
        {
            sqlCon = prmcon;
            SqlTrans = sqlTrans;
        }

        public DataTable IsTransactionExists(string pr, string bu, string pol)
        {
            DataTable dt = new DataTable();
            string sql = "select * from transaction_log where proj_key = '" + pr + "' and batch_key = '" + bu + "' and box_number = '1' and policy_number = '" + pol + "' ";
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(dt);
            return dt;
        }

        public int GetFilenameSerial(int projectKey, int batchKey, string filename)
        {
            int serial = 0;
            OdbcCommand getFilenameSerialCmd = null;
            try
            {
                #region GetFilenameSerial
                getFilenameSerialCmd = new OdbcCommand($@"
                SELECT running_serial
                FROM
                metadata_entry
                WHERE
                proj_code=? AND
                bundle_key=? AND
                filename=?
                ", sqlCon);
                #endregion

                getFilenameSerialCmd.Parameters.Add("proj_code", OdbcType.Int).Value = projectKey;
                getFilenameSerialCmd.Parameters.Add("bundle_key", OdbcType.Int).Value = batchKey;
                getFilenameSerialCmd.Parameters.Add("filename", OdbcType.VarChar).Value = filename;

                serial = Convert.ToInt32(getFilenameSerialCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot fetch serial no. " + ex.Message, "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return serial > 0 ? serial : default;
        }

        public string GetFilenameFromSerial(int projectKey, int batchKey, int serial)
        {
            string filename = string.Empty;
            OdbcCommand getFilenameFromSerialCmd = null;
            try
            {
                #region GetFilenameSerial
                getFilenameFromSerialCmd = new OdbcCommand($@"
                SELECT filename
                FROM
                metadata_entry
                WHERE
                proj_code=? AND
                bundle_key=? AND
                running_serial=?
                ", sqlCon);
                #endregion

                getFilenameFromSerialCmd.Parameters.Add("proj_code", OdbcType.Int).Value = projectKey;
                getFilenameFromSerialCmd.Parameters.Add("bundle_key", OdbcType.Int).Value = batchKey;
                getFilenameFromSerialCmd.Parameters.Add("running_serial", OdbcType.Int).Value = serial;

                filename = getFilenameFromSerialCmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot fetch filename no. " + ex.Message, "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return filename != string.Empty ? filename : default;
        }

        public bool UpdateTransactionLog(eSTATES state, Credentials prmCrd, string filename, CtrlPolicy ctrlPolicy, OdbcTransaction sqlTrans = null)
        {
            string sqlStr = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            bool flagTrans = false;
            switch (state)
            {
                case eSTATES.POLICY_SCANNED:
                    {
                        sqlStr = @"insert into transaction_log (proj_key,Batch_Key,Box_number,Policy_number,Scanned_user,scanned_dttm,fqc_user)" +
                        " values(" + ctrlPolicy.ProjectKey + "," + ctrlPolicy.BatchKey + ",'" + ctrlPolicy.BoxNumber + "','" + filename + "','" + prmCrd.created_by + "','" + prmCrd.created_dttm + "','')";
                        break;
                    }
                case eSTATES.POLICY_QC:
                    {
                        sqlStr = @"update transaction_log" +
                        " set QC_User='" + prmCrd.created_by + "',Qc_DTTM='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                        " and batch_key=" + ctrlPolicy.BatchKey + " and box_number='" + ctrlPolicy.BoxNumber + "'" +
                        " and policy_number='" + ctrlPolicy.PolicyNumber + "'";
                        break;
                    }
                case eSTATES.POLICY_INDEXED:
                    {
                        sqlStr = @"update transaction_log" +
                        " set Index_User='" + prmCrd.created_by + "',Index_DTTM='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                        " and batch_key=" + ctrlPolicy.BatchKey + " and box_number='" + ctrlPolicy.BoxNumber + "'" +
                        " and policy_number='" + ctrlPolicy.PolicyNumber + "'";
                        break;
                    }
                case eSTATES.POLICY_FQC:
                    {
                        sqlStr = @"update transaction_log" +
                        " set Fqc_User='" + prmCrd.created_by + "',fqc_DTTM='" + prmCrd.created_dttm + "' where proj_key=" + ctrlPolicy.ProjectKey +
                        " and batch_key=" + ctrlPolicy.BatchKey + " and box_number='" + ctrlPolicy.BoxNumber + "'" +
                        " and policy_number='" + ctrlPolicy.PolicyNumber + "'";
                        break;
                    }
            }
            try
            {
                if (sqlTrans == null)
                {
                    sqlTrans = sqlCon.BeginTransaction();
                    flagTrans = true;
                }
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                if (flagTrans)
                {
                    sqlTrans.Commit();
                }
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n" + "Wfe State--" + Convert.ToString(Convert.ToInt32(state)) + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return commitBol;
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

        public DataSet GetFileCount(string projkey, string bundleKey)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select filename from case_file_master " +
                    " where proj_code=" + projkey +
                " and bundle_key=" + bundleKey + " and (status = '3' or status = '2' or status = '4')";


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
            return dsImage;
        }

        public DataSet GetPolicyPhotoStatus(CtrlPolicy ctrlPolicy)
        {
            string sqlStr = null;

            DataSet policyDs = new DataSet();

            try
            {
                sqlStr = "select photo from case_file_master where proj_code=" + ctrlPolicy.ProjectKey + " and bundle_key=" + ctrlPolicy.BatchKey + "  and filename='" + ctrlPolicy.PolicyNumber + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return policyDs;
        }

        public DataTable GetFileDetails(string projKey, string bundleKey, string file_no)
        {
            DataTable dt = new DataTable();

            string sql = "select case_file_no, case_status,case_nature, case_type,case_year from case_file_master where proj_code = '" + projKey + "' and bundle_key = '" + bundleKey + "' and filename = '" + file_no + "'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);
            return dt;
        }

        public DataSet GetPolicyStatus(CtrlPolicy ctrlPolicy)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select status from case_file_master " +
                    " where proj_code=" + ctrlPolicy.ProjectKey +
                " and bundle_key=" + ctrlPolicy.BatchKey + " and filename ='" + ctrlPolicy.PolicyNumber + "'";

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
            return dsImage;
        }

        public DataSet GetImagesItems(eITEMS item, string case_file_no, string projKey, string bundleKey)
        {
            OdbcDataAdapter wAdap;
            OdbcTransaction trns = null;
            OdbcCommand oCom = new OdbcCommand();
            string strQuery = null;
            DataSet ds = new DataSet();
            strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A,case_file_master B where A.proj_key=B.proj_code and A.batch_key = B.bundle_key  and A.policy_number = B.filename and A.photo <> 1 and A.proj_key=" + projKey + " and A.batch_key=" + bundleKey + " and  A.policy_number='" + case_file_no + "' and a.status <> 29 and (b.status = 4 or b.status = 5 or b.status = 6 or b.status ='7' or b.status = '8' or b.status = '30' or b.status = '31' or b.status = '37' or b.status = '40' or b.status = '77') order by a.serial_no";

            oCom.Connection = sqlCon;
            oCom.CommandText = strQuery;
            wAdap = new OdbcDataAdapter(oCom);
            wAdap.Fill(ds);
            return ds;
        }

        public bool DeleteMeta(string fileName, string projKey, string bundleKey)
        {
            string sqlStr = null;

            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;


            sqlStr = "DELETE from metadata_entry WHERE proj_code = '" + projKey + "' AND bundle_key = '" + bundleKey + "' and filename = '" + fileName + "' ";
            System.Diagnostics.Debug.Print(sqlStr);
            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon, SqlTrans);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }
            return retVal;
        }

        public bool DeleteImage(string fileName, string projKey, string bundleKey)
        {
            string sqlStr = null;

            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;


            sqlStr = "DELETE from image_master WHERE proj_key = '" + projKey + "' AND batch_key = '" + bundleKey + "' and policy_number = '" + fileName + "' ";
            System.Diagnostics.Debug.Print(sqlStr);
            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon, SqlTrans);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }
            return retVal;
        }

        public bool DeleteTrans(string fileName, string projKey, string bundleKey)
        {
            string sqlStr = null;

            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;


            sqlStr = "DELETE from transaction_log WHERE proj_key = '" + projKey + "' AND batch_key = '" + bundleKey + "' and policy_number = '" + fileName + "' ";
            System.Diagnostics.Debug.Print(sqlStr);
            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon, SqlTrans);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }
            return retVal;
        }

        public bool DeleteCusEx(string fileName, string projKey, string bundleKey)
        {
            string sqlStr = null;

            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;


            sqlStr = "DELETE from custom_exception WHERE proj_key = '" + projKey + "' AND batch_key = '" + bundleKey + "' and policy_number = '" + fileName + "' ";
            System.Diagnostics.Debug.Print(sqlStr);
            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon, SqlTrans);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }
            return retVal;
        }

        public bool DeleteQa(string fileName, string projKey, string bundleKey)
        {
            string sqlStr = null;

            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;


            sqlStr = "DELETE from lic_qa_log WHERE proj_key = '" + projKey + "' AND batch_key = '" + bundleKey + "' and policy_number = '" + fileName + "' ";

            System.Diagnostics.Debug.Print(sqlStr);
            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon, SqlTrans);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }
            return retVal;
        }
        public bool DeleteCaseFile(string fileName, string projKey, string bundleKey)
        {
            string sqlStr = null;

            OdbcCommand sqlCmd = new OdbcCommand();

            bool retVal = false;
            string sql = string.Empty;


            sqlStr = "DELETE from case_file_master WHERE proj_code = '" + projKey + "' AND bundle_key = '" + bundleKey + "' and filename = '" + fileName + "' ";
            System.Diagnostics.Debug.Print(sqlStr);
            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon, SqlTrans);
            if (cmd.ExecuteNonQuery() > 0)
            {
                retVal = true;
            }
            return retVal;
        }

        public DataSet GetBundleName(int prmProjectKey, int prmBundleKey)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();

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

        public DataSet GetProjectName(int prmProjectKey)
        {
            string sqlStr = null;

            DataSet projDs = new DataSet();

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
        public DataTable GetFileNamesByImageCount(string projKey, string batchKey, string imgCount, imgCountConStatus comparisonOperator)
        {
            try
            {
                var _operator = imgCountConStatus.lessThan == comparisonOperator ? "<" : ">=";
                var query = $@"
                     SELECT 
                        cfm.filename AS policy_number
                    FROM 
                        case_file_master cfm
                    LEFT JOIN 
                        image_master im ON im.policy_number = cfm.filename 
                            AND im.proj_key = {projKey} 
                            AND im.batch_key = {batchKey}
                            AND im.status <> '29'
                    WHERE 
                        cfm.proj_code = {projKey} 
                        AND cfm.bundle_key = {batchKey}
                    GROUP BY 
                        cfm.filename
                    HAVING 
                        COUNT(im.policy_number) {_operator} {imgCount};
                      
                ";
                var cmd = new OdbcCommand(query, sqlCon);
                var sqlAdap = new OdbcDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlAdap.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
