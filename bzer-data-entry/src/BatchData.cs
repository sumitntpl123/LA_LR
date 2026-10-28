using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;

namespace bzer_data_entry.src
{
    public class BatchMetadata
    {
        public MasterItem District { get; set; }

        public MasterItem Block { get; set; }

        public MasterItem Mouza { get; set; }

        public BatchMetadata()
        {
            District = new MasterItem();
            Block = new MasterItem();
            Mouza = new MasterItem();
        }
    }

    public class BatchKeyData
    {
        public string BundleKey { get; set; }
        public string BundleName { get; set; }
        public string BundleNo { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        //public DateTime HandoverDate { get; set; }
    }

    public class BatchDataStore
    {
        #region constants
        const string TABLE_BATCH_VALUE = "batch_value_cs";

        const string TABLE_METADATA_MASTER = "metadata_master_cs";

        const string COLUMN_MASTER_ID = "master_id";

        const string COLUMN_FIELD_NAME = "field_name";

        const string COLUMN_LANG = "lang";

        const string COLUMN_SEQ = "seq";

        const string COLUMN_VAL = "val";

        const string COLUMN_PROJECT_CODE = "project_code";

        const string COLUMN_BATCH_CODE = "batch_code";

        private static string FUNC_UUID_TO_BIN(string val)
        {
            return $"UNHEX(REPLACE({val}, '-', ''))";
        }

        private static string FUNC_BIN_TO_UUID(string column)
        {
            return $"HEX({column})";
        }

        const string LANG_EN = "En";

        const string LANG_BN = "Bn";

        const string LANG_BN_PHON = "BnPhonetic";
        #endregion

        #region prepared statements
        private readonly OdbcCommand DeleteValueCmd;

        private readonly OdbcCommand AddMasterValueCmd;

        private readonly OdbcCommand FetchMasterIDCmd;

        private readonly OdbcCommand FetchMasterValueCmd;

        private readonly OdbcCommand FetchBatchInfoCmd;
        #endregion

        private readonly OdbcConnection SqlCon;

        private readonly OdbcTransaction SqlTran;

        /// <summary>
        /// The usage of OdbcCommand.Prepare() has been removed from this class as part
        /// of testing and investigation into the recurring "Connection has been disabled"
        /// issue. In certain environments and driver versions, prepared statements have
        /// been linked to unstable connection behavior.
        ///
        /// By removing .Prepare(), we aim to simplify command execution and monitor
        /// whether this change improves connection stability and prevents unexpected
        /// disconnections during runtime.
        /// </summary>
        public BatchDataStore(OdbcConnection sqlCon, OdbcTransaction sqlTran = null)
        {
            SqlCon = sqlCon;
            SqlTran = sqlTran;

            #region DeleteValue
            DeleteValueCmd = new OdbcCommand($@"
            DELETE FROM {TABLE_BATCH_VALUE} 
            WHERE 
                {COLUMN_PROJECT_CODE}=? AND
                {COLUMN_BATCH_CODE}=? AND
                {COLUMN_FIELD_NAME}=?
            ", SqlCon);
            DeleteValueCmd.Transaction = SqlTran;
            DeleteValueCmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int);
            DeleteValueCmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int);
            DeleteValueCmd.Parameters.Add(COLUMN_FIELD_NAME, OdbcType.VarChar);
            //DeleteValueCmd.Prepare();
            #endregion

            #region AddMasterValue
            AddMasterValueCmd = new OdbcCommand($@"
            INSERT INTO {TABLE_BATCH_VALUE}
            ({COLUMN_PROJECT_CODE}, {COLUMN_BATCH_CODE}, {COLUMN_FIELD_NAME}, {COLUMN_LANG}, {COLUMN_SEQ}, {COLUMN_MASTER_ID})
            VALUES
            (?,?,?,?,?,{FUNC_UUID_TO_BIN("?")})
            ", SqlCon);
            AddMasterValueCmd.Transaction = SqlTran;
            AddMasterValueCmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int);
            AddMasterValueCmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int);
            AddMasterValueCmd.Parameters.Add(COLUMN_FIELD_NAME, OdbcType.VarChar);
            AddMasterValueCmd.Parameters.Add(COLUMN_LANG, OdbcType.VarChar);
            AddMasterValueCmd.Parameters.Add(COLUMN_SEQ, OdbcType.Int);
            AddMasterValueCmd.Parameters.Add(COLUMN_MASTER_ID, OdbcType.VarChar);
            //AddMasterValueCmd.Prepare();
            #endregion

