using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;
using System.Web;
using RestSharp;
using System.Net;
using static Hopac.Stream;
using static System.Net.WebRequestMethods;
using Mailjet.Client;
using Mailjet.Client.Resources;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Mailjet.Client.TransactionalEmails.Response;
using static HttpFs.Client;

namespace TechnipFMC.Finapp.Data
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository()
        { }
        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteCustomerMaster";
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

        public IEnumerable<Customer> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCustomer";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<Customer> obj = new List<Customer>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    Customer customer = new Customer();
                    customer.CustomerID = Convert.ToInt32(results.Rows[i]["CustomerID"]);
                    customer.CustomerName = results.Rows[i]["CustomerName"].ToString().Decrypt();
                    customer.CustomerCode = results.Rows[i]["CustomerCode"].ToString();
                    customer.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    customer.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    obj.Add(customer);
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

        public Customer GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCustomer";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new Customer()
                        {
                            CustomerID = (int)reader["CustomerID"],
                            CustomerName = (string)reader["CustomerName"].ToString().Decrypt(),                            
                            CountryID = (int)reader["CountryID"],
                            CustomerCode = (string)reader["CustomerCode"],
                            Admin = (string)reader["Admin"].ToString().Decrypt(),
                            Designation = (string)reader["Designation"],
                            Email = (string)reader["Email"].ToString().Decrypt(),
                            //  CreatedBy = (int)reader["CreatedBy"],
                            CurrencyID = (int)reader["CurrencyID"],
                            Unit = (int)reader["Unit"],
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

        public Customer Save(Customer customer)
        {
            try
            {
                customer.CustomerName = customer.CustomerName.Encrypt();
                customer.Admin = customer.Admin.Encrypt();
                customer.Email = customer.Email.Encrypt();
                customer.Pasword = customer.Pasword.Encrypt();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveCustomer";
                cmd.Parameters.AddWithValue("@P_Id", customer.CustomerID);
                cmd.Parameters.AddWithValue("@P_CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@P_CustomerCode", customer.CustomerCode);
                cmd.Parameters.AddWithValue("@P_CountryName", customer.CountryName);
                cmd.Parameters.AddWithValue("@P_Admin", customer.Admin);
                cmd.Parameters.AddWithValue("@P_Password", customer.Pasword);
                cmd.Parameters.AddWithValue("@P_Designation", customer.Designation);
                cmd.Parameters.AddWithValue("@P_Email", customer.Email);  
                cmd.Parameters.AddWithValue("@P_Active", customer.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", customer.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        customer.CustomerID = Convert.ToInt32(reader.GetInt32(0));
                        customer.CustomerName = customer.CustomerName.Decrypt();
                        customer.Admin = customer.Admin.Decrypt();
                        customer.Email = customer.Email.Decrypt();
                        customer.Pasword = customer.Pasword.Decrypt();
                    }
                }

                return customer;
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
        public int Signup(Customer customer)
        {
            try
            {
                customer.CustomerName = customer.CustomerName.Encrypt();
                customer.Admin = customer.Admin.Encrypt();
                customer.Email = customer.Email.Encrypt();
                customer.Pasword = customer.Pasword.Encrypt();
                int val = 0;
                var loginId = customer.Email.Encrypt();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveCustomer";
                cmd.Parameters.AddWithValue("@P_CustomerMasterID", customer.CustomerID);
                cmd.Parameters.AddWithValue("@P_CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@P_Admin", customer.Admin);
                cmd.Parameters.AddWithValue("@P_Password", customer.Pasword);
                cmd.Parameters.AddWithValue("@P_Designation", customer.Designation);
                cmd.Parameters.AddWithValue("@P_Email", customer.Email);
                cmd.Parameters.AddWithValue("@P_Plan", customer.PlanName);
                cmd.Parameters.AddWithValue("@P_PlanType", customer.PlanType);
                cmd.Parameters.AddWithValue("@P_CountryName", customer.CountryID);
                cmd.Parameters.AddWithValue("@P_Interval", customer.DataEntryInterval);
                cmd.Parameters.AddWithValue("@P_CurrencyId", customer.CurrencyID);
                cmd.Parameters.AddWithValue("@P_Unit", customer.Unit);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        val = Convert.ToInt32(reader.GetInt32(0));

                    }
                }

                return val;
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
        public async Task<int> InitialSignup(Customer customer)
        {
            try
            {
                customer.Admin = customer.Admin.Encrypt();
                customer.Email = customer.Email.Encrypt();
                customer.Pasword = customer.Pasword.Encrypt();
                int val = 0;
                var encodedloginId = HttpUtility.UrlEncode(customer.Email);
                var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var resultToken = new string(
                              Enumerable.Repeat(allChar, 8)
                              .Select(token => token[random.Next(token.Length)]).ToArray());
                string authToken = resultToken.ToString();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SignupCustomer";
                cmd.Parameters.AddWithValue("@P_Admin", customer.Admin);
                cmd.Parameters.AddWithValue("@P_Password", customer.Pasword);
                cmd.Parameters.AddWithValue("@P_Email", customer.Email);
                cmd.Parameters.AddWithValue("@P_Plan", customer.PlanName);
                cmd.Parameters.AddWithValue("@P_PlanType", customer.PlanType);
                cmd.Parameters.AddWithValue("@P_Token", authToken);
                //cmd.Parameters.AddWithValue("@P_Active", customer.Active);
                //cmd.Parameters.AddWithValue("@P_CreatedBy", customer.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        val = Convert.ToInt32(reader.GetInt32(0));

                    }
                }
                //val = 2;
                if (val > 0)  //(val > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"Hi {customer.Admin.Decrypt()},<br/> Please use the provided link below to confirm your email.<br/><br/>");
                    //  sb.Append("<b><a href=http://localhost:4200/emailconfirmed?username=" + encodedloginId + "&code=" + token + ">Click here</a><br/></b>");
                    sb.Append("<b><a href=https://fincast.app/emailconfirmed?username=" + encodedloginId + "&code=" + authToken + ">Click here</a><br/></b>");
                    sb.Append("Thanks,<br> Fincast Team <br/>");
                    int ret = await SendEmail(customer.Email.Decrypt(), sb.ToString(), "Welcome to Fincast");
                }  

                return val;
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
        public async Task<Customer> ResendEmail(string email)
        {
            try
            {
                email = email.Encrypt();
                Customer customer = new Customer();
                int val = 0;
                var encodedloginId = HttpUtility.UrlEncode(email);
                var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var resultToken = new string(
                              Enumerable.Repeat(allChar, 8)
                              .Select(token => token[random.Next(token.Length)]).ToArray());
                string authToken = resultToken.ToString();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ResendEmail";
                cmd.Parameters.AddWithValue("@P_Email", email);
                cmd.Parameters.AddWithValue("@P_Token", authToken);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        customer.Admin = (string)reader["Admin"].ToString();
                        customer.Email = (string)reader["Email"].ToString();
                        StringBuilder sb = new StringBuilder();
                        sb.Append($"Hi {customer.Admin.Decrypt()},<br/> Please use the provided link below to confirm your email.<br/><br/>");
                        //  sb.Append("<b><a href=http://localhost:4200/emailconfirmed?username=" + encodedloginId + "&code=" + token + ">Click here</a><br/></b>");
                        sb.Append("<b><a href=https://fincast.app/emailconfirmed?username=" + encodedloginId + "&code=" + authToken + ">Click here</a><br/></b>");
                        sb.Append("Thanks,<br> Fincast Team <br/>");

                        int ret = await SendEmail(customer.Email.Decrypt(), sb.ToString(), "Welcome to Fincast");
                        if(ret == 0)
                        {
                            customer.CustomerID = -1;
                        }
                    }
                }     
                

                return customer;
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
        
        public Customer Update(Customer customer)
        {
            try
            {
                customer.CustomerName = customer.CustomerName.Encrypt();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateCustomer";
                cmd.Parameters.AddWithValue("@P_Id", customer.CustomerID);
                cmd.Parameters.AddWithValue("@P_CurrencyId", customer.CurrencyID);
                cmd.Parameters.AddWithValue("@P_Unit", customer.Unit.ToString());
                //cmd.Parameters.AddWithValue("@P_CreatedBy", customer.CreatedBy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        customer.CustomerID = Convert.ToInt32(reader.GetInt32(0));
                        customer.CustomerName = customer.CustomerName.Decrypt();
                    }
                }

                return customer;
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
        public VerifyCustomer VerifyEmail(string loginId, string token)
        {
            try
            {
                //string decodedLoginId = HttpUtility.UrlDecode(loginId);
                VerifyCustomer customer = new VerifyCustomer();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ValidateSignUp";
                cmd.Parameters.AddWithValue("@P_LoginId", loginId);
                //cmd.Parameters.AddWithValue("@P_DecodedLoginId", loginId);
                cmd.Parameters.AddWithValue("@P_Token", token);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        customer.CustomerID = (int)reader["CustomerID"];
                        customer.Admin = (string)reader["Admin"].ToString().Decrypt();
                        customer.Email = (string)reader["EmailID"].ToString().Decrypt();
                        customer.PlanName = (string)reader["PlanName"].ToString();
                        customer.PlanType = (string)reader["PlanType"].ToString();
                    }
                }
                bool hasMoreResults = reader.NextResult();

                // Check if there is a second result set
                if (hasMoreResults && reader.HasRows)
                {
                    // Iterate through the rows of the second result set
                    while (reader.Read())
                    {
                        // Retrieve the desired information from the second dataset
                        customer.Link = reader.GetString(0);
                    }
                }
                

                return customer;
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
        public VerifyCustomer GetPlanDetailsofuser(string loginId, string token)
        {
            try
            {
                string decodedLoginId = HttpUtility.UrlDecode(loginId);
                //loginId = loginId.Encrypt();
                VerifyCustomer customer = new VerifyCustomer();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPlanDetailsofuser";
                cmd.Parameters.AddWithValue("@P_LoginId", loginId);
                cmd.Parameters.AddWithValue("@P_Token", token);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        customer.CustomerID = (int)reader["CustomerID"];
                        customer.Admin = (string)reader["Admin"].ToString().Decrypt();
                        customer.OrgName = (string)reader["CustomerName"].ToString().Decrypt();
                        customer.Email = (string)reader["EmailID"].ToString().Decrypt();
                        customer.PlanName = (string)reader["PlanName"].ToString();
                        customer.PlanType = (string)reader["PlanType"].ToString();
                    }
                }
                bool hasMoreResults = reader.NextResult();

                // Check if there is a second result set
                if (hasMoreResults && reader.HasRows)
                {
                    // Iterate through the rows of the second result set
                    while (reader.Read())
                    {
                        // Retrieve the desired information from the second dataset
                        customer.Link = reader.GetString(0);
                    }
                }


                return customer;
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
        public IEnumerable<SignUpCountry> GetAllCountries()
        {
            try
            {
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCountries";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<SignUpCountry> obj = new List<SignUpCountry>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    SignUpCountry country = new SignUpCountry();
                    country.CountryID = Convert.ToInt32(results.Rows[i]["CountryID"]);
                    country.CountryName = results.Rows[i]["CountryName"].ToString();
                    obj.Add(country);
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
        public string GetPlanUrl()
        {
            try
            {
                string url = "";
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPlanUrl";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        url = reader.GetString(0);
                    }
                }

                return url;
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
        public PlanSpecs GetPlanDetails(int Id)
        {
            try
            {
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPlanDetails";
                cmd.Parameters.AddWithValue("@P_CustomerId", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new PlanSpecs()
                        {
                            NumberOfUsers = (int)reader["NumberOfUsers"],
                            NumberOfScenarios = (int)reader["NumberOfScenarios"],
                            NumberOfDepartments = (int)reader["NumberOfDepartments"],
                            NumberOfProjects = (int)reader["NumberOfProjects"],
                           
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
        public string GetPaymentLink(string planname, string plantype)
        {
            try
            {
                string url = "";
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPaymentlink";
                cmd.Parameters.AddWithValue("@P_PlanName", planname);
                cmd.Parameters.AddWithValue("@P_PlanType", plantype);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        url = reader.GetString(0);
                    }
                }

                return url;
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
        public string Getrenewlink(string email)
        {
            try
            {
                string url = "";
                int val = 0;
                email = email.Encrypt();
                var encodedloginId = HttpUtility.UrlEncode(email);
                var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var resultToken = new string(
                              Enumerable.Repeat(allChar, 8)
                              .Select(token => token[random.Next(token.Length)]).ToArray());
                string authToken = resultToken.ToString();
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveToken";
                cmd.Parameters.AddWithValue("@P_Email", email);
                cmd.Parameters.AddWithValue("@P_Token", authToken);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        val = Convert.ToInt32(reader.GetInt32(0));

                    }
                }
                if (val == 1)
                    {
                        url = "https://fincast.app/renew?username=" + encodedloginId + "&code=" + authToken;
                    }

                return url;
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
        public List<PlanDetails> getallplans()
        {
            try
            {
                SqlCommand cmd = base.MasterDBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "getallplans";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.MasterDBConnection.Close();

                List<PlanDetails> obj = new List<PlanDetails>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    PlanDetails plan = new PlanDetails();
                    plan.plan_name = results.Rows[i]["PlanName"].ToString();
                    plan.payment_interval = results.Rows[i]["PlanType"].ToString();
                    plan.link = results.Rows[i]["Link"].ToString();

                    obj.Add(plan);
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
