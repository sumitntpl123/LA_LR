using PhoneticBengaliInput;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Threading;

namespace bzer_data_entry
{
    public class Metadata
    {
        public Guid? ID { get; set; }

        public MasterItem District { get; set; }

        public MasterItem Block { get; set; }

        public MasterItem Mouza { get; set; }

        public ValueItem KhatianNo { get; set; }

        public ValueItem BataKhatianNo { get; set; }

        public List<ValueItem> Plots { get; set; }

        public ValueItem subdivision { get; set; }

        public ValueItem PsCode{ get; set; }

        public ValueItem CaseType { get; set; }

        public ValueItem CaseYear { get; set; }

        public List<ValueItem> plottype { get; set; }

        public ValueItem LAPlotType { get; set; }

        public List<ValueItem> pt1 { get; set; }

        public List<ValueItem> pt2 { get; set; }

        public List<ValueItem> ptsr { get; set; }

        public ValueItem purpose { get; set; }

        public ValueItem actno { get; set; }

        public ValueItem runningbody { get; set; }


        public ValueItem case_no { get; set; }

        public ValueItem jlno { get; set; }

        public List<ValueItem> block { get; set; }

        public List<ValueItem> district { get; set; }

        public List<ValueItem> mauza { get; set; }
        public List<ValueItem> BataPlots { get; set; }

        public List<ValueItem> mesurment { get; set; }

        //public List<ValueItem> plotTYpe { get; set; }


        //public List<ValueItem> serial { get; set; }

        //public List<ValueItem> plottype { get; set; }


        public List<ValueItem> SatyadhikariKhatians { get; set; }

        public List<ValueItem> SatyadhikariBataKhatians { get; set; }

        public List<ValueItem> SatyadhikariNames { get; set; }

        public List<ValueItem> SatyadhikariTitles { get; set; }

        public MasterItem ProjaSotterType { get; set; }

        public List<ValueItem> ProjaNames { get; set; }

        public List<ValueItem> ProjaTitles { get; set; }

        public ValueItem FileName { get; set; }

        public ValueItem officecode { get; set; }

        public ValueItem record { get; set; }


        public Metadata()
        {
            District = new MasterItem();
            Block = new MasterItem();
            Mouza = new MasterItem();
            KhatianNo = new ValueItem();
            BataKhatianNo = new ValueItem();
            Plots = new List<ValueItem>();
            BataPlots = new List<ValueItem>();
            mesurment = new List<ValueItem>();
            record = new ValueItem();
            case_no = new ValueItem();
            FileName = new ValueItem();
            PsCode = new ValueItem();
            CaseType = new ValueItem();
            CaseYear = new ValueItem();
            LAPlotType = new ValueItem(); //recently added
            plottype = new List<ValueItem>();
            district = new List<ValueItem>();
            block = new List<ValueItem>();
            mauza = new List<ValueItem>();
            pt1 = new List<ValueItem>();
            pt2 = new List<ValueItem>();
            ptsr = new List<ValueItem>();
            purpose = new ValueItem();
            actno = new ValueItem();
            runningbody = new ValueItem();
            jlno = new ValueItem();

        }

        public void Validate()
        {
            NotEmpty(MetadataStore.FIELD_DISTRICT, District, Language.En);
            //NotEmpty(MetadataStore.FIELD_BLOCK, Block, Language.En);
            //NotEmpty(MetadataStore.FIELD_MOUZA, Mouza, Language.En);
            //NotEmpty(MetadataStore.FIELD_PROJA_ST_TYPE, ProjaSotterType.Value, Language.BnPhonetic);

            //EqualCount(MetadataStore.FIELD_SATYA_NAME, MetadataStore.FIELD_SATYA_TITLE, SatyadhikariNames, SatyadhikariTitles);
            //EqualCount(MetadataStore.FIELD_SATYA_NAME, MetadataStore.FIELD_SATYA_KHATIAN, SatyadhikariNames, SatyadhikariKhatians);
            //LECount(MetadataStore.FIELD_SATYA_BATA_KHATIAN, MetadataStore.FIELD_SATYA_KHATIAN, SatyadhikariBataKhatians, SatyadhikariKhatians);
            //EqualCount(MetadataStore.FIELD_PROJA_NAME, MetadataStore.FIELD_PROJA_TITLE, ProjaNames, ProjaTitles);
        }

        #region validation utility functions
        private static void NotEmpty(string fieldName, ValueItem v, Language? lang = null)
        {
            var langValidations = new Dictionary<Language, bool>() {
                { Language.En, false },
                { Language.Bn, false },
                { Language.BnPhonetic, false },
            };
            langValidations[Language.En] = lang != null && lang.Value.HasFlag(Language.En);
            langValidations[Language.Bn] = lang != null && lang.Value.HasFlag(Language.Bn);
            langValidations[Language.BnPhonetic] = lang != null && lang.Value.HasFlag(Language.BnPhonetic);
            langValidations[Language.En] = !langValidations[Language.En] || !string.IsNullOrWhiteSpace(v.EnglishText);
            langValidations[Language.Bn] = !langValidations[Language.Bn] || !string.IsNullOrWhiteSpace(v.BengaliText);
            langValidations[Language.BnPhonetic] = !langValidations[Language.BnPhonetic] || !string.IsNullOrWhiteSpace(v.BengaliPhoneticText);
            var langStr = "all";
            var emptyInLangs = langValidations.Where(langValidation => !langValidation.Value).Select(langValidation => Enum.GetName(typeof(Language), langValidation.Key));
            if (emptyInLangs.Count() > 0)
            {
                if (emptyInLangs.Count() < langValidations.Count)
                {
                    langStr = string.Join(", ", emptyInLangs);
                }
                throw new DomainError($"{fieldName} is empty for {langStr} language(s)");
            }
        }

        private static void NotEmpty(string fieldName, MasterItem m, Language? lang = null)
        {
            if (m.MasterID != Guid.Empty)
            {
                NotEmpty(fieldName, m.Value, lang);
            }
            else
            {
                throw new DomainError($"{fieldName} doesn't correspond to any valid value from the master data");
            }
        }

        private static void NotEmpty(string fieldName, List<ValueItem> vs, Language? lang = null)
        {
            if (vs.Count != 0)
            {
                foreach (var (valItem, index) in vs.Select((valItem, index) => (valItem, index)))
                {
                    NotEmpty($"{fieldName}-{index}", valItem, lang);
                }
            }
            else
            {
                throw new DomainError($"{fieldName} is an empty list");
            }
        }

        private static void EqualCount<T, U>(string fieldName1, string fieldName2, List<T> l1, List<U> l2)
        {
            if (l1.Count != l2.Count)
            {
                throw new DomainError($"{fieldName1} and {fieldName2} must have equal number of entries");
            }
        }

        private static void LECount<T, U>(string fieldName1, string fieldName2, List<T> l1, List<U> l2)
        {
            if (l1.Count > l2.Count)
            {
                throw new DomainError($"{fieldName1} must have equal or less entries than {fieldName2}");
            }
        }
        #endregion
    }

    #region data type details
    public class ValueItem
    {
        public string EnglishText { get; set; }

        public string BengaliText { get; set; }

        public string BengaliPhoneticText { get; set; }

        public ValueItem()
        {
            EnglishText = "";
            BengaliText = "";
            BengaliPhoneticText = "";
        }

        public static implicit operator ValueItem(LanguageItem item)
        {
            return new ValueItem()
            {
                EnglishText = item.EnglishText,
                BengaliText = item.BengaliText,
                BengaliPhoneticText = item.BengaliPhoneticText
            };
        }

