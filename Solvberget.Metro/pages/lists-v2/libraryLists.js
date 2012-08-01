(function () {

    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    var listRequestUrl = Data.serverBaseUrl + "/List/GetListsStaticAndDynamic";
    var docRequestUrl  = Data.serverBaseUrl + "/Document/GetDocumentLight/";

    var lists = new Array();
    var listsBinding;

    var listSelectionIndex = 0;
    var continueToGetDocuments = false;

    ui.Pages.define("/pages/lists-v2/libraryLists.html", {

        ready: function (element, options) {
            continueToGetDocuments = true;
            //Set page header
            element.querySelector("header[role=banner] .pagetitle").textContent = "Anbefalinger";

            //Init ListView
            var listView = element.querySelector(".listView").winControl;
            if (listView) {
                listView.layout = new ui.ListLayout();
                listView.onselectionchanged = this.listViewSelectionChanged.bind(this);
                listView.itemTemplate = document.getElementById("listViewTemplateId");
            }

            //Get the lists
            this.getLists(listRequestUrl, listView);

        },

        unload: function () {
            continueToGetDocuments = false;
            console.log("Unload triggered");
        },

        listViewSelectionChanged: function (args) {
            if (!continueToGetDocuments) return;
            var that = this;
            var listViewForLists = this.element.querySelector(".listView").winControl;
            if (listViewForLists) {
                listViewForLists.selection.getItems().done(function updateDetails(items) {
                    if (items.length > 0) {
                        listSelectionIndex = items[0].index;
                        var listContent = that.element.querySelector(".listContentSection");
                        binding.processAll(listContent, items[0].data);

                        that.renderListContent(items[0].data);
                        listContent.scrollTop = 0;

                        if (that.doneLoadingDocuments(items[0].data.DocumentNumbers)) {
                            $(".headerProgress").hide();
                        }

                    }
                });
            }
        },

        doneLoadingDocuments: function (documentNumbers) {
            if (documentNumbers !== undefined) {
                for (var docnumber in documentNumbers) {
                    if (!documentNumbers[docnumber]) {
                        return false;
                    }
                }
            }
            return true;
        },

        getLists: function (requestStr, listView) {
            var that = this;
            WinJS.xhr({ url: requestStr }).then(
                function (request) {
                    if (!continueToGetDocuments) return;
                    var obj = JSON.parse(request.responseText);
                    if (obj.Lists !== undefined) {
                        lists = obj.Lists;
                        listsBinding = new WinJS.Binding.List(lists);
                        listView.itemDataSource = listsBinding.dataSource;
                        listView.selection.set(listSelectionIndex);
                        // Hide progress-ring, show content
                        $("#listsLoading").hide();
                        $("#listViewId").fadeIn();
                        that.processRemainingDocuments();
                    } else {
                        //Error handling   
                    }
                },
                function (request) {
                    //Error handling
                });
        },

        goHome: function () {
            WinJS.Navigation.navigate("/pages/home/home.html");
        },

        isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        renderListContent: function (listModel) {
            var that = this;

            var documentTemplateHolder = document.getElementById("documentsHolder");
            documentTemplateHolder.innerHTML = "";
            var documentTemplateDiv = document.getElementById("documentTemplate");
            var documentTemplate = undefined;
            if (documentTemplateDiv)
                documentTemplate = new WinJS.Binding.Template(documentTemplateDiv);
            if (listModel.Documents) {
                for (var i = 0; i < listModel.Documents.length; i++) {
                    var doc = listModel.Documents[i];

                    if (documentTemplate && documentTemplateHolder && doc) {
                        that.populateDocElement(doc);
                        documentTemplateHolder.innerHTML += doc.element.innerHTML;
                    }
                }

            }
        },

        docIsVisible: function (docNumber) {
            var that = this;
            if (docNumber) {
                var items = lists[listSelectionIndex];
                for (var i = 0; i < items.Documents.length; i++) {
                    if (docNumber == items.Documents[i].DocumentNumber) {
                        return true;
                    }
                }
            }
            return false;
        },

        updateListViewSelectionIfDocIsVisible: function (docNumber) {
            if (this.docIsVisible(docNumber))
                this.listViewSelectionChanged();
        },

        populateDocElement: function (doc) {
            var that = this;
            if (doc) {
                if (doc.element === undefined) {
                    var item = new Object();
                    item.data = doc;
                    var documentTemplateDiv = document.getElementById("documentTemplate");
                    if (documentTemplateDiv) {
                        var documentTemplate = new WinJS.Binding.Template(documentTemplateDiv);
                        documentTemplate.renderItem(WinJS.Promise.wrap(item), true).renderComplete.then(function (renderedElement) {
                            doc.element = renderedElement;
                            if (doc.ThumbnailUrl !== undefined && doc.ThumbnailUrl != "") {
                                WinJS.Utilities.query("img", doc.element).forEach(function (img) {
                                    img.addEventListener("load", function () {
                                        WinJS.Utilities.addClass(img, "loaded");
                                        that.updateListViewSelectionIfDocIsVisible(doc.DocumentNumber);
                                    });
                                });
                            }
                        });
                    }
                }
            }
        },

        processRemainingDocuments: function () {
            var that = this;
            return new WinJS.Promise(function (complete, error, progress) {
                for (var i = 0; i < lists.length; i++) {
                    var listItem = lists[i];
                    var documentNumbers = listItem.DocumentNumbers;
                    for (var documentNumber in documentNumbers) {
                        if (!documentNumbers[documentNumber]) {
                            if (!listItem.Documents) {
                                listItem.Documents = new Array();
                            }
                            var reqStr = docRequestUrl + documentNumber;
                            var jsonContext = new Object();
                            jsonContext.listItem = listItem;
                            jsonContext.docNo = documentNumber;
                            $.getJSON(reqStr).then($.proxy(function (data) {
                                this.listItem.Documents.push(data);
                                this.listItem.DocumentNumbers[this.docNo] = true;
                                that.processThumbnailOnDoc(this.listItem);
                                that.updateListViewSelectionIfDocIsVisible(data.DocumentNumber);
                                complete();
                            }, jsonContext));
                        } else {
                            that.processThumbnailOnDoc(listItem);
                        }
                    }
                }
            });
        },
        
        processThumbnailOnDoc: function (doc) {
            var that = this;
            return new WinJS.Promise(function (complete, error, progress) {
                if (doc !== undefined) {
                    var documents = doc.Documents;
                    if (documents !== undefined) {
                        for (var j = 0; j < documents.length; j++) {
                            var checkDoc = documents[j];
                            if (checkDoc.ThumbnailUrl === undefined || checkDoc.ThumbnailUrl == "") {
                                if (checkDoc.TriedFetchingThumbnail === undefined) {
                                    checkDoc.TriedFetchingThumbnail = true;
                                    var url = window.Data.serverBaseUrl + "/Document/GetDocumentThumbnailImage/";
                                    $.getJSON(url + checkDoc.DocumentNumber).then($.proxy(function (data) {
                                        this.ThumbnailUrl = data;
                                        checkDoc.element = undefined;
                                        that.populateDocElement(checkDoc);
                                    }, checkDoc)).then(function () {
                                        complete();
                                    });
                                }
                            }
                            else {
                                checkDoc.TriedFetchingThumbnail = true;
                            }
                            that.populateDocElement(checkDoc);
                        }
                    }
                }
                
            });
        },

        clickHandler: function (ev) {

        },

    });
})();

