(function () {

    var delayImageLoader = WinJS.Class.define(
            function (element, options) {
                this._element = element || document.createElement("div");
                this.element.winControl = this;
                WinJS.Utilities.addClass(this.element, "imageLoader");
                WinJS.Utilities.query("img", element).forEach(function (img) {
                    img.addEventListener("load", function () {
                        WinJS.Utilities.addClass(img, "loaded");
                    });
                });
            },
            {
                element: {
                    get: function () { return this._element; }
                },
            });

    WinJS.Namespace.define("Solvberget.Utils", {
        DelayImageLoader: delayImageLoader
    });
})();

(function () {

    var styleNullToHiddenConverter = WinJS.Binding.converter(function (val) {
        var returnvalue;
        if (val != null) {
            returnvalue = val.toString() == [] ? "none" : "block";
            ;
        } else {
            returnvalue = "none";
        }
        return returnvalue;
    });
    WinJS.Namespace.define("Solvberget.Converters", {
        styleNullToHiddenConverter: styleNullToHiddenConverter
    });
})();

(function () {


    function getLoggedInBorrowerId() {


        var applicationData = Windows.Storage.ApplicationData.current;
        if (applicationData)
            var roamingSettings = applicationData.roamingSettings;

        var borrowerId = undefined;
        if (roamingSettings) {
            borrowerId = roamingSettings.values["BorrowerId"];
        }
        if (borrowerId == undefined || borrowerId == "")
            borrowerId = window.localStorage.getItem("BorrowerId");
        
        return borrowerId != undefined ? borrowerId : "";
    }

    function getLoggedInLibraryUserId() {

        var applicationData = Windows.Storage.ApplicationData.current;
        if (applicationData)
            var roamingSettings = applicationData.roamingSettings;

        var libraryUserId = undefined;
        if (roamingSettings) {
            libraryUserId = roamingSettings.values["LibraryUserId"];
        }
        
        if (libraryUserId == undefined || libraryUserId == "")
            libraryUserId = window.localStorage.getItem("LibraryUserId");

        return libraryUserId != undefined ? libraryUserId : "";
    }
    function updateAppBarButton() {

        if (document.getElementById("cmdLoginFlyout")) {
            var user = LoginFlyout.getLoggedInBorrowerId();
            if (user && user != "") {
                document.getElementById("cmdLoginFlyout").winControl.label = "Logg ut";
                document.getElementById("cmdLoginFlyout").winControl.tooltip = "Logg ut";
            }
            else {
                document.getElementById("cmdLoginFlyout").winControl.label = "Logg inn";
                document.getElementById("cmdLoginFlyout").winControl.tooltip = "Logg inn";
            }
        }

    }
    function logout() {

        var activePage = WinJS.Navigation.location;

        window.localStorage.setItem("BorrowerId", "");
        window.localStorage.setItem("LibraryUserId", "");

        var applicationData = Windows.Storage.ApplicationData.current;
        var roamingSettings = applicationData.roamingSettings;

        roamingSettings.values["BorrowerId"] = "";
        roamingSettings.values["LibraryUserId"] = "";

        document.getElementById("logoutConfimationMsg").innerHTML = "Du blir nå logget ut";
               
        setTimeout(function () {
            var flyout = document.getElementById("loginFlyout");
            if (flyout != undefined)
                flyout.winControl.hide();
            if (WinJS.Navigation.location == "/pages/mypage/mypage.html")
                Data.navigateToHome();
            updateAppBarButton();
        }, 1200);


        setTimeout(function () {        
            $("#logoutConfimationMsg").css("display", "none").css("visibility", "hidden");
            $("#confirmLogoutButton").css("display", "none").css("visibility", "hidden");
            $("#cancelLogoutButton").css("display", "none").css("visibility", "hidden");

            $("#labelForUserId").css("display", "block").css("visibility", "visible");
            $("#userId").css("display", "block").css("visibility", "visible");
            $("#laberForPin").css("display", "block").css("visibility", "visible");
            $("#pin").css("display", "block").css("visibility", "visible");
            $("#submitLoginButton").css("display", "block").css("visibility", "visible");
            $("#outputMsg").css("display", "block").css("visibility", "visible");
        }, 1300);
        
    }

    WinJS.Namespace.define("LoginFlyout", {
        getLoggedInBorrowerId: getLoggedInBorrowerId,
        getLoggedInLibraryUserId: getLoggedInLibraryUserId,
        updateAppBarButton: updateAppBarButton,
        logout : logout,
    });

})();