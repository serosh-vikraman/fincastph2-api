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
    public class ContractStatusBL: IContractStatusBL
    {
         
        public ContractStatusBL(IContractStatusRepository contractstatusRepo)
        {
            //_countryRepo = countryRepo;
        }
        public ContractStatusBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ContractStatusRepository _contractstatusRepo = new ContractStatusRepository();
            return _contractstatusRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<ContractStatus> GetAll()
        {
            ContractStatusRepository _contractstatusRepo = new ContractStatusRepository();
            return _contractstatusRepo.GetAll();
        }

        public ContractStatus GetById(int id)
        {
            ContractStatusRepository _contractstatusRepo = new ContractStatusRepository();
            return _contractstatusRepo.GetById(id);
        }

        public ContractStatus Save(ContractStatus contractstatus)
        {
            ContractStatusRepository _contractstatusRepo = new ContractStatusRepository();
            return _contractstatusRepo.Save(contractstatus);
        }
        public ContractStatus Update(ContractStatus contractstatus)
        {
            throw new NotImplementedException();
        }
    }
}
