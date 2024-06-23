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
    public class LockQuarterBL : ILockQuarterBL
    {

        public LockQuarterBL(ILockQuarterRepository lockQuarterRepo)
        {
            //_countryRepo = countryRepo;
        }
        public LockQuarterBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            LockQuarterRepository _lockQuarterRepo = new LockQuarterRepository();
            return _lockQuarterRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<LockQuarter> GetAll()
        {
            LockQuarterRepository _lockQuarterRepo = new LockQuarterRepository();
            return _lockQuarterRepo.GetAll();
        }

        public LockQuarter GetById(int Id)
        {
            LockQuarterRepository _lockQuarterRepo = new LockQuarterRepository();
            return _lockQuarterRepo.GetById(Id);
        }

        public LockQuarter Save(LockQuarter lockQuarter)
        {
            LockQuarterRepository _lockQuarterRepo = new LockQuarterRepository();
            return _lockQuarterRepo.Save(lockQuarter);
        }

    }
}
