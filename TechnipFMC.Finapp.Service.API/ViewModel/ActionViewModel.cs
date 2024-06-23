using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ActionViewModel
    {
        public int ActionID { get; set; }
        public string ActionCode { get; set; }
        public string ActionName { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
    }
}