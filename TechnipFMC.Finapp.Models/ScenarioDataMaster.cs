using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ScenarioDataMaster : BaseEntity

    {
        public int ScenarioId { get; set; }
        public int ProjectId { get; set; }

        public string Comments { get; set; }

        public List<ScenarioData> ScenarioData { get; set; }
    }
    public class ScenarioData 
    {        
        public int ScenarioId { get; set; }
        public int ProjectId { get; set; }
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
        public decimal? Q5New { get; set; }
        public decimal? Q5Variant { get; set; }
        public decimal? Q6New { get; set; }
        public decimal? Q6Variant { get; set; }
        public decimal? Q7New { get; set; }
        public decimal? Q7Variant { get; set; }
        public decimal? Q8New { get; set; }
        public decimal? Q8Variant { get; set; }
        public decimal? Q9New { get; set; }
        public decimal? Q9Variant { get; set; }
        public decimal? Q10New { get; set; }
        public decimal? Q10Variant { get; set; }
        public decimal? Q11New { get; set; }
        public decimal? Q11Variant { get; set; }
        public decimal? Q12New { get; set; }
        public decimal? Q12Variant { get; set; }
        public string UploadSessionIdQ1 { get; set; }
        public string UploadSessionIdQ2 { get; set; }
        public string UploadSessionIdQ3 { get; set; }
        public string UploadSessionIdQ4 { get; set; }
        public int RowNumber { get; set; }
    }
    //public class QuarterlyData : BaseEntity
    //{
        

    // }
    public class ScenarioDataList
    {
        public int ScenarioId { get; set; }
        public int ProjectId { get; set; }
        public string DataEntryInterval { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Q1New { get; set; }
        public string Q1Variant { get; set; }
        public string Q2New { get; set; }
        public string Q2Variant { get; set; }
        public string Q3New { get; set; }
        public string Q3Variant { get; set; }
        public string Q4New { get; set; }
        public string Q4Variant { get; set; }
        public string Q5New { get; set; }
        public string Q5Variant { get; set; }
        public string Q6New { get; set; }
        public string Q6Variant { get; set; }
        public string Q7New { get; set; }
        public string Q7Variant { get; set; }
        public string Q8New { get; set; }
        public string Q8Variant { get; set; }
        public string Q9New { get; set; }
        public string Q9Variant { get; set; }
        public string Q10New { get; set; }
        public string Q10Variant { get; set; }
        public string Q11New { get; set; }
        public string Q11Variant { get; set; }
        public string Q12New { get; set; }
        public string Q12Variant { get; set; }


        public int ScenarioDataTypeId { get; set; }
        public int Year { get; set; }
        public string FinancialDataTypeName { get; set; }
        public bool Q1Lock { get; set; }
        public bool Q2Lock { get; set; }
        public bool Q3Lock { get; set; }
        public bool Q4Lock { get; set; }
        public bool Q5Lock { get; set; }
        public bool Q6Lock { get; set; }
        public bool Q7Lock { get; set; }
        public bool Q8Lock { get; set; }
        public bool Q9Lock { get; set; }
        public bool Q10Lock { get; set; }
        public bool Q11Lock { get; set; }
        public bool Q12Lock { get; set; }
    }

    public class YearlyScenarioData
    {
        public int Year { get; set; }
        public bool YearLock { get; set; }
        public bool QuarterApplicable { get; set; }
        public string Comments { get; set; }
        public List<ScenarioDataList> ScenarioDataLists { get; set; }
        public bool Editable { get; set; }

    }

    public class ScenarioLayout
    {
        public string Year { get; set; }
        public bool YearLock { get; set; }
        public bool QuarterApplicable { get; set; }
        public List<QuartersLayOut> Quarters { get; set; }
        public List<FinancialDataTypeMaster> FinancialDataTypes { get; set; }
        public List<string> QuarterDivs { get; set; }
        public string DataEntryInterval { get; set; }
        
    }
    public class ScenarioDataModel
    {
        public List<ScenarioData> ScenarioDatas { get; set; }
    }
    public class HeaderStructure
    {
        public string HeaderName { get; set; }
        public string FieldName { get; set; }
        public string Year { get; set; }

    }
    public class DetailedStructure
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
        public decimal? CurrentYearRevenueQ5New { get; set; }
        public decimal? CurrentYearRevenueQ5Variant { get; set; }
        public decimal? CurrentYearRevenueQ6New { get; set; }
        public decimal? CurrentYearRevenueQ6Variant { get; set; }
        public decimal? CurrentYearRevenueQ7New { get; set; }
        public decimal? CurrentYearRevenueQ7Variant { get; set; }
        public decimal? CurrentYearRevenueQ8New { get; set; }
        public decimal? CurrentYearRevenueQ8Variant { get; set; }
        public decimal? CurrentYearRevenueQ9New { get; set; }
        public decimal? CurrentYearRevenueQ9Variant { get; set; }
        public decimal? CurrentYearRevenueQ10New { get; set; }
        public decimal? CurrentYearRevenueQ10Variant { get; set; }
        public decimal? CurrentYearRevenueQ11New { get; set; }
        public decimal? CurrentYearRevenueQ11Variant { get; set; }
        public decimal? CurrentYearRevenueQ12New { get; set; }
        public decimal? CurrentYearRevenueQ12Variant { get; set; }
        public decimal? CurrentYearRevenueCumulative { get; set; }
        public decimal? NextYearRevenueQ1New { get; set; }
        public decimal? NextYearRevenueQ1Variant { get; set; }
        public decimal? NextYearRevenueQ2New { get; set; }
        public decimal? NextYearRevenueQ2Variant { get; set; }
        public decimal? NextYearRevenueQ3New { get; set; }
        public decimal? NextYearRevenueQ3Variant { get; set; }
        public decimal? NextYearRevenueQ4New { get; set; }
        public decimal? NextYearRevenueQ4Variant { get; set; }
        public decimal? NextYearRevenueQ5New { get; set; }
        public decimal? NextYearRevenueQ5Variant { get; set; }
        public decimal? NextYearRevenueQ6New { get; set; }
        public decimal? NextYearRevenueQ6Variant { get; set; }
        public decimal? NextYearRevenueQ7New { get; set; }
        public decimal? NextYearRevenueQ7Variant { get; set; }
        public decimal? NextYearRevenueQ8New { get; set; }
        public decimal? NextYearRevenueQ8Variant { get; set; }
        public decimal? NextYearRevenueQ9New { get; set; }
        public decimal? NextYearRevenueQ9Variant { get; set; }
        public decimal? NextYearRevenueQ10New { get; set; }
        public decimal? NextYearRevenueQ10Variant { get; set; }
        public decimal? NextYearRevenueQ11New { get; set; }
        public decimal? NextYearRevenueQ11Variant { get; set; }
        public decimal? NextYearRevenueQ12New { get; set; }
        public decimal? NextYearRevenueQ12Variant { get; set; }
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
        public decimal? CurrentYearGrossMarginQ5New { get; set; }
        public decimal? CurrentYearGrossMarginQ5Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ6New { get; set; }
        public decimal? CurrentYearGrossMarginQ6Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ7New { get; set; }
        public decimal? CurrentYearGrossMarginQ7Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ8New { get; set; }
        public decimal? CurrentYearGrossMarginQ8Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ9New { get; set; }
        public decimal? CurrentYearGrossMarginQ9Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ10New { get; set; }
        public decimal? CurrentYearGrossMarginQ10Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ11New { get; set; }
        public decimal? CurrentYearGrossMarginQ11Variant { get; set; }
        public decimal? CurrentYearGrossMarginQ12New { get; set; }
        public decimal? CurrentYearGrossMarginQ12Variant { get; set; }
        public decimal? CurrentYearGrossMarginCumulative { get; set; }
        public decimal? NextYearGrossMarginQ1New { get; set; }
        public decimal? NextYearGrossMarginQ1Variant { get; set; }
        public decimal? NextYearGrossMarginQ2New { get; set; }
        public decimal? NextYearGrossMarginQ2Variant { get; set; }
        public decimal? NextYearGrossMarginQ3New { get; set; }
        public decimal? NextYearGrossMarginQ3Variant { get; set; }
        public decimal? NextYearGrossMarginQ4New { get; set; }
        public decimal? NextYearGrossMarginQ4Variant { get; set; }
        public decimal? NextYearGrossMarginQ5New { get; set; }
        public decimal? NextYearGrossMarginQ5Variant { get; set; }
        public decimal? NextYearGrossMarginQ6New { get; set; }
        public decimal? NextYearGrossMarginQ6Variant { get; set; }
        public decimal? NextYearGrossMarginQ7New { get; set; }
        public decimal? NextYearGrossMarginQ7Variant { get; set; }
        public decimal? NextYearGrossMarginQ8New { get; set; }
        public decimal? NextYearGrossMarginQ8Variant { get; set; }
        public decimal? NextYearGrossMarginQ9New { get; set; }
        public decimal? NextYearGrossMarginQ9Variant { get; set; }
        public decimal? NextYearGrossMarginQ10New { get; set; }
        public decimal? NextYearGrossMarginQ10Variant { get; set; }
        public decimal? NextYearGrossMarginQ11New { get; set; }
        public decimal? NextYearGrossMarginQ11Variant { get; set; }
        public decimal? NextYearGrossMarginQ12New { get; set; }
        public decimal? NextYearGrossMarginQ12Variant { get; set; }
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
        public decimal? CurrentYearManHoursQ5New { get; set; }
        public decimal? CurrentYearManHoursQ5Variant { get; set; }
        public decimal? CurrentYearManHoursQ6New { get; set; }
        public decimal? CurrentYearManHoursQ6Variant { get; set; }
        public decimal? CurrentYearManHoursQ7New { get; set; }
        public decimal? CurrentYearManHoursQ7Variant { get; set; }
        public decimal? CurrentYearManHoursQ8New { get; set; }
        public decimal? CurrentYearManHoursQ8Variant { get; set; }
        public decimal? CurrentYearManHoursQ9New { get; set; }
        public decimal? CurrentYearManHoursQ9Variant { get; set; }
        public decimal? CurrentYearManHoursQ10New { get; set; }
        public decimal? CurrentYearManHoursQ10Variant { get; set; }
        public decimal? CurrentYearManHoursQ11New { get; set; }
        public decimal? CurrentYearManHoursQ11Variant { get; set; }
        public decimal? CurrentYearManHoursQ12New { get; set; }
        public decimal? CurrentYearManHoursQ12Variant { get; set; }
        public decimal? CurrentYearManHoursCumulative { get; set; }
        public decimal? NextYearManHoursQ1New { get; set; }
        public decimal? NextYearManHoursQ1Variant { get; set; }
        public decimal? NextYearManHoursQ2New { get; set; }
        public decimal? NextYearManHoursQ2Variant { get; set; }
        public decimal? NextYearManHoursQ3New { get; set; }
        public decimal? NextYearManHoursQ3Variant { get; set; }
        public decimal? NextYearManHoursQ4New { get; set; }
        public decimal? NextYearManHoursQ4Variant { get; set; }
        public decimal? NextYearManHoursQ5New { get; set; }
        public decimal? NextYearManHoursQ5Variant { get; set; }
        public decimal? NextYearManHoursQ6New { get; set; }
        public decimal? NextYearManHoursQ6Variant { get; set; }
        public decimal? NextYearManHoursQ7New { get; set; }
        public decimal? NextYearManHoursQ7Variant { get; set; }
        public decimal? NextYearManHoursQ8New { get; set; }
        public decimal? NextYearManHoursQ8Variant { get; set; }
        public decimal? NextYearManHoursQ9New { get; set; }
        public decimal? NextYearManHoursQ9Variant { get; set; }
        public decimal? NextYearManHoursQ10New { get; set; }
        public decimal? NextYearManHoursQ10Variant { get; set; }
        public decimal? NextYearManHoursQ11New { get; set; }
        public decimal? NextYearManHoursQ11Variant { get; set; }
        public decimal? NextYearManHoursQ12New { get; set; }
        public decimal? NextYearManHoursQ12Variant { get; set; }
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
        public decimal? CurrentYearCostOfSalesQ5New { get; set; }
        public decimal? CurrentYearCostOfSalesQ5Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ6New { get; set; }
        public decimal? CurrentYearCostOfSalesQ6Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ7New { get; set; }
        public decimal? CurrentYearCostOfSalesQ7Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ8New { get; set; }
        public decimal? CurrentYearCostOfSalesQ8Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ9New { get; set; }
        public decimal? CurrentYearCostOfSalesQ9Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ10New { get; set; }
        public decimal? CurrentYearCostOfSalesQ10Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ11New { get; set; }
        public decimal? CurrentYearCostOfSalesQ11Variant { get; set; }
        public decimal? CurrentYearCostOfSalesQ12New { get; set; }
        public decimal? CurrentYearCostOfSalesQ12Variant { get; set; }
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
        public decimal? CurrentYearProvFutureLossQ5New { get; set; }
        public decimal? CurrentYearProvFutureLossQ5Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ6New { get; set; }
        public decimal? CurrentYearProvFutureLossQ6Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ7New { get; set; }
        public decimal? CurrentYearProvFutureLossQ7Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ8New { get; set; }
        public decimal? CurrentYearProvFutureLossQ8Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ9New { get; set; }
        public decimal? CurrentYearProvFutureLossQ9Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ10New { get; set; }
        public decimal? CurrentYearProvFutureLossQ10Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ11New { get; set; }
        public decimal? CurrentYearProvFutureLossQ11Variant { get; set; }
        public decimal? CurrentYearProvFutureLossQ12New { get; set; }
        public decimal? CurrentYearProvFutureLossQ12Variant { get; set; }
        public decimal? ProvFutureLossGrandTotal { get; set; }

    }
    public class ScenarioDetails
    {

        public List<HeaderStructure> Headers { get; set; }
        public List<DetailedStructure> Details { get; set; }
    }
}
