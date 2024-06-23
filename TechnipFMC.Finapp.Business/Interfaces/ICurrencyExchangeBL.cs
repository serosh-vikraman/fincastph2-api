using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business.Interfaces
{
    public interface ICurrencyExchangeBL
    {
        IEnumerable<CurrencyExchange> GetAll();
        CurrencyExchange Save(CurrencyExchange currencyExchange);
        CurrencyExchange GetById(int Id);
        bool Delete(int Id, string Deletedby);
    }
}
