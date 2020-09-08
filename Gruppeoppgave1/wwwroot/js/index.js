$(function () {
    hentAlleStasjoner();
});

function hentAlleStasjoner() {
    $.get("Stasjon/HentAlleStasjoner", function (stasjoner) {
        stasjonerAutocomplete(stasjoner);
    });
}

var stasjon = {

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
            //var stasjonsId = ui.item.value;
            //var stasjonsNavn = ui.item.label;

            stasjonsValg(stasjon);
            return false;
        }
    });
}

//Velger stasjoner og lagrer hvilken stasjon som er valgt som til eller fra
function stasjonsValg(stasjon) {
    var step = sessionStorage.getItem("step");
 
        //Det er første stasjons valg
        if (step === null || step === 1) {
            sessionStorage.setItem("fra", stasjon.navn);
            sessionStorage.setItem("step", 1);
            lagDestinasjonsBoksFra(stasjon);
        }
        //Andre valg
        else {
            sessionStorage.setItem("til", stasjon.navn);
            sessionStorage.setItem("step", 2);
            lagDestinasjonsBoksTil(stasjon);
        }
}


//Lager bokser etter at man har valg en stasjon enten fra eller til
function lagDestinasjonsBoksFra(stasjon) {
    $("#fraSøk").hide("slow");
    var ut = "";

    //var stasjon = sessionStorage.getItem("fra");

    ut += '<b style="font-size:3em;">Fra: </b>' +
          '<b style="font-size:2.5em;">' + stasjon.navn + '</b>' +
          '<button class="btn btn-danger" style="display:flex;justify-content:flex-end;align-items:center"' +
          'onclick="endreStasjon(fra)">Endre</button></br>';

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
    //var stasjon = sessionStorage.getItem("til");

    ut += '<b style="font-size:3em;">Til: </b>' +
          '<b style="font-size:2.5em;">' + stasjon.navn + '</b>' +
          '<button class="btn btn-danger" style="display:flex;justify-content:flex-end;align-items:center"' +
          'onclick="endreStasjon(til)">Endre</button></br>';

    $("#tilBoks").html(ut);
    $("#tilBoks").show("slow");

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    if (($("#tilBoks").is(":visible")) && ($("#fraBoks").is(":visible"))) {
        lagBestillingBoks();
    }
}

//Boks for dato og billett valg
function lagBestillingBoks() {
    $("#bestillingsBoks").show("slow");
    $("#datovalg").datepicker();

    //lagBestilling(stasjonFraId, stasjonTilId, dato, tidspunkt);
}

//Når alle valg er utført lager vi en bestilling og pusher til database
function lagBestilling(stasjonFra, stasjonTil, dato, tidspunkt) {

}


//Hvis bruker trykker på rød endre knapp.
//Retning er enten "fra" eller "til"
function endreStasjon(retning) {
    //Fjerner tekst fra tidligere
    $(".stasjonerAutocomplete").val("");

    console.log(retning);

    if (retning == "fra") {
        $("#fraSøk").show("slow");
        $("#fraBoks").hide();
        sessionStorage.setItem("step", 1);
    } else{
        $("#tilSøk").show("slow");
        $("#tilBoks").hide();
        sessionStorage.setItem("step", 1);
    }
}