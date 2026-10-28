using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;

namespace ImageHeaven.Logic
{
    public class frmMouzaExportLogic
    {
        OdbcConnection SqlCon;
        private static string FUNC_UUID_TO_BIN(string val)
        {
            return $"UNHEX(REPLACE({val}, '-', ''))";
        }
        public frmMouzaExportLogic(OdbcConnection conn)
        {
            SqlCon = conn;
        }

        /// <summary>
        /// Retrieves the list of Mouzas (villages/regions) eligible for export 
        /// based on their batch processing status.
        /// </summary>
        /// <param name="bState">
        /// The batch state(s) to filter by. 
        /// - If the first state is <c>BATCH_READY_FOR_UAT</c> (status = 7), 
        ///   then the function will return mouzas that are either strictly UAT-ready 
        ///   or have both UAT-ready and already exported bundles. 
        /// - Otherwise, it will return mouzas where all bundles are fully exported (status = 8).
        /// </param>
        public DataTable GetMouzaForExport(eSTATES[] bState, Guid MasterId)
        {
            var query = string.Empty;
            var UAT_COMPLETE_STATUS = (int)eSTATES.BATCH_READY_FOR_UAT;
            var RE_EXPORT_STATUS = (int)eSTATES.BATCH_EXPORTED;
            if (bState[0] == eSTATES.BATCH_READY_FOR_UAT)
            {
                query = $@"
                    SELECT DISTINCT m.val, HEX(m.master_id) AS master_id
                    FROM metadata_master_cs m
                    JOIN batch_value_cs v ON v.master_id = m.master_id
                    JOIN 



bm ON bm.bundle_key = v.batch_code
                    JOIN project_master pm ON pm.proj_key = bm.proj_code
                    WHERE m.field_name = 'mouza'
                      AND m.lang = 'Bn'
                      AND m.parent_master_id = {FUNC_UUID_TO_BIN(" ? ")}
                      AND(
                        -- Case 1: only 7 exists
                            (EXISTS(SELECT 1 FROM bundle_master bm2
                                     JOIN batch_value_cs v2 ON bm2.bundle_key = v2.batch_code
                                     WHERE v2.master_id = m.master_id AND bm2.status = {UAT_COMPLETE_STATUS})
                             AND NOT EXISTS(SELECT 1 FROM bundle_master bm3
                                             JOIN batch_value_cs v3 ON bm3.bundle_key = v3.batch_code
                                             WHERE v3.master_id = m.master_id AND bm3.status NOT IN({UAT_COMPLETE_STATUS})))
                            OR
                            -- Case 2: both 7 and 8 exist
                            (EXISTS(SELECT 1 FROM bundle_master bm2
                                     JOIN batch_value_cs v2 ON bm2.bundle_key = v2.batch_code
                                     WHERE v2.master_id = m.master_id AND bm2.status = {UAT_COMPLETE_STATUS})
                             AND EXISTS(SELECT 1 FROM bundle_master bm3
                                         JOIN batch_value_cs v3 ON bm3.bundle_key = v3.batch_code
                                         WHERE v3.master_id = m.master_id AND bm3.status = {RE_EXPORT_STATUS}))
                          )";
            }
            else
            {
                query = $@"
                    SELECT m.val, HEX(m.master_id) AS master_id
                    FROM metadata_master_cs m
                    JOIN batch_value_cs v ON v.master_id = m.master_id
                    JOIN bundle_master bm ON bm.bundle_key = v.batch_code
                    JOIN project_master pm ON pm.proj_key = bm.proj_code
                    WHERE m.field_name = 'mouza'
                      AND m.lang = 'Bn'
                      AND m.parent_master_id = {FUNC_UUID_TO_BIN(" ? ")}
                    GROUP BY m.master_id, m.val
                    HAVING COUNT(DISTINCT bm.status) = 1
                       AND MAX(bm.status) = {RE_EXPORT_STATUS}";
            }

            var dt = new DataTable();
            var _status = (int)bState[0];
            try
            {
                var cmd = new OdbcCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("?", MasterId.ToString());
                cmd.Parameters.AddWithValue("?", _status);
                var oAdpt = new OdbcDataAdapter(cmd);
                oAdpt.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        public DataTable GetBundleDetailsByMouzaMasterId(List<string> masterIds)
        {
            var masterIdList = string.Join(",", masterIds.Select(x => $"'{x}'"));
            var query = $@"select bundle_key, bundle_name, proj_code, mm.val from bundle_master bm
                    inner join batch_value_cs v on bm.bundle_key = v.batch_code
                    and hex(v.master_id) in ({masterIdList}) 
                    inner join metadata_master_cs mm on mm.master_id = v.master_id
                    and mm.field_name ='mouza' and mm.lang='Bn'
                ";
            var dt = new DataTable();
            try
            {
                var oAdpt = new OdbcDataAdapter(query, SqlCon);
                oAdpt.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        public DataTable getProjectName(string pcode)
        {
            DataTable dt = new DataTable();
            string sql = "select distinct proj_key,proj_code from project_master where proj_key = '" + pcode + "' ";
            try
            {
                OdbcCommand cmd = new OdbcCommand(sql, SqlCon);
                OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
                odap.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        public List<(string filename, int status)> GetAllPolicyStatus(string proj_key, string batch_key)
        {
            var fetchRecord = new List<(string filename, int status)>();
            try
            {
                var sqlStr = $@"select cfm.filename, cfm.status from case_file_master cfm, metadata_entry me where cfm.proj_code=? and 
                    cfm.bundle_key= ? and cfm.proj_code = me.proj_code and cfm.bundle_key = me.bundle_key and cfm.filename = me.filename
                     order by me.running_serial";

                var cmd = new OdbcCommand(sqlStr, SqlCon);
                cmd.Parameters.AddWithValue("?", proj_key);
                cmd.Parameters.AddWithValue("?", batch_key);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var flagCode = reader.GetString(0);
                        var caseReference = reader.GetInt32(1);
                        fetchRecord.Add((flagCode, caseReference));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return fetchRecord;
        }
    }
}
