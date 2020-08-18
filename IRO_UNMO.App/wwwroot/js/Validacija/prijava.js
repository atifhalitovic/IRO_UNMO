$(function () {
    $.validator.addMethod("isValidEmail", function (value, element) {
        return this.optional(element) ||
            (/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/i).test(value);
    }, "Please enter valid e-mail address.");    $("#registration").validate(
        {
            rules: {
                name: {
                    required: true,
                    minlength: 2,
                    maxlength: 100
                },
                surname: {
                    required: true,
                    minlength: 3,
                    maxlength: 100
                },
                mobile: {
                    required: true,
                    minlength: 2,
                    maxlength: 20
                },
                studyfield: {
                    required: true,
                    minlength: 2,
                    maxlength: 40
                },
                email: {
                    required: true,
                    maxlength: 320,
                    isValidEmail: true
                },
                nationality: {
                    required: true,
                }
            },
            messages: {
                name: {
                    required: "Please enter your name.",
                    minlength: "Name is too short.",
                    maxlength: "Name is too long."
                },
                surname: {
                    required: "Please enter your surname.",
                    minlength: "Surname is too short.",
                    maxlength: "Surname is too long."
                },
                mobile: {
                    required: "Please enter mobile number.",
                    maxlength: "Please enter valid mobile number."
                },
                email: {
                    required: "Please enter e-mail address.",
                    maxlength: "Please enter valid e-mail address."
                },
                studyfield: {
                    required: "Please enter study field.",
                    maxlength: "Please enter valid e-mail address."
                },
                nationality: {
                    required: "Please choose your nationality.",
                },
            },
            errorPlacement: function (error, element) {
                if (element.attr("name") == "name")
                    $(".name-error-message").append(error);
                if (element.attr("name") == "surname")
                    $(".surname-error-message").append(error);
                if (element.attr("name") == "mobile")
                    $(".mobile-error-message").append(error);
                if (element.attr("name") == "email")
                    $(".email-error-message").append(error);
                if (element.attr("name") == "studyfield")
                    $(".studyfield-error-message").append(error);
                if (element.attr("name") == "nationality")
                    $(".nationality-error-message").append(error);
            }
        }
    );
});