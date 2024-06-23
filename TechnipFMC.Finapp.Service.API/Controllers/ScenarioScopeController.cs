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
    /// ScenarioScope Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ScenarioScopeController : ApiController
    {
        private readonly IScenarioScopeBL _scenarioScopeBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_scenarioScopeBL"></param>
        /// 

        public ScenarioScopeController(IScenarioScopeBL scenarioScopeBL)
        {
            _scenarioScopeBL = scenarioScopeBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/scenarioscopes/getall")]
        //[Authorize]
        public HttpResponseMessage GetAllScenarioScope()
        {
            try
            {
                List<ScenarioScope> scenarioScopes = _scenarioScopeBL.GetAll().ToList();
                List<ScenarioScopeViewModel> scenarioScopesViewModel = new List<ScenarioScopeViewModel>();
                Mapper.Map(scenarioScopes, scenarioScopesViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioScopeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioScopeViewModel>>(HttpStatusCode.OK, scenarioScopesViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioScopeController", "GetAllScenarioScope", "");

                return Request.CreateResponse<APIResponse<List<ScenarioScopeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioScopeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarioscopes/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ScenarioScope> scenarioScopes = _scenarioScopeBL.GetAll().ToList();
                List<ScenarioScopeViewModel> scenarioScopesViewModel = new List<ScenarioScopeViewModel>();
                var entities = scenarioScopes.Where(a => a.Active == true).ToList();
                Mapper.Map(scenarioScopes, scenarioScopesViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioScopeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioScopeViewModel>>(HttpStatusCode.OK, scenarioScopesViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioScopeController", "GetAllScenarioScope", "");

                return Request.CreateResponse<APIResponse<List<ScenarioScopeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioScopeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarioscopes/getbyid/{Id}")]
        public HttpResponseMessage GetScenarioScopeById(int Id)
        {
            try
            {
                ScenarioScope scenarioScopes = _scenarioScopeBL.GetById(Id);
                ScenarioScopeViewModel scenarioScopeViewModel = new ScenarioScopeViewModel();
                Mapper.Map(scenarioScopes, scenarioScopeViewModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ScenarioScopeViewModel>(HttpStatusCode.OK, scenarioScopeViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioScopeController", "GetScenarioScopeById", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioScopeViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                return response;
            }
        }
        [HttpPost]
        [Route("api/scenarioscopes/save")]
        public HttpResponseMessage SaveScenarioScope(ScenarioScopeViewModel scenarioScopeView)
        {
            try
            {
                ScenarioScope scenarioScopedatamodel = new ScenarioScope();
                //scenarioScopeView.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(scenarioScopeView, scenarioScopedatamodel);
                var scenarioScope = _scenarioScopeBL.Save(scenarioScopedatamodel);
                var message = "";
                if (scenarioScope != null && scenarioScope.ScenarioScopeID > 0)
                {
                    scenarioScopeView.ScenarioScopeID = scenarioScope.ScenarioScopeID;
                    if (scenarioScopedatamodel.Active == true)
                    {
                        scenarioScopeView.Status = "Active";
                    }
                    else
                    {
                        scenarioScopeView.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("SSSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioScopeViewModel>(HttpStatusCode.OK, scenarioScopeView, "", message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("SSNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("SSCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, null, null, "ScenarioScope cannot be edited as it is already used in Scenario.", "", ""));
                else

                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioScopeController", "SaveScenarioScope", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioScopeController", "SaveScenarioScope", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                return response;
            }
        }

        [HttpPost]
        [Route("api/scenarioscopes/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteScenarioScopes(int Id, string DeletedBy)
        {
            //throw new NotImplementedException();
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deletedSuccess = _scenarioScopeBL.Delete(Id, DeletedBy);
                if (deletedSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ScenarioScopeController", "DeleteScenarioScopes", "Deleted ScenarioScope Id=" + Id + " by " + DeletedBy);

                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, "ScenarioScope deleted successfully.", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, "ScenarioScope could not be deleted.", "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, "ScenarioScope cannot be deleted as it is used in Scenario.", "", ""));
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioScopeController", "DeleteScenarioScopes", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioScopeController", "DeleteScenarioScopes", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioTypeViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
    }
}
