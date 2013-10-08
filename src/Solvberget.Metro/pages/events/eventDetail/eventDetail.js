(function () {
    "use strict";

    var ui = WinJS.UI;
    var event;

    ui.Pages.define("/pages/events/eventDetail/eventDetail.html", {

        ready: function (element, options) {
            var item = options && options.item ? EventData.resolveItemReference(options.item) : EventData.items.getAt(0);
            event = item;
            WinJS.Binding.processAll(element, item);
            
            if (!item.PictureUrl) {
                $(".event-image-container").css("display", "none");
                $(".event-image-container").css("-ms-grid-row", "0");
                $(".event-content-holder").css("-ms-grid-row", "2");
            }

            element.querySelector(".titlearea .pagetitle").textContent = EventData.getGroupByKey(item.TypeId).Name;
            document.getElementById("cal-button").addEventListener("click", this.openIcalForEvent);
            document.getElementById("link-button").addEventListener("click", this.openLinkToEvent);
            element.querySelector(".content").focus();
        },

        openIcalForEvent: function () {
            var uriRaw = event.ICalLink;
            var uri = new Windows.Foundation.Uri(uriRaw);
            Windows.System.Launcher.launchUriAsync(uri).then(
            function (success) {
                if (success) {
                    //No-op
                } else {
                    //Error
                }
            });
        },

        openLinkToEvent: function () {
            var uriRaw = event.Link;
            var uri = new Windows.Foundation.Uri(uriRaw);
            Windows.System.Launcher.launchUriAsync(uri).then(
            function (success) {
                if (success) {
                    //No-op
                } else {
                    //Error
                }
            });
        }

    });

    WinJS.Namespace.define("EventItemConverters", {

        startConverter: WinJS.Binding.converter(function (start) {
            return (start == undefined || start === "") ? "" : "Starter kl. " + start;
        }),

        stopConverter: WinJS.Binding.converter(function (stop) {
            return (stop == undefined || stop === "") ? "" : "Slutter kl. " + stop;
        }),

        typeConverter: WinJS.Binding.converter(function (type) {
            return (type == undefined || type === "") ? "" : "Type arrangement: " + type;
        }),

        priceConverter: WinJS.Binding.converter(function (price) {
            return (price == undefined || price === "") ? "" : "Pris: " + price + " kr";
        })

    });

})();
