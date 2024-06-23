using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IGroupingParametersBL
    {
        IEnumerable<GroupingParameters> GetAll();
        GroupingParameters Save(GroupingParameters groupingParameters);
        GroupingParameters GetById(int Id);

        GroupingParameters Update(GroupingParameters groupingParameters);

        bool Delete(int Id, string Deletedby);
    }
}
