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


        if (LoginFlyout.getLoggedInBorrowerId() == undefined || LoginFlyout.getLoggedInBorrowerId() == "") {

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

            var outputMsg = document.getElementById("outputMsg");
            if (this.outputMsg != undefined)

                outputMsg.innerHTML = "";

            $.when(ajaxDoLogin($("#userId").val(), $("#pin").val()))
                .then($.proxy(function (response) {

                    var outputMsg = document.getElementById("outputMsg");


                    if (response.IsAuthorized) {

                        if (outputMsg != undefined) {
                            outputMsg.innerHTML = "Du er nå logget inn!";
                        }
                        $("#outputMsg").removeClass("error");
                        $("#outputMsg").addClass("success");

                        var borrowerId = response.BorrowerId;
                        var libraryId = response.Id;

                        if (borrowerId != undefined && libraryId != undefined) {

                            var applicationData = Windows.Storage.ApplicationData.current;

                            if (applicationData)
                                var roamingSettings = applicationData.roamingSettings;

                            if (roamingSettings) {
                                roamingSettings.values["BorrowerId"] = borrowerId;
                                roamingSettings.values["LibraryUserId"] = libraryId;
                            }

                            window.localStorage.setItem("BorrowerId", borrowerId);
                            window.localStorage.setItem("LibraryUserId", libraryId);

                            LoginFlyout.updateAppBarButton();

                        }

                        setTimeout(function () {
                            var flyout = document.getElementById("loginFlyout");
                            if (flyout != undefined)
                                flyout.winControl.hide();
                            setTimeout(function () {
                                if (navigateToUrl && navigateToUrl != "")
                                    WinJS.Navigation.navigate(navigateToUrl);
                            }, 200);
                        }, 1200);



                    }
                    else {
                        if (outputMsg != undefined)
                            outputMsg.innerHTML = "Feil lånernummer/pin";

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

        var loginDivHolder = document.getElementById("loginDiv");
        if (loginDivHolder)
            loginDivHolder.innerHTML = "";

        if (!loggedIn) {
            WinJS.log && WinJS.log("Du er ikke logget inn.", "solvberget", "status");
        }
    }

    WinJS.Namespace.define("LoginFlyout", {
        showLogin: showLoginFlyout,

    });

})();
