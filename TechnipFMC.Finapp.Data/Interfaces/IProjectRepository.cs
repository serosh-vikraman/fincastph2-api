using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAll(int departmentId);
        IEnumerable<Projects> GetAllProjects();
        IEnumerable<ProjectsToLink> GetAllProjectsOfScenario(int scenarioId);

        int GetAllInactiveProjectsOfScenario(int scenarioId);
        Project Save(Project project);
        Project GetById(int Id);
        bool Delete(int Id, string DeletedBy);
        bool UploadProjectData(string sessionId, string fileName, string filePath, string userName);

        bool BulkUploadProjectData(XElement tempProject, XElement projectErrorLog);
        List<ProjectUploadLog> GetProjectErrorLog(string sessionId);
        bool IsProjectExist(string projectName);
        List<Project> GetAllProjectByClubParam(string clubParam);
    }
}
