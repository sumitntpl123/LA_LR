using NovaNet.Utils;
using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;

namespace ImageHeaven.Logic
{
    public class UpdateStatusLogic
    {
        OdbcConnection sqlCon;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        MemoryStream stateLog;
        byte[] tmpWrite;
        public UpdateStatusLogic(OdbcConnection prmcon)
        {
            sqlCon = prmcon;
        }
        public bool UpdateStatusByQuery(string tableName, List<(string field, string op, string value)> updateTuplesToSet, List<(string field, string op, string value)> indexPredicates, List<(string field, string op, string value)> pr_withStates, List<(string field, string op, string value)> pr_withoutStates, OdbcTransaction sqlTrans = null)
        {
            string sqlStr = null;
            bool commitBol = true;
            OdbcCommand sqlCmd = new OdbcCommand();

            string BuildConditionGroup(IEnumerable<(string field, string op, string value)> predicates, string separator)
            {
                return predicates.Any() ?
                        $"({string.Join($" {separator} ", predicates.Select(item => item.field + item.op + item.value))})"
                        : string.Empty;
            }
            sqlStr = $@"UPDATE {tableName}
            SET {string.Join(",", updateTuplesToSet.Select(item => item.field + item.op + item.value))}
            WHERE {string.Join(" AND ", new[]
            {
                BuildConditionGroup(indexPredicates,"AND"),
                BuildConditionGroup(pr_withStates,"OR"),
                BuildConditionGroup(pr_withoutStates,"AND")
            }.Where(x => !string.IsNullOrEmpty(x)))}";
            sqlCmd.Connection = sqlCon;
            sqlCmd.Transaction = sqlTrans;
            sqlCmd.CommandText = sqlStr;
            int i = sqlCmd.ExecuteNonQuery();
            if (i > 0)
            {
                commitBol = true;
            }
            else
            {
                commitBol = false;
            }
            return commitBol;
        }

        public bool UpdateStatus(string projectKey, string bundleKey, string policyNumber, Credentials cred, eSTATES[] withStates, eSTATES[] withoutStates, eSTATES newState)
        {
            List<(string field, string op, string value)> predicates1, predicates2, pr_withStates, pr_withoutStates;
            predicates1 = new List<(string field, string op, string value)>()
            {
                ("status", "=", ((int)newState).ToString()),
                ("modified_by", "=", $"'{cred.created_by}'"),
                ("modified_dttm", "=", $"'{cred.created_dttm}'")
            };

            predicates2 = new List<(string field, string op, string value)>()
            {
                ("proj_code", "=", projectKey),
                ("bundle_key", "=", bundleKey),
                ("filename", "=", $"'{policyNumber}'"),
            };

            pr_withStates = new List<(string field, string op, string value)>();
            if (withStates.Length > 0)
            {
                pr_withStates.AddRange(withStates.Select(s => ("status", "=", ((int)s).ToString())));
            }

            pr_withoutStates = new List<(string field, string op, string value)>();
            if (withoutStates.Length > 0)
            {
                pr_withoutStates.AddRange(withoutStates.Select(s => ("status", "<>", ((int)s).ToString())));
            }

            var transMgr = TransactionManager.GetInstance(sqlCon);
            transMgr.OpenTransaction();
            UpdateStatusLogic ob = new UpdateStatusLogic(sqlCon);
            try
            {
                var result = UpdateStatusByQuery("case_file_master", predicates1, predicates2, pr_withStates, pr_withoutStates, transMgr.Txn);
                transMgr.CommitTransaction();
                return result;
            }
            catch (Exception)
            {
                transMgr.AbortTransaction();
                throw;
            }
        }
        public bool UpdateBundleStatus(string projectKey, string bundleKey, eSTATES[] withStates, eSTATES[] withoutStates, eSTATES newState)
        {
            List<(string field, string op, string value)> predicates1, predicates2, pr_withStates, pr_withoutStates;
            predicates1 = new List<(string field, string op, string value)>()
            {
                ("status", "=", ((int)newState).ToString()),
            };
            predicates2 = new List<(string field, string op, string value)>()
            {
                ("proj_code", "=", $"'{projectKey}'"),
                ("bundle_key", "=", bundleKey),
            };

            pr_withStates = new List<(string field, string op, string value)>();
            if (withStates.Length > 0)
            {
                pr_withStates.AddRange(withStates.Select(s => ("status", "=", ((int)s).ToString())));
            }

            pr_withoutStates = new List<(string field, string op, string value)>();
            if (withoutStates.Length > 0)
            {
                pr_withoutStates.AddRange(withoutStates.Select(s => ("status", "<>", ((int)s).ToString())));
            }

            return UpdateStatusByQuery("bundle_master", predicates1, predicates2, pr_withStates, pr_withoutStates);
        }

        private bool UpdateFileStatusByQuery(string filename, List<(string field, string op, string value)> list1, List<(string field, string op, string value)> list2)
        {
            bool retVal = false;
            OdbcCommand sqlCmd = new OdbcCommand();
            string sqlStr = $"UPDATE {filename} SET {string.Join(" AND ", list1.Select(x => x.field + x.op + x.value))} WHERE {string.Join(" AND ", list2.Select(x => x.field + x.op + x.value))}";
            System.Diagnostics.Debug.Print(sqlStr);

            OdbcCommand cmd = new OdbcCommand(sqlStr, sqlCon);
            int val = cmd.ExecuteNonQuery();
            if (val > 0)
            {
                retVal = true;
            }
            return retVal;
        }
        public bool UpdateFileStatus(string projectKey, string bundleKey, eSTATES state)
        {
            List<(string, string, string)> list1 = new List<(string, string, string)>()
            {
                ("status","=",((int)state).ToString())
            };

            List<(string, string, string)> list2 = new List<(string, string, string)>()
            {
                ("proj_code",  "=", projectKey),
                ("bundle_key", "=", bundleKey)
            };

            return UpdateFileStatusByQuery("case_file_master", list1, list2);
        }
        /// <summary>
        /// Update status of all files to new status based on condition of with status and without status
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="bundleKey"></param>
        /// <param name="withStates"></param>
        /// <param name="withoutStates"></param>
        /// <param name="newState"></param>
        /// <returns>if any row updated return true or false</returns>
        public bool UpdateStatus(string projectKey, string bundleKey, eSTATES[] withStates, eSTATES[] withoutStates, eSTATES newState)
        {
            List<(string field, string op, string value)> setQuery, whereQuery, pr_withStates, pr_withoutStates;
            setQuery = new List<(string field, string op, string value)>()
            {
                ("status", "=", ((int)newState).ToString()),
            };

            whereQuery = new List<(string field, string op, string value)>()
            {
                ("proj_code", "=", projectKey),
                ("bundle_key", "=", bundleKey),
            };

            pr_withStates = new List<(string field, string op, string value)>();
            if (withStates.Length > 0)
            {
                pr_withStates.AddRange(withStates.Select(s => ("status", "=", ((int)s).ToString())));
            }

            pr_withoutStates = new List<(string field, string op, string value)>();
            if (withoutStates.Length > 0)
            {
                pr_withoutStates.AddRange(withoutStates.Select(s => ("status", "<>", ((int)s).ToString())));
            }

            var transMgr = TransactionManager.GetInstance(sqlCon);
            transMgr.OpenTransaction();
            try
            {
                var result = UpdateStatusByQuery("case_file_master", setQuery, whereQuery, pr_withStates, pr_withoutStates, transMgr.Txn);
                transMgr.CommitTransaction();
                return result;
            }
            catch (Exception)
            {
                transMgr.AbortTransaction();
                throw;
            }
        }
    }
}
