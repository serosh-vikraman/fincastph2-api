using TechnipFMC.Finapp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Business.Interfaces
{
    public interface ICurrencyBL
    {
        IEnumerable<Currency> GetAll();
        Currency Save(Currency currency);
        Currency GetById(int Id);
        bool Delete(int Id, string Deletedby);
    }
}
