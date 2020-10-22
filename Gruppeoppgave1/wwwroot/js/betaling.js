$(function () {
    var urlParametere = new URLSearchParams(window.location.search);
    var brukerId = urlParametere.get('bruker');
    hentBestillinger(brukerId);
});

//Gir en tilbakemelding utifra hvordan det gikk på backend, forsvinner etter 15 sek
function statusMelding(tekst, sannhet) {
    if (sannhet) {
        $("#statusMeldingOK").html(tekst);
        $("#statusMeldingOK").fadeIn('fast').delay(10000).fadeOut('slow');
    } else {
        $("#statusMeldingFeil").html(tekst);
        $("#statusMeldingFeil").fadeIn('fast').delay(10000).fadeOut('slow');
    }
}


function test() {
    alert("ello");
}


function hentBestillinger(brukerId) {
    const url = "Bestilling/HentBestillinger?brukerId=" + brukerId

    $.get(url, function (bestillinger) {
        formaterBestillinger(bestillinger);
    });
}


function formaterBestillinger(bestillinger) {
    const datoOptions = { year: 'numeric', month: 'numeric', day: 'numeric' };
    const tidOptions = { hour12: false, hour: 'numeric', minute: 'numeric', second: 'numeric' };

    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Fra</th>" +
        "<th>Til</th>" +
        "<th>Dato</th>" +
        "<th>Avreise</th>" +
        "<th>Antall billetter</th>" +
        "<th>Total pris</th>" +
        "</tr>";
    for (let bestilling of bestillinger) {
        var totalpris = (bestilling.antall * bestilling.avgang.pris);
        var dato = new Date(bestilling.avgang.dato);

        ut += "<tr>" +
            "<td>" + bestilling.avgang.stasjonFra.navn + "</td>" +
            "<td>" + bestilling.avgang.stasjonTil.navn + "</td>" +
            "<td>" + dato.toLocaleDateString(undefined, datoOptions) + "</td>" +
            "<td>" + dato.toLocaleTimeString(undefined, tidOptions) + "</td>" +
            "<td>" + bestilling.antall + "</td>" +
            "<td>" + totalpris + ",- </td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#bestillinger").html(ut);
}


function betalingsMetodeEndring() {
    var betalingsValg = $("#betalingsMetodeValg").val();

    if (betalingsValg == 1) {
        $("#kort").hide();
        $("#vipps").show();
    } else {
        $("#vipps").hide();
        $("#kort").show();
    }
}


function betaling() {
    var sannhet = false;
    var betalingsValg = $("#betalingsMetodeValg").val();

    //Vipps
    if (betalingsValg == 1) {
        const telefonnrOK = validerTelefonnr($("#vippsTelefonnr").val());
        if (telefonnrOK) {
            sannhet = true;
        } else {
            sannhet = false;
        }
    }
    //Kort
    else {
        var kortNr = $("#kortNr").val();
        var kortCVC = $("#kortCVC").val();

        const kortOK = validerKort(kortNr, kortCVC);

        if (kortOK) {
            sannhet = true;
        } else {
            sannhet = false;
        }
    }

    if (sannhet) {
        $("#email").show();
        statusMelding("Betaling godkjent!", true);
    } else {
        //statusMelding("Betaling ikke godkjent!", false);
    }
}

function sendMail() {
    var email = $("#emailInput").val();
    const emailOK = validerEmail(email);

    if (emailOK) {
        var urlParametere = new URLSearchParams(window.location.search);
        var bruker = urlParametere.get('bruker');
        alert(bruker + " " + email);

        const url = "Bestilling/SendBestillingMail";

        var data = {
            tilMail: email,
            brukerId: bruker
        };

        $.get(url, data, function () {
            statusMelding("Mail sendt", true);
        })
            .fail(function () {
                statusMelding("Mail ble ikke sendt", false);
            });
    }
}