            #region FetchMasterID
            FetchMasterIDCmd = new OdbcCommand($@"
            SELECT 
                {COLUMN_SEQ}, 
                {FUNC_BIN_TO_UUID(COLUMN_MASTER_ID)} AS {COLUMN_MASTER_ID}
            FROM {TABLE_BATCH_VALUE}
            WHERE
                {COLUMN_PROJECT_CODE}=? AND
                {COLUMN_BATCH_CODE}=? AND
                {COLUMN_FIELD_NAME}=?
            ", SqlCon);
            FetchMasterIDCmd.Transaction = SqlTran;
            FetchMasterIDCmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int);
            FetchMasterIDCmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int);
            FetchMasterIDCmd.Parameters.Add(COLUMN_FIELD_NAME, OdbcType.VarChar);
            //FetchMasterIDCmd.Prepare();
            #endregion

            #region FetchMasterValue
            FetchMasterValueCmd = new OdbcCommand($@"
            SELECT 
                {COLUMN_LANG},
                {COLUMN_SEQ},
                {COLUMN_VAL},
                {COLUMN_FIELD_NAME}
            FROM {TABLE_METADATA_MASTER}
            WHERE
                {COLUMN_MASTER_ID}={FUNC_UUID_TO_BIN("?")}
            ", SqlCon);
            FetchMasterValueCmd.Transaction = SqlTran;
            FetchMasterValueCmd.Parameters.Add(COLUMN_MASTER_ID, OdbcType.VarChar);
            //FetchMasterValueCmd.Prepare();
            #endregion

            #region FetchBatchInfo
            FetchBatchInfoCmd = new OdbcCommand($@"
                SELECT
                b.bundle_key ,
                b.bundle_name ,
                b.bundle_no,
                b.proj_code,
                pm.Proj_Code
                FROM bundle_master b
                INNER JOIN batch_value_cs bv ON b.proj_code = bv.project_code AND b.bundle_key = bv.batch_code AND bv.master_id={FUNC_UUID_TO_BIN("?")}
                INNER JOIN project_master pm ON pm.proj_key=b.proj_code", SqlCon);
            FetchBatchInfoCmd.Transaction = SqlTran;
            FetchBatchInfoCmd.Parameters.Add(COLUMN_MASTER_ID, OdbcType.VarChar);
            //FetchBatchInfoCmd.Prepare();
            #endregion

        }

        public void SaveBatchMetadata(int projectID, int batchID, BatchMetadata data)
        {
            SetValue(projectID, batchID, MetadataStore.FIELD_DISTRICT, data.District);
            SetValue(projectID, batchID, MetadataStore.FIELD_BLOCK, data.Block);
            SetValue(projectID, batchID, MetadataStore.FIELD_MOUZA, data.Mouza);
        }

        public BatchMetadata GetBatchMetadata(int projectID, int batchID)
        {
            var batchData = new BatchMetadata();
            batchData.District = GetMasterValue(projectID, batchID, MetadataStore.FIELD_DISTRICT);
            //batchData.Block = GetMasterValue(projectID, batchID, MetadataStore.FIELD_BLOCK);
            //batchData.Mouza = GetMasterValue(projectID, batchID, MetadataStore.FIELD_MOUZA);

            return batchData;
        }
        public List<BatchKeyData> GetBatchKeyData(Guid masterID)
        {
            List<BatchKeyData> batchInfo = new List<BatchKeyData>();
            FetchBatchInfoCmd.Parameters[COLUMN_MASTER_ID].Value = masterID.ToString();
            using (var reader = FetchBatchInfoCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    batchInfo.Add(new BatchKeyData
                    {
                        BundleKey = reader["bundle_key"].ToString(),
                        BundleName = reader["bundle_name"].ToString(),
                        BundleNo = reader["bundle_no"].ToString(),
                        ProjectCode = reader["proj_code"].ToString(),
                        ProjectName = reader["Proj_Code"].ToString()
                       // HandoverDate = DateTime.Parse(reader["handover_date"].ToString())
                    });
                }
            }
            return batchInfo;
        }

        #region private methods
        private void SetValue(int projectKey, int batchKey, string fieldName, MasterItem fieldValue)
        {
            DeleteValue(projectKey, batchKey, fieldName);
            AddValue(projectKey, batchKey, fieldName, 0, fieldValue);
        }

