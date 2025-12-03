let loginUrl = "http://localhost:5009/api/Auth/Login";

function jwtLogin() {
    $("#spinner-placeholder").addClass("spinner");
    $("#login-button").prop("disabled", true);

    let loginData = {
        "username": $("#username").val(),
        "password": $("#password").val()
    }
    if (loginData.username === "" || loginData.password === "") {
        $("#error-message").text("Enter username and password");
        $("#error-message").css("visibility", "visible");
        $("#spinner-placeholder").removeClass("spinner");
        $("#login-button").prop("disabled", false);
        return;
    }
    $.ajax({
        url: loginUrl,
        method: "POST",
        data: JSON.stringify(loginData),
        contentType: 'application/json'
    }).done(function (tokenData) {
        console.log(tokenData);
        localStorage.setItem("JWT", tokenData);

        $("#spinner-placeholder").removeClass("spinner");
        $("#login-button").prop("disabled", false);

        window.location.href = "logs.html";
    }).fail(function (err) {
        $("#error-message").text("Wrong username or password");
        $("#error-message").css("visibility", "visible");

        localStorage.removeItem("JWT");
        $("#spinner-placeholder").removeClass("spinner");
        $("#login-button").prop("disabled", false);
    });
}
