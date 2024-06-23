using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IContractTypeBL
    {
        IEnumerable<ContractType> GetAll();
        ContractType Save(ContractType contractType);
        ContractType GetById(int Id);

        ContractType Update(ContractType contractType);

        bool Delete(int Id, string Deletedby);
    }
}
