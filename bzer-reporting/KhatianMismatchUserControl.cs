using LItems;
using NovaNet.wfe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Windows.Forms;

namespace Reporting
{
    public partial class KhatianMismatchUserControl : UserControl
    {
        public KhatianMismatchUserControl()
        {
            InitializeComponent();
        }

        public void GetKhatianValidationResult(OdbcConnection sqlCon, string projectKey, string bundleKey, string bundleName)
        {
            try
            {
                var file = new wfePolicy(sqlCon, new CtrlPolicy(int.Parse(projectKey), int.Parse(bundleKey), ""));
                var ds = file.GetFileList(new eSTATES[] { });

                List<string> fileNames = new List<string>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    fileNames = ds.Tables[0].Rows
                        .Cast<DataRow>()
                        .Where(row => row["filename"] != DBNull.Value)
                        .Select(row => row["filename"].ToString())
                        .ToList();
                }

                // Extract Khatian numbers (e.g., "1", "2", "3", "4")
                var khatians = fileNames
                    .Where(name => name.Count(c => c == '_') == 1)
                    .Select(name =>
                    {
                        int underscoreIndex = name.IndexOf('_');
                        return name.Substring(underscoreIndex + 1);
                    })
                    .Select(int.Parse)
                    .ToList();

                // Extract Bata Khatian numbers (e.g., "3_1", "3_2", "4_1")
                var khatianWithBataNumbers = fileNames
                    .Where(name => name.Count(c => c == '_') == 2)
                    .Select(name =>
                    {
                        var indexKhatianWithBata = name.IndexOf('_') + 1;
                        var khatianWithBata = name.Substring(indexKhatianWithBata);
                        return khatianWithBata;
                    })
                    .ToList();


                int startIndex = bundleName.IndexOf("_") + 1;
                var vol = bundleName.Substring(startIndex);
                var arrStr = vol.Split('_');
                if (arrStr.Length != 2)
                {
                    MessageBox.Show("Invalid bundle name format.");
                    return;
                }
                var (missingKhatians, extraKhatians, sortedkhatianWithBataNumbers) = AnalyzeKhatianData(khatians, khatianWithBataNumbers, arrStr);
                var maxCount = Math.Max(missingKhatians.Count, Math.Max(extraKhatians.Count, khatianWithBataNumbers.Count));
                listViewMissingKhatian.Items.Clear();
                listViewMissingKhatian.Items.AddRange(
                    Enumerable.Range(0, maxCount)
                    .Select(i => new ListViewItem(new[] {
                    i < missingKhatians.Count ? missingKhatians[i].ToString() : string.Empty,
                    i < extraKhatians.Count ? extraKhatians[i].ToString() : string.Empty,
                    i < sortedkhatianWithBataNumbers.Count ? sortedkhatianWithBataNumbers[i] : string.Empty
                    }))
                    .ToArray()
                );
                statusKhatianCount.Text = $"Count:- Missing: {missingKhatians.Count}, OutOfRange: {extraKhatians.Count}, Bata: {sortedkhatianWithBataNumbers.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khatian validation error occurred: " + ex.Message);
            }
        }
        public (List<int> missingKhatians, List<int> extraKhatians, List<string> sortedkhatianWithBataNumbers)
            AnalyzeKhatianData(List<int> khatians, List<string> khatianWithBataNumbers, string[] volRange)
        {
            try
            {
                int minKhatian = Convert.ToInt32(volRange[0]);
                int maxKhatian = Convert.ToInt32(volRange[1]);
                int count = (maxKhatian + 1) - minKhatian;

                var missingKhatians = Enumerable.Range(minKhatian, count)
                    .Except(khatians)
                    .ToList();

                var extraKhatians = khatians
                    .Where(v => v < minKhatian || v > maxKhatian)
                    .ToList();

                var sortedkhatianWithBataNumbers = khatianWithBataNumbers.OrderBy(item =>
                {
                    return int.Parse(item.Substring(0, item.IndexOf('_')));
                })
                 .ThenBy(item =>
                 {
                     return int.Parse(item.Substring(item.IndexOf('_') + 1));
                 })
                 .ToList();

                return (missingKhatians, extraKhatians, sortedkhatianWithBataNumbers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
