using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class StatutoryCategory:BaseEntity
    {
        public int StatutoryCategoryID { get; set; }
        public string StatutoryCategoryCode { get; set; }
        public string StatutoryCategoryName { get; set; }
    }
}
