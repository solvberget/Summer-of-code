(function () {
    "use strict";

    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    ui.Pages.define("/pages/itemDetail/bookDetail/bookDetail.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            var item = options.item;
            var documentId = options.key;

            element.querySelector(".pagetitle").textContent = item.Title;
            element.querySelector("article .item-author").textContent = item.Author.Name;
            element.querySelector("article .item-published").textContent = item.Publisher + ", " + item.PublishedYear;
            element.querySelector("article .item-pages").textContent = item.NumberOfPages;
            element.querySelector("article .item-isbn").textContent = item.Isbn;

            var genres = "";
            for ( var genre in item.Genre ) {

                genres += item.Genre[genre] + ", ";

            }
            genres.substr(0, genres.length - 2);
            
            element.querySelector("article .item-genre").textContent = genres;


            element.querySelector("article .item-image").src = "/images/dummydata/hp1.jpeg";
            //element.querySelector("article .item-image").alt = item.subtitle;
            //element.querySelector("article .item-content").innerHTML = item.content;

            $("#openShare").click(function () {
                Windows.ApplicationModel.DataTransfer.DataTransferManager.showShareUI();
            });


            this.registerForShare();
            element.querySelector(".content").focus();
        },
        registerForShare: function () {

            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
            dataTransferManager.addEventListener("datarequested", this.shareHtmlHandler);
            
        },

        shareHtmlHandler: function (e) {

            var request = e.request;
            request.data.properties.title = "Deling";
            request.data.properties.description = 
                "Del innholdet med dine venner!";

            // Data to share
            var imagePath = $("article .item-image").attr("src");
            var documentTitle = $(".pagetitle").text();

            var shareText = "Denne her tror jeg er noe for deg!";
            
            var localImage = "ms-appx://"+imagePath;
            var htmlContent = '<h2>' + documentTitle +'</h2><br>'+
                              '<p>'+
                                '<img src="' + localImage + '"alt="'+documentTitle+'" title="'+documentTitle+'"></img>'+
                              '</p>'+
                              '<p>' + shareText + '</p>';

            var htmlFormat = 
                Windows.ApplicationModel.DataTransfer.HtmlFormatHelper.createHtmlFormat(htmlContent);
            request.data.setHtmlFormat(htmlFormat);
            var streamRef = 
                Windows.Storage.Streams.RandomAccessStreamReference.createFromUri(new Windows.Foundation.Uri(localImage));

            request.data.resourceMap[localImage] = streamRef;

        }

    });
})();


