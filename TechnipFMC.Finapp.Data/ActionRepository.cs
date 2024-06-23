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
    public class ActionRepository : BaseRepository, IActionRepository
    {
        public ActionRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteActionMaster";
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
        public IEnumerable<ActionEntity> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllActions";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                List<ActionEntity> obj = new List<ActionEntity>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    ActionEntity action = new ActionEntity();
                    action.ActionID = Convert.ToInt32(results.Rows[i]["ActionID"]);
                    action.ActionName = results.Rows[i]["ActionName"].ToString();
                    action.ActionCode = results.Rows[i]["ActionCode"].ToString();
                    action.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    action.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    action.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(action);
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
        public ActionEntity GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllActions";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new ActionEntity()
                        {
                            ActionID = (int)reader["ActionID"],
                            ActionName = (string)reader["ActionName"],
                            ActionCode = (string)reader["ActionCode"],
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

        public ActionEntity Save(ActionEntity actionEntity)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveActions";
                cmd.Parameters.AddWithValue("@P_Id", actionEntity.ActionID);
                cmd.Parameters.AddWithValue("@P_ActionName", actionEntity.ActionName);
                cmd.Parameters.AddWithValue("@P_ActionCode", actionEntity.ActionCode);
                cmd.Parameters.AddWithValue("@P_CreatedBy", actionEntity.CreatedBy);
                cmd.Parameters.AddWithValue("@P_Active", actionEntity.Active);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        actionEntity.ActionID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return actionEntity;
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

        public ActionEntity Update(ActionEntity actionEntity)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveActions";
                cmd.Parameters.AddWithValue("@P_Id", actionEntity.ActionID);
                cmd.Parameters.AddWithValue("@P_ActionName", actionEntity.ActionName);
                cmd.Parameters.AddWithValue("@P_ActionCode", actionEntity.ActionCode);
                cmd.Parameters.AddWithValue("@P_CreatedBy", actionEntity.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        actionEntity.ActionID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return actionEntity;
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
        //public Details SaveDetails(Details details)
        //{
        //    try
        //    {
        //        //param[10] = new SqlParameter("@patient_Name_Arabic", SqlDbType.NVarChar, 50);
        //        //param[10].Value = patient_Name_Arabic;

        //        SqlCommand cmd = base.DBConnection.CreateCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "SaveDetails";
        //        cmd.Parameters.AddWithValue("@P_Id", 0);
        //        cmd.Parameters.AddWithValue("@P_EngName", details.EnglishName);
        //        cmd.Parameters.AddWithValue("@P_Age", details.Age);
        //        cmd.Parameters.Add("@P_ArabicName", SqlDbType.NVarChar, 50).Value = details.ArabicName;
        //        //cmd.Parameters.AddWithValue("@P_ArabicName",details.ArabicName);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                details.Id = Convert.ToInt32(reader.GetInt32(0));
        //            }
        //        }

        //        return details;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        base.Dispose();
        //    }
        //}
        
    }
}
