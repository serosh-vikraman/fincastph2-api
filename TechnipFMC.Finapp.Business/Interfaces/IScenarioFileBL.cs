using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business.Interfaces
{
    public interface IScenarioFileBL
    {
        List<ScenarioUploadLog> GetScenarioErrorlog(string sessionid);
        List<ScenarioUploadLog> GetScenarioErrorlogByScenarioId(int ScenarioId);
        ScenarioFileUploadResponse UploadProjectScenarioDataType_1_New(int scenarioId, string activeQuarters, string fileName, string filePath, string userName,int customerId,int departmentId, int clientId);
        ScenarioFileUploadResponse UploadProjectScenarioDataType_2_New(int scenarioId, string activeQuarters, string fileName, string filePath, string userName, int customerId,int departmentId, int clientId);
        ScenarioFileUploadResponse UploadProjectScenarioData(int scenarioId, string activeQuarters, string fileName, string filePath, string userName,int customerId, int departmentId, int clientId);
        List<FileUploadLayout> GetFileUploadLayout(int scenarioId);
        List<ScenarioFile> GetScenariouploadlog();
        List<ScenarioUploadLog> GetScenarioFailedlog(string sessionid);
        string GetuploadToken(string uploadsessionId,string CreatedBy,int secondtime);
        ScenarioFile GetuploadFile(string token);
    }
}
