using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TalyllynRailwayHostel
{
    public partial class CreateBooking : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["bookStart"] = txtStartDate.Text;
            Session["bookEnd"] = txtEndDate.Text;
            //if (Session["Volunteer"] != null)
            getData();
            volBookingsToGrid();
            this.grdVolunteerBookings.SelectedIndexChanged += GrdVolunteerBookings_SelectedIndexChanged;
        }

        private void GrdVolunteerBookings_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["bookingID"] = this.grdVolunteerBookings.SelectedRow.Cells[1].Text;
            Response.Redirect("~/VolunteerSelectBooking.aspx");

        }

        [System.Web.Services.WebMethod]
        public static string loadFields(string volID, string chkIn, string chkOut, string fields, string table)
        {

            string message = "successful"; //Return message for ajax so it can empty array

            //Get values from the fields

            int c = 0;
            string column = "";
            for (c = 0; c <= fields.Split(',').Length - 1; c++)
            {
                if (string.IsNullOrEmpty(column))
                {
                    column = "[" + fields.Split(',')[c].Replace(" ", "") + "] VARCHAR (100)";
                }
                else
                {
                    column = column + ", [" + fields.Split(',')[c].Replace(" ", "") + "] VARCHAR (100)";
                }
            }


            string[] args = fields.Remove(fields.Length - 1, 1).Split(',');

            if (Convert.ToDateTime(chkIn) < DateTime.Now)
            {

            }
            else if (Convert.ToDateTime(chkOut) < Convert.ToDateTime(chkIn))
            {

            }
            else
            {
                int volunteerID = int.Parse(volID);

                System.Threading.Thread.Sleep(1000);

                string bookingQuery = "SELECT BookingID FROM Booking WHERE VolunteerID = '" + volunteerID + "' AND CheckInDate = '" + chkIn + "' AND CheckOutDate = '" + chkOut + "'";
                ConClass bookingConnect = new ConClass();
                bookingConnect.retrieveData(bookingQuery);
                int bookingID = (int)bookingConnect.SQLTable.Rows[0].ItemArray.GetValue(0);

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "") continue;
                    string firstName = args[i++];
                    string surname = args[i++];
                    string dob = args[i++];
                    string gender = args[i++];

                    string guestQuery = "INSERT into Guest(FirstName, Surname, DOB, Gender) VALUES('" + firstName + "', '" + surname + "', '" + dob + "', '" + gender + "'); " +
                        "SELECT GuestID FROM Guest WHERE FirstName = '" + firstName + "' AND Surname = '" + surname + "' AND DOB = '" + dob + "'";
                    ConClass guestConnect = new ConClass();
                    guestConnect.retrieveData(guestQuery);


                    int guestID = (int)guestConnect.SQLTable.Rows[0].ItemArray.GetValue(0);
                    //int volunteerID = int.Parse(volID);

                    //string bookingQuery = "SELECT BookingID FROM Booking WHERE VolunteerID = '" + volunteerID + "' AND CheckInDate = '" + chkIn + "' AND CheckOutDate = '" + chkOut + "'";
                    //ConClass bookingConnect = new ConClass();
                    //bookingConnect.retrieveData(bookingQuery);
                    //int bookingID = (int)bookingConnect.SQLTable.Rows[0].ItemArray.GetValue(0);

                    if (gender == "Male")
                    {
                        string setGenderQuery = @"UPDATE Booking 
                                SET NumMaleGuests = NumMaleGuests + 1 " +
                                        "WHERE BookingID = '" + bookingID + "'";
                        ConClass setGenderConnection = new ConClass();
                        setGenderConnection.retrieveData(setGenderQuery);
                    }
                    else
                    {
                        string setGenderQuery = @"UPDATE Booking 
                                SET NumFemaleGuests = NumFemaleGuests + 1  " +
                                        "WHERE BookingID = '" + bookingID + "'";
                        ConClass setGenderConnection = new ConClass();
                        setGenderConnection.retrieveData(setGenderQuery);
                    }

                    string bedQuery = "INSERT into BedAssignment(BookingID, GuestID) VALUES('" + bookingID + "', '" + guestID + "')";
                    ConClass bedConnect = new ConClass();
                    bedConnect.retrieveData(bedQuery);
                }
            }
            return message;

        }


        protected void getData()
        {
            //Get volunteers name, gender and age and display it
            string sqlQuery = @"SELECT VolunteerID, FirstName, Surname, DOB, Gender
                                FROM Volunteer WHERE Username = '" + Session["Volunteer"].ToString() + "'";
            ConClass connection = new ConClass();
            connection.retrieveData(sqlQuery);

            //int volID = 0;

            foreach (DataRow dr in connection.SQLTable.Rows)
            {
                txtID.Text = dr[0].ToString();
                txtFirstName.Text = (string)dr[1];
                txtLastName.Text = (string)dr[2];
                txtDOB.Text = ((DateTime)dr[3]).ToString("yyyy-MM-dd");
                txtGender.Text = (string)dr[4];

            }
        }

        //protected void btnConfirm_Click(object sender, EventArgs e)
        [System.Web.Services.WebMethod]
        public static string makeBooking(string volID, string chkIn, string chkOut)
        {
            string message = "successful";
            int volunteerID = int.Parse(volID);

            if (Convert.ToDateTime(chkIn) < DateTime.Now)
            {

            }
            else if (Convert.ToDateTime(chkOut) < Convert.ToDateTime(chkIn))
            {

            }
            else
            {
                string insertBookingQuery = @"INSERT into Booking(VolunteerID, CheckInDate, CheckOutDate, NumFemaleGuests, NumMaleGuests, 
                                Confirmed) VALUES('" + volunteerID + "', '" + chkIn + "', '" + chkOut + "', '0', '0', 'False')";
                ConClass insertBookingConnect = new ConClass();
                insertBookingConnect.retrieveData(insertBookingQuery);

                string bookingQuery = "SELECT BookingID FROM Booking WHERE VolunteerID = '" + volunteerID + "' AND CheckInDate = '" + chkIn + "' AND CheckOutDate = '" + chkOut + "'";
                ConClass bookingConnect = new ConClass();
                bookingConnect.retrieveData(bookingQuery);

                int bookingID = (int)bookingConnect.SQLTable.Rows[0].ItemArray.GetValue(0);

                string genderQuery = @"SELECT Gender FROM Volunteer WHERE VolunteerID = '" + volunteerID + "'";
                ConClass genderConnect = new ConClass();
                genderConnect.retrieveData(genderQuery);

                string gender = (string)genderConnect.SQLTable.Rows[0].ItemArray.GetValue(0);

                if (gender == "Male")
                {
                    string setGenderQuery = @"UPDATE Booking 
                                SET NumMaleGuests = NumMaleGuests + 1 " +
                                    "WHERE BookingID = '" + bookingID + "'";
                    ConClass setGenderConnection = new ConClass();
                    setGenderConnection.retrieveData(setGenderQuery);
                }
                else
                {
                    string setGenderQuery = @"UPDATE Booking 
                                SET NumFemaleGuests = NumFemaleGuests + 1  " +
                                    "WHERE BookingID = '" + bookingID + "'";
                    ConClass setGenderConnection = new ConClass();
                    setGenderConnection.retrieveData(setGenderQuery);
                }

                string bedQuery = "INSERT into BedAssignment(BookingID, VolunteerID) VALUES('" + bookingID + "', '" + volunteerID + "')";
                ConClass bedConnect = new ConClass();
                bedConnect.retrieveData(bedQuery);

            }
            return message;
            //Response.Redirect("~/ConfirmBooking.aspx");

        }

        private void Redirect(string v)
        {
            throw new NotImplementedException();
        }

        private void volBookingsToGrid()
        {
            string sqlQuery = @"SELECT BookingID, CheckInDate, CheckOutDate, NumFemaleGuests, NumMaleGuests, confirmed 
                            FROM Booking INNER JOIN Volunteer ON Volunteer.VolunteerID = Booking.VolunteerID 
                            WHERE Username = '" + Session["Volunteer"].ToString() + "'";
            ConClass connection = new ConClass();
            connection.retrieveData(sqlQuery);


            //ConClass connection = new ConClass();
            //connection.retrieveData(sqlVolBookingQuery);

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("BookingId", typeof(int)),
                    new DataColumn("CheckIn", typeof(string)),
                    new DataColumn("CheckOut", typeof(string)),
                    new DataColumn("NumFemale", typeof(int)),
                    new DataColumn("NumMale", typeof(int)),
                    new DataColumn("Confirmed",typeof(Boolean)) });


            foreach (DataRow dr in connection.SQLTable.Rows)
            {
                int bookingId = (int)dr[0];
                string checkInDate = ((DateTime)dr[1]).ToShortDateString();
                string checkOutDate = ((DateTime)dr[2]).ToShortDateString();
                int numberOfFemale = (int)dr[3];
                int numberOfMale = (int)dr[4];
                Boolean confirmed = (Boolean)dr[5];

                dt.Rows.Add(bookingId, checkInDate, checkOutDate, numberOfFemale, numberOfMale, confirmed);

            }

            this.grdVolunteerBookings.DataSource = dt;
            this.grdVolunteerBookings.DataBind();
        }
        // redirect to ConfirmBooking
        protected void btnConfirm_Click(object sender, EventArgs e)
        {

            if (Convert.ToDateTime(txtStartDate.Text) < DateTime.Now)
            {
                Response.Write("<script>alert('Please select a date in the future')</script>");
            }
            else if (Convert.ToDateTime(txtEndDate.Text) < Convert.ToDateTime(txtStartDate.Text))
            {
                Response.Write("<script>alert('Please select a depart date that is after the arrival date')</script>");
            }
            else
            {
                Response.Redirect("~/ConfirmBooking.aspx");
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