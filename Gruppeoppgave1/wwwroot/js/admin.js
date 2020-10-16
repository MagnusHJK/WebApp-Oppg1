﻿$(function () {
    hentAlleStasjoner();
});

//Henter stasjoner
function hentAlleStasjoner() {
    $.get("Stasjon/HentAlleStasjoner", function (stasjoner) {
        fyllInnStasjoner(stasjoner);
    });
}

//Lag stasjoner
function lagStasjon() {
    const stasjonsNavnOK = validerStasjonsNavn($("#nyStasjonInput").val());
    if (stasjonsNavnOK) {

        const stasjon = {
            navn: $("#nyStasjonInput").val()
        }

        const url = "Stasjon/LagStasjon";
        $.get(url, stasjon, function (OK) {
            if (OK) {
                hentAlleStasjoner();
                $("#vellykketStasjoner").html(stasjon.navn + " lagt til i stasjoner");
            }
            else {
                $("#feilStasjoner").html("Feilet");
            }
        })
            .fail(function () {
                $("#feilStasjoner").html("Feilet på server - prøv igjen senere");
            });
    }
}


//Endre stasjoner
function endreStasjon() {
    const stasjonsNavnOK = validerStasjonsNavn($("#endreStasjonInput").val());
    if (stasjonsNavnOK) {
        var gamleNavn = $("#endreStasjonSelect option:selected").text();

        const stasjon = {
            id: $("#endreStasjonSelect").val(),
            navn: $("#endreStasjonInput").val()
        }

        const url = "Stasjon/EndreStasjon";
        $.get(url, stasjon, function (OK) {
            if (OK) {
                hentAlleStasjoner();
                $("#vellykketStasjoner").html(gamleNavn + " ble endret til: " + stasjon.navn);
            }
        })
            .fail(function () {
            });
    }
}


// Slette stasjoner
function slettStasjon() {
    const valgtStasjon = $("#slettStasjonSelect").val();
    const url = "Stasjon/SlettStasjon?id=" + valgtStasjon;

    $.get(url, function (OK) {
        if (OK) {
            var stasjonNavn = $("#slettStasjonSelect option:selected").text();
            $("#vellykketStasjoner").html("Stasjonen " + stasjonNavn + " ble fjernet");
        } else {
            $("#feilStasjoner").html("Feilet på server - prøv igjen senere");
        }
    });
}


//Fyller inn stasjoner i valg
function fyllInnStasjoner(stasjoner) {
    $(".stasjoner").html("");

    $(".stasjoner").append($('<option></option>').val('').html(''));

    for (i = 0; i < stasjoner.length; i++) {
        $(".stasjoner").append($('<option></option>').val(stasjoner[i].id).html(stasjoner[i].navn));
    }
}

//Automatisk fyller inn stasjons navn utifra hva du velger
function oppdaterTekstStasjon() {
    var stasjonsNavn = $("#endreStasjonSelect option:selected").text();
    $("#endreStasjonInput").val(stasjonsNavn);
}

//Sjekker om stasjonen til og fra er like, hvis ikke så blir avganene mellom disse hentet
function endretAvgangerValg() {
    if ($("#endreAvgangStasjonFra option:selected").val() == $("#endreAvgangStasjonTil option:selected").val()) {
        $("#feilAvganger").html("Stasjonene kan ikke være like!");
    }
    else {
        $("#feilAvganger").html("");
        datoValgEndreAvganger();
    }

    if ($("#lagAvgangStasjonFra option:selected").val() == $("#lagAvgangStasjonTil option:selected").val()) {
        $("#feilAvganger").html("Stasjonene kan ikke være like!");
    }
    else {
        $("#feilAvganger").html("");
        datoValgLagAvganger();
    }
}

//Dato boks for å opprette avganger
function datoValgLagAvganger() {
    //Dato settings
    $("#datoValgLagAvganger").datepicker({
        dateFormat: 'dd.mm.yy',
        minDate: 0,
        firstDay: 1
    });
}

function lagAvgang() {
    var stasjonFraId = $("#lagAvgangStasjonFra option:selected").val();
    var stasjonTilId = $("#lagAvgangStasjonTil option:selected").val();
    var datoObj = $("#datoValgLagAvganger").datepicker('getDate');
    var tidspunkt = $("#tidspunktLagAvganger").val();
    var tidsArr = tidspunkt.split(':');
    var pris = $("#prisLagAvganger").val();
    var datoISO = datoObj.toISOString();

    const avgangOK = validerAvgang(datoISO, tidspunkt, pris);

    if (avgangOK) {
        datoObj.setHours(tidsArr[0]);
        datoObj.setMinutes(tidsArr[1]);

        var datoISO = datoObj.toISOString();

        let url = "Avgang/LagAvgang";

        let avgang = {
            stasjonFraId: stasjonFraId,
            stasjonTilId: stasjonTilId,
            datoTid: datoISO,
            pris: pris
        }

        $.post(url, avgang, function () {
            $("#vellykketAvganger").html("Avgang lagt til");
        })
            .fail(function () {
                $("#feilAvganger").html("Avgang ble ikke opprettet");
            });
    }
}


