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
    public class ContractStatusRepository : BaseRepository, IContractStatusRepository
    {
        public ContractStatusRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteContractStatusMaster";
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
        public IEnumerable<ContractStatus> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllContractStatus";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ContractStatus> obj = new List<ContractStatus>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ContractStatus contractstatus = new ContractStatus();
                    contractstatus.ContractStatusID = Convert.ToInt32(results.Rows[i]["ContractStatusID"]);
                    contractstatus.ContractStatusName = results.Rows[i]["ContractStatusName"].ToString();
                    contractstatus.ContractStatusCode = results.Rows[i]["ContractStatusCode"].ToString();
                    contractstatus.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    contractstatus.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    contractstatus.Status = results.Rows[i]["Status"].ToString();

                    obj.Add(contractstatus);
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
        public ContractStatus GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllContractStatus";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new ContractStatus()
                        {
                            ContractStatusID = (int)reader["ContractStatusID"],
                            ContractStatusName = (string)reader["ContractStatusName"],
                            ContractStatusCode = (string)reader["ContractStatusCode"],
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

        public ContractStatus Save(ContractStatus contractstatus)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveContractStatus";
                cmd.Parameters.AddWithValue("@P_Id", contractstatus.ContractStatusID);
                cmd.Parameters.AddWithValue("@P_ContractStatusName", contractstatus.ContractStatusName);
                cmd.Parameters.AddWithValue("@P_ContractStatusCode", contractstatus.ContractStatusCode);
                cmd.Parameters.AddWithValue("@P_Active", contractstatus.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", contractstatus.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        contractstatus.ContractStatusID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return contractstatus;
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

        public ContractStatus Update(ContractStatus ContractStatuss)
        {
            throw new NotImplementedException();
        }
    }
}
