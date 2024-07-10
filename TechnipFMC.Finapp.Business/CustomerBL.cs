using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class CustomerBL : ICustomerBL
    {
        public CustomerBL(ICustomerRepository customerRepo)
        {
            //_CustomerRepo = CustomerRepo;
        }
        public CustomerBL()
        { }
        public bool Delete(int Id, string Deletedby)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<Customer> GetAll()
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.GetAll();
        }
        public IEnumerable<SignUpCountry> GetAllCountries()
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.GetAllCountries();
        }

        public Customer GetById(int id)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.GetById(id);
        }

        public Customer Save(Customer customer)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.Save(customer);
        }
        public int Signup(Customer customer)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.Signup(customer);
        }
        public Task<int> InitialSignup(Customer customer)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return  _customerRepo.InitialSignup(customer);
        }
        public VerifyCustomer VerifyEmail(string loginId, string token)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.VerifyEmail(loginId, token);
        }
        public string GetPlanUrl()
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.GetPlanUrl();
        }

        public Customer Update(Customer customer)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.Update(customer);
        }
        public PlanSpecs GetPlanDetails(int id)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.GetPlanDetails(id);
        }
        public VerifyCustomer GetPlanDetailsofuser(string loginId, string token)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.GetPlanDetailsofuser(loginId, token);
        }
        public string GetPaymentLink(string planname, string plantype)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.GetPaymentLink(planname, plantype);
        }
        public string Getrenewlink(string loginId)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.Getrenewlink(loginId);
        }
        public Task<Customer> ResendEmail(string email)
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.ResendEmail(email);
        }
        public List<PlanDetails> getallplans()
        {
            CustomerRepository _customerRepo = new CustomerRepository();
            return _customerRepo.getallplans();
        }
    }
}
