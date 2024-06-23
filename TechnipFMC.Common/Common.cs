using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TechnipFMC.Common
{
    public static class Common
    {
        public static List<T> ToListOfObject<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = ToObject<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T ToObject<T>(this DataRow dr)
        {
            bool b = true;
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {

                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            if (pro.PropertyType.Name.Equals("Boolean"))
                            {
                                pro.SetValue(obj, Convert.ToBoolean(dr[column.ColumnName]), null);
                            }

                            else
                            {
                                pro.SetValue(obj, dr[column.ColumnName], null);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (dr[column.ColumnName].ToString() == "")
                                pro.SetValue(obj, null, null);
                            else
                                pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        public static T ToObject<T>(this DataTable dt)
        {

            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dt.Rows[0].Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dt.Rows[0][column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        //public static T ToObject<T>(this DataRow dr)
        //{
        //    Type temp = typeof(T);
        //    T obj = Activator.CreateInstance<T>();
        //    foreach (DataColumn column in dr.Table.Columns)
        //    {
        //        foreach (PropertyInfo pro in temp.GetProperties())
        //        {
        //            if (pro.Name == column.ColumnName)
        //                pro.SetValue(obj, dr[column.ColumnName], null);
        //            else
        //                continue;
        //        }
        //    }
        //    return obj;
        //}

        public static T ToObject<T>(this DataSet ds)
        {

            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in ds.Tables[0].Rows[0].Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, ds.Tables[0].Rows[0][column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public static int GetLastInsertId(this System.Data.DataSet ds)
        {
            int LASTINSERTID = 0;
            try
            {
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        LASTINSERTID = Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString().Trim());

                    }

                }

                return LASTINSERTID;
            }
            catch (Exception Ex) { return LASTINSERTID; }


        }
        public static int GetRowCount(this System.Data.DataTable dt)
        {
            int RowCount = 0;
            try
            {
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    RowCount = Convert.ToInt32(dt.Rows[0][0].ToString().Trim());

                }

                return RowCount;
            }
            catch (Exception Ex) { return RowCount; }


        }
        public static string TrimObject(this string str)
        {
            string retString = string.Empty;
            try
            {
                if (str != null)
                {
                    retString = str.Trim();
                }

                return retString;
            }
            catch (Exception Ex) { return retString; }


        }
        public static string ReplaceSingleQuots(this string str)
        {
            string retString = string.Empty;
            try
            {
                if (str != null)
                {
                    retString = str.Trim().Replace("'", "€");
                    retString = retString.Replace('€', '"');
                }

                return retString;
            }
            catch (Exception Ex) { return retString; }


        }


        public static string ToXML(this object obj)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(stringwriter, obj);
            return stringwriter.ToString();
        }



        public static void LogEntry(string module, string function, string message)
        {
            try
            {
                var logEntry = ConfigurationManager.AppSettings["logEntry"].ToString() == "OFF" ? false : true;

            }
            catch (Exception ex)
            {

            }
        }

        public static string DecimalCheck(this decimal Number)
        {
            string retval = Number.ToString();
            decimal result = Number - Math.Truncate(Number);
            if (result == 0)
            {
                retval = Math.Truncate(Number).ToString();
            }

            return retval;
        }
        public static string Convert_ImageTo_Base64(string path)
        {
            try
            {
                string filename = path;
                FileStream f = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                int size = (int)f.Length;
                byte[] MyData = new byte[f.Length + 1];
                f.Read(MyData, 0, size);
                f.Close();
                return Convert.ToBase64String(MyData);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }


}
