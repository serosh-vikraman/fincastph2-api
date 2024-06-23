using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IClubbingParameterBL
    {
        IEnumerable<ClubbingParameter> GetAll();
        ClubbingParameter Save(ClubbingParameter clubbingParameter);
        ClubbingParameter GetById(int Id );

        ClubbingParameter Update(ClubbingParameter clubbingParameter);

        bool Delete(int Id, string Deletedby);
    }
}
