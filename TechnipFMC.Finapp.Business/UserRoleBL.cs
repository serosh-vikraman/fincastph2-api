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
    public class UserRoleBL : IUserRoleBL
    {
        
        public UserRoleBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            UserRoleRepository _userroleRepo = new UserRoleRepository();
            return _userroleRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<UserRole> GetAll()
        {
            UserRoleRepository _userroleRepo = new UserRoleRepository();
            return _userroleRepo.GetAll();
        }

        public UserRole GetById(int Id)
        {
            UserRoleRepository _userroleRepo = new UserRoleRepository();
            return _userroleRepo.GetById(Id);
        }

        public UserRole Save(UserRole userRole)
        {
            UserRoleRepository _userroleRepo = new UserRoleRepository();
            return _userroleRepo.Save(userRole);
        }
        public UserRole Update(UserRole userRole)
        {
            UserRoleRepository _userroleRepo = new UserRoleRepository();
            return _userroleRepo.Update(userRole);
        }
        public bool MapUserProject(UserProjects userProjects)
        {
            UserRoleRepository _userroleRepo = new UserRoleRepository();
            return _userroleRepo.MapUserProject(userProjects);
        }
        public bool MapUserDepartment(UserDepartment userDepartment)
        {
            UserRoleRepository _userroleRepo = new UserRoleRepository();
            return _userroleRepo.MapUserDepartment(userDepartment);
        }

    }
}
