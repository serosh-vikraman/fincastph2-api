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
    public class SubActionRepository : BaseRepository, ISubActionRepository
    {
        public SubActionRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();   
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteSubActionMaster";
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
        public IEnumerable<SubAction> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllSubActions";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<SubAction> obj = new List<SubAction>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    SubAction subaction = new SubAction();
                    subaction.SubActionID = Convert.ToInt32(results.Rows[i]["SubActionID"]);
                    subaction.SubActionName = results.Rows[i]["SubActionName"].ToString();
                    subaction.SubActionCode = results.Rows[i]["SubActionCode"].ToString();
                    subaction.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    subaction.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    subaction.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(subaction);
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
        public SubAction GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllSubActions";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new SubAction()
                        {
                            SubActionID = (int)reader["SubActionID"],
                            SubActionName = (string)reader["SubActionName"],
                            SubActionCode = (string)reader["SubActionCode"],
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

        public SubAction Save(SubAction subaction)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveSubAction";
                cmd.Parameters.AddWithValue("@P_Id", subaction.SubActionID);
                cmd.Parameters.AddWithValue("@P_SubActionName", subaction.SubActionName);
                cmd.Parameters.AddWithValue("@P_SubActionCode", subaction.SubActionCode);
                cmd.Parameters.AddWithValue("@P_CreatedBy", subaction.CreatedBy);
                cmd.Parameters.AddWithValue("@P_Active", subaction.Active);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        subaction.SubActionID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return subaction;
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

        public SubAction Update(SubAction subAction)
        {
            throw new NotImplementedException();
        }
    }
}
