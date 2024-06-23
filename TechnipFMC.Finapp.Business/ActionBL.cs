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
    public class ActionBL : IActionBL
    {
       public ActionBL(IActionRepository actionRepo)
        {
            //_countryRepo = countryRepo;
        }
        public ActionBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ActionRepository _actionRepo = new ActionRepository();
            return _actionRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<ActionEntity> GetAll()
        {
            ActionRepository _actionRepo = new ActionRepository();
            return _actionRepo.GetAll();
        }
        public ActionEntity GetById(int Id)
        {
            ActionRepository _actionRepo = new ActionRepository();
            return _actionRepo.GetById(Id);
        }

        public ActionEntity Save(ActionEntity actionEntity)
        {
            ActionRepository _actionRepo = new ActionRepository();
            return _actionRepo.Save(actionEntity);
        }
        public ActionEntity Update(ActionEntity actionEntity)
        {
            ActionRepository _actionRepo = new ActionRepository();
            return _actionRepo.Update(actionEntity);
        }
    }
}
