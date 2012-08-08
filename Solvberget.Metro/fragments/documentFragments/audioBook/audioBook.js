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
    getReview();
};


var ajaxGetReview = function () {
    var url = "http://localhost:7089/Document/GetDocumentReview/" + documentModel.DocumentNumber;
    Solvberget.Queue.QueueDownload("documentdetails", { url: url }, ajaxGetReviewCallback, this, true);
};

var ajaxGetReviewCallback = function (request, context) {
    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

    if (response != undefined && response !== "") {
        var data = { documentReview: response };
        var reviewTemplate = new WinJS.Binding.Template(document.getElementById("reviewTemplate"));
        var reviewTemplateContainer = document.getElementById("reviewContainer");
        reviewTemplate.outerHTML = "";
        reviewTemplate.render(data, reviewTemplateContainer);
    }
};

var getReview = function () {
    ajaxGetReview();
};


WinJS.Namespace.define("DocumentDetailFragment", {

    ready: fragmentReady,

});
