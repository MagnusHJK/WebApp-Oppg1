﻿$(function () {
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
          '<button class="btn btn-danger" style="display:flex;justify-content:flex-end;align-items:center"' +
          'onclick="endreStasjon(1)">Endre</button></br>';

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
          '<button class="btn btn-danger" style="display:flex;justify-content:flex-end;align-items:center"' +
          'onclick="endreStasjon(2)">Endre</button></br>';

    $("#tilBoks").html(ut);
    $("#tilBoks").show("slow");

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    if (($("#tilBoks").is(":visible")) && ($("#fraBoks").is(":visible"))) {
        lagBestillingBoks();
    }
}

//Boks for dato og billett valg
function lagBestillingBoks() {
    var dato;
    var tidspunkt;
    $("#bestillingsBoks").show("slow");
    $("#datovalg").datepicker({
        minDate: 0,
        firstDay: 1,
        onSelect: function (dateText, inst) {
            dato = dateText;
            //Ligger her midlertidig slik at den ikke alltid blir "triggered"
            hentAvganger();
        }
    });

    //lagBestilling(stasjonFraId, stasjonTilId, dato, tidspunkt);
}

//VIL BLI BYTTET UT
//genererer avganger i js, alle avganger er bare oppdiktet utifra stasjoner, dato og tid.
function genererAvganger() {
    var dato = "05/05/2005";
    var tidspunkt = "12:30";
    var splittetTid = tidspunkt.split(":");
    var ut = "Avganger fra " + stasjonFra.navn + ", til " + stasjonTil.navn + " på dato " + dato;

    var time = splittetTid[0];
    var minutt = splittetTid[1];

    for (i = time; i < 24; i++) {
        ut += "Tid: "+ i +"</br>";
    }

    $("#avganger").html(ut);
}

//Henter avganger fra backend
function hentAvganger(stasjonFraId, stasjonTilId, dato, tidspunkt) {
    const url = "Stasjoner/HentAvganger";
    $.get(url, function (avganger) {

    });
}



//Når alle valg er utført lager vi en bestilling og pusher til database
function lagBestilling(stasjonFra, stasjonTil, dato, tidspunkt) {

}


//Hvis bruker trykker på rød endre knapp.
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