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
    /// SubAction Controller
    /// </summary>

    //[Authorize] //remove comment
    public class SubActionController : ApiController
    {
        private readonly ISubActionBL _subActionBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SubActionBL"></param>
        /// 

        public SubActionController(ISubActionBL subActionBL)
        {
            _subActionBL = subActionBL;

        }
        /// <summary>
        /// Get All SubAction 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/subactions/getall")]
        public HttpResponseMessage GetAllSubActions()
        {
            try
            {
                List<SubAction> subActions = _subActionBL.GetAll().ToList();
                List<SubActionViewModel> subActionsModel = new List<SubActionViewModel>();
                Mapper.Map(subActions, subActionsModel);

                return Request.CreateResponse<APIResponse<List<SubActionViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<SubActionViewModel>>(HttpStatusCode.OK, subActionsModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SubActionController", "GetAllSubAction", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<SubActionViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
               
            }
        }
        [HttpPost]
        [Route("api/subactions/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<SubAction> subActions = _subActionBL.GetAll().ToList();
                List<SubActionViewModel> subActionsModel = new List<SubActionViewModel>();
                var entities = subActions.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, subActionsModel);

                return Request.CreateResponse<APIResponse<List<SubActionViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<SubActionViewModel>>(HttpStatusCode.OK, subActionsModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SubActionController", "GetAllSubAction", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<SubActionViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/subactions/getbyid/{Id}")]
        public HttpResponseMessage GetSubActionById(int Id)
        {
            try
            {
                SubAction subActions = _subActionBL.GetById(Id);
                SubActionViewModel subActionsModel = new SubActionViewModel();
                Mapper.Map(subActions, subActionsModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<SubActionViewModel>(HttpStatusCode.OK, subActionsModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SubActionController", "GetSubActionById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<SubActionViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                
            }
        }
        [HttpPost]
        [Route("api/subactions/save")]
        public HttpResponseMessage SaveSubAction(SubActionViewModel subActionViewModel)
        {
            try
            {
                SubAction subActionDatamodel = new SubAction();
                //subActionViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(subActionViewModel, subActionDatamodel);
                var subAction = _subActionBL.Save(subActionDatamodel);
                
                if (subAction != null && subAction.SubActionID > 0)
                {
                    subActionViewModel.SubActionID = subAction.SubActionID;
                    if (subActionDatamodel.Active == true)
                    {
                        subActionViewModel.Status = "Active";
                    }
                    else
                    {
                        subActionViewModel.Status = "Inactive";
                    }
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<SubActionViewModel>(HttpStatusCode.OK, subActionViewModel, null , "SubAction saved successfully.", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, "SubAction could not be saved.", "", ""));

                }               
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<SubActionViewModel>(HttpStatusCode.BadRequest, null, "", "Code or Name already used.", "", ""));
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SubActionController", "SaveSubActions", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SubActionController", "SaveSubActions", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<SubActionViewModel>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));
                return response;
            }
        }

        [HttpPost]
        [Route("api/subactions/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteSubAction(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _subActionBL.Delete(Id, DeletedBy);
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.SubActionController", "DeleteSubAction", "Deleted SubAction Id=" + Id + " by " + DeletedBy);

                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, "SubAction deleted Successfully.", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, "", "SubAction could not be deleted.", "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, "Sub Action cannot be deleted as it is used in RolePermissions.", "", ""));
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SubActionController", "DeleteSubAction", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SubActionController", "DeleteSubAction", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                
            }
        }

    }
}
