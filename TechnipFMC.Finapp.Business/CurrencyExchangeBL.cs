using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Business.Interfaces;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class CurrencyExchangeBL: ICurrencyExchangeBL
    {
        
        public CurrencyExchangeBL()
        { }
        public bool Delete(int Id, string Deletedby)
        {
            CurrencyExchangeRepository _currencyExchangeRepository = new CurrencyExchangeRepository();
            return _currencyExchangeRepository.Delete(Id, Deletedby);
        }
        public IEnumerable<CurrencyExchange> GetAll()
        {
            CurrencyExchangeRepository _currencyExchangeRepository = new CurrencyExchangeRepository();
            return _currencyExchangeRepository.GetAll();
        }

        public CurrencyExchange GetById(int Id )
        {
            CurrencyExchangeRepository _currencyExchangeRepository = new CurrencyExchangeRepository();
            return _currencyExchangeRepository.GetById(Id );
        }

        public CurrencyExchange Save(CurrencyExchange currencyExchange)
        {
            CurrencyExchangeRepository _currencyExchangeRepository = new CurrencyExchangeRepository();
            return _currencyExchangeRepository.Save(currencyExchange);
        }
    }
}
