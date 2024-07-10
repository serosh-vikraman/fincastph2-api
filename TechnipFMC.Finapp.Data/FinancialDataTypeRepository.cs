using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;
using System.Data;
using System.Xml.Linq;

namespace TechnipFMC.Finapp.Data
{
    public class FinancialDataTypeRepository : BaseRepository, IFinancialDataTypeRepository
    {
        public FinancialDataTypeRepository()
        { }
        public bool Delete(int Id, string DeletedBy)
        {
            try
            {
                //throw new NotImplementedException();
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteFinancialDataTypeMaster";
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

        public IEnumerable<FinancialDataType> GetAll()
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllFinancialDataType";
                cmd.Parameters.AddWithValue("@P_Id", 0);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<FinancialDataType> obj = new List<FinancialDataType>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    FinancialDataType financialDataType = new FinancialDataType();
                    financialDataType.FinancialDataTypeID = Convert.ToInt32(results.Rows[i]["FinancialDataTypeID"]);
                    financialDataType.FinancialDataTypeName = results.Rows[i]["FinancialDataTypeName"].ToString();
                    financialDataType.FinancialDataTypeCode = results.Rows[i]["FinancialDataTypeCode"].ToString();
                    financialDataType.CreatedBy = Convert.ToInt32(results.Rows[i]["CreatedBy"]);
                    financialDataType.Active = Convert.ToBoolean(results.Rows[i]["Active"]);
                    financialDataType.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(financialDataType);
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
        public IEnumerable<FinancialDataType> GetAllMapped(string scope)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllFinancialDataTypesofScopeType";
                cmd.Parameters.AddWithValue("@P_Scope", scope);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<FinancialDataType> obj = new List<FinancialDataType>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    FinancialDataType financialDataType = new FinancialDataType();
                    financialDataType.FinancialDataTypeID = Convert.ToInt32(results.Rows[i]["FinancialDataTypeID"]);
                    financialDataType.FinancialDataTypeName = results.Rows[i]["FinancialDataTypeName"].ToString();
                    financialDataType.FinancialDataTypeCode = results.Rows[i]["FinancialDataTypeCode"].ToString();
                    financialDataType.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(financialDataType);
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
        public List<FinancialDataType> GetAllFinancialDataTypesofReport(int id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllFinancialDataTypesofReport";
                cmd.Parameters.AddWithValue("@P_ID", id);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<FinancialDataType> obj = new List<FinancialDataType>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    FinancialDataType financialDataType = new FinancialDataType();
                    financialDataType.FinancialDataTypeID = Convert.ToInt32(results.Rows[i]["FinancialDataTypeID"]);
                    financialDataType.FinancialDataTypeName = results.Rows[i]["FinancialDataTypeName"].ToString();
                    financialDataType.FinancialDataTypeCode = results.Rows[i]["FinancialDataTypeCode"].ToString();
                    financialDataType.Status = results.Rows[i]["Status"].ToString();
                    obj.Add(financialDataType);
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
        public List<FinancialDataType> GetAllFinancialDataTypesOfScenario(int id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllFinancialDataTypesOfScenario";
                cmd.Parameters.AddWithValue("@P_Id", id);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable results = new DataTable();

                adapter.Fill(results);
                base.DBConnection.Close();

                List<FinancialDataType> obj = new List<FinancialDataType>();

                for (int i = 0; i < results.Rows.Count; i++)
                {
                    FinancialDataType financialDataType = new FinancialDataType();
                    financialDataType.FinancialDataTypeID = Convert.ToInt32(results.Rows[i]["FinancialDataTypeID"]);
                    financialDataType.FinancialDataTypeName = results.Rows[i]["FinancialDataTypeName"].ToString();
                    financialDataType.FinancialDataTypeCode = results.Rows[i]["FinancialDataTypeCode"].ToString();
                    obj.Add(financialDataType);
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

        public FinancialDataType GetById(int Id)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllFinancialDataType";
                cmd.Parameters.AddWithValue("@P_Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        return new FinancialDataType()
                        {
                            FinancialDataTypeID = (int)reader["FinancialDataTypeID"],
                            FinancialDataTypeName = (string)reader["FinancialDataTypeName"],
                            FinancialDataTypeCode = (string)reader["FinancialDataTypeCode"],
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

        public FinancialDataType Save(FinancialDataType financialDataType)
        {
            try
            {
                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SaveFinancialDataType";
                cmd.Parameters.AddWithValue("@P_Id", financialDataType.FinancialDataTypeID);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeName", financialDataType.FinancialDataTypeName);
                cmd.Parameters.AddWithValue("@P_FinancialDataTypeCode", financialDataType.FinancialDataTypeCode);
                cmd.Parameters.AddWithValue("@P_Active", financialDataType.Active);
                cmd.Parameters.AddWithValue("@P_CreatedBy", financialDataType.CreatedBy);
                //cmd.Parameters.AddWithValue("@P_CustomerId", financialDataType.CustomerID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        financialDataType.FinancialDataTypeID = Convert.ToInt32(reader.GetInt32(0));
                        financialDataType.Message = reader.GetString(1);
                    }
                }

                return financialDataType;
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
        public bool Map(FinancialDataTypeMapping financialDataType)
        {
            try
            {
                var xmldata = new XElement("TDS", financialDataType.Financialdatatype.Select(x => new XElement("TD", x)));
                //new XElement("FinancialDataTypeCode", Financialdatatype)));
                //var xmldata = new XElement("TDS", from ObjDetails in financialDataType
                //                                  select new XElement("TD",
                //                                 new XElement("FinancialDataTypeCode", Financialdatatype)));

                SqlCommand cmd = base.DBConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MapFinancialDataType";
                cmd.Parameters.AddWithValue("@P_Scope", financialDataType.Scope);
                cmd.Parameters.AddWithValue("@P_Type", financialDataType.Type);
                cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());
                cmd.Parameters.AddWithValue("@P_CreatedBy", financialDataType.CreatedBy);

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
        public bool MapReportDatatypes(int reportId, string[] codes)
        {
            try
            {
                var xmldata = new XElement("TDS", codes.Select(code => new XElement("TD", code)));
                using (SqlCommand cmd = base.DBConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[SaveReportDatatypeMapping]";
                    cmd.Parameters.AddWithValue("@P_ReportId", reportId);
                    cmd.Parameters.AddWithValue("@XMLData", xmldata.ToString());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
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

        public FinancialDataType Update(FinancialDataType FinancialDataType)
        {
            throw new NotImplementedException();
        }
    }
}
