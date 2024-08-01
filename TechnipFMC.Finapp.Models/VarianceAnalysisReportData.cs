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
        public Currency Currency { get; set; }
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
        public decimal RV_S1Q5 { get; set; }
        public decimal RV_S1Q6 { get; set; }
        public decimal RV_S1Q7 { get; set; }
        public decimal RV_S1Q8 { get; set; }
        public decimal RV_S1Q9 { get; set; }
        public decimal RV_S1Q10 { get; set; }
        public decimal RV_S1Q11 { get; set; }
        public decimal RV_S1Q12 { get; set; }

        //Revenue - CompareScenario A
        public decimal RV_S2Q1 { get; set; }
        public decimal RV_S2Q2 { get; set; }
        public decimal RV_S2Q3 { get; set; }
        public decimal RV_S2Q4 { get; set; }
        public decimal RV_S2Q5 { get; set; }
        public decimal RV_S2Q6 { get; set; }
        public decimal RV_S2Q7 { get; set; }
        public decimal RV_S2Q8 { get; set; }
        public decimal RV_S2Q9 { get; set; }
        public decimal RV_S2Q10 { get; set; }
        public decimal RV_S2Q11 { get; set; }
        public decimal RV_S2Q12 { get; set; }

        //Revenue - CompareScenario B
        public decimal RV_S3Q1 { get; set; }
        public decimal RV_S3Q2 { get; set; }
        public decimal RV_S3Q3 { get; set; }
        public decimal RV_S3Q4 { get; set; }
        public decimal RV_S3Q5 { get; set; }
        public decimal RV_S3Q6 { get; set; }
        public decimal RV_S3Q7 { get; set; }
        public decimal RV_S3Q8 { get; set; }
        public decimal RV_S3Q9 { get; set; }
        public decimal RV_S3Q10 { get; set; }
        public decimal RV_S3Q11 { get; set; }
        public decimal RV_S3Q12 { get; set; }

        //Gross Margin - BaseScenario
        public decimal GM_S1Q1 { get; set; }
        public decimal GM_S1Q2 { get; set; }
        public decimal GM_S1Q3 { get; set; }
        public decimal GM_S1Q4 { get; set; }
        public decimal GM_S1Q5 { get; set; }
        public decimal GM_S1Q6 { get; set; }
        public decimal GM_S1Q7 { get; set; }
        public decimal GM_S1Q8 { get; set; }
        public decimal GM_S1Q9 { get; set; }
        public decimal GM_S1Q10 { get; set; }
        public decimal GM_S1Q11 { get; set; }
        public decimal GM_S1Q12 { get; set; }

        //Gross Margin - CompareScenario A
        public decimal GM_S2Q1 { get; set; }
        public decimal GM_S2Q2 { get; set; }
        public decimal GM_S2Q3 { get; set; }
        public decimal GM_S2Q4 { get; set; }
        public decimal GM_S2Q5 { get; set; }
        public decimal GM_S2Q6 { get; set; }
        public decimal GM_S2Q7 { get; set; }
        public decimal GM_S2Q8 { get; set; }
        public decimal GM_S2Q9 { get; set; }
        public decimal GM_S2Q10 { get; set; }
        public decimal GM_S2Q11 { get; set; }
        public decimal GM_S2Q12 { get; set; }

        //Gross Margin - CompareScenario B
        public decimal GM_S3Q1 { get; set; }
        public decimal GM_S3Q2 { get; set; }
        public decimal GM_S3Q3 { get; set; }
        public decimal GM_S3Q4 { get; set; }
        public decimal GM_S3Q5 { get; set; }
        public decimal GM_S3Q6 { get; set; }
        public decimal GM_S3Q7 { get; set; }
        public decimal GM_S3Q8 { get; set; }
        public decimal GM_S3Q9 { get; set; }
        public decimal GM_S3Q10 { get; set; }
        public decimal GM_S3Q11 { get; set; }
        public decimal GM_S3Q12 { get; set; }
    }

}
