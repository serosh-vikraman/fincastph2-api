using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class VarianceAnalysisReport
    {
        public List<VarianceAnalysisReportData> VarianceAnalysisReportDatas { get; set; }
        public List<Scenario> Scenarios { get; set; }
        public List<FinancialDataTypeMaster> FinancialDataTypes { get; set; }
    }
    public class VarianceAnalysisReportData
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string IFSProjectCode { get; set; }
        public string ProjectEntityName { get; set; }
        public string ProjectSegmentName { get; set; }
        public string BUCategoryName { get; set; }
        public string ContractStatusName { get; set; }
        public string ContractTypeName { get; set; }
        
        //Revenue - BaseScenario
        public decimal RV_S1Q1 { get; set; }
        public decimal RV_S1Q2 { get; set; }
        public decimal RV_S1Q3 { get; set; }
        public decimal RV_S1Q4 { get; set; }
        //Revenue - CompareScenario A
        public decimal RV_S2Q1 { get; set; }
        public decimal RV_S2Q2 { get; set; }
        public decimal RV_S2Q3 { get; set; }
        public decimal RV_S2Q4 { get; set; }
        //Revenue - CompareScenario B
        public decimal RV_S3Q1 { get; set; }
        public decimal RV_S3Q2 { get; set; }
        public decimal RV_S3Q3 { get; set; }
        public decimal RV_S3Q4 { get; set; }

        //Gross Margin - BaseScenario
        public decimal GM_S1Q1 { get; set; }
        public decimal GM_S1Q2 { get; set; }
        public decimal GM_S1Q3 { get; set; }
        public decimal GM_S1Q4 { get; set; }
        //Gross Margin - CompareScenario A
        public decimal GM_S2Q1 { get; set; }
        public decimal GM_S2Q2 { get; set; }
        public decimal GM_S2Q3 { get; set; }
        public decimal GM_S2Q4 { get; set; }
        //Gross Margin - CompareScenario B
        public decimal GM_S3Q1 { get; set; }
        public decimal GM_S3Q2 { get; set; }
        public decimal GM_S3Q3 { get; set; }
        public decimal GM_S3Q4 { get; set; }
    }
}
