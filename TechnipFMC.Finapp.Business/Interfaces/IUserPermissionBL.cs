using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IUserPermissionBL
    {
        IEnumerable<UserPermission> GetAll();
        UserPermission Save(UserPermission userPermission);
        UserPermission GetById(int Id);

        UserPermission Update(UserPermission userPermission);

        bool Delete(int Id, string Deletedby);
        UserPermission ValidateUser(string loginId, string password);
    }
}
