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
        public int ClearScenarioData(int ScenarioId, string DeletedBy)
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
                            Q4Variant = (ds.Tables[3].Rows[row]["Q4Variant"] != null) ? ds.Tables[3].Rows[row]["Q4Variant"].ToString() : string.Empty
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

                                Q1Lock = q1Lock,
                                Q2Lock = q2Lock,
                                Q3Lock = q3Lock,
                                Q4Lock = q4Lock
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
                                                 new XElement("Q4Variant", ObjDetails.Q4Variant)));
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

                if ((ds != null) && (ds.Tables.Count > 5) && (ds.Tables[5] != null) && (ds.Tables[5].Rows.Count > 0))
                {
                    financialYear = Convert.ToInt32(ds.Tables[5].Rows[0][2]);
                    scenarioScopeCode = Convert.ToString(ds.Tables[5].Rows[0][0]);
                    scenarioTypeCode = Convert.ToString(ds.Tables[5].Rows[0][1]);
                }

                var scenarioLayout = new List<ScenarioLayout>();
                List<string> defaultyQuarters = new List<string>() { "q1", "q2", "q3", "q4" };
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
                                                               qName = q.Quarter.ToLower()
                                                           }).ToList() : new List<QuartersLayOut>()
                                                               {
                                                                   new QuartersLayOut{ qName = "q1",qLock =false},
                                                                   new QuartersLayOut{ qName = "q2",qLock =false},
                                                                   new QuartersLayOut{ qName = "q3",qLock =false},
                                                                   new QuartersLayOut{ qName = "q4",qLock =false}
                                                               },
                        QuarterApplicable = true,
                        FinancialDataTypes = new List<FinancialDataTypeMaster>(financialDataTypes),
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
                                                                   qName = q.Quarter.ToLower()
                                                               }).ToList() : new List<QuartersLayOut>()
                                                               {
                                                                   new QuartersLayOut{ qName = "q1",qLock =false},
                                                                   new QuartersLayOut{ qName = "q2",qLock =false},
                                                                   new QuartersLayOut{ qName = "q3",qLock =false},
                                                                   new QuartersLayOut{ qName = "q4",qLock =false}
                                                               },
                            QuarterApplicable = (Convert.ToInt32(year) > (Convert.ToInt32(financialYear) + 1)) ? false : true,
                        };
                        layout.FinancialDataTypes.AddRange(financialDataTypes);
                        //foreach (var dataType in financialDataTypes)
                        //{
                        //    layout.FinancialDataTypes.Add(dataType.FinancialDataTypeName);
                        //}
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
                            Q4Variant = (ds.Tables[0].Rows[row]["Q4Variant"] != null) ? ds.Tables[0].Rows[row]["Q4Variant"].ToString() : string.Empty
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
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q1", FieldName = "CurrentYearRevenueQ1New", Year = financialYear.ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q1 New", FieldName = "CurrentYearRevenueQ1New", Year = financialYear.ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q1 Variant", FieldName = "CurrentYearRevenueQ1Variant", Year = financialYear.ToString() });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q2", FieldName = "CurrentYearRevenueQ2New", Year = financialYear.ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q2 New", FieldName = "CurrentYearRevenueQ2New", Year = financialYear.ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q2 Variant", FieldName = "CurrentYearRevenueQ2Variant", Year = financialYear.ToString() });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q3", FieldName = "CurrentYearRevenueQ3New", Year = financialYear.ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q3 New", FieldName = "CurrentYearRevenueQ3New", Year = financialYear.ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q3 Variant", FieldName = "CurrentYearRevenueQ3Variant", Year = financialYear.ToString() });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q4", FieldName = "CurrentYearRevenueQ4New", Year = financialYear.ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q4 New", FieldName = "CurrentYearRevenueQ4New", Year = financialYear.ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Q4 Variant", FieldName = "CurrentYearRevenueQ4Variant", Year = financialYear.ToString() });
                    }

                    listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + financialYear + " Cumulative", FieldName = "CurrentYearRevenueCumulative", Year = financialYear.ToString() });
                    if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                    {
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q1", FieldName = "NextYearRevenueQ1New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q1 New", FieldName = "NextYearRevenueQ1New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q1 Variant", FieldName = "NextYearRevenueQ1Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q2", FieldName = "NextYearRevenueQ2New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q2 New", FieldName = "NextYearRevenueQ2New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q2 Variant", FieldName = "NextYearRevenueQ2Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q3", FieldName = "NextYearRevenueQ3New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q3 New", FieldName = "NextYearRevenueQ3New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q3 Variant", FieldName = "NextYearRevenueQ3Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q4", FieldName = "NextYearRevenueQ4New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q4 New", FieldName = "NextYearRevenueQ4New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Q4 Variant", FieldName = "NextYearRevenueQ4Variant", Year = (financialYear + 1).ToString() });
                        }
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 1).ToString() + " Cumulative", FieldName = "NextYearRevenueCumulative", Year = (financialYear + 1).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 2).ToString(), FieldName = "ThirdYearRevenueCumulative", Year = (financialYear + 2).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 3).ToString(), FieldName = "FourthYearRevenueCumulative", Year = (financialYear + 3).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue FY " + (financialYear + 4).ToString(), FieldName = "FifthYearRevenueCumulative", Year = (financialYear + 4).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Revenue GrandTotal", FieldName = "RevenueGrandTotal" });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q1", FieldName = "CurrentYearGrossMarginQ1New", Year = (financialYear).ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q1 New", FieldName = "CurrentYearGrossMarginQ1New", Year = (financialYear).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q1 Variant", FieldName = "CurrentYearGrossMarginQ1Variant", Year = (financialYear).ToString() });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q2", FieldName = "CurrentYearGrossMarginQ2New", Year = (financialYear).ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q2 New", FieldName = "CurrentYearGrossMarginQ2New", Year = (financialYear).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q2 Variant", FieldName = "CurrentYearGrossMarginQ2Variant", Year = (financialYear).ToString() });

                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q3", FieldName = "CurrentYearGrossMarginQ3New", Year = (financialYear).ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q3 New", FieldName = "CurrentYearGrossMarginQ3New", Year = (financialYear).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q3 Variant", FieldName = "CurrentYearGrossMarginQ3Variant", Year = (financialYear).ToString() });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q4", FieldName = "CurrentYearGrossMarginQ4New", Year = (financialYear).ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q4 New", FieldName = "CurrentYearGrossMarginQ4New", Year = (financialYear).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Q4 Variant", FieldName = "CurrentYearGrossMarginQ4Variant", Year = (financialYear).ToString() });
                    }
                    listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + financialYear + " Cumulative", FieldName = "CurrentYearGrossMarginCumulative", Year = (financialYear).ToString() });
                    if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                    {
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q1", FieldName = "NextYearGrossMarginQ1New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q1 New", FieldName = "NextYearGrossMarginQ1New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q1 Variant", FieldName = "NextYearGrossMarginQ1Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q2", FieldName = "NextYearGrossMarginQ2New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q2 New", FieldName = "NextYearGrossMarginQ2New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q2 Variant", FieldName = "NextYearGrossMarginQ2Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q3", FieldName = "NextYearGrossMarginQ3New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q3 New", FieldName = "NextYearGrossMarginQ3New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q3 Variant", FieldName = "NextYearGrossMarginQ3Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q4", FieldName = "NextYearGrossMarginQ4New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q4 New", FieldName = "NextYearGrossMarginQ4New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Q4 Variant", FieldName = "NextYearGrossMarginQ4Variant", Year = (financialYear + 1).ToString() });
                        }
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 1).ToString() + " Cumulative", FieldName = "NextYearGrossMarginCumulative", Year = (financialYear + 1).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 2).ToString(), FieldName = "ThirdYearGrossMarginCumulative", Year = (financialYear + 2).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 3).ToString(), FieldName = "FourthYearGrossMarginCumulative", Year = (financialYear + 3).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin FY " + (financialYear + 4).ToString(), FieldName = "FifthYearGrossMarginCumulative", Year = (financialYear + 4).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Gross Margin GrandTotal", FieldName = "GrossMarginGrandTotal" });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q1", FieldName = "CurrentYearManHoursQ1New", Year = (financialYear).ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q1 New", FieldName = "CurrentYearManHoursQ1New", Year = (financialYear).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q1 Variant", FieldName = "CurrentYearManHoursQ1Variant", Year = (financialYear).ToString() });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q2 New", FieldName = "CurrentYearManHoursQ2New", Year = (financialYear).ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q2 New", FieldName = "CurrentYearManHoursQ2New", Year = (financialYear).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q2 Variant", FieldName = "CurrentYearManHoursQ2Variant", Year = (financialYear).ToString() });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q3 New", FieldName = "CurrentYearManHoursQ3New", Year = (financialYear).ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q3 New", FieldName = "CurrentYearManHoursQ3New", Year = (financialYear).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q3 Variant", FieldName = "CurrentYearManHoursQ3Variant", Year = (financialYear).ToString() });
                    }
                    if (scenarioScopeCode != "OI")
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q4 New", FieldName = "CurrentYearManHoursQ4New", Year = (financialYear).ToString() });
                    else
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q4 New", FieldName = "CurrentYearManHoursQ4New", Year = (financialYear).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Q4 Variant", FieldName = "CurrentYearManHoursQ4Variant", Year = (financialYear).ToString() });
                    }
                    listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + financialYear + " Cumulative", FieldName = "CurrentYearManHoursCumulative", Year = (financialYear).ToString() });
                    if (!(scenarioScopeCode == "PL" && scenarioTypeCode == "AC"))
                    {
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q1", FieldName = "NextYearManHoursQ1New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q1 New", FieldName = "NextYearManHoursQ1New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q1 Variant", FieldName = "NextYearManHoursQ1Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q2", FieldName = "NextYearManHoursQ2New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q2 New", FieldName = "NextYearManHoursQ2New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q2 Variant", FieldName = "NextYearManHoursQ2Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q3", FieldName = "NextYearManHoursQ3New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q3 New", FieldName = "NextYearManHoursQ3New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q3 Variant", FieldName = "NextYearManHoursQ3Variant", Year = (financialYear + 1).ToString() });
                        }
                        if (scenarioScopeCode != "OI")
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q4", FieldName = "NextYearManHoursQ4New", Year = (financialYear + 1).ToString() });
                        else
                        {
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q4 New", FieldName = "NextYearManHoursQ4New", Year = (financialYear + 1).ToString() });
                            listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Q4 Variant", FieldName = "NextYearManHoursQ4Variant", Year = (financialYear + 1).ToString() });
                        }
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 1).ToString() + " Cumulative", FieldName = "NextYearManHoursCumulative", Year = (financialYear + 1).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 2).ToString(), FieldName = "ThirdYearManHoursCumulative", Year = (financialYear + 2).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 3).ToString(), FieldName = "FourthYearManHoursCumulative", Year = (financialYear + 3).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours FY " + (financialYear + 4).ToString(), FieldName = "FifthYearManHoursCumulative", Year = (financialYear + 4).ToString() });
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Man Hours Grand Total", FieldName = "ManHoursGrandTotal" });
                    }

                    if (scenarioScopeCode == "PL" && scenarioTypeCode == "AC")
                    {
                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Cost Of Sales FY " + financialYear + " Q1", FieldName = "CurrentYearCostOfSalesQ1New", Year = (financialYear).ToString() });

                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Cost Of Sales FY " + financialYear + " Q2", FieldName = "CurrentYearCostOfSalesQ2New", Year = (financialYear).ToString() });

                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Cost Of Sales FY " + financialYear + " Q3", FieldName = "CurrentYearCostOfSalesQ3New", Year = (financialYear).ToString() });

                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Cost Of Sales FY " + financialYear + " Q4", FieldName = "CurrentYearCostOfSalesQ4New", Year = (financialYear).ToString() });

                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Cost Of Sales Grand Total", FieldName = "CostOfSalesGrandTotal" });

                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Prov. for Loss FY " + financialYear + " Q1", FieldName = "CurrentYearProvFutureLossQ1New", Year = (financialYear).ToString() });

                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Prov. for Loss FY " + financialYear + " Q2", FieldName = "CurrentYearProvFutureLossQ2New", Year = (financialYear).ToString() });

                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Prov. for Loss FY " + financialYear + " Q3", FieldName = "CurrentYearProvFutureLossQ3New", Year = (financialYear).ToString() });

                        listHeaderStructure.Add(new HeaderStructure() { HeaderName = "Prov. for Loss FY " + financialYear + " Q4", FieldName = "CurrentYearProvFutureLossQ4New", Year = (financialYear).ToString() });

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
                    decimal? CurrentYearRevenueCumulative_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ1New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ1Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ2New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ2Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ3New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ3Variant_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ4New_Gross = decimal.Zero;
                    decimal? NextYearRevenueQ4Variant_Gross = decimal.Zero;
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
                    decimal? CurrentYearGrossMarginCumulative_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ1New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ1Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ2New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ2Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ3New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ3Variant_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ4New_Gross = decimal.Zero;
                    decimal? NextYearGrossMarginQ4Variant_Gross = decimal.Zero;
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
                    decimal? CurrentYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ1New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ1Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ2New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ2Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ3New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ3Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ4New_Gross = decimal.Zero;
                    decimal? NextYearManHoursQ4Variant_Gross = decimal.Zero;
                    decimal? NextYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? ThirdYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? FourthYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? FifthYearManHoursCumulative_Gross = decimal.Zero;
                    decimal? ManHoursGrandTotal_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ1New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ2New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ3New_Gross = decimal.Zero;
                    decimal? CurrentYearCostOfSalesQ4New_Gross = decimal.Zero;                    
                    decimal? CostOfSalesGrandTotal_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ1New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ2New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ3New_Gross = decimal.Zero;
                    decimal? CurrentYearProvFutureLossQ4New_Gross = decimal.Zero;
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

                            obj.CurrentYearRevenueQ1New = (currentRevenue.Q1New != string.Empty) ? Convert.ToDecimal(currentRevenue.Q1New) : 0;
                            obj.CurrentYearRevenueQ1Variant = (currentRevenue.Q1Variant != string.Empty) ? Convert.ToDecimal(currentRevenue.Q1Variant) : 0;
                            obj.CurrentYearRevenueQ2New = (currentRevenue.Q2New != string.Empty) ? Convert.ToDecimal(currentRevenue.Q2New) : 0;
                            obj.CurrentYearRevenueQ2Variant = (currentRevenue.Q2Variant != string.Empty) ? Convert.ToDecimal(currentRevenue.Q2Variant) : 0;
                            obj.CurrentYearRevenueQ3New = (currentRevenue.Q3New != string.Empty) ? Convert.ToDecimal(currentRevenue.Q3New) : 0;
                            obj.CurrentYearRevenueQ3Variant = (currentRevenue.Q3Variant != string.Empty) ? Convert.ToDecimal(currentRevenue.Q3Variant) : 0;
                            obj.CurrentYearRevenueQ4New = (currentRevenue.Q4New != string.Empty) ? Convert.ToDecimal(currentRevenue.Q4New) : 0;
                            obj.CurrentYearRevenueQ4Variant = (currentRevenue.Q4Variant != string.Empty) ? Convert.ToDecimal(currentRevenue.Q4Variant) : 0;
                            obj.CurrentYearRevenueCumulative = (obj.CurrentYearRevenueQ1New + obj.CurrentYearRevenueQ1Variant +
                                                 obj.CurrentYearRevenueQ2New + obj.CurrentYearRevenueQ2Variant +
                                                 obj.CurrentYearRevenueQ3New + obj.CurrentYearRevenueQ3Variant +
                                                 obj.CurrentYearRevenueQ4New + obj.CurrentYearRevenueQ4Variant);
                            CurrentYearRevenueQ1New_Gross += obj.CurrentYearRevenueQ1New;
                            CurrentYearRevenueQ1Variant_Gross += obj.CurrentYearRevenueQ1Variant;
                            CurrentYearRevenueQ2New_Gross += obj.CurrentYearRevenueQ2New;
                            CurrentYearRevenueQ2Variant_Gross += obj.CurrentYearRevenueQ2Variant;
                            CurrentYearRevenueQ3New_Gross += obj.CurrentYearRevenueQ3New;
                            CurrentYearRevenueQ3Variant_Gross += obj.CurrentYearRevenueQ3Variant;
                            CurrentYearRevenueQ4New_Gross += obj.CurrentYearRevenueQ4New;
                            CurrentYearRevenueQ4Variant_Gross += obj.CurrentYearRevenueQ4Variant;
                            CurrentYearRevenueCumulative_Gross += obj.CurrentYearRevenueCumulative;

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
                                obj.NextYearRevenueQ1New = (SecondRevenue.Q1New != string.Empty) ? Convert.ToDecimal(SecondRevenue.Q1New) : 0;
                                obj.NextYearRevenueQ1Variant = (SecondRevenue.Q1Variant != string.Empty) ? Convert.ToDecimal(SecondRevenue.Q1Variant) : 0;
                                obj.NextYearRevenueQ2New = (SecondRevenue.Q2New != string.Empty) ? Convert.ToDecimal(SecondRevenue.Q2New) : 0;
                                obj.NextYearRevenueQ2Variant = (SecondRevenue.Q2Variant != string.Empty) ? Convert.ToDecimal(SecondRevenue.Q2Variant) : 0;
                                obj.NextYearRevenueQ3New = (SecondRevenue.Q3New != string.Empty) ? Convert.ToDecimal(SecondRevenue.Q3New) : 0;
                                obj.NextYearRevenueQ3Variant = (SecondRevenue.Q3Variant != string.Empty) ? Convert.ToDecimal(SecondRevenue.Q3Variant) : 0;
                                obj.NextYearRevenueQ4New = (SecondRevenue.Q4New != string.Empty) ? Convert.ToDecimal(SecondRevenue.Q4New) : 0;
                                obj.NextYearRevenueQ4Variant = (SecondRevenue.Q4Variant != string.Empty) ? Convert.ToDecimal(SecondRevenue.Q4Variant) : 0;
                                obj.NextYearRevenueCumulative = (obj.NextYearRevenueQ1New + obj.NextYearRevenueQ1Variant +
                                                    obj.NextYearRevenueQ2New + obj.NextYearRevenueQ2Variant +
                                                    obj.NextYearRevenueQ3New + obj.NextYearRevenueQ3Variant +
                                                    obj.NextYearRevenueQ4New + obj.NextYearRevenueQ4Variant);
                                NextYearRevenueQ1New_Gross += obj.NextYearRevenueQ1New;
                                NextYearRevenueQ1Variant_Gross += obj.NextYearRevenueQ1Variant;
                                NextYearRevenueQ2New_Gross += obj.NextYearRevenueQ2New;
                                NextYearRevenueQ2Variant_Gross += obj.NextYearRevenueQ2Variant;
                                NextYearRevenueQ3New_Gross += obj.NextYearRevenueQ3New;
                                NextYearRevenueQ3Variant_Gross += obj.NextYearRevenueQ3Variant;
                                NextYearRevenueQ4New_Gross += obj.NextYearRevenueQ4New;
                                NextYearRevenueQ4Variant_Gross += obj.NextYearRevenueQ4Variant;
                                NextYearRevenueCumulative_Gross += obj.NextYearRevenueCumulative;
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

                            obj.CurrentYearGrossMarginQ1New = (currentGrossMargin.Q1New != string.Empty) ? Convert.ToDecimal(currentGrossMargin.Q1New) : 0;
                            obj.CurrentYearGrossMarginQ1Variant = (currentGrossMargin.Q1Variant != string.Empty) ? Convert.ToDecimal(currentGrossMargin.Q1Variant) : 0;
                            obj.CurrentYearGrossMarginQ2New = (currentGrossMargin.Q2New != string.Empty) ? Convert.ToDecimal(currentGrossMargin.Q2New) : 0;
                            obj.CurrentYearGrossMarginQ2Variant = (currentGrossMargin.Q2Variant != string.Empty) ? Convert.ToDecimal(currentGrossMargin.Q2Variant) : 0;
                            obj.CurrentYearGrossMarginQ3New = (currentGrossMargin.Q3New != string.Empty) ? Convert.ToDecimal(currentGrossMargin.Q3New) : 0;
                            obj.CurrentYearGrossMarginQ3Variant = (currentGrossMargin.Q3Variant != string.Empty) ? Convert.ToDecimal(currentGrossMargin.Q3Variant) : 0;
                            obj.CurrentYearGrossMarginQ4New = (currentGrossMargin.Q4New != string.Empty) ? Convert.ToDecimal(currentGrossMargin.Q4New) : 0;
                            obj.CurrentYearGrossMarginQ4Variant = (currentGrossMargin.Q4Variant != string.Empty) ? Convert.ToDecimal(currentGrossMargin.Q4Variant) : 0;
                            obj.CurrentYearGrossMarginCumulative = (obj.CurrentYearGrossMarginQ1New + obj.CurrentYearGrossMarginQ1Variant +
                                                 obj.CurrentYearGrossMarginQ2New + obj.CurrentYearGrossMarginQ2Variant +
                                                 obj.CurrentYearGrossMarginQ3New + obj.CurrentYearGrossMarginQ3Variant +
                                                 obj.CurrentYearGrossMarginQ4New + obj.CurrentYearGrossMarginQ4Variant);
                            CurrentYearGrossMarginQ1New_Gross += obj.CurrentYearGrossMarginQ1New;
                            CurrentYearGrossMarginQ1Variant_Gross += obj.CurrentYearGrossMarginQ1Variant;
                            CurrentYearGrossMarginQ2New_Gross += obj.CurrentYearGrossMarginQ2New;
                            CurrentYearGrossMarginQ2Variant_Gross += obj.CurrentYearGrossMarginQ2Variant;
                            CurrentYearGrossMarginQ3New_Gross += obj.CurrentYearGrossMarginQ3New;
                            CurrentYearGrossMarginQ3Variant_Gross += obj.CurrentYearGrossMarginQ3Variant;
                            CurrentYearGrossMarginQ4New_Gross += obj.CurrentYearGrossMarginQ4New;
                            CurrentYearGrossMarginQ4Variant_Gross += obj.CurrentYearGrossMarginQ4Variant;
                            CurrentYearGrossMarginCumulative_Gross += obj.CurrentYearGrossMarginCumulative;
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
                                obj.NextYearGrossMarginQ1New = (secondGrossMargin.Q1New != string.Empty) ? Convert.ToDecimal(secondGrossMargin.Q1New) : 0;
                                obj.NextYearGrossMarginQ1Variant = (secondGrossMargin.Q1Variant != string.Empty) ? Convert.ToDecimal(secondGrossMargin.Q1Variant) : 0;
                                obj.NextYearGrossMarginQ2New = (secondGrossMargin.Q2New != string.Empty) ? Convert.ToDecimal(secondGrossMargin.Q2New) : 0;
                                obj.NextYearGrossMarginQ2Variant = (secondGrossMargin.Q2Variant != string.Empty) ? Convert.ToDecimal(secondGrossMargin.Q2Variant) : 0;
                                obj.NextYearGrossMarginQ3New = (secondGrossMargin.Q3New != string.Empty) ? Convert.ToDecimal(secondGrossMargin.Q3New) : 0;
                                obj.NextYearGrossMarginQ3Variant = (secondGrossMargin.Q3Variant != string.Empty) ? Convert.ToDecimal(secondGrossMargin.Q3Variant) : 0;
                                obj.NextYearGrossMarginQ4New = (secondGrossMargin.Q4New != string.Empty) ? Convert.ToDecimal(secondGrossMargin.Q4New) : 0;
                                obj.NextYearGrossMarginQ4Variant = (secondGrossMargin.Q4Variant != string.Empty) ? Convert.ToDecimal(secondGrossMargin.Q4Variant) : 0;
                                obj.NextYearGrossMarginCumulative = (obj.NextYearGrossMarginQ1New + obj.NextYearGrossMarginQ1Variant +
                                                   obj.NextYearGrossMarginQ2New + obj.NextYearGrossMarginQ2Variant +
                                                   obj.NextYearGrossMarginQ3New + obj.NextYearGrossMarginQ3Variant +
                                                   obj.NextYearGrossMarginQ4New + obj.NextYearGrossMarginQ4Variant);
                                NextYearGrossMarginQ1New_Gross += obj.NextYearGrossMarginQ1New;
                                NextYearGrossMarginQ1Variant_Gross += obj.NextYearGrossMarginQ1Variant;
                                NextYearGrossMarginQ2New_Gross += obj.NextYearGrossMarginQ2New;
                                NextYearGrossMarginQ2Variant_Gross += obj.NextYearGrossMarginQ2Variant;
                                NextYearGrossMarginQ3New_Gross += obj.NextYearGrossMarginQ3New;
                                NextYearGrossMarginQ3Variant_Gross += obj.NextYearGrossMarginQ3Variant;
                                NextYearGrossMarginQ4New_Gross += obj.NextYearGrossMarginQ4New;
                                NextYearGrossMarginQ4Variant_Gross += obj.NextYearGrossMarginQ4Variant;
                                NextYearGrossMarginCumulative_Gross += obj.NextYearGrossMarginCumulative;

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

                            obj.CurrentYearManHoursQ1New = (currentManHours.Q1New != string.Empty) ? Convert.ToDecimal(currentManHours.Q1New) : 0;
                            obj.CurrentYearManHoursQ1Variant = (currentManHours.Q1Variant != string.Empty) ? Convert.ToDecimal(currentManHours.Q1Variant) : 0;
                            obj.CurrentYearManHoursQ2New = (currentManHours.Q2New != string.Empty) ? Convert.ToDecimal(currentManHours.Q2New) : 0;
                            obj.CurrentYearManHoursQ2Variant = (currentManHours.Q2Variant != string.Empty) ? Convert.ToDecimal(currentManHours.Q2Variant) : 0;
                            obj.CurrentYearManHoursQ3New = (currentManHours.Q3New != string.Empty) ? Convert.ToDecimal(currentManHours.Q3New) : 0;
                            obj.CurrentYearManHoursQ3Variant = (currentManHours.Q3Variant != string.Empty) ? Convert.ToDecimal(currentManHours.Q3Variant) : 0;
                            obj.CurrentYearManHoursQ4New = (currentManHours.Q4New != string.Empty) ? Convert.ToDecimal(currentManHours.Q4New) : 0;
                            obj.CurrentYearManHoursQ4Variant = (currentManHours.Q4Variant != string.Empty) ? Convert.ToDecimal(currentManHours.Q4Variant) : 0;
                            obj.CurrentYearManHoursCumulative = (obj.CurrentYearManHoursQ1New + obj.CurrentYearManHoursQ1Variant +
                                                obj.CurrentYearManHoursQ2New + obj.CurrentYearManHoursQ2Variant +
                                                obj.CurrentYearManHoursQ3New + obj.CurrentYearManHoursQ3Variant +
                                                obj.CurrentYearManHoursQ4New + obj.CurrentYearManHoursQ4Variant);
                            CurrentYearManHoursQ1New_Gross += obj.CurrentYearManHoursQ1New;
                            CurrentYearManHoursQ1Variant_Gross += obj.CurrentYearManHoursQ1Variant;
                            CurrentYearManHoursQ2New_Gross += obj.CurrentYearManHoursQ2New;
                            CurrentYearManHoursQ2Variant_Gross += obj.CurrentYearManHoursQ2Variant;
                            CurrentYearManHoursQ3New_Gross += obj.CurrentYearManHoursQ3New;
                            CurrentYearManHoursQ3Variant_Gross += obj.CurrentYearManHoursQ3Variant;
                            CurrentYearManHoursQ4New_Gross += obj.CurrentYearManHoursQ4New;
                            CurrentYearManHoursQ4Variant_Gross += obj.CurrentYearManHoursQ4Variant;
                            CurrentYearManHoursCumulative_Gross += obj.CurrentYearManHoursCumulative;

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
                                obj.NextYearManHoursQ1New = (secondManHours.Q1New != string.Empty) ? Convert.ToDecimal(secondManHours.Q1New) : 0;
                                obj.NextYearManHoursQ1Variant = (secondManHours.Q1Variant != string.Empty) ? Convert.ToDecimal(secondManHours.Q1Variant) : 0;
                                obj.NextYearManHoursQ2New = (secondManHours.Q2New != string.Empty) ? Convert.ToDecimal(secondManHours.Q2New) : 0;
                                obj.NextYearManHoursQ2Variant = (secondManHours.Q2Variant != string.Empty) ? Convert.ToDecimal(secondManHours.Q2Variant) : 0;
                                obj.NextYearManHoursQ3New = (secondManHours.Q3New != string.Empty) ? Convert.ToDecimal(secondManHours.Q3New) : 0;
                                obj.NextYearManHoursQ3Variant = (secondManHours.Q3Variant != string.Empty) ? Convert.ToDecimal(secondManHours.Q3Variant) : 0;
                                obj.NextYearManHoursQ4New = (secondManHours.Q4New != string.Empty) ? Convert.ToDecimal(secondManHours.Q4New) : 0;
                                obj.NextYearManHoursQ4Variant = (secondManHours.Q4Variant != string.Empty) ? Convert.ToDecimal(secondManHours.Q4Variant) : 0;
                                obj.NextYearManHoursCumulative = (obj.NextYearManHoursQ1New + obj.NextYearManHoursQ1Variant +
                                                   obj.NextYearManHoursQ2New + obj.NextYearManHoursQ2Variant +
                                                   obj.NextYearManHoursQ3New + obj.NextYearManHoursQ3Variant +
                                                   obj.NextYearManHoursQ4New + obj.NextYearManHoursQ4Variant);
                                NextYearManHoursQ1New_Gross += obj.NextYearManHoursQ1New;
                                NextYearManHoursQ1Variant_Gross += obj.NextYearManHoursQ1Variant;
                                NextYearManHoursQ2New_Gross += obj.NextYearManHoursQ2New;
                                NextYearManHoursQ2Variant_Gross += obj.NextYearManHoursQ2Variant;
                                NextYearManHoursQ3New_Gross += obj.NextYearManHoursQ3New;
                                NextYearManHoursQ3Variant_Gross += obj.NextYearManHoursQ3Variant;
                                NextYearManHoursQ4New_Gross += obj.NextYearManHoursQ4New;
                                NextYearManHoursQ4Variant_Gross += obj.NextYearManHoursQ4Variant;
                                NextYearManHoursCumulative_Gross += obj.NextYearManHoursCumulative;
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
                                obj.CostOfSalesGrandTotal = (obj.CurrentYearCostOfSalesQ1New + obj.CurrentYearCostOfSalesQ2New +
                                                                obj.CurrentYearCostOfSalesQ3New + obj.CurrentYearCostOfSalesQ4New);
                                CurrentYearCostOfSalesQ1New_Gross += obj.CurrentYearCostOfSalesQ1New;
                                CurrentYearCostOfSalesQ2New_Gross += obj.CurrentYearCostOfSalesQ2New;
                                CurrentYearCostOfSalesQ3New_Gross += obj.CurrentYearCostOfSalesQ3New;
                                CurrentYearCostOfSalesQ4New_Gross += obj.CurrentYearCostOfSalesQ4New;
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
                                obj.ProvFutureLossGrandTotal = (obj.CurrentYearProvFutureLossQ1New + obj.CurrentYearProvFutureLossQ2New +
                                                                obj.CurrentYearProvFutureLossQ3New + obj.CurrentYearProvFutureLossQ4New);
                                CurrentYearProvFutureLossQ1New_Gross += obj.CurrentYearProvFutureLossQ1New;
                                CurrentYearProvFutureLossQ2New_Gross += obj.CurrentYearProvFutureLossQ2New;
                                CurrentYearProvFutureLossQ3New_Gross += obj.CurrentYearProvFutureLossQ3New;
                                CurrentYearProvFutureLossQ4New_Gross += obj.CurrentYearProvFutureLossQ4New;
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
                    obj1.CurrentYearRevenueCumulative = CurrentYearRevenueCumulative_Gross;
                    obj1.NextYearRevenueQ1New = NextYearRevenueQ1New_Gross;
                    obj1.NextYearRevenueQ1Variant = NextYearRevenueQ1Variant_Gross;
                    obj1.NextYearRevenueQ2New = NextYearRevenueQ2New_Gross;
                    obj1.NextYearRevenueQ2Variant = NextYearRevenueQ2Variant_Gross;
                    obj1.NextYearRevenueQ3New = NextYearRevenueQ3New_Gross;
                    obj1.NextYearRevenueQ3Variant = NextYearRevenueQ3Variant_Gross;
                    obj1.NextYearRevenueQ4New = NextYearRevenueQ4New_Gross;
                    obj1.NextYearRevenueQ4Variant = NextYearRevenueQ4Variant_Gross;
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
                    obj1.CurrentYearGrossMarginCumulative = CurrentYearGrossMarginCumulative_Gross;
                    obj1.NextYearGrossMarginQ1New = NextYearGrossMarginQ1New_Gross;
                    obj1.NextYearGrossMarginQ1Variant = NextYearGrossMarginQ1Variant_Gross;
                    obj1.NextYearGrossMarginQ2New = NextYearGrossMarginQ2New_Gross;
                    obj1.NextYearGrossMarginQ2Variant = NextYearGrossMarginQ2Variant_Gross;
                    obj1.NextYearGrossMarginQ3New = NextYearGrossMarginQ3New_Gross;
                    obj1.NextYearGrossMarginQ3Variant = NextYearGrossMarginQ3Variant_Gross;
                    obj1.NextYearGrossMarginQ4New = NextYearGrossMarginQ4New_Gross;
                    obj1.NextYearGrossMarginQ4Variant = NextYearGrossMarginQ4Variant_Gross;
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
                    obj1.CurrentYearManHoursCumulative = CurrentYearManHoursCumulative_Gross;
                    obj1.NextYearManHoursQ1New = NextYearManHoursQ1New_Gross;
                    obj1.NextYearManHoursQ1Variant = NextYearManHoursQ1Variant_Gross;
                    obj1.NextYearManHoursQ2New = NextYearManHoursQ2New_Gross;
                    obj1.NextYearManHoursQ2Variant = NextYearManHoursQ2Variant_Gross;
                    obj1.NextYearManHoursQ3New = NextYearManHoursQ3New_Gross;
                    obj1.NextYearManHoursQ3Variant = NextYearManHoursQ3Variant_Gross;
                    obj1.NextYearManHoursQ4New = NextYearManHoursQ4New_Gross;
                    obj1.NextYearManHoursQ4Variant = NextYearManHoursQ4Variant_Gross;
                    obj1.NextYearManHoursCumulative = NextYearManHoursCumulative_Gross;
                    obj1.ThirdYearManHoursCumulative = ThirdYearManHoursCumulative_Gross;
                    obj1.FourthYearManHoursCumulative = FourthYearManHoursCumulative_Gross;
                    obj1.FifthYearManHoursCumulative = FifthYearManHoursCumulative_Gross;
                    obj1.ManHoursGrandTotal = ManHoursGrandTotal_Gross;
                    obj1.CurrentYearCostOfSalesQ1New = CurrentYearCostOfSalesQ1New_Gross;
                    obj1.CurrentYearCostOfSalesQ2New = CurrentYearCostOfSalesQ2New_Gross;
                    obj1.CurrentYearCostOfSalesQ3New = CurrentYearCostOfSalesQ3New_Gross;
                    obj1.CurrentYearCostOfSalesQ4New = CurrentYearCostOfSalesQ4New_Gross;
                    obj1.CostOfSalesGrandTotal = CostOfSalesGrandTotal_Gross;
                    obj1.CurrentYearProvFutureLossQ1New = CurrentYearProvFutureLossQ1New_Gross;
                    obj1.CurrentYearProvFutureLossQ2New = CurrentYearProvFutureLossQ2New_Gross;
                    obj1.CurrentYearProvFutureLossQ3New = CurrentYearProvFutureLossQ3New_Gross;
                    obj1.CurrentYearProvFutureLossQ4New = CurrentYearProvFutureLossQ4New_Gross;
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
