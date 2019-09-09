﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TalyllynRailwayHostel
{
    public partial class WardenUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnBack.Click += BtnBack_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnAssign.Click += BtnAssign_Click;

            if (!IsPostBack)
            {
                addData();
            }

        }

        private void BtnAssign_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AssignBeds.aspx");
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WardenBookings.aspx");
        }

        //DELETE BUTTON TRYOUT---------------------------------------------
        private void BtnDelete_Click(object sender, EventArgs e)
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
            Response.Redirect("~/WardenBookings.aspx");
        }
        //---------------------------------------------------------------------------


        //Update Dates


        private void BtnUpdate_Click(object sender, EventArgs e)
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
                Response.Redirect("~/WardenBookings.aspx");
            }
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

            foreach(DataRow dr in connection.SQLTable.Rows)
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
            Session.Remove("Admin");
            Response.Redirect("~/LoginPage.aspx");
        }
    }
}
