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
    public class ProjectEntityBL : IProjectEntityBL
    {
       public ProjectEntityBL(IProjectEntityRepository projectEntityrepo)
        {
            //_countryRepo = countryRepo;
        }
        public ProjectEntityBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ProjectEntityRepository _projectentityRepo = new ProjectEntityRepository();
            return _projectentityRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<ProjectEntity> GetAll()
        {
            ProjectEntityRepository _projectentityRepo = new ProjectEntityRepository();
            return _projectentityRepo.GetAll();
        }

        public ProjectEntity GetById(int Id)
        {
            ProjectEntityRepository _projectentityRepo = new ProjectEntityRepository();
            return _projectentityRepo.GetById(Id);
        }

        public ProjectEntity Save(ProjectEntity projectEntity)
        {
            ProjectEntityRepository _projectentityRepo = new ProjectEntityRepository();
            return _projectentityRepo.Save(projectEntity);
        }
        public ProjectEntity Update(ProjectEntity projectEntity)
        {
            ProjectEntityRepository _projectentityRepo = new ProjectEntityRepository();
            return _projectentityRepo.Update(projectEntity);
        }
    }
}
