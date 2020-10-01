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
            //IF STEP: 0, SET STEP: 1
            if (step == 0) {
                step = 1;
                stasjonFra = stasjon;
                lagDestinasjonsBoks(stasjon);
            }
            //IF STEP: 2, SET STEP: 3
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
    //IF STEP: 1, SET STEP: 2
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
    //IF STEP: 3, SET STEP: 4
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
    //IF STEP: 4, SET STEP: 5
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

    if (step == 0) {
        $("#fraSøk").show("slow");
        $("#fraBoks").hide();
        $("#tilBoks").html("");
    } else if(step == 2) {
        $("#tilSøk").show("slow");
        $("#tilBoks").hide();
    }
}


//Boks for dato og billett valg for første reise
function lagAvreiseBoks() {
    $("#bestillingsBoks").show("slow");

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
            hentAvganger(stasjonFra, stasjonTil, dato);
        }
        else {
            url = "Avgang/GenererAvganger";
            $.get(url, data, function (OK) {
                if (OK) {
                    hentAvganger(stasjonFra, stasjonTil, dato);
                } else {
                    ut = "<h2>Feil i db...</h2>";
                    $("#avganger").html(ut);
                }
            });
        }
    });
}

//Henter avganger for gitt strekning og dato
function hentAvganger(stasjonFra, stasjonTil, dato) {
    const url = "Avgang/HentAvganger";
    const avgang = {
        stasjonFraId: stasjonFra.id,
        stasjonTilId: stasjonTil.id,
        dato: dato
    }

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
        //var avgangJSON = JSON.stringify(av);

        ut += "<tr>" +
            "<td>" + datoAvgang.toTimeString() + "</td>" +
            "<td>" + avgang.pris + ",-</td>" +
            "<td><button class='btn btn-success' onclick='lagBillett(\"" + avgang.id + "\", \"" + datoAvgang.toISOString() + "\", \"" + avgang.pris + "\")' > Velg</button ></td > " +
            "</tr>";
    }
    ut += "</table>";

    $("#avganger").html(ut);
}

//Lager "billett" som gir info om valgte avganger
function lagBillett(avgangId, datoAvgang, pris) {
    let ut = "";
    let knapper = "";
    var dato = new Date(datoAvgang);
    var antall = $("#antallBilletter").val();

    $("#reiseValg").hide();
    $("#avganger").html("");


    //IF STEP: 5, SET STEP: 6
    if (step == 5) {
        step = 6;
        avgangerId[0] = avgangId;

        ut = "<h4>Avreise:</h4>" +
            "<b>Fra: " + stasjonFra.navn + " Til: " + stasjonTil.navn + "</b><br>" +
            dato.toString() + "<br>" +
            antall + " billett(er) til " + pris + ",- per stk <br>";
        $("#avreise").html(ut);

        knapper = "<button class='btn btn-warning' id='endreAvganger' onclick='endreAvganger()'>Endre</button>  " +
                  "<button class='btn btn-secondary' id='returKnapp' onclick='lagRetur(\"" + datoAvgang + "\")'>Jeg ønsker retur billett</button>  " +
                  "<button class='btn btn-success' id='bestillKnapp' onclick='lagBestilling()'>Bestill</button>";
            
        $("#knapper").html(knapper);
    }
    //IF STEP: 6, SET STEP: 7
    else if (step == 6) {
        step = 7;
        avgangerId[1] = avgangId;

        ut = "<h4>Retur:</h4>" +
            "<b>Fra: " + stasjonTil.navn + " Til: " + stasjonFra.navn + "</b><br>" +
            dato.toString() + "<br>" +
            antall + " billett(er) til " + pris + ",- per stk";
        $("#retur").html(ut);

        $("#returKnapp").remove();
    }
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
    $("avreiseValg").hide();
    $("#reiseTekst").html("Når vil du returnere?");
    $("#reiseValg").show();

    lagReturBoks(dato);
}

//Når alle valg er utført lager vi selve bestillingen til backend
function lagBestilling() {
    var antall = $("#antallBilletter").val();
    const url = "Bestilling/LagBestilling";
    var sannhet = false;

    for (var i = 0; i < avgangerId.length; i++) {
        const bestilling = {
            avgangId: avgangerId[i],
            antall: antall
        }
        sannhet = bestillingAjax(url, bestilling);
    }

    if (sannhet) {
        window.location.href = 'bestillingsliste.html';
    } else {
        $("#feil").html("Feil i db - prøv igjen senere");
    }
}

//Ajax kall i egen funskjon
function bestillingAjax(url, bestilling) {
    var sannhet = false;

    $.ajax({
        method: 'get',
        url: url,
        data: bestilling,
        async: false,
        success: function (OK) {
            if (OK) {
                sannhet = true;
            } else {
                sannhet = false;
            }
        }
    });
    return sannhet;
}