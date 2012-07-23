(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/fragments/documentFragments/audioBook/audioBook.html", {
        ready: function (element, options) {



        }
    });

})();

var fragmentReady = function() {


};


WinJS.Namespace.define("DocumentDetailFragment", {

    ready: fragmentReady,

});

WinJS.Namespace.define("DocumentDetailConverters", {
    // Title converter
    // Subtitle converter
    // BackgroundImage converter (empty ==> dummy)
    // 
});