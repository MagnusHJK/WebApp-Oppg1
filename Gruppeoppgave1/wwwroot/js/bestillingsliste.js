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
            "<td>" + dato.toDateString() + "</td>" +
            "<td>" + dato.toTimeString() + "</td>" +
            "<td>" + bestilling.antall + "</td>" +
            "<td>" + totalpris + ",- </td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#bestillinger").html(ut);
}