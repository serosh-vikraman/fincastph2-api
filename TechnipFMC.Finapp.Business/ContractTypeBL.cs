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
    public class ContractTypeBL: IContractTypeBL
    {
         public ContractTypeBL(IContractTypeRepository contractypeRepo)
        {
            //_countryRepo = countryRepo;
        }
        public ContractTypeBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ContractTypeRepository _contractypeRepo = new ContractTypeRepository();
            return _contractypeRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<ContractType> GetAll()
        {
            ContractTypeRepository _contractypeRepo = new ContractTypeRepository();
            return _contractypeRepo.GetAll();
        }

        public ContractType GetById(int id )
        {
            ContractTypeRepository _contractypeRepo = new ContractTypeRepository();
            return _contractypeRepo.GetById(id);
        }

        public ContractType Save(ContractType contracttype)
        {
            ContractTypeRepository _contractypeRepo = new ContractTypeRepository();
            return _contractypeRepo.Save(contracttype);
        }
        public ContractType Update(ContractType contracttype)
        {
            throw new NotImplementedException();
        }
    }
}
