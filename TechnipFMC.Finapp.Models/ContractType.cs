using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ContractType : BaseEntity
    {
        public int ContractTypeID { get; set; }
        public string ContractTypeCode { get; set; }
        public string ContractTypeName { get; set; }
    }
}
