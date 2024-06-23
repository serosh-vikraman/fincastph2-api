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
    public class BillingTypeRepository : BaseRepository, IBillingTypeRepository
    {
        public BillingTypeRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteBillingTypesMaster";
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
        public IEnumerable<BillingType> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllBillingTypes";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<BillingType> obj = new List<BillingType>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    BillingType billingTypes = new BillingType();
                    billingTypes.BillingTypeID = Convert.ToInt32(results.Rows[i]["BillingTypeID"]);
                    billingTypes.BillingTypeName = results.Rows[i]["BillingTypeName"].ToString();
                    billingTypes.BillingTypeCode = results.Rows[i]["BillingTypeCode"].ToString();
                    billingTypes.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    billingTypes.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    billingTypes.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(billingTypes);
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
        public BillingType GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllBillingTypes";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new BillingType()
                        {
                            BillingTypeID = (int)reader["BillingTypeID"],
                            BillingTypeName = (string)reader["BillingTypeName"],
                            BillingTypeCode = (string)reader["BillingTypeCode"],
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

        public BillingType Save(BillingType billingtype)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveBillingTypes";
                cmd.Parameters.AddWithValue("@P_Id", billingtype.BillingTypeID);
                cmd.Parameters.AddWithValue("@P_BillingTypeName", billingtype.BillingTypeName);
                cmd.Parameters.AddWithValue("@P_BillingTypeCode", billingtype.BillingTypeCode);
                cmd.Parameters.AddWithValue("@P_Active", billingtype.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", billingtype.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        billingtype.BillingTypeID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return billingtype;
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

        public BillingType Update(BillingType billingTypes)
        {
            try
            {
                throw new NotImplementedException();
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
