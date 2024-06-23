using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ProjectViewModel
    {
        public int ProjectID { get; set; }
        public string DepartmentCode { get; set; }
        public string ClientCode { get; set; }
        //public string ProjectType { get; set; }
        public string ProjectCode { get; set; }
        //public string ManualProjectCode { get; set; }
       
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
        public int CreatedBy { get; set; }
        public int MappingCount { get; set; }
        public bool ActiveFlag { get; set; }
        public int CustomerID { get; set; }

    }
    public class ProjectsToLinkViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }

        //public string ManualProjectCode { get; set; }
        public string ProjectStatus { get; set; }

    }
    public class ProjectsViewModel
    {
        public int ProjectId { get; set; }

        //public string ProjectType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        //public string ManualProjectCode { get; set; }
        public string ProjectStatus { get; set; }

    }
    public class ProjectLifeCycleViewModel
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<ProjectLifeCycleViewModelData> ProjectLifeCycleRevenueData { get; set; }
        public List<ProjectLifeCycleViewModelData> ProjectLifeCycleGMData { get; set; }
    }

    public class ProjectLifeCycleViewModelData
    {
        public Int32 Year { get; set; }
        public decimal Amount { get; set; }
        public string YearText { get; set; }
        public string HeaderText { get; set; }
        public bool Active { get; set; }
    }
    
    public class ExtractDataDataViewModel
    {
        public string FieldName { get; set; }
        public string DataValue { get; set; }
    }
    public class ProjectLifeCycleDataViewModel
    {
        public List<ExtractHeaderViewModel> Header { get; set; }
        public List<ExtractDataDataViewModel> RVDataValue { get; set; }
        public List<ExtractDataDataViewModel> GMDataValue { get; set; }
    }
}