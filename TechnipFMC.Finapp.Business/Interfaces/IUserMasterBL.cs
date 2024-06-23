using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IUserMasterBL
    {
        IEnumerable<UserMaster> GetAll();
        UserMaster Save(UserMaster userMaster);
        string Changepassword(ChangePasswordModel passwordModel);
        string Forgotpassword(ForgotPasswordModel passwordModel);
        UserMaster GetById(int Id);

        UserMaster Update(UserMaster userMaster);

        string Delete(int Id, int Deletedby);
        UserMaster ValidateUser(string loginId, string password);
        UserMaster ValidateEmail(string loginId);
        int UpdateLicense(int CustomerId, DateTime licenseEndDate);
        void ChangetoFree(int CustomerId);

    }
}
