function loggUt() {
    $.get("Bruker/LoggUt", function () {
        window.location.href = 'loggInn.html';
    });
}