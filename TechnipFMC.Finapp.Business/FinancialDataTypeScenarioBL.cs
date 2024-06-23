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
    public class FinancialDataTypeScenarioBL : IFinancialDataTypeScenarioBL
    {
        public FinancialDataTypeScenarioBL(IFinancialDataTypeScenarioRepository FinancialDataTypeScenarioRepo)
        {
            //_ScenarioDataPointsRepo = ScenarioDataPointsRepo;
        }
        public FinancialDataTypeScenarioBL()
        { }
        public bool Delete(int Id, string Deletedby)
        {
            FinancialDataTypeScenarioRepository _financialDataTypeScenarioRepo = new FinancialDataTypeScenarioRepository();
            return _financialDataTypeScenarioRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<FinancialDataTypesScenario> GetAll()
        {
            FinancialDataTypeScenarioRepository _financialDataTypeScenarioRepo = new FinancialDataTypeScenarioRepository();
            return _financialDataTypeScenarioRepo.GetAll();
        }//IEnumerable<ScenarioScopeTypes>GetAllScopeTypes (string financialTypeCode);
        public IEnumerable<ScenarioScopeTypes> GetAllScopeTypes(string financialTypeCode)
        {
            FinancialDataTypeScenarioRepository _financialDataTypeScenarioRepo = new FinancialDataTypeScenarioRepository();
            return _financialDataTypeScenarioRepo.GetAllScopeTypes(financialTypeCode);
        }
        public List<string> GetAllFinancialDataTypes(string scope,string type)
        {
            FinancialDataTypeScenarioRepository _financialDataTypeScenarioRepo = new FinancialDataTypeScenarioRepository();
            return _financialDataTypeScenarioRepo.GetAllFinancialDataTypes(scope,type);
        }
        public FinancialDataTypesScenario GetById(int id)
        {
            FinancialDataTypeScenarioRepository _financialDataTypeScenarioRepo = new FinancialDataTypeScenarioRepository();
            return _financialDataTypeScenarioRepo.GetById(id);
        }

        public bool SaveScenarioDataPoints(ScenarioDataPoints scenarioDataPoints)
        {
            FinancialDataTypeScenarioRepository _financialDataTypeScenarioRepo = new FinancialDataTypeScenarioRepository();
            return _financialDataTypeScenarioRepo.SaveScenarioDataPoints(scenarioDataPoints);
        }

        public ScenarioDataPoints Update(ScenarioDataPoints scenarioDataPoints)
        {
            throw new NotImplementedException();
        }
    }
}
