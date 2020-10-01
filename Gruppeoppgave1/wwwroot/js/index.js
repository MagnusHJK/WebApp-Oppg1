$(function () {
    hentAlleStasjoner();
    step = 0;
});

//vet dette er dumt men...
var stasjonFra;
var stasjonTil;
var avgangerId = [];
var step;

function hentAlleStasjoner() {
    $.get("Stasjon/HentAlleStasjoner", function (stasjoner) {
        stasjonerAutocomplete(stasjoner);
    });
}

//Autocompleter søk på stasjoner
function stasjonerAutocomplete(stasjoner) {
    $(".stasjonerAutocomplete").autocomplete({
        minLength: 1,
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

            //Ser hvilken retning som var valgt
            //IF STEP: 0 (Ingen valgt tatt)
            if (step == 0) {
                step = 1;
                stasjonFra = stasjon;
                lagDestinasjonsBoks(stasjon);
            }
            //IF STEP: 2 (Stasjon til valgt)
            else if (step == 2) {
                step = 3;
                stasjonTil = stasjon;
                lagDestinasjonsBoks(stasjon);
            }
            return false; 
        }
    });
}

function lagDestinasjonsBoks(stasjon) {
    var ut = "";
    //Destinasjon fra
    //IF STEP: 1 (Stasjon fra valgt)
    if (step == 1) {
        step = 2;

        $("#fraSøk").hide("slow");
        ut = '<b style="font-size:3em;">Fra: </b>' +
            '<b style="font-size:2.5em;" id="stasjonFraNavn">' + stasjon.navn + '</b><br/>' +
            '<button class="btn btn-warning" onclick="endreStasjon(0)">Endre</button></br>';

        $("#fraBoks").html(ut);
        $("#fraBoks").show("slow");
        $("#tilSøk").show("slow");
    }
    //Destinasjon til
    //IF STEP: 3 (Stasjon til valgt, og beskrivelse kommer opp)
    else if (step == 3) {
        step = 4;

        $("#tilSøk").hide("slow");
        ut = '<b style="font-size:3em;">Til: </b>' +
            '<b style="font-size:2.5em;"id="stasjonTilNavn">' + stasjon.navn + '</b><br/>' +
            '<button class="btn btn-warning onclick="endreStasjon(2)">Endre</button></br>';

        $("#tilBoks").html(ut);
        $("#tilBoks").show("slow");
    }

    //Hvis både til og fra stasjon er valgt vil de neste instillingene komme opp
    //IF STEP: 4 (Begge stasjoner valgt)
    if (step == 4) {
        step = 5;
        //Sjekker kjapt om stasjonene er identiske
        if (stasjonFra.id === stasjonTil.id) {
            $("#feil").html("Stasjonene kan ikke være like!");
        } else {
            $("#feil").html("");
            lagAvreiseBoks();
        }
    }
}

//Hvis bruker trykker på gul endre knapp.
//nyStep sier hvilket steg vi skal gå tilbake til
function endreStasjon(nyStep) {
    //Fjerner tekst fra tidligere, og gjemmer ting som skal velges senere i prosessen
    $(".stasjonerAutocomplete").val("");
    $("#bestillingsBoks").hide();
    $("#reiseTekst").html("Når vil du reise?");
    $("#reiseValg").show();
    $("#datoValg").datepicker("setDate", null);
    $("#avreise").html("");
    $("#retur").html("");
    $("#knapper").html("");

    step = nyStep;

    //Step 0: ingen valg tatt
    if (step == 0) {
        $("#fraSøk").show("slow");
        $("#fraBoks").hide();
        $("#tilBoks").html("");
    }
    //Step 2: Til stasjon skal velges
    else if (step == 2) {
        $("#tilSøk").show("slow");
        $("#tilBoks").hide();
    }
}


//Boks for dato og billett valg for første reise
function lagAvreiseBoks() {
    $("#bestillingsBoks").show("slow");
    $("#avreiseValg").show();

    //Genererer dropdown for valg av antall billetter
    for (i = 1; i <= 10; i++) {
        $("#antallBilletter").append($('<option></option>').val(i).html(i));
    }

    //Hvis dato valg allerede er initializert må vi sørge for at den bruke avreise og ikke retur innstillinger
    $("#datoValg").datepicker("option", "minDate", 0);
    $("#datoValg").datepicker("option", "onSelect", function () {
        var datoObj = $(this).datepicker('getDate');
        var datoISO = datoObj.toISOString();

        sjekkAvganger(stasjonFra, stasjonTil, datoISO);
    });
    //Dato settings
    $("#datoValg").datepicker({
        dateFormat: 'dd/mm/yy',
        minDate: 0,
        firstDay: 1,
        onSelect: function (dateText, inst) {
            //Henter ut dato og sørger for at den er på ISO8601 standard
            var datoObj = $(this).datepicker('getDate');
            var datoISO = datoObj.toISOString();

            //Ved første tur
            sjekkAvganger(stasjonFra, stasjonTil, datoISO);
        }
    });
}

//Boks for dato og billett valg for retur
function lagReturBoks(dato) {
    if (dato == null) {
        dato = 0;
    }

    //Endrer dato valg til å håndtere retur (Sørger for at den ikke kan bli satt før avreise dato)
    $("#datoValg").datepicker("option", "minDate", new Date(dato));
    $("#datoValg").datepicker("option", "onSelect", function () {
        var datoObj = $(this).datepicker('getDate');
        var datoISO = datoObj.toISOString();

        sjekkAvganger(stasjonTil, stasjonFra, datoISO);
    });
}

//Hvis det trykkes på gul endre knapp under avreise og retur valg
function endreAvganger() {
    $("#avreise").html("");
    $("#retur").html("");
    $("#knapper").html("");
    $("#reiseTekst").html("Når vil du reise?");
    avgangerId = [];

    step = 5;
    $("#reiseValg").show("slow");
    lagAvreiseBoks();
}


//Etter avreise er valgt, skal retur velges
function lagRetur(dato) {
    $("#avreiseValg").hide();
    $("#reiseTekst").html("Når vil du returnere?");
    $("#reiseValg").show();

    lagReturBoks(dato);
}