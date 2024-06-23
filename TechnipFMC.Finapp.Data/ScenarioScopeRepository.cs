using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public class ScenarioScopeRepository : BaseRepository, IScenarioScopeRepository
    {
        public ScenarioScopeRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteScenarioScopeMaster";
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
        public IEnumerable<ScenarioScope> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllScenarioScope";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ScenarioScope> obj = new List<ScenarioScope>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ScenarioScope scenarioScope = new ScenarioScope();
                    scenarioScope.ScenarioScopeID = Convert.ToInt32(results.Rows[i]["ScenarioScopeID"]);
                    scenarioScope.ScenarioScopeName = results.Rows[i]["ScenarioScopeName"].ToString();
                    scenarioScope.ScenarioScopeCode = results.Rows[i]["ScenarioScopeCode"].ToString();
                    scenarioScope.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    scenarioScope.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    scenarioScope.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(scenarioScope);
                }

                //ScenarioScopes source = new ScenarioScopes() { ScenarioScopesID = 1, ScenarioScopesName = "India" };
                //List<ScenarioScopes> obj = new List<ScenarioScopes>();
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
        public ScenarioScope GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllScenarioScope";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new ScenarioScope()
                        {
                            ScenarioScopeID = (int)reader["ScenarioScopeID"],
                            ScenarioScopeName = (string)reader["ScenarioScopeName"],
                            ScenarioScopeCode = (string)reader["ScenarioScopeCode"],
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

        public ScenarioScope Save(ScenarioScope scenarioScope)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveScenarioScope";
                cmd.Parameters.AddWithValue("@P_Id", scenarioScope.ScenarioScopeID);
                cmd.Parameters.AddWithValue("@P_ScenarioScopeName", scenarioScope.ScenarioScopeName);
                cmd.Parameters.AddWithValue("@P_ScenarioScopeCode", scenarioScope.ScenarioScopeCode);
                cmd.Parameters.AddWithValue("@P_Active", scenarioScope.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", scenarioScope.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarioScope.ScenarioScopeID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return scenarioScope;
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

        public ScenarioScope Update(ScenarioScope scenarioScope)
        {
            throw new NotImplementedException();
        }
    }
}
