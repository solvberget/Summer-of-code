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

            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());

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
    $("#openingHoursContent").css("display", "none").css("visibility", "none");
    $("#openingHoursLoading").css("display", "block").css("visibility", "visible");

    // Get the user information from server
    $.when(ajaxGetOpeningHoursInformation())
        .then($.proxy(function (response) {
     

                // avoid processing null (if user navigates to fast away from page etc)
                if (response != undefined && response !== "") {
                   
                    populateOpeningHours(response);
                }

            // Hide progress-ring, show content
            $("#openingHoursContent").css("display", "block").css("visibility", "visible");
            $("#openingHoursLoading").css("display", "none").css("visibility", "none");


        }, this)
    );


    var populateOpeningHours = function (response) {

        var openingHoursTemplateDiv = document.getElementById("openingHoursInformationTemplate");
        var openingHoursTemplateHolder = document.getElementById("openingHoursInformationTemplateHolder");

        var openingHoursTemplate = undefined;
        if (openingHoursTemplateDiv)
            openingHoursTemplate = new WinJS.Binding.Template(openingHoursTemplateDiv);

        var model;

        if (response) {

            for (var i = 0; i < response.length; i++) {
                model = response[i];

                if (openingHoursTemplate && openingHoursTemplateHolder && model)
                    openingHoursTemplate.render(model, openingHoursTemplateHolder);

            }
        }

    };
}
