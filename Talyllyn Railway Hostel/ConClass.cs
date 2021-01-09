using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace TalyllynRailwayHostel
{
    public class ConClass
    {
        SqlConnection SQLCon = new SqlConnection();
        public DataTable SQLTable = new DataTable();

        public ConClass()
        {
            SQLCon.ConnectionString = "Data Source = socem1.uopnet.plymouth.ac.uk; Integrated Security = False; User ID = prdc251f; Password = PRDC251_F!; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
        }

        //Code below is to view the data thats in the database
        public void retrieveData(string command)
        {
            try
            {
                SQLCon.Open();
                SqlDataAdapter da = new SqlDataAdapter(command, SQLCon);
                da.Fill(SQLTable);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("<script>alert('Something wrong with connecting to DB " + ex.Message + "');</script>");
            }
            finally
            {
                SQLCon.Close();
            }
        }

    }
}