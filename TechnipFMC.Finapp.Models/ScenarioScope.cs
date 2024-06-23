using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ScenarioScope:BaseEntity
    {
        public int ScenarioScopeID { get; set; }
        public string ScenarioScopeCode { get; set; }
        public string ScenarioScopeName { get; set; }
    }
}
