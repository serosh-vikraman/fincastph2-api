using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ClientViewModel
    {
        public int ClientID { get; set; }
        //public int DepartmentID { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public int CustomerID { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}