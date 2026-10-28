using bzer_data_entry;
using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;

namespace Reporting
{
    public class ReportingDataStore
    {
        private const string FUNC_UUID_TO_BIN = "UNHEX(REPLACE(?, '-', ''))";
        private const string TABLE_METADATA = "metadata_record_cs";
        private const string TABLE_METADATA_VALUE = "metadata_value_cs";
        private const string TABLE_IMAGE = "image_master";
        private const string COLUMN_FIELD_NAME = "field_name";
        private const string COLUMN_VAL = "val";
        private const string COLUMN_FILE_ID = "file_id";
        private const string COLUMN_PROJECT_CODE = "project_code";
        private const string COLUMN_BATCH_CODE = "batch_code";
        private const string COLUMN_LANG = "lang";
        private const string COLUMN_IMAGE_FILE_NAME = "policy_number";
        private const string COLUMN_IMAGE_PAGE = "page_index_name";
        private const string COLUMN_IMAGE_PROJ_CODE = "proj_key";
        private const string COLUMN_IMAGE_BATCH_CODE = "batch_key";
        private const string COLUMN_IMAGE_STATUS = "status";
        private const string FIELD_FILE_NAME = "file-name";
        private const string FIELD_KHATIAN = "khatian";
        private const string FIELD_BATA_KHATIAN = "bata-khatian";
        private const string FIELD_MOUZA = "mouza";
        private const string LANG_EN = "En";

        public static readonly Dictionary<eSTATES, int> policy_states = new Dictionary<eSTATES, int>()
        {
            { eSTATES.POLICY_INITIALIZED, 1 },
            { eSTATES.POLICY_CREATED, 2 },
            { eSTATES.POLICY_SCANNED, 3 },
            { eSTATES.POLICY_QC, 4 },
            { eSTATES.POLICY_FQC, 5 },
            { eSTATES.POLICY_INDEXED, 19 },
            { eSTATES.POLICY_CONFLICT, 21},
            { eSTATES.POLICY_NOT_INDEXED, 40 },
            { eSTATES.POLICY_EXPORTED, 22 },
            { eSTATES.POLICY_EXCEPTION, 30 },
            { eSTATES.POLICY_CHECKED, 31 }
        };

        private readonly OdbcConnection SqlCon;
        private readonly OdbcTransaction SqlTran;

        private static string FUNC_BIN_TO_UUID(string column)
        {
            return $"HEX({column})";
        }
        private static string FirstKhatianBataKhatianQueryString(string order)
        {
            return $@"
                SELECT 
                {FUNC_BIN_TO_UUID($"kh.{COLUMN_FILE_ID}")} AS file_id,
                kh.val AS khatian,
                bkh.val AS bata_khatian
            FROM 
                metadata_value_cs kh
            JOIN 
                metadata_record_cs mrc1 ON kh.{COLUMN_FILE_ID} = mrc1.{COLUMN_FILE_ID}
            JOIN 
                metadata_value_cs bkh ON kh.{COLUMN_FILE_ID} = bkh.{COLUMN_FILE_ID}
            WHERE 
                kh.{COLUMN_FIELD_NAME} = '{FIELD_KHATIAN}' AND 
                kh.{COLUMN_LANG} = '{LANG_EN}' AND 
                bkh.{COLUMN_FIELD_NAME} = '{FIELD_BATA_KHATIAN}' AND 
                bkh.{COLUMN_LANG} = '{LANG_EN}' AND
                mrc1.project_code = ? AND
                mrc1.batch_code = ?
            ORDER BY 
                CAST(kh.val AS UNSIGNED) {order}, 
                CAST(bkh.val AS UNSIGNED) {order}
            LIMIT 1;
            ";
        }

        public ReportingDataStore(OdbcConnection sqlCon, OdbcTransaction sqlTran = null)
        {
            SqlCon = sqlCon;
            SqlTran = sqlTran;
        }

        public Guid GetFirstEntryFileInBatch(int projectCode, int batchCode)
        {
            try
            {
                var cmd = new OdbcCommand($@"
                SELECT 
                    {FUNC_BIN_TO_UUID($"meta.{COLUMN_FILE_ID}")} AS {COLUMN_FILE_ID}
                FROM
                    case_file_master cfm,
                    {TABLE_METADATA} meta,
                    {TABLE_METADATA_VALUE} val
                WHERE
                    meta.{COLUMN_PROJECT_CODE} = ? AND
                    meta.{COLUMN_BATCH_CODE} = ? AND
                    val.{COLUMN_FIELD_NAME} = '{FIELD_FILE_NAME}' AND
                    val.{COLUMN_VAL} = cfm.filename AND
                    val.{COLUMN_FILE_ID} = meta.{COLUMN_FILE_ID}
                ORDER BY 
                    cfm.item_no ASC
                LIMIT 1
                ", SqlCon);
                cmd.Transaction = SqlTran;
                cmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int).Value = projectCode;
                cmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int).Value = batchCode;

                var result = cmd.ExecuteScalar();
                return Guid.ParseExact(result.ToString(), "N");
            }
            catch (Exception ex)
            {
                throw new FetchError(ex.Message);
            }
        }

