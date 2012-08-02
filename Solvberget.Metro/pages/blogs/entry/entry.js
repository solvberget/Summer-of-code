(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/blogs/entry/entry.html", {

        ready: function (element, options) {

            // TODO: Check this 
            populateEntry(options.entry);

        },

        goHome: function () {
            WinJS.Navigation.navigate("/pages/home/home.html");

        },
    });

})();


var populateEntry = function (entry) {

    var entryTemplateDiv = document.getElementById("blogTemplate");
    var entryTemplateHolder = document.getElementById("entryTemplateHolder");

    var blogTemplate = undefined;
    if (entryTemplateDiv)
        blogTemplate = new WinJS.Binding.Template(entryTemplateDiv);

    var model;

    if (entry) {

        blogTemplate.render(entry, entryTemplateHolder);          
    }

};

