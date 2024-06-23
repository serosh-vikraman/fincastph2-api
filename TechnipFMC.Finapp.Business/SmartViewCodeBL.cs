using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class SmartViewCodeBL : ISmartViewCodeBL
    {
         
        public SmartViewCodeBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            SmartViewCodeRepository _smartViewCodeRepo = new SmartViewCodeRepository();
            return _smartViewCodeRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<SmartViewCodeMaster> GetAll()
        {
            SmartViewCodeRepository _smartViewCodeRepo = new SmartViewCodeRepository();
            return _smartViewCodeRepo.GetAll();
        }

        public SmartViewCodeMaster GetById(int id)
        {
            SmartViewCodeRepository _smartViewCodeRepo = new SmartViewCodeRepository();
            return _smartViewCodeRepo.GetById(id);
        }

        public SmartViewCodeMaster Save(SmartViewCodeMaster smartviewCode)
        {
            SmartViewCodeRepository _smartViewCodeRepo = new SmartViewCodeRepository();
            return _smartViewCodeRepo.Save(smartviewCode);
        }
        public SmartViewCodeMaster Update(SmartViewCodeMaster smartviewCodes)
        {
            throw new NotImplementedException();
        }
    }
}
