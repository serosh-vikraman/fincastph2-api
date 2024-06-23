using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Models
{
    public class REPExtractReportData
    {
        public string ProjectEntityCode { get; set; }
        public string ProjectEntityName { get; set; }
        public string ProjectSegmentCode { get; set; }
        public string ProjectSegmentName { get; set; }
        public string SmartViewCode { get; set; }
        public string SmartViewName { get; set; }
        public string BUCategoryCode { get; set; }
        public string BUCategoryName { get; set; }
        public int ScenarioDataTypeID { get; set; }
        public int Year { get; set; }
        public decimal Q1New { get; set; }
        public decimal Q2New { get; set; }
        public decimal Q3New { get; set; }
        public decimal Q4New { get; set; }
    }

    public class REPExtractReport
    {
        public List<REPExtractReportData> REPExtractData { get; set; }
        public Scenario Scenario { get; set; }
        public List<REPExtractReportData> CurrForeCast { get; set; }
        public List<REPExtractReportData> FutureForeCast { get; set; }
    }
}
