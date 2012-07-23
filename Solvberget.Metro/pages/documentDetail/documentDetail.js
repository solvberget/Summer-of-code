(function () {
    "use strict";


    var ui = WinJS.UI;

    ui.Pages.define("/pages/documentDetail/documentDetail.html", {

        ready: function (element, options) {

            documentModel = options.documentModel;
            getDocument(documentModel.DocumentNumber);

        },
    });

})();


var documentModel = undefined;

var ajaxGetDocument = function (documentNumber) {
    return $.getJSON(window.Data.serverBaseUrl + "/Document/GetDocument/" + documentNumber);
};

var ajaxGetDocumentImage = function () {
    return $.getJSON(window.Data.serverBaseUrl + "/Document/GetDocumentImage/" + documentModel.DocumentNumber);
};

var populateFragment = function (documentModel) {


    var documentFragmentHolder = document.getElementById("documentFragmentHolder");
    documentFragmentHolder.innerHTML = "";

    var documentType = documentModel.DocType;
    var that = this;

    WinJS.UI.Fragments.renderCopy("/fragments/documentFragments/" + documentType + "/" + documentType + ".html", documentFragmentHolder).done(function () {

        var fragmentContent = document.getElementById("fragmentContent");
        // generate code for documentdetailpage
        var htmlGenerated = CodeGenerator.documentToFactsHTML(documentModel);
        if (fragmentContent != undefined && documentModel != undefined)
            WinJS.Binding.processAll(fragmentContent, documentModel);

        DocumentDetailFragment.ready(that.documentModel);
        WinJS.Resources.processAll();

        populateAvailability();

        // Hide progress-ring, show content
        $("#documentDetailLoading").css("display", "none").css("visibility", "none");
        $("#documentDetailData").css("display", "block").css("visibility", "visible").hide().fadeIn(500);


    });

};

var populateAvailability = function () {

    var availabilityTemplateDiv = document.getElementById("availabilityTemplate");
    var availabilityTemplateHolder = document.getElementById("availabilityHolder");

    var availabilityTemplate = undefined;
    if (availabilityTemplateDiv)
        availabilityTemplate = new WinJS.Binding.Template(availabilityTemplateDiv);

    var model;

    for (var i = 0; i < documentModel.AvailabilityInfo.length; i++) {
        model = documentModel.AvailabilityInfo[i];

        if (availabilityTemplate && availabilityTemplateHolder && model)
            availabilityTemplate.render(model, availabilityTemplateHolder);

    }

};

var getDocumentImageUrl = function () {


    $.when(ajaxGetDocumentImage())
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {

                documentModel.ImageUrl = response;

                var documentImageDiv = document.getElementById("documentImage");

                if (documentImageDiv != undefined && documentModel != undefined)
                    WinJS.Binding.processAll(documentImageDiv, documentModel);
            }
        }, this)
    );

};

var getDocument = function (documentNumber) {

    // Show progress-ring, hide content
    $("#documentDetailData").css("display", "none").css("visibility", "none");
    $("#documentDetailLoading").css("display", "block").css("visibility", "visible");

    $.when(ajaxGetDocument(documentNumber))
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {


                documentModel = response;

                populateFragment(response);

                // Select HTML-section to process with the new binding lists
                var documentTitleDiv = document.getElementById("documentTitle");
                var documentImageDiv = document.getElementById("documentImage");
                var documentSubTitleDiv = document.getElementById("item-subtitle");

                // avoid processing null (if user navigates to fast away from page etc)
                if (documentTitleDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(documentTitleDiv, response);
                if (documentImageDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(documentImageDiv, response);
                if (documentSubTitleDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(documentSubTitleDiv, response);


            }

        }, this)
    );

};

WinJS.Namespace.define("DocumentDetail", {
    model: documentModel,
});

WinJS.Namespace.define("DocumentDetailConverters", {

    imageUrl: WinJS.Binding.converter(function (imageUrl) {

        if (imageUrl != "")
            return imageUrl;

        getDocumentImageUrl();
        return "/images/placeholders/" + documentModel.DocType + ".png";

    }),
    hideNullOrEmptyConverter: WinJS.Binding.converter(function (factSrc) {
        if (factSrc == "" || factSrc === "null" || factSrc == undefined) {
            return "none";
        }
        return "block";
    }),
    docAvailableStyle: WinJS.Binding.converter(function(numAvailable) {
        return (!numAvailable || numAvailable < 1 || numAvailable == "0" || numAvailable == "null") ? "Red" : "Green";
    }),
    docAvailable: WinJS.Binding.converter(function (numAvailable) {
        return numAvailable + " av";
    }),
    docTotalCount: WinJS.Binding.converter(function (totalCount) {
        var pluralFix = totalCount  < 2 ? "eksemplar" : "eksemplarer";
        return totalCount + " " + pluralFix + " tilgjengelig";
    }),
    docEarliestAvailableDate: WinJS.Binding.converter(function (date) {
        return (date || date != "") ? "Estimert tilgjengelig: " + date : "";
    }),

});
