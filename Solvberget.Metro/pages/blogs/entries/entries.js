(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/blogs/entries/entries.html", {

        ready: function (element, options) {

            var blogId = options.Id;
            getBlogWithEntries(blogId);

        },

        goHome: function () {
            WinJS.Navigation.navigate("/pages/home/home.html");

        },
    });

})();


var ajaxGetBlogWithEntries = function (blogId) {
    return $.getJSON(window.Data.serverBaseUrl + "/Blog/GetBlogWithEntries/"+blogId);
};



var getBlogWithEntries = function (blogId) {

    // Show progress-ring, hide content
    $("#entriesContent").css("display", "none").css("visibility", "none");
    $("#entriesLoading").css("display", "block").css("visibility", "visible");

    // Get the user information from server
    $.when(ajaxGetBlogWithEntries(blogId))
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {
                populateEntries(response);
            }



            // Hide progress-ring, show content
            $("#entriesContent").css("display", "block").css("visibility", "visible");
            $("#entriesLoading").css("display", "none").css("visibility", "none");


        }, this)
    );


    var populateEntries = function (response) {

        var entriesTemplateDiv = document.getElementById("entryTemplate");
        var entriesTemplateHolder = document.getElementById("entriesTemplateHolder");

        var entryTemplate = undefined;
        if (entriesTemplateDiv)
            entryTemplate = new WinJS.Binding.Template(entriesTemplateDiv);

        var model;

        if (response) {

            for (var i = 0; i < response.length; i++) {
                model = response[i];

                if (entryTemplate && entriesTemplateHolder && model)
                    entryTemplate.render(model, entriesTemplateHolder);

            }
        }

    };




};