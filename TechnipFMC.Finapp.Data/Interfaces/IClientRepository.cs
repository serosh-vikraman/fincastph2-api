using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAll();
        IEnumerable<Client> GetAllClientsofUser(int userid);
        Client Save(Client client);
        Client GetById(int Id);

        Client Update(Client client);

        bool Delete(int Id, int DeletedBy);
    }
}
