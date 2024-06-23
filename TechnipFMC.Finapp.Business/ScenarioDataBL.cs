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
    public class ScenarioDataBL : IScenarioDataBL
    {
        
        public ScenarioDataBL(IScenarioDataRepository scenarioDataRepo)
        {
            //_countryRepo = countryRepo;
        }
        public ScenarioDataBL()
        { }
        public bool Delete(int Id, string Deletedby)
        {
            ScenarioDataRepository _scenarioDataRepo = new ScenarioDataRepository();
            return _scenarioDataRepo.Delete(Id, Deletedby);
        }
        public int ClearScenarioData(int ScenarioId, string Deletedby)
        {
            ScenarioDataRepository _scenarioDataRepo = new ScenarioDataRepository();
            return _scenarioDataRepo.ClearScenarioData(ScenarioId, Deletedby);
        }
        public bool ClearProjectScenarioData(ScenarioProjectMapper scenarioProject)
        {
            ScenarioDataRepository _scenarioDataRepo = new ScenarioDataRepository();
            return _scenarioDataRepo.ClearProjectScenarioData(scenarioProject);
        }
        public IEnumerable<YearlyScenarioData> GetAll(int projectId, int scenarioId)
        {
            ScenarioDataRepository _scenarioDataRepo = new ScenarioDataRepository();
            return _scenarioDataRepo.GetAll(projectId, scenarioId);
        }
        public ScenarioDetails GetAllQuarterlyDataOfScenario(int scenarioId)
        {
            ScenarioDataRepository _scenarioDataRepo = new ScenarioDataRepository();
            return _scenarioDataRepo.GetAllQuarterlyDataOfScenario(scenarioId);
        }

        public ScenarioData GetById(int Id)
        {
            throw new NotImplementedException();

            //return _scenarioDataRepo.GetById(Id);
        }

        public bool Save(ScenarioDataMaster scenarioData)
        {
            ScenarioDataRepository _scenarioDataRepo = new ScenarioDataRepository();
            return _scenarioDataRepo.Save(scenarioData);
        }

        public ScenarioData Update(ScenarioData scenarioData)
        {
            throw new NotImplementedException();
        }

        public List<ScenarioLayout> GetScenarioLayout(int scenarioId)
        {
            ScenarioDataRepository _scenarioDataRepo = new ScenarioDataRepository();
            return _scenarioDataRepo.GetScenarioLayout(scenarioId);
        }
    }
}
