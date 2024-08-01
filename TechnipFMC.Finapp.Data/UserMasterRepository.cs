using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;
using System.Xml.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using System.Threading.Tasks;



namespace TechnipFMC.Finapp.Data
{
    public class UserMasterRepository : BaseRepository, IUserMasterRepository
    {
        public UserMasterRepository()
        { }

        public string Delete(int Id, int DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteUserMaster";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                cmd.Parameters.AddWithValue("@P_User", DeletedBy);
                string message = "";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        message = reader.GetString(0);
                        return message;
                    }
                }

                return message;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public IEnumerable<UserMaster> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllUserMaster";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<UserMaster> obj = new List<UserMaster>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    UserMaster UserMaster = new UserMaster();
                    UserMaster.Id = Convert.ToInt32(results.Rows[i]["Id"]);
                    UserMaster.UserName = results.Rows[i]["UserName"].ToString().Decrypt();
                    UserMaster.EmailID = results.Rows[i]["EmailID"].ToString().Decrypt();
                    UserMaster.RoleID = Convert.ToInt32(results.Rows[i]["RoleID"]);
                    UserMaster.RoleCode = results.Rows[i]["RoleCode"].ToString();
                    UserMaster.RoleName = results.Rows[i]["RoleName"].ToString();
                    UserMaster.DepartmentID = Convert.ToInt32(results.Rows[i]["DepartmentID"]);
                    UserMaster.DepartmentName = results.Rows[i]["DepartmentName"].ToString().Decrypt();
                    //UserMaster.ClientIDs = Convert.ToInt32(results.Rows[i]["ClientID"]);
                    UserMaster.ClientName = results.Rows[i]["ClientName"].ToString();
                    UserMaster.ActiveStatus = Convert.ToBoolean(results.Rows[i]["ActiveStatus"]);
                    UserMaster.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    UserMaster.Status = results.Rows[i]["Status"].ToString();

