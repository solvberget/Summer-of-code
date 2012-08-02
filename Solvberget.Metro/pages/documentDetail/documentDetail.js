(function () {
    "use strict";


    var ui = WinJS.UI;

    ui.Pages.define("/pages/documentDetail/documentDetail.html", {

        ready: function (element, options) {

            documentModel = options.documentModel;
            getDocument(documentModel.DocumentNumber);

            this.registerForShare();
            var self = this;

            document.getElementById("sendHoldRequestButton").addEventListener("click", registerHoldRequest);
        },

        goHome: function () {
            WinJS.Navigation.navigate("/pages/home/home.html");

        },

        registerForShare: function () {

            // Register/listen to share requests
            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
            dataTransferManager.addEventListener("datarequested", this.shareHtmlHandler);

            // Open share dialog on openShare-link clicked
            document.getElementById("openShare").addEventListener("click", function () {
                Windows.ApplicationModel.DataTransfer.DataTransferManager.showShareUI();
            });

        },
        shareHtmlHandler: function (e) {

            var request = e.request;

            // This documents title and img
            var documentTitle = documentModel.Title;

            if (documentTitle !== "") {

                var range = document.createRange();
                range.selectNode(document.getElementById("documentShareContent"));
                request.data = MSApp.createDataPackage(range);

                // Set the title and description of this share-event
                request.data.properties.title = documentTitle;
                request.data.properties.description = "Sjekk ut " + documentTitle + " fra bilioteket!";

                var first = true;
                $("img").each(function (index, item) {
                    var path = $(this).attr("src");
                    if (path !== undefined && path !== "undefined") {

                        if (path.indexOf("ms-appx://") == 1){
                            path = "";

                        }else{
                            var imageUri = new Windows.Foundation.Uri(path);
                            var streamReference = Windows.Storage.Streams.RandomAccessStreamReference.createFromUri(imageUri);
                            if (path.indexOf("http") != -1) {
                                request.data.setBitmap(streamReference);
                                first = false;
                            }
                            request.data.resourceMap[path] = streamReference;
                        }
                    }
                });

                var deferral = request.getDeferral();
                Windows.ApplicationModel.Package.current.installedLocation.getFileAsync("images\\solvberget30.png").then(function (thumbnailFile) {
                    request.data.properties.thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.createFromFile(thumbnailFile);

                    deferral.complete();
                }, function (err) {
                    request.failWithDisplayText(err);
                });


            } else {

                request.failWithDisplayText("Hmm, vi fant faktisk ingen tittel å dele");

            }

        },

    });

})();





var documentModel = undefined;

var ajaxGetDocument = function (documentNumber) {
    return $.getJSON(window.Data.serverBaseUrl + "/Document/GetDocument/" + documentNumber);
};

var ajaxGetDocumentImage = function () {
    return $.getJSON(window.Data.serverBaseUrl + "/Document/GetDocumentThumbnailImage/" + documentModel.DocumentNumber);
};

var populateFragment = function (documentModel) {


    var documentFragmentHolder = document.getElementById("documentFragmentHolder");
    documentFragmentHolder.innerHTML = "";

    var documentType = documentModel.DocType;
    var that = this;

    WinJS.UI.Fragments.renderCopy("/fragments/documentFragments/" + documentType + "/" + documentType + ".html", documentFragmentHolder).done(function () {

        var fragmentContent = document.getElementById("fragmentContent");
        // generate code for documentdetailpage
        //var htmlGenerated = CodeGenerator.documentToFactsHTML(documentModel);
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
    var availabilityTemplateHolderShared = document.getElementById("availabilityHolderShared");

    var availabilityTemplate = undefined;
    if (availabilityTemplateDiv)
        availabilityTemplate = new WinJS.Binding.Template(availabilityTemplateDiv);

    var model;

    if (documentModel.AvailabilityInfo) {

        for (var i = 0; i < documentModel.AvailabilityInfo.length; i++) {
            model = documentModel.AvailabilityInfo[i];
            model.LocationCode = documentModel.LocationCode;
            model.ClassificationNr = documentModel.ClassificationNr;

            if (availabilityTemplate && availabilityTemplateHolder && model)
                availabilityTemplate.render(model, availabilityTemplateHolder);
            availabilityTemplate.render(model, availabilityTemplateHolderShared);

        }
    }
    else {
        $("#sendHoldRequestButton").attr("disabled", "disabled");
        $("#docLocAndAvail").css("display", "none");
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

                var documentCompressedSubTitleDiv = document.getElementById("document-compressedsubtitle-container");
                var documentShareContent = document.getElementById("documentShareContent");


                // avoid processing null (if user navigates to fast away from page etc)
                if (documentTitleDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(documentTitleDiv, response);
                if (documentImageDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(documentImageDiv, response);
                if (documentSubTitleDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(documentSubTitleDiv, response);

                
                if (documentCompressedSubTitleDiv != undefined && response != undefined) {
                    if (response.MainResponsible != undefined) {

                        if (response.MainResponsible.Name != undefined) {
                            response.CompressedSubTitle = response.MainResponsible.Name + ", " + response.CompressedSubTitle;
                        }
                    }
                }
                WinJS.Binding.processAll(documentCompressedSubTitleDiv, response);
                
            }


            if (documentShareContent != undefined && response != undefined) {
                WinJS.Binding.processAll(documentShareContent, response);
            }

        }), this);

};

function registerHoldRequest() {

    var that = this;
    var holdRequestDiv = document.getElementById("holdRequestFragmentHolder");
    holdRequestDiv.innerHTML = "";
    WinJS.UI.Fragments.renderCopy("/fragments/holdRequest/holdRequest.html", holdRequestDiv).done(function () {

        var holdRequestAnchor = document.getElementById("sendHoldRequestButton");

        HoldRequest.showFlyout(holdRequestAnchor, documentModel);
    });
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
        if (factSrc == "" || factSrc == null || factSrc == undefined) {
            return "none";
        }
        return "block";
    }),
    docAvailableStyle: WinJS.Binding.converter(function (numAvailable) {
        return (!numAvailable || numAvailable < 1 || numAvailable == "0" || numAvailable == "null") ? "Red" : "Green";
    }),
    docAvailable: WinJS.Binding.converter(function (numAvailable) {
        return numAvailable + " av";
    }),
    docTotalCount: WinJS.Binding.converter(function (totalCount) {
        var pluralFix = totalCount < 2 ? "eksemplar" : "eksemplarer";
        return totalCount + " " + pluralFix + " tilgjengelig";
    }),
    docEarliestAvailableDate: WinJS.Binding.converter(function (date) {
        return (date || date != "") ? "Estimert tilgjengelig: " + date : "";
    }),
    personConverter: WinJS.Binding.converter(function (persons) {
        if (!persons) return "";
        var output = "";
        for (var x in persons) {
            output += persons[x].Name;
            if (persons[x].Role) output += " (" + persons[x].Role + ")";
            output += "\r\n";
        }

        return output;
    }),
    listConverter: WinJS.Binding.converter(function (list) {
        if (!list || list.length == 0) return "";
        var output = "";
        for (var x in list) {
            output += list[x] + "\r\n";
        }
        return output;
    }),
    locationCodeConverter: WinJS.Binding.converter(function (locCode) {
        if (!locCode) return "";
        return "Hyllesign: " + locCode;
    }),
    nullToEmptyStringConverter: WinJS.Binding.converter(function (prop) {
        if (!prop) return "";
        return prop;
    }),

});
