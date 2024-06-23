using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IScenarioTypeRepository
    {
        IEnumerable<ScenarioType> GetAll();
        ScenarioType Save(ScenarioType scenarioType);
        ScenarioType GetById(int Id);

        ScenarioType Update(ScenarioType scenarioType);

        bool Delete(int Id, string DeletedBy);
    }
}
