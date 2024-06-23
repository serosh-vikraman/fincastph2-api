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
    /// Action Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ActionController : ApiController
    {
        private readonly IActionBL _actionBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="actionBL"></param>
        /// 

        public ActionController(IActionBL actionBL)
        {
            _actionBL = actionBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/actions/getall")]
        //[Authorize]
        public HttpResponseMessage GetAllActions()
        {
            try
            {
                //string token = System.Web.HttpContext.Current.Request.Headers["Authorization"].ToString().re
                //List<ActionEntity> actionEntitys = new List<ActionEntity>();
                List <ActionEntity> actionEntitys = _actionBL.GetAll().ToList();
                List<ActionViewModel> actionEntitysModel = new List<ActionViewModel>();
                //Mapper.Map(actionEntitys, actionEntitysModel);

                return Request.CreateResponse<APIResponse<List<ActionViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ActionViewModel>>(HttpStatusCode.OK, actionEntitysModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ActionController", "GetAllActions", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ActionViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }

        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/actions/getall/search")]
        // [Authorize]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ActionEntity> actionEntitys = _actionBL.GetAll().ToList();
                List<ActionViewModel> actionEntitysModel = new List<ActionViewModel>();
                var entities = actionEntitys.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, actionEntitysModel);

                return Request.CreateResponse<APIResponse<List<ActionViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ActionViewModel>>(HttpStatusCode.OK, actionEntitysModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ActionController", "GetAllActions", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ActionViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }

        [HttpPost]
        [Route("api/actions/getbyid/{Id}")]
        public HttpResponseMessage GetActionById(int Id)
        {
            try
            {
                ActionEntity actionEntity = _actionBL.GetById(Id);
                ActionViewModel actionEntityModel = new ActionViewModel();
                Mapper.Map(actionEntity, actionEntityModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ActionViewModel>(HttpStatusCode.OK, actionEntityModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ActionController", "GetActionById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ActionViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/actions/save")]
        public HttpResponseMessage SaveAction(ActionViewModel actionViewModel)
        {
            try
            {
                ActionEntity actionEntity = new ActionEntity();
                //actionViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(actionViewModel, actionEntity);
                var actionDataModel = _actionBL.Save(actionEntity);


                if (actionDataModel != null && actionDataModel.ActionID > 0)
                {
                    actionViewModel.ActionID = actionDataModel.ActionID;
                    if (actionEntity.Active == true)
                    {
                        actionViewModel.Status = "Active";
                    }
                    else
                    {
                        actionViewModel.Status = "Inactive";
                    }
                    Mapper.Map(actionDataModel, actionViewModel);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ActionViewModel>(HttpStatusCode.OK, actionViewModel, null, "Action saved succesfully.", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, "Action could not be saved.", "", ""));

                }

            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, "Code or Name already used.", "", ""));
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, " ", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ActionController", "SaveAction", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/actions/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteAction(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deletedSuccess = _actionBL.Delete(Id, DeletedBy);
                if (deletedSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ActionController", "DeleteAction", "Deleted Action Id=" + Id + " by " + DeletedBy);

                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, "Action deleted succesfully.", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, "Action could not be deleted.", "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", "", "Action cannot be deleted as it is used in RolePermissions.", "", ""));
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ActionController", "DeleteAction", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }
    }
}
