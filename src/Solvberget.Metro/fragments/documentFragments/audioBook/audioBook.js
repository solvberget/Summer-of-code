(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/audioBook/audioBook.html", {
        ready: function (element, options) {
            
        }
    });

})();

var documentModel;

var fragmentReady = function (model) {
    documentModel = model;
    ajaxGetReview();
};

var ajaxGetReview = function () {
    var url = window.Data.serverBaseUrl + "/Document/GetDocumentReview/" + documentModel.DocumentNumber;
    Solvberget.Queue.QueueDownload("documentdetails", { url: url }, ajaxGetReviewCallback, this, true);
};

var ajaxGetReviewCallback = function (request, context) {

    var response;

    if (request.responseText !== "") {
        response = JSON.parse(request.responseText);
    }

    if (response != undefined && response !== "") {

        var data = { documentReview: response };

        var reviewTemplate = new WinJS.Binding.Template(document.getElementById("reviewTemplate"));
        var reviewTemplateContainer = document.getElementById("reviewContainer");

        reviewTemplate.outerHTML = "";
        reviewTemplate.render(data, reviewTemplateContainer);

        $("#docLocAndAvail").css("margin-top", "20px");
        $("#reviewContainer").show("200");
        DocumentDetail.setHaveReview();
        DocumentDetail.cssForReview();

    }

};

WinJS.Namespace.define("DocumentDetailFragment", {
    readyAudioBook: fragmentReady,
});
