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
                this.subtitle = "Språkkurs gitt ut i " + item.PublishedYear;
            },
        }),
    });
})();
