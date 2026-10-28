using NovaNet.Utils;
using Reporting.Export;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reporting
{
    namespace Export
    {
        internal class Image
        {
            public string DocumentType;

            public Image()
            {
                DocumentType = string.Empty;
            }
        }

        internal class File
        {
            public readonly List<string> Images;

            public readonly Dictionary<string, Image> ImageDict;

            public File()
            {
                Images = new List<string>();
                ImageDict = new Dictionary<string, Image>();
            }
        }

        internal class Batch
        {
            public int Key;

            public readonly List<string> Files;

            public readonly Dictionary<string, File> FileDict;

            public Batch()
            {
                Key = -1;
                Files = new List<string>();
                FileDict = new Dictionary<string, File>();
            }
        }

        internal class Project
        {
            public int Key;

            public readonly List<string> Batches;

            public readonly Dictionary<string, Batch> BatchDict;

            public Project()
            {
                Key = -1;
                Batches = new List<string>();
                BatchDict = new Dictionary<string, Batch>();
            }
        }
    }

    public abstract class AbstractExporter : IExporter
    {
        private readonly List<string> Projects;

        private readonly Dictionary<string, Project> ProjectDict;

        public Dictionary<string, List<string>> AllPdfImages { get; set; }
       = new Dictionary<string, List<string>>();

        public Dictionary<string, Dictionary<string, List<string>>> DocTypePdfImages { get; set; }
            = new Dictionary<string, Dictionary<string, List<string>>>();
        public AbstractExporter()
        {
            Projects = new List<string>();
            ProjectDict = new Dictionary<string, Project>();
        }

        public IEnumerable<string> GetProjects()
        {
            return Projects;
        }

        public IEnumerable<string> GetBatches(string underProjectName)
        {
            return ProjectDict[underProjectName].Batches;
        }

        public IEnumerable<string> GetFiles(string underProjectName, string underBatchName)
        {
            return ProjectDict[underProjectName].BatchDict[underBatchName].Files;
        }

        public IEnumerable<string> GetImages(string underProjectName, string underBatchName, string underFileName)
        {
            return ProjectDict[underProjectName].BatchDict[underBatchName].FileDict[underFileName].Images;
        }

        public string GetDocumentType(string underProjectName, string underBatchName, string underFileName, string forImage)
        {
            return ProjectDict[underProjectName].BatchDict[underBatchName].FileDict[underFileName].ImageDict[forImage].DocumentType;
        }

        public void AddImage(string projectName, int projectKey, string batchName, int batchKey, string fileName, string imageFilePath, string docType)
        {
            if (!ProjectDict.ContainsKey(projectName))
            {
                ProjectDict.Add(projectName, new Project());
                ProjectDict[projectName].Key = projectKey;
                Projects.Add(projectName);
            }
            if (!ProjectDict[projectName].BatchDict.ContainsKey(batchName))
            {
                ProjectDict[projectName].BatchDict.Add(batchName, new Batch());
                ProjectDict[projectName].BatchDict[batchName].Key = batchKey;
                ProjectDict[projectName].Batches.Add(batchName);
            }
            if (!ProjectDict[projectName].BatchDict[batchName].FileDict.ContainsKey(fileName))
            {
                ProjectDict[projectName].BatchDict[batchName].FileDict.Add(fileName, new File());
                ProjectDict[projectName].BatchDict[batchName].Files.Add(fileName);
            }
            if (!ProjectDict[projectName].BatchDict[batchName].FileDict[fileName].ImageDict.ContainsKey(imageFilePath))
            {
                ProjectDict[projectName].BatchDict[batchName].FileDict[fileName].ImageDict.Add(imageFilePath, new Image());
                ProjectDict[projectName].BatchDict[batchName].FileDict[fileName].Images.Add(imageFilePath);
            }
            ProjectDict[projectName].BatchDict[batchName].FileDict[fileName].ImageDict[imageFilePath].DocumentType = docType;
        }

        public int GetProjectKey(string projectName)
        {
            return ProjectDict[projectName].Key;
        }

        public int GetBatchKey(string projectName, string batchName)
        {
            return ProjectDict[projectName].BatchDict[batchName].Key;
        }

        public abstract Task<ExportValidationResult> ValidatePostconditions();

        public abstract Task<ExportValidationResult> ValidatePreconditions();

        public abstract ExportTaskResult Export(string exportPath, Credentials cred);

        public abstract void CancelTasks();
    }
}
