using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TechnipFMC.Finapp.Models;
using System.IO;
using OfficeOpenXml;

namespace TechnipFMC.Finapp.Business
{
    public class ScenarioFileBL : IScenarioFileBL
    {
        public ScenarioFileBL()
        {

        }

        public List<ScenarioUploadLog> GetScenarioErrorlog(string sessionid)
        {
            ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            var logs = _scenarioFileRepo.GetScenarioErrorlog(sessionid);
            var filteredLogs = new List<ScenarioUploadLog>();
            var projectCode = ""; var uploadStatus = false;
            foreach (var log in logs.OrderBy(l => l.ProjectCode))
            {
                if (log.UploadStatus == true)
                {
                    if (projectCode == log.ProjectCode && uploadStatus == log.UploadStatus)
                        continue;

                    projectCode = log.ProjectCode;
                    uploadStatus = log.UploadStatus;
                    filteredLogs.Add(log);
                }
                else
                {
                    filteredLogs.Add(log);
                }
            }
            return filteredLogs;
        }

        public List<ScenarioUploadLog> GetScenarioErrorlogByScenarioId(int ScenarioId)
        {
            ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            return _scenarioFileRepo.GetScenarioErrorlogByScenarioId(ScenarioId);
        }
        public bool CheckExelRow(ExcelWorksheet Sheet ,int Start,int End,int row,int type,bool firstTime)
        {
            bool returnvalue = false;
            if (type == 1)
            {


                for (int col = Start; col <= End; col++)
                { // ... Cell by cell...
                    string cellstring = "";
                    object cellValue = Sheet.Cells[row, col].Value; // This got me the actual value I needed.
                    if(cellValue!=null)
                    {
                        cellstring = cellValue.ToString();
                    }
                   
                    if (!string.IsNullOrEmpty(cellstring))
                    {
                        if (Convert.ToDecimal(cellstring) != 0)
                        {
                            returnvalue = true;
                        }
                    }
                }
            }
            else
            {
                returnvalue= true;
                //if(firstTime)
                //{
                //    while (!reachedEndOfFile)
                //    {
                //    }

                //    }


            }
            return returnvalue;
        }
       
