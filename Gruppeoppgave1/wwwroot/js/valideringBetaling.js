//Gir en tilbakemelding utifra hvordan det gikk på backend, forsvinner etter 15 sek
function statusMelding(tekst, sannhet) {
    if (sannhet) {
        $("#statusMeldingOK").html(tekst);
        $("#statusMeldingOK").fadeIn('fast').delay(15000).fadeOut('slow');
    } else {
        $("#statusMeldingFeil").html(tekst);
        $("#statusMeldingFeil").fadeIn('fast').delay(15000).fadeOut('slow');
    }
}


function validerTelefonnr(telefonnr) {
    const regex = /^[0-9]{8}$/;
    const ok = regex.test(telefonnr);

    if (!ok) {
        statusMelding("Telefon nummeret må være et åttesifred nummer", false);
        return false;
    }
    else {
        return true;
    }
}

function validerKort(kortNr, kortCVC) {
    const kortNrRegex = /\b\d{13,16}\b$/;
    const kortCVCRegex = /^[0-9]{3,4}$/;
    const nrOK = kortNrRegex.test(kortNr);
    const cvcOK = kortCVCRegex.test(kortCVC);

    if (!nrOK && !cvcOK) {
        statusMelding("Både nummer og CVC er feil!", false);
        return false;
    } else if (!nrOK) {
        statusMelding("Kort nummer er feil!", false);
        return false;
    } else if (!cvcOK) {
        statusMelding("Kort CVC er feil!", false);
        return false;
    } else {
        return true;
    }
}

function validerEmail(email) {
    const regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    const emailOK = regex.test(email);

    if (!emailOK) {
        statusMelding("Email er ikke i akseptabelt format", false);
        return false;
    }
    else {
        return true;
    }
}