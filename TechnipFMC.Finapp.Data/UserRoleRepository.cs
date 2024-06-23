using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;
using System.Xml.Linq;

namespace TechnipFMC.Finapp.Data
{
    public class UserRoleRepository : BaseRepository, IUserRoleRepository
    {
        public UserRoleRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteUserRoleMaster";
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
        public IEnumerable<UserRole> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllUserRole";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<UserRole> obj = new List<UserRole>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    UserRole userrole = new UserRole();
                    userrole.RoleID = Convert.ToInt32(results.Rows[i]["RoleID"]);
                    userrole.RoleName = results.Rows[i]["RoleName"].ToString();
                    userrole.RoleCode = results.Rows[i]["RoleCode"].ToString();
                    userrole.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    userrole.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    userrole.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(userrole);
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
        public UserRole GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllUserRole";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new UserRole()
                        {
                            RoleID = (int)reader["RoleID"],
                            RoleName = (string)reader["RoleName"],
                            RoleCode = (string)reader["RoleCode"],
                            CreatedBy = (int)reader["CreatedBy"],
                            Active = (bool)reader["Active"],
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

        public UserRole Save(UserRole userRole)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveUserRole";
                cmd.Parameters.AddWithValue("@P_Id", userRole.RoleID);
                cmd.Parameters.AddWithValue("@P_RoleName", userRole.RoleName);
                cmd.Parameters.AddWithValue("@P_RoleCode", userRole.RoleCode);
                cmd.Parameters.AddWithValue("@P_CreatedBy", userRole.CreatedBy);
                cmd.Parameters.AddWithValue("@P_Active", userRole.Active);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userRole.RoleID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return userRole;
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
        public UserRole Update(UserRole userRole)
        {
            throw new NotImplementedException();
        }
        public bool MapUserProject(UserProjects userProjects)
        {
            try
            {

                var xmldata = new XElement("TDS", from ObjDetails in userProjects.UserProject
                                                  select new XElement("TD",
                                                 new XElement("UserProjectId", ObjDetails.UserProjectId),
                                                 new XElement("ProjectId", ObjDetails.ProjectId),
                                                 new XElement("StartDate", ObjDetails.StartDate),
                                                 new XElement("EndDate", ObjDetails.EndDate)));

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MapScenarioToProjects";
                cmd.Parameters.AddWithValue("@P_Id", userProjects.UserId);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                cmd.Parameters.AddWithValue("@P_CreatedBy", userProjects.CreatedBy);


                cmd.ExecuteNonQuery();

                return true;
                
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

        public bool MapUserDepartment(UserDepartment userDepartment)
        {
            try
            {

                var xmldata = new XElement("TDS", userDepartment.DepartmentId);                                                  

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MapScenarioToProjects";
                cmd.Parameters.AddWithValue("@P_Id", userDepartment.UserId);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                cmd.Parameters.AddWithValue("@P_CreatedBy", userDepartment.CreatedBy);


                cmd.ExecuteNonQuery();

                return true;

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

        public IEnumerable<ProjectForScenario> GetAllProjects(int ScenarioID)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllProjectOfUser";
                cmd.Parameters.AddWithValue("@P_ScenarioID", ScenarioID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ProjectForScenario> obj = new List<ProjectForScenario>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ProjectForScenario projectForScenario = new ProjectForScenario();
                    //projectForScenario.ManualProjectCode = results.Rows[i]["ManualProjectCode"].ToString();
                    projectForScenario.ProjectCode = results.Rows[i]["ProjectCode"].ToString();
                    projectForScenario.ProjectName = results.Rows[i]["ProjectName"].ToString();
                    obj.Add(projectForScenario);
                }

                //Scenarios source = new Scenarios() { ScenariosID = 1, ScenariosName = "India" };
                //List<Scenarios> obj = new List<Scenarios>();
                //obj.Add(source);
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

    }
}
