using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class UserMasterViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserMasterId { get; set; }
        public string EmailID { get; set; }
        public string Mobile { get; set; }
        public string Pasword { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool ActiveStatus { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public List<int> ClientIDs { get; set; }

        public string ClientName { get; set; }
        public int CustomerID { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
        
        public string Token { get; set; }
        public string stripe_status { get; set; }
        public string Authorized { get; set; }
        public string PlanName { get; set; }
    }
    public class SubscriptionDetails
    {
        public string plan_name { get; set; }

        public string currency { get; set; }

        public double? amount { get; set; }

        public int? total_period_days { get; set; }

        public int? number_of_users { get; set; }

        public string statement_description { get; set; }

        public string stripe_id { get; set; }

        public string stripe_product_id { get; set; }

        public string stripe_price { get; set; }

        public string stripe_status { get; set; }
        public string date_cancel_at { get; set; }
        public string date_cancelled_at { get; set; }
        public string subscription_start { get; set; }
        public string subscription_end { get; set; }
    }
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        public string LoginId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}