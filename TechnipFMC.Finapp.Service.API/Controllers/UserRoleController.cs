using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Formatting;
using TechnipFMC.Common;
using TechnipFMC.Finapp.Business;
using TechnipFMC.Finapp.Models;
using TechnipFMC.Finapp.Service.API.ViewModel;
using System.Data.SqlClient;


namespace TechnipFMC.Finapp.Service.API.Controllers
{
    /// <summary>
    /// UserRole Controller
    /// </summary>

    //[Authorize] //remove comment
    public class UserRoleController : ApiController
    {
        private readonly IUserRoleBL _userRoleBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRoleBL"></param>
        /// 

        public UserRoleController(IUserRoleBL userRoleBL)
        {
            _userRoleBL = userRoleBL;

        }
        /// <summary>
        /// Get All UserRole 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/userroles/getall")]
        public HttpResponseMessage GetAllUserRole()
        {
            try
            {
                List<UserRole> userRoles = _userRoleBL.GetAll().ToList();
                List<UserRoleViewModel> userRolesModel = new List<UserRoleViewModel>();
                Mapper.Map(userRoles, userRolesModel);

                return Request.CreateResponse<APIResponse<List<UserRoleViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<UserRoleViewModel>>(HttpStatusCode.OK, userRolesModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "GetAllUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<UserRoleViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                
            }
        }
        [HttpPost]
        [Route("api/userroles/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<UserRole> userRoles = _userRoleBL.GetAll().ToList();
                List<UserRoleViewModel> userRolesModel = new List<UserRoleViewModel>();
                var entities = userRoles.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, userRolesModel);

                return Request.CreateResponse<APIResponse<List<UserRoleViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<UserRoleViewModel>>(HttpStatusCode.OK, userRolesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "GetAllUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<UserRoleViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/userroles/getbyid/{Id}")]
        public HttpResponseMessage GetUserRoleById(int Id)
        {
            try
            {
                UserRole userRoles = _userRoleBL.GetById(Id);
                UserRoleViewModel userRolesModel = new UserRoleViewModel();
                Mapper.Map(userRoles, userRolesModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<UserRoleViewModel>(HttpStatusCode.OK, userRolesModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "GetUserRoleById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<UserRoleViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                
            }
        }
        [HttpPost]
        [Route("api/userroles/save")]
        public HttpResponseMessage SaveUserRole(UserRoleViewModel userRoleViewModel)
        {
            try
            {
                UserRole userRoleDatamodel = new UserRole();
                //userRoleViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(userRoleViewModel, userRoleDatamodel); 
                var userRole = _userRoleBL.Save(userRoleDatamodel);
                var message = "";
                if (userRole != null && userRole.RoleID > 0)
                {
                    userRoleViewModel.RoleID = userRole.RoleID;
                    if (userRoleDatamodel.Active == true)
                    {
                        userRoleViewModel.Status = "Active";
                    }
                    else
                    {
                        userRoleViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("URSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<UserRoleViewModel>(HttpStatusCode.OK, userRoleViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("URNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", "", message, "", ""));

                }

                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("URCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<UserRoleViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "SaveUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "SaveUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                
            }
        }

        [HttpPost]
        [Route("api/userroles/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteUserRole(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _userRoleBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.UserRoleController", "DeleteUserRole", "Deleted UserRole Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("URDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("URDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, message, "", "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("URDNURP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "DeleteUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null,"Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "DeleteUserRole", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                return response;
            }
        }

        [HttpPost]
        [Route("api/userroles/mapuserproject")]
        public HttpResponseMessage MapUserProject(UserProjectsViewModel userProjectsViewModel)
        {
            try
            {
                UserProjects userProjectDatamodel = new UserProjects();
                //userProjectViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(userProjectsViewModel, userProjectDatamodel);
                var IsSuccess = _userRoleBL.MapUserProject(userProjectDatamodel);
                var message = "";
                if (IsSuccess)
                {                  
                    message = new CommonBL().GetMessage("UPMS");//Project mapped to user Succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<UserProjectViewModel>(HttpStatusCode.OK, userProjectsViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("UPNM");//Project could not be mapped
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", "", message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("URCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<UserProjectViewModel>(HttpStatusCode.BadRequest, null, null, "Unique key error", "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "SaveUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "SaveUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }

        [HttpPost]
        [Route("api/userroles/mapuserdepartment")]
        public HttpResponseMessage MapUserDepartment(UserDepartmentViewModel userDepartmentViewModel)
        {
            try
            {
                UserDepartment userDepartmentDatamodel = new UserDepartment();
                //userProjectViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(userDepartmentViewModel, userDepartmentDatamodel);
                var IsSuccess = _userRoleBL.MapUserDepartment(userDepartmentDatamodel);
                var message = "";
                if (IsSuccess)
                {
                    message = new CommonBL().GetMessage("UPMS");//Project mapped to user Succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<UserProjectViewModel>(HttpStatusCode.OK, userDepartmentViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("UPNM");//Project could not be mapped
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", "", message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("URCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<UserProjectViewModel>(HttpStatusCode.BadRequest, null, null, "Unique key error", "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "SaveUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserRoleController", "SaveUserRole", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }



    }
}
