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
    public class ScenarioTypeBL : IScenarioTypeBL
    {
         
        public ScenarioTypeBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ScenarioTypeRepository _scenarioTypeRepo = new ScenarioTypeRepository();
            return _scenarioTypeRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<ScenarioType> GetAll()
        {
            ScenarioTypeRepository _scenarioTypeRepo = new ScenarioTypeRepository();
            return _scenarioTypeRepo.GetAll();
        }

        public ScenarioType GetById(int id)
        {
            ScenarioTypeRepository _scenarioTypeRepo = new ScenarioTypeRepository();
            return _scenarioTypeRepo.GetById(id);
        }

        public ScenarioType Save(ScenarioType scenarioType)
        {
            ScenarioTypeRepository _scenarioTypeRepo = new ScenarioTypeRepository();
            return _scenarioTypeRepo.Save(scenarioType);
        }
        public ScenarioType Update(ScenarioType scenarioTypes)
        {
            throw new NotImplementedException();
        }
    }
}
