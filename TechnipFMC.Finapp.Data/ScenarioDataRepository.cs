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
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Reflection;

namespace TechnipFMC.Finapp.Data
{
    public class ScenarioDataRepository : BaseRepository, IScenarioDataRepository
    {
        public ScenarioDataRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {

            throw new NotImplementedException();
            //SqlCommand cmd = base.DBConnection.CreateCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "DeleteScenarioData";
            //cmd.Parameters.AddWithValue("@P_Id", Id);
            //cmd.Parameters.AddWithValue("@P_User", DeletedBy);
            //SqlDataReader reader = cmd.ExecuteReader();
            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        return true;
            //    }
            //}

            //return true;
        }
        public int ClearScenarioData(int ScenarioId, int DeletedBy)
        {
            SqlCommand cmd = base.DBConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClearScenarioData";
            cmd.Parameters.AddWithValue("@P_ScenarioId", ScenarioId);
            cmd.Parameters.AddWithValue("@P_User", DeletedBy);
            SqlDataReader reader = cmd.ExecuteReader();
            int deletesuccess = 0;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    deletesuccess = Convert.ToInt32(reader.GetInt32(0));
                }
            }

            return deletesuccess;
        }
        public bool ClearProjectScenarioData(ScenarioProjectMapper scenarioProject)
        {
            SqlCommand cmd = base.DBConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClearProjectScenarioData";
            cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioProject.ScenarioID);
            cmd.Parameters.AddWithValue("@P_ProjectId", scenarioProject.ProjectID);
            cmd.Parameters.AddWithValue("@P_User", scenarioProject.UpdatedBy);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return true;
                }
            }

            return true;
        }
        public IEnumerable<YearlyScenarioData> GetAll(int projectId, int scenarioId)
        {
            try
            {
                var financialYears = new List<int>();
                var yearLocks = new List<LockYear>();
                var quarterLocks = new List<LockQuarter>();
                var scenarioDatasRaw = new List<ScenarioDataList>(); 
                var CurrentFinancialYear = new int();
                var comments = "";
                bool editable = true;
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllScenarioData";
                cmd.Parameters.AddWithValue("@P_ProjectId", projectId);
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    for (var row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        financialYears.Add(Convert.ToInt32(ds.Tables[0].Rows[row]["value"]));
                    }

                    CurrentFinancialYear = Convert.ToInt32(ds.Tables[0].Rows[0]["value"]);
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null))
                    yearLocks = ds.Tables[1].ToListOfObject<LockYear>().ToList();
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[2] != null))
                    quarterLocks = ds.Tables[2].ToListOfObject<LockQuarter>().ToList();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[3] != null))
                {
                    for (var row = 0; row < ds.Tables[3].Rows.Count; row++)
                    {
                        scenarioDatasRaw.Add(new ScenarioDataList
                        {
                            FinancialDataTypeName = ds.Tables[3].Rows[row]["ScenarioDataType"].ToString(),
                            ProjectId = projectId,
                            ScenarioId = scenarioId,
                            ScenarioDataTypeId = (ds.Tables[3].Rows[row]["ScenarioDataTypeId"] != null) ? Convert.ToInt32(ds.Tables[3].Rows[row]["ScenarioDataTypeId"]) : 0,
                            Year = (ds.Tables[3].Rows[row]["Year"] != null) ? Convert.ToInt32(ds.Tables[3].Rows[row]["Year"]) : 0,

                            Q1New = (ds.Tables[3].Rows[row]["Q1New"] != null) ? ds.Tables[3].Rows[row]["Q1New"].ToString() : string.Empty,
                            Q1Variant = (ds.Tables[3].Rows[row]["Q1Variant"] != null) ? ds.Tables[3].Rows[row]["Q1Variant"].ToString() : string.Empty,

                            Q2New = (ds.Tables[3].Rows[row]["Q2New"] != null) ? ds.Tables[3].Rows[row]["Q2New"].ToString() : string.Empty,
                            Q2Variant = (ds.Tables[3].Rows[row]["Q2Variant"] != null) ? ds.Tables[3].Rows[row]["Q2Variant"].ToString() : string.Empty,

                            Q3New = (ds.Tables[3].Rows[row]["Q3New"] != null) ? ds.Tables[3].Rows[row]["Q3New"].ToString() : string.Empty,
                            Q3Variant = (ds.Tables[3].Rows[row]["Q3Variant"] != null) ? ds.Tables[3].Rows[row]["Q3Variant"].ToString() : string.Empty,

                            Q4New = (ds.Tables[3].Rows[row]["Q4New"] != null) ? ds.Tables[3].Rows[row]["Q4New"].ToString() : string.Empty,
                            Q4Variant = (ds.Tables[3].Rows[row]["Q4Variant"] != null) ? ds.Tables[3].Rows[row]["Q4Variant"].ToString() : string.Empty,

                            Q5New = (ds.Tables[3].Rows[row]["Q5New"] != null) ? ds.Tables[3].Rows[row]["Q5New"].ToString() : string.Empty,
                            Q5Variant = (ds.Tables[3].Rows[row]["Q5Variant"] != null) ? ds.Tables[3].Rows[row]["Q5Variant"].ToString() : string.Empty,

                            Q6New = (ds.Tables[3].Rows[row]["Q6New"] != null) ? ds.Tables[3].Rows[row]["Q6New"].ToString() : string.Empty,
                            Q6Variant = (ds.Tables[3].Rows[row]["Q6Variant"] != null) ? ds.Tables[3].Rows[row]["Q6Variant"].ToString() : string.Empty,

                            Q7New = (ds.Tables[3].Rows[row]["Q7New"] != null) ? ds.Tables[3].Rows[row]["Q7New"].ToString() : string.Empty,
                            Q7Variant = (ds.Tables[3].Rows[row]["Q7Variant"] != null) ? ds.Tables[3].Rows[row]["Q7Variant"].ToString() : string.Empty,

                            Q8New = (ds.Tables[3].Rows[row]["Q8New"] != null) ? ds.Tables[3].Rows[row]["Q8New"].ToString() : string.Empty,
                            Q8Variant = (ds.Tables[3].Rows[row]["Q8Variant"] != null) ? ds.Tables[3].Rows[row]["Q8Variant"].ToString() : string.Empty,

                            Q9New = (ds.Tables[3].Rows[row]["Q9New"] != null) ? ds.Tables[3].Rows[row]["Q9New"].ToString() : string.Empty,
                            Q9Variant = (ds.Tables[3].Rows[row]["Q9Variant"] != null) ? ds.Tables[3].Rows[row]["Q9Variant"].ToString() : string.Empty,

                            Q10New = (ds.Tables[3].Rows[row]["Q10New"] != null) ? ds.Tables[3].Rows[row]["Q10New"].ToString() : string.Empty,
                            Q10Variant = (ds.Tables[3].Rows[row]["Q10Variant"] != null) ? ds.Tables[3].Rows[row]["Q10Variant"].ToString() : string.Empty,

                            Q11New = (ds.Tables[3].Rows[row]["Q11New"] != null) ? ds.Tables[3].Rows[row]["Q11New"].ToString() : string.Empty,
                            Q11Variant = (ds.Tables[3].Rows[row]["Q11Variant"] != null) ? ds.Tables[3].Rows[row]["Q11Variant"].ToString() : string.Empty,

                            Q12New = (ds.Tables[3].Rows[row]["Q12New"] != null) ? ds.Tables[3].Rows[row]["Q12New"].ToString() : string.Empty,
                            Q12Variant = (ds.Tables[3].Rows[row]["Q12Variant"] != null) ? ds.Tables[3].Rows[row]["Q12Variant"].ToString() : string.Empty
                        });
                    }
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[4] != null))
                {
                    if(ds.Tables[4].Rows.Count == 0)
                    {
                        comments = " ";
                    }
                    else
                    {
                        comments = ds.Tables[4].Rows[0]["Comments"].ToString();
                    }
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[5] != null))
                {
                    editable = Convert.ToBoolean(ds.Tables[5].Rows[0]["Editable"]);
                }
                var yearlyScenarioData = new List<YearlyScenarioData>();
                if (scenarioDatasRaw.Count > 0)
                {
                    foreach (var year in financialYears)
                    {
                        var scenarioDatas = new List<ScenarioDataList>();
                        var quarters = quarterLocks.Where(q => q.Year == year).ToList();
                        var yearLock = (yearLocks.Select(y => y.Year).Contains(year)) ? yearLocks.First(y => y.Year == year).Lock : false;
                        var q1Lock = (quarters.Select(q => q.Quarter).Contains("Q1")) ? quarters.First(q => q.Quarter == "Q1").Lock : false;
                        var q2Lock = (quarters.Select(q => q.Quarter).Contains("Q2")) ? quarters.First(q => q.Quarter == "Q2").Lock : false;
                        var q3Lock = (quarters.Select(q => q.Quarter).Contains("Q3")) ? quarters.First(q => q.Quarter == "Q3").Lock : false;
                        var q4Lock = (quarters.Select(q => q.Quarter).Contains("Q4")) ? quarters.First(q => q.Quarter == "Q4").Lock : false;
                        var q5Lock = (quarters.Select(q => q.Quarter).Contains("Q5")) ? quarters.First(q => q.Quarter == "Q5").Lock : false;
                        var q6Lock = (quarters.Select(q => q.Quarter).Contains("Q6")) ? quarters.First(q => q.Quarter == "Q6").Lock : false;
                        var q7Lock = (quarters.Select(q => q.Quarter).Contains("Q7")) ? quarters.First(q => q.Quarter == "Q7").Lock : false;
                        var q8Lock = (quarters.Select(q => q.Quarter).Contains("Q8")) ? quarters.First(q => q.Quarter == "Q8").Lock : false;
                        var q9Lock = (quarters.Select(q => q.Quarter).Contains("Q9")) ? quarters.First(q => q.Quarter == "Q9").Lock : false;
                        var q10Lock = (quarters.Select(q => q.Quarter).Contains("Q10")) ? quarters.First(q => q.Quarter == "Q10").Lock : false;
                        var q11Lock = (quarters.Select(q => q.Quarter).Contains("Q11")) ? quarters.First(q => q.Quarter == "Q11").Lock : false;
                        var q12Lock = (quarters.Select(q => q.Quarter).Contains("Q12")) ? quarters.First(q => q.Quarter == "Q12").Lock : false;
                        foreach (var data in scenarioDatasRaw.Where(r => r.Year == year).ToList())
                        {
                            scenarioDatas.Add(new ScenarioDataList
                            {
                                ScenarioId = scenarioId,
                                ProjectId = 0,
                                ScenarioDataTypeId = data.ScenarioDataTypeId,
                                FinancialDataTypeName = data.FinancialDataTypeName,
                                Year = year,

                                Q1New = data.Q1New,
                                Q1Variant = data.Q1Variant,

                                Q2New = data.Q2New,
                                Q2Variant = data.Q2Variant,

                                Q3New = data.Q3New,
                                Q3Variant = data.Q3Variant,

                                Q4New = data.Q4New,
                                Q4Variant = data.Q4Variant,

                                Q5New = data.Q5New,
                                Q5Variant = data.Q5Variant,

                                Q6New = data.Q6New,
                                Q6Variant = data.Q6Variant,

                                Q7New = data.Q7New,
                                Q7Variant = data.Q7Variant,

                                Q8New = data.Q8New,
                                Q8Variant = data.Q8Variant,

                                Q9New = data.Q9New,
                                Q9Variant = data.Q9Variant,

                                Q10New = data.Q10New,
                                Q10Variant = data.Q10Variant,

                                Q11New = data.Q11New,
                                Q11Variant = data.Q11Variant,

                                Q12New = data.Q12New,
                                Q12Variant = data.Q12Variant,

                                Q1Lock = q1Lock,
                                Q2Lock = q2Lock,
                                Q3Lock = q3Lock,
                                Q4Lock = q4Lock,
                                Q5Lock = q5Lock,
                                Q6Lock = q6Lock,
                                Q7Lock = q7Lock,
                                Q8Lock = q8Lock,
                                Q9Lock = q9Lock,
                                Q10Lock = q10Lock,
                                Q11Lock = q11Lock,
                                Q12Lock = q12Lock
                            });
                        }

                        yearlyScenarioData.Add(new YearlyScenarioData
                        {
                            Year = year,
                            YearLock = yearLock,
                            ScenarioDataLists = scenarioDatas,
                            QuarterApplicable = ((year == CurrentFinancialYear) || (year == (CurrentFinancialYear + 1))) ? true : false,
                            Comments = comments,
                            Editable = editable
                        });
                    }
                }
                return yearlyScenarioData;
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

        public bool Save(ScenarioDataMaster scenarioData)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveScenarioData";
                var xmldata = new XElement("TDS", from ObjDetails in scenarioData.ScenarioData
                                                  select new XElement("TD",
                                                 new XElement("ScenarioDataTypeID", ObjDetails.ScenarioDataTypeId),
                                                 new XElement("Year", ObjDetails.Year),
                                                 new XElement("Q1New", ObjDetails.Q1New),
                                                 new XElement("Q1Variant", ObjDetails.Q1Variant),
                                                 new XElement("Q2New", ObjDetails.Q2New),
                                                 new XElement("Q2Variant", ObjDetails.Q2Variant),
                                                 new XElement("Q3New", ObjDetails.Q3New),
                                                 new XElement("Q3Variant", ObjDetails.Q3Variant),
                                                 new XElement("Q4New", ObjDetails.Q4New),
                                                 new XElement("Q4Variant", ObjDetails.Q4Variant),
                                                 new XElement("Q5New", ObjDetails.Q5New),
                                                 new XElement("Q5Variant", ObjDetails.Q5Variant),
                                                 new XElement("Q6New", ObjDetails.Q6New),
                                                 new XElement("Q6Variant", ObjDetails.Q6Variant),
                                                 new XElement("Q7New", ObjDetails.Q7New),
                                                 new XElement("Q7Variant", ObjDetails.Q7Variant),
                                                 new XElement("Q8New", ObjDetails.Q8New),
                                                 new XElement("Q8Variant", ObjDetails.Q8Variant),
                                                 new XElement("Q9New", ObjDetails.Q9New),
                                                 new XElement("Q9Variant", ObjDetails.Q9Variant),
                                                 new XElement("Q10New", ObjDetails.Q10New),
                                                 new XElement("Q10Variant", ObjDetails.Q10Variant),
                                                 new XElement("Q11New", ObjDetails.Q11New),
                                                 new XElement("Q11Variant", ObjDetails.Q11Variant),
                                                 new XElement("Q12New", ObjDetails.Q12New),
                                                 new XElement("Q12Variant", ObjDetails.Q12Variant)));

                cmd.Parameters.AddWithValue("@P_ScenarioID", scenarioData.ScenarioId);
                cmd.Parameters.AddWithValue("@P_ProjectID", scenarioData.ProjectId);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                cmd.Parameters.AddWithValue("@P_Comments", scenarioData.Comments);
                cmd.Parameters.AddWithValue("@P_CreatedBy", scenarioData.CreatedBy);
                cmd.Parameters.AddWithValue("@P_CustomerId", scenarioData.CustomerID);
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

        public ScenarioData Update(ScenarioData ScenarioData)
        {
            throw new NotImplementedException();
        }

        public List<ScenarioLayout> GetScenarioLayout(int scenarioId)
        {
            try
            {
                var financialYears = new List<string>();
                var yearLocks = new List<LockYear>();
                var quarterLocks = new List<LockQuarter>();
                var financialDataTypes = new List<FinancialDataTypeMaster>();
                var quarterDivs = new List<String>();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetScenarioLayOut";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    for (var row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        financialYears.Add(ds.Tables[0].Rows[row]["value"].ToString());
                    }
                }
                if ((ds != null) && (ds.Tables.Count > 1) && (ds.Tables[1] != null) && (ds.Tables[1].Rows.Count > 0))
                    yearLocks = ds.Tables[1].ToListOfObject<LockYear>().ToList();
                if ((ds != null) && (ds.Tables.Count > 2) && (ds.Tables[2] != null) && (ds.Tables[2].Rows.Count > 0))
                    quarterLocks = ds.Tables[2].ToListOfObject<LockQuarter>().ToList();
                if ((ds != null) && (ds.Tables.Count > 3) && (ds.Tables[3] != null) && (ds.Tables[3].Rows.Count > 0))
                    financialDataTypes = ds.Tables[3].ToListOfObject<FinancialDataTypeMaster>().ToList();
                if ((ds != null) && (ds.Tables.Count > 4) && (ds.Tables[4] != null) && (ds.Tables[4].Rows.Count > 0))
                {
                    if (Convert.ToBoolean(ds.Tables[4].Rows[0]["IsNew"]))
                        quarterDivs.Add("New");
                    if (Convert.ToBoolean(ds.Tables[4].Rows[0]["IsVariant"]))
                        quarterDivs.Add("Variant");
                }

                var financialYear = 0;
                var scenarioScopeCode = "";
                var scenarioTypeCode = "";
                var dataEntryInterval = "";

                if ((ds != null) && (ds.Tables.Count > 5) && (ds.Tables[5] != null) && (ds.Tables[5].Rows.Count > 0))
                {
                    financialYear = Convert.ToInt32(ds.Tables[5].Rows[0][2]);
                    scenarioScopeCode = Convert.ToString(ds.Tables[5].Rows[0][0]);
                    scenarioTypeCode = Convert.ToString(ds.Tables[5].Rows[0][1]);
                    dataEntryInterval = Convert.ToString(ds.Tables[5].Rows[0][3]);
                }

                var scenarioLayout = new List<ScenarioLayout>();
                List<string> defaultyQuarters;

                if (dataEntryInterval == "Quarterly")
                {
                    defaultyQuarters = new List<string>() { "q1", "q2", "q3", "q4" };
                }
                else
                {
                    defaultyQuarters = new List<string>() { "q1", "q2", "q3", "q4", "q5", "q6", "q7", "q8", "q9", "q10", "q11", "q12" };
                }
                string GetQuarterHeader(string quarter)
                {
                    if (dataEntryInterval == "Monthly")
                    {
                        switch (quarter.ToLower())
                        {
                            case "q1": return "Jan";
                            case "q2": return "Feb";
                            case "q3": return "Mar";
                            case "q4": return "Apr";
                            case "q5": return "May";
                            case "q6": return "Jun";
                            case "q7": return "Jul";
                            case "q8": return "Aug";
                            case "q9": return "Sep";
                            case "q10": return "Oct";
                            case "q11": return "Nov";
                            case "q12": return "Dec";
                            default: return "Unknown";  // Default value to handle unexpected cases
                        }
                    }
                    else
                    {
                        switch (quarter.ToLower())
                        {
                            case "q1": return "q1";
                            case "q2": return "q2";
                            case "q3": return "q3";
                            case "q4": return "q4";

                            default: return "Unknown";  // Default value to handle unexpected cases
                        }
                    }

                }
                if (scenarioScopeCode == "PL" && scenarioTypeCode == "AC")
                {
                    var quarters = quarterLocks.Where(q => q.Year == financialYear).ToList();
                    foreach (var item in defaultyQuarters)
                    {
                        var existQuarter = quarters.Find(a => a.Quarter.ToLower() == item);
                        if (existQuarter == null)
                            quarters.Add(new LockQuarter() { Lock = false, Quarter = item });

                    }
                    var yearLock = (yearLocks.Select(y => y.Year).Contains(financialYear)) ?
                        yearLocks.First(y => y.Year == financialYear).Lock : false;

                    scenarioLayout.Add(new ScenarioLayout()
                    {
                        QuarterDivs = quarterDivs,
                        Year = financialYear.ToString(),
                        YearLock = yearLock,
                        Quarters = (quarters.Count > 0) ? (from q in quarters
                                                           select new QuartersLayOut
                                                           {
                                                               qLock = q.Lock,
                                                               qName = q.Quarter.ToLower(),
                                                               qHeader = GetQuarterHeader(q.Quarter)
                                                           }).ToList() : (dataEntryInterval == "Quarterly" ?
                                                      new List<QuartersLayOut>()
                                                      {
                                                          new QuartersLayOut{ qName = "q1",qLock =false,qHeader = "q1"},
                                                          new QuartersLayOut{ qName = "q2",qLock =false,qHeader = "q2"},
                                                          new QuartersLayOut{ qName = "q3",qLock =false,qHeader = "q3"},
                                                          new QuartersLayOut{ qName = "q4",qLock =false,qHeader = "q4"}
                                                      } :
                                                      new List<QuartersLayOut>()
                                                      {
                                                          new QuartersLayOut { qName = "q1", qLock = false, qHeader = "Jan" },
                                                            new QuartersLayOut { qName = "q2", qLock = false, qHeader = "Feb" },
                                                            new QuartersLayOut { qName = "q3", qLock = false, qHeader = "Mar" },
                                                            new QuartersLayOut { qName = "q4", qLock = false, qHeader = "Apr" },
                                                            new QuartersLayOut { qName = "q5", qLock = false, qHeader = "May" },
                                                            new QuartersLayOut { qName = "q6", qLock = false, qHeader = "Jun" },
                                                            new QuartersLayOut { qName = "q7", qLock = false, qHeader = "Jul" },
                                                            new QuartersLayOut { qName = "q8", qLock = false, qHeader = "Aug" },
                                                            new QuartersLayOut { qName = "q9", qLock = false, qHeader = "Sep" },
                                                            new QuartersLayOut { qName = "q10", qLock = false, qHeader = "Oct" },
                                                            new QuartersLayOut { qName = "q11", qLock = false, qHeader = "Nov" },
                                                            new QuartersLayOut { qName = "q12", qLock = false, qHeader = "Dec" }
                                                      }),
                        QuarterApplicable = true,
                        FinancialDataTypes = new List<FinancialDataTypeMaster>(financialDataTypes),
                        DataEntryInterval = dataEntryInterval
                    });
                }
                else
                {
                    var yearCount = financialYears.Count;
                    //int k = 1;
                    foreach (var year in financialYears)
                    {
                        string yearDisplay = year.ToString();
                        //if (yearCount == k)
                        //{
                        //    yearDisplay = year.ToString() + "+";
                        //}
                        //k++;
                        var quarters = quarterLocks.Where(q => q.Year.ToString() == year.ToString()).ToList();

                        foreach (var item in defaultyQuarters)
                        {
                            var existQuarter = quarters.Find(a => a.Quarter.ToLower() == item);
                            if (existQuarter == null)
                                quarters.Add(new LockQuarter() { Lock = false, Quarter = item });

                        }
                        var yearLock = (yearLocks.Select(y => y.Year.ToString()).Contains(year)) ? yearLocks.First(y => y.Year.ToString() == year.ToString()).Lock : false;

                        var layout = new ScenarioLayout()
                        {
                            QuarterDivs = quarterDivs,
                            Year = yearDisplay,
                            YearLock = yearLock,
                            FinancialDataTypes = new List<FinancialDataTypeMaster>(),
                            Quarters = (quarters.Count > 0) ? (from q in quarters
                                                               select new QuartersLayOut
                                                               {
                                                                   qLock = q.Lock,
                                                                   qName = q.Quarter.ToLower(),
                                                                   qHeader = GetQuarterHeader(q.Quarter)
                                                               }).ToList() : (dataEntryInterval == "Quarterly" ?
                                                               new List<QuartersLayOut>()
                                                               {
                                                                   new QuartersLayOut{ qName = "q1",qLock =false,qHeader = "q1"},
                                                          new QuartersLayOut{ qName = "q2",qLock =false,qHeader = "q2"},
                                                          new QuartersLayOut{ qName = "q3",qLock =false,qHeader = "q3"},
                                                          new QuartersLayOut{ qName = "q4",qLock =false,qHeader = "q4"}
                                                               } :
                                                      new List<QuartersLayOut>()
                                                      {
                                                          new QuartersLayOut { qName = "q1", qLock = false, qHeader = "Jan" },
                                                            new QuartersLayOut { qName = "q2", qLock = false, qHeader = "Feb" },
                                                            new QuartersLayOut { qName = "q3", qLock = false, qHeader = "Mar" },
                                                            new QuartersLayOut { qName = "q4", qLock = false, qHeader = "Apr" },
                                                            new QuartersLayOut { qName = "q5", qLock = false, qHeader = "May" },
                                                            new QuartersLayOut { qName = "q6", qLock = false, qHeader = "Jun" },
                                                            new QuartersLayOut { qName = "q7", qLock = false, qHeader = "Jul" },
                                                            new QuartersLayOut { qName = "q8", qLock = false, qHeader = "Aug" },
                                                            new QuartersLayOut { qName = "q9", qLock = false, qHeader = "Sep" },
                                                            new QuartersLayOut { qName = "q10", qLock = false, qHeader = "Oct" },
                                                            new QuartersLayOut { qName = "q11", qLock = false, qHeader = "Nov" },
                                                            new QuartersLayOut { qName = "q12", qLock = false, qHeader = "Dec" }
                                                      }),
                            QuarterApplicable = (Convert.ToInt32(year) > (Convert.ToInt32(financialYear) + 1)) ? false : true,
                            DataEntryInterval = dataEntryInterval
                        };
                        layout.FinancialDataTypes.AddRange(financialDataTypes);
                        
                        scenarioLayout.Add(layout);
                    }
                }
                return scenarioLayout;
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
        public ScenarioDetails GetAllQuarterlyDataOfScenario(int scenarioId)
        {
            try
            {
                var dataEntryInterval = "Quarterly";
                var scenarioDatas = new List<ScenarioDataList>();
                ScenarioDetails listScenarioProject = new ScenarioDetails();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllQuarterlyDataOfScenario";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    for (var row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        dataEntryInterval = ds.Tables[0].Rows[0]["DataEntryInterval"].ToString();
                        scenarioDatas.Add(new ScenarioDataList
                        {
                            FinancialDataTypeName = ds.Tables[0].Rows[row]["ScenarioDataType"].ToString(),
                            ProjectId = Convert.ToInt32(ds.Tables[0].Rows[row]["ProjectId"]),
                            //ProjectCode = ds.Tables[0].Rows[row]["ProjectCode"].ToString(),
                            //ProjectName = ds.Tables[0].Rows[row]["ProjectName"].ToString(),
                            //ScenarioId = scenarioId,
                            ScenarioDataTypeId = (ds.Tables[0].Rows[row]["ScenarioDataTypeId"] != null) ? Convert.ToInt32(ds.Tables[0].Rows[row]["ScenarioDataTypeId"]) : 0,
                            Year = (ds.Tables[0].Rows[row]["Year"] != null) ? Convert.ToInt32(ds.Tables[0].Rows[row]["Year"]) : 0,

                            Q1New = (ds.Tables[0].Rows[row]["Q1New"] != null) ? ds.Tables[0].Rows[row]["Q1New"].ToString() : string.Empty,
                            Q1Variant = (ds.Tables[0].Rows[row]["Q1Variant"] != null) ? ds.Tables[0].Rows[row]["Q1Variant"].ToString() : string.Empty,

                            Q2New = (ds.Tables[0].Rows[row]["Q2New"] != null) ? ds.Tables[0].Rows[row]["Q2New"].ToString() : string.Empty,
                            Q2Variant = (ds.Tables[0].Rows[row]["Q2Variant"] != null) ? ds.Tables[0].Rows[row]["Q2Variant"].ToString() : string.Empty,

                            Q3New = (ds.Tables[0].Rows[row]["Q3New"] != null) ? ds.Tables[0].Rows[row]["Q3New"].ToString() : string.Empty,
                            Q3Variant = (ds.Tables[0].Rows[row]["Q3Variant"] != null) ? ds.Tables[0].Rows[row]["Q3Variant"].ToString() : string.Empty,

                            Q4New = (ds.Tables[0].Rows[row]["Q4New"] != null) ? ds.Tables[0].Rows[row]["Q4New"].ToString() : string.Empty,
                            Q4Variant = (ds.Tables[0].Rows[row]["Q4Variant"] != null) ? ds.Tables[0].Rows[row]["Q4Variant"].ToString() : string.Empty,

                            Q5New = (ds.Tables[0].Rows[row]["Q5New"] != null) ? ds.Tables[0].Rows[row]["Q5New"].ToString() : string.Empty,
                            Q5Variant = (ds.Tables[0].Rows[row]["Q5Variant"] != null) ? ds.Tables[0].Rows[row]["Q5Variant"].ToString() : string.Empty,

                            Q6New = (ds.Tables[0].Rows[row]["Q6New"] != null) ? ds.Tables[0].Rows[row]["Q6New"].ToString() : string.Empty,
                            Q6Variant = (ds.Tables[0].Rows[row]["Q6Variant"] != null) ? ds.Tables[0].Rows[row]["Q6Variant"].ToString() : string.Empty,

                            Q7New = (ds.Tables[0].Rows[row]["Q7New"] != null) ? ds.Tables[0].Rows[row]["Q7New"].ToString() : string.Empty,
                            Q7Variant = (ds.Tables[0].Rows[row]["Q7Variant"] != null) ? ds.Tables[0].Rows[row]["Q7Variant"].ToString() : string.Empty,

                            Q8New = (ds.Tables[0].Rows[row]["Q8New"] != null) ? ds.Tables[0].Rows[row]["Q8New"].ToString() : string.Empty,
                            Q8Variant = (ds.Tables[0].Rows[row]["Q8Variant"] != null) ? ds.Tables[0].Rows[row]["Q8Variant"].ToString() : string.Empty,

                            Q9New = (ds.Tables[0].Rows[row]["Q9New"] != null) ? ds.Tables[0].Rows[row]["Q9New"].ToString() : string.Empty,
                            Q9Variant = (ds.Tables[0].Rows[row]["Q9Variant"] != null) ? ds.Tables[0].Rows[row]["Q9Variant"].ToString() : string.Empty,

                            Q10New = (ds.Tables[0].Rows[row]["Q10New"] != null) ? ds.Tables[0].Rows[row]["Q10New"].ToString() : string.Empty,
                            Q10Variant = (ds.Tables[0].Rows[row]["Q10Variant"] != null) ? ds.Tables[0].Rows[row]["Q10Variant"].ToString() : string.Empty,

                            Q11New = (ds.Tables[0].Rows[row]["Q11New"] != null) ? ds.Tables[0].Rows[row]["Q11New"].ToString() : string.Empty,
                            Q11Variant = (ds.Tables[0].Rows[row]["Q11Variant"] != null) ? ds.Tables[0].Rows[row]["Q11Variant"].ToString() : string.Empty,

                            Q12New = (ds.Tables[0].Rows[row]["Q12New"] != null) ? ds.Tables[0].Rows[row]["Q12New"].ToString() : string.Empty,
                            Q12Variant = (ds.Tables[0].Rows[row]["Q12Variant"] != null) ? ds.Tables[0].Rows[row]["Q12Variant"].ToString() : string.Empty
                        });
                    }
                    var financialYear = 0;
                    var scenarioScopeCode = "";
                    var scenarioTypeCode = "";

                    if ((ds != null) && (ds.Tables.Count > 2) && (ds.Tables[2] != null) && (ds.Tables[2].Rows.Count > 0))
                    {
                        financialYear = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                        scenarioScopeCode = Convert.ToString(ds.Tables[2].Rows[0][1]);
                        scenarioTypeCode = Convert.ToString(ds.Tables[2].Rows[0][2]);
                    }
                    //DataSet dsProjects = new DataSet();
                    int currentYear = financialYear;

                    List<DetailedStructure> DetailedStructures = new List<DetailedStructure>();
                    List<HeaderStructure> listHeaderStructure = new List<HeaderStructure>();
                    listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Project Code", FieldName = "ProjectCode" });
                    listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Project Name", FieldName = "ProjectName" });
                    string[] monthNames = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
                    bool isQuarterly = (dataEntryInterval == "Quarterly");

                    for (int q = 0; q < (isQuarterly ? 4 : 12); q++)
                    {
                        string fieldName = $"CurrentYearRevenueQ{q + 1}New";
                        string headerName = isQuarterly ? $"Revenue FY {financialYear} Q{q+1}": $"Revenue FY {financialYear} {monthNames[q]}";

                        if (scenarioScopeCode != "OI")
                        {
                            listHeaderStructure.Add(new HeaderStructure
                            {
                                HeaderName = headerName,
                                FieldName = fieldName,
                                Year = financialYear.ToString()
                            });
                        }
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure
                            {
                                HeaderName = $"{headerName} New",
                                FieldName = fieldName,
                                Year = financialYear.ToString()
                            });

                            
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = $"{headerName} Variant",
                                    FieldName = $"CurrentYearRevenueQ{q + 1}Variant",
                                    Year = financialYear.ToString()
                                });
                           
                        }
                    }

                    // Add cumulative header
                    listHeaderStructure.Add(new HeaderStructure
                    {
                        HeaderName = $"Revenue FY {financialYear} Cumulative",
                        FieldName = "CurrentYearRevenueCumulative",
                        Year = financialYear.ToString()
                    });
                    if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                    {
                        for (int q = 0; q < (isQuarterly ? 4 : 12); q++)
                        {
                            string fieldName = $"NextYearRevenueQ{q + 1}New";
                            string headerName = isQuarterly ? $"Revenue FY {financialYear +1} Q{q + 1}" : $"Revenue FY {financialYear +1} {monthNames[q]}";

                            if (scenarioScopeCode != "OI")
                            {
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = headerName,
                                    FieldName = fieldName,
                                    Year = (financialYear+1).ToString()
                                });
                            }
                            else
                            {
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = $"{headerName} New",
                                    FieldName = fieldName,
                                    Year = (financialYear+1).ToString()
                                });

                                
                                    listHeaderStructure.Add(new HeaderStructure
                                    {
                                        HeaderName = $"{headerName} Variant",
                                        FieldName = $"NextYearRevenueQ{q + 1}Variant",
                                        Year = (financialYear+1).ToString()
                                    });
                                
                            }
                        }

                        // Add cumulative header
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"Revenue FY {financialYear + 1} Cumulative",
                            FieldName = "NextYearRevenueCumulative",
                            Year = (financialYear + 1).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"Revenue FY {financialYear + 2} Cumulative",
                            FieldName = "ThirdYearRevenueCumulative",
                            Year = (financialYear + 2).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"Revenue FY {financialYear + 3} Cumulative",
                            FieldName = "FifthYearRevenueCumulative",
                            Year = (financialYear + 3).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"Revenue FY {financialYear + 4} Cumulative",
                            FieldName = "NextYearRevenueCumulative",
                            Year = (financialYear + 4).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue GrandTotal", FieldName = "RevenueGrandTotal" });
                    }
                    for (int q = 0; q < (isQuarterly ? 4 : 12); q++)
                    {
                        string fieldName = $"CurrentYearGrossMarginQ{q + 1}New";
                        string headerName = isQuarterly ? $"GrossMargin FY {financialYear} Q{q+1}": $"GrossMargin FY {financialYear} {monthNames[q]}";

                        if (scenarioScopeCode != "OI")
                        {
                            listHeaderStructure.Add(new HeaderStructure
                            {
                                HeaderName = headerName,
                                FieldName = fieldName,
                                Year = financialYear.ToString()
                            });
                        }
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure
                            {
                                HeaderName = $"{headerName} New",
                                FieldName = fieldName,
                                Year = financialYear.ToString()
                            });

                            
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = $"{headerName} Variant",
                                    FieldName = $"CurrentYearGrossMarginQ{q + 1}Variant",
                                    Year = financialYear.ToString()
                                });
                            
                        }
                    }

                    // Add cumulative header
                    listHeaderStructure.Add(new HeaderStructure
                    {
                        HeaderName = $"GrossMargin FY {financialYear} Cumulative",
                        FieldName = "CurrentYearGrossMarginCumulative",
                        Year = financialYear.ToString()
                    });
                    if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                    {
                        for (int q = 0; q < (isQuarterly ? 4 : 12); q++)
                        {
                            string fieldName = $"NextYearGrossMarginQ{q + 1}New";
                            string headerName = isQuarterly ? $"GrossMargin FY {financialYear + 1} Q{q + 1}" : $"GrossMargin FY {financialYear + 1} {monthNames[q]}";

                            if (scenarioScopeCode != "OI")
                            {
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = headerName,
                                    FieldName = fieldName,
                                    Year = (financialYear + 1).ToString()
                                });
                            }
                            else
                            {
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = $"{headerName} New",
                                    FieldName = fieldName,
                                    Year = (financialYear + 1).ToString()
                                });

                                if (isQuarterly)
                                {
                                    listHeaderStructure.Add(new HeaderStructure
                                    {
                                        HeaderName = $"{headerName} Variant",
                                        FieldName = $"NextYearGrossMarginQ{q + 1}Variant",
                                        Year = (financialYear + 1).ToString()
                                    });
                                }
                            }
                        }

                        // Add cumulative header
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"GrossMargin FY {financialYear + 1} Cumulative",
                            FieldName = "NextYearGrossMarginCumulative",
                            Year = (financialYear + 1).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"GrossMargin FY {financialYear + 2} Cumulative",
                            FieldName = "ThirdYearGrossMarginCumulative",
                            Year = (financialYear + 2).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"GrossMargin FY {financialYear + 3} Cumulative",
                            FieldName = "FifthYearGrossMarginCumulative",
                            Year = (financialYear + 3).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"GrossMargin FY {financialYear + 4} Cumulative",
                            FieldName = "NextYearGrossMarginCumulative",
                            Year = (financialYear + 4).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "GrossMargin GrandTotal", FieldName = "GrossMarginGrandTotal" });

                    }
                    for (int q = 0; q < (isQuarterly ? 4 : 12); q++)
                    {
                        string fieldName = $"CurrentYearManHoursQ{q + 1}New";
                        string headerName = isQuarterly ? $"ManHours FY {financialYear} Q{q + 1}" : $"ManHours FY {financialYear} {monthNames[q]}";

                        if (scenarioScopeCode != "OI")
                        {
                            listHeaderStructure.Add(new HeaderStructure
                            {
                                HeaderName = headerName,
                                FieldName = fieldName,
                                Year = financialYear.ToString()
                            });
                        }
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure
                            {
                                HeaderName = $"{headerName} New",
                                FieldName = fieldName,
                                Year = financialYear.ToString()
                            });

                            
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = $"{headerName} Variant",
                                    FieldName = $"CurrentYearManHoursQ{q + 1}Variant",
                                    Year = financialYear.ToString()
                                });
                            
                        }
                    }

                    // Add cumulative header
                    listHeaderStructure.Add(new HeaderStructure
                    {
                        HeaderName = $"ManHours FY {financialYear} Cumulative",
                        FieldName = "CurrentYearManHoursCumulative",
                        Year = financialYear.ToString()
                    });
                    if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                    {
                        for (int q = 0; q < (isQuarterly ? 4 : 12); q++)
                        {
                            string fieldName = $"NextYearManHoursQ{q + 1}New";
                            string headerName = isQuarterly ? $"ManHours FY {financialYear + 1} Q{q + 1}" : $"ManHours FY {financialYear + 1} {monthNames[q]}";

                            if (scenarioScopeCode != "OI")
                            {
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = headerName,
                                    FieldName = fieldName,
                                    Year = (financialYear + 1).ToString()
                                });
                            }
                            else
                            {
                                listHeaderStructure.Add(new HeaderStructure
                                {
                                    HeaderName = $"{headerName} New",
                                    FieldName = fieldName,
                                    Year = (financialYear + 1).ToString()
                                });

                                
                                    listHeaderStructure.Add(new HeaderStructure
                                    {
                                        HeaderName = $"{headerName} Variant",
                                        FieldName = $"NextYearManHoursQ{q + 1}Variant",
                                        Year = (financialYear + 1).ToString()
                                    });
                               
                            }
                        }

                        // Add cumulative header
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"ManHours FY {financialYear + 1} Cumulative",
                            FieldName = "NextYearManHoursCumulative",
                            Year = (financialYear + 1).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"ManHours FY {financialYear + 2} Cumulative",
                            FieldName = "ThirdYearManHoursCumulative",
                            Year = (financialYear + 2).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"ManHours FY {financialYear + 3} Cumulative",
                            FieldName = "FifthYearManHoursCumulative",
                            Year = (financialYear + 3).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure
                        {
                            HeaderName = $"ManHours FY {financialYear + 4} Cumulative",
                            FieldName = "NextYearManHoursCumulative",
                            Year = (financialYear + 4).ToString()
                        });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "ManvHours GrandTotal", FieldName = "ManHoursGrandTotal" });
                    }

                    if (scenarioScopeCode == "PL" && scenarioTypeCode == "AC")
                    {
                        for (int q = 0; q < (isQuarterly ? 4 : 12); q++)
                        {
                            string fieldName = $"CurrentYearCostOfSalesQ{q + 1}New";
                            string headerName = isQuarterly ? $"Cost Of Sales FY {financialYear} Q{q + 1}" : $"Cost Of Sales FY {financialYear} {monthNames[q]}";
                            listHeaderStructure.Add(new HeaderStructure
                            {
                                HeaderName = headerName,
                                FieldName = fieldName,
                                Year = (financialYear).ToString()
                            });
                        }
                            
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Cost Of Sales Grand Total", FieldName = "CostOfSalesGrandTotal" });
                        for (int q = 0; q < (isQuarterly ? 4 : 12); q++)
                        {
                            string fieldName = $"CurrentYearProvFutureLossQ{q + 1}New";
                            string headerName = isQuarterly ? $"Prov. for Loss FY {financialYear} Q{q + 1}" : $"Prov. for Loss FY {financialYear} {monthNames[q]}";
                            listHeaderStructure.Add(new HeaderStructure
                            {
                                HeaderName = headerName,
                                FieldName = fieldName,
                                Year = (financialYear).ToString()
                            });
                        }
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Prov. for Loss", FieldName = "ProvFutureLossGrandTotal", Year = (financialYear).ToString() });
                    }

                    decimal? CurrentYearRevenueQ1New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ1Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ2New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ2Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ3New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ3Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ4New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ4Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ5New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ5Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ6New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ6Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ7New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ7Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ8New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ8Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ9New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ9Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ10New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ10Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ11New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ11Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ12New_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueQ12Variant_Gross = decimal.Zero;
                    decimal? CurrentYearRevenueCumulative_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ1New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ1Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ2New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ2Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ3New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ3Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ4New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ4Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ5New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ5Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ6New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ6Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ7New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ7Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ8New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ8Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ9New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ9Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ10New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ10Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ11New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ11Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ12New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ12Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueCumulative_Gross = decimal.Zero;
                    decimal? ThirdYearRevenueCumulative_Gross = decimal.Zero;
                    decimal? FourthYearRevenueCumulative_Gross = decimal.Zero;
                    decimal? FifthYearRevenueCumulative_Gross = decimal.Zero;
                    decimal? RevenueGrandTotal_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ1New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ1Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ2New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ2Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ3New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ3Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ4New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ4Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ5New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ5Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ6New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ6Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ7New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ7Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ8New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ8Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ9New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ9Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ10New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ10Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ11New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ11Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ12New_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginQ12Variant_Gross = decimal.Zero;
                    decimal? CurrentYearGrossMarginCumulative_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ1New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ1Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ2New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ2Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ3New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ3Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ4New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ4Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ5New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ5Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ6New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ6Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ7New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ7Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ8New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ8Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ9New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ9Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ10New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ10Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ11New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ11Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ12New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ12Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginCumulative_Gross = decimal.Zero;
                    decimal? ThirdYearGrossMarginCumulative_Gross = decimal.Zero;
                    decimal? FourthYearGrossMarginCumulative_Gross = decimal.Zero;
                    decimal? FifthYearGrossMarginCumulative_Gross = decimal.Zero;
                    decimal? GrossMarginGrandTotal_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ1New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ1Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ2New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ2Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ3New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ3Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ4New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ4Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ5New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ5Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ6New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ6Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ7New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ7Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ8New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ8Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ9New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ9Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ10New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ10Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ11New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ11Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ12New_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursQ12Variant_Gross = decimal.Zero;
                    decimal? CurrentYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ1New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ1Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ2New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ2Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ3New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ3Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ4New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ4Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ5New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ5Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ6New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ6Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ7New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ7Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ8New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ8Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ9New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ9Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ10New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ10Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ11New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ11Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ12New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ12Variant_Gross = decimal.Zero;

                    decimal? NextYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? ThirdYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? FourthYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? FifthYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? ManHoursGrandTotal_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ1New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ2New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ3New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ4New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ5New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ6New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ7New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ8New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ9New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ10New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ11New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ12New_Gross = decimal.Zero;
                    decimal? CostOfSalesGrandTotal_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ1New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ2New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ3New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ4New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ5New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ6New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ7New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ8New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ9New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ10New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ11New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ12New_Gross = decimal.Zero;
                    decimal? ProvFutureLossGrandTotal_Gross = decimal.Zero;

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        DetailedStructure obj = new DetailedStructure();
                        obj.ProjectCode = ds.Tables[1].Rows[i]["ProjectCode"].ToString();
                        obj.ProjectName = ds.Tables[1].Rows[i]["ProjectName"].ToString();
                        //Get all Revenue Values
                        var revenueDetails = scenarioDatas.Where(a => a.ProjectId == Convert.ToInt32(ds.Tables[1].Rows[i]["ProjectId"].ToString())
                            && a.FinancialDataTypeName == "Revenue").ToList();

                        var currentRevenue = revenueDetails.Where(a => a.Year == currentYear).FirstOrDefault();
                        if (currentRevenue != null)
                        {
                            decimal cumulativeRevenue = 0;
                            decimal cumulativeRevenueGross = 0;
                            int intervalCount = dataEntryInterval == "Monthly" ? 12 : 4;

                            for (int j = 1; j <= intervalCount; j++)
                            {
                                string intervalNew = $"Q{j}New";
                                string intervalVariant = $"Q{j}Variant";

                                decimal intervalNewValue = !string.IsNullOrEmpty(currentRevenue.GetType().GetProperty(intervalNew)?.GetValue(currentRevenue, null)?.ToString())
                                    ? Convert.ToDecimal(currentRevenue.GetType().GetProperty(intervalNew)?.GetValue(currentRevenue, null))
                                    : 0;

                                decimal intervalVariantValue = !string.IsNullOrEmpty(currentRevenue.GetType().GetProperty(intervalVariant)?.GetValue(currentRevenue, null)?.ToString())
                                    ? Convert.ToDecimal(currentRevenue.GetType().GetProperty(intervalVariant)?.GetValue(currentRevenue, null))
                                    : 0;

                                // Setting the values to obj
                                obj.GetType().GetProperty($"CurrentYearRevenue{intervalNew}")?.SetValue(obj, intervalNewValue);
                                obj.GetType().GetProperty($"CurrentYearRevenue{intervalVariant}")?.SetValue(obj, intervalVariantValue);

                                cumulativeRevenue += intervalNewValue + intervalVariantValue;

                                // Cumulative gross
                                cumulativeRevenueGross += intervalNewValue;
                                cumulativeRevenueGross += intervalVariantValue;

                                // Accumulating values for gross
                                typeof(DetailedStructure).GetField($"CurrentYearRevenue{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                    (decimal?)typeof(DetailedStructure).GetField($"CurrentYearRevenue{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalNewValue);

                                typeof(DetailedStructure).GetField($"CurrentYearRevenue{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                    (decimal?)typeof(DetailedStructure).GetField($"CurrentYearRevenue{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalVariantValue);
                            }

                            obj.CurrentYearRevenueCumulative = cumulativeRevenue;
                            CurrentYearRevenueCumulative_Gross += cumulativeRevenue;
                        }
                        else
                        {
                            obj.CurrentYearRevenueCumulative = 0;
                            CurrentYearRevenueCumulative_Gross += obj.CurrentYearRevenueCumulative;
                        }
                        if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                        {
                            var SecondRevenue = revenueDetails.Where(a => a.Year == currentYear + 1).FirstOrDefault();
                            if (SecondRevenue != null)
                            {
                                decimal cumulativeRevenue = 0;
                                int intervalCount = dataEntryInterval == "Monthly" ? 12 : 4;

                                for (int k = 1; k <= intervalCount; k++)
                                {
                                    string intervalNew = $"Q{k}New";
                                    string intervalVariant = $"Q{k}Variant";

                                    decimal intervalNewValue = !string.IsNullOrEmpty(SecondRevenue.GetType().GetProperty(intervalNew)?.GetValue(SecondRevenue, null)?.ToString())
                                        ? Convert.ToDecimal(SecondRevenue.GetType().GetProperty(intervalNew)?.GetValue(SecondRevenue, null))
                                        : 0;

                                    decimal intervalVariantValue = !string.IsNullOrEmpty(SecondRevenue.GetType().GetProperty(intervalVariant)?.GetValue(SecondRevenue, null)?.ToString())
                                        ? Convert.ToDecimal(SecondRevenue.GetType().GetProperty(intervalVariant)?.GetValue(SecondRevenue, null))
                                        : 0;

                                    // Setting the values to obj
                                    obj.GetType().GetProperty($"NextYearRevenue{intervalNew}")?.SetValue(obj, intervalNewValue);
                                    obj.GetType().GetProperty($"NextYearRevenue{intervalVariant}")?.SetValue(obj, intervalVariantValue);

                                    cumulativeRevenue += intervalNewValue + intervalVariantValue;

                                    // Accumulating values for gross
                                    typeof(DetailedStructure).GetField($"NextYearRevenue{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                        (decimal?)typeof(DetailedStructure).GetField($"NextYearRevenue{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalNewValue);

                                    typeof(DetailedStructure).GetField($"NextYearRevenue{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                        (decimal?)typeof(DetailedStructure).GetField($"NextYearRevenue{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalVariantValue);
                                }

                                obj.NextYearRevenueCumulative = cumulativeRevenue;
                                NextYearRevenueCumulative_Gross += cumulativeRevenue;
                            }

                            else
                            {
                                obj.NextYearRevenueCumulative = 0;
                                NextYearRevenueCumulative_Gross += obj.NextYearRevenueCumulative;
                            }
                            var thirdRevenue = revenueDetails.Where(a => a.Year == currentYear + 2).FirstOrDefault();
                            if (thirdRevenue != null)
                            {
                                obj.ThirdYearRevenueCumulative = (thirdRevenue.Q1New != string.Empty) ? Convert.ToDecimal(thirdRevenue.Q1New) : 0;
                            }
                            else
                            {
                                obj.ThirdYearRevenueCumulative = 0;
                            }
                            ThirdYearRevenueCumulative_Gross += obj.ThirdYearRevenueCumulative;

                            var fourthRevenue = revenueDetails.Where(a => a.Year == currentYear + 3).FirstOrDefault();
                            if (fourthRevenue != null)
                            {
                                obj.FourthYearRevenueCumulative = (fourthRevenue.Q1New != string.Empty) ? Convert.ToDecimal(fourthRevenue.Q1New) : 0;
                            }
                            else
                            {
                                obj.FourthYearRevenueCumulative = 0;
                            }
                            FourthYearRevenueCumulative_Gross += obj.FourthYearRevenueCumulative;

                            var fifthRevenue = revenueDetails.Where(a => a.Year == currentYear + 4).FirstOrDefault();
                            if (fifthRevenue != null)
                            {
                                obj.FifthYearRevenueCumulative = (fifthRevenue.Q1New != string.Empty) ? Convert.ToDecimal(fifthRevenue.Q1New) : 0;
                            }
                            else
                            {
                                obj.FifthYearRevenueCumulative = 0;
                            }
                            FifthYearRevenueCumulative_Gross += obj.FifthYearRevenueCumulative;

                            obj.RevenueGrandTotal = (obj.CurrentYearRevenueCumulative + obj.NextYearRevenueCumulative +
                                                     obj.ThirdYearRevenueCumulative + obj.FourthYearRevenueCumulative +
                                                     obj.FifthYearRevenueCumulative);
                            RevenueGrandTotal_Gross += obj.RevenueGrandTotal;

                        }
                        //Get all Gross Margin Values
                        var grossMarginDetails = scenarioDatas.Where(a => a.ProjectId == Convert.ToInt32(ds.Tables[1].Rows[i]["ProjectId"].ToString())
                            && a.FinancialDataTypeName == "Gross Margin").ToList();

                        var currentGrossMargin = grossMarginDetails.Where(a => a.Year == currentYear).FirstOrDefault();
                        if (currentGrossMargin != null)
                        {
                            decimal cumulativeGrossMargin = 0;
                            int intervalCount = dataEntryInterval == "monthly" ? 12 : 4;

                            for (int l = 1; l <= intervalCount; l++)
                            {
                                string intervalNew = $"Q{l}New";
                                string intervalVariant = $"Q{l}Variant";

                                decimal intervalNewValue = !string.IsNullOrEmpty(currentGrossMargin.GetType().GetProperty(intervalNew)?.GetValue(currentGrossMargin, null)?.ToString())
                                    ? Convert.ToDecimal(currentGrossMargin.GetType().GetProperty(intervalNew)?.GetValue(currentGrossMargin, null))
                                    : 0;

                                decimal intervalVariantValue = !string.IsNullOrEmpty(currentGrossMargin.GetType().GetProperty(intervalVariant)?.GetValue(currentGrossMargin, null)?.ToString())
                                    ? Convert.ToDecimal(currentGrossMargin.GetType().GetProperty(intervalVariant)?.GetValue(currentGrossMargin, null))
                                    : 0;

                                // Setting the values to obj
                                obj.GetType().GetProperty($"CurrentYearGrossMargin{intervalNew}")?.SetValue(obj, intervalNewValue);
                                obj.GetType().GetProperty($"CurrentYearGrossMargin{intervalVariant}")?.SetValue(obj, intervalVariantValue);

                                cumulativeGrossMargin += intervalNewValue + intervalVariantValue;

                                // Accumulating values for gross
                                typeof(DetailedStructure).GetField($"CurrentYearGrossMargin{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                    (decimal?)typeof(DetailedStructure).GetField($"CurrentYearGrossMargin{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalNewValue);

                                typeof(DetailedStructure).GetField($"CurrentYearGrossMargin{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                    (decimal?)typeof(DetailedStructure).GetField($"CurrentYearGrossMargin{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalVariantValue);
                            }

                            obj.CurrentYearGrossMarginCumulative = cumulativeGrossMargin;
                            CurrentYearGrossMarginCumulative_Gross += cumulativeGrossMargin;
                        }

                        else
                        {
                            obj.CurrentYearGrossMarginCumulative = 0;
                            CurrentYearGrossMarginCumulative_Gross += obj.CurrentYearGrossMarginCumulative;
                        }
                        if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                        {
                            var secondGrossMargin = grossMarginDetails.Where(a => a.Year == currentYear + 1).FirstOrDefault();
                            if (secondGrossMargin != null)
                            {
                                decimal cumulativeNextYearGrossMargin = 0;
                                int intervalCount = dataEntryInterval == "monthly" ? 12 : 4;

                                for (int m = 1; m <= intervalCount; m++)
                                {
                                    string intervalNew = $"Q{m}New";
                                    string intervalVariant = $"Q{m}Variant";

                                    decimal intervalNewValue = !string.IsNullOrEmpty(secondGrossMargin.GetType().GetProperty(intervalNew)?.GetValue(secondGrossMargin, null)?.ToString())
                                        ? Convert.ToDecimal(secondGrossMargin.GetType().GetProperty(intervalNew)?.GetValue(secondGrossMargin, null))
                                        : 0;

                                    decimal intervalVariantValue = !string.IsNullOrEmpty(secondGrossMargin.GetType().GetProperty(intervalVariant)?.GetValue(secondGrossMargin, null)?.ToString())
                                        ? Convert.ToDecimal(secondGrossMargin.GetType().GetProperty(intervalVariant)?.GetValue(secondGrossMargin, null))
                                        : 0;

                                    // Setting the values to obj
                                    obj.GetType().GetProperty($"NextYearGrossMargin{intervalNew}")?.SetValue(obj, intervalNewValue);
                                    obj.GetType().GetProperty($"NextYearGrossMargin{intervalVariant}")?.SetValue(obj, intervalVariantValue);

                                    cumulativeNextYearGrossMargin += intervalNewValue + intervalVariantValue;

                                    // Accumulating values for gross
                                    typeof(DetailedStructure).GetField($"NextYearGrossMargin{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                        (decimal?)typeof(DetailedStructure).GetField($"NextYearGrossMargin{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalNewValue);

                                    typeof(DetailedStructure).GetField($"NextYearGrossMargin{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                        (decimal?)typeof(DetailedStructure).GetField($"NextYearGrossMargin{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalVariantValue);
                                }

                                obj.NextYearGrossMarginCumulative = cumulativeNextYearGrossMargin;
                                NextYearGrossMarginCumulative_Gross += cumulativeNextYearGrossMargin;
                            }

                            else
                            {
                                obj.NextYearGrossMarginCumulative = 0;
                                NextYearGrossMarginCumulative_Gross += obj.NextYearGrossMarginCumulative;
                            }
                            var thirdGrossMargin = grossMarginDetails.Where(a => a.Year == currentYear + 2).FirstOrDefault();
                            if (thirdGrossMargin != null)
                            {
                                obj.ThirdYearGrossMarginCumulative = (thirdGrossMargin.Q1New != string.Empty) ? Convert.ToDecimal(thirdGrossMargin.Q1New) : 0;
                            }
                            else
                            {
                                obj.ThirdYearGrossMarginCumulative = 0;
                            }
                            ThirdYearGrossMarginCumulative_Gross += obj.ThirdYearGrossMarginCumulative;

                            var fourthGrossMargin = grossMarginDetails.Where(a => a.Year == currentYear + 3).FirstOrDefault();
                            if (fourthGrossMargin != null)
                            {
                                obj.FourthYearGrossMarginCumulative = (fourthGrossMargin.Q1New != string.Empty) ? Convert.ToDecimal(fourthGrossMargin.Q1New) : 0;
                            }
                            else
                            {
                                obj.FourthYearGrossMarginCumulative = 0;
                            }
                            FourthYearGrossMarginCumulative_Gross += obj.FourthYearGrossMarginCumulative;

                            var fifthGrossMargin = grossMarginDetails.Where(a => a.Year == currentYear + 4).FirstOrDefault();
                            if (fifthGrossMargin != null)
                            {
                                obj.FifthYearGrossMarginCumulative = (fifthGrossMargin.Q1New != string.Empty) ? Convert.ToDecimal(fifthGrossMargin.Q1New) : 0;
                            }
                            else
                            {
                                obj.FifthYearGrossMarginCumulative = 0;
                            }
                            FifthYearGrossMarginCumulative_Gross += obj.FifthYearGrossMarginCumulative;

                            obj.GrossMarginGrandTotal = (obj.CurrentYearGrossMarginCumulative + obj.NextYearGrossMarginCumulative +
                                                     obj.ThirdYearGrossMarginCumulative + obj.FourthYearGrossMarginCumulative +
                                                     obj.FifthYearGrossMarginCumulative);
                            GrossMarginGrandTotal_Gross += obj.GrossMarginGrandTotal;

                        }
                        //Get all ManHours Values

                        var manHoursDetails = scenarioDatas.Where(a => a.ProjectId == Convert.ToInt32(ds.Tables[1].Rows[i]["ProjectId"].ToString())
                            && a.FinancialDataTypeName == "Man Hours").ToList();

                        var currentManHours = manHoursDetails.Where(a => a.Year == currentYear).FirstOrDefault();
                        if (currentManHours != null)
                        {
                            decimal cumulativeCurrentYearManHours = 0;
                            int intervalCount = dataEntryInterval == "monthly" ? 12 : 4;

                            for (int n = 1; n <= intervalCount; n++)
                            {
                                string intervalNew = $"Q{n}New";
                                string intervalVariant = $"Q{n}Variant";

                                decimal intervalNewValue = !string.IsNullOrEmpty(currentManHours.GetType().GetProperty(intervalNew)?.GetValue(currentManHours, null)?.ToString())
                                    ? Convert.ToDecimal(currentManHours.GetType().GetProperty(intervalNew)?.GetValue(currentManHours, null))
                                    : 0;

                                decimal intervalVariantValue = !string.IsNullOrEmpty(currentManHours.GetType().GetProperty(intervalVariant)?.GetValue(currentManHours, null)?.ToString())
                                    ? Convert.ToDecimal(currentManHours.GetType().GetProperty(intervalVariant)?.GetValue(currentManHours, null))
                                    : 0;

                                // Setting the values to obj
                                obj.GetType().GetProperty($"CurrentYearManHours{intervalNew}")?.SetValue(obj, intervalNewValue);
                                obj.GetType().GetProperty($"CurrentYearManHours{intervalVariant}")?.SetValue(obj, intervalVariantValue);

                                cumulativeCurrentYearManHours += intervalNewValue + intervalVariantValue;

                                // Accumulating values for gross
                                typeof(DetailedStructure).GetField($"CurrentYearManHours{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                    (decimal?)typeof(DetailedStructure).GetField($"CurrentYearManHours{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalNewValue);

                                typeof(DetailedStructure).GetField($"CurrentYearManHours{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                    (decimal?)typeof(DetailedStructure).GetField($"CurrentYearManHours{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalVariantValue);
                            }

                            obj.CurrentYearManHoursCumulative = cumulativeCurrentYearManHours;
                            CurrentYearManHoursCumulative_Gross += cumulativeCurrentYearManHours;
                        }

                        else
                        {
                            obj.CurrentYearManHoursCumulative = 0;
                            CurrentYearManHoursCumulative_Gross += obj.CurrentYearManHoursCumulative;
                        }
                        if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                        {
                            var secondManHours = manHoursDetails.Where(a => a.Year == currentYear + 1).FirstOrDefault();
                            if (secondManHours != null)
                            {
                                decimal cumulativeNextYearManHours = 0;
                                int intervalCount = dataEntryInterval == "monthly" ? 12 : 4;

                                for (int o = 1; o <= intervalCount; o++)
                                {
                                    string intervalNew = $"Q{o}New";
                                    string intervalVariant = $"Q{o}Variant";

                                    decimal intervalNewValue = !string.IsNullOrEmpty(secondManHours.GetType().GetProperty(intervalNew)?.GetValue(secondManHours, null)?.ToString())
                                        ? Convert.ToDecimal(secondManHours.GetType().GetProperty(intervalNew)?.GetValue(secondManHours, null))
                                        : 0;

                                    decimal intervalVariantValue = !string.IsNullOrEmpty(secondManHours.GetType().GetProperty(intervalVariant)?.GetValue(secondManHours, null)?.ToString())
                                        ? Convert.ToDecimal(secondManHours.GetType().GetProperty(intervalVariant)?.GetValue(secondManHours, null))
                                        : 0;

                                    // Setting the values to obj
                                    obj.GetType().GetProperty($"NextYearManHours{intervalNew}")?.SetValue(obj, intervalNewValue);
                                    obj.GetType().GetProperty($"NextYearManHours{intervalVariant}")?.SetValue(obj, intervalVariantValue);

                                    cumulativeNextYearManHours += intervalNewValue + intervalVariantValue;

                                    // Accumulating values for gross
                                    typeof(DetailedStructure).GetField($"NextYearManHours{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                        (decimal?)typeof(DetailedStructure).GetField($"NextYearManHours{intervalNew}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalNewValue);

                                    typeof(DetailedStructure).GetField($"NextYearManHours{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(this,
                                        (decimal?)typeof(DetailedStructure).GetField($"NextYearManHours{intervalVariant}_Gross", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) + intervalVariantValue);
                                }

                                obj.NextYearManHoursCumulative = cumulativeNextYearManHours;
                                NextYearManHoursCumulative_Gross += cumulativeNextYearManHours;
                            }

                            else
                            {
                                obj.NextYearManHoursCumulative = 0;
                                NextYearManHoursCumulative_Gross += obj.NextYearManHoursCumulative;
                            }
                            var thirdManHours = manHoursDetails.Where(a => a.Year == currentYear + 2).FirstOrDefault();
                            if (thirdManHours != null)
                            {
                                obj.ThirdYearManHoursCumulative = (thirdManHours.Q1New != string.Empty) ? Convert.ToDecimal(thirdManHours.Q1New) : 0;
                            }
                            else
                            {
                                obj.ThirdYearManHoursCumulative = 0;
                            }
                            ThirdYearManHoursCumulative_Gross += obj.ThirdYearManHoursCumulative;

                            var fourthManHours = manHoursDetails.Where(a => a.Year == currentYear + 3).FirstOrDefault();
                            if (fourthManHours != null)
                            {
                                obj.FourthYearManHoursCumulative = (fourthManHours.Q1New != string.Empty) ? Convert.ToDecimal(fourthManHours.Q1New) : 0;
                            }
                            else
                            {
                                obj.FourthYearManHoursCumulative = 0;
                            }
                            FourthYearManHoursCumulative_Gross += obj.FourthYearManHoursCumulative;

                            var fifthManHours = manHoursDetails.Where(a => a.Year == currentYear + 4).FirstOrDefault();
                            if (fifthManHours != null)
                            {
                                obj.FifthYearManHoursCumulative = (fifthManHours.Q1New != string.Empty) ? Convert.ToDecimal(fifthManHours.Q1New) : 0;
                            }
                            else
                            {
                                obj.FifthYearManHoursCumulative = 0;
                            }
                            FifthYearManHoursCumulative_Gross += obj.FifthYearManHoursCumulative;

                            obj.ManHoursGrandTotal = (obj.CurrentYearManHoursCumulative + obj.NextYearManHoursCumulative +
                                                     obj.ThirdYearManHoursCumulative + obj.FourthYearManHoursCumulative +
                                                     obj.FifthYearManHoursCumulative);
                            ManHoursGrandTotal_Gross += obj.ManHoursGrandTotal;

                        }
                        if (scenarioScopeCode == "PL" && scenarioTypeCode == "AC")
                        {
                            var costOfSalesDetails = scenarioDatas.Where(a => a.ProjectId == Convert.ToInt32(ds.Tables[1].Rows[i]["ProjectId"].ToString())
                                                        && a.FinancialDataTypeName == "Cost Of Sales").ToList();

                            var currentCostOfSales = costOfSalesDetails.Where(a => a.Year == currentYear).FirstOrDefault();
                            if (currentCostOfSales != null)
                            {
                                obj.CurrentYearCostOfSalesQ1New = (currentCostOfSales.Q1New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q1New) : 0;
                                obj.CurrentYearCostOfSalesQ2New = (currentCostOfSales.Q2New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q2New) : 0;
                                obj.CurrentYearCostOfSalesQ3New = (currentCostOfSales.Q3New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q3New) : 0;
                                obj.CurrentYearCostOfSalesQ4New = (currentCostOfSales.Q4New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q4New) : 0;
                                obj.CurrentYearCostOfSalesQ5New = (currentCostOfSales.Q5New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q5New) : 0;
                                obj.CurrentYearCostOfSalesQ6New = (currentCostOfSales.Q6New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q6New) : 0;
                                obj.CurrentYearCostOfSalesQ7New = (currentCostOfSales.Q7New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q7New) : 0;
                                obj.CurrentYearCostOfSalesQ8New = (currentCostOfSales.Q8New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q8New) : 0;
                                obj.CurrentYearCostOfSalesQ9New = (currentCostOfSales.Q9New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q9New) : 0;
                                obj.CurrentYearCostOfSalesQ10New = (currentCostOfSales.Q10New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q10New) : 0;
                                obj.CurrentYearCostOfSalesQ11New = (currentCostOfSales.Q11New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q11New) : 0;
                                obj.CurrentYearCostOfSalesQ12New = (currentCostOfSales.Q12New != string.Empty) ? Convert.ToDecimal(currentCostOfSales.Q12New) : 0;

                                obj.CostOfSalesGrandTotal = obj.CurrentYearCostOfSalesQ1New + obj.CurrentYearCostOfSalesQ2New +
                                                            obj.CurrentYearCostOfSalesQ3New + obj.CurrentYearCostOfSalesQ4New +
                                                            obj.CurrentYearCostOfSalesQ5New + obj.CurrentYearCostOfSalesQ6New +
                                                            obj.CurrentYearCostOfSalesQ7New + obj.CurrentYearCostOfSalesQ8New +
                                                            obj.CurrentYearCostOfSalesQ9New + obj.CurrentYearCostOfSalesQ10New +
                                                            obj.CurrentYearCostOfSalesQ11New + obj.CurrentYearCostOfSalesQ12New;

                                CurrentYearCostOfSalesQ1New_Gross += obj.CurrentYearCostOfSalesQ1New;
                                CurrentYearCostOfSalesQ2New_Gross += obj.CurrentYearCostOfSalesQ2New;
                                CurrentYearCostOfSalesQ3New_Gross += obj.CurrentYearCostOfSalesQ3New;
                                CurrentYearCostOfSalesQ4New_Gross += obj.CurrentYearCostOfSalesQ4New;
                                CurrentYearCostOfSalesQ5New_Gross += obj.CurrentYearCostOfSalesQ5New;
                                CurrentYearCostOfSalesQ6New_Gross += obj.CurrentYearCostOfSalesQ6New;
                                CurrentYearCostOfSalesQ7New_Gross += obj.CurrentYearCostOfSalesQ7New;
                                CurrentYearCostOfSalesQ8New_Gross += obj.CurrentYearCostOfSalesQ8New;
                                CurrentYearCostOfSalesQ9New_Gross += obj.CurrentYearCostOfSalesQ9New;
                                CurrentYearCostOfSalesQ10New_Gross += obj.CurrentYearCostOfSalesQ10New;
                                CurrentYearCostOfSalesQ11New_Gross += obj.CurrentYearCostOfSalesQ11New;
                                CurrentYearCostOfSalesQ12New_Gross += obj.CurrentYearCostOfSalesQ12New;
                                CostOfSalesGrandTotal_Gross += obj.CostOfSalesGrandTotal;
                            }

                            else
                            {
                                obj.CostOfSalesGrandTotal = 0;
                                CostOfSalesGrandTotal_Gross += obj.CostOfSalesGrandTotal;
                            }
                            var provForFutureLoss = scenarioDatas.Where(a => a.ProjectId == Convert.ToInt32(ds.Tables[1].Rows[i]["ProjectId"].ToString())
                                                       && a.FinancialDataTypeName == "Prov. for Future Losses").ToList();

                            var currentProvForFutureLoss = provForFutureLoss.Where(a => a.Year == currentYear).FirstOrDefault();
                            if (currentProvForFutureLoss != null)
                            {
                                obj.CurrentYearProvFutureLossQ1New = (currentProvForFutureLoss.Q1New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q1New) : 0;
                                obj.CurrentYearProvFutureLossQ2New = (currentProvForFutureLoss.Q2New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q2New) : 0;
                                obj.CurrentYearProvFutureLossQ3New = (currentProvForFutureLoss.Q3New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q3New) : 0;
                                obj.CurrentYearProvFutureLossQ4New = (currentProvForFutureLoss.Q4New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q4New) : 0;
                                obj.CurrentYearProvFutureLossQ5New = (currentProvForFutureLoss.Q5New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q5New) : 0;
                                obj.CurrentYearProvFutureLossQ6New = (currentProvForFutureLoss.Q6New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q6New) : 0;
                                obj.CurrentYearProvFutureLossQ7New = (currentProvForFutureLoss.Q7New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q7New) : 0;
                                obj.CurrentYearProvFutureLossQ8New = (currentProvForFutureLoss.Q8New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q8New) : 0;
                                obj.CurrentYearProvFutureLossQ9New = (currentProvForFutureLoss.Q9New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q9New) : 0;
                                obj.CurrentYearProvFutureLossQ10New = (currentProvForFutureLoss.Q10New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q10New) : 0;
                                obj.CurrentYearProvFutureLossQ11New = (currentProvForFutureLoss.Q11New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q11New) : 0;
                                obj.CurrentYearProvFutureLossQ12New = (currentProvForFutureLoss.Q12New != string.Empty) ? Convert.ToDecimal(currentProvForFutureLoss.Q12New) : 0;

                                obj.ProvFutureLossGrandTotal = obj.CurrentYearProvFutureLossQ1New + obj.CurrentYearProvFutureLossQ2New +
                                                               obj.CurrentYearProvFutureLossQ3New + obj.CurrentYearProvFutureLossQ4New +
                                                               obj.CurrentYearProvFutureLossQ5New + obj.CurrentYearProvFutureLossQ6New +
                                                               obj.CurrentYearProvFutureLossQ7New + obj.CurrentYearProvFutureLossQ8New +
                                                               obj.CurrentYearProvFutureLossQ9New + obj.CurrentYearProvFutureLossQ10New +
                                                               obj.CurrentYearProvFutureLossQ11New + obj.CurrentYearProvFutureLossQ12New;

                                CurrentYearProvFutureLossQ1New_Gross += obj.CurrentYearProvFutureLossQ1New;
                                CurrentYearProvFutureLossQ2New_Gross += obj.CurrentYearProvFutureLossQ2New;
                                CurrentYearProvFutureLossQ3New_Gross += obj.CurrentYearProvFutureLossQ3New;
                                CurrentYearProvFutureLossQ4New_Gross += obj.CurrentYearProvFutureLossQ4New;
                                CurrentYearProvFutureLossQ5New_Gross += obj.CurrentYearProvFutureLossQ5New;
                                CurrentYearProvFutureLossQ6New_Gross += obj.CurrentYearProvFutureLossQ6New;
                                CurrentYearProvFutureLossQ7New_Gross += obj.CurrentYearProvFutureLossQ7New;
                                CurrentYearProvFutureLossQ8New_Gross += obj.CurrentYearProvFutureLossQ8New;
                                CurrentYearProvFutureLossQ9New_Gross += obj.CurrentYearProvFutureLossQ9New;
                                CurrentYearProvFutureLossQ10New_Gross += obj.CurrentYearProvFutureLossQ10New;
                                CurrentYearProvFutureLossQ11New_Gross += obj.CurrentYearProvFutureLossQ11New;
                                CurrentYearProvFutureLossQ12New_Gross += obj.CurrentYearProvFutureLossQ12New;

                                ProvFutureLossGrandTotal_Gross += obj.ProvFutureLossGrandTotal;
                            }

                            else
                            {
                                obj.ProvFutureLossGrandTotal = 0;
                                ProvFutureLossGrandTotal_Gross += obj.ProvFutureLossGrandTotal;
                            }
                        }

                        DetailedStructures.Add(obj);
                    }

                    DetailedStructure obj1 = new DetailedStructure();
                    obj1.ProjectName = "GrandTotal";
                    obj1.CurrentYearRevenueQ1New = CurrentYearRevenueQ1New_Gross;
                    obj1.CurrentYearRevenueQ1Variant = CurrentYearRevenueQ1Variant_Gross;
                    obj1.CurrentYearRevenueQ2New = CurrentYearRevenueQ2New_Gross;
                    obj1.CurrentYearRevenueQ2Variant = CurrentYearRevenueQ2Variant_Gross;
                    obj1.CurrentYearRevenueQ3New = CurrentYearRevenueQ3New_Gross;
                    obj1.CurrentYearRevenueQ3Variant = CurrentYearRevenueQ3Variant_Gross;
                    obj1.CurrentYearRevenueQ4New = CurrentYearRevenueQ4New_Gross;
                    obj1.CurrentYearRevenueQ4Variant = CurrentYearRevenueQ4Variant_Gross;
                    obj1.CurrentYearRevenueQ5New = CurrentYearRevenueQ5New_Gross;
                    obj1.CurrentYearRevenueQ5Variant = CurrentYearRevenueQ5Variant_Gross;
                    obj1.CurrentYearRevenueQ6New = CurrentYearRevenueQ6New_Gross;
                    obj1.CurrentYearRevenueQ6Variant = CurrentYearRevenueQ6Variant_Gross;
                    obj1.CurrentYearRevenueQ7New = CurrentYearRevenueQ7New_Gross;
                    obj1.CurrentYearRevenueQ7Variant = CurrentYearRevenueQ7Variant_Gross;
                    obj1.CurrentYearRevenueQ8New = CurrentYearRevenueQ8New_Gross;
                    obj1.CurrentYearRevenueQ8Variant = CurrentYearRevenueQ8Variant_Gross;
                    obj1.CurrentYearRevenueQ9New = CurrentYearRevenueQ9New_Gross;
                    obj1.CurrentYearRevenueQ9Variant = CurrentYearRevenueQ9Variant_Gross;
                    obj1.CurrentYearRevenueQ10New = CurrentYearRevenueQ10New_Gross;
                    obj1.CurrentYearRevenueQ10Variant = CurrentYearRevenueQ10Variant_Gross;
                    obj1.CurrentYearRevenueQ11New = CurrentYearRevenueQ11New_Gross;
                    obj1.CurrentYearRevenueQ11Variant = CurrentYearRevenueQ11Variant_Gross;
                    obj1.CurrentYearRevenueQ12New = CurrentYearRevenueQ12New_Gross;
                    obj1.CurrentYearRevenueQ12Variant = CurrentYearRevenueQ12Variant_Gross;

                    obj1.CurrentYearRevenueCumulative = CurrentYearRevenueCumulative_Gross;

                    obj1.NextYearRevenueQ1New = NextYearRevenueQ1New_Gross;
                    obj1.NextYearRevenueQ1Variant = NextYearRevenueQ1Variant_Gross;
                    obj1.NextYearRevenueQ2New = NextYearRevenueQ2New_Gross;
                    obj1.NextYearRevenueQ2Variant = NextYearRevenueQ2Variant_Gross;
                    obj1.NextYearRevenueQ3New = NextYearRevenueQ3New_Gross;
                    obj1.NextYearRevenueQ3Variant = NextYearRevenueQ3Variant_Gross;
                    obj1.NextYearRevenueQ4New = NextYearRevenueQ4New_Gross;
                    obj1.NextYearRevenueQ4Variant = NextYearRevenueQ4Variant_Gross;
                    obj1.NextYearRevenueQ5New = NextYearRevenueQ5New_Gross;
                    obj1.NextYearRevenueQ5Variant = NextYearRevenueQ5Variant_Gross;
                    obj1.NextYearRevenueQ6New = NextYearRevenueQ6New_Gross;
                    obj1.NextYearRevenueQ6Variant = NextYearRevenueQ6Variant_Gross;
                    obj1.NextYearRevenueQ7New = NextYearRevenueQ7New_Gross;
                    obj1.NextYearRevenueQ7Variant = NextYearRevenueQ7Variant_Gross;
                    obj1.NextYearRevenueQ8New = NextYearRevenueQ8New_Gross;
                    obj1.NextYearRevenueQ8Variant = NextYearRevenueQ8Variant_Gross;
                    obj1.NextYearRevenueQ9New = NextYearRevenueQ9New_Gross;
                    obj1.NextYearRevenueQ9Variant = NextYearRevenueQ9Variant_Gross;
                    obj1.NextYearRevenueQ10New = NextYearRevenueQ10New_Gross;
                    obj1.NextYearRevenueQ10Variant = NextYearRevenueQ10Variant_Gross;
                    obj1.NextYearRevenueQ11New = NextYearRevenueQ11New_Gross;
                    obj1.NextYearRevenueQ11Variant = NextYearRevenueQ11Variant_Gross;
                    obj1.NextYearRevenueQ12New = NextYearRevenueQ12New_Gross;
                    obj1.NextYearRevenueQ12Variant = NextYearRevenueQ12Variant_Gross;

                    obj1.NextYearRevenueCumulative = NextYearRevenueCumulative_Gross;

                    obj1.ThirdYearRevenueCumulative = ThirdYearRevenueCumulative_Gross;
                    obj1.FourthYearRevenueCumulative = FourthYearRevenueCumulative_Gross;
                    obj1.FifthYearRevenueCumulative = FifthYearRevenueCumulative_Gross;

                    obj1.RevenueGrandTotal = RevenueGrandTotal_Gross;

                    obj1.CurrentYearGrossMarginQ1New = CurrentYearGrossMarginQ1New_Gross;
                    obj1.CurrentYearGrossMarginQ1Variant = CurrentYearGrossMarginQ1Variant_Gross;
                    obj1.CurrentYearGrossMarginQ2New = CurrentYearGrossMarginQ2New_Gross;
                    obj1.CurrentYearGrossMarginQ2Variant = CurrentYearGrossMarginQ2Variant_Gross;
                    obj1.CurrentYearGrossMarginQ3New = CurrentYearGrossMarginQ3New_Gross;
                    obj1.CurrentYearGrossMarginQ3Variant = CurrentYearGrossMarginQ3Variant_Gross;
                    obj1.CurrentYearGrossMarginQ4New = CurrentYearGrossMarginQ4New_Gross;
                    obj1.CurrentYearGrossMarginQ4Variant = CurrentYearGrossMarginQ4Variant_Gross;
                    obj1.CurrentYearGrossMarginQ5New = CurrentYearGrossMarginQ5New_Gross;
                    obj1.CurrentYearGrossMarginQ5Variant = CurrentYearGrossMarginQ5Variant_Gross;
                    obj1.CurrentYearGrossMarginQ6New = CurrentYearGrossMarginQ6New_Gross;
                    obj1.CurrentYearGrossMarginQ6Variant = CurrentYearGrossMarginQ6Variant_Gross;
                    obj1.CurrentYearGrossMarginQ7New = CurrentYearGrossMarginQ7New_Gross;
                    obj1.CurrentYearGrossMarginQ7Variant = CurrentYearGrossMarginQ7Variant_Gross;
                    obj1.CurrentYearGrossMarginQ8New = CurrentYearGrossMarginQ8New_Gross;
                    obj1.CurrentYearGrossMarginQ8Variant = CurrentYearGrossMarginQ8Variant_Gross;
                    obj1.CurrentYearGrossMarginQ9New = CurrentYearGrossMarginQ9New_Gross;
                    obj1.CurrentYearGrossMarginQ9Variant = CurrentYearGrossMarginQ9Variant_Gross;
                    obj1.CurrentYearGrossMarginQ10New = CurrentYearGrossMarginQ10New_Gross;
                    obj1.CurrentYearGrossMarginQ10Variant = CurrentYearGrossMarginQ10Variant_Gross;
                    obj1.CurrentYearGrossMarginQ11New = CurrentYearGrossMarginQ11New_Gross;
                    obj1.CurrentYearGrossMarginQ11Variant = CurrentYearGrossMarginQ11Variant_Gross;
                    obj1.CurrentYearGrossMarginQ12New = CurrentYearGrossMarginQ12New_Gross;
                    obj1.CurrentYearGrossMarginQ12Variant = CurrentYearGrossMarginQ12Variant_Gross;

                    obj1.CurrentYearGrossMarginCumulative = CurrentYearGrossMarginCumulative_Gross;

                    obj1.NextYearGrossMarginQ1New = NextYearGrossMarginQ1New_Gross;
                    obj1.NextYearGrossMarginQ1Variant = NextYearGrossMarginQ1Variant_Gross;
                    obj1.NextYearGrossMarginQ2New = NextYearGrossMarginQ2New_Gross;
                    obj1.NextYearGrossMarginQ2Variant = NextYearGrossMarginQ2Variant_Gross;
                    obj1.NextYearGrossMarginQ3New = NextYearGrossMarginQ3New_Gross;
                    obj1.NextYearGrossMarginQ3Variant = NextYearGrossMarginQ3Variant_Gross;
                    obj1.NextYearGrossMarginQ4New = NextYearGrossMarginQ4New_Gross;
                    obj1.NextYearGrossMarginQ4Variant = NextYearGrossMarginQ4Variant_Gross;
                    obj1.NextYearGrossMarginQ5New = NextYearGrossMarginQ5New_Gross;
                    obj1.NextYearGrossMarginQ5Variant = NextYearGrossMarginQ5Variant_Gross;
                    obj1.NextYearGrossMarginQ6New = NextYearGrossMarginQ6New_Gross;
                    obj1.NextYearGrossMarginQ6Variant = NextYearGrossMarginQ6Variant_Gross;
                    obj1.NextYearGrossMarginQ7New = NextYearGrossMarginQ7New_Gross;
                    obj1.NextYearGrossMarginQ7Variant = NextYearGrossMarginQ7Variant_Gross;
                    obj1.NextYearGrossMarginQ8New = NextYearGrossMarginQ8New_Gross;
                    obj1.NextYearGrossMarginQ8Variant = NextYearGrossMarginQ8Variant_Gross;
                    obj1.NextYearGrossMarginQ9New = NextYearGrossMarginQ9New_Gross;
                    obj1.NextYearGrossMarginQ9Variant = NextYearGrossMarginQ9Variant_Gross;
                    obj1.NextYearGrossMarginQ10New = NextYearGrossMarginQ10New_Gross;
                    obj1.NextYearGrossMarginQ10Variant = NextYearGrossMarginQ10Variant_Gross;
                    obj1.NextYearGrossMarginQ11New = NextYearGrossMarginQ11New_Gross;
                    obj1.NextYearGrossMarginQ11Variant = NextYearGrossMarginQ11Variant_Gross;
                    obj1.NextYearGrossMarginQ12New = NextYearGrossMarginQ12New_Gross;
                    obj1.NextYearGrossMarginQ12Variant = NextYearGrossMarginQ12Variant_Gross;

                    obj1.NextYearGrossMarginCumulative = NextYearGrossMarginCumulative_Gross;

                    obj1.ThirdYearGrossMarginCumulative = ThirdYearGrossMarginCumulative_Gross;
                    obj1.FourthYearGrossMarginCumulative = FourthYearGrossMarginCumulative_Gross;
                    obj1.FifthYearGrossMarginCumulative = FifthYearGrossMarginCumulative_Gross;

                    obj1.GrossMarginGrandTotal = GrossMarginGrandTotal_Gross;

                    obj1.CurrentYearManHoursQ1New = CurrentYearManHoursQ1New_Gross;
                    obj1.CurrentYearManHoursQ1Variant = CurrentYearManHoursQ1Variant_Gross;
                    obj1.CurrentYearManHoursQ2New = CurrentYearManHoursQ2New_Gross;
                    obj1.CurrentYearManHoursQ2Variant = CurrentYearManHoursQ2Variant_Gross;
                    obj1.CurrentYearManHoursQ3New = CurrentYearManHoursQ3New_Gross;
                    obj1.CurrentYearManHoursQ3Variant = CurrentYearManHoursQ3Variant_Gross;
                    obj1.CurrentYearManHoursQ4New = CurrentYearManHoursQ4New_Gross;
                    obj1.CurrentYearManHoursQ4Variant = CurrentYearManHoursQ4Variant_Gross;
                    obj1.CurrentYearManHoursQ5New = CurrentYearManHoursQ5New_Gross;
                    obj1.CurrentYearManHoursQ5Variant = CurrentYearManHoursQ5Variant_Gross;
                    obj1.CurrentYearManHoursQ6New = CurrentYearManHoursQ6New_Gross;
                    obj1.CurrentYearManHoursQ6Variant = CurrentYearManHoursQ6Variant_Gross;
                    obj1.CurrentYearManHoursQ7New = CurrentYearManHoursQ7New_Gross;
                    obj1.CurrentYearManHoursQ7Variant = CurrentYearManHoursQ7Variant_Gross;
                    obj1.CurrentYearManHoursQ8New = CurrentYearManHoursQ8New_Gross;
                    obj1.CurrentYearManHoursQ8Variant = CurrentYearManHoursQ8Variant_Gross;
                    obj1.CurrentYearManHoursQ9New = CurrentYearManHoursQ9New_Gross;
                    obj1.CurrentYearManHoursQ9Variant = CurrentYearManHoursQ9Variant_Gross;
                    obj1.CurrentYearManHoursQ10New = CurrentYearManHoursQ10New_Gross;
                    obj1.CurrentYearManHoursQ10Variant = CurrentYearManHoursQ10Variant_Gross;
                    obj1.CurrentYearManHoursQ11New = CurrentYearManHoursQ11New_Gross;
                    obj1.CurrentYearManHoursQ11Variant = CurrentYearManHoursQ11Variant_Gross;
                    obj1.CurrentYearManHoursQ12New = CurrentYearManHoursQ12New_Gross;
                    obj1.CurrentYearManHoursQ12Variant = CurrentYearManHoursQ12Variant_Gross;

                    obj1.CurrentYearManHoursCumulative = CurrentYearManHoursCumulative_Gross;

                    obj1.NextYearManHoursQ1New = NextYearManHoursQ1New_Gross;
                    obj1.NextYearManHoursQ1Variant = NextYearManHoursQ1Variant_Gross;
                    obj1.NextYearManHoursQ2New = NextYearManHoursQ2New_Gross;
                    obj1.NextYearManHoursQ2Variant = NextYearManHoursQ2Variant_Gross;
                    obj1.NextYearManHoursQ3New = NextYearManHoursQ3New_Gross;
                    obj1.NextYearManHoursQ3Variant = NextYearManHoursQ3Variant_Gross;
                    obj1.NextYearManHoursQ4New = NextYearManHoursQ4New_Gross;
                    obj1.NextYearManHoursQ4Variant = NextYearManHoursQ4Variant_Gross;
                    obj1.NextYearManHoursQ5New = NextYearManHoursQ5New_Gross;
                    obj1.NextYearManHoursQ5Variant = NextYearManHoursQ5Variant_Gross;
                    obj1.NextYearManHoursQ6New = NextYearManHoursQ6New_Gross;
                    obj1.NextYearManHoursQ6Variant = NextYearManHoursQ6Variant_Gross;
                    obj1.NextYearManHoursQ7New = NextYearManHoursQ7New_Gross;
                    obj1.NextYearManHoursQ7Variant = NextYearManHoursQ7Variant_Gross;
                    obj1.NextYearManHoursQ8New = NextYearManHoursQ8New_Gross;
                    obj1.NextYearManHoursQ8Variant = NextYearManHoursQ8Variant_Gross;
                    obj1.NextYearManHoursQ9New = NextYearManHoursQ9New_Gross;
                    obj1.NextYearManHoursQ9Variant = NextYearManHoursQ9Variant_Gross;
                    obj1.NextYearManHoursQ10New = NextYearManHoursQ10New_Gross;
                    obj1.NextYearManHoursQ10Variant = NextYearManHoursQ10Variant_Gross;
                    obj1.NextYearManHoursQ11New = NextYearManHoursQ11New_Gross;
                    obj1.NextYearManHoursQ11Variant = NextYearManHoursQ11Variant_Gross;
                    obj1.NextYearManHoursQ12New = NextYearManHoursQ12New_Gross;
                    obj1.NextYearManHoursQ12Variant = NextYearManHoursQ12Variant_Gross;

                    obj1.NextYearManHoursCumulative = NextYearManHoursCumulative_Gross;

                    obj1.ThirdYearManHoursCumulative = ThirdYearManHoursCumulative_Gross;
                    obj1.FourthYearManHoursCumulative = FourthYearManHoursCumulative_Gross;
                    obj1.FifthYearManHoursCumulative = FifthYearManHoursCumulative_Gross;

                    obj1.ManHoursGrandTotal = ManHoursGrandTotal_Gross;

                    obj1.CurrentYearCostOfSalesQ1New = CurrentYearCostOfSalesQ1New_Gross;
                    obj1.CurrentYearCostOfSalesQ2New = CurrentYearCostOfSalesQ2New_Gross;
                    obj1.CurrentYearCostOfSalesQ3New = CurrentYearCostOfSalesQ3New_Gross;
                    obj1.CurrentYearCostOfSalesQ4New = CurrentYearCostOfSalesQ4New_Gross;
                    obj1.CurrentYearCostOfSalesQ5New = CurrentYearCostOfSalesQ5New_Gross;
                    obj1.CurrentYearCostOfSalesQ6New = CurrentYearCostOfSalesQ6New_Gross;
                    obj1.CurrentYearCostOfSalesQ7New = CurrentYearCostOfSalesQ7New_Gross;
                    obj1.CurrentYearCostOfSalesQ8New = CurrentYearCostOfSalesQ8New_Gross;
                    obj1.CurrentYearCostOfSalesQ9New = CurrentYearCostOfSalesQ9New_Gross;
                    obj1.CurrentYearCostOfSalesQ10New = CurrentYearCostOfSalesQ10New_Gross;
                    obj1.CurrentYearCostOfSalesQ11New = CurrentYearCostOfSalesQ11New_Gross;
                    obj1.CurrentYearCostOfSalesQ12New = CurrentYearCostOfSalesQ12New_Gross;

                    obj1.CostOfSalesGrandTotal = CostOfSalesGrandTotal_Gross;

                    obj1.CurrentYearProvFutureLossQ1New = CurrentYearProvFutureLossQ1New_Gross;
                    obj1.CurrentYearProvFutureLossQ2New = CurrentYearProvFutureLossQ2New_Gross;
                    obj1.CurrentYearProvFutureLossQ3New = CurrentYearProvFutureLossQ3New_Gross;
                    obj1.CurrentYearProvFutureLossQ4New = CurrentYearProvFutureLossQ4New_Gross;
                    obj1.CurrentYearProvFutureLossQ5New = CurrentYearProvFutureLossQ5New_Gross;
                    obj1.CurrentYearProvFutureLossQ6New = CurrentYearProvFutureLossQ6New_Gross;
                    obj1.CurrentYearProvFutureLossQ7New = CurrentYearProvFutureLossQ7New_Gross;
                    obj1.CurrentYearProvFutureLossQ8New = CurrentYearProvFutureLossQ8New_Gross;
                    obj1.CurrentYearProvFutureLossQ9New = CurrentYearProvFutureLossQ9New_Gross;
                    obj1.CurrentYearProvFutureLossQ10New = CurrentYearProvFutureLossQ10New_Gross;
                    obj1.CurrentYearProvFutureLossQ11New = CurrentYearProvFutureLossQ11New_Gross;
                    obj1.CurrentYearProvFutureLossQ12New = CurrentYearProvFutureLossQ12New_Gross;

                    obj1.ProvFutureLossGrandTotal = ProvFutureLossGrandTotal_Gross;


                    DetailedStructures.Add(obj1);

                    listScenarioProject.Details = DetailedStructures;
                    listScenarioProject.Headers = listHeaderStructure;
                }

                return listScenarioProject;
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
