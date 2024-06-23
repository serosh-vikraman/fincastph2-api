using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class ScenarioBL : IScenarioBL
    {
        private ScenarioRepository _scenarioRepo;

        public ScenarioBL(IScenarioTypeRepository ScenarioTypesRepo)
        {
            //_countryRepo = countryRepo;
        }
        public ScenarioBL()
        { }

        public string Delete(int Id, string Deletedby)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.Delete(Id, Deletedby);
        }
        public string DeleteMultipleScenario(ScenarioID scenarioIds)
        {
            _scenarioRepo = new ScenarioRepository();
             return _scenarioRepo.DeleteMultipleScenario(scenarioIds);
        }

        public bool GetLegacyInsertionStatus()
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetLegacyInsertionStatus();
        }

        public IEnumerable<Scenario> GetAll(int departmentId, int clientId, string spec)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetAll(departmentId, clientId, spec);
        }
        public IEnumerable<Scenario> GetAllScenarioOfProject(int projectid)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetAllScenarioOfProject(projectid);
        }
        public IEnumerable<ProjectForScenario> GetAllProjects(int ScenarioID)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetAllProjects(ScenarioID);
        }
        public ScenarioApplicableYears GetApplicableYears(int scenarioId)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetApplicableYears(scenarioId);
        }

        public Scenario GetById(int id)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetById(id);
        }

        public Scenario Save(Scenario scenario)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.Save(scenario);
        }
        public Scenario DuplicateScenario(Scenario scenario)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.DuplicateScenario(scenario);
        }
        public bool MapScenarioProjects(ProjectScenarioModel scenarioProject)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.MapScenarioProjects(scenarioProject);
        }
        public string GetScenarioSequence(string scenarioScopeCode, string scenarioTypeCode, int financialYear)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetScenarioSequence(scenarioScopeCode, scenarioTypeCode, financialYear);
        }

        public string RemoveScenarioProjects(ProjectScenarioModel projectScenarioModel)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.RemoveScenarioProjects(projectScenarioModel);
        }

        public ScenarioProjectLog ChangeScenarionStatus(ScenarioProjectLog scenarioProjectLog)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.ChangeScenarionStatus(scenarioProjectLog);
        }
        public ScenarioProjectLog GetWIPStatus(ScenarioProjectLog scenarioProjectLog)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetWIPStatus(scenarioProjectLog);
        }
        public ScenarioProjectLog IsScenarioLockedforProjectUpdate(ScenarioProjectLog scenarioProjectLog)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.IsScenarioLockedforProjectUpdate(scenarioProjectLog);
        }
        public ScenarioProjectLog IsScenarioLockedforUpload(ScenarioProjectLog scenarioProjectLog)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.IsScenarioLockedforUpload(scenarioProjectLog);
        }
        public List<ScenarioIDS> GetAllWIPStatusTrueScenario()
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetAllWIPStatusTrueScenario();
        }

        public List<Scenario> GetScenarioByYear(int year)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetScenarioByYear(year);
        }
        public List<Scenario> GetOrgScenarioByYear(int year)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetOrgScenarioByYear(year);
        }

        public List<Scenario> GetScenarioByScenarioId(int scenarioId, int year)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetScenarioByScenarioId(scenarioId, year);
        }
        public List<Scenario> GetScenariosByYearAndScope(DashboardConfig config)
        {
            _scenarioRepo = new ScenarioRepository();
            return _scenarioRepo.GetScenariosByYearAndScope(config);
        }        
    }
}
