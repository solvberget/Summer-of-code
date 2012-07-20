(function () {

    // Track if the log in was successful
    var loggedIn;

    "use strict";
    var page = WinJS.UI.Pages.define("/fragments/documentFragments/film/film.html", {
        ready: function (element, options) {

        }
    });

})();



var fragmentReady = function () {

    console.log("yeah film");
}


WinJS.Namespace.define("DocumentDetailFragment", {

    ready: fragmentReady,

});

WinJS.Namespace.define("DocumentDetailConverters", {
    imdbStyleConverter: WinJS.Binding.converter(function (imdbSrc) {
        return "display:block";
    }),
});