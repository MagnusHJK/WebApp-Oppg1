$(function () {
    hentAlleBestillinger();
});

function hentAlleBestillinger() {
    $.get("Bestilling/HentAlleBestillinger", function (bestillinger) {
        formaterBestillinger(bestillinger);
    });
}

function formaterBestillinger(bestillinger) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Fra</th><th>Til</th><th>Dato</th><th>Tidspunkt</th>" +
        "</tr>";
    for (let bestilling of bestillinger) {
        ut += "<tr>" +
            "<td>" + bestilling.avgang.stasjonFra.navn + "</td><td>" + bestilling.avgang.stasjonTil.navn + "</td><td>" + bestilling.avgang.dato + "</td><td>" + bestilling.avgang.tidspunkt + "</td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#bestillinger").html(ut);
}