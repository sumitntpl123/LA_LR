using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NovaNet
{
    namespace Utils
    {
        internal class Count
        {
            public int Total = 0;

            public int Finished = 0;
        }

        public class ExportTaskStage
        {
            private readonly Dictionary<string, List<Task>> TaskGroups;
            private readonly List<Task> Tasks;
            private readonly Dictionary<string, Count> TaskGroupCounts;
            private readonly Count TaskCounts;

            public ExportTaskStage()
            {
                TaskGroups = new Dictionary<string, List<Task>>();
                Tasks = new List<Task>();
                TaskGroupCounts = new Dictionary<string, Count>();
                TaskCounts = new Count();
            }

            public bool IsEmpty()
            {
                return TaskCounts.Total == TaskCounts.Finished;
            }

            public int GetTotalTaskCount(string groupName = null)
            {
                if (groupName != null)
                {
                    return TaskGroupCounts.ContainsKey(groupName) ? TaskGroupCounts[groupName].Total : 0;
                }
                return TaskCounts.Total;
            }

            public int GetFinishedTaskCount(string groupName = null)
            {
                if (groupName != null)
                {
                    return TaskGroupCounts.ContainsKey(groupName) ? TaskGroupCounts[groupName].Finished : 0;
                }
                return TaskCounts.Finished;
            }

            public void AddTask(string group, Task task)
            {
                if (!TaskGroups.ContainsKey(group))
                {
                    TaskGroups.Add(group, new List<Task>());
                    TaskGroupCounts.Add(group, new Count());
                }
                TaskGroups[group].Add(task);
                Tasks.Add(task);
                ++TaskGroupCounts[group].Total;
                ++TaskCounts.Total;
            }

            public async Task<(string groupName, Task finishedTask)> WhenAny()
            {
                var finishedTask = await Task.WhenAny(Tasks);
                var groupOfFinishedTask = TaskGroups.First(kv => kv.Value.Contains(finishedTask)).Key;
                ++TaskGroupCounts[groupOfFinishedTask].Finished;
                ++TaskCounts.Finished;
                Tasks.Remove(finishedTask);
                TaskGroups[groupOfFinishedTask].Remove(finishedTask);
                var counts = TaskGroupCounts[groupOfFinishedTask];
                return (groupOfFinishedTask, finishedTask);
            }
        }

        public class ExportTaskResult
        {
            private readonly List<ExportTaskStage> Stages;

            public ExportTaskResult()
            {
                Stages = new List<ExportTaskStage>();
            }

            public ExportTaskStage this[int index]
            {
                get
                {
                    return Stages[index];
                }
            }

            public ExportTaskStage AddStage()
            {
                var stage = new ExportTaskStage();
                Stages.Add(stage);
                return stage;
            }

            public ExportTaskStage InsertStage(int index)
            {
                var stage = new ExportTaskStage();
                Stages.Insert(index, stage);
                return stage;
            }

            public ExportTaskStage CurrentStage
            {
                get
                {
                    var pendingStages = Stages.Where(stage => !stage.IsEmpty());
                    if (pendingStages.Count() > 0)
                    {
                        return pendingStages.First();
                    }
                    else if (Stages.Count > 0)
                    {
                        return Stages.Last();
                    }
                    else
                    {
                        return default;
                    }
                }
            }

            public bool IsEmpty()
            {
                return Stages.All(stage => stage.IsEmpty());
            }

            public int GetTotalTaskCount()
            {
                return Stages.Sum(stage => stage.GetTotalTaskCount());
            }

            public int GetFinishedTaskCount()
            {
                return Stages.Sum(stage => stage.GetFinishedTaskCount());
            }

            public async Task<(string groupName, Task finishedTask)> WhenAny()
            {
                return await CurrentStage.WhenAny();
            }
        }

        public class ExportValidationResult
        {
            public List<string> Warnings { get; set; }

            public List<string> Errors { get; set; }

            public ExportValidationResult()
            {
                Warnings = new List<string>();
                Errors = new List<string>();
            }
        }

        public interface IExporter
        {
            IEnumerable<string> GetProjects();

            IEnumerable<string> GetBatches(string underProjectName);

            IEnumerable<string> GetFiles(string underProjectName, string underBatchName);

            IEnumerable<string> GetImages(string underProjectName, string underBatchName, string underFileName);
            Dictionary<string, List<string>> AllPdfImages { get; set; }

            Dictionary<string, Dictionary<string, List<string>>> DocTypePdfImages { get; set; }
            string GetDocumentType(string underProjectName, string underBatchName, string underFileName, string forImage);

            int GetProjectKey(string projectName);

            int GetBatchKey(string projectName, string batchName);

            void AddImage(string projectName, int projectKey, string batchName, int batchKey, string fileName, string imageFilePath, string docType = null);

            Task<ExportValidationResult> ValidatePreconditions();

            Task<ExportValidationResult> ValidatePostconditions();

            ExportTaskResult Export(string exportPath, Credentials cred);



            void CancelTasks();
        }

        public static class ExporterUtils
        {
            public static void WriteCSVRow(StreamWriter fs, params string[] cols)
            {
                // quote if there is whitespace in entry
                cols = cols.Select(col => $"\"{col}\"").ToArray();
                var row = string.Join(",", cols) + "\n";
                fs.Write(row);
            }

            public static CSV ReadCSV(StreamReader fs)
            {
                var csvLs = new List<List<string>>();
                for (var csvLine = fs.ReadLine(); csvLine != null; csvLine = fs.ReadLine())
                {
                    csvLs.Add(csvLine.Split(',').ToList());
                }
                var header = csvLs
                    .First()
                    .Select((Name, Index) => (Name, Index))
                    .ToDictionary(column => column.Name.Trim('"', ' ', '\t'), column => column.Index);
                var csv = new CSV(header);
                foreach (var csvLine in csvLs.Skip(1))
                {
                    var row = new CSV.Row(header);
                    row.Cells.AddRange(csvLine.Select(cell => cell.Trim('"', ' ', '\t')));
                    csv.Rows.Add(row);
                }
                return csv;
            }

            public class CSV
            {
                public class Row
                {
                    private readonly Dictionary<string, int> _Header;

                    public List<string> Cells { get; }

                    public IEnumerable<string> Header
                    {
                        get => _Header.OrderBy(kv => kv.Value).Select(kv => kv.Key);
                    }

                    public Row(Dictionary<string, int> header)
                    {
                        _Header = header;
                        Cells = new List<string>();
                    }

                    public string this[string columnName]
                    {
                        get
                        {
                            return Cells[_Header[columnName]];
                        }

                        set
                        {
                            Cells[_Header[columnName]] = value;
                        }
                    }
                }

                private readonly Dictionary<string, int> _Header;

                public List<Row> Rows { get; }

                public IEnumerable<string> Header
                {
                    get => _Header.OrderBy(kv => kv.Value).Select(kv => kv.Key);
                }

                public CSV(Dictionary<string, int> header)
                {
                    _Header = header;
                    Rows = new List<Row>();
                }

            }
        }
    }
}
