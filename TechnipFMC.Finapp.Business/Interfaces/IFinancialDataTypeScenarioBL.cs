using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IFinancialDataTypeScenarioBL
    {
        IEnumerable<FinancialDataTypesScenario> GetAll();
        IEnumerable<ScenarioScopeTypes> GetAllScopeTypes(string financialTypeCode);
        List<string> GetAllFinancialDataTypes(string scope,string type);
        bool SaveScenarioDataPoints(ScenarioDataPoints scenarioDataPoints);
        FinancialDataTypesScenario GetById(int Id);

        ScenarioDataPoints Update(ScenarioDataPoints scenarioDataPoints);

        bool Delete(int Id, string Deletedby);
    }
}
