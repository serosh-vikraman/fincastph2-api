using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class UserPermissionViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string Pasword { get; set; }
        public int RoleID { get; set; }
        public bool ActiveStatus { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int CustomerID { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}