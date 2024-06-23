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
using TechnipFMC.Common;

namespace TechnipFMC.Finapp.Data
{
    public class FinancialDataTypeScenarioRepository : BaseRepository, IFinancialDataTypeScenarioRepository
    {
        public FinancialDataTypeScenarioRepository()
        { }
        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteFinancialDataTypeScenario";
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

        public IEnumerable<FinancialDataTypesScenario> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllFinancialDataTypeScenario";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<FinancialDataTypesScenario> obj = new List<FinancialDataTypesScenario>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    FinancialDataTypesScenario financialDataTypesScenario = new FinancialDataTypesScenario();
                    financialDataTypesScenario.ScenarioDataTypeID = Convert.ToInt32(results.Rows[i]["ScenarioDataTypeID"]);
                    financialDataTypesScenario.ScenarioScopeID = Convert.ToInt32(results.Rows[i]["ScenarioScopeID"]);
                    financialDataTypesScenario.ScenarioScopeCode = results.Rows[i]["ScenarioScopeCode"].ToString();
                    financialDataTypesScenario.ScenarioTypeID = Convert.ToInt32(results.Rows[i]["ScenarioTypeID"]);
                    financialDataTypesScenario.ScenarioTypeCode = results.Rows[i]["ScenarioTypeCode"].ToString();
                    financialDataTypesScenario.FinancialDataTypeID = Convert.ToInt32(results.Rows[i]["FinancialDataTypeID"]);
                    financialDataTypesScenario.FinancialDataTypeCode = results.Rows[i]["FinancialDataTypeCode"].ToString();
                    financialDataTypesScenario.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    financialDataTypesScenario.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    financialDataTypesScenario.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(financialDataTypesScenario);
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
        public IEnumerable<ScenarioScopeTypes> GetAllScopeTypes(string financialTypeCode)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllScopeTypesOfFinancialTypeCode";
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeCode", financialTypeCode);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                base.DBConnection.Close();
                var scopeTypes = new ScenarioScopeTypes();
                List<ScenarioScopeTypes> obj = new List<ScenarioScopeTypes>();
                List<FinancialDataTypesScenario> FinancialDataTypesScenarios = new List<FinancialDataTypesScenario>();
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    var data = ds.Tables[0].ToListOfObject<FinancialTypesScenario>();

                    var distinctScenarioScope = data.Select(a => a.ScenarioScopeCode).Distinct();
                    foreach (var item in distinctScenarioScope)
                    {
                        scopeTypes.ScenarioScopeCode = item;
                        scopeTypes.ScenarioScopeName = data.FirstOrDefault(a => a.ScenarioScopeCode == item).ScenarioScopeName;

                        scopeTypes.Budget = false;
                        scopeTypes.Forecast = false;
                        scopeTypes.Actuals = false;
                        var bdData = data.FirstOrDefault(a => a.ScenarioScopeCode == item && a.ScenarioTypeCode == "BD");
                        if (bdData != null)
                            scopeTypes.Budget = true;
                        var fcData = data.FirstOrDefault(a => a.ScenarioScopeCode == item && a.ScenarioTypeCode == "FC");
                        if (fcData != null)
                            scopeTypes.Forecast = true;
                        var acData = data.FirstOrDefault(a => a.ScenarioScopeCode == item && a.ScenarioTypeCode == "AC");
                        if (acData != null)
                            scopeTypes.Actuals = true;
                        obj.Add(new ScenarioScopeTypes
                        {
                            ScenarioScopeCode = scopeTypes.ScenarioScopeCode,
                            ScenarioScopeName = scopeTypes.ScenarioScopeName,
                            Budget = scopeTypes.Budget,
                            Forecast = scopeTypes.Forecast,
                            Actuals = scopeTypes.Actuals,
                        });
                    }
                }
                //if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                //{
                //    var tempScope = ds.Tables[0].Rows[0]["ScenarioScopeCode"].ToString();
                //    for (var row = 0; row < ds.Tables[0].Rows.Count; row++)
                //    {
                //        scopeTypes.ScenarioScopeCode = ds.Tables[0].Rows[row]["ScenarioScopeCode"].ToString();
                //        scopeTypes.ScenarioScopeName = ds.Tables[0].Rows[row]["ScenarioScopeName"].ToString();
                //        scopeTypes.Budget = false;
                //        scopeTypes.Forecast = false;
                //        scopeTypes.Actuals = false;

