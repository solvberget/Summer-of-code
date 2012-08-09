(function () {
    "use strict";

    var ui = WinJS.UI;

    ui.Pages.define("/pages/events/eventDetail/eventDetail.html", {

        ready: function (element, options) {
            var item = options && options.item ? EventData.resolveItemReference(options.item) : EventData.items.getAt(0);
            WinJS.Binding.processAll(element, item);
            element.querySelector(".titlearea .pagetitle").textContent = EventData.getGroupByKey(item.TypeId).Name;
            element.querySelector(".content").focus();
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
