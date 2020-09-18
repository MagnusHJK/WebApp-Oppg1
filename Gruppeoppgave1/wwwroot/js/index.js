﻿$(function () {
    hentAlleStasjoner();
    sessionStorage.clear();
});

//vet dette er dumt men...
var stasjonFra;
var stasjonTil;

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
            $("#feil").html(this.id);
            var stasjon = ui.item.obj;

            if (this.id == "fraStasjoner") {
                stasjonFra = stasjon;
                lagDestinasjonsBoks(stasjon, "fra");

            } else {
                stasjonTil = stasjon;
                lagDestinasjonsBoks(stasjon, "til");
            }

            //stasjonsValg(stasjon);
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
        $("#tilSøk").show("slow"); //Denne gir en feil
        lagDestinasjonsBoksFra(stasjon);
    }
    //Andre valg
    else {
        sessionStorage.setItem("step", 2);
        stasjonTil = stasjon;
        lagDestinasjonsBoksTil(stasjon);
    }
}

function lagDestinasjonsBoks(stasjon, retning) {
    var ut = "";

    //Destinasjon fra
    if (retning === "fra") {
        $("#fraSøk").hide("slow");
        ut = '<b style="font-size:3em;">Fra: </b>' +
            '<b style="font-size:2.5em;" id="stasjonFraNavn">' + stasjon.navn + '</b>' +
            '<button class="btn btn-warning" style="display:flex;justify-content:flex-end;align-items:center"' +
            'onclick="endreStasjon(1)">Endre</button></br>';

        $("#fraBoks").html(ut);
        $("#fraBoks").show("slow");
    }
    //Destinasjon til
    else {
        $("#tilSøk").hide("slow");
        ut = '<b style="font-size:3em;">Til: </b>' +
            '<b style="font-size:2.5em;"id="stasjonTilNavn">' + stasjon.navn + '</b>' +
            '<button class="btn btn-warning" style="display:flex;justify-content:flex-end;align-items:center"' +
            'onclick="endreStasjon(2)">Endre</button></br>';

        $("#tilBoks").html(ut);
        $("#tilBoks").show("slow");
    }

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    if (($("#tilBoks").is(":visible")) && ($("#fraBoks").is(":visible"))) {
        //Sjekker kjapt om stasjonene er identiske
        if (stasjonFra.id === stasjonTil.id) {
            $("#feil").html("Stasjonene kan ikke være like!");
        } else {
            $("#feil").html("");
            lagBestillingBoks();
        }
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

    $("#fraBoks").html(ut);
    $("#fraBoks").show("slow");

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    if (($("#tilBoks").is(":visible")) && ($("#fraBoks").is(":visible"))) {
        //Sjekker kjapt om stasjonene er identiske
        if (stasjonFra.id === stasjonTil.id) {
            $("#feil").html("Stasjonene kan ikke være like!");
        } else {
            $("#feil").html("");
            lagBestillingBoks();
        }
    }
}


function lagDestinasjonsBoksTil(stasjon) {
    $("#tilSøk").hide("slow");
    var ut = "";

    ut += '<b style="font-size:3em;">Til: </b>' +
          '<b style="font-size:2.5em;"id="stasjonTilNavn">' + stasjon.navn + '</b>' +
          '<button class="btn btn-warning" style="display:flex;justify-content:flex-end;align-items:center"' +
          'onclick="endreStasjon(2)">Endre</button></br>';

    $("#tilBoks").html(ut);
    $("#tilBoks").show("slow");

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    if (($("#tilBoks").is(":visible")) && ($("#fraBoks").is(":visible"))) {
        //Sjekker kjapt om stasjonene er identiske
        if (stasjonFra.id === stasjonTil.id) {
            $("#feil").html("Stasjonene kan ikke være like!");
        } else {
            $("#feil").html("");
            lagBestillingBoks();
        }
    }
}

//Boks for dato og billett valg
function lagBestillingBoks() {
    var dato;
    $("#bestillingsBoks").show("slow");
    $("#datovalg").datepicker({
        minDate: 0,
        firstDay: 1,
        onSelect: function (dateText, inst) {
            dato = dateText;
            sjekkAvganger(stasjonFra, stasjonTil, dato);
        }
    });
}

$(document).ready(function () {
    var ut = "";
    var sjekk = true;
    $("#btnlagre").click(function () {
        console.log("hei");
        if ($("#datovalg").val() == "") {
            ut += "Det er nødvendig å velge en dato. <br/>";
            sjekk = false;
        }

        if ($("#tidspunkt").val() == "") {
            ut += "Det må velges et tidspunkt. <br/>";
            sjekk = false;
        }
        $("#feil").html(ut);
        ut = "";

        if (sjekk === true) {
            visOrdre();
        }
    });
});


//Sjekker om avganger eksisterer, hvis de gjør det hentes de. Hvis ikke blir de generert og så hentet
function sjekkAvganger(stasjonFra, stasjonTil, dato) {
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
            hentAvganger(stasjonFra.id, stasjonTil.id, dato);
        }
        else {
            url = "Avgang/GenererAvganger";
            $.get(url, data, function (OK) {
                if (OK) {
                    hentAvganger(stasjonFra.id, stasjonTil.id, dato);
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
function hentAvganger(stasjonFraId, stasjonTilId, dato) {
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
    $("#avganger").show();

    let ut = "<h3>Avganger for dato: " + avganger[0].dato + "</h3>" +
        "<table class='table table-hover'>" +
        "<tr>" +
        "<th>Avreise tidspunkt</th>" +
        "<th>Pris</th>" +
        "<th></th>" +
        "</tr>";
    for (let avgang of avganger) {
        var avgangStringify = JSON.stringify(avgang);
        var variabel = "Hello";

        ut += "<tr>" +
            "<td>" + avgang.tidspunkt + "</td>" +
            "<td>" + avgang.pris + ",-</td>" +
            '<td> <button class="btn btn-success" onclick="test(\'' + avgangStringify + '\')">Velg</button></td>' +
            "</tr>";
    }
    ut += "</table>";
    $("#avganger").html(ut);
}


function test(avgangStringify) {
   var avgang = JSON.parse(avgangStringify);

    $("#feil").html(avgang.stasjonFra);
}

//Når alle valg er utført lager vi en bestilling og pusher til database
function lagBestilling(avgangStringify) {
    var avgang = JSON.parse(avgangStringify);

    $("#feil").html("StasjonFra: " + avgang.stasjonFra + " StasjonTil: " + avgang.stasjonTil + " dato: " + avgang.dato + " tidspunkt: " + avgang.tidspunkt);

    const bestilling = {
        avgang: avgang
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

//Hvis bruker trykker på gul endre knapp.
//Retning er enten "fra" eller "til"
function endreStasjon(retning) {
    //Fjerner tekst fra tidligere, og gjemmer ting som skal velges senere i prosessen
    $(".stasjonerAutocomplete").val("");
    $("#bestillingsBoks").hide();
    $("#datovalg").datepicker("setDate", null);
    $("#avganger").hide();

    $("#feil").html(retning);

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