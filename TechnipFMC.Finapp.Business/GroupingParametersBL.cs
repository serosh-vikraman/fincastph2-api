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
    public class GroupingParametersBL : IGroupingParametersBL
    {
        public GroupingParametersBL(IGroupingParametersRepository groupingparametersrepo)
        {
            //_countryRepo = countryRepo;
        }
        public GroupingParametersBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            GroupingParametersRepository _groupingparametersrepo = new GroupingParametersRepository();
            return _groupingparametersrepo.Delete(Id, Deletedby);
        }


        public IEnumerable<GroupingParameters> GetAll()
        {
            GroupingParametersRepository _groupingparametersrepo = new GroupingParametersRepository();
            return _groupingparametersrepo.GetAll();
        }

        public GroupingParameters GetById(int id)
        {
            GroupingParametersRepository _groupingparametersrepo = new GroupingParametersRepository();
            return _groupingparametersrepo.GetById(id);
        }

        public GroupingParameters Save(GroupingParameters groupingparameters)
        {
            GroupingParametersRepository _groupingparametersrepo = new GroupingParametersRepository();
            return _groupingparametersrepo.Save(groupingparameters);
        }
        public GroupingParameters Update(GroupingParameters groupingparameters)
        {
            throw new NotImplementedException();
        }
    }
}
