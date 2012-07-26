(function () {

    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    var requestUrl = "http://localhost:7089/List/GetListsStaticAndDynamic";
    var docReqStrBase = "http://localhost:7089/Document/GetDocumentLight/";

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
            this.getLists(requestUrl, listView);

        },

        unload: function () {
            continueToGetDocuments = false;
            console.log("Unload triggered");
        },

        listViewSelectionChanged: function (args) {
            var listViewForLists = this.element.querySelector(".listView").winControl;
            if (listViewForLists) {

                console.log("ListView selection changed");

                var listContent;
                var that = this;
                listViewForLists.selection.getItems().done(function updateDetails(items) {
                    if (items.length > 0) {
                        listSelectionIndex = items[0].index;
                        listContent = that.element.querySelector(".listContentSection");
                        binding.processAll(listContent, items[0].data);
                        document.getElementById("documentsHolder").innerHTML = "";
                        renderListContent(items[0].data);
                        listContent.scrollTop = 0;
                    }
                });
            }
        },

        getLists: function (requestStr, listView) {

            var that = this;

            WinJS.xhr({ url: requestStr }).then(

                function (request) {
                    var obj = JSON.parse(request.responseText);
                    if (obj.Lists !== undefined) {

                        lists = obj.Lists;
                        listsBinding = new WinJS.Binding.List(lists);
                        listView.itemDataSource = listsBinding.dataSource;
                        listView.selection.set(listSelectionIndex);

                        // Hide progress-ring, show content
                        $("#listsLoading").css("display", "none").css("visibility", "none");
                        $("#listViewId").css("display", "block").css("visibility", "visible").hide().fadeIn(500);

                        processRemainingDocuments().then(function () {
                            //alert("hei");
                        });

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
        }

    });

    var renderListContent = function (listModel) {

        var documentTemplateDiv = document.getElementById("documentTemplate");
        var documentTemplateHolder = document.getElementById("documentsHolder");

        var documentTemplate = undefined;
        if (documentTemplateDiv)
            documentTemplate = new WinJS.Binding.Template(documentTemplateDiv);
        var model;

        if (listModel.Documents) {
            for (var i = 0; i < listModel.Documents.length; i++) {

                model = listModel.Documents[i];

                if (documentTemplate && documentTemplateHolder && model) {
                    var test = documentTemplate.render(model, documentTemplateHolder);
                }

            }
        }
    };

    var addListContent = function (documentModel) {

        var documentTemplateDiv = document.getElementById("documentTemplate");
        var documentTemplateHolder = document.getElementById("documentsHolder");

        var documentTemplate = undefined;
        if (documentTemplateDiv)
            documentTemplate = new WinJS.Binding.Template(documentTemplateDiv);
        
        if (documentTemplate && documentTemplateHolder && documentModel) {
            documentTemplate.renderItem(documentModel);
        }
    };

    var processRemainingDocuments = function () {
        return new WinJS.Promise(function () {
            
            for (var i = 0; i < lists.length; i++) {
                var listItem = lists[i];
                var documentNumbers = listItem.DocumentNumbers;
                for (var document in documentNumbers) {
                    if (!documentNumbers[document]) {
                        if (!listItem.Documents) {
                            listItem.Documents = new Array();
                        }
                        $.when(getDocumentLight(document)).then($.proxy(function (data) {
                            this.Documents.push(data);
                            //addListContent(data);
                            
                        }, listItem));
                    }
                }
            }
        });
    };


    var cancelRemainingDocuments = function () {

    };

    var downloadRemaningDocuments = function () {

    };

    var getDocumentLight = function (docnr) {

        var reqStr = docReqStrBase + docnr;

        return $.getJSON(reqStr);

        //return WinJS.xhr({ url: reqStr }).done(
        //    function (request) {
        //        var obj = JSON.parse(request.responseText);
        //        if (obj !== undefined) {
        //            return obj;
        //        } else {
        //            //Error handling
        //        }
        //    },
        //    function (request) {
        //        //Error handling
        //    });
    };

})();

