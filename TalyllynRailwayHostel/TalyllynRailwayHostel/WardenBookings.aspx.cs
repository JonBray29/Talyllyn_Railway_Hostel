using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace TalyllynRailwayHostel
{
    public partial class WardenBookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            this.grdBookings.SelectedIndexChanged += GrdBookings_SelectedIndexChanged;
            this.grdConfirmed.SelectedIndexChanged += GrdConfirmed_SelectedIndexChanged;
            bindToGrid();
            bindToGridConfirmed();
        }

        private void bindToGrid()
        {
            string sqlQuery = @"SELECT Booking.BookingID, Volunteer.FirstName, Volunteer.Surname,
                                Booking.CheckInDate, Booking.CheckOutDate, Booking.NumFemaleGuests,
                                Booking.NumMaleGuests, Booking.Confirmed
                                FROM Booking JOIN Volunteer ON Booking.VolunteerID = Volunteer.VolunteerID
                                WHERE Confirmed != 'True'";
            ConClass connection = new ConClass();
            connection.retrieveData(sqlQuery);

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("BookingId", typeof(int)),
                    new DataColumn("FirstName", typeof(string)),
                    new DataColumn("Surname", typeof(string)),
                    new DataColumn("CheckIn", typeof(string)),
                    new DataColumn("CheckOut", typeof(string)),
                    new DataColumn("NumFemale", typeof(int)),
                    new DataColumn("NumMale", typeof(int)),
                    new DataColumn("Confirmed",typeof(Boolean)) });

            foreach (DataRow dr in connection.SQLTable.Rows)
            {
                int bookingId = (int)dr[0];
                string firstName = (string)dr[1];
                string surname = (string)dr[2];
                string checkInDate = ((DateTime)dr[3]).ToShortDateString();
                string checkOutDate = ((DateTime)dr[4]).ToShortDateString();
                int numberOfFemale = (int)dr[5];
                int numberOfMale = (int)dr[6];
                Boolean confirmed = (Boolean)dr[7];

                dt.Rows.Add(bookingId, firstName, surname, checkInDate, checkOutDate, numberOfFemale, numberOfMale, confirmed);
            }

            this.grdBookings.DataSource = dt;
            this.grdBookings.DataBind();
        }
        private void bindToGridConfirmed()
        {
            string sqlQuery = @"SELECT Booking.BookingID, Volunteer.FirstName, Volunteer.Surname,
                                Booking.CheckInDate, Booking.CheckOutDate, Booking.NumFemaleGuests,
                                Booking.NumMaleGuests, Booking.Confirmed
                                FROM Booking JOIN Volunteer ON Booking.VolunteerID = Volunteer.VolunteerID
                                WHERE Confirmed = 'True' AND Booking.CheckInDate > DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";
            ConClass connection = new ConClass();
            connection.retrieveData(sqlQuery);

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("BookingId", typeof(int)),
                    new DataColumn("FirstName", typeof(string)),
                    new DataColumn("Surname", typeof(string)),
                    new DataColumn("CheckIn", typeof(string)),
                    new DataColumn("CheckOut", typeof(string)),
                    new DataColumn("NumFemale", typeof(int)),
                    new DataColumn("NumMale", typeof(int)),
                    new DataColumn("Confirmed",typeof(Boolean)) });

            foreach (DataRow dr in connection.SQLTable.Rows)
            {
                int bookingId = (int)dr[0];
                string firstName = (string)dr[1];
                string surname = (string)dr[2];
                string checkInDate = ((DateTime)dr[3]).ToShortDateString();
                string checkOutDate = ((DateTime)dr[4]).ToShortDateString();
                int numberOfFemale = (int)dr[5];
                int numberOfMale = (int)dr[6];
                Boolean confirmed = (Boolean)dr[7];

                dt.Rows.Add(bookingId, firstName, surname, checkInDate, checkOutDate, numberOfFemale, numberOfMale, confirmed);
            }

            this.grdConfirmed.DataSource = dt;
            this.grdConfirmed.DataBind();
        }
        private void GrdBookings_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["bookingID"] = this.grdBookings.SelectedRow.Cells[1].Text;
            Response.Redirect("WardenUpdate.aspx");

        }
        private void GrdConfirmed_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["bookingID"] = this.grdConfirmed.SelectedRow.Cells[1].Text;
            Response.Redirect("WardenUpdate.aspx");

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Remove("Admin");
            Response.Redirect("~/LoginPage.aspx");
        }

        protected void btnPast_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PastBooking.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(txtDateFrom.Text) < DateTime.Now)
            {
                Response.Write("<script>alert('Please select a date in the future')</script>");
            }
            else if (Convert.ToDateTime(txtDateTo.Text) < Convert.ToDateTime(txtDateFrom.Text))
            {
                Response.Write("<script>alert('Please select a date to that is after date from')</script>");
            }
            else
            {

                string sqlQuery = @"SELECT Booking.BookingID, Volunteer.FirstName, Volunteer.Surname,
                                Booking.CheckInDate, Booking.CheckOutDate, Booking.NumFemaleGuests,
                                Booking.NumMaleGuests, Booking.Confirmed
                                FROM Booking JOIN Volunteer ON Booking.VolunteerID = Volunteer.VolunteerID
                                WHERE Confirmed = 'True' AND Booking.CheckInDate >= '" + txtDateFrom.Text +
                                "' AND Booking.CheckInDate <= '" + txtDateTo.Text + "'";
                ConClass connection = new ConClass();
                connection.retrieveData(sqlQuery);

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[8] { new DataColumn("BookingId", typeof(int)),
                    new DataColumn("FirstName", typeof(string)),
                    new DataColumn("Surname", typeof(string)),
                    new DataColumn("CheckIn", typeof(string)),
                    new DataColumn("CheckOut", typeof(string)),
                    new DataColumn("NumFemale", typeof(int)),
                    new DataColumn("NumMale", typeof(int)),
                    new DataColumn("Confirmed",typeof(Boolean)) });

                foreach (DataRow dr in connection.SQLTable.Rows)
                {
                    int bookingId = (int)dr[0];
                    string firstName = (string)dr[1];
                    string surname = (string)dr[2];
                    string checkInDate = ((DateTime)dr[3]).ToShortDateString();
                    string checkOutDate = ((DateTime)dr[4]).ToShortDateString();
                    int numberOfFemale = (int)dr[5];
                    int numberOfMale = (int)dr[6];
                    Boolean confirmed = (Boolean)dr[7];

                    dt.Rows.Add(bookingId, firstName, surname, checkInDate, checkOutDate, numberOfFemale, numberOfMale, confirmed);
                }

                this.grdConfirmed.DataSource = dt;
                this.grdConfirmed.DataBind();
            }
        }
    }
}