using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class DepartmentViewModel
    {
        public int DepartmentID { get; set; }
        public int CustomerID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}