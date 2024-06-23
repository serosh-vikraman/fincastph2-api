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
using System.Net.Http.Headers;
using System.Web;
using System.IO;

namespace TechnipFMC.Finapp.Service.API.Controllers
{
    /// <summary>
    /// Project Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ProjectController : ApiController
    {
        private readonly IProjectBL _projectBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ProjectBL"></param>
        /// 

        public ProjectController(IProjectBL projectBL)
        {
            _projectBL = projectBL;

        }
        /// <summary>
        /// Get All Project
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/projects/getall/{DepartmentId}")]
        public HttpResponseMessage GetAllProject(int departmentId)
         {
            try
            {
                List<Project> projects = _projectBL.GetAll(departmentId).ToList();
                List<ProjectViewModel> projectViewModel = new List<ProjectViewModel>();
                Mapper.Map(projects, projectViewModel);

                return Request.CreateResponse<APIResponse<List<ProjectViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectViewModel>>(HttpStatusCode.OK, projectViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "GetAllProject", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projects/getall/search/{DepartmentId}")]
        public HttpResponseMessage Search(int departmentId)
        {
            try
            {
                List<Project> projects = _projectBL.GetAll(departmentId).ToList();
                List<ProjectViewModel> projectViewModel = new List<ProjectViewModel>();
                var entities = projects.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, projectViewModel);

                return Request.CreateResponse<APIResponse<List<ProjectViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectViewModel>>(HttpStatusCode.OK, projectViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "GetAllProject", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projects/getbyid/{Id}")]
        public HttpResponseMessage GetProjectById(int Id)
        {
            try
            {
                Project projects = _projectBL.GetById(Id);
                ProjectViewModel projectViewModel = new ProjectViewModel();
                Mapper.Map(projects, projectViewModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ProjectViewModel>(HttpStatusCode.OK, projectViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "GetProjectById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/projects/save")]
        public HttpResponseMessage SaveProject(ProjectViewModel projectViewModel)
        {
            try
            {

                Project projectDataModel = new Project();
                //projectViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(projectViewModel, projectDataModel);
                var projects = _projectBL.Save(projectDataModel);
                var message = "";
                //if (projects != null && projects.ProjectID > 0)
                if (projects != null && projects.Message == "Success")
                {
                    projectViewModel.ProjectID = projects.ProjectID;
                    projectViewModel.ProjectStatus = projects.ProjectStatus;
                    message = new CommonBL().GetMessage("PRSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ProjectViewModel>(HttpStatusCode.OK, projectViewModel, null, message, "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, projects.Message, "", ""));
                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY constraint 'UK_Project_ManualProjectCode'"))
                {
                    var message = new CommonBL().GetMessage("MPCAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ProjectViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                    if (ex.Message.Contains("UNIQUE KEY constraint 'UK_Project_ProjectName'"))
                {
                    var message = new CommonBL().GetMessage("PNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ProjectViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractTypeController", "SaveContractType", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ProjectViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "SaveProject", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/projects/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteProject(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deletedSuccess = _projectBL.Delete(Id, DeletedBy);
                var message = "";
                if (deletedSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ProjectController", "DeleteProject", "Deleted Project Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("PDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("PAS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("PAS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ActionController", "DeleteProject", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/projectsofscenario/{ScenarioId}")]
        public HttpResponseMessage GetAllProjectsOfScenario(int scenarioId)
        {
            try
            {
                List<ProjectsToLink> projects = _projectBL.GetAllProjectsOfScenario(scenarioId).ToList();
                List<ProjectsToLinkViewModel> projectViewModel = new List<ProjectsToLinkViewModel>();
                Mapper.Map(projects, projectViewModel);

                return Request.CreateResponse<APIResponse<List<ProjectsToLinkViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectsToLinkViewModel>>(HttpStatusCode.OK, projectViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "GetAllProjectsOfScenario", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/inactiveprojectsofscenario/{ScenarioId}")]
        public HttpResponseMessage GetAllInactiveProjectsOfScenario(int scenarioId)
        {
            try
            {
                //List<ProjectsToLink> projects = _projectBL.GetAllInactiveProjectsOfScenario(scenarioId).ToList();
                //List<ProjectsToLinkViewModel> projectViewModel = new List<ProjectsToLinkViewModel>();
                //Mapper.Map(projects, projectViewModel);
                int count = _projectBL.GetAllInactiveProjectsOfScenario(scenarioId);
                return Request.CreateResponse<APIResponse<int>>(HttpStatusCode.OK,
                    new APIResponse<int>(HttpStatusCode.OK, count, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "GetAllInactiveProjectsOfScenario", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpGet]
        [Route("api/downloadprojecttemplate")]
        public HttpResponseMessage DownloadProjectTemplate()
        {
            try
            {
                //string CreatedBy = User.Identity.Name.GetUserName();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

                //Read the File into a Byte Array.
                byte[] bytes = _projectBL.DownloadProjectTemplate();

                //Set the Response Content.
                response.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                response.Content.Headers.ContentLength = bytes.LongLength;

                //Set the Content Disposition Header Value and FileName.
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "Project_Template.xlsx";

                //Set the File Content Type.
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping("Project_Template.xlsx"));


                return response;

                //return Request.CreateResponse<APIResponse<HttpResponseMessage>>(HttpStatusCode.OK,
                //    new APIResponse<HttpResponseMessage>(HttpStatusCode.OK, response, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "GetScenariouploadlog", "");
                return Request.CreateResponse<APIResponse<HttpResponseMessage>>(HttpStatusCode.InternalServerError,
                    new APIResponse<HttpResponseMessage>(HttpStatusCode.InternalServerError, null, "Exception occured." + ex.ToString(), "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/uploadprojectdata")]
        public HttpResponseMessage UploadProjectData()
        {
            try
            {
                string defaultPath = System.Configuration.ConfigurationManager.AppSettings["ProjectFilePath"].ToString();
                int cntSuccess = 0;
                var filePath = "";
                var httpRequest = HttpContext.Current.Request;

                var fileName = (httpRequest.Form["FileName"] != null || httpRequest.Form["FileName"] != "") ? httpRequest.Form["FileName"].Split('\\').ToList().LastOrDefault() : null;
                var userName = User.Identity.Name.GetUserName();
                if (!Directory.Exists(defaultPath))
                {
                    Directory.CreateDirectory(defaultPath);
                }

                if (fileName == null || fileName == "" || httpRequest.Files.Count == 0)
                {
                    return Request.CreateResponse<APIResponse<List<string>>>(HttpStatusCode.BadRequest,
                   new APIResponse<List<string>>(HttpStatusCode.BadRequest, null, null, "No Files selected", "", ""));
                }
                var sessionId = Guid.NewGuid().ToString();
                fileName = sessionId + "_" + fileName;

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[0];
                        filePath = defaultPath + fileName;
                        try
                        {
                            postedFile.SaveAs(filePath);
                            cntSuccess++;
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }

                var response = _projectBL.UploadProjectData(sessionId, fileName, defaultPath, userName);
                return Request.CreateResponse<APIResponse<ProjectFileUploadResponse>>(HttpStatusCode.OK,
                   new APIResponse<ProjectFileUploadResponse>(HttpStatusCode.OK, response, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "UploadProjectData", "");
                return Request.CreateResponse<APIResponse<List<string>>>(HttpStatusCode.InternalServerError,
                   new APIResponse<List<string>>(HttpStatusCode.InternalServerError, "", "Exception occured." + ex.ToString(), "", " ", " "));

            }
        }


        [HttpPost]
        [Route("api/getprojecterrorlog/{sessionId}")]
        public HttpResponseMessage GetProjectErrorLog(string sessionId)
        {
            try
            {

                var response = _projectBL.GetProjectErrorLog(sessionId);
                return Request.CreateResponse<APIResponse<List<ProjectUploadLog>>>(HttpStatusCode.OK,
                   new APIResponse<List<ProjectUploadLog>>(HttpStatusCode.OK, response, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "UploadProjectData", "");
                return Request.CreateResponse<APIResponse<List<string>>>(HttpStatusCode.InternalServerError,
                   new APIResponse<List<string>>(HttpStatusCode.InternalServerError, "", "Exception occured." + ex.ToString(), "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/allprojectsforprojectlifecyclereport")]
        public HttpResponseMessage GetAllProjectsToLinkForLifeCycleReport()
        {
            try
            {
                List<ProjectDeLink> projects = _projectBL.GetAllProjectsToLinkForLifeCycleReport().ToList();
                List<ProjectDeLink> projectViewModel = new List<ProjectDeLink>();
                Mapper.Map(projects, projectViewModel);

                return Request.CreateResponse<APIResponse<List<ProjectDeLink>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectDeLink>>(HttpStatusCode.OK, projectViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "GetAllProjectsToLink", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projects/getallprojects")]
        public HttpResponseMessage GetAllProjects()
        {
            try
            {
                List<Projects> projects = _projectBL.GetAllProjects().ToList();
                List<ProjectsViewModel> projectViewModel = new List<ProjectsViewModel>();
                Mapper.Map(projects, projectViewModel);

                return Request.CreateResponse<APIResponse<List<ProjectsViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectsViewModel>>(HttpStatusCode.OK, projectViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectController", "GetAllProject", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

    }
}
