using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class CurrencyExchangeViewModel
    {
        public int Id { get; set; }
        public int SourceCurrencyID { get; set; }
        public int TargetCurrencyID { get; set; }
        public int Year { get; set; }
        public string Quarter { get; set; }
        public decimal AverageRate { get; set; }
        public bool LockStatus { get; set; }
        public bool CancelStatus { get; set; }
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }
        public int CreatedBy { get; set; }
        public bool Active { get; set; }
        public string Status { get; set; }

        public string CancelActiveStatus { get; set; }
        public int CustomerID { get; set; }

    }
}