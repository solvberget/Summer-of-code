(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/cd/cd.html", {
        ready: function (element, options) {

        }
    });

})();

var fragmentReady = function() {

};

WinJS.Namespace.define("DocumentDetailFragment", {
    readyCd: fragmentReady
});

WinJS.Namespace.define("DocumentDetailConverters", {
   
});