                    obj.Add(UserMaster);
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public UserMaster GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllUserMaster";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                //SqlDataReader reader = cmd.ExecuteReader();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds);
                base.DBConnection.Close();
                UserMaster userMaster = new UserMaster();
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    userMaster.Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                    userMaster.UserName = ds.Tables[0].Rows[0]["UserName"].ToString().Decrypt();
                    userMaster.Pasword = ds.Tables[0].Rows[0]["Pasword"].ToString().Decrypt();
                    userMaster.EmailID = ds.Tables[0].Rows[0]["EmailID"].ToString().Decrypt();
                    userMaster.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString().Decrypt();
                    userMaster.RoleID = Convert.ToInt32(ds.Tables[0].Rows[0]["RoleID"]);
                    userMaster.RoleCode = ds.Tables[0].Rows[0]["RoleCode"].ToString();
                    userMaster.DepartmentID = Convert.ToInt32(ds.Tables[0].Rows[0]["DepartmentID"]);
                    userMaster.DepartmentName = ds.Tables[0].Rows[0]["DepartmentName"].ToString().Decrypt();
                    //UserMaster.ClientIDs = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientID"]);
                    userMaster.ClientName = ds.Tables[0].Rows[0]["ClientName"].ToString();
                    userMaster.ActiveStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["ActiveStatus"]);
                    userMaster.CreatedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["CreatedBy"]);
                    userMaster.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null))
                {
                    List<int> clientids = new List<int>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        int j = Convert.ToInt32(ds.Tables[1].Rows[i]["ClientId"]);
                        clientids.Add(j);                       
                    }
                    userMaster.ClientIDs = clientids;
                }

                return userMaster;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }

        }

        public UserMaster Save(UserMaster userMaster)
        {
            try
            {
                var xmldata = new XElement("TDS", from ObjDetails in userMaster.ClientIDs
                                                  select new XElement("TD",
                                                 new XElement("ClientId", ObjDetails.ToString())));
                userMaster.UserName = userMaster.UserName.Encrypt();
                userMaster.EmailID = userMaster.EmailID.Encrypt();
                userMaster.Mobile = userMaster.Mobile.Encrypt();
                userMaster.Pasword = userMaster.Pasword.Encrypt();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveUserMaster";
                cmd.Parameters.AddWithValue("@P_Id", userMaster.Id);
                cmd.Parameters.AddWithValue("@P_UserMasterId", userMaster.UserMasterId);
                cmd.Parameters.AddWithValue("@P_UserName", userMaster.UserName);
                cmd.Parameters.AddWithValue("@P_EmailId", userMaster.EmailID);
                cmd.Parameters.AddWithValue("@P_Mobile", userMaster.Mobile);
                cmd.Parameters.AddWithValue("@P_Password", userMaster.Pasword);
                cmd.Parameters.AddWithValue("@P_RoleId", userMaster.RoleID);
                cmd.Parameters.AddWithValue("@P_DepartmentID", userMaster.DepartmentID);
                //cmd.Parameters.AddWithValue("@P_ClientID", userMaster.ClientID);
                cmd.Parameters.AddWithValue("@P_CustomerID", userMaster.CustomerID);
                cmd.Parameters.AddWithValue("@P_CustomerCode", userMaster.OrgCode);
                cmd.Parameters.AddWithValue("@P_ActiveStatus", userMaster.ActiveStatus);
                cmd.Parameters.AddWithValue("@P_CreatedBy", userMaster.CreatedBy);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userMaster.Id = reader.GetInt32(0);
                        userMaster.UserName = userMaster.UserName.Decrypt();
                        userMaster.EmailID = userMaster.EmailID.Decrypt();
                        userMaster.Pasword = userMaster.Pasword.Decrypt();
                    }
                }

                return userMaster;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }

        }
        public string Changepassword(ChangePasswordModel passwordModel)
        {
            try
            {

                passwordModel.OldPassword = passwordModel.OldPassword.Encrypt();
                passwordModel.NewPassword = passwordModel.NewPassword.Encrypt();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ChangeUserPassword";
                cmd.Parameters.AddWithValue("@P_UserId", passwordModel.userId);
                cmd.Parameters.AddWithValue("@P_OldPassword", passwordModel.OldPassword);
                cmd.Parameters.AddWithValue("@P_NewPassword", passwordModel.NewPassword);
                SqlDataReader reader = cmd.ExecuteReader();
                var message = "";
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        message = reader.GetString(0);
                    }
                }

                return message;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }

        }
        public string Forgotpassword(ForgotPasswordModel passwordModel)
        {
            try
            {
                //passwordModel.LoginId = HttpUtility.UrlDecode(passwordModel.LoginId);
                passwordModel.NewPassword = passwordModel.NewPassword.Encrypt();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ForgotEmailPassword";
                cmd.Parameters.AddWithValue("@P_LoginId", passwordModel.LoginId);
                cmd.Parameters.AddWithValue("@P_Token", passwordModel.Token);
                cmd.Parameters.AddWithValue("@P_NewPassword", passwordModel.NewPassword);
                SqlDataReader reader = cmd.ExecuteReader();
                var message = "";
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        message = reader.GetString(0);
                    }
                }

                return message;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }

        }

        public UserMaster ValidateUser(string loginId, string password)
        {
            try
            {                
                loginId = loginId.Encrypt();
                password = password.Encrypt();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ValidateUser";
                cmd.Parameters.AddWithValue("@P_LoginId", loginId);
                cmd.Parameters.AddWithValue("@P_Password", password);
                //SqlDataReader reader = cmd.ExecuteReader();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds);
                base.DBConnection.Close();
                UserMaster userMaster = new UserMaster();
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {

                    userMaster.Authorized = ds.Tables[0].Rows[0][0].ToString();

                }
                if ((ds != null) && (ds.Tables.Count > 1) && (ds.Tables[1] != null))
                {
                    userMaster.Id = Convert.ToInt32(ds.Tables[1].Rows[0]["Id"]);
                    userMaster.UserName = ds.Tables[1].Rows[0]["UserName"].ToString().Decrypt();
                    userMaster.EmailID = ds.Tables[1].Rows[0]["EmailID"].ToString().Decrypt();
                    userMaster.RoleName = ds.Tables[1].Rows[0]["RoleName"].ToString();
                    userMaster.RoleID = Convert.ToInt32(ds.Tables[1].Rows[0]["RoleID"]);
                    userMaster.DepartmentID = Convert.ToInt32(ds.Tables[1].Rows[0]["DepartmentID"]);
                    userMaster.DepartmentName = ds.Tables[1].Rows[0]["DepartmentName"].ToString().Decrypt();
                    userMaster.DepartmentCode = ds.Tables[1].Rows[0]["DepartmentCode"].ToString();
                    userMaster.ActiveStatus = Convert.ToBoolean(ds.Tables[1].Rows[0]["ActiveStatus"]);
                    userMaster.CustomerID = Convert.ToInt32(ds.Tables[1].Rows[0]["CustomerMasterID"]);
                    userMaster.OrgCode = ds.Tables[1].Rows[0]["OrgCode"].ToString();
                    userMaster.OrgName = ds.Tables[1].Rows[0]["OrgName"].ToString().Decrypt();
                    userMaster.AdminEmail = ds.Tables[1].Rows[0]["AdminEmail"].ToString().Decrypt();
                    userMaster.UserMasterId = Convert.ToInt32(ds.Tables[1].Rows[0]["UserMasterId"]);
                    userMaster.DataEntryInterval = ds.Tables[1].Rows[0]["DataEntryInterval"].ToString();
                }
                if ((ds != null) && (ds.Tables.Count > 2) && (ds.Tables[2] != null))
                {
                    
                        userMaster.planurl = ds.Tables[2].Rows[0][0].ToString();
                  
                }
                if ((ds != null) && (ds.Tables.Count > 3) && (ds.Tables[3] != null))
                {

                    userMaster.subscription_end = ds.Tables[3].Rows[0]["subscription_end"].ToString();
                    userMaster.PlanName = ds.Tables[3].Rows[0]["PlanName"].ToString();
                    userMaster.CurrencyName = ds.Tables[3].Rows[0]["CurrencyName"].ToString();
                    userMaster.CurrencyCode = ds.Tables[3].Rows[0]["CurrencyCode"].ToString();
                    userMaster.CurrencySymbol = ds.Tables[3].Rows[0]["CurrencySymbol"].ToString();
                    userMaster.Unit = ds.Tables[3].Rows[0]["Unit"].ToString();

                }
                return userMaster;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public async Task<UserMaster> ValidateEmail(string loginId)
        {
            try
            {
                loginId = loginId.Encrypt();
                var encodedloginId = HttpUtility.UrlEncode(loginId);
                var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var resultToken = new string(
                              Enumerable.Repeat(allChar, 8)
                              .Select(token => token[random.Next(token.Length)]).ToArray());
                string authToken = resultToken.ToString();
                var userMaster = new UserMaster();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ValidateEmail";
                cmd.Parameters.AddWithValue("@P_LoginId", loginId);
                cmd.Parameters.AddWithValue("@P_Token", authToken);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userMaster.UserName = (string)reader["UserName"].ToString().Decrypt();
                        userMaster.Id = (int)reader["Id"];
                        userMaster.RoleID = (int)reader["RoleID"];
                        userMaster.EmailID = (string)reader["EmailID"].ToString().Decrypt();
                        userMaster.DepartmentID = (int)reader["DepartmentID"];
                        //userMaster.ClientID = (int)reader["ClientID"];
                        userMaster.CustomerID = (int)reader["CustomerID"];
                        userMaster.ActiveStatus = (bool)reader["ActiveStatus"];
                        try
                        {                           
                            StringBuilder sb = new StringBuilder();
                            sb.Append($"Hi {userMaster.UserName.ToUpper()},<br/> Click on below given link to Reset Your Password<br/>");
                            sb.Append("<b><a href=https://fincast.app/forgotpassword?username=" + encodedloginId + "&code=" + authToken + ">Click here</a><br/></b>");
                            sb.Append("Thanks,<br> Fincast Team <br/>");
                            int ret = await SendEmail(userMaster.EmailID, sb.ToString(), "Fincast Reset Password Link");                            
                        }
                        catch (Exception ex) {
                        }
                    }
                }
                return userMaster;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public int UpdateLicense(int customerId, DateTime licenseEndDate)
        {
            try
            {
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateCustomerLicense";
                cmd.Parameters.AddWithValue("@P_Id", customerId);
                cmd.Parameters.AddWithValue("@P_LicenseEndDate", licenseEndDate.ToString("yyyy-MM-dd HH:mm:ss")); // Format DateTime as string
                int check = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        check = reader.GetInt32(0);
                    }
                }

                return check;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public void ChangetoFree(int customerId)
        {
            try
            {
                DateTime licenseEndDate = DateTime.Now.AddYears(2);
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ChangeToFree";
                cmd.Parameters.AddWithValue("@P_Id", customerId);
                cmd.Parameters.AddWithValue("@P_LicenseEndDate", licenseEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                int check = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                //if (reader.HasRows)
                //{
                //    while (reader.Read())
                //    {
                //        check = reader.GetInt32(0);
                //    }
                //}

                //return check;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }


    }
}
