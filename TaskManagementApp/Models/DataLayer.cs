using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TaskManagementApp.Models
{
    public class DataLayer
    {
        private readonly string _connString;

        public DataLayer()
        {
            _connString = ConfigurationManager.AppSettings["con"];
        }

        public string GetConnString()
        {
            return _connString;
        }

        // Common Function: Data Select karne ke liye
        public DataTable GetTable(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ExecuteQuerytoGetDataTble(string query, SqlParameter[] parameters = null)
        {
            // Simple logic using System.Data.SqlClient
            using (SqlConnection con = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    // System.Data.SqlClient is very stable for con.Open()
                    con.Open();
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception("SQL Exception: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // Common Function: Insert/Update/Delete ke liye
        public int ExecuteQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }


      
        public DataTable GetDataTableByProcedure(string procName, SqlParameter[] parameters = null)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
               
                using (SqlCommand cmd = new SqlCommand(procName, con))
                {
                   
                    cmd.CommandType = CommandType.StoredProcedure;

                   
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        try
                        { 
                            da.Fill(dt);
                            return dt;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("SQL Exception in GetDataTableByProcedure: " + ex.Message);
                        }
                    }
                }
            }
        }
        public int ExecuteProcedureWithTransAndCon(string procName, SqlParameter[] parameters, SqlTransaction trans, SqlConnection con)
        {
            // SqlCommand ko using mein rakhein taaki memory release ho jaye
            using (SqlCommand cmd = new SqlCommand(procName, con, trans))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteNonQuery();
            }
        }
    }
}