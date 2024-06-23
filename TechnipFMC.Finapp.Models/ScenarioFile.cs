using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ScenarioFile
    {
        public string UploadSessionId { get; set; }
        public string ScenarioFileName { get; set; }

        public string FilePath { get; set; }
        public int ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TypeId { get; set; }
        public string CreatedDateTime { get; set; }
        public int Year { get; set; }
        public string Quarter { get; set; }
    }
    public class ScenarioFileUploadResponse
    {
        public string UploadSessionId { get; set; }
        public int ErrorCount { get; set; }
        public string Message { get; set; }
    }

    public class ScenarioMappedProjects
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        //public string ManualProjectCode { get; set; }
    }
    public class FileUploadLayout
    {
        public string ScenarioName { get; set; }
        public int Year { get; set; }
        public bool YearLock { get; set; }
        public List<LockQuarter> Quarters { get; set; }
    }
}
