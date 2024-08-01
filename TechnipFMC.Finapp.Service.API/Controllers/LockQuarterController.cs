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
    /// LockQuarter Controller
    /// </summary>
    //[Authorize]  // remove this comment
    public class LockQuarterController : ApiController
    {
        private readonly ILockQuarterBL _lockQuarterBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lockQuarterBL"></param>
        public LockQuarterController(ILockQuarterBL lockQuarterBL)
        {
            _lockQuarterBL = lockQuarterBL;

        }

        /// <summary>
        /// Get All LockQuarter 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/lockquarters/getall")]
        public HttpResponseMessage GetAllLockQuarter()
        {
            try
            {
                List<LockQuarter> lockQuarter = _lockQuarterBL.GetAll().ToList();
                List<LockQuarterViewModel> lockQuarterModel = new List<LockQuarterViewModel>();
                Mapper.Map(lockQuarter, lockQuarterModel);

                return Request.CreateResponse<APIResponse<List<LockQuarterViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<LockQuarterViewModel>>(HttpStatusCode.OK, lockQuarterModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockQuarterController", "GetAlllockQuarter", "");

                return Request.CreateResponse<APIResponse<List<LockQuarterViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<LockQuarterViewModel>>(HttpStatusCode.InternalServerError, null,"Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/lockquarters/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<LockQuarter> lockQuarter = _lockQuarterBL.GetAll().ToList();
                List<LockQuarterViewModel> lockQuarterModel = new List<LockQuarterViewModel>();
                var entities = lockQuarter.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, lockQuarterModel);

                return Request.CreateResponse<APIResponse<List<LockQuarterViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<LockQuarterViewModel>>(HttpStatusCode.OK, lockQuarterModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockQuarterController", "GetAlllockQuarter", "");

                return Request.CreateResponse<APIResponse<List<LockQuarterViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<LockQuarterViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/lockquarters/getbyid/{Id}")]
        public HttpResponseMessage GetLockQuarterById(int Id)
        {
            try
            {
                LockQuarter lockquarter = _lockQuarterBL.GetById(Id);
                LockQuarterViewModel lockquarterModel = new LockQuarterViewModel();
                Mapper.Map(lockquarter, lockquarterModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<LockQuarterViewModel>(HttpStatusCode.OK, lockquarterModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockQuarterController", "GetLockQuarterById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<LockQuarterViewModel>(HttpStatusCode.InternalServerError, null,"Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/lockquarters/save")]
        public HttpResponseMessage SaveLockQuarter(LockQuarterViewModel lockQuarterViewModel)
        {
            try
            {

                LockQuarter lockQuarterDatamodel = new LockQuarter();
                //lockQuarterViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(lockQuarterViewModel, lockQuarterDatamodel);
                var lockQuarter = _lockQuarterBL.Save(lockQuarterDatamodel);
                var message = "";
                var quarterToMonthMap = new Dictionary<string, string>
{
    { "Q1", "Jan" }, { "Q2", "Feb" }, { "Q3", "Mar" }, { "Q4", "Apr" },
    { "Q5", "May" }, { "Q6", "Jun" }, { "Q7", "Jul" }, { "Q8", "Aug" },
    { "Q9", "Sep" }, { "Q10", "Oct" }, { "Q11", "Nov" }, { "Q12", "Dec" }
};
                if (lockQuarter != null && lockQuarter.Id > 0)
                {
                    lockQuarterViewModel.Id = lockQuarter.Id;
                    lockQuarterViewModel.Quarter = lockQuarter.DataEntryInterval == "Monthly" ? quarterToMonthMap[lockQuarter.Quarter] : lockQuarter.Quarter; ;
                    if (lockQuarterDatamodel.Lock == true)
                    {
                        lockQuarterViewModel.Status = "Yes";
                    }
                    else
                    {
                        lockQuarterViewModel.Status = "No";
                    }
                    message = new CommonBL().GetMessage("QSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<LockQuarterViewModel>(HttpStatusCode.OK, lockQuarterViewModel, null, message, "", ""));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, lockQuarter.Message, "", ""));

                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("QAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("QNUAUS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }

            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockQuarterController", "SaveLockQuarter", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "","Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/lockquarters/delete/{Year}/{DeletedBy}")]
        public HttpResponseMessage DeleteLockQuarter(int Year, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _lockQuarterBL.Delete(Year, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    message = new CommonBL().GetMessage("QDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("QDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }
            }
            
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.LockQuarterController", "DeleteLockQuarter", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<LockQuarterViewModel>(HttpStatusCode.InternalServerError, null,"Exception occured.", "", " ", " "));

            }
        }

    }
}
