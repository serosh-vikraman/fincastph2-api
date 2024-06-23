using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ScenarioProjectMapper : BaseEntity
    {
        public int ScenarioID { get; set; }
        public int ProjectID { get; set; }
    }
    public class ProjectScenarioModel : BaseEntity
    {
        public int ScenarioId { get; set; }
        public List<int> ProjectIds { get; set; }
    }
}
