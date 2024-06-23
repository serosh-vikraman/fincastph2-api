using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;
using System.IO;

namespace TechnipFMC.Finapp.Data
{
    public class ProjectEntityRepository : BaseRepository, IProjectEntityRepository
    {
        public ProjectEntityRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteProjectEntityMaster";
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
        public IEnumerable<ProjectEntity> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllProjectEntity";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ProjectEntity> obj = new List<ProjectEntity>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ProjectEntity projectEntity = new ProjectEntity();
                    projectEntity.ProjectEntityID = Convert.ToInt32(results.Rows[i]["ProjectEntityID"]);
                    projectEntity.ProjectEntityName = results.Rows[i]["ProjectEntityName"].ToString();
                    projectEntity.ProjectEntityCode = results.Rows[i]["ProjectEntityCode"].ToString();
                    projectEntity.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    projectEntity.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    projectEntity.Status = results.Rows[i]["Status"].ToString();

                    obj.Add(projectEntity);
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
        public ProjectEntity GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllProjectEntity";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new ProjectEntity()
                        {
                            ProjectEntityID = (int)reader["ProjectEntityID"],
                            ProjectEntityName = (string)reader["ProjectEntityName"],
                            ProjectEntityCode = (string)reader["ProjectEntityCode"],
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

        public ProjectEntity Save(ProjectEntity projectEntity)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveProjectEntity";
                cmd.Parameters.AddWithValue("@P_Id", projectEntity.ProjectEntityID);
                cmd.Parameters.AddWithValue("@P_ProjectEntityName", projectEntity.ProjectEntityName);
                cmd.Parameters.AddWithValue("@P_ProjectEntityCode", projectEntity.ProjectEntityCode);
                cmd.Parameters.AddWithValue("@P_Active", projectEntity.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", projectEntity.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        projectEntity.ProjectEntityID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return projectEntity;
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
        public ProjectEntity Update(ProjectEntity projectEntity)
        {
            throw new NotImplementedException();
        }

        
    }
}
