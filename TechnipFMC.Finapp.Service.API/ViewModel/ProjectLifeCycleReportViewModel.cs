using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ProjectLifeCycleReportViewModel
    {
        public int ProjectID { get; set; }
        public int ScenarioDataTypeID { get; set; }
        public string IFSProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ScenarioName { get; set; }
        public int Year { get; set; }
        public decimal Q1New { get; set; }
        public decimal Q2New { get; set; }
        public decimal Q3New { get; set; }
        public decimal Q4New { get; set; }
        public decimal Total { get; set; }
    }
}