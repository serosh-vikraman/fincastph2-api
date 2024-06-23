using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ManagementCategory : BaseEntity
    {
        public int ManagementCategoryID { get; set; }
        public string ManagementCategoryCode { get; set; }
        public string ManagementCategoryName { get; set; }
    }
}
