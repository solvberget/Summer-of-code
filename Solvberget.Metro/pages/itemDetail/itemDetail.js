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

            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
            dataTransferManager.addEventListener("datarequested", this.shareHtmlHandler);
            $("#openShare").click(function () {
                Windows.ApplicationModel.DataTransfer.DataTransferManager.showShareUI();
            });
        },

        shareHtmlHandler: function (e) {

            var request = e.request;
            var documentTitle = $(".item-title").text();

            request.data.properties.title = documentTitle;
            request.data.properties.description = 
                "Del innholdet med dine venner!";

            // Data to share
            var imagePath = $("article .item-image").attr("src");

            var shareText = "Denne her tror jeg er noe for deg!";
            
            var localImage = "ms-appx://"+imagePath;
            var htmlContent = '<p>'+
                                '<img src="' + localImage + '"alt="'+documentTitle+'" title="'+documentTitle+'"></img>'+
                              '</p>'+
                              '<p>' + shareText + '</p>';

            var htmlFormat = 
                Windows.ApplicationModel.DataTransfer.HtmlFormatHelper.createHtmlFormat(htmlContent);

            request.data.setHtmlFormat(htmlFormat);
            request.data.setText("Denne må du sjekke ut!");

            var streamRef =
                Windows.Storage.Streams.RandomAccessStreamReference.createFromUri(new Windows.Foundation.Uri(localImage));

            request.data.resourceMap[localImage] = streamRef;

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
        }
            );
        }
    });
})();
