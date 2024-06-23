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
    public class ScenarioScopeBL : IScenarioScopeBL
    {
        public ScenarioScopeBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ScenarioScopeRepository _scenarioScopeRepo = new ScenarioScopeRepository();
            return _scenarioScopeRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<ScenarioScope> GetAll()
        {
            ScenarioScopeRepository _scenarioScopeRepo = new ScenarioScopeRepository();
            return _scenarioScopeRepo.GetAll();
        }

        public ScenarioScope GetById(int id)
        {
            ScenarioScopeRepository _scenarioScopeRepo = new ScenarioScopeRepository();
            return _scenarioScopeRepo.GetById(id);
        }

        public ScenarioScope Save(ScenarioScope scenarioScope)
        {
            ScenarioScopeRepository _scenarioScopeRepo = new ScenarioScopeRepository();
            return _scenarioScopeRepo.Save(scenarioScope);
        }
        public ScenarioScope Update(ScenarioScope scenarioScopes)
        {
            throw new NotImplementedException();
        }

    }
}
