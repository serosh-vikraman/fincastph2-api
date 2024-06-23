using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface ICurrencyExchangeRepository
    {
        CurrencyExchange GetById(int Id);
        IEnumerable<CurrencyExchange> GetAll();
        CurrencyExchange Save(CurrencyExchange currency);
        bool Delete(int Id, string DeletedBy);
    }
}
