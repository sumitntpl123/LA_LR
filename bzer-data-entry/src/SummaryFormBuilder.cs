using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;
using NovaNet.Utils;
using NovaNet.wfe;

namespace bzer_data_entry
{
    public class SummaryFormBuilder : ISummaryFormBuilder
    {
        public Form CreateSummaryForm(OdbcConnection pCon, Credentials pcrd, eSTATES[] prmState)
        {
            return new frmEntrySummary(pCon, pcrd, prmState);
        }

        public (string projKey, string bundleKey) GetBundleControl()
        {
            return (frmEntrySummary.projKey, frmEntrySummary.bundleKey);
        }

        public DataSet GetSummaryUploadData(OdbcConnection pCon, string projKey, string bundleKey)
        {
            var ds = new DataSet();
            var dt = new DataTable();
            var store = new MetadataStore(pCon);
            var files = store.GetAllFilesFromBatch(int.Parse(projKey), int.Parse(bundleKey));

            dt.Columns.Add("filename");
            dt.Columns.Add(MetadataStore.FIELD_DISTRICT);
            dt.Columns.Add(MetadataStore.FIELD_BLOCK);
            dt.Columns.Add(MetadataStore.FIELD_MOUZA);
            dt.Columns.Add(MetadataStore.FIELD_KHATIAN);
            dt.Columns.Add(MetadataStore.FIELD_BATA_KHATIAN);
            dt.Columns.Add(MetadataStore.FIELD_FILE_NAME);

            foreach (var file in files)
            {
                dt.Rows.Add(new string[] {
                    file.FileName.EnglishText,
                    file.District.Value.EnglishText,
                    file.Block.Value.EnglishText,
                    file.Mouza.Value.EnglishText,
                    file.KhatianNo.EnglishText,
                    file.BataKhatianNo.EnglishText,
                    file.FileName.EnglishText
                });
            }

            ds.Tables.Add(dt);
            return ds;
        }
    }
}
