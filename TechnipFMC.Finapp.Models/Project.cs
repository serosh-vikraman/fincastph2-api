using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class Project : BaseEntity
    {
        public int ProjectID { get; set; }
        public string DepartmentCode { get; set; }
        public string ClientCode { get; set; }
        public string ProjectType { get; set; }
        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }
        public string ProjectSegmentCode { get; set; }
        public string ProjectEntityCode { get; set; }
        public string BUCategoryCode { get; set; }
        public string StatutoryCategoryCode { get; set; }
        public string CountryCode { get; set; }
        public string BillingTypesCode { get; set; }
        public string ContractTypeCode { get; set; }
        public string ContractStatusCode { get; set; }
        public string SmartViewCode { get; set; }
        public string GroupingParametersCode { get; set; }
        public string ManagementCategoryCode { get; set; }
        public string ClubbingParameterCode { get; set; }
        public string NOTES { get; set; }
        public string ProjectStatus { get; set; }

        public int MappingCount { get; set; }
        public string Message { get; set; }
        public bool ActiveFlag { get; set; }
    }
    public class ProjectsToLink
    {
        public int ProjectId { get; set; }

        //public string ProjectType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        //public string ManualProjectCode { get; set; }
        public string ProjectStatus { get; set; }

    }
    public class Projects
    {
        public int ProjectId { get; set; }

        //public string ProjectType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        //public string ManualProjectCode { get; set; }
        public string ProjectStatus { get; set; }

    }
    public class ProjectLink
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string CreatedBy { get; set; }

    }
    public class ProjectDeLink
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
        public string CreatedBy { get; set; }


    }
    public class ProjectFileUploadResponse
    {
        public string SessionId { get; set; }
        public int ErrorCount { get; set; }
        public string Message { get; set; }
    }

    public class TempProject
    {

        public string SessionId { get; set; }
        public int RowNum { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public string ProjectCode { get; set; }
        public string ClubbingParameterName { get; set; }
        public string ClubbingParameterCode { get; set; }
        public string ProjectSegmentName { get; set; }
        public string ProjectEntityName { get; set; }
        public string BUCategoryName { get; set; }
        public string StatutoryCategoryName { get; set; }
        public string CountryName { get; set; }
        public string BillingTypesName { get; set; }
        public string ContractTypeName { get; set; }
        public string ContractStatusName { get; set; }
        public string SmartViewName { get; set; }
        public string GroupingParametersName { get; set; }
        public string ManagementCategoryName { get; set; }

        public string ProjectSegmentCode { get; set; }
        public string ProjectEntityCode { get; set; }
        public string BUCategoryCode { get; set; }
        public string StatutoryCategoryCode { get; set; }
        public string CountryCode { get; set; }
        public string BillingTypesCode { get; set; }
        public string ContractTypeCode { get; set; }
        public string ContractStatusCode { get; set; }
        public string SmartViewCode { get; set; }
        public string GroupingParametersCode { get; set; }
        public string ManagementCategoryCode { get; set; }
        public string NOTES { get; set; }
        public string Message { get; set; }
        public string ProjectStatus { get; set; }
        public string RecordStatus { get; set; }
    }
    public class ProjectLifeCycle
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<ProjectLifeCycleData> ProjectLifeCycleRevenueData { get; set; }
        public List<ProjectLifeCycleData> ProjectLifeCycleGMData { get; set; }
    }

    public class ProjectLifeCycleData
    {
        public Int32 Year { get; set; }
        public decimal Amount { get; set; }
        public string YearText { get; set; }
        public string HeaderText { get; set; }
        public bool Active { get; set; }
    }

}
