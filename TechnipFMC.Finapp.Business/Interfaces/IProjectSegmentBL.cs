using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IProjectSegmentBL
    {
        IEnumerable<ProjectSegment> GetAll();
        ProjectSegment Save(ProjectSegment projectsegment);
        ProjectSegment GetById(int Id);

        ProjectSegment Update(ProjectSegment projectsegment);

        bool Delete(int Id, string Deletedby);
    }
}
