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
    /// ProjectSegment Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ProjectSegmentController : ApiController
    {
        private readonly IProjectSegmentBL _projectSegmentBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ProjectSegmentBL"></param>
        /// 

        public ProjectSegmentController(IProjectSegmentBL projectSegmentBL)
        {
            _projectSegmentBL = projectSegmentBL;

        }
        /// <summary>
        /// Get All ProjectSegment 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/projectsegments/getall")]
        public HttpResponseMessage GetAllProjectSegment()
        {
            try
            {
                List<ProjectSegment> projectSegments = _projectSegmentBL.GetAll().ToList();
                List<ProjectSegmentViewModel> projectSegmentsModel = new List<ProjectSegmentViewModel>();
                Mapper.Map(projectSegments, projectSegmentsModel);

                return Request.CreateResponse<APIResponse<List<ProjectSegmentViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectSegmentViewModel>>(HttpStatusCode.OK, projectSegmentsModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectSegmentController", "GetAllProjectSegment", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectSegmentViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projectsegments/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ProjectSegment> projectSegments = _projectSegmentBL.GetAll().ToList();
                List<ProjectSegmentViewModel> projectSegmentsModel = new List<ProjectSegmentViewModel>();
                var entities = projectSegments.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, projectSegmentsModel);

                return Request.CreateResponse<APIResponse<List<ProjectSegmentViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectSegmentViewModel>>(HttpStatusCode.OK, projectSegmentsModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectSegmentController", "GetAllProjectSegment", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectSegmentViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projectsegments/getbyid/{Id}")]
        public HttpResponseMessage GetProjectSegmentById(int Id)
        {
            try
            {
                ProjectSegment source = _projectSegmentBL.GetById(Id );
                ProjectSegmentViewModel destination = new ProjectSegmentViewModel();
                Mapper.Map(source, destination);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ProjectSegmentViewModel>(HttpStatusCode.OK, destination, null, "", "", ""));

               
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectSegmentController", "GetProjectSegmentById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ProjectSegmentViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projectsegments/save")]
        public HttpResponseMessage SaveProjectSegment(ProjectSegmentViewModel projectSegmentViewModel)
        {
            try
            {
                ProjectSegment projectSegmentDataModel = new ProjectSegment();
                //projectSegmentViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(projectSegmentViewModel, projectSegmentDataModel);
                var projectSegment = _projectSegmentBL.Save(projectSegmentDataModel);
                var message = "";
                if (projectSegment != null && projectSegment.ProjectSegmentID > 0)
                {
                    projectSegmentViewModel.ProjectSegmentID = projectSegment.ProjectSegmentID;
                    if (projectSegmentDataModel.Active == true)
                    {
                        projectSegmentViewModel.Status = "Active";
                    }
                    else
                    {
                        projectSegmentViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("PSSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ProjectSegmentViewModel>(HttpStatusCode.OK, projectSegmentViewModel, null , message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("PSNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("PSCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));

                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("PSNUUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectSegmentController", "SaveProjectSegment", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/projectsegments/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteProjectSegment(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _projectSegmentBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    message = new CommonBL().GetMessage("PSDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("PSDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, "", message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("PSDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectSegmentController", "DeleteProjectSegment", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "","Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectSegmentController", "DeleteProjectSegment", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,"", "Exception occured.", "", " ", " "));

            }
        }


    }
}
