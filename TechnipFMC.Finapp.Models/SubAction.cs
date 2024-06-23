using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class SubAction:BaseEntity
    {
        public int SubActionID { get; set; }
        public string SubActionCode { get; set; }
        public string SubActionName { get; set; }
    }
}
