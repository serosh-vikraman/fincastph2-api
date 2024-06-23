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
    /// BUCategory Controller
    /// </summary>

    //[Authorize] //remove comment
    public class BUCategoryController : ApiController
    {
        private readonly IBUCategoryBL _buCategoryBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="BUCategoryBL"></param>
        /// 

        public BUCategoryController(IBUCategoryBL buCategoryBL)
        {
            _buCategoryBL = buCategoryBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/bucategories/getall")]
        public HttpResponseMessage GetAllBUCategories()
        {
            try
            {
                List<BUCategory> buCategories = _buCategoryBL.GetAll().ToList();
                List<BUCategoryViewModel> buCategoriesModel = new List<BUCategoryViewModel>();
                Mapper.Map(buCategories, buCategoriesModel);

                return Request.CreateResponse<APIResponse<List<BUCategoryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<BUCategoryViewModel>>(HttpStatusCode.OK, buCategoriesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BUCategoryController", "GetAllBUCategories", "");

                HttpResponseMessage response = Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }
        [HttpPost]
        [Route("api/bucategories/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<BUCategory> buCategories = _buCategoryBL.GetAll().ToList();
                List<BUCategoryViewModel> buCategoriesModel = new List<BUCategoryViewModel>();
                var entities = buCategories.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, buCategoriesModel);

                return Request.CreateResponse<APIResponse<List<BUCategoryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<BUCategoryViewModel>>(HttpStatusCode.OK, buCategoriesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BUCategoryController", "GetAllBUCategories", "");

                HttpResponseMessage response = Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }
        [HttpPost]
        [Route("api/bucategories/getbyid/{Id}")]
        public HttpResponseMessage GetBUCategoryById(int Id)
        {
            try
            {
                BUCategory buCategories = _buCategoryBL.GetById(Id);
                BUCategoryViewModel buCategoriesModel = new BUCategoryViewModel();
                Mapper.Map(buCategories, buCategoriesModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<BUCategoryViewModel>(HttpStatusCode.OK, buCategoriesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BUCategoryController", "GetBUCategoryById", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<BUCategoryViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));
                return response;
            }
        }
        [HttpPost]
        [Route("api/bucategories/save")]
        public HttpResponseMessage SaveBUCategory(BUCategoryViewModel buCategoryViewModel)
        {
            try
            {
                BUCategory buCategoryDatamodel = new BUCategory();
                //buCategoryViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(buCategoryViewModel, buCategoryDatamodel);
                var buCategory = _buCategoryBL.Save(buCategoryDatamodel);
                var message = "";

                if (buCategory != null && buCategory.BUCategoryID > 0)
                {
                    buCategoryViewModel.BUCategoryID = buCategory.BUCategoryID;
                    if (buCategoryDatamodel.Active == true)
                    {
                        buCategoryViewModel.Status = "Active";
                    }
                    else
                    {
                        buCategoryViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("BUCSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<BUCategoryViewModel>(HttpStatusCode.OK, buCategoryViewModel, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("BUCNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("BUCCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));

                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("BUCNU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));

                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BUCategoryController", "SaveBUCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BUCategoryController", "SaveBUCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, " ", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/bucategories/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteBUCategory(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _buCategoryBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.BUCategory", "DeleteBUCategory", "Deleted BUCategory Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("BUCDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("BUCND");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("BUCND");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));

                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, " ", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.BUCategoryController", "DeleteBUCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

    }
}
