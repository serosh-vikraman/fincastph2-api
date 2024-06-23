using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ScenarioTypeViewModel
    {
        public int ScenarioTypeID { get; set; }
        public string ScenarioTypeCode { get; set; }
        public string ScenarioTypeName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}