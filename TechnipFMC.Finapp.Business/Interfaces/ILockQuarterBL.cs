﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface ILockQuarterBL
    {
        IEnumerable<LockQuarter> GetAll();
        LockQuarter Save(LockQuarter lockQuarter);
        LockQuarter GetById(int Id);        

        bool Delete(int Id, string DeletedBy);
    }
}
