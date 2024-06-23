using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;
using System.Xml.Linq;

namespace TechnipFMC.Finapp.Data
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteProject";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                cmd.Parameters.AddWithValue("@P_User", DeletedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public IEnumerable<Project> GetAll( int departmentId)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllProject";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                //cmd.Parameters.AddWithValue("@P_ClientId", clientId);
                cmd.Parameters.AddWithValue("@P_DepartmentId", departmentId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Project> obj = new List<Project>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Project projects = new Project();
                    projects.ProjectID = Convert.ToInt32(results.Rows[i]["ProjectID"]);
                    projects.DepartmentCode = results.Rows[i]["DepartmentCode"].ToString();
                    projects.ClientCode = results.Rows[i]["ClientCode"].ToString();
                    //projects.IFSProjectCode = results.Rows[i]["IFSProjectCode"].ToString();
                    projects.ProjectCode = results.Rows[i]["ProjectCode"].ToString();
                    projects.ProjectName = results.Rows[i]["ProjectName"].ToString();
                    projects.ProjectSegmentCode = results.Rows[i]["ProjectSegmentCode"].ToString();
                    projects.ProjectEntityCode = results.Rows[i]["ProjectEntityCode"].ToString();
                    projects.BUCategoryCode = results.Rows[i]["BUCategoryCode"].ToString();
                    projects.StatutoryCategoryCode = results.Rows[i]["StatutoryCategoryCode"].ToString();
                    projects.CountryCode = results.Rows[i]["CountryCode"].ToString();
                    projects.BillingTypesCode = results.Rows[i]["BillingTypesCode"].ToString();
                    projects.ContractTypeCode = results.Rows[i]["ContractTypeCode"].ToString();
                    projects.ContractStatusCode = results.Rows[i]["ContractStatusCode"].ToString();
                    projects.SmartViewCode = results.Rows[i]["SmartViewCode"].ToString();
                    projects.GroupingParametersCode = results.Rows[i]["GroupingParametersCode"].ToString();
                    projects.ManagementCategoryCode = results.Rows[i]["ManagementCategoryCode"].ToString();
                    projects.ClubbingParameterCode = results.Rows[i]["ClubbingParameterCode"].ToString();
                    projects.NOTES = results.Rows[i]["NOTES"].ToString();
                    projects.ProjectStatus = results.Rows[i]["ProjectStatus"].ToString();
                    projects.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    projects.MappingCount = Convert.ToInt32(results.Rows[i]["MappingCount"]);

                    obj.Add(projects);
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public IEnumerable<Projects> GetAllProjects()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllProjects";
                //cmd.Parameters.AddWithValue("@P_ClientId", clientId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Projects> obj = new List<Projects>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Projects projects = new Projects();
                    projects.ProjectId = Convert.ToInt32(results.Rows[i]["ProjectID"]);
                    projects.ProjectCode = results.Rows[i]["ProjectCode"].ToString();
                    projects.ProjectName = results.Rows[i]["ProjectName"].ToString();
                    
                    obj.Add(projects);
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public Project GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetProjectById";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                //cmd.Parameters.AddWithValue("@P_ClientId", clientId);
                //cmd.Parameters.AddWithValue("@P_DepartmentId", departmentId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();


                Project projects = new Project();
                projects.ProjectID = Convert.ToInt32(results.Rows[0]["ProjectID"]);
                //projects.ProjectType = results.Rows[0]["ProjectType"].ToString();
                projects.DepartmentCode = results.Rows[0]["DepartmentCode"].ToString();
                projects.ClientCode = results.Rows[0]["ClientCode"].ToString();
                projects.ProjectCode = results.Rows[0]["ProjectCode"].ToString();
                //projects.ManualProjectCode = results.Rows[0]["ManualProjectCode"].ToString();
                projects.ProjectName = results.Rows[0]["ProjectName"].ToString();
                projects.ProjectSegmentCode = results.Rows[0]["ProjectSegmentCode"].ToString();
                projects.ProjectEntityCode = results.Rows[0]["ProjectEntityCode"].ToString();
                projects.BUCategoryCode = results.Rows[0]["BUCategoryCode"].ToString();
                projects.StatutoryCategoryCode = results.Rows[0]["StatutoryCategoryCode"].ToString();
                projects.CountryCode = results.Rows[0]["CountryCode"].ToString();
                projects.BillingTypesCode = results.Rows[0]["BillingTypesCode"].ToString();
                projects.ContractTypeCode = results.Rows[0]["ContractTypeCode"].ToString();
                projects.ContractStatusCode = results.Rows[0]["ContractStatusCode"].ToString();
                projects.SmartViewCode = results.Rows[0]["SmartViewCode"].ToString();
                projects.GroupingParametersCode = results.Rows[0]["GroupingParametersCode"].ToString();
                projects.ManagementCategoryCode = results.Rows[0]["ManagementCategoryCode"].ToString();
                projects.ClubbingParameterCode = results.Rows[0]["ClubbingParameterCode"].ToString();
                projects.NOTES = results.Rows[0]["NOTES"].ToString();
                if (results.Rows[0]["ProjectStatus"].ToString() == "Active")
                {
                    projects.ActiveFlag = true;
                }
                else
                {
                    projects.ActiveFlag = false;
                }

                projects.CreatedBy = Convert.ToInt32(results.Rows[0]["CreatedBy"]);
                projects.MappingCount = Convert.ToInt32(results.Rows[0]["MappingCount"]);

                return projects;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }

        }

        public Project Save(Project project)
        {
            try
            {
                project.ProjectStatus = project.ActiveFlag == true ? "Active" : "Inactive";
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveProject";
                cmd.Parameters.AddWithValue("@P_Id", project.ProjectID);
     
                if (project.DepartmentCode != null && project.DepartmentCode != "" && project.DepartmentCode != "null")
                    cmd.Parameters.AddWithValue("@P_DepartmentCode", project.DepartmentCode);
                else
                    cmd.Parameters.AddWithValue("@P_DepartmentCode", DBNull.Value);
                if (project.ClientCode != null && project.ClientCode != "" && project.ClientCode != "null")
                    cmd.Parameters.AddWithValue("@P_ClientCode", project.ClientCode);
                else
                    cmd.Parameters.AddWithValue("@P_ClientCode", DBNull.Value);
                if (project.ProjectCode != null && project.ProjectCode != "")
                    cmd.Parameters.AddWithValue("@P_ProjectCode", project.ProjectCode);
                else 
                    cmd.Parameters.AddWithValue("@P_ProjectCode", DBNull.Value);
                cmd.Parameters.AddWithValue("@P_ProjectName", project.ProjectName);
                if (project.ProjectSegmentCode != null && project.ProjectSegmentCode != "" && project.ProjectSegmentCode != "null")
                    cmd.Parameters.AddWithValue("@P_ProjectSegmentCode", project.ProjectSegmentCode);
                else
                    cmd.Parameters.AddWithValue("@P_ProjectSegmentCode", DBNull.Value);
                if (project.ProjectEntityCode != null && project.ProjectEntityCode != "" && project.ProjectEntityCode != "null")
                    cmd.Parameters.AddWithValue("@P_ProjectEntityCode", project.ProjectEntityCode);
                else
                    cmd.Parameters.AddWithValue("@P_ProjectEntityCode", DBNull.Value);
                if (project.BUCategoryCode != null && project.BUCategoryCode != "" && project.BUCategoryCode != "null")
                    cmd.Parameters.AddWithValue("@P_BUCategoryCode", project.BUCategoryCode);
                else
                    cmd.Parameters.AddWithValue("@P_BUCategoryCode", DBNull.Value);
                if (project.StatutoryCategoryCode != null && project.StatutoryCategoryCode != "" && project.StatutoryCategoryCode != "null")
                    cmd.Parameters.AddWithValue("@P_StatutoryCategoryCode", project.StatutoryCategoryCode);
                else
                    cmd.Parameters.AddWithValue("@P_StatutoryCategoryCode", DBNull.Value);
                if (project.CountryCode != null && project.CountryCode != "" && project.CountryCode != "null")
                    cmd.Parameters.AddWithValue("@P_CountryCode", project.CountryCode);
                else
                    cmd.Parameters.AddWithValue("@P_CountryCode", DBNull.Value);
                if (project.BillingTypesCode != null && project.BillingTypesCode != "" && project.BillingTypesCode != "null")
                    cmd.Parameters.AddWithValue("@P_BillingTypesCode", project.BillingTypesCode);
                else
                    cmd.Parameters.AddWithValue("@P_BillingTypesCode", DBNull.Value);
                if (project.ContractTypeCode != null && project.ContractTypeCode != "" && project.ContractTypeCode != "null")
                    cmd.Parameters.AddWithValue("@P_ContractTypeCode", project.ContractTypeCode);
                else
                    cmd.Parameters.AddWithValue("@P_ContractTypeCode", DBNull.Value);
                if (project.ContractStatusCode != null && project.ContractStatusCode != "" && project.ContractStatusCode != "null")
                    cmd.Parameters.AddWithValue("@P_ContractStatusCode", project.ContractStatusCode);
                else
                    cmd.Parameters.AddWithValue("@P_ContractStatusCode", DBNull.Value);
                if (project.SmartViewCode != null && project.SmartViewCode != "" && project.SmartViewCode != "null")
                    cmd.Parameters.AddWithValue("@P_SmartViewCode", project.SmartViewCode);
                else
                    cmd.Parameters.AddWithValue("@P_SmartViewCode", DBNull.Value);
                if (project.GroupingParametersCode != null && project.GroupingParametersCode != "" && project.GroupingParametersCode != "null")
                    cmd.Parameters.AddWithValue("@P_GroupingParametersCode", project.GroupingParametersCode);
                else
                    cmd.Parameters.AddWithValue("@P_GroupingParametersCode", DBNull.Value);
                if (project.ManagementCategoryCode != null && project.ManagementCategoryCode != "" && project.ManagementCategoryCode != "null")
                    cmd.Parameters.AddWithValue("@P_ManagementCategoryCode", project.ManagementCategoryCode);
                else
                    cmd.Parameters.AddWithValue("@P_ManagementCategoryCode", DBNull.Value);
                if (project.ClubbingParameterCode != null && project.ClubbingParameterCode != "" && project.ClubbingParameterCode != "null")
                    cmd.Parameters.AddWithValue("@P_ClubbingParameterCode", project.ClubbingParameterCode);
                else
                    cmd.Parameters.AddWithValue("@P_ClubbingParameterCode", DBNull.Value);
                cmd.Parameters.AddWithValue("@P_NOTES", (project.NOTES == null ? "" : project.NOTES));
                cmd.Parameters.AddWithValue("@P_ProjectStatus", project.ProjectStatus);
                cmd.Parameters.AddWithValue("@P_CreatedBy", project.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        project.Message = reader.GetString(0);
                        project.ProjectID = Convert.ToInt32(reader.GetInt32(1));
                    }
                }

                return project;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public IEnumerable<ProjectDeLink> GetAllProjectsToLinkForLifeCycleReport()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ProjectList_ProjectLifeCycleReport";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ProjectDeLink> obj = new List<ProjectDeLink>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ProjectDeLink projects = new ProjectDeLink();
                    projects.ProjectName = results.Rows[i]["ProjectName"].ToString();
                    projects.ProjectId =Convert.ToInt32(results.Rows[i]["ProjectId"].ToString());
                    obj.Add(projects);
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        } 
        
        public IEnumerable<ProjectsToLink> GetAllProjectsOfScenario(int scenarioId)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetProjectsOfScenario";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ProjectsToLink> obj = new List<ProjectsToLink>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ProjectsToLink projects = new ProjectsToLink();
                    projects.ProjectId = Convert.ToInt32(results.Rows[i]["ProjectId"]);
                    //projects.ProjectType = results.Rows[i]["ProjectType"].ToString();
                    projects.ProjectCode = results.Rows[i]["ProjectCode"].ToString();
                    //projects.ManualProjectCode = results.Rows[i]["ManualProjectCode"].ToString();
                    projects.ProjectName = results.Rows[i]["ProjectName"].ToString();
                    projects.ProjectStatus = results.Rows[i]["ProjectStatus"].ToString();
                    obj.Add(projects);
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }

        public int GetAllInactiveProjectsOfScenario(int scenarioId)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetInactiveProjectsOfScenario";
                cmd.Parameters.AddWithValue("@P_ScenarioId", scenarioId);
                SqlDataReader reader = cmd.ExecuteReader();
                int count = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }

        }

        public bool UploadProjectData(string sessionId, string fileName, string filePath, string userName)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveProjectFiles";
                cmd.Parameters.AddWithValue("@P_UploadSessionId", sessionId);
                cmd.Parameters.AddWithValue("@P_FileName", fileName);
                cmd.Parameters.AddWithValue("@P_FilePath", filePath);
                cmd.Parameters.AddWithValue("@P_CreatedBy", userName);
                SqlDataReader reader = cmd.ExecuteReader();
                base.DBConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool IsProjectExist(string projectName)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "IsProjectExist";
                cmd.Parameters.AddWithValue("@P_ProjectName", projectName);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var count = Convert.ToInt32(reader.GetInt32(0));
                        if (count == 0)
                            return false;
                        else
                            return true;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
        public bool BulkUploadProjectData(XElement tempProject, XElement projectErrorLog)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "BulkProjectUpload";
                cmd.Parameters.AddWithValue("@P_TempXml",Convert.ToString(tempProject));
                cmd.Parameters.AddWithValue("@P_ErrorXml", Convert.ToString(projectErrorLog));
                cmd.CommandTimeout = 60000;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //base.DBConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }

        public List<ProjectUploadLog> GetProjectErrorLog(string sessionId)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetProjectErrorLog";
                cmd.Parameters.AddWithValue("@P_SessionId", sessionId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<ProjectUploadLog> obj = new List<ProjectUploadLog>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ProjectUploadLog log = new ProjectUploadLog();
                    log.SessionID = results.Rows[i]["SessionID"].ToString();
                    log.RowNumber = Convert.ToInt32(results.Rows[i]["RowNumber"]);
                    log.ColumnNumber = Convert.ToInt32(results.Rows[i]["ColumnNumber"]);
                    log.Status = false;// Convert.ToBoolean(results.Rows[i]["Status"]);
                    log.Message = results.Rows[i]["Message"].ToString();
                    log.CreatedBy = results.Rows[i]["CreatedBy"].ToString();
                    log.ErrorCode = results.Rows[i]["ErrorCode"].ToString();
                    obj.Add(log);
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }

        public List<Project> GetAllProjectByClubParam(string clubParam)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetProjectByClubbingParam";
                cmd.Parameters.AddWithValue("@P_ClubParam", clubParam);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Project> obj = new List<Project>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Project projects = new Project();
                    projects.ProjectID = Convert.ToInt32(results.Rows[i]["ProjectID"]);
                    
                    projects.ProjectSegmentCode = results.Rows[i]["ProjectSegmentCode"].ToString();
                    projects.ProjectEntityCode = results.Rows[i]["ProjectEntityCode"].ToString();
                  
                    projects.ContractTypeCode = results.Rows[i]["ContractTypeCode"].ToString();
                    projects.ContractStatusCode = results.Rows[i]["ContractStatusCode"].ToString();
                   
                    projects.ManagementCategoryCode = results.Rows[i]["ManagementCategoryCode"].ToString();
                   
                    obj.Add(projects);
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}
