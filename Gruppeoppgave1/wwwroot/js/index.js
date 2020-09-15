$(function () {
    hentAlleStasjoner();
    sessionStorage.clear();
});

//vet dette er dumt men...
var stasjonTil;
var stasjonFra;

function hentAlleStasjoner() {
    $.get("Stasjon/HentAlleStasjoner", function (stasjoner) {
        stasjonerAutocomplete(stasjoner);
    });
}
//Autocompleter søk på stasjoner
function stasjonerAutocomplete(stasjoner) {
    $(".stasjonerAutocomplete").autocomplete({
        minLength: 2,
        source: function (request, response) {
            response($.map(stasjoner, function (obj, key) {

                var navn = obj.navn.toUpperCase();

                if (navn.indexOf(request.term.toUpperCase()) != -1) {
                    return {
                        label: obj.navn,
                        value: obj.id,
                        obj: obj
                    }
                } else {
                    return null;
                }
            }));
        },
        //Hva som skjer når stasjon får fokus
        focus: function (event, ui) {
            event.preventDefault();
            return false;
        },
        //Hva som skjer når en stasjon blir valgt
        //ui.item er objektet valgt i dropdown, den har to attributter. Label som er teksten(navn) og value som er indeks i db
        select: function (event, ui) {
            var stasjon = ui.item.obj;
            stasjonsValg(stasjon);
            return false; 
            //Sett inn if som sjekker om stasjon til er lik fra.
        }
    });
}

//Velger stasjoner og lagrer hvilken stasjon som er valgt som til eller fra
function stasjonsValg(stasjon) {
    var step = sessionStorage.getItem("step");

    //Det er første stasjons valg
    if (step == null || step == 1) {
        sessionStorage.setItem("step", 2);
        stasjonFra = stasjon;
        $("#tilSøk").show("slow");
        lagDestinasjonsBoksFra(stasjon);
    }
    //Andre valg
    else {
        sessionStorage.setItem("step", 2);
        stasjonTil = stasjon;
        lagDestinasjonsBoksTil(stasjon);
    }
}


//Lager bokser etter at man har valg en stasjon enten fra eller til
function lagDestinasjonsBoksFra(stasjon) {
    $("#fraSøk").hide("slow");
    var ut = "";

    ut += '<b style="font-size:3em;">Fra: </b>' +
          '<b style="font-size:2.5em;" id="stasjonFraNavn">' + stasjon.navn + '</b>' +
          '<button class="btn btn-warning" style="display:flex;justify-content:flex-end;align-items:center"' +
          'onclick="endreStasjon(1)">Endre</button></br>';

    //stasjonFraVar = stasjon.navn;

    $("#fraBoks").html(ut);
    $("#fraBoks").show("slow");

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    if (($("#tilBoks").is(":visible")) && ($("#fraBoks").is(":visible"))) {
        lagBestillingBoks();
    }
}


function lagDestinasjonsBoksTil(stasjon) {
    $("#tilSøk").hide("slow");
    var ut = "";

    ut += '<b style="font-size:3em;">Til: </b>' +
          '<b style="font-size:2.5em;"id="stasjonTilNavn">' + stasjon.navn + '</b>' +
          '<button class="btn btn-warning" style="display:flex;justify-content:flex-end;align-items:center"' +
          'onclick="endreStasjon(2)">Endre</button></br>';

    //stasjonTilVar = stasjon.navn;

    $("#tilBoks").html(ut);
    $("#tilBoks").show("slow");

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    if (($("#tilBoks").is(":visible")) && ($("#fraBoks").is(":visible"))) {
        lagBestillingBoks();
    }
}

//Boks for dato og billett valg
function lagBestillingBoks() {
<<<<<<< Updated upstream
    var dato;
    //FIKS TID FRA VALG
    var tidspunkt = "12:00";
    $("#bestillingsBoks").show("slow");
    $("#datovalg").datepicker({
        minDate: 0,
        firstDay: 1,
        onSelect: function (dateText, inst) {
            dato = dateText;
            //Ligger her midlertidig slik at den ikke alltid blir "triggered"
            sjekkAvganger(stasjonFra, stasjonTil, dato, tidspunkt);
        }
    });

    //lagBestilling(stasjonFraId, stasjonTilId, dato, tidspunkt);
}

