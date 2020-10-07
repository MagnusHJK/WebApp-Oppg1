$(function () {
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
        dateFormat: 'dd/mm/yy',
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
        dateFormat: 'dd/mm/yy',
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
        "<th>Dato (DD/MM/YYYY)</th>" +
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
        $("#datoValgEndreAvgang" + avgang.id).val(datoAvgang.toLocaleDateString(undefined, datoOptions));
        $("#tidspunktEndreAvgang" + avgang.id).val(datoAvgang.toLocaleTimeString(undefined, tidOptions));
        $("#prisEndreAvgang" + avgang.id).val(avgang.pris);
    }
}

//Tallet gitt fra Endre knappen avgjør hvilken stasjon som skal endres
//Det som er skrevet i input per avgang er det som blir endret
function endreAvgang(avgangId) {
    var dato = $("#datoValgEndreAvgang" + avgangId).val();
    var tidspunkt = $("#tidspunktEndreAvgang" + avgangId).val();
    var tidsArr = tidspunkt.split(':');
    var pris = $("#prisEndreAvgang" + avgangId).val();

    const avgangOK = validerAvgang(dato, tidspunkt, pris);

    if (avgangOK) {
        //Må bygge dato objektet manuelt siden new Date() tar inn dato string i MM/DD/YYYY format
        var datoArr = dato.split("/");
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

function slettAvgang() {

}