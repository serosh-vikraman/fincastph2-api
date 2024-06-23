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
    public class CountryRepository : BaseRepository, ICountryRepository
    {
        public CountryRepository()
        { }
        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteCountryMaster";
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

        public IEnumerable<Country> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCountry";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Country> obj = new List<Country>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Country country = new Country();
                    country.CountryID = Convert.ToInt32(results.Rows[i]["CountryID"]);
                    country.CountryName = results.Rows[i]["CountryName"].ToString();
                    country.CountryCode = results.Rows[i]["CountryCode"].ToString();
                    country.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    country.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    country.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(country);
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

        public Country GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCountry";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new Country()
                        {
                            CountryID = (int)reader["CountryID"],
                            CountryName = (string)reader["CountryName"],
                            CountryCode = (string)reader["CountryCode"],
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

        public Country Save(Country country)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveCountry";
                cmd.Parameters.AddWithValue("@P_Id", country.CountryID);
                cmd.Parameters.AddWithValue("@P_CountryName", country.CountryName);
                cmd.Parameters.AddWithValue("@P_CountryCode", country.CountryCode);
                cmd.Parameters.AddWithValue("@P_Active", country.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", country.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        country.CountryID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return country;
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

        public Country Update(Country Country)
        {
            throw new NotImplementedException();
        }
    }
}
