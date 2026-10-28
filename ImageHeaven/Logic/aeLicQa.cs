using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Data;
using System.Data.Odbc;
using System.Linq;

namespace ImageHeaven.Logic
{
    internal class aeLicQaLogic
    {
        OdbcConnection sqlCon;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        private OdbcDataAdapter sqlAdap = null;
        private Credentials crd = new Credentials();
        public aeLicQaLogic(OdbcConnection prmcon)
        {
            sqlCon = prmcon;
        }
        public DataSet PolicyWithLICException(int prmProjKey, int prmBatchKey)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();

            try
            {
                sqlStr = @"select filename from case_file_master where proj_code=" + prmProjKey + " and bundle_key=" + prmBatchKey + " and status=" + (int)eSTATES.POLICY_EXCEPTION;
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

        public DataTable GetAllFiles(string projCode, string batchCode)
        {
            string sqlStr = null;
            DataTable dsImage = new DataTable();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select filename from metadata_entry " +
                    " where proj_code=" + projCode +
                " and bundle_key=" + batchCode + " ";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return dsImage;
        }

        public DataSet GetAllException(string policy, string project, string batch)
        {
            string sqlStr = null;

            DataSet expDs = new DataSet();

            try
            {
                sqlStr = "select missing_img_exp,crop_clean_exp,poor_scan_exp,wrong_indexing_exp,linked_policy_exp,decision_misd_exp,extra_page_exp,rearrange_exp,other_exp,move_to_respective_policy_exp,comments from lic_qa_log where proj_key=" + project + " and batch_key=" + batch + " and policy_number='" + policy + "' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(expDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                exMailLog.Log(ex);
            }

            return expDs;
        }

        public bool UpdateQaExceptionStatus(int prmStatus, int prmExpStatus, string policy, string project, string batch, Credentials crd)
        {
            string sqlStr = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();
            OdbcTransaction prmTrans;
            try
            {
                prmTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = prmTrans;

                sqlStr = @"update lic_qa_log" +
                " set solved=" + prmStatus + " where proj_key=" + project +
                " and batch_key=" + batch + " and box_number='" + 1 + "'" +
                " and policy_number='" + policy + "' and solved <>" + 7;


                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();

                sqlStr = @"update lic_qa_log" +
                " set qa_status=" + prmExpStatus + ",created_by = '" + crd.created_by + "',created_dttm = '" + crd.created_dttm + "' where proj_key=" + project +
                " and batch_key=" + batch + " and box_number='" + 1 + "'" +
                " and policy_number='" + policy + "'";


                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();

                prmTrans.Commit();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlCmd.Dispose();
                exMailLog.Log(ex);
            }
            return commitBol;
        }

        public DataSet GetPolicyStatus(string projCode, string batchCode, string policyNumber)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select status from case_file_master " +
                    " where proj_code=" + projCode +
                " and bundle_key=" + batchCode + " and filename='" + policyNumber + "'";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return dsImage;
        }
        public DataSet GetAllValues(int prmProjectKey)
        {
            string sqlStr = null;

            DataSet batchDs = new DataSet();

            try
            {
                sqlStr = "select bundle_key,bundle_name from bundle_master where proj_code=" + prmProjectKey + " and status = '6' order by bundle_name";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(batchDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return batchDs;
        }

        public DataSet GetAllBox(int prmBatchKey, string projCode)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select distinct count(filename) as files from case_file_master where proj_code=" + projCode + " and bundle_key=" + prmBatchKey + " ";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return dsBox;
        }

        public DataSet GetReadyImageCount(eSTATES[] state, eSTATES[] prmPolicyState, string projCode, string batchCode)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;


            sqlStr = "select count(page_name) as page_Count,sum(qc_size) as index_size from image_master A,case_file_master B" +
                    " where A.proj_key = B.proj_code and A.batch_key = B.bundle_key and A.policy_number = B.filename and B.proj_code=" + projCode +
                " and B.bundle_key=" + batchCode + " and a.box_number='1' and A.status<>29";

            for (int j = 0; j < state.Length; j++)
            {
                if ((int)state[j] != 0)
                {
                    if (j == 0)
                    {
                        sqlStr = sqlStr + " and (b.status = 4 or b.status = 40 or B.status=" + (int)prmPolicyState[j];
                    }
                    else
                        sqlStr = sqlStr + " or B.status=" + (int)prmPolicyState[j];
                }
            }
            sqlStr = sqlStr + " )";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return dsImage;
        }

