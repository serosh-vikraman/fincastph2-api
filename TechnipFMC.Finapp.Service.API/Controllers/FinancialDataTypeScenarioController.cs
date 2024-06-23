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
    public class FinancialDataTypeScenarioController : ApiController
    {
        private readonly IFinancialDataTypeScenarioBL _financialDataTypeScenarioBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FinancialDataTypeBL"></param>
        public FinancialDataTypeScenarioController(IFinancialDataTypeScenarioBL financialDataTypeScenarioBL)
        {
            _financialDataTypeScenarioBL = financialDataTypeScenarioBL;

        }

        /// <summary>
        /// Get All FinancialDataTypeScenario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financialdatatypescenario/getallscopetypes/{FinancialDataTypeCode}")]
        public HttpResponseMessage GetAllScopeTypes(string financialDataTypeCode)
        {
            try
            {
                List<ScenarioScopeTypes> financialDataTypes = _financialDataTypeScenarioBL.GetAllScopeTypes(financialDataTypeCode).ToList();
                List<ScenarioScopeTypesViewModel> financialDataTypesViewModel = new List<ScenarioScopeTypesViewModel>();
                Mapper.Map(financialDataTypes, financialDataTypesViewModel);

                return Request.CreateResponse<APIResponse<List<ScenarioScopeTypesViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ScenarioScopeTypesViewModel>>(HttpStatusCode.OK, financialDataTypesViewModel, null, "", "", ""));                

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "GetAllFinancialDataTypes", "");

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/financialdatatypescenario/getallfinancialdatatypes/{scope}/{type}")]
        public HttpResponseMessage GetAllFinancialDataTypes(string scope,string type)
        {
            try
            {
                List<string> financialDataTypes = _financialDataTypeScenarioBL.GetAllFinancialDataTypes(scope,type).ToList();
                //List<FinancialDataTypesView> financialDataTypesViewModel = new List<FinancialDataTypesView>();
                //Mapper.Map(financialDataTypes, financialDataTypesViewModel);

                return Request.CreateResponse<APIResponse<List<string>>>(HttpStatusCode.OK,
                    new APIResponse<List<string>>(HttpStatusCode.OK, financialDataTypes, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.FinancialDataTypeController", "GetAllFinancialDataTypes", "");

                return Request.CreateResponse<APIResponse<List<FinancialDataTypeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<FinancialDataTypeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

    }
}
