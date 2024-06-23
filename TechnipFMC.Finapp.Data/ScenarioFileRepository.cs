using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using TechnipFMC.Common;
using System.Linq;

namespace TechnipFMC.Finapp.Data
{
    public class ScenarioFileRepository : BaseRepository, IScenarioFileRepository
    {
        public ScenarioFileRepository()
        {

        }

        public void UploadProjectScenarioDataType_1(string activeQuarters, XElement xml)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //base.DBConnection.Open();

                cmd.CommandText = "UploadProjectScenarioDataType_1_New";
                 cmd.Parameters.AddWithValue("@P_Xml", xml.ToString());
                cmd.Parameters.AddWithValue("@P_ActiveQuarters", activeQuarters);

                SqlDataReader reader = cmd.ExecuteReader();
                //base.DBConnection.Close();
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

        public List<TemplateConfiguration> GetTemplateConfigurations(int typeId)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTemplateConfigurations";
                cmd.Parameters.AddWithValue("@P_Type", typeId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                //base.DBConnection.Close();

                var configs = new List<TemplateConfiguration>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    configs.Add(new TemplateConfiguration
                    {
                        Id = Convert.ToInt32(results.Rows[i]["Id"]),
                        FieldName = results.Rows[i]["FieldName"].ToString(),
                        Quarter = results.Rows[i]["Quarter"].ToString(),
                        ExcelCellPosition = results.Rows[i]["ExcelCellPosition"].ToString(),
                        ScenarioDataTypeId = Convert.ToInt32(results.Rows[i]["ScenarioDataTypeId"]),
                        Year = results.Rows[i]["Year"].ToString()
                    });
                }
                return configs;
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

        public bool SaveScenarioFile(ScenarioFile scenarioFile, string createdBy)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
               // base.DBConnection.Open();
                cmd.CommandText = "SaveScenarioFiles";
                cmd.Parameters.AddWithValue("@P_UploadSessionId", scenarioFile.UploadSessionId);
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioFile.ScenarioId);
                cmd.Parameters.AddWithValue("@P_ScenarioFileName", scenarioFile.ScenarioFileName);
                cmd.Parameters.AddWithValue("@P_FilePath", scenarioFile.FilePath);
                cmd.Parameters.AddWithValue("@P_TypeId", scenarioFile.TypeId);
                cmd.Parameters.AddWithValue("@P_CreatedBy", createdBy);
                cmd.Parameters.AddWithValue("@P_Year", scenarioFile.Year);
                cmd.Parameters.AddWithValue("@P_Quarter", scenarioFile.Quarter);
                SqlDataReader reader = cmd.ExecuteReader();
               // base.DBConnection.Close();
                return true;
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

        public void UploadProjectScenarioDataType_2(string activeQuarter, XElement xml)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //base.DBConnection.Open();

                cmd.CommandText = "UploadProjectScenarioDataType_2_New";
                cmd.Parameters.AddWithValue("@P_ActiveQuarter", activeQuarter);
                cmd.Parameters.AddWithValue("@P_Xml", xml.ToString());

                SqlDataReader reader = cmd.ExecuteReader();
                //base.DBConnection.Close();
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

        public List<ScenarioUploadLog> GetScenarioErrorlog(string sessionid)
        {
            List<ScenarioUploadLog> scenarioUploadLogs = null;
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioErrorLog";
                cmd.Parameters.AddWithValue("@P_SessionId", sessionid);
                cmd.Parameters.AddWithValue("@P_Type", 1);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    scenarioUploadLogs = ds.Tables[0].ToListOfObject<ScenarioUploadLog>().ToList();
                }

                return scenarioUploadLogs;
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