        public static implicit operator LanguageItem(ValueItem v)
        {
            return new LanguageItem()
            {
                EnglishText = v.EnglishText,
                BengaliText = v.BengaliText,
                BengaliPhoneticText = v.BengaliPhoneticText
            };
        }
    }

    public class MasterItem
    {
        public Guid MasterID { get; set; }

        public ValueItem Value { get; set; }

        public MasterItem()
        {
            Value = new ValueItem();
        }
    }
    #endregion

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
    public class MetadataStore
    {
        #region constants
        const string TABLE_METADATA = "metadata_record_cs";

        const string TABLE_METADATA_VALUE = "metadata_value_cs";

        const string TABLE_METADATA_MASTER = "metadata_master_cs";

        const string TABLE_MASTER_CODE = "master_code_cs";

        const string COLUMN_MASTER_ID = "master_id";

        const string COLUMN_PARENT_MASTER_ID = "parent_master_id";

        const string COLUMN_FIELD_NAME = "field_name";

        const string COLUMN_LANG = "lang";

        const string COLUMN_SEQ = "seq";

        const string COLUMN_VAL = "val";

        const string COLUMN_FILE_ID = "file_id";

        const string COLUMN_PROJECT_CODE = "project_code";

        const string COLUMN_BATCH_CODE = "batch_code";

        const string COLUMN_ID_CODE = "code";

        const string FILE_NAME = "filename";

        //const string File_Mesurment = "mesurment";

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

        public const string FIELD_DISTRICT = "district";

        public const string FIELD_PS = "PS";

        public const string FIELD_CaseType = "CaseType";

        public const string FIELD_CaseYear = "Caseyear";

        public const string FIELD_CasePlotType = "plottype";

        public const string FIELD_PT1 = "pt1";

        public const string FIELD_PT2 = "pt2";

        public const string FIELD_PTSR = "ptsr";

        public const string FIELD_Purpose = "purpose";

        public const string FIELD_ACTNO = "actno";

        public const string FIELD_RunningBody = "runningbody";

        public const string FIELD_JLNO = "jlno";

        public const string FIELD_BLOCK = "block";

        public const string FIELD_MOUZA = "mouza";

        public const string FIELD_KHATIAN = "khatian";

        public const string FIELD_PLOT = "plot";

        public const string FIELD_PLOT_TYPE = "plotType";

        public const string FIELD_BATA_PLOT = "bata-plot";

        public const string FIELD_BATA_KHATIAN = "bata-khatian";

        public const string FIELD_SATYA_KHATIAN = "satyadhikari-khatian";

        public const string FIELD_SATYA_BATA_KHATIAN = "satyadhikari-bata-khatian";

        public const string FIELD_SATYA_NAME = "satyadhikari-name";

        public const string FIELD_SATYA_TITLE = "satyadhikari-title";

        public const string FIELD_PROJA_ST_TYPE = "proja-sotter-type";

        public const string FIELD_PROJA_NAME = "proja-name";

        public const string FIELD_PROJA_TITLE = "proja-title";

        public const string FIELD_FILE_NAME = "file-name";

        public const string INVALID_PLOT_BATA_KHATIAN = "";

        public const string FIELD_MESUREMENT = "mesurement";

        public const string FIELD_OFFICE = "office";

        public const string FIELD_PLOT_DISTRICT = "plot-district";

        public const string FIELD_PLOT_BLOCK = "plot-block";
        
        public const string FIELD_PLOT_MOUZA = "plot-mouza";
        
        public const string FIELD_PLOT_TYPE_LIST = "plot-type-list";

        //public const string FIELD_SERIAL = "serial";

        public const string FIELD_RECORDID = "recordid";
        #endregion

        #region prepared statements
        private readonly OdbcCommand DeleteMetadataValueCmd;

        private readonly OdbcCommand DeleteValueCmd;

        private readonly OdbcCommand AddValueCmd;

        private readonly OdbcCommand FetchValueCmd;

        private readonly OdbcCommand AddMasterValueCmd;

        private readonly OdbcCommand FetchMasterIDCmd;

        private readonly OdbcCommand FetchMasterValueCmd;

        private readonly OdbcCommand FetchMasterListCmd;

        private readonly OdbcCommand FetchMasterListUnderParentCmd;

        private readonly OdbcCommand DeleteFileCmd;

        private readonly OdbcCommand AddFileCmd;

        private readonly OdbcCommand FetchFileCmd;

        private readonly OdbcCommand FetchAllFilesFromBatchCmd;

        private readonly OdbcCommand AddFileToImportTableCmd;

        private readonly OdbcCommand AddFileToLegacyMetadataTable;

        private readonly OdbcCommand FindMatchingFileCmd;

        private readonly OdbcCommand FetchMasterCodeCmd;

        private readonly OdbcCommand FetchFileWithFileNameCmd;

        private readonly OdbcCommand UpdateFilenamesCmd;

        private readonly OdbcCommand UpdateImagenamesCmd;
        #endregion

        private readonly OdbcConnection SqlCon;

        private readonly OdbcTransaction SqlTran;