        private MasterItem GetMasterValue(int projectKey, int batchKey, string fieldName)
        {
            var masterID = GetMasterID(projectKey, batchKey, fieldName);
            var (master, field) = GetMasterValue(masterID);
            if (field != fieldName)
            {
                throw new FetchError($"Field name {fieldName} for {masterID} doesn't match with found field name {field}");
            }
            return master;
        }

        private Guid GetMasterID(int projectKey, int batchKey, string fieldName)
        {
            var items = new Dictionary<int, Guid>();
            FetchMasterIDCmd.Parameters[COLUMN_PROJECT_CODE].Value = projectKey;
            FetchMasterIDCmd.Parameters[COLUMN_BATCH_CODE].Value = batchKey;
            FetchMasterIDCmd.Parameters[COLUMN_FIELD_NAME].Value = fieldName;
            using (var reader = FetchMasterIDCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var seq = reader.GetInt32(reader.GetOrdinal(COLUMN_SEQ));
                    var masterID = Guid.ParseExact(reader.GetString(reader.GetOrdinal(COLUMN_MASTER_ID)), "N");

                    if (!items.ContainsKey(seq))
                    {
                        items.Add(seq, Guid.Empty);
                    }
                    items[seq] = masterID;
                }
            }
            if (items.Count == 0)
            {
                throw new FetchError($"Field with {fieldName} is not found");
            }
            else if (items.Count != 1)
            {
                throw new FetchError($"Multiple items found against {fieldName}");
            }
            return items.Single().Value;
        }

        private (MasterItem value, string fieldName) GetMasterValue(Guid masterID)
        {
            var items = new Dictionary<int, (MasterItem, string)>();
            FetchMasterValueCmd.Parameters[COLUMN_MASTER_ID].Value = masterID.ToString();
            using (var reader = FetchMasterValueCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var lang = reader.GetString(reader.GetOrdinal(COLUMN_LANG));
                    var seq = reader.GetInt32(reader.GetOrdinal(COLUMN_SEQ));
                    var val = reader.GetString(reader.GetOrdinal(COLUMN_VAL));
                    var fieldName = reader.GetString(reader.GetOrdinal(COLUMN_FIELD_NAME));

                    if (!items.ContainsKey(seq))
                    {
                        items.Add(seq, (new MasterItem(), ""));
                    }
                    items[seq].Item1.MasterID = masterID;
                    if (lang == LANG_EN)
                    {
                        items[seq].Item1.Value.EnglishText = val;
                    }
                    else if (lang == LANG_BN)
                    {
                        items[seq].Item1.Value.BengaliText = val;
                    }
                    else if (lang == LANG_BN_PHON)
                    {
                        items[seq].Item1.Value.BengaliPhoneticText = val;
                    }
                    else
                    {
                        throw new FetchError($"Language value {lang} was not recognized");
                    }
                    items[seq] = (items[seq].Item1, fieldName);
                }
            }
            if (items.Count == 0)
            {
                throw new FetchError($"Field with {masterID} is not found");
            }
            else if (items.Count != 1)
            {
                throw new FetchError($"Multiple items found against {masterID} on fields {items.Select(item => item.Value.Item2).Aggregate((item1, item2) => $"{item1}, {item2}")}");
            }
            return items.Single().Value;
        }

        private int DeleteValue(int projectKey, int batchKey, string fieldName)
        {
            DeleteValueCmd.Parameters[COLUMN_PROJECT_CODE].Value = projectKey;
            DeleteValueCmd.Parameters[COLUMN_BATCH_CODE].Value = batchKey;
            DeleteValueCmd.Parameters[COLUMN_FIELD_NAME].Value = fieldName;
            return DeleteValueCmd.ExecuteNonQuery();
        }

        private int AddValue(int projectKey, int batchKey, string fieldName, int index, MasterItem fieldValue)
        {
            AddMasterValueCmd.Parameters[COLUMN_PROJECT_CODE].Value = projectKey;
            AddMasterValueCmd.Parameters[COLUMN_BATCH_CODE].Value = batchKey;
            AddMasterValueCmd.Parameters[COLUMN_FIELD_NAME].Value = fieldName;
            AddMasterValueCmd.Parameters[COLUMN_SEQ].Value = index;

            AddMasterValueCmd.Parameters[COLUMN_LANG].Value = LANG_EN;
            AddMasterValueCmd.Parameters[COLUMN_MASTER_ID].Value = fieldValue.MasterID.ToString();
            var inserted = AddMasterValueCmd.ExecuteNonQuery();
            if (inserted != 1)
            {
                throw new StoreError($"{fieldName} was not successfully stored");
            }
            return inserted;
        }
        #endregion
    }
}
