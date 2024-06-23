using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IReportRepository
    {
        VarianceAnalysisReport GetVarianceAnalysisReportData(VarianceAnalysisConfig config);
        ProjectLifeCycleReport GetProjectLifeCycleReportData(string projectIds, int scopeId);
        REPExtractReport GetREPExtractReportData(int year,int reportTypeId, int scenarioId, string groupLevels);
        ProjectLifeCycle ProjectLifeCycleReport(int projectid, string scenarioscope);
        DeviationAnalysisYearWiseModel GetDeviationAnalysisReport(DevianceReportConfig config);
        DashboardDataModel GetDashboardData(DashboardConfig config);
        FinancePerformanceDataModel FinancePerformanceReport(DashboardConfig config);
        ProjectPerformanceDataModel ProjectPerformanceReport(DashboardConfig config);
        TrendReportData TrendAnalysisReport(DashboardConfig config);
        List<ProjectDataModel> YearOverYear(int id1, int id2, string code);
    }
}
