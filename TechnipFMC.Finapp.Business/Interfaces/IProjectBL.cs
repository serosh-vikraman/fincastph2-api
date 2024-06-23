using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IProjectBL
    {
        IEnumerable<Project> GetAll(int departmentId);
        IEnumerable<Projects> GetAllProjects();
        IEnumerable<ProjectsToLink> GetAllProjectsOfScenario(int scenarioId);
        int GetAllInactiveProjectsOfScenario(int scenarioId);
        IEnumerable<ProjectDeLink> GetAllProjectsToLinkForLifeCycleReport();
        Project Save(Project project);
        Project GetById(int Id);
        bool Delete(int Id, string Deletedby);
        byte[] DownloadProjectTemplate();

        ProjectFileUploadResponse UploadProjectData(string sessionId, string fileName, string filePath, string userName);
        List<ProjectUploadLog> GetProjectErrorLog(string sessionId);

        List<Project> GetAllProjectByClubParam(string clubParam);

    }
}
