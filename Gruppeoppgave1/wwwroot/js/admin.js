$(function () {
    hentAlleStasjoner();
});

//Henter stasjoner
function hentAlleStasjoner() {
    $.get("Stasjon/HentAlleStasjoner", function (stasjoner) {
        fyllInnStasjoner(stasjoner);
    });
}

//Fyller inn stasjoner i valg
function fyllInnStasjoner(stasjoner) {
    for (i = 0; i < stasjoner.length; i++) {
        $(".stasjoner").append($('<option></option>').val(stasjoner[i].id).html(stasjoner[i].navn));
    }
}

//Automatisk fyller inn stasjons navn utifra hva du velger
function oppdaterTekstStasjon() {
    var stasjonsNavn = $("#endreStasjon option:selected").text();

    $("#stasjonNyttNavn").val(stasjonsNavn);
}

//Sjekker om stasjonen til og fra er like, hvis ikke så blir avganene mellom disse hentet
function endretAvgangerValg() {
    if ($("#avgangStasjonFra option:selected").val() == $("#avgangStasjonTil option:selected").val()) {
        $("#feilAvganger").html("Stasjonene kan ikke være like!");
    } else {
        $("#feilAvganger").html("");
        //henter avganger
    }
}