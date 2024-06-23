using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class QuarterlyData : BaseEntity
    {
        public int ScenarioId { get; set; }

        public List<ProjectDataList> ProjectDataLists { get; set; }
        
    }
    public class ProjectDataList {

        public int ProjectId { get; set; }
        public string Comments { get; set; }
        public List<QuarterlyDataList> QuarterlyDataLists { get; set; }
    }
    public class QuarterlyDataList { 
        public int ScenarioDataTypeId { get; set; }
        public int Year { get; set; }
        public decimal? Q1New { get; set; }
        public decimal? Q1Variant { get; set; }
        public decimal? Q2New { get; set; }
        public decimal? Q2Variant { get; set; }
        public decimal? Q3New { get; set; }
        public decimal? Q3Variant { get; set; }
        public decimal? Q4New { get; set; }
        public decimal? Q4Variant { get; set; }
        
    }
}
