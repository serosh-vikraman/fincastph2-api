using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IScenarioScopeBL
    {
        IEnumerable<ScenarioScope> GetAll();
        ScenarioScope Save(ScenarioScope scenarioScope);
        ScenarioScope GetById(int Id);

        ScenarioScope Update(ScenarioScope scenarioScope);

        bool Delete(int Id, string DeletedBy);
    }
}
