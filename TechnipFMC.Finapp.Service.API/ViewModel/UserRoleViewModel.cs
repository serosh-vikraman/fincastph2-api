using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class UserRoleViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
    public class UserProjectsViewModel
    {
        public int UserId { get; set; }
        public List<UserProjectViewModel> UserProjects { get; set; }
        public int CreatedBy { get; set; }
    }
    public class UserProjectViewModel
    {
        public int UserProjectId { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int CreatedBy { get; set; }
    }
    public class UserDepartmentViewModel 
    {
        public int UserId { get; set; }
        public List<int> DepartmentId { get; set; }
    }

    //public class ProjectUIModel
    //{
    //    public List<CountryViewModel> Countries { get; set; }
    //    public List<BUCategoryViewModel> BUCategories { get; set; }

    //}
}