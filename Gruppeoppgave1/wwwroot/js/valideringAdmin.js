//Validering for all input brukt i adminpanel

//Gir en tilbakemelding, forsvinner etter 15 sek
function statusMelding(tekst, sannhet) {
    if (sannhet) {
        $("#statusMeldingOK").html(tekst);
        $("#statusMeldingOK").fadeIn('fast').delay(15000).fadeOut('slow');
    } else {
        $("#statusMeldingFeil").html(tekst);
        $("#statusMeldingFeil").fadeIn('fast').delay(15000).fadeOut('slow');
    }
}



function validerStasjonsNavn(stasjonsNavn) {
    const regex = /^[a-zA-ZæøåÆØÅ. \-]{2,40}$/;
    const ok = regex.test(stasjonsNavn);

    if (!ok) {
        statusMelding("Stasjonsnavnet er ikke gyldig", false);
        return false;
    }
    else {
        $("#statusMeldingFeil").html("");
        return true;
    }
}

function validerTest() {
    alert("test");
    return true;
}

function validerAvgang(dato, tid, pris) {
    const regexPris = /^[0-9]{1,5}$/;
    const regexTid = /^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$$/;
    const okPris = regexPris.test(pris);
    const okTid = regexTid.test(tid);
    const datoOk = Date.parse(dato);

    if (datoOk.isNaN || !okPris || !okTid) {
        statusMelding("Dato, tidspunkt eller pris ikke gyldig", false);
        return false;
    } else {
        $("#statusMeldingFeil").html("");
        return true;
    }
}

//For antall resende ved bestilling
function validerAntall(antall) {
    const regexAntall = /^[0-9]{1,2}$/

    const okAntall = regexAntall.test(antall);

    if (!okAntall) {
        statusMelding("Antall er ikke gyldig, maks to sifrede tall", false);
        return false;
    } else {
        $("#statusMeldingFeil").html("");
        return true;
    }
}