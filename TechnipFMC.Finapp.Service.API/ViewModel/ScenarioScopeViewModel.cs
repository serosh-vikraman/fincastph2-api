using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ScenarioScopeViewModel
    {
        public int ScenarioScopeID { get; set; }
        public string ScenarioScopeCode { get; set; }
        public string ScenarioScopeName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}