using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IFinancialDataTypeRepository
    {
        IEnumerable<FinancialDataType> GetAll();
        IEnumerable<FinancialDataType> GetAllMapped(string scope);
        List<FinancialDataType> GetAllFinancialDataTypesofReport(int id);
        List<FinancialDataType> GetAllFinancialDataTypesOfScenario(int id);
        FinancialDataType Save(FinancialDataType financialDataType);
        bool Map(FinancialDataTypeMapping financialDataType);
        FinancialDataType GetById(int Id);
        bool MapReportDatatypes(int reportId,int[] ids);
        
        FinancialDataType Update(FinancialDataType financialDataType);

        bool Delete(int Id, string DeletedBy);
    }
}
