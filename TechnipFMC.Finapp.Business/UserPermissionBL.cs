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
    public class UserPermissionBL : IUserPermissionBL
    {
         
        public UserPermissionBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            UserPermissionRepository _userPermissionRepo = new UserPermissionRepository();
            return _userPermissionRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<UserPermission> GetAll()
        {
            UserPermissionRepository _userPermissionRepo = new UserPermissionRepository();
            return _userPermissionRepo.GetAll();
        }

        public UserPermission GetById(int Id)
        {
            UserPermissionRepository _userPermissionRepo = new UserPermissionRepository();
            return _userPermissionRepo.GetById(Id);
        }

        public UserPermission Save(UserPermission UserPermission)
        {
            UserPermissionRepository _userPermissionRepo = new UserPermissionRepository();
            return _userPermissionRepo.Save(UserPermission);
        }
        public UserPermission Update(UserPermission UserPermission)
        {
            throw new NotImplementedException();
            //return _userPermissionRepo.Update(UserPermission);
        }
        public UserPermission ValidateUser(string loginId,string password)
        {
            UserPermissionRepository _userPermissionRepo = new UserPermissionRepository();
            return _userPermissionRepo.ValidateUser(loginId, password);
        }
    }
}
