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
    public class FinancialDataTypeBL : IFinancialDataTypeBL
    {
        public FinancialDataTypeBL(IFinancialDataTypeRepository financialDataTypeRepo)
        {
            //_FinancialDataTypeRepo = FinancialDataTypeRepo;
        }
        public FinancialDataTypeBL()
        { }
        public bool Delete(int Id, string Deletedby)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<FinancialDataType> GetAll()
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.GetAll();
        }
        public IEnumerable<FinancialDataType> GetAllMapped(string scope)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.GetAllMapped( scope);
        }
        public List<FinancialDataType> GetAllFinancialDataTypesofReport(int id)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.GetAllFinancialDataTypesofReport(id);
        }
        public List<FinancialDataType> GetAllFinancialDataTypesOfScenario( int id)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.GetAllFinancialDataTypesOfScenario(id);
        }

        public FinancialDataType GetById(int id)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.GetById(id);
        }

        public FinancialDataType Save(FinancialDataType financialDataType)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.Save(financialDataType);
        }
        public bool Map(FinancialDataTypeMapping financialDataType)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.Map(financialDataType);
        }
        public bool MapReportDatatypes(int reportId,string[] ids)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.MapReportDatatypes(reportId,ids);
        }

        public FinancialDataType Update(FinancialDataType financialDataType)
        {
            throw new NotImplementedException();
        }
    }
}
