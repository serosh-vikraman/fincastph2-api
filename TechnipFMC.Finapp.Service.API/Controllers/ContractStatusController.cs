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
    /// ContractStatus Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ContractStatusController : ApiController
    {
        private readonly IContractStatusBL _contractStatusBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ContractStatusBL"></param>
        /// 

        public ContractStatusController(IContractStatusBL contractStatusBL)
        {
            _contractStatusBL = contractStatusBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/contractstatus/getall")]
        public HttpResponseMessage GetAllContractStatus()
        {
            try
            {
                List<ContractStatus> contractStatus = _contractStatusBL.GetAll().ToList();
                List<ContractStatusViewModel> contractStatusModel = new List<ContractStatusViewModel>();
                Mapper.Map(contractStatus, contractStatusModel);

                return Request.CreateResponse<APIResponse<List<ContractStatusViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ContractStatusViewModel>>(HttpStatusCode.OK, contractStatusModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractStatusController", "GetAllContractStatus", "");

                return Request.CreateResponse<APIResponse<List<ContractStatusViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ContractStatusViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/contractstatus/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ContractStatus> contractStatus = _contractStatusBL.GetAll().ToList();
                List<ContractStatusViewModel> contractStatusModel = new List<ContractStatusViewModel>();
                var entities = contractStatus.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, contractStatusModel);

                return Request.CreateResponse<APIResponse<List<ContractStatusViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ContractStatusViewModel>>(HttpStatusCode.OK, contractStatusModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractStatusController", "GetAllContractStatus", "");

                return Request.CreateResponse<APIResponse<List<ContractStatusViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ContractStatusViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/contractstatus/getbyid/{Id}")]
        public HttpResponseMessage GetContractStatusById(int Id)
        {
            try
            {
                ContractStatus contractStatus = _contractStatusBL.GetById(Id);
                ContractStatusViewModel contractStatusModel = new ContractStatusViewModel();
                Mapper.Map(contractStatus, contractStatusModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ContractStatusViewModel>(HttpStatusCode.OK, contractStatusModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractStatusController", "GetContractStatus", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ContractStatusViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/contractstatus/save")]
        public HttpResponseMessage SaveContractStatus(ContractStatusViewModel contractStatusViewModel)
        {
            try
            {
                ContractStatus contractStatusDatamodel = new ContractStatus();
                //contractStatusViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(contractStatusViewModel, contractStatusDatamodel);
                var contractStatus = _contractStatusBL.Save(contractStatusDatamodel);
                var message = "";
                if (contractStatus != null && contractStatus.ContractStatusID > 0)
                {
                    contractStatusViewModel.ContractStatusID = contractStatus.ContractStatusID;
                    if (contractStatusDatamodel.Active == true)
                    {
                        contractStatusViewModel.Status = "Active";
                    }
                    else
                    {
                        contractStatusViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("CSSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ContractStatusViewModel>(HttpStatusCode.OK, contractStatusViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CSNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("CSCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));

                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CSCAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));

                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractStatusController", "SaveContractStatus", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/contractstatus/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteContractStatus(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _contractStatusBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {

                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ContractStatusController", "DeleteContractStatus", "Deleted ContractStatus Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("CSDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CSDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CSDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractStatusController", "DeleteContractStatus", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

    }
}
