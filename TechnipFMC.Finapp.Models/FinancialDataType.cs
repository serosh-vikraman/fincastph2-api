using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class FinancialDataType : BaseEntity
    {
        public int FinancialDataTypeID { get; set; }
        public string FinancialDataTypeCode { get; set; }
        public string FinancialDataTypeName { get; set; }
        public string Indicator { get; set; }
        public string Message { get; set; }

    }

    public class FinancialDataTypeMaster
    {
        public int ScenarioDataTypeID { get; set; } 
        public string FinancialDataTypeName { get; set; }
    }
}
