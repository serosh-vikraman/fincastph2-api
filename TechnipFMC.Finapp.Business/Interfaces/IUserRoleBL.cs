using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IUserRoleBL
    {
        IEnumerable<UserRole> GetAll();
        UserRole Save(UserRole userRole);
        bool MapUserProject(UserProjects userProjects);
        bool MapUserDepartment(UserDepartment userDepartment);
        UserRole GetById(int Id);

        UserRole Update(UserRole userRole);

        bool Delete(int Id, string Deletedby);
    }
}
