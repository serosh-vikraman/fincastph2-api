using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class GroupingParameters : BaseEntity
    {
        public int GroupingParametersID { get; set; }
        public string GroupingParametersCode { get; set; }
        public string GroupingParametersName { get; set; }
    }
}
