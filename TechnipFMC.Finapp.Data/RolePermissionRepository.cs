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
    public class RolePermissionRepository : BaseRepository, IRolePermissionRepository
    {
        public RolePermissionRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteRolePermission";
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
        public IEnumerable<RolePermission> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllRolePermission";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<RolePermission> obj = new List<RolePermission>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    RolePermission rolePermission = new RolePermission();
                    rolePermission.Id = Convert.ToInt32(results.Rows[i]["Id"]);
                    rolePermission.RoleCode = results.Rows[i]["RoleCode"].ToString();
                    rolePermission.ActionCode = results.Rows[i]["ActionCode"].ToString();
                    rolePermission.SubActionCode = results.Rows[i]["SubActionCode"].ToString();
                    rolePermission.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    rolePermission.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    rolePermission.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(rolePermission);
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
        public RolePermission GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllRolePermission";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new RolePermission()
                        {
                            Id = (int)reader["Id"],
                            RoleCode = (string)reader["RoleCode"],
                            ActionCode = (string)reader["ActionCode"],
                            SubActionCode = (string)reader["SubActionCode"],
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

        public RolePermission Save(RolePermission rolePermission)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveRolePermission";
                cmd.Parameters.AddWithValue("@P_Id", rolePermission.Id);
                cmd.Parameters.AddWithValue("@P_RoleCode", rolePermission.RoleCode);
                cmd.Parameters.AddWithValue("@P_ActionCode", rolePermission.ActionCode);
                cmd.Parameters.AddWithValue("@P_SubActionCode", rolePermission.SubActionCode);
                cmd.Parameters.AddWithValue("@P_CreatedBy", rolePermission.CreatedBy);
                cmd.Parameters.AddWithValue("@P_Active", rolePermission.Active);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rolePermission.Id = reader.GetInt32(0);
                    }
                }

                return rolePermission;
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
