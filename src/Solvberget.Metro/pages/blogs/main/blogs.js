(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/blogs/main/blogs.html", {

        ready: function (element, options) {
            getBlogs();

            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());
        },
        unload: function () {
            Solvberget.Queue.CancelQueue('blogs');
        }
    });
})();

var ajaxGetBlogs = function() {
    var url = window.Data.serverBaseUrl + "/Blog/GetBlogs";
    Solvberget.Queue.QueueDownload("blogs", { url: url }, ajaxGetBlogsCallback, this, true);
};

var ajaxGetBlogsCallback = function (request, context) {
    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
    if (response != undefined && response !== "") {
        populateBlogs(response);
    }
    $("#blogsContent").fadeIn("slow");
    $("#blogsLoading").hide();
};

var populateBlogs = function (response) {

    var blogsTemplateDiv = document.getElementById("blogTemplate");
    var blogsTemplateHolder = document.getElementById("blogsTemplateHolder");

    var blogTemplate = undefined;
    if (blogsTemplateDiv)
        blogTemplate = new WinJS.Binding.Template(blogsTemplateDiv);

    var model;

    if (response) {
        for (var i = 0; i < response.length; i++) {
            model = response[i];
            var context = { model: model, index: i };
            if (blogTemplate && blogsTemplateHolder && model) {
                blogTemplate.render(model, blogsTemplateHolder).done($.proxy(function () {
                    $(".blog:last").css("background-color", Data.getColorFromBlogsPool(i%3, "1.0"));

                    $(".blog:last").click($.proxy(function () {
                        var blogId = this.index;
                        var blogModel = this.model;
                        WinJS.Navigation.navigate("pages/blogs/entries/entries.html", { blogId: blogId, blogModel: blogModel });
                    }, this));
                }, context));
            }
        }
    }
};

var getBlogs = function () {
    $("#blogsContent").hide();
    $("#blogsLoading").fadeIn();
    ajaxGetBlogs();
};


WinJS.Namespace.define("BlogConverters", {
    cateogriesConverter: WinJS.Binding.converter(function (categories) {
        var outputStr = "Kategorier: ";
        for (x in categories) {
            outputStr += categories[x] + ", ";
        }
        outputStr = outputStr.substr(0, outputStr.length - 2);
        return outputStr;
    }),
    entriesConverter: WinJS.Binding.converter(function (entries) {
        return "Antall innlegg: " + entries.length;
    }),
    subtitleConverter: WinJS.Binding.converter(function (title) {
        return "De siste innleggene fra " + title;
    }),
    publishedDate: WinJS.Binding.converter(function (published) {
        return "Publisert: " + published;
    }),
    updatedDate: WinJS.Binding.converter(function (updated) {
        return "Sist oppdatert: " + updated;
    }),
    descriptionWrapper: WinJS.Binding.converter(function (description) {
        return description;
    }),
    authorConverter: WinJS.Binding.converter(function (author) {
        return "Av: " + author;
    }),
    thumbnailConverter: WinJS.Binding.converter(function (thumbnailUrl) {
        return thumbnailUrl ? thumbnailUrl : "#";
    }),
    undefinedHider: WinJS.Binding.converter(function (attr) {
        return attr ? "block" : "none";
    }),
});