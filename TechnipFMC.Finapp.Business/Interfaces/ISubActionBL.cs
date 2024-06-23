using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;
namespace TechnipFMC.Finapp.Business
{
    public interface ISubActionBL
    {
        IEnumerable<SubAction> GetAll();
        SubAction Save(SubAction subaction);
        SubAction GetById(int Id);

        SubAction Update(SubAction subaction);

        bool Delete(int Id, string Deletedby);
    }
}
