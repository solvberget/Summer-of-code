(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        AudioBook: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (audioBookItem) {
                this.title = audioBookItem.Title;
                this.subtitle = "Lydbok gitt ut i " +audioBookItem.PublishedYear;
                this.Author = audioBookItem.Author.Name;
               
            },
        }),


    });

})();


