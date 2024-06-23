using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;
using System.Xml.Linq;


namespace TechnipFMC.Finapp.Data
{
    public class ScenarioRepository : BaseRepository, IScenarioRepository
    {
        public ScenarioRepository()
        { }

        public string Delete(int Id, string DeletedBy)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteScenario";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                cmd.Parameters.AddWithValue("@P_User", DeletedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                string message = "";
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        message = reader.GetString(0);
                        return message;
                        //return true;
                    }
                }
                return message;
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

        public string DeleteMultipleScenario(ScenarioID scenarioIds)
        {
            try
            {
                var xmldata = new XElement("TDS", from ObjDetails in scenarioIds.ScenarioIds
                                                  select new XElement("TD",
                                                 new XElement("ScenarioId", ObjDetails.ToString())));

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteMultipleScenario";
                cmd.Parameters.AddWithValue("@P_User", scenarioIds.CreatedBy);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                DataSet ds = new DataSet();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(ds);
                string message = "";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    message = ds.Tables[0].Rows[0][0].ToString();
                }
                return message;
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

        public bool GetLegacyInsertionStatus()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetLegacyInsertionStatus";

                SqlDataReader reader = cmd.ExecuteReader();
                bool status = false;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        status = reader.GetBoolean(0);
                        return status;
                    }
                }
                return status;
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

        public IEnumerable<Scenario> GetAll(int departmentId, int clientId, string spec)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllScenario";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                cmd.Parameters.AddWithValue("@P_DepartmentId", departmentId);
                cmd.Parameters.AddWithValue("@P_ClientId", clientId);
                cmd.Parameters.AddWithValue("@P_Spec", spec);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Scenario> obj = new List<Scenario>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Scenario scenario = new Scenario();
                    scenario.ScenarioID = Convert.ToInt32(results.Rows[i]["ScenarioID"]);
                    scenario.Spec = results.Rows[i]["Spec"].ToString();
                    scenario.ScenarioScopeCode = results.Rows[i]["ScenarioScopeCode"].ToString();
                    scenario.ScenarioTypeCode = results.Rows[i]["ScenarioTypeCode"].ToString();
                    scenario.FinancialYear = Convert.ToInt32(results.Rows[i]["FinancialYear"]);
                    scenario.ScenarioSequenceNumber = Convert.ToInt32(results.Rows[i]["ScenarioSequenceNumber"]);
                    scenario.ScenarioName = results.Rows[i]["ScenarioName"].ToString();
                    scenario.Description = results.Rows[i]["Description"].ToString();
                    scenario.ScenarioLock = Convert.ToBoolean(results.Rows[i]["ScenarioLock"]);
                    scenario.Milestone = Convert.ToBoolean(results.Rows[i]["Milestone"]);
                    scenario.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    scenario.Status = results.Rows[i]["Status"].ToString();
                    scenario.ScenarioScopeName = results.Rows[i]["ScenarioScopeName"].ToString();
                    scenario.ScenarioTypeName = results.Rows[i]["ScenarioTypeName"].ToString();
                    scenario.DepartmentID = Convert.ToInt32(results.Rows[i]["DepartmentID"]);
                    obj.Add(scenario);
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
        public Scenario GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioById"; //GetScenarioById
                cmd.Parameters.AddWithValue("@P_Id", Id);
                //cmd.Parameters.AddWithValue("@P_DepartmentId", departmentId);
                //cmd.Parameters.AddWithValue("@P_ClientId", clientId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new Scenario()
                        {
                            ScenarioID = (int)reader["ScenarioID"],
                            ScenarioScopeCode = (string)reader["ScenarioScopeCode"],
                            ScenarioTypeCode = (string)reader["ScenarioTypeCode"],
                            Spec = (string)reader["Spec"],
                            FinancialYear = (int)reader["FinancialYear"],
                            ScenarioSequenceNumber = (int)reader["ScenarioSequenceNumber"],
                            ScenarioName = (string)reader["ScenarioName"],
                            Description = (string)reader["Description"],
                            ScenarioLock = (bool)reader["ScenarioLock"],
                            Milestone = (bool)reader["Milestone"],
                            CreatedBy = (int)reader["CreatedBy"],
                            Status = (string)reader["Status"],
                            ScenarioScopeName = (string)reader["ScenarioScopeName"],
                            ScenarioTypeName = (string)reader["ScenarioTypeName"],
                            DepartmentID = (int)reader["DepartmentID"],
                            ClientID = (int)reader["ClientID"],
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

        public Scenario Save(Scenario scenario)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveScenario";
                cmd.Parameters.AddWithValue("@P_Id", scenario.ScenarioID);
                cmd.Parameters.AddWithValue("@P_ScenarioScopeCode", scenario.ScenarioScopeCode);
                cmd.Parameters.AddWithValue("@P_ScenarioTypeCode", scenario.ScenarioTypeCode);
                cmd.Parameters.AddWithValue("@P_FinancialYear", scenario.FinancialYear);
                cmd.Parameters.AddWithValue("@P_Description", scenario.Description);
                cmd.Parameters.AddWithValue("@P_ScenarioLock", scenario.ScenarioLock);
                cmd.Parameters.AddWithValue("@P_CreatedBy", scenario.CreatedBy);
                cmd.Parameters.AddWithValue("@P_DepartmentId", scenario.DepartmentID);
                cmd.Parameters.AddWithValue("@P_ClientId", scenario.ClientID);
                cmd.Parameters.AddWithValue("@P_Milestone", scenario.Milestone);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenario.message = reader.GetString(0);
                        if (Convert.ToInt32(reader.GetInt32(1)) != 0)
                        {
                            scenario.ScenarioID = Convert.ToInt32(reader.GetInt32(1));
                            scenario.ScenarioName = reader.GetString(2);
                        }
                    }
                }
                return scenario;
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

        public IEnumerable<ProjectForScenario> GetAllProjects(int ScenarioID)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllProjectForScenario";
                cmd.Parameters.AddWithValue("@P_ScenarioID", ScenarioID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ProjectForScenario> obj = new List<ProjectForScenario>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ProjectForScenario projectForScenario = new ProjectForScenario();
                    projectForScenario.ProjectId = Convert.ToInt32(results.Rows[i]["ProjectId"]);
                    projectForScenario.ProjectCode = results.Rows[i]["ProjectCode"].ToString();
                    projectForScenario.ProjectName = results.Rows[i]["ProjectName"].ToString();
                    //projectForScenario.ProjectType = results.Rows[i]["ProjectType"].ToString();
                    obj.Add(projectForScenario);
                }

                //Scenarios source = new Scenarios() { ScenariosID = 1, ScenariosName = "India" };
                //List<Scenarios> obj = new List<Scenarios>();
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

        public bool MapScenarioProjects(ProjectScenarioModel projectScenarioModel)
        {
            try
            {
                var xmldata = new XElement("TDS", from ObjDetails in projectScenarioModel.ProjectIds
                                                  select new XElement("TD",
                                                 new XElement("ProjectId", ObjDetails.ToString())));

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MapScenarioToProjects";
                cmd.Parameters.AddWithValue("@P_Id", projectScenarioModel.ScenarioId);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                cmd.Parameters.AddWithValue("@P_CreatedBy", projectScenarioModel.CreatedBy);


                cmd.ExecuteNonQuery();

                return true;
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
        public ScenarioApplicableYears GetApplicableYears(int scenarioId)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioApplicableYears";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new ScenarioApplicableYears()
                        {
                            ScenarioID = reader.GetInt32(0),
                            Year1 = reader.GetInt32(1),
                            Year2 = reader.GetInt32(2),
                            Year3 = reader.GetInt32(3),
                            Year4 = reader.GetInt32(4),
                            Year5 = reader.GetInt32(5),

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

        public string GetScenarioSequence(string scenarioScopeCode, string scenarioTypeCode, int financialYear)
        {
            try
            {
                var scenarioSequence = "";

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioSequence";
                cmd.Parameters.AddWithValue("@P_ScenarioScopeCode", scenarioScopeCode);
                cmd.Parameters.AddWithValue("@P_ScenarioTypeCode", scenarioTypeCode);
                cmd.Parameters.AddWithValue("@P_FinancialYear", financialYear);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarioSequence = reader.GetString(0);
                    }
                }
                return scenarioSequence;
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

        public IEnumerable<Scenario> GetAllScenarioOfProject(int projectid)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllScenarioOfProject";
                cmd.Parameters.AddWithValue("@P_ProjectId", projectid);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Scenario> obj = new List<Scenario>();


                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Scenario scenario = new Scenario();
                    scenario.ScenarioID = Convert.ToInt32(results.Rows[i]["ScenarioID"]);
                    scenario.ScenarioScopeCode = results.Rows[i]["ScenarioScopeCode"].ToString();
                    scenario.ScenarioTypeCode = results.Rows[i]["ScenarioTypeCode"].ToString();
                    scenario.FinancialYear = Convert.ToInt32(results.Rows[i]["FinancialYear"]);
                    scenario.ScenarioSequenceNumber = Convert.ToInt32(results.Rows[i]["ScenarioSequenceNumber"]);
                    scenario.ScenarioName = results.Rows[i]["ScenarioName"].ToString();
                    scenario.Description = results.Rows[i]["Description"].ToString();
                    scenario.ScenarioLock = Convert.ToBoolean(results.Rows[i]["ScenarioLock"]);
                    scenario.Status = results.Rows[i]["Status"].ToString();
                    scenario.ScenarioScopeName = results.Rows[i]["ScenarioScopeName"].ToString();
                    scenario.ScenarioTypeName = results.Rows[i]["ScenarioTypeName"].ToString();
                    obj.Add(scenario);
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

        public Scenario DuplicateScenario(Scenario scenario)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DuplicateScenario";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenario.ScenarioID);
                cmd.Parameters.AddWithValue("@P_ScenarioScopeCode", scenario.ScenarioScopeCode);
                cmd.Parameters.AddWithValue("@P_ScenarioTypeCode", scenario.ScenarioTypeCode);
                cmd.Parameters.AddWithValue("@P_FinancialYear", scenario.FinancialYear);
                cmd.Parameters.AddWithValue("@P_Description", scenario.Description);
                cmd.Parameters.AddWithValue("@P_CreatedBy", scenario.CreatedBy);
                cmd.Parameters.AddWithValue("@P_DepartmentId", scenario.DepartmentID);
                cmd.Parameters.AddWithValue("@P_ClientId", scenario.ClientID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenario.message = reader.GetString(0);
                        if (Convert.ToInt32(reader.GetInt32(1)) != 0)
                        {
                            scenario.ScenarioID = Convert.ToInt32(reader.GetInt32(1));
                            scenario.ScenarioName = reader.GetString(2);
                        }
                    }
                }

                return scenario;
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

        public string RemoveScenarioProjects(ProjectScenarioModel projectScenarioModel)
        {
            try
            {
                var xmldata = new XElement("TDS", from ObjDetails in projectScenarioModel.ProjectIds
                                                  select new XElement("TD",
                                                 new XElement("ProjectId", ObjDetails.ToString())));

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RemoveScenarioProjects";
                cmd.Parameters.AddWithValue("@P_ScenarioId", projectScenarioModel.ScenarioId);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                cmd.Parameters.AddWithValue("@P_CreatedBy", projectScenarioModel.CreatedBy);
                DataSet ds = new DataSet();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(ds);
                string message = "";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    message = ds.Tables[0].Rows[0][0].ToString();
                }
                return message;
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

        public ScenarioProjectLog ChangeScenarionStatus(ScenarioProjectLog scenarioProjectLog)
        {
            try
            {
                int projectId = 0;
                if (scenarioProjectLog.ProjectId.HasValue)
                    projectId = scenarioProjectLog.ProjectId.Value;
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveScenarioProjectLog";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioProjectLog.ScenarioId);
                cmd.Parameters.AddWithValue("@P_ProjectId", projectId);
                cmd.Parameters.AddWithValue("@P_Status", scenarioProjectLog.Status);
                cmd.Parameters.AddWithValue("@P_UserId", scenarioProjectLog.UserId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarioProjectLog.ScenarioProjectLogID = Convert.ToInt32(reader.GetInt32(0));
                        scenarioProjectLog.Status = (reader.GetBoolean(1));
                    }
                }

                return scenarioProjectLog;
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

        public ScenarioProjectLog GetWIPStatus(ScenarioProjectLog scenarioProjectLog)
        {
            try
            {
                int projectId = 0;
                if (scenarioProjectLog.ProjectId.HasValue)
                    projectId = scenarioProjectLog.ProjectId.Value;
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetWIPStatus";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioProjectLog.ScenarioId);
                cmd.Parameters.AddWithValue("@P_ProjectId", projectId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarioProjectLog.ScenarioProjectLogID = Convert.ToInt32(reader.GetInt32(0));
                        scenarioProjectLog.ScenarioId = Convert.ToInt32(reader.GetInt32(1));
                        scenarioProjectLog.ProjectId = Convert.ToInt32(reader.GetInt32(2));
                        scenarioProjectLog.Status = Convert.ToInt32(reader.GetInt32(3)) == 0 ? false : true;
                        scenarioProjectLog.WIPLockedBy  = (reader.GetString(4).Decrypt());
                    }
                }

                return scenarioProjectLog;
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

        public ScenarioProjectLog IsScenarioLockedforProjectUpdate(ScenarioProjectLog scenarioProjectLog)
        {
            try
            {
                int projectId = 0;
                if (scenarioProjectLog.ProjectId.HasValue)
                    projectId = scenarioProjectLog.ProjectId.Value;
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetWIPStatusByScenarioId_ProjectId";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioProjectLog.ScenarioId);
                cmd.Parameters.AddWithValue("@P_ProjectId", projectId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarioProjectLog.ScenarioProjectLogID = Convert.ToInt32(reader.GetInt32(0));
                        scenarioProjectLog.ScenarioId = Convert.ToInt32(reader.GetInt32(1));
                        scenarioProjectLog.ProjectId = Convert.ToInt32(reader.GetInt32(2));
                        scenarioProjectLog.Status = Convert.ToInt32(reader.GetInt32(3)) == 0 ? false : true;
                        //if (scenarioProjectLog.WIPLockedBy== reader.GetString(4))
                        if (scenarioProjectLog.UserId == Convert.ToInt32(reader.GetInt32(5)))
                            scenarioProjectLog.Status = false;
                        scenarioProjectLog.CreatedBy = (reader.GetString(4).Decrypt());
                    }
                }

                return scenarioProjectLog;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public ScenarioProjectLog IsScenarioLockedforUpload(ScenarioProjectLog scenarioProjectLog)
        {
            try
            {
                int projectId = 0;
                if (scenarioProjectLog.ProjectId.HasValue)
                    projectId = scenarioProjectLog.ProjectId.Value;
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetWIPStatusByScenarioId";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioProjectLog.ScenarioId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarioProjectLog.ScenarioProjectLogID = Convert.ToInt32(reader.GetInt32(0));
                        scenarioProjectLog.ScenarioId = Convert.ToInt32(reader.GetInt32(1));
                        scenarioProjectLog.ProjectId = Convert.ToInt32(reader.GetInt32(2));
                        scenarioProjectLog.Status = Convert.ToInt32(reader.GetInt32(3)) == 0 ? false : true;
                        scenarioProjectLog.WIPLockedBy = (reader.GetString(4).Decrypt());
                    }
                }

                return scenarioProjectLog;
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

        public List<ScenarioIDS> GetAllWIPStatusTrueScenario()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllWIPStatusTrueScenario";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ScenarioIDS> obj = new List<ScenarioIDS>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ScenarioIDS scenario = new ScenarioIDS();
                    scenario.ScenarioID = Convert.ToInt32(results.Rows[i]["ScenarioID"]);
                    scenario.ScenarioName = results.Rows[i]["ScenarioName"].ToString();
                    scenario.CreatedBy = results.Rows[i]["CreatedBy"].ToString().Decrypt();
                    obj.Add(scenario);
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

        public List<Scenario> GetScenarioByYear(int year)
        {
            try
            {
                List<Scenario> scenarios = new List<Scenario>();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioByYear";
                cmd.Parameters.AddWithValue("@P_Year", year);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarios.Add(new Scenario()
                        {
                            ScenarioID = (int)reader["ScenarioID"],
                            ScenarioScopeCode = (string)reader["ScenarioScopeCode"],
                            ScenarioTypeCode = (string)reader["ScenarioTypeCode"],
                            FinancialYear = (int)reader["FinancialYear"],
                            ScenarioSequenceNumber = (int)reader["ScenarioSequenceNumber"],
                            ScenarioName = (string)reader["ScenarioName"],
                            Spec = (string)reader["Spec"],
                            Description = (string)reader["Description"],
                            ScenarioLock = (bool)reader["ScenarioLock"],
                            CreatedBy = (int)reader["CreatedBy"],
                            Status = (string)reader["Status"],
                            ScenarioScopeName = (string)reader["ScenarioScopeName"],
                            ScenarioTypeName = (string)reader["ScenarioTypeName"],
                        }
                        ); ;

                    }
                }
                return scenarios;
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
        public List<Scenario> GetOrgScenarioByYear(int year)
        {
            try
            {
                List<Scenario> scenarios = new List<Scenario>();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetOrgScenarioByYear";
                cmd.Parameters.AddWithValue("@P_Year", year);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarios.Add(new Scenario()
                        {
                            ScenarioID = (int)reader["ScenarioID"],
                            ScenarioScopeCode = (string)reader["ScenarioScopeCode"],
                            ScenarioTypeCode = (string)reader["ScenarioTypeCode"],
                            FinancialYear = (int)reader["FinancialYear"],
                            ScenarioSequenceNumber = (int)reader["ScenarioSequenceNumber"],
                            ScenarioName = (string)reader["ScenarioName"],
                            Spec = (string)reader["Spec"],
                            Description = (string)reader["Description"],
                            ScenarioLock = (bool)reader["ScenarioLock"],
                            CreatedBy = (int)reader["CreatedBy"],
                            Status = (string)reader["Status"],
                            ScenarioScopeName = (string)reader["ScenarioScopeName"],
                            ScenarioTypeName = (string)reader["ScenarioTypeName"],
                        }
                        ); ;

                    }
                }
                return scenarios;
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

        public List<Scenario> GetScenarioByScenarioId(int scenarioId, int year)
        {
            try
            {
                List<Scenario> scenarios = new List<Scenario>();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioByScenarioId";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);
                cmd.Parameters.AddWithValue("@P_Year", year);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarios.Add(new Scenario()
                        {
                            ScenarioID = (int)reader["ScenarioID"],
                            ScenarioScopeCode = (string)reader["ScenarioScopeCode"],
                            ScenarioTypeCode = (string)reader["ScenarioTypeCode"],
                            FinancialYear = (int)reader["FinancialYear"],
                            ScenarioSequenceNumber = (int)reader["ScenarioSequenceNumber"],
                            ScenarioName = (string)reader["ScenarioName"],
                            Description = (string)reader["Description"],
                            ScenarioLock = (bool)reader["ScenarioLock"],
                            CreatedBy = (int)reader["CreatedBy"],
                            Status = (string)reader["Status"],
                            ScenarioScopeName = (string)reader["ScenarioScopeName"],
                            ScenarioTypeName = (string)reader["ScenarioTypeName"],
                        }
                        ); ;

                    }
                }
                return scenarios;
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
        public List<Scenario> GetScenariosByYearAndScope(DashboardConfig config)
        {
            try
            {
                List<Scenario> scenarios = new List<Scenario>();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioByYearAndScope";
                cmd.Parameters.AddWithValue("@P_Year", config.Year);
                cmd.Parameters.AddWithValue("@P_Scope", config.Scope);
                cmd.Parameters.AddWithValue("@P_Spec", config.Spec);
                cmd.Parameters.AddWithValue("@P_Department", config.DepartmentId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        scenarios.Add(new Scenario()
                        {
                            ScenarioID = (int)reader["ScenarioID"],
                            ScenarioName = (string)reader["ScenarioName"],
                        }
                        ); 

                    }
                }
                return scenarios;
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
