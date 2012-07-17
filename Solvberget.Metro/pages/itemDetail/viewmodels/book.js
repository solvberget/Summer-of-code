(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        Book: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (bookItem) {
                this.title = bookItem.Title;
                this.subtitle = "Bok skrevet av " + bookItem.Author.Name;           
                this.Author = bookItem.Author.Name;
                
                
            },
        }),

 
    });
})();
