using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class BUCategory : BaseEntity
    {
        public int BUCategoryID { get; set; }
        public string BUCategoryCode { get; set; }
        public string BUCategoryName { get; set; }
    }
}
