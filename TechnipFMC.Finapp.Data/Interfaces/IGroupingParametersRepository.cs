using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IGroupingParametersRepository
    {
        IEnumerable<GroupingParameters> GetAll();
        GroupingParameters Save(GroupingParameters groupingparameters);
        GroupingParameters GetById(int Id );

        GroupingParameters Update(GroupingParameters groupingparameters);

        bool Delete(int Id, string DeletedBy);
    }
}
