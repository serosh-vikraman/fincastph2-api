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
    public class BUCategoryRepository : BaseRepository, IBUCategoryRepository
    {
        public BUCategoryRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteBUCategoryMaster";
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
        public IEnumerable<BUCategory> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllBUCategory";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<BUCategory> obj = new List<BUCategory>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    BUCategory buCategory = new BUCategory();
                    buCategory.BUCategoryID = Convert.ToInt32(results.Rows[i]["BUCategoryID"]);
                    buCategory.BUCategoryName = results.Rows[i]["BUCategoryName"].ToString();
                    buCategory.BUCategoryCode = results.Rows[i]["BUCategoryCode"].ToString();
                    buCategory.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    buCategory.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    buCategory.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(buCategory);
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
        public BUCategory GetById(int Id)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllBUCategory";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new BUCategory()
                        {
                            BUCategoryID = (int)reader["BUCategoryID"],
                            BUCategoryName = (string)reader["BUCategoryName"],
                            BUCategoryCode = (string)reader["BUCategoryCode"],
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

        public BUCategory Save(BUCategory buCategory)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveBUCategory";
                cmd.Parameters.AddWithValue("@P_Id", buCategory.BUCategoryID);
                cmd.Parameters.AddWithValue("@P_BUCategoryName", buCategory.BUCategoryName);
                cmd.Parameters.AddWithValue("@P_BUCategoryCode", buCategory.BUCategoryCode);
                cmd.Parameters.AddWithValue("@P_Active", buCategory.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", buCategory.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        buCategory.BUCategoryID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return buCategory;
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

        public BUCategory Update(BUCategory buCategory)
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
