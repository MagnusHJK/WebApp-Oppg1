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
        $("#vellykketAvganger").html("Fant avganger! (Det er bare ikke implementert enda)");
        //Hent avganger og formater
    })
        .fail(function () {
            $("#feilAvganger").html("Fant ingen avganger for gitte parametere");
        });

}