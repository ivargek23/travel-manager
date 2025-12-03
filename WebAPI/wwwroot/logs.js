const allLogsUrl = "http://localhost:5009/api/Logs/Get/N";

function jwtLogout() {
    localStorage.removeItem("JWT");

    window.location.href = "login.html";
}

function fillLogs() {
    let jwt = localStorage.getItem("JWT");

    $.ajax({
        url: allLogsUrl,
        headers: { "Authorization": `Bearer ${jwt}` }
    }).done(function (logs) {
        showLogs(logs);
    }).fail(function () {
        console.error("There was an error while trying to load your data");
    });
}

function showLogs() {
    let numberOfLogs = $("#number-of-logs").val();
    let jwt = localStorage.getItem("JWT");
    $.ajax({
        url: allLogsUrl.replace("N", numberOfLogs),
        headers: { "Authorization": `Bearer ${jwt}` }
    }).done(function (logs) {
        renderLogs(logs);
    }).fail(function () {
        console.error("There was an error while trying to load your data");
    });
}

function renderLogs(logs) {
    const $ph = $("#placeholder");
    $ph.empty();

    for (let log of logs) {
        var level = log.logLevel === 1 ? "Info" : "Error";
        const html = `
        <div class="log">
        <p><b>Date and time:</b> ${log.timestamp}</p>
        <p><b>Log level:</b> ${level}</p>
        <p><b>Description:</b> ${log.message}</p>`;

        $ph.append(html);
    }
}

$(function () {
    let jwt = localStorage.getItem("JWT");
    if (!jwt) {
        window.location.href = "login.html";
    }

    fillLogs();
});