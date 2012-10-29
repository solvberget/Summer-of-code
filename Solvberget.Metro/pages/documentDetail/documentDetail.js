(function () {
    "use strict";

    var ui = WinJS.UI;
    var avv = Windows.UI.ViewManagement.ApplicationView.value;
    var avs = Windows.UI.ViewManagement.ApplicationViewState;

    ui.Pages.define("/pages/documentDetail/documentDetail.html", {

        ready: function (element, options) {
            documentModel = options.documentModel;
            getDocument(documentModel.DocumentNumber);
            this.registerForShare();
            document.getElementById("sendHoldRequestButton").addEventListener("click", registerHoldRequest);
            document.getElementById("addToFavoritesButton").addEventListener("click", addToFavorites);
            this.setShareButtonState();
        },

        unload: function () {
            Solvberget.Queue.CancelQueue('documentdetails');
        },

        setShareButtonState: function() {
            if (avv === avs.snapped) {
                $("#openShareButton").attr("disabled", "disabled");
            } else {
                $("#openShareButton").removeAttr("disabled");
            }
        },

        updateLayout: function (element, viewState, lastViewState) {

            // Long and maybe a bit of duplication due to time... 

            if (viewState === avs.snapped) {
                $("#openShareButton").attr("disabled", "disabled");
            } else {
                $("#openShareButton").removeAttr("disabled");
            }

            if (lastViewState === avs.fullScreenLandscape || lastViewState === avs.filled) {
            
                if (viewState === avs.fullScreenPortrait) {

                    if (haveAvailability) {
                        cssForLeftContentPortrait();
                    }

                    $("#details").css("-ms-grid-column", "1");

                } else if (viewState === avs.snapped) {
                    
                    if (haveAvailability) {
                        $("#details").css("margin-top", "20px");
                    }

                    $("#details").css("-ms-grid-column", "1");

                }

            } else if (lastViewState === avs.fullScreenPortrait) {
                
                if (viewState === avs.fullScreenLandscape || viewState === avs.filled) {

                    if (haveAvailability) {
                        cssForLeftContentLandscapeOrFilled();
                    } else {
                        $("#details").css("-ms-grid-column", "1");
                    }
                
                    $("#details").css("margin-top", "0px");
                    $("#details").css("margin-bottom", "0px");

                } else if (viewState === avs.snapped) {

                    // We can't reach here?
                    
                }

            } else if (lastViewState === avs.snapped) {
                
                if (viewState === avs.fullScreenLandscape || viewState === avs.filled) {

                    if (haveAvailability) {
                        cssForLeftContentLandscapeOrFilled();
                        $("#details").css("margin-top", "0px");
                    }

                } else if (viewState === avs.fullScreenPortrait) {
                    
                    if (haveAvailability) {
                        cssForLeftContentPortrait();
                    }

                    $("#details").css("-ms-grid-column", "1");

                }

            }

            if (haveReview) {
                cssForReview(viewState);
            }

        },

        registerForShare: function () {

            // Register/listen to share requests
            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
            dataTransferManager.addEventListener("datarequested", this.shareHtmlHandler);

            // Open share dialog on openShare-link clicked
            document.getElementById("openShareButton").addEventListener("click", function () {
                Windows.ApplicationModel.DataTransfer.DataTransferManager.showShareUI();
            });

        },

        shareHtmlHandler: function (e) {

            var request = e.request;

            // This documents title and img
            var documentTitle = documentModel.Title;

            if (documentTitle !== "") {
                var replaceAll = function(txt, replace, with_this) {
                    return txt.replace(new RegExp(replace, 'g'), with_this);
                };
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

                        if (path.indexOf("/images/placeholders") >= 0) {
                            var path = "";

                        } else {
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
var haveAvailability = false;
var haveReview = false;

var ajaxGetDocumentImage = function () {

    var url = window.Data.serverBaseUrl + "/Document/GetDocumentThumbnailImage/" + documentModel.DocumentNumber;
    Solvberget.Queue.QueueDownload("documentdetails", { url: url }, ajaxGetDocumentImageCallback, this, true);

};

var ajaxGetDocumentImageCallback = function (request, context) {
    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
    if (response != undefined && response !== "") {
        documentModel.ImageUrl = response;
        var documentImageDiv = document.getElementById("documentImage");
        if (documentImageDiv != undefined && documentModel != undefined)
            WinJS.Binding.processAll(documentImageDiv, documentModel);
    }
};

var ajaxGetDocument = function (documentNumber) {
    var url = window.Data.serverBaseUrl + "/Document/GetDocument/" + documentNumber;
    Solvberget.Queue.QueueDownload("documentdetails", { url: url }, ajaxGetDocumentCallback, this, true);
};

var ajaxGetDocumentCallback = function (request, context) {

    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
    if (response != undefined && response !== "") {

        documentModel = response;
        populateFragment(response);

        // Select HTML-section to process with the new binding lists
        var documentTitleDiv = document.getElementById("document-title");
        var documentImageDiv = document.getElementById("documentImage");
        var documentSubTitleDiv = document.getElementById("document-subtitle");
        var documentCompressedSubTitleDiv = document.getElementById("document-compressedsubtitle");
        var documentShareContent = document.getElementById("documentShareContent");

        // avoid processing null (if user navigates to fast away from page etc)
        if (documentTitleDiv != undefined && response != undefined)
            WinJS.Binding.processAll(documentTitleDiv, response);
        if (documentImageDiv != undefined && response != undefined)
            WinJS.Binding.processAll(documentImageDiv, response);
        if (documentSubTitleDiv != undefined && response != undefined)
            WinJS.Binding.processAll(documentSubTitleDiv, response);

        // No need to do this...
        //if (documentCompressedSubTitleDiv != undefined && response != undefined) {
        //    if (response.MainResponsible != undefined) {
        //        if (response.MainResponsible.Name != undefined) {
        //            response.CompressedSubTitle = response.MainResponsible.Name + ", " + response.CompressedSubTitle;
        //        }
        //    }
        //}

        WinJS.Binding.processAll(documentCompressedSubTitleDiv, response);

    }

    if (documentShareContent != undefined && response != undefined) {
        WinJS.Binding.processAll(documentShareContent, response);
    }

};


var populateFragment = function (documentModel) {


    var documentFragmentHolder = document.getElementById("document-fragment-holder");
    documentFragmentHolder.innerHTML = "";

    var documentType = documentModel.DocType;
    var that = this;

    WinJS.UI.Fragments.renderCopy("/fragments/documentFragments/" + documentType + "/" + documentType + ".html", documentFragmentHolder).done(function () {

        var fragmentContent = document.getElementById("fragmentContent");

        if (fragmentContent != undefined && documentModel != undefined)
            WinJS.Binding.processAll(fragmentContent, documentModel);

        var type = that.documentModel.DocType;
        callFragmentReady(type, that.documentModel);

        WinJS.Resources.processAll();

        populateAvailability();

        $("#documentDetailLoading").css("display", "none").css("visibility", "none");
        $("#document-content").css("display", "block").css("visibility", "visible").hide().fadeIn(500);

        //Do not allow hold request for journals
        if (documentType == "Journal")
            $("#sendHoldRequestButton").css("display", "none").css("visibility", "none");

    });

};

var callFragmentReady = function(type, dm) {
    if (type === "Book")
        DocumentDetailFragment.readyBook(documentModel);
    else if (type === "Film")
        DocumentDetailFragment.readyFilm(dm);
    else if (type === "AudioBook")
        DocumentDetailFragment.readyAudioBook(dm);
    else if (type === "Cd")
        DocumentDetailFragment.readyCd(dm);
    else if (type === "SheetMusic")
        DocumentDetailFragment.readySheetMusic(dm);
    else if (type === "LanguageCourse")
        DocumentDetailFragment.readyLanguageCourse(dm);
    else if (type === "Journal")
        DocumentDetailFragment.readyJournal(dm);
    else if (type === "Game")
        DocumentDetailFragment.readyGame(dm);
    else {
        DocumentDetailFragment.readyDocument(dm);
    }
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

        haveAvailability = true;

        for (var i = 0; i < documentModel.AvailabilityInfo.length; i++) {

            model = documentModel.AvailabilityInfo[i];
            model.LocationCode = documentModel.LocationCode;
            model.ClassificationNr = documentModel.ClassificationNr;

            if (availabilityTemplate && availabilityTemplateHolder && model)
                availabilityTemplate.render(model, availabilityTemplateHolder);
            availabilityTemplate.render(model, availabilityTemplateHolderShared);

        }
        
        var avv = Windows.UI.ViewManagement.ApplicationView.value;
        var avs = Windows.UI.ViewManagement.ApplicationViewState;

        if (avv === avs.fullScreenLandscape || avv === avs.filled) {
            cssForLeftContentLandscapeOrFilled();
        } else if (avv === avs.fullScreenPortrait) {
            cssForLeftContentPortrait();
        } else {
            cssForAvailabilityInSnapped();
        }

    } else {
        $("#sendHoldRequestButton").attr("disabled", "disabled");
        $("#docLocAndAvail").css("display", "none");
    }

};

function cssForLeftContentLandscapeOrFilled () {
    $("#fragmentContent").css("-ms-grid-columns", "280px 1fr");
    $("#left-content").css("display", "inline");
    $("#details").css("-ms-grid-column", "2");
}

function cssForLeftContentPortrait () {
    $("#details").css("margin-top", "20px");
    $("#details").css("margin-bottom", "-25px");
}

function cssForAvailabilityInSnapped() {
    $("#left-content").css("display", "inline");
    $("#details").css("margin-top", "20px");
}

function cssForReview (avv) {

    if (!avv) {
        avv = Windows.UI.ViewManagement.ApplicationView.value;
    }
    var avs = Windows.UI.ViewManagement.ApplicationViewState;

    if (avv === avs.fullScreenLandscape || avv.value === avs.filled) {
        $("#fragmentContent").css("-ms-grid-columns", "360px 1fr");
    } else if (avv === avs.fullScreenPortrait) {
        $("#fragmentContent").css("-ms-grid-columns", "360px 1fr");
    } else {
        $("#fragmentContent").css("-ms-grid-columns", "1fr");
    }
    
}

function setHaveReview () {
    haveReview = true;
}

var getDocumentImageUrl = function () {
    ajaxGetDocumentImage();
};

var getDocument = function (documentNumber) {
    $("#document-content").hide();
    $("#documentDetailLoading").fadeIn();
    ajaxGetDocument(documentNumber);
};

function registerHoldRequest () {
    var that = this;
    var holdRequestDiv = document.getElementById("holdRequestFragmentHolder");
    holdRequestDiv.innerHTML = "";
    WinJS.UI.Fragments.renderCopy("/fragments/holdRequest/holdRequest.html", holdRequestDiv).done(function () {
        var holdRequestAnchor = document.getElementById("sendHoldRequestButton");
        HoldRequest.showFlyout(holdRequestAnchor, documentModel);
    });
};

function addToFavorites () {
    var applicationData = Windows.Storage.ApplicationData.current;
    if (applicationData) {
        var internalLibraryUserId = LoginFlyout.getLoggedInLibraryUserId();
        if (internalLibraryUserId && internalLibraryUserId !== "") {
            addToRoamingStorage(applicationData, internalLibraryUserId);
        }
        else {
            renderAddToFavoritesFlyout(false, "Du må være logget inn for å legge til favoritter!",
                                      "Velg \"Logg inn\" fra bunnmenyen eller gå til \"Min side\"");
        }
    }

}

function addToRoamingStorage (applicationData, internalLibraryUserId) {

    var roamingSettings = applicationData.roamingSettings;

    ////Debug - delete favorites:
    //var key = "favorites-" + internalLibraryUserId;
    //roamingSettings.values.remove(key);

    if (roamingSettings)
        storeFavorites(roamingSettings, internalLibraryUserId)
    else {
        renderAddToFavoritesFlyout(false, "Det oppstod en feil...",
                                  "(Ikke tilgang til roaming storage)");
    }

}

function storeFavorites (roamingSettings, internalLibraryUserId) {

    var key = "favorites-" + internalLibraryUserId;

    var existing = roamingSettings.values[key];
    var docs;

    if (!existing || jQuery.isEmptyObject(existing)) {
        docs = [];
    }
    else {
        var favorites = JSON.parse(existing);
        if (favorites) {
            for (var i = 0; i < favorites.length; i++) {
                if (favorites[i].DocumentNumber === documentModel.DocumentNumber) {
                    renderAddToFavoritesFlyout(false, "Dette dokumentet ligger allerede i dine favoritter!", "");
                    return;
                }
            }
        }

        docs = favorites;

    }

    if (documentModel.Author) {
        if (documentModel.Author.Name) {
            docs.push({ DocumentNumber: documentModel.DocumentNumber, Title: documentModel.Title, Author: documentModel.Author.Name, SubTitle: documentModel.CompressedSubTitle, ThumbnailUrl: documentModel.ThumbnailUrl });
        }
    }
    else {
        docs.push({ DocumentNumber: documentModel.DocumentNumber, Title: documentModel.Title, CompressedSubTitle: documentModel.CompressedSubTitle, ThumbnailUrl: documentModel.ThumbnailUrl });
    }

    roamingSettings.values[key] = JSON.stringify(docs);
    renderAddToFavoritesFlyout(true, "Dokumentet ble lagt til i dine favoritter!", "");

}

function renderAddToFavoritesFlyout (success, message1, message2) {
    var addToFavoritesDiv = document.getElementById("addToFavoritesFragmentHolder");
    addToFavoritesDiv.innerHTML = "";
    WinJS.UI.Fragments.renderCopy("/fragments/addToFavorites/addToFavorites.html", addToFavoritesDiv).done(function () {
        var holdRequestAnchor = document.getElementById("addToFavoritesButton");
        AddToFavorites.showFlyout(holdRequestAnchor, success, message1, message2);
    });
};

WinJS.Namespace.define("DocumentDetail", {
    model: documentModel,
    setHaveReview: setHaveReview,
    cssForReview: cssForReview,
});

WinJS.Namespace.define("DocumentDetailConverters", {

    imageUrl: WinJS.Binding.converter(function (imageUrl) {
        if (imageUrl != "")
            return imageUrl;

        getDocumentImageUrl();

        if (documentModel.DocType == "Film" && documentModel.TypeOfMedia == "Blu-ray") {
            return "images/placeholders/Blu-ray.png";
        }
        else if (documentModel.DocType == "Film" && documentModel.TypeOfMedia == "Blu-ray") {
            return "images/placeholders/3D.png";
        }

        else {
            return "/images/placeholders/" + documentModel.DocType + ".png";
        }
    }),
    hideNullOrEmptyConverter: WinJS.Binding.converter(function (factSrc) {
        if (factSrc == "" || factSrc == null || factSrc == undefined) {
            return "none";
        }
        return "block";
    }),
    docAvailableStyle: WinJS.Binding.converter(function (numAvailable) {
        return (!numAvailable || numAvailable < 1 || numAvailable == "0" || numAvailable == "null") ? "#cc3300" : "#448811";
    }),
    docAvailable: WinJS.Binding.converter(function (numAvailable) {
        return numAvailable + " av";
    }),
    docTotalCount: WinJS.Binding.converter(function (totalCount) {
        var pluralFix = totalCount < 2 ? "eksemplar" : "eksemplarer";
        return totalCount + " " + pluralFix + " tilgjengelig";
    }),
    docEarliestAvailableDate: WinJS.Binding.converter(function (date) {
        return (date || date != "") ? "Ventes å være ledig fra: " + date : "";
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
    classificationCodeConverter: WinJS.Binding.converter(function (locCode) {
        if (!locCode)
            return "Finnes på hylle: ";
        return "Finnes på hylle: " + locCode;
    }),
    nullToEmptyStringConverter: WinJS.Binding.converter(function (prop) {
        if (!prop) return "";
        return prop;
    }),

});
