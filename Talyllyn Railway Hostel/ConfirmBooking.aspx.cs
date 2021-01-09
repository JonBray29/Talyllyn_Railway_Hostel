using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TalyllynRailwayHostel
{
    public partial class ConfirmBooking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {          
            lblConfirmBooking.Text = Session["volunteer"] + "," + " You have preliminary booked the following dates: " + Session["bookStart"].ToString() + " to " + Session["bookEnd"].ToString() + ".";
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CreateBooking.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Remove("Admin");
            Response.Redirect("~/LoginPage.aspx");
        }
    }
}