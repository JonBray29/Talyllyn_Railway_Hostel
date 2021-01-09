using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TalyllynRailwayHostel
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Admin"] != null)
            {
                Response.Redirect("~/WardenBookings.aspx");
            }
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ToString());
            con.Open();


            //Staff
            string staffLoginQuery = "SELECT Count(*) FROM Staff WHERE Username='" + txtusernameInsert.Text + "' and Pssword='" + txtpasswordInsert.Text + "' ";

            SqlCommand command = new SqlCommand (staffLoginQuery, con);
            string outputStaffLogin = command.ExecuteScalar().ToString();


            //Volunteer 
            string volunteerLoginQuery = "SELECT Count(*) FROM Volunteer WHERE Username='" + txtusernameInsert.Text + "' and Pssword='" + txtpasswordInsert.Text + "' ";

            SqlCommand volCommand = new SqlCommand(volunteerLoginQuery, con);
            string outputVolunteerLogin = volCommand.ExecuteScalar().ToString();


            if (outputStaffLogin == "1")
            {
                Session["Admin"] = txtusernameInsert.Text;
                Response.Redirect("~/WardenBookings.aspx");
            }
            else if (outputVolunteerLogin == "1")
            {
                Session["Volunteer"] = txtusernameInsert.Text;
                Response.Redirect("~/CreateBooking.aspx");
            }
            else
            {
                Response.Write("Failed");
            }

            con.Close();

        }

       
    }
}