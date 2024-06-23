using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ContractStatus : BaseEntity
    {
        public int ContractStatusID { get; set; }
        public string ContractStatusCode { get; set; }
        public string ContractStatusName { get; set; }
    }
}
