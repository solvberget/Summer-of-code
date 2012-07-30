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
    var uri = "http://localhost:7089/Document/GetDocumentReview/" + documentModel.DocumentNumber;
    return $.getJSON(uri);
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


WinJS.Namespace.define("DocumentDetailFragment", {

    ready: fragmentReady,

});

WinJS.Namespace.define("DocumentDetailConverters", {
    // Title converter
    // Subtitle converter
    // BackgroundImage converter (empty ==> dummy)
    // 
});