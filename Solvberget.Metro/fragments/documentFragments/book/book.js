(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/book/book.html", {
        ready: function (element, options) {



        }
    });

})();

var fragmentReady = function () {

    console.log("yeah");
}


WinJS.Namespace.define("DocumentDetailFragment", {

    ready: fragmentReady,

});

WinJS.Namespace.define("DocumentDetailConverters", {
    // Title converter
    // Subtitle converter
    // BackgroundImage converter (empty ==> dummy)
    // 
});