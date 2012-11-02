(function () {

    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;
    var nav = WinJS.Navigation;
    var app = WinJS.Application;

    var listRequestUrl = Data.serverBaseUrl + "/List/GetListsStaticAndDynamic";
    var docRequestUrl = Data.serverBaseUrl + "/Document/GetDocumentLight/";
    var thumbRequestUrl = Data.serverBaseUrl + "/Document/GetDocumentThumbnailImage/";

    var lists = new Array();
    var listsBinding;

    var listSelectionIndex = -1;
    var continueToGetDocuments = false;
    var getListsHasReturnedCallback = false;

    var isUpdatingListContent = false;
    var isPopulatingElement = false;

    //var delayRendringLimit = 8;

    ui.Pages.define("/pages/lists/libraryLists.html", {

        ready: function (element, options) {

            continueToGetDocuments = true;

            if (options && "selectedIndex" in options) {
                listSelectionIndex = options.selectedIndex;
            }
            else if (app.sessionState.listpageSelectionIndex && !this.isSingleColumn()) {
                if (app.sessionState.listpageSelectionIndex.index) {
                    listSelectionIndex = app.sessionState.listpageSelectionIndex.index;
                } else {
                    listSelectionIndex = -1;
                }
            }
            else {
                listSelectionIndex = -1;
            }

            //Set page header
            element.querySelector("header[role=banner] .pagetitle").textContent = "Anbefalinger";

            //Init ListView
            var listView = element.querySelector(".listView").winControl;
            if (listView) {
                listView.layout = new ui.ListLayout();
                listView.onselectionchanged = this.listViewSelectionChanged.bind(this);
                listView.itemTemplate = document.getElementById("listViewTemplateId");
            }

            //Hide either ListView (if we have selectionIndex != -1) or ListContent (selectionIndex == -1)
            this.updateVisibility();

            //Get the lists
            if (!getListsHasReturnedCallback)
                this.getLists(listRequestUrl, listView);
            else {
                listView.itemDataSource = listsBinding.dataSource;
                $("#listsLoading").hide();
                $("#listViewId").fadeIn();
                this.processRemainingDocuments();
            }

            if (this.isSingleColumn()) {
                if (listSelectionIndex >= 0) {
                    var listContent = this.element.querySelector(".listContentSection");
                    binding.processAll(listContent, lists[listSelectionIndex]);
                    this.renderList(lists[listSelectionIndex]);
                    listContent.scrollTop = 0;
                    Solvberget.Queue.PrioritizeUrls('libraryList', lists[listSelectionIndex].urls);
                    if (this.doneLoadingDocuments(lists[listSelectionIndex].DocumentNumbers)) {
                        $(".headerProgress").hide();
                    }
                }
            } else {
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/lists/libraryLists.html") {
                    // Clean up the backstack to handle a user snapping, navigating
                    // away, unsnapping, and then returning to this page.
                    nav.history.backStack.pop();
                }
                // If this page has a selectionIndex, make that appear in the ListView.
                listSelectionIndex = Math.max(listSelectionIndex, 0);
                if (getListsHasReturnedCallback) listView.selection.set(listSelectionIndex);
            }

            //AppBar
            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());

        },

        unload: function () {
            continueToGetDocuments = false;
            Solvberget.Queue.CancelQueue('libraryList');
        },

        listViewSelectionChanged: function (args) {
            if (!continueToGetDocuments) return;
            var that = this;
            var listViewForListsElement = this.element.querySelector(".listView");
            var listViewForLists = listViewForListsElement.winControl;
            if (listViewForLists) {
                listViewForLists.selection.getItems().done(function updateDetails(items) {
                    if (items.length > 0) {
                        listSelectionIndex = items[0].index;
                        if (that.isSingleColumn()) {
                            // If snapped or portrait, navigate to a new page containing the
                            // selected item's details.
                            nav.navigate("/pages/lists/libraryLists.html", { selectedIndex: listSelectionIndex });
                        }
                        else {
                            // If fullscreen or filled, update the details column with new data.
                            that.saveListSelectionIndex();
                            var listContent = that.element.querySelector(".listContentSection");
                            binding.processAll(listContent, items[0].data);
                            that.renderList(items[0].data);
                            listContent.scrollTop = 0;
                            Solvberget.Queue.PrioritizeUrls('libraryList', items[0].data.urls);
                            if (that.doneLoadingDocuments(items[0].data.DocumentNumbers)) {
                                $(".headerProgress").hide();
                            }
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
                    getListsHasReturnedCallback = true;
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

        renderList: function (listModel) {
            var that = this;
            if (listModel.Documents) {
                //if (Object.keys(listModel.DocumentNumbers).length > delayRendringLimit) {
                //    if (!this.doneLoadingDocuments(listModel.DocumentNumbers)) {
                //        return;
                //    }
                //}
                this.renderListContent(listModel, that);
            }
        },

        renderListContent: function (listModel, context) {
            var documentTemplateHolder = document.getElementById("documentsHolder");
            documentTemplateHolder.innerHTML = "";
            var documentTemplateDiv = document.getElementById("documentTemplate");
            var documentTemplate = undefined;
            if (documentTemplateDiv)
                documentTemplate = new WinJS.Binding.Template(documentTemplateDiv);

            for (var i = 0; i < listModel.Documents.length; i++) {
                var doc = listModel.Documents[i];
                if (documentTemplate && documentTemplateHolder && doc) {
                    context.populateDocElement(doc);
                    documentTemplateHolder.innerHTML += window.toStaticHTML(doc.element.innerHTML);

                    $('#' + doc.DocumentNumber).die('click').live('click', function () {
                        var model = { DocumentNumber: $(this).attr("id") };
                        context.saveListSelectionIndex();
                        nav.navigate("/pages/documentDetail/documentDetail.html", { documentModel: model });
                    });
                }
            }
        },

        resolveDocumentFromDocumentNumber: function (documentNumber) {
            if (documentNumber) {
                for (var i = 0; i < lists.length; i++) {
                    var listItem = lists[i];
                    for (var j = 0; j < listItem.Documents.length; j++) {
                        var document = listItem.Documents[j];
                        if (document.DocumentNumber == documentNumber) {
                            return document;
                        }
                    }
                }
            }
        },

        docIsVisible: function (docNumber) {
            if (docNumber) {
                if (this.isSingleColumn()) {
                    var contentIsPrimary = $(".listContentSection").hasClass("primarycolumn");
                    if (contentIsPrimary) {
                        var items = lists[listSelectionIndex];
                        for (var i = 0; i < items.Documents.length; i++) {
                            if (docNumber == items.Documents[i].DocumentNumber) {
                                return true;
                            }
                        }
                    }
                }
                else {
                    var items = lists[listSelectionIndex];
                    for (var i = 0; i < items.Documents.length; i++) {
                        if (docNumber == items.Documents[i].DocumentNumber) {
                            return true;
                        }
                    }
                }

            }

            return false;

        },

        updateContentSectionIfDocIsVisible: function (docNumber) {
            if (!continueToGetDocuments) return;
            var that = this;
            if (!that.isUpdatingListContent && this.docIsVisible(docNumber)) {
                that.isUpdatingListContent = true;
                var listViewForListsElement = this.element.querySelector(".listView");
                var listViewForLists = listViewForListsElement.winControl;
                if (listViewForLists) {
                    listViewForLists.selection.getItems().done(function updateDetails(items) {
                        if (items.length > 0) {
                            listSelectionIndex = items[0].index;
                            var listContent = that.element.querySelector(".listContentSection");
                            binding.processAll(listContent, items[0].data);
                            that.renderList(items[0].data);
                            listContent.scrollTop = 0;
                            if (that.doneLoadingDocuments(items[0].data.DocumentNumbers)) {
                                $(".headerProgress").hide();
                            }
                        }
                    });
                }
                that.isUpdatingListContent = false;
            }
        },

        populateDocElement: function (doc) {
            var that = this;
            if (doc) {
                if (!isPopulatingElement && doc.element === undefined) {
                    isPopulatingElement = true;
                    var item = new Object();
                    item.data = doc;
                    var documentTemplateDiv = document.getElementById("documentTemplate");
                    if (documentTemplateDiv) {
                        var documentTemplate = new WinJS.Binding.Template(documentTemplateDiv);
                        documentTemplate.renderItem(WinJS.Promise.wrap(item), true).renderComplete.then(function (renderedElement) {
                            doc.element = renderedElement;
                            doc.element.firstElementChild.id = doc.DocumentNumber;
                            if (doc.ThumbnailUrl !== undefined && doc.ThumbnailUrl != "") {
                                WinJS.Utilities.query("img", doc.element).forEach(function (img) {
                                    img.addEventListener("load", function () {
                                        WinJS.Utilities.addClass(img, "loaded");
                                        that.updateContentSectionIfDocIsVisible(doc.DocumentNumber);
                                    });
                                });
                            }
                        });
                    }
                    isPopulatingElement = false;
                }
            }
        },

        processRemainingDocuments: function () {
            var that = this;

            var completed = function (request, context) {
                var obj = JSON.parse(request.responseText);
                context.listItem.Documents.push(obj);
                context.listItem.DocumentNumbers[context.docNo] = true;
                that.processThumbnailOnDoc(context.listItem);
                that.updateContentSectionIfDocIsVisible(obj.DocumentNumber);
            }

            for (var i = 0; i < lists.length; i++) {
                var listItem = lists[i];
                if (!listItem.urls) listItem.urls = [];
                var documentNumbers = listItem.DocumentNumbers;

                for (var documentNumber in documentNumbers) {
                    if (!documentNumbers[documentNumber]) {
                        if (!listItem.Documents) {
                            listItem.Documents = new Array();
                        }
                        var reqStr = docRequestUrl + documentNumber;
                        var jsonContext = { listItem: listItem, docNo: documentNumber };
                        listItem.urls.push(reqStr);
                        Solvberget.Queue.QueueDownload('libraryList', { url: reqStr }, completed, jsonContext);
                    } else {
                        that.processThumbnailOnDoc(listItem);
                    }
                }
            }
        },

        processThumbnailOnDoc: function (doc) {
            var that = this;
            var completed = function (request, context) {
                var obj = JSON.parse(request.responseText);
                if (obj !== "") context.ThumbnailUrl = obj;
                context.element = undefined;
                that.populateDocElement(context);
            }

            if (doc !== undefined) {
                if (!doc.urls) doc.urls = [];
                var documents = doc.Documents;
                if (documents !== undefined) {
                    for (var j = 0; j < documents.length; j++) {
                        var checkDoc = documents[j];
                        if (checkDoc.ThumbnailUrl === undefined || checkDoc.ThumbnailUrl == "") {
                            if (checkDoc.TriedFetchingThumbnail === undefined) {
                                checkDoc.ThumbnailUrl = "/images/placeholders/" + checkDoc.DocType + ".png";
                                checkDoc.TriedFetchingThumbnail = true;
                                var url = thumbRequestUrl + checkDoc.DocumentNumber;
                                doc.urls.push(url);
                                Solvberget.Queue.QueueDownload('libraryList', { url: url }, completed, checkDoc, true);

                            }
                        }
                        else {
                            checkDoc.TriedFetchingThumbnail = true;
                        }
                        that.populateDocElement(checkDoc);
                    }
                }
            }
        },

        saveListSelectionIndex: function () {
            var indexObj = { index: listSelectionIndex };
            app.sessionState.listpageSelectionIndex = indexObj;
        },

        updateLayout: function (element, viewState, lastViewState) {

            var listView = element.querySelector(".listView").winControl;

            if (listView) {

                var firstVisible = listView.indexOfFirstVisible;
                this.updateVisibility();

                var handler = function (e) {
                    listView.removeEventListener("contentanimating", handler, false);
                    e.preventDefault();
                }

                if (this.isSingleColumn()) {

                    listView.selection.clear();

                    if (listSelectionIndex >= 0) {
                        // If the app has snapped into a single-column detail view,
                        // add the single-column list view to the backstack.
                        nav.history.current.state = {
                            selectedIndex: listSelectionIndex
                        };
                        nav.history.backStack.push({
                            location: "/pages/lists/libraryLists.html",
                            state: {}
                        });
                        element.querySelector(".listContentSection").focus();
                    } else {
                        listView.addEventListener("contentanimating", handler, false);
                        if (firstVisible >= 0 && listView.itemDataSource.list.length > 0) {
                            listView.indexOfFirstVisible = firstVisible;
                        }
                        listView.forceLayout();
                    }

                } else {
                    // If the app has unsnapped into the two-column view, remove any
                    // splitPage instances that got added to the backstack.
                    if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/lists/libraryLists.html") {
                        nav.history.backStack.pop();
                    }
                    if (viewState !== lastViewState) {
                        listView.addEventListener("contentanimating", handler, false);
                        if (firstVisible >= 0 && listView.itemDataSource.list.length > 0) {
                            listView.indexOfFirstVisible = firstVisible;
                        }
                        listView.forceLayout();
                    }

                    listView.selection.set(listSelectionIndex >= 0 ? listSelectionIndex : Math.max(firstVisible, 0));

                }

            }
        },

        isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        updateVisibility: function () {
            var oldPrimary = document.querySelector(".primarycolumn");
            if (oldPrimary) {
                utils.removeClass(oldPrimary, "primarycolumn");
            }
            if (this.isSingleColumn()) {
                if (listSelectionIndex >= 0) {
                    utils.addClass(document.querySelector(".listContentSection"), "primarycolumn");
                    document.querySelector(".listContentSection").focus();
                } else {
                    utils.addClass(document.querySelector(".listViewSection"), "primarycolumn");
                    document.querySelector(".listView").focus();
                }
            } else {
                document.querySelector(".listView").focus();
            }
        }

    });
})();

