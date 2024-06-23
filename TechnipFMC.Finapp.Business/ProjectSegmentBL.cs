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
    public class ProjectSegmentBL : IProjectSegmentBL
    {
        public ProjectSegmentBL(IProjectSegmentRepository projectsegmentrepo)
        {
            //_countryRepo = countryRepo;
        }
        public ProjectSegmentBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ProjectSegmentRepository _projectsegmentrepo = new ProjectSegmentRepository();
            return _projectsegmentrepo.Delete(Id, Deletedby);
        }

        public IEnumerable<ProjectSegment> GetAll()
        {
            ProjectSegmentRepository _projectsegmentrepo = new ProjectSegmentRepository();
            return _projectsegmentrepo.GetAll();
        }

        public ProjectSegment GetById(int Id)
        {
            ProjectSegmentRepository _projectsegmentrepo = new ProjectSegmentRepository();
            return _projectsegmentrepo.GetById(Id);
        }

        public ProjectSegment Save(ProjectSegment projectsegment)
        {
            ProjectSegmentRepository _projectsegmentrepo = new ProjectSegmentRepository();
            return _projectsegmentrepo.Save(projectsegment);
        }
        public ProjectSegment Update(ProjectSegment projectsegment)
        {
            ProjectSegmentRepository _projectsegmentrepo = new ProjectSegmentRepository();
            return _projectsegmentrepo.Update(projectsegment);
        }
    }
}
