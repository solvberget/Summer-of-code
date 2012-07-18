(function () {

    // Track if the log in was successful
    var loggedIn;

    "use strict";
    var page = WinJS.UI.Pages.define("/fragments/login/login.html", {
        ready: function (element, options) {

        }
    });

    // Ajax for login
    function ajaxDoLogin(userId, verification) {
        return $.getJSON("http://localhost:7089/User/GetUserInformation/" + userId + "/" + verification);
    }

    var navigateToUrl = '';

    // Show the flyout
    function showLoginFlyout(element, navigateTo) {
        loggedIn = false;
        navigateToUrl = navigateTo;
        WinJS.log && WinJS.log("", "solvberget", "status", "status");

        
        if (getLoggedInBorrowerId() == undefined || getLoggedInBorrowerId() == "") {

            var loginFlyout = document.getElementById("loginFlyout");
            WinJS.UI.processAll(loginFlyout);

            document.getElementById("submitLoginButton").addEventListener("click", submitLogin, false);
            document.getElementById("loginFlyout").addEventListener("afterhide", onDismiss, false);

            loginFlyout.winControl.show(element, "right");

        }
        else {

            WinJS.Navigation.navigate(navigateTo);

        }


    }

    function getLoggedInBorrowerId() {
        var borrowerId = window.localStorage.getItem("BorrowerId");
        return borrowerId != undefined ? borrowerId : "";
    }

    // Show errors if any of the text fields are not filled out when the Login button is clicked
    function submitLogin() {
        var error = false;
        if (document.getElementById("pin").value.trim() === "") {
            document.getElementById("pinError").innerHTML = "Pinkoden kan ikke være tom";
            document.getElementById("pin").focus();
            error = true;
        } else {
            document.getElementById("pinError").innerHTML = "";
        }
        if (document.getElementById("userId").value.trim() === "") {
            document.getElementById("userIdError").innerHTML = "Lånekortnummer må fylles inn";
            document.getElementById("userId").focus();
            error = true;
        } else {
            document.getElementById("userIdError").innerHTML = "";
        }

        if (!error) {

            $("#loginLoading").css("display", "block").css("visibility", "visible");
            document.getElementById("outputMsg").innerHTML = "";

            $.when(ajaxDoLogin($("#userId").val(), $("#pin").val()))
                .then($.proxy(function (response) {

                    if (response.IsAuthorized) {

                        document.getElementById("outputMsg").innerHTML = "Du er nå logget inn!";
                        $("#outputMsg").removeClass("error");
                        $("#outputMsg").addClass("success");

                        var borrowerId = response.BorrowerId;
                        if (borrowerId != undefined) {
                            window.localStorage.setItem("BorrowerId", borrowerId);
                        }

                        setTimeout(function () {
                            document.getElementById("loginFlyout").winControl.hide();                                                  
                        }, 1200);

                        setTimeout(function () {
                            if (navigateToUrl && navigateToUrl != "")
                                WinJS.Navigation.navigate(navigateToUrl);
                        }, 1400);

                    }
                    else {

                        document.getElementById("outputMsg").innerHTML = "Feil lånernummer/pin";

                        $("#outputMsg").removeClass("success");
                        $("#outputMsg").addClass("error");

                    }
                    $("#loginLoading").css("display", "none").css("visibility", "hidden");

                }, this));

        }
    }

    // On dismiss of the flyout, reset the fields in the flyout
    function onDismiss() {

        // Clear fields on dismiss
        document.getElementById("userId").value = "";
        document.getElementById("userIdError").innerHTML = "";
        document.getElementById("pin").value = "";
        document.getElementById("pinError").innerHTML = "";
        document.getElementById("outputMsg").innerHTML = "";


        if (!loggedIn) {
            WinJS.log && WinJS.log("Du er ikke logget inn.", "solvberget", "status");
        }
    }

    WinJS.Namespace.define("LoginFlyout", {
        showLogin: showLoginFlyout,
        getLoggedInBorrowerId: getLoggedInBorrowerId
    });

})();
