using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace TalyllynRailwayHostel
{
    public partial class AssignBeds : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    setTextboxes();
            //}
            setTextboxes();
            this.btnBack.Click += BtnBack_Click;
            this.btnConfirm.Click += BtnConfirm_Click;
        }

        private string getBookingID()
        {
            string booking = Session["bookingID"].ToString();
            return booking;
        }

        //get number of textboxes and values
        private int getNumOfPeople()
        {
            string booking = getBookingID();

            string genderQuery = @"SELECT NumFemaleGuests, NumMaleGuests
                                FROM Booking  WHERE BookingID = '" + booking + "'";
            ConClass genderConnect = new ConClass();
            genderConnect.retrieveData(genderQuery);

            //Get number of people linked to booking
            int n = (int)genderConnect.SQLTable.Rows[0].ItemArray.GetValue(0) + (int)genderConnect.SQLTable.Rows[0].ItemArray.GetValue(1);
            return n;
        }

        private DropDownList[] dropdown;
        private TextBox[] textBox;

        private void setTextboxes()
        {
            string booking = getBookingID();

            string volunteerQuery = @"SELECT Volunteer.VolunteerID, Volunteer.FirstName, 
                                    Volunteer.Surname, Volunteer.Gender
                                    FROM BedAssignment JOIN Volunteer ON BedAssignment.VolunteerID = Volunteer.VolunteerID
                                    WHERE BedAssignment.BookingID = '" + booking + "'";
            ConClass volunteerConnect = new ConClass();
            volunteerConnect.retrieveData(volunteerQuery);

            string guestQuery = @"SELECT Guest.GuestID, Guest.FirstName, 
                                    Guest.Surname, Guest.Gender
                                    FROM BedAssignment JOIN Guest ON BedAssignment.GuestID = Guest.GuestID
                                    WHERE BedAssignment.BookingID = '" + booking + "'";
            ConClass guestConnect = new ConClass();
            guestConnect.retrieveData(guestQuery);

            string datesQuery = @"SELECT CheckInDate, CheckOutDate
                                FROM Booking WHERE BookingID = '" + booking + "'";
            ConClass datesConnect = new ConClass();
            datesConnect.retrieveData(datesQuery);

            string checkIn = ((DateTime)datesConnect.SQLTable.Rows[0].ItemArray.GetValue(0)).ToString("yyyy-MM-dd");
            string checkOut = ((DateTime)datesConnect.SQLTable.Rows[0].ItemArray.GetValue(1)).ToString("yyyy-MM-dd");

            string bedNotExistQuery = @"SELECT Bed.BedID FROM Bed
                                        WHERE NOT EXISTS (SELECT BedID FROM BedAssignment WHERE BedAssignment.BedID = Bed.BedID)";
            ConClass bedNotExistConnect = new ConClass();
            bedNotExistConnect.retrieveData(bedNotExistQuery);

            //Update query to check dates
            string bedQuery = @"SELECT Bed.BedID FROM Bed
                                JOIN BedAssignment ON Bed.BedID = BedAssignment.BedID
                                JOIN Booking ON BedAssignment.BookingID = Booking.BookingID
                                WHERE('" + checkIn + "' NOT BETWEEN Booking.CheckInDate AND Booking.CheckOutDate)" +
                                "AND('" + checkOut + "' NOT BETWEEN Booking.CheckInDate AND Booking.CheckOutDate)" +
                                "AND(Booking.CheckInDate NOT BETWEEN '" + checkIn + "' AND '" + checkOut + "')" +
                                "AND(Booking.CheckOutDate NOT BETWEEN '" + checkIn + "' AND '" + checkOut + "')";

            ConClass bedConnect = new ConClass();
            bedConnect.retrieveData(bedQuery);

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("BedID", typeof(string)) });

            foreach (DataRow dr in bedConnect.SQLTable.Rows)
            {
                string bedId = (string)dr[0];

                dt.Rows.Add(bedId);
            }

            foreach (DataRow dr in bedNotExistConnect.SQLTable.Rows)
            {
                string bedId = (string)dr[0];

                dt.Rows.Add(bedId);
            }

            dt.DefaultView.Sort = "BedID ASC";
            int n = getNumOfPeople();
            int i = 0;
            int j = 0;
            textBox = new TextBox[n];
            TextBox[] txtBox = new TextBox[n];
            Label[] label = new Label[n];
            dropdown = new DropDownList[n];
            Panel[] panel = new Panel[n];
            Panel[] subPanel = new Panel[n * 5];

            //Fill Volunteer
            foreach (DataRow dr in volunteerConnect.SQLTable.Rows)
            {
                panel[i] = new Panel();
                panel[i].CssClass = "input-group";
                form1.Controls.Add(panel[i]);


                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " Volunteer ID: ";
                label[i].CssClass = "input-group-text";
                textBox[i] = new TextBox();
                textBox[i].Text = Convert.ToString((int)dr[0]);
                textBox[i].ReadOnly = true;
                textBox[i].CssClass = "form-control";
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(textBox[i]);

                j++;

                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " FirstName: ";
                label[i].CssClass = "input-group-text";
                txtBox[i] = new TextBox();
                txtBox[i].Text = (string)dr[1];
                txtBox[i].ReadOnly = true;
                txtBox[i].CssClass = "form-control";
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(txtBox[i]);

                j++;

                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " Surname: ";
                label[i].CssClass = "input-group-text";
                txtBox[i] = new TextBox();
                txtBox[i].Text = (string)dr[2];
                txtBox[i].ReadOnly = true;
                txtBox[i].CssClass = "form-control";
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(txtBox[i]);

                j++;

                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " Gender: ";
                label[i].CssClass = "input-group-text";
                txtBox[i] = new TextBox();
                txtBox[i].Text = (string)dr[3];
                txtBox[i].ReadOnly = true;
                txtBox[i].CssClass = "form-control";
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(txtBox[i]);

                j++;

                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " Bed: ";
                label[i].CssClass = "input-group-text";
                dropdown[i] = new DropDownList();
                dropdown[i].AutoPostBack = true;
                dropdown[i].CssClass = "form-control";
                dropdown[i].SelectedIndexChanged += new EventHandler(dropdown_SelectedValueChanged);
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(dropdown[i]);


                dropdown[i].DataSource = dt;
                dropdown[i].DataTextField = "BedID";
                dropdown[i].DataValueField = "BedID";
                dropdown[i].DataBind();
                dropdown[i].Items.Insert(0, new ListItem(" Choose Bed ", "0"));
                dropdown[i].SelectedIndex = 0;


                panel[i].Controls.Add(new LiteralControl("<br />"));
                panel[i].Controls.Add(new LiteralControl("<br />"));
            }

            //Fill guests
            foreach (DataRow dr in guestConnect.SQLTable.Rows)
            {
                i++;
                j++;

                panel[i] = new Panel();
                panel[i].CssClass = "input-group";
                form1.Controls.Add(panel[i]);

                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " Guest ID: ";
                label[i].CssClass = "input-group-text";
                textBox[i] = new TextBox();
                textBox[i].Text = Convert.ToString((int)dr[0]);
                textBox[i].ReadOnly = true;
                textBox[i].CssClass = "form-control";
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(textBox[i]);


                j++;

                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " FirstName: ";
                label[i].CssClass = "input-group-text";
                txtBox[i] = new TextBox();
                txtBox[i].Text = (string)dr[1];
                txtBox[i].ReadOnly = true;
                txtBox[i].CssClass = "form-control";
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(txtBox[i]);

                j++;

                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " Surname: ";
                label[i].CssClass = "input-group-text";
                txtBox[i] = new TextBox();
                txtBox[i].Text = (string)dr[2];
                txtBox[i].ReadOnly = true;
                txtBox[i].CssClass = "form-control";
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(txtBox[i]);

                j++;


                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " Gender: ";
                label[i].CssClass = "input-group-text";
                txtBox[i] = new TextBox();
                txtBox[i].Text = (string)dr[3];
                txtBox[i].ReadOnly = true;
                txtBox[i].CssClass = "form-control";
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(txtBox[i]);

                j++;

                subPanel[j] = new Panel();
                subPanel[j].CssClass = "input-group-prepend";
                label[i] = new Label();
                label[i].Text = " Bed: ";
                label[i].CssClass = "input-group-text";
                dropdown[i] = new DropDownList();
                dropdown[i].AutoPostBack = true;
                dropdown[i].CssClass = "form-control";
                dropdown[i].SelectedIndexChanged += new EventHandler(dropdown_SelectedValueChanged);
                subPanel[j].Controls.Add(label[i]);
                panel[i].Controls.Add(subPanel[j]);
                panel[i].Controls.Add(dropdown[i]);

                dropdown[i].DataSource = dt;
                dropdown[i].DataTextField = "BedID";
                dropdown[i].DataValueField = "BedID";
                dropdown[i].DataBind();
                dropdown[i].Items.Insert(0, new ListItem(" Choose Bed ", "0"));
                dropdown[i].SelectedIndex = 0;

                //Should update dropdown after one has a selected value


                panel[i].Controls.Add(new LiteralControl("<br />"));
                panel[i].Controls.Add(new LiteralControl("<br />"));
            }

        }

        private void dropdown_SelectedValueChanged(Object sender, EventArgs e)
        {

            foreach (DropDownList d in dropdown)
            {
                string value = d.SelectedValue;

                foreach (DropDownList dr in dropdown)
                {
                    if (dr.SelectedValue != value)
                    {
                        dr.Items.Remove(dr.Items.FindByValue(value));
                    }
                }
            }

        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WardenUpdate.aspx");
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {

            string booking = getBookingID();
            int n = getNumOfPeople();

            for (int i = 0; i < n; i++)
            {
                string bed = dropdown[i].SelectedValue;
                string id = textBox[i].Text;

                string bedQuery = @"UPDATE BedAssignment SET BedID = '" + bed + "'" +
                                "WHERE (BookingID = '" + booking + "' AND VolunteerID = '" + id + "')" +
                                "OR (BookingID = '" + booking + "' AND GuestID = '" + id + "')";
                ConClass bedConnect = new ConClass();
                bedConnect.retrieveData(bedQuery);
            }
            string confirmQuery = @"UPDATE Booking SET Confirmed = 'True'
                                WHERE BookingID = '" + booking + "'";
            ConClass confirmConnect = new ConClass();
            confirmConnect.retrieveData(confirmQuery);
            Response.Redirect("~/WardenUpdate.aspx");
            //-----------------------------------------------------------------
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Remove("Admin");
            Response.Redirect("~/LoginPage.aspx");
        }
    }
}