using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface ISmartViewCodeBL
    {
        IEnumerable<SmartViewCodeMaster> GetAll();
        SmartViewCodeMaster Save(SmartViewCodeMaster smartviewcode);
        SmartViewCodeMaster GetById(int Id);

        SmartViewCodeMaster Update(SmartViewCodeMaster smartviewcode);

        bool Delete(int Id, string Deletedby);
    }
}
