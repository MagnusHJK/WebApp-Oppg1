//Validering for all input brukt i adminpanel

function validerStasjonsNavn(stasjonsNavn) {
    alert(hello);
    const regex = /^[a-zA-ZæøåÆØÅ. \-]{2,40}$/;
    const ok = regex.test(stasjonsNavn);

    if (!ok) {
        $("#feilStasjoner").html("Stasjonsnavnet er ikke gyldig");
        return false;
    }
    else {
        $("#feilStasjoner").html("");
        return true;
    }
}

function validerTest() {
    alert("test");
    return true;
}

function validerAvgang(dato, tid, pris) {
    alert("hello validering");
    const regexPris = /^[0-9]{1,5}$/;
    const regexTid = /^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$$/;
    const okPris = regexPris.test(pris);
    const okTid = regexTid.test(tid);
    const datoOk = Date.parse(dato);

    if (datoOk.isNaN || !okPris || !okTid) {
        $("#feilAvganger").html("Dato, tidspunkt eller pris ikke gyldig");
        return false;
    } else {
        $("#feilStasjoner").html("");
        return true;
    }
}