//Sjekker om avganger eksisterer, hvis de gjør det hentes de. Hvis ikke blir de generert og så hentet
function sjekkAvganger(stasjonFra, stasjonTil, dato, tidspunkt) {
    let url = "Avgang/SjekkAvganger";
    let data = {
        stasjonFraId: stasjonFra.id,
        stasjonTilId: stasjonTil.id,
        dato: dato
    }
    let ut = "";

    for (i = time; i < 24; i++) {
        ut += "Tid: " + i + "</br>";
    }

    $("#avganger").html(ut);
}

//Sjekker om avganger eksisterer, hvis de gjør det hentes de. Hvis ikke blir de generert og så hentet
function sjekkAvganger(stasjonFra, stasjonTil, dato, tidspunkt) {
    let url = "Avgang/SjekkAvganger";
    let data = {
        stasjonFraId: stasjonFra.id,
        stasjonTilId: stasjonTil.id,
        dato: dato
    }
    let ut = "";

    //Sjekker om avganger mellom stasjoner eksisterer, hvis ikke blir de generert
    $.get(url, data, function (OK) {
        if (OK) {
            hentAvganger(stasjonFra.id, stasjonTil.id, dato, tidspunkt);
            //hentAvganger();
        }
        else {
            url = "Avgang/GenererAvganger";

            $.get(url, data, function (OK) {
                if (OK) {
                    hentAvganger(stasjonFra.id, stasjonTil.id, dato, tidspunkt);
                    //hentAvganger();
                } else {
                    ut = "<h2>Feil i db...</h2>";
                    $("#avganger").html(ut);
                }
            });
        }
    });
}

//Henter avganger for gitt strekning og dato
//filtrer også slik at bare de av dem som oppfyller tidspunkt blir sendt til formatering
function hentAvganger(stasjonFraId, stasjonTilId, dato, tidspunkt) {
    const url = "Avgang/HentAvganger";
    const data = {
        stasjonFraId: stasjonFraId,
        stasjonTilId: stasjonTilId,
        dato: dato
    }

    $.get(url, data, function (avganger) {
        formaterAvganger(avganger);
    });
}


//Formaterer de hentede avgangene i table
function formaterAvganger(avganger) {
    let ut = "<h3>Avganger for dato: " + avganger[0].dato + "</h3>" +
        "<table class='table table-hover'>" +
        "<tr>" +
        "<th>Avreise tidspunkt</th>" +
        "<th>Pris</th>" +
        "<th></th>" +
        "</tr>";
    for (let avgang of avganger) {
        ut += "<tr>" +
            "<td>" + avgang.tidspunkt + "</td>" +
            "<td>" + avgang.pris + ",-</td>" +
            "<td> <button class='btn btn-success' onclick='lagBestilling(" + avgang + ")'>Velg</button></td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#avganger").html(ut);
}

//Når alle valg er utført lager vi en bestilling og pusher til database
function lagBestilling(avgang) {
    $("#feil").html("StasjonFra: " + avgang.stasjonFra + " StasjonTil: " + avgang.stasjonTil + " dato: " + avgang.dato + " tidspunkt: " + avgang.tidspunkt);

    const bestilling = {
        stasjonFra: avgang.stasjonFra,
        stasjonTil: avgang.stasjonTil,
        dato: avgang.dato,
        tidspunkt: avgang.tidspunkt
    }
    const url = "Bestilling/Bestill";
    $.post(url, bestilling, function (OK) {
        if (OK) {
            window.location.href = 'bestillingsliste.html';
        }
        else {
            $("#feil").html("Feil i db - prøv igjen senere");
        }
    });
};

=======
>>>>>>> Stashed changes

//Hvis bruker trykker på gul endre knapp.
//Retning er enten "fra" eller "til"
function endreStasjon(retning) {
    //Fjerner tekst fra tidligere
    $(".stasjonerAutocomplete").val("");

    if (retning == 1) {
        $("#fraSøk").show("slow");
        $("#fraBoks").hide();
        sessionStorage.setItem("step", 1);
    } else{
        $("#tilSøk").show("slow");
        $("#tilBoks").hide();
        sessionStorage.setItem("step", 2);
    }
}