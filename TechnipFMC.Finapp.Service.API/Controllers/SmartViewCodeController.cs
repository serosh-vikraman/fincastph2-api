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
    /// SmartViewCode Controller
    /// </summary>

    //[Authorize] //remove comment
    public class SmartViewCodeController : ApiController
    {
        private readonly ISmartViewCodeBL _smartViewCodeBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SmartViewCodeBL"></param>
        /// 

        public SmartViewCodeController(ISmartViewCodeBL smartViewCodeBL)
        {
            _smartViewCodeBL = smartViewCodeBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/smartviewcodes/getall")]
        public HttpResponseMessage GetAllSmartViewCode()
        {
            try
            {
                List<SmartViewCodeMaster> smartViewCodes = _smartViewCodeBL.GetAll().ToList();
                List<SmartViewCodeViewModel> smartViewCodesModel = new List<SmartViewCodeViewModel>();
                Mapper.Map(smartViewCodes, smartViewCodesModel);

                return Request.CreateResponse<APIResponse<List<SmartViewCodeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<SmartViewCodeViewModel>>(HttpStatusCode.OK, smartViewCodesModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SmartViewCodeController", "GetAllSmartViewCode", "");

                return Request.CreateResponse<APIResponse<List<SmartViewCodeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<SmartViewCodeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                
            }
        }
        [HttpPost]
        [Route("api/smartviewcodes/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<SmartViewCodeMaster> smartViewCodes = _smartViewCodeBL.GetAll().ToList();
                List<SmartViewCodeViewModel> smartViewCodesModel = new List<SmartViewCodeViewModel>();
                var entities = smartViewCodes.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, smartViewCodesModel);

                return Request.CreateResponse<APIResponse<List<SmartViewCodeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<SmartViewCodeViewModel>>(HttpStatusCode.OK, smartViewCodesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SmartViewCodeController", "GetAllSmartViewCode", "");

                return Request.CreateResponse<APIResponse<List<SmartViewCodeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<SmartViewCodeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/smartviewcodes/getbyid/{Id}")]
        public HttpResponseMessage GetSmartViewCodeById(int Id)
        {
            try
            {
                SmartViewCodeMaster source = _smartViewCodeBL.GetById(Id);
                SmartViewCodeViewModel destination = new SmartViewCodeViewModel();
                Mapper.Map(source, destination);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<SmartViewCodeViewModel>(HttpStatusCode.OK, destination, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SmartViewCodeController", "GetSmartViewCodeById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<SmartViewCodeViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
               
            }
        }
        [HttpPost]
        [Route("api/smartviewcodes/save")]
        public HttpResponseMessage SaveSmartViewCode(SmartViewCodeViewModel smartViewCodeViewModel)
        {
            try
            {
                SmartViewCodeMaster smartViewCodeDatamodel = new SmartViewCodeMaster();
                //smartViewCodeViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(smartViewCodeViewModel, smartViewCodeDatamodel);
                var smartViewCode = _smartViewCodeBL.Save(smartViewCodeDatamodel);
                var message = "";
                if (smartViewCode != null && smartViewCode.SmartViewCodeID > 0)
                {
                    smartViewCodeViewModel.SmartViewCodeID = smartViewCode.SmartViewCodeID;
                    if (smartViewCodeDatamodel.Active == true)
                    {
                        smartViewCodeViewModel.Status = "Active";
                    }
                    else
                    {
                        smartViewCodeViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("SVCSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<SmartViewCodeViewModel>(HttpStatusCode.OK, smartViewCodeViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("SVCNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }

                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("SVCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, "", message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("SVCNUAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SmartViewCodeController", "SaveSmartViewCode", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null,"Exception occured.","", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SmartViewCodeController", "SaveSmartViewCode", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));
                
            }
        }

        [HttpPost]
        [Route("api/smartviewcodes/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteSmartViewCode(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deletedSuccess = _smartViewCodeBL.Delete(Id, DeletedBy);
                var message = "";
                if (deletedSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.SmartViewCodeController", "DeleteSmartViewCode", "Deleted SmartViewCode Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("SVCDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("SVCDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));
                }

                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("SVCDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SmartViewCodeController", "DeleteSmartviewcode", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.SmartViewCodeController", "DeleteSmartviewcode", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                
            }
        }

    }
}
