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
        byte[] GetVarianceAnalysisReportExcel(VarianceAnalysisConfig config,int cid);
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
        byte[] GetVarianceAnalysisExcel(VarianceAnalysisConfig config, VarianceAnalysisResponseModel data, VarianceAnalysisResponseModel dataGM,int cid);
        byte[] GetVarianceAnalysisExcel2(VarianceAnalysisConfig config, List<VarianceAnalysisResponseModel> data, int cid);

        byte[] GetDevianceReportExcel(DevianceReportConfig config,List<DevianceResponseModel> data,int cid);
        byte[] GetFinanceReportExcel(DashboardConfig config, FinancePerformanceDataModel data,int cid);
        byte[] GetProjectPerformanceReportExcel(DashboardConfig config, ProjectPerformanceDataModel data,int cid);

        byte[] ProjectLifeCycleReportDownload(ProjectLifeCycle projectLife, string scenarioscope,int cid);
        byte[] REPExtractReportDownload(ExtractResponseDataModel response, int Year, string scenarioTypeCode, string isCurrencyConversionRequired);
        byte[] ProjectLifeCycleReportDownload1(ProjectLifeCycle projectLife, string scenarioscope,int cid);
        ProjectLifeCycleDataModel ProjectLifeCycleReport1(int projectid, string scenarioscope);
        List<ProjectDataModel> YearOverYear(int id1, int id2, string code);
    }
}
