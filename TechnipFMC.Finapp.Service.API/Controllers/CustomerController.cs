using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using TechnipFMC.Common;
using TechnipFMC.Finapp.Business;
using TechnipFMC.Finapp.Models;
using TechnipFMC.Finapp.Service.API.ViewModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Web.Helpers;

namespace TechnipFMC.Finapp.Service.API.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly ICustomerBL _customerBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CustomerBL"></param>
        public CustomerController(ICustomerBL customerBL)
        {
            _customerBL = customerBL;

        }

        /// <summary>
        /// Get All Customer 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/customers/getall")]
        public HttpResponseMessage GetAllCustomers()
        {
            try
            {
                List<Customer> customers = _customerBL.GetAll().ToList();
                List<CustomerViewModel> customersModel = new List<CustomerViewModel>();
                Mapper.Map(customers, customersModel);

                return Request.CreateResponse<APIResponse<List<CustomerViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<CustomerViewModel>>(HttpStatusCode.OK, customersModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetAllCustomers", "");

                return Request.CreateResponse<APIResponse<List<CustomerViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CustomerViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<Customer> customers = _customerBL.GetAll().ToList();
                List<CustomerViewModel> customersModel = new List<CustomerViewModel>();
                var entities = customers.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, customersModel);

                return Request.CreateResponse<APIResponse<List<CustomerViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<CustomerViewModel>>(HttpStatusCode.OK, customersModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetAllCustomers", "");

                return Request.CreateResponse<APIResponse<List<CustomerViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CustomerViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/getbyid/{Id}")]
        public HttpResponseMessage GetCustomerById(int Id)
        {
            try
            {
                Customer customers = _customerBL.GetById(Id);
                CustomerViewModel customersModel = new CustomerViewModel();
                Mapper.Map(customers, customersModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.OK, customersModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetCustomerById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/save")]
        public HttpResponseMessage SaveCustomer(CustomerViewModel customerViewModel)
        {
            try
            {

                Customer customerDatamodel = new Customer();
                //customerViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(customerViewModel, customerDatamodel);
                var Customer = _customerBL.Save(customerDatamodel);
                var message = "";
                if (Customer != null && Customer.CustomerID > 0)
                {
                    customerViewModel.CustomerID = Customer.CustomerID;
                    if (customerDatamodel.Active == true)
                    {
                        customerViewModel.Status = "Active";
                    }
                    else
                    {
                        customerViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("CUSS");//Customer saved succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<CustomerViewModel>(HttpStatusCode.OK, customerViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CUNS");//Customer could not be saved
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY constraint 'UK_Customer_Email'"))
                {
                    var message = new CommonBL().GetMessage("EAR");//Customer code or name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else if (ex.Message.Contains("UNIQUE KEY constraint 'UK_Customer_CustomerName'"))
                {
                    var message = new CommonBL().GetMessage("CUSCNAU");//Customer code or name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "SaveCustomer", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "SaveCustomer", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/signup")]
        public HttpResponseMessage Signup(CustomerViewModel customerViewModel)
        {
            try
            {

                Customer customerDatamodel = new Customer();
                //customerViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(customerViewModel, customerDatamodel);
                int Customer = _customerBL.Signup(customerDatamodel);
                var message = "";
                if (Customer  > 0)
                {
                    
                    message = new CommonBL().GetMessage("CUSS");//Customer saved succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<CustomerViewModel>(HttpStatusCode.OK, customerViewModel, null, message, "", ""));

                }
                else
                {
                    //message = new CommonBL().GetMessage("CUNS");//Customer could not be saved
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, "Customer Code is already taken. Please choose another Code.", "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY constraint 'UK_Customer_Email'"))
                {
                    var message = new CommonBL().GetMessage("EAR");//Customer code or name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else if (ex.Message.Contains("UNIQUE KEY constraint 'UK_Customer_CustomerName'"))
                {
                    var message = new CommonBL().GetMessage("CUSCNAU");//Customer code or name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "SaveCustomer", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "SaveCustomer", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/initialsignup")]
        public async Task<HttpResponseMessage> InitialSignup(CustomerViewModel customerViewModel)
        {
            try
            {

                Customer customerDatamodel = new Customer();
                //customerViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(customerViewModel, customerDatamodel);
                int Customer = await _customerBL.InitialSignup(customerDatamodel);
                var message = "";
                if (Customer > 0)
                {

                    message = new CommonBL().GetMessage("CUSS");//Customer saved succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<CustomerViewModel>(HttpStatusCode.OK, customerViewModel, null, message, "", ""));

                }
                else
                {
                    if (Customer == -1) message = "Email Not Verified";
                    else if (Customer == -2) message = "Email Verified";
                    else  message = new CommonBL().GetMessage("CUNS");//Customer could not be saved
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY constraint 'UQ_UserMaster_Email'"))
                {
                    //var message = new CommonBL().GetMessage("EAR");//Customer code or name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, "Email already exist. Please login.", "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "InitialSignup", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "InitialSignup", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }



        [HttpPost]
        [Route("api/customers/update")]
        public HttpResponseMessage UpdateCustomer(CustomerViewModel customerViewModel)
        {
            try
            {

                Customer customerDatamodel = new Customer();
                //customerViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(customerViewModel, customerDatamodel);
                var Customer = _customerBL.Update(customerDatamodel);
                var message = "";
                if (Customer != null && Customer.CustomerID > 0)
                {
                    customerViewModel.CustomerID = Customer.CustomerID;
                    if (customerDatamodel.Active == true)
                    {
                        customerViewModel.Status = "Active";
                    }
                    else
                    {
                        customerViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("CUSS");//Customer saved succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<CustomerViewModel>(HttpStatusCode.OK, customerViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CUNS");//Customer could not be saved
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY constraint 'UK_Customer_Email'"))
                {
                    var message = new CommonBL().GetMessage("EAR");//Customer code or name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else if (ex.Message.Contains("UNIQUE KEY constraint 'UK_Customer_CustomerName'"))
                {
                    var message = new CommonBL().GetMessage("CUSCNAU");//Customer code or name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "SaveCustomer", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "SaveCustomer", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }


        [HttpPost]
        [Route("api/customers/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteCustomer(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _customerBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.CustomerController", "DeleteCustomer", "Deleted Customer Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("CUSDS");//Customer deleted Succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CUSDNS");//Customer could not be deleted
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CUSDNS");//Customer could not be deleted
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "DeleteCustomer", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "DeleteCustomer", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/verifyemail")]
        public HttpResponseMessage VerifyEmail(VerifyEmailModel body)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                VerifyCustomer customer = _customerBL.VerifyEmail(body.loginId, body.token);
                var message = "";
                if (customer.CustomerID > 0)
                {
                    RaintelsLogManager.Info("Service.API.CustomerController", "VerifyEmail", "Verified Customer Id=" + customer.CustomerID  );
                    message = "Email Verified Successfully";
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<VerifyCustomer>(HttpStatusCode.OK, customer, null, message, "", ""));

                }
                else
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.CustomerController", "VerifyEmail", "Invalid Token for Customer Id=" + customer.CustomerID);
                    message = customer.Link;
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<VerifyCustomer>(HttpStatusCode.NotFound, customer, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CUSDNS");//Customer could not be deleted
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "VerifyEmail", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "VerifyEmail", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/getallcountries")]
        public HttpResponseMessage GetAllCountries()
        {
            try
            {
                List<SignUpCountry> countries = _customerBL.GetAllCountries().ToList();
                List<SignUpCountryViewModel> countriesview = new List<SignUpCountryViewModel>();
                Mapper.Map(countries, countriesview);

                return Request.CreateResponse<APIResponse<List<SignUpCountryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<SignUpCountryViewModel>>(HttpStatusCode.OK, countriesview, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetAllCountries", "");

                return Request.CreateResponse<APIResponse<List<CustomerViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CustomerViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        
        
        [Route("api/customers/getallinvoices")]
        [HttpPost]
        public async Task<HttpResponseMessage> Getallinvoices([FromBody] UserMaster obj)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();

                //Dictionary<string, string> userInfo = LDAPQuery.GetUserInfo(User.Identity.Name);
                //if (userInfo["LoginUserName"] != null)
                //{
                //var user = _userMasterBL.ValidateUser(userInfo["LoginUserName"]);

               //InvoiceDetails invoiceList = new InvoiceDetails();
             

                string url = _customerBL.GetPlanUrl();
                url = url + "api/listinvoice";
                
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("accept", "*/*");
                    request.Headers.Add("Accept", "application/json");
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.Headers.Add("X-Authorization", "BxybvQb457YhdKC7HduyxUnN5l4auMvEVF2HOfkOAIJbKTu6G4rPgLfOM2vkrvxV");
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(obj.EmailID), "email");
                    content.Add(new StringContent("fincast"), "product_slugname");
                request.Content = content;

                    var res = await client.SendAsync(request);
                    res.EnsureSuccessStatusCode();
                    
                string jsonResponse = await res.Content.ReadAsStringAsync();
                JObject parsedResponse = JObject.Parse(jsonResponse);
                //JToken parsedResponse = JToken.Parse(jsonResponse);
                //invoiceList = JsonConvert.DeserializeObject<InvoiceDetails>(jsonResponse);

                List<InvoiceDetails> invoiceList = parsedResponse
     .Properties()
     .Select(property => property.Value.ToObject<InvoiceDetails>())
     .ToList();
                return Request.CreateResponse(HttpStatusCode.OK,
                     new APIResponse<List<InvoiceDetails>>(HttpStatusCode.OK, invoiceList, null, "", "", ""));
              


            }
            catch (Exception ex)
            {
                try
                {
                    //HttpResponseMessage response = new HttpResponseMessage();
                    //Dictionary<string, string> userInfo = new Dictionary<string, string>();
                    //userInfo.Add("LoginUserName", "admin");
                    //userInfo.Add("TenantId", "fincast");
                    //var token = TokenValidationHandler.GenerateToken("admin", userInfo);
                    //userInfo.Add("token", token.ToString());
                    //response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                    RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "Getallinvoices", "", "Success", "", "", 0, 0, 0, 0, 0, 0, "");
                    //return response;

                }
                catch (Exception)
                {

                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }
        [HttpPost]
        [Route("api/customers/listSubscriptions")]
        public async Task<HttpResponseMessage> ListSubscriptions([FromBody] UserMaster obj)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();


                SubscriptionDetails subscriptiondetail = new SubscriptionDetails();


                string url = _customerBL.GetPlanUrl();
                url = url + "api/listsubscription";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("X-Authorization", "5UnHob5Q0NUl4ICXf2c9AYG3EeFBNYhiNU0X7eTvX6I6cDUbobLy0cozTNbZ2YIS");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(obj.EmailID), "email");
                content.Add(new StringContent("fincast"), "product_slugname");
                request.Content = content;

                var res = await client.SendAsync(request);
                res.EnsureSuccessStatusCode();
                string jsonResponse = await res.Content.ReadAsStringAsync();
                subscriptiondetail = JsonConvert.DeserializeObject<SubscriptionDetails>(jsonResponse);
                return Request.CreateResponse(HttpStatusCode.OK,
                       new APIResponse<SubscriptionDetails>(HttpStatusCode.OK, subscriptiondetail, null, "", "", ""));



            }
            catch (Exception ex)
            {
                try
                {
                    //HttpResponseMessage response = new HttpResponseMessage();
                    //Dictionary<string, string> userInfo = new Dictionary<string, string>();
                    //userInfo.Add("LoginUserName", "admin");
                    //userInfo.Add("TenantId", "fincast");
                    //var token = TokenValidationHandler.GenerateToken("admin", userInfo);
                    //userInfo.Add("token", token.ToString());
                    //response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                    RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "User Info", "", "Success", "", "", 0, 0, 0, 0, 0, 0, "");
                    //return response;

                }
                catch (Exception)
                {

                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }
        [HttpPost]
        [Route("api/customers/cancelSubscription")]
        public async Task<HttpResponseMessage> CancelSubscription([FromBody] SubscriptionDetails detail, string LoginId)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();

                string url = _customerBL.GetPlanUrl();
                url = url + "api/cancelsubscription";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("X-Authorization", "dKidkf15VcbdUE9BSNakmS2nBcEx7aEkoDG15ocXILFanlaQ0adtuaerQj1UGShr");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(LoginId), "email");
                content.Add(new StringContent("fincast"), "product_slugname");
                //content.Add(new StringContent(detail.price_id), "price_id");
                //content.Add(new StringContent(detail.subscription_id), "subscription_id ");
                request.Content = content;

                var res = await client.SendAsync(request);
                res.EnsureSuccessStatusCode();
                string jsonResponse = await res.Content.ReadAsStringAsync();
                detail = JsonConvert.DeserializeObject<SubscriptionDetails>(jsonResponse);
                return Request.CreateResponse(HttpStatusCode.OK,
                       new APIResponse<SubscriptionDetails>(HttpStatusCode.OK, detail, null, "Subscription cancelled succesfully!", "", ""));



            }
            catch (Exception ex)
            {
                try
                {
                    //HttpResponseMessage response = new HttpResponseMessage();
                    //Dictionary<string, string> userInfo = new Dictionary<string, string>();
                    //userInfo.Add("LoginUserName", "admin");
                    //userInfo.Add("TenantId", "fincast");
                    //var token = TokenValidationHandler.GenerateToken("admin", userInfo);
                    //userInfo.Add("token", token.ToString());
                    //response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                    RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "User Info", "", "Success", "", "", 0, 0, 0, 0, 0, 0, "");
                    //return response;

                }
                catch (Exception)
                {

                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }
        [HttpPost]
        [Route("api/customers/listPlan")]
        public async Task<HttpResponseMessage> ListPlans([FromBody] UserMaster obj)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();

                List<PlanDetails> plandetails = new List<PlanDetails>();


                string url = _customerBL.GetPlanUrl();
                url = url + "api/planlist";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("X-Authorization", "qkA2UiLc7eGFDUZk3MPUnGJ7mCFbrQbFE9pfDAqN7hQBfevkrRAF9wF7gtl1x2QU");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(obj.EmailID), "email");
                content.Add(new StringContent("fincast"), "product_slugname");
                request.Content = content;

                var res = await client.SendAsync(request);
                res.EnsureSuccessStatusCode();
                string jsonResponse = await res.Content.ReadAsStringAsync();
                JObject parsedResponse = JObject.Parse(jsonResponse);
                //JToken parsedResponse = JToken.Parse(jsonResponse);
                //invoiceList = JsonConvert.DeserializeObject<InvoiceDetails>(jsonResponse);

                List<PlanDetails> planList = parsedResponse
     .Properties()
     .Select(property => property.Value.ToObject<PlanDetails>())
     .ToList();
                //plandetails = JsonConvert.DeserializeObject<List<PlanDetails>>(jsonResponse);
                return Request.CreateResponse(HttpStatusCode.OK,
                       new APIResponse<List<PlanDetails>>(HttpStatusCode.OK, planList, null, "", "", ""));



            }
            catch (Exception ex)
            {
                try
                {
                    //HttpResponseMessage response = new HttpResponseMessage();
                    //Dictionary<string, string> userInfo = new Dictionary<string, string>();
                    //userInfo.Add("LoginUserName", "admin");
                    //userInfo.Add("TenantId", "fincast");
                    //var token = TokenValidationHandler.GenerateToken("admin", userInfo);
                    //userInfo.Add("token", token.ToString());
                    //response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                    RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "User Info", "", "Success", "", "", 0, 0, 0, 0, 0, 0, "");
                    //return response;

                }
                catch (Exception)
                {

                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }
        [HttpPost]
        [Route("api/customers/checkplans")]
        public async Task<HttpResponseMessage> GetPlansList([FromBody] UserMaster obj)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();

                List<PlanDetails> plandetails = new List<PlanDetails>();


                string url = _customerBL.GetPlanUrl();
                url = url + "api/checkplans";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("X-Authorization", "sglgvE0a0wvHNBhyT72ub4UHHn4Xk4JsgLr5WxxwWYjBzCUmZR4ZPn95KG0e5tmx");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(obj.EmailID), "email");
                content.Add(new StringContent("fincast"), "product_slugname");
                request.Content = content;

                var res = await client.SendAsync(request);
                res.EnsureSuccessStatusCode();
                string jsonResponse = await res.Content.ReadAsStringAsync();
                plandetails = JsonConvert.DeserializeObject<List<PlanDetails>>(jsonResponse);
                return Request.CreateResponse(HttpStatusCode.OK,
                       new APIResponse<List<PlanDetails>>(HttpStatusCode.OK, plandetails, null, "", "", ""));



            }
            catch (Exception ex)
            {
                try
                {
                    //HttpResponseMessage response = new HttpResponseMessage();
                    //Dictionary<string, string> userInfo = new Dictionary<string, string>();
                    //userInfo.Add("LoginUserName", "admin");
                    //userInfo.Add("TenantId", "fincast");
                    //var token = TokenValidationHandler.GenerateToken("admin", userInfo);
                    //userInfo.Add("token", token.ToString());
                    //response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                    RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "User Info", "", "Success", "", "", 0, 0, 0, 0, 0, 0, "");
                    //return response;

                }
                catch (Exception)
                {

                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }
        [HttpPost]
        [Route("api/customers/checksubscription")]
        public async Task<HttpResponseMessage> GetSubscription([FromBody] UserMaster obj)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();

                PlanDetails plandetails = new PlanDetails();


                string url = _customerBL.GetPlanUrl();
                url = url + "api/checksubscription";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("X-Authorization", "AlXoSUIrs9QSXVjMiDkpLX5ipqJUb1tPkKoEKKAIV22X70YkTH5ftXzaN5ryddl5");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(obj.EmailID), "email");
                content.Add(new StringContent("fincast"), "product_slugname");
                request.Content = content;

                var res = await client.SendAsync(request);
                res.EnsureSuccessStatusCode();
                string jsonResponse = await res.Content.ReadAsStringAsync();
                if(jsonResponse != "[]") plandetails = JsonConvert.DeserializeObject<PlanDetails>(jsonResponse);
                return Request.CreateResponse(HttpStatusCode.OK,
                       new APIResponse<PlanDetails>(HttpStatusCode.OK, plandetails, null, "", "", ""));



            }
            catch (Exception ex)
            {
                try
                {
                    //HttpResponseMessage response = new HttpResponseMessage();
                    //Dictionary<string, string> userInfo = new Dictionary<string, string>();
                    //userInfo.Add("LoginUserName", "admin");
                    //userInfo.Add("TenantId", "fincast");
                    //var token = TokenValidationHandler.GenerateToken("admin", userInfo);
                    //userInfo.Add("token", token.ToString());
                    //response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                    RaintelsActivityManager.ActivityLog(0, 0, 0, "checksubscription", "GetSubscription", "", "Error", "", "", 0, 0, 0, 0, 0, 0, "");
                    //return response;

                }
                catch (Exception)
                {

                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }
        [HttpPost]
        [Route("api/customers/getnewsubscription")]
        public async Task<HttpResponseMessage> ChangePlan([FromBody] PlanDetails plan)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();
                List<PlanDetails> plandetails = new List<PlanDetails>();

                string url = _customerBL.GetPlanUrl();
                url = url + "api/getnewsubscription";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("X-Authorization", "z7mTEy8mRZJKuwWGZFLrHSAdXTAHm5ZyJJObZKgMvay9k1uchig1yPVhpgLu2CpV");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(plan.email), "email");
                content.Add(new StringContent(plan.plan_name), "plan_name");
                content.Add(new StringContent(plan.product_code), "product_code");
                content.Add(new StringContent("fincast"), "product_slugname");
                request.Content = content;

                var res = await client.SendAsync(request);
                res.EnsureSuccessStatusCode();
                string plan_name_new = await res.Content.ReadAsStringAsync();
                // string plan_name_new = JsonConvert.DeserializeObject<List<PlanDetails>>(jsonResponse);
                var msg = "Plan changed to " + plan_name_new + "successfully.";
                return Request.CreateResponse(HttpStatusCode.OK,
                       new APIResponse<string>(HttpStatusCode.OK, plan_name_new, null, msg, "", ""));



            }
            catch (Exception ex)
            {
                try
                {
                    //HttpResponseMessage response = new HttpResponseMessage();
                    //Dictionary<string, string> userInfo = new Dictionary<string, string>();
                    //userInfo.Add("LoginUserName", "admin");
                    //userInfo.Add("TenantId", "fincast");
                    //var token = TokenValidationHandler.GenerateToken("admin", userInfo);
                    //userInfo.Add("token", token.ToString());
                    //response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                    RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "User Info", "", "Success", "", "", 0, 0, 0, 0, 0, 0, "");
                    //return response;

                }
                catch (Exception)
                {

                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }
        [HttpPost]
        [Route("api/customers/getplan")]
        public async Task<HttpResponseMessage> GetPlan([FromBody] UserMaster obj)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();

                SubscriptionDetails subscriptiondetail = new SubscriptionDetails();


                string url = _customerBL.GetPlanUrl();
                url = url + "api/getplan";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("X-Authorization", "B78QZhy7rGG9EJvBQTa9bzVYzg4NT9jRdQ5JMV7rgmiDWCv0iBuaub4SAEXCBbP2");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(obj.EmailID), "email");
                content.Add(new StringContent("fincast"), "product_slugname");
                request.Content = content;

                var res = await client.SendAsync(request);
                res.EnsureSuccessStatusCode();
                string jsonResponse = await res.Content.ReadAsStringAsync();
                subscriptiondetail = JsonConvert.DeserializeObject<SubscriptionDetails>(jsonResponse);
                return Request.CreateResponse(HttpStatusCode.OK,
                       new APIResponse<SubscriptionDetails>(HttpStatusCode.OK, subscriptiondetail, null, "", "", ""));



            }
            catch (Exception ex)
            {
                try
                {
                   
                    RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "User Info", "", "Success", "", "", 0, 0, 0, 0, 0, 0, "");
                    //return response;

                }
                catch (Exception)
                {

                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }
        [HttpPost]
        [Route("api/customers/getplandetails/{CustomerId}")]
        public HttpResponseMessage GetPlanDetails(int CustomerId)
        {
            try
            {
                PlanSpecs plan = _customerBL.GetPlanDetails(CustomerId);
                //CustomerViewModel customersModel = new CustomerViewModel();
                //Mapper.Map(customers, customersModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<PlanSpecs>(HttpStatusCode.OK, plan, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetCustomerById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/getplandetailsofuser")]
        public HttpResponseMessage GetPlanDetailsofuser(VerifyEmailModel body)
        {
            try
            {
                VerifyCustomer customer = _customerBL.GetPlanDetailsofuser(body.loginId,body.token);
                var message = "";
                if (customer.CustomerID > 0)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.CustomerController", "VerifyEmail", "Verified Customer Id=" + customer.CustomerID);
                    message = "Email Verified Successfully";
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<VerifyCustomer>(HttpStatusCode.OK, customer, null, message, "", ""));

                }
                else
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.CustomerController", "VerifyEmail", "Invalid Token for Customer Id=" + customer.CustomerID);
                    message = customer.Link;
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<VerifyCustomer>(HttpStatusCode.NotFound, customer, null, message, "", ""));

                }           


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetCustomerById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/getpaymentlink")]
        public HttpResponseMessage GetPaymentLink(PlanModel body)
        {
            try
            {
                string planlink = _customerBL.GetPaymentLink(body.PlanName, body.PlanType);
                var message = "";
                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<string>(HttpStatusCode.OK, planlink, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetCustomerById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/getrenewlink")]
        public HttpResponseMessage Getrenewlink(VerifyEmailModel body)
        {
            try
            {
                string planlink = _customerBL.Getrenewlink(body.loginId);
                var message = "";
                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<string>(HttpStatusCode.OK, planlink, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetCustomerById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/resendemail")]
        public async Task<HttpResponseMessage> ResendEmail(VerifyEmailModel body)
        {
            try
            {
                Customer customer  = await _customerBL.ResendEmail(body.loginId);
                var message = "";
                if(customer.CustomerID == -1)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<Customer>(HttpStatusCode.InternalServerError, customer, null, "Email could not be sent", "", ""));
                }
                else{
                    return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<Customer>(HttpStatusCode.OK, customer, null, "", "", ""));
                }
                


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetCustomerById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CustomerViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/customers/gatallplans")]
        public async Task<HttpResponseMessage> getallplans([FromBody] UserMaster obj)
        {
            try
            
            {
                List<PlanDetailsViewModel> plandetailsview = new List<PlanDetailsViewModel>();
                List<PlanDetails> plandetails = new List<PlanDetails>();
                
                plandetails = _customerBL.getallplans();
                Mapper.Map(plandetails, plandetailsview);
                return Request.CreateResponse<APIResponse<List<PlanDetailsViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<PlanDetailsViewModel>>(HttpStatusCode.OK, plandetailsview, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CustomerController", "GetAllCustomers", "");

                return Request.CreateResponse<APIResponse<List<CustomerViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CustomerViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }

        }
    }
}
