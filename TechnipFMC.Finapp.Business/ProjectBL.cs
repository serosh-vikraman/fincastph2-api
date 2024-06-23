using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class ProjectBL :   IProjectBL
    {
        public ProjectBL(IProjectRepository projectRepo)
        {
            //_countryRepo = countryRepo;
        }
        public ProjectBL()
        { }
        public bool Delete(int Id, string Deletedby)
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<Project> GetAll(int departmentId)
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.GetAll(departmentId);
        }
        public IEnumerable<Projects> GetAllProjects()
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.GetAllProjects();
        }
        public IEnumerable<ProjectsToLink> GetAllProjectsOfScenario(int scenarioId)
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.GetAllProjectsOfScenario(scenarioId);
        }
        public int GetAllInactiveProjectsOfScenario(int scenarioId)
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.GetAllInactiveProjectsOfScenario(scenarioId);
        }
        public IEnumerable<ProjectDeLink> GetAllProjectsToLinkForLifeCycleReport()
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.GetAllProjectsToLinkForLifeCycleReport();
        }
        public Project GetById(int id)
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.GetById(id);
        }

        public Project Save(Project project)
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.Save(project);
        }        
        public byte[] DownloadProjectTemplate()
        {
            throw new NotImplementedException();
            //byte[] bytes;
            //var projectSegments = new ProjectSegmentRepository().GetAll();
            //var projectEntities = new ProjectEntityRepository().GetAll();
            //var bus = new BUCategoryRepository().GetAll();
            //var stats = new StatutoryCategoryRepository().GetAll();
            //var countries = new CountryRepository().GetAll();
            //var billingTypes = new BillingTypeRepository().GetAll();
            //var contractTypes = new ContractTypeRepository().GetAll();
            //var contractStatuses = new ContractStatusRepository().GetAll();
            //var smartViewCodes = new SmartViewCodeRepository().GetAll();
            //var groupingParameters = new GroupingParametersRepository().GetAll();
            //var mmgmtCategories = new ManagementCategoryRepository().GetAll();
            //var clubbingParams = new ClubbingParameterRepository().GetAll();

            //ExcelPackage ExcelPkg = new ExcelPackage();
            //ExcelWorksheet worksheet = ExcelPkg.Workbook.Worksheets.Add("ProjectTemplate");

            //ExcelWorksheet worksheetRef = ExcelPkg.Workbook.Worksheets.Add("ReferenceData");
            //var colcount = 79;
            //var rowcount = 1;
            //foreach (var item in projectSegments)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.ProjectSegmentName;
            //    rowcount++;
            //}
            //colcount++;
            //rowcount = 1;
            //foreach (var item in projectEntities)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.ProjectEntityName;
            //    rowcount++;
            //}
            //colcount++;
            //rowcount = 1;
            //foreach (var item in bus)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.BUCategoryName;
            //    rowcount++;
            //}
            //colcount++;
            //rowcount = 1;
            //foreach (var item in stats)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.StatutoryCategoryName;
            //    rowcount++;
            //}
            //colcount++;
            //rowcount = 1;
            //foreach (var item in countries)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.CountryName;
            //    rowcount++;
            //}

            //colcount++;
            //rowcount = 1;
            //foreach (var item in billingTypes)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.BillingTypeName;
            //    rowcount++;
            //}
            //colcount++;
            //rowcount = 1;
            //foreach (var item in contractTypes)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.ContractTypeName;
            //    rowcount++;
            //}

            //colcount++;
            //rowcount = 1;
            //foreach (var item in contractStatuses)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.ContractStatusName;
            //    rowcount++;
            //}

            //colcount++;
            //rowcount = 1;
            //foreach (var item in smartViewCodes)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.SmartViewName;
            //    rowcount++;
            //}

            //colcount++;
            //rowcount = 1;
            //foreach (var item in groupingParameters)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.GroupingParametersName;
            //    rowcount++;
            //}

            //colcount++;
            //rowcount = 1;
            //foreach (var item in mmgmtCategories)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.ManagementCategoryName;
            //    rowcount++;
            //}

            //colcount++;
            //rowcount = 1;
            //foreach (var item in clubbingParams)
            //{
            //    worksheet.Cells[rowcount, colcount].Value = item.ClubbingParameterName;
            //    rowcount++;
            //}
            //colcount = 1;
            //rowcount = 1;
            //worksheet.Cells[rowcount, colcount].Value = "Project Name";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "IFS Project Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Manual Project Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Clubbing Parameters Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Project Segment Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Project Entity Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "BU Category Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Statutory Category Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Country Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Billing Type Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Contract Type Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Contract Status Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Smart View Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Grouping Parameters Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Management Category Code";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Project Status";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;
            //colcount++;
            //worksheet.Cells[rowcount, colcount].Value = "Notes";
            //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = 25;
            //worksheet.Column(colcount).Style.WrapText = true;
            //worksheet.Cells[rowcount, colcount].Style.Locked = true;


            //for (int i = 0; i < 100; i++)
            //{


            //    rowcount++;
            //    colcount = 1;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //    colcount++;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //    colcount++;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //    colcount++;

            //    var za = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    za.AllowBlank = true;
            //    var formula = "$CL$1:$CL$" + clubbingParams.Count();
            //    za.Formula.ExcelFormula = formula;
            //    //foreach (var item in clubbingParams)
            //    //{
            //    //    za.Formula.Values.Add(item.ClubbingParameterName);
            //    //}


            //    colcount++;
            //    var a = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    a.AllowBlank = true;
            //    formula = "$CA$1:$CA$" + projectSegments.Count();
            //    a.Formula.ExcelFormula = formula;
            //    //foreach (var item in projectSegments)
            //    //{
            //    //    a.Formula.Values.Add(item.ProjectSegmentName);
            //    //}
            //    colcount++;
            //    var b = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    b.AllowBlank = true;
            //    formula = "$CB$1:$CB$" + projectEntities.Count();
            //    b.Formula.ExcelFormula = formula;
            //    //foreach (var item in projectEntities)
            //    //{
            //    //    b.Formula.Values.Add(item.ProjectEntityName);
            //    //}
            //    colcount++;
            //    var c = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    c.AllowBlank = true;
            //    formula = "$CC$1:$CC$" + bus.Count();
            //    c.Formula.ExcelFormula = formula;
            //    //foreach (var item in bus)
            //    //{
            //    //    c.Formula.Values.Add(item.BUCategoryName);
            //    //}

            //    colcount++;
            //    var d = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    d.AllowBlank = true;
            //    formula = "$CD$1:$CD$" + stats.Count();
            //    d.Formula.ExcelFormula = formula;
            //    //foreach (var item in stats)
            //    //{
            //    //    d.Formula.Values.Add(item.StatutoryCategoryName);
            //    //}

            //    colcount++;
            //    var e = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    e.AllowBlank = true;
            //    formula = "$CE$1:$CE$" + countries.Count();
            //    e.Formula.ExcelFormula = formula;
            //    //foreach (var item in countries)
            //    //{
            //    //    e.Formula.Values.Add(item.CountryName);
            //    //}

            //    colcount++;
            //    var f = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    f.AllowBlank = true;
            //    formula = "$CF$1:$CF$" + billingTypes.Count();
            //    f.Formula.ExcelFormula = formula;
            //    //foreach (var item in billingTypes)
            //    //{
            //    //    f.Formula.Values.Add(item.BillingTypeName);
            //    //}

            //    colcount++;
            //    var g = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    g.AllowBlank = true;
            //    formula = "$CG$1:$CG$" + contractTypes.Count();
            //    g.Formula.ExcelFormula = formula;
            //    //foreach (var item in contractTypes)
            //    //{
            //    //    g.Formula.Values.Add(item.ContractTypeName);
            //    //}

            //    colcount++;
            //    var h = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    h.AllowBlank = true;
            //    formula = "$CH$1:$CH$" + contractStatuses.Count();
            //    h.Formula.ExcelFormula = formula;
            //    //foreach (var item in contractStatuses)
            //    //{
            //    //    h.Formula.Values.Add(item.ContractStatusName);
            //    //}

            //    colcount++;
            //    var iq = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    iq.AllowBlank = true;
            //    formula = "$CI$1:$CI$" + smartViewCodes.Count();
            //    iq.Formula.ExcelFormula = formula;
            //    //foreach (var item in smartViewCodes)
            //    //{
            //    //    iq.Formula.Values.Add(item.SmartViewName);
            //    //}

            //    colcount++;
            //    var j = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    j.AllowBlank = true;
            //    formula = "$CJ$1:$CJ$" + groupingParameters.Count();
            //    j.Formula.ExcelFormula = formula;
            //    //foreach (var item in groupingParameters)
            //    //{
            //    //    j.Formula.Values.Add(item.GroupingParametersName);
            //    //}

            //    colcount++;
            //    var k = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    k.AllowBlank = true;
            //    formula = "$CK$1:$CK$" + mmgmtCategories.Count();
            //    k.Formula.ExcelFormula = formula;
            //    //foreach (var item in mmgmtCategories)
            //    //{
            //    //    k.Formula.Values.Add(item.ManagementCategoryName);
            //    //}

            //    colcount++;
            //    var l = worksheet.Cells[rowcount, colcount].DataValidation.AddListDataValidation() as ExcelDataValidationList;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //    l.AllowBlank = true;
            //    l.Formula.Values.Add("Active");
            //    l.Formula.Values.Add("Inactive");


            //    colcount++;
            //    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //}

            //worksheet.Protection.IsProtected = false;
            //worksheetRef.Protection.IsProtected = true;
            ////worksheet.Protection.AllowSelectLockedCells = false;
            //bytes = ExcelPkg.GetAsByteArray();
            //return bytes;
        }

        public ProjectFileUploadResponse UploadProjectData(string sessionId, string fileName, string filePath, string userName)
        {
            throw new NotImplementedException();
            //List<ProjectUploadLog> listProjectUploadLog = new List<ProjectUploadLog>();

            //List<TempProject> listTempProject = new List<TempProject>();
            //var response = new ProjectFileUploadResponse();


            //var isSuccess = new ProjectRepository().UploadProjectData(sessionId, fileName, filePath, userName);

            //var projectFilePath = filePath + fileName;
            //var stream = new MemoryStream(File.ReadAllBytes(projectFilePath));
            //var package = new ExcelPackage(stream);
            //var workSheet = package.Workbook.Worksheets[1];

            //var projectSegments = new ProjectSegmentRepository().GetAll();
            //var projectEntities = new ProjectEntityRepository().GetAll();
            //var bus = new BUCategoryRepository().GetAll();
            //var stats = new StatutoryCategoryRepository().GetAll();
            //var countries = new CountryRepository().GetAll();
            //var billingTypes = new BillingTypeRepository().GetAll();
            //var contractTypes = new ContractTypeRepository().GetAll();
            //var contractStatuses = new ContractStatusRepository().GetAll();
            //var smartViewCodes = new SmartViewCodeRepository().GetAll();
            //var groupingParameters = new GroupingParametersRepository().GetAll();
            //var mmgmtCategories = new ManagementCategoryRepository().GetAll();
            //var clubParams = new ClubbingParameterRepository().GetAll();

            //int columnNumber = 1;
            //var projectNameColumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var ifsprojectCodeColumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var projectCodeColumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var clubbingParameterColumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var projectSegmentscolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var projectEntitiescolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var buscolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var statscolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var countryscolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var billingTypecolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var contractTypecolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var contractStatuscolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var smartViewCodecolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var groupingParametercolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var mmgmtCategoriescolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var projectStatuscolumnNumber = columnNumber;
            //columnNumber = columnNumber + 1;
            //var NotescolumnNumber = columnNumber;

            //var lastRowrowNumberDataEndsAtInExcelSheetNumber = workSheet.Dimension.End.Row;
            //var rowNum = 2;
            //for (int i = 2; i <= lastRowrowNumberDataEndsAtInExcelSheetNumber; i++)
            //{
            //    var isError = false;
            //    try
            //    {
            //        TempProject project = new TempProject { };
            //        project.RowNum = rowNum;
            //        project.SessionId = sessionId;
            //        var manualExist = false;
            //        var ifsExist = false;
            //        project.IFSProjectCode = Convert.ToString(workSheet.Cells[rowNum, ifsprojectCodeColumnNumber].Value);
            //        project.ProjectCode = Convert.ToString(workSheet.Cells[rowNum, projectCodeColumnNumber].Value);
            //        if (project.ProjectCode.Length == 0)
            //            break;

            //        project.ProjectName = Convert.ToString(workSheet.Cells[rowNum, projectNameColumnNumber].Value);

            //        if (new ProjectRepository().IsProjectExist(project.ProjectName))
            //        {
            //            listProjectUploadLog.Add(new ProjectUploadLog()
            //            {
            //                ColumnNumber = projectNameColumnNumber,
            //                RowNumber = rowNum,
            //                SessionID = sessionId,
            //                Message = "Project Name exist."
            //            });
            //            isError = true;
            //        }
            //        if (project.ProjectName.Length < 3)
            //        {
            //            listProjectUploadLog.Add(new ProjectUploadLog()
            //            {
            //                ColumnNumber = projectNameColumnNumber,
            //                RowNumber = rowNum,
            //                SessionID = sessionId,
            //                Message = "Name should contain atleast 3 characters."
            //            });
            //            isError = true;
            //        }
            //        if (project.ProjectName.Length > 250)
            //        {
            //            listProjectUploadLog.Add(new ProjectUploadLog()
            //            {
            //                ColumnNumber = projectNameColumnNumber,
            //                RowNumber = rowNum,
            //                SessionID = sessionId,
            //                Message = "Name should not exceed 250 characters."
            //            });
            //            isError = true;
            //        }

            //        if (project.IFSProjectCode.Length > 0)
            //        {
            //            ifsExist = true;
            //        }
            //        if (project.ProjectCode.Length > 0)
            //        {
            //            manualExist = true;
            //        }
            //        if (manualExist == false)
            //        {
            //            listProjectUploadLog.Add(new ProjectUploadLog()
            //            {
            //                ColumnNumber = projectCodeColumnNumber,
            //                RowNumber = rowNum,
            //                SessionID = sessionId,
            //                Message = "IFS Project Code or  Manual Project Code is mandatory."
            //            });
            //            isError = true;
            //        }

            //        if (manualExist)
            //        {
            //            if (project.ProjectCode.Length < 3)
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = projectCodeColumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Manual Project Code should contain atleast 3 characters."
            //                });
            //                isError = true;
            //            }
            //            if (project.ProjectCode.Length > 10)
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = projectCodeColumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Manual Project Code should not exceed 10 characters."
            //                });
            //                isError = true;
            //            }
            //        }
            //        if (project.IFSProjectCode.Length > 0)
            //            project.ProjectType = "Referential";
            //        else if (project.ManualProjectCode.Length > 0)
            //            project.ProjectType = "Manual";


            //        project.ClubbingParameterName = Convert.ToString(workSheet.Cells[rowNum, clubbingParameterColumnNumber].Value);

            //        if (project.ClubbingParameterName.Trim() != "")
            //        {
            //            var result = clubParams.Where(a => a.ClubbingParameterName == project.ClubbingParameterName);
            //            if (result.Count() > 0)
            //                project.ClubbingParameterCode = result.FirstOrDefault().ClubbingParameterCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = clubbingParameterColumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid clubbing parameter selected."
            //                });
            //            }
            //        }

            //        project.ProjectSegmentName = Convert.ToString(workSheet.Cells[rowNum, projectSegmentscolumnNumber].Value);
            //        if (project.ProjectSegmentName.Trim() != "")
            //        {
            //            var result = projectSegments.Where(a => a.ProjectSegmentName == project.ProjectSegmentName);
            //            if (result.Count() > 0)
            //                project.ProjectSegmentCode = result.FirstOrDefault().ProjectSegmentCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = projectSegmentscolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Project Segment selected."
            //                });
            //            }
            //        }
            //        project.ProjectEntityName = Convert.ToString(workSheet.Cells[rowNum, projectEntitiescolumnNumber].Value);
            //        if (project.ProjectEntityName.Trim() != "")
            //        {
            //            var result = projectEntities.Where(a => a.ProjectEntityName == project.ProjectEntityName);
            //            if (result.Count() > 0)
            //                project.ProjectEntityCode = result.FirstOrDefault().ProjectEntityCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = projectEntitiescolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Project Entity selected."
            //                });
            //            }

            //        }
            //        project.BUCategoryName = Convert.ToString(workSheet.Cells[rowNum, buscolumnNumber].Value);
            //        if (project.BUCategoryName.Trim() != "")
            //        {
            //            var result = bus.Where(a => a.BUCategoryName == project.BUCategoryName);
            //            if (result.Count() > 0)
            //                project.BUCategoryCode = result.FirstOrDefault().BUCategoryCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = buscolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid BU Category code selected."
            //                });
            //            }

            //        }
            //        project.StatutoryCategoryName = Convert.ToString(workSheet.Cells[rowNum, statscolumnNumber].Value);
            //        if (project.StatutoryCategoryName.Trim() != "")
            //        {
            //            var result = stats.Where(a => a.StatutoryCategoryName == project.StatutoryCategoryName);
            //            if (result.Count() > 0)
            //                project.StatutoryCategoryCode = result.FirstOrDefault().StatutoryCategoryCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = statscolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Statutory Category selected."
            //                });
            //            }
            //        }
            //        project.CountryName = Convert.ToString(workSheet.Cells[rowNum, countryscolumnNumber].Value);
            //        if (project.CountryName.Trim() != "")
            //        {
            //            var result = countries.Where(a => a.CountryName == project.CountryName);
            //            if (result.Count() > 0)
            //                project.CountryCode = result.FirstOrDefault().CountryCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = countryscolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Country selected."
            //                });
            //            }
            //        }
            //        project.BillingTypesName = Convert.ToString(workSheet.Cells[rowNum, billingTypecolumnNumber].Value);
            //        if (project.BillingTypesName.Trim() != "")
            //        {
            //            var result = billingTypes.Where(a => a.BillingTypeName == project.BillingTypesName);
            //            if (result.Count() > 0)
            //                project.BillingTypesCode = result.FirstOrDefault().BillingTypeCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = billingTypecolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Billing Type selected."
            //                });
            //            }
            //        }
            //        project.ContractTypeName = Convert.ToString(workSheet.Cells[rowNum, contractTypecolumnNumber].Value);
            //        if (project.ContractTypeName.Trim() != "")
            //        {
            //            var result = contractTypes.Where(a => a.ContractTypeName == project.ContractTypeName);
            //            if (result.Count() > 0)
            //                project.ContractTypeCode = result.FirstOrDefault().ContractTypeCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = contractTypecolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Contract Type selected."
            //                });
            //            }
            //        }

            //        project.ContractStatusName = Convert.ToString(workSheet.Cells[rowNum, contractStatuscolumnNumber].Value);
            //        if (project.ContractStatusName.Trim() != "")
            //        {
            //            var result = contractStatuses.Where(a => a.ContractStatusName == project.ContractStatusName);
            //            if (result.Count() > 0)
            //                project.ContractStatusCode = result.FirstOrDefault().ContractStatusCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = contractStatuscolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Contract Status selected."
            //                });
            //            }
            //        }
            //        project.SmartViewName = Convert.ToString(workSheet.Cells[rowNum, smartViewCodecolumnNumber].Value);
            //        if (project.SmartViewName.Trim() != "")
            //        {
            //            var result = smartViewCodes.Where(a => a.SmartViewName == project.SmartViewName);
            //            if (result.Count() > 0)
            //                project.SmartViewCode = result.FirstOrDefault().SmartViewCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = smartViewCodecolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Smart View Code selected."
            //                });
            //            }
            //        }
            //        project.GroupingParametersName = Convert.ToString(workSheet.Cells[rowNum, groupingParametercolumnNumber].Value);
            //        if (project.GroupingParametersName.Trim() != "")
            //        {
            //            var result = groupingParameters.Where(a => a.GroupingParametersName == project.GroupingParametersName);
            //            if (result.Count() > 0)
            //                project.GroupingParametersCode = result.FirstOrDefault().GroupingParametersCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = groupingParametercolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Grouping Parameter Code selected."
            //                });
            //            }
            //        }

            //        project.ManagementCategoryName = Convert.ToString(workSheet.Cells[rowNum, mmgmtCategoriescolumnNumber].Value);
            //        if (project.ManagementCategoryName.Trim() != "")
            //        {
            //            var result = mmgmtCategories.Where(a => a.ManagementCategoryName == project.ManagementCategoryName);
            //            if (result.Count() > 0)
            //                project.ManagementCategoryCode = result.FirstOrDefault().ManagementCategoryCode;
            //            else
            //            {
            //                listProjectUploadLog.Add(new ProjectUploadLog()
            //                {
            //                    ColumnNumber = mmgmtCategoriescolumnNumber,
            //                    RowNumber = rowNum,
            //                    SessionID = sessionId,
            //                    Message = "Invalid Management Category Code selected."
            //                });
            //            }
            //        }
            //        project.ProjectStatus = Convert.ToString(workSheet.Cells[rowNum, projectStatuscolumnNumber].Value);
            //        project.NOTES = Convert.ToString(workSheet.Cells[rowNum, NotescolumnNumber].Value);

            //        if (project.ClubbingParameterName.Trim() != "")
            //        {
            //            var tempFirstList = listTempProject.Where(a => a.ClubbingParameterName == project.ClubbingParameterName).ToList();
            //            if (tempFirstList.Count > 0)
            //            {
            //                var tempFirst = tempFirstList.FirstOrDefault();
            //                if (tempFirst.ProjectEntityCode != project.ProjectEntityCode || tempFirst.ProjectSegmentCode != project.ProjectSegmentCode
            //                    || tempFirst.ContractStatusCode != project.ContractStatusCode || tempFirst.ContractTypeCode != project.ContractTypeCode
            //                    || tempFirst.ManagementCategoryCode != project.ManagementCategoryCode)
            //                {
            //                    listProjectUploadLog.Add(new ProjectUploadLog()
            //                    {
            //                        ColumnNumber = projectCodeColumnNumber,
            //                        RowNumber = rowNum,
            //                        SessionID = sessionId,
            //                        Message = "Clubbiing parameter not matching."
            //                    });
            //                    isError = true;
            //                }
            //            }
            //            else
            //            {
            //                var firstProjectList = new ProjectRepository().GetAllProjectByClubParam(project.ClubbingParameterName).ToList();
            //                if (firstProjectList.Count > 0)
            //                {
            //                    var firstProject = firstProjectList.FirstOrDefault();
            //                    if (firstProject.ProjectEntityCode != project.ProjectEntityCode || firstProject.ProjectSegmentCode != project.ProjectSegmentCode
            //                        || firstProject.ContractStatusCode != project.ContractStatusCode || firstProject.ContractTypeCode != project.ContractTypeCode
            //                        || firstProject.ManagementCategoryCode != project.ManagementCategoryCode)
            //                    {
            //                        listProjectUploadLog.Add(new ProjectUploadLog()
            //                        {
            //                            ColumnNumber = projectCodeColumnNumber,
            //                            RowNumber = rowNum,
            //                            SessionID = sessionId,
            //                            Message = "Clubbiing parameter not matching."
            //                        });
            //                        isError = true;
            //                    }
            //                }
            //            }
            //        }

            //        project.RecordStatus = (isError == true ? " Error" : "OK");

            //        listTempProject.Add(project);
            //    }
            //    catch (Exception ex)
            //    {
            //        listProjectUploadLog.Add(new ProjectUploadLog()
            //        {
            //            ColumnNumber = 0,
            //            RowNumber = rowNum,
            //            SessionID = sessionId,
            //            Message = ex.Message,
            //            ErrorCode = ""
            //        });

            //    }

            //    rowNum = rowNum + 1;

            //}
            //XElement xml = null;
            //XElement xmlError = null;
            //if (listTempProject.Count > 0)
            //{
            //    xml = new XElement("Projects",
            //          from data in listTempProject
            //          select new XElement("Project",
            //                          new XElement("SessionId", data.SessionId),
            //                          new XElement("RowNum", data.RowNum),
            //                          new XElement("ProjectName", data.ProjectName),
            //                          new XElement("ProjectType", data.ProjectType),
            //                          new XElement("IFSProjectCode", data.IFSProjectCode),
            //                          new XElement("ManualProjectCode", data.ProjectCode),
            //                          new XElement("ProjectSegmentName", data.ProjectSegmentName),
            //                          new XElement("ProjectEntityName", data.ProjectEntityName),
            //                          new XElement("BUCategoryName", data.BUCategoryName),
            //                          new XElement("StatutoryCategoryName", data.StatutoryCategoryName),
            //                          new XElement("CountryName", data.CountryName),
            //                          new XElement("BillingTypesName", data.BillingTypesName),
            //                          new XElement("ContractTypeName", data.ContractTypeName),
            //                          new XElement("ContractStatusName", data.ContractStatusName),
            //                          new XElement("SmartViewName", data.SmartViewName),
            //                          new XElement("GroupingParametersName", data.GroupingParametersName),
            //                          new XElement("ManagementCategoryName", data.ManagementCategoryName),
            //                          new XElement("ClubbingParameterName", data.ClubbingParameterName),

            //                          new XElement("ProjectSegmentCode", data.ProjectSegmentCode),
            //                          new XElement("ProjectEntityCode", data.ProjectEntityCode),
            //                          new XElement("BUCategoryCode", data.BUCategoryCode),
            //                          new XElement("StatutoryCategoryCode", data.StatutoryCategoryCode),
            //                          new XElement("CountryCode", data.CountryCode),
            //                          new XElement("BillingTypesCode", data.BillingTypesCode),
            //                          new XElement("ContractTypeCode", data.ContractTypeCode),
            //                          new XElement("ContractStatusCode", data.ContractStatusCode),
            //                          new XElement("SmartViewCode", data.SmartViewCode),
            //                          new XElement("GroupingParametersCode", data.GroupingParametersCode),
            //                          new XElement("ManagementCategoryCode", data.ManagementCategoryCode),
            //                           new XElement("ClubbingParameterCode", data.ClubbingParameterCode),
            //                          new XElement("Notes", data.NOTES),
            //                          new XElement("Message", data.Message),
            //                          new XElement("ProjectStatus", data.ProjectStatus),
            //                          new XElement("RecordStatus", data.RecordStatus),
            //                          new XElement("UserName", userName)

            //                       ));
            //}

            //if (listProjectUploadLog.Count > 0)
            //{
            //    xmlError = new XElement("Logs",
            //          from data in listProjectUploadLog
            //          select new XElement("Log",
            //                        new XElement("SessionID", data.SessionID),
            //                        new XElement("RowNumber", data.RowNumber),
            //                        new XElement("ColumnNumber", data.ColumnNumber),
            //                        new XElement("Message", data.Message),
            //                        new XElement("UserName", userName),
            //                        new XElement("ErrorCode", data.ErrorCode)
            //                       ));
            //}
            //new ProjectRepository().BulkUploadProjectData(xml, xmlError);
            //response.SessionId = sessionId;
            //response.ErrorCount = listProjectUploadLog.Count();
            //return response;


        }

        public List<ProjectUploadLog> GetProjectErrorLog(string sessionId)
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.GetProjectErrorLog(sessionId);
        }

        public List<Project> GetAllProjectByClubParam(string clubParam)
        {
            ProjectRepository _projectRepo = new ProjectRepository();
            return _projectRepo.GetAllProjectByClubParam(clubParam);
        }
    }
}
