using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class CurrencyViewModel
    {
        public int CurrencyID { get; set; }       
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        //public string CurrencySymbol { get; set; }
        public int CreatedBy { get; set; }
        public bool Active { get; set; }

        public string Status { get; set; }
    }
}