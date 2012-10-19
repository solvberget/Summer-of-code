(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/book/book.html", {
        ready: function (element, options) {
        }
    });

})();

var documentModel;

var fragmentReady = function (model) {
    documentModel = model;
    getReview();
    getBokelskereRating();
    //$("#details").css("margin-top", "0px");
};

// !------------ AJAX METHODS -------------! //

var ajaxGetReview = function () {
    var url = window.Data.serverBaseUrl + "/Document/GetDocumentReview/" + documentModel.DocumentNumber;
    Solvberget.Queue.QueueDownload("documentdetails", { url: url }, ajaxGetReviewCallback, this, true);
};

var ajaxGetRating = function () {
    var url = window.Data.serverBaseUrl + "/Document/GetDocumentRating/" + documentModel.DocumentNumber;
    Solvberget.Queue.QueueDownload("documentdetails", { url: url }, ajaxGetRatingCallback, this, true);
};

// !------------ AJAX CALLBACKS -------------! //


var ajaxGetReviewCallback = function (request, context) {

    var response;

    if (request.responseText !== "") {
        response = JSON.parse(request.responseText);
    }

    if (response != undefined && response !== "") {

        var data = { documentReview: response };

        var reviewTemplate = new WinJS.Binding.Template(document.getElementById("reviewTemplate"));
        var reviewTemplateContainer = document.getElementById("reviewHolder");
        
        reviewTemplate.outerHTML = "";
        reviewTemplate.render(data, reviewTemplateContainer);

        $("#docLocAndAvail").css("margin-top", "20px");
        DocumentDetail.cssForReview();

    } else {
        $("#reviewContainer").css("display", "none");
    }

};


var ajaxGetRatingCallback = function (request, context) {
    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

    if (response != undefined && response !== "") {

        if (response === "0.0") return;

        var data = { BokElskereRating: response };

        var bokelskereTemplate = new WinJS.Binding.Template(document.getElementById("bokelskeretemplate"));
        var ratingContainer = document.getElementById("ratingContainer");

        ////Render for sharing with facebook etc.
        //var imdbTemplateContainerShared = document.getElementById("ratingContainerShared");

        bokelskereTemplate.outerHTML = "";
        bokelskereTemplate.render(data, ratingContainer);

        var rating = document.getElementById("ratingControl").winControl;
        if (rating) {
            rating.maxRating = 6.0;
            rating.averageRating = response;
        }

        //imdbTemplate.render(data, imdbTemplateContainerShared);
        
        //What?
        //$("#details").css("margin-top", "65px");

    }
};

// !------------ END AJAX END -------------! //


var getReview = function () {
    //$("#reviewContainer").css("margin-top","0px");
    ajaxGetReview();
};

var getBokelskereRating = function () {
    ajaxGetRating();
};

// !------------ Namespace bindings -------------! //


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