(function () {
    // "use strict";
    WinJS.Namespace.define("ViewModel", {
        SheetMusic: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (item) {
                this.title = item.Title;
                this.subtitle = "Noter gitt ut i " + item.PublishedYear;
            },
        }),
    });
})();
