(function () {
    // "use strict";
    WinJS.Namespace.define("ViewModel", {
        Film: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList:[],
            fillProperties: function (filmItem) {
                this.title = filmItem.Title + " (" + filmItem.PublishedYear + ")";
                this.subtitle = "Film gitt ut i " + filmItem.PublishedYear;
            },
        }),
    });
})();
