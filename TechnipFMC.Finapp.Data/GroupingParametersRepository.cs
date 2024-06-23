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
    public class GroupingParametersRepository : BaseRepository, IGroupingParametersRepository
    {
        public GroupingParametersRepository()
        { }

        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteGroupingParametersMaster";
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
        public IEnumerable<GroupingParameters> GetAll()
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllGroupingParameter";
                cmd.Parameters.AddWithValue("@P_Id", 0);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<GroupingParameters> obj = new List<GroupingParameters>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    GroupingParameters groupingparameters = new GroupingParameters();
                    groupingparameters.GroupingParametersID = Convert.ToInt32(results.Rows[i]["GroupingParametersID"]);
                    groupingparameters.GroupingParametersName = results.Rows[i]["GroupingParametersName"].ToString();
                    groupingparameters.GroupingParametersCode = results.Rows[i]["GroupingParametersCode"].ToString();
                    groupingparameters.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    groupingparameters.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    groupingparameters.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(groupingparameters);
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
        public GroupingParameters GetById(int Id )
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllGroupingParameter";
                cmd.Parameters.AddWithValue("@P_Id", Id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        return new GroupingParameters()
                        {
                            GroupingParametersID = (int)reader["GroupingParametersID"],
                            GroupingParametersName = (string)reader["GroupingParametersName"],
                            GroupingParametersCode = (string)reader["GroupingParametersCode"],
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

        public GroupingParameters Save(GroupingParameters groupingparameters)
        {
            try
            {

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveGroupingParameter";
                cmd.Parameters.AddWithValue("@P_Id", groupingparameters.GroupingParametersID);
                cmd.Parameters.AddWithValue("@P_GroupingParameterName", groupingparameters.GroupingParametersName);
                cmd.Parameters.AddWithValue("@P_GroupingParameterCode", groupingparameters.GroupingParametersCode);
                cmd.Parameters.AddWithValue("@P_Active", groupingparameters.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", groupingparameters.CreatedBy);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        groupingparameters.GroupingParametersID = Convert.ToInt32(reader.GetInt32(0));
                    }
                }

                return groupingparameters;
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

        public GroupingParameters Update(GroupingParameters GroupingParameterss)
        {
            throw new NotImplementedException();
        }
    }
}
