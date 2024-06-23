using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class Country:BaseEntity
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}
