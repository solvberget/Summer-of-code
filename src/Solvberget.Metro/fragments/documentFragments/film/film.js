(function () {

    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/film/film.html", {
        ready: function (element, options) {

        }
    });

})();

var documentModel;

var fragmentReady = function (model) {
    documentModel = model;
    ajaxGetImdbRating();
};


// !------------ AJAX METHODS -------------! //
var ajaxGetImdbRating = function () {
    var url = window.Data.serverBaseUrl + "/Document/GetDocumentRating/" + documentModel.DocumentNumber;
    Solvberget.Queue.QueueDownload("documentdetails", { url: url }, ajaxGetImdbRatingCallback, this, true);
};


// !------------ AJAX CALLBACKS -------------! //
var ajaxGetImdbRatingCallback = function (request, context) {

    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

    if (response != undefined && response !== "") {

        var data = { ImdbRating: response };

        var imdbTemplate = new WinJS.Binding.Template(document.getElementById("imdbTemplate"));
        var imdbTemplateContainer = document.getElementById("ratingContainer");

        imdbTemplate.outerHTML = "";
        imdbTemplate.render(data, imdbTemplateContainer);

        var rating = document.getElementById("ratingControl").winControl;
        if (rating) {
            rating.maxRating = 10.0;
            rating.averageRating = response;
        }

    }

};
// !------------ END AJAX END -------------! //

WinJS.Namespace.define("DocumentDetailFragment", {
    readyFilm: fragmentReady
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