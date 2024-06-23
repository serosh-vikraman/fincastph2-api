using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IBUCategoryBL
    {
        IEnumerable<BUCategory> GetAll();
        BUCategory Save(BUCategory bucategory);
        BUCategory GetById(int Id);

        BUCategory Update(BUCategory bucategory);

        bool Delete(int Id, string Deletedby);
    }
}
