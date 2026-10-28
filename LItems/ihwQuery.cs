/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 19/3/2009
 * Time: 7:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;

namespace LItems
{
    /// <summary>
    ///  Description of ihwQuery.
    /// The usage of OdbcCommand.Prepare() has been removed from this class as part
    /// of testing and investigation into the recurring "Connection has been disabled"
    /// issue. In certain environments and driver versions, prepared statements have
    /// been linked to unstable connection behavior.
    ///
    /// By removing .Prepare(), we aim to simplify command execution and monitor
    /// whether this change improves connection stability and prevents unexpected
    /// disconnections during runtime.
    /// </summary>
    public class ihwQuery : wQuery
    {
        private OdbcConnection sqlCon = null;
        private int photo = -1;
        private Credentials crd;
        private int stage;

        const string TABLE_METADATA = "metadata_record_cs";

        const string TABLE_METADATA_VALUE = "metadata_value_cs";

        const string TABLE_METADATA_MASTER = "metadata_master_cs";

        const string TABLE_BUNDLE_MASTER = "bundle_master";

        const string COLUMN_PROJECT_CODE = "project_code";

        const string COLUMN_MASTER_ID = "master_id";

        const string COLUMN_FILE_ID = "file_id";

        const string COLUMN_BATCH_NAME = "batch_name";

        const string COLUMN_BATCH_CODE = "batch_code";

        const string COLUMN_LEGACY_BATCH_CODE = "bundle_key";

        const string COLUMN_LEGACY_BATCH_NAME = "bundle_name";

        const string COLUMN_BATCH_STATUS = "batch_status";

        const string COLUMN_LEGACY_PROJECT_CODE = "proj_code";

        const string COLUMN_FIELD_NAME = "field_name";

        const string COLUMN_FIELD_LANG = "lang";

        const string COLUMN_FILECOUNT = "file_count";

        const string FIELD_VALUE_DISTRICT = "'district'";

        const string FIELD_VALUE_LANG = "'BnPhonetic'";

        private OdbcCommand FetchBatchesFromProjectCmd;

        private OdbcCommand FetchBatchesForParentCmd;

