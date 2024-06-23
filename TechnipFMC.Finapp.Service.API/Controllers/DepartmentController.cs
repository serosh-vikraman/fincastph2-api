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
    public class DepartmentController : ApiController
    {
        private readonly IDepartmentBL _departmentBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DepartmentBL"></param>
        public DepartmentController(IDepartmentBL departmentBL)
        {
            _departmentBL = departmentBL;

        }

        /// <summary>
        /// Get All Department 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/departments/getall")]
        public HttpResponseMessage GetAllDepartments()
        {
            try
            {
                List<Department> departments = _departmentBL.GetAll().ToList();
                List<DepartmentViewModel> departmentsModel = new List<DepartmentViewModel>();
                Mapper.Map(departments, departmentsModel);

                return Request.CreateResponse<APIResponse<List<DepartmentViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<DepartmentViewModel>>(HttpStatusCode.OK, departmentsModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.DepartmentController", "GetAllDepartments", "");

                return Request.CreateResponse<APIResponse<List<DepartmentViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<DepartmentViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/departments/getall/search")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<Department> departments = _departmentBL.GetAll().ToList();
                List<DepartmentViewModel> departmentModel = new List<DepartmentViewModel>();
                var entities = departments.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, departmentModel);

                return Request.CreateResponse<APIResponse<List<DepartmentViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<DepartmentViewModel>>(HttpStatusCode.OK, departmentModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.DepartmentController", "GetAllDepartments", "");

                return Request.CreateResponse<APIResponse<List<DepartmentViewModel>>>(HttpStatusCode.InternalServerError,
                    new APIResponse<List<DepartmentViewModel>>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/departments/getbyid/{Id}")]
        public HttpResponseMessage GetDepartmentById(int Id)
        {
            try
            {
                Department departments = _departmentBL.GetById(Id);
                DepartmentViewModel departmentsModel = new DepartmentViewModel();
                Mapper.Map(departments, departmentsModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<DepartmentViewModel>(HttpStatusCode.OK, departmentsModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.DepartmentController", "GetDepartmentById", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<DepartmentViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/departments/save")]
        public HttpResponseMessage SaveDepartment(DepartmentViewModel departmentViewModel)
        {
            try
            {

                Department departmentDatamodel = new Department();
                //departmentViewModel.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(departmentViewModel, departmentDatamodel);
                var department = _departmentBL.Save(departmentDatamodel);
                var message = "";
                if (department != null && department.DepartmentID > 0)
                {
                    departmentViewModel.DepartmentID = department.DepartmentID;
                    if (departmentDatamodel.Active == true)
                    {
                        departmentViewModel.Status = "Active";
                    }
                    else
                    {
                        departmentViewModel.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("DEPSS");//Department saved Succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<DepartmentViewModel>(HttpStatusCode.OK, departmentViewModel, null, message, "", ""));

                }
                else if (department.DepartmentID == -1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, "Maximum limit for departments exceeded", "", ""));
                }
                else
                {
                    message = new CommonBL().GetMessage("DNS");//Department could not be saved
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("DCNAU"); //Department code or name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("DCNUP");//Department code already used in Customer
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.DepartmentController", "SaveDepartment", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.DepartmentController", "SaveDepartment", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/departments/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteDepartment(int Id, int DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deleteSuccess = _departmentBL.Delete(Id, DeletedBy);
                var message = "";
                if (deleteSuccess == true)
                {
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.DepartmentController", "DeleteDepartment", "Deleted Department Id=" + Id + " by " + DeletedBy);
                    message = new CommonBL().GetMessage("DDS"); //Department deleted Succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("DND");//Department could not be deleted
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("DND"); // Department could not be deleted
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.DepartmentController", "DeleteDepartment", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.DepartmentController", "DeleteDepartment", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<DepartmentViewModel>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

    }
}
