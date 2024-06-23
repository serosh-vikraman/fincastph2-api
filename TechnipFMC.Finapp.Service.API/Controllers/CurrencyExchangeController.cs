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
using TechnipFMC.Finapp.Business.Interfaces;

namespace TechnipFMC.Finapp.Service.API.Controllers
{
    public class CurrencyExchangeController : ApiController
    {
        private readonly ICurrencyExchangeBL _currencyExchangeBL;
        public CurrencyExchangeController(CurrencyExchangeBL currencyExchangeBL)
        {
            _currencyExchangeBL = currencyExchangeBL;
        }

        /// <summary>
        /// Get All Country 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/currencyexchanges/getall")]
        public HttpResponseMessage GetAllCurrencyExchanges()
        {
            try
            {
                List<CurrencyExchange> currencyExchanges = _currencyExchangeBL.GetAll().ToList();
                List<CurrencyExchangeViewModel> currencyExchangeModels = new List<CurrencyExchangeViewModel>();
                Mapper.Map(currencyExchanges, currencyExchangeModels);

                return Request.CreateResponse<APIResponse<List<CurrencyExchangeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<CurrencyExchangeViewModel>>(HttpStatusCode.OK, currencyExchangeModels, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyExchangeController", "GetAllCurrencyExchanges", "");

                return Request.CreateResponse<APIResponse<List<CurrencyExchangeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CurrencyExchangeViewModel>>(HttpStatusCode.InternalServerError, null,"Exception occured.", "", " ", " "));
            }
        }
        [HttpPost]
        [Route("api/currencyexchanges/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<CurrencyExchange> currencyExchanges = _currencyExchangeBL.GetAll().ToList();
                List<CurrencyExchangeViewModel> currencyExchangeModels = new List<CurrencyExchangeViewModel>();
                var entities = currencyExchanges.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, currencyExchangeModels);

                return Request.CreateResponse<APIResponse<List<CurrencyExchangeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<CurrencyExchangeViewModel>>(HttpStatusCode.OK, currencyExchangeModels, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyExchangeController", "GetAllCurrencyExchanges", "");

                return Request.CreateResponse<APIResponse<List<CurrencyExchangeViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CurrencyExchangeViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));
            }
        }

        [HttpPost]
        [Route("api/currencyexchanges/getbyid/{Id}")]
        public HttpResponseMessage GetCurrencyExchangeById(int Id)
        {
            try
            {
                CurrencyExchange currencyExchange = _currencyExchangeBL.GetById(Id);
                CurrencyExchangeViewModel currencyExchangeViewModel = new CurrencyExchangeViewModel();
                Mapper.Map(currencyExchange, currencyExchangeViewModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<CurrencyExchangeViewModel>(HttpStatusCode.OK, currencyExchangeViewModel, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyExchangeController", "GetCurrencyExchangeById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CurrencyExchangeViewModel>(HttpStatusCode.InternalServerError, null,"Exception occured.", "", " ", " "));
            }
        }

        [HttpPost]
        [Route("api/currencyexchanges/save")]
        public HttpResponseMessage SaveCurrencyExchange(CurrencyExchangeViewModel currencyExchangeViewModel)
        {
            try
            {

                CurrencyExchange currencyExchangeDataModel = new CurrencyExchange();
                //currencyExchangeViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(currencyExchangeViewModel, currencyExchangeDataModel);
                var currencyExchange = _currencyExchangeBL.Save(currencyExchangeDataModel);
                var message = "";
                if (currencyExchange != null && currencyExchange.Id > 0)
                {
                    currencyExchangeViewModel.Id = currencyExchange.Id;
                    currencyExchangeViewModel.SourceCurrencyCode = currencyExchange.SourceCurrencyCode;
                    currencyExchangeViewModel.TargetCurrencyCode = currencyExchange.TargetCurrencyCode;
                    if (currencyExchangeDataModel.LockStatus == true)
                    {
                        currencyExchangeViewModel.Status = "Locked";
                    }
                    else
                    {
                        currencyExchangeViewModel.Status = "Not Locked";
                    }
                    if (currencyExchangeDataModel.CancelStatus == true)
                    {
                        currencyExchangeViewModel.CancelActiveStatus = "Cancelled";
                    }
                    else
                    {
                        currencyExchangeViewModel.CancelActiveStatus = "Active";
                    }
                    message = new CommonBL().GetMessage("CESS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<CurrencyExchangeViewModel>(HttpStatusCode.OK, currencyExchangeViewModel, null, message, "", ""));
                }
                else
                {
                    //message = new CommonBL().GetMessage("CENS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, currencyExchange.Message, "", ""));
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("CEAE");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyExchangeController", "SaveCurrencyExchange", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "","Exception occured.", "", " ", " "));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyExchangeController", "SaveCurrencyExchange", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,"","Exception occured.", "", " ", " "));
            }
        }

        [HttpPost]
        [Route("api/currencyexchanges/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteCurrencyExchange(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _currencyExchangeBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    message = new CommonBL().GetMessage("CEDS");
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.CurrencyExchangeController", "CurrencyExchange", "Deleted CurrencyExchange Id=" + Id + " by " + DeletedBy);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CEND");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }

            }
            
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyExchangeController", "DeleteCurrencyExchange", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CountryViewModel>(HttpStatusCode.InternalServerError, null,"Exception occured.", "", " ", " "));

            }
        }

    }
}