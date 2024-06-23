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
    /// GroupingParameters Controller
    /// </summary>

    //[Authorize] //remove comment
    public class GroupingParametersController : ApiController
    {
        private readonly IGroupingParametersBL _groupingParametersBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="GroupingParametersBL"></param>
        /// 

        public GroupingParametersController(IGroupingParametersBL groupingParametersBL)
        {
            _groupingParametersBL = groupingParametersBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/groupingparameters/getall")]
        public HttpResponseMessage GetAllGroupingParameters()
        {
            try
            {
                List<GroupingParameters> groupingParameters = _groupingParametersBL.GetAll().ToList();
                List<GroupingParametersViewModel> groupingParametersModel = new List<GroupingParametersViewModel>();
                Mapper.Map(groupingParameters, groupingParametersModel);

                return Request.CreateResponse<APIResponse<List<GroupingParametersViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<GroupingParametersViewModel>>(HttpStatusCode.OK, groupingParametersModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.GroupingParametersController", "GetAllGroupingParameters", "");

                return Request.CreateResponse<APIResponse<List<GroupingParametersViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<GroupingParametersViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/groupingparameters/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<GroupingParameters> groupingParameters = _groupingParametersBL.GetAll().ToList();
                List<GroupingParametersViewModel> groupingParametersModel = new List<GroupingParametersViewModel>();
                var entities = groupingParameters.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, groupingParametersModel);

                return Request.CreateResponse<APIResponse<List<GroupingParametersViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<GroupingParametersViewModel>>(HttpStatusCode.OK, groupingParametersModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.GroupingParametersController", "GetAllGroupingParameters", "");

                return Request.CreateResponse<APIResponse<List<GroupingParametersViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<GroupingParametersViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/groupingparameters/getbyid/{Id}")]
        public HttpResponseMessage GetGroupingParametersById(int Id)
        {
            try
            {
                GroupingParameters groupingParameters = _groupingParametersBL.GetById(Id);
                GroupingParametersViewModel groupingParametersModel = new GroupingParametersViewModel();
                Mapper.Map(groupingParameters, groupingParametersModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<GroupingParametersViewModel>(HttpStatusCode.OK, groupingParametersModel, null, "", "", ""));
                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.GroupingParametersController", "GetGroupingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<GroupingParametersViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/groupingparameters/save")]
        public HttpResponseMessage SaveGroupingParameters(GroupingParametersViewModel groupingParametersViewModel)
        {
            try
            {

                GroupingParameters groupingParametersDatamodel = new GroupingParameters();
                //groupingParametersViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(groupingParametersViewModel, groupingParametersDatamodel);
                var groupingParameters = _groupingParametersBL.Save(groupingParametersDatamodel);
                var message = "";
                if (groupingParameters != null && groupingParameters.GroupingParametersID > 0)
                {
                    groupingParametersViewModel.GroupingParametersID = groupingParameters.GroupingParametersID;
                    if (groupingParametersDatamodel.Active == true)
                    {
                        groupingParametersViewModel.Status = "Active";
                    }
                    else
                    {
                        groupingParametersViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("GPSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<GroupingParametersViewModel>(HttpStatusCode.OK, groupingParametersViewModel,null , message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("GPNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }

               
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("GPCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("GPNUAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.GroupingParametersController", "SaveGroupingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.GroupingParametersController", "SaveGroupingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  "", "Exception occured.", "", "", ""));

            }
        }

        [HttpPost]
        [Route("api/groupingparameters/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteGroupingParameters(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _groupingParametersBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.GroupingParametersController", "DeleteGroupingParameters", "Deleted GroupingParameter Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("GPDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("GPNDAUP");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("GPNDAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.GroupingParametersController", "DeleteGroupingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "","Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.GroupingParametersController", "DeleteGroupingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

    }
}
