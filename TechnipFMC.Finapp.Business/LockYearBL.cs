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
    public class LockYearBL : ILockYearBL
    {

        public LockYearBL(ILockYearRepository lockYearRepo)
        {
            //_countryRepo = countryRepo;
        }
        public LockYearBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            LockYearRepository _lockYearRepo = new LockYearRepository();
            return _lockYearRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<LockYear> GetAll()
        {
            LockYearRepository _lockYearRepo = new LockYearRepository();
            return _lockYearRepo.GetAll();
        }

        public LockYear GetById(int Id)
        {
            LockYearRepository _lockYearRepo = new LockYearRepository();
            return _lockYearRepo.GetById(Id);
        }

        public LockYear Save(LockYear lockYear)
        {
            LockYearRepository _lockYearRepo = new LockYearRepository();
            return _lockYearRepo.Save(lockYear);
        }
    }
}
