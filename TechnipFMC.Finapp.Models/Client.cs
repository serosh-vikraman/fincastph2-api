using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class Client : BaseEntity
    {
        public int ClientID { get; set; }
        //public int DepartmentID { get; set; }
        public string ClientName { get; set; } 
        public string ClientCode { get; set; }
    }
}
