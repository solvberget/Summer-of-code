(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/book/book.html", {
        ready: function (element, options) {

           
            

        }
    });

})();

var documentModel;

var fragmentReady = function(model) {
    documentModel = model;
    getReview();
    getBokelskereRating();
 
};

var ajaxGetReview = function () {
    return $.getJSON("http://localhost:7089/Document/GetDocumentReview/" + documentModel.DocumentNumber);
};

var ajaxGetRating = function () {
    return $.getJSON("http://localhost:7089/Document/GetDocumentRating/" + documentModel.DocumentNumber);
   
};


var getReview = function () {
    $.when(ajaxGetReview())
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {

                var data = { documentReview: response };

                var reviewTemplate = new WinJS.Binding.Template(document.getElementById("reviewTemplate"));
                var reviewTemplateContainer = document.getElementById("reviewContainer");
                reviewTemplate.outerHTML = ""
                reviewTemplate.render(data, reviewTemplateContainer);

            }
        }, this)
    );

};

var getBokelskereRating = function () {
    $.when(ajaxGetRating())
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {

                var data = { BokElskereRating: response };

                var imdbTemplate = new WinJS.Binding.Template(document.getElementById("bokelskeretemplate"));
                var imdbTemplateContainer = document.getElementById("ratingContainer");
                //Render for sharing with facebook etc.
                var imdbTemplateContainerShared = document.getElementById("ratingContainerShared");


                imdbTemplate.outerHTML = "";
                imdbTemplate.render(data, imdbTemplateContainer);
                imdbTemplate.render(data, imdbTemplateContainerShared);




            }
        }, this)
    );

};


WinJS.Namespace.define("DocumentDetailFragment", {

    ready: fragmentReady,

});

WinJS.Namespace.define("DocumentDetailConverters", {
    bokelskereStyleConverter: WinJS.Binding.converter(function (imdbSrc) {
        return "display:block";
    }),
    nullStyleConverter: WinJS.Binding.converter(function (attr) {
        if (attr == undefined || attr == "" || attr === "null")
            return "display: none";
        else
            return "display:block";
    }),
    bokelskereRatingConverter: WinJS.Binding.converter(function (rating) {
        return rating + "/6";
    }),
});