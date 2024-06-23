using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IActionBL
    {
        IEnumerable<ActionEntity> GetAll();
        ActionEntity Save(ActionEntity actionEntity);
        ActionEntity GetById(int Id);

        ActionEntity Update(ActionEntity actionEntity);

        bool Delete(int Id, string DeletedBy);
    }
}
