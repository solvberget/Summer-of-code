(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        AudioBook: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (item) {
                this.title = item.Title;
                this.subtitle = "Lydbok";
                if (item.PublishedYear != undefined && item.PublishedYear != 0) {
                    this.subtitle += " gitt ut i " + item.PublishedYear;
                }
               
               
            },
        }),


    });

})();


