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
    /// StatutoryCategory Controller
    /// </summary>

    //[Authorize] //remove comment
    public class StatutoryCategoryController : ApiController
    {
        private readonly IStatutoryCategoryBL _statutoryCategoryBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StatutoryCategoryBL"></param>
        /// 

        public StatutoryCategoryController(IStatutoryCategoryBL statutoryCategoryBL)
        {
            _statutoryCategoryBL = statutoryCategoryBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("api/statutorycategories/getall")]
        public HttpResponseMessage GetAllStatutoryCategorys()
        {
            try
            {
                List<StatutoryCategory> source = _statutoryCategoryBL.GetAll().ToList();
                List<StatutoryCategoryViewModel> destination = new List<StatutoryCategoryViewModel>();
                Mapper.Map(source, destination);

                return Request.CreateResponse<APIResponse<List<StatutoryCategoryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<StatutoryCategoryViewModel>>(HttpStatusCode.OK, destination, null, "", "", ""));

               
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.StatutoryCategoryController", "GetAllStatutoryCategorys", "");

                return Request.CreateResponse<APIResponse<List<StatutoryCategoryViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<StatutoryCategoryViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
               
            }
        }
        [HttpPost]
        [Route("api/statutorycategories/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<StatutoryCategory> source = _statutoryCategoryBL.GetAll().ToList();
                List<StatutoryCategoryViewModel> destination = new List<StatutoryCategoryViewModel>();
                var entities = source.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, destination);

                return Request.CreateResponse<APIResponse<List<StatutoryCategoryViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<StatutoryCategoryViewModel>>(HttpStatusCode.OK, destination, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.StatutoryCategoryController", "GetAllStatutoryCategorys", "");

                return Request.CreateResponse<APIResponse<List<StatutoryCategoryViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<StatutoryCategoryViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/statutorycategories/getbyid/{Id}")]
        public HttpResponseMessage GetStatutoryCategoryById(int Id)
        {
            try
            {
                StatutoryCategory source = _statutoryCategoryBL.GetById(Id);
                StatutoryCategoryViewModel destination = new StatutoryCategoryViewModel();
                Mapper.Map(source, destination);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<StatutoryCategoryViewModel>(HttpStatusCode.OK, destination, null, "", "", ""));

                
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.StatutoryCategoryController", "GetStatutoryCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<StatutoryCategoryViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));
                
            }
        }
        [HttpPost]
        [Route("api/statutorycategories/save")]
        public HttpResponseMessage SaveStatutoryCategory(StatutoryCategoryViewModel statutoryCategoryViewModel)
        {
            try
            {
                StatutoryCategory statutoryCategoryDatamodel = new StatutoryCategory();
                //statutoryCategoryViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(statutoryCategoryViewModel, statutoryCategoryDatamodel);
                var statutoryCategory = _statutoryCategoryBL.Save(statutoryCategoryDatamodel);
                var message = "";
                if (statutoryCategory != null && statutoryCategory.StatutoryCategoryID > 0)
                {
                    statutoryCategoryViewModel.StatutoryCategoryID = statutoryCategory.StatutoryCategoryID;
                    if (statutoryCategoryDatamodel.Active == true)
                    {
                        statutoryCategoryViewModel.Status = "Active";
                    }
                    else
                    {
                        statutoryCategoryViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("STCSS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<StatutoryCategoryViewModel>(HttpStatusCode.OK, statutoryCategoryViewModel, null , message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("STCNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }

                
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("STCNAU");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));
                }
                else
                    if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("STCNUAUP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));

                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.StatutoryCategoryController", "SaveStatutoryCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ActionViewModel>(HttpStatusCode.InternalServerError,null,"Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.StatutoryCategoryController", "SaveStatutoryCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,null,"Exception occured.", "", "", ""));
                
            }
        }

        [HttpPost]
        [Route("api/statutorycategories/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteStatutoryCategory(int Id, string DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _statutoryCategoryBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.StatutoryCategoryController", "DeleteStatutoryCategory", "Deleted StatutoryCategory Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("STCDS");
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("STCDNS");
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));
                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("STCDNS");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.StatutoryCategoryController", "DeleteStatutoryCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.StatutoryCategoryController", "DeleteStatutoryCategory", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError,  null, "Exception occured.", "", "", ""));
                
            }
        }


    }
}
