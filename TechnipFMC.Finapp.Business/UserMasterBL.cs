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
    public class UserMasterBL : IUserMasterBL
    {
        public UserMasterBL()
        { }

        public string Delete(int Id, int Deletedby)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<UserMaster> GetAll()
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.GetAll();
        }

        public UserMaster GetById(int Id)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.GetById(Id);
        }

        public UserMaster Save(UserMaster UserMaster)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.Save(UserMaster);
        }
        public string Changepassword(ChangePasswordModel UserMaster)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.Changepassword(UserMaster);
        }
        public string Forgotpassword(ForgotPasswordModel UserMaster)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.Forgotpassword(UserMaster);
        }
        public UserMaster Update(UserMaster UserMaster)
        {
            throw new NotImplementedException();
            //return _userMasterRepo.Update(UserMaster);
        }
        public UserMaster ValidateUser(string loginId, string password)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.ValidateUser(loginId, password);
        }
        public UserMaster ValidateEmail(string loginId)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.ValidateEmail(loginId);
        }
        public int UpdateLicense(int customerId, DateTime licenseEndDate)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            return _userMasterRepo.UpdateLicense(customerId, licenseEndDate);
        }
        public void ChangetoFree(int customerId)
        {
            UserMasterRepository _userMasterRepo = new UserMasterRepository();
            _userMasterRepo.ChangetoFree(customerId);
        }
    }
}
