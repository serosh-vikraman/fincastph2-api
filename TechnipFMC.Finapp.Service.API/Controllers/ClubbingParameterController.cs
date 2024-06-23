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
    /// ClubbingParameter Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ClubbingParameterController : ApiController
    {
        private readonly IClubbingParameterBL _clubbingParameterBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ClubbingParametersBL"></param>
        /// 

        public ClubbingParameterController(IClubbingParameterBL clubbingParameterBL)
        {
            _clubbingParameterBL = clubbingParameterBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/clubbingparameters/getall")]
        public HttpResponseMessage GetAllClubbingParameters()
        {
            try
            {
                List<ClubbingParameter> clubbingParameter = _clubbingParameterBL.GetAll().ToList();
                List<ClubbingParameterViewModel> clubbingParameterModel = new List<ClubbingParameterViewModel>();
                Mapper.Map(clubbingParameter, clubbingParameterModel);

                return Request.CreateResponse<APIResponse<List<ClubbingParameterViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ClubbingParameterViewModel>>(HttpStatusCode.OK, clubbingParameterModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClubbingParameterController", "GetAllClubbingParameters", "");

                return Request.CreateResponse<APIResponse<List<ClubbingParameterViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ClubbingParameterViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/clubbingparameters/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ClubbingParameter> clubbingParameters = _clubbingParameterBL.GetAll().ToList();
                List<ClubbingParameterViewModel> clubbingParameterModel = new List<ClubbingParameterViewModel>();
                var entities = clubbingParameters.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, clubbingParameterModel);

                return Request.CreateResponse<APIResponse<List<ClubbingParameterViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ClubbingParameterViewModel>>(HttpStatusCode.OK, clubbingParameterModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClubbingParametersController", "GetAllClubbingParameters", "");

                return Request.CreateResponse<APIResponse<List<ClubbingParameterViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ClubbingParameterViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/clubbingparameters/getbyid/{Id}")]
        public HttpResponseMessage GetClubbingParametersById(int Id)
        {
            try
            {
                ClubbingParameter clubbingParameter = _clubbingParameterBL.GetById(Id);
                ClubbingParameterViewModel clubbingParameterModel = new ClubbingParameterViewModel();
                Mapper.Map(clubbingParameter, clubbingParameterModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ClubbingParameterViewModel>(HttpStatusCode.OK, clubbingParameterModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClubbingParametersController", "GetClubbingParametersById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ClubbingParameterViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/clubbingparameters/save")]
        public HttpResponseMessage SaveClubbingParameters(ClubbingParameterViewModel clubbingParameterViewModel)
        {
            try
            {

                ClubbingParameter clubbingParameterDatamodel = new ClubbingParameter();
                //clubbingParameterViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(clubbingParameterViewModel, clubbingParameterDatamodel);
                var clubbingParameters = _clubbingParameterBL.Save(clubbingParameterDatamodel);
                var message = "";
                if (clubbingParameters != null && clubbingParameters.ClubbingParameterID > 0)
                {
                    clubbingParameterViewModel.ClubbingParameterID = clubbingParameters.ClubbingParameterID;
                    if (clubbingParameterDatamodel.Active == true)
                    {
                        clubbingParameterViewModel.Status = "Active";
                    }
                    else
                    {
                        clubbingParameterViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("CPSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ClubbingParameterViewModel>(HttpStatusCode.OK, clubbingParameterViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CPNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("CPCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CPNUAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClubbingParametersController", "SaveClubbingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClubbingParametersController", "SaveClubbingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", "", ""));

            }
        }

        [HttpPost]
        [Route("api/clubbingparameters/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteClubbingParameters(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _clubbingParameterBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ClubbingParametersController", "DeleteClubbingParameters", "Deleted ClubbingParameter Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("CPDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CPNDAUP");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CPNDAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClubbingParametersController", "DeleteClubbingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClubbingParametersController", "DeleteClubbingParameters", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

    }
}
