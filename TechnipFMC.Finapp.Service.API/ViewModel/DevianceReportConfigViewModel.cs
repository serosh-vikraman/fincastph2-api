using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class DevianceReportConfigViewModel
    {
        public int Year { get; set; }
        public int OrgScenarioId { get; set; }
        //public string ScenarioScope { get; set; }
        //public string ScenarioType { get; set; }
        public string Quarters { get; set; }

        public string ScenarioDataTypeId { get; set; }
        public string Spec { get; set; }
        public int DepartmentId { get; set; }
        //public string SubTotalRequired { get; set; }
    }
    public class DevianceResponseViewModel
    {
        //public string RecordType { get; set; }

        public Decimal? OrgQ1 { get; set; }
        public Decimal? OrgQ2 { get; set; }
        public Decimal? OrgQ3 { get; set; }
        public Decimal? OrgQ4 { get; set; }
        public Decimal? TotalOrg { get; set; }
        public string OrgScenarioName { get; set; }
        public List<DepartmentDataViewModel> DData { get; set; }
        public List<ClientDataViewModel> CData { get; set; }
    }
    public class DepartmentDataViewModel
    {
        public string DepartmentName { get; set; }
        public Decimal? DQ1 { get; set; }
        public Decimal? DQ2 { get; set; }
        public Decimal? DQ3 { get; set; }
        public Decimal? DQ4 { get; set; }
        public Decimal? TotalDep { get; set; }

    }
    public class ClientDataViewModel
    {
        public string ClientName { get; set; }
        public Decimal? CQ1 { get; set; }
        public Decimal? CQ2 { get; set; }
        public Decimal? CQ3 { get; set; }
        public Decimal? CQ4 { get; set; }
        public Decimal? TotalClient { get; set; }

    }
    public class DevianceGridResponseViewModel
    {
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public Decimal? Total { get; set; }
        public string EntityName { get; set; }
        public bool IsBold { get; set; }
    }
    public class DeviationAnalysisReportViewModel
    {
        //public string RecordType { get; set; }

        
        public List<OrgScenarioDataModel> OrgData { get; set; }
        public List<NonOrgScenarioDataModel> NonOrgData { get; set; }
    }
    public class DeviationAnalysisYearWiseViewModel
    {
        //public string RecordType { get; set; }


        public List<int> OrgData { get; set; }
        public List<int> NonOrgData { get; set; }
        public List<DepartmentWiseDataViewModel> DepartmentYearlyData { get; set; }
        public List<ProjectDataViewModel> ProjectYearlyData { get; set; }
        public List<BudgetDeviationDataViewModel> BudgetDeviationData { get; set; }
        public List<BudgetDeviationDataViewModel> ForecastDeviationData { get; set; }
    }
    public class NonOrgDataViewModel
    {
        public decimal YearlyData { get; set; }
        public string DepartmentName { get; set; }
    }
    public class DepartmentWiseDataViewModel
    {
        public string DepartmentName { get; set; }
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public string ScenarioName { get; set; }

    }
    public class ProjectDataViewModel
    {
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public string ProjectName { get; set; }
    }
    public class BudgetDeviationDataViewModel
    {
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public string ScenarioType { get; set; }
        
             public string ScenarioName { get; set; }
    }

    public class OrgScenarioDataModel
    {
        public Decimal? OrgQ1 { get; set; }
        public Decimal? OrgQ2 { get; set; }
        public Decimal? OrgQ3 { get; set; }
        public Decimal? OrgQ4 { get; set; }
        public int Year { get; set; }
    }
    public class NonOrgScenarioDataModel
    {
        public Decimal? Q1 { get; set; }
        public Decimal? Q2 { get; set; }
        public Decimal? Q3 { get; set; }
        public Decimal? Q4 { get; set; }
        public int Year { get; set; }
    }
    public class DashboardDataViewModel
    {
        public List<BudgetDeviationDataViewModel> BudgetDeviationData { get; set; }
        public List<BudgetDeviationDataViewModel> ForecastDeviationData { get; set; }
        public List<FinancialDataGrossViewModel> FinancialDataGross { get; set; }
        public List<DifferenceDataViewModel> DifferenceData { get; set; }
        //public List<NonOrgDataModel> DepClientYearlyData { get; set; }
    }
    public class TrendReportDataViewModel
    {
        public List<TrendDataViewModel> TrendData { get; set; }
        public List<ProjectGrossViewModel> TopProjects { get; set; }
    }
    public class TrendDataViewModel
    {
        public decimal YearlyAverage { get; set; }
        public string ProjectName { get; set; }
        public int Year { get; set; }
    }
    public class FinancePerformanceDataViewModel
    {
        public BudgetDeviationDataViewModel FinanceData { get; set; }
        public Decimal? FinancialDataGross { get; set; }
        public List<DepartmentWiseDataViewModel> FinancialDataChart { get; set; }
    }
    public class ProjectPerformanceDataViewModel
    {
        public BudgetDeviationDataViewModel FinanceData { get; set; }
        public Decimal? FinancialDataGross { get; set; }
        public List<ProjectGrossViewModel> TopProjects { get; set; }
        public List<DepartmentWiseDataViewModel> ProjectData { get; set; }
    }
    public class ProjectGrossViewModel
    {
        public Decimal? GrossYear { get; set; }
        public string ProjectName { get; set; }
    }
    public class FinancialDataGrossViewModel
    {
        public Decimal? GrossYear { get; set; }
        public string FinancialDataTypeName { get; set; }
        public string ScenarioType { get; set; }
    }
    public class DifferenceDataViewModel
    {
        public Decimal? Difference { get; set; }
        public string FinancialDataTypeName { get; set; }
    }
    public class DashboardConfigViewModel
    {
        public int Year { get; set; }
        public string Scope { get; set; }
        //public string ScenarioScope { get; set; }
        //public string ScenarioType { get; set; }
        public string DepartmentId { get; set; }
        public string ProjectId { get; set; }
        public string ScenarioDataTypeId { get; set; }
        //public string SubTotalRequired { get; set; }
        public string Quarters { get; set; }
    }
}