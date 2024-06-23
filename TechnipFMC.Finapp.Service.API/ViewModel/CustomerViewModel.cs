using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class CustomerViewModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Pasword { get; set; }
        public string CountryName { get; set; }
        public int CountryID { get; set; }
        public string Admin { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }        
        public bool Active { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
    }
    public class VerifyEmailModel
    {
        public string loginId { get; set; }
        public string token { get; set; }
    }
    public class PlanModel
    {
        public string PlanName { get; set; }
        public string PlanType { get; set; }
    }
    public class SignUpCountryViewModel
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
    public class InvoiceDetails
    {
        public string invoice_date { get; set; }
        public string invoice_number { get; set; }
        public string plan_name { get; set; }
        public string currency { get; set; }
        public double amount { get; set; }
        public string inv_id   { get; set; }
        public string invoice_pdf_list { get; set; }

    }
    public class PlanDetailsViewModel
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

        public double amount { get; set; }
        public int? number_of_users { get; set; }
        public string product_code { get; set; }
        public string payment_interval { get; set; }
        public string Description { get; set; }
        public string link { get; set; }

    }
}