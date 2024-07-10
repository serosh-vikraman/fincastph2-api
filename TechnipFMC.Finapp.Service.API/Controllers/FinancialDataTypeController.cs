using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using TechnipFMC.Common;
using TechnipFMC.Finapp.Business;
using TechnipFMC.Finapp.Models;
using TechnipFMC.Finapp.Service.API.ViewModel;
using System.Data.SqlClient;

namespace TechnipFMC.Finapp.Service.API.Controllers
{
    public class FinancialDataTypeController : ApiController
    {
        private readonly IFinancialDataTypeBL _financialDataTypeBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FinancialDataTypeBL"></param>
        public FinancialDataTypeController(IFinancialDataTypeBL financialDataTypeBL)
        {
            _financialDataTypeBL = financialDataTypeBL;

        }

        /// <summary>
        /// Get All FinancialDataType 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financialdatatype/getall")]
        public HttpResponseMessage GetAllFinancialDataTypes()
        {
            try
            {
                List<FinancialDataType> financialDataTypes = _financialDataTypeBL.GetAll().ToList();
                List<FinancialDataTypeViewModel> financialDataTypesModel = new List<FinancialDataTypeViewModel>();
                Mapper.Map(financialDataTypes, financialDataTypesModel);

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.OK, financialDataTypesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "GetAllFinancialDataTypes", "");

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/financialdatatype/getallfinancialdatatypesmapped/{scope}")]
        public HttpResponseMessage GetAllFinancialDataTypes(string scope)
        {
            try
            {
                List<FinancialDataType> financialDataTypes = _financialDataTypeBL.GetAllMapped(scope).ToList();
                List<FinancialDataTypeViewModel> financialDataTypesModel = new List<FinancialDataTypeViewModel>();
                Mapper.Map(financialDataTypes, financialDataTypesModel);

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.OK, financialDataTypesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "GetAllFinancialDataTypes", "");

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/financialdatatype/getall/search/{CustomerId}")]
        public HttpResponseMessage Search(int customerId)
        {
            try
            {
                List<FinancialDataType> financialDataTypes = _financialDataTypeBL.GetAll().ToList();
                List<FinancialDataTypeViewModel> financialDataTypesModel = new List<FinancialDataTypeViewModel>();
                var entities = financialDataTypes.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, financialDataTypesModel);

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.OK, financialDataTypesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "GetAllFinancialDataTypes", "");

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/financialdatatype/getbyid/{Id}")]
        public HttpResponseMessage GetFinancialDataTypeById(int Id)
        {
            try
            {
                FinancialDataType financialDataType = _financialDataTypeBL.GetById(Id);
                FinancialDataTypeViewModel financialDataTypeModel = new FinancialDataTypeViewModel();
                Mapper.Map(financialDataType, financialDataTypeModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<FinancialDataTypeViewModel>(HttpStatusCode.OK, financialDataTypeModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "GetFinancialDataTypeById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<FinancialDataTypeViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/financialdatatype/save")]
        public HttpResponseMessage SaveFinancialDataType(FinancialDataTypeViewModel financialDataTypeViewModel)
        {
            try
            {

                FinancialDataType financialDataTypeDatamodel = new FinancialDataType();
                //FinancialDataTypeViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(financialDataTypeViewModel, financialDataTypeDatamodel);
                var financialDataType = _financialDataTypeBL.Save(financialDataTypeDatamodel);
                var message = "";
                if (financialDataType != null && financialDataType.FinancialDataTypeID > 0 && financialDataType.Message == "DS")
                {
                    financialDataTypeViewModel.FinancialDataTypeID = financialDataType.FinancialDataTypeID;
                    if (financialDataTypeDatamodel.Active == true)
                    {
                        financialDataTypeViewModel.Status = "Active";
                    }
                    else
                    {
                        financialDataTypeViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("FDTSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<FinancialDataTypeViewModel>(HttpStatusCode.OK, financialDataTypeViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage(financialDataType.Message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("FDTCNU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("FDTUS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "SaveFinancialDataType", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "SaveFinancialDataType", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/financialdatatype/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteFinancialDataType(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _financialDataTypeBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "DeleteFinancialDataType", "Deleted FinancialDataType Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("FDTDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("FDTCND");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("FDUSD");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "DeleteFinancialDataType", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "DeleteFinancialDataType", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<FinancialDataTypeViewModel>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/financialdatatype/scope-type-mapping")]
        public HttpResponseMessage MapFinancialDataType(FinancialDataTypeMappingViewModel mappings)
        {
            try
            {
                FinancialDataTypeMapping financialDataTypeMap = new FinancialDataTypeMapping();
                Mapper.Map(mappings, financialDataTypeMap);
                bool deleteSuccess = _financialDataTypeBL.Map(financialDataTypeMap);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "MapFinancialDataType", "");
                    message = "Successfully Mapped";
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = "Mapping Not Done";
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "DeleteFinancialDataType", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "DeleteFinancialDataType", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<FinancialDataTypeViewModel>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/financialdatatype/getall/{ScenarioId}")]
        public HttpResponseMessage GetAllFinancialDataTypesOfScenario(int ScenarioId)
        {
            try
            {
                List<FinancialDataType> financialDataTypes = _financialDataTypeBL.GetAllFinancialDataTypesOfScenario(ScenarioId).ToList();
                List<FinancialDataTypeViewModel> financialDataTypesModel = new List<FinancialDataTypeViewModel>();
                Mapper.Map(financialDataTypes, financialDataTypesModel);

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.OK, financialDataTypesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "GetAllFinancialDataTypes", "");

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/financialdatatype/getallfinancialdatatypesforreport/{reportid}")]
        public HttpResponseMessage GetAllFinancialDataTypesofReport(int reportid)
        {
            try
            {
                List<FinancialDataType> financialDataTypes = _financialDataTypeBL.GetAllFinancialDataTypesofReport(reportid).ToList();
                List<FinancialDataTypeViewModel> financialDataTypesModel = new List<FinancialDataTypeViewModel>();
                Mapper.Map(financialDataTypes, financialDataTypesModel);

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.OK, financialDataTypesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "GetAllFinancialDataTypes", "");

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [Route("api/financialdatatype/mapreportdatatypes")]
        public HttpResponseMessage MapReportDatatypes(MapReportDatatypesRequest mapReportDatatypesRequest)
        {
            try
            {
                bool success = _financialDataTypeBL.MapReportDatatypes(mapReportDatatypesRequest.ReportId, mapReportDatatypesRequest.SelectedIds);
                var message = "";
                if (success == true)
                {
                    RaintelsLogManager.Info("Service.API.FinancialDataTypeController", "MapReportDatatypes", "");
                    message = "Successfully Mapped";
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = "Mapping Not Done";
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "Service.API.FinancialDataTypeController", "MapReportDatatypes", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "Service.API.FinancialDataTypeController", "MapReportDatatypes", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<FinancialDataTypeViewModel>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        public class MapReportDatatypesRequest
        {
            public int ReportId { get; set; }
            public string[] SelectedIds { get; set; }
        }
    }
}
