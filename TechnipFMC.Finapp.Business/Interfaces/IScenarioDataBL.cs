using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IScenarioDataBL
    {
        IEnumerable<YearlyScenarioData> GetAll(int projectId, int scenarioId);

        ScenarioDetails GetAllQuarterlyDataOfScenario(int scenarioId);
        bool Save(ScenarioDataMaster scenarioData);
        ScenarioData GetById(int Id);

        ScenarioData Update(ScenarioData scenarioData);

        bool Delete(int Id, string Deletedby);

        int ClearScenarioData(int ScenarioId, string Deletedby);
        bool ClearProjectScenarioData(ScenarioProjectMapper scenarioProject);
        List<ScenarioLayout> GetScenarioLayout(int scenarioId);
    }
}
