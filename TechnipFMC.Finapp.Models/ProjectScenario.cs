using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ProjectScenario
    {
        public string UploadSessionId { get; set; }
        public string ProjectCode { get; set; }
        public int RowNumber { get; set; }
        public List<ScenarioData> ScenarioDatas { get; set; }
    }
}
