using TechnipFMC.Finapp.Models;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TechnipFMC.Finapp.Data.Interfaces
{
    public interface IScenarioFileRepository
    {
        void UploadProjectScenarioDataType_1(string activeQuarters, XElement xml);
        void UploadProjectScenarioDataType_2(string activeQuarter, XElement xml);
        List<TemplateConfiguration> GetTemplateConfigurations(int typeId);
        bool SaveScenarioFile(ScenarioFile scenarioFile, string createdBy);
        List<ScenarioUploadLog> GetScenarioErrorlog(string sessionid);
        List<ScenarioUploadLog> GetScenarioErrorlogByScenarioId(int ScenarioId);
        List<ScenarioMappedProjects> GetScenarioMappedProjects(int scenarioId);
        bool SaveScenarioUploadLog(XElement xml);
        List<FileUploadLayout> GetFileUploadLayouts(int scenarioId);
        List<ScenarioFile> GetScenariouploadlog();
        List<ScenarioUploadLog> GetScenarioFailedlog(string sessionid);
    }
}
