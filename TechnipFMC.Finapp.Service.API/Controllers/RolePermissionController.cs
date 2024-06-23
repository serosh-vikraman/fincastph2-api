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
    /// RolePermission Controller
    /// </summary>
    //[Authorize] //remove comment
    public class RolePermissionController : ApiController
    {
        private readonly IRolePermissionBL _rolePermissionBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RolePermissionBL"></param>
        /// 

        public RolePermissionController(IRolePermissionBL rolePermissionBL)
        {
            _rolePermissionBL = rolePermissionBL;

        }
        
        [HttpPost]
        [Route("api/rolepermissions/getall")]
        public HttpResponseMessage GetAllRolePermission()
        {
            try
            {
                List<RolePermission> rolePermission = _rolePermissionBL.GetAll().ToList();
                List<RolePermissionViewModel> rolePermissionViewModel = new List<RolePermissionViewModel>();
                Mapper.Map(rolePermission, rolePermissionViewModel);

                return Request.CreateResponse<APIResponse<List<RolePermissionViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<RolePermissionViewModel>>(HttpStatusCode.OK, rolePermissionViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.RolePermissionController", "GetAllRolePermission", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<RolePermissionViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/rolepermissions/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<RolePermission> rolePermission = _rolePermissionBL.GetAll().ToList();
                List<RolePermissionViewModel> rolePermissionViewModel = new List<RolePermissionViewModel>();
                var entities = rolePermission.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, rolePermissionViewModel);

                return Request.CreateResponse<APIResponse<List<RolePermissionViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<RolePermissionViewModel>>(HttpStatusCode.OK, rolePermissionViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.RolePermissionController", "GetAllRolePermission", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<RolePermissionViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/rolepermissions/getbyid/{Id}")]
        public HttpResponseMessage GetRolePermissionById(int Id)
        {
            try
            {
                RolePermission rolePermissions = _rolePermissionBL.GetById(Id);
                RolePermissionViewModel rolePermissionsViewModel = new RolePermissionViewModel();
                Mapper.Map(rolePermissions, rolePermissionsViewModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<RolePermissionViewModel>(HttpStatusCode.OK, rolePermissionsViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.RolePermissionController", "GetRolePermissionById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<RolePermissionViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/rolepermissions/save")]
        public HttpResponseMessage SaveRolePermission(RolePermissionViewModel rolePermissionViewModel)
        {
            try
            {
                RolePermission rolePermissionDatamodel = new RolePermission();
                //rolePermissionViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(rolePermissionViewModel, rolePermissionDatamodel);
                var rolePermission = _rolePermissionBL.Save(rolePermissionDatamodel);
                var message = "";
                if (rolePermission != null)// && rolePermission.RoleID > 0)
                {
                    rolePermissionViewModel.Id = rolePermission.Id;
                    if (rolePermissionDatamodel.Active == true)
                    {
                        rolePermissionViewModel.Status = "Active";
                    }
                    else
                    {
                        rolePermissionViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("RPSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<RolePermissionViewModel>(HttpStatusCode.OK, rolePermissionViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("RPNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", "", message, "", ""));
                }
            }
            
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.RolePermissionController", "SaveRolePermission", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  "", "Exception occured.", "", " ", " "));
            }
        }

        [HttpPost]
        [Route("api/rolepermissions/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteRolePermission(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _rolePermissionBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    message = new CommonBL().GetMessage("RPDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("RPDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.RolePermissionController", "DeleteRolePermission", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,"", "Exception occured.", "", " ", " "));
                return response;
            }
        }

    }
}
