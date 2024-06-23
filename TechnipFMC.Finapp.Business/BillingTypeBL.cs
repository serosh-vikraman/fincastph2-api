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
    public class BillingTypeBL : IBillingTypeBL
    {
        public BillingTypeBL(IBillingTypeRepository BillingTypesRepo)
        {
            //_countryRepo = countryRepo;
        }
        public BillingTypeBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            BillingTypeRepository _BillingTypeRepo = new BillingTypeRepository();
            return _BillingTypeRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<BillingType> GetAll()
        {
            BillingTypeRepository _BillingTypeRepo = new BillingTypeRepository();
            return _BillingTypeRepo.GetAll();
        }

        public BillingType GetById(int id)
        {
            BillingTypeRepository _BillingTypeRepo = new BillingTypeRepository();
            return _BillingTypeRepo.GetById(id);
        }

        public BillingType Save(BillingType billingType)
        {
            BillingTypeRepository _BillingTypeRepo = new BillingTypeRepository();
            return _BillingTypeRepo.Save(billingType);
        }
        public BillingType Update(BillingType billingTypes)
        {
            throw new NotImplementedException();
        }

    }
    public class CommonBL
    {
        public string GetMessage(string code)
        {
            return new MessageRepository().GetMessage(code);
        }
    }
}
