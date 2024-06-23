using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        IEnumerable<SignUpCountry> GetAllCountries();
        Customer Save(Customer customer);
        int Signup(Customer customer);
        string GetPlanUrl();
        VerifyCustomer VerifyEmail(string loginId, string token);
        Customer GetById(int Id);

        Customer Update(Customer customer);

        bool Delete(int Id, string DeletedBy);
        PlanSpecs GetPlanDetails(int Id);
        VerifyCustomer GetPlanDetailsofuser(string loginId, string token);
        string GetPaymentLink(string planname, string plantype);
        string Getrenewlink(string Email);
        Customer ResendEmail(string email);
        List<PlanDetails> getallplans();
    }
}
