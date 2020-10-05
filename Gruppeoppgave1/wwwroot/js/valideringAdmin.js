//Validering for all input brukt i adminpanel

function validerStasjonsNavn(stasjonsNavn) {
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

function validerAvgang(dato, pris) {
    const regexPris = /^[0-9]{ 1,5}$/
    const okPris = regexPris.test(pris);
    const datoOk = Date.parse(dato);

    if (datoOk.isNaN || !okPris) {
        $("#feilAvganger").html("Avgang ikke gyldig");
        return false;
    } else {
        $("#feilStasjoner").html("");
        return true;
    }

}