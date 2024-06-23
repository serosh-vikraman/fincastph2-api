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
    /// Project Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ProjectEntityController : ApiController
    {
        private readonly IProjectEntityBL _projectEntityBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ProjectEntityBL"></param>
        /// 

        public ProjectEntityController(IProjectEntityBL projectEntityBL)
        {
            _projectEntityBL = projectEntityBL;

        }
        /// <summary>
        /// Get All ProjectEntity 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/projectentities/getall")]
        public HttpResponseMessage GetAllProjectEntity()
        {
            try
            {
                List<ProjectEntity> projectEntitys = _projectEntityBL.GetAll().ToList();
                List<ProjectEntityViewModel> projectEntitysModel = new List<ProjectEntityViewModel>();
                Mapper.Map(projectEntitys, projectEntitysModel);

                return Request.CreateResponse<APIResponse<List<ProjectEntityViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectEntityViewModel>>(HttpStatusCode.OK, projectEntitysModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectEntityController", "GetAllProjectEntity", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectEntityViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projectentities/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ProjectEntity> projectEntitys = _projectEntityBL.GetAll().ToList();
                List<ProjectEntityViewModel> projectEntitysModel = new List<ProjectEntityViewModel>();
                var entities = projectEntitys.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, projectEntitysModel);

                return Request.CreateResponse<APIResponse<List<ProjectEntityViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectEntityViewModel>>(HttpStatusCode.OK, projectEntitysModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectEntityController", "GetAllProjectEntity", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ProjectEntityViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projectentities/getbyid/{Id}")]
        public HttpResponseMessage GetProjectEntityById(int Id)
        {
            try
            {
                ProjectEntity projectEntitys = _projectEntityBL.GetById(Id);
                ProjectEntityViewModel projectEntitysModel = new ProjectEntityViewModel();
                Mapper.Map(projectEntitys, projectEntitysModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ProjectEntityViewModel>(HttpStatusCode.OK, projectEntitysModel, null, "", "", ""));

               
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectEntityController", "GetProjectEntityById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ProjectEntityViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/projectentities/save")]
        public HttpResponseMessage SaveProjectEntity(ProjectEntityViewModel projectEntityViewModel)
        {
            try
            {
                ProjectEntity projectEntityDataModel = new ProjectEntity();
                //projectEntityViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(projectEntityViewModel, projectEntityDataModel);
                var projectEntity = _projectEntityBL.Save(projectEntityDataModel);
                var message = "";
                if (projectEntity != null && projectEntity.ProjectEntityID > 0)
                {
                    projectEntityViewModel.ProjectEntityID = projectEntity.ProjectEntityID;
                    if (projectEntityDataModel.Active == true)
                    {
                        projectEntityViewModel.Status = "Active";
                    }
                    else
                    {
                        projectEntityViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("PESS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ProjectEntityViewModel>(HttpStatusCode.OK, projectEntityViewModel,null,message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("PENS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }

                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("PECNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("PENUAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectEntityController", "SaveProjectEntity", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectEntityController", "SaveProjectEntity", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,null, "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/projectentities/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteProjectEntity(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _projectEntityBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    message = new CommonBL().GetMessage("PEDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("PEDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false,null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("PEDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectEntityController", "DeleteProjectEntity", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ProjectEntityController", "DeleteProjectEntity", "");

                return  Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  "", "Exception occured.", "", " ", " "));

            }
        }


    }
}
