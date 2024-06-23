using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IContractStatusBL
    {
        IEnumerable<ContractStatus> GetAll();
        ContractStatus Save(ContractStatus contractStatus);
        ContractStatus GetById(int Id );

        ContractStatus Update(ContractStatus contractStatus);

        bool Delete(int Id, string Deletedby);
    }
}