        public Guid GetLastEntryFileInBatch(int projectCode, int batchCode)
        {
            try
            {
                var cmd = new OdbcCommand($@"
                SELECT 
                    {FUNC_BIN_TO_UUID($"meta.{COLUMN_FILE_ID}")} AS {COLUMN_FILE_ID}
                FROM
                    case_file_master cfm,
                    {TABLE_METADATA} meta,
                    {TABLE_METADATA_VALUE} val
                WHERE
                    meta.{COLUMN_PROJECT_CODE} = ? AND
                    meta.{COLUMN_BATCH_CODE} = ? AND
                    val.{COLUMN_FIELD_NAME} = '{FIELD_FILE_NAME}' AND
                    val.{COLUMN_VAL} = cfm.filename AND
                    val.{COLUMN_FILE_ID} = meta.{COLUMN_FILE_ID}
                ORDER BY 
                    cfm.item_no DESC
                LIMIT 1
                ", SqlCon);
                cmd.Transaction = SqlTran;
                cmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int).Value = projectCode;
                cmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int).Value = batchCode;

                var result = cmd.ExecuteScalar();
                return Guid.ParseExact(result.ToString(), "N");
            }
            catch (Exception ex)
            {
                throw new FetchError(ex.Message);
            }
        }

        public int GetFileCountByStatus(List<eSTATES> eSTATEs, int? projectID = null, int? batchID = null)
        {
            try
            {
                var conditions = new List<string>();
                if (eSTATEs.Count > 0 && eSTATEs != null)
                {
                    conditions.Add($"status in ({string.Join(",", eSTATEs.Select(s => $"{policy_states[s]}"))}) ");
                }
                if (projectID.HasValue && batchID.HasValue)
                {
                    conditions.Add("proj_code=?");
                    conditions.Add("bundle_key=?");
                }
                else if (projectID.HasValue)
                {
                    conditions.Add("proj_code=?");
                }
                var query = "SELECT COUNT(*) AS file_count FROM case_file_master" + (conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : "");

                var cmd = new OdbcCommand(query, SqlCon);
                cmd.Transaction = SqlTran;
                if (projectID.HasValue && batchID.HasValue)
                {
                    cmd.Parameters.Add("proj_code", OdbcType.Int).Value = projectID;
                    cmd.Parameters.Add("bundle_key", OdbcType.Int).Value = batchID;
                }
                else if (projectID.HasValue)
                {
                    cmd.Parameters.Add("proj_code", OdbcType.Int).Value = projectID;
                }
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new FetchError(ex.Message);
            }
        }

        public (Guid fileID, string khatianNumber, string bataKhatianNumber) GetMaximumKhatianEntry(int projectID, int batchID)
        {
            try
            {
                var khEntries = new List<(Guid fileID, string khatianNumber, string bataKhatianNumber)>();
                var cmd = new OdbcCommand(FirstKhatianBataKhatianQueryString("DESC"), SqlCon);
                cmd.Transaction = SqlTran;
                cmd.Parameters.AddWithValue(COLUMN_PROJECT_CODE, projectID);
                cmd.Parameters.AddWithValue(COLUMN_BATCH_CODE, batchID);
                using (var result = cmd.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var fileID = Guid.ParseExact(result.GetString(result.GetOrdinal(COLUMN_FILE_ID)), "N");
                        var khatian = result.GetString(result.GetOrdinal("khatian"));
                        var bataKhatian = result.GetString(result.GetOrdinal("bata_khatian"));
                        khEntries.Add((fileID, khatianNumber: khatian, bataKhatianNumber: bataKhatian));
                    }
                }
                Debug.Assert(khEntries.Count == 1);
                return khEntries[0];
            }
            catch (Exception ex)
            {
                throw new FetchError(ex.Message);
            }
        }

        public (Guid fileID, string khatianNumber, string bataKhatianNumber) GetMinimumKhatianEntry(int projectID, int batchID)
        {
            try
            {
                var khEntries = new List<(Guid fileID, string khatianNumber, string bataKhatianNumber)>();
                var cmd = new OdbcCommand(FirstKhatianBataKhatianQueryString("ASC"), SqlCon);
                cmd.Transaction = SqlTran;
                cmd.Parameters.AddWithValue(COLUMN_PROJECT_CODE, projectID);
                cmd.Parameters.AddWithValue(COLUMN_BATCH_CODE, batchID);
                using (var result = cmd.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var fileID = Guid.ParseExact(result.GetString(result.GetOrdinal(COLUMN_FILE_ID)), "N");
                        var khatian = result.GetString(result.GetOrdinal("khatian"));
                        var bataKhatian = result.GetString(result.GetOrdinal("bata_khatian"));
                        khEntries.Add((fileID, khatianNumber: khatian, bataKhatianNumber: bataKhatian));
                    }
                }
                Debug.Assert(khEntries.Count == 1);
                return khEntries[0];
            }
            catch (Exception ex)
            {
                throw new FetchError(ex.Message);
            }
        }

        public List<int> GetKhatianEntriesForMouza(Guid mouzaGuid)
        {
            var khatianEntries = new List<int>();
            try
            {
                var cmd = new OdbcCommand($@"
                SELECT 
                    CAST(mv1.val AS UNSIGNED) AS khatian_number
                FROM {TABLE_METADATA_VALUE} mv1
                JOIN {TABLE_METADATA_VALUE} mv2 
                    ON mv1.file_id = mv2.file_id
                WHERE 
                    mv1.{COLUMN_FIELD_NAME} = '{FIELD_KHATIAN}'
                    AND mv1.{COLUMN_LANG} = '{LANG_EN}'
                    AND mv2.{COLUMN_FIELD_NAME} = '{FIELD_MOUZA}'
                    AND mv2.master_id = {FUNC_UUID_TO_BIN}
                    AND mv2.{COLUMN_LANG} = '{LANG_EN}'
                ORDER BY CAST(mv1.val AS UNSIGNED);
                ", SqlCon);

                cmd.Transaction = SqlTran;
                cmd.Parameters.Add("@master_id", OdbcType.VarChar).Value = mouzaGuid.ToString();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        khatianEntries.Add(reader.GetInt32(reader.GetOrdinal("khatian_number")));
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new FetchError(Ex.Message);
            }
            return khatianEntries;
        }

        public List<int> GetMissingKhatianNumbers(Guid mouzaGuid)
        {
            var khatianEntries = GetKhatianEntriesForMouza(mouzaGuid);

            // khatian number starts from 1, refer to https://github.com/NevaehTechPMO/bzer/issues/4#issuecomment-2254976019
            int minKhatianNumber = 1;
            int maxKhatianNumber = khatianEntries.Max();

            var missingKhatianNumbers = Enumerable.Range(minKhatianNumber, maxKhatianNumber - minKhatianNumber + 1)
                                        .Except(khatianEntries)
                                        .ToList();
            return missingKhatianNumbers;
        }

        public List<(string ImagePath, string FileName)> GetImages(int projectID, int batchID, string fileName = null, List<eSTATES> states = null)
        {
            var conditions = new List<(string query, OdbcParameter[] param)> {
                (query: $"{COLUMN_IMAGE_PROJ_CODE}=?", param: new[]{new OdbcParameter(COLUMN_IMAGE_PROJ_CODE, OdbcType.Int) { Value = projectID } }),
                (query: $"{COLUMN_IMAGE_BATCH_CODE}=?", param: new[] {new OdbcParameter(COLUMN_IMAGE_BATCH_CODE, OdbcType.Int) { Value = batchID } })
            };
            if (!string.IsNullOrEmpty(fileName))
            {
                conditions.Add((query: $"{COLUMN_IMAGE_FILE_NAME}=?", param: new[] { new OdbcParameter(COLUMN_IMAGE_FILE_NAME, OdbcType.VarChar) { Value = fileName } }));
            }
            if (states != null && states.Count > 0)
            {
                conditions.Add(
                    (
                        query: $"{COLUMN_IMAGE_STATUS} IN ({string.Join(",", states.Select(state => "?"))})",
                        param: states.Select((state, id) => new OdbcParameter($"{COLUMN_IMAGE_STATUS}_{id}", OdbcType.Int) { Value = (int)state }).ToArray()
                    )
                );
            }
            var conditionStr = conditions.Count > 0 ? $"WHERE {string.Join(" AND ", conditions.Select(cond => cond.query))}" : string.Empty;
            var fetchImageCmd = new OdbcCommand($@"
                SELECT
                    {COLUMN_IMAGE_PAGE},
                    {COLUMN_IMAGE_FILE_NAME}
                FROM
                    {TABLE_IMAGE}
                {conditionStr}
            ", SqlCon);
            fetchImageCmd.Transaction = SqlTran;
            fetchImageCmd.Parameters.AddRange(conditions.SelectMany(cond => cond.param).ToArray());

            var batchImages = new List<(string ImagePath, string FileName)>();
            using (var reader = fetchImageCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    batchImages.Add((ImagePath: reader.GetString(reader.GetOrdinal(COLUMN_IMAGE_PAGE)), FileName: reader.GetString(reader.GetOrdinal(COLUMN_IMAGE_FILE_NAME))));
                }
            }
            return batchImages;
        }


    }
}
