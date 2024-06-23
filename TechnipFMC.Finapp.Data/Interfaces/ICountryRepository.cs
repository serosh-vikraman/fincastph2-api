using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAll();
        Country Save(Country country);
        Country GetById(int Id );

        Country Update(Country country);

        bool Delete(int Id, string DeletedBy);
    }
}
