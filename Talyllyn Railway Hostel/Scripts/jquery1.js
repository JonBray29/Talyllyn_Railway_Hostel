$(document).ready(function () {
    var max_fields = 12; //maximum input boxes allowed
    var wrapper = $(".wrapper"); //Fields wrapper
    var add_button = $(".btnAddPerson"); //Add button ID
    var htmlCode = {
        row: function (f) {
            return '<div class="input-group mb-3"><div class="input-group-prepend"><span class="input-group-text">FirstName:</span></div><input class="input form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" id="FirstName[' + f + '][]" /><div class="input-group-prepend"><span class="input-group-text">Surname:</span></div><input class="input form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" id="Surname[' + f + '][]" /><div class="input-group-prepend"><span class="input-group-text">Date Of Birth:</span></div><input type="date" class="input form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" id="DOB[' + f + '][]" /><div class="input-group-prepend"><span class="input-group-text">Gender:</span></div><select class="input form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" id="gender[' + f + '][]"><option value="Male">Male</option><option value="Female">Female</option></select><a href="#" class="input remove_field btn btn-outline-danger">Remove</a></div>'; //HTML for each new line
        }
    };

    var x = 0; //initlal text box count
    $(add_button).click(function (e) { //on add input button click
        e.preventDefault();
        if (x < max_fields) { //max input box allowed
            $(wrapper).append(htmlCode.row(x)); //Append div wrapper with new line of html code
            x++; //text box increment
        }
    });

    $(wrapper).on("click", ".remove_field", function (e) {
        e.preventDefault(); $(this).parent('div').remove();
        x--;
    });

    $('#btnConfirm').click(function () {
        makeBooking();
        getValues();
    });
});

function makeBooking() {
    $.ajax({
        type: 'POST',
        url: 'CreateBooking.aspx/makeBooking',
        data: "{'volID':'" + $('#txtID').val() + "', 'chkIn':'" + $('#txtStartDate').val() + "', 'chkOut':'" + $('#txtEndDate').val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //alert(response.d);
        },
    });
}

function getValues() {
    var values = [];
    $('.input').each(function () {
        if (this.value != '')
            values.push(this.value);
    });

    if (values != '') {
        //Pass values back through to the aspx.cs web method
        $.ajax({
            type: 'POST',
            url: 'CreateBooking.aspx/loadFields',
            data: "{'volID':'" + $('#txtID').val() + "', 'chkIn':'" + $('#txtStartDate').val() + "', 'chkOut':'" + $('#txtEndDate').val() + "', 'fields':'" + values + "', 'table': '" + $('#Guest').val() + "'}",
            dataType: 'json',
            headers: { "Content-Type": "application/json" },
            success: function (response) {
                values = [];    // EMPTY THE ARRAY.
                //alert(response.d);
            },
        });
    }
}