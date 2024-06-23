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
    public class ScenarioTypeRepository : BaseRepository, IScenarioTypeRepository
    {
        public ScenarioTypeRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteScenarioTypeMaster";
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
        public IEnumerable<ScenarioType> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllScenarioType";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ScenarioType> obj = new List<ScenarioType>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ScenarioType scenarioType = new ScenarioType();
                    scenarioType.ScenarioTypeID = Convert.ToInt32(results.Rows[i]["ScenarioTypeID"]);
                    scenarioType.ScenarioTypeName = results.Rows[i]["ScenarioTypeName"].ToString();
                    scenarioType.ScenarioTypeCode = results.Rows[i]["ScenarioTypeCode"].ToString();
                    scenarioType.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    scenarioType.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    scenarioType.Status = results.Rows[i]["Status"].ToString();

                    obj.Add(scenarioType);
                }

                //ScenarioTypes source = new ScenarioTypes() { ScenarioTypesID = 1, ScenarioTypesName = "India" };
                //List<ScenarioTypes> obj = new List<ScenarioTypes>();
                //obj.Add(source);
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
        public ScenarioType GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllScenarioType";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new ScenarioType()
                        {
                            ScenarioTypeID = (int)reader["ScenarioTypeID"],
                            ScenarioTypeName = (string)reader["ScenarioTypeName"],
                            ScenarioTypeCode = (string)reader["ScenarioTypeCode"],
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

        public ScenarioType Save(ScenarioType scenarioType)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveScenarioType";
                cmd.Parameters.AddWithValue("@P_Id", scenarioType.ScenarioTypeID);
                cmd.Parameters.AddWithValue("@P_ScenarioTypeName", scenarioType.ScenarioTypeName);
                cmd.Parameters.AddWithValue("@P_ScenarioTypeCode", scenarioType.ScenarioTypeCode);
                cmd.Parameters.AddWithValue("@P_Active", scenarioType.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", scenarioType.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarioType.ScenarioTypeID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return scenarioType;
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

        public ScenarioType Update(ScenarioType scenarioType)
        {
            throw new NotImplementedException();
        }
    }
}
