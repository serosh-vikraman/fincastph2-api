using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class BillingType : BaseEntity
    {
        public int BillingTypeID { get; set; }
        public string BillingTypeCode { get; set; }
        public string BillingTypeName { get; set; }
    }
}
