using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ProjectLifeCycleReportData
    {
        public int ProjectID { get; set; }
        public int ScenarioDataTypeID { get; set; }
        public string ProjectName { get; set; }
        public string ScenarioName { get; set; }
        public int Year { get; set; }
        public decimal Q1New { get; set; }
        public decimal Q2New { get; set; }
        public decimal Q3New { get; set; }
        public decimal Q4New { get; set; }
        public decimal Total { get; set; }
    }
    public class ProjectLifeCycleReport
    {
        public List<ProjectLifeCycleReportData> Actuals { get; set; }
        public List<ProjectLifeCycleReportData> ForeCast { get; set; }
    }
}