        public ScenarioFileUploadResponse UploadProjectScenarioDataType_1_New(int scenarioId, string activeQuarters, string fileName, string filePath, string userName,int customerId, int departmentId, int clientId)
        {
            //ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            //ScenarioRepository _scenarioRepo = new ScenarioRepository();
            //LockYearRepository _lockYearRepository = new LockYearRepository();
            var errorLogs = new List<ScenarioUploadLog>();
            var nullProjectCodeEntriesCount = 0;
            var quarters = activeQuarters != null ? activeQuarters.Split(',').ToList() : new List<string>();
            var _uploadSessionId = Guid.NewGuid().ToString().ToUpper();

            List<ProjectScenario> scenarios = new List<ProjectScenario>();
            try
            {
                var configurations = new ScenarioFileRepository().GetTemplateConfigurations(1);
                var scenario = new ScenarioRepository().GetById(scenarioId);
                var scenarioTypes = configurations.Select(c => c.ScenarioDataTypeId).Distinct().ToList();
                var startYear = scenario.FinancialYear;
                var years = configurations.Select(c => c.Year).Distinct().ToList();

                var scenarioFile = new ScenarioFile
                {
                    ScenarioFileName = fileName,
                    UploadSessionId = _uploadSessionId,
                    ScenarioId = scenarioId,
                    TypeId = 1,
                    Year = scenario.FinancialYear,
                    Quarter = quarters.First(),
                    FilePath = filePath
                };
                new ScenarioFileRepository().SaveScenarioFile(scenarioFile, userName);

                //File Read & Excel Package
                var stream = new MemoryStream(File.ReadAllBytes(filePath));
                var package = new ExcelPackage(stream);
                var workSheet = package.Workbook.Worksheets[1];

                var IsValidTemplate = true;
                var row = 4;
                if (string.IsNullOrEmpty(workSheet.Cells["A4"].Value?.ToString().Replace("\n", ""))
                    || workSheet.Cells["A4"].Value?.ToString().Replace("\n", "") != "Project Code")
                {
                    IsValidTemplate = false;
                    errorLogs.Add(new ScenarioUploadLog
                    {
                        ScenarioID = scenarioId,
                        RowNumber = row,
                        ProjectCode = "",
                        CreatedBy = userName,
                        UploadSessionID = new Guid(_uploadSessionId),
                        UploadDescription = $"InCorrect value at cell A{row} - Expected Value: Project Code, Actual Value : {workSheet.Cells[$"A{row}"].Value?.ToString().Replace("\n", "")}.",
                        ColumnNumber = workSheet.Cells[$"A{row}"].Start.Column
                    });
                }
                foreach (var config in configurations)
                {
                    var headerText = "";
                    if (config.Year == "Year1")
                        headerText = config.FieldName.Replace(config.Year, $"{startYear}").Replace(" ", "");
                    else if (config.Year == "Year2")
                        headerText = config.FieldName.Replace(config.Year, $"{startYear + 1}").Replace(" ", "");
                    else if (config.Year == "Year3")
                        headerText = config.FieldName.Replace(config.Year, $"{startYear + 2}").Replace(" ", "");
                    else if (config.Year == "Year4")
                        headerText = config.FieldName.Replace(config.Year, $"{startYear + 3}").Replace(" ", "");

                    var excelCellValue = workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Value?.ToString().Replace("\n", "").Replace("\r", "");

                    if (string.IsNullOrWhiteSpace(excelCellValue) || !headerText.Equals(excelCellValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        IsValidTemplate = false;
                        errorLogs.Add(new ScenarioUploadLog
                        {
                            ScenarioID = scenarioId,
                            RowNumber = row,
                            ProjectCode = "",
                            CreatedBy = userName,
                            UploadSessionID = new Guid(_uploadSessionId),
                            UploadDescription = $"InCorrect value at cell {config.ExcelCellPosition}{row} - Expected Value: {config.FieldName}, Actual Value : {excelCellValue}.",
                            ColumnNumber = workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Start.Column
                        });
                    }
                }

                if (!IsValidTemplate)
                {
                    if (errorLogs.Count > 0)
                    {
                        var xml = new XElement("FileUploadLog",
                                from data in errorLogs
                                select new XElement("Log",
                                              new XElement("ScenarioID", data.ScenarioID),
                                              new XElement("ProjectCode", data.ProjectCode),
                                              new XElement("UploadSessionId", data.UploadSessionID),
                                              new XElement("UploadDescription", data.UploadDescription),
                                              new XElement("RowNumber", data.RowNumber),
                                              new XElement("CreatedBy", userName),
                                              new XElement("ColumnNumber", data.ColumnNumber)
                                             ));
                        new ScenarioFileRepository().SaveScenarioUploadLog(xml);
                    }
                    //Returns Exception if Wrong Template
                    return new ScenarioFileUploadResponse
                    {
                        ErrorCount = 1,
                        Message = "Invalid File Upload Template.",
                        UploadSessionId = _uploadSessionId
                    };
                }

                var scenarioMappedProjects = new ScenarioFileRepository().GetScenarioMappedProjects(scenarioId);
                row = 5;
                var endOfFile = false;
                var yearLocksData = new LockYearRepository().GetAll();
                var scenarioDatas = new List<ScenarioData>();

                while (!endOfFile)
                {
                    if (workSheet.Cells[$"A{row}"].Value?.ToString().Trim() == "Do not insert ROW below this line.")
                    {
                        endOfFile = true;
                        break;
                    }
                    if (string.IsNullOrWhiteSpace(workSheet.Cells[$"A{row}"].Value?.ToString()))
                    {
                        if (nullProjectCodeEntriesCount >= 15)
                        {
                            endOfFile = true;
                            break;
                        }
                        nullProjectCodeEntriesCount++;
                    }
                    else
                    {
                        nullProjectCodeEntriesCount = 1;
                    }

                    var projectCode = workSheet.Cells[$"A{row}"].Value?.ToString().Trim();
                    if (string.IsNullOrWhiteSpace(projectCode))
                    {
                       
                        if (!string.IsNullOrWhiteSpace(workSheet.Cells[$"A{row + 1}"].Value?.ToString().Trim()))
                        {
                            errorLogs.Add(new ScenarioUploadLog
                            {
                                ScenarioID = scenarioId,
                                RowNumber = row,
                                ProjectCode = projectCode,
                                CreatedBy = userName,
                                UploadSessionID = new Guid(_uploadSessionId),
                                UploadDescription = $"ProjectCode Empty at CellPosition : A{row}.",
                                ColumnNumber = workSheet.Cells[$"A{row}"].Start.Column
                            });
                        }
                    }
                    else if (!scenarioMappedProjects.Select(s => s.ProjectCode).Contains(projectCode))
                    {
                        bool hasvalue = CheckExelRow(workSheet, 36, 56, row,1,false);
                        if (hasvalue)
                        {
                            errorLogs.Add(new ScenarioUploadLog
                            {
                                ScenarioID = scenarioId,
                                RowNumber = row,
                                ProjectCode = projectCode,
                                CreatedBy = userName,
                                UploadSessionID = new Guid(_uploadSessionId),
                                UploadDescription = $"ProjectCode not mapped to Scenario at CellPosition : A{row}.",
                                ColumnNumber = workSheet.Cells[$"A{row}"].Start.Column
                            });
                        }
                    }
                    else
                    {
                        var scenarioDataTypeId = 0;
                        var projectId = scenarioMappedProjects.FirstOrDefault(s => s.ProjectCode == projectCode).ProjectId;

                        if (scenarioDatas.Select(s => s.ProjectId).Contains(projectId))
                        {
                            errorLogs.Add(new ScenarioUploadLog
                            {
                                ScenarioID = scenarioId,
                                RowNumber = row,
                                ProjectCode = projectCode,
                                CreatedBy = userName,
                                UploadSessionID = new Guid(_uploadSessionId),
                                UploadDescription = $"Duplicate Entry for Project CellPosition : A{row}.",
                                ColumnNumber = workSheet.Cells[$"A{row}"].Start.Column
                            });
                        }

                        foreach (var config in configurations)
                        {
                            if (scenarioDataTypeId == config.ScenarioDataTypeId)
                                continue;

                            scenarioDataTypeId = config.ScenarioDataTypeId;
                            foreach (var year in years)
                            {
                                var data = new ScenarioData()
                                {
                                    ScenarioId = scenarioId,
                                    ProjectId = projectId,
                                    ScenarioDataTypeId = scenarioDataTypeId,
                                    UploadSessionIdQ1 = _uploadSessionId,
                                    RowNumber = row
                                };
                                if (year == "Year1")
                                    data.Year = startYear;
                                else if (year == "Year2")
                                    data.Year = startYear + 1;
                                else if (year == "Year3")
                                    data.Year = startYear + 2;
                                else if (year == "Year4")
                                    data.Year = startYear + 3;

                                bool isValidYearData = true;

                                var IsYearLocked = (yearLocksData.FirstOrDefault(y => y.Year == data.Year) != null) ?
                                    yearLocksData.FirstOrDefault(y => y.Year == data.Year).Lock : false;

                                if (!IsYearLocked)
                                {
                                    foreach (var item in configurations.Where(c => c.ScenarioDataTypeId == scenarioDataTypeId && c.Year == year).ToList())
                                    {
                                        decimal? _scenarioDataValue = null;
                                        bool isNegetive = false;
                                        var val = (workSheet.Cells[$"{item.ExcelCellPosition}{row}"].Value != null) ? workSheet.Cells[$"{item.ExcelCellPosition}{row}"].Value?.ToString() : string.Empty;

                                        if (val.Contains("E-") || (val == "-0"))
                                        {
                                            continue;
                                        }
                                        else if (val.Contains("-"))
                                        {
                                            isNegetive = true;
                                            val = val.Replace("-", "");
                                        }

                                        try
                                        {
                                            if (val != "")
                                                _scenarioDataValue = Math.Round(Convert.ToDecimal(val), 3);
                                        }
                                        catch (Exception ex)
                                        {
                                            isValidYearData = false;
                                            errorLogs.Add(new ScenarioUploadLog
                                            {
                                                ScenarioID = scenarioId,
                                                RowNumber = row,
                                                ProjectCode = projectCode,
                                                CreatedBy = userName,
                                                UploadSessionID = new Guid(_uploadSessionId),
                                                UploadDescription = $"Invalid value for column {config.FieldName} at {item.ExcelCellPosition}{row}.",
                                                ColumnNumber = workSheet.Cells[$"{item.ExcelCellPosition}{row}"].Start.Column
                                            });
                                            continue;
                                        }
                                        if (item.Quarter == "Q1")
                                        {
                                            if (year == "Year1" && (quarters.Contains("Q1")))
                                                data.Q1New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                            else if (year == "Year2")
                                                data.Q1New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                            else if (year == "Year3" || year == "Year4")
                                            {
                                                data.Q1New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                            }
                                        }
                                        else if (item.Quarter == "Q2")
                                        {
                                            if (year == "Year1" && (quarters.Contains("Q2")))
                                                data.Q2New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                            else if (year == "Year2")
                                                data.Q2New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                        }
                                        else if (item.Quarter == "Q3")
                                        {
                                            if (year == "Year1" && (quarters.Contains("Q3")))
                                                data.Q3New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                            else if (year == "Year2")
                                                data.Q3New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                        }
                                        else if (item.Quarter == "Q4")
                                        {
                                            if (year == "Year1" && (quarters.Contains("Q4")))
                                                data.Q4New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                            else if (year == "Year2")
                                                data.Q4New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                                        }


                                    }
                                }
                                if (isValidYearData)
                                    scenarioDatas.Add(data);
                            }
                        }
                    }
                    row++;
                }

                var uploadList = new List<ScenarioData>();
                foreach (var item in scenarioDatas)
                {
                    uploadList.Add(item);
                    if (uploadList.Count == 100)
                    {
                        var xml = new XElement("ScenarioDatas",
                            from data in uploadList
                            select new XElement("ScenarioData",
                                          new XElement("ScenarioID", data.ScenarioId),
                                          new XElement("Year", data.Year),
                                          new XElement("ProjectID", data.ProjectId),
                                          new XElement("ScenarioDataTypeId", data.ScenarioDataTypeId),
                                          new XElement("UploadSessionId", data.UploadSessionIdQ1),
                                          new XElement("Q1New", data.Q1New),
                                          new XElement("Q2New", data.Q2New),
                                          new XElement("Q3New", data.Q3New),
                                          new XElement("Q4New", data.Q4New),
                                          new XElement("CreatedBy", userName),
                                          new XElement("RowNumber", data.RowNumber)
                                         ));
                        new ScenarioFileRepository().UploadProjectScenarioDataType_1(activeQuarters, xml);
                        uploadList.Clear();
                    }
                }
                if (uploadList.Count > 0)
                {
                    var xml = new XElement("ScenarioDatas",
                            from data in uploadList
                            select new XElement("ScenarioData",
                                          new XElement("ScenarioID", data.ScenarioId),
                                          new XElement("Year", data.Year),
                                          new XElement("ProjectID", data.ProjectId),
                                          new XElement("ScenarioDataTypeId", data.ScenarioDataTypeId),
                                          new XElement("UploadSessionId", data.UploadSessionIdQ1),
                                          new XElement("Q1New", data.Q1New),
                                          new XElement("Q2New", data.Q2New),
                                          new XElement("Q3New", data.Q3New),
                                          new XElement("Q4New", data.Q4New),
                                          new XElement("CreatedBy", userName),
                                          new XElement("RowNumber", data.RowNumber)
                                         ));
                    new ScenarioFileRepository().UploadProjectScenarioDataType_1(activeQuarters, xml);
                    uploadList.Clear();
                }

                // Save ErrorLogs to be Done.
                if (errorLogs.Count > 0)
                {
                    var xml = new XElement("FileUploadLog",
                            from data in errorLogs
                            select new XElement("Log",
                                          new XElement("ScenarioID", data.ScenarioID),
                                          new XElement("ProjectCode", data.ProjectCode),
                                          new XElement("UploadSessionId", data.UploadSessionID),
                                          new XElement("UploadDescription", data.UploadDescription),
                                          new XElement("RowNumber", data.RowNumber),
                                          new XElement("CreatedBy", userName),
                                          new XElement("ColumnNumber", data.ColumnNumber)
                                         ));
                    new ScenarioFileRepository().SaveScenarioUploadLog(xml);
                }

                return new ScenarioFileUploadResponse
                {
                    ErrorCount = (errorLogs.Count > 0) ? 1 : 0,
                    UploadSessionId = _uploadSessionId
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ScenarioFileUploadResponse UploadProjectScenarioDataType_2_New(int scenarioId, string activeQuarters, string fileName, string filePath, string userName, int customerId,int departmentId, int clientId)
        {
            //ScenarioRepository _scenarioRepo = new ScenarioRepository();
            //ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            //LockYearRepository _lockYearRepository = new LockYearRepository();
            var errorLogs = new List<ScenarioUploadLog>();
            int row = 3;
            bool reachedEndOfFile = false;
            var nullProjectCodeEntriesCount = 0;
            var _uploadSessionId = Guid.NewGuid().ToString().ToUpper();

            try
            {
                var configurations = new ScenarioFileRepository().GetTemplateConfigurations(2);
                var scenario = new ScenarioRepository().GetById(scenarioId);
                var year = scenario.FinancialYear;
                var yearLocksData = new LockYearRepository().GetAll();
                var IsYearLocked = (yearLocksData.FirstOrDefault(y => y.Year == year) != null) ?
                                    yearLocksData.FirstOrDefault(y => y.Year == year).Lock : false;

                var scenarioFile = new ScenarioFile
                {
                    ScenarioFileName = fileName,
                    UploadSessionId = _uploadSessionId,
                    ScenarioId = scenarioId,
                    TypeId = 2,
                    Year = scenario.FinancialYear,
                    Quarter = activeQuarters,
                    FilePath = filePath
                };
                new ScenarioFileRepository().SaveScenarioFile(scenarioFile, userName);

                if (IsYearLocked)
                {
                    return new ScenarioFileUploadResponse
                    {
                        ErrorCount = 1,
                        Message = "Scenario - Financial Year Locked.",
                        UploadSessionId = ""
                    };
                }

                var stream = new MemoryStream(File.ReadAllBytes(filePath));
                var package = new ExcelPackage(stream);
                var workSheet = package.Workbook.Worksheets[1];

                var IsValidTemplate = true;
                if (string.IsNullOrEmpty(workSheet.Cells[$"C{row}"].Value?.ToString().Replace("\n", ""))
                    || workSheet.Cells[$"C{row}"].Value?.ToString().Replace("\n", "") != "Grouped")
                {
                    IsValidTemplate = false;
                    errorLogs.Add(new ScenarioUploadLog
                    {
                        ScenarioID = scenarioId,
                        RowNumber = row,
                        ProjectCode = "",
                        CreatedBy = userName,
                        UploadSessionID = new Guid(_uploadSessionId),
                        UploadDescription = $"InCorrect value at cell C{row} - Expected Value: Grouped, Actual Value : {workSheet.Cells[$"C{row}"].Value?.ToString().Replace("\n", "")}.",
                        ColumnNumber = workSheet.Cells[$"C{row}"].Start.Column
                    });
                }
                foreach (var config in configurations)
                {
                    var headerText = config.FieldName;
                    var excelCellValue = workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Value?.ToString().Replace("\n", "");
                    if (string.IsNullOrWhiteSpace(excelCellValue) || !headerText.Equals(excelCellValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        IsValidTemplate = false;
                        errorLogs.Add(new ScenarioUploadLog
                        {
                            ScenarioID = scenarioId,
                            RowNumber = row,
                            ProjectCode = "",
                            CreatedBy = userName,
                            UploadSessionID = new Guid(_uploadSessionId),
                            UploadDescription = $"InCorrect value at cell {config.ExcelCellPosition}{row} - Expected Value: {config.FieldName}, Actual Value : {excelCellValue}.",
                            ColumnNumber = workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Start.Column
                        });
                    }
                }

                if (!IsValidTemplate)
                {
                    if (errorLogs.Count > 0)
                    {
                        var xml = new XElement("FileUploadLog",
                                from data in errorLogs
                                select new XElement("Log",
                                              new XElement("ScenarioID", data.ScenarioID),
                                              new XElement("ProjectCode", data.ProjectCode),
                                              new XElement("UploadSessionId", data.UploadSessionID),
                                              new XElement("UploadDescription", data.UploadDescription),
                                              new XElement("RowNumber", data.RowNumber),
                                              new XElement("CreatedBy", userName),
                                              new XElement("ColumnNumber", data.ColumnNumber)
                                             ));
                        new ScenarioFileRepository().SaveScenarioUploadLog(xml);
                    }
                    //Returns Exception if Wrong Template
                    return new ScenarioFileUploadResponse
                    {
                        ErrorCount = 1,
                        Message = "Invalid File Upload Template.",
                        UploadSessionId = _uploadSessionId
                    };
                }

                var scenarioMappedProjects = new ScenarioFileRepository().GetScenarioMappedProjects(scenarioId);
                row = 4;
                var scenarioDatas = new List<ScenarioData>();
                var failedProjectIds = new List<int>();
                bool firsttime = true;
                var listProjectCode = new List<string>();
                while (!reachedEndOfFile)
                {
                    if (workSheet.Cells[$"A{row}"].Value?.ToString().Trim() == "Do not insert ROW below this line." || workSheet.Cells[$"C{row}"].Value?.ToString().Trim() == "TOTAL")
                    {
                        reachedEndOfFile = true;
                        break;
                    }
                    if (string.IsNullOrWhiteSpace(workSheet.Cells[$"C{row}"].Value?.ToString()))
                    {
                        if (nullProjectCodeEntriesCount >= 10)
                        {
                            reachedEndOfFile = true;
                            break;
                        }
                        nullProjectCodeEntriesCount++;
                    }
                    var projectCode = workSheet.Cells[$"C{row}"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(projectCode))
                    {
                        if (!string.IsNullOrWhiteSpace(workSheet.Cells[$"A{row + 1}"].Value?.ToString().Trim()))
                        {
                            errorLogs.Add(new ScenarioUploadLog
                            {
                                ScenarioID = scenarioId,
                                RowNumber = row,
                                ProjectCode = projectCode,
                                CreatedBy = userName,
                                UploadSessionID = new Guid(_uploadSessionId),
                                UploadDescription = $"ProjectCode Empty at CellPosition : C{row}."
                            });
                        }
                    }
                    else if (!scenarioMappedProjects.Select(s => s.ProjectCode).Contains(projectCode))
                    {
                       if(!listProjectCode.Contains(projectCode))
                        {
                            listProjectCode.Add(projectCode);
                            bool hasvalue = CheckExelRow(workSheet, 5, 9, row, 2, firsttime);
                            if (hasvalue)
                            {
                                errorLogs.Add(new ScenarioUploadLog
                                {
                                    ScenarioID = scenarioId,
                                    RowNumber = row,
                                    ProjectCode = projectCode,
                                    CreatedBy = userName,
                                    UploadSessionID = new Guid(_uploadSessionId),
                                    UploadDescription = $"ProjectCode not mapped to Scenario at CellPosition : C{row}."
                                });
                            }
                            firsttime = false;
                        }
                      
                        
                    }
                    else
                    {
                        var projectId = scenarioMappedProjects.FirstOrDefault(s => s.ProjectCode == projectCode).ProjectId;

                        foreach (var config in configurations)
                        {
                            bool isNegetive = false;
                            var scenarioData = new ScenarioData()
                            {
                                ScenarioId = scenarioId,
                                ProjectId = projectId,
                                ScenarioDataTypeId = config.ScenarioDataTypeId,
                                UploadSessionIdQ1 = _uploadSessionId,
                                Year = year,
                                ///CreatedBy = userName,
                                RowNumber = row
                            };
                            bool isNullValue = false;
                            if (workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Value ==null || workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Value?.ToString() == "")
                            {
                                isNullValue = true;
                            }
                            var val = (workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Value != null) ? workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Value?.ToString() : "0";
                            if (val.Contains("(") || val.Contains(")") || val.Contains("-"))
                            {
                                val = val.Replace("-", "");
                                isNegetive = true;
                            }
                            //else if (string.IsNullOrWhiteSpace(val))
                            //{
                            //    val = "0";
                            //}
                            decimal? _scenarioDataValue = null;
                            try
                            {
                                if (!isNullValue)
                                {
                                    if (val != "")
                                        _scenarioDataValue = Math.Round(Convert.ToDecimal(val), 3);
                                }                                
                            }
                            catch (Exception ex)
                            {
                                failedProjectIds.Add(scenarioData.ScenarioId);
                                errorLogs.Add(new ScenarioUploadLog
                                {
                                    ScenarioID = scenarioId,
                                    RowNumber = row,
                                    ProjectCode = projectCode,
                                    CreatedBy = userName,
                                    UploadSessionID = new Guid(_uploadSessionId),
                                    UploadDescription = $"Invalid value at Column {config.FieldName} at {config.ExcelCellPosition}{row}.",
                                    ColumnNumber = workSheet.Cells[$"{config.ExcelCellPosition}{row}"].Start.Column
                                });
                                continue;
                            }

                            if (activeQuarters == "Q1")
                            {
                                scenarioData.Q1New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                            }
                            else if (activeQuarters == "Q2")
                            {
                                scenarioData.Q2New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                            }
                            else if (activeQuarters == "Q3")
                            {
                                scenarioData.Q3New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                            }
                            else if (activeQuarters == "Q4")
                            {
                                scenarioData.Q4New = (isNegetive) ? (-1 * _scenarioDataValue) : _scenarioDataValue;
                            }

                            if (scenarioDatas.Where(sd => sd.ProjectId == projectId && sd.ScenarioDataTypeId == config.ScenarioDataTypeId).ToList().Count > 0)
                            {
                                var existingData = scenarioDatas.FirstOrDefault(p => p.ProjectId == projectId && p.ScenarioDataTypeId == config.ScenarioDataTypeId);
                                scenarioDatas.Remove(existingData);

                                if (activeQuarters == "Q1")
                                {
                                    if (!isNullValue)
                                        existingData.Q1New += scenarioData.Q1New;
                                }
                                else if (activeQuarters == "Q2")
                                {
                                    if (!isNullValue)
                                        existingData.Q2New += scenarioData.Q2New;
                                }
                                else if (activeQuarters == "Q3")
                                {
                                    if (!isNullValue)
                                        existingData.Q3New += scenarioData.Q3New;
                                }
                                else if (activeQuarters == "Q4")
                                {
                                    if (!isNullValue)
                                        existingData.Q4New += scenarioData.Q4New;
                                }

                                scenarioDatas.Add(existingData);
                            }
                            else
                            {
                                scenarioDatas.Add(scenarioData);
                            }
                        }
                    }
                    row++;
                }

                scenarioDatas.RemoveAll(x => failedProjectIds.Contains(x.ProjectId));

                var uploadList = new List<ScenarioData>();
                foreach (var item in scenarioDatas)
                {
                    uploadList.Add(item);
                    if (uploadList.Count == 100)
                    {
                        var xml = new XElement("ScenarioDatas",
                             from data in uploadList
                             select new XElement("ScenarioData",
                                           new XElement("ScenarioID", data.ScenarioId),
                                           new XElement("Year", data.Year),
                                           new XElement("ProjectID", data.ProjectId),
                                           new XElement("ScenarioDataTypeId", data.ScenarioDataTypeId),
                                           new XElement("UploadSessionId", data.UploadSessionIdQ1),
                                           new XElement("Q1New", data.Q1New),
                                           new XElement("Q2New", data.Q2New),
                                           new XElement("Q3New", data.Q3New),
                                           new XElement("Q4New", data.Q4New),
                                           new XElement("CreatedBy", userName),
                                           new XElement("RowNumber", data.RowNumber)
                                          ));
                        new ScenarioFileRepository().UploadProjectScenarioDataType_2(activeQuarters, xml);
                        uploadList.Clear();
                    }
                }
                if (uploadList.Count > 0)
                {
                    var xml = new XElement("ScenarioDatas",
                            from data in uploadList
                            select new XElement("ScenarioData",
                                          new XElement("ScenarioID", data.ScenarioId),
                                          new XElement("Year", data.Year),
                                          new XElement("ProjectID", data.ProjectId),
                                          new XElement("ScenarioDataTypeId", data.ScenarioDataTypeId),
                                          new XElement("UploadSessionId", data.UploadSessionIdQ1),
                                          new XElement("Q1New", data.Q1New),
                                          new XElement("Q2New", data.Q2New),
                                          new XElement("Q3New", data.Q3New),
                                          new XElement("Q4New", data.Q4New),
                                          new XElement("CreatedBy", userName),
                                          new XElement("RowNumber", data.RowNumber)
                                         ));
                    new ScenarioFileRepository().UploadProjectScenarioDataType_2(activeQuarters, xml);
                    uploadList.Clear();
                }

                if (errorLogs.Count > 0)
                {
                    var xml = new XElement("FileUploadLog",
                            from data in errorLogs
                            select new XElement("Log",
                                          new XElement("ScenarioID", data.ScenarioID),
                                          new XElement("ProjectCode", data.ProjectCode),
                                          new XElement("UploadSessionId", data.UploadSessionID),
                                          new XElement("UploadDescription", data.UploadDescription),
                                          new XElement("RowNumber", data.RowNumber),
                                          new XElement("CreatedBy", userName),
                                          new XElement("ColumnNumber", data.ColumnNumber)
                                         ));
                    new ScenarioFileRepository().SaveScenarioUploadLog(xml);
                }

                return new ScenarioFileUploadResponse
                {
                    ErrorCount = (errorLogs.Count > 0) ? 1 : 0,
                    Message = "",
                    UploadSessionId = _uploadSessionId
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ScenarioFileUploadResponse UploadProjectScenarioData(int scenarioId, string activeQuarters, string fileName, string filePath, string userName, int customerId, int departmentId, int clientId)
        {
            //var scenarioRepository = new ScenarioRepository();
            var fileUploadResponse = new ScenarioFileUploadResponse();

            var scenario = new ScenarioRepository().GetById(scenarioId);

            var scenarioScopeCode = scenario.ScenarioScopeCode;
            var scenarioTypeCode = scenario.ScenarioTypeCode;
            //ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            FileUploadLayout activeScenario = new ScenarioFileRepository().GetFileUploadLayouts(scenarioId)[0];
            List<string> defaultyQuarters = new List<string>() { "Q1", "Q2", "Q3", "Q4" };
            List<string> activeQuaterList = new List<string>();
            // List<string>  quarterList= activeScenario.Quarters.Where(i=>i.Lock==false).Select(i => i.Quarter.ToString()).ToList();
            List<string> quarterList = activeScenario.Quarters.Where(i => i.Lock == true).Select(i => i.Quarter.ToString()).ToList();
            //List<string> activeQuaterListString = activeQuaterList.Select(i => i.Quarter.ToString()).ToList();

            foreach (var item in defaultyQuarters)
            {
                var found = false;
                foreach (var quarter in quarterList)
                {
                    if (item == quarter)
                        found = true;
                }
                if (!found)
                    activeQuaterList.Add(item);

            }

            if (scenarioScopeCode == "PL" && scenarioTypeCode == "FC")
            {
                activeQuarters = string.Join(",", activeQuaterList);

                fileUploadResponse = UploadProjectScenarioDataType_1_New(scenarioId, activeQuarters, fileName, filePath, userName,customerId,departmentId,clientId);
            }
            else if (scenarioScopeCode == "PL" && scenarioTypeCode == "AC")
            {
                activeQuarters = activeQuaterList[0].ToString();
                fileUploadResponse = UploadProjectScenarioDataType_2_New(scenarioId, activeQuarters, fileName, filePath, userName,customerId,departmentId,clientId);
            }
            else
            {
                fileUploadResponse = new ScenarioFileUploadResponse
                {
                    ErrorCount = 1,
                    Message = "Upload Not Allowed For Scenario",
                    UploadSessionId = ""
                };
            }

            return fileUploadResponse;
        }

        public List<FileUploadLayout> GetFileUploadLayout(int scenarioId)
        {
            ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            return _scenarioFileRepo.GetFileUploadLayouts(scenarioId);
        }

        public List<ScenarioFile> GetScenariouploadlog()
        {
            ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            return _scenarioFileRepo.GetScenariouploadlog();
        }
      
        public List<ScenarioUploadLog> GetScenarioFailedlog(string sessionid)
        {
            ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            return _scenarioFileRepo.GetScenarioFailedlog(sessionid);

        }

        public string GetuploadToken(string uploadsessionId,string CreatedBy, int secondtime)
        {
            ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            return _scenarioFileRepo.GetuploadToken(uploadsessionId, CreatedBy, secondtime );
        }
        public ScenarioFile GetuploadFile(string token)
        {
            ScenarioFileRepository _scenarioFileRepo = new ScenarioFileRepository();
            return _scenarioFileRepo.GetuploadFile(token);
        }
    }
}
