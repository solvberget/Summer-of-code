(function () {
    // "use strict";
    WinJS.Namespace.define("ViewModel", {
        Journal: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (item) {
                this.title = item.Title + " (" + item.PublishedYear + ")";
                this.subtitle = "Tidskrift gitt ut i " + item.PublishedYear;
            },
        }),
    });
})();
