using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ProjectEntityViewModel
    {
        public int ProjectEntityID { get; set; }
        public string ProjectEntityCode { get; set; }
        public string ProjectEntityName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}