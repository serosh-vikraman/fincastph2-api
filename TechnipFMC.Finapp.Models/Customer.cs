using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class Customer : BaseEntity
    {
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Pasword { get; set; }
        public string CountryName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string Admin { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public string PaymentLink { get; set; }
        public int CountryID { get; set; }
        
    }
    public class PlanDetails
    {
        public string stripe_product { get; set; }
        public string email { get; set; }

        public string price_id { get; set; }

        public string subscription_item_id { get; set; }

        public string subscription_id { get; set; }

        public int active_flag { get; set; }
        public int plan_id { get; set; }
        public string plan_name { get; set; }

        public string currency { get; set; }

        public double? amount { get; set; }
        public int? number_of_users { get; set; }
        public string product_code { get; set; }
        public string payment_interval { get; set; }
        public string Description { get; set; }
        public string link { get; set; }


    }
    public class VerifyCustomer 
    {
        public string Email { get; set; }
        public string Admin { get; set; }
        public int CustomerID { get; set; }
        public string Link { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public string OrgName { get; set; }
    }
    public class SignUpCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
    public class PlanSpecs
    {
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfScenarios { get; set; }
        public int NumberOfDepartments { get; set; }
        public int NumberOfProjects { get; set; }
    }
}
