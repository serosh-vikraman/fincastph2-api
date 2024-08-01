using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using TechnipFMC.Common;
using TechnipFMC.Finapp.Business;
using TechnipFMC.Finapp.Models;
using TechnipFMC.Finapp.Service.API.ViewModel;
using System.Data.SqlClient;
using TechnipFMC.Finapp.Business.Interfaces;
using System.IO;
using System.Web.Http;
using System.Dynamic;
using Newtonsoft.Json;

namespace TechnipFMC.Finapp.Service.API.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportBL _reportBL;

        public ReportController(IReportBL reportBL)
        {
            _reportBL = reportBL;
        }

        #region Old Code
        [HttpPost]
        [Route("api/getvarianceanalysisreportfile_Old/{cid}")]
        public IHttpActionResult GetVarianceAnalysisReportExcel(VarianceAnalysisConfigViewModel config,int cid)
        {
            VarianceAnalysisConfig varianceAnalysisConfig = new VarianceAnalysisConfig();
            Mapper.Map(config, varianceAnalysisConfig);
            byte[] byteinfo = _reportBL.GetVarianceAnalysisReportExcel(varianceAnalysisConfig,cid);
            var dataStream = new MemoryStream(byteinfo);
            return new eBookResult(dataStream, Request, $"VarianceAnalysisReport-{DateTime.Now.Date.ToString("dd-mm-yyyy")}.xlsx");
        }

        [HttpPost]
        [Route("api/getprojectlifecycleeeportexcel/{projectIds}/{scenarioScopeId}")]
        public IHttpActionResult GetProjectLifeCycleReportExcel(string projectIds, int scenarioScopeId)
        {
            byte[] byteinfo = _reportBL.GetProjectLifeCycleReportExcel(projectIds, scenarioScopeId);
            var dataStream = new MemoryStream(byteinfo);
            return new eBookResult(dataStream, Request, $"ProjectLifeCycle - {DateTime.Now.Date.ToString("dd-mm-yyyy")}.xlsx");
        }

        [HttpPost]
        [Route("api/getrepextractreportexcel/{year}/{reportTypeId}/{scenarioId}/{groupLevels}")]
        public IHttpActionResult GetREPExtractReportExcel(int year, int reportTypeId, int scenarioId, string groupLevels)
        {
            byte[] byteinfo = _reportBL.GetREPExtractReportDataExcel(year, reportTypeId, scenarioId, groupLevels);
            var dataStream = new MemoryStream(byteinfo);
            return new eBookResult(dataStream, Request, $"REPExtractReport - {DateTime.Now.Date.ToString("dd-mm-yyyy")}.xlsx");
        }

        #endregion


        #region Project Life Cycle Report

        [HttpPost]
        [Route("api/projectlifecyclereport/{projectid}/{scenarioscope}")]
        public HttpResponseMessage ProjectLifeCycleReport(int projectid, string scenarioscope)
        {
            try
            {

                var response = _reportBL.ProjectLifeCycleReport1(projectid, scenarioscope);
                //ProjectLifeCycleDataViewModel responseViewModel = new ProjectLifeCycleDataViewModel();
                //Mapper.Map(response, responseViewModel);

                dynamic dataHeader = new ExpandoObject();
                IDictionary<string, object> dictionaryHeader = (IDictionary<string, object>)dataHeader;
                foreach (var item in response.Header)
                {
                    dictionaryHeader.Add(item.FieldName, item.HeaderText);
                }
                string jsonHeader = JsonConvert.SerializeObject(dataHeader, Formatting.Indented);



                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                int i = 0;
                foreach (var item in response.Header)
                {
                    dictionary.Add(item.FieldName, response.RVDataValue[i].DataValue);
                    i++;
                }
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);

                dynamic data1 = new ExpandoObject();
                IDictionary<string, object> dictionary1 = (IDictionary<string, object>)data1;

                i = 0;
                foreach (var item in response.Header)
                {
                    dictionary1.Add(item.FieldName, response.GMDataValue[i].DataValue);
                    i++;
                }
                 
                string json1 = JsonConvert.SerializeObject(data1, Formatting.Indented);

                var jsonResponse = "[" + jsonHeader + "," + json + "," + json1 + "]";
                string finaldata = jsonResponse.Replace("\r\n", "");

                return Request.CreateResponse<APIResponse<string>>(HttpStatusCode.OK,
                    new APIResponse<string>(HttpStatusCode.OK, finaldata, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "ProjectLifeCycleReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ProjectLifeCycleDataViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }

        [HttpPost]
        [Route("api/projectlifecyclereportdownload/{projectid}/{scenarioscope}/{cid}")]
        // [Authorize]
        public HttpResponseMessage ProjectLifeCycleReportDownload(int projectid, string scenarioscope,int cid)
        {
            try
            {
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));

                var response = _reportBL.ProjectLifeCycleReport(projectid, scenarioscope);
                byte[] byteinfo = _reportBL.ProjectLifeCycleReportDownload(response, scenarioscope, cid);
                var fileName = $"ProjectLifeCycleReport_{response.ProjectName}.xlsx";
                var sourceFile = reportPath + fileName;
                File.WriteAllBytes(sourceFile, byteinfo.ToArray());

                string destFile = sharedReportPath + fileName;
                System.IO.File.Copy(sourceFile, destFile, true);


                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + fileName;
                return Request.CreateResponse<APIResponse<ReportPath>>(HttpStatusCode.OK,
                   new APIResponse<ReportPath>(HttpStatusCode.OK, obj, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "ProjectLifeCycleReportDownload", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ReportPath>(HttpStatusCode.InternalServerError, null, "Exception occured." + ex.ToString(), "", "", ""));



            }
        }

        [HttpPost]
        [Route("api/projectlifecyclereportdownload1/{projectid}/{scenarioscope}/{cid}")]
        // [Authorize]
        public HttpResponseMessage ProjectLifeCycleReportDownload1(int projectid, string scenarioscope,int cid)
        {
            try
            {
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));

                var response = _reportBL.ProjectLifeCycleReport(projectid, scenarioscope);
                byte[] byteinfo = _reportBL.ProjectLifeCycleReportDownload1(response, scenarioscope,cid);
                var fileName = $"ProjectLifeCycleReport_{response.ProjectName}.xlsx";
                var sourceFile = reportPath + fileName;
                File.WriteAllBytes(sourceFile, byteinfo.ToArray());

                string destFile = sharedReportPath + fileName;
                System.IO.File.Copy(sourceFile, destFile, true);


                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + fileName;
                return Request.CreateResponse<APIResponse<ReportPath>>(HttpStatusCode.OK,
                   new APIResponse<ReportPath>(HttpStatusCode.OK, obj, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "ProjectLifeCycleReportDownload", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ReportPath>(HttpStatusCode.InternalServerError, null, "Exception occured." + ex.ToString(), "", "", ""));



            }
        }
        #endregion


        #region REP Extract
        [HttpPost]
        [Route("api/repextractreport/{year}/{scenarioTypeCode}/{isCurrencyConversionRequired}")]
        public HttpResponseMessage REPExtractReport(int year, string scenarioTypeCode, string isCurrencyConversionRequired)
        {
            try
            {
                var response = _reportBL.REPExtractReport(year, scenarioTypeCode, isCurrencyConversionRequired);
                ExtractResponseViewModel responseViewModel = new ExtractResponseViewModel();
                Mapper.Map(response, responseViewModel);
                return Request.CreateResponse<APIResponse<ExtractResponseViewModel>>(HttpStatusCode.OK,
                    new APIResponse<ExtractResponseViewModel>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "REPExtractReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ExtractResponseViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/repextractreportdownload/{year}/{scenarioTypeCode}/{isCurrencyConversionRequired}")]
        // [Authorize]
        public HttpResponseMessage REPExtractReportDownload(int year, string scenarioTypeCode, string isCurrencyConversionRequired)
        {
            try
            {
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();
                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));

                var response = _reportBL.REPExtractReport(year, scenarioTypeCode, isCurrencyConversionRequired);
                byte[] byteinfo = _reportBL.REPExtractReportDownload(response, year, scenarioTypeCode, isCurrencyConversionRequired);
                var fileName = $"REPExtractReport_{year}_{scenarioTypeCode}.xlsx";
                var sourceFile = reportPath + fileName;
                File.WriteAllBytes(sourceFile, byteinfo.ToArray());

                string destFile = sharedReportPath + fileName;
                System.IO.File.Copy(sourceFile, destFile, true);

                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + fileName;
                return Request.CreateResponse<APIResponse<ReportPath>>(HttpStatusCode.OK,
                   new APIResponse<ReportPath>(HttpStatusCode.OK, obj, null, "", "", ""));


            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "REPExtractReportDownload", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ReportPath>(HttpStatusCode.InternalServerError, null, "Exception occured." + ex.ToString(), "", "", ""));



            }
        }
        #endregion

        #region Variance Analysis
        [HttpPost]
        [Route("api/getvarianceanalysisreportfile")]
        public HttpResponseMessage GetVarianceAnalysisReport(VarianceAnalysisConfigViewModel config)
        {
            try
            {
                config.SubTotalRequired = "N";
                VarianceAnalysisConfig varianceAnalysisConfig = new VarianceAnalysisConfig();
                Mapper.Map(config, varianceAnalysisConfig);
                var response = _reportBL.GetVarianceAnalysisReport(varianceAnalysisConfig);

                VarianceAnalysisResponseGridModel responseViewModel = new VarianceAnalysisResponseGridModel();
                Mapper.Map(response, responseViewModel);


                return Request.CreateResponse<APIResponse<VarianceAnalysisResponseGridModel>>(HttpStatusCode.OK,
                    new APIResponse<VarianceAnalysisResponseGridModel>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetVarianceAnalysisReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<VarianceAnalysisResponseGridModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/getvarianceanalysisreportdownload/{cid}")]
        // [Authorize]
        public HttpResponseMessage GetVarianceAnalysisReportDownload(VarianceAnalysisConfigViewModel config,int cid)
        {
            try
            {
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));

                config.SubTotalRequired = "Y";
                VarianceAnalysisConfig varianceAnalysisConfig = new VarianceAnalysisConfig();
                Mapper.Map(config, varianceAnalysisConfig);
                varianceAnalysisConfig.ScenarioDataTypeId = "RV";
                var response = _reportBL.GetVarianceAnalysisReport(varianceAnalysisConfig);
                varianceAnalysisConfig.ScenarioDataTypeId = "GM";
                var responseGM = _reportBL.GetVarianceAnalysisReport(varianceAnalysisConfig);
                byte[] byteinfo = _reportBL.GetVarianceAnalysisExcel(varianceAnalysisConfig, response, responseGM, cid);
                var fileName = $"VarianceAnalysisReport_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
                var sourceFile = reportPath + fileName;
                File.WriteAllBytes(sourceFile, byteinfo.ToArray());

                string destFile = sharedReportPath + fileName;
                System.IO.File.Copy(sourceFile, destFile, true);


                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + fileName;
                return Request.CreateResponse<APIResponse<ReportPath>>(HttpStatusCode.OK,
                   new APIResponse<ReportPath>(HttpStatusCode.OK, obj, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetVarianceAnalysisReportDownload", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ReportPath>(HttpStatusCode.InternalServerError, null, "Exception occured." + ex.ToString(), "", "", ""));



            }
        }

        #endregion

        #region Deviance Report
        [HttpPost]
        [Route("api/getdeviance")]
        public HttpResponseMessage GetDevianceReport(DevianceReportConfigViewModel config)
        {
            try
            {
                //config.SubTotalRequired = "N";
                DevianceReportConfig devianceConfig = new DevianceReportConfig();
                Mapper.Map(config, devianceConfig);
                var response = _reportBL.GetDevianceReport(devianceConfig);

                List<DevianceGridResponseViewModel> responseViewModel = new List<DevianceGridResponseViewModel>();
                Mapper.Map(response, responseViewModel);


                return Request.CreateResponse<APIResponse<List<DevianceGridResponseViewModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<DevianceGridResponseViewModel>>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetVarianceAnalysisReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<VarianceAnalysisResponseGridModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/getdeviancereportdownload/{cid}")]
        // [Authorize]
        public HttpResponseMessage GetDevianceReportDownload(DevianceReportConfigViewModel config,int cid)
        {
            try
            {
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));

                //config.SubTotalRequired = "Y";
                DevianceReportConfig varianceAnalysisConfig = new DevianceReportConfig();
                Mapper.Map(config, varianceAnalysisConfig);
                List<FinancialDataType> financialDataTypes = new List<FinancialDataType>();
                financialDataTypes = _reportBL.GetAllFinancialDataTypesOfScenario(varianceAnalysisConfig.OrgScenarioId);
                List<DevianceResponseModel> data = new List<DevianceResponseModel>();
                foreach (FinancialDataType item in financialDataTypes)
                {
                    varianceAnalysisConfig.ScenarioDataTypeId = item.FinancialDataTypeCode;
                    var response = _reportBL.GetDevianceReport(varianceAnalysisConfig);
                    data.Add(new DevianceResponseModel
                    {
                        FinancialDataType = item.FinancialDataTypeName,
                        GridResponse = response
                    });
                }                
                byte[] byteinfo = _reportBL.GetDevianceReportExcel(varianceAnalysisConfig, data,cid);
                var fileName = $"DevianceReport_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
                var sourceFile = reportPath + fileName;
                File.WriteAllBytes(sourceFile, byteinfo.ToArray());

                string destFile = sharedReportPath + fileName;
                System.IO.File.Copy(sourceFile, destFile, true);


                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + fileName;
                return Request.CreateResponse<APIResponse<ReportPath>>(HttpStatusCode.OK,
                   new APIResponse<ReportPath>(HttpStatusCode.OK, obj, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetDevianceReportDownload", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ReportPath>(HttpStatusCode.InternalServerError, null, "Exception occured." + ex.ToString(), "", "", ""));



            }
        }
        [HttpPost]
        [Route("api/getfinancereportdownload/{cid}")]
        // [Authorize]
        public HttpResponseMessage GetFinanceReportDownload(DashboardConfigViewModel financeconfigviewmodel,int cid)
        {
            try
            {
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));

                //config.SubTotalRequired = "Y";
                DashboardConfig financeConfig = new DashboardConfig();
                Mapper.Map(financeconfigviewmodel, financeConfig);
               // List<FinancialDataType> financialDataTypes = new List<FinancialDataType>();
                //financialDataTypes = _reportBL.GetAllFinancialDataTypesOfScenario(financeConfig.OrgScenarioId);
                FinancePerformanceDataModel data = new FinancePerformanceDataModel();
                //foreach (FinancialDataType item in financialDataTypes)
                //{
                //    varianceAnalysisConfig.ScenarioDataTypeId = item.FinancialDataTypeCode;
                    data = _reportBL.FinancePerformanceReport(financeConfig);
                //    data.Add(new DevianceResponseModel
                //    {
                //        FinancialDataType = item.FinancialDataTypeName,
                //        GridResponse = response
                //    });
                //}
                byte[] byteinfo = _reportBL.GetFinanceReportExcel(financeConfig, data,cid);
                var fileName = $"FinanceReport_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
                var sourceFile = reportPath + fileName;
                File.WriteAllBytes(sourceFile, byteinfo.ToArray());

                string destFile = sharedReportPath + fileName;
                System.IO.File.Copy(sourceFile, destFile, true);


                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + fileName;
                return Request.CreateResponse<APIResponse<ReportPath>>(HttpStatusCode.OK,
                   new APIResponse<ReportPath>(HttpStatusCode.OK, obj, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetDevianceReportDownload", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ReportPath>(HttpStatusCode.InternalServerError, null, "Exception occured." + ex.ToString(), "", "", ""));



            }
        }
        [HttpPost]
        [Route("api/getprojectperformancereportdownload/{cid}")]
        // [Authorize]
        public HttpResponseMessage GetProjectPerformanceReportDownload(DashboardConfigViewModel financeconfigviewmodel,int cid)
        {
            try
            {
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));

                //config.SubTotalRequired = "Y";
                DashboardConfig financeConfig = new DashboardConfig();
                Mapper.Map(financeconfigviewmodel, financeConfig);
                // List<FinancialDataType> financialDataTypes = new List<FinancialDataType>();
                //financialDataTypes = _reportBL.GetAllFinancialDataTypesOfScenario(financeConfig.OrgScenarioId);
                ProjectPerformanceDataModel data = new ProjectPerformanceDataModel();
                //foreach (FinancialDataType item in financialDataTypes)
                //{
                //    varianceAnalysisConfig.ScenarioDataTypeId = item.FinancialDataTypeCode;
                data = _reportBL.ProjectPerformanceReport(financeConfig);
                //    data.Add(new DevianceResponseModel
                //    {
                //        FinancialDataType = item.FinancialDataTypeName,
                //        GridResponse = response
                //    });
                //}
                byte[] byteinfo = _reportBL.GetProjectPerformanceReportExcel(financeConfig, data,cid);
                var fileName = $"ProjectPerformanceReport_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
                var sourceFile = reportPath + fileName;
                File.WriteAllBytes(sourceFile, byteinfo.ToArray());

                string destFile = sharedReportPath + fileName;
                System.IO.File.Copy(sourceFile, destFile, true);


                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + fileName;
                return Request.CreateResponse<APIResponse<ReportPath>>(HttpStatusCode.OK,
                   new APIResponse<ReportPath>(HttpStatusCode.OK, obj, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetDevianceReportDownload", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<ReportPath>(HttpStatusCode.InternalServerError, null, "Exception occured." + ex.ToString(), "", "", ""));



            }
        }

        #endregion
        [HttpPost]
        [Route("api/getDashboardData")]
        public HttpResponseMessage GetDashboardData(DashboardConfigViewModel config)
        {
            try
            {
                DashboardConfig devianceConfig = new DashboardConfig();
                Mapper.Map(config, devianceConfig);
                var response = _reportBL.GetDashboardData(devianceConfig);

                DashboardDataViewModel responseViewModel = new DashboardDataViewModel();
                Mapper.Map(response, responseViewModel);


                return Request.CreateResponse<APIResponse<DashboardDataViewModel>>(HttpStatusCode.OK,
                    new APIResponse<DashboardDataViewModel>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetDashboardData", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<DeviationAnalysisYearWiseViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/getFinancePerformanceReport")]
        public HttpResponseMessage GetFinancePerformanceReport(DashboardConfigViewModel config)
        {
            try
            {
                DashboardConfig devianceConfig = new DashboardConfig();
                Mapper.Map(config, devianceConfig);
                var response = _reportBL.FinancePerformanceReport(devianceConfig);

                FinancePerformanceDataViewModel responseViewModel = new FinancePerformanceDataViewModel();
                Mapper.Map(response, responseViewModel);


                return Request.CreateResponse<APIResponse<FinancePerformanceDataViewModel>>(HttpStatusCode.OK,
                    new APIResponse<FinancePerformanceDataViewModel>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetFinancePerformanceReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<DeviationAnalysisYearWiseViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/getProjectePerformanceReport")]
        public HttpResponseMessage GetProjectPerformanceReport(DashboardConfigViewModel config)
        {
            try
            {
                DashboardConfig devianceConfig = new DashboardConfig();
                Mapper.Map(config, devianceConfig);
                var response = _reportBL.ProjectPerformanceReport(devianceConfig);

                ProjectPerformanceDataViewModel responseViewModel = new ProjectPerformanceDataViewModel();
                Mapper.Map(response, responseViewModel);


                return Request.CreateResponse<APIResponse<ProjectPerformanceDataViewModel>>(HttpStatusCode.OK,
                    new APIResponse<ProjectPerformanceDataViewModel>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetFinancePerformanceReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<DeviationAnalysisYearWiseViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/getTrendAnalysisReport")]
        public HttpResponseMessage GetTrendAnalysisReport(DashboardConfigViewModel config)
        {
            try
            {
                DashboardConfig devianceConfig = new DashboardConfig();
                Mapper.Map(config, devianceConfig);
                var response = _reportBL.TrendAnalysisReport(devianceConfig);

                TrendReportDataViewModel responseViewModel = new TrendReportDataViewModel();
                Mapper.Map(response, responseViewModel);


                return Request.CreateResponse<APIResponse<TrendReportDataViewModel>>(HttpStatusCode.OK,
                    new APIResponse<TrendReportDataViewModel>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetFinancePerformanceReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<DeviationAnalysisYearWiseViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/getYearOverYearReport/{id1}/{id2}/{code}")]
        public HttpResponseMessage GetYearOverYearReport(int id1, int id2, string code)
        {
            try
            {
                var response = _reportBL.YearOverYear(id1, id2, code);

                return Request.CreateResponse<APIResponse<List<ProjectDataModel>>>(HttpStatusCode.OK,
                    new APIResponse<List<ProjectDataModel>>(HttpStatusCode.OK, response, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetFinancePerformanceReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<DeviationAnalysisYearWiseViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
        [HttpPost]
        [Route("api/getDeviationAnalysisReport")]
        public HttpResponseMessage GetDeviationAnalysisReport(DevianceReportConfigViewModel config)
        {
            try
            {
                DevianceReportConfig devianceConfig = new DevianceReportConfig();
                Mapper.Map(config, devianceConfig);
                var response = _reportBL.GetDeviationAnalysisReport(devianceConfig);

                DeviationAnalysisYearWiseViewModel responseViewModel = new DeviationAnalysisYearWiseViewModel();
                Mapper.Map(response, responseViewModel);


                return Request.CreateResponse<APIResponse<DeviationAnalysisYearWiseViewModel>>(HttpStatusCode.OK,
                    new APIResponse<DeviationAnalysisYearWiseViewModel>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetDeviationAnalysisReport", "");

                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<DeviationAnalysisYearWiseViewModel>(HttpStatusCode.InternalServerError, null, "Exception occured.", "", "", ""));

            }
        }
    }
    public class ReportPath
    {
        public string FilePath { get; set; }
    }
    public class eBookResult : IHttpActionResult
    {
        MemoryStream bookStuff;
        string PdfFileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public eBookResult(MemoryStream data, HttpRequestMessage request, string filename)
        {
            bookStuff = data;
            httpRequestMessage = request;
            PdfFileName = filename;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(bookStuff);
            //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }



    }
}