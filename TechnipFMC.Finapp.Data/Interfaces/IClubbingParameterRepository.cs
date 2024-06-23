using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IClubbingParameterRepository
    {
        IEnumerable<ClubbingParameter> GetAll();
        ClubbingParameter Save(ClubbingParameter ClubbingParameter);
        ClubbingParameter GetById(int Id );

        ClubbingParameter Update(ClubbingParameter ClubbingParameter);

        bool Delete(int Id, string DeletedBy);
    }
}
