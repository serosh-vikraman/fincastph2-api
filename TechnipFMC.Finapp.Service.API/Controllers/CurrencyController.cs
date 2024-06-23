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
    public class CurrencyController : ApiController
    {
        private readonly ICurrencyBL _currencyBL;
        public CurrencyController(CurrencyBL currencyBL)
        {
            _currencyBL = currencyBL;
        }

        /// <summary>
        /// Get All Country 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/currencies/getall")]
        public HttpResponseMessage GetAllCurrencies()
        {
            try
            {
                List<Currency> currencies = _currencyBL.GetAll().ToList();
                List<CurrencyViewModel> currencyModels = new List<CurrencyViewModel>();
                Mapper.Map(currencies, currencyModels);

                return Request.CreateResponse<APIResponse<List<CurrencyViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<CurrencyViewModel>>(HttpStatusCode.OK, currencyModels, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyController", "GetAllCurrencies", "");

                return Request.CreateResponse<APIResponse<List<CurrencyViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CurrencyViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/currencies/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<Currency> currencies = _currencyBL.GetAll().ToList();
                List<CurrencyViewModel> currencyModels = new List<CurrencyViewModel>();
                var entities = currencies.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, currencyModels);

                return Request.CreateResponse<APIResponse<List<CurrencyViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<CurrencyViewModel>>(HttpStatusCode.OK, currencyModels, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyController", "GetAllCurrencies", "");

                return Request.CreateResponse<APIResponse<List<CurrencyViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CurrencyViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/currencies/getbyid/{Id}")]
        public HttpResponseMessage GetCurrencyById(int Id )
        {
            try
            {
                Currency currency = _currencyBL.GetById(Id);
                CurrencyViewModel currencyModel = new CurrencyViewModel();
                Mapper.Map(currency, currencyModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<CurrencyViewModel>(HttpStatusCode.OK, currencyModel, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyController", "GetCurrencyById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CurrencyViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/currencies/save")]
        public HttpResponseMessage SaveCurrency(CurrencyViewModel currencyViewModel)
        {
            try
            {

                Currency currencyDataModel = new Currency();
                //currencyViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(currencyViewModel, currencyDataModel);
                var currency = _currencyBL.Save(currencyDataModel);
                var message = "";
                if (currency != null && currency.CurrencyID > 0)
                {
                    currencyViewModel.CurrencyID = currency.CurrencyID;
                    if (currencyDataModel.Active == true)
                    {
                        currencyViewModel.Status = "Active";
                    }
                    else
                    {
                        currencyViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("CUSSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<CurrencyViewModel>(HttpStatusCode.OK, currencyViewModel, null, message, "", ""));
                }
                else
                {
                    message = new CommonBL().GetMessage("CUSNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", "", message, "", ""));
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("CUCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CUCAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyController", "SaveCurrency", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyController", "SaveCurrency", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/currencies/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteCurrency(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _currencyBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.CurrencyController", "DeleteCurrency", "Deleted Currency Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("CUDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CUDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {

                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CUDNSUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CountryController", "DeleteCurrency", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CurrencyController", "DeleteCurrency", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CountryViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

    }
}