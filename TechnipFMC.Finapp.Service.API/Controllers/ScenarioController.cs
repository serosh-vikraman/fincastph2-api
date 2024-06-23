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
    /// Scenario Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ScenarioController : ApiController
    {
        private readonly IScenarioBL _scenarioBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_scenarioBL"></param>
        /// 

        public ScenarioController(IScenarioBL scenarioBL)
        {
            _scenarioBL = scenarioBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/scenarios/getlegacyinsertionstatus")]
        public HttpResponseMessage GetLegacyInsertionStatus()
        {
            try
            {

                bool status = _scenarioBL.GetLegacyInsertionStatus();

                return Request.CreateResponse<APIResponse<bool>>(HttpStatusCode.OK,
                    new APIResponse<bool>(HttpStatusCode.OK, status, null, "", "", ""));
            }
            catch (Exception ex)
            {
                // RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<bool>>(HttpStatusCode.InternalServerError,
                    new APIResponse<bool>(HttpStatusCode.InternalServerError, null, "Exception Occured:", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarios/getlegacyyears/1")]
        public HttpResponseMessage GetLegacyYears()
        {
            try
            {

                bool status = _scenarioBL.GetLegacyInsertionStatus();
                List<LeagacyYears> legacyYears = new List<LeagacyYears>();
                if (status)
                {
                    legacyYears.Add(new LeagacyYears() { KeyData = 2011, ValueData = "2011", Active=true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2012, ValueData = "2012", Active = true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2013, ValueData = "2013", Active = true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2014, ValueData = "2014", Active = true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2015, ValueData = "2015", Active = true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2016, ValueData = "2016", Active = true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2017, ValueData = "2017", Active = true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2018, ValueData = "2018", Active = true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2019, ValueData = "2019", Active = true });

                }
                else
                {
                    legacyYears.Add(new LeagacyYears() { KeyData = 2020, ValueData = "2020", Active = true });
                    legacyYears.Add(new LeagacyYears() { KeyData = 2021, ValueData = "2021", Active = true });
                }

                return Request.CreateResponse<APIResponse<List<LeagacyYears>>>(HttpStatusCode.OK,
                    new APIResponse<List<LeagacyYears>>(HttpStatusCode.OK, legacyYears, null, "", "", ""));
            }
            catch (Exception ex)
            {
                // RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<List<LeagacyYears>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<LeagacyYears>>(HttpStatusCode.InternalServerError, null, "Exception Occured:", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarios/getall/{DepartmentId}/{clientId}/{spec}")]
        public HttpResponseMessage GetAllScenario(int departmentId,int clientId, string spec)
        {
            try
            {

                List<Scenario> scenarios = _scenarioBL.GetAll(departmentId, clientId, spec).ToList();
                List<ScenarioViewModel> scenarioViewModel = new List<ScenarioViewModel>();
                Mapper.Map(scenarios, scenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.OK, scenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                // RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured:" + ex.ToString(), "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarios/getall/search/{DepartmentId}/{clientId}/{spec}")]
        public HttpResponseMessage Search(int departmentId, int clientId, string spec)
        {
            try
            {

                List<Scenario> scenarios = _scenarioBL.GetAll(departmentId, clientId, spec).ToList();
                List<ScenarioViewModel> scenarioViewModel = new List<ScenarioViewModel>();
                var entities = scenarios.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, scenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.OK, scenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                // RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured:" + ex.ToString(), "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarios/getallunlockedScenario/{year}/{DepartmentId}/{clientId}/{spec}")]
        public HttpResponseMessage GetUnlockedScenario(int year, int departmentId,int clientId,string spec)
        {
            try
            {

                List<Scenario> scenarios = _scenarioBL.GetAll(departmentId, clientId,spec).ToList();
                List<ScenarioViewModel> scenarioViewModel = new List<ScenarioViewModel>();
                var entities = scenarios.Where(a => a.ScenarioLock == false && a.FinancialYear == year).ToList();
                Mapper.Map(entities, scenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.OK, scenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured:" + ex.ToString(), "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarios/getbyid/{Id}")]
        public HttpResponseMessage GetScenarioById(int Id)
        {
            try
            {
                Scenario scenarios = _scenarioBL.GetById(Id);
                ScenarioViewModel scenarioViewModel = new ScenarioViewModel();
                Mapper.Map(scenarios, scenarioViewModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ScenarioViewModel>(HttpStatusCode.OK, scenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetScenarioById", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));
                return response;
            }
        }
        [HttpPost]
        [Route("api/scenarios/save")]
        public HttpResponseMessage SaveScenario(ScenarioViewModel scenarioView)
        {
            try
            {
                Scenario scenariodatamodel = new Scenario();
                //scenarioView.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(scenarioView, scenariodatamodel);
                var scenario = _scenarioBL.Save(scenariodatamodel);
                var message = "";
                if (scenario != null && scenario.message == "Success")
                {
                    scenarioView.ScenarioID = scenario.ScenarioID;
                    scenarioView.ScenarioName = scenario.ScenarioName;
                    if (scenarioView.ScenarioScopeCode == "OI")
                    {
                        scenarioView.ScenarioScopeName = "ORDERINTAKE";
                    }
                    else if (scenarioView.ScenarioScopeCode == "PL")
                    {
                        scenarioView.ScenarioScopeName = "PROFIT AND LOSS";
                    }
                    else if (scenarioView.ScenarioScopeCode == "BL")
                    {
                        scenarioView.ScenarioScopeName = "BACKLOG";
                    }
                    if (scenarioView.ScenarioTypeCode == "BD")
                    {
                        scenarioView.ScenarioTypeName = "BUDGET";
                    }
                    else if (scenarioView.ScenarioTypeCode == "FC")
                    {
                        scenarioView.ScenarioTypeName = "FORECAST";
                    }
                    else if (scenarioView.ScenarioTypeCode == "AC")
                    {
                        scenarioView.ScenarioTypeName = "ACTUALS";
                    }
                    message = new CommonBL().GetMessage("SCSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioViewModel>(HttpStatusCode.OK, scenarioView, "", message, "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, scenario.message, "", ""));
                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "SaveScenario", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/scenarios/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteScenarios(int Id, string DeletedBy)
        {
            //throw new NotImplementedException();
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                var check = _scenarioBL.Delete(Id, DeletedBy);
                var message = "";
                if (check == "IsSuccess")
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ScenarioController", "DeleteScenarios", "Deleted Scenario Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("SCDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, "Scenario deleted successfully.", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, check, "", ""));

                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("PASC");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "DeleteScenarios", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenarios/delete")]
        public HttpResponseMessage DeleteMultipleScenario(ScenarioIDViewModel scenarioIdsView)
        {
            try
            {
                //string DeletedBy = User.Identity.Name.GetUserName();
                ScenarioID scenarioIds = new ScenarioID();
                Mapper.Map(scenarioIdsView, scenarioIds);
                var check = _scenarioBL.DeleteMultipleScenario(scenarioIds);
                if (check == "IsSuccess")
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ScenarioController", "DeleteScenarios", "Deleted Scenario by " + scenarioIds.CreatedBy);
                    var message = new CommonBL().GetMessage("MSDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, check, "", ""));

                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("PASC");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "DeleteScenarios", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ScenarioViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }


        [HttpPost]
        [Route("api/projectsforscenario/{ScenarioID}")]
        public HttpResponseMessage GetAllProjectsForScenario(int ScenarioID)
        {
            try
            {
                List<ProjectForScenario> Scenarios = _scenarioBL.GetAllProjects(ScenarioID).ToList();
                List<ProjectForScenarioViewModel> ScenarioViewModel = new List<ProjectForScenarioViewModel>();
                Mapper.Map(Scenarios, ScenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ProjectForScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectForScenarioViewModel>>(HttpStatusCode.OK, ScenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllProjectsForScenario", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenarioprojectmapper")]
        public HttpResponseMessage MapScenarioProject(ProjectScenarioViewModel scenarioProjectViewModel)
        {
            try
            {
                //scenarioProjectViewModel.CreatedBy = User.Identity.Name.GetUserName();
                ProjectScenarioModel scenarioProjectdatamodel = new ProjectScenarioModel();
                Mapper.Map(scenarioProjectViewModel, scenarioProjectdatamodel);
                var IsSuccess = _scenarioBL.MapScenarioProjects(scenarioProjectdatamodel);
                var message = "";
                if (IsSuccess)
                {
                    message = new CommonBL().GetMessage("SMPS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("SMPNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<bool>(HttpStatusCode.BadRequest, false, null, message, "", ""));

                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "MapScenarioProject", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/scenarioapplicableyears/{ScenarioId}")]
        public HttpResponseMessage GetApplicableYearsForScenario(int scenarioId)
        {
            try
            {
                ScenarioApplicableYears scenarioApplicableYears = _scenarioBL.GetApplicableYears(scenarioId);
                ScenarioApplicableYearsViewModel scenarioApplicableYearsViewModel = new ScenarioApplicableYearsViewModel();
                Mapper.Map(scenarioApplicableYears, scenarioApplicableYearsViewModel);

                return Request.CreateResponse<APIResponse<ScenarioApplicableYearsViewModel>>(HttpStatusCode.OK,
                    new APIResponse<ScenarioApplicableYearsViewModel>(HttpStatusCode.OK, scenarioApplicableYearsViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetApplicableYearsForScenario", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenariossequence/{scenarioScopeCode}/{scenarioTypeCode}/{financialYear}")]
        public HttpResponseMessage GetScenarioSequence(string scenarioScopeCode, string scenarioTypeCode, int financialYear)
        {
            try
            {
                var sequence = _scenarioBL.GetScenarioSequence(scenarioScopeCode, scenarioTypeCode, financialYear);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<string>(HttpStatusCode.OK, sequence, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetScenarioById", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/scenariosofproject/{ProjectId}")]
        public HttpResponseMessage GetAllScenarioOfProject(int projectid)
        {
            try
            {
                List<Scenario> Scenarios = _scenarioBL.GetAllScenarioOfProject(projectid).ToList();
                List<ScenarioViewModel> ScenarioViewModel = new List<ScenarioViewModel>();
                Mapper.Map(Scenarios, ScenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.OK, ScenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/duplicatescenario")]
        public HttpResponseMessage DuplicateScenario(ScenarioViewModel scenarioView)
        {
            try
            {
                //scenarioView.CreatedBy = User.Identity.Name.GetUserName();
                Scenario scenariodatamodel = new Scenario();
                Mapper.Map(scenarioView, scenariodatamodel);
                var scenario = _scenarioBL.DuplicateScenario(scenariodatamodel);
                var message = "";
                if (scenario != null && scenario.message == "Success")
                {
                    scenarioView.ScenarioID = scenario.ScenarioID;
                    scenarioView.ScenarioName = scenario.ScenarioName;
                    scenarioView.ScenarioLock = false;
                    if (scenarioView.ScenarioScopeCode == "OI")
                    {
                        scenarioView.ScenarioScopeName = "ORDERINTAKE";
                    }
                    else if (scenarioView.ScenarioScopeCode == "PL")
                    {
                        scenarioView.ScenarioScopeName = "PROFIT AND LOSS";
                    }
                    else if (scenarioView.ScenarioScopeCode == "BL")
                    {
                        scenarioView.ScenarioScopeName = "BACKLOG";
                    }
                    if (scenarioView.ScenarioTypeCode == "BD")
                    {
                        scenarioView.ScenarioTypeName = "BUDGET";
                    }
                    else if (scenarioView.ScenarioTypeCode == "FC")
                    {
                        scenarioView.ScenarioTypeName = "FORECAST";
                    }
                    else if (scenarioView.ScenarioTypeCode == "AC")
                    {
                        scenarioView.ScenarioTypeName = "ACTUALS";
                    }
                    
                    message = new CommonBL().GetMessage("SDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioViewModel>(HttpStatusCode.OK, scenarioView, "", message, "", ""));

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, scenario.message, "", ""));

                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "DuplicateScenario", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/removescenarioproject")]
        public HttpResponseMessage RemoveScenarioProject(ProjectScenarioViewModel projectScenarioViewModel)
        {
            try
            {
                ProjectScenarioModel projectScenarioModel = new ProjectScenarioModel();
                Mapper.Map(projectScenarioViewModel, projectScenarioModel);
                //projectScenarioModel.CreatedBy = User.Identity.Name.GetUserName();
                var check = _scenarioBL.RemoveScenarioProjects(projectScenarioModel);
                if (check == "Success")
                {
                    var message = new CommonBL().GetMessage("PRS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<bool>(HttpStatusCode.BadRequest, false, null, check, "", ""));

                }

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "RemoveScenarioProject", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }
        [HttpPost]
        [Route("api/scenario/updatewipstatus")]
        public HttpResponseMessage UpdateWIPStatus(ScenarioProjectLogViewModel scenarioProjectLogViewModel)
        {

            try
            {
                ScenarioProjectLog dbEntity = new ScenarioProjectLog();
                //scenarioProjectLogViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(scenarioProjectLogViewModel, dbEntity);
                dbEntity = _scenarioBL.ChangeScenarionStatus(dbEntity);


                if (dbEntity != null && dbEntity.ScenarioProjectLogID > 0)
                {
                    Mapper.Map(dbEntity, scenarioProjectLogViewModel);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioProjectLogViewModel>(HttpStatusCode.OK, scenarioProjectLogViewModel, null, "", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "", null, "", "", ""));

                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "UpdateWIPStatus", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", ex.ToString(), " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenario/getwipstatus")]
        public HttpResponseMessage GetWIPStatus(ScenarioProjectLogViewModel scenarioProjectLogViewModel)
        {

            try
            {
                ScenarioProjectLog dbEntity = new ScenarioProjectLog();
                //scenarioProjectLogViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(scenarioProjectLogViewModel, dbEntity);
                dbEntity = _scenarioBL.GetWIPStatus(dbEntity);


                if (dbEntity != null && dbEntity.ScenarioProjectLogID > 0)
                {
                    Mapper.Map(dbEntity, scenarioProjectLogViewModel);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioProjectLogViewModel>(HttpStatusCode.OK, scenarioProjectLogViewModel, null, "", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioProjectLogViewModel>(HttpStatusCode.OK, null, null, "", "", ""));

                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetWIPStatus", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenario/IsScenarioLockedforProjectUpdate")]
        public HttpResponseMessage IsScenarioLockedforProjectUpdate(ScenarioProjectLogViewModel scenarioProjectLogViewModel)
        {

            try
            {
                ScenarioProjectLog dbEntity = new ScenarioProjectLog();
                //scenarioProjectLogViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(scenarioProjectLogViewModel, dbEntity);
                dbEntity = _scenarioBL.IsScenarioLockedforProjectUpdate(dbEntity);


                if (dbEntity != null && dbEntity.ScenarioProjectLogID > 0)
                {
                    Mapper.Map(dbEntity, scenarioProjectLogViewModel);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioProjectLogViewModel>(HttpStatusCode.OK, scenarioProjectLogViewModel, null, "", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioProjectLogViewModel>(HttpStatusCode.OK, null, null, "", "", ""));

                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "IsScenarioLockedforProjectUpdate", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }


        [HttpPost]
        [Route("api/scenario/IsScenarioLockedforUpload")]
        public HttpResponseMessage IsScenarioLockedforUpload(ScenarioProjectLogViewModel scenarioProjectLogViewModel)
        {

            try
            {
                ScenarioProjectLog dbEntity = new ScenarioProjectLog();
                //scenarioProjectLogViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(scenarioProjectLogViewModel, dbEntity);
                dbEntity = _scenarioBL.GetWIPStatus(dbEntity);


                if (dbEntity != null && dbEntity.ScenarioProjectLogID > 0)
                {
                    Mapper.Map(dbEntity, scenarioProjectLogViewModel);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioProjectLogViewModel>(HttpStatusCode.OK, scenarioProjectLogViewModel, null, "", "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ScenarioProjectLogViewModel>(HttpStatusCode.OK, null, null, "", "", ""));

                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "IsScenarioLockedforUpload", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/scenario/getwiptruescenarios")]
        public HttpResponseMessage GetAllWIPStatusTrueScenario()
        {

            try
            {
                List<ScenarioIDS> scenarioids = _scenarioBL.GetAllWIPStatusTrueScenario().ToList();
                List<ScenarioIDSViewModel> scenarioidsViewModel = new List<ScenarioIDSViewModel>();
                Mapper.Map(scenarioids, scenarioidsViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioIDSViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioIDSViewModel>>(HttpStatusCode.OK, scenarioidsViewModel, null, "", "", ""));
            }
            catch (Exception ex)
            {

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured:", "", "", ""));
            }
        }

        [HttpPost]
        [Route("api/scenarios/getscenariobyyear/{year}")]
        public HttpResponseMessage GetScenarioByYear(int year)
        {
            try
            {
                List<Scenario> scenarios = _scenarioBL.GetScenarioByYear(year).ToList();
                List<ScenarioViewModel> scenarioViewModel = new List<ScenarioViewModel>();

                Mapper.Map(scenarios, scenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.OK, scenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                // RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured:" + ex.ToString(), "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarios/getOrgscenariobyyear/{year}")]
        public HttpResponseMessage GetOrgScenarioByYear(int year)
        {
            try
            {
                List<Scenario> scenarios = _scenarioBL.GetOrgScenarioByYear(year).ToList();
                List<ScenarioViewModel> scenarioViewModel = new List<ScenarioViewModel>();

                Mapper.Map(scenarios, scenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.OK, scenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                // RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured:" + ex.ToString(), "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarios/getscenariobyscenarioid/{scenarioId}/{year}")]
        public HttpResponseMessage GetScenarioByScenarioId(int scenarioId, int year)
        {
            try
            {

                List<Scenario> scenarios = _scenarioBL.GetScenarioByScenarioId(scenarioId, year).ToList();
                List<ScenarioViewModel> scenarioViewModel = new List<ScenarioViewModel>();
                Mapper.Map(scenarios, scenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.OK, scenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                // RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured:" + ex.ToString(), "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/scenarios/getscenariobyyearandscope")]
        public HttpResponseMessage Getscenariobyyearandscope(DashboardConfig config)
        {
            try
            {
                List<Scenario> scenarios = _scenarioBL.GetScenariosByYearAndScope(config).ToList();
                List<ScenarioViewModel> scenarioViewModel = new List<ScenarioViewModel>();

                Mapper.Map(scenarios, scenarioViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.OK, scenarioViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                // RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioController", "GetAllScenario", "");

                return Request.CreateResponse<APIResponse<List<ScenarioViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioViewModel>>(HttpStatusCode.InternalServerError, null, "Exception Occured:" + ex.ToString(), "", "", ""));

            }
        }
    }
}
