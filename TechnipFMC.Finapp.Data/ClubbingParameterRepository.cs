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
    public class ClubbingParameterRepository : BaseRepository, IClubbingParameterRepository
    {
        public ClubbingParameterRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteClubbingParameterMaster";
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
        public IEnumerable<ClubbingParameter> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllClubbingParameter";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ClubbingParameter> obj = new List<ClubbingParameter>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ClubbingParameter clubbingParameter = new ClubbingParameter();
                    clubbingParameter.ClubbingParameterID = Convert.ToInt32(results.Rows[i]["ClubbingParameterID"]);
                    clubbingParameter.ClubbingParameterName = results.Rows[i]["ClubbingParameterName"].ToString();
                    clubbingParameter.ClubbingParameterCode = results.Rows[i]["ClubbingParameterCode"].ToString();
                    clubbingParameter.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    clubbingParameter.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    clubbingParameter.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(clubbingParameter);
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
        public ClubbingParameter GetById(int Id)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllClubbingParameter";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new ClubbingParameter()
                        {
                            ClubbingParameterID = (int)reader["ClubbingParameterID"],
                            ClubbingParameterName = (string)reader["ClubbingParameterName"],
                            ClubbingParameterCode = (string)reader["ClubbingParameterCode"],
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

        public ClubbingParameter Save(ClubbingParameter clubbingParameter)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveClubbingParameter";
                cmd.Parameters.AddWithValue("@P_Id", clubbingParameter.ClubbingParameterID);
                cmd.Parameters.AddWithValue("@P_ClubbingParameterName", clubbingParameter.ClubbingParameterName);
                cmd.Parameters.AddWithValue("@P_ClubbingParameterCode", clubbingParameter.ClubbingParameterCode);
                cmd.Parameters.AddWithValue("@P_Active", clubbingParameter.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", clubbingParameter.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clubbingParameter.ClubbingParameterID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return clubbingParameter;
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

        public ClubbingParameter Update(ClubbingParameter ClubbingParameters)
        {
            throw new NotImplementedException();
        }
    }
}
