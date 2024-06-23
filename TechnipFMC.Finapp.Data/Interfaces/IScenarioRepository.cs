using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IScenarioRepository
    {
        bool GetLegacyInsertionStatus();
        IEnumerable<Scenario> GetAll(int departmentId, int clientId, string spec);
        IEnumerable<Scenario> GetAllScenarioOfProject(int projectid);
        IEnumerable<ProjectForScenario> GetAllProjects(int ScenarioID);
        ScenarioApplicableYears GetApplicableYears(int scenarioId);
        Scenario Save(Scenario scenario);
        Scenario DuplicateScenario(Scenario scenario);

        bool MapScenarioProjects(ProjectScenarioModel scenarioProject);
        Scenario GetById(int Id);
        string Delete(int Id, string DeletedBy);
        string DeleteMultipleScenario(ScenarioID scenarioIds);
        string GetScenarioSequence(string scenarioScopeCode, string scenarioTypeCode, int financialYear);
        string RemoveScenarioProjects(ProjectScenarioModel projectScenarioModel);
        ScenarioProjectLog ChangeScenarionStatus(ScenarioProjectLog scenarioProjectLog);
        ScenarioProjectLog GetWIPStatus(ScenarioProjectLog scenarioProjectLog);

        List<ScenarioIDS> GetAllWIPStatusTrueScenario();
        ScenarioProjectLog IsScenarioLockedforUpload(ScenarioProjectLog scenarioProjectLog);
        ScenarioProjectLog IsScenarioLockedforProjectUpdate(ScenarioProjectLog scenarioProjectLog);
        List<Scenario> GetScenarioByYear(int year);
        List<Scenario> GetOrgScenarioByYear(int year);
        List<Scenario> GetScenarioByScenarioId(int scenarioId, int year);
        List<Scenario> GetScenariosByYearAndScope(DashboardConfig config);
    }
}
