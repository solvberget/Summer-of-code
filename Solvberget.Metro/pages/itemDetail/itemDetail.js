(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    var ajaxGetDocumentImage = function (query) {
        var url = "http://localhost:7089/Document/GetDocumentImage/";
        return $.getJSON(url + query);
    }
    var ajaxGetThumbnailDocumentImage = function (query, size) {
        var url = "http://localhost:7089/Document/GetDocumentImage/";
        var thumbUrl = size == undefined ? url + query : url + query + "/" + size;
        return $.getJSON(thumbUrl);

    }

    ui.Pages.define("/pages/itemDetail/itemDetail.html", {

        item: undefined,
        documentId: undefined,
        viewModel: undefined,
        itemSelectionIndex: -1,
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.

        isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        selectionChanged: function (args) {
            var listView = document.body.querySelector(".itemlist").winControl;
            var details;
            var that = this;
            // By default, the selection is restriced to a single item.
            listView.selection.getItems().done(function updateDetails(items) {
                if (items.length > 0) {
                    that.itemSelectionIndex = items[0].index;
                    if (that.isSingleColumn()) {
                        // If snapped or portrait, navigate to a new page containing the
                        // selected item's details.
                        // NOT IMPLEMENTED
                        //nav.navigate("/pages/itemDetail/itemDetail.html", { selectedIndex: that.itemSelectionIndex, item: items[0].data });
                    } else {
                        // If fullscreen or filled, update the details column with new data.

                        details = document.querySelector(".itemlist");
                        binding.processAll(details, items[0].data);


                        // Fix for removing cached data, Windows error. 
                        setTimeout(function () {
                            window.focus();
                        }, 0);

                    }
                }
            });
        },
        ready: function (element, options) {
            var that = this;
            this.item = options.item;
            this.documentId = options.key;

            //Init viewmodel

            var initViewModel = function () {
               
                var contentDiv = element.querySelector(".article");
              
                that.setViewModel(that.item);
                WinJS.Binding.processAll(contentDiv, that.viewModel);
            }
            initViewModel();



            $.when(ajaxGetThumbnailDocumentImage(this.documentId, 1000))
               .then($.proxy(function (response) {
                   if (response != undefined && response != "") {
                       // Set the new value in the model of this item
                       this.viewModel.image = response;
                       var imageDiv = document.getElementById("#item-image");

                       WinJS.Binding.processAll(imageDiv, this.viewModel);

                   }
               }, this));

            //Init list
            var listView = element.querySelector(".itemlist").winControl;

            //Setup the EventDataSource
            var documentDataSource = new DataSources.documentDataSource(this.documentId);

            this.itemSelectionIndex = (options && "selectedIndex" in options) ? options.selectedIndex : -1;

            // Set up the ListView.
            listView.itemDataSource = documentDataSource;
            listView.itemTemplate = element.querySelector(".itemtemplate");
            listView.onselectionchanged = this.selectionChanged.bind(this);
            listView.layout = new ui.GridLayout();

           
            if (this.isSingleColumn()) {
                if (this.itemSelectionIndex >= 0) {
                    // For single-column detail view, load the article.
                    console.log("Binding break");
                    //binding.processAll(element.querySelector(".articlesection"), options.item);
                }
            } else {
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/itemDetail/itemDetail.html") {
                    // Clean up the backstack to handle a user snapping, navigating
                    // away, unsnapping, and then returning to this page.
                    nav.history.backStack.pop();
                }
                // If this page has a selectionIndex, make that selection
                // appear in the ListView.
                listView.selection.set(Math.max(this.itemSelectionIndex, 0));
            }
            // Store information about the group and selection that this page will
            // display.


            this.registerForShare();


            element.querySelector(".itemdetailpage").focus();
        },

        setViewModel: function (item) {
            eval("this.viewModel = ViewModel." + item.DocType.toString());
            if (this.viewModel !== undefined)
                this.viewModel.fillProperties(item);
        },
        unload: function () {
            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
            dataTransferManager.removeEventListener("datarequested", this.shareHtmlHandler);
        },

        // This function updates the page layout in response to viewState changes.
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />
            /// <param name="viewState" value="Windows.UI.ViewManagement.ApplicationViewState" />
            /// <param name="lastViewState" value="Windows.UI.ViewManagement.ApplicationViewState" />

            var listView = element.querySelector(".itemlist").winControl;
            var firstVisible = listView.indexOfFirstVisible;
          

            var handler = function (e) {
                listView.removeEventListener("contentanimating", handler, false);
                e.preventDefault();
            }

            if (this.isSingleColumn()) {
                listView.selection.clear();
                if (this.itemSelectionIndex >= 0) {
                    // If the app has snapped into a single-column detail view,
                    // add the single-column list view to the backstack.
                    nav.history.current.state = {
                        selectedIndex: this.itemSelectionIndex
                    };
                    nav.history.backStack.push({
                        location: "/pages/itemDetail/itemDetail.html",
                        state: { }
                    });
                   
                    element.querySelector(".itemdetailpage").focus();
                } else {
                    listView.addEventListener("contentanimating", handler, false);
                    listView.indexOfFirstVisible = firstVisible;
                    listView.forceLayout();
                }
            } else {
                // If the app has unsnapped into the two-column view, remove any
                // splitPage instances that got added to the backstack.
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/itemDetail/itemDetail.html") {
                    nav.history.backStack.pop();
                }
                if (viewState !== lastViewState) {
                    listView.addEventListener("contentanimating", handler, false);
                    listView.indexOfFirstVisible = firstVisible;
                    listView.forceLayout();
                }

                listView.selection.set(this.itemSelectionIndex >= 0 ? this.itemSelectionIndex : Math.max(firstVisible, 0));
                //Init viewmodel
               

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
            var documentTitle = $("#item-title").text();

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

                var imageUri = new Windows.Foundation.Uri(path);
                var streamReference = Windows.Storage.Streams.RandomAccessStreamReference.createFromUri(imageUri);
                request.data.resourceMap[path] = streamReference;


                if (shareMode == SHARE_MODE_FACEBOOK) {

                    request.data.setUri(new Windows.Foundation.Uri("http://www.stavanger-kulturhus.no/soelvberget/soek_i_biblioteket?searchstring=" + documentTitle));

                }

            } else {

                request.failWithDisplayText("Fant ingen tittel å dele!");

            }

        },

    });

})();


