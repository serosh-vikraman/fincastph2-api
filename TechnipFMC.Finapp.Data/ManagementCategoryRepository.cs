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
    public class ManagementCategoryRepository : BaseRepository, IManagementCategoryRepository
    {
        public ManagementCategoryRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteManagementCategoryMaster";
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
        public IEnumerable<ManagementCategory> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllManagementCategory";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ManagementCategory> obj = new List<ManagementCategory>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ManagementCategory managementcategory = new ManagementCategory();
                    managementcategory.ManagementCategoryID = Convert.ToInt32(results.Rows[i]["ManagementCategoryID"]);
                    managementcategory.ManagementCategoryName = results.Rows[i]["ManagementCategoryName"].ToString();
                    managementcategory.ManagementCategoryCode = results.Rows[i]["ManagementCategoryCode"].ToString();
                    managementcategory.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    managementcategory.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    managementcategory.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(managementcategory);
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
        public ManagementCategory GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllManagementCategory";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new ManagementCategory()
                        {
                            ManagementCategoryID = (int)reader["ManagementCategoryID"],
                            ManagementCategoryName = (string)reader["ManagementCategoryName"],
                            ManagementCategoryCode = (string)reader["ManagementCategoryCode"],
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

        public ManagementCategory Save(ManagementCategory managementcategory)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveManagementCategory";
                cmd.Parameters.AddWithValue("@P_Id", managementcategory.ManagementCategoryID);
                cmd.Parameters.AddWithValue("@P_ManagementCategoryName", managementcategory.ManagementCategoryName);
                cmd.Parameters.AddWithValue("@P_ManagementCategoryCode", managementcategory.ManagementCategoryCode);
                cmd.Parameters.AddWithValue("@P_Active", managementcategory.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", managementcategory.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        managementcategory.ManagementCategoryID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return managementcategory;
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

        public ManagementCategory Update(ManagementCategory managementcategory)
        {
            throw new NotImplementedException();
        }
    }
}
