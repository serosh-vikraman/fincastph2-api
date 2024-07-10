using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class UserMaster : BaseEntity
    {
        public int Id { get; set; }
        public int UserMasterId { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string Mobile { get; set; }
        public string Pasword { get; set; }
        public int RoleID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public List<int> ClientIDs { get; set; }
        //public int ClientID { get; set; }
        public string ClientName { get; set; }
        public bool ActiveStatus { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string AdminEmail { get; set; }
        public string planurl { get; set; }
        public string Authorized { get; set; }
        public string subscription_end { get; set; }
        public string PlanName { get; set; }
        public string DataEntryInterval { get; set; }
        
    }
    public class ChangePasswordModel
    {
       public int userId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ForgotPasswordModel
    {
        public string LoginId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
