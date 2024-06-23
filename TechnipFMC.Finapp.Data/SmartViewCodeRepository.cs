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
    public class SmartViewCodeRepository : BaseRepository, ISmartViewCodeRepository
    {
        public SmartViewCodeRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteSmartViewCodeMaster";
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
        public IEnumerable<SmartViewCodeMaster> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllSmartViewCode";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<SmartViewCodeMaster> obj = new List<SmartViewCodeMaster>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    SmartViewCodeMaster smartviewcode = new SmartViewCodeMaster();
                    smartviewcode.SmartViewCodeID = Convert.ToInt32(results.Rows[i]["SmartViewCodeID"]);
                    smartviewcode.SmartViewName = results.Rows[i]["SmartViewName"].ToString();
                    smartviewcode.SmartViewCode = results.Rows[i]["SmartViewCode"].ToString();
                    smartviewcode.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    smartviewcode.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    smartviewcode.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(smartviewcode);
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
        public SmartViewCodeMaster GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllSmartViewCode";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new SmartViewCodeMaster()
                        {
                            SmartViewCodeID = (int)reader["SmartViewCodeID"],
                            SmartViewName = (string)reader["SmartViewName"],
                            SmartViewCode = (string)reader["SmartViewCode"],
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

        public SmartViewCodeMaster Save(SmartViewCodeMaster smartviewcode)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveSmartViewCode";
                cmd.Parameters.AddWithValue("@P_Id", smartviewcode.SmartViewCodeID);
                cmd.Parameters.AddWithValue("@P_SmartViewName", smartviewcode.SmartViewName);
                cmd.Parameters.AddWithValue("@P_SmartViewCode", smartviewcode.SmartViewCode);
                cmd.Parameters.AddWithValue("@P_Active", smartviewcode.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", smartviewcode.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        smartviewcode.SmartViewCodeID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return smartviewcode;
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

        public SmartViewCodeMaster Update(SmartViewCodeMaster smartviewcode)
        {
            throw new NotImplementedException();
        }
    }
}
