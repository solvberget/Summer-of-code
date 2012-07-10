(function () {
    "use strict";

    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    var ajaxGetDocumentImage = function (query) {
        var url = "http://localhost:7089/Document/GetDocumentImage/";
        return $.getJSON(url + query);
    }


    ui.Pages.define("/pages/itemDetail/itemDetail.html", {

        item: undefined,
        documentId: undefined,
        viewModel: undefined,
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            WinJS.Resources.processAll();
            this.item = options.item;
            this.documentId = options.key;
            this.contentDiv = element.querySelector("#item-detailpage");
            this.factsFragmentsDiv = element.querySelector("#factsFragment");
            this.defaultScript();
            this.registerForShare();


            element.querySelector(".itemdetailpage").focus();
        },
        unload: function () {
            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
            dataTransferManager.removeEventListener("datarequested", this.shareHtmlHandler);
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
                range.selectNode(document.getElementById("content"));
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

        defaultScript: function () {

            this.factsFragmentsDiv.innerHTML = "";
            var self = this;

            var setViewModel = function (item) {

                if (ViewModel.DocumentList[self.documentId] != undefined) {
                    self.viewModel = ViewModel.DocumentList[self.documentId];
                } else {
                    if (item.DocType == "Book") {
                        self.viewModel = ViewModel.Book;
                        self.viewModel.viewPath = "/pages/itemDetail/fragments/bookFragment/bookFragment.html";
                        //Handle changes in book ui
                        self.viewModel.fragment = Book_Fragment;
                    }
                    if (item.DocType == "Film") {
                        self.viewModel = ViewModel.Movie;
                        self.viewModel.viewPath = "/pages/itemDetail/fragments/movieFragment/movieFragment.html";
                        self.viewModel.fragment = Movie_Fragment;
                    }
                    if (item.DocType == "AudioBook") {
                        self.viewModel = ViewModel.AudioBook;
                        self.viewModel.viewPath = "/pages/itemDetail/fragments/audioBookFragment/audioBookFragment.html";
                        self.viewModel.fragment = AudioBook_Fragment;
                    }

                    ViewModel.DocumentList[self.documentId] = self.viewModel;
                }
                if (self.viewModel !== undefined)
                    self.viewModel.fillProperties(item);
            };
            var ajaxGetDocument = function (query) {
                return $.getJSON("http://localhost:7089/Document/GetDocument/" + query);
            };
            var render = function () {

                // Read fragment from the HMTL file and load it into the div.  This
                // fragment also loads linked CSS and JavaScript specified in the fragment
                WinJS.UI.Fragments.renderCopy(self.viewModel.viewPath,
                    self.factsFragmentsDiv)
                    .done(function (fragment) {
                        // After the fragment is loaded into the target element,
                        // CSS and JavaScript referenced in the fragment are loaded.  The
                        // fragment loads script that defines an initialization function,
                        // so we can now call it to initialize the fragment's contents.
                        $("#item-dynamic-content").html(self.viewModel.output);

                        //Load existing data first
                        WinJS.Binding.processAll(self.factsFragmentsDiv, self.viewModel);
                        self.viewModel.fragment.fragmentLoad(fragment);
                        //Then get more data

                        WinJS.log && WinJS.log("successfully loaded fragment.", "sample", "status");
                    },
                        function (error) {
                            WinJS.log && WinJS.log("error loading fragment: " + error, "sample", "error");
                        });

            };

            //render
            setViewModel(self.item);
            render();
            WinJS.Binding.processAll(self.contentDiv, self.viewModel);

            $.when(ajaxGetDocument(self.item.DocumentNumber))
                .then($.proxy(function (response) {
                    setViewModel(response);
                    WinJS.Binding.processAll(self.contentDiv, self.viewModel);
                }, self)
             );

            $.when(ajaxGetDocumentImage(this.documentId))
               .then($.proxy(function (response) {

                   var fragmentsDiv = this.element.querySelector(".content");

                   if (response != undefined && response != "") {
                       // Set the new value in the model of this item
                       this.viewModel.image = response;
                       var imageDiv = document.getElementById("content-image");

                       WinJS.Binding.processAll(imageDiv, this.viewModel);

                   }
               }, this));



        }
    });

})();