        public DataSet GetPolicyCount(eSTATES[] state, string projCode, string batchCode)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select count(*) from case_file_master " +
                    " where proj_code=" + projCode +
                " and bundle_key=" + batchCode + " and (status = 4 or status = 5 or status = 19 or status = 22 or status = 30 or status = 31 ) ";


            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return dsImage;
        }

        public DataTable BundDetails(int prmProjectKey, int prmBatch)
        {
            string sqlStr = null;

            DataTable batchDs = new DataTable();

            try
            {
                sqlStr = "select establishment,date_format(handover_date,'%d-%m-%Y') from bundle_master where proj_code=" + prmProjectKey + " and bundle_key = '" + prmBatch + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(batchDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return batchDs;
        }

        public DataTable MainMetaDetails(int prmProjectKey, int prmBatch, string prmFilename)
        {
            string sqlStr = null;

            DataTable batchDs = new DataTable();

            try
            {
                sqlStr = "SELECT CONCAT('',a.district,' ', '\r'), REPLACE(REPLACE(a.petitioner_name, '||', ' , \r'), '||', '\n'),REPLACE(REPLACE(a.respondant_name, '||', ' , \r'), '||', '\n') from metadata_entry a, bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.proj_code ='" + prmProjectKey + "' and b.bundle_key ='" + prmBatch + "' and a.filename = '" + prmFilename + "' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(batchDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return batchDs;
        }

        public System.Data.DataTable CounselMetaDetails(int prmProjectKey, int prmBatch, string prmFilename)
        {
            string sqlStr = null;

            DataTable batchDs = new DataTable();

            try
            {
                sqlStr = "select REPLACE(REPLACE(a.judge_name, '||', ' \t\r'),'||','\n'),REPLACE(REPLACE(a.lc_judge_name, '||', ' \t\r'),'||','\n'),REPLACE(REPLACE(a.petitioner_counsel_name, '||', ' , \r'),'||','\n') as 'Petitioner Counsel',REPLACE(REPLACE(a.respondant_counsel_name, '||', ' , \r'),'||','\n') as 'Respondant Counsel',date_format(a.disposal_date,'%d-%m-%Y'),date_format(a.case_filling_date,'%d-%m-%Y'),a.ps_name,a.ps_case_no,REPLACE(REPLACE(a.lc_case_no, '||', ' \t\r'),'||','\n') as 'LC Case No',date_format(a.lc_order_date,'%d-%m-%Y'),REPLACE(REPLACE(a.conn_app_case_no, '||', ' \t\r'),'||','\n') as 'Conn App Case No',a.conn_disposal_type,REPLACE(REPLACE(a.conn_main_case_no, '||', ' \t\r'),'||','\n') as 'Conn Main Case No',REPLACE(REPLACE(a.analogous_case_no, '||', ' \t\r'),'||','\n') as 'Analogous Case No',a.old_case_type,a.old_case_no,a.old_case_year,a.file_move_history,a.dept_remark,a.cnrno from metadata_entry a, bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and b.proj_code ='" + prmProjectKey + "' and b.bundle_key ='" + prmBatch + "' and a.filename = '" + prmFilename + "' ";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(batchDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return batchDs;
        }

        public DataSet GetTotalPolicies(eSTATES prmState, string projCode, string batchCode)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select filename as filename from case_file_master where proj_code=" + projCode + " and bundle_key=" + batchCode;
            if ((int)prmState == 0)
            {
                sqlStr = sqlStr + " and 1=1 order by filename";
            }
            else
            {
                sqlStr = sqlStr + " and status=" + (int)prmState + " order by filename";
            }

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return dsBox;
        }

        public DataSet GetTotalPolicies(eSTATES[] prmState, string projCode, string batchCode)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select filename as filename from case_file_master where proj_code=" + projCode + " and bundle_key=" + batchCode;

            for (int j = 0; j < prmState.Length; j++)
            {
                if ((int)prmState[j] != 0)
                {
                    if (j == 0)
                    {
                        sqlStr = sqlStr + " and (status=" + (int)prmState[j];
                    }
                    else
                        sqlStr = sqlStr + " or status=" + (int)prmState[j];
                }
            }
            sqlStr = sqlStr + ") order by filename";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return dsBox;
        }

        public double GetTotalBatchSize(string projCode, string batchCode)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            double size = 0;

            ///changed in version 1.0.0.1
            sqlStr = "select sum(A.qc_size) as size from image_master A,case_file_master B where A.proj_key=B.proj_code and A.batch_key=B.bundle_key and A.policy_number=B.filename and A.proj_key=" + projCode + " and A.batch_key=" + batchCode + " and B.status<>" + (int)eSTATES.POLICY_ON_HOLD + " and A.status<>" + (int)eSTATES.PAGE_DELETED;
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
                size = Convert.ToInt32(dsBox.Tables[0].Rows[0]["size"]) / 1024;
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return size;
        }

        public DataSet GetTotalImageCount(eSTATES[] state, bool prmIsSignaturePage, eSTATES[] prmPolicyState, string projCode, string batchCode)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select count(page_name) as page_Count,sum(qc_size) as index_size from image_master A,case_file_master B" +
                    " where A.proj_key = B.proj_code and A.batch_key = B.bundle_key and A.policy_number = B.filename and B.proj_code=" + projCode +
                " and B.bundle_key=" + batchCode + " and A.status<>29";

            for (int j = 0; j < prmPolicyState.Length; j++)
            {
                if ((int)prmPolicyState[j] != 0)
                {
                    if (j == 0)
                    {
                        sqlStr = sqlStr + " and (b.status = 4 or b.status = 5 or B.status=" + (int)prmPolicyState[j];
                    }
                    else
                        sqlStr = sqlStr + " or B.status = " + (int)prmPolicyState[j];
                }
            }
            if (prmIsSignaturePage == false)
            {
                sqlStr = sqlStr + " )";
            }
            else
            {
                sqlStr = sqlStr + " ) and A.doc_type<>''";
            }
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return dsBox;
        }

        public DataSet GetBatchStatus(int prmBatchKey)
        {
            string sqlStr = null;
            DataSet batchDs = new DataSet();

            try
            {
                sqlStr = "select status from bundle_master where bundle_key=" + prmBatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(batchDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return batchDs;
        }

        public DataSet GetPolicyList(eSTATES[] prmState, string projCode, string batchCode)
        {
            DataSet policyDs = new DataSet();

            try
            {
                var stateCodeFilterQuery = string.Join(" or ",
                    prmState
                        .Where(stateCode => (int)stateCode != 0)
                        .Select(stateCode => $"cfm.status={(int)stateCode}")
                    );

                stateCodeFilterQuery = prmState.Length == 0 ? string.Empty : $"and ({stateCodeFilterQuery})";


                var sqlStr = $@"select 
                                cfm.case_file_no,
                                cfm.filename,
                                cfm.case_status,
                                cfm.case_nature,
                                cfm.case_type,
                                cfm.case_year,
                                cfm.status 
                            from 
                                case_file_master cfm,
                                metadata_entry me 
                            where 
                                cfm.proj_code={projCode} and 
                                cfm.bundle_key={batchCode} and 
                                (
                                    locked_uid='{crd.created_by}' or 
                                    expires_dttm <= NOW() or 
                                    invalid = 0
                                ) and 
                                cfm.proj_code = me.proj_code and 
                                cfm.bundle_key = me.bundle_key and
                                cfm.filename = me.filename
                                {stateCodeFilterQuery}
                            order by 
                                me.running_serial";

                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(policyDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();

                exMailLog.Log(ex);
            }
            return policyDs;
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

                exMailLog.Log(ex);
            }
            return projDs;
        }
    }

}
