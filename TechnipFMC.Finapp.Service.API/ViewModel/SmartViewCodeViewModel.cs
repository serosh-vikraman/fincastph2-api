using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class SmartViewCodeViewModel
    {
        public int SmartViewCodeID { get; set; }
        public string SmartViewCode { get; set; }
        public string SmartViewName { get; set; }
        public bool Active { get; set; }

        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}