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
using Swashbuckle.Swagger;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Service.API.Helpers;

namespace TechnipFMC.Finapp.Service.API.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportBL _reportBL;
        private readonly IFinancialDataTypeBL _financialdatatypeBL;
        private readonly GeminiAIHelper _geminiAIHelper;

        public ReportController(IReportBL reportBL, IFinancialDataTypeBL financialDataTypeBL)
        {
            _reportBL = reportBL;
            _financialdatatypeBL = financialDataTypeBL;
            _geminiAIHelper = new GeminiAIHelper();
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
                var financialdatatypes = _financialdatatypeBL.GetAllFinancialDataTypesofReport(1);
                List<VarianceAnalysisResponseModel> responses = new List<VarianceAnalysisResponseModel> ();
                foreach (FinancialDataType financialdatatype in financialdatatypes) {
                    varianceAnalysisConfig.ScenarioDataTypeId = financialdatatype.FinancialDataTypeCode;
                    var response = new VarianceAnalysisResponseModel ();
                    response = _reportBL.GetVarianceAnalysisReport(varianceAnalysisConfig);
                    response.FinancialDataTypeName = financialdatatype.FinancialDataTypeName;
                    responses.Add(response);
                }
                //varianceAnalysisConfig.ScenarioDataTypeId = "RV";
                //var response = _reportBL.GetVarianceAnalysisReport(varianceAnalysisConfig);
                //varianceAnalysisConfig.ScenarioDataTypeId = "GM";
                //var responseGM = _reportBL.GetVarianceAnalysisReport(varianceAnalysisConfig);
                //byte[] byteinfo = _reportBL.GetVarianceAnalysisExcel(varianceAnalysisConfig, response, responseGM, cid);
                byte[] byteinfo = _reportBL.GetVarianceAnalysisExcel2(varianceAnalysisConfig, responses, cid);
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
                string testrrr = Path.GetDirectoryName(reportPath);
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
        [HttpPost]
        [Route("api/getdepartmentprojectreportdownload/{cid}")]
        // [Authorize]
        public HttpResponseMessage GetDepartmentProjectReportDownload(DevianceReportConfigViewModel financeconfigviewmodel, int cid)
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
                DevianceReportConfig financeConfig = new DevianceReportConfig();
                Mapper.Map(financeconfigviewmodel, financeConfig);
                // List<FinancialDataType> financialDataTypes = new List<FinancialDataType>();
                //financialDataTypes = _reportBL.GetAllFinancialDataTypesOfScenario(financeConfig.OrgScenarioId);
                List<ProjectDataModel> data = new List<ProjectDataModel>();
                //foreach (FinancialDataType item in financialDataTypes)
                //{
                //    varianceAnalysisConfig.ScenarioDataTypeId = item.FinancialDataTypeCode;
                data = _reportBL.DepartmentProjectReport(financeConfig);
                //    data.Add(new DevianceResponseModel
                //    {
                //        FinancialDataType = item.FinancialDataTypeName,
                //        GridResponse = response
                //    });
                //}
                byte[] byteinfo = _reportBL.GetDepartmentProjectReportExcel(financeConfig, data, cid);
                var fileName = $"DepartmentProjectReport_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
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
        [Route("api/getdepartmentreportdownload/{cid}")]
        // [Authorize]
        public HttpResponseMessage GetDepartmentReportDownload(DevianceReportConfigViewModel financeconfigviewmodel, int cid)
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
                DevianceReportConfig financeConfig = new DevianceReportConfig();
                Mapper.Map(financeconfigviewmodel, financeConfig);
                // List<FinancialDataType> financialDataTypes = new List<FinancialDataType>();
                //financialDataTypes = _reportBL.GetAllFinancialDataTypesOfScenario(financeConfig.OrgScenarioId);
                List<BudgetDeviationDataModel> data = new List<BudgetDeviationDataModel>();
                //foreach (FinancialDataType item in financialDataTypes)
                //{
                //    varianceAnalysisConfig.ScenarioDataTypeId = item.FinancialDataTypeCode;
                data = _reportBL.DepartmentReport(financeConfig);
                //    data.Add(new DevianceResponseModel
                //    {
                //        FinancialDataType = item.FinancialDataTypeName,
                //        GridResponse = response
                //    });
                //}
                byte[] byteinfo = _reportBL.GetDepartmentReportExcel(financeConfig, data, cid);
                var fileName = $"DepartmentReport_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
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
        [Route("api/downloaddashboardreport/{cid}")]
        // [Authorize]
        public HttpResponseMessage Downloaddashboardreport(DashboardConfigViewModel configView, int cid)
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
                DashboardConfig config = new DashboardConfig();
                Mapper.Map(configView, config);
                var response = _reportBL.GetDashboardData(config);
                byte[] byteinfo = _reportBL.Downloaddashboardreport(config, response, cid);
                var fileName = $"DashboardReport_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
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
        [Route("api/getDashboardGridData")]
        public HttpResponseMessage GetDashboardGridData(DashboardConfigViewModel config)
        {
            try
            {
                DashboardConfig devianceConfig = new DashboardConfig();
                Mapper.Map(config, devianceConfig);
                var response = _reportBL.GetDashboardGridData(devianceConfig);

                DashboardDataViewModel responseViewModel = new DashboardDataViewModel();
                Mapper.Map(response, responseViewModel);


                return Request.CreateResponse<APIResponse<DashboardDataViewModel>>(HttpStatusCode.OK,
                    new APIResponse<DashboardDataViewModel>(HttpStatusCode.OK, responseViewModel, null, "", "", ""));

            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetDashboardGridData", "");

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
        [HttpPost]
        [Route("api/getAIAssistedReport")]
        public async Task<HttpResponseMessage> GetAIAssistedReport(DashboardConfigViewModel config)
        {
            try
            {
                int year = config.Year;
                
                // Step 1: Generate the file and get the file path
                string filePath = _reportBL.GetAIAssistedReportData(year);

                // Step 2: Generate AI report using Gemini AI
                string aiReport;
                bool isAIGenerated = false;
                try
                {
                    aiReport = await _geminiAIHelper.GenerateReportFromFileAsync(filePath, _geminiAIHelper.GetEnhancedHTMLPrompt());
                    isAIGenerated = true;
                }
                catch (Exception aiEx)
                {
                    // Fallback: Generate basic report if AI is unavailable
                    RaintelsLogManager.Error(aiEx,"TechnipFMC.Finapp.Service.API.ReportController", "GetAIAssistedReport", 
                        $"AI service unavailable, generating fallback report. Error: {aiEx.Message}");
                    
                    aiReport = GenerateFallbackReport(filePath, year);
                }

                // Step 3: Ensure the report has working charts
                string finalReport = EnsureWorkingCharts(aiReport, filePath, year, isAIGenerated);

                // Step 4: Save the AI report following the same pattern as other download methods
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                // Clean up old AI report files (older than 1 day)
                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));
                string aiReportFileName = $"AIAssistedReport_{year}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.html";
                var sourceFile = reportPath + aiReportFileName;
                
                // Write AI report to temporary file first
                File.WriteAllText(sourceFile, finalReport);

                // Copy to shared folder
                string destFile = sharedReportPath + aiReportFileName;
                System.IO.File.Copy(sourceFile, destFile, true);

                // Step 5: Return response following the same pattern as other methods
                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + aiReportFileName;

                var response = new
                {
                    DataFilePath = filePath,
                    AIReportFilePath = obj.FilePath,
                    AIReportContent = finalReport,
                    ReportFormat = "HTML",
                    IsAIGenerated = isAIGenerated
                };

                return Request.CreateResponse<APIResponse<object>>(HttpStatusCode.OK,
                   new APIResponse<object>(HttpStatusCode.OK, response, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetAIAssistedReport", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<object>(HttpStatusCode.InternalServerError, null, "Exception occurred: " + ex.ToString(), "", "", ""));
            }
        }

        private string EnsureWorkingCharts(string aiReport, string fileName, int year, bool isAIGenerated)
        {
            try
            {
                // Read the financial data - handle both relative and absolute paths
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string tempReportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();
                
                // Fix: Construct the correct path to the financial data file
                // fileName is the relative path like "Reports/AIAssistedReportData_2024_18062025090352.json"
                string fullPath;
                if (fileName.StartsWith("Reports/"))
                {
                    // If it's a relative path starting with Reports/, use the shared path
                    fullPath = Path.Combine(sharedReportPath, fileName);
                }
                else
                {
                    // Otherwise, assume it's just a filename and use temp path
                    fullPath = Path.Combine(tempReportPath, fileName);
                }
                
                // Log the file path being used
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                    $"Reading data from: {fullPath}");
                                
                string fileContent = File.ReadAllText(fullPath);
                var financialData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(fileContent);
                
                // Log the data loading
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                    $"Loaded {financialData.Count} financial records");
                
                // Generate robust chart script with real data
                string chartScript = GenerateRobustChartScript(financialData);
                
                // Log the chart script generation
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                    $"Generated chart script length: {chartScript.Length}");
                
                // Always replace any existing chart script with our working one
                // This ensures charts display with real data instead of zeros
                
                // Log the original report length
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                    $"Original AI report length: {aiReport.Length}");
                
                // More robust script removal - find all script tags and remove them
                var scriptStartIndex = aiReport.IndexOf("<script>");
                int scriptsRemoved = 0;
                while (scriptStartIndex != -1)
                {
                    var scriptEndIndex = aiReport.IndexOf("</script>", scriptStartIndex);
                    if (scriptEndIndex != -1)
                    {
                        // Remove this script section
                        aiReport = aiReport.Remove(scriptStartIndex, scriptEndIndex - scriptStartIndex + 9);
                        scriptsRemoved++;
                        // Look for next script tag
                        scriptStartIndex = aiReport.IndexOf("<script>");
                    }
                    else
                    {
                        break; // No closing tag found
                    }
                }
                
                // Log script removal
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                    $"Removed {scriptsRemoved} script tags");
                
                // Ensure Chart.js library is included
                if (!aiReport.Contains("Chart.js"))
                {
                    aiReport = aiReport.Replace("</head>", 
                        "<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>\n</head>");
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                        "Added Chart.js library");
                }
                
                // Add our working chart script before closing body tag
                if (aiReport.Contains("</body>"))
                {
                    aiReport = aiReport.Replace("</body>", 
                        $"<script>\n{chartScript}\n</script>\n</body>");
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                        "Added chart script before </body>");
                }
                else
                {
                    aiReport += $"\n<script>\n{chartScript}\n</script>\n</body>\n</html>";
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                        "Added chart script and closing tags");
                }
                
                // Log final report length
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                    $"Final report length: {aiReport.Length}, Chart script replacement completed");
                
                return aiReport;
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "EnsureWorkingCharts", 
                    $"Error in EnsureWorkingCharts: {ex.Message}");
                // If all else fails, return the fallback report
                return GenerateFallbackReport(fileName, year);
            }
        }

        private string GenerateRobustChartScript(List<dynamic> financialData)
        {
            try
            {
                // Extract and process data for charts
                var revenueData = financialData.Where(d => d.ScenarioDataType?.ToString() == "Revenue").ToList();
                var grossMarginData = financialData.Where(d => d.ScenarioDataType?.ToString() == "Gross Margin").ToList();
                var manHoursData = financialData.Where(d => d.ScenarioDataType?.ToString() == "Man Hours").ToList();

                // Log data counts for debugging
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "GenerateRobustChartScript", 
                    $"Data counts - Total: {financialData.Count}, Revenue: {revenueData.Count}, Gross Margin: {grossMarginData.Count}, Man Hours: {manHoursData.Count}");

                // Generate quarterly labels
                var quarters = Enumerable.Range(1, 12).Select(q => $"Q{q}").ToArray();

                // Extract quarterly values for revenue - Fixed to use dynamic property access
                var revenueValues = new List<double>();
                for (int q = 1; q <= 12; q++)
                {
                    var quarterValue = revenueData.Sum(d => 
                    {
                        try
                        {
                            var propertyName = $"Q{q}New";
                            var value = ((IDictionary<string, object>)d)[propertyName];
                            return Convert.ToDouble(value ?? 0);
                        }
                        catch
                        {
                            return 0.0;
                        }
                    });
                    revenueValues.Add(quarterValue);
                }

                // Extract quarterly values for gross margin - Fixed to use dynamic property access
                var grossMarginValues = new List<double>();
                for (int q = 1; q <= 12; q++)
                {
                    var quarterValue = grossMarginData.Sum(d => 
                    {
                        try
                        {
                            var propertyName = $"Q{q}New";
                            var value = ((IDictionary<string, object>)d)[propertyName];
                            return Convert.ToDouble(value ?? 0);
                        }
                        catch
                        {
                            return 0.0;
                        }
                    });
                    grossMarginValues.Add(quarterValue);
                }

                // Extract quarterly values for man hours - Fixed to use dynamic property access
                var manHoursValues = new List<double>();
                for (int q = 1; q <= 12; q++)
                {
                    var quarterValue = manHoursData.Sum(d => 
                    {
                        try
                        {
                            var propertyName = $"Q{q}New";
                            var value = ((IDictionary<string, object>)d)[propertyName];
                            return Convert.ToDouble(value ?? 0);
                        }
                        catch
                        {
                            return 0.0;
                        }
                    });
                    manHoursValues.Add(quarterValue);
                }

                // Log extracted values for debugging
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "GenerateRobustChartScript", 
                    $"Revenue values: [{string.Join(", ", revenueValues)}]");
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "GenerateRobustChartScript", 
                    $"Gross Margin values: [{string.Join(", ", grossMarginValues)}]");
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.ReportController", "GenerateRobustChartScript", 
                    $"Man Hours values: [{string.Join(", ", manHoursValues)}]");

                // Get project names for distribution chart
                var projectNames = financialData
                    .Select(d => d.ProjectName?.ToString())
                    .Where(p => !string.IsNullOrEmpty(p))
                    .Distinct()
                    .Take(5)
                    .ToArray();

                // Calculate project revenues - Fixed to use dynamic property access
                var projectRevenues = projectNames.Select(project =>
                {
                    var projectData = revenueData.Where(d => d.ProjectName?.ToString() == project).ToList();
                    var totalRevenue = 0.0;
                    for (int q = 1; q <= 12; q++)
                    {
                        var quarterValue = projectData.Sum(d => 
                        {
                            try
                            {
                                var propertyName = $"Q{q}New";
                                var value = ((IDictionary<string, object>)d)[propertyName];
                                return Convert.ToDouble(value ?? 0);
                            }
                            catch
                            {
                                return 0.0;
                            }
                        });
                        totalRevenue += quarterValue;
                    }
                    return totalRevenue;
                }).ToArray();

                return $@"
        // Wait for DOM to be ready
        document.addEventListener('DOMContentLoaded', function() {{
            console.log('Initializing charts...');
            
            // Chart 1: Revenue and Gross Margin Trends
            const revenueCtx = document.getElementById('revenueGrossMarginChart');
            if (revenueCtx) {{
                new Chart(revenueCtx.getContext('2d'), {{
                    type: 'line',
                    data: {{
                        labels: {Newtonsoft.Json.JsonConvert.SerializeObject(quarters)},
                        datasets: [
                            {{
                                label: 'Revenue',
                                data: {Newtonsoft.Json.JsonConvert.SerializeObject(revenueValues)},
                                borderColor: 'rgb(75, 192, 192)',
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                tension: 0.1
                            }},
                            {{
                                label: 'Gross Margin',
                                data: {Newtonsoft.Json.JsonConvert.SerializeObject(grossMarginValues)},
                                borderColor: 'rgb(255, 99, 132)',
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                tension: 0.1
                            }}
                        ]
                    }},
                    options: {{
                        responsive: true,
                        plugins: {{
                            title: {{
                                display: true,
                                text: 'Revenue and Gross Margin Trends'
                            }}
                        }},
                        scales: {{
                            y: {{
                                beginAtZero: true
                            }}
                        }}
                    }}
                }});
                console.log('Revenue chart created');
            }}
            
            // Chart 2: Quarterly Performance
            const quarterlyCtx = document.getElementById('quarterlyPerformanceChart');
            if (quarterlyCtx) {{
                new Chart(quarterlyCtx.getContext('2d'), {{
                    type: 'bar',
                    data: {{
                        labels: {Newtonsoft.Json.JsonConvert.SerializeObject(quarters)},
                        datasets: [
                            {{
                                label: 'Revenue',
                                data: {Newtonsoft.Json.JsonConvert.SerializeObject(revenueValues)},
                                backgroundColor: 'rgba(54, 162, 235, 0.8)'
                            }},
                            {{
                                label: 'Man Hours',
                                data: {Newtonsoft.Json.JsonConvert.SerializeObject(manHoursValues)},
                                backgroundColor: 'rgba(255, 205, 86, 0.8)'
                            }}
                        ]
                    }},
                    options: {{
                        responsive: true,
                        plugins: {{
                            title: {{
                                display: true,
                                text: 'Quarterly Performance'
                            }}
                        }},
                        scales: {{
                            y: {{
                                beginAtZero: true
                            }}
                        }}
                    }}
                }});
                console.log('Quarterly chart created');
            }}
            
            // Chart 3: Revenue Distribution
            const distributionCtx = document.getElementById('revenueDistributionChart');
            if (distributionCtx) {{
                new Chart(distributionCtx.getContext('2d'), {{
                    type: 'pie',
                    data: {{
                        labels: {Newtonsoft.Json.JsonConvert.SerializeObject(projectNames)},
                        datasets: [{{
                            data: {Newtonsoft.Json.JsonConvert.SerializeObject(projectRevenues)},
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.8)',
                                'rgba(54, 162, 235, 0.8)',
                                'rgba(255, 205, 86, 0.8)',
                                'rgba(75, 192, 192, 0.8)',
                                'rgba(153, 102, 255, 0.8)'
                            ]
                        }}]
                    }},
                    options: {{
                        responsive: true,
                        plugins: {{
                            title: {{
                                display: true,
                                text: 'Revenue Distribution by Project'
                            }}
                        }}
                    }}
                }});
                console.log('Distribution chart created');
            }}
            
            // Chart 4: Efficiency Metrics
            const efficiencyCtx = document.getElementById('efficiencyMetricsChart');
            if (efficiencyCtx) {{
                const efficiencyData = {GenerateEfficiencyData(financialData)};
                new Chart(efficiencyCtx.getContext('2d'), {{
                    type: 'scatter',
                    data: {{
                        datasets: [{{
                            label: 'Revenue vs Man Hours',
                            data: efficiencyData,
                            backgroundColor: 'rgba(255, 99, 132, 0.8)'
                        }}]
                    }},
                    options: {{
                        responsive: true,
                        plugins: {{
                            title: {{
                                display: true,
                                text: 'Efficiency Analysis'
                            }}
                        }},
                        scales: {{
                            x: {{
                                title: {{
                                    display: true,
                                    text: 'Man Hours'
                                }}
                            }},
                            y: {{
                                title: {{
                                    display: true,
                                    text: 'Revenue'
                                }}
                            }}
                        }}
                    }}
                }});
                console.log('Efficiency chart created');
            }}
            
            console.log('All charts initialized');
        }});
