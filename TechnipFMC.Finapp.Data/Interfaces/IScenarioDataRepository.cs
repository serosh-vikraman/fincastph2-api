using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IScenarioDataRepository
    {
        IEnumerable<YearlyScenarioData> GetAll(int projectId, int scenarioId);
        ScenarioDetails GetAllQuarterlyDataOfScenario(int scenarioId);
        bool Save(ScenarioDataMaster scenarioData);
        //ScenarioData GetById(int Id);

        ScenarioData Update(ScenarioData scenarioData);

        bool Delete(int Id, string DeletedBy);

        int ClearScenarioData(int ScenarioId, int DeletedBy);
        bool ClearProjectScenarioData(ScenarioProjectMapper scenarioProject);
        List<ScenarioLayout> GetScenarioLayout(int scenarioId);
    }
}
