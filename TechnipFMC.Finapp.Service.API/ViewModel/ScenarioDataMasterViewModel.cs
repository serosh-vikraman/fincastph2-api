using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ScenarioDataMasterViewModel

    {
        public int ScenarioId { get; set; }
        public int ProjectId { get; set; }

        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public int CustomerID { get; set; }
        public List<ScenarioDataViewModel> ScenarioData { get; set; }
    }

    public class ScenarioDataViewModel
    {
        public int ScenarioDataId { get; set; }
        public int ScenarioId { get; set; }
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
        public int ScenarioDataTypeId { get; set; }
        public int Year { get; set; }

        public decimal? Q1New { get; set; }
        public decimal? Q1Variant { get; set; }
        public decimal? Q2New { get; set; }
        public decimal? Q2Variant { get; set; }
        public decimal? Q3New { get; set; }
        public decimal? Q3Variant { get; set; }
        public decimal? Q4New { get; set; }
        public decimal? Q4Variant { get; set; }
        public string UploadSessionID { get; set; }

        //public string CreatedBy { get; set; }
    }
    public class HeaderStructureViewModel
    {
        public string HeaderName { get; set; }
        public string FieldName { get; set; }
        public string Year { get; set; }

    }
    public class DetailedStructureViewModel
    {
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public decimal? CurrentYearRevenueQ1New { get; set; }
        public decimal? CurrentYearRevenueQ1Variant { get; set; }
        public decimal? CurrentYearRevenueQ2New { get; set; }
        public decimal? CurrentYearRevenueQ2Variant { get; set; }
        public decimal? CurrentYearRevenueQ3New { get; set; }
        public decimal? CurrentYearRevenueQ3Variant { get; set; }
        public decimal? CurrentYearRevenueQ4New { get; set; }
        public decimal? CurrentYearRevenueQ4Variant { get; set; }
        public decimal? CurrentYearRevenueCumulative { get; set; }
        public decimal? NextYearRevenueQ1New { get; set; }
        public decimal? NextYearRevenueQ1Variant { get; set; }
        public decimal? NextYearRevenueQ2New { get; set; }
        public decimal? NextYearRevenueQ2Variant { get; set; }
        public decimal? NextYearRevenueQ3New { get; set; }
        public decimal? NextYearRevenueQ3Variant { get; set; }
        public decimal? NextYearRevenueQ4New { get; set; }
        public decimal? NextYearRevenueQ4Variant { get; set; }
        public decimal? NextYearRevenueCumulative { get; set; }

        public decimal? ThirdYearRevenueCumulative { get; set; }
        public decimal? FourthYearRevenueCumulative { get; set; }
        public decimal? FifthYearRevenueCumulative { get; set; }

        public decimal? RevenueGrandTotal { get; set; }

        public decimal? CurrentYearGrossMarginQ1New { get; set; }
        public decimal? CurrentYearGrossMarginQ1Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ2New { get; set; }
        public decimal? CurrentYearGrossMarginQ2Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ3New { get; set; }
        public decimal? CurrentYearGrossMarginQ3Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ4New { get; set; }
        public decimal? CurrentYearGrossMarginQ4Variant { get; set; }
        public decimal? CurrentYearGrossMarginCumulative { get; set; }
        public decimal? NextYearGrossMarginQ1New { get; set; }
        public decimal? NextYearGrossMarginQ1Variant { get; set; }
        public decimal? NextYearGrossMarginQ2New { get; set; }
        public decimal? NextYearGrossMarginQ2Variant { get; set; }
        public decimal? NextYearGrossMarginQ3New { get; set; }
        public decimal? NextYearGrossMarginQ3Variant { get; set; }
        public decimal? NextYearGrossMarginQ4New { get; set; }
        public decimal? NextYearGrossMarginQ4Variant { get; set; }
        public decimal? NextYearGrossMarginCumulative { get; set; }
        public decimal? ThirdYearGrossMarginCumulative { get; set; }
        public decimal? FourthYearGrossMarginCumulative { get; set; }
        public decimal? FifthYearGrossMarginCumulative { get; set; }

        public decimal? GrossMarginGrandTotal { get; set; }

        public decimal? CurrentYearManHoursQ1New { get; set; }
        public decimal? CurrentYearManHoursQ1Variant { get; set; }
        public decimal? CurrentYearManHoursQ2New { get; set; }
        public decimal? CurrentYearManHoursQ2Variant { get; set; }
        public decimal? CurrentYearManHoursQ3New { get; set; }
        public decimal? CurrentYearManHoursQ3Variant { get; set; }
        public decimal? CurrentYearManHoursQ4New { get; set; }
        public decimal? CurrentYearManHoursQ4Variant { get; set; }
        public decimal? CurrentYearManHoursCumulative { get; set; }
        public decimal? NextYearManHoursQ1New { get; set; }
        public decimal? NextYearManHoursQ1Variant { get; set; }
        public decimal? NextYearManHoursQ2New { get; set; }
        public decimal? NextYearManHoursQ2Variant { get; set; }
        public decimal? NextYearManHoursQ3New { get; set; }
        public decimal? NextYearManHoursQ3Variant { get; set; }
        public decimal? NextYearManHoursQ4New { get; set; }
        public decimal? NextYearManHoursQ4Variant { get; set; }
        public decimal? NextYearManHoursCumulative { get; set; }

        public decimal? ThirdYearManHoursCumulative { get; set; }
        public decimal? FourthYearManHoursCumulative { get; set; }
        public decimal? FifthYearManHoursCumulative { get; set; }

        public decimal? ManHoursGrandTotal { get; set; }
        //Cost of sales only for PL-AC current year
        public decimal? CurrentYearCostOfSalesQ1New { get; set; }
        public decimal? CurrentYearCostOfSalesQ1Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ2New { get; set; }
        public decimal? CurrentYearCostOfSalesQ2Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ3New { get; set; }
        public decimal? CurrentYearCostOfSalesQ3Variant { get; set; }

        public decimal? CurrentYearCostOfSalesQ4New { get; set; }
        public decimal? CurrentYearCostOfSalesQ4Variant { get; set; }
        public decimal? CostOfSalesGrandTotal { get; set; }

        //Provision for Future Loss only for PL-AC current year
        public decimal? CurrentYearProvFutureLossQ1New { get; set; }
        public decimal? CurrentYearProvFutureLossQ1Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ2New { get; set; }
        public decimal? CurrentYearProvFutureLossQ2Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ3New { get; set; }
        public decimal? CurrentYearProvFutureLossQ3Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ4New { get; set; }
        public decimal? CurrentYearProvFutureLossQ4Variant { get; set; }
        public decimal? ProvFutureLossGrandTotal { get; set; }

    }
    public class ScenarioDetailsViewModel
    {

        public List<HeaderStructureViewModel> Headers { get; set; }
        public List<DetailedStructureViewModel> Details { get; set; }
    }

}