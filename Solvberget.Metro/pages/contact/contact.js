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
            element.querySelector(".titlearea").addEventListener("click", this.showHeaderMenu, false);
            document.getElementById("headerMenuLists").addEventListener("click", function () { window.Data.itemByKey("lists").navigateTo(); }, false);
            document.getElementById("headerMenuEvents").addEventListener("click", function () { window.Data.itemByKey("events").navigateTo(); }, false);
            document.getElementById("headerMenuSearch").addEventListener("click", function () { window.Data.itemByKey("search").navigateTo(); }, false);
            document.getElementById("headerMenuHomeMenuItem").addEventListener("click", function () { self.goHome(); }, false);

            var theMenu = document.getElementById("HeaderMenu");
            WinJS.UI.processAll(theMenu);

            getContactInformation();
            $("#renewalLoading").css("display", "none").css("visibility", "hidden");

        },


        showHeaderMenu: function () {

            var title = document.querySelector("header .titlearea");
            var menu = document.getElementById("HeaderMenu").winControl;
            menu.anchor = title;
            menu.placement = "bottom";
            menu.alignment = "left";

            menu.show();

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