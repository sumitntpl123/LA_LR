using iTextSharp.text.xml.xmp;
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHeaven.Logic
{
    internal class frmExportLogic
    {
        OdbcConnection sqlCon;
        MemoryStream stateLog;
        byte[] tmpWrite;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        private OdbcDataAdapter sqlAdap = null;
        public string err = null;
        private Dictionary<string, string> customExceptions = null;

        public frmExportLogic(OdbcConnection prmcon)
        {
            sqlCon = prmcon;
        }

        public DataSet GetCustomExceptions()
        {
            var getCustomExceptionsStmt = "SELECT ExcepTion_Code as excode, ExcepTion_Name as exname from tblexception";
            var dsCustomEx = new DataSet();
            var sqlAdap = new OdbcDataAdapter(getCustomExceptionsStmt, sqlCon);
            sqlAdap.Fill(dsCustomEx);
            return dsCustomEx;
        }

        public DataTable PopulateProject()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select proj_key, proj_code from project_master  ";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);
            return dt;
        }

        public DataTable PopulateBatchReexport(string project)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.status = '8'  and a.proj_code = '" + project + "'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);
            return dt;
        }

        public DataTable PopulateBatch(string project)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = "select a.bundle_key, a.bundle_code from bundle_master a, project_master b where a.proj_code = b.proj_key and a.status = '7'  and a.proj_code = '" + project + "'";

            OdbcDataAdapter odap = new OdbcDataAdapter(sql, sqlCon);
            odap.Fill(dt);
            return dt;
        }

        public DataSet GetAllBatchRun(string proj_key, string batch_key)
        {
            string sqlStr = null;

            sqlStr = "select a.filename,a.proj_code,a.bundle_key from case_file_master a, bundle_master b where a.proj_code = b.proj_code and a.bundle_key = b.bundle_key and a.proj_code = '" + proj_key + "' and a.bundle_key = '" + batch_key + "' and (b.status = '7' or b.status = '8') order by a.item_no";

            OdbcDataAdapter odap = new OdbcDataAdapter(sqlStr, sqlCon);

            DataSet projDs = new DataSet();
            try
            {
                odap = new OdbcDataAdapter(sqlStr, sqlCon);
                odap.Fill(projDs);
            }
            catch (Exception ex)
            {
                odap.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return projDs;
        }

        public DataSet GetAllExportedImageAll(string proj_key, string batch_key, string filename)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            sqlStr = "select page_index_name,status,page_name,doc_type from image_master " +
                    " where proj_key ='" + proj_key + "'" +
                " and batch_key ='" + batch_key + "' " +
                " and status <> 29 and policy_number ='" + filename + "' order by serial_no";
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

        public DataSet GetAllExportedImage(string proj_key, string batch_key, string filename)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select page_index_name,status,page_name,doc_type from image_master " +
                    " where proj_key ='" + proj_key + "'" +
                " and batch_key ='" + batch_key + "' " +
                " and status <> 29 and policy_number ='" + filename + "' group by doc_type order by serial_no";
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

        public DataSet GetAllExportedImageWithDoc(string proj_key, string batch_key, string filename, string doc)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            sqlStr = "select page_index_name,status,page_name,doc_type from image_master " +
                    " where proj_key ='" + proj_key + "'" +
                " and batch_key ='" + batch_key + "' " +
                " and status <> 29 and policy_number ='" + filename + "' and doc_type = '" + doc + "' order by serial_no";
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

        public DataSet GetExportableDeed(string project, string batch, string file)
        {
            string sqlStr = null;
            DataSet dsBox = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select Distinct(a.filename) as File_Name,a.proj_code,a.bundle_key,a.status,b.status as 'Export Status' from case_file_master a, metadata_entry b where a.proj_code=b.proj_code and a.bundle_key = b.bundle_key and a.proj_code=" + project + " and a.bundle_key=" + batch + " and a.filename = b.filename and a.filename = '" + file + "' ";

            sqlStr = sqlStr + " order by a.item_no";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsBox);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                stateLog = new System.IO.MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return dsBox;
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
                err = ex.Message;
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return projDs;
        }

        public DataSet GetStatus(string prmProjKey, string prmBatchKey)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();
            try
            {
                sqlStr = @"select status from bundle_master where proj_code=" + prmProjKey + " and bundle_key=" + prmBatchKey;
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(projDs);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                err = ex.Message;
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return projDs;
        }

        public bool UpdateMetaStatus(string ProjectCode, string BatchKey, string FileNumber)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = "update metadata_entry set status='Exported' where proj_code= '" + ProjectCode + "' and bundle_key= '" + BatchKey + "' and filename='" + FileNumber + "'";

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return commitBol;
        }

        public DataSet GetImageCount(eSTATES state, string ProjectKey, string BatchKey, string PolicyNumber)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;


            sqlStr = "select count(*) from image_master " +
                    " where proj_key=" + ProjectKey +
                " and batch_key=" + BatchKey + " and box_number='" + 1 + "'" +
                " and policy_number='" + PolicyNumber + "' and status=" + (int)state + " and status<>" + (int)eSTATES.PAGE_DELETED;
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

        public bool UpdateBatchStatus(string Proj_Key, string Batch_Key)
        {
            string sqlStr = null;
            OdbcTransaction sqlTrans = null;
            bool commitBol = true;

            OdbcCommand sqlCmd = new OdbcCommand();

            sqlStr = @"update bundle_master" +
                " set status=" + (int)eSTATES.BATCH_EXPORTED + " where " +
                " bundle_key=" + Batch_Key + " and proj_code = '" + Proj_Key + "'";

            try
            {

                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Connection = sqlCon;
                sqlCmd.Transaction = sqlTrans;
                sqlCmd.CommandText = sqlStr;
                int i = sqlCmd.ExecuteNonQuery();
                sqlTrans.Commit();
                commitBol = true;
            }
            catch (Exception ex)
            {
                commitBol = false;
                sqlTrans.Rollback();
                sqlCmd.Dispose();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(sqlStr + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex);
            }
            return commitBol;
        }

        public DataSet GetTotalImageCount(string proj, string batch, string file)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();

            try
            {
                sqlStr = @"select count(*) from image_master where proj_key=" + proj + " and batch_key=" + batch + " and policy_number ='" + file + "' and status <> 29";
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


        public DataSet GetCNR(string proj, string batch, string file)
        {
            string sqlStr = null;
            DataSet projDs = new DataSet();
            try
            {
                sqlStr = @"select cnrno from metadata_entry where proj_code=" + proj + " and bundle_key=" + batch + " and filename ='" + file + "'";
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

        public DataSet GetAllDeedEX(string proj_key, string batch_key, string batch)
        {
            DataSet dsImage = new DataSet();

            string sqlStr = "select c.establishment as 'Establishment',c.bundle_name as 'BundleNumber',c.handover_date as 'HandoverDate',a.case_status as 'CaseStatus',a.case_nature as 'CaseNature',a.case_type as 'CaseType',a.case_file_no as 'CaseNumber',a.filename as 'FileName',a.case_year as 'CaseYear',REPLACE(REPLACE(a.judge_name, '||', '|| '),';','; ') as 'JudgeName',a.disposal_date as 'DateofDisposal',REPLACE(TRIM(a.district),';','') as 'District',REPLACE(REPLACE(a.petitioner_name, '||', '|| '),'||','|| ') as 'PetitionerName'," +
                     "REPLACE(REPLACE(a.respondant_name, '||', '|| '),'||','|| ') as 'RespondantName',REPLACE(REPLACE(a.petitioner_counsel_name, '||', '|| '),'||','|| ') as 'PetitionerCounselName',REPLACE(REPLACE(a.respondant_counsel_name, '||', '|| '),'||','|| ') as 'RespondantCounselName'," +
                     "a.case_filling_date as 'DateOfFilling',a.ps_name as 'PoliceStation',a.ps_case_no as 'PoliceStationCaseNo',REPLACE(REPLACE(a.lc_case_no, '||', '|| '),'||','|| ') as 'LowerCourtCaseNo',a.lc_order_date as 'LowerCourtOrderDate'," +
                     "REPLACE(REPLACE(a.lc_judge_name, '||', '|| '),'||','|| ') as 'LowerCourtJudge',REPLACE(REPLACE(a.conn_app_case_no, '||', '|| '),'||','|| ') as 'ConnectedApplication'," +
                     "a.conn_disposal_type as 'ConnectedApplicationDisposalType',REPLACE(REPLACE(a.conn_main_case_no, '||', '|| '),'||','|| ') as 'ConnectedMainCase'," +
                     "REPLACE(REPLACE(a.analogous_case_no, '||', '|| '),'||','|| ') as 'AnalogousCaseNumber'," +
                     "a.old_case_type as 'OldCaseType',a.old_case_no as 'OldCaseNumber',a.old_case_year as 'OldCaseYear',a.file_move_history as 'FileMovementHistory',TRIM(a.dept_remark) as 'DepartmentalNotes'," +
                     "REPLACE(REPLACE(b.entry_exception, '||', '||'),'||','||') as 'entryEx'," +
                     "REPLACE(REPLACE(b.image_exception, '||', '||'),'||','||') as 'imageEx'" +
                     "from metadata_entry a,bundle_master c, case_file_master b where c.proj_code = a.proj_code and c.bundle_key = a.bundle_key and a.proj_code = '" + proj_key + "' and a.bundle_key = '" + batch_key + "' and b.filename=a.filename ";
            try
            {
                OdbcDataAdapter sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);

                dsImage.Tables[0].Columns.Add("Pages");
                for (int i = 0; i < dsImage.Tables[0].Rows.Count; i++)
                {
                    string filename = dsImage.Tables[0].Rows[i][7].ToString();
                    string imgCount = string.Empty;
                    imgCount = GetTotalImageCount(proj_key, batch_key, filename).ToString();
                    dsImage.Tables[0].Rows[i]["Pages"] = imgCount;
                }
                dsImage.Tables[0].Columns.Add("cnrno");
                for (int i = 0; i < dsImage.Tables[0].Rows.Count; i++)
                {
                    string filename = dsImage.Tables[0].Rows[i][7].ToString();
                    string cnr = string.Empty;
                    cnr = GetCNR(proj_key, batch_key, filename).ToString();
                    dsImage.Tables[0].Rows[i]["cnrno"] = cnr;
                }
                dsImage.Tables[0].Columns.Add("ImagePath");
                for (int i = 0; i < dsImage.Tables[0].Rows.Count; i++)
                {
                    string casefile = dsImage.Tables[0].Rows[i][7].ToString();
                    string path = "C:" + "\\Nevaeh\\" + batch + "\\" + casefile;
                    string[] array1 = Directory.GetFiles(path);
                    string pdfnames = "";
                    for (int j = 0; j < array1.Length; j++)
                    {
                        if (j == array1.Length - 1)
                        {
                            pdfnames = pdfnames + array1[j].Remove(0, 2).Insert(0, "/data").Replace("\\", "/").ToString();
                        }
                        else
                        {
                            pdfnames = pdfnames + array1[j].Remove(0, 2).Insert(0, "/data").Replace("\\", "/") + "||";
                        }

                    }
                    dsImage.Tables[0].Rows[i]["ImagePath"] = pdfnames;

                    var exceptionNotes = new List<string>();
                    if (dsImage.Tables[0].Rows[i]["entryEx"].ToString() != null)
                    {
                        string[] split = dsImage.Tables[0].Rows[i]["entryEx"].ToString().Split(new string[] { "||" }, StringSplitOptions.None);

                        exceptionNotes.AddRange(split
                            .Where(exCode => !string.IsNullOrWhiteSpace(exCode))
                            .Select(exCode =>
                                customExceptions.ContainsKey(exCode) ?
                                customExceptions[exCode] : "Unknown exception code " + exCode
                            )
                        );
                    }


                    if (dsImage.Tables[0].Rows[i]["imageEx"].ToString() != null)
                    {
                        string[] split = dsImage.Tables[0].Rows[i]["imageEx"].ToString().Split(new string[] { "||" }, StringSplitOptions.None);

                        exceptionNotes.AddRange(split
                            .Where(exCode => !string.IsNullOrWhiteSpace(exCode))
                            .Select(exCode =>
                                customExceptions.ContainsKey(exCode) ?
                                customExceptions[exCode] : "Unknown exception code " + exCode
                            )
                        );
                    }

                    dsImage.Tables[0].Rows[i]["DepartmentalNotes"] = string.Join("|",
                        new List<string>() { dsImage.Tables[0].Rows[i]["DepartmentalNotes"].ToString() }
                        .Concat(exceptionNotes)
                        .Where(str => !string.IsNullOrWhiteSpace(str))
                    );

                }
                dsImage.Tables[0].Columns["FileName"].SetOrdinal(dsImage.Tables[0].Columns["cnrno"].Ordinal - 1);
                dsImage.Tables[0].Columns.Remove("entryEx");
                dsImage.Tables[0].Columns.Remove("imageEx");
                dsImage.Tables[0].Columns.RemoveAt(2);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
            }
            return dsImage;
        }
    }
}
