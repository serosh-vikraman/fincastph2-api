using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ProjectSegmentViewModel
    {
        public int ProjectSegmentID { get; set; }
        public string ProjectSegmentCode { get; set; }
        public string ProjectSegmentName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}