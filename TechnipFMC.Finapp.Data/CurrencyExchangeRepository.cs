using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TechnipFMC.Common;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data
{
    public class CurrencyExchangeRepository : BaseRepository, ICurrencyExchangeRepository
    {
        public CurrencyExchangeRepository()
        { }
        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteCurrencyExchange";
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

        public IEnumerable<CurrencyExchange> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCurrencyExchange";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<CurrencyExchange> obj = new List<CurrencyExchange>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    CurrencyExchange currencyExchange = new CurrencyExchange();
                    currencyExchange.Id = Convert.ToInt32(results.Rows[i]["Id"]);
                    currencyExchange.SourceCurrencyID = Convert.ToInt32(results.Rows[i]["SourceCurrencyID"]);
                    currencyExchange.TargetCurrencyID = Convert.ToInt32(results.Rows[i]["TargetCurrencyID"]);
                    currencyExchange.Year = Convert.ToInt32(results.Rows[i]["Year"]);
                    currencyExchange.Quarter = results.Rows[i]["Quarter"].ToString();
                    currencyExchange.AverageRate = Convert.ToDecimal(results.Rows[i]["AverageRate"]);
                    currencyExchange.LockStatus = Convert.ToBoolean(results.Rows[i]["LockStatus"]);
                    currencyExchange.CancelStatus = Convert.ToBoolean(results.Rows[i]["CancelStatus"]);
                    currencyExchange.SourceCurrencyCode = results.Rows[i]["SourceCurrencyCode"].ToString();
                    currencyExchange.TargetCurrencyCode = results.Rows[i]["TargetCurrencyCode"].ToString();
                    currencyExchange.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    currencyExchange.Status = results.Rows[i]["Status"].ToString();
                    currencyExchange.CancelActiveStatus = results.Rows[i]["CancelActiveStatus"].ToString();
                    obj.Add(currencyExchange);
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
        public CurrencyExchange GetById(int Id)
        {
            try
            {
                var currencyExchange = new CurrencyExchange();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCurrencyExchange";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new CurrencyExchange()
                        {
                            Id = (int)reader["Id"],
                            SourceCurrencyID = (int)reader["SourceCurrencyID"],
                            TargetCurrencyID = (int)reader["TargetCurrencyID"],
                            Year = (int)reader["Year"],
                            Quarter = (string)reader["Quarter"],
                            AverageRate = (decimal)reader["AverageRate"],
                            LockStatus = (bool)reader["LockStatus"],
                            CancelStatus = (bool)reader["CancelStatus"],
                            SourceCurrencyCode = (string)reader["SourceCurrencyCode"],
                            TargetCurrencyCode = (string)reader["TargetCurrencyCode"],
                            CreatedBy = (int)reader["CreatedBy"],
                            Status = (string)reader["Status"],
                            CancelActiveStatus = (string)reader["CancelActiveStatus"],
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

        public CurrencyExchange Save(CurrencyExchange currencyExchange)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveCurrencyExchange";
                cmd.Parameters.AddWithValue("@P_Id", currencyExchange.Id);
                cmd.Parameters.AddWithValue("@P_SourceCurrencyId", currencyExchange.SourceCurrencyID);
                //cmd.Parameters.AddWithValue("@P_SourceCurrencyCode", currencyExchange.SourceCurrencyCode);
                cmd.Parameters.AddWithValue("@P_TargetCurrencyID", currencyExchange.TargetCurrencyID);
                //cmd.Parameters.AddWithValue("@P_TargetCurrencyCode", currencyExchange.TargetCurrencyCode);
                cmd.Parameters.AddWithValue("@P_Year", currencyExchange.Year);
                cmd.Parameters.AddWithValue("@P_Quarter", currencyExchange.Quarter);
                cmd.Parameters.AddWithValue("@P_AverageRate", currencyExchange.AverageRate);
                cmd.Parameters.AddWithValue("@P_LockStatus", currencyExchange.LockStatus);
                cmd.Parameters.AddWithValue("@P_CancelStatus", currencyExchange.CancelStatus);
                cmd.Parameters.AddWithValue("@P_CreatedBy", currencyExchange.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        currencyExchange.Id = Convert.ToInt32(reader.GetInt32(0));
                        currencyExchange.SourceCurrencyCode = reader.GetString(1);
                        currencyExchange.TargetCurrencyCode = reader.GetString(2);
                        currencyExchange.Message = reader.GetString(3);
                    }
                }
                return currencyExchange;
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
