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
    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository()
        { }
        public bool Delete(int Id, int DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteClientMaster";
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

        public IEnumerable<Client> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllClient";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                //cmd.Parameters.AddWithValue("@P_DepartmentId", departmentId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Client> obj = new List<Client>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Client client = new Client();
                    client.ClientID = Convert.ToInt32(results.Rows[i]["ClientID"]);
                    //client.DepartmentID = Convert.ToInt32(results.Rows[i]["DepartmentID"]);
                    client.ClientName = results.Rows[i]["ClientName"].ToString().Decrypt();
                    client.ClientCode = results.Rows[i]["ClientCode"].ToString();
                    client.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    client.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(client);
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
        public IEnumerable<Client> GetAllClientsofUser(int userid)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllClientsofUser";
                cmd.Parameters.AddWithValue("@P_UserId", userid);
                //cmd.Parameters.AddWithValue("@P_DepartmentId", departmentId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Client> obj = new List<Client>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Client client = new Client();
                    client.ClientID = Convert.ToInt32(results.Rows[i]["ClientID"]);
                    //client.DepartmentID = Convert.ToInt32(results.Rows[i]["DepartmentID"]);
                    client.ClientName = results.Rows[i]["ClientName"].ToString().Decrypt();
                    client.ClientCode = results.Rows[i]["ClientCode"].ToString();
                    client.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    client.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(client);
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

        public Client GetById(int Id)//, int departmentId
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCLient";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                //cmd.Parameters.AddWithValue("@P_DepartmentId", departmentId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new Client()
                        {
                            ClientID = (int)reader["ClientID"],
                            //DepartmentID = (int)reader["DepartmentID"],
                            ClientName = ((string)reader["ClientName"]).Decrypt(),
                            ClientCode = (string)reader["ClientCode"],
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

        public Client Save(Client client)
        {
            try
            {
                client.ClientName = client.ClientName.Encrypt();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveClient";
                cmd.Parameters.AddWithValue("@P_Id", client.ClientID);
                //cmd.Parameters.AddWithValue("@P_Id", client.DepartmentID);
                cmd.Parameters.AddWithValue("@P_ClientName", client.ClientName);
                cmd.Parameters.AddWithValue("@P_ClientCode", client.ClientCode);
                cmd.Parameters.AddWithValue("@P_Active", client.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", client.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        client.ClientID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return client;
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

        public Client Update(Client Client)
        {
            throw new NotImplementedException();
        }
    }
}
