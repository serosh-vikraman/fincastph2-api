using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class RolePermissionViewModel
    {
        public int Id { get; set; }
        public string RoleCode { get; set; }
        public string ActionCode { get; set; }
        public string SubActionCode { get; set; }
        public int CreatedBy { get; set; }

        public bool Active { get; set; }
        public string Status { get; set; }
    }
}