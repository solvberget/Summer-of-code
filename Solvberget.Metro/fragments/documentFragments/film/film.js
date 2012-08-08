(function () {

    // Track if the log in was successful
    var loggedIn;

    "use strict";
    var page = WinJS.UI.Pages.define("/fragments/documentFragments/film/film.html", {
        ready: function (element, options) {

        }
    });

})();

var documentModel;

var fragmentReady = function (model) {

    documentModel = model;
    getImdbRating();

};


// !------------ AJAX METHODS -------------! //

var ajaxGetImdbRating = function () {
    var url = window.Data.serverBaseUrl + "/Document/GetDocumentRating/" + documentModel.DocumentNumber;
    Solvberget.Queue.QueueDownload("documentdetails", { url: url }, ajaxGetImdbRatingCallback, this, true);
};


// !------------ AJAX CALLBACKS -------------! //


var ajaxGetImdbRatingCallback = function (request, context) {
    var response = JSON.parse(request.responseText);

    if (response != undefined && response !== "") {

        var data = { ImdbRating: response };

        var imdbTemplate = new WinJS.Binding.Template(document.getElementById("imdbTemplate"));
        var imdbTemplateContainer = document.getElementById("ratingContainer");
        //Render for sharing with facebook etc.
        var imdbTemplateContainerShared = document.getElementById("ratingContainerShared");


        imdbTemplate.outerHTML = "";
        imdbTemplate.render(data, imdbTemplateContainer);
        imdbTemplate.render(data, imdbTemplateContainerShared);

    }

};


// !------------ END AJAX END -------------! //


var getImdbRating = function () {
    ajaxGetImdbRating();
};



WinJS.Namespace.define("DocumentDetailFragment", {

    ready: fragmentReady,

});

WinJS.Namespace.define("DocumentDetailConverters", {
    imdbStyleConverter: WinJS.Binding.converter(function (imdbSrc) {
        return "display:block";
    }),
    nullStyleConverter: WinJS.Binding.converter(function (attr) {
        if (attr == undefined || attr == "" || attr === "null")
            return "display: none";
        else
            return "display:block";
    }),
    imdbRatingConverter: WinJS.Binding.converter(function (rating) {
        return rating + "/10";
    }),
});