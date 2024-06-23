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
    public class StatutoryCategoryRepository : BaseRepository, IStatutoryCategoryRepository
    {
        public StatutoryCategoryRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteStatutoryCategoryMaster";
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
        public IEnumerable<StatutoryCategory> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllStatutoryCategory";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<StatutoryCategory> obj = new List<StatutoryCategory>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    StatutoryCategory statutorycategory = new StatutoryCategory();
                    statutorycategory.StatutoryCategoryID = Convert.ToInt32(results.Rows[i]["StatutoryCategoryID"]);
                    statutorycategory.StatutoryCategoryName = results.Rows[i]["StatutoryCategoryName"].ToString();
                    statutorycategory.StatutoryCategoryCode = results.Rows[i]["StatutoryCategoryCode"].ToString();
                    statutorycategory.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    statutorycategory.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    statutorycategory.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(statutorycategory);
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
        public StatutoryCategory GetById(int Id )
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllStatutoryCategory";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new StatutoryCategory()
                        {
                            StatutoryCategoryID = (int)reader["StatutoryCategoryID"],
                            StatutoryCategoryName = (string)reader["StatutoryCategoryName"],
                            StatutoryCategoryCode = (string)reader["StatutoryCategoryCode"],
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

        public StatutoryCategory Save(StatutoryCategory statutorycategory)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveStatutoryCategory";
                cmd.Parameters.AddWithValue("@P_Id", statutorycategory.StatutoryCategoryID);
                cmd.Parameters.AddWithValue("@P_StatutoryCategoryName", statutorycategory.StatutoryCategoryName);
                cmd.Parameters.AddWithValue("@P_StatutoryCategoryCode", statutorycategory.StatutoryCategoryCode);
                cmd.Parameters.AddWithValue("@P_Active", statutorycategory.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", statutorycategory.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        statutorycategory.StatutoryCategoryID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return statutorycategory;
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

        public StatutoryCategory Update(StatutoryCategory statutorycategory)
        {
            throw new NotImplementedException();
        }
    }
}
