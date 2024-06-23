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
    public class ContractTypeRepository : BaseRepository, IContractTypeRepository
    {
        public ContractTypeRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteContractTypeMaster";
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
        public IEnumerable<ContractType> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllContractType";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ContractType> obj = new List<ContractType>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ContractType contractType = new ContractType();
                    contractType.ContractTypeID = Convert.ToInt32(results.Rows[i]["ContractTypeID"]);
                    contractType.ContractTypeName = results.Rows[i]["ContractTypeName"].ToString();
                    contractType.ContractTypeCode = results.Rows[i]["ContractTypeCode"].ToString();
                    contractType.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    contractType.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    contractType.Status = results.Rows[i]["Status"].ToString();

                    obj.Add(contractType);
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
        public ContractType GetById(int Id)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllContractType";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new ContractType()
                        {
                            ContractTypeID = (int)reader["ContractTypeID"],
                            ContractTypeName = (string)reader["ContractTypeName"],
                            ContractTypeCode = (string)reader["ContractTypeCode"],
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

        public ContractType Save(ContractType contracttype)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveContractType";
                cmd.Parameters.AddWithValue("@P_Id", contracttype.ContractTypeID);
                cmd.Parameters.AddWithValue("@P_ContractTypeName", contracttype.ContractTypeName);
                cmd.Parameters.AddWithValue("@P_ContractTypeCode", contracttype.ContractTypeCode);
                cmd.Parameters.AddWithValue("@P_Active", contracttype.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", contracttype.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        contracttype.ContractTypeID = Convert.ToInt32(reader.GetInt32(0));
                        return contracttype;
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

        public ContractType Update(ContractType contracttype)
        {
            throw new NotImplementedException();
        }
    }
}
