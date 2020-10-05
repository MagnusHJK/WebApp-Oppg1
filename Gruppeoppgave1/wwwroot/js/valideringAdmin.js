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