        public ihwQuery(OdbcConnection prmCon)
        {
            sqlCon = prmCon;
            init();
        }
        public ihwQuery(OdbcConnection prmCon, int prmPhoto)
        {
            sqlCon = prmCon;
            photo = prmPhoto;
            init();
        }
        public ihwQuery(OdbcConnection prmCon, int prmPhoto, int pStage)
        {
            sqlCon = prmCon;
            photo = prmPhoto;
            stage = pStage;
            init();
        }
        public ihwQuery(OdbcConnection prmCon, Credentials pCrd)
        {
            sqlCon = prmCon;
            crd = pCrd;
            init();
        }
        private void init()
        {
            #region FetchBatchesFromProject
            FetchBatchesFromProjectCmd = new OdbcCommand($@"
            SELECT 
                distinct(bm.bundle_name) as {COLUMN_BATCH_NAME}, 
                bm.status as {COLUMN_BATCH_STATUS}
            FROM 
                {TABLE_METADATA} mrc
            INNER JOIN 
                {TABLE_METADATA_VALUE} mvc
            ON 
                mvc.{COLUMN_FILE_ID} = mrc.{COLUMN_FILE_ID}
            INNER JOIN
                {TABLE_BUNDLE_MASTER} bm
            ON
                mrc.{COLUMN_PROJECT_CODE} = bm.{COLUMN_LEGACY_PROJECT_CODE}
            AND 
                mrc.{COLUMN_BATCH_CODE} = bm.{COLUMN_LEGACY_BATCH_CODE}
            WHERE
                bm.{COLUMN_LEGACY_BATCH_NAME} LIKE ?
            ORDER By
                bm.status
            ", sqlCon);
            FetchBatchesFromProjectCmd.Parameters.Add(COLUMN_LEGACY_BATCH_NAME, OdbcType.VarChar);
            //FetchBatchesFromProjectCmd.Prepare();
            #endregion

            #region FetchBatchesForDistrict
            FetchBatchesForParentCmd = new OdbcCommand($@"
            SELECT 
                bm.bundle_name as {COLUMN_BATCH_NAME}, 
                bm.status as {COLUMN_BATCH_STATUS}
            FROM 
                {TABLE_METADATA} mrc
            INNER JOIN 
                {TABLE_METADATA_VALUE} mvc
            ON 
                mrc.{COLUMN_FILE_ID} = mvc.{COLUMN_FILE_ID}
            INNER JOIN 
                {TABLE_METADATA_MASTER} mmc
            ON 
                mvc.{COLUMN_MASTER_ID} = mmc.{COLUMN_MASTER_ID}
            INNER JOIN 
                {TABLE_BUNDLE_MASTER} bm
            ON 
                mrc.{COLUMN_PROJECT_CODE} = bm.{COLUMN_LEGACY_PROJECT_CODE} AND 
                mrc.{COLUMN_BATCH_CODE} = bm.{COLUMN_LEGACY_BATCH_CODE}
            WHERE
                mmc.{COLUMN_FIELD_LANG} = {FIELD_VALUE_LANG} AND 
                mmc.{COLUMN_MASTER_ID} = UNHEX(?)
            GROUP BY
                mrc.{COLUMN_PROJECT_CODE}, mrc.{COLUMN_BATCH_CODE}, mvc.{COLUMN_MASTER_ID}
", sqlCon);
            FetchBatchesForParentCmd.Parameters.Add(COLUMN_MASTER_ID, OdbcType.VarChar);
            //FetchBatchesForParentCmd.Prepare();
            #endregion

        }
        public string GetSysConfigValue(string pKey)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select sysvalues from sysconfig " +
                    " where syskeys='" + pKey + "'";


            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                sqlAdap.Dispose();
            }
            return dsImage.Tables[0].Rows[0]["sysvalues"].ToString();
        }
        public override DataSet GetDeedVolume(string proj_key, string batch_key, string box_key, string pPolicyNo)
        {
            string sqlStr = null;
            DataSet dsVol = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select deed_vol,page_from,page_to from policy_master where proj_key = '" + proj_key + "' and batch_key = '" + batch_key + "' and box_number = '" + box_key + "' and policy_number='" + pPolicyNo + "'";


            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsVol);
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                sqlAdap.Dispose();
            }
            return dsVol;
        }
        public override DataSet GetDeedVolume(string deed_no)
        {
            string sqlStr = null;
            DataSet dsVol = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select deed_vol,page_from,page_to from policy_master where policy_no = '" + deed_no + "'";


            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsVol);
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                sqlAdap.Dispose();
            }
            return dsVol;
        }
        public override DataSet GetIndexDetails(string deed_no, string deed_year)
        {
            string sqlStr = null;
            DataSet dsVol = new DataSet();
            OdbcDataAdapter sqlAdap = null;

            sqlStr = "select letter_no,a.deed_no,b.ec_name,CONCAT(a.First_name , ' '  ,a.LAST_name) as name from index_of_name a,party_code b where a.party_code = b.ec_code and a.deed_no = '" + deed_no + "' ";


            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsVol);
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                sqlAdap.Dispose();
            }
            return dsVol;
        }
        public override ArrayList GetItems(eITEMS item, eSTATES[] state, wItem wi)
        {
            OdbcDataAdapter wAdap;
            OdbcTransaction trns = null;
            OdbcCommand oCom = new OdbcCommand();
            string strQuery = null;
            wItemControl wic = null;
            DataSet ds = new DataSet();
            ArrayList arrItem = new ArrayList();

            switch (item)
            {
                case eITEMS.PROJECT:
                    strQuery = "select proj_key, proj_code from Project_master where";
                    for (int j = 0; j < state.Length; j++)
                    {
                        strQuery = strQuery + state[j] + " or ";
                    }
                    strQuery = System.Text.RegularExpressions.Regex.Replace(strQuery, " or $", "");
                    wAdap = new OdbcDataAdapter(strQuery, sqlCon);
                    wAdap.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        wic = new CtrlProject((int)ds.Tables[0].Rows[i]["proj_key"], ds.Tables[0].Rows[i]["proj_code"].ToString());
                        arrItem.Add(wic);
                    }
                    break;
                case eITEMS.BATCH:
                    strQuery = "select batch_key, proj_code,batch_code,batch_name from batch_master where";
                    for (int j = 0; j < state.Length; j++)
                    {
                        strQuery = strQuery + state[j] + " or ";
                    }
                    strQuery = System.Text.RegularExpressions.Regex.Replace(strQuery, " or $", "");
                    wAdap = new OdbcDataAdapter(strQuery, sqlCon);
                    wAdap.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        wic = new CtrlBatch((int)ds.Tables[0].Rows[i]["batch_key"], ds.Tables[0].Rows[i]["batch_code"].ToString(), (int)ds.Tables[0].Rows[i]["proj_code"], ds.Tables[0].Rows[i]["batch_name"].ToString());
                        arrItem.Add(wic);
                    }
                    break;
                case eITEMS.BOX:
                    strQuery = "select proj_key,batch_key,box_number from box_master where";
                    for (int j = 0; j < state.Length; j++)
                    {
                        strQuery = strQuery + state[j] + " or ";
                    }
                    strQuery = System.Text.RegularExpressions.Regex.Replace(strQuery, " or $", "");
                    wAdap = new OdbcDataAdapter(strQuery, sqlCon);
                    wAdap.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        wic = new CtrlBox((int)ds.Tables[0].Rows[i]["proj_key"], (int)ds.Tables[0].Rows[i]["batch_key"], ds.Tables[0].Rows[i]["box_number"].ToString());
                        arrItem.Add(wic);
                    }
                    break;
                case eITEMS.POLICY:
                    wfeBox queryBox = (wfeBox)wi;

                    List<(string field, string op, string value)> predicates1 = new List<(string field, string op, string value)>();
                    predicates1.AddRange(state.Select(s => ("cfm.status", "=", $"'{(int)s}'")));

                    string predicate1String = string.Join(" OR ", predicates1.Select(p => $"({p.field} {p.op} {p.value})"));
                    strQuery = @"select cfm.proj_code, cfm.bundle_key, cfm.case_file_no, cfm.filename from case_file_master cfm,metadata_entry me"
                            + " where cfm.proj_code= '" + queryBox.ctrlBox.ProjectCode + "' and cfm.bundle_key='" + queryBox.ctrlBox.BatchKey
                            + "' and (" + predicate1String + ")"
                            + " and cfm.proj_code = me.proj_code "
                            + "and cfm.bundle_key = me.bundle_key and cfm.filename = me.filename" +
                            " order by me.running_serial";

                    oCom.Connection = sqlCon;
                    oCom.CommandText = strQuery;
                    wAdap = new OdbcDataAdapter(oCom);
                    wAdap.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string p = ds.Tables[0].Rows[i]["proj_code"].ToString();
                            string b = ds.Tables[0].Rows[i]["bundle_key"].ToString();

                            string pp = ds.Tables[0].Rows[i]["case_file_no"].ToString();
                            string ii = ds.Tables[0].Rows[i]["filename"].ToString();

                            wic = new CtrlPolicy(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_code"]), Convert.ToInt32(ds.Tables[0].Rows[i]["bundle_key"]), "1", ds.Tables[0].Rows[i]["filename"].ToString());
                            arrItem.Add(wic);

                        }
                    }

                    break;
                case eITEMS.PAGE:
                    string strQr = string.Empty;
                    wfePolicy queryPolicy = (wfePolicy)wi;
                    if ((int)state[0] == (int)NovaNet.wfe.eSTATES.POLICY_SCANNED)
                    {
                        strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A,policy_master B where A.proj_key=B.proj_key and A.batch_key = B.batch_key and A.box_number=B.box_number and A.policy_number = B.policy_number and A.photo <> 1 and A.proj_key=" + queryPolicy.ctrlPolicy.ProjectKey + " and A.batch_key=" + queryPolicy.ctrlPolicy.BatchKey + " and A.box_number='" + queryPolicy.ctrlPolicy.BoxNumber + "' and A.policy_number='" + queryPolicy.ctrlPolicy.PolicyNumber + "'";

                    }
                    else
                    {
                        strQr = "select * from (select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A inner join policy_master B on A.policy_number = B.policy_number left join doc_type_master c on a.doc_type = c.doc_type where A.proj_key=B.proj_key and A.batch_key = B.batch_key and A.box_number=B.box_number and A.policy_number = B.policy_number and A.proj_key=B.proj_key and A.proj_key=" + queryPolicy.ctrlPolicy.ProjectKey + " and A.batch_key=" + queryPolicy.ctrlPolicy.BatchKey + " and A.box_number='" + queryPolicy.ctrlPolicy.BoxNumber + "' and A.policy_number='" + queryPolicy.ctrlPolicy.PolicyNumber + "'";
                    }
                    for (int j = 0; j < state.Length; j++)
                    {
                        if ((int)state[j] != 0)
                        {
                            if (j == 0)
                            {
                                strQuery = strQuery + " and (B.status=" + (int)state[j];
                            }
                            else
                                strQuery = strQuery + " or B.status=" + (int)state[j];
                        }
                    }
                    if (((int)state[0] == (int)NovaNet.wfe.eSTATES.POLICY_SCANNED))
                    {
                        strQuery = strQuery + ") and A.status<>" + (int)eSTATES.PAGE_DELETED + " order by A.serial_no";
                    }
                    else
                    {
                        strQuery = strQuery + ") and A.status<>" + (int)eSTATES.PAGE_DELETED + " order by c.srlno,A.serial_no)";
                        string qr1 = strQr + strQuery + " as aa where aa.doc_type is not null union all ";
                        string qr2 = strQr + strQuery + " as aa where aa.doc_type is null";
                        strQuery = qr1 + qr2;
                    }

                    wAdap = new OdbcDataAdapter(strQuery, sqlCon);
                    wAdap.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        wic = new CtrlImage(Convert.ToInt32(ds.Tables[0].Rows[i]["proj_key"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[i]["batch_key"].ToString()), ds.Tables[0].Rows[i]["box_number"].ToString(), ds.Tables[0].Rows[i]["policy_number"].ToString(), ds.Tables[0].Rows[i]["page_name"].ToString(), ds.Tables[0].Rows[i]["doc_type"].ToString());
                        arrItem.Add(wic);
                    }
                    break;
                case eITEMS.LIC_QA_PAGE:
                    wfePolicy licPolicy = (wfePolicy)wi;
                    strQuery = "select distinct A.proj_key,A.batch_key,A.box_number,A.policy_number,A.page_name,A.page_index_name,A.doc_type from image_master A,policy_master B,doc_type_master c where A.proj_key=B.proj_key and A.batch_key=B.batch_key and A.box_number=B.box_number and A.policy_number = B.policy_number and A.doc_type = c.doc_type and  A.proj_key=" + licPolicy.ctrlPolicy.ProjectKey + " and A.batch_key=" + licPolicy.ctrlPolicy.BatchKey + " and A.box_number=" + licPolicy.ctrlPolicy.BoxNumber + " and A.policy_number='" + licPolicy.ctrlPolicy.PolicyNumber + "'";
                    for (int j = 0; j < state.Length; j++)
                    {
                        if ((int)state[j] != 0)
                        {
                            if (j == 0)
                            {
                                strQuery = strQuery + " and (B.status=" + (int)state[j];
                            }
                            else
                                strQuery = strQuery + " or B.status=" + (int)state[j];
                        }
                    }
                    strQuery = strQuery + ") and A.status<>" + (int)eSTATES.PAGE_DELETED + " order by c.srlno,A.serial_no";
                    wAdap = new OdbcDataAdapter(strQuery, sqlCon);
                    wAdap.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        wic = new CtrlImage((int)ds.Tables[0].Rows[i]["proj_key"], (int)ds.Tables[0].Rows[i]["batch_key"], (string)ds.Tables[0].Rows[i]["box_number"].ToString(), ds.Tables[0].Rows[i]["policy_number"].ToString(), ds.Tables[0].Rows[i]["page_name"].ToString(), ds.Tables[0].Rows[i]["doc_type"].ToString());
                        arrItem.Add(wic);
                    }
                    break;
            }
            return arrItem;
        }
        public List<(string name, int status)> GetAllBatches(string bundleName = "")
        {
            var batchesUnderProject = new List<(string, int)>();
            FetchBatchesFromProjectCmd.Parameters[COLUMN_LEGACY_BATCH_NAME].Value = $@"%{bundleName}%";
            using (var reader = FetchBatchesFromProjectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var batchName = reader.GetString(reader.GetOrdinal(COLUMN_BATCH_NAME));
                    var batchStatus = reader.GetInt16(reader.GetOrdinal(COLUMN_BATCH_STATUS));
                    batchesUnderProject.Add((batchName, batchStatus));
                }
                reader.Close();
            }
            return batchesUnderProject;
        }

        public List<(string name, int status)> GetAllBatchesByParent(string districtID)
        {
            var batchesUnderDistrict = new List<(string, int)>();
            FetchBatchesForParentCmd.Parameters[COLUMN_MASTER_ID].Value = districtID;

            using (var reader = FetchBatchesForParentCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var batchName = reader.GetString(reader.GetOrdinal(COLUMN_BATCH_NAME));
                    var batchStatus = reader.GetInt16(reader.GetOrdinal(COLUMN_BATCH_STATUS));
                    batchesUnderDistrict.Add((batchName, batchStatus));
                }
                reader.Close();
            }
            return batchesUnderDistrict;
        }
    }
}