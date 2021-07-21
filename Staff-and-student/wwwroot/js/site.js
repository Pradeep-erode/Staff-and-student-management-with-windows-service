// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function SubmitDetailsa() {

    var file = getElementById("UploadedFile").value();
    if (!file) {
        $("#formfirst").submit();
    }
    else {
        alert("Please select file...");
    };
}

//validatoin for adding new student information 

$(function () {
    $("#validatestudent").validate({
        rules:
        {
            StudentRollNo:
            {
                required: true,
                maxlength: 4,
                number: true, 
            },
            Password:
            {
                required: true,
                maxlength: 4,
                number: true
                
            },
            StudentFirstName:
            {
                required: true,
                maxlength: 40,
                albhabets: true
                
            },
            StudentLastName:
            {
                required: true,
                maxlength: 40,
                letter: true
                
            },
            Gender:
            {
                required: true
            },
            Dob:
            {
                required: true
            },
            FatherFirstName:
            {
                required: true,
                letter: true,
                maxlength: 40
            },
            FatherLastName:
            {
                required: true,
                letter: true,
                maxlength: 40
            },
            MotherFirstName:
            {
                required: true,
                letter: true,
                maxlength: 40
            },
            MotherLastName:
            {
                required: true,
                letter: true,
                maxlength: 40
            },
            Email:
            {
                required: true,
                email: true,
                maxlength: 60
            },
            StudentContactNo:
            {
                required: true,
                number: true,
                maxlength: 10
            },
            FatherSContactNo:
            {
                required: true,
                number: true,
                maxlength: 10
            },
            FatherSOccupation:
            {
                required: true,
                letter: true,
                maxlength:30
            }
        },
        messages:
        {
            StudentRollNo:
            {
                required: "Student roll number is Required...",
                number: "Roll number must be between 1000 to 9999"
                
            },
            Password:
            {
                required: "Password is required",
                maxlength:"maximum 4 digit only...",
                number: "Password accepted in number only"
            },
            StudentFirstName:
            {
                required: "Student First name requied",
                albhabets: "Albhabets only allowed",
                maxlength: "maximum characters reached..."
            },
            StudentLastName:
            {
                required: "Student Last name requied",
                letter: "Albhabets only allowed",
                maxlength: "maximum characters reached..."
            },
            Gender:
            {
                required: "please select your gender",
            },
            Dob:
            {
                required: "Date of birth is required..."
            },
            FatherFirstName:
            {
                required: "Father first name requied",
                letter: "Albhabets only allowed",
                maxlength: "maximum characters reached..."
            },
            FatherLastName:
            {
                required: "Father first name requied",
                letter: "Albhabets only allowed",
                maxlength: "maximum characters reached..."
            },
            MotherFirstName:
            {
                required: "Mother first name requied",
                letter: "Albhabets only allowed",
                maxlength: "maximum characters reached..."
            },
            MotherLastName:
            {
                required: "Mother first name requied",
                letter: "Albhabets only allowed",
                maxlength: "maximum characters reached..."
            },
            Email:
            {
                required: "Email is must",
                email: "please Enter valid Email",
                maxlength: "maximum characters reached..."
            },
            StudentContactNo:
            {
                required: "Student contact number is required",
                number: "please enter valid mobile number...",
                maxlength: "Accepted mobile number is 10 digits only"
            },
            FatherSContactNo:
            {
                required: "Student's fathers contact number is required",
                number: "please enter valid mobile number...",
                maxlength: "Accepted mobile number is 10 digits only"
            },
            FatherSOccupation:
            {
                required: "Fathers occupation field should not empty",
                letter: "Use albhabets only ",
                maxlength: "maximum characters reached"
            }
        }
    });
});
function SubmitDetails() {

    if ($("#validatestudent").validate()) {

        $("#validatestudent").submit();
        
    }
}

//to make roll number=null when edit

$(function () {
    if ($("#Rollnum").val() == 0) {
        $("#Rollnum").val("");
    }
});

//student no null
$(function () {
    if ($("#studcontact").val() == 0) {
        $("#studcontact").val("");
    }
});

//father number null
$(function () {
    if ($("#fathercontact").val() == 0) {
        $("#fathercontact").val("");
    }
});


//validation for Studentlogin

$(function () {
    $("#loginforma").validate({
        rules:
        {
            StudentRollNo:
            {
                required: true,
                maxlength: 4,
                number: true
            },
            Password:
            {
                required: true,
                maxlength: 4,
                number: true

            }
        },
        messages:
        {
            StudentRollNo:
            {
                required: "Student roll number is Required...",
                maxlength:"Roll number must be between 1000 to 9999",
                number: "Enter number only..."

            },
            Password:
            {
                required: "Password is required",
                maxlength: "maximum 4 digit only...",
                number: "Password accepted in number only"
            }
        }
    });
});
function Studentcheck() {

    if ($("#loginforma").validate()) {

        $("#loginforma").submit();

    }
}


//validation for Stafflogin

$(function () {
    $("#stafflogin").validate({
        rules:
        {
            Name:
            {
                required: true
            },
            password:
            {
                required: true,
                maxlength: 4,
                number: true

            }
        },
        messages:
        {
            Name:
            {
                required: "Enter staff name to login...",
            },
            password:
            {
                required: "Password is required",
                maxlength: "maximum 4 digit only...",
                number: "Password accepted in number only"
            }
        }
    });
});
function StaffDetails() {

    if ($("#stafflogin").validate()) {

        $("#stafflogin").submit();

    }
}

//for loading logo which is used in Layout

$("#btnsubmit").click(function () {
    $('.spin').css('display', 'block');
});


$("#updatespin").click(function () {
    $('.spincenter').css('display', 'block');
});