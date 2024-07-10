using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TechnipFMC.Finapp.Models;
using Newtonsoft.Json;

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
        public async Task<int> SendEmail(string email, string sb, string subject)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "http://mail.raintels.in/api/send-mail");
                    request.Headers.Add("X-Authorization-Token", "HBcv0ehXARdCUuLpyoUuVV4hMI0rzAtlX8uM1QC5PKTZqK9tRfOEuvckESAK8cYQ");

                    var payload = new
                    {
                        gateway = "mailjet",
                        to = email,
                        subject = subject,
                        html = sb,
                        from_name = "Fincast <fincast@raintels.com>"
                    };

                    var jsonPayload = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    request.Content = content;

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Email sent successfully!");
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine($"Error sending email: {response.StatusCode}");
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return 0;
            }
        }
    }

}
