using System.Collections.Generic;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface ICurrencyRepository
    {
        Currency GetById(int Id);
        IEnumerable<Currency> GetAll();
        Currency Save(Currency currency);
        bool Delete(int Id, string DeletedBy);
    }
}