";
            }
            catch (Exception ex)
            {
                return $"console.error('Error generating charts: {ex.Message}');";
            }
        }

        private string GenerateFallbackReport(string fileName, int year)
        {
            try
            {
                // Read the data file - handle both relative and absolute paths
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string tempReportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();
                
                string fullPath = Path.Combine(tempReportPath, fileName);
                
                string fileContent = File.ReadAllText(fullPath);
                
                // Parse the JSON data for chart generation
                var financialData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(fileContent);
                
                // Generate chart data
                string chartScript = GenerateChartScript(financialData);
                
                // Generate a basic HTML report with charts
                return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Financial Report - {year}</title>
    <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 40px; line-height: 1.6; background-color: #f5f5f5; }}
        .container {{ max-width: 1200px; margin: 0 auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 20px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; border-radius: 10px; margin-bottom: 30px; text-align: center; }}
        .section {{ margin-bottom: 40px; padding: 25px; border: 1px solid #e0e0e0; border-radius: 8px; background: #fafafa; }}
        .section h2 {{ color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px; margin-bottom: 20px; }}
        .warning {{ background: linear-gradient(135deg, #ffecd2 0%, #fcb69f 100%); border: 1px solid #ffa726; padding: 20px; border-radius: 8px; margin-bottom: 30px; }}
        .data-summary {{ background: linear-gradient(135deg, #a8edea 0%, #fed6e3 100%); padding: 20px; border-radius: 8px; }}
        .chart-container {{ background: white; padding: 20px; border-radius: 8px; margin: 20px 0; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
        .chart-container h3 {{ color: #34495e; margin-bottom: 15px; }}
        canvas {{ max-height: 400px; }}
        .metrics-grid {{ display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; margin: 20px 0; }}
        .metric-card {{ background: white; padding: 20px; border-radius: 8px; text-align: center; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }}
        .metric-value {{ font-size: 2em; font-weight: bold; color: #3498db; }}
        .metric-label {{ color: #7f8c8d; margin-top: 5px; }}
        @media (max-width: 768px) {{ .container {{ margin: 10px; padding: 15px; }} }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📊 Financial Performance Report - {year}</h1>
            <p><strong>Generated:</strong> {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}</p>
            <p><strong>Data Source:</strong> {fileName}</p>
        </div>
        
        <div class='warning'>
            <h3>⚠️ AI Service Notice</h3>
            <p>This is a fallback report generated when the AI service was temporarily unavailable. 
            The charts below are generated from your actual financial data and are fully interactive.</p>
        </div>
        
        <div class='section'>
            <h2>📈 Executive Summary</h2>
            <p>This comprehensive financial report analyzes performance across multiple projects and scenarios for {year}. 
            The data shows revenue trends, gross margins, and resource utilization across different business units.</p>
            
            <div class='metrics-grid'>
                <div class='metric-card'>
                    <div class='metric-value'>{financialData.Count}</div>
                    <div class='metric-label'>Data Records</div>
                </div>
                <div class='metric-card'>
                    <div class='metric-value'>{year}</div>
                    <div class='metric-label'>Report Year</div>
                </div>
                <div class='metric-card'>
                    <div class='metric-value'>{fileContent.Length:N0}</div>
                    <div class='metric-label'>Data Size (chars)</div>
                </div>
            </div>
        </div>
        
        <div class='section'>
            <h2>📊 Interactive Financial Charts</h2>
            <p>Explore your financial data through these interactive visualizations. Hover over charts for detailed information.</p>
            
            <div class='chart-container'>
                <h3>💰 Revenue and Gross Margin Trends</h3>
                <canvas id='revenueChart'></canvas>
            </div>
            
            <div class='chart-container'>
                <h3>📊 Quarterly Performance Comparison</h3>
                <canvas id='quarterlyChart'></canvas>
            </div>
            
            <div class='chart-container'>
                <h3>🥧 Revenue Distribution by Project</h3>
                <canvas id='distributionChart'></canvas>
            </div>
            
            <div class='chart-container'>
                <h3>📈 Efficiency Metrics Analysis</h3>
                <canvas id='efficiencyChart'></canvas>
            </div>
        </div>
        
        <div class='section'>
            <h2>🔍 Data Overview</h2>
            <div class='data-summary'>
                <p><strong>Projects Analyzed:</strong> {GetUniqueProjects(financialData)}</p>
                <p><strong>Scenarios Included:</strong> {GetUniqueScenarios(financialData)}</p>
                <p><strong>Data Types:</strong> Revenue, Gross Margin, Man Hours, Bank Interest, Overseas Revenue</p>
                <p><strong>Time Period:</strong> Q1-Q12 {year}</p>
            </div>
        </div>
        
        <div class='section'>
            <h2>📋 Key Insights</h2>
            <ul>
                <li><strong>Revenue Trends:</strong> Analysis shows varying performance across quarters</li>
                <li><strong>Project Performance:</strong> Different projects show distinct financial patterns</li>
                <li><strong>Resource Utilization:</strong> Man hours correlate with revenue generation</li>
                <li><strong>Margin Analysis:</strong> Gross margins vary by project and scenario</li>
            </ul>
        </div>
        
        <div class='section'>
            <h2>🚀 Next Steps</h2>
            <ol>
                <li>Try generating the AI-enhanced report again when the service is available</li>
                <li>Use the interactive charts above for detailed analysis</li>
                <li>Export chart data for further analysis in Excel</li>
                <li>Schedule regular financial reviews using this data</li>
            </ol>
        </div>
    </div>

    <script>
        // Financial data from your system
        const financialData = {fileContent};
        
        {chartScript}
    </script>
</body>
</html>";
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GenerateFallbackReport", "");
                return $"<html><body><h1>Error Generating Report</h1><p>Unable to generate report: {ex.Message}</p></body></html>";
            }
        }

        private string GetUniqueProjects(List<dynamic> data)
        {
            try
            {
                var projects = data.Select(d => d.ProjectName?.ToString()).Where(p => !string.IsNullOrEmpty(p)).Distinct();
                return string.Join(", ", projects);
            }
            catch
            {
                return "Multiple Projects";
            }
        }

        private string GetUniqueScenarios(List<dynamic> data)
        {
            try
            {
                var scenarios = data.Select(d => d.ScenarioTypeName?.ToString()).Where(s => !string.IsNullOrEmpty(s)).Distinct();
                return string.Join(", ", scenarios);
            }
            catch
            {
                return "Multiple Scenarios";
            }
        }

        private string GenerateChartScript(List<dynamic> financialData)
        {
            try
            {
                // Extract and process data for charts
                var revenueData = financialData.Where(d => d.ScenarioDataType?.ToString() == "Revenue").ToList();
                var grossMarginData = financialData.Where(d => d.ScenarioDataType?.ToString() == "Gross Margin").ToList();
                var manHoursData = financialData.Where(d => d.ScenarioDataType?.ToString() == "Man Hours").ToList();

                // Generate quarterly labels
                var quarters = Enumerable.Range(1, 12).Select(q => $"Q{q}").ToArray();

                // Extract quarterly values for revenue
                var revenueValues = new List<double>();
                for (int q = 1; q <= 12; q++)
                {
                    var quarterValue = revenueData
                        .Where(d => d.GetType().GetProperty($"Q{q}New") != null)
                        .Sum(d => Convert.ToDouble(d.GetType().GetProperty($"Q{q}New").GetValue(d) ?? 0));
                    revenueValues.Add(quarterValue);
                }

                // Extract quarterly values for gross margin
                var grossMarginValues = new List<double>();
                for (int q = 1; q <= 12; q++)
                {
                    var quarterValue = grossMarginData
                        .Where(d => d.GetType().GetProperty($"Q{q}New") != null)
                        .Sum(d => Convert.ToDouble(d.GetType().GetProperty($"Q{q}New").GetValue(d) ?? 0));
                    grossMarginValues.Add(quarterValue);
                }

                // Get project names for distribution chart
                var projectNames = financialData
                    .Select(d => d.ProjectName?.ToString())
                    .Where(p => !string.IsNullOrEmpty(p))
                    .Distinct()
                    .Take(5)
                    .ToArray();

                // Calculate project revenues
                var projectRevenues = projectNames.Select(project =>
                {
                    var projectData = revenueData.Where(d => d.ProjectName?.ToString() == project).ToList();
                    var totalRevenue = 0.0;
                    for (int q = 1; q <= 12; q++)
                    {
                        var quarterValue = projectData
                            .Where(d => d.GetType().GetProperty($"Q{q}New") != null)
                            .Sum(d => Convert.ToDouble(d.GetType().GetProperty($"Q{q}New").GetValue(d) ?? 0));
                        totalRevenue += quarterValue;
                    }
                    return totalRevenue;
                }).ToArray();

                return $@"
        // Chart 1: Revenue and Gross Margin Trends
        const revenueCtx = document.getElementById('revenueChart').getContext('2d');
        new Chart(revenueCtx, {{
            type: 'line',
            data: {{
                labels: {Newtonsoft.Json.JsonConvert.SerializeObject(quarters)},
                datasets: [
                    {{
                        label: 'Revenue',
                        data: {Newtonsoft.Json.JsonConvert.SerializeObject(revenueValues)},
                        borderColor: 'rgb(75, 192, 192)',
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        tension: 0.1
                    }},
                    {{
                        label: 'Gross Margin',
                        data: {Newtonsoft.Json.JsonConvert.SerializeObject(grossMarginValues)},
                        borderColor: 'rgb(255, 99, 132)',
                        backgroundColor: 'rgba(255, 99, 132, 0.2)',
                        tension: 0.1
                    }}
                ]
            }},
            options: {{
                responsive: true,
                plugins: {{
                    title: {{
                        display: true,
                        text: 'Revenue and Gross Margin Trends'
                    }}
                }},
                scales: {{
                    y: {{
                        beginAtZero: true
                    }}
                }}
            }}
        }});

        // Chart 2: Quarterly Performance
        const quarterlyCtx = document.getElementById('quarterlyChart').getContext('2d');
        new Chart(quarterlyCtx, {{
            type: 'bar',
            data: {{
                labels: {Newtonsoft.Json.JsonConvert.SerializeObject(quarters)},
                datasets: [
                    {{
                        label: 'Revenue',
                        data: {Newtonsoft.Json.JsonConvert.SerializeObject(revenueValues)},
                        backgroundColor: 'rgba(54, 162, 235, 0.8)'
                    }}
                ]
            }},
            options: {{
                responsive: true,
                plugins: {{
                    title: {{
                        display: true,
                        text: 'Quarterly Revenue Performance'
                    }}
                }},
                scales: {{
                    y: {{
                        beginAtZero: true
                    }}
                }}
            }}
        }});

        // Chart 3: Revenue Distribution
        const distributionCtx = document.getElementById('distributionChart').getContext('2d');
        new Chart(distributionCtx, {{
            type: 'pie',
            data: {{
                labels: {Newtonsoft.Json.JsonConvert.SerializeObject(projectNames)},
                datasets: [{{
                    data: {Newtonsoft.Json.JsonConvert.SerializeObject(projectRevenues)},
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.8)',
                        'rgba(54, 162, 235, 0.8)',
                        'rgba(255, 205, 86, 0.8)',
                        'rgba(75, 192, 192, 0.8)',
                        'rgba(153, 102, 255, 0.8)'
                    ]
                }}]
            }},
            options: {{
                responsive: true,
                plugins: {{
                    title: {{
                        display: true,
                        text: 'Revenue Distribution by Project'
                    }}
                }}
            }}
        }});

        // Chart 4: Efficiency Metrics
        const efficiencyCtx = document.getElementById('efficiencyChart').getContext('2d');
        new Chart(efficiencyCtx, {{
            type: 'scatter',
            data: {{
                datasets: [{{
                    label: 'Revenue vs Man Hours',
                    data: {GenerateEfficiencyData(financialData)},
                    backgroundColor: 'rgba(255, 99, 132, 0.8)'
                }}]
            }},
            options: {{
                responsive: true,
                plugins: {{
                    title: {{
                        display: true,
                        text: 'Efficiency Analysis: Revenue vs Man Hours'
                    }}
                }},
                scales: {{
                    x: {{
                        title: {{
                            display: true,
                            text: 'Man Hours'
                        }}
                    }},
                    y: {{
                        title: {{
                            display: true,
                            text: 'Revenue'
                        }}
                    }}
                }}
            }}
        }});
";
            }
            catch (Exception ex)
            {
                return "// Error generating charts: " + ex.Message;
            }
        }

        private string GenerateEfficiencyData(List<dynamic> financialData)
        {
            try
            {
                var efficiencyPoints = new List<object>();
                
                // Group data by project and scenario
                var groupedData = financialData.GroupBy(d => new { Project = d.ProjectName?.ToString(), Scenario = d.ScenarioTypeName?.ToString() });
                
                foreach (var group in groupedData.Take(10)) // Limit to 10 points for clarity
                {
                    var revenue = 0.0;
                    var manHours = 0.0;
                    
                    foreach (var item in group)
                    {
                        if (item.ScenarioDataType?.ToString() == "Revenue")
                        {
                            for (int q = 1; q <= 12; q++)
                            {
                                try
                                {
                                    var propertyName = $"Q{q}New";
                                    var value = ((IDictionary<string, object>)item)[propertyName];
                                    revenue += Convert.ToDouble(value ?? 0);
                                }
                                catch
                                {
                                    // Property not found, continue
                                }
                            }
                        }
                        else if (item.ScenarioDataType?.ToString() == "Man Hours")
                        {
                            for (int q = 1; q <= 12; q++)
                            {
                                try
                                {
                                    var propertyName = $"Q{q}New";
                                    var value = ((IDictionary<string, object>)item)[propertyName];
                                    manHours += Convert.ToDouble(value ?? 0);
                                }
                                catch
                                {
                                    // Property not found, continue
                                }
                            }
                        }
                    }
                    
                    if (revenue > 0 && manHours > 0)
                    {
                        efficiencyPoints.Add(new { x = manHours, y = revenue });
                    }
                }
                
                return Newtonsoft.Json.JsonConvert.SerializeObject(efficiencyPoints);
            }
            catch
            {
                return "[]";
            }
        }

        [HttpPost]
        [Route("api/getAIAssistedReportWithFormat")]
        public async Task<HttpResponseMessage> GetAIAssistedReportWithFormat(DashboardConfigViewModel config, string reportFormat = "HTML")
        {
            try
            {
                int year = config.Year;
                
                // Step 1: Generate the file and get the file path
                string filePath = _reportBL.GetAIAssistedReportData(year);

                // Step 2: Choose prompt based on format
                string prompt;
                string fileExtension;
                
                switch (reportFormat.ToUpper())
                {
                    case "WORD":
                    case "DOCX":
                        prompt = _geminiAIHelper.GetWordDocumentPrompt();
                        fileExtension = "md";
                        break;
                    case "TEXT":
                    case "TXT":
                        prompt = _geminiAIHelper.GetDefaultFinancialReportPrompt();
                        fileExtension = "txt";
                        break;
                    case "HTML":
                    default:
                        prompt = _geminiAIHelper.GetEnhancedHTMLPrompt();
                        fileExtension = "html";
                        break;
                }

                // Step 3: Generate AI report using Gemini AI
                string aiReport = await _geminiAIHelper.GenerateReportFromFileAsync(filePath, prompt);

                // Step 4: Save the AI report following the same pattern as other download methods
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                // Clean up old AI report files (older than 1 day)
                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));
                string aiReportFileName = $"AIAssistedReport_{year}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{fileExtension}";
                var sourceFile = reportPath + aiReportFileName;
                
                // Write AI report to temporary file first
                File.WriteAllText(sourceFile, aiReport);

                // Copy to shared folder
                string destFile = sharedReportPath + aiReportFileName;
                System.IO.File.Copy(sourceFile, destFile, true);

                // Step 5: Return response following the same pattern as other methods
                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + aiReportFileName;

                var response = new
                {
                    DataFilePath = filePath,
                    AIReportFilePath = obj.FilePath,
                    AIReportContent = aiReport,
                    ReportFormat = reportFormat.ToUpper()
                };

                return Request.CreateResponse<APIResponse<object>>(HttpStatusCode.OK,
                   new APIResponse<object>(HttpStatusCode.OK, response, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetAIAssistedReportWithFormat", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<object>(HttpStatusCode.InternalServerError, null, "Exception occurred: " + ex.ToString(), "", "", ""));
            }
        }

        [HttpPost]
        [Route("api/getAIAssistedReportPDF")]
        public async Task<HttpResponseMessage> GetAIAssistedReportPDF(DashboardConfigViewModel config)
        {
            try
            {
                int year = config.Year;
                
                // Step 1: Generate the file and get the file path
                string filePath = _reportBL.GetAIAssistedReportData(year);

                // Step 2: Generate AI report using Gemini AI
                string aiReport;
                bool isAIGenerated = false;
                try
                {
                    aiReport = await _geminiAIHelper.GenerateReportFromFileAsync(filePath, _geminiAIHelper.GetEnhancedHTMLPrompt());
                    isAIGenerated = true;
                }
                catch (Exception aiEx)
                {
                    // Fallback: Generate basic report if AI is unavailable
                    RaintelsLogManager.Error(aiEx,"TechnipFMC.Finapp.Service.API.ReportController", "GetAIAssistedReportPDF", 
                        $"AI service unavailable, generating fallback report. Error: {aiEx.Message}");
                    
                    aiReport = GenerateFallbackReport(filePath, year);
                }

                // Step 3: Ensure the report has working charts
                string finalReport = EnsureWorkingCharts(aiReport, filePath, year, isAIGenerated);

                // Step 4: Convert HTML to PDF
                byte[] pdfBytes = ConvertHtmlToPdf(finalReport);

                // Step 5: Save PDF following the same pattern as other download methods
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string reportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();

                // Clean up old AI report files (older than 1 day)
                Directory.GetFiles(reportPath)
                     .Select(f => new FileInfo(f))
                     .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                     .ToList()
                     .ForEach(f => f.Delete());

                string excelFolderName = Path.GetFileName(Path.GetDirectoryName(reportPath));
                string pdfFileName = $"AIAssistedReport_{year}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.pdf";
                var sourceFile = reportPath + pdfFileName;
                
                // Write PDF to temporary file first
                File.WriteAllBytes(sourceFile, pdfBytes);

                // Copy to shared folder
                string destFile = sharedReportPath + pdfFileName;
                System.IO.File.Copy(sourceFile, destFile, true);

                // Step 6: Return response following the same pattern as other methods
                ReportPath obj = new ReportPath();
                obj.FilePath = excelFolderName + "/" + pdfFileName;

                var response = new
                {
                    DataFilePath = filePath,
                    AIReportFilePath = obj.FilePath,
                    ReportFormat = "PDF",
                    IsAIGenerated = isAIGenerated
                };

                return Request.CreateResponse<APIResponse<object>>(HttpStatusCode.OK,
                   new APIResponse<object>(HttpStatusCode.OK, response, null, "", "", ""));
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "GetAIAssistedReportPDF", "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new APIResponse<object>(HttpStatusCode.InternalServerError, null, "Exception occurred: " + ex.ToString(), "", "", ""));
            }
        }

        private byte[] ConvertHtmlToPdf(string htmlContent)
        {
            try
            {
                // Note: You'll need to add a PDF conversion library to your project
                // Options include: iTextSharp, DinkToPdf, PuppeteerSharp, etc.
                
                // Example using a hypothetical PDF converter:
                // var pdfConverter = new HtmlToPdfConverter();
                // return pdfConverter.ConvertHtmlToPdf(htmlContent);
                
                // For now, return a placeholder
                // You'll need to implement this based on your chosen PDF library
                throw new NotImplementedException("PDF conversion not yet implemented. Please add a PDF conversion library.");
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.ReportController", "ConvertHtmlToPdf", "");
                throw;
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