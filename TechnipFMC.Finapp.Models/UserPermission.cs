using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class UserPermission:BaseEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string Pasword { get; set; }
        public int RoleID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public bool ActiveStatus { get; set; }
        public string RoleCode { get; set; }
    }
}
