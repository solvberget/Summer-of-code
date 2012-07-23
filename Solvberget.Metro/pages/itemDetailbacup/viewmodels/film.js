(function () {
    // "use strict";
    WinJS.Namespace.define("ViewModel", {
        Film: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            imdbRating:undefined,
            propertiesList:[],
            fillProperties: function (item) {
                this.title = item.Title 
                this.subtitle = "Film"

                if (item.PublishedYear != undefined && item.PublishedYear != 0) {
                    this.title += " (" + item.PublishedYear + ")";
                    this.subtitle += " gitt ut i " + item.PublishedYear;
                }
            },
        }),
    });
})();
