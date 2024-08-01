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
    public class LockQuarterRepository : BaseRepository, ILockQuarterRepository
    {
        public LockQuarterRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteLockQuarter";
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
        public IEnumerable<LockQuarter> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllLockQuarter";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<LockQuarter> obj = new List<LockQuarter>();
                var dataEntryInterval = results.Rows[0]["DataEntryInterval"].ToString();
                var quarterToMonthMap = new Dictionary<string, string>
{
    { "Q1", "Jan" }, { "Q2", "Feb" }, { "Q3", "Mar" }, { "Q4", "Apr" },
    { "Q5", "May" }, { "Q6", "Jun" }, { "Q7", "Jul" }, { "Q8", "Aug" },
    { "Q9", "Sep" }, { "Q10", "Oct" }, { "Q11", "Nov" }, { "Q12", "Dec" }
};
                for (int i = 0; i < results.Rows.Count; i++)
                {
                    LockQuarter lockQuarter = new LockQuarter();
                    lockQuarter.Id = Convert.ToInt32(results.Rows[i]["Id"]);
                    lockQuarter.Year = Convert.ToInt32(results.Rows[i]["Year"]);
                    lockQuarter.Quarter = dataEntryInterval == "Monthly" ? quarterToMonthMap[results.Rows[i]["Quarter"].ToString()] : results.Rows[i]["Quarter"].ToString(); ;
                    lockQuarter.Lock = Convert.ToBoolean(results.Rows[i]["Lock"]);
                    lockQuarter.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    lockQuarter.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(lockQuarter);
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
        public LockQuarter GetById(int Id)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllLockQuarter";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new LockQuarter()
                        {
                            Id = (int)reader["Id"],
                            Quarter = (string)reader["Quarter"],
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

        public LockQuarter Save(LockQuarter lockQuarter)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveLockQuarter";
                cmd.Parameters.AddWithValue("@P_Id", lockQuarter.Id);
                cmd.Parameters.AddWithValue("@P_Year", lockQuarter.Year);
                cmd.Parameters.AddWithValue("@P_Quarter", lockQuarter.Quarter);
                cmd.Parameters.AddWithValue("@P_Lock", lockQuarter.Lock);
                cmd.Parameters.AddWithValue("@P_CreatedBy", lockQuarter.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lockQuarter.Id = Convert.ToInt32(reader.GetInt32(0));
                        lockQuarter.Message = reader.GetString(1);
                        lockQuarter.DataEntryInterval = reader.GetString(2);
                    }
                }
                return lockQuarter;
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
