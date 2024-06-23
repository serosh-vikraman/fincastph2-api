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
    /// ContractType Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ContractTypeController : ApiController
    {
        private readonly IContractTypeBL _contractTypeBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ContractTypeBL"></param>
        /// 

        public ContractTypeController(IContractTypeBL contractTypeBL)
        {
            _contractTypeBL = contractTypeBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/contracttypes/getall")]
        public HttpResponseMessage GetAllContractType()
        {
            try
            {
                List<ContractType> contractTypes = _contractTypeBL.GetAll().ToList();
                List<ContractTypeViewModel> contractTypesModel = new List<ContractTypeViewModel>();
                Mapper.Map(contractTypes, contractTypesModel);

                return Request.CreateResponse<APIResponse<List<ContractTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ContractTypeViewModel>>(HttpStatusCode.OK, contractTypesModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractTypesController", "GetAllContractType", "");

                return Request.CreateResponse<APIResponse<List<ContractTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ContractTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/contracttypes/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ContractType> contractTypes = _contractTypeBL.GetAll().ToList();
                List<ContractTypeViewModel> contractTypesModel = new List<ContractTypeViewModel>();
                var entities = contractTypes.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, contractTypesModel);

                return Request.CreateResponse<APIResponse<List<ContractTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ContractTypeViewModel>>(HttpStatusCode.OK, contractTypesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractTypesController", "GetAllContractType", "");

                return Request.CreateResponse<APIResponse<List<ContractTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ContractTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/contracttypes/getbyid/{Id}")]
        public HttpResponseMessage GetContractTypeById(int Id)
        {
            try
            {
                ContractType contractTypes = _contractTypeBL.GetById(Id);
                ContractTypeViewModel contractTypesModel = new ContractTypeViewModel();
                Mapper.Map(contractTypes, contractTypesModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ContractTypeViewModel>(HttpStatusCode.OK, contractTypesModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractTypeController", "GetContractTypeById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ContractTypeViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/contracttypes/save")]
        public HttpResponseMessage SaveContractType(ContractTypeViewModel contractTypeViewModel)
        {
            try
            {
                ContractType contractTypeDatamodel = new ContractType();
                //contractTypeViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(contractTypeViewModel, contractTypeDatamodel);
                var contractType = _contractTypeBL.Save(contractTypeDatamodel);
                var message = "";
                if (contractType == null)
                {
                    message = new CommonBL().GetMessage("CTNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<ContractTypeViewModel>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));
                }
                 if (contractType != null && contractType.ContractTypeID > 0)
                {
                    contractTypeViewModel.ContractTypeID = contractType.ContractTypeID;
                    if (contractTypeDatamodel.Active == true)
                    {
                        contractTypeViewModel.Status = "Active";
                    }
                    else
                    {
                        contractTypeViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("CTSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ContractTypeViewModel>(HttpStatusCode.OK, contractTypeViewModel, null , message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CTNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }               
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("CTCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));

                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CTNUAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractTypeController", "SaveContractType", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ActionViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractTypeController", "SaveContractType", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, " ", "Exception occured.", "", " ", " "));

            }
        }


        [HttpPost]
        [Route("api/contracttypes/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteContractType(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _contractTypeBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ContractType", "DeleteContractType", "Deleted ContractType Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("CTDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null,message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CTDNSUP");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CTDNSUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ContractTypeController", "DeleteContractType", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,"" , "Exception occured.", "", " ", " "));

            }
        }

    }
}
