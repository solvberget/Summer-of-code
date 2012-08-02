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

        window.localStorage.setItem("BorrowerId", "");
        window.localStorage.setItem("LibraryUserId", "");

        var applicationData = Windows.Storage.ApplicationData.current;
        var roamingSettings = applicationData.roamingSettings;

        roamingSettings.values["BorrowerId"] = "";
        roamingSettings.values["LibraryUserId"] = "";

        updateAppBarButton();
    }


    WinJS.Namespace.define("LoginFlyout", {
        getLoggedInBorrowerId: getLoggedInBorrowerId,
        getLoggedInLibraryUserId: getLoggedInLibraryUserId,
        updateAppBarButton: updateAppBarButton,
        logout : logout,
    });

})();