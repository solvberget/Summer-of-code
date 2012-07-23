(function () {
    // "use strict";
    WinJS.Namespace.define("ViewModel", {
        LanguageCourse: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (item) {
                this.title = item.Title;
                this.subtitle = "Språkkurs";

                if (item.PublishedYear != undefined && item.PublishedYear != 0) {
                    this.title += " (" + item.PublishedYear + ")";
                    this.subtitle += " gitt ut i " + item.PublishedYear;
                }
            },
        }),
    });
})();
