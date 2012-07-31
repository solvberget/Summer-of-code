(function () {

    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    var listRequestUrl = Data.serverBaseUrl + "/List/GetListsStaticAndDynamic";
    var docRequestUrl  = Data.serverBaseUrl + "/Document/GetDocumentLight/";

    //var promise = new WinJS.Promise(processRemainingDocuments, cancelRemainingDocuments);

    var lists = new Array();
    var listsBinding;

    var listSelectionIndex = 0;
    var continueToGetDocuments = false;

    ui.Pages.define("/pages/lists-v2/libraryLists.html", {

        ready: function (element, options) {
            continueToGetDocuments = true;

            //HeaderMenu
            var self = this;
            element.querySelector(".titlearea").addEventListener("click", this.showHeaderMenu, false);
            document.getElementById("headerMenuMyPage").addEventListener("click", function () { window.Data.itemByKey("mypage").navigateTo(); }, false);
            document.getElementById("headerMenuEvents").addEventListener("click", function () { window.Data.itemByKey("events").navigateTo(); }, false);
            document.getElementById("headerMenuSearch").addEventListener("click", function () { window.Data.itemByKey("search").navigateTo(); }, false);
            document.getElementById("headerMenuHomeMenuItem").addEventListener("click", function () { self.goHome(); }, false);
            var theMenu = document.getElementById("HeaderMenu");
            WinJS.UI.processAll(theMenu);

            //Set page header
            element.querySelector("header[role=banner] .pagetitle").textContent = "Lister";

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
            var listViewForLists = this.element.querySelector(".listView").winControl;
            if (listViewForLists) {

                console.log("ListView selection changed");

                var that = this;
                listViewForLists.selection.getItems().done(function updateDetails(items) {
                    if (items.length > 0) {
                        listSelectionIndex = items[0].index;
                        var listContent = that.element.querySelector(".listContentSection");
                        binding.processAll(listContent, items[0].data);
                        that.renderListContent(items[0].data);
                        listContent.scrollTop = 0;

                        var documentNumbers = items[0].data.DocumentNumbers;
                        var doneLoading = true;
                        for (var doc in documentNumbers) {
                            if (!documentNumbers[doc]) {
                                doneLoading = false;
                            }
                        }
                        if (doneLoading) {
                            $(".headerProgress").hide();
                        }
                    }
                });
            }
        },

        getLists: function (requestStr, listView) {
            var that = this;
            WinJS.xhr({ url: requestStr }).then(
                function (request) {
                    //
                    var obj = JSON.parse(request.responseText);
                    if (obj.Lists !== undefined) {
                        lists = obj.Lists;
                        listsBinding = new WinJS.Binding.List(lists);
                        listView.itemDataSource = listsBinding.dataSource;
                        listView.selection.set(listSelectionIndex);
                        // Hide progress-ring, show content
                        $("#listsLoading").css("display", "none").css("visibility", "none");
                        $("#listViewId").css("display", "block").css("visibility", "visible").hide().fadeIn(500);
                        that.processRemainingDocuments();
                    } else {
                        //Error handling   
                    }
                },
                function (request) {
                    //Error handling
                });
        },

        showHeaderMenu: function () {
            var title = document.querySelector("header .titlearea");
            var menu = document.getElementById("HeaderMenu").winControl;
            menu.anchor = title;
            menu.placement = "bottom";
            menu.alignment = "left";
            menu.show();
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

            var documentTemplateDiv = document.getElementById("documentTemplate");
            var documentTemplateHolder = document.getElementById("documentsHolder");
            documentTemplateHolder.innerHTML = "";

            var documentTemplate = undefined;
            if (documentTemplateDiv)
                documentTemplate = new WinJS.Binding.Template(documentTemplateDiv);
            var model;

            if (listModel.Documents) {
                for (var i = 0; i < listModel.Documents.length; i++) {
                    model = listModel.Documents[i];
                    if (documentTemplate && documentTemplateHolder && model) {
                        documentTemplate.render(model, documentTemplateHolder);
                    }
                }
            }
        },

        processRemainingDocuments: function () {
            var that = this;
            return new WinJS.Promise(function () {
                for (var i = 0; i < lists.length; i++) {
                    var listItem = lists[i];
                    var documentNumbers = listItem.DocumentNumbers;
                    for (var document in documentNumbers) {
                        if (!documentNumbers[document]) {
                            if (!listItem.Documents) {
                                listItem.Documents = new Array();
                            }
                            var reqStr = docRequestUrl + document;
                            var jsonContext = new Object();
                            jsonContext.listItem = listItem;
                            jsonContext.document = document;
                            $.getJSON(reqStr).then($.proxy(function (data) {
                                this.listItem.Documents.push(data);
                                this.listItem.DocumentNumbers[this.document] = true;
                                that.listViewSelectionChanged();
                                that.processRemainingThumbnails();
                            }, jsonContext));
                        }
                    }
                }
            });
        },

        processRemainingThumbnails: function () {
            var that = this;
            return new WinJS.Promise(function () {
                for (var i = 0; i < lists.length; i++) {
                    var listItem = lists[i];
                    var documents = listItem.Documents;
                    for (var j = 0; j < documents.length; j++) {
                        var doc = documents[j];
                        if (doc.ThumbnailUrl === undefined || doc.ThumbnailUrl == "") {
                            //var url = window.Data.serverBaseUrl + "/Document/GetDocumentThumbnailImage/";
                            //$.getJSON(url + doc.DocumentNumber + "/" + 60).then($.proxy(function (data) {
                            //    this.ThumbnailUrl = data;
                            //    that.listViewSelectionChanged();
                            //}, doc));
                        }
                    }
                }
            });
        }

    });




})();

