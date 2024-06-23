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
    public class CountryBL : ICountryBL
    {
        public CountryBL(ICountryRepository countryRepo)
        {
            //_countryRepo = countryRepo;
        }
        public CountryBL()
        { }
        public bool Delete(int Id, string Deletedby)
        {
            CountryRepository _countryRepo = new CountryRepository();
            return _countryRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<Country> GetAll()
        {
            CountryRepository _countryRepo = new CountryRepository();
            return _countryRepo.GetAll();
        }

        public Country GetById(int id )
        {
            CountryRepository _countryRepo = new CountryRepository();
            return _countryRepo.GetById(id);
        }

        public Country Save(Country country)
        {
            CountryRepository _countryRepo = new CountryRepository();
            return _countryRepo.Save(country);
        }

        public Country Update(Country country)
        {
            throw new NotImplementedException();
        }
    }
}
