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

            // TODO: Get document object and set the stuff below me 
            
            
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
            element.querySelector(".content").focus();
        }
    });
})();
