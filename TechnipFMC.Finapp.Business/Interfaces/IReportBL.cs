using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business.Interfaces
{
    public interface IReportBL
    {
        byte[] GetVarianceAnalysisReportExcel(VarianceAnalysisConfig config);
        byte[] GetProjectLifeCycleReportExcel(string projectIds, int scopeId);
        byte[] GetREPExtractReportDataExcel(int year, int reportTypeId, int scenarioId, string groupLevels);
        VarianceAnalysisResponseModel GetVarianceAnalysisReport(VarianceAnalysisConfig config);
        List<DevianceGridResponse> GetDevianceReport(DevianceReportConfig config);
        DeviationAnalysisYearWiseModel GetDeviationAnalysisReport(DevianceReportConfig devianceConfig);
        DashboardDataModel GetDashboardData(DashboardConfig devianceConfig);
        FinancePerformanceDataModel FinancePerformanceReport(DashboardConfig devianceConfig);
        ProjectPerformanceDataModel ProjectPerformanceReport(DashboardConfig devianceConfig);
        TrendReportData TrendAnalysisReport(DashboardConfig devianceConfig);
        List<FinancialDataType> GetAllFinancialDataTypesOfScenario(int id);

        ProjectLifeCycle ProjectLifeCycleReport(int projectid, string scenarioscope);
        ExtractResponseDataModel REPExtractReport(int year, string scenarioTypeCode, string isCurrencyConversionRequired);
        byte[] GetVarianceAnalysisExcel(VarianceAnalysisConfig config, VarianceAnalysisResponseModel data, VarianceAnalysisResponseModel dataGM);
        byte[] GetDevianceReportExcel(DevianceReportConfig config,List<DevianceResponseModel> data);
        byte[] GetFinanceReportExcel(DashboardConfig config, FinancePerformanceDataModel data);
        byte[] GetProjectPerformanceReportExcel(DashboardConfig config, ProjectPerformanceDataModel data);

        byte[] ProjectLifeCycleReportDownload(ProjectLifeCycle projectLife, string scenarioscope);
        byte[] REPExtractReportDownload(ExtractResponseDataModel response, int Year, string scenarioTypeCode, string isCurrencyConversionRequired);
        byte[] ProjectLifeCycleReportDownload1(ProjectLifeCycle projectLife, string scenarioscope);
        ProjectLifeCycleDataModel ProjectLifeCycleReport1(int projectid, string scenarioscope);
        List<ProjectDataModel> YearOverYear(int id1, int id2, string code);
    }
}
