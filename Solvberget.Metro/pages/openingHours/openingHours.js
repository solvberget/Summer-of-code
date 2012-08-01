(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    var self;
    ui.Pages.define("/pages/openingHours/openingHours.html", {

        ready: function (element, options) {

            self = this;

            getOpeningHoursInformation();
            $("#renewalLoading").css("display", "none").css("visibility", "hidden");

        },

        goHome: function () {
            WinJS.Navigation.navigate("/pages/home/home.html");

        },
    });

})();


var ajaxGetOpeningHoursInformation = function () {

    return $.getJSON(window.Data.serverBaseUrl + "/Document/GetOpeningHoursInformation");
};



var getOpeningHoursInformation = function () {

    // Show progress-ring, hide content
    $("#openingHoursData").css("display", "none").css("visibility", "none");
    $("#openingHoursLoading").css("display", "block").css("visibility", "visible");

    // Get the user information from server
    $.when(ajaxGetOpeningHoursInformation())
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {
                // Select HTML-section to process with the new binding lists
                var contentDiv = document.getElementById("openingHoursData");
                
                // avoid processing null (if user navigates to fast away from page etc)
                if (contentDiv != undefined && response != undefined) {
                    var data = {
                        InformationValue: response[0].InformationValue
                    };
                    WinJS.Binding.processAll(contentDiv, data);
                }
                    
            }

            // Hide progress-ring, show content
            $("#openingHoursData").css("display", "block").css("visibility", "visible");
            $("#openingHoursLoading").css("display", "none").css("visibility", "none");
            

        }, this)
    );







};