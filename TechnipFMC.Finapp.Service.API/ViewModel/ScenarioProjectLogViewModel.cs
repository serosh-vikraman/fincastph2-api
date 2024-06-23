using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ScenarioProjectLogViewModel
    {
        public int? ScenarioProjectLogID { get; set; }
        public int ScenarioId { get; set; }
        public int? ProjectId { get; set; }
        public bool Status { get; set; }
        public string WIPLockedBy { get; set; }
        public string CreatedBy { get; set; }
        public int UserId { get; set; }

    }
    public class ScenarioIDSViewModel
    {
        public int ScenarioID { get; set; }
        public string ScenarioName { get; set; }
        public int CreatedBy { get; set; }
    }
}