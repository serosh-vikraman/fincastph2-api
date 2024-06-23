using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class LockQuarterViewModel
    {
        public int Id  { get; set; }
        public int Year { get; set; }
        public string Quarter { get; set; }
        public bool Lock { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}