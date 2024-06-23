using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ClubbingParameter : BaseEntity
    {
        public int ClubbingParameterID { get; set; }
        public string ClubbingParameterCode { get; set; }
        public string ClubbingParameterName { get; set; }
    }
}
