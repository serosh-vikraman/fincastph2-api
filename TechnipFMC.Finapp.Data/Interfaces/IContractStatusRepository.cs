using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IContractStatusRepository
    {
        IEnumerable<ContractStatus> GetAll();
        ContractStatus Save(ContractStatus contractstatus);
        ContractStatus GetById(int Id );

        ContractStatus Update(ContractStatus contractstatus);

        bool Delete(int Id, string DeletedBy);
    }
}
