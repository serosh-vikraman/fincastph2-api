using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class Scenario : BaseEntity
    {
        public int ScenarioID { get; set; }
        public int FinancialYear { get; set; }
        public int ScenarioSequenceNumber { get; set; }
        public string ScenarioName { get; set; }
        public string Description { get; set; }
        public string Spec { get; set; }
        public bool ScenarioLock { get; set; }
        public string ScenarioScopeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string ScenarioScopeName { get; set; }
        public string ScenarioTypeName { get; set; }

        public bool Milestone { get; set; }
        public string message { get; set; }
        public int DepartmentID { get; set; }
        public int ClientID { get; set; }
    }

    public class ScenarioApplicableYears
    {
        public int ScenarioID { get; set; }
        public int Year1 { get; set; }
        public int Year2 { get; set; }
        public int Year3 { get; set; }
        public int Year4 { get; set; }
        public int Year5 { get; set; }

    }
    public class ScenarioProject : BaseEntity
    {
        public int ScenarioID { get; set; }
        public List<ProjectForScenario> Projects { get; set; }

    }
    public class ProjectForScenario
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }

    }
    public class ScenarioID
    {
        public List<int> ScenarioIds { get; set; }
        public string CreatedBy { get; set; }

    }
    public class ScenarioDDLModel
    {
        public int ScenarioID { get; set; }

        public string ScenarioName { get; set; }


    }
}
