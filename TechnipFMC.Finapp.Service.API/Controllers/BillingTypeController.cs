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
    /// BillingType Controller
    /// </summary>

    //[Authorize] //remove comment
    public class BillingTypeController : ApiController
    {
        private readonly IBillingTypeBL _billingTypeBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="BillingTypesBL"></param>
        /// 

        public BillingTypeController(IBillingTypeBL billingTypeBL)
        {
            _billingTypeBL = billingTypeBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/billingTypes/getall")]
        public HttpResponseMessage GetAllBillingTypes()
        {
            try
            {
                List<BillingType> billingTypes = _billingTypeBL.GetAll().ToList();
                List<BillingTypeViewModel> billingTypesModel = new List<BillingTypeViewModel>();
                Mapper.Map(billingTypes, billingTypesModel);

                return Request.CreateResponse<APIResponse<List<BillingTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<BillingTypeViewModel>>(HttpStatusCode.OK, billingTypesModel, null, "", "", ""));

               
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BillingTypesController", "GetAllBillingTypes", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/billingTypes/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<BillingType> billingTypes = _billingTypeBL.GetAll().ToList();
                List<BillingTypeViewModel> billingTypesModel = new List<BillingTypeViewModel>();
                var entities = billingTypes.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, billingTypesModel);

                return Request.CreateResponse<APIResponse<List<BillingTypeViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<BillingTypeViewModel>>(HttpStatusCode.OK, billingTypesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BillingTypesController", "GetAllBillingTypes", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/billingTypes/getbyid/{Id}")]
        public HttpResponseMessage GetBillingTypeById(int Id)
        {
            try
            {
                BillingType billingTypes = _billingTypeBL.GetById(Id);
                BillingTypeViewModel billingTypesModel = new BillingTypeViewModel();
                Mapper.Map(billingTypes, billingTypesModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<BillingTypeViewModel>(HttpStatusCode.OK, billingTypesModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BillingTypeController", "GetBillingTypeById", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }
        [HttpPost]
        [Route("api/billingTypes/save")]
        public HttpResponseMessage SaveBillingType(BillingTypeViewModel billingTypeView)
        {
            try
            {
                BillingType billingtypedatamodel = new BillingType();
                //billingTypeView.CreatedBy = User.Identity.Name.GetUserName() ;
                Mapper.Map(billingTypeView, billingtypedatamodel);
                var billingType = _billingTypeBL.Save(billingtypedatamodel);
                var message = "";
                //var billingtypedatamodel = _billingTypeBL.Save(billingType);
                //BillingTypeViewModel billingTypeViewModel = new BillingTypeViewModel();
                //Mapper.Map(billingtypedatamodel, billingTypeViewModel);
                if (billingType != null && billingType.BillingTypeID > 0)
                {
                    billingTypeView.BillingTypeID = billingType.BillingTypeID;
                    if (billingtypedatamodel.Active == true)
                    {
                        billingTypeView.Status = "Active";
                    }
                    else
                    {
                        billingTypeView.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("BTSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<BillingTypeViewModel>(HttpStatusCode.OK, billingTypeView,null , message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("BTNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure",null,message, "", ""));

                }

                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("BTCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));

                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("BTUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                                                 new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BillingTypeController", "SaveBillingType", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/billingTypes/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteBillingTypes(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deletedSuccess = _billingTypeBL.Delete(Id, DeletedBy);
                var message = "";
                if (deletedSuccess == true)
                {
                    message = new CommonBL().GetMessage("BTDS");
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.BillingTypes", "DeleteBillingTypes", "Deleted BillingTypes Id=" + Id + " by " + DeletedBy);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("BTDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("BTDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                                 new APIResponse<string>(HttpStatusCode.BadRequest, "", null,message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,"", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BillingTypeController", "DeleteBillingTypes", "");

                return  Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

    }
}
