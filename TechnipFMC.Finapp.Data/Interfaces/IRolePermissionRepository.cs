using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IRolePermissionRepository
    {
        IEnumerable<RolePermission> GetAll();
        RolePermission Save(RolePermission rolePermission);
        RolePermission GetById(int Id);

        //RolePermission Update(RolePermission rolePermission);

        bool Delete(int Id, string DeletedBy);
    }
}