        public MetadataStore(OdbcConnection sqlCon, OdbcTransaction sqlTran = null)
        {
            SqlCon = sqlCon;
            SqlTran = sqlTran;

            #region DeleteValueData
            DeleteMetadataValueCmd = new OdbcCommand($@"
            DELETE FROM {TABLE_METADATA_VALUE}
            WHERE 
                {COLUMN_FILE_ID}={FUNC_UUID_TO_BIN("?")}
            ", SqlCon);
            DeleteMetadataValueCmd.Transaction = SqlTran;
            DeleteMetadataValueCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
            //DeleteMetadataValueCmd.Prepare();
            #endregion

            #region DeleteValue
            DeleteValueCmd = new OdbcCommand($@"
            DELETE FROM {TABLE_METADATA_VALUE} 
            WHERE 
                {COLUMN_FILE_ID}={FUNC_UUID_TO_BIN("?")} AND
                {COLUMN_FIELD_NAME}=?
            ", SqlCon);
            DeleteValueCmd.Transaction = SqlTran;
            DeleteValueCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
            DeleteValueCmd.Parameters.Add(COLUMN_FIELD_NAME, OdbcType.VarChar);
            //DeleteValueCmd.Prepare();
            #endregion

            #region AddValue
            AddValueCmd = new OdbcCommand($@"
            INSERT INTO {TABLE_METADATA_VALUE}
            ({COLUMN_FILE_ID}, {COLUMN_FIELD_NAME}, {COLUMN_LANG}, {COLUMN_SEQ}, {COLUMN_VAL})
            VALUES
            ({FUNC_UUID_TO_BIN("?")},?,?,?,?)
            ", SqlCon);
            AddValueCmd.Transaction = SqlTran;
            AddValueCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
            AddValueCmd.Parameters.Add(COLUMN_FIELD_NAME, OdbcType.VarChar);
            AddValueCmd.Parameters.Add(COLUMN_LANG, OdbcType.VarChar);
            AddValueCmd.Parameters.Add(COLUMN_SEQ, OdbcType.Int);
            AddValueCmd.Parameters.Add(COLUMN_VAL, OdbcType.VarChar);
            //AddValueCmd.Prepare();
            #endregion

            #region FetchValue
            FetchValueCmd = new OdbcCommand($@"
            SELECT  
                {COLUMN_LANG}, 
                {COLUMN_SEQ}, 
                {COLUMN_VAL}
            FROM {TABLE_METADATA_VALUE}
            WHERE
                {COLUMN_FILE_ID}={FUNC_UUID_TO_BIN("?")} AND
                {COLUMN_FIELD_NAME}=?
            ", SqlCon);
            FetchValueCmd.Transaction = SqlTran;
            FetchValueCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
            FetchValueCmd.Parameters.Add(COLUMN_FIELD_NAME, OdbcType.VarChar);
            //FetchValueCmd.Prepare();
            #endregion

            #region AddMasterValue
            AddMasterValueCmd = new OdbcCommand($@"
            INSERT INTO {TABLE_METADATA_VALUE}
            ({COLUMN_FILE_ID}, {COLUMN_FIELD_NAME}, {COLUMN_LANG}, {COLUMN_SEQ}, {COLUMN_MASTER_ID})
            VALUES
            ({FUNC_UUID_TO_BIN("?")},?,?,?,{FUNC_UUID_TO_BIN("?")})
            ", SqlCon);
            AddMasterValueCmd.Transaction = SqlTran;
            AddMasterValueCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
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
            FROM {TABLE_METADATA_VALUE}
            WHERE
                {COLUMN_FILE_ID}={FUNC_UUID_TO_BIN("?")} AND
                {COLUMN_FIELD_NAME}=?
            ", SqlCon);
            FetchMasterIDCmd.Transaction = SqlTran;
            FetchMasterIDCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
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

            #region FetchMasterList
            FetchMasterListCmd = new OdbcCommand($@"
            SELECT 
                {FUNC_BIN_TO_UUID(COLUMN_MASTER_ID)} AS {COLUMN_MASTER_ID},
                {COLUMN_FIELD_NAME},
                {COLUMN_LANG},
                {COLUMN_SEQ},
                {COLUMN_VAL}
            FROM {TABLE_METADATA_MASTER}
            WHERE
                {COLUMN_PARENT_MASTER_ID} IS NULL
            ", SqlCon);
            FetchMasterListCmd.Transaction = SqlTran;
            //FetchMasterListCmd.Prepare();
            #endregion

            #region FetchMasterListUnderParent
            FetchMasterListUnderParentCmd = new OdbcCommand($@"
            SELECT
                {FUNC_BIN_TO_UUID(COLUMN_MASTER_ID)} AS {COLUMN_MASTER_ID},
                {COLUMN_FIELD_NAME},
                {COLUMN_LANG},
                {COLUMN_SEQ},
                {COLUMN_VAL}
            FROM {TABLE_METADATA_MASTER}
            WHERE
                {COLUMN_PARENT_MASTER_ID}={FUNC_UUID_TO_BIN("?")}
            ", SqlCon);
            FetchMasterListUnderParentCmd.Transaction = SqlTran;
            FetchMasterListUnderParentCmd.Parameters.Add(COLUMN_PARENT_MASTER_ID, OdbcType.VarChar);
            //FetchMasterListUnderParentCmd.Prepare();
            #endregion

            #region DeleteFile
            DeleteFileCmd = new OdbcCommand($@"
            DELETE FROM {TABLE_METADATA}
            WHERE
                {COLUMN_FILE_ID}={FUNC_UUID_TO_BIN("?")}
            ", SqlCon);
            DeleteFileCmd.Transaction = SqlTran;
            DeleteFileCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
            //DeleteFileCmd.Prepare();
            #endregion

            #region AddFile
            AddFileCmd = new OdbcCommand($@"
            INSERT INTO {TABLE_METADATA}
            ({COLUMN_FILE_ID}, {COLUMN_PROJECT_CODE}, {COLUMN_BATCH_CODE})
            VALUES
            ({FUNC_UUID_TO_BIN("?")},?,?)
            ", SqlCon);
            AddFileCmd.Transaction = SqlTran;
            AddFileCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
            AddFileCmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int);
            AddFileCmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int);
            //AddFileCmd.Prepare();
            #endregion

            #region FetchFile
            FetchFileCmd = new OdbcCommand($@"
            SELECT
                {COLUMN_PROJECT_CODE},
                {COLUMN_BATCH_CODE}
            FROM 
                {TABLE_METADATA}
            WHERE
                {COLUMN_FILE_ID}={FUNC_UUID_TO_BIN("?")}
            ", SqlCon);
            FetchFileCmd.Transaction = SqlTran;
            FetchFileCmd.Parameters.Add(COLUMN_FILE_ID, OdbcType.VarChar);
            //FetchFileCmd.Prepare();
            #endregion

            #region FetchAllFilesFromBatch
            FetchAllFilesFromBatchCmd = new OdbcCommand($@"
            SELECT
                {FUNC_BIN_TO_UUID($"meta.{COLUMN_FILE_ID}")} AS {COLUMN_FILE_ID}
            FROM
                case_file_master cfm,
                {TABLE_METADATA} meta,
                {TABLE_METADATA_VALUE} val
            WHERE
                meta.{COLUMN_PROJECT_CODE}=? AND
                meta.{COLUMN_BATCH_CODE}=? AND
                val.{COLUMN_FIELD_NAME}='{FIELD_FILE_NAME}' AND
                val.{COLUMN_VAL}=cfm.filename AND
                val.{COLUMN_FILE_ID}=meta.{COLUMN_FILE_ID}
            ", SqlCon);
            FetchAllFilesFromBatchCmd.Transaction = SqlTran;
            FetchAllFilesFromBatchCmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int);
            FetchAllFilesFromBatchCmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int);
            //FetchAllFilesFromBatchCmd.Prepare();
            #endregion

            #region FindMatchingFile
            FindMatchingFileCmd = new OdbcCommand($@"   
             select count(d.{COLUMN_FILE_ID}) from (
             select c.{COLUMN_FILE_ID} from (
             select b.{COLUMN_FILE_ID} from (
             select a.{COLUMN_FILE_ID} from (
             (SELECT {COLUMN_FILE_ID} FROM  {TABLE_METADATA_VALUE} m1  
                     where (m1.{COLUMN_FIELD_NAME} = '{FIELD_KHATIAN}' AND m1.{COLUMN_LANG} = '{LANG_EN}' AND m1.{COLUMN_VAL} = ?))) as a 
                 inner join
                     {TABLE_METADATA_VALUE} mvc2
                 on a.{COLUMN_FILE_ID} = mvc2.{COLUMN_FILE_ID}
                 where mvc2.{COLUMN_FIELD_NAME}='{FIELD_BATA_KHATIAN}' and mvc2.{COLUMN_LANG}='{LANG_EN}' and mvc2.{COLUMN_VAL}=? ) as b
                 inner join 
                     {TABLE_METADATA_VALUE} mvc3
                 on b.{COLUMN_FILE_ID} = mvc3.{COLUMN_FILE_ID}
                     where ({COLUMN_FIELD_NAME}='{FIELD_DISTRICT}' AND {COLUMN_MASTER_ID}={FUNC_UUID_TO_BIN("?")}) ) as c
                 //inner join 
                 //    {TABLE_METADATA_VALUE} mvc4
                 //on c.{COLUMN_FILE_ID} = mvc4.{COLUMN_FILE_ID}
                 //where ({COLUMN_FIELD_NAME}='{FIELD_BLOCK}' and {COLUMN_MASTER_ID}={FUNC_UUID_TO_BIN("?")}) ) as d
                 //inner join 
                 //    {TABLE_METADATA_VALUE} mvc5
                 //on d.{COLUMN_FILE_ID} = mvc5.{COLUMN_FILE_ID}
                 //where ({COLUMN_FIELD_NAME}='{FIELD_MOUZA}' and {COLUMN_MASTER_ID}={FUNC_UUID_TO_BIN("?")})  

            ", SqlCon);
            FindMatchingFileCmd.Transaction = SqlTran;
            FindMatchingFileCmd.Parameters.Add(FIELD_KHATIAN, OdbcType.VarChar);
            FindMatchingFileCmd.Parameters.Add(FIELD_BATA_KHATIAN, OdbcType.VarChar);
            FindMatchingFileCmd.Parameters.Add(FIELD_DISTRICT, OdbcType.VarChar);
            //FindMatchingFileCmd.Parameters.Add(FIELD_BLOCK, OdbcType.VarChar);
            //FindMatchingFileCmd.Parameters.Add(FIELD_MOUZA, OdbcType.VarChar);
            //FindMatchingFileCmd.Prepare();
            #endregion

            #region AddFileToImportTable
            AddFileToImportTableCmd = new OdbcCommand($@"
            INSERT INTO case_file_master
            (
                proj_code,
                bundle_key,
                item_no,
                sl_no,
                case_file_no,
                case_status,
                case_nature,
                case_type,
                case_year,
                filename,
                created_by,
                created_dttm,
                status,
                record_id
            )
            VALUES
            (
                ?,
                ?,
                (SELECT COALESCE(MAX(a.item_no + 1), 1) FROM case_file_master a WHERE a.proj_code = ? AND a.bundle_key = ?),
                (SELECT COALESCE(MAX(a.item_no + 1), 1) FROM case_file_master a WHERE a.proj_code = ? AND a.bundle_key = ?),
                'NA',
                'NA',
                'NA',
                'NA',
                'NA',
                ?,
                ?,
                ?,
                0,
                0
            )
            ", SqlCon);
            AddFileToImportTableCmd.Transaction = SqlTran;
            AddFileToImportTableCmd.Parameters.Add("proj_code", OdbcType.Int);
            AddFileToImportTableCmd.Parameters.Add("bundle_key", OdbcType.Int);
            AddFileToImportTableCmd.Parameters.Add("proj_code", OdbcType.Int);
            AddFileToImportTableCmd.Parameters.Add("bundle_key", OdbcType.Int);
            AddFileToImportTableCmd.Parameters.Add("proj_code", OdbcType.Int);
            AddFileToImportTableCmd.Parameters.Add("bundle_key", OdbcType.Int);
            AddFileToImportTableCmd.Parameters.Add("filename", OdbcType.VarChar);
            AddFileToImportTableCmd.Parameters.Add("created_by", OdbcType.VarChar);
            AddFileToImportTableCmd.Parameters.Add("created_dttm", OdbcType.DateTime);
            //AddFileToImportTableCmd.Prepare();
            #endregion

            #region AddFileToLegacyMetadataTable
            AddFileToLegacyMetadataTable = new OdbcCommand($@"
            INSERT INTO 
                metadata_entry 
            (
                proj_code,
                bundle_key,
                item_no,
                running_serial,
                filename,
                case_file_no,
                case_status,
                case_type,
                case_nature,
                case_year,
                district,
                created_by,
                created_dttm
            )
            VALUES
            (
                ?,
                ?,
                (SELECT COALESCE(MAX(a.item_no + 1), 1) FROM metadata_entry a WHERE a.proj_code = ? AND a.bundle_key = ?),
                (SELECT COALESCE(MAX(a.item_no + 1), 1) FROM metadata_entry a WHERE a.proj_code = ? AND a.bundle_key = ?),
                ?,
                'NA',
                'NA',
                'NA',
                'NA',
                'NA',
                'NA',
                ?,
                ?
            )
            ", SqlCon);
            AddFileToLegacyMetadataTable.Transaction = SqlTran;
            AddFileToLegacyMetadataTable.Parameters.Add("proj_code", OdbcType.Int);
            AddFileToLegacyMetadataTable.Parameters.Add("bundle_key", OdbcType.Int);
            AddFileToLegacyMetadataTable.Parameters.Add("proj_code", OdbcType.Int);
            AddFileToLegacyMetadataTable.Parameters.Add("bundle_key", OdbcType.Int);
            AddFileToLegacyMetadataTable.Parameters.Add("proj_code", OdbcType.Int);
            AddFileToLegacyMetadataTable.Parameters.Add("bundle_key", OdbcType.Int);
            AddFileToLegacyMetadataTable.Parameters.Add("filename", OdbcType.VarChar);
            AddFileToLegacyMetadataTable.Parameters.Add("created_by", OdbcType.VarChar);
            AddFileToLegacyMetadataTable.Parameters.Add("created_dttm", OdbcType.DateTime);
            //AddFileToLegacyMetadataTable.Prepare();
            #endregion

            #region FetchMasterCode
            FetchMasterCodeCmd = new OdbcCommand($@"
            SELECT
                {COLUMN_ID_CODE}
            FROM {TABLE_MASTER_CODE}
            WHERE
                {COLUMN_MASTER_ID}={FUNC_UUID_TO_BIN("?")}
                ", SqlCon);
            FetchMasterCodeCmd.Transaction = SqlTran;
            FetchMasterCodeCmd.Parameters.Add(COLUMN_MASTER_ID, OdbcType.VarChar);
            //FetchMasterCodeCmd.Prepare();
            #endregion

            #region FetchFileWithFileName
            FetchFileWithFileNameCmd = new OdbcCommand($@"
                SELECT 
                    {FUNC_BIN_TO_UUID(COLUMN_FILE_ID)} AS {COLUMN_FILE_ID}
                FROM
                    {TABLE_METADATA_VALUE}
                WHERE
                    {COLUMN_FIELD_NAME}='{FIELD_FILE_NAME}' AND
                    {COLUMN_LANG}='{LANG_EN}' AND
                    {COLUMN_SEQ}=0 AND
                    {COLUMN_VAL}=?
            ", SqlCon);
            FetchFileWithFileNameCmd.Transaction = SqlTran;
            FetchFileWithFileNameCmd.Parameters.Add(COLUMN_VAL, OdbcType.VarChar);
            //FetchFileWithFileNameCmd.Prepare();
            #endregion

            #region UpdateFilenames
            UpdateFilenamesCmd = new OdbcCommand($@"
                    UPDATE 
                        case_file_master cfm
                        JOIN metadata_entry me 
                            ON cfm.proj_code = me.proj_code 
                            AND cfm.bundle_key = me.bundle_key 
                            AND cfm.filename = me.filename
                        JOIN image_master im 
                            ON cfm.proj_code = im.proj_key 
                            AND cfm.bundle_key = im.batch_key
                            AND cfm.filename=im.policy_number
                    SET 
                        cfm.filename = ?,
                        me.filename = ?,
                        im.policy_number = ?
                    WHERE 
                        cfm.proj_code = ?
                        AND cfm.bundle_key =?
                        AND cfm.filename = ?;", SqlCon);
            UpdateFilenamesCmd.Transaction = SqlTran;
            UpdateFilenamesCmd.Parameters.Add("cfm_newfilename", OdbcType.VarChar);
            UpdateFilenamesCmd.Parameters.Add("me_newfilename", OdbcType.VarChar);
            UpdateFilenamesCmd.Parameters.Add("im_policy_number", OdbcType.VarChar);
            UpdateFilenamesCmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int);
            UpdateFilenamesCmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int);
            UpdateFilenamesCmd.Parameters.Add("cfm_oldfilename", OdbcType.VarChar);
            //UpdateFilenamesCmd.Prepare();
            #endregion

            #region UpdateImageNames
            UpdateImagenamesCmd = new OdbcCommand($@"
            UPDATE image_master
                SET page_index_name = CONCAT(
                        ? ,'_',
                        CASE
                            WHEN page_index_name REGEXP '_A'
                            THEN 
                                SUBSTRING_INDEX(page_index_name, '_', -2)                                    
                            ELSE 
                                SUBSTRING_INDEX(page_index_name, '_', -1)                                     
                        END
                    ),
                    Page_name = CONCAT(
                        ? ,'_',
                        CASE
                            WHEN Page_name REGEXP '_A'
                            THEN
                                SUBSTRING_INDEX(Page_name, '_', -2)                                     
                            ELSE 
                                SUBSTRING_INDEX(Page_name, '_', -1)                                     
                        END
                    )
                WHERE 
                    proj_key=? AND 
                    batch_key=? AND 
                    policy_number=? 
            ", SqlCon);
            UpdateImagenamesCmd.Transaction = SqlTran;
            UpdateImagenamesCmd.Parameters.Add("newfilename_1", OdbcType.VarChar);
            UpdateImagenamesCmd.Parameters.Add("newfilename_2", OdbcType.VarChar);
            UpdateImagenamesCmd.Parameters.Add(COLUMN_PROJECT_CODE, OdbcType.Int);
            UpdateImagenamesCmd.Parameters.Add(COLUMN_BATCH_CODE, OdbcType.Int);
            UpdateImagenamesCmd.Parameters.Add("newfilename_3", OdbcType.VarChar);
            //UpdateImagenamesCmd.Prepare();
            #endregion
        }

        public void SaveFile(int projectCode, int batchCode, string creator, Metadata file)
        {
            var fileID = file.ID ?? Guid.NewGuid();
            var filename = file.ID.HasValue ? GetValue(fileID, FIELD_FILE_NAME).Single().EnglishText : default;
            //come here later
            SetValue(fileID, FIELD_DISTRICT, file.District);
            SetValue(fileID, FIELD_BLOCK, file.Block);
            SetValue(fileID, FIELD_MOUZA, file.Mouza);
            SetValue(fileID, FIELD_PLOT_DISTRICT, file.district);
            SetValue(fileID, FIELD_PLOT_BLOCK, file.block);
            SetValue(fileID, FIELD_PLOT_MOUZA, file.mauza);
            SetValue(fileID, FIELD_PLOT_TYPE_LIST, file.plottype);
            //SetValue(fileID, FIELD_CaseType, file.CaseType);
            //SetValue(fileID, FIELD_OFFICE, file.officecode);
            SetValue(fileID, FIELD_CaseYear, file.CaseYear);
            SetValue(fileID, FIELD_PLOT_TYPE, file.LAPlotType);
            SetValue(fileID, FIELD_PT1, file.pt1);
            SetValue(fileID, FIELD_PT2, file.pt1);
            SetValue(fileID, FIELD_PTSR, file.ptsr);
            SetValue(fileID, FIELD_Purpose, file.purpose);
            SetValue(fileID, FIELD_ACTNO, file.actno);
            SetValue(fileID, FIELD_RunningBody, file.runningbody);
            SetValue(fileID, FIELD_JLNO, file.jlno);
            //SetValue(fileID, FIELD_BATA_KHATIAN, file.BataKhatianNo);
            SetValue(fileID, FIELD_PLOT, file.Plots);
            SetValue(fileID, FIELD_BATA_PLOT, file.BataPlots);
            //SetValue(fileID, FIELD_SATYA_KHATIAN, file.SatyadhikariKhatians);
            //SetValue(fileID, FIELD_SATYA_BATA_KHATIAN, file.SatyadhikariBataKhatians);
            //SetValue(fileID, FIELD_SATYA_NAME, file.SatyadhikariNames);
            //SetValue(fileID, FIELD_SATYA_TITLE, file.SatyadhikariTitles);
            //SetValue(fileID, FIELD_PROJA_ST_TYPE, file.ProjaSotterType);
            //SetValue(fileID, FIELD_PROJA_NAME, file.ProjaNames);
            //SetValue(fileID, FIELD_PROJA_TITLE, file.ProjaTitles);
            SetValue(fileID, FIELD_FILE_NAME, file.FileName);
            SetValue(fileID, FIELD_MESUREMENT, file.mesurment);
            //SetValue(fileID,FIELD_RECORDID,file.recordid);
            //SetValue(fileID, FIELD_SERIAL, file.serial);
            if (!file.ID.HasValue)
            {
                AddFileCmd.Parameters[COLUMN_FILE_ID].Value = fileID.ToString("N");
                AddFileCmd.Parameters[COLUMN_PROJECT_CODE].Value = projectCode;
                AddFileCmd.Parameters[COLUMN_BATCH_CODE].Value = batchCode;
                if (AddFileCmd.ExecuteNonQuery() != 1)
                {
                    throw new StoreError($"File wasn't added");
                }

                AddFileToImportTableCmd.Parameters[0].Value = projectCode;
                AddFileToImportTableCmd.Parameters[1].Value = batchCode;
                AddFileToImportTableCmd.Parameters[2].Value = projectCode;
                AddFileToImportTableCmd.Parameters[3].Value = batchCode;
                AddFileToImportTableCmd.Parameters[4].Value = projectCode;
                AddFileToImportTableCmd.Parameters[5].Value = batchCode;
                AddFileToImportTableCmd.Parameters["filename"].Value = file.FileName.EnglishText;
                AddFileToImportTableCmd.Parameters["created_by"].Value = creator;
                AddFileToImportTableCmd.Parameters["created_dttm"].Value = DateTime.Now;
                if (AddFileToImportTableCmd.ExecuteNonQuery() != 1)
                {
                    throw new StoreError($"File wasn't added");
                }

                AddFileToLegacyMetadataTable.Parameters[0].Value = projectCode;
                AddFileToLegacyMetadataTable.Parameters[1].Value = batchCode;
                AddFileToLegacyMetadataTable.Parameters[2].Value = projectCode;
                AddFileToLegacyMetadataTable.Parameters[3].Value = batchCode;
                AddFileToLegacyMetadataTable.Parameters[4].Value = projectCode;
                AddFileToLegacyMetadataTable.Parameters[5].Value = batchCode;
                AddFileToLegacyMetadataTable.Parameters["filename"].Value = file.FileName.EnglishText;
                AddFileToLegacyMetadataTable.Parameters["created_by"].Value = creator;
                AddFileToLegacyMetadataTable.Parameters["created_dttm"].Value = DateTime.Now;
                if (AddFileToLegacyMetadataTable.ExecuteNonQuery() != 1)
                {
                    throw new StoreError($"File wasn't added");
                }
            }
            else
            {
                var newFilename = file.FileName.EnglishText;
                if (newFilename == filename)
                {
                    return;
                }

                UpdateFilenamesCmd.Parameters["cfm_newfilename"].Value = newFilename;
                UpdateFilenamesCmd.Parameters["me_newfilename"].Value = newFilename;
                UpdateFilenamesCmd.Parameters["im_policy_number"].Value = newFilename;
                UpdateFilenamesCmd.Parameters[COLUMN_PROJECT_CODE].Value = projectCode;
                UpdateFilenamesCmd.Parameters[COLUMN_BATCH_CODE].Value = batchCode;
                UpdateFilenamesCmd.Parameters["cfm_oldfilename"].Value = filename;
                if (UpdateFilenamesCmd.ExecuteNonQuery() <= 0)
                {
                    throw new StoreError($"Filename couldn't renamed in database");
                }

                UpdateImagenamesCmd.Parameters["newfilename_1"].Value = newFilename;
                UpdateImagenamesCmd.Parameters["newfilename_2"].Value = newFilename;
                UpdateImagenamesCmd.Parameters[COLUMN_PROJECT_CODE].Value = projectCode;
                UpdateImagenamesCmd.Parameters[COLUMN_BATCH_CODE].Value = batchCode;
                UpdateImagenamesCmd.Parameters["newfilename_3"].Value = newFilename;
                if (UpdateImagenamesCmd.ExecuteNonQuery() <= 0)
                {
                    throw new StoreError("Imagename couldn't renamed in database");
                }
            }
        }
        public int DeleteMetadataRecord(Guid fileId)
        {
            DeleteFileCmd.Parameters[COLUMN_FILE_ID].Value = fileId.ToString();
            return DeleteFileCmd.ExecuteNonQuery();
        }
        public int DeleteMetadataValue(Guid fileId)
        {
            DeleteMetadataValueCmd.Parameters[COLUMN_FILE_ID].Value = fileId.ToString();
            return DeleteMetadataValueCmd.ExecuteNonQuery();
        }
        public (int projectCode, int batchCode, Metadata) GetFile(Guid fileID)
        {
            var files = new Dictionary<Guid, (int projectCode, int batchCode)>();
            FetchFileCmd.Parameters[COLUMN_FILE_ID].Value = fileID.ToString();
            using (var reader = FetchFileCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var projectCode = reader.GetInt32(reader.GetOrdinal(COLUMN_PROJECT_CODE));
                    var batchCode = reader.GetInt32(reader.GetOrdinal(COLUMN_BATCH_CODE));
                    files.Add(fileID, (projectCode, batchCode));
                }
            }
            if (files.Count == 0)
            {
                throw new FetchError($"File {fileID} is not found");
            }
            var file = new Metadata();
            file.ID = fileID;
            //blocked by sumit
            //file.District = GetValue(fileID, FIELD_DISTRICT);
            //file.Block = GetValue(fileID, FIELD_BLOCK);
            //file.Mouza = GetValue(fileID, FIELD_MOUZA);
            //***************************************************************************
            //file.KhatianNo = GetValue(fileID, FIELD_KHATIAN).Single();
            //file.BataKhatianNo = GetValue(fileID, FIELD_BATA_KHATIAN).Single();
            file.Plots = GetValue(fileID, FIELD_PLOT);
            file.district = GetValue(fileID, FIELD_PLOT_DISTRICT);
            file.block = GetValue(fileID, FIELD_PLOT_BLOCK);
            file.mauza = GetValue(fileID, FIELD_PLOT_MOUZA);
            file.plottype = GetValue(fileID, FIELD_PLOT_TYPE_LIST);
            file.BataPlots = GetValue(fileID, FIELD_BATA_PLOT);
            file.mesurment = GetValue(fileID, FIELD_MESUREMENT);
            //file.recordid = GetValue(fileID, FIELD_RECORDID).FirstOrDefault();

            file.FileName = GetValue(fileID, FIELD_FILE_NAME).Single();
            //file.PsCode = GetValue(fileID, FIELD_PS).Single();
            //file.CaseType = GetValue(fileID,FIELD_CaseType).Single();
            file.CaseYear = GetValue(fileID, FIELD_CaseYear).SingleOrDefault() ?? new ValueItem();
            file.LAPlotType = GetValue(fileID, FIELD_PLOT_TYPE).SingleOrDefault() ?? new ValueItem();
            file.pt2 = GetValue(fileID, FIELD_PT1);
            file.pt1 = GetValue(fileID, FIELD_PT2);
            file.purpose = GetValue(fileID, FIELD_Purpose).SingleOrDefault() ?? new ValueItem();
            file.actno = GetValue(fileID, FIELD_ACTNO).SingleOrDefault() ?? new ValueItem();
            file.runningbody = GetValue(fileID, FIELD_RunningBody).SingleOrDefault() ?? new ValueItem();
            file.officecode = GetValue(fileID,FIELD_OFFICE).SingleOrDefault() ?? new ValueItem();
            //file.jlno = GetValue(fileID, FIELD_JLNO).Single();
            file.ptsr = GetValue(fileID, FIELD_PTSR);
            return (files[fileID].projectCode, files[fileID].batchCode, file);
        }

        public List<MasterItem> GetMasterList(Guid? underFieldID = null, List<string> fieldNames = null)
        {
            var items = new Dictionary<string, Dictionary<int, MasterItem>>();
            fieldNames = fieldNames ?? new List<string>();
            OdbcCommand cmd;
            if (underFieldID != null && fieldNames.Count > 0)
            {
                cmd = new OdbcCommand($@"
                SELECT
                    {FUNC_BIN_TO_UUID(COLUMN_MASTER_ID)} AS {COLUMN_MASTER_ID},
                    {COLUMN_FIELD_NAME},
                    {COLUMN_LANG},
                    {COLUMN_SEQ},
                    {COLUMN_VAL}
                FROM {TABLE_METADATA_MASTER}
                WHERE
                    {COLUMN_PARENT_MASTER_ID}={FUNC_UUID_TO_BIN("?")} AND
                    {COLUMN_FIELD_NAME} IN ({string.Join(",", fieldNames.Select(fieldName => $"'{fieldName}'"))})
                ", SqlCon);
                cmd.Transaction = SqlTran;
                cmd.Parameters.Add(COLUMN_PARENT_MASTER_ID, OdbcType.VarChar).Value = underFieldID.ToString();
            }
            else if (underFieldID == null && fieldNames.Count > 0)
            {
                cmd = new OdbcCommand($@"
                SELECT 
                    {FUNC_BIN_TO_UUID(COLUMN_MASTER_ID)} AS {COLUMN_MASTER_ID},
                    {COLUMN_FIELD_NAME},
                    {COLUMN_LANG},
                    {COLUMN_SEQ},
                    {COLUMN_VAL}
                FROM {TABLE_METADATA_MASTER}
                WHERE
                    {COLUMN_PARENT_MASTER_ID} IS NULL AND
                    {COLUMN_FIELD_NAME} IN ({string.Join(",", fieldNames.Select(fieldName => $"'{fieldName}'"))})
                ", SqlCon);
                cmd.Transaction = SqlTran;
            }
            else if (underFieldID != null && fieldNames.Count == 0)
            {
                cmd = FetchMasterListUnderParentCmd;
                cmd.Parameters[COLUMN_PARENT_MASTER_ID].Value = underFieldID.ToString();
            }
            else
            {
                cmd = FetchMasterListCmd;
            }
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var masterID = Guid.ParseExact(reader.GetString(reader.GetOrdinal(COLUMN_MASTER_ID)), "N");
                    var fieldName = reader.GetString(reader.GetOrdinal(COLUMN_FIELD_NAME));
                    var lang = reader.GetString(reader.GetOrdinal(COLUMN_LANG));
                    var seq = reader.GetInt32(reader.GetOrdinal(COLUMN_SEQ));
                    var val = reader.GetString(reader.GetOrdinal(COLUMN_VAL));

                    if (!items.ContainsKey(fieldName))
                    {
                        items.Add(fieldName, new Dictionary<int, MasterItem>());
                    }
                    if (!items[fieldName].ContainsKey(seq))
                    {
                        items[fieldName].Add(seq, new MasterItem());
                    }
                    if (lang == LANG_EN)
                    {
                        items[fieldName][seq].Value.EnglishText = val;
                    }
                    else if (lang == LANG_BN)
                    {
                        items[fieldName][seq].Value.BengaliText = val;
                    }
                    else if (lang == LANG_BN_PHON)
                    {
                        items[fieldName][seq].Value.BengaliPhoneticText = val;
                    }
                    else
                    {
                        throw new FetchError($"Language value {lang} was not recognized");
                    }
                    items[fieldName][seq].MasterID = masterID;
                }
            }
            return items.SelectMany(byFieldName => byFieldName.Value.OrderBy(item => item.Key)).Select(item => item.Value).ToList();
        }

        public string GetMasterCode(Guid masterID)
        {
            FetchMasterCodeCmd.Parameters[COLUMN_MASTER_ID].Value = masterID.ToString();
            var res = FetchMasterCodeCmd.ExecuteScalar();
            return res != null && res != DBNull.Value ? res.ToString() : string.Empty;
        }

        public List<Metadata> GetAllFilesFromBatch(int projectCode, int batchCode)
        {
            var filesUnderbatch = new List<Metadata>();
            FetchAllFilesFromBatchCmd.Parameters[COLUMN_PROJECT_CODE].Value = projectCode;
            FetchAllFilesFromBatchCmd.Parameters[COLUMN_BATCH_CODE].Value = batchCode;
            using (var reader = FetchAllFilesFromBatchCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var fileIDStr = reader.GetString(reader.GetOrdinal(COLUMN_FILE_ID));
                    var fileID = Guid.Parse(fileIDStr);
                    var (foundProjectCode, foundBatchCode, file) = GetFile(fileID);
                    Debug.Assert(projectCode == foundProjectCode);
                    Debug.Assert(batchCode == foundBatchCode);
                    filesUnderbatch.Add(file);
                }
            }
            return filesUnderbatch;
        }

        public Guid? GetFileFromFileName(string fileName)
        {
            var fileIDs = new List<Guid>();
            FetchFileWithFileNameCmd.Parameters[COLUMN_VAL].Value = fileName;
            using (var reader = FetchFileWithFileNameCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var fileID = Guid.ParseExact(reader.GetString(reader.GetOrdinal(COLUMN_FILE_ID)), "N");
                    fileIDs.Add(fileID);
                }
            }
            if (fileIDs.Count == 0)
            {
                return null;
            }
            else if (fileIDs.Count > 1)
            {
                throw new DomainError($"More than one file found against file name {fileName}");
            }
            return fileIDs.First();
        }

        public Dictionary<string, IEnumerable<LanguageItem>> GetFieldValue(List<string> fieldNames, CancellationToken token, string parentType = null, Guid? parentID = null)
        {
            var baseQuery = $@"
            SELECT 
                {FUNC_BIN_TO_UUID(COLUMN_FILE_ID)} AS {COLUMN_FILE_ID},
                {COLUMN_FIELD_NAME},
                {COLUMN_SEQ}, 
                {COLUMN_LANG}, 
                {COLUMN_VAL} 
            FROM 
                {TABLE_METADATA_VALUE}";

            var whereClauseConditions = new List<string>();
            var parameters = new List<OdbcParameter>();

            if (fieldNames.Count > 0)
            {
                whereClauseConditions.Add($"{COLUMN_FIELD_NAME} IN ({string.Join(", ", fieldNames.Select(name => "?"))})");
                parameters.AddRange(fieldNames.Select((name, index) => new OdbcParameter($"{COLUMN_FIELD_NAME}_{index}", OdbcType.VarChar) { Value = name }));
            }

            if (parentType != null && parentID.HasValue)
            {
                whereClauseConditions.Add($@"{COLUMN_FILE_ID} IN (
                    SELECT
                        {COLUMN_FILE_ID}
                    FROM
                        {TABLE_METADATA_VALUE}
                    WHERE
                        {COLUMN_FIELD_NAME}=? AND
                        {COLUMN_LANG}='En' AND
                        {COLUMN_MASTER_ID}={FUNC_UUID_TO_BIN("?")}
                )");

                parameters.Add(new OdbcParameter(COLUMN_FIELD_NAME, OdbcType.VarChar) { Value = parentType });
                parameters.Add(new OdbcParameter(COLUMN_MASTER_ID, OdbcType.VarChar) { Value = parentID.ToString() });
            }

            var whereQuery = string.Join(" AND ", whereClauseConditions);
            baseQuery = string.Join(@"
            WHERE ", new List<string> { baseQuery, whereQuery });

            var rows = new List<(string fileID, string fieldName, int seq, string lang, string val)>();
            var fetchValueUnderParentCmd = new OdbcCommand(baseQuery, SqlCon);
            fetchValueUnderParentCmd.Transaction = SqlTran;
            fetchValueUnderParentCmd.CommandTimeout = 180;

            fetchValueUnderParentCmd.Parameters.AddRange(parameters.ToArray());

            using (var reader = fetchValueUnderParentCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (token.IsCancellationRequested)
                    {
                        return null;
                    }
                    rows.Add((
                        fileID: reader.GetString(reader.GetOrdinal(COLUMN_FILE_ID)),
                        fieldName: reader.GetString(reader.GetOrdinal(COLUMN_FIELD_NAME)),
                        seq: reader.GetInt32(reader.GetOrdinal(COLUMN_SEQ)),
                        lang: reader.GetString(reader.GetOrdinal(COLUMN_LANG)),
                        val: reader.GetString(reader.GetOrdinal(COLUMN_VAL))
                    ));
                }
            }
            var filteredFieldValues = rows.GroupBy(row => row.fieldName.Split('-').ElementAt(1))
                .ToDictionary(
                fieldGroup => fieldGroup.Key,
                fieldGroup => GetUniqueNameTitleSuggestions(fieldGroup
                        .GroupBy(row => row.fileID)
                        .SelectMany(e => e.OrderBy(row => row.seq)
                            .GroupBy(row => row.seq)
                            .SelectMany(seqGroup => seqGroup.GroupBy(item => item.fieldName).Select(seq => new LanguageItem
                            {
                                EnglishText = seq.Where(row => row.lang == LANG_EN).Single().val,
                                BengaliPhoneticText = seq.Where(row => row.lang == LANG_BN_PHON).Single().val,
                                BengaliText = seq.Where(row => row.lang == LANG_BN).Single().val
                            }))
                            .Where((f) => f.BengaliPhoneticText != String.Empty)))); ;
            return filteredFieldValues;
        }
        private IEnumerable<LanguageItem> GetUniqueNameTitleSuggestions(IEnumerable<LanguageItem> uniqueNamesTitles)
        {
            var uniqueValues = uniqueNamesTitles
                                      .Where(item => item.BengaliText.Trim().Length > 0)
                                      .GroupBy(item => item.BengaliText.Trim())
                                      .Select(group => group.First())
                                      .Select(item => new LanguageItem
                                      {
                                          BengaliText = item.BengaliText.Trim(),
                                          EnglishText = item.EnglishText,
                                          BengaliPhoneticText = item.BengaliPhoneticText.Trim()
                                      });
            return uniqueValues;
        }
        public void Validate(Metadata file)
        {
            if (!HasMatchingFileName(file))
            {
                //ValidateExecutionQuery(file);
            }
        }

        private bool HasMatchingFileName(Metadata file)
        {
            if (!file.ID.HasValue)
            {
                return false;
            }
            var existingFileName = GetValue(file.ID.Value, FIELD_FILE_NAME)
                                    .Single()
                                    .EnglishText;
            return existingFileName == file.FileName.EnglishText;
        }

        public void ValidateExecutionQuery(Metadata file)
        {
            var matchingFileCount = 0;
            FindMatchingFileCmd.Parameters[FIELD_KHATIAN].Value = file.KhatianNo.EnglishText;
            FindMatchingFileCmd.Parameters[FIELD_BATA_KHATIAN].Value = file.BataKhatianNo.EnglishText;
            FindMatchingFileCmd.Parameters[FIELD_DISTRICT].Value = file.District.MasterID.ToString();
            //FindMatchingFileCmd.Parameters[FIELD_BLOCK].Value = file.Block.MasterID.ToString();
            //FindMatchingFileCmd.Parameters[FIELD_MOUZA].Value = file.Mouza.MasterID.ToString();
            using (var reader = FindMatchingFileCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    matchingFileCount += reader.GetInt32(0);
                }
            }
            if (matchingFileCount > 0)
            {
                var bataKhPresent = file.BataKhatianNo.EnglishText == MetadataStore.INVALID_PLOT_BATA_KHATIAN ? "" : $"/{file.BataKhatianNo.EnglishText}";
                throw new DomainError($"Duplicate file " +
                    $"{file.District.Value.BengaliText}/" +
                    $"{file.Block.Value.BengaliText}/" +
                    $"{file.Mouza.Value.BengaliText}/" +
                    $"{file.KhatianNo.EnglishText}{bataKhPresent}");
            }
        }

        public string GenerateBatchName(Guid districtID, string volumeName, string year)
        {
            string districtCode = GetMasterCode(districtID);
            string sanitizedVolumeName = volumeName.Replace("-", "_");
            string batchName = $"{districtCode.PadLeft(2, '0')}_{sanitizedVolumeName}_{year}";
            return batchName;
        }

        public string GenerateFileName(Guid districtID, Guid blockID, Guid mouzaID, string _fileName,string year)
        {
            string districtCode = GetMasterCode(districtID);
            string blockCode = GetMasterCode(blockID);
            string mouzaCode = GetMasterCode(mouzaID);;
            string fileName = $"{districtCode.PadLeft(2, '0')}_{_fileName}_{year}"; 
            return fileName;
        }

        #region private methods
        private void SetValue(Guid fileID, string fieldName, ValueItem fieldValue)
        {
            DeleteValue(fileID, fieldName);
            AddValue(fileID, fieldName, 0, fieldValue);
        }

        private void SetValue(Guid fileID, string fieldName, List<ValueItem> fieldValue)
        {
            DeleteValue(fileID, fieldName);
            for (int index = 0; index < fieldValue.Count; ++index)
            {
                AddValue(fileID, fieldName, index, fieldValue[index]);
            };
        }

        private void SetValue(Guid fileID, string fieldName, MasterItem fieldValue)
        {
            DeleteValue(fileID, fieldName);
            AddValue(fileID, fieldName, 0, fieldValue);
        }

        private List<ValueItem> GetValue(Guid fileID, string fieldName)
        {
            var items = new Dictionary<int, ValueItem>();
            FetchValueCmd.Parameters[COLUMN_FILE_ID].Value = fileID.ToString();
            FetchValueCmd.Parameters[COLUMN_FIELD_NAME].Value = fieldName;
            using (var reader = FetchValueCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var lang = reader.GetString(reader.GetOrdinal(COLUMN_LANG));
                    var seq = reader.GetInt32(reader.GetOrdinal(COLUMN_SEQ));
                    var val = reader.GetString(reader.GetOrdinal(COLUMN_VAL));

                    if (!items.ContainsKey(seq))
                    {
                        items.Add(seq, new ValueItem());
                    }
                    if (lang == LANG_EN)
                    {
                        items[seq].EnglishText = val;
                    }
                    else if (lang == LANG_BN)
                    {
                        items[seq].BengaliText = val;
                    }
                    else if (lang == LANG_BN_PHON)
                    {
                        items[seq].BengaliPhoneticText = val;
                    }
                    else
                    {
                        throw new FetchError($"Language value {lang} was not recognized");
                    }
                }
            }
            return items.OrderBy(item => item.Key).Select(item => item.Value).ToList();
        }

        private MasterItem GetMasterValue(Guid fileID, string fieldName)
        {
            var masterID = GetMasterID(fileID, fieldName);
            var (master, field) = GetMasterValue(masterID);
            if (field != fieldName)
            {
                throw new FetchError($"Field name {fieldName} for {masterID} doesn't match with found field name {field}");
            }
            return master;
        }
        // *****************************************************************************************
        // ** TEMPORARY FUNCTION: THIS IS A TARGETED HACK FOR MIGRATION ONLY. **
        // ** THIS FUNCTION SHOULD BE REMOVED IN A LATER MILESTONE AS THIS FEATURE WILL NO LONGER **
        // *****************************************************************************************
        private MasterItem GetProjaSotterTypeValue(Guid fileID)
        {
            var masterID = GetProjaSotterTypeID(fileID);
            if (masterID == Guid.Empty)
            {
                return null;
            }
            var (master, field) = GetMasterValue(masterID);
            return master;
        }
        // *****************************************************************************************
        // ** TEMPORARY FUNCTION: THIS IS A TARGETED HACK FOR MIGRATION ONLY. **
        // ** THIS FUNCTION SHOULD BE REMOVED IN A LATER MILESTONE AS THIS FEATURE WILL NO LONGER **
        // *****************************************************************************************
        private Guid GetProjaSotterTypeID(Guid fileID)
        {
            var items = new Dictionary<int, Guid>();
            FetchMasterIDCmd.Parameters[COLUMN_FILE_ID].Value = fileID.ToString();
            FetchMasterIDCmd.Parameters[COLUMN_FIELD_NAME].Value = FIELD_PROJA_ST_TYPE;
            using (var reader = FetchMasterIDCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Guid? masterIDValue = reader.IsDBNull(reader.GetOrdinal(COLUMN_MASTER_ID))
                                  ? (Guid?)null
                                  : Guid.ParseExact(reader.GetString(reader.GetOrdinal(COLUMN_MASTER_ID)), "N");

                    var seq = reader.GetInt32(reader.GetOrdinal(COLUMN_SEQ));

                    if (masterIDValue.HasValue)
                    {
                        if (!items.ContainsKey(seq))
                        {
                            items.Add(seq, masterIDValue.Value);
                        }
                        items[seq] = masterIDValue.Value;
                    }
                }
            }
            if (items.Count == 0)
            {
                return Guid.Empty;
            }
            else if (items.Count != 1)
            {
                throw new FetchError($"Multiple items found against {FIELD_PROJA_ST_TYPE}");
            }
            return items.Single().Value;
        }
        private Guid GetMasterID(Guid fileID, string fieldName)
        {
            var items = new Dictionary<int, Guid>();
            FetchMasterIDCmd.Parameters[COLUMN_FILE_ID].Value = fileID.ToString();
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

        private int DeleteValue(Guid fileID, string fieldName)
        {
            DeleteValueCmd.Parameters[COLUMN_FILE_ID].Value = fileID.ToString();
            DeleteValueCmd.Parameters[COLUMN_FIELD_NAME].Value = fieldName;
            return DeleteValueCmd.ExecuteNonQuery();
        }

        private int AddValue(Guid fileID, string fieldName, int index, ValueItem fieldValue)
        {
            int insertCount = 0;
            AddValueCmd.Parameters[COLUMN_FILE_ID].Value = fileID.ToString();
            AddValueCmd.Parameters[COLUMN_FIELD_NAME].Value = fieldName;
            AddValueCmd.Parameters[COLUMN_SEQ].Value = index;

            AddValueCmd.Parameters[COLUMN_LANG].Value = LANG_EN;
            AddValueCmd.Parameters[COLUMN_VAL].Value = fieldValue.EnglishText;
            insertCount += AddValueCmd.ExecuteNonQuery();

            AddValueCmd.Parameters[COLUMN_LANG].Value = LANG_BN;
            AddValueCmd.Parameters[COLUMN_VAL].Value = fieldValue.BengaliText;
            insertCount += AddValueCmd.ExecuteNonQuery();

            AddValueCmd.Parameters[COLUMN_LANG].Value = LANG_BN_PHON;
            AddValueCmd.Parameters[COLUMN_VAL].Value = fieldValue.BengaliPhoneticText;
            insertCount += AddValueCmd.ExecuteNonQuery();

            if (insertCount != 3)
            {
                throw new StoreError($"{fieldName} value was not successfully stored");
            }

            return insertCount;
        }

        private int AddValue(Guid fileID, string fieldName, int index, MasterItem fieldValue)
        {
            AddMasterValueCmd.Parameters[COLUMN_FILE_ID].Value = fileID.ToString();
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

    public class StoreError : Exception
    {
        public StoreError(string message) : base(message) { }
    }

    public class FetchError : Exception
    {
        public FetchError(string message) : base(message) { }
    }

    public class DomainError : Exception
    {
        public DomainError(string message) : base(message) { }
    }
}