﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class ClubbingParameterViewModel
    {
        public int ClubbingParameterID { get; set; }
        public string ClubbingParameterCode { get; set; }
        public string ClubbingParameterName { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
    }
}