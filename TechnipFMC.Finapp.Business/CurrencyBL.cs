using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Business.Interfaces;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class CurrencyBL : ICurrencyBL
    {
        public CurrencyBL()
        { }
        public bool Delete(int Id, string Deletedby)
        {
            CurrencyRepository _currencyRepository = new CurrencyRepository();
            return _currencyRepository.Delete(Id, Deletedby);
        }
        public IEnumerable<Currency> GetAll()
        {
            CurrencyRepository _currencyRepository = new CurrencyRepository();
            return _currencyRepository.GetAll();
        }

        public Currency GetById(int Id )
        {
            CurrencyRepository _currencyRepository = new CurrencyRepository();
            return _currencyRepository.GetById(Id );
        }

        public Currency Save(Currency currency)
        {
            CurrencyRepository _currencyRepository = new CurrencyRepository();
            return _currencyRepository.Save(currency);
        }
    }
}
