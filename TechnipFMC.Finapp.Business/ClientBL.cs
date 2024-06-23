using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class ClientBL : IClientBL
    {
        public ClientBL(IClientRepository clientRepo)
        {
            //_ClientRepo = ClientRepo;
        }
        public ClientBL()
        { }
        public bool Delete(int Id, int Deletedby)
        {
            ClientRepository _clientRepo = new ClientRepository();
            return _clientRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<Client> GetAll()
        {
            ClientRepository _clientRepo = new ClientRepository();
            return _clientRepo.GetAll();
        }
        public IEnumerable<Client> GetAllClientsofUser(int userId)
        {
            ClientRepository _clientRepo = new ClientRepository();
            return _clientRepo.GetAllClientsofUser(userId);
        }

        public Client GetById(int id)
        {
            ClientRepository _clientRepo = new ClientRepository();
            return _clientRepo.GetById(id);
        }

        public Client Save(Client Client)
        {
            ClientRepository _clientRepo = new ClientRepository();
            return _clientRepo.Save(Client);
        }

        public Client Update(Client Client)
        {
            throw new NotImplementedException();
        }
    }
}
