using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TechnipFMC.Common;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        public ReportRepository()
        { }

        #region Old Code
        public ProjectLifeCycleReport GetProjectLifeCycleReportData(string projectIds, int scopeId)
        {
            try
            {
                var reportData = new ProjectLifeCycleReport();

                var command = "" +
                    "\nSELECT DISTINCT P.ProjectID,P.IFSProjectCode,P.ProjectName,S.ScenarioName" +
                    "\n,SD.[Year],SD.ScenarioDataTypeID,SD.Q1New,SD.Q2New,SD.Q3New,SD.Q4New,(SD.Q1New + SD.Q2New + SD.Q3New + SD.Q4New) Total " +
                    "\nFROM Project P " +
                    "\nINNER JOIN ScenarioData SD ON SD.ProjectID = P.ProjectID " +
                    "\nINNER JOIN Scenario S ON S.ScenarioID = SD.ScenarioID " +
                    $"\nWHERE P.ProjectID IN({projectIds}) " +
                    $"\nAND S.ScenarioScopeCode = (SELECT ScenarioScopeCode FROM ScenarioScopeMaster WHERE ScenarioScopeID = {scopeId}) " +
                    "\nAND S.ScenarioTypeCode = (SELECT ScenarioTypeCode FROM ScenarioTypeMaster WHERE ScenarioTypeID = 3) " +
                    "\nAND SD.ScenarioDataTypeID IN(1,2) " +
                    "\nORDER BY P.ProjectID,SD.[Year] ASC; " +

                    "\nSELECT DISTINCT P.ProjectID,P.IFSProjectCode,P.ProjectName,S.ScenarioName" +
                    "\n,SD.[Year],SD.ScenarioDataTypeID,SD.Q1New,SD.Q2New,SD.Q3New,SD.Q4New,(SD.Q1New + SD.Q2New + SD.Q3New + SD.Q4New) Total " +
                    "\nFROM Project P " +
                    "\nINNER JOIN ScenarioData SD ON SD.ProjectID = P.ProjectID " +
                    "\nINNER JOIN Scenario S ON S.ScenarioID = SD.ScenarioID " +
                    $"\nWHERE P.ProjectID IN({projectIds}) " +
                    $"\nAND S.ScenarioScopeCode = (SELECT ScenarioScopeCode FROM ScenarioScopeMaster WHERE ScenarioScopeID = {scopeId}) " +
                    "\nAND S.ScenarioTypeCode = (SELECT ScenarioTypeCode FROM ScenarioTypeMaster WHERE ScenarioTypeID = 5) " +
                    "\nAND SD.ScenarioDataTypeID IN(1,2) " +
                    "\nORDER BY P.ProjectID,SD.[Year] ASC; ";


                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = command;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    reportData.Actuals = ds.Tables[0].ToListOfObject<ProjectLifeCycleReportData>();
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null))
                {
                    reportData.ForeCast = ds.Tables[1].ToListOfObject<ProjectLifeCycleReportData>();
                }
                return reportData;
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

        public REPExtractReport GetREPExtractReportData(int year, int reportTypeId, int scenarioId, string groupLevels)
        {
            try
            {
                var reportData = new REPExtractReport();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetREPExtractReportData";
                cmd.Parameters.AddWithValue("@P_Year", year);
                cmd.Parameters.AddWithValue("@P_ReportTypeId", reportTypeId);
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                base.DBConnection.Close();
                if (reportTypeId == 1)
                {
                    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                    {
                        reportData.REPExtractData = ds.Tables[0].ToListOfObject<REPExtractReportData>();
                    }
                    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null))
                    {
                        reportData.Scenario = ds.Tables[1].ToObject<Scenario>();
                    }
                }
                else
                {
                    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                    {
                        reportData.CurrForeCast = ds.Tables[0].ToListOfObject<REPExtractReportData>();
                    }
                    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null))
                    {
                        reportData.Scenario = ds.Tables[1].ToObject<Scenario>();
                    }
                    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[2] != null))
                    {
                        reportData.FutureForeCast = ds.Tables[2].ToListOfObject<REPExtractReportData>();
                    }
                }


                return reportData;
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
        public VarianceAnalysisReport GetVarianceAnalysisReportData(VarianceAnalysisConfig config)
        {
            try
            {
                var reportData = new VarianceAnalysisReport();
                DataSet ds = new DataSet();

                var rv_ScenarioBase = "SELECT ScenarioDataTypeID From FinancialDataTypesScenario " +
                    $"WHERE ScenarioScopeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.BaseScenarioId}) " +
                    $"AND ScenarioTypeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.BaseScenarioId}) AND FinancialDataTypeID = 1";
                var gm_ScenarioBase = "SELECT ScenarioDataTypeID From FinancialDataTypesScenario " +
                    $"WHERE ScenarioScopeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.BaseScenarioId}) " +
                    $"AND ScenarioTypeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.BaseScenarioId}) AND FinancialDataTypeID = 2";

                var rv_ScenarioA = "SELECT ScenarioDataTypeID From FinancialDataTypesScenario " +
                    $"WHERE ScenarioScopeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.CompareScenarioAId}) " +
                    $"AND ScenarioTypeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.CompareScenarioAId}) AND FinancialDataTypeID = 1";
                var gm_ScenarioA = "SELECT ScenarioDataTypeID From FinancialDataTypesScenario " +
                    $"WHERE ScenarioScopeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.CompareScenarioAId}) " +
                    $"AND ScenarioTypeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.CompareScenarioAId}) AND FinancialDataTypeID = 2";

                var rv_ScenarioB = "SELECT ScenarioDataTypeID From FinancialDataTypesScenario " +
                    $"WHERE ScenarioScopeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.CompareScenarioBId}) " +
                    $"AND ScenarioTypeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.CompareScenarioBId}) AND FinancialDataTypeID = 1";
                var gm_ScenarioB = "SELECT ScenarioDataTypeID From FinancialDataTypesScenario " +
                    $"WHERE ScenarioScopeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.CompareScenarioBId}) " +
                    $"AND ScenarioTypeCode = (Select ScenarioScopeCode FROM Scenario WHERE ScenarioID = {config.CompareScenarioBId}) AND FinancialDataTypeID = 2";

                var command = "SELECT DISTINCT  P.ProjectID,P.ProjectName,P.IFSProjectCode,PE.ProjectEntityName" +
     ", PS.ProjectSegmentName,CS.ContractStatusName,CT.ContractTypeName,BU.BUCategoryName" +
     ", RVSD1.Q1New RV_S1Q1, RVSD1.Q2New RV_S1Q2, RVSD1.Q3New RV_S1Q3, RVSD1.Q4New RV_S1Q4" +
     ", RVSD1.Q5New RV_S1Q5, RVSD1.Q6New RV_S1Q6, RVSD1.Q7New RV_S1Q7, RVSD1.Q8New RV_S1Q8" +
     ", RVSD1.Q9New RV_S1Q9, RVSD1.Q10New RV_S1Q10, RVSD1.Q11New RV_S1Q11, RVSD1.Q12New RV_S1Q12" +
     ", RVSD2.Q1New RV_S2Q1, RVSD2.Q2New RV_S2Q2, RVSD2.Q3New RV_S2Q3, RVSD2.Q4New RV_S2Q4" +
     ", RVSD2.Q5New RV_S2Q5, RVSD2.Q6New RV_S2Q6, RVSD2.Q7New RV_S2Q7, RVSD2.Q8New RV_S2Q8" +
     ", RVSD2.Q9New RV_S2Q9, RVSD2.Q10New RV_S2Q10, RVSD2.Q11New RV_S2Q11, RVSD2.Q12New RV_S2Q12" +
     ", RVSD3.Q1New RV_S3Q1, RVSD3.Q2New RV_S3Q2, RVSD3.Q3New RV_S3Q3, RVSD3.Q4New RV_S3Q4" +
     ", RVSD3.Q5New RV_S3Q5, RVSD3.Q6New RV_S3Q6, RVSD3.Q7New RV_S3Q7, RVSD3.Q8New RV_S3Q8" +
     ", RVSD3.Q9New RV_S3Q9, RVSD3.Q10New RV_S3Q10, RVSD3.Q11New RV_S3Q11, RVSD3.Q12New RV_S3Q12" +
     ", GMSD1.Q1New GM_S1Q1, GMSD1.Q2New GM_S1Q2, GMSD1.Q3New GM_S1Q3, GMSD1.Q4New GM_S1Q4" +
     ", GMSD1.Q5New GM_S1Q5, GMSD1.Q6New GM_S1Q6, GMSD1.Q7New GM_S1Q7, GMSD1.Q8New GM_S1Q8" +
     ", GMSD1.Q9New GM_S1Q9, GMSD1.Q10New GM_S1Q10, GMSD1.Q11New GM_S1Q11, GMSD1.Q12New GM_S1Q12" +
     ", GMSD2.Q1New GM_S2Q1, GMSD2.Q2New GM_S2Q2, GMSD2.Q3New GM_S2Q3, GMSD2.Q4New GM_S2Q4" +
     ", GMSD2.Q5New GM_S2Q5, GMSD2.Q6New GM_S2Q6, GMSD2.Q7New GM_S2Q7, GMSD2.Q8New GM_S2Q8" +
     ", GMSD2.Q9New GM_S2Q9, GMSD2.Q10New GM_S2Q10, GMSD2.Q11New GM_S2Q11, GMSD2.Q12New GM_S2Q12" +
     ", GMSD3.Q1New GM_S3Q1, GMSD3.Q2New GM_S3Q2, GMSD3.Q3New GM_S3Q3, GMSD3.Q4New GM_S3Q4" +
     ", GMSD3.Q5New GM_S3Q5, GMSD3.Q6New GM_S3Q6, GMSD3.Q7New GM_S3Q7, GMSD3.Q8New GM_S3Q8" +
     ", GMSD3.Q9New GM_S3Q9, GMSD3.Q10New GM_S3Q10, GMSD3.Q11New GM_S3Q11, GMSD3.Q12New GM_S3Q12 " +

     "FROM Project P " +
     "INNER JOIN BUCategoryMaster BU ON BU.BUCategoryCode = P.BUCategoryCode " +
     "INNER JOIN ProjectEntityMaster PE ON PE.ProjectEntityCode = P.ProjectEntityCode " +
     "INNER JOIN ProjectSegmentMaster PS ON PS.ProjectSegmentCode = P.ProjectSegmentCode " +
     "INNER JOIN ContractStatusMaster CS ON CS.ContractStatusCode = P.ContractStatusCode " +
     "INNER JOIN ContractTypeMaster CT ON CT.ContractTypeCode = P.ContractTypeCode " +

     $"LEFT JOIN ScenarioData RVSD1 ON(RVSD1.ProjectID = P.ProjectID AND RVSD1.ScenarioDataTypeID = ({rv_ScenarioBase}) AND RVSD1.[Year] = {config.Year}) " +
     $"LEFT JOIN ScenarioData RVSD2 ON(RVSD2.ProjectID = P.ProjectID AND RVSD2.ScenarioDataTypeID = ({rv_ScenarioA}) AND RVSD2.[Year] = {config.Year}) " +
     $"LEFT JOIN ScenarioData RVSD3 ON(RVSD3.ProjectID = P.ProjectID AND RVSD3.ScenarioDataTypeID = ({rv_ScenarioB}) AND RVSD3.[Year] = {config.Year}) " +
     $"LEFT JOIN ScenarioData GMSD1 ON(GMSD1.ProjectID = P.ProjectID AND GMSD1.ScenarioDataTypeID = ({gm_ScenarioBase}) AND GMSD1.[Year] = {config.Year}) " +
     $"LEFT JOIN ScenarioData GMSD2 ON(GMSD2.ProjectID = P.ProjectID AND GMSD2.ScenarioDataTypeID = ({gm_ScenarioA}) AND GMSD2.[Year] = {config.Year}) " +
     $"LEFT JOIN ScenarioData GMSD3 ON(GMSD3.ProjectID = P.ProjectID AND GMSD3.ScenarioDataTypeID = ({gm_ScenarioB}) AND GMSD3.[Year] = {config.Year}) " +

     $"WHERE(RVSD1.ScenarioID = {config.BaseScenarioId} AND RVSD2.ScenarioID = {config.CompareScenarioAId} AND RVSD3.ScenarioID = {config.CompareScenarioBId}) " +
     $"OR (GMSD1.ScenarioID = {config.BaseScenarioId} OR GMSD2.ScenarioID = {config.CompareScenarioAId} AND GMSD3.ScenarioID = {config.CompareScenarioBId}) ";

                if (config.GroupLevels != "")
                {
                    command += $"ORDER BY {config.GroupLevels}";
                }

                command += ";\n" +
                    "SELECT S.ScenarioID,S.ScenarioName,ST.ScenarioTypeName " +
                    "FROM Scenario S INNER JOIN ScenarioTypeMaster ST ON S.ScenarioTypeCode = ST.ScenarioTypeCode " +
                    $"WHERE S.ScenarioID IN({config.BaseScenarioId},{config.CompareScenarioAId},{config.CompareScenarioBId});";

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = command;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    reportData.VarianceAnalysisReportDatas = ds.Tables[0].ToListOfObject<VarianceAnalysisReportData>();
                }

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null))
                {
                    reportData.Scenarios = ds.Tables[1].ToListOfObject<Scenario>();
                }

                return reportData;
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
        #endregion

        public List<VarianceAnalysisResponse> GetVarianceAnalysisReport(VarianceAnalysisConfig config)
        {
            try
            {
                var reportData = new List<VarianceAnalysisResponse>();
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "VarianceAnalysisReport";
                cmd.Parameters.AddWithValue("@P_BaseScenarioId", config.BaseScenarioId);
                cmd.Parameters.AddWithValue("@P_CompareScenarioId1", config.CompareScenarioAId);
                cmd.Parameters.AddWithValue("@P_CompareScenarioId2", config.CompareScenarioBId);
                cmd.Parameters.AddWithValue("@P_Year", config.Year);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeID", config.ScenarioDataTypeId);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    reportData = ds.Tables[0].ToListOfObject<VarianceAnalysisResponse>();
                }


                return reportData;
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
        public DevianceResponse GetDevianceReport(DevianceReportConfig config)
        {
            try
            {
                var reportData = new DevianceResponse();
                List<DepartmentData> listDepartment = new List<DepartmentData>();
                List<ClientData> listClient = new List<ClientData>();
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CumulativeDevianceReport";
                cmd.Parameters.AddWithValue("@P_OrgScenarioId", config.OrgScenarioId);
                //cmd.Parameters.AddWithValue("@P_ScenarioType", config.ScenarioType);
                cmd.Parameters.AddWithValue("@P_ScenarioDataType", config.ScenarioDataTypeId);
                cmd.Parameters.AddWithValue("@P_Year", config.Year);
                //cmd.Parameters.AddWithValue("@P_FinancialDataTypeID", config.ScenarioDataTypeId);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    reportData.OrgQ1 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ1"]);
                    reportData.OrgQ2 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ2"]);
                    reportData.OrgQ3 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ3"]);
                    reportData.OrgQ4 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ4"]);
                    reportData.OrgQ5 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ5"]);
                    reportData.OrgQ6 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ6"]);
                    reportData.OrgQ7 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ7"]);
                    reportData.OrgQ8 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ8"]);
                    reportData.OrgQ9 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ9"]);
                    reportData.OrgQ10 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ10"]);
                    reportData.OrgQ11 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ11"]);
                    reportData.OrgQ12 = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrgQ12"]);
                    reportData.TotalOrg = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalOrg"]);
                    reportData.OrgScenarioName = Convert.ToString(ds.Tables[0].Rows[0]["OrgScenarioName"]);

                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null))
                {
                    listDepartment = ds.Tables[1].ToListOfObject<DepartmentData>();
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[2] != null))
                {
                    listClient = ds.Tables[2].ToListOfObject<ClientData>();
                }
                reportData.DData = listDepartment;
                reportData.CData = listClient;

                return reportData;
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

        public ExtractResponseDataModel REPExtractReport(int year, string scenarioTypeCode, string isCurrencyConversionRequired)
        {

            try
            {
                ExtractResponseDataModel objResponse = new ExtractResponseDataModel();
                List<ExtractDetailDataModel> listResponse = new List<ExtractDetailDataModel>();
                var repExtractResponses = new List<RepExtractResponse>();
                var currencyExchangeDatas = new List<CurrencyExchangeData>();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "REPExtractReport";
                cmd.Parameters.AddWithValue("@P_Year", year);
                cmd.Parameters.AddWithValue("@P_ScenarioTypeCode", scenarioTypeCode);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                base.DBConnection.Close();
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null))
                {
                    currencyExchangeDatas = ds.Tables[0].ToListOfObject<CurrencyExchangeData>();
                }
                if ((ds != null) && (ds.Tables.Count > 1) && (ds.Tables[1] != null))
                {
                    repExtractResponses = ds.Tables[1].ToListOfObject<RepExtractResponse>();
                    if (repExtractResponses.Count() > 0)
                        objResponse.ScenarioName = repExtractResponses.FirstOrDefault().ScenarioName;
                }


                var distinctSmartViewCodes = repExtractResponses.Select(s => s.SmartViewCode).Distinct();
                if (scenarioTypeCode == "AC")
                {

                    decimal Q1Rate = 1;
                    decimal Q2Rate = 1;
                    decimal Q3Rate = 1;
                    decimal Q4Rate = 1;
                    if (isCurrencyConversionRequired.Trim() == "Y")
                    {
                        try
                        {
                            Q1Rate = currencyExchangeDatas.Where(a => a.Quarter == "Q1").FirstOrDefault().AverageRate.Value;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            Q2Rate = currencyExchangeDatas.Where(a => a.Quarter == "Q2").FirstOrDefault().AverageRate.Value;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            Q3Rate = currencyExchangeDatas.Where(a => a.Quarter == "Q3").FirstOrDefault().AverageRate.Value;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            Q4Rate = currencyExchangeDatas.Where(a => a.Quarter == "Q4").FirstOrDefault().AverageRate.Value;
                        }
                        catch (Exception)
                        { }
                    }
                    decimal GrossRevenueQ1New = Convert.ToDecimal(0);
                    decimal GrossRevenueQ2New = Convert.ToDecimal(0);
                    decimal GrossRevenueQ3New = Convert.ToDecimal(0);
                    decimal GrossRevenueQ4New = Convert.ToDecimal(0);
                    decimal GrossGMQ1New = Convert.ToDecimal(0);
                    decimal GrossGMQ2New = Convert.ToDecimal(0);
                    decimal GrossGMQ3New = Convert.ToDecimal(0);
                    decimal GrossGMQ4New = Convert.ToDecimal(0);
                    foreach (var item in distinctSmartViewCodes)
                    {
                        ExtractDetailDataModel obj = new ExtractDetailDataModel();
                        obj.RecordType = "P";
                        var dataRevenue = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "RV").FirstOrDefault();
                        if (dataRevenue != null)
                        {
                            obj.ProjectEntityCode = dataRevenue.ProjectEntityCode;
                            obj.ProjectSegmentCode = dataRevenue.ProjectSegmentCode;
                            obj.SmartViewCode = dataRevenue.SmartViewCode;
                            obj.SmartViewName = dataRevenue.SmartViewName;
                            obj.BUCategoryName = dataRevenue.BUCategoryName;
                            obj.StatutoryCategoryName = dataRevenue.StatutoryCategoryName;
                            obj.FirstYearRevenueQ1New = (dataRevenue.Q1New.Value * Q1Rate);
                            GrossRevenueQ1New = GrossRevenueQ1New + (obj.FirstYearRevenueQ1New.HasValue == true ? obj.FirstYearRevenueQ1New.Value : Convert.ToDecimal(0));
                            obj.FirstYearRevenueQ2New = (dataRevenue.Q2New.Value * Q2Rate) + (dataRevenue.Q1New.Value * Q1Rate);
                            GrossRevenueQ2New = GrossRevenueQ2New + (obj.FirstYearRevenueQ2New.HasValue == true ? obj.FirstYearRevenueQ2New.Value : Convert.ToDecimal(0));
                            obj.FirstYearRevenueQ3New = (dataRevenue.Q3New.Value * Q3Rate) + (dataRevenue.Q2New.Value * Q2Rate) + (dataRevenue.Q1New.Value * Q1Rate);
                            GrossRevenueQ3New = GrossRevenueQ3New + (obj.FirstYearRevenueQ3New.HasValue == true ? obj.FirstYearRevenueQ3New.Value : Convert.ToDecimal(0));
                            obj.FirstYearRevenueQ4New = (dataRevenue.Q4New.Value * Q4Rate) + (dataRevenue.Q3New.Value * Q3Rate) +
                                (dataRevenue.Q2New.Value * Q2Rate) + (dataRevenue.Q1New.Value * Q1Rate);
                            GrossRevenueQ4New = GrossRevenueQ4New + (obj.FirstYearRevenueQ4New.HasValue == true ? obj.FirstYearRevenueQ4New.Value : Convert.ToDecimal(0));
                        }
                        var dataGM = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "GM").FirstOrDefault();
                        if (dataGM != null)
                        {

                            obj.FirstYearGMQ1New = (dataGM.Q1New.Value * Q1Rate);
                            GrossGMQ1New = GrossGMQ1New + (obj.FirstYearGMQ1New.HasValue == true ? obj.FirstYearGMQ1New.Value : Convert.ToDecimal(0));
                            obj.FirstYearGMQ2New = (dataGM.Q2New.Value * Q2Rate) + (dataGM.Q1New.Value * Q1Rate);
                            GrossGMQ2New = GrossGMQ2New + (obj.FirstYearGMQ2New.HasValue == true ? obj.FirstYearGMQ2New.Value : Convert.ToDecimal(0));
                            obj.FirstYearGMQ3New = (dataGM.Q3New.Value * Q3Rate) + (dataGM.Q2New.Value * Q2Rate) + (dataGM.Q1New.Value * Q1Rate);
                            GrossGMQ3New = GrossGMQ3New + (obj.FirstYearGMQ3New.HasValue == true ? obj.FirstYearGMQ3New.Value : Convert.ToDecimal(0));
                            obj.FirstYearGMQ4New = (dataGM.Q4New.Value * Q4Rate) + (dataGM.Q3New.Value * Q3Rate) +
                                (dataGM.Q2New.Value * Q2Rate) + (dataGM.Q1New.Value * Q1Rate);
                            GrossGMQ4New = GrossGMQ4New + (obj.FirstYearGMQ4New.HasValue == true ? obj.FirstYearGMQ4New.Value : Convert.ToDecimal(0));
                        }
                        listResponse.Add(obj);
                    }
                    if (distinctSmartViewCodes.Count() > 0)
                    {
                        listResponse.Add(new ExtractDetailDataModel()
                        {
                            FirstYearGMQ1New = GrossGMQ1New,
                            FirstYearGMQ2New = GrossGMQ2New,
                            FirstYearGMQ3New = GrossGMQ3New,
                            FirstYearGMQ4New = GrossGMQ4New,
                            FirstYearRevenueQ1New = GrossRevenueQ1New,
                            FirstYearRevenueQ2New = GrossRevenueQ2New,
                            FirstYearRevenueQ3New = GrossRevenueQ3New,
                            FirstYearRevenueQ4New = GrossRevenueQ4New,
                            RecordType = "Grand Total"

                        });
                    }
                    List<ExtractHeaderDataModel> headers = new List<ExtractHeaderDataModel>();
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SmartViewName", HeaderText = "SmartView Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "ProjectEntityCode", HeaderText = "ProjectEntity Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "ProjectSegmentCode", HeaderText = "ProjectSegment Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "BUCategoryName", HeaderText = "BUCategory Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "StatutoryCategoryName", HeaderText = "StatutoryCategory Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearRevenueQ1New", HeaderText = "RV Q1 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearRevenueQ2New", HeaderText = "RV Q2 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearRevenueQ3New", HeaderText = "RV Q3 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearRevenueQ4New", HeaderText = "RV Q4 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearGMQ1New", HeaderText = "GM Q1 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearGMQ2New", HeaderText = "GM Q2 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearGMQ3New", HeaderText = "GM Q3 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearGMQ4New", HeaderText = "GM Q4 (YTD)" });
                    objResponse.ListExtractDetailDataModel = listResponse;
                    objResponse.ListCurrencyExchangeData = currencyExchangeDatas;
                    objResponse.ListExtractHeader = headers;
                }
                else
                {

                    decimal Q1Rate = 1;
                    decimal Q2Rate = 1;
                    decimal Q3Rate = 1;
                    decimal Q4Rate = 1;
                    decimal Q1RateNextYear = 1;
                    decimal Q2RateNextYear = 1;
                    decimal Q3RateNextYear = 1;
                    decimal Q4RateNextYear = 1;
                    if (isCurrencyConversionRequired.Trim() == "Y")
                    {
                        try
                        {
                            Q1Rate = currencyExchangeDatas.Where(a => a.Quarter == "Q1" && a.Year == year).FirstOrDefault().AverageRate.Value;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Q2Rate = currencyExchangeDatas.Where(a => a.Quarter == "Q2" && a.Year == year).FirstOrDefault().AverageRate.Value;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Q3Rate = currencyExchangeDatas.Where(a => a.Quarter == "Q3" && a.Year == year).FirstOrDefault().AverageRate.Value;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Q4Rate = currencyExchangeDatas.Where(a => a.Quarter == "Q4" && a.Year == year).FirstOrDefault().AverageRate.Value;
                        }
                        catch (Exception)
                        {
                        }

                        //Q1RateNextYear = currencyExchangeDatas.Where(a => a.Quarter == "Q1" && a.Year == year).FirstOrDefault().AverageRate.Value;
                        //Q2RateNextYear = currencyExchangeDatas.Where(a => a.Quarter == "Q2" && a.Year == year).FirstOrDefault().AverageRate.Value;
                        //Q3RateNextYear = currencyExchangeDatas.Where(a => a.Quarter == "Q3" && a.Year == year).FirstOrDefault().AverageRate.Value;
                        //Q4RateNextYear = currencyExchangeDatas.Where(a => a.Quarter == "Q4" && a.Year == year).FirstOrDefault().AverageRate.Value;
                        Q1RateNextYear = Q4Rate;
                        Q2RateNextYear = Q4Rate;
                        Q3RateNextYear = Q4Rate;
                        Q4RateNextYear = Q4Rate;


                    }
                    decimal GrossRevenueQ1New = Convert.ToDecimal(0);
                    decimal GrossRevenueQ2New = Convert.ToDecimal(0);
                    decimal GrossRevenueQ3New = Convert.ToDecimal(0);
                    decimal GrossRevenueQ4New = Convert.ToDecimal(0);
                    decimal GrossGMQ1New = Convert.ToDecimal(0);
                    decimal GrossGMQ2New = Convert.ToDecimal(0);
                    decimal GrossGMQ3New = Convert.ToDecimal(0);
                    decimal GrossGMQ4New = Convert.ToDecimal(0);

                    decimal GrossRevenueQ1NewNextYear = Convert.ToDecimal(0);
                    decimal GrossRevenueQ2NewNextYear = Convert.ToDecimal(0);
                    decimal GrossRevenueQ3NewNextYear = Convert.ToDecimal(0);
                    decimal GrossRevenueQ4NewNextYear = Convert.ToDecimal(0);
                    decimal GrossGMQ1NewNextYear = Convert.ToDecimal(0);
                    decimal GrossGMQ2NewNextYear = Convert.ToDecimal(0);
                    decimal GrossGMQ3NewNextYear = Convert.ToDecimal(0);
                    decimal GrossGMQ4NewNextYear = Convert.ToDecimal(0);

                    decimal GrossRevenueTCThirdYear = Convert.ToDecimal(0);
                    decimal GrossGMTCThirdYear = Convert.ToDecimal(0);
                    decimal GrossRevenueTCFourthYear = Convert.ToDecimal(0);
                    decimal GrossGMTCFourthYear = Convert.ToDecimal(0);

                    decimal GrossRevenueTCFifthYear = Convert.ToDecimal(0);
                    decimal GrossGMTCFifthYear = Convert.ToDecimal(0);

                    foreach (var item in distinctSmartViewCodes)
                    {
                        ExtractDetailDataModel obj = new ExtractDetailDataModel();
                        obj.RecordType = "P";
                        var dataRevenue = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "RV" && a.Year.Value == year).FirstOrDefault();
                        obj.FirstYearRevenueQ1New = Convert.ToDecimal(0);
                        obj.FirstYearRevenueQ2New = Convert.ToDecimal(0);
                        obj.FirstYearRevenueQ3New = Convert.ToDecimal(0);
                        obj.FirstYearRevenueQ4New = Convert.ToDecimal(0);
                        var isMainDataBinded = false;
                        if (dataRevenue != null)
                        {
                            obj.ProjectEntityCode = dataRevenue.ProjectEntityCode;
                            obj.ProjectSegmentCode = dataRevenue.ProjectSegmentCode;
                            obj.SmartViewCode = dataRevenue.SmartViewCode;
                            obj.SmartViewName = dataRevenue.SmartViewName;
                            obj.BUCategoryName = dataRevenue.BUCategoryName;
                            obj.StatutoryCategoryName = dataRevenue.StatutoryCategoryName;
                            isMainDataBinded = true;
                            obj.FirstYearRevenueQ1New = (dataRevenue.Q1New.Value * Q1Rate);
                            GrossRevenueQ1New = GrossRevenueQ1New + (obj.FirstYearRevenueQ1New.HasValue == true ? obj.FirstYearRevenueQ1New.Value : Convert.ToDecimal(0));
                            obj.FirstYearRevenueQ2New = (dataRevenue.Q2New.Value * Q2Rate) + (dataRevenue.Q1New.Value * Q1Rate);
                            GrossRevenueQ2New = GrossRevenueQ2New + (obj.FirstYearRevenueQ2New.HasValue == true ? obj.FirstYearRevenueQ2New.Value : Convert.ToDecimal(0));
                            obj.FirstYearRevenueQ3New = (dataRevenue.Q3New.Value * Q3Rate) + (dataRevenue.Q2New.Value * Q2Rate) + (dataRevenue.Q1New.Value * Q1Rate);
                            GrossRevenueQ3New = GrossRevenueQ3New + (obj.FirstYearRevenueQ3New.HasValue == true ? obj.FirstYearRevenueQ3New.Value : Convert.ToDecimal(0));
                            obj.FirstYearRevenueQ4New = (dataRevenue.Q4New.Value * Q4Rate) + (dataRevenue.Q3New.Value * Q3Rate) +
                                (dataRevenue.Q2New.Value * Q2Rate) + (dataRevenue.Q1New.Value * Q1Rate);
                            GrossRevenueQ4New = GrossRevenueQ4New + (obj.FirstYearRevenueQ4New.HasValue == true ? obj.FirstYearRevenueQ4New.Value : Convert.ToDecimal(0));
                        }

                        var dataRevenueNextYear = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "RV" && a.Year.Value == (year + 1)).FirstOrDefault();
                        obj.SecondYearRevenueQ1New = Convert.ToDecimal(0);
                        obj.SecondYearRevenueQ2New = Convert.ToDecimal(0);
                        obj.SecondYearRevenueQ3New = Convert.ToDecimal(0);
                        obj.SecondYearRevenueQ4New = Convert.ToDecimal(0);
                        if (dataRevenueNextYear != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataRevenueNextYear.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataRevenueNextYear.ProjectSegmentCode;
                                obj.SmartViewCode = dataRevenueNextYear.SmartViewCode;
                                obj.SmartViewName = dataRevenueNextYear.SmartViewName;
                                obj.BUCategoryName = dataRevenueNextYear.BUCategoryName;
                                obj.StatutoryCategoryName = dataRevenueNextYear.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }
                            obj.SecondYearRevenueQ1New = (dataRevenueNextYear.Q1New.Value * Q1RateNextYear);
                            GrossRevenueQ1NewNextYear = GrossRevenueQ1NewNextYear + (obj.SecondYearRevenueQ1New.HasValue == true ? obj.SecondYearRevenueQ1New.Value : Convert.ToDecimal(0));
                            obj.SecondYearRevenueQ2New = (dataRevenueNextYear.Q2New.Value * Q2RateNextYear) + (dataRevenueNextYear.Q1New.Value * Q1RateNextYear);
                            GrossRevenueQ2NewNextYear = GrossRevenueQ2NewNextYear + (obj.SecondYearRevenueQ2New.HasValue == true ? obj.SecondYearRevenueQ2New.Value : Convert.ToDecimal(0));
                            obj.SecondYearRevenueQ3New = (dataRevenueNextYear.Q3New.Value * Q3RateNextYear) + (dataRevenueNextYear.Q2New.Value * Q2RateNextYear) + (dataRevenueNextYear.Q1New.Value * Q1RateNextYear);
                            GrossRevenueQ3NewNextYear = GrossRevenueQ3NewNextYear + (obj.SecondYearRevenueQ3New.HasValue == true ? obj.SecondYearRevenueQ3New.Value : Convert.ToDecimal(0));
                            obj.SecondYearRevenueQ4New = (dataRevenueNextYear.Q4New.Value * Q4RateNextYear) + (dataRevenueNextYear.Q3New.Value * Q3RateNextYear) +
                                (dataRevenueNextYear.Q2New.Value * Q2RateNextYear) + (dataRevenueNextYear.Q1New.Value * Q1RateNextYear);
                            GrossRevenueQ4NewNextYear = GrossRevenueQ4NewNextYear + (obj.SecondYearRevenueQ4New.HasValue == true ? obj.SecondYearRevenueQ4New.Value : Convert.ToDecimal(0));
                        }
                        var dataRevenueThirdYear = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "RV" && a.Year.Value == (year + 2)).FirstOrDefault();
                        obj.ThirdYearRevenueTC = Convert.ToDecimal(0);
                        if (dataRevenueThirdYear != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataRevenueThirdYear.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataRevenueThirdYear.ProjectSegmentCode;
                                obj.SmartViewCode = dataRevenueThirdYear.SmartViewCode;
                                obj.SmartViewName = dataRevenueThirdYear.SmartViewName;
                                obj.BUCategoryName = dataRevenueThirdYear.BUCategoryName;
                                obj.StatutoryCategoryName = dataRevenueThirdYear.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }
                            obj.ThirdYearRevenueTC = (dataRevenueThirdYear.Q1New.Value * Q4RateNextYear);
                            GrossRevenueTCThirdYear = GrossRevenueTCThirdYear + (obj.ThirdYearRevenueTC.HasValue == true ? obj.ThirdYearRevenueTC.Value : Convert.ToDecimal(0));

                        }
                        var dataRevenueFourthYear = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "RV" && a.Year.Value == (year + 3)).FirstOrDefault();
                        obj.FourthYearRevenueTC = Convert.ToDecimal(0);
                        if (dataRevenueFourthYear != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataRevenueFourthYear.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataRevenueFourthYear.ProjectSegmentCode;
                                obj.SmartViewCode = dataRevenueFourthYear.SmartViewCode;
                                obj.SmartViewName = dataRevenueFourthYear.SmartViewName;
                                obj.BUCategoryName = dataRevenueFourthYear.BUCategoryName;
                                obj.StatutoryCategoryName = dataRevenueFourthYear.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }
                            obj.FourthYearRevenueTC = (dataRevenueFourthYear.Q1New.Value * Q4RateNextYear);
                            GrossRevenueTCFourthYear = GrossRevenueTCFourthYear + (obj.FourthYearRevenueTC.HasValue == true ? obj.FourthYearRevenueTC.Value : Convert.ToDecimal(0));

                        }
                        var dataRevenueFifthYear = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "RV" && a.Year.Value == (year + 4)).FirstOrDefault();
                        obj.FifthYearRevenueTC = Convert.ToDecimal(0);
                        if (dataRevenueFifthYear != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataRevenueFifthYear.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataRevenueFifthYear.ProjectSegmentCode;
                                obj.SmartViewCode = dataRevenueFifthYear.SmartViewCode;
                                obj.SmartViewName = dataRevenueFifthYear.SmartViewName;
                                obj.BUCategoryName = dataRevenueFifthYear.BUCategoryName;
                                obj.StatutoryCategoryName = dataRevenueFifthYear.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }
                            obj.FifthYearRevenueTC = (dataRevenueFifthYear.Q1New.Value * Q4RateNextYear);
                            GrossRevenueTCFifthYear = GrossRevenueTCFifthYear + (obj.FifthYearRevenueTC.HasValue == true ? obj.FifthYearRevenueTC.Value : Convert.ToDecimal(0));

                        }


                        var dataGM = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "GM" && a.Year.Value == year).FirstOrDefault();
                        obj.FirstYearGMQ1New = Convert.ToDecimal(0);
                        obj.FirstYearGMQ2New = Convert.ToDecimal(0);
                        obj.FirstYearGMQ3New = Convert.ToDecimal(0);
                        obj.FirstYearGMQ4New = Convert.ToDecimal(0);
                        if (dataGM != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataGM.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataGM.ProjectSegmentCode;
                                obj.SmartViewCode = dataGM.SmartViewCode;
                                obj.SmartViewName = dataGM.SmartViewName;
                                obj.BUCategoryName = dataGM.BUCategoryName;
                                obj.StatutoryCategoryName = dataGM.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }
                            obj.FirstYearGMQ1New = (dataGM.Q1New.Value * Q1Rate);
                            GrossGMQ1New = GrossGMQ1New + (obj.FirstYearGMQ1New.HasValue == true ? obj.FirstYearGMQ1New.Value : Convert.ToDecimal(0));
                            obj.FirstYearGMQ2New = (dataGM.Q2New.Value * Q2Rate) + (dataGM.Q1New.Value * Q1Rate);
                            GrossGMQ2New = GrossGMQ2New + (obj.FirstYearGMQ2New.HasValue == true ? obj.FirstYearGMQ2New.Value : Convert.ToDecimal(0));
                            obj.FirstYearGMQ3New = (dataGM.Q3New.Value * Q3Rate) + (dataGM.Q2New.Value * Q2Rate) + (dataGM.Q1New.Value * Q1Rate);
                            GrossGMQ3New = GrossGMQ3New + (obj.FirstYearGMQ3New.HasValue == true ? obj.FirstYearGMQ3New.Value : Convert.ToDecimal(0));
                            obj.FirstYearGMQ4New = (dataGM.Q4New.Value * Q4Rate) + (dataGM.Q3New.Value * Q3Rate) +
                                (dataGM.Q2New.Value * Q2Rate) + (dataGM.Q1New.Value * Q1Rate);
                            GrossGMQ4New = GrossGMQ4New + (obj.FirstYearGMQ4New.HasValue == true ? obj.FirstYearGMQ4New.Value : Convert.ToDecimal(0));
                        }

                        var dataGMNextYear = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "GM" && a.Year.Value == (year + 1)).FirstOrDefault();
                        obj.SecondYearGMQ1New = Convert.ToDecimal(0);
                        obj.SecondYearGMQ2New = Convert.ToDecimal(0);
                        obj.SecondYearGMQ3New = Convert.ToDecimal(0);
                        obj.SecondYearGMQ4New = Convert.ToDecimal(0);
                        if (dataGMNextYear != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataGMNextYear.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataGMNextYear.ProjectSegmentCode;
                                obj.SmartViewCode = dataGMNextYear.SmartViewCode;
                                obj.SmartViewName = dataGMNextYear.SmartViewName;
                                obj.BUCategoryName = dataGMNextYear.BUCategoryName;
                                obj.StatutoryCategoryName = dataGMNextYear.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }

                            obj.SecondYearGMQ1New = (dataGMNextYear.Q1New.Value * Q1RateNextYear);
                            GrossGMQ1NewNextYear = GrossGMQ1NewNextYear + (obj.SecondYearGMQ1New.HasValue == true ? obj.SecondYearGMQ1New.Value : Convert.ToDecimal(0));
                            obj.SecondYearGMQ2New = (dataGMNextYear.Q2New.Value * Q2RateNextYear) + (dataGMNextYear.Q1New.Value * Q1RateNextYear);
                            GrossGMQ2NewNextYear = GrossGMQ2NewNextYear + (obj.SecondYearGMQ2New.HasValue == true ? obj.SecondYearGMQ2New.Value : Convert.ToDecimal(0));
                            obj.SecondYearGMQ3New = (dataGMNextYear.Q3New.Value * Q3RateNextYear) + (dataGMNextYear.Q2New.Value * Q2RateNextYear) + (dataGMNextYear.Q1New.Value * Q1RateNextYear);
                            GrossGMQ3NewNextYear = GrossGMQ3NewNextYear + (obj.SecondYearGMQ3New.HasValue == true ? obj.SecondYearGMQ3New.Value : Convert.ToDecimal(0));
                            obj.SecondYearGMQ4New = (dataGMNextYear.Q4New.Value * Q4RateNextYear) + (dataGMNextYear.Q3New.Value * Q3RateNextYear) +
                                (dataGMNextYear.Q2New.Value * Q2RateNextYear) + (dataGMNextYear.Q1New.Value * Q1RateNextYear);
                            GrossGMQ4NewNextYear = GrossGMQ4NewNextYear + (obj.SecondYearGMQ4New.HasValue == true ? obj.SecondYearGMQ4New.Value : Convert.ToDecimal(0));
                        }
                        var dataGMThirdYear = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "GM" && a.Year.Value == (year + 2)).FirstOrDefault();
                        obj.ThirdYearGMTC = Convert.ToDecimal(0);
                        if (dataGMThirdYear != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataGMThirdYear.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataGMThirdYear.ProjectSegmentCode;
                                obj.SmartViewCode = dataGMThirdYear.SmartViewCode;
                                obj.SmartViewName = dataGMThirdYear.SmartViewName;
                                obj.BUCategoryName = dataGMThirdYear.BUCategoryName;
                                obj.StatutoryCategoryName = dataGMThirdYear.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }
                            obj.ThirdYearGMTC = (dataGMThirdYear.Q1New.Value * Q4RateNextYear);
                            GrossGMTCThirdYear = GrossGMTCThirdYear + (obj.ThirdYearGMTC.HasValue == true ? obj.ThirdYearGMTC.Value : Convert.ToDecimal(0));

                        }

                        var dataGMFourthYear = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "GM" && a.Year.Value == (year + 3)).FirstOrDefault();
                        obj.FourthYearGMTC = Convert.ToDecimal(0);
                        if (dataGMFourthYear != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataGMFourthYear.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataGMFourthYear.ProjectSegmentCode;
                                obj.SmartViewCode = dataGMFourthYear.SmartViewCode;
                                obj.SmartViewName = dataGMFourthYear.SmartViewName;
                                obj.BUCategoryName = dataGMFourthYear.BUCategoryName;
                                obj.StatutoryCategoryName = dataGMFourthYear.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }
                            obj.FourthYearGMTC = (dataGMFourthYear.Q1New.Value * Q4RateNextYear);
                            GrossGMTCFourthYear = GrossGMTCFourthYear + (obj.FourthYearGMTC.HasValue == true ? obj.FourthYearGMTC.Value : Convert.ToDecimal(0));
                        }

                        var dataGMFifthYear = repExtractResponses.Where(a => a.SmartViewCode == item.ToString()
                        && a.FinancialDataTypeCode == "GM" && a.Year.Value == (year + 4)).FirstOrDefault();
                        obj.FifthYearGMTC = Convert.ToDecimal(0);
                        if (dataGMFifthYear != null)
                        {
                            if (!isMainDataBinded)
                            {
                                obj.ProjectEntityCode = dataGMFifthYear.ProjectEntityCode;
                                obj.ProjectSegmentCode = dataGMFifthYear.ProjectSegmentCode;
                                obj.SmartViewCode = dataGMFifthYear.SmartViewCode;
                                obj.SmartViewName = dataGMFifthYear.SmartViewName;
                                obj.BUCategoryName = dataGMFifthYear.BUCategoryName;
                                obj.StatutoryCategoryName = dataGMFifthYear.StatutoryCategoryName;
                                isMainDataBinded = true;
                            }
                            obj.FifthYearGMTC = (dataGMFifthYear.Q1New.Value * Q4RateNextYear);
                            GrossGMTCFifthYear = GrossGMTCFifthYear + (obj.FifthYearGMTC.HasValue == true ? obj.FifthYearGMTC.Value : Convert.ToDecimal(0));
                        }
                        listResponse.Add(obj);
                    }
                    if (distinctSmartViewCodes.Count() > 0)
                    {
                        listResponse.Add(new ExtractDetailDataModel()
                        {
                            FirstYearGMQ1New = GrossGMQ1New,
                            FirstYearGMQ2New = GrossGMQ2New,
                            FirstYearGMQ3New = GrossGMQ3New,
                            FirstYearGMQ4New = GrossGMQ4New,
                            FirstYearRevenueQ1New = GrossRevenueQ1New,
                            FirstYearRevenueQ2New = GrossRevenueQ2New,
                            FirstYearRevenueQ3New = GrossRevenueQ3New,
                            FirstYearRevenueQ4New = GrossRevenueQ4New,

                            SecondYearGMQ1New = GrossGMQ1NewNextYear,
                            SecondYearGMQ2New = GrossGMQ2NewNextYear,
                            SecondYearGMQ3New = GrossGMQ3NewNextYear,
                            SecondYearGMQ4New = GrossGMQ4NewNextYear,
                            SecondYearRevenueQ1New = GrossRevenueQ1NewNextYear,
                            SecondYearRevenueQ2New = GrossRevenueQ2NewNextYear,
                            SecondYearRevenueQ3New = GrossRevenueQ3NewNextYear,
                            SecondYearRevenueQ4New = GrossRevenueQ4NewNextYear,

                            ThirdYearRevenueTC = GrossRevenueTCThirdYear,
                            ThirdYearGMTC = GrossGMTCThirdYear,

                            FourthYearRevenueTC = GrossRevenueTCFourthYear,
                            FourthYearGMTC = GrossGMTCFourthYear,

                            FifthYearRevenueTC = GrossRevenueTCFifthYear,
                            FifthYearGMTC = GrossGMTCFifthYear,

                            RecordType = "Grand Total",


                        });
                    }
                    List<ExtractHeaderDataModel> headers = new List<ExtractHeaderDataModel>();
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SmartViewName", HeaderText = "SmartView Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "ProjectEntityCode", HeaderText = "ProjectEntity Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "ProjectSegmentCode", HeaderText = "ProjectSegment Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "BUCategoryName", HeaderText = "BUCategory Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "StatutoryCategoryName", HeaderText = "StatutoryCategory Name" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearRevenueQ1New", HeaderText = "RV Q1 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearRevenueQ2New", HeaderText = "RV Q2 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearRevenueQ3New", HeaderText = "RV Q3 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearRevenueQ4New", HeaderText = "RV Q4 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearGMQ1New", HeaderText = "GM Q1 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearGMQ2New", HeaderText = "GM Q2 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearGMQ3New", HeaderText = "GM Q3 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FirstYearGMQ4New", HeaderText = "GM Q4 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SecondYearRevenueQ1New", HeaderText = "RV Q1 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SecondYearRevenueQ2New", HeaderText = "RV Q2 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SecondYearRevenueQ3New", HeaderText = "RV Q3 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SecondYearRevenueQ4New", HeaderText = "RV Q4 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SecondYearGMQ1New", HeaderText = "GM Q1 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SecondYearGMQ2New", HeaderText = "GM Q2 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SecondYearGMQ3New", HeaderText = "GM Q3 (YTD)" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "SecondYearGMQ4New", HeaderText = "GM Q4 (YTD)" });

                    headers.Add(new ExtractHeaderDataModel() { FieldName = "ThirdYearRevenueTC", HeaderText = "FY" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "ThirdYearYearGMTC", HeaderText = "FY" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FourthYearRevenueTC", HeaderText = "FY" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FourthYearYearGMTC", HeaderText = "FY" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FifthYearRevenueTC", HeaderText = "FY" });
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "FifthYearYearGMTC", HeaderText = "FY" });
                    objResponse.ListExtractDetailDataModel = listResponse;
                    objResponse.ListCurrencyExchangeData = currencyExchangeDatas;
                    objResponse.ListExtractHeader = headers;
                }
                return objResponse;
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
        public ProjectLifeCycle ProjectLifeCycleReport(int projectid, string scenarioscope)
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var reportData = new ProjectLifeCycle();
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ProjectLifeCycleReport";
                cmd.Parameters.AddWithValue("@P_ProjectId", projectid);
                cmd.Parameters.AddWithValue("@P_CurrentYear", currentYear);
                cmd.Parameters.AddWithValue("@P_ScenariotypeCode", scenarioscope);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    reportData.ProjectId = ds.Tables[0].Rows[0][0].ToString();
                    reportData.ProjectName = ds.Tables[0].Rows[0][1].ToString();

                }
                reportData.ProjectLifeCycleRevenueData = new List<ProjectLifeCycleData>();
                reportData.ProjectLifeCycleGMData = new List<ProjectLifeCycleData>();
                int rvYear = 1;
                int gmYear = 1;
                if ((ds != null) && (ds.Tables.Count > 1) && (ds.Tables[1] != null) && (ds.Tables[1].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[1].Rows[i]["FinancialDataTypeCode"]) == "RV")
                        {
                            ProjectLifeCycleData data5 = new ProjectLifeCycleData()
                            {
                                Active = true,
                                Amount = Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["Amount"])),
                                Year = Convert.ToInt32(Convert.ToString(ds.Tables[1].Rows[i]["Year"])),
                                HeaderText = Convert.ToString(ds.Tables[1].Rows[i]["ScenarioName"]),
                                YearText = "Y" + rvYear.ToString() + "  " + Convert.ToString(ds.Tables[1].Rows[i]["Year"])

                            };
                            reportData.ProjectLifeCycleRevenueData.Add(data5);
                            rvYear++;
                        }
                        else if (Convert.ToString(ds.Tables[1].Rows[i]["FinancialDataTypeCode"]) == "GM")
                        {
                            ProjectLifeCycleData data6 = new ProjectLifeCycleData()
                            {
                                Active = true,
                                Amount = Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["Amount"])),
                                Year = Convert.ToInt32(Convert.ToString(ds.Tables[1].Rows[i]["Year"])),
                                HeaderText = Convert.ToString(ds.Tables[1].Rows[i]["ScenarioName"]),
                                YearText = "Y" + gmYear.ToString() + "  " + Convert.ToString(ds.Tables[1].Rows[i]["Year"])

                            };
                            reportData.ProjectLifeCycleGMData.Add(data6);
                            gmYear++;
                        }

                    }
                }
                if ((ds != null) && (ds.Tables.Count > 2) && (ds.Tables[2] != null) && (ds.Tables[2].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[2].Rows[i]["FinancialDataTypeCode"]) == "RV")
                        {
                            ProjectLifeCycleData data3 = new ProjectLifeCycleData()
                            {
                                Active = true,
                                Amount = Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["Amount"])),
                                Year = Convert.ToInt32(Convert.ToString(ds.Tables[2].Rows[i]["Year"])),
                                HeaderText = Convert.ToString(ds.Tables[2].Rows[i]["ScenarioName"]),
                                YearText = "Y" + rvYear.ToString() + "  " + Convert.ToString(ds.Tables[2].Rows[i]["Year"])

                            };
                            reportData.ProjectLifeCycleRevenueData.Add(data3);
                            rvYear++;
                        }
                        else if (Convert.ToString(ds.Tables[2].Rows[i]["FinancialDataTypeCode"]) == "GM")
                        {
                            ProjectLifeCycleData data4 = new ProjectLifeCycleData()
                            {
                                Active = true,
                                Amount = Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["Amount"])),
                                Year = Convert.ToInt32(Convert.ToString(ds.Tables[2].Rows[i]["Year"])),
                                HeaderText = Convert.ToString(ds.Tables[2].Rows[i]["ScenarioName"]),
                                YearText = "Y" + gmYear.ToString() + "  " + Convert.ToString(ds.Tables[2].Rows[i]["Year"])

                            };
                            reportData.ProjectLifeCycleGMData.Add(data4);
                            gmYear++;
                        }

                    }
                }
                decimal rvAmount = Convert.ToDecimal(00.0);
                decimal gmAmount = Convert.ToDecimal(00.0);
                if ((ds != null) && (ds.Tables.Count > 3) && (ds.Tables[3] != null) && (ds.Tables[3].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[3].Rows[i]["FinancialDataTypeCode"]) == "RV")
                        {
                            rvAmount += Convert.ToDecimal(Convert.ToString(ds.Tables[3].Rows[i]["Amount"]));

                        }
                        else if (Convert.ToString(ds.Tables[3].Rows[i]["FinancialDataTypeCode"]) == "GM")
                        {
                            gmAmount += Convert.ToDecimal(Convert.ToString(ds.Tables[3].Rows[i]["Amount"]));
                        }

                    }

                }
                ProjectLifeCycleData data = new ProjectLifeCycleData()
                {
                    Active = true,
                    Amount = rvAmount,
                    Year = (currentYear + 1),
                    HeaderText = "Latest Forecast for available future years",
                    YearText = (currentYear + 1).ToString() + "+"

                };
                reportData.ProjectLifeCycleRevenueData.Add(data);

                ProjectLifeCycleData data1 = new ProjectLifeCycleData()
                {
                    Active = true,
                    Amount = gmAmount,
                    Year = (currentYear + 1),
                    HeaderText = "Latest Forecast for available future years",
                    YearText = (currentYear + 1).ToString() + "+"

                };
                reportData.ProjectLifeCycleGMData.Add(data1);

                return reportData;
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

        public ProjectLifeCycleDataModel ProjectLifeCycleReport1(int projectid, string scenarioscope)
        {
            try
            {
                ProjectLifeCycleDataModel obj = new ProjectLifeCycleDataModel();
                var currentYear = DateTime.Now.Year;
                var reportData = new ProjectLifeCycle();
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ProjectLifeCycleReport";
                cmd.Parameters.AddWithValue("@P_ProjectId", projectid);
                cmd.Parameters.AddWithValue("@P_CurrentYear", currentYear);
                cmd.Parameters.AddWithValue("@P_ScenariotypeCode", scenarioscope);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    reportData.ProjectId = ds.Tables[0].Rows[0][0].ToString();
                    reportData.ProjectName = ds.Tables[0].Rows[0][1].ToString();

                }

                List<ExtractHeaderDataModel> headers = new List<ExtractHeaderDataModel>();
                headers.Add(new ExtractHeaderDataModel() { FieldName = "ProjectId", HeaderText = "Project Code" });
                headers.Add(new ExtractHeaderDataModel() { FieldName = "ProjectName", HeaderText = "Project Name" });
                headers.Add(new ExtractHeaderDataModel() { FieldName = "Type", HeaderText = "Type" });

                List<ExtractDataDataModel> rvData = new List<ExtractDataDataModel>();
                List<ExtractDataDataModel> gmData = new List<ExtractDataDataModel>();
                rvData.Add(new ExtractDataDataModel() { FieldName = "ProjectId", DataValue = reportData.ProjectId });
                rvData.Add(new ExtractDataDataModel() { FieldName = "ProjectName", DataValue = reportData.ProjectName });
                rvData.Add(new ExtractDataDataModel() { FieldName = "Type", DataValue = "REVENUE" });

                gmData.Add(new ExtractDataDataModel() { FieldName = "ProjectId", DataValue = reportData.ProjectId });
                gmData.Add(new ExtractDataDataModel() { FieldName = "ProjectName", DataValue = reportData.ProjectName });
                gmData.Add(new ExtractDataDataModel() { FieldName = "Type", DataValue = "GROSS MARGIN" });


                decimal rvGrandTotal = Convert.ToDecimal(0);
                decimal gmGrandTotal = Convert.ToDecimal(0);
                reportData.ProjectLifeCycleRevenueData = new List<ProjectLifeCycleData>();
                reportData.ProjectLifeCycleGMData = new List<ProjectLifeCycleData>();
                int rvYear = 1;
                int gmYear = 1;
                if ((ds != null) && (ds.Tables.Count > 1) && (ds.Tables[1] != null) && (ds.Tables[1].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[1].Rows[i]["FinancialDataTypeCode"]) == "RV")
                        {
                            headers.Add(new ExtractHeaderDataModel() { FieldName = "Y" + rvYear.ToString() + Convert.ToString(ds.Tables[1].Rows[i]["Year"]), HeaderText = "Y" + rvYear.ToString() + "  " + Convert.ToString(ds.Tables[1].Rows[i]["Year"]) });
                            rvData.Add(new ExtractDataDataModel() { FieldName = "Y" + rvYear.ToString() + Convert.ToString(ds.Tables[1].Rows[i]["Year"]), DataValue = Convert.ToString(ds.Tables[1].Rows[i]["Amount"]) });
                            rvGrandTotal += Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["Amount"]));
                            rvYear++;
                        }
                        else if (Convert.ToString(ds.Tables[1].Rows[i]["FinancialDataTypeCode"]) == "GM")
                        {
                            gmData.Add(new ExtractDataDataModel() { FieldName = "Y" + rvYear.ToString() + Convert.ToString(ds.Tables[1].Rows[i]["Year"]), DataValue = Convert.ToString(ds.Tables[1].Rows[i]["Amount"]) });
                            gmGrandTotal += Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["Amount"]));
                            gmYear++;
                        }

                    }

                }
                if ((ds != null) && (ds.Tables.Count > 2) && (ds.Tables[2] != null) && (ds.Tables[2].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[2].Rows[i]["FinancialDataTypeCode"]) == "RV")
                        {
                            headers.Add(new ExtractHeaderDataModel() { FieldName = "Y" + rvYear.ToString() + Convert.ToString(ds.Tables[2].Rows[i]["Year"]), HeaderText = "Y" + rvYear.ToString() + "  " + Convert.ToString(ds.Tables[2].Rows[i]["Year"]) });
                            rvData.Add(new ExtractDataDataModel() { FieldName = "Y" + rvYear.ToString() + Convert.ToString(ds.Tables[2].Rows[i]["Year"]), DataValue = Convert.ToString(ds.Tables[2].Rows[i]["Amount"]) });
                            rvGrandTotal += Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["Amount"]));
                            rvYear++;
                        }
                        else if (Convert.ToString(ds.Tables[2].Rows[i]["FinancialDataTypeCode"]) == "GM")
                        {
                            gmData.Add(new ExtractDataDataModel() { FieldName = "Y" + rvYear.ToString() + Convert.ToString(ds.Tables[2].Rows[i]["Year"]), DataValue = Convert.ToString(ds.Tables[2].Rows[i]["Amount"]) });
                            gmGrandTotal += Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["Amount"]));
                            gmYear++;
                        }

                    }
                }
                decimal rvAmount = Convert.ToDecimal(00.0);
                decimal gmAmount = Convert.ToDecimal(00.0);
                if ((ds != null) && (ds.Tables.Count > 3) && (ds.Tables[3] != null) && (ds.Tables[3].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[3].Rows[i]["FinancialDataTypeCode"]) == "RV")
                        {
                            rvAmount += Convert.ToDecimal(Convert.ToString(ds.Tables[3].Rows[i]["Amount"]));
                            rvGrandTotal += Convert.ToDecimal(Convert.ToString(ds.Tables[3].Rows[i]["Amount"]));
                        }
                        else if (Convert.ToString(ds.Tables[3].Rows[i]["FinancialDataTypeCode"]) == "GM")
                        {
                            gmAmount += Convert.ToDecimal(Convert.ToString(ds.Tables[3].Rows[i]["Amount"]));
                            gmGrandTotal += Convert.ToDecimal(Convert.ToString(ds.Tables[3].Rows[i]["Amount"]));
                        }


                    }
                    headers.Add(new ExtractHeaderDataModel() { FieldName = "Y" + (currentYear + 1).ToString(), HeaderText = "Y" + rvYear.ToString() + "  " + (currentYear + 1).ToString() + " + " });
                    rvData.Add(new ExtractDataDataModel() { FieldName = "Y" + (currentYear + 1).ToString(), DataValue = rvAmount.ToString() });
                    gmData.Add(new ExtractDataDataModel() { FieldName = "Y" + (currentYear + 1).ToString(), DataValue = gmAmount.ToString() });


                }


                headers.Add(new ExtractHeaderDataModel() { FieldName = "GrandTotal", HeaderText = "Grand Total" });
                rvData.Add(new ExtractDataDataModel() { FieldName = "GrandTotal", DataValue = rvGrandTotal.ToString() });
                gmData.Add(new ExtractDataDataModel() { FieldName = "GrandTotal", DataValue = gmGrandTotal.ToString() });

                obj.Header = headers;
                obj.RVDataValue = rvData;
                obj.GMDataValue = gmData;
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
        public DeviationAnalysisYearWiseModel GetDeviationAnalysisReport(DevianceReportConfig config)
        {
            try
            {
                var reportData = new DeviationAnalysisYearWiseModel();
                decimal[] orgData = new decimal[12];
                decimal[] totalnonOrgData = new decimal[12];
                List<NonOrgDataModel> depclientData = new List<NonOrgDataModel>();
                List<DepartmentWiseDataModel> departmentDataModel = new List<DepartmentWiseDataModel>();
                List<ProjectDataModel> projectDataModel = new List<ProjectDataModel>();
                List<BudgetDeviationDataModel> budgetDataModel = new List<BudgetDeviationDataModel>();
                List<BudgetDeviationDataModel> forecastDataModel = new List<BudgetDeviationDataModel>();
                //List<decimal> nonOrgData = new List<decimal>();
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeviationAnalysisReport";
                cmd.Parameters.AddWithValue("@P_BaseOrgScenarioId", config.OrgScenarioId);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeID", config.ScenarioDataTypeId);
                cmd.Parameters.AddWithValue("@P_Spec", config.Spec);
                cmd.Parameters.AddWithValue("@P_DepartmentID", config.DepartmentId);
                //cmd.Parameters.AddWithValue("@P_ClientId", config.ClientId);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    for(int i=0;i < 12;i++)
                    {
                        decimal val = Convert.ToDecimal(ds.Tables[0].Rows[0][i]);
                        orgData[i]=val;
                    }
                      

                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null) && (ds.Tables[1].Rows.Count > 0))
                {
                    for (int i = 0; i < 12; i++)
                    {
                        decimal val = Convert.ToDecimal(ds.Tables[1].Rows[0][i]);
                        totalnonOrgData[i] = val;
                    }
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[2] != null) && (ds.Tables[2].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        DepartmentWiseDataModel client = new DepartmentWiseDataModel();
                        client.DepartmentName = ds.Tables[2].Rows[i]["DepartmentName"].ToString().Decrypt();
                        client.ScenarioName = ds.Tables[2].Rows[i]["ScenarioName"].ToString();
                        client.Q1 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q1"]);
                        client.Q2 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q2"]);
                        client.Q3 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q3"]);
                        client.Q4 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q4"]);
                        client.Q5 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q5"]);
                        client.Q6 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q6"]);
                        client.Q7 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q7"]);
                        client.Q8 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q8"]);
                        client.Q9 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q9"]);
                        client.Q10 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q10"]);
                        client.Q11 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q11"]);
                        client.Q12 = Convert.ToDecimal(ds.Tables[2].Rows[i]["Q12"]);
                        departmentDataModel.Add(client);
                    }
                    
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[3] != null) && (ds.Tables[3].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ProjectDataModel client = new ProjectDataModel();
                        client.ProjectName = ds.Tables[3].Rows[i]["ProjectName"].ToString();
                        client.ScenarioName = ds.Tables[3].Rows[i]["ScenarioName"].ToString();
                        client.Q1 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q1"]);
                        client.Q2 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q2"]);
                        client.Q3 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q3"]);
                        client.Q4 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q4"]);
                        client.Q5 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q5"]);
                        client.Q6 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q6"]);
                        client.Q7 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q7"]);
                        client.Q8 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q8"]);
                        client.Q9 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q9"]);
                        client.Q10 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q10"]);
                        client.Q11 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q11"]);
                        client.Q12 = Convert.ToDecimal(ds.Tables[3].Rows[i]["Q12"]);
                        projectDataModel.Add(client);
                    }

                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[4] != null) && (ds.Tables[4].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        BudgetDeviationDataModel client = new BudgetDeviationDataModel();
                        client.ScenarioName = ds.Tables[4].Rows[i]["ScenarioName"].ToString();
                        client.ScenarioType = ds.Tables[4].Rows[i]["ScenarioType"].ToString();
                        client.Q1 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q1"]);
                        client.Q2 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q2"]);
                        client.Q3 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q3"]);
                        client.Q4 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q4"]);
                        client.Q5 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q5"]);
                        client.Q6 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q6"]);
                        client.Q7 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q7"]);
                        client.Q8 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q8"]);
                        client.Q9 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q9"]);
                        client.Q10 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q10"]);
                        client.Q11 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q11"]);
                        client.Q12 = Convert.ToDecimal(ds.Tables[4].Rows[i]["Q12"]);
                        budgetDataModel.Add(client);
                    }

                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[5] != null) && (ds.Tables[5].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                    {
                        BudgetDeviationDataModel client = new BudgetDeviationDataModel();
                        client.ScenarioType = ds.Tables[5].Rows[i]["ScenarioType"].ToString();
                        client.ScenarioName = ds.Tables[5].Rows[i]["ScenarioName"].ToString();
                        client.Q1 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q1"]);
                        client.Q2 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q2"]);
                        client.Q3 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q3"]);
                        client.Q4 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q4"]);
                        client.Q5 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q5"]);
                        client.Q6 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q6"]);
                        client.Q7 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q7"]);
                        client.Q8 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q8"]);
                        client.Q9 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q9"]);
                        client.Q10 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q10"]);
                        client.Q11 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q11"]);
                        client.Q12 = Convert.ToDecimal(ds.Tables[5].Rows[i]["Q12"]);
                        forecastDataModel.Add(client);
                    }

                }

                reportData.OrgData = orgData;
                reportData.NonOrgData = totalnonOrgData;
                reportData.DepartmentYearlyData = departmentDataModel;
                reportData.ProjectYearlyData = projectDataModel;
                reportData.BudgetDeviationData = budgetDataModel;
                reportData.ForecastDeviationData = forecastDataModel;
                return reportData;
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
        public DashboardDataModel GetDashboardData(DashboardConfig config)
        {
            try
            {
                var reportData = new DashboardDataModel();
                
                List<BudgetDeviationDataModel> budgetDataModel = new List<BudgetDeviationDataModel>();
                List<BudgetDeviationDataModel> forecastDataModel = new List<BudgetDeviationDataModel>();
                List<FinancialDataGross> financialDataModel = new List<FinancialDataGross>();
                List<DifferenceData> differenceDataModel = new List<DifferenceData>();
                //List<decimal> nonOrgData = new List<decimal>();
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DashboardData";
                cmd.Parameters.AddWithValue("@P_Scope", config.Scope);
                cmd.Parameters.AddWithValue("@P_Year", config.Year);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeID", config.ScenarioDataTypeId);
                //cmd.Parameters.AddWithValue("@P_DepartmentID", config.DepartmentId);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        BudgetDeviationDataModel client = new BudgetDeviationDataModel();
                        client.ScenarioType = ds.Tables[0].Rows[i]["ScenarioType"].ToString();
                        client.ScenarioName = ds.Tables[0].Rows[i]["ScenarioName"].ToString();
                        client.Q1 = ds.Tables[0].Rows[i]["Q1"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["Q1"]);
                        client.Q2 = ds.Tables[0].Rows[i]["Q2"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["Q2"]);
                        client.Q3 = ds.Tables[0].Rows[i]["Q3"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["Q3"]);
                        client.Q4 = ds.Tables[0].Rows[i]["Q4"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[i]["Q4"]);
                        budgetDataModel.Add(client);
                    }

                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1] != null)&&(ds.Tables[1].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        BudgetDeviationDataModel client = new BudgetDeviationDataModel();
                        client.ScenarioType = ds.Tables[1].Rows[i]["ScenarioType"].ToString();
                        client.ScenarioName = ds.Tables[1].Rows[i]["ScenarioName"].ToString();
                        client.Q1 = ds.Tables[1].Rows[i]["Q1"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[i]["Q1"]);
                        client.Q2 = ds.Tables[1].Rows[i]["Q2"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[i]["Q2"]);
                        client.Q3 = ds.Tables[1].Rows[i]["Q3"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[i]["Q3"]);
                        client.Q4 = ds.Tables[1].Rows[i]["Q4"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[i]["Q4"]);
                        forecastDataModel.Add(client);
                    }

                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[2] != null) && (ds.Tables[2].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        FinancialDataGross client = new FinancialDataGross();
                        client.GrossYear = ds.Tables[2].Rows[i]["GrossYear"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["GrossYear"]);
                        client.FinancialDataTypeName = ds.Tables[2].Rows[i]["FinancialDataTypeName"] == DBNull.Value ? "":ds.Tables[2].Rows[i]["FinancialDataTypeName"].ToString();
                        client.ScenarioType = ds.Tables[2].Rows[i]["ScenarioType"] == DBNull.Value ? "" : ds.Tables[2].Rows[i]["ScenarioType"].ToString();
                        financialDataModel.Add(client);
                    }

                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[3] != null) && (ds.Tables[3].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        DifferenceData deviation = new DifferenceData();
                        decimal? grossYear = 0;
                        if(ds.Tables[3].Rows[i]["FinancialDataTypeName"].ToString() != "")
                        {
                            deviation.FinancialDataTypeName = ds.Tables[3].Rows[i]["FinancialDataTypeName"].ToString();
                            decimal? grossPrevYear = ds.Tables[3].Rows[i]["GrossYear"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[3].Rows[i]["GrossYear"]);
                            grossYear = financialDataModel[i].GrossYear - grossPrevYear;
                            deviation.Difference = grossPrevYear == 0 ? 0 : grossYear / grossPrevYear * 100;
                            differenceDataModel.Add(deviation);
                        }
                       
                    }

                }


                reportData.BudgetDeviationData = budgetDataModel;
                reportData.ForecastDeviationData = forecastDataModel;
                reportData.FinancialDataGross = financialDataModel;
                reportData.DifferenceData = differenceDataModel;
                return reportData;
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
        public FinancePerformanceDataModel FinancePerformanceReport(DashboardConfig config)
        {
            try
            {
                var reportData = new FinancePerformanceDataModel();

                BudgetDeviationDataModel budgetDataModel = new BudgetDeviationDataModel();
                List<DepartmentWiseDataModel> listchartData = new List<DepartmentWiseDataModel>();
                decimal GrossYear = 0;
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "FinancePerformanceReport";
                cmd.Parameters.AddWithValue("@P_Scope", config.Scope);
                cmd.Parameters.AddWithValue("@P_Year", config.Year);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeCode", config.ScenarioDataTypeId);
                cmd.Parameters.AddWithValue("@P_Department", config.DepartmentId);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    budgetDataModel.ScenarioName = ds.Tables[0].Rows[0]["ScenarioName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["ScenarioName"].ToString();
                    budgetDataModel.Q1 = ds.Tables[0].Rows[0]["Q1"] == DBNull.Value ? 0:Convert.ToDecimal(ds.Tables[0].Rows[0]["Q1"]);
                    budgetDataModel.Q2 = ds.Tables[0].Rows[0]["Q2"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q2"]);
                    budgetDataModel.Q3 = ds.Tables[0].Rows[0]["Q3"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q3"]);
                    budgetDataModel.Q4 = ds.Tables[0].Rows[0]["Q4"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q4"]);
                    budgetDataModel.Q5 = ds.Tables[0].Rows[0]["Q5"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q5"]);
                    budgetDataModel.Q6 = ds.Tables[0].Rows[0]["Q6"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q6"]);
                    budgetDataModel.Q7 = ds.Tables[0].Rows[0]["Q7"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q7"]);
                    budgetDataModel.Q8 = ds.Tables[0].Rows[0]["Q8"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q8"]);
                    budgetDataModel.Q9 = ds.Tables[0].Rows[0]["Q9"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q9"]);
                    budgetDataModel.Q10 = ds.Tables[0].Rows[0]["Q10"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q10"]);
                    budgetDataModel.Q11 = ds.Tables[0].Rows[0]["Q11"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q11"]);
                    budgetDataModel.Q12 = ds.Tables[0].Rows[0]["Q12"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q12"]);

                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1].Rows.Count > 0))
                {
                    GrossYear = ds.Tables[1].Rows[0][0] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[0][0].ToString());
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[2].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        DepartmentWiseDataModel chartData = new DepartmentWiseDataModel();
                        chartData.ScenarioName = ds.Tables[2].Rows[i]["ScenarioName"] == DBNull.Value ? "" : ds.Tables[2].Rows[i]["ScenarioName"].ToString();
                        chartData.DepartmentName = ds.Tables[2].Rows[i]["DepartmentName"] == DBNull.Value ? "" : ds.Tables[2].Rows[i]["DepartmentName"].ToString().Decrypt();
                        chartData.Q1 = ds.Tables[2].Rows[i]["Q1"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q1"]);
                        chartData.Q2 = ds.Tables[2].Rows[i]["Q2"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q2"]);
                        chartData.Q3 = ds.Tables[2].Rows[i]["Q3"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q3"]);
                        chartData.Q4 = ds.Tables[2].Rows[i]["Q4"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q4"]);

                        chartData.Q5 = ds.Tables[2].Rows[i]["Q5"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q5"]);
                        chartData.Q6 = ds.Tables[2].Rows[i]["Q6"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q6"]);
                        chartData.Q7 = ds.Tables[2].Rows[i]["Q7"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q7"]);
                        chartData.Q8 = ds.Tables[2].Rows[i]["Q8"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q8"]);
                        chartData.Q9 = ds.Tables[2].Rows[i]["Q9"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q9"]);
                        chartData.Q10 = ds.Tables[2].Rows[i]["Q10"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q10"]);
                        chartData.Q11 = ds.Tables[2].Rows[i]["Q11"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q11"]);
                        chartData.Q12 = ds.Tables[2].Rows[i]["Q12"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q12"]);

                        listchartData.Add(chartData);
                    }
                }

                reportData.FinanceData = budgetDataModel;
                reportData.FinancialDataGross = GrossYear;
                reportData.FinancialDataChart = listchartData;
                return reportData;
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
        public ProjectPerformanceDataModel ProjectPerformanceReport(DashboardConfig config)
        {
            try
            {
                var reportData = new ProjectPerformanceDataModel();

                BudgetDeviationDataModel budgetDataModel = new BudgetDeviationDataModel();
                List<ProjectGross> projectModel = new List<ProjectGross>();
                List<DepartmentWiseDataModel> listchartData = new List<DepartmentWiseDataModel>();
                decimal GrossYear = 0;
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ProjectPerformanceReport";
                cmd.Parameters.AddWithValue("@P_Scope", config.Scope);
                cmd.Parameters.AddWithValue("@P_Year", config.Year);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeCode", config.ScenarioDataTypeId);
                cmd.Parameters.AddWithValue("@P_Project", config.ProjectId);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    budgetDataModel.ScenarioName = ds.Tables[0].Rows[0]["ScenarioName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["ScenarioName"].ToString();
                    budgetDataModel.Q1 = ds.Tables[0].Rows[0]["Q1"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q1"]);
                    budgetDataModel.Q2 = ds.Tables[0].Rows[0]["Q2"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q2"]);
                    budgetDataModel.Q3 = ds.Tables[0].Rows[0]["Q3"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q3"]);
                    budgetDataModel.Q4 = ds.Tables[0].Rows[0]["Q4"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q4"]);
                    budgetDataModel.Q5 = ds.Tables[0].Rows[0]["Q5"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q5"]);
                    budgetDataModel.Q6 = ds.Tables[0].Rows[0]["Q6"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q6"]);
                    budgetDataModel.Q7 = ds.Tables[0].Rows[0]["Q7"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q7"]);
                    budgetDataModel.Q8 = ds.Tables[0].Rows[0]["Q8"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q8"]);
                    budgetDataModel.Q9 = ds.Tables[0].Rows[0]["Q9"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q9"]);
                    budgetDataModel.Q10 = ds.Tables[0].Rows[0]["Q10"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q10"]);
                    budgetDataModel.Q11 = ds.Tables[0].Rows[0]["Q11"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q11"]);
                    budgetDataModel.Q12 = ds.Tables[0].Rows[0]["Q12"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["Q12"]);
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1].Rows.Count > 0))
                {
                    GrossYear = Convert.ToDecimal(ds.Tables[1].Rows[0][0].ToString());
                }
                //if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[2].Rows.Count > 0))
                //{
                //    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //    {
                //        ProjectGross client = new ProjectGross();
                //        client.ProjectName = ds.Tables[2].Rows[i]["ProjectName"].ToString();
                //        client.GrossYear = Convert.ToDecimal(ds.Tables[2].Rows[i]["GrossYear"]);
                //        projectModel.Add(client);
                //    }
                //}
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[2].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        DepartmentWiseDataModel chartData = new DepartmentWiseDataModel();
                        chartData.ScenarioName = ds.Tables[2].Rows[i]["ScenarioName"] == DBNull.Value ? "" : ds.Tables[2].Rows[i]["ScenarioName"].ToString();
                        chartData.DepartmentName = ds.Tables[2].Rows[i]["ProjectName"] == DBNull.Value ? "" : ds.Tables[2].Rows[i]["ProjectName"].ToString();
                        chartData.Q1 = ds.Tables[2].Rows[i]["Q1"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q1"]);
                        chartData.Q2 = ds.Tables[2].Rows[i]["Q2"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q2"]);
                        chartData.Q3 = ds.Tables[2].Rows[i]["Q3"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q3"]);
                        chartData.Q4 = ds.Tables[2].Rows[i]["Q4"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q4"]);
                        chartData.Q5 = ds.Tables[2].Rows[i]["Q5"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q5"]);
                        chartData.Q6 = ds.Tables[2].Rows[i]["Q6"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q6"]);
                        chartData.Q7 = ds.Tables[2].Rows[i]["Q7"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q7"]);
                        chartData.Q8 = ds.Tables[2].Rows[i]["Q8"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q8"]);
                        chartData.Q9 = ds.Tables[2].Rows[i]["Q9"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q9"]);
                        chartData.Q10 = ds.Tables[2].Rows[i]["Q10"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q10"]);
                        chartData.Q11 = ds.Tables[2].Rows[i]["Q11"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q11"]);
                        chartData.Q12 = ds.Tables[2].Rows[i]["Q12"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[2].Rows[i]["Q12"]);
                        listchartData.Add(chartData);
                    }
                }


                reportData.FinanceData = budgetDataModel;
                reportData.FinancialDataGross = GrossYear;
                reportData.ProjectData = listchartData;
                return reportData;
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
        public TrendReportData TrendAnalysisReport(DashboardConfig config)
        {
            try
            {
                var reportData = new TrendReportData();

                List <TrendDataModel> trendDataModel = new List<TrendDataModel>();
                List<ProjectGross> projectModel = new List<ProjectGross>();
                decimal GrossYear = 0;
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "TrendAnalysisReport";
                cmd.Parameters.AddWithValue("@P_Scope", config.Scope);
                cmd.Parameters.AddWithValue("@P_Year", config.Year);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeCode", config.ScenarioDataTypeId);
               


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        TrendDataModel client = new TrendDataModel();
                        client.YearlyAverage = Convert.ToDecimal(ds.Tables[0].Rows[i]["AverageGross"]);
                        client.ProjectName = (ds.Tables[0].Rows[i]["ProjectName"]).ToString();
                        client.Year = Convert.ToInt32(ds.Tables[0].Rows[i]["Year"]);
                        trendDataModel.Add(client);
                    }
                }
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[1].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ProjectGross client = new ProjectGross();
                        client.ProjectName = ds.Tables[1].Rows[i]["ProjectName"].ToString();
                        client.GrossYear = Convert.ToDecimal(ds.Tables[1].Rows[i]["GrossYear"]);
                        projectModel.Add(client);
                    }
                }

                reportData.TrendData = trendDataModel;
                reportData.TopProjects = projectModel;
                return reportData;
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
        public List<ProjectDataModel> YearOverYear(int id1,int id2,string code)
        {
            try
            {
                var reportData = new TrendReportData();

                List<ProjectDataModel> scenariodatalist = new List<ProjectDataModel>();
                DataSet ds = new DataSet();

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "YearOverYearReport";
                cmd.Parameters.AddWithValue("@P_ScenarioId1", id1);
                cmd.Parameters.AddWithValue("@P_ScenarioId2", id2);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeCode", code);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                base.DBConnection.Close();

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ProjectDataModel data = new ProjectDataModel();
                        data.Q1 = Convert.ToDecimal(ds.Tables[0].Rows[i]["Q1"]);
                        data.Q2 = Convert.ToDecimal(ds.Tables[0].Rows[i]["Q2"]);
                        data.Q3 = Convert.ToDecimal(ds.Tables[0].Rows[i]["Q3"]);
                        data.Q4 = Convert.ToDecimal(ds.Tables[0].Rows[i]["Q4"]);
                        data.Year = Convert.ToInt32(ds.Tables[0].Rows[i]["Year"]);
                        data.ProjectName = ds.Tables[0].Rows[i]["ScenarioName"].ToString();
                        scenariodatalist.Add(data);
                    }
                }
                
                return scenariodatalist;
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