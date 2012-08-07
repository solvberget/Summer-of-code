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
        unload: function () {
            Solvberget.Queue.CancelQueue('opening');
        }

    });

})();


var ajaxGetOpeningHoursInformation = function () {
    var url = window.Data.serverBaseUrl + "/Document/GetOpeningHoursInformation";
    Solvberget.Queue.QueueDownload("opening", { url: url }, ajaxGetOpeningHoursInformationCallback, this, true);

};

var ajaxGetOpeningHoursInformationCallback = function (request, context) {
    var response = JSON.parse(request.responseText);

    // avoid processing null (if user navigates to fast away from page etc)
    if (response != undefined && response !== "")
        populateOpeningHours(response);


    // Hide progress-ring, show content
    $("#openingHoursContent").fadeIn("slow");
    $("#openingHoursLoading").hide();

};

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

var getOpeningHoursInformation = function () {

    // Show progress-ring, hide content
    $("#openingHoursContent").hide();
    $("#openingHoursLoading").fadeIn();

    // Get the user information from server
    ajaxGetOpeningHoursInformation();

}
