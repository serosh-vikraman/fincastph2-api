using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IUserPermissionRepository
    {
        IEnumerable<UserPermission> GetAll();
        UserPermission Save(UserPermission userPermission);
        UserPermission GetById(int Id);

        //RolePermission Update(RolePermission rolePermission);

        bool Delete(int Id, string DeletedBy);
        UserPermission ValidateUser(string loginId, string password);
    }
}
