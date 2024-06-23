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
    public class SubActionBL : ISubActionBL
    {
         
        public SubActionBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            SubActionRepository _subactionRepo = new SubActionRepository();
            return _subactionRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<SubAction> GetAll()
        {
            SubActionRepository _subactionRepo = new SubActionRepository();
            return _subactionRepo.GetAll();
        }

        public SubAction GetById(int Id)
        {
            SubActionRepository _subactionRepo = new SubActionRepository();
            return _subactionRepo.GetById(Id);
        }

        public SubAction Save(SubAction subaction)
        {
            SubActionRepository _subactionRepo = new SubActionRepository();
            return _subactionRepo.Save(subaction);
        }
        public SubAction Update(SubAction subaction)
        {
            SubActionRepository _subactionRepo = new SubActionRepository();
            return _subactionRepo.Update(subaction);
        }
    }
}
