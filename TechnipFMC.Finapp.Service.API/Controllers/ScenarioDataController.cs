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
    /// ScenarioData Controller
    /// </summary>
    /// 
    //[Authorize] //remove comment
    public class ScenarioDataController : ApiController
    {
        private readonly IScenarioDataBL _scenarioDataBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ScenarioDataBL"></param>
        /// 

        public ScenarioDataController(IScenarioDataBL scenarioDataBL)
        {
            _scenarioDataBL = scenarioDataBL;

        }


        [HttpPost]
        [Route("api/getallscenariodata/{projectId}/{scenarioId}")]
        public HttpResponseMessage GetAllScenarioData(int projectId, int scenarioId)
        {
            try
            {
                var scenarioDatas = _scenarioDataBL.GetAll(projectId, scenarioId).ToList();

                return Request.CreateResponse<APIResponse<List<YearlyScenarioData>>>(HttpStatusCode.OK,
                    new APIResponse<List<YearlyScenarioData>>(HttpStatusCode.OK, scenarioDatas, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioDataController", "GetAllScenarioData", "");

                return Request.CreateResponse<APIResponse<List<ScenarioDataViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioDataViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/getallquarterlydataofscenario/{scenarioId}")]
        public HttpResponseMessage GetAllQuarterlyDataOfScenario(int scenarioId)
        {
            try
            {
                ScenarioDetails scenarioDatas = _scenarioDataBL.GetAllQuarterlyDataOfScenario(scenarioId);//.ToList();
                ScenarioDetailsViewModel scenarioDatasViewModel = new ScenarioDetailsViewModel();
                Mapper.Map(scenarioDatas, scenarioDatasViewModel);
                //var scenarioDatas = _scenarioDataBL.GetAllQuarterlyDataOfScenario(scenarioId);//.ToList();

                return Request.CreateResponse<APIResponse<ScenarioDetailsViewModel>>(HttpStatusCode.OK,
                    new APIResponse<ScenarioDetailsViewModel>(HttpStatusCode.OK, scenarioDatasViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioDataController", "GetAllQuarterlyDataOfScenario", "");

                return Request.CreateResponse<APIResponse<List<ScenarioDataViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioDataViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenariodata/getbyid/{Id}")]
        public HttpResponseMessage GetScenarioDataById(int Id)
        {
            try
            {
                ScenarioData source = _scenarioDataBL.GetById(Id);
                ScenarioDataViewModel destination = new ScenarioDataViewModel();
                Mapper.Map(source, destination);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ScenarioDataViewModel>(HttpStatusCode.OK, destination, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioDataController", "GetScenarioDataById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioDataViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/scenariodata/save")]
        public HttpResponseMessage SaveScenarioData(ScenarioDataMasterViewModel scenarioData)
        {
            try
            {
                ScenarioDataMaster scenarioDatamodel = new ScenarioDataMaster();
                //var userName = User.Identity.Name.GetUserName();
                //scenarioData.ScenarioData.ForEach(i => i.CreatedBy = userName);
                //scenarioData.CreatedBy = userName;
                Mapper.Map(scenarioData, scenarioDatamodel);
                var message = "";
                var IsSuccess = _scenarioDataBL.Save(scenarioDatamodel);

                if (IsSuccess)
                {
                    message = new CommonBL().GetMessage("SDSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));
                }
                else
                {
                    message = new CommonBL().GetMessage("SDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<bool>(HttpStatusCode.BadRequest, false, null, message, "", ""));
                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioDataController", "SaveScenarioData", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<bool>(HttpStatusCode.InternalServerError, false, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenariodata/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteScenarioData(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _scenarioDataBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ScenarioDataController", "DeleteScenarioData", "Deleted ScenarioData Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("SDDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("SDDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioDataController", "DeleteScenarioData", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/getscenariolayout/{scenarioId}")]
        public HttpResponseMessage GetScenarioLayOut(int scenarioId)
        {
            try
            {
                var scenarioLayout = _scenarioDataBL.GetScenarioLayout(scenarioId).ToList();

                return Request.CreateResponse<APIResponse<List<ScenarioLayout>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioLayout>>(HttpStatusCode.OK, scenarioLayout, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioDataController", "GetScenarioLayOut", "");

                return Request.CreateResponse<APIResponse<List<ScenarioDataViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioDataViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenariodata/clear/{ScenarioId}/{DeletedBy}")]
        public HttpResponseMessage ClearScenarioData(int ScenarioId, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                int deleteSuccess = _scenarioDataBL.ClearScenarioData(ScenarioId, DeletedBy);
                var message = "";
                if (deleteSuccess > 0)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ScenarioDataController", "ClearScenarioData", "Cleared ScenarioData from SCenario =" + ScenarioId + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("SDCS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("SDCNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioDataController", "ClearScenarioData", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenariodata/clearprojectdata")]
        public HttpResponseMessage ClearProjectScenarioData(ScenarioProjectMapper scenarioProject)
        {
            try
            {
                //scenarioProject.UpdatedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _scenarioDataBL.ClearProjectScenarioData(scenarioProject);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ScenarioDataController", "ClearProjectScenarioData", "Cleared ScenarioData ScenarioId=" + scenarioProject.ScenarioID +" ProjectId="+ scenarioProject.ProjectID + " by " + scenarioProject.UpdatedBy);
                    message = new CommonBL().GetMessage("PDCS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("PDCNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioDataController", "ClearProjectScenarioData", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

    }
}
