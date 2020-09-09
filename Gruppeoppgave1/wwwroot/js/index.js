$(function () {
    hentAlleStasjoner();
    sessionStorage.clear();
});

//vet dette er dumt men...
var stasjonTilVar;
var stasjonFraVar;

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

    stasjonFraVar = stasjon.navn;

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

    stasjonTilVar = stasjon.navn;

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
        }
    });

    stasjonFraId = sessionStorage.getItem("fraId");
    stasjonTilId = sessionStorage.getItem("tilId");
    genererAvganger();

    //lagBestilling(stasjonFraId, stasjonTilId, dato, tidspunkt);
}

//genererer avganger, alle avganger er bare oppdiktet utifra stasjoner, dato og tid.
function genererAvganger() {


}
//Når alle valg er utført lager vi en bestilling og pusher til database
function lagBestilling() {
    var fraStasjon = stasjonFraVar;
    var tilStasjon = stasjonTilVar;


    const bestilling = {
        stasjonFra: fraStasjon,
        stasjonTil: tilStasjon,
        dato: $("#datovalg").val(),
        tidspunkt: $("#tidspunkt").val()
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