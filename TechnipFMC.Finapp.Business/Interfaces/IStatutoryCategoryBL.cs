using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IStatutoryCategoryBL
    {
        IEnumerable<StatutoryCategory> GetAll();
        StatutoryCategory Save(StatutoryCategory statutorycategory);
        StatutoryCategory GetById(int Id);

        StatutoryCategory Update(StatutoryCategory statutorycategory);

        bool Delete(int Id, string Deletedby);
    }
}
