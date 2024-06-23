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
    public class ClubbingParameterBL:IClubbingParameterBL
    {
        public ClubbingParameterBL(IClubbingParameterRepository clubbingParameterrepo)
        {
            //_countryRepo = countryRepo;
        }
        public ClubbingParameterBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ClubbingParameterRepository _clubbingParameterrepo = new ClubbingParameterRepository();
            return _clubbingParameterrepo.Delete(Id, Deletedby);
        }


        public IEnumerable<ClubbingParameter> GetAll()
        {
            ClubbingParameterRepository _clubbingParameterrepo = new ClubbingParameterRepository();
            return _clubbingParameterrepo.GetAll();
        }

        public ClubbingParameter GetById(int id)
        {
            ClubbingParameterRepository _clubbingParameterrepo = new ClubbingParameterRepository();
            return _clubbingParameterrepo.GetById(id );
        }

        public ClubbingParameter Save(ClubbingParameter clubbingParameter)
        {
            ClubbingParameterRepository _clubbingParameterrepo = new ClubbingParameterRepository();
            return _clubbingParameterrepo.Save(clubbingParameter);
        }
        public ClubbingParameter Update(ClubbingParameter clubbingParameter)
        {
            throw new NotImplementedException();
        }
    }
}
