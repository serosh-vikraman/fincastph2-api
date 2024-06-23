using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IFinancialDataTypeBL
    {
        IEnumerable<FinancialDataType> GetAll();
        IEnumerable<FinancialDataType> GetAllMapped(string scope);
        List<FinancialDataType> GetAllFinancialDataTypesOfScenario(int id);
        FinancialDataType Save(FinancialDataType financialDataType);
        bool Map(FinancialDataTypeMapping financialDataType);
        bool MapReportDatatypes(int reportId, int[] ids);
        FinancialDataType GetById(int Id);
        List<FinancialDataType> GetAllFinancialDataTypesofReport(int id);
        FinancialDataType Update(FinancialDataType financialDataType);

        bool Delete(int Id, string Deletedby);
    }
}
