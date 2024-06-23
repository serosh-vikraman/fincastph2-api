using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ContractStatusViewModel
    {
        public int ContractStatusID { get; set; }
        public string ContractStatusCode { get; set; }
        public string ContractStatusName { get; set; }
        public bool Active { get; set; }

        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}