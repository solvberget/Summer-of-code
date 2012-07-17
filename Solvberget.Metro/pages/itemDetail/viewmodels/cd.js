(function () {
    // "use strict";
    WinJS.Namespace.define("ViewModel", {
        Cd: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (item) {
                this.title = item.Title + " (" + item.PublishedYear + ")";
                this.subtitle = "CD gitt ut i " + item.PublishedYear;
            },
        }),
    });
})();
