using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IUserMasterRepository
    {
        IEnumerable<UserMaster> GetAll();
        UserMaster Save(UserMaster userMaster);
        string Changepassword(ChangePasswordModel userMaster);
        string Forgotpassword(ForgotPasswordModel userMaster);
        UserMaster GetById(int Id);

        //RoleMaster Update(RoleMaster roleMaster);

        string Delete(int Id, int DeletedBy);
        UserMaster ValidateUser(string loginId, string password);
        Task<UserMaster> ValidateEmail(string loginId);
        int UpdateLicense(int CustomerId, DateTime licenseEndDate);
        void ChangetoFree(int CustomerId);
    }
}
