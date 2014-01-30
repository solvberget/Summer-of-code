(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/game/game.html", {
        ready: function (element, options) {

        }
    });

})();

var fragmentReady = function() {

};


WinJS.Namespace.define("DocumentDetailFragment", {
    readyGame: fragmentReady
});

WinJS.Namespace.define("DocumentDetailConverters", {

});