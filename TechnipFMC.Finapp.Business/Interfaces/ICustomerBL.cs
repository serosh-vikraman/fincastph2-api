using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface ICustomerBL
    {
        IEnumerable<Customer> GetAll();
        IEnumerable<SignUpCountry> GetAllCountries();
        Customer Save(Customer customer);
        int Signup(Customer customer);
        Task<int> InitialSignup(Customer customer);
        VerifyCustomer VerifyEmail(string loginId,string token);
        Customer GetById(int Id);
        string GetPlanUrl();

        Customer Update(Customer customer);

        bool Delete(int Id, string Deletedby);
        PlanSpecs GetPlanDetails(int Id);
        VerifyCustomer GetPlanDetailsofuser(string loginId, string token);
        string GetPaymentLink(string planname, string plantype);
        string Getrenewlink(string Email);
        Task<Customer> ResendEmail(string email);
        List<PlanDetails> getallplans();
    }
}
