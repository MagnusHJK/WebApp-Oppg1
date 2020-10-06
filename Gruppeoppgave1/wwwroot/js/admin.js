$(function () {
    hentAlleStasjoner();
    oppdaterTekstStasjon();
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
    if ($("#avgangStasjonFra option:selected").val() == $("#avgangStasjonTil option:selected").val()) {
        $("#feilAvganger").html("Stasjonene kan ikke være like!");
    } else {
        $("#feilAvganger").html("");
        lagDatoAvganger();
    }
}

function lagDatoAvganger() {
    //Dato settings
    $("#datoValgAvganger").datepicker({
        dateFormat: 'dd/mm/yy',
        minDate: 0,
        firstDay: 1,
        onSelect: function (dateText, inst) {
            //Henter ut dato og sørger for at den er på ISO8601 standard
            var datoObj = $(this).datepicker('getDate');
            var datoISO = datoObj.toISOString();

            sjekkAvgang(datoISO);
        }
    });
}

function sjekkAvgang(dato) {
    var stasjonFraId = $("#avgangStasjonFra option:selected").val();
    var stasjonTilId = $("#avgangStasjonTil option:selected").val();

    let url = "Avgang/SjekkAvganger";
    let avgang = {
        stasjonFraId: stasjonFraId,
        stasjonTilId: stasjonTilId,
        dato: dato
    }

    $.get(url, avgang, function (OK) {
        hentAvganger(avgang);
    })
        .fail(function () {
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
    const tidOptions = {hour12: false, hour: 'numeric', minute: 'numeric', second: 'numeric'};

    $("#avganger").show();

    let ut = "<table class='table table-sm table-hover' id='avgangerTable'>" +
        "<tr>" +
        "<th>Dato (DD/MM/YYYY)</th>" +
        "<th>Avreise tidspunkt (HH:MM:SS)</th>" +
        "<th>Pris per billett</th>" +
        "<th></th>" +
        "<th></th>" +
        "</tr>" +
        "</table>";
    $("#avganger").html(ut);
    
    for (let avgang of avganger) {
        var datoAvgang = new Date(avgang.dato);

        ut = "<tr>" +
            "<td><input type='text' class='form-control' id='avgangDato" + avgang.id + "'/></td>" + 
            "<td><input type='text' class='form-control' id='avgangTid" + avgang.id + "'/></td>" +
            "<td><input type='text' class='form-control' id='avgangPris" + avgang.id + "'/></td>" +
            "<td><button class='btn btn-warning' onclick='endreAvgang(" + avgang.id + ")'>Endre</button></td>" +
            "<td><button class='btn btn-danger' onclick='slettAvgang(" + avgang.id + ")'>Slett</button></td>" +
            "</tr>";
        $('#avgangerTable tr:last').after(ut);

        $("#avgangDato" + avgang.id).val(datoAvgang.toLocaleDateString(undefined, datoOptions));
        $("#avgangTid" + avgang.id).val(datoAvgang.toLocaleTimeString(undefined, tidOptions));
        $("#avgangPris" + avgang.id).val(avgang.pris);
    }
}

//Tallet gitt fra Endre knappen avgjør hvilken stasjon som skal endres
function endreAvgang() {

}

function slettAvgang() {

}