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
    /// LockYear Controller
    /// </summary>
    //[Authorize]  // remove this comment
    public class LockYearController : ApiController
    {
        private readonly ILockYearBL _lockYearBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lockYearBL"></param>
        public LockYearController(ILockYearBL lockYearBL)
        {
            _lockYearBL = lockYearBL;

        }

        /// <summary>
        /// Get All LockYear 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/lockYears/getall")]
        public HttpResponseMessage GetAllLockYear()
        {
            try
            {
                List<LockYear> lockYear = _lockYearBL.GetAll().ToList();
                List<LockYearViewModel> lockYearModel = new List<LockYearViewModel>();
                Mapper.Map(lockYear, lockYearModel);

                return Request.CreateResponse<APIResponse<List<LockYearViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<LockYearViewModel>>(HttpStatusCode.OK, lockYearModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockYearController", "GetAlllockYear", "");

                return Request.CreateResponse<APIResponse<List<LockYearViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<LockYearViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/lockYears/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<LockYear> lockYear = _lockYearBL.GetAll().ToList();
                List<LockYearViewModel> lockYearModel = new List<LockYearViewModel>();
                var entities = lockYear.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, lockYearModel);

                return Request.CreateResponse<APIResponse<List<LockYearViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<LockYearViewModel>>(HttpStatusCode.OK, lockYearModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockYearController", "GetAlllockYear", "");

                return Request.CreateResponse<APIResponse<List<LockYearViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<LockYearViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/lockYears/getbyid/{Year}")]
        public HttpResponseMessage GetLockYearById(int Year)
        {
            try
            {
                LockYear lockYear = _lockYearBL.GetById(Year);
                LockYearViewModel lockYearModel = new LockYearViewModel();
                Mapper.Map(lockYear, lockYearModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<LockYearViewModel>(HttpStatusCode.OK, lockYearModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockYearController", "GetLockYearById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<LockYearViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/lockYears/save")]
        public HttpResponseMessage SaveLockYear(LockYearViewModel lockYearViewModel)
        {
            try
            {

                LockYear lockYearDatamodel = new LockYear();
                //lockYearViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(lockYearViewModel, lockYearDatamodel);
                var lockyear = _lockYearBL.Save(lockYearDatamodel);
                var message = "";
                if (lockyear != null && lockyear.Id > 0)
                {
                    lockYearViewModel.Id = lockyear.Id;
                    if (lockYearDatamodel.Lock == true)
                    {
                        lockYearViewModel.Status = "Yes";
                    }
                    else
                    {
                        lockYearViewModel.Status = "No";
                    }
                    message = new CommonBL().GetMessage("YSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<LockYearViewModel>(HttpStatusCode.OK, lockYearViewModel, null, message, "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, lockyear.Message, "", ""));

                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("YAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("YNUAUS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }

            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockYearController", "SaveLockYear", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/lockyears/delete/{Year}/{DeletedBy}")]
        public HttpResponseMessage DeleteLockYear(int Year, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _lockYearBL.Delete(Year, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    message = new CommonBL().GetMessage("YDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("YND");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("YNDQEP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }

            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockYearController", "DeleteLockYear", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<LockYearViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

    }
}
