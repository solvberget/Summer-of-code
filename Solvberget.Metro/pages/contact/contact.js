(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    var self;
    ui.Pages.define("/pages/contact/contact.html", {

        ready: function (element, options) {

            self = this;

            getContactInformation();
            $("#renewalLoading").css("display", "none").css("visibility", "hidden");

            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());

        },
        unload: function () {
            Solvberget.Queue.CancelQueue('contact');
        }
    });

})();


var ajaxGetContactInformation = function () {
    var url = window.Data.serverBaseUrl + "/Document/GetContactInformation"; 
    Solvberget.Queue.QueueDownload("contact", { url: url }, ajaxGetContantInformationCallback, this, true);

};

var ajaxGetContantInformationCallback = function (request, context) {
    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

    if (response != undefined && response !== "") {
        populateContact(response, context);
    }

    // Hide progress-ring, show content
    $("#contactContent").fadeIn("slow");
    $("#contactLoading").hide();

};


var populateContact = function (response, context) {

    var contactTemplateDiv = document.getElementById("contactTemplate");
    var contactTemplateHolder = document.getElementById("contactInformationTemplateHolder");

    var contactTemplate = undefined;
    if (contactTemplateDiv)
        contactTemplate = new WinJS.Binding.Template(contactTemplateDiv);

    var model;

    if (response) {

        for (var i = 0; i < response.length; i++) {
            model = response[i];

            if (contactTemplate && contactTemplateHolder && model)
                contactTemplate.render(model, contactTemplateHolder).done($.proxy(function () {
                    $(".contactTile:last").css("background-color", Data.getRandomColor());
                }, context));

        }
    }

};

var getContactInformation = function () {

    // Show progress-ring, hide content
    $("#contactContent").hide();
    $("#contactLoading").fadeIn();

    // Get the user information from server
    ajaxGetContactInformation();

};