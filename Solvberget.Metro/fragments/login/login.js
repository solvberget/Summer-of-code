(function () {
    "use strict";
    var page = WinJS.UI.Pages.define("/fragments/login/login.html", {
        ready: function (element, options) {

        }
    });
    
    // Track if the log in was successful
    var loggedIn;    // Track if the log in was successful
    
    function cancel() {
        var flyout = document.getElementById("loginFlyout");
        flyout.winControl.hide();
    }



    // !------------ AJAX METHODS -------------! //

    var ajaxDoLogin = function (userId, verification, context) {
        var url = Data.serverBaseUrl + "/User/GetUserInformation/" + userId + "/" + verification;
        Solvberget.Queue.QueueDownload("login", { url: url }, ajaxDoLoginCallback, context, true);
    };


    // !------------ AJAX CALLBACKS -------------! //

    var ajaxDoLoginCallback = function (request, context) {
        var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

        if (response.IsAuthorized) {

            $("#outputMsg").html("Du er nå logget inn!");
            $("#outputMsg").removeClass("normal");
            $("#outputMsg").removeClass("error");
            $("#outputMsg").addClass("success");

            var borrowerId = response.BorrowerId;
            var libraryId = response.Id;
            var notifications = response.Notifications;

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

                if (notifications) {
                    if (!Notifications.areNotificationsSeen()) {
                        for (var i = 0; i < notifications.length; i++) {
                            Toast.showToast(notifications[i].Title, notifications[i].Content);
                        }
                    }
                    Notifications.setUserNotifications(notifications);
                }

                //Notifications.setAreNotificationsSeen(true);

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
                LiveTile.liveTile();
            }, 1200);



        }
        else {

            $("#outputMsg").html("Feil lånernummer/pin");
            $("#outputMsg").removeClass("normal");
            $("#outputMsg").removeClass("success");
            $("#outputMsg").addClass("error");
            $("#submitLoginButton").removeAttr("disabled");

        }
        $("#loginLoading").hide();

    };


    // !------------ END AJAX END -------------! //


    var navigateToUrl = '';

    // Show the flyout
    function showLoginFlyout(element, navigateTo) {
        loggedIn = false;
        $("#loginLoading").hide();

        navigateToUrl = navigateTo;
        WinJS.log && WinJS.log("", "solvberget", "status", "status");

        var loginFlyout = document.getElementById("loginFlyout");

        if (LoginFlyout.getLoggedInBorrowerId() == undefined || LoginFlyout.getLoggedInBorrowerId() == "") {

            WinJS.UI.processAll(loginFlyout);

            document.getElementById("submitLoginButton").addEventListener("click", submitLogin, false);
            document.getElementById("loginFlyout").addEventListener("afterhide", onDismiss, false);
            document.getElementById("outputMsg").addEventListener("click", showPinRrequestFlyout, false);


            $("#logoutConfimationMsg").css("display", "none").css("visibility", "hidden");
            $("#confirmLogoutButton").css("display", "none").css("visibility", "hidden");
            $("#cancelLogoutButton").css("display", "none").css("visibility", "hidden");

            loginFlyout.winControl.show(element, "right");

        }
        else if (navigateTo != undefined && navigateTo != "") {

            WinJS.Navigation.navigate(navigateTo);

        }
        else {

            WinJS.UI.processAll(loginFlyout);

            document.getElementById("confirmLogoutButton").addEventListener("click", logout, false);
            document.getElementById("cancelLogoutButton").addEventListener("click", cancel, false);

            $("#logoutConfimationMsg").css("display", "block").css("visibility", "visible");
            $("#confirmLogoutButton").css("display", "block").css("visibility", "visible");
            $("#cancelLogoutButton").css("display", "block").css("visibility", "visible");

            $("#labelForUserId").css("display", "none").css("visibility", "hidden");
            $("#userId").css("display", "none").css("visibility", "hidden");
            $("#laberForPin").css("display", "none").css("visibility", "hidden");
            $("#pin").css("display", "none").css("visibility", "hidden");
            $("#submitLoginButton").css("display", "none").css("visibility", "hidden");
            $("#outputMsg").css("display", "none").css("visibility", "hidden");
            $("#loginLoading").hide();

            loginFlyout.winControl.show(element, "right");

        }

    }

    function logout() {
        LoginFlyout.logout();
    }



    // Show errors if any of the text fields are not filled out when the Login button is clicked
    function submitLogin() {
        var error = false;
        $("#submitLoginButton").attr("disabled", "disabled");

        if (document.getElementById("pin").value.trim() === "") {
            document.getElementById("pinError").innerHTML = "Pinkoden kan ikke være tom";
            $("#submitLoginButton").removeAttr("disabled");
            document.getElementById("pin").focus();
            error = true;
        } else {
            document.getElementById("pinError").innerHTML = "";
        }
        if (document.getElementById("userId").value.trim() === "") {
            document.getElementById("userIdError").innerHTML = "Lånekortnummer må fylles inn";
            $("#submitLoginButton").removeAttr("disabled");
            document.getElementById("userId").focus();
            error = true;
        } else {
            document.getElementById("userIdError").innerHTML = "";
        }

        if (!error) {

            $("#outputMsg").html("");

            $("#loginLoading").fadeIn();

            var context = { that: this };
            ajaxDoLogin($("#userId").val(), $("#pin").val(), context);

        }
    }

    // On dismiss of the flyout, reset the fields in the flyout
    function onDismiss() {

        // Clear fields on dismiss
        document.getElementById("userId").value = "";
        document.getElementById("userIdError").innerHTML = "";
        document.getElementById("pin").value = "";
        document.getElementById("pinError").innerHTML = "";
        document.getElementById("outputMsg").innerHTML = "Glemt pin-kode?";

        $("#outputMsg").removeClass("error");
        $("#outputMsg").removeClass("success");
        $("#outputMsg").addClass("normal");
        $("#submitLoginButton").removeAttr("disabled");

        var loginDivHolder = document.getElementById("loginDiv");
        if (loginDivHolder)
            loginDivHolder.innerHTML = "";

        if (!loggedIn) {
            WinJS.log && WinJS.log("Du er ikke logget inn.", "solvberget", "status");
        }
        Solvberget.Queue.CancelQueue('login');

    }

    function showPinRrequestFlyout() {

        var flyout = document.getElementById("loginFlyout");
        if (flyout != undefined)
            flyout.winControl.hide();

        var pinRequestDiv = document.getElementById("pinRequestFragmentHolder");
        pinRequestDiv.innerHTML = "";

        WinJS.UI.Fragments.renderCopy("/fragments/pinRequest/pinRequest.html", pinRequestDiv).done(function () {
            var pinRequestAnchor = document.querySelector("div");
            PinRequestFlyout.showPinRequestFlyout(pinRequestAnchor, null);
        });

    }

    WinJS.Namespace.define("LoginFlyout", {
        showLogin: showLoginFlyout,

    });

})();
