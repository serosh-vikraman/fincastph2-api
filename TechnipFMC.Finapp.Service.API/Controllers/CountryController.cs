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
    /// <summary>
    /// Country Controller
    /// </summary>
    //[Authorize]  // remove this comment
    public class CountryController : ApiController
    {
        private readonly ICountryBL _countryBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="countryBL"></param>
        public CountryController(ICountryBL countryBL)
        {
            _countryBL = countryBL;

        }

        /// <summary>
        /// Get All Country 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/countries/getall")]
        public HttpResponseMessage GetAllCountries()
        {
            try
            {
                List<Country> countries = _countryBL.GetAll().ToList();
                List<CountryViewModel> countriesModel = new List<CountryViewModel>();
                Mapper.Map(countries, countriesModel);

                return Request.CreateResponse<APIResponse<List<CountryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<CountryViewModel>>(HttpStatusCode.OK, countriesModel, null, "", "", ""));

               
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CountryController", "GetAllCountries", "");

                return Request.CreateResponse<APIResponse<List<CountryViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CountryViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/countries/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<Country> countries = _countryBL.GetAll().ToList();
                List<CountryViewModel> countriesModel = new List<CountryViewModel>();
                var entities = countries.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, countriesModel);

                return Request.CreateResponse<APIResponse<List<CountryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<CountryViewModel>>(HttpStatusCode.OK, countriesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CountryController", "GetAllCountries", "");

                return Request.CreateResponse<APIResponse<List<CountryViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<CountryViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/countries/getbyid/{Id}")]
        public HttpResponseMessage GetCountryById(int Id)
        {
            try
            {
                Country countries = _countryBL.GetById(Id);
                CountryViewModel countriesModel = new CountryViewModel();
                Mapper.Map(countries, countriesModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<CountryViewModel>(HttpStatusCode.OK, countriesModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CountryController", "GetCountryById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CountryViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/countries/save")]
        public HttpResponseMessage SaveCountry(CountryViewModel countryViewModel)
        {
            try
            {

                Country countryDatamodel = new Country();
                //countryViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(countryViewModel, countryDatamodel);
                var country = _countryBL.Save(countryDatamodel);
                var message = "";
                if (country != null && country.CountryID > 0)
                {
                    countryViewModel.CountryID = country.CountryID;
                    if (countryDatamodel.Active == true)
                    {
                        countryViewModel.Status = "Active";
                    }
                    else
                    {
                        countryViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("CSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<CountryViewModel>(HttpStatusCode.OK, countryViewModel,null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null,message, "", ""));

                }

                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("CCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CCNUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CountryController", "SaveCountry", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "","Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CountryController", "SaveCountry", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,"", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/countries/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteCountry(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _countryBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.CountryController", "DeleteCountry", "Deleted Country Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("CDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CDNS");
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
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CountryController", "DeleteCountry", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.CountryController", "DeleteCountry", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<CountryViewModel>(HttpStatusCode.InternalServerError, "","Exception occured.", "", " ", " "));
                
            }
        }

    }

}
