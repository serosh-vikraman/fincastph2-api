using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ScenarioFileViewModel
    {
        public string UploadSessionId { get; set; }
        public string ScenarioFileName { get; set; }
        public string FilePath { get; set; }
        public int ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TypeId { get; set; }
        public string CreatedDateTime { get; set; }
        public int Year { get; set; }
        public string Quarter { get; set; }
    }
}