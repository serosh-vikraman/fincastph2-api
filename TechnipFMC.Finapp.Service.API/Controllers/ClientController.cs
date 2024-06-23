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
    public class ClientController : ApiController
    {
        private readonly IClientBL _clientBL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ClientsBL"></param>
        /// 

        public ClientController(IClientBL clientBL)
        {
            _clientBL = clientBL;

        }
        /// <summary>
        /// Get All Action 
        /// </summary> 
        /// <returns></returns>

        [HttpPost]
        [Route("api/clients/getall")]
        public HttpResponseMessage GetAllClients()
        {
            try
            {
                List<Client> clients = _clientBL.GetAll().ToList();
                List<ClientViewModel> clientsModel = new List<ClientViewModel>();
                Mapper.Map(clients, clientsModel);

                return Request.CreateResponse<APIResponse<List<ClientViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ClientViewModel>>(HttpStatusCode.OK, clientsModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClientController", "GetAllClients", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/clients/getallclientsofuser/{userId}")]
        public HttpResponseMessage GetAllClientsofUser(int userId)
        {
            try
            {
                List<Client> clients = _clientBL.GetAllClientsofUser(userId).ToList();
                List<ClientViewModel> clientsModel = new List<ClientViewModel>();
                Mapper.Map(clients, clientsModel);

                return Request.CreateResponse<APIResponse<List<ClientViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ClientViewModel>>(HttpStatusCode.OK, clientsModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClientController", "GetAllClients", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }
        [HttpPost]
        [Route("api/clients/getall/search/{departmentId}")]
        public HttpResponseMessage Search()
        {
            try
            {
                List<Client> clients = _clientBL.GetAll().ToList();
                List<ClientViewModel> clientsModel = new List<ClientViewModel>();
                var entities = clients.Where(a => a.Active == true).ToList();
                Mapper.Map(entities, clientsModel);

                return Request.CreateResponse<APIResponse<List<ClientViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ClientViewModel>>(HttpStatusCode.OK, clientsModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClientsController", "GetAllClients", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

        [HttpPost]
        [Route("api/clients/getbyid/{Id}")]
        public HttpResponseMessage GetClientById(int Id)
        {
            try
            {
                Client clients = _clientBL.GetById(Id);
                ClientViewModel clientsModel = new ClientViewModel();
                Mapper.Map(clients, clientsModel);

                return Request.CreateResponse(HttpStatusCode.OK,
                    new APIResponse<ClientViewModel>(HttpStatusCode.OK, clientsModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClientController", "GetClientById", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }
        [HttpPost]
        [Route("api/clients/save")]
        public HttpResponseMessage SaveClient(ClientViewModel clientView)
        {
            try
            {
                Client clientdatamodel = new Client();
                //clientView.CreatedBy = User.Identity.Name.GetUserName();
                Mapper.Map(clientView, clientdatamodel);
                var client = _clientBL.Save(clientdatamodel);
                var message = "";
                //var Clientdatamodel = _ClientBL.Save(Client);
                //ClientViewModel ClientViewModel = new ClientViewModel();
                //Mapper.Map(Clientdatamodel, ClientViewModel);
                if (client != null && client.ClientID > 0)
                {
                    clientView.ClientID = client.ClientID;
                    if (clientdatamodel.Active == true)
                    {
                        clientView.Status = "Active";
                    }
                    else
                    {
                        clientView.Status = "Inactive";
                    }
                    message = new CommonBL().GetMessage("CLSS");// Client saved succesfully
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<ClientViewModel>(HttpStatusCode.OK, clientView, null, message, "", ""));

                }
                else if (client.ClientID == -1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, "Maximum limit for clients exceeded", "", ""));
                }
                else
                {
                    message = new CommonBL().GetMessage("CLNS");//Client not saved
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new APIResponse<string>(HttpStatusCode.BadRequest, "Failure", null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    var message = new CommonBL().GetMessage("CLCNAU");//Client Code or Name already used
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                            new APIResponse<ActionViewModel>(HttpStatusCode.BadRequest, null, null, message, "", ""));

                }
                else
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CLUIP");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                             new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClientController", "SaveClient", "");

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));
                return response;
            }
        }

        [HttpPost]
        [Route("api/clients/delete/{Id}/{DeletedBy}")]
        public HttpResponseMessage DeleteClients(int Id, int DeletedBy)
        {
            try
            {
                //DeletedBy = User.Identity.Name.GetUserName();
                bool deletedSuccess = _clientBL.Delete(Id, DeletedBy);
                var message = "";
                if (deletedSuccess == true)
                {
                    message = new CommonBL().GetMessage("CLDS"); //Client deleted Succesfully
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.Clients", "DeleteClients", "Deleted Clients Id=" + Id + " by " + DeletedBy);
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new APIResponse<bool>(HttpStatusCode.OK, true, null, message, "", ""));

                }
                else
                {
                    message = new CommonBL().GetMessage("CLDNS"); //Client could not be deleted
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                    new APIResponse<bool>(HttpStatusCode.NotFound, false, null, message, "", ""));

                }


            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    var message = new CommonBL().GetMessage("CLDNS");//Client has Projects.
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                                 new APIResponse<string>(HttpStatusCode.BadRequest, "", null, message, "", ""));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ClientController", "DeleteClients", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<string>(HttpStatusCode.InternalServerError, "", "Exception occured.", "", " ", " "));

            }
        }

    }
}
