using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ContractTypeViewModel
    {
        public int ContractTypeID { get; set; }
        public string ContractTypeCode { get; set; }
        public string ContractTypeName { get; set; }
        public bool Active { get; set; }

        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}