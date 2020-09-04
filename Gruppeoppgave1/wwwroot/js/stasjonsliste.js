$(function () {
    hentAlleStasjoner();
});

function hentAlleStasjoner() {
    $.get("Stasjon/HentAlleStasjoner", function (stasjoner) {
        formaterStasjoner(stasjoner);
    });
}


function formaterStasjoner(stasjoner) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Navn</th>" +
        "</tr>";
    for (let stasjon of stasjoner) {
        ut += "<tr>" +
            "<td>" + stasjon.navn + "</td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#stasjoner").html(ut);
}