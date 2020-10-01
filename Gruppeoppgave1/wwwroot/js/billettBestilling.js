//Lager "billett" som gir info om valgte avganger
function lagBillett(avgangId, datoAvgang, pris) {
    let ut = "";
    let knapper = "";
    var dato = new Date(datoAvgang);
    var antall = $("#antallBilletter").val();

    $("#reiseValg").hide();
    $("#avganger").html("");


    //IF STEP 5 (Billett avreise)
    if (step == 5) {
        step = 6;
        avgangerId[0] = avgangId;

        ut = "<h4>Avreise:</h4>" +
            "<b>Fra: " + stasjonFra.navn + " Til: " + stasjonTil.navn + "</b><br>" +
            dato.toString() + "<br>" +
            antall + " billett(er) til " + pris + ",- per stk <br>";
        $("#avreise").html(ut);

        knapper = "<button class='btn btn-warning' id='endreAvganger' onclick='endreAvganger()'>Endre</button>  " +
                  "<button class='btn btn-secondary' id='returKnapp' onclick='lagRetur(\"" + datoAvgang + "\")'>Jeg ønsker retur billett</button>  " +
                  "<button class='btn btn-success' id='bestillKnapp' onclick='lagBestilling()'>Bestill</button>";

        $("#knapper").html(knapper);
    }
    //IF STEP: 6 (Billett retur)
    else if (step == 6) {
        step = 7;
        avgangerId[1] = avgangId;

        ut = "<h4>Retur:</h4>" +
            "<b>Fra: " + stasjonTil.navn + " Til: " + stasjonFra.navn + "</b><br>" +
            dato.toString() + "<br>" +
            antall + " billett(er) til " + pris + ",- per stk";
        $("#retur").html(ut);

        $("#returKnapp").remove();
    }
}

//Når alle valg er utført lager vi selve bestillingen til backend
function lagBestilling() {
    var antall = $("#antallBilletter").val();
    var sannhet = false;

    for (var i = 0; i < avgangerId.length; i++) {
        const bestilling = {
            avgangId: avgangerId[i],
            antall: antall
        }
        sannhet = bestillingAjax(bestilling);
    }
    //Separat sannhet som avgjør, ettersom det er i loop.
    if (sannhet) {
        window.location.href = 'bestillingsliste.html';
    } else {
        $("#feil").html("Feil i db - prøv igjen senere");
    }
}

//Ajax kall for bestilling i egen funskjon
function bestillingAjax(bestilling) {
    const url = "Bestilling/LagBestilling";
    var sannhet = false;

    $.ajax({
        method: 'get',
        url: url,
        data: bestilling,
        async: false
    })
        .done(function () {
            sannhet = true;
        })
        .fail(function () {
            sannhet = false;
        });
    return sannhet;
}