using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class VarianceAnalysisConfigViewModel // : BaseEntity 
    {
        public int Year { get; set; }
        public int BaseScenarioId { get; set; }
        public int CompareScenarioAId { get; set; }
        public int CompareScenarioBId { get; set; }
        public string Quarters { get; set; }
        public string GroupLevels { get; set; }
        public string ScenarioDataTypeId { get; set; }
        public string SubTotalRequired { get; set; }
    }
    public class VarianceAnalysisResponseGridModel
    {
        public List<VarianceAnalysisResponseViewModel> GridResponse { get; set; }
        public List<VarianceAnalysisResponseViewModel> GrandTotal { get; set; }
    }
    public class VarianceAnalysisResponseViewModel
    {
        public string RecordType { get; set; }
        public string ProjectNo { get; set; }
        public string GroupingParametersCode { get; set; }
        public string GroupingParametersName { get; set; }        
        public string ProjectName { get; set; }
        public string ContractTypeCode { get; set; }
        public string ProjectEntityCode { get; set; }
        public string ContractStatusCode { get; set; }
        public string ProjectSegmentCode { get; set; }
        public string ManagementCategoryCode { get; set; }

        public Decimal? BaseQ1 { get; set; }
        public Decimal? BaseQ2 { get; set; }
        public Decimal? BaseQ3 { get; set; }
        public Decimal? BaseQ4 { get; set; }
        public Decimal? BaseQ5 { get; set; }
        public Decimal? BaseQ6 { get; set; }
        public Decimal? BaseQ7 { get; set; }
        public Decimal? BaseQ8 { get; set; }
        public Decimal? BaseQ9 { get; set; }
        public Decimal? BaseQ10 { get; set; }
        public Decimal? BaseQ11 { get; set; }
        public Decimal? BaseQ12 { get; set; }
        public Decimal? TotalBase { get; set; }

        public Decimal? CS1Q1 { get; set; }
        public Decimal? CS1Q2 { get; set; }
        public Decimal? CS1Q3 { get; set; }
        public Decimal? CS1Q4 { get; set; }
        public Decimal? CS1Q5 { get; set; }
        public Decimal? CS1Q6 { get; set; }
        public Decimal? CS1Q7 { get; set; }
        public Decimal? CS1Q8 { get; set; }
        public Decimal? CS1Q9 { get; set; }
        public Decimal? CS1Q10 { get; set; }
        public Decimal? CS1Q11 { get; set; }
        public Decimal? CS1Q12 { get; set; }
        public Decimal? TotalCS1 { get; set; }

        public Decimal? CS2Q1 { get; set; }
        public Decimal? CS2Q2 { get; set; }
        public Decimal? CS2Q3 { get; set; }
        public Decimal? CS2Q4 { get; set; }
        public Decimal? CS2Q5 { get; set; }
        public Decimal? CS2Q6 { get; set; }
        public Decimal? CS2Q7 { get; set; }
        public Decimal? CS2Q8 { get; set; }
        public Decimal? CS2Q9 { get; set; }
        public Decimal? CS2Q10 { get; set; }
        public Decimal? CS2Q11 { get; set; }
        public Decimal? CS2Q12 { get; set; }
        public Decimal? TotalCS2 { get; set; }

        public Decimal? Var1 { get; set; }
        public Decimal? Var2 { get; set; }
        public string OriginalProjectNo { get; set; }
        public string OriginalProjectName { get; set; }
        
    }
}