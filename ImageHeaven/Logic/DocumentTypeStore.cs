using NovaNet.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;

namespace ImageHeaven.Logic
{
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
    public class DocumentTypeStore
    {
        #region constants
        const string TBL_IMAGE_MASTER = "image_master";

        const string COL_FILE_NAME = "policy_number";

        const string COL_DOC_TYPE = "doc_type";

        const string COL_PROJECT_KEY = "proj_key";

        const string COL_BATCH_KEY = "batch_key";
        #endregion

        private readonly OdbcConnection SqlCon;

        private readonly OdbcTransaction SqlTran;

        private readonly OdbcCommand GetDocTypeCountCmd;

        private readonly Dictionary<string, char> KeyConfig;

        private readonly Dictionary<char, string> KeyConfigRev;

        private ImageConfig ConfigFileReader;

        public DocumentTypeStore(OdbcConnection sqlCon, OdbcTransaction sqlTran = null)
        {
            ConfigFileReader = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            KeyConfig = ConfigFileReader.GetValueIndex(ihConstants.INDEX_SHORTCUT_KEY_SECTION)
                .Where(pair => !string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value.Trim()[0]);
            KeyConfigRev = KeyConfig.ToDictionary(pair => pair.Value, pair => pair.Key);
            SqlCon = sqlCon;
            SqlTran = sqlTran;
            GetDocTypeCountCmd = new OdbcCommand($@"
                SELECT
                    {COL_DOC_TYPE}, COUNT(*) as count
                FROM
                    {TBL_IMAGE_MASTER}
                WHERE
                    {COL_PROJECT_KEY}=? AND
                    {COL_BATCH_KEY}=? AND
                    {COL_FILE_NAME}=? AND
                    {COL_DOC_TYPE}<>''
                GROUP BY
                    {COL_DOC_TYPE}
            ", SqlCon);
            GetDocTypeCountCmd.Transaction = SqlTran;
            GetDocTypeCountCmd.Parameters.Add(COL_PROJECT_KEY, OdbcType.Int);
            GetDocTypeCountCmd.Parameters.Add(COL_BATCH_KEY, OdbcType.Int);
            GetDocTypeCountCmd.Parameters.Add(COL_FILE_NAME, OdbcType.VarChar);
            //GetDocTypeCountCmd.Prepare();
        }

        public List<string> GetDocumentTypes()
        {
            return KeyConfig.Keys.ToList();
        }

        public char GetDocumentTypeShortcut(string documentType)
        {
            if (KeyConfig.TryGetValue(documentType, out char shortcut))
            {
                return shortcut;
            }
            else
            {
                throw new DocumentTypeStoreError($"No shortcut found for document type {documentType}");
            }
        }

        public string GetDocumentTypeForShortcut(char shortcut)
        {
            if (KeyConfigRev.TryGetValue(shortcut, out string docType))
            {
                return docType;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, int> GetDocumentTypeCountsForFile(int projectKey, int batchKey, string fileName)
        {
            var counts = new Dictionary<string, int>();
            GetDocTypeCountCmd.Parameters[COL_PROJECT_KEY].Value = projectKey;
            GetDocTypeCountCmd.Parameters[COL_BATCH_KEY].Value = batchKey;
            GetDocTypeCountCmd.Parameters[COL_FILE_NAME].Value = fileName;
            using (var reader = GetDocTypeCountCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    counts.Add(reader.GetString(reader.GetOrdinal(COL_DOC_TYPE)), reader.GetInt32(reader.GetOrdinal("count")));
                }
            }
            return counts;
        }
    }

    public class DocumentTypeStoreError : Exception
    {
        public DocumentTypeStoreError(string message) : base(message) { }
    }
}
