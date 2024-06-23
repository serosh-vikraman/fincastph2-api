using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;

namespace TechnipFMC.Finapp.Data
{
    public class UserPermissionRepository : BaseRepository, IUserPermissionRepository
    {
        public UserPermissionRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteUserPermission";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                cmd.Parameters.AddWithValue("@P_User", DeletedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                }

                return false;
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
        public IEnumerable<UserPermission> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllUserPermission";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<UserPermission> obj = new List<UserPermission>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    UserPermission UserPermission = new UserPermission();
                    UserPermission.Id = Convert.ToInt32(results.Rows[i]["Id"]);
                    UserPermission.UserName = results.Rows[i]["UserName"].ToString();
                    UserPermission.EmailID = results.Rows[i]["EmailID"].ToString();
                    UserPermission.RoleID = Convert.ToInt32(results.Rows[i]["RoleID"]);
                    UserPermission.RoleCode = results.Rows[i]["RoleCode"].ToString();
                    UserPermission.DepartmentID = Convert.ToInt32(results.Rows[i]["DepartmentID"]);
                    UserPermission.DepartmentName = results.Rows[i]["DepartmentName"].ToString();
                    UserPermission.ActiveStatus = Convert.ToBoolean(results.Rows[i]["ActiveStatus"]);
                    UserPermission.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    UserPermission.Status = results.Rows[i]["Status"].ToString();

                    obj.Add(UserPermission);
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
        public UserPermission GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllUserPermission";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new UserPermission()
                        {
                            Id = (int)reader["Id"],
                            UserName = (string)reader["UserName"],
                            EmailID = (string)reader["EmailID"],
                            Pasword= (string)reader["Pasword"],
                            RoleID = (int)reader["RoleID"],
                            RoleCode = (string)reader["RoleCode"],
                            DepartmentID = (int)reader["DepartmentID"],
                            DepartmentName = (string)reader["DepartmentName"],
                            ActiveStatus = (bool)reader["ActiveStatus"],
                            CreatedBy = (int)reader["CreatedBy"],
                            Status = (string)reader["Status"],
                        };
                    }
                }
                return null;
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

        public UserPermission Save(UserPermission userPermission)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveUserPermission";
                cmd.Parameters.AddWithValue("@P_Id", userPermission.Id);
                cmd.Parameters.AddWithValue("@P_UserName", userPermission.UserName);
                cmd.Parameters.AddWithValue("@P_EmailId", userPermission.EmailID);
                cmd.Parameters.AddWithValue("@P_Password", userPermission.Pasword);
                cmd.Parameters.AddWithValue("@P_RoleId", userPermission.RoleID);
                cmd.Parameters.AddWithValue("@P_DepartmentID", userPermission.DepartmentID);
                cmd.Parameters.AddWithValue("@P_ActiveStatus", userPermission.ActiveStatus);
                cmd.Parameters.AddWithValue("@P_CreatedBy", userPermission.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userPermission.Id = reader.GetInt32(0);
                    }
                }

                return userPermission;
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

        public UserPermission ValidateUser(string loginId,string password)
        {
            try
            {
                var userPermission = new UserPermission();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ValidateUser";
                cmd.Parameters.AddWithValue("@P_LoginId", loginId);
                cmd.Parameters.AddWithValue("@P_Password", password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userPermission.UserName = (string)reader["UserName"];
                        userPermission.Id = (int)reader["Id"];
                        userPermission.RoleID = (int)reader["RoleID"];
                        userPermission.EmailID = (string)reader["EmailID"];
                        userPermission.DepartmentID = (int)reader["DepartmentID"];
                        userPermission.CustomerID = (int)reader["CustomerID"];
                        userPermission.ActiveStatus = (bool)reader["ActiveStatus"];
                    }
                }
                return userPermission;
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