                //        if (tempScope == scopeTypes.ScenarioScopeCode)
                //        {
                //            if (Convert.ToString(ds.Tables[0].Rows[row][3]) == "BD")
                //            {
                //                scopeTypes.Budget = true;
                //            }
                //            else if (Convert.ToString(ds.Tables[0].Rows[row][3]) == "FC")
                //            {
                //                scopeTypes.Forecast = true;
                //            }
                //            else if (Convert.ToString(ds.Tables[0].Rows[row][3]) == "AC")
                //            {
                //                scopeTypes.Actuals = true;
                //            }
                //        }
                //        else
                //        {
                //            obj.Add(new ScenarioScopeTypes
                //            {
                //                ScenarioScopeCode = scopeTypes.ScenarioScopeCode,
                //                ScenarioScopeName = scopeTypes.ScenarioScopeName,
                //                Budget = scopeTypes.Budget,
                //                Forecast = scopeTypes.Forecast,
                //                Actuals = scopeTypes.Actuals,
                //            });
                //        }
                //        tempScope = scopeTypes.ScenarioScopeCode;
                //    }
                //}
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
        public List<string> GetAllFinancialDataTypes(string scope,string type)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllFinancialDataTypesMapped";
                cmd.Parameters.AddWithValue("@P_Scope", scope);
                cmd.Parameters.AddWithValue("@P_Type", type);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                base.DBConnection.Close();
                var scopeTypes = new ScenarioScopeTypes();
                List<FinancialDataTypes> obj = new List<FinancialDataTypes>();
                List<FinancialDataTypesScenario> FinancialDataTypesScenarios = new List<FinancialDataTypesScenario>();
                //if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                //{
                //    obj = ds.Tables[0].Enumerable<string>();                    
                //}
                List<string> financialCodes = new List<string>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    financialCodes.Add(row["FinancialDataTypeCode"].ToString());
                }
                return financialCodes;
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

        public FinancialDataTypesScenario GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllFinancialDataTypeScenario";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new FinancialDataTypesScenario()
                        {
                            ScenarioDataTypeID = (int)reader["ScenarioDataTypeID"],
                            ScenarioScopeID = (int)reader["ScenarioScopeID"],
                            ScenarioScopeCode = (string)reader["ScenarioScopeCode"],
                            ScenarioTypeID = (int)reader["ScenarioTypeID"],
                            ScenarioTypeCode = (string)reader["ScenarioTypeCode"],
                            FinancialDataTypeID = (int)reader["FinancialDataTypeID"],
                            FinancialDataTypeCode = (string)reader["FinancialDataTypeCode"],
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

        public bool SaveScenarioDataPoints(ScenarioDataPoints scenarioDataPoints)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveScenarioDataPoints";
                var xmldata = new XElement("TDS", from ObjDetails in scenarioDataPoints.FinancialDataType
                                                  select new XElement("TD",
                                                 new XElement("FinancialDataTypeID", ObjDetails.FinancialDataTypeID),
                                                 new XElement("FinancialDataTypeCode", ObjDetails.FinancialDataTypeCode)));
                cmd.Parameters.AddWithValue("@P_ScenarioScopeID", scenarioDataPoints.ScenarioScopeID);
                cmd.Parameters.AddWithValue("@P_ScenarioScopeCode", scenarioDataPoints.ScenarioScopeCode);
                cmd.Parameters.AddWithValue("@P_ScenarioTypeID", scenarioDataPoints.ScenarioTypeID);
                cmd.Parameters.AddWithValue("@P_ScenarioTypeCode", scenarioDataPoints.ScenarioTypeCode);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                cmd.Parameters.AddWithValue("@P_CreatedBy", scenarioDataPoints.CreatedBy);
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
        public bool SaveFinancialScenarioDataPoints(FinancialScenarioDataPoints scenarioDataPoints)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveFinancialScenarioDataPoints";

                var xmldata = new XElement("TDS", from ObjDetails in scenarioDataPoints.ScenarioScopeType
                                                  select new XElement("TD",
                                                  new XElement("ScenarioScopeCode", ObjDetails.ScenarioScopeCode),
                                                  new XElement("ScenarioScopeId", ObjDetails.ScenarioScopeId),
                                                 new XElement("Budget", ObjDetails.Budget),
                                                 new XElement("Forecast", ObjDetails.Forecast),
                                                 new XElement("Actuals", ObjDetails.Actuals)));
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeID", scenarioDataPoints.FinancialDataTypeID);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeCode", scenarioDataPoints.FinancialDataTypeCode);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                cmd.Parameters.AddWithValue("@P_CreatedBy", scenarioDataPoints.CreatedBy);
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

        public ScenarioDataPoints Update(ScenarioDataPoints ScenarioDataPoints)
        {
            throw new NotImplementedException();
        }
    }
}
