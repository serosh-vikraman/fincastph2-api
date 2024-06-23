using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class SmartViewCodeMaster:BaseEntity
    {
        public int SmartViewCodeID { get; set; }
        public string SmartViewCode { get; set; }
        public string SmartViewName { get; set; }
    }
}