//Dato boks for å endre/slette avganger
function datoValgEndreAvganger() {
    //Dato settings
    $("#datoValgEndreAvganger").datepicker({
        dateFormat: 'dd.mm.yy',
        minDate: 0,
        firstDay: 1
    });
}

function sjekkAvgang() {
    var stasjonFraId = $("#endreAvgangStasjonFra option:selected").val();
    var stasjonTilId = $("#endreAvgangStasjonTil option:selected").val();
    var datoObj = $("#datoValgEndreAvganger").datepicker('getDate');
    var datoISO = datoObj.toISOString();

    let url = "Avgang/SjekkAvganger";
    let avgang = {
        stasjonFraId: stasjonFraId,
        stasjonTilId: stasjonTilId,
        dato: datoISO
    };

    $.get(url, avgang, function (OK) {
        hentAvganger(avgang);
    })
        .fail(function () {
            $("#avganger").html("");
            $("#feilAvganger").html("Fant ingen avganger for gitte parametere.");
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
    //Bestemmer format for dato og tid
    const datoOptions = {year: 'numeric', month: 'numeric', day: 'numeric'};
    const tidOptions = {hour12: false, hour: '2-digit', minute: '2-digit'};

    $("#avganger").show();

    let ut = "<table class='table table-sm table-hover' id='avgangerTable'>" +
        "<tr>" +
        "<th>Dato (DD.MM.YYYY)</th>" +
        "<th>Avreise tidspunkt (HH:MM)</th>" +
        "<th>Pris per billett</th>" +
        "<th></th>" +
        "<th></th>" +
        "</tr>" +
        "</table>";
    $("#avganger").html(ut);

    for (let avgang of avganger) {
        var datoAvgang = new Date(avgang.dato);

        ut = "<tr>" +
            "<td><input type='text' class='form-control' id='datoValgEndreAvgang" + avgang.id + "'/></td>" + 
            "<td><input type='text' class='form-control' id='tidspunktEndreAvgang" + avgang.id + "'/></td>" +
            "<td><input type='text' class='form-control' id='prisEndreAvgang" + avgang.id + "'/></td>" +
            "<td><button class='btn btn-warning' onclick='endreAvgang(" + avgang.id + ")'>Endre</button></td>" +
            "<td><button class='btn btn-danger' onclick='slettAvgang(" + avgang.id + ")'>Slett</button></td>" +
            "</tr>";
        $('#avgangerTable tr:last').after(ut);

        //Fyller inn info om avgang i html input
        $("#datoValgEndreAvgang" + avgang.id).val(datoAvgang.toLocaleDateString("no-NO", datoOptions));
        $("#tidspunktEndreAvgang" + avgang.id).val(datoAvgang.toLocaleTimeString("no-NO", tidOptions));
        $("#prisEndreAvgang" + avgang.id).val(avgang.pris);
    }
}

//Tallet gitt fra Endre knappen avgjør hvilken stasjon som skal endres
//Det som er skrevet i input per avgang er det som blir endret
function endreAvgang(avgangId) {
    //Henter ut dato og pris, deler opp tiden i timer og minutter
    var dato = $("#datoValgEndreAvgang" + avgangId).val();
    var tidspunkt = $("#tidspunktEndreAvgang" + avgangId).val();
    var tidsArr = tidspunkt.split(':');
    var pris = $("#prisEndreAvgang" + avgangId).val();

    const avgangOK = validerAvgang(dato, tidspunkt, pris);

    if (avgangOK) {
        //Må bygge dato objektet manuelt siden new Date() tar inn dato string i MM/DD/YYYY format...
        var datoArr = dato.split(".");
        var datoObj = new Date(+datoArr[2], datoArr[1] - 1, + datoArr[0]);
        datoObj.setHours(tidsArr[0]);
        datoObj.setMinutes(tidsArr[1]);
        var datoISO = datoObj.toISOString();

        let url = "Avgang/EndreAvgang";
        let avgang = {
            avgangId: avgangId,
            datoTid: datoISO,
            pris: pris
        };

        $.post(url, avgang, function () {
            $("#vellykketAvganger").html("Avgang endret");
        })
            .fail(function () {
                $("#feilAvganger").html("Avgang ble ikke endret");
            });

    }
}


function slettAvgang(avgangId) {
    const url = "Avgang/SlettAvgang"

    $.get(url, avgangId, function (OK) {
        $("#vellykketAvganger").html("Avgang ble slettet");
    })
        .fail(function (){
            $("#feilAvganger").html("Avgang ble ikke slettet");
        });
}


function lagBestilling() {
    let url = "Bestilling/LagBestilling";
    let antallBestillinger = $("#lagBestillingAntall").val();
    let avgang = $("#lagBestillingAvgang").val();

    const antallOK = validerAntall(antallBestillinger);

    if (antallOK) {
        let bestilling = {
            avgangId: avgang,
            antall: antallBestillinger
        };

        $.get(url, bestilling, function () {
            $("#vellykketBestillinger").html("Bestilling opprettet");
        })
            .fail(function () {
                $("#feilBestillinger").html("Kunne ikke opprette bestilling");
            });
    }
}

function endreBestilling(bestillingId) {
    const url = "Bestilling/EndreBestilling";
    var avgangsId = $("#avgangValgEndreBestilling" + bestillingId).val();
    var antall = $("#antallEndreBestilling" + bestillingId).val();
    var antallOK = validerAntall(antall);

    if (antallOK) {
        var bestilling = {
            nyAvgangId: avgangsId,
            nyttAntall: antall
        };

        $.get(url, bestilling, function () {
            $("#vellykketBestillinger").html("Bestilling endret");
        })
            .fail(function () {
                $("#feilBestillinger").html("Kunne ikke endre bestilling");
            });
    }
}

//Henter alle bestillinger, ingen sjekk eller filtrering først
function hentAlleBestillinger() {
    const url = "Bestilling/HentAlleBestillinger";

    $.get(url, function (bestillinger) {
        formaterBestillinger(bestillinger);
    })
        .fail(function () {
            $("#feilBestillinger").html("Ingen bestillinger ble funnet");
        })
}

function formaterBestillinger(bestillinger) {
    //Bestemmer format for dato og tid
    const datoOptions = { year: 'numeric', month: 'numeric', day: 'numeric' };
    const tidOptions = { hour12: false, hour: '2-digit', minute: '2-digit' };

    $("#bestillinger").show();

    let ut = "<table class='table table-sm table-hover' id='bestillingTable'>" +
        "<tr>" +
        "<th>Avgang</th>" +
        "<th>Antall</th>" +
        "<th></th>" +
        "<th></th>" +
        "</tr>" +
        "</table>";
    $("#bestillinger").html(ut);

    for (let bestilling of bestillinger) {

        ut = "<tr>" +
            "<td><select class='avganger form-control' id='avgangValgEndreBestilling" + bestilling.id + "'></select></td>" +
            "<td><input type='text' class='form-control' id='antallEndreBestilling" + bestilling.id + "'/></td>" +
            "<td><button class='btn btn-warning' onclick='endreBestilling(" + bestilling.id + ")'>Endre</button></td>" +
            "<td><button class='btn btn-danger' onclick='slettBestilling(" + bestilling.id + ")'>Slett</button></td>" +
            "</tr>";
        $('#bestillingTable tr:last').after(ut);

        //Fyller inn info om bestilling i html input
        $("#avgangValgEndreBestilling" + bestilling.id).append($('<option></option>').val(bestilling.avgang.id).html('Hello'));
        $("#antallEndreBestilling" + bestilling.id).val(bestilling.antall);

        
        var datoISO = bestilling.avgang.dato.toISOString();
        let avgang = {
            stasjonFraId: bestilling.avgang.stasjonFra.id,
            stasjonTilId: bestilling.avgang.stasjonTil.id,
            dato: datoISO
        };
        
        //Henter alle avganger og senere formaterer de i selecten for avganger
        test();
        hentAvgangerForBestillinger(avgang);
    }
}

function test() {
    alert("test");
}

function hentAvgangerForBestillinger(avgang) {
    alert("Hello");

    $.ajax({
        method: 'get',
        url: "Avgang/HentAvganger",
        data: avgang,
        async: false
    })
        .done(function (avganger) {
            fyllInnAvganger(avganger);
        });

    /*
    $.get(url, avgang, function (avganger) {
        fyllInnAvganger(avganger);
    })
        .fail(function () {
            $("#feilAvganger").html("Ingen Avganger ble funnet");
        });
        */
}


//Fyller inn stasjoner i valg
function fyllInnAvganger(avganger) {
    $(".avganger").html("");

    $(".avganger").append($('<option></option>').val('').html(''));

    alert("Hello 2");

    for (let avgang of avganger) {
        $(".avganger").append($('<option></option>').val(avgang.id).html(avgang.id + ": " + avgang.stasjonFra.navn + " -> " + avgang.stasjonTil.navn + " " + avgang.dato));
    }
}