using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IContractTypeRepository
    {
        IEnumerable<ContractType> GetAll();
        ContractType Save(ContractType contracttype);
        ContractType GetById(int Id );

        ContractType Update(ContractType contracttype);

        bool Delete(int Id, string DeletedBy);
    }
}
