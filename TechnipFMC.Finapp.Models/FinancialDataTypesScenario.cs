using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class FinancialDataTypesScenario : BaseEntity
    {
        public int ScenarioDataTypeID { get; set; }
        public int ScenarioScopeID { get; set; }
        public int ScenarioTypeID { get; set; }
        public int FinancialDataTypeID { get; set; }
        public string ScenarioScopeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string FinancialDataTypeCode { get; set; }
    }
    public class ScenarioDataPoints : BaseEntity
    {
        public int ScenarioDataTypeID { get; set; }
        public int ScenarioScopeID { get; set; }
        public int ScenarioTypeID { get; set; }
        public string ScenarioScopeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public List<FinancialDataType> FinancialDataType { get; set; }

    }
    public class FinancialScenarioDataPoints : BaseEntity
    {
        public int FinancialDataTypeID { get; set; }
        public string FinancialDataTypeCode { get; set; }
        public List<ScenarioScopeTypes> ScenarioScopeType { get; set; }

    }
    public class ScenarioScopeTypes
    {
        public string ScenarioScopeName { get; set; }
        public string ScenarioScopeCode { get; set; }
        public int ScenarioScopeId { get; set; }
        public bool Budget { get; set; }
        public bool Forecast { get; set; }
        public bool Actuals { get; set; }
    }


    public class FinancialTypesScenario
    {
        public int ScenarioScopeID { get; set; }
        public string ScenarioScopeName { get; set; }
        public string ScenarioScopeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string FinancialDataTypeCode { get; set; }
    }
    public class FinancialDataTypeMapping
    {
        public string Scope { get; set; }
        public string Type { get; set; }
        public string[] Financialdatatype { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }

    }
    public class FinancialDataTypes
    {
        public string FinancialDataTypeCode { get; set; }
        public string FinancialDataTypeName { get; set; }
    }
}
