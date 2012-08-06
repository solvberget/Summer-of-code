(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/blogs/entries/entries.html", {

        ready: function (element, options) {

            var blogId = options.blogId;
            var blogModel = options.blogModel;
            WinJS.Binding.processAll(element.querySelector(".fragment"), blogModel);
            getBlogWithEntries(blogId);

        },
    });

})();


var ajaxGetBlogWithEntries = function (blogId) {
    return $.getJSON(window.Data.serverBaseUrl + "/Blog/GetBlogWithEntries/" + blogId);
};



var getBlogWithEntries = function (blogId) {

    // Show progress-ring, hide content
    $("#entriesContent").hide();
    $("#entriesLoading").fadeIn();

    // Get the user information from server
    $.when(ajaxGetBlogWithEntries(blogId))
        .then($.proxy(function (response) {

            if (response != undefined && response !== "")
                populateEntries(response);

            // Hide progress-ring, show content
            $("#entriesContent").fadeIn("slow");
            $("#entriesLoading").hide();

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

            for (var i = 0; i < response.Entries.length; i++) {

                model = response.Entries[i];

                if (entryTemplate && entriesTemplateHolder && model)
                    entryTemplate.render(model, entriesTemplateHolder).done($.proxy(function () {

                        $(".entry:last").css("background-color", Data.getRandomColor());

                        $(".entry:last").click($.proxy(function () {
                            WinJS.Navigation.navigate("pages/blogs/entry/entry.html", { model: this });
                        }, this));
                    }, model));
            }
        }

    };



};