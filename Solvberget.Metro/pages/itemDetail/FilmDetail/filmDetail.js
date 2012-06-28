(function () {
    "use strict";

    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    ui.Pages.define("/pages/itemDetail/itemDetail.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            var item = options.item;
            var documentId = options.key;



            console.log("From itemDetails, key: " + documentId);
            element.querySelector("article .item-title").textContent = item.Title;
            element.querySelector("article .item-subtitle").textContent = item.PublishedYear;
            element.querySelector("article .item-image").src = "/images/dummydata/hp1.jpeg";
            //element.querySelector("article .item-image").alt = item.subtitle;
            //element.querySelector("article .item-content").innerHTML = item.content;
            element.querySelector(".content").focus();
        }
    });
})();
