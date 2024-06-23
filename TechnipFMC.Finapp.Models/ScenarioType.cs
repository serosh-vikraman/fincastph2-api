using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ScenarioType:BaseEntity
    {
        public int ScenarioTypeID { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string ScenarioTypeName { get; set; }
    }
}
