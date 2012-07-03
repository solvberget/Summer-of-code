(function () {
    "use strict";

    var ui = WinJS.UI;
    var utils = WinJS.Utilities;
    ui.Pages.define("/pages/itemDetail/itemDetail.html", {
        item: undefined,
        documentId: undefined,

        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {

            this.fragmentScriptCSSDiv = element.querySelector("#fragments");
            this.item = options.item;
            this.documentId = options.key;
            this.defaultScript();
            this.registerForShare();
            element.querySelector(".content").focus();

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



                    request.data.setUri(new Windows.Foundation.Uri("http://www.stavanger-kulturhus.no/soelvberget/soek_i_biblioteket?searchstring=" + documentTitle));


                    var range = document.createRange();
                    range.selectNode(document.getElementById("fragments"));
                    request.data = MSApp.createDataPackage(range);

                    request.data.setText("Hello!!");

                    // Set the title and description of this share-event
                    request.data.properties.title = documentTitle;
                    request.data.properties.description =
                        "Del innholdet med dine venner!";



                    var path = document.getElementById("item-image").getAttribute("src");

                    var imageUri = new Windows.Foundation.Uri(path);
                    var streamReference = Windows.Storage.Streams.RandomAccessStreamReference.createFromUri(imageUri);
                    request.data.resourceMap[path] = streamReference;
                   

                    if (shareMode == SHARE_MODE_FACEBOOK) {


                    }
                
            } else {

                request.failWithDisplayText("Fant ingen tittel å dele!");

            }

        },

        defaultScript: function () {

            this.fragmentScriptCSSDiv.innerHTML = "";

            var self = this;

            // Read fragment from the HMTL file and load it into the div.  This
            // fragment also loads linked CSS and JavaScript specified in the fragment

            WinJS.UI.Fragments.renderCopy("/pages/itemDetail/fragments/bookFragment/bookFragment.html",
                this.fragmentScriptCSSDiv)
                .done(function (fragment) {

                    if (self.item.DocType == "Book") {
                        var book;
                        if (ViewModel.BookList[self.documentId] != undefined) {
                            book = ViewModel.BookList[self.documentId];
                        } else {
                            book = ViewModel.Book;
                            book.fillProperties(self.item);
                            ViewModel.BookList[self.documentId] = book;
                        }
                        var itemDetail = document.getElementById("item-detail");
                        WinJS.Binding.processAll(itemDetail, book);
                        // After the fragment is loaded into the target element,
                        // CSS and JavaScript referenced in the fragment are loaded.  The
                        // fragment loads script that defines an initialization function,
                        // so we can now call it to initialize the fragment's contents.
                        Fragment.fragmentLoad(fragment);
                        WinJS.log && WinJS.log("successfully loaded fragment, change date to fire change event.", "sample", "status");
                    }

                    if (self.item.DocType == "Film") {
                        var movie;
                        if (ViewModel.MovieList[self.documentId] != undefined) {
                            movie = ViewModel.MovieList[self.documentId];
                        }
                        else {
                            movie = ViewModel.Movie;
                            movie.fillProperties(self.item);
                            ViewModel.MovieList[self.documentId] = movie;
                        }
                        ///var itemDetail = document.getElementById("item-detail");
                        ///WinJS.Binding.processAll(itemDetail, movie);
                        // After the fragment is loaded into the target element,
                        // CSS and JavaScript referenced in the fragment are loaded.  The
                        // fragment loads script that defines an initialization function,
                        // so we can now call it to initialize the fragment's contents.
                        Fragment.fragmentLoad(fragment);
                        WinJS.log && WinJS.log("successfully loaded fragment, change date to fire change event.", "sample", "status");
                    }

                },

            function (error) {
                WinJS.log && WinJS.log("error loading fragment: " + error, "sample", "error");
            });
        }
    });
})();
