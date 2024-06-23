using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ManagementCategoryViewModel
    {
        public int ManagementCategoryID { get; set; }
        public string ManagementCategoryCode { get; set; }
        public string ManagementCategoryName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}