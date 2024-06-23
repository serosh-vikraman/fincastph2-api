using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class UserRole:BaseEntity
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
    }
    public class UserProjects : BaseEntity
    {
        public int UserId { get; set; }
        public List<UserProject> UserProject { get; set; }
    }
    public class UserProject
    {
        public int UserProjectId { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class UserDepartment : BaseEntity
    {
        public int UserId { get; set; }
        public List<int> DepartmentId { get; set; }
    }
}
