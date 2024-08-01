using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using TechnipFMC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace TechnipFMC.Finapp.Data
{
    public class CurrencyRepository : BaseRepository, ICurrencyRepository
    {
        public CurrencyRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteCurrencyMaster";
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

        public IEnumerable<Currency> GetAll()
        {
            try
            {
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCurrencies";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Currency> obj = new List<Currency>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Currency currency = new Currency();
                    currency.CurrencyID = Convert.ToInt32(results.Rows[i]["CurrencyID"]);
                    currency.CurrencyName = results.Rows[i]["CurrencyName"].ToString();
                    currency.CurrencyCode = results.Rows[i]["CurrencyCode"].ToString();
                    currency.CurrencySymbol = results.Rows[i]["CurrencySymbol"].ToString();
                    //currency.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    currency.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    currency.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(currency);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public Currency GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCurrencies";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new Currency()
                        {
                            CurrencyID = (int)reader["CurrencyID"],
                            CurrencyName = (string)reader["CurrencyName"],
                            CurrencyCode = (string)reader["CurrencyCode"],
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

        public Currency Save(Currency currency)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveCurrency";
                cmd.Parameters.AddWithValue("@P_Id", currency.CurrencyID);
                cmd.Parameters.AddWithValue("@P_CurrencyName", currency.CurrencyName);
                cmd.Parameters.AddWithValue("@P_CurrencyCode", currency.CurrencyCode);
                //cmd.Parameters.AddWithValue("@P_CurrencySymbol", currency.CurrencySymbol);
                cmd.Parameters.AddWithValue("@P_CreatedBy", currency.CreatedBy);
                cmd.Parameters.AddWithValue("@P_Active", currency.Active);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        currency.CurrencyID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }
                return currency;
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
