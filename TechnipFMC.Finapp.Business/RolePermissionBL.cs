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
    public class RolePermissionBL : IRolePermissionBL
    {
         
        public RolePermissionBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            RolePermissionRepository _rolePermissionRepo = new RolePermissionRepository();
            return _rolePermissionRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<RolePermission> GetAll()
        {
            RolePermissionRepository _rolePermissionRepo = new RolePermissionRepository();
            return _rolePermissionRepo.GetAll();
        }

        public RolePermission GetById(int Id)
        {
            RolePermissionRepository _rolePermissionRepo = new RolePermissionRepository();
            return _rolePermissionRepo.GetById(Id);
        }

        public RolePermission Save(RolePermission rolePermission)
        {
            RolePermissionRepository _rolePermissionRepo = new RolePermissionRepository();
            return _rolePermissionRepo.Save(rolePermission);
        }

        public RolePermission Update(RolePermission rolePermission)
        {
            RolePermissionRepository _rolePermissionRepo = new RolePermissionRepository();
            throw new NotImplementedException();
        }      

    }
}
