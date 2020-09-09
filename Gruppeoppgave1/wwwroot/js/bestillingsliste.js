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
        "<th>StasjonFra</th><th>StasjonTil</th><th>Dato</th><th>Tidspunkt</th>" +
        "</tr>";
    for (let bestilling of bestillinger) {
        ut += "<tr>" +
            "<td>" + bestilling.stasjonFra + "</td><td>" + bestilling.stasjonTil + "</td><td>" + bestilling.dato + "</td><td>" + bestilling.tidspunkt + "</td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#bestillinger").html(ut);
}