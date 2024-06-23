using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ScenarioProjectMapperViewModel
    {
        public int ScenarioID { get; set; }
        public int ProjectID { get; set; }

        public int CreatedBy { get; set; }
    }
    public class CurrencyExchangeViewUIModel
    {
        public Int32 Year { get; set; }
        public string Quarter { get; set; }
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }

        public Decimal? AverageRate { get; set; }
    }

    public class RepExtractFullResponseViewModel
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
    public class RepExtractViewModel
    {
        public List<RepExtractFullResponseViewModel> ListRepExtractFullResponse { get; set; }
        public List<CurrencyExchangeViewUIModel> ListCurrencyExchangeData { get; set; }
    }
    public class ExtractDetailViewModel
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
        public Decimal? FirstYearQ2New { get; set; }
        public Decimal? FirstYearRevenueQ3New { get; set; }
        public Decimal? FirstYearRevenueQ4New { get; set; }

        public Decimal? FirstYearGMQ1New { get; set; }
        public Decimal? FirstYearGMQ2New { get; set; }
        public Decimal? FirstYearGMQ3New { get; set; }
        public Decimal? FirstYearGMQ4New { get; set; }

        public Decimal? SecondYearRevenueQ1New { get; set; }
        public Decimal? SecondYearQ2New { get; set; }
        public Decimal? SecondYearRevenueQ3New { get; set; }
        public Decimal? SecondYearRevenueQ4New { get; set; }

        public Decimal? SecondYearGMQ1New { get; set; }
        public Decimal? SecondYearGMQ2New { get; set; }
        public Decimal? SecondYearGMQ3New { get; set; }
        public Decimal? SecondYearGMQ4New { get; set; }
        public Decimal? ThirdYearRevenueTC { get; set; }
        public Decimal? ThirdYearYearGMTC { get; set; }
        public Decimal? FourthYearRevenueTC { get; set; }
        public Decimal? FourthYearYearGMTC { get; set; }
        public Decimal? FifthYearRevenueTC { get; set; }
        public Decimal? FifthYearYearGMTC { get; set; }

    }
    public class ExtractResponseViewModel
    {
        public List<ExtractHeaderViewModel> ListExtractHeader { get; set; }
        public List<ExtractDetailViewModel> ListExtractDetailDataModel { get; set; }
        public List<CurrencyExchangeViewUIModel> ListCurrencyExchangeData { get; set; }
    }

    public class ExtractHeaderViewModel
    {
        public string FieldName { get; set; }
        public string HeaderText { get; set; }
    }
}