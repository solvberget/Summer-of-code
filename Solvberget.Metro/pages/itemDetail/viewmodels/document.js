(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        Document: WinJS.Binding.as({
            documentId: undefined,
            title: undefined,
            subtitle: undefined,
            image: undefined,
            propertiesList: [],
            fillProperties: function (documentModel) {
                this.title = documentModel.Title;
                this.subtitle="";

            },
        }),


    });

})();

