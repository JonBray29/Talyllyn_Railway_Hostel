using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TalyllynRailwayHostel
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                addData();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CreateBooking.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            updateBooking();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            deleteBooking();
        }
        private void updateBooking()
        {
            string chkIn = txtArriveDate.Text;
            string chkOut = txtDepartDate.Text;

            if (Convert.ToDateTime(chkIn) < DateTime.Now)
            {
                Response.Write("<script>alert('Please select a date in the future')</script>");
            }
            else if (Convert.ToDateTime(chkOut) < Convert.ToDateTime(chkIn))
            {
                Response.Write("<script>alert('Please select a depart date that is after the arrival date')</script>");
            }
            else
            {
                string booking = Session["bookingID"].ToString();
                string sqlQuery = @"UPDATE Booking 
                                SET CheckInDate = '" + chkIn + "'," +
                                    "CheckOutDate = '" + chkOut + "'" +
                                    "WHERE BookingID = '" + booking + "'";
                ConClass connection = new ConClass();
                connection.retrieveData(sqlQuery);
                Response.Redirect("~/CreateBooking.aspx");
            }
        }

        private void deleteBooking()
        {
            string booking = Session["bookingID"].ToString();
            string bedQuery = @"DELETE BedAssignment
                                WHERE BookingID = '" + booking + "'";
            ConClass bedConnect = new ConClass();
            bedConnect.retrieveData(bedQuery);

            string sqlBooking = @"DELETE Booking
                                WHERE BookingID = '" + booking + "'";
            ConClass bookingConnect = new ConClass();
            bookingConnect.retrieveData(sqlBooking);
            Response.Redirect("~/CreateBooking.aspx");
        }

        private void addData()
        {
            string booking = Session["bookingID"].ToString();
            string sqlQuery = @"SELECT Volunteer.FirstName, Volunteer.Surname,
                                Booking.CheckInDate, Booking.CheckOutDate, 
                                Booking.NumFemaleGuests, Booking.NumMaleGuests 
                                FROM Booking JOIN Volunteer ON Booking.VolunteerID = Volunteer.VolunteerID
                                WHERE Booking.BookingID = '" + booking + "'";
            ConClass connection = new ConClass();
            connection.retrieveData(sqlQuery);

            foreach (DataRow dr in connection.SQLTable.Rows)
            {
                txtFirstName.Text = (string)dr[0];
                txtLastName.Text = (string)dr[1];
                txtArriveDate.Text = ((DateTime)dr[2]).ToString("yyyy-MM-dd");
                txtDepartDate.Text = ((DateTime)dr[3]).ToString("yyyy-MM-dd");
                txtNumberOfFemale.Text = Convert.ToString((int)dr[4]);
                txtNumberOfMale.Text = Convert.ToString((int)dr[5]);

            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Remove("Volunteer");
            Session.Remove("bookStart");
            Session.Remove("bookEnd");
            Response.Redirect("~/LoginPage.aspx");
        }
    }
}
