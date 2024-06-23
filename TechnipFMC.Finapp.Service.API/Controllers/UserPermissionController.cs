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
    /// UserPermission Controller
    /// </summary>
    //[Authorize] //remove comment
    public class UserPermissionController : ApiController
    {
        private readonly IUserPermissionBL _userPermissionBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userPermissionBL"></param>
        /// 

        public UserPermissionController(IUserPermissionBL userPermissionBL)
        {
            _userPermissionBL = userPermissionBL;

        }

        [HttpPost]
        [Route("api/userpermissions/getall")]
        public HttpResponseMessage GetAllUserPermission()
        {
            try
            {
                List<UserPermission> userPermission = _userPermissionBL.GetAll().ToList();
                List<UserPermissionViewModel> userPermissionViewModel = new List<UserPermissionViewModel>();
                Mapper.Map(userPermission, userPermissionViewModel);

                return Request.CreateResponse<APIResponse<List<UserPermissionViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<UserPermissionViewModel>>(HttpStatusCode.OK, userPermissionViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserPermissionController", "GetAllUserPermission", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<UserPermissionViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/userpermissions/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<UserPermission> userPermission = _userPermissionBL.GetAll().ToList();
                List<UserPermissionViewModel> userPermissionViewModel = new List<UserPermissionViewModel>();
                var entities = userPermission.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, userPermissionViewModel);

                return Request.CreateResponse<APIResponse<List<UserPermissionViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<UserPermissionViewModel>>(HttpStatusCode.OK, userPermissionViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserPermissionController", "GetAllUserPermission", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<UserPermissionViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/userpermissions/getbyid/{Id}")]
        public HttpResponseMessage GetUserPermissionById(int Id)
        {
            try
            {
                UserPermission userPermissions = _userPermissionBL.GetById(Id);
                UserPermissionViewModel userPermissionsViewModel = new UserPermissionViewModel();
                Mapper.Map(userPermissions, userPermissionsViewModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<UserPermissionViewModel>(HttpStatusCode.OK, userPermissionsViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserPermissionController", "GetUserPermissionById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<UserPermissionViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/userpermissions/save")]
        public HttpResponseMessage SaveUserPermission(UserPermissionViewModel userPermissionViewModel)
        {
            try
            {
                UserPermission userPermissionDatamodel = new UserPermission();
                //userPermissionViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(userPermissionViewModel, userPermissionDatamodel);
                var userPermission = _userPermissionBL.Save(userPermissionDatamodel);
                var message = "";
                if ((userPermission != null) && (userPermission.Id > 0))
                {
                    userPermissionViewModel.Id = userPermission.Id;
                    if (userPermissionViewModel.ActiveStatus == true)
                    {
                        userPermissionViewModel.Status = "Active";
                    }
                    else
                    {
                        userPermissionViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("UPSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<UserPermissionViewModel>(HttpStatusCode.OK, userPermissionViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("UPNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }

            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserPermissionController", "SaveUserPermission", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }

        [HttpPost]
        [Route("api/userpermissions/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteUserPermission(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _userPermissionBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.UserPermissionController", "DeleteUserPermission", "Deleted UserPermission Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("UPDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("UPDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserPermissionController", "DeleteUserPermission", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));
                return response;
            }
        }

        [Route("api/validateuser/{loginId}/{password}")]
        [HttpPost]
        public HttpResponseMessage ValidateUser(string loginId, string password)
        {
            try
            {
                var user = _userPermissionBL.ValidateUser(loginId, password);

                if (user.ActiveStatus)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<string>(HttpStatusCode.OK, user.RoleCode, null, "", "", ""));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, user.ActiveStatus, null, "", "", ""));
                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.UserController", "ValidateUser()", "");
                throw;
            }
        }

    }
}
