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
    public class ProjectSegmentRepository : BaseRepository, IProjectSegmentRepository
    {
        public ProjectSegmentRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteProjectSegmentMaster";
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
        public IEnumerable<ProjectSegment> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllProjectSegment";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ProjectSegment> obj = new List<ProjectSegment>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ProjectSegment projectsegment = new ProjectSegment();
                    projectsegment.ProjectSegmentID = Convert.ToInt32(results.Rows[i]["ProjectSegmentID"]);
                    projectsegment.ProjectSegmentName = results.Rows[i]["ProjectSegmentName"].ToString();
                    projectsegment.ProjectSegmentCode = results.Rows[i]["ProjectSegmentCode"].ToString();
                    projectsegment.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    projectsegment.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    projectsegment.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(projectsegment);
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
        public ProjectSegment GetById(int Id )
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllProjectSegment";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new ProjectSegment()
                        {
                            ProjectSegmentID = (int)reader["ProjectSegmentID"],
                            ProjectSegmentName = (string)reader["ProjectSegmentName"],
                            ProjectSegmentCode = (string)reader["ProjectSegmentCode"],
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

        public ProjectSegment Save(ProjectSegment projectsegment)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveProjectSegment";
                cmd.Parameters.AddWithValue("@P_Id", projectsegment.ProjectSegmentID);
                cmd.Parameters.AddWithValue("@P_ProjectSegmentName", projectsegment.ProjectSegmentName);
                cmd.Parameters.AddWithValue("@P_ProjectSegmentCode", projectsegment.ProjectSegmentCode);
                cmd.Parameters.AddWithValue("@P_Active", projectsegment.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", projectsegment.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        projectsegment.ProjectSegmentID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return projectsegment;
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
        public ProjectSegment Update(ProjectSegment projectsegment)
        {
            throw new NotImplementedException();
        }

    }
}
