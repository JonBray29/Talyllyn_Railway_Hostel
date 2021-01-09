<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WardenBookings.aspx.cs" Inherits="TalyllynRailwayHostel.WardenBookings" %>

<!doctype html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Llechfan Volunteer Hostel - Talyllyn Railway</title>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-3.3.1.min.js"></script>
    <link rel='stylesheet' href='https://www.talyllyn.co.uk/wp-content/themes/talyllyn/dist/app.css?ver=1.5'  />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.1/css/all.css"/>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"/>

</head>

<body>
    <form id="form2" runat="server">
            <div class="off-canvas-content data-off-canvas-content">
                <div class = "data-sticky-container">

                    <header class="sticky data-sticky" data-options="marginTop:0;" style="width:100%" data-top-anchor="0" data-btm-anchor="content:bottom" data-sticky-on="small">

                        <div class="top-bar">
                            <div class="top-bar-left">
                                <a class="top-bar__logo" href="/"><img src="https://www.talyllyn.co.uk/wp-content/themes/talyllyn/images/talyllyn-logo.png" alt="Rheilfford Talyllyn Railway Logo"/></a>
                            </div>
                            <div class="top-bar-right">
                                <ul class="top-bar-right__menu top-bar-right__menu--push top-bar-right__row-1 hide-for-small-only">
                                    <asp:Button ID="btnLogout" class="btn btn-light" runat="server" Text="Logout" OnClick="btnLogout_Click" />
                                </ul>
                            </div>
                        </div>
                    </header>
                </div>


                <div class="pointer pointer--grey">
                    <div class="pointer__container">
                        <h1 class="pointer__heading">Llechfan Volunteer Hostel</h1><p class="pointer__text"></p>
                    </div>

                    <div class="pointer__bottom pointer__bottom--top-position">
                        <svg class="svg-content" viewBox="0 0 500 28">
                            <polygon points="0,0 500,0 250,28" ></polygon>
                        </svg>
                    </div>
                </div>
            </div>

        <br />
        <br />
        <br />
        <br />
        <br />

        <div style="position: static; margin-left: 10px;">
            <asp:Button ID="btnPast" class="btn btn-outline-success" runat="server" Text="View Past Bookings" OnClick="btnPast_Click" />
        </div>
        <br />
        <br />

        <h2>Uncomfirmed Bookings:</h2>

        <div class="row text-center">
                <div class="col">
                    <asp:GridView ID="grdBookings" Width="100%" runat="server" >
                        <Columns>
                            <asp:ButtonField Text="Select" ControlStyle-CssClass="btn btn-outline-info" CommandName="Select" ItemStyle-Width="100"/>
                        </Columns>
                    </asp:GridView>                   
                </div>
            </div>

        <br />
        <br />

        <h2>Upcoming Bookings:</h2>

        <div class="input-group mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text">Date from:</span>
            </div>
                <asp:TextBox runat="server" ID="txtDateFrom" TextMode="Date" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default"/>
            <div class="input-group-prepend">
                <span class="input-group-text">Date to:</span>
            </div>
                <asp:TextBox runat="server" ID="txtDateTo" TextMode="Date" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default"/>
                <asp:Button ID="btnSearch" class="btn btn-outline-success" runat="server" Text="Search" OnClick="btnSearch_Click" />
        </div>

        <br />
        <br />

        <div class="row text-center">
            <div class ="col">
                <asp:GridView ID="grdConfirmed" Width="100%" runat="server" >
                        <Columns>
                            <asp:ButtonField Text="Select"  ControlStyle-CssClass="btn btn-outline-info" CommandName="Select" ItemStyle-Width="100"/>
                        </Columns>
                    </asp:GridView>
            </div>
        </div>

    </form>

    <footer>
        <div class="grid-container">
            <div class="grid-x grid-x-margin">
                <div class="cell medium-6">
                    <ul class="hide-for-small-only">
                        <li><img src="https://www.talyllyn.co.uk/wp-content/themes/talyllyn/images/talyllyn-logo.png" alt="Rheilfford Talyllyn Railway Logo"/></li>
                        <li><img src="https://www.talyllyn.co.uk/wp-content/themes/talyllyn/images/cymri-wales.png" alt="Cymru Wales Logo"/></li>
                        <li><a target="_blank" href="https://www.tripadvisor.co.uk/Attraction_Review-g552038-d2663321-Reviews-Talyllyn_Railway-Tywyn_Gwynedd_North_Wales_Wales.html"><img src="https://www.talyllyn.co.uk/wp-content/themes/talyllyn/images/tallylyn_trip-advisor-excellence-Award-2017.png" alt="Trip advisor approved"/></a></li>
                        <li>
                            <a href="tel:01654 710472"><i class="fa fa-phone"></i>01654 710472</a>
                            <a href="mailto:enquiries@talyllyn.co.uk"><i class="fa fa-envelope"></i>enquiries@talyllyn.co.uk</a>
                        </li>
                    </ul>
                </div>
                <div class="cell medium-6">
                    <ul class="text-right footer__social">
                        <li><small>Let's get social</small></li>
                        <li><a target="_blank" href="https://www.facebook.com/Talyllyn/"><i class="fab fa-facebook-square"></i></a></li>
                        <li><a target="_blank" href="https://twitter.com/TalyllynRailway?ref_src=twsrc%5Egoogle%7Ctwcamp%5Eserp%7Ctwgr%5Eauthor"><i class="fab fa-twitter-square"></i></a></li>
                        <li><a target="_blank" href="https://www.instagram.com/talyllynrailway/"><i class="fab fa-instagram"></i></a></li>
                        <li><a target="_blank" href="https://www.youtube.com/user/TalyllynRail"><i class="fab fa-youtube-square"></i></a></li>
                        <li><a target="_blank" href="https://www.flickr.com/groups/talyllyn_railway/"><i class="fab fa-flickr"></i></a></li>

                    </ul>

                    <ul class="text-right footer--small">
                        <li id="menu-item-103" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-103"><a href="https://www.talyllyn.co.uk/terms-conditions">T&#038;Cs</a></li>
                        <li id="menu-item-102" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-102"><a href="https://www.talyllyn.co.uk/privacy-policy">Privacy &#038; Cookies Policy</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </footer>


</body>
</html>




