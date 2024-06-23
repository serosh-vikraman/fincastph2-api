using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class StatutoryCategoryViewModel
    {
        public int StatutoryCategoryID { get; set; }
        public string StatutoryCategoryCode { get; set; }
        public string StatutoryCategoryName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}