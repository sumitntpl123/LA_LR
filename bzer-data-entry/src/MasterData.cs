using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace bzer_data_entry
{
    public class MasterDataStore
    {
        private const string FUNC_UUID_TO_BIN = "UNHEX(REPLACE(?, '-', ''))";

        private readonly OdbcConnection SqlCon;

        private readonly OdbcTransaction SqlTran;

        public MasterDataStore(OdbcConnection sqlCon, OdbcTransaction sqlTran = null)
        {
            this.SqlCon = sqlCon;
            this.SqlTran = sqlTran;
        }

        public void AddMasterItem(string fieldName, MasterItem masterItem, Guid? underParent = null)
        {
            try
            {
                if (IsDuplicateMasterItem(fieldName, masterItem.Value, underParent))
                {
                    throw new StoreError($"Duplicate {fieldName} name found.");
                }
                string query = underParent.HasValue ?
                    $@" INSERT INTO metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id) SELECT {FUNC_UUID_TO_BIN}, ?, ?,
                    (SELECT COALESCE(MAX(seq), 0) + 1 FROM metadata_master_cs WHERE field_name=? AND lang=? AND parent_master_id={FUNC_UUID_TO_BIN}), 
                    ?, {FUNC_UUID_TO_BIN}"
                    :
                    $@" INSERT INTO metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
                    SELECT {FUNC_UUID_TO_BIN}, ?, ?, (SELECT COALESCE(MAX(seq), 0) + 1 FROM metadata_master_cs WHERE field_name=? AND lang=?), ?, NULL";
                var command = new OdbcCommand(query, SqlCon);
                command.Transaction = SqlTran;
                if (underParent.HasValue)
                {
                    command.Parameters.Add("@master_id", OdbcType.VarChar).Value = masterItem.MasterID.ToString();
                    command.Parameters.Add("@field_name", OdbcType.VarChar).Value = fieldName;
                    command.Parameters.Add("@lang_1", OdbcType.VarChar);
                    command.Parameters.Add("@field_name", OdbcType.VarChar).Value = fieldName;
                    command.Parameters.Add("@lang_2", OdbcType.VarChar);
                    command.Parameters.Add("@parent_master_id", OdbcType.VarChar).Value = underParent.ToString();
                    command.Parameters.Add("@val", OdbcType.VarChar);
                    command.Parameters.Add("@parent_master_id", OdbcType.VarChar).Value = underParent.ToString();
                }
                else
                {
                    command.Parameters.Add("@master_id", OdbcType.VarChar).Value = masterItem.MasterID.ToString();
                    command.Parameters.Add("@field_name", OdbcType.VarChar).Value = fieldName;
                    command.Parameters.Add("@lang_1", OdbcType.VarChar);
                    command.Parameters.Add("@field_name", OdbcType.VarChar).Value = fieldName;
                    command.Parameters.Add("@lang_2", OdbcType.VarChar);
                    command.Parameters.Add("@val", OdbcType.VarChar);
                }

                command.Parameters["@lang_1"].Value = "Bn";
                command.Parameters["@lang_2"].Value = "Bn";
                command.Parameters["@val"].Value = masterItem.Value.BengaliText;
                if (command.ExecuteNonQuery() != 1)
                {
                    throw new StoreError($"Failed to insert BengaliText for {fieldName}");
                }
                command.Parameters["@lang_1"].Value = "En";
                command.Parameters["@lang_2"].Value = "En";
                command.Parameters["@val"].Value = masterItem.Value.EnglishText;
                if (command.ExecuteNonQuery() != 1)
                {
                    throw new StoreError($"Failed to insert EnglishText for {fieldName}");
                }
                command.Parameters["@lang_1"].Value = "BnPhonetic";
                command.Parameters["@lang_2"].Value = "BnPhonetic";
                command.Parameters["@val"].Value = masterItem.Value.BengaliPhoneticText;
                if (command.ExecuteNonQuery() != 1)
                {
                    throw new StoreError($"Failed to insert BengaliPhoneticText for {fieldName}");
                }

            }
            catch (Exception ex)
            {
                throw new StoreError(ex.Message);
            }
        }

        public void DeleteMasterItem(Guid masterID, List<string> codeFields = null)
        {
            try
            {
                MetadataStore ob = new MetadataStore(SqlCon, SqlTran);
                var childItems = ob.GetMasterList(masterID, codeFields);
                foreach (var childItem in childItems)
                {
                    DeleteMasterItem(childItem.MasterID, codeFields);
                }
                var command = new OdbcCommand($@"
                    DELETE FROM metadata_master_cs
                    WHERE master_id={FUNC_UUID_TO_BIN}
                ", SqlCon);
                command.Transaction = SqlTran;
                command.Parameters.Add("@master_id", OdbcType.VarChar).Value = masterID.ToString();
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new StoreError($"Failed to delete {masterID}");
                }
            }
            catch (Exception ex)
            {
                throw new StoreError(ex.Message);
            }
        }

        public bool IsDuplicateMasterItem(string fieldName, ValueItem value, Guid? underParent = null)
        {
            string query = underParent.HasValue ? $@"
                SELECT COUNT(*)
                FROM metadata_master_cs
                WHERE field_name=? AND (val=? OR val=? OR val=?) AND parent_master_id={FUNC_UUID_TO_BIN} AND
                TRIM(val)!=''" :
                $@"
                SELECT COUNT(*) 
                FROM metadata_master_cs
                WHERE field_name=? AND (val=? OR val=? OR val=?) AND
                TRIM(val)!=''";
            var command = new OdbcCommand(query, SqlCon);
            command.Transaction = SqlTran;
            command.Parameters.Add("field_name", OdbcType.VarChar).Value = fieldName;
            command.Parameters.Add("val", OdbcType.VarChar).Value = value.BengaliText;
            command.Parameters.Add("val", OdbcType.VarChar).Value = value.EnglishText;
            command.Parameters.Add("val", OdbcType.VarChar).Value = value.BengaliPhoneticText;
            if (underParent.HasValue)
            {
                command.Parameters.Add("parent_master_id", OdbcType.VarChar).Value = underParent.ToString();
            }
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }

        public void AddMasterCode(string fieldName, Guid masterId, string code, Guid? underParent = null)
        {
            try
            {
                if (IsDuplicateCodeItem(fieldName, code, underParent))
                {
                    throw new StoreError($"Duplicate {fieldName} code {code} found.");
                }
                var command = new OdbcCommand($@"
                    INSERT INTO master_code_cs (master_id, code) VALUES({FUNC_UUID_TO_BIN},?)
                ", SqlCon);
                command.Transaction = SqlTran;
                command.Parameters.Add("master_id", OdbcType.VarChar).Value = masterId.ToString();
                command.Parameters.Add("code", OdbcType.VarChar).Value = code;
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new StoreError($"Failed to insert code {code}");
                }
            }
            catch (Exception ex)
            {
                throw new StoreError(ex.Message);
            }
        }

        public void DeleteMasterCodeItem(Guid masterID, List<string> codeFields)
        {
            try
            {
                MetadataStore obj = new MetadataStore(SqlCon, SqlTran);
                var childItems = obj.GetMasterList(masterID,
                    new List<string> { MetadataStore.FIELD_DISTRICT, MetadataStore.FIELD_BLOCK, MetadataStore.FIELD_MOUZA });
                foreach (var childItem in childItems)
                {
                    DeleteMasterCodeItem(childItem.MasterID, codeFields);
                }
                var command = new OdbcCommand($@"
                    DELETE FROM master_code_cs WHERE master_id={FUNC_UUID_TO_BIN}
                ", SqlCon);
                command.Transaction = SqlTran;
                command.Parameters.Add("master_id", OdbcType.VarChar).Value = masterID.ToString();
                if (command.ExecuteNonQuery() < 1)
                {
                    throw new FetchError($"Failed to delete code for {masterID} ");
                }
            }
            catch (Exception ex)
            {
                throw new FetchError(ex.Message);
            }
        }

        public bool IsDuplicateCodeItem(string fieldName, string code, Guid? underParent = null)
        {
            string query = underParent.HasValue ?
                $@"SELECT COUNT(*) FROM master_code_cs mc INNER JOIN metadata_master_cs mm ON mc.master_id = mm.master_id WHERE mm.field_name=? AND mc.code=? AND mm.parent_master_id={FUNC_UUID_TO_BIN}"
                :
                $@"SELECT COUNT(*) FROM master_code_cs mc INNER JOIN metadata_master_cs mm ON mc.master_id = mm.master_id WHERE mm.field_name=? AND mc.code = ? ";
            var command = new OdbcCommand(query, SqlCon);
            command.Transaction = SqlTran;
            command.Parameters.Add("field_name", OdbcType.VarChar).Value = fieldName;
            command.Parameters.Add("code", OdbcType.VarChar).Value = code;
            if (underParent.HasValue)
            {
                command.Parameters.Add("parent_master_id", OdbcType.VarChar).Value = underParent.ToString();
            }
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
}
