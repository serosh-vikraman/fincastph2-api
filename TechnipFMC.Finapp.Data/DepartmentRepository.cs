using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;

namespace TechnipFMC.Finapp.Data
{
    public class DepartmentRepository : BaseRepository, IDepartmentRepository
    {
        public DepartmentRepository()
        { }
        public bool Delete(int Id, int DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDepartmentMaster";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                cmd.Parameters.AddWithValue("@P_UserId", DeletedBy);
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

        public IEnumerable<Department> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllDepartment";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Department> obj = new List<Department>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Department department = new Department();
                    department.DepartmentID = Convert.ToInt32(results.Rows[i]["DepartmentID"]);
                    department.DepartmentName = results.Rows[i]["DepartmentName"].ToString().Decrypt(); ;
                    department.DepartmentCode = results.Rows[i]["DepartmentCode"].ToString();
                    department.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    department.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    department.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(department);
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

        public Department GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllDepartment";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new Department()
                        {
                            DepartmentID = (int)reader["DepartmentID"],
                            DepartmentName = ((string)reader["DepartmentName"]).Decrypt(),
                            DepartmentCode = (string)reader["DepartmentCode"],
                            CreatedBy = (int)reader["CreatedBy"],
                            Active = (bool)reader["Active"],
                            Status = (string)reader["Status"],
                        };
                    }
                }

                return null;
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

        public Department Save(Department Department)
        {
            try
            {
                Department.DepartmentName = Department.DepartmentName.Encrypt();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveDepartment";
                cmd.Parameters.AddWithValue("@P_Id", Department.DepartmentID);
                cmd.Parameters.AddWithValue("@P_DepartmentName", Department.DepartmentName);
                cmd.Parameters.AddWithValue("@P_DepartmentCode", Department.DepartmentCode);
                cmd.Parameters.AddWithValue("@P_Active", Department.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", Department.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Department.DepartmentID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return Department;
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

        public Department Update(Department Department)
        {
            throw new NotImplementedException();
        }
    }
}
