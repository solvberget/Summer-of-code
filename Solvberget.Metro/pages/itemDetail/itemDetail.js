(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    ui.Pages.define("/pages/itemDetail/itemDetail.html", {

        itemModel: undefined,
        documentId: undefined,
        viewModel: undefined,
        itemSelectionIndex: -1,

        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            var that = this;
            this.itemModel = options.itemModel;
            this.documentId = options.key;

            //Init viewmodel
            (function () {
                var article = element.querySelector(".article");
                var pageTitle = element.querySelector(".pagetitle");
             

                //Create new viewmodel
                that.setViewModel(that.itemModel);

                WinJS.Binding.processAll(article, that.viewModel);
                WinJS.Binding.processAll(pageTitle, that.viewModel);
            })();


            var ajaxGetDocument = function (query) {
                return $.getJSON("http://localhost:7089/Document/GetDocument/" + query);
            };


            $.when(ajaxGetDocument(this.documentId))
                .then($.proxy(function (response) {
                    this.viewModel.properties = ViewModel.propertiesList.documentToPropertiesList(response);
                }, this))
                .then($.proxy(function (response) {
                    //Init list
                    var listView = element.querySelector(".itemlist").winControl;

                    //Setup the DataSource
                    var documentDataSource = new WinJS.Binding.List(this.viewModel.properties);

                    // Set up the ListView.
                    listView.itemDataSource = documentDataSource.dataSource;
                    listView.itemTemplate = element.querySelector(".itemtemplate");

                    this.isSingleColumn ? listView.layout = new ui.GridLayout() :
                        listView.layout = new ui.GridLayout();

                    //Refresh list..
                    listView.selection.set(0);
                    listView.selection.clear();

                  }, this))

            $.when(Solvberget.DocumentImage.get(this.documentId))
               .then($.proxy(function (response) {
                   if (response != undefined && response != "") {
                       // Set the new value in the model of this item
                       this.viewModel.image = response;
                       var imageDiv = document.querySelector(".item-image-container");
                       WinJS.Binding.processAll(imageDiv, this.viewModel);
                   }
               }, this));



            this.registerForShare();

            element.querySelector(".itemdetailpage").focus();
        },

        setViewModel: function (itemModel) {
            eval("this.viewModel = ViewModel." + itemModel.DocType);
            this.viewModel.fillProperties(itemModel);
            this.viewModel.image = undefined;
            this.viewModel.properties = undefined;
        },

        //Clean up
        unload: function () {
            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
            dataTransferManager.removeEventListener("datarequested", this.shareHtmlHandler);
        },

        //Check if snapview or portrait
        isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        // This function updates the page layout in response to viewState changes.
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />
            /// <param name="viewState" value="Windows.UI.ViewManagement.ApplicationViewState" />
            /// <param name="lastViewState" value="Windows.UI.ViewManagement.ApplicationViewState" />

            var listView = element.querySelector(".itemlist").winControl;
            if (this.isSingleColumn()) {
                listView.layout = new ui.ListLayout();
                listView.forceLayout();
            } else {

                listView.layout = new ui.GridLayout();
                listView.forceLayout();
            }
        },
        registerForShare: function () {

            // Register/listen to share requests
            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
            dataTransferManager.addEventListener("datarequested", this.shareHtmlHandler);

            // Open share dialog on openShare-link clicked
            $("#openShare").click(function () {
                Windows.ApplicationModel.DataTransfer.DataTransferManager.showShareUI();
            });

        },

        shareHtmlHandler: function (e) {

            // Set in settings? Get from apps settings?

            var SHARE_MODE_FACEBOOK = "facebook", SHARE_MODE_HTML = "html";
            var shareMode = SHARE_MODE_HTML;

            // Share request object
            var request = e.request;

            // This documents title and img
            var documentTitle = $(".pagetitle").text();

            if ((typeof documentTitle === "string") && (documentTitle !== "")) {

                var SHARE_MODE_FACEBOOK = "facebook", SHARE_MODE_HTML = "html";
                var shareMode = SHARE_MODE_HTML;

                var range = document.createRange();
                range.selectNode(document.getElementById("share-content"));
                request.data = MSApp.createDataPackage(range);

                // Set the title and description of this share-event
                request.data.properties.title = documentTitle;
                request.data.properties.description =
                    "Del innholdet med dine venner!";


                var path = document.getElementById("item-image").getAttribute("src");
                if (path !== undefined && path !== "undefined") {
                    var imageUri = new Windows.Foundation.Uri(path);
                    var streamReference = Windows.Storage.Streams.RandomAccessStreamReference.createFromUri(imageUri);
                    request.data.resourceMap[path] = streamReference;
                }

                if (shareMode == SHARE_MODE_FACEBOOK) {

                    request.data.setUri(new Windows.Foundation.Uri("http://www.stavanger-kulturhus.no/soelvberget/soek_i_biblioteket?searchstring=" + documentTitle));

                }

            } else {

                request.failWithDisplayText("Fant ingen tittel å dele!");

            }

        },

    });

})();


