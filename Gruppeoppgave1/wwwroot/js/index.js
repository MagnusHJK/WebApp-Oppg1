$(function () {
    hentAlleStasjoner();
});

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
                        value: obj.id
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
            event.preventDefault();
            var divId = $(this).attr("id");
            var retning = divId.substring(0, 3);
            var stasjonsId = ui.item.value;

            lagDestinasjonsBoks(retning, stasjonsId);
            return false;
        }
    });
}

//Lager bokser etter at man har valg en stasjon enten fra eller til
function lagDestinasjonsBoks(retning, stasjonsId) {
    $("#" + retning + "Søk").hide("slow");
    const url = "Stasjon/HentEnStasjon?id=" + stasjonsId;
    var ut = "";

    $.get(url, function (stasjon) {
        ut += '<b style="font-size:3em;">' + retning.toUpperCase() + ': </b>' +
              '<b style="font-size:2.5em;">' + stasjon.navn + '</b>' +
              '<button class="btn btn-danger" id="test" style="display:flex;justify-content:flex-end;align-items:center"' +
              'onclick="endreStasjon(\'' + retning + '\')" > Endre</button > </br > ';

        $("#" + retning + "Boks").html(ut);
    });

    //Lagre info i div
    $("#" + retning + "Boks")
        .data("retning", retning)
        .data("stasjonsid", stasjonsId);

    $("#" + retning + "Boks").show("slow");

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    if (($("#tilBoks").is(":visible")) && ($("#fraBoks").is(":visible"))) {
        lagBestillingBoks();
    }
}

//Boks for dato og billett valg
function lagBestillingBoks() {
    $("#bestillingsBoks").show("slow");
    $("#datovalg").datepicker();

    lagBestilling();
}

//Når alle valg er utført lager vi en bestilling og pusher til database
function lagBestilling(stasjonFra, stasjonTil, dato, tidspunkt) {

}


//Hvis bruker trykker på rød endre knapp.
//Retning er enten "fra" eller "til"
function endreStasjon(retning) {
    //Fjerner tekst fra tidligere
    $(".stasjonerAutocomplete").val("");

    //Gjemmer og tar frem relevante divs
    $("#" + retning + "Søk").show("slow");
    $("#" + retning + "Boks").hide();
}

function test() {
    $.get("Stasjon/Test", function (OK) {
        if (OK) {
            $("#feil").html("Return true");
        }
        else {
            $("#feil").html("Return false");
        }
    });
}