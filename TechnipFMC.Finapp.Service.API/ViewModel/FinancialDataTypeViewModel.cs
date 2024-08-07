using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class FinancialDataTypeViewModel
    {
        public int FinancialDataTypeID { get; set; }
        public string FinancialDataTypeCode { get; set; }
        public string FinancialDataTypeName { get; set; }
        public string Indicator { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
        public bool Active { get; set; }

    }
    
        public class FinancialDataTypeMappingViewModel
    {
        public string Scope { get; set; }
        public string Type { get; set; }
        public string[] Financialdatatype { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }

    }
    public class FinancialDataTypesView
    {
        public string FinancialDataTypeCode  { get; set; }
        public string FinancialDataTypeName { get; set; }
    }
}