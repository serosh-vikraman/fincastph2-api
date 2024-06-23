using System.DirectoryServices;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System;
using System.Text;

namespace TP.Utility.ActiveDirectory
{
    public class LDAPQuery
    {
        static string passportImage = "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";
        public static Dictionary<string, string> GetUserInfo(string userName)
        {
            if(userName=="")
            {
                byte[] bytes = Encoding.ASCII.GetBytes(passportImage);
                Dictionary<string, string> usr = new Dictionary<string, string>();

                usr["UserId"] = "ADA123Sree";
                usr["LoginUserName"] = "Sreekanth";
                usr["Department"] ="Authorization";
                usr["Title"] = "Admin";
                //u["r.EmpType = (user.Properties["employeeType"] != null && user.Properties["employeeType"].Value != null)? user.Properties["employeeType"].Value.ToString() : "";
                usr["FirstName"] = "Sreekanth";
                usr["LastName"] = "S";
                //u["sr.Email = (user.Properties["mail"] != null && user.Properties["mail"].Value != null)? user.Properties["mail"].Value.ToString() : "";
                //u["sr.Country = (user.Properties["co"] != null && user.Properties["co"].Value != null)? user.Properties["co"].Value.ToString() : "";
                //usr.Manager = (user.Properties["manager"] != null && user.Properties["manager"].Value != null)? user.Properties["manager"].Value.ToString() : "";
                usr["distintname"] = "Sreeks";
                usr["Entity"] = "";
                usr["thumbnailPhoto"] = ("data:image/JPEG;base64," + passportImage);

                return usr;
            }
            else
            {
                string AdminUser = ConfigurationManager.AppSettings["admin.ad.logon"].ToString();
                string AdminPassword = ConfigurationManager.AppSettings["admin.ad.password"].ToString();
                string LDAPPath = ConfigurationManager.AppSettings["ldap.path"].ToString();
                //string SettingIcon = ConfigurationManager.AppSettings["ShowSettingIcon"].ToString();
                Dictionary<string, string> usr = new Dictionary<string, string>();

                DirectoryEntry deADS = new DirectoryEntry(LDAPPath, AdminUser, AdminPassword);
                using (var dsSearcher = new DirectorySearcher(deADS))
                {
                    var idx = userName.IndexOf('\\');
                    if (idx > 0)
                    {
                        userName = userName.Substring(idx + 1);
                        //  dsSearcher.Filter = "(&(objectCategory=person)(|(sAMAccountName=" + userName + "*)(sn=" + userName + "*)))";
                        dsSearcher.Filter = "(&(objectCategory=person)(sAMAccountName=" + userName + "))";
                        SearchResult result = dsSearcher.FindOne();
                        if (result != null)
                        {
                            using (var user = new DirectoryEntry(result.Path, AdminUser, AdminPassword))
                            {
                                usr["UserId"] = (user.Properties["employeeID"] != null && user.Properties["employeeID"].Value != null) ? user.Properties["employeeID"].Value.ToString() : "";
                                usr["LoginUserName"] = (user.Properties["sAMAccountName"] != null && user.Properties["sAMAccountName"].Value != null) ? user.Properties["sAMAccountName"].Value.ToString() : "";
                                usr["Department"] = (user.Properties["department"] != null && user.Properties["department"].Value != null) ? user.Properties["department"].Value.ToString() : "";
                                usr["Title"] = (user.Properties["title"] != null && user.Properties["title"].Value != null) ? user.Properties["title"].Value.ToString() : "";
                                //u["r.EmpType = (user.Properties["employeeType"] != null && user.Properties["employeeType"].Value != null)? user.Properties["employeeType"].Value.ToString() : "";
                                usr["FirstName"] = (user.Properties["givenName"] != null && user.Properties["givenName"].Value != null) ? user.Properties["givenName"].Value.ToString() : "";
                                usr["LastName"] = (user.Properties["sn"] != null && user.Properties["sn"].Value != null) ? user.Properties["sn"].Value.ToString() : "";
                                //u["sr.Email = (user.Properties["mail"] != null && user.Properties["mail"].Value != null)? user.Properties["mail"].Value.ToString() : "";
                                //u["sr.Country = (user.Properties["co"] != null && user.Properties["co"].Value != null)? user.Properties["co"].Value.ToString() : "";
                                //usr.Manager = (user.Properties["manager"] != null && user.Properties["manager"].Value != null)? user.Properties["manager"].Value.ToString() : "";
                                usr["distintname"] = (user.Properties["distinguishedName"] != null && user.Properties["distinguishedName"].Value != null) ? user.Properties["distinguishedName"].Value.ToString() : "";
                                usr["Entity"] = ((user.Properties["c"] != null && user.Properties["c"].Value != null) ? user.Properties["c"].Value.ToString() : "");
                                usr["Entity"] += "::" + ((user.Properties["company"] != null && user.Properties["company"].Value != null) ? user.Properties["company"].Value.ToString() : "");
                                usr["Entity"] += "::" + ((user.Properties["extensionattribute3"] != null && user.Properties["extensionattribute3"].Value != null) ? user.Properties["extensionattribute3"].Value.ToString() : "");
                                usr["thumbnailPhoto"] = ((user.Properties["thumbnailPhoto"] != null && user.Properties["thumbnailPhoto"].Value != null) ? ("data:image/JPEG;base64," + Convert.ToBase64String(user.Properties["thumbnailPhoto"].Value as byte[])) : "");

                                //Icon Show Property : Added By : Syed Saqib 
                                //if (SettingIcon.Contains(usr.UserName))
                                //{
                                //    usr.IsShow = true;
                                //}
                                return usr;
                            }
                        }
                    }
                    return usr;
                }
            }
            

        }

