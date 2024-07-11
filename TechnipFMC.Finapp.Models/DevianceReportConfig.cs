using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class DevianceReportConfig
    {
        public int Year { get; set; }
        public int OrgScenarioId { get; set; }
        //public string ScenarioScope { get; set; }
        //public string ScenarioType { get; set; }
        public string ScenarioDataTypeId { get; set; }
        //public string SubTotalRequired { get; set; }
        public string Quarters { get; set; }
        public string Spec { get; set; }
        public int DepartmentId { get; set; }
        public int ClientId { get; set; }
    }
    public class DashboardConfig
    {
        public int Year { get; set; }
        public string Scope { get; set; }
        public string Type { get; set; }
        //public string ScenarioScope { get; set; }
        public string DepartmentId { get; set; }
        public int ProjectId { get; set; }
        public string ScenarioDataTypeId { get; set; }
        public string Spec { get; set; }
        public string Quarters { get; set; }
    }
    public class DevianceResponseModel
    {
        public string FinancialDataType { get; set; }
        public List<DevianceGridResponse> GridResponse { get; set; }
        //public List<DevianceGridResponse> GrandTotal { get; set; }
    }
    public class DevianceGridResponse
    {
        
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public Decimal? Q5 { get; set; }
        public Decimal? Q6 { get; set; }
        public Decimal? Q7 { get; set; }
        public Decimal? Q8 { get; set; }
        public Decimal? Q9 { get; set; }
        public Decimal? Q10 { get; set; }
        public Decimal? Q11 { get; set; }
        public Decimal? Q12 { get; set; }
        public Decimal? Total { get; set; }
        public string EntityName { get; set; }
        public bool IsBold { get; set; }
    }
    public class DevianceResponse
    {
        //public string RecordType { get; set; }
        
        public Decimal? OrgQ1 { get; set; }
        public Decimal? OrgQ2 { get; set; }
        public Decimal? OrgQ3 { get; set; }
        public Decimal? OrgQ4 { get; set; }
        public Decimal? OrgQ5 { get; set; }
        public Decimal? OrgQ6 { get; set; }
        public Decimal? OrgQ7 { get; set; }
        public Decimal? OrgQ8 { get; set; }
        public Decimal? OrgQ9 { get; set; }
        public Decimal? OrgQ10 { get; set; }
        public Decimal? OrgQ11 { get; set; }
        public Decimal? OrgQ12 { get; set; }
        public Decimal? TotalOrg { get; set; }
        public string OrgScenarioName { get; set; }
        public List<DepartmentData> DData { get; set; }
        public List<ClientData> CData { get; set; }
    }
    public class DepartmentData
    {
        public string DepartmentName { get; set; }
        public string ScenarioName { get; set; }
        public Decimal? DQ1 { get; set; }
        public Decimal? DQ2 { get; set; }
        public Decimal? DQ3 { get; set; }
        public Decimal? DQ4 { get; set; }
        public Decimal? DQ5 { get; set; }
        public Decimal? DQ6 { get; set; }
        public Decimal? DQ7 { get; set; }
        public Decimal? DQ8 { get; set; }
        public Decimal? DQ9 { get; set; }
        public Decimal? DQ10 { get; set; }
        public Decimal? DQ11 { get; set; }
        public Decimal? DQ12 { get; set; }
        public Decimal? TotalDep { get; set; }

    }
    public class ClientData
    {
        public string ClientName { get; set; }
        public string ScenarioName { get; set; }
        public Decimal? CQ1 { get; set; }
        public Decimal? CQ2 { get; set; }
        public Decimal? CQ3 { get; set; }
        public Decimal? CQ4 { get; set; }
        public Decimal? CQ5 { get; set; }
        public Decimal? CQ6 { get; set; }
        public Decimal? CQ7 { get; set; }
        public Decimal? CQ8 { get; set; }
        public Decimal? CQ9 { get; set; }
        public Decimal? CQ10 { get; set; }
        public Decimal? CQ11 { get; set; }
        public Decimal? CQ12 { get; set; }
        public Decimal? TotalClient { get; set; }

    }
    public class DeviationAnalysisYearWiseModel
    {
        //public string RecordType { get; set; }


        public decimal[] OrgData { get; set; }
        public decimal[] NonOrgData { get; set; }
        public List<DepartmentWiseDataModel> DepartmentYearlyData { get; set; }
        public List<ProjectDataModel> ProjectYearlyData { get; set; }
        public List<BudgetDeviationDataModel> BudgetDeviationData { get; set; }
        public List<BudgetDeviationDataModel> ForecastDeviationData { get; set; }
        //public List<NonOrgDataModel> DepClientYearlyData { get; set; }
    }
    public class NonOrgDataModel
    {
        public decimal YearlyData { get; set; }
        public string DepartmentName { get; set; }
    }
    public class DepartmentWiseDataModel
    {
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public Decimal? Q5 { get; set; }
        public Decimal? Q6 { get; set; }
        public Decimal? Q7 { get; set; }
        public Decimal? Q8 { get; set; }
        public Decimal? Q9 { get; set; }
        public Decimal? Q10 { get; set; }
        public Decimal? Q11 { get; set; }
        public Decimal? Q12 { get; set; }
        public string DepartmentName { get; set; }
        public string ScenarioName { get; set; }
    }
    public class ProjectDataModel
    {
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public Decimal? Q5 { get; set; }
        public Decimal? Q6 { get; set; }
        public Decimal? Q7 { get; set; }
        public Decimal? Q8 { get; set; }
        public Decimal? Q9 { get; set; }
        public Decimal? Q10 { get; set; }
        public Decimal? Q11 { get; set; }
        public Decimal? Q12 { get; set; }
        public int Year { get; set; }
        public string ProjectName { get; set; }
        public string ScenarioName { get; set; }
    }
    public class BudgetDeviationDataModel
    {
        public string DepartmentName { get; set; }
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public Decimal? Q5 { get; set; }
        public Decimal? Q6 { get; set; }
        public Decimal? Q7 { get; set; }
        public Decimal? Q8 { get; set; }
        public Decimal? Q9 { get; set; }
        public Decimal? Q10 { get; set; }
        public Decimal? Q11 { get; set; }
        public Decimal? Q12 { get; set; }

        public string ScenarioType { get; set; }
        public string ScenarioName { get; set; }
    }
    public class TrendReportData
    {
        public List<TrendDataModel> TrendData { get; set; }
        public List<ProjectGross> TopProjects { get; set; }
    }
    public class TrendDataModel
    {
        public decimal YearlyAverage { get; set; }
        public string ProjectName { get; set; }
        public int Year { get; set; }
    }
    public class DeviationAnalysisReportModel
    {
        //public string RecordType { get; set; }


        public List<OrgScenarioDataModel> OrgData { get; set; }
        public List<NonOrgScenarioDataModel> NonOrgData { get; set; }
    }
    public class OrgScenarioDataModel
    {
        public Decimal? OrgQ1 { get; set; }
        public Decimal? OrgQ2 { get; set; }
        public Decimal? OrgQ3 { get; set; }
        public Decimal? OrgQ4 { get; set; }
        public Decimal? OrgQ5 { get; set; }
        public Decimal? OrgQ6 { get; set; }
        public Decimal? OrgQ7 { get; set; }
        public Decimal? OrgQ8 { get; set; }
        public Decimal? OrgQ9 { get; set; }
        public Decimal? OrgQ10 { get; set; }
        public Decimal? OrgQ11 { get; set; }
        public Decimal? OrgQ12 { get; set; }
        public int Year { get; set; }
    }
    public class NonOrgScenarioDataModel
    {
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public Decimal? Q5 { get; set; }
        public Decimal? Q6 { get; set; }
        public Decimal? Q7 { get; set; }
        public Decimal? Q8 { get; set; }
        public Decimal? Q9 { get; set; }
        public Decimal? Q10 { get; set; }
        public Decimal? Q11 { get; set; }
        public Decimal? Q12 { get; set; }
        public int Year { get; set; }
    }

    public class DashboardDataModel
    {
        public List<BudgetDeviationDataModel> BudgetDeviationData { get; set; }
        public List<BudgetDeviationDataModel> ForecastDeviationData { get; set; }
        public List<FinancialDataGross> FinancialDataGross { get; set; }
        public List<DifferenceData> DifferenceData { get; set; }
        //public List<NonOrgDataModel> DepClientYearlyData { get; set; }
    }
    public class FinancePerformanceDataModel
    {
        public BudgetDeviationDataModel FinanceData { get; set; }
        public Decimal? FinancialDataGross { get; set; }
        public List<DepartmentWiseDataModel> FinancialDataChart { get; set; }
    }
    public class ProjectPerformanceDataModel
    {
        public BudgetDeviationDataModel FinanceData { get; set; }
        public Decimal? FinancialDataGross { get; set; }
        public List<ProjectGross> TopProjects { get; set; }
        public List<DepartmentWiseDataModel> ProjectData { get; set; }
    }

    public class FinancialDataGross
    {
        public Decimal? GrossYear { get; set; }
        public string FinancialDataTypeName { get; set; }
        public string ScenarioType { get; set; }
    }
    public class ProjectGross
    {
        public Decimal? GrossYear { get; set; }
        public string ProjectName { get; set; }
    }
    public class DifferenceData
    {
        public Decimal? Difference { get; set; }
        public string FinancialDataTypeName { get; set; }
    }
}
