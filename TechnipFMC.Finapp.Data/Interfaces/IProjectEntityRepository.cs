using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IProjectEntityRepository
    {
        IEnumerable<ProjectEntity> GetAll();
        ProjectEntity Save(ProjectEntity projectentity);
        ProjectEntity GetById(int Id );

        ProjectEntity Update(ProjectEntity projectentity);

        bool Delete(int Id, string DeletedBy);
       
    }
}
