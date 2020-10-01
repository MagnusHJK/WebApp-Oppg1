//Sjekker om avganger eksisterer, hvis de gjør det hentes de. Hvis ikke blir de generert og så hentet
function sjekkAvganger(stasjonFra, stasjonTil, dato) {
    let url = "Avgang/SjekkAvganger";
    let avgang = {
        stasjonFraId: stasjonFra.id,
        stasjonTilId: stasjonTil.id,
        dato: dato
    }

    //Sjekker om avganger mellom stasjoner eksisterer, hvis ikke blir de generert
    $.get(url, avgang, function () {
        hentAvganger(avgang);
    })
        .fail(function () {
            genererAvganger(avgang);
        });
}

//Genererer avganger
function genererAvganger(avgang) {
    let url = "Avgang/GenererAvganger";

    $.get(url, avgang, function () {
        hentAvganger(avgang);
    })
        .fail(function () {
            $("#feil").html("Kunne ikke generere");
        });
}

//Henter avganger for gitt strekning og dato
function hentAvganger(avgang) {
    const url = "Avgang/HentAvganger";

    $.get(url, avgang, function (avganger) {
        formaterAvganger(avganger);
    });
}

//Formaterer de hentede avgangene i table
function formaterAvganger(avganger) {
    $("#avganger").show();

    var dato = new Date(avganger[0].dato);

    let ut = "<h3>Avganger for " + dato.toDateString() + "</h3>" +
        "<table class='table table-hover'>" +
        "<tr>" +
        "<th>Avreise tidspunkt</th>" +
        "<th>Pris per billett</th>" +
        "<th></th>" +
        "</tr>";
    for (let avgang of avganger) {
        var datoAvgang = new Date(avgang.dato);

        ut += "<tr>" +
            "<td>" + datoAvgang.toTimeString() + "</td>" +
            "<td>" + avgang.pris + ",-</td>" +
            "<td><button class='btn btn-success' onclick = 'lagBillett(\"" + avgang.id + "\", \"" + datoAvgang.toISOString() + "\", \"" + avgang.pris + "\")' > Velg</button ></td > " +
            "</tr>";
    }
    ut += "</table>";

    $("#avganger").html(ut);
}