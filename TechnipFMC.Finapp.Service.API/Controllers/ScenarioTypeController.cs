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
    /// ScenarioType Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ScenarioTypeController : ApiController
    {
        private readonly IScenarioTypeBL _scenarioTypeBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_scenarioTypeBL"></param>
        /// 

        public ScenarioTypeController(IScenarioTypeBL scenarioTypeBL)
        {
            _scenarioTypeBL = scenarioTypeBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/scenariotypes/getall")]
        public HttpResponseMessage GetAllScenarioType()
        {
            try
            {
                List<ScenarioType> scenarioTypes = _scenarioTypeBL.GetAll().ToList();
                List<ScenarioTypeViewModel> scenarioTypeViewModel = new List<ScenarioTypeViewModel>();
                Mapper.Map(scenarioTypes, scenarioTypeViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioTypeViewModel>>(HttpStatusCode.OK, scenarioTypeViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioTypeController", "GetAllScenarioType", "");

                return Request.CreateResponse<APIResponse<List<ScenarioTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenariotypes/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ScenarioType> scenarioTypes = _scenarioTypeBL.GetAll().ToList();
                List<ScenarioTypeViewModel> scenarioTypeViewModel = new List<ScenarioTypeViewModel>();
                var entities = scenarioTypes.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, scenarioTypeViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioTypeViewModel>>(HttpStatusCode.OK, scenarioTypeViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioTypeController", "GetAllScenarioType", "");

                return Request.CreateResponse<APIResponse<List<ScenarioTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenariotypes/getbyid/{Id}")]
        public HttpResponseMessage GetScenarioTypeById(int Id)
        {
            try
            {
                ScenarioType scenarioTypes = _scenarioTypeBL.GetById(Id);
                ScenarioTypeViewModel scenarioTypeViewModel = new ScenarioTypeViewModel();
                Mapper.Map(scenarioTypes, scenarioTypeViewModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ScenarioTypeViewModel>(HttpStatusCode.OK, scenarioTypeViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioTypeController", "GetScenarioTypeById", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioTypeViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                return response;
            }
        }
        [HttpPost]
        [Route("api/scenariotypes/save")]
        public HttpResponseMessage SaveScenarioType(ScenarioTypeViewModel scenarioTypeView)
        {
            try
            {
                ScenarioType scenarioTypedatamodel = new ScenarioType();
                //scenarioTypeView.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(scenarioTypeView, scenarioTypedatamodel);
                var scenarioType = _scenarioTypeBL.Save(scenarioTypedatamodel);

                if (scenarioType != null && scenarioType.ScenarioTypeID > 0)
                {
                    scenarioTypeView.ScenarioTypeID = scenarioType.ScenarioTypeID;
                    if (scenarioTypedatamodel.Active == true)
                    {
                        scenarioTypeView.Status = "Active";
                    }
                    else
                    {
                        scenarioTypeView.Status = "Inactive";
                    }
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioTypeViewModel>(HttpStatusCode.OK, scenarioTypeView,"" , "ScenarioType saved successfully.", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, "ScenarioType could not be saved.", "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, "Code or Name already used.", "", ""));
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, null, null, "ScenarioType cannot be edited as it is already used in Scenario.", "", ""));
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioTypeController", "SaveScenarioType", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));

            }

            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioTypeController", "SaveScenarioType", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));
                return response;
            }
        }

        [HttpPost]
        [Route("api/scenariotypes/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteScenarioTypes(int Id, string DeletedBy)
        {
            //throw new NotImplementedException();
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deletedSuccess = _scenarioTypeBL.Delete(Id, DeletedBy);
                if (deletedSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ScenarioTypesController", "DeleteScenarioTypes", "Deleted ScenarioTypes Id=" + Id + " by " + DeletedBy);

                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, "ScenarioType deleted successfully.", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, "ScenarioType could not be deleted.", "", ""));

                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, null, null, "ScenarioType cannot be deleted as it is used in Scenario.", "", ""));
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioTypeController", "DeleteScenarioTypes", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioTypeController", "DeleteScenarioTypes", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioTypeViewModel>(HttpStatusCode.InternalServerError,null,"Exception occured.", "", "", ""));

            }
        }
    }
}
