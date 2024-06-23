using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IBUCategoryRepository
    {
        IEnumerable<BUCategory> GetAll();
        BUCategory Save(BUCategory buCategory);
        BUCategory GetById(int Id );

        BUCategory Update(BUCategory buCategory);

        bool Delete(int Id, string DeletedBy);
    }
}
