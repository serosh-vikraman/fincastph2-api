using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class GroupingParametersViewModel
    {
        public int GroupingParametersID { get; set; }
        public string GroupingParametersCode { get; set; }
        public string GroupingParametersName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}