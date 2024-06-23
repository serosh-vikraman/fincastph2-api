using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class LeagacyYears
    {
        public int KeyData { get; set; }
        public string ValueData { get; set; }
        public bool Active { get; set; }
    }
    public class ScenarioViewModel
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

        public bool Milestone { get; set; }
        public int CreatedBy { get; set; }
        public int CustomerID { get; set; }
        public string Status { get; set; }
        public string ScenarioScopeName { get; set; }
        public string ScenarioTypeName { get; set; }
        public int DepartmentID { get; set; }
        public int ClientID { get; set; }

    }

    public class ScenarioApplicableYearsViewModel
    {
        public int ScenarioID { get; set; }
        public int Year1 { get; set; }
        public int Year2 { get; set; }
        public int Year3 { get; set; }
        public int Year4 { get; set; }
        public int Year5 { get; set; }


    }

    public class ScenarioProjectViewModel
    {
        public int ScenarioID { get; set; }
        public List<ProjectForScenarioViewModel> Projects { get; set; }
        public int CreatedBy { get; set; }

    }
    public class ProjectForScenarioViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }
    public class ProjectScenarioViewModel
    {
        public int ScenarioId { get; set; }
        public List<int> ProjectIds { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ScenarioIDViewModel
    {
        public List<int> ScenarioIds { get; set; }
        public int CreatedBy { get; set; }

    }
    public class ScenarioDDLViewModel
    {
        public int ScenarioID { get; set; }

        public string ScenarioName { get; set; }


    }
}