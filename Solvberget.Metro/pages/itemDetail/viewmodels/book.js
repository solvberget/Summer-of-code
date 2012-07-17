(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        Book: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (item) {
                this.title = item.Title;
                this.subtitle = "Bok"
                if (item.Author.Name != undefined) {
                    this.subtitle +=  " skrevet av " + item.Author.Name;
                }
                this.Author = item.Author.Name;
                
                
            },
        }),

 
    });
})();
