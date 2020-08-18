
$(function () {

    $.validator.addMethod("isValidPassword", function (value, element) {
        return this.optional(element) ||
            (/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$/i).test(value);
    }, "Dužina lozinke je minimalno 8 karaktera. Lozinka se sastoji od malih i velikih slova, brojeva te specijalnih karaktera.");

    $("#uredi-profil").validate(
        {
            rules: {
                ime: {
                    required: true,
                    minlength: 2,
                    maxlength: 100
                },
                prezime: {
                    required: true,
                    minlength: 3,
                    maxlength: 100
                },
                telefon: {
                    required: true,
                    maxlength: 20
                }
            },
            messages: {
                ime: {
                    required: "Molimo unesite ime.",
                    minlength: "Ime je prekratko.",
                    maxlength: "Ime je predugo."
                },
                prezime: {
                    required: "Molimo unesite prezime.",
                    minlength: "Prezime je prekratko.",
                    maxlength: "Prezime je predugo."
                },
                telefon: {
                    required: "Molimo unesite telefonski broj.",
                    maxlength: "Molimo unesite validan telefonski broj."
                }
            },
            errorPlacement: function (error, element) {
                if (element.attr("name") == "ime")
                    $(".name-error-message").append(error);
                if (element.attr("name") == "prezime")
                    $(".surname-error-message").append(error);
                if (element.attr("name") == "telefon")
                    $(".telephone-error-message").append(error);
            }
        }
    );
    $("#promijeni-lozinku").validate(
        {
            rules: {
                trenutnaLozinka: {
                    required: true
                },
                novaLozinka: {
                    required: true,
                    isValidPassword: true
                },
                potvrdaNoveLozinke: {
                    required: true,
                    equalTo: $("input[name='novaLozinka']")
                }
            },
            messages: {
                trenutnaLozinka: {
                    required: "Molimo unesite trenutnu lozinku."
                },
                novaLozinka: {
                    required: "Molimo unesite novu lozinku."
                },
                potvrdaNoveLozinke: {
                    required: "Molimo ponovo unesite novu lozinku.",
                    equalTo: "Lozinke se razlikuju!"
                }
            },
            errorPlacement: function (error, element) {
                if (element.attr("name") == "trenutnaLozinka")
                    $(".current-password-error-message").append(error);
                if (element.attr("name") == "novaLozinka")
                    $(".new-password-error-message").append(error);
                if (element.attr("name") == "potvrdaNoveLozinke")
                    $(".check-new-password-error-message").append(error);
            }
        }
    );
});