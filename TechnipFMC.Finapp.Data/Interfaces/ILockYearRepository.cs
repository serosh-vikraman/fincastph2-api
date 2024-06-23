using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface ILockYearRepository
    {
        IEnumerable<LockYear> GetAll();
        LockYear Save(LockYear lockYear);
        LockYear GetById(int Id);

        bool Delete(int Id, string DeletedBy);
    }
}
