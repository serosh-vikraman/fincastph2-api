using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using TechnipFMC.Common;
using TechnipFMC.Finapp.Business;
using TechnipFMC.Finapp.Business.Interfaces;
using TechnipFMC.Finapp.Models;
using TechnipFMC.Finapp.Service.API.ViewModel;

namespace TechnipFMC.Finapp.Service.API.Controllers
{
    public class ScenarioFileUploadController : ApiController
    {
        private readonly IScenarioFileBL _scenarioFileBL;

        public ScenarioFileUploadController(IScenarioFileBL scenarioFileBL, IScenarioBL scenarioBL)
        {
            _scenarioFileBL = scenarioFileBL;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadprojectscenariodata")]
        public HttpResponseMessage UploadProjectScenarioData()
        {
            try
            {
                string defaultPath = System.Configuration.ConfigurationManager.AppSettings["DefaultFilePath"].ToString();
                int cntSuccess = 0;
                var filePath = "";
                var httpRequest = HttpContext.Current.Request;

                var scenarioId = (httpRequest.Form["ScenarioId"] != null || httpRequest.Form["ScenarioId"] != "") ? Convert.ToInt32(httpRequest.Form["ScenarioId"]) : 0;
                var fileName = (httpRequest.Form["FileName"] != null || httpRequest.Form["FileName"] != "") ? httpRequest.Form["FileName"].Split('\\').ToList().LastOrDefault() : null;
                var userName = (httpRequest.Form["UserName"] != null || httpRequest.Form["UserName"] != "") ? httpRequest.Form["UserName"] : null;
                var customerId = (httpRequest.Form["CustomerId"] != null || httpRequest.Form["CustomerId"] != "") ? Convert.ToInt32(httpRequest.Form["CustomerId"]) : 0;
                var departmentId = (httpRequest.Form["CustomerId"] != null || httpRequest.Form["CustomerId"] != "") ? Convert.ToInt32(httpRequest.Form["DepartmentId"]) : 0;
                var clientId = (httpRequest.Form["CustomerId"] != null || httpRequest.Form["CustomerId"] != "") ? Convert.ToInt32(httpRequest.Form["ClientId"]) : 0;
                var activeQuarters = (httpRequest.Form["ActiveQuarters"] != null || httpRequest.Form["ActiveQuarters"] != "") ? httpRequest.Form["ActiveQuarters"] : null;
                if(!Directory.Exists(defaultPath))
                {
                    Directory.CreateDirectory(defaultPath);
                }
                userName = User.Identity.Name.GetUserName();
                if (fileName == null || fileName == "" || httpRequest.Files.Count == 0)
                {
                    return Request.CreateResponse<APIResponse<List<string>>>(HttpStatusCode.BadRequest,
                   new APIResponse<List<string>>(HttpStatusCode.BadRequest, null, null, "No Files selected", "", ""));
                }

                fileName = Guid.NewGuid().ToString() + "_" + fileName;

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

                ScenarioFileUploadResponse scenarioFileSession = _scenarioFileBL.UploadProjectScenarioData(scenarioId, activeQuarters, fileName, filePath, userName, customerId,departmentId,clientId);
                return Request.CreateResponse<APIResponse<ScenarioFileUploadResponse>>(HttpStatusCode.OK,
                   new APIResponse<ScenarioFileUploadResponse>(HttpStatusCode.OK, scenarioFileSession, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "UploadProjectScenarioData", "");
                return Request.CreateResponse<APIResponse<List<string>>>(HttpStatusCode.InternalServerError,
                   new APIResponse<List<string>>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/getscenarioerrorlog/{sessionid}")]
        public HttpResponseMessage GetScenarioErrorlog(string sessionid)
        {
            try
            {
                List<ScenarioUploadLog> ScenarioUploadLogs = _scenarioFileBL.GetScenarioErrorlog(sessionid);
                List<ScenarioUploadLogViewModel> ScenarioUploadLogsViewModel = new List<ScenarioUploadLogViewModel>();
                Mapper.Map(ScenarioUploadLogs, ScenarioUploadLogsViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioUploadLogViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioUploadLogViewModel>>(HttpStatusCode.OK, ScenarioUploadLogsViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "GetScenarioErrorlog", "");
                return Request.CreateResponse<APIResponse<List<ScenarioUploadLogViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioUploadLogViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/getscenarioFailedlog/{sessionid}")]
        public HttpResponseMessage GetScenarioFailedlog(string sessionid)
        {
            try
            {
                List<ScenarioUploadLog> ScenarioUploadLogs = _scenarioFileBL.GetScenarioFailedlog(sessionid);
                List<ScenarioUploadLogViewModel> ScenarioUploadLogsViewModel = new List<ScenarioUploadLogViewModel>();
                Mapper.Map(ScenarioUploadLogs, ScenarioUploadLogsViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioUploadLogViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioUploadLogViewModel>>(HttpStatusCode.OK, ScenarioUploadLogsViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "GetScenarioFailedlog", "");
                return Request.CreateResponse<APIResponse<List<ScenarioUploadLogViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioUploadLogViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
       
        [HttpPost]
        [Route("api/getscenariouploadlog")]
        public HttpResponseMessage GetScenariouploadlog()
        {
            try
            {
                List<ScenarioFile> ScenarioUploadLogs = _scenarioFileBL.GetScenariouploadlog();
                List<ScenarioFileViewModel> ScenarioUploadLogsViewModel = new List<ScenarioFileViewModel>();
                Mapper.Map(ScenarioUploadLogs, ScenarioUploadLogsViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioFileViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioFileViewModel>>(HttpStatusCode.OK, ScenarioUploadLogsViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "GetScenariouploadlog", "");
                return Request.CreateResponse<APIResponse<List<ScenarioUploadLogViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioUploadLogViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/GetuploadToken/{UploadsessionId}/{SecondTime}/{Page}/{UserAction}")]
        public HttpResponseMessage GetuploadToken(string UploadsessionId, int SecondTime, string Page, string UserAction)
        {
            string CreatedBy = User.Identity.Name.GetUserName();
            try
            {
                string token = _scenarioFileBL.GetuploadToken(UploadsessionId, CreatedBy, SecondTime);
               
              
                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.OK,
                    new APIResponse<string>(HttpStatusCode.OK, token, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "GetScenariouploadlog", "");
                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpGet]
        [Route("api/GetuploadFile/{token}/{UploadsessionId}/{Page}/{UserAction}")]
        public HttpResponseMessage GetuploadFile(string token, string UploadsessionId, string Page, string UserAction)
        {
            try
            {
                string CreatedBy = User.Identity.Name.GetUserName();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

                ScenarioFile fileInfo = _scenarioFileBL.GetuploadFile(token);
                string RemoteFilePath = System.Configuration.ConfigurationManager.AppSettings["DefaultFilePath"].ToString();
                if (fileInfo.ScenarioFileName != "")
                {
                    
                    //Set the File Path.
                    //string filePath = HttpContext.Current.Server.MapPath("~/") + fileName;
                    // var basepath = AppDomain.CurrentDomain.BaseDirectory;
                    string docPath = fileInfo.ScenarioFileName;
                    string filePath = RemoteFilePath + docPath;
                    //Check whether File exists.
                    if (!File.Exists(filePath))
                    {
                        //Throw 404 (Not Found) exception if File not found.
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.ReasonPhrase = string.Format("File not found: {0} .", docPath);
                        throw new HttpResponseException(response);
                    }

                    //Read the File into a Byte Array.
                    byte[] bytes = File.ReadAllBytes(filePath);

                    //Set the Response Content.
                    response.Content = new ByteArrayContent(bytes);

                    //Set the Response Content Length.
                    response.Content.Headers.ContentLength = bytes.LongLength;

                    //Set the Content Disposition Header Value and FileName.
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = fileInfo.ScenarioFileName;

                    //Set the File Content Type.
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(docPath));
                   
                }
                return response;

                //return Request.CreateResponse<APIResponse<HttpResponseMessage>>(HttpStatusCode.OK,
                //    new APIResponse<HttpResponseMessage>(HttpStatusCode.OK, response, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "GetScenariouploadlog", "");
                return Request.CreateResponse<APIResponse<HttpResponseMessage>>(HttpStatusCode.InternalServerError,
                    new APIResponse<HttpResponseMessage>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/getscenarioerrorlogbyid/{scenarioId}")]
        public HttpResponseMessage GetScenarioErrorlogById(int scenarioId)
        {
            try
            {
                List<ScenarioUploadLog> ScenarioUploadLogs = _scenarioFileBL.GetScenarioErrorlogByScenarioId(scenarioId);
                List<ScenarioUploadLogViewModel> ScenarioUploadLogsViewModel = new List<ScenarioUploadLogViewModel>();
                Mapper.Map(ScenarioUploadLogs, ScenarioUploadLogsViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioUploadLogViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioUploadLogViewModel>>(HttpStatusCode.OK, ScenarioUploadLogsViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "GetScenarioErrorlogById", "");
                return Request.CreateResponse<APIResponse<List<ScenarioUploadLogViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ScenarioUploadLogViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/getscenariofileuploadlayout/{scenarioId}")]
        public HttpResponseMessage GetFileUploadLayout(int scenarioId)
        {
            try
            {
                var scenarioFileUploadLayout = _scenarioFileBL.GetFileUploadLayout(scenarioId).ToList();

                return Request.CreateResponse<APIResponse<List<FileUploadLayout>>>(HttpStatusCode.OK,
                    new APIResponse<List<FileUploadLayout>>(HttpStatusCode.OK, scenarioFileUploadLayout, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ScenarioFileUploadController", "GetFileUploadLayout", "");

                return Request.CreateResponse<APIResponse<List<FileUploadLayout>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<FileUploadLayout>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));
            }
        }
    }
}