        public List<ScenarioUploadLog> GetScenarioErrorlogByScenarioId(int ScenarioId)
        {
            List<ScenarioUploadLog> scenarioUploadLogs = null;
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioErrorLogBySessionId";
                cmd.Parameters.AddWithValue("@P_ScenarioId", ScenarioId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    scenarioUploadLogs = ds.Tables[0].ToListOfObject<ScenarioUploadLog>().ToList();
                }

                return scenarioUploadLogs;
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

        public List<ScenarioMappedProjects> GetScenarioMappedProjects(int scenarioId)
        {
            var scenarioprojects = new List<ScenarioMappedProjects>();
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioMappedProjects";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    scenarioprojects = ds.Tables[0].ToListOfObject<ScenarioMappedProjects>().ToList();
                }

                return scenarioprojects;
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

        public bool SaveScenarioUploadLog(XElement xml)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveFileUploadLog";
                cmd.Parameters.AddWithValue("@P_Xml", xml.ToString());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();
                return true;
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

        public List<FileUploadLayout> GetFileUploadLayouts(int scenarioId)
        {
            try
            {
                var financialYears = new List<int>();
                var yearLocks = new List<LockYear>();
                var quarterLocks = new List<LockQuarter>();
                var quarterDivs = new List<String>();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetFileUploadScenarioLayout";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    for (var row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        financialYears.Add(Convert.ToInt32(ds.Tables[0].Rows[row]["value"]));
                    }
                }
                if ((ds != null) && (ds.Tables.Count > 1) && (ds.Tables[1] != null) && (ds.Tables[1].Rows.Count > 0))
                    yearLocks = ds.Tables[1].ToListOfObject<LockYear>().ToList();
                if ((ds != null) && (ds.Tables.Count > 2) && (ds.Tables[2] != null) && (ds.Tables[2].Rows.Count > 0))
                    quarterLocks = ds.Tables[2].ToListOfObject<LockQuarter>().ToList();

                var financilaYear = 0;
                var scenarioScopeCode = "";
                var scenarioTypeCode = "";
                var scenarioName = "";

                if ((ds != null) && (ds.Tables.Count > 3) && (ds.Tables[3] != null) && (ds.Tables[3].Rows.Count > 0))
                {
                    financilaYear = Convert.ToInt32(ds.Tables[3].Rows[0][2]);
                    scenarioScopeCode = Convert.ToString(ds.Tables[3].Rows[0][0]);
                    scenarioTypeCode = Convert.ToString(ds.Tables[3].Rows[0][1]);
                    scenarioName = Convert.ToString(ds.Tables[3].Rows[0][3]);
                }

                var scenarioFileUploadLayout = new List<FileUploadLayout>();

                if (scenarioScopeCode == "PL" && scenarioTypeCode == "AC")
                {
                    var quarters = quarterLocks.Where(q => q.Year == financilaYear).ToList();
                    var yearLock = (yearLocks.Select(y => y.Year).Contains(financilaYear)) ?
                        yearLocks.First(y => y.Year == financilaYear).Lock : false;

                    scenarioFileUploadLayout.Add(new FileUploadLayout()
                    {
                        ScenarioName = scenarioName,
                        Quarters = quarters,
                        Year = financilaYear,
                        YearLock = yearLock
                    });
                }
                else
                {
                    foreach (var year in financialYears)
                    {
                        var quarters = quarterLocks.Where(q => q.Year == year).ToList();
                        var yearLock = (yearLocks.Select(y => y.Year).Contains(year)) ? yearLocks.First(y => y.Year == year).Lock : false;

                        var layout = new FileUploadLayout()
                        {
                            ScenarioName = scenarioName,
                            Quarters = quarters,
                            Year = year,
                            YearLock = yearLock,
                        };
                        scenarioFileUploadLayout.Add(layout);
                    }
                }

                return scenarioFileUploadLayout;
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
        public List<ScenarioFile> GetScenariouploadlog()
        {
            List<ScenarioFile> scenarioUploadLogs = null;
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioFileUploadLogs";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    scenarioUploadLogs = ds.Tables[0].ToListOfObject<ScenarioFile>().ToList();
                }

                return scenarioUploadLogs;
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

        public List<ScenarioUploadLog> GetScenarioFailedlog(string sessionid)
        {
            List<ScenarioUploadLog> scenarioUploadLogs = null;
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioErrorLog";
                cmd.Parameters.AddWithValue("@P_SessionId", sessionid);
                cmd.Parameters.AddWithValue("@P_Type", 2);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    scenarioUploadLogs = ds.Tables[0].ToListOfObject<ScenarioUploadLog>().ToList();
                }

                return scenarioUploadLogs;
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

        public string GetuploadToken(string uploadsessionId,string CreatedBy, int secondtime)
        {
            try
            {
                string token = "";
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetUploadToken";
                cmd.Parameters.AddWithValue("@P_SessionId", uploadsessionId);
                cmd.Parameters.AddWithValue("@P_UserId", CreatedBy);
                cmd.Parameters.AddWithValue("@P_SecondTime", secondtime);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                    token = ds.Tables[0].Rows[0][0].ToString();
                return token;
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

        public ScenarioFile GetuploadFile(string token)
        {
            try
            {
                ScenarioFile obj = new ScenarioFile();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetUploadFile";
                cmd.Parameters.AddWithValue("@P_GUId", token);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                    obj = ds.Tables[0].ToObject<ScenarioFile>();
                return obj;
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
    }
}
