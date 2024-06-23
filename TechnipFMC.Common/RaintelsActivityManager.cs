using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Common
{
    public class RaintelsActivityManager
    {
        
  

         public static string _connStr = ConfigurationManager.ConnectionStrings["FinappMasterConnection"].ConnectionString;
        
        public static void ActivityLog(int DisciplineId,int CofigItemId,int DocumentId,  string Page, string UserAction, 
            string UserId,string Staus,string DisciplineIds, string CofigItemIds,int DisciplineLevelId, 
            Int64 DocumentTypeId, Int64 ProjectTypeId,Int64 ProjectPhaseId,Int64 VendorId,Int64 DocumentSourceId, string DepartmentCode)
        {
            try
            {


                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("SaveActivityLog", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@P_DisciplineId", DisciplineId);
                        cmd.Parameters.AddWithValue("@P_CofigItemId", CofigItemId);
                        cmd.Parameters.AddWithValue("@P_DocumentId", DocumentId);
                        cmd.Parameters.AddWithValue("@P_Page", Page);
                        cmd.Parameters.AddWithValue("@P_Action", UserAction);
                        cmd.Parameters.AddWithValue("@P_CreatedBy", UserId);
                        cmd.Parameters.AddWithValue("@P_Staus", Staus);
                        cmd.Parameters.AddWithValue("@P_DisciplineIds", DisciplineIds);
                        cmd.Parameters.AddWithValue("@P_CofigItemIdsList", CofigItemIds);
                        cmd.Parameters.AddWithValue("@P_DisciplineLevelId", DisciplineLevelId);
                         cmd.Parameters.AddWithValue("@P_DocumentTypeId", DocumentTypeId);
                        cmd.Parameters.AddWithValue("@P_ProjectTypeId", ProjectTypeId);
                        cmd.Parameters.AddWithValue("@P_ProjectPhaseId", ProjectPhaseId);
                        cmd.Parameters.AddWithValue("@P_VendorId", VendorId);
                        cmd.Parameters.AddWithValue("@P_DocumentSourceId", DocumentSourceId);
                        cmd.Parameters.AddWithValue("@P_DepartmentCode", DepartmentCode);

                        con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        con.Close();
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
