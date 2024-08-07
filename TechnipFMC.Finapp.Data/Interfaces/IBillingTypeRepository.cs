﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IBillingTypeRepository
    {
        IEnumerable<BillingType> GetAll();
        BillingType Save(BillingType billingType);
        BillingType GetById(int Id);

        BillingType Update(BillingType billingType);

        bool Delete(int Id, string DeletedBy);
    }
}
