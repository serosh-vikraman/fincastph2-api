using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class VarianceAnalysisConfig
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
    public class VarianceAnalysisResponseModel
    {
        public string FinancialDataTypeName { get; set; }
        public List<VarianceAnalysisResponse> GridResponse { get; set; }
        public List<VarianceAnalysisResponse> GrandTotal { get; set; }
    }
    public class VarianceAnalysisResponse
    {
        public string DataEntryInterval { get; set; }
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
        public string ClubbingParameterName { get; set; }
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

        public string BaseScenario { get; set; }
        public string CS1 { get; set; }
        public string CS2 { get; set; }
        public string OriginalProjectNo { get; set; }

        public string OriginalProjectName { get; set; }
    }

    public class RepExtractResponse
    {

        public string ScenarioName { get; set; }
        public string SmartViewCode { get; set; }
        public string FinancialDataTypeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string SmartViewName { get; set; }
        public string ProjectEntityCode { get; set; }
        public string ProjectSegmentCode { get; set; }
        public string BUCategoryName { get; set; }
        public string StatutoryCategoryName { get; set; }
        public Decimal? Q1New { get; set; }
        public Decimal? Q2New { get; set; }
        public Decimal? Q3New { get; set; }
        public Decimal? Q4New { get; set; }
        public Int32? Year { get; set; }
    }
    public class CurrencyExchangeData
    {
        public Int32 Year { get; set; }
        public string Quarter { get; set; }
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }

        public Decimal? AverageRate { get; set; }
    }

    public class RepExtractFullResponse
    {
        public string RecordType { get; set; }
        public string SmartViewCode { get; set; }
        public string FinancialDataTypeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string SmartViewName { get; set; }
        public string ProjectEntityCode { get; set; }
        public string ProjectSegmentCode { get; set; }
        public string BUCategoryName { get; set; }
        public string StatutoryCategoryName { get; set; }
        public Decimal? RevenueQ1New { get; set; }
        public Decimal? RevenueQ2New { get; set; }
        public Decimal? RevenueQ3New { get; set; }
        public Decimal? RevenueQ4New { get; set; }

        public Decimal? GMQ1New { get; set; }
        public Decimal? GMQ2New { get; set; }
        public Decimal? GMQ3New { get; set; }
        public Decimal? GMQ4New { get; set; }

    }
    public class RepExtractFullResponseModel
    {
        public List<RepExtractFullResponse> ListRepExtractFullResponse { get; set; }
        public List<CurrencyExchangeData> ListCurrencyExchangeData { get; set; }
    }
    public class ExtractDetailDataModel
    {
        public string RecordType { get; set; }
        public string SmartViewCode { get; set; }
        public string FinancialDataTypeCode { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string SmartViewName { get; set; }
        public string ProjectEntityCode { get; set; }
        public string ProjectSegmentCode { get; set; }
        public string BUCategoryName { get; set; }
        public string StatutoryCategoryName { get; set; }
        public Decimal? FirstYearRevenueQ1New { get; set; }
        public Decimal? FirstYearRevenueQ2New { get; set; }
        public Decimal? FirstYearRevenueQ3New { get; set; }
        public Decimal? FirstYearRevenueQ4New { get; set; }

        public Decimal? FirstYearGMQ1New { get; set; }
        public Decimal? FirstYearGMQ2New { get; set; }
        public Decimal? FirstYearGMQ3New { get; set; }
        public Decimal? FirstYearGMQ4New { get; set; }

        public Decimal? SecondYearRevenueQ1New { get; set; }
        public Decimal? SecondYearRevenueQ2New { get; set; }
        public Decimal? SecondYearRevenueQ3New { get; set; }
        public Decimal? SecondYearRevenueQ4New { get; set; }

        public Decimal? SecondYearGMQ1New { get; set; }
        public Decimal? SecondYearGMQ2New { get; set; }
        public Decimal? SecondYearGMQ3New { get; set; }
        public Decimal? SecondYearGMQ4New { get; set; }
        public Decimal? ThirdYearRevenueTC { get; set; }
        public Decimal? ThirdYearGMTC { get; set; }
        public Decimal? FourthYearRevenueTC { get; set; }
        public Decimal? FourthYearGMTC { get; set; }
        public Decimal? FifthYearRevenueTC { get; set; }
        public Decimal? FifthYearGMTC { get; set; }

    }
    public class ExtractResponseDataModel
    {
        public string ScenarioName { get; set; }
        public List<ExtractHeaderDataModel> ListExtractHeader { get; set; }
        public List<ExtractDetailDataModel> ListExtractDetailDataModel { get; set; }
        public List<CurrencyExchangeData> ListCurrencyExchangeData { get; set; }
    }

    public class ExtractHeaderDataModel
    {
        public string FieldName { get; set; }
        public string HeaderText { get; set; }
    }
    public class ExtractDataDataModel
    {
        public string FieldName { get; set; }
        public string DataValue { get; set; }
    }
    public class ProjectLifeCycleDataModel
    {
        public List<ExtractHeaderDataModel> Header { get; set; }
        public List<ExtractDataDataModel> RVDataValue { get; set; }
        public List<ExtractDataDataModel> GMDataValue { get; set; }
    }
}
