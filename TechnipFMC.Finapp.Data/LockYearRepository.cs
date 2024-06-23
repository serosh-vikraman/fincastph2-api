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
    public class LockYearRepository : BaseRepository, ILockYearRepository
    {
        public LockYearRepository()
        { }

        public bool Delete(int Year, string DeletedBy)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteLockYear";
                cmd.Parameters.AddWithValue("@P_Year", Year);
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
        public IEnumerable<LockYear> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllLockYear";
                cmd.Parameters.AddWithValue("@P_Year", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<LockYear> obj = new List<LockYear>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    LockYear lockYear = new LockYear();

                    lockYear.Id = Convert.ToInt32(results.Rows[i]["Id"]);
                    lockYear.Year = Convert.ToInt32(results.Rows[i]["Year"]);
                    lockYear.Lock = Convert.ToBoolean(results.Rows[i]["Lock"]);
                    lockYear.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    lockYear.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(lockYear);
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
        public LockYear GetById(int Year)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllLockYear";
                cmd.Parameters.AddWithValue("@P_Year", Year);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new LockYear()
                        {
                            Id = (int)reader["Id"],
                            Year = (int)reader["Year"],
                            Lock = (bool)reader["Lock"],
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

        public LockYear Save(LockYear lockYear)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveLockYear";
                cmd.Parameters.AddWithValue("@P_Id", lockYear.Id);
                cmd.Parameters.AddWithValue("@P_Year", lockYear.Year);
                cmd.Parameters.AddWithValue("@P_Lock", lockYear.Lock);
                cmd.Parameters.AddWithValue("@P_CreatedBy", lockYear.CreatedBy);
                //cmd.Parameters.AddWithValue("@P_CustomerId", lockYear.CustomerID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lockYear.Id = Convert.ToInt32(reader.GetInt32(0));
                        lockYear.Message = reader.GetString(1);
                    }
                }
                return lockYear;
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
