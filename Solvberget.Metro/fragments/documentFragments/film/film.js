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

var ajaxGetImdbRating = function () {
    return $.getJSON(window.Data.serverBaseUrl + "/Document/GetDocumentRating/" + documentModel.DocumentNumber);
};

var getImdbRating = function () {
    

    $.when(ajaxGetImdbRating())
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {

                var data = { ImdbRating: response };

                var imdbTemplate = new WinJS.Binding.Template(document.getElementById("imdbTemplate"));
                var imdbTemplateContainer = document.getElementById("ratingContainer");

                    imdbTemplate.outerHTML = "";
                    imdbTemplate.render(data, imdbTemplateContainer);
               
              
                
            }
        }, this)
    );

};



WinJS.Namespace.define("DocumentDetailFragment", {

    ready: fragmentReady,

});

WinJS.Namespace.define("DocumentDetailConverters", {
    imdbStyleConverter: WinJS.Binding.converter(function (imdbSrc) {
        return "display:block";
    }),
    nullStyleConverter : WinJS.Binding.converter(function (attr) {
        if (attr == undefined || attr == "" || attr === "null")
            return "display: none";
        else
            return "display:block";
    }),
    imdbRatingConverter : WinJS.Binding.converter(function (rating) {
        return rating + "/10";
    }),
});