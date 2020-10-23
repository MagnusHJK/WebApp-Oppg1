$(function () {
    sjekkInnlogget();
});


function loggInn() {
    const brukernavnOK = validerBrukernavn($("#brukernavn").val());
    const passordOK = validerPassord($("#passord").val());

    if (brukernavnOK && passordOK) {
        const bruker = {
            brukernavn: $("#brukernavn").val(),
            passord: $("#passord").val()
        }

        $.post("Bruker/LoggInn", bruker, function (OK) {
            if (OK) {
                window.location.href = 'admin.html';
            }
            else {
                $("#feil").html("Feil brukernavn eller passord");
            }
        })
            .fail(function () {
                $("#feil").html("Feil på server");
            });
    }
}

//Sjekker om man er logget inn, hvis man er det kan man gå rett til admin.html og slippe logginn.html
function sjekkInnlogget() {
    const url = "Bruker/SjekkInnlogget";

    $.get(url, function () {
        $("#formAdminpanel").attr("action", "admin.html");
    })
        .fail(function () {
            $("#formAdminpanel").attr("action", "logginn.html");
        });
}