        public static byte[] GetUserPicture(string userName)
        {
            string AdminUser = ConfigurationManager.AppSettings["admin.ad.logon"].ToString();
            string AdminPassword = ConfigurationManager.AppSettings["admin.ad.password"].ToString();
            string LDAPPath = ConfigurationManager.AppSettings["ldap.path"].ToString();

            DirectoryEntry deADS = new DirectoryEntry(LDAPPath, AdminUser, AdminPassword);
            using (var dsSearcher = new DirectorySearcher(deADS))
            {
                var idx = userName.IndexOf('\\');
                if (idx > 0)
                {
                    userName = userName.Substring(idx + 1);
                    dsSearcher.Filter = "(&(objectCategory=person)(|(sAMAccountName=" + userName + "*)(sn=" + userName + "*)))";
                    SearchResult result = dsSearcher.FindOne();
                    if (result != null)
                    {
                        using (var user = new DirectoryEntry(result.Path, AdminUser, AdminPassword))
                        {
                            var data = user.Properties["thumbnailPhoto"].Value as byte[];
                            if (data != null)
                            {
                                return data;
                            }
                        }
                    }
                }
                return null;
            }
        }


        public static bool FindUserInGroup(string username, string GroupName)
        {
            string AdminUser = ConfigurationManager.AppSettings["admin.ad.logon"].ToString();
            string AdminPassword = ConfigurationManager.AppSettings["admin.ad.password"].ToString();
            string LDAPPath = ConfigurationManager.AppSettings["ldap.path"].ToString();
            string distintname = string.Empty;

            DirectoryEntry deADS = new DirectoryEntry(LDAPPath, AdminUser, AdminPassword);
            using (var dsSearcher = new DirectorySearcher(deADS))
            {
                var idx = username.IndexOf('\\');
                if (idx > 0)
                {
                    username = username.Substring(idx + 1);
                    dsSearcher.Filter = "(&(objectClass=group)(name=" + GroupName + "))";
                    SearchResult result = dsSearcher.FindOne();
                    if (result != null)
                    {
                        using (var user = new DirectoryEntry(result.Path, AdminUser, AdminPassword))
                        {
                            distintname = user.Properties["distinguishedName"].Value.ToString();
                        }

                        using (var dsUsrSearch = new DirectorySearcher(deADS))
                        {
                            dsUsrSearch.Filter = "(&(objectClass=user)(memberOf:1.2.840.113556.1.4.1941:="+distintname+ ")(|(sAMAccountName=" + username + "*)(sn=" + username + "*)))";
                            SearchResult usrSearchres = dsUsrSearch.FindOne();
                            if (usrSearchres != null)
                            {
                                
                                return true;
                            }
                        }                        
                    }                    
                }
            }
            return false;
        }

        public static DataSet FindUserGroups(string username, string distintname)
        {

            //username = "TP\\emolina";
            //distintname = "CN=Emerson Molina,OU=QA_D241,OU=QA_Project_Support_Disciplines,OU=QA_Head_Office,OU=Users,OU=Doha,OU=QA,OU=EMEA,DC=tp,DC=tpnet,DC=intra";

            string AdminUser = ConfigurationManager.AppSettings["admin.ad.logon"].ToString();
            string AdminPassword = ConfigurationManager.AppSettings["admin.ad.password"].ToString();
            string LDAPPath = ConfigurationManager.AppSettings["ldap.path"].ToString();
            //string distintname = string.Empty;
            DataSet ds = new DataSet("UserGroups");
            DataTable dt = new DataTable("UserGroup");

            dt.Columns.Add("UserId");
            dt.Columns.Add("GroupId");
            dt.AcceptChanges();

            ds.Tables.Add(dt);
            ds.AcceptChanges();

            DirectoryEntry deADS = new DirectoryEntry(LDAPPath, AdminUser, AdminPassword);
            using (var dsSearcher = new DirectorySearcher(deADS))
            {
                var idx = username.IndexOf('\\');
                if (idx > 0)
                {
                    username = username.Substring(idx + 1);
                    //dsSearcher.Filter = "(&(objectCategory=person)(|(sAMAccountName=" + username + "*)(sn=" + username + "*)))";
                    //SearchResult result = dsSearcher.FindOne();
                    //if (result != null)
                    //{
                        //using (var user = new DirectoryEntry(result.Path, AdminUser, AdminPassword))
                        //{
                        //    distintname = user.Properties["distinguishedName"].Value.ToString();
                        //}

                        using (var dsUsrSearch = new DirectorySearcher(deADS))
                        {
                            dsUsrSearch.Filter = "(&(objectClass=Group)(member=" + distintname + "))";

                        //dsUsrSearch.PropertiesToLoad.Add("cn");
                        dsUsrSearch.PropertiesToLoad.Add("samaccountname");
                        //dsUsrSearch.PropertiesToLoad.Add("memberOf");

                        SearchResultCollection usrSearchres = dsUsrSearch.FindAll();
                            if (usrSearchres != null)
                            {
                                foreach (SearchResult group in usrSearchres)
                                {
                                    using (var user = new DirectoryEntry(group.Path, AdminUser, AdminPassword))
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr["UserId"] = username;
                                        dr["GroupId"] = user.Properties["sAMAccountName"].Value.ToString();

                                        dt.Rows.Add(dr);
                                    }
                                }                                
                            }
                        }

                  
                   

                    //}
                }
            }
            return ds;
        }


    }
}
