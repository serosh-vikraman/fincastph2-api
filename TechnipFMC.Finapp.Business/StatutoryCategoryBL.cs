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
    public class StatutoryCategoryBL: IStatutoryCategoryBL
    {
        
        public StatutoryCategoryBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            StatutoryCategoryRepository _statutorycategoryRepo = new StatutoryCategoryRepository();
            return _statutorycategoryRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<StatutoryCategory> GetAll()
        {
            StatutoryCategoryRepository _statutorycategoryRepo = new StatutoryCategoryRepository();
            return _statutorycategoryRepo.GetAll();
        }

        public StatutoryCategory GetById(int id)
        {
            StatutoryCategoryRepository _statutorycategoryRepo = new StatutoryCategoryRepository();
            return _statutorycategoryRepo.GetById(id);
        }

        public StatutoryCategory Save(StatutoryCategory statutorycategory)
        {
            StatutoryCategoryRepository _statutorycategoryRepo = new StatutoryCategoryRepository();
            return _statutorycategoryRepo.Save(statutorycategory);
        }
        public StatutoryCategory Update(StatutoryCategory statutorycategory)
        {
            throw new NotImplementedException();
        }
    }
}
