﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class BUCategoryViewModel
    {
        public int BUCategoryID { get; set; }
        public string BUCategoryCode { get; set; }
        public string BUCategoryName { get; set; }
        public bool Active { get; set; }

        public int CreatedBy { get; set; }
        public string Status { get; set; }

    }
}