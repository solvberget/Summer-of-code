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

        },

        goHome: function () {
            WinJS.Navigation.navigate("/pages/home/home.html");

        },
    });

})();


var ajaxGetContactInformation = function () {

    return $.getJSON(window.Data.serverBaseUrl + "/Document/GetContactInformation");
};



var getContactInformation = function () {

    // Show progress-ring, hide content
    $("#contactContent").css("display", "none").css("visibility", "none");
    $("#contactLoading").css("display", "block").css("visibility", "visible");

    // Get the user information from server
    $.when(ajaxGetContactInformation())
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {

               
                    populateContact(response);
                }
                    


            // Hide progress-ring, show content
            $("#contactContent").css("display", "block").css("visibility", "visible");
            $("#contactLoading").css("display", "none").css("visibility", "none");
            

        }, this)
    );
    

    var populateContact = function (response) {

        var contactTemplateDiv = document.getElementById("contactInformationTemplate");
        var contactTemplateHolder = document.getElementById("contactInformationTemplateHolder");

        var contactTemplate = undefined;
        if (contactTemplateDiv)
            contactTemplate = new WinJS.Binding.Template(contactTemplateDiv);

        var model;

        if (response) {

            for (var i = 0; i < response.length; i++) {
                model = response[i];

                if (contactTemplate && contactTemplateHolder && model)
                    contactTemplate.render(model, contactTemplateHolder);

            }
        }

    };


   

};