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
    /// ManagementCategory Controller
    /// </summary>

    //[Authorize] //remove comment
    public class ManagementCategoryController : ApiController
    {
        private readonly IManagementCategoryBL _managementCategoryBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ManagementCategoryBL"></param>
        /// 

        public ManagementCategoryController(IManagementCategoryBL managementCategoryBL)
        {
            _managementCategoryBL = managementCategoryBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/managementcategories/getall")]
        public HttpResponseMessage GetAllManagementCategory()
        {
            try
            {
                List<ManagementCategory> managementCategories = _managementCategoryBL.GetAll().ToList();
                List<ManagementCategoryViewModel> managementCategoriesModel = new List<ManagementCategoryViewModel>();
                Mapper.Map(managementCategories, managementCategoriesModel);

                return Request.CreateResponse<APIResponse<List<ManagementCategoryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ManagementCategoryViewModel>>(HttpStatusCode.OK, managementCategoriesModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ManagementCategoryController", "GetAllManagementCategory", "");

                return Request.CreateResponse<APIResponse<List<ManagementCategoryViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ManagementCategoryViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/managementcategories/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<ManagementCategory> managementCategories = _managementCategoryBL.GetAll().ToList();
                List<ManagementCategoryViewModel> managementCategoriesModel = new List<ManagementCategoryViewModel>();
                var entities = managementCategories.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, managementCategoriesModel);

                return Request.CreateResponse<APIResponse<List<ManagementCategoryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ManagementCategoryViewModel>>(HttpStatusCode.OK, managementCategoriesModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ManagementCategoryController", "GetAllManagementCategory", "");

                return Request.CreateResponse<APIResponse<List<ManagementCategoryViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<ManagementCategoryViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/managementcategories/getbyid/{Id}")]
        public HttpResponseMessage GetManagementCategoryById(int Id)
        {
            try
            {
                ManagementCategory managementCategorys = _managementCategoryBL.GetById(Id );
                ManagementCategoryViewModel managementCategorysModel = new ManagementCategoryViewModel();
                Mapper.Map(managementCategorys, managementCategorysModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ManagementCategoryViewModel>(HttpStatusCode.OK, managementCategorysModel, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ManagementCategoryController", "GetManagementCategoryById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ManagementCategoryViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/managementcategories/save")]
        public HttpResponseMessage SaveManagementCategory(ManagementCategoryViewModel managementCategoryViewModel)
        {
            try
            {
                ManagementCategory managementCategoryDataModel = new ManagementCategory();
                //managementCategoryViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(managementCategoryViewModel, managementCategoryDataModel);
                var managementCategory = _managementCategoryBL.Save(managementCategoryDataModel);
                var message = "";
                if (managementCategory != null && managementCategory.ManagementCategoryID > 0)
                {
                    managementCategoryViewModel.ManagementCategoryID = managementCategory.ManagementCategoryID;
                    if (managementCategoryDataModel.Active == true)
                    {
                        managementCategoryViewModel.Status = "Active";
                    }
                    else
                    {
                        managementCategoryViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("MCSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ManagementCategoryViewModel>(HttpStatusCode.OK, managementCategoryViewModel,null , message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("MCNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("MCCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("MCNUUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ManagementCategoryController", "SaveManagementCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ManagementCategoryController", "SaveManagementCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ManagementCategoryViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/managementcategories/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteManagementCategory(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _managementCategoryBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ManagementCategory", "DeleteManagementCategory", "Deleted ManagementCategory Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("MCDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("MCNDUP");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("MCNDUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ManagementCategoryController", "DeleteManagementCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,"", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ManagementCategoryController", "DeleteManagementCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

    }
}
