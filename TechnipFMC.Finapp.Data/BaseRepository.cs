using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TechnipFMC.Finapp.Data
{
    public class BaseRepository : IDisposable
    {
        public SqlConnection DBConnection { get; set; }
        public SqlConnection MasterDBConnection { get; set; }

        public BaseRepository()
        {
            string tenantDatabase = "";
            if (System.Web.HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                string jwtToken = System.Web.HttpContext.Current.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
                tenantDatabase = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "OrgCode")?.Value;
            }

            string tenantDatabaseName = "FINCAST_" + tenantDatabase;
            //string tenantDatabaseName = "FINCAST"; 
            // Construct the connection strings using the tenant-specific and master database information
            string tenantConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TenantConnectionString"].ConnectionString.Replace("TenantDatabaseName", tenantDatabaseName);
            string masterConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["FinappMasterConnection"].ConnectionString;

            // Create connections to both databases
            DBConnection = new SqlConnection(tenantConnectionString);
            MasterDBConnection = new SqlConnection(masterConnectionString);

            // Open the tenant database connection
            if (System.Web.HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                if (DBConnection.State == ConnectionState.Closed)
                    DBConnection.Open();
            }

            // Open the master database connection
            if (MasterDBConnection.State == ConnectionState.Closed)
                MasterDBConnection.Open();
        }

        public void Dispose()
        {
            if (DBConnection.State != ConnectionState.Closed)
                DBConnection.Close();
            DBConnection.Dispose();

            if (MasterDBConnection.State != ConnectionState.Closed)
                MasterDBConnection.Close();
            MasterDBConnection.Dispose();
        }
    }

}
