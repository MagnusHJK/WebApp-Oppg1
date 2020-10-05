function validerBrukernavn(brukernavn) {
    const regex = /^[a-zA-ZæøåÆØÅ\.\ \-0-9]{2,20}$/;
    const ok = regex.test(brukernavn);

    if (!ok) {
        $("#feilBrukernavn").html("Brukernavnet må bestå av 2 til 20 tegn");
        return false;
    }
    else {
        $("#feilBrukernavn").html("");
        return true;
    }
}


function validerPassord(passord) {
    const regex = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&-])[A-Za-z\d@$!%*#?&-]{6,}$/;
    const ok = regex.test(passord);

    if (!ok) {
        $("#feilPassord").html("Passordet må bestå av minst 6 tegn, bestående av minst en stor og liten bokstav, tall og spesialltegn");
        return false;
    }
    else {
        $("#feilPassord").html("");
        return true;
    }
}

