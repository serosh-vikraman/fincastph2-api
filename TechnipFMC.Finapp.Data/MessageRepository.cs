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
    public class MessageRepository : BaseRepository
    {
        public string GetMessage(string code)
        {
            try
            {
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetMessage";
                cmd.Parameters.AddWithValue("@P_Code", code);
                SqlDataReader reader = cmd.ExecuteReader();
                string message = "";
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        message = (string)reader["Message"];
                        return message;
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
    }
}
