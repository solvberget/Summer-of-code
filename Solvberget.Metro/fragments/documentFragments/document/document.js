(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/document/document.html", {
        ready: function (element, options) {

        }
    });

})();

var fragmentReady = function() {

};

WinJS.Namespace.define("DocumentDetailFragment", {
    readyDocument: fragmentReady
});

WinJS.Namespace.define("DocumentDetailConverters", {

});