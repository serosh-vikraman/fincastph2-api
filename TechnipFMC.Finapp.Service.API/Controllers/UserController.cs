using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechnipFMC.Common;
using TP.Utility.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechnipFMC.Finapp.Business;
using TechnipFMC.Finapp.Service.API.ViewModel;
using TechnipFMC.Finapp.Models;
using AutoMapper;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Mailgun.Extensions;

namespace TechnipFMC.Finapp.Service.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    ////[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private readonly IUserMasterBL _userMasterBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userMasterBL"></param>
        /// 

        public UserController(IUserMasterBL userMasterBL)
        {
            _userMasterBL = userMasterBL;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/user/get_Userinfo/{LoginId}/{Password}")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetUserInfo(string loginId, string Password)
        {
            try
            {

                HttpResponseMessage response = new HttpResponseMessage();

                //Dictionary<string, string> userInfo = LDAPQuery.GetUserInfo(User.Identity.Name);
                //if (userInfo["LoginUserName"] != null)
                //{
                //var user = _userMasterBL.ValidateUser(userInfo["LoginUserName"]);

                UserMasterViewModel userMasterView = new UserMasterViewModel();
                UserMaster userMasterModel = new UserMaster();

                userMasterModel = _userMasterBL.ValidateUser(loginId, Password);
                if (userMasterModel.Authorized == "UnAuthorized")
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new APIResponse<List<UserMasterViewModel>>(HttpStatusCode.Unauthorized, null, "", "Unauthorized access.", "", ""));
                }
                else if (userMasterModel.Authorized == "Password mismatch")
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new APIResponse<List<UserMasterViewModel>>(HttpStatusCode.Unauthorized, null, "", "Password and User Id mismatch.", "", ""));
                }
                else if (userMasterModel.Authorized == "Email not verified")
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new APIResponse<List<UserMasterViewModel>>(HttpStatusCode.Unauthorized, null, "", "Please verify your email to complete your registration", "", ""));
                }
                else
                {
                    Dictionary<string, string> userInfo = new Dictionary<string, string>();
                    userInfo.Add("LoginUserName", userMasterModel.UserName);
                    userInfo.Add("OrgCode", userMasterModel.OrgCode);
                    userInfo.Add("Id", userMasterModel.Id.ToString());
                    userInfo.Add("RoleID", userMasterModel.RoleID.ToString());
                    userInfo.Add("RoleName", userMasterModel.RoleName);
                    userInfo.Add("EmailID", userMasterModel.EmailID);
                    userInfo.Add("DepartmentID", userMasterModel.DepartmentID.ToString());
                    userInfo.Add("CustomerID", userMasterModel.CustomerID.ToString());
                    userInfo.Add("DepartmentName", userMasterModel.DepartmentName);
                    userInfo.Add("DepartmentCode", userMasterModel.DepartmentCode);
                    userInfo.Add("UserMasterId", userMasterModel.UserMasterId.ToString());
                    userInfo.Add("PlanName", userMasterModel.PlanName.ToString());
                    string subscriptionEndString = userMasterModel.subscription_end;
                    
                    if (!string.IsNullOrWhiteSpace(subscriptionEndString))
                    {
                        DateTime subscriptionEnd;
                        if (DateTime.TryParse(subscriptionEndString, out subscriptionEnd))
                        {
                            DateTime currentDate = DateTime.Now;

                            if (subscriptionEnd < currentDate)
                            {
                                userMasterView.stripe_status = "inactive";
                                SubscriptionDetails subscriptiondetail = new SubscriptionDetails();
                                string url = userMasterModel.planurl + "api/getplan";

                                var client = new HttpClient();
                                var request = new HttpRequestMessage(HttpMethod.Post, url);
                                request.Headers.Add("accept", "*/*");
                                //request.Headers.Add("Content-Type", "application/json");
                                request.Headers.Add("Accept", "application/json");
                                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                                request.Headers.Add("X-Authorization", "B78QZhy7rGG9EJvBQTa9bzVYzg4NT9jRdQ5JMV7rgmiDWCv0iBuaub4SAEXCBbP2");

                                var content = new MultipartFormDataContent();
                                content.Add(new StringContent(userMasterModel.AdminEmail), "email");
                                content.Add(new StringContent("fincast"), "product_slugname");
                                request.Content = content;
                                var res = await client.SendAsync(request);
                                res.EnsureSuccessStatusCode();
                                string jsonResponse = await res.Content.ReadAsStringAsync();
                                subscriptiondetail = JsonConvert.DeserializeObject<SubscriptionDetails>(jsonResponse);
                                if (subscriptiondetail.stripe_status == "active")
                                {
                                    
                                    subscriptionEndString = subscriptiondetail.subscription_end;
                                    if (DateTime.TryParse(subscriptiondetail.subscription_end, out DateTime subscriptionEndDate))
                                    {
                                        if (subscriptionEndDate != subscriptionEnd)
                                        {

                                            int check = _userMasterBL.UpdateLicense(userMasterModel.CustomerID, subscriptionEndDate);
                                            userMasterView.stripe_status = "active";
                                        }
                                    }
                                }
                                else
                                {
                                    userMasterModel.PlanName = "Free Tier";
                                    _userMasterBL.ChangetoFree(userMasterModel.CustomerID);

                                    userMasterView.stripe_status = "active";
                                }
                                                              
                            }
                            else
                            {
                                userMasterView.stripe_status = "active";
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid subscription end date format.");
                        }
                    }
                    else
                    {
                        userMasterView.stripe_status = "inactive";
                        SubscriptionDetails subscriptiondetail = new SubscriptionDetails();
                        string url = userMasterModel.planurl + "api/getplan";

                        var client = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Post, url);
                        request.Headers.Add("accept", "*/*");
                        //request.Headers.Add("Content-Type", "application/json");
                        request.Headers.Add("Accept", "application/json");
                        request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        request.Headers.Add("X-Authorization", "B78QZhy7rGG9EJvBQTa9bzVYzg4NT9jRdQ5JMV7rgmiDWCv0iBuaub4SAEXCBbP2");

                        var content = new MultipartFormDataContent();
                        content.Add(new StringContent(userMasterModel.AdminEmail), "email");
                        content.Add(new StringContent("fincast"), "product_slugname");
                        request.Content = content;
                        var res = await client.SendAsync(request);
                        res.EnsureSuccessStatusCode();
                        string jsonResponse = await res.Content.ReadAsStringAsync();
                        subscriptiondetail = JsonConvert.DeserializeObject<SubscriptionDetails>(jsonResponse);
                        subscriptionEndString = subscriptiondetail.subscription_end;
                        DateTime subscriptionEnd;
                        if (DateTime.TryParse(subscriptionEndString, out subscriptionEnd))
                        {
                            DateTime currentDate = DateTime.Now;

                            if (subscriptionEnd > currentDate) { 
                                int check = _userMasterBL.UpdateLicense(userMasterModel.CustomerID, subscriptionEnd);
                            userMasterView.stripe_status = "active";
                            }
                        }
                    }
                        

                        var token = TokenValidationHandler.GenerateToken(userMasterModel.Id.ToString(), userInfo);
                    
                    Mapper.Map(userMasterModel, userMasterView);
                    userMasterView.Token = token.ToString();
                    userMasterView.OrgName = userMasterModel.OrgName;
                    //userMasterView.stripe_status = "active";
                    return Request.CreateResponse(HttpStatusCode.OK,
                       new APIResponse<UserMasterViewModel>(HttpStatusCode.OK, userMasterView, null, "Login Success", "", ""));
                   
                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserMasterController", "get_Userinfo", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<UserMasterViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }


        }
        [Route("api/user/verify-recaptcha")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyRecaptcha()
        {
            var recaptchaSecretKey = "6LdG8sknAAAAAD4yt1vBpjg5cr7I8yEl2prFfm6z"; // Replace with your actual secret key
            var captchaResponse = await Request.Content.ReadAsStringAsync();
            //RaintelsLogManager.Info("",recaptchaSecretKey, captchaResponse.Substring(0, 10));
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(
    "https://www.google.com/recaptcha/api/siteverify",
    new StringContent($"secret={recaptchaSecretKey}&response={captchaResponse}", Encoding.UTF8, "application/x-www-form-urlencoded")
);

                var responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response and check if the reCAPTCHA verification succeeded
                // Example: If responseBody contains "success": true
                if (responseBody.Contains("\"success\": true"))
                {
                    // reCAPTCHA validation succeeded, proceed with your logic
                    // ...
                    return Ok("reCAPTCHA validation successful.");
                }
                else
                {
                    RaintelsLogManager.Info(responseBody, recaptchaSecretKey, captchaResponse);
                    return BadRequest("reCAPTCHA validation failed.");
                }
            }
        }
        [Route("api/getenvironment")]
        [HttpPost]
        public HttpResponseMessage GetEnvironment()
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                EnvironmentDetails obj = new EnvironmentDetails();
                obj.EnvironmentName = System.Configuration.ConfigurationManager.AppSettings["Environment"].ToString();
                response = Request.CreateResponse(HttpStatusCode.OK, obj);
                return response;
            }
            catch (Exception ex)
            {
                EnvironmentDetails obj = new EnvironmentDetails();
                obj.EnvironmentName = "Not Configured";
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserController", "GetUserInfo()", "");
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            }


        }
        [HttpPost]
        [Route("api/user/getall")]
        public HttpResponseMessage GetAllUserMaster()
        {
            try
            {
                List<UserMaster> userMaster = _userMasterBL.GetAll().ToList();
                List<UserMasterViewModel> userMasterViewModel = new List<UserMasterViewModel>();
                Mapper.Map(userMaster, userMasterViewModel);

                return Request.CreateResponse<APIResponse<List<UserMasterViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<UserMasterViewModel>>(HttpStatusCode.OK, userMasterViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserMasterController", "GetAllUserMaster", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<UserMasterViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }

        [HttpPost]
        [Route("api/user/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<UserMaster> userMaster = _userMasterBL.GetAll().ToList();
                List<UserMasterViewModel> userMasterViewModel = new List<UserMasterViewModel>();
                var entities = userMaster.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, userMasterViewModel);

                return Request.CreateResponse<APIResponse<List<UserMasterViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<UserMasterViewModel>>(HttpStatusCode.OK, userMasterViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserMasterController", "GetAllUserMaster", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<UserMasterViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }

        [HttpPost]
        [Route("api/user/getbyid/{Id}")]
        public HttpResponseMessage GetUserMasterById(int Id)
        {
            try
            {
                UserMaster userMasters = _userMasterBL.GetById(Id);
                UserMasterViewModel userMastersViewModel = new UserMasterViewModel();
                Mapper.Map(userMasters, userMastersViewModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<UserMasterViewModel>(HttpStatusCode.OK, userMastersViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserMasterController", "GetUserMasterById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<UserMasterViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/user/save")]
        public HttpResponseMessage SaveUserMaster(UserMasterViewModel userMasterViewModel)
        {
            try
            {
                UserMaster userMasterDatamodel = new UserMaster();
                //userMasterViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(userMasterViewModel, userMasterDatamodel);
                var userMaster = _userMasterBL.Save(userMasterDatamodel);
                var message = "";
                if ((userMaster != null) && (userMaster.Id > 0))
                {
                    userMasterViewModel.Id = userMaster.Id;
                    if (userMasterViewModel.ActiveStatus == true)
                    {
                        userMasterViewModel.Status = "Active";
                    }
                    else
                    {
                        userMasterViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("UPSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<UserMasterViewModel>(HttpStatusCode.OK, userMasterViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("UPNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("UEAX");//Client Code or Name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));

                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("UEAX");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserMasterController", "SaveUserMaster", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/user/changepassword")]
        public HttpResponseMessage Changepassword(ChangePasswordViewModel passwordViewModel)
        {
            try
            {
                ChangePasswordModel passwordModel = new ChangePasswordModel();
                //userMasterViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(passwordViewModel, passwordModel);
                var resmessage = _userMasterBL.Changepassword(passwordModel);
                var message = "";
                if ((resmessage != null) && (resmessage == "Success"))
                {
                    message = new CommonBL().GetMessage("PCS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<string>(HttpStatusCode.OK, resmessage, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("PCC");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, resmessage, null, message, "", ""));

                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserMasterController", "SaveUserMaster", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/user/forgotpassword")]
        public HttpResponseMessage Forgotpassword(ForgotPasswordViewModel passwordViewModel)
        {
            try
            {
                ForgotPasswordModel passwordModel = new ForgotPasswordModel();
                //userMasterViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(passwordViewModel, passwordModel);
                var resmessage = _userMasterBL.Forgotpassword(passwordModel);
                var message = "";
                if ((resmessage != null) && (resmessage == "Success"))
                {
                    message = new CommonBL().GetMessage("PCS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<string>(HttpStatusCode.OK, resmessage, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("PCC");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, resmessage, null, message, "", ""));

                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserMasterController", "SaveUserMaster", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/user/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteUserMaster(int Id, int DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                //bool deleteSuccess = _userMasterBL.Delete(Id, DeletedBy);
                var check = _userMasterBL.Delete(Id, DeletedBy);
                var message = "";
                if (check == "DS")
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.UserMasterController", "DeleteUserMaster", "Deleted UserMaster Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("DS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage(check);
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserMasterController", "DeleteUserMaster", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                return response;
            }
        }

        [Route("api/user/verifyemail")]
        [HttpPost]
        public HttpResponseMessage Verifyemail([FromBody] UserMaster obj)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                UserMasterViewModel userMasterView = new UserMasterViewModel();
                UserMaster userMasterModel = new UserMaster();

                userMasterModel = _userMasterBL.ValidateEmail(obj.EmailID);
                if (userMasterModel == null || userMasterModel.ActiveStatus == false)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, obj.EmailID);
                }
                else
                {
                    Mapper.Map(userMasterModel, userMasterView);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<UserMasterViewModel>(HttpStatusCode.OK, userMasterView, null, "Please check your email.", "", ""));

                    //userInfo.Add("RoleCode", user.RoleCode);
                    //    userInfo.Add("ActiveStatus", user.ActiveStatus.ToString());
                }

                //} 

                //response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                //return response;
            }
            catch (Exception ex)
            {
                //try
                //{
                //    HttpResponseMessage response = new HttpResponseMessage();
                //    Dictionary<string, string> userInfo = new Dictionary<string, string>();
                //    userInfo.Add("LoginUserName", "admin");
                //    var token = createToken("admin");
                //    userInfo.Add("token", token.ToString());
                //    response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
                //    RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "User Info", "", "Success", "", "", 0, 0, 0, 0, 0, 0, "");
                //    return response;

                //}
                //catch (Exception)
                //{

                //}

                // RaintelsActivityManager.ActivityLog(0, 0, 0, "Login", "User Info", "", "Failed", "", "", 0, 0, 0, 0, 0, 0, "");
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserController", "Verifyemail()", "");
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>());
            }


        }



    }
    public class EnvironmentDetails
    {
        public string EnvironmentName { get; set; }
    }
}
