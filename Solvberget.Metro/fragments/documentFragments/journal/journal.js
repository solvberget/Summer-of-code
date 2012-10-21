(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/journal/journal.html", {
        ready: function (element, options) {

        }
    });

})();

var fragmentReady = function() {

};

WinJS.Namespace.define("DocumentDetailFragment", {
    readyJournal: fragmentReady
});

WinJS.Namespace.define("DocumentDetailConverters", {

});