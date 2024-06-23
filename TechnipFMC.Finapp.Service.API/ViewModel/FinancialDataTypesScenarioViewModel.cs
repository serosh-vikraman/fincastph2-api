using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class FinancialDataTypesScenarioViewModel
    {
        public int ScenarioDataTypeID { get; set; }
        public int ScenarioScopeID { get; set; }
        public int ScenarioTypeID { get; set; }
        public int FinancialDataTypeID { get; set; }
        public string ScenarioScopeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string FinancialDataTypeCode { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ScenarioDataPointsViewModel 
    {
        public int ScenarioDataTypeID { get; set; }
        public int ScenarioScopeID { get; set; }
        public int ScenarioTypeID { get; set; }
        public string ScenarioScopeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public List<FinancialDataTypeViewModel> FinancialDataType { get; set; }

    }
    public class FinancialScenarioDataPointsViewModel 
    {
        public int FinancialDataTypeID { get; set; }
        public string FinancialDataTypeCode { get; set; }
        public List<ScenarioScopeTypesViewModel> ScenarioScopeTypeViewModel { get; set; }

    }
    public class ScenarioScopeTypesViewModel
    {
        public string ScenarioScopeName { get; set; }
        public string ScenarioScopeCode { get; set; }
        public int ScenarioScopeId { get; set; }
        public bool Budget { get; set; }
        public bool Forecast { get; set; }
        public bool Actuals { get; set; }
    }
}