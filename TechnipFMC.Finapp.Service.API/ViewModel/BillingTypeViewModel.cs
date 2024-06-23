using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class BillingTypeViewModel
    {
        public int BillingTypeID { get; set; }
        public string BillingTypeCode { get; set; }
        public string BillingTypeName { get; set; }
        public bool Active { get; set; }

        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}