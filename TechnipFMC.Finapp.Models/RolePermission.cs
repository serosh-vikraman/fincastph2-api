using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class RolePermission:BaseEntity
    {
        public int Id { get; set; }
        public string RoleCode { get; set; }
        public string ActionCode { get; set; }
        public string SubActionCode { get; set; }
    }
}
   