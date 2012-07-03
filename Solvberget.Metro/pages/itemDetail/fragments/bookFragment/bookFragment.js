(function () {
    "use strict";

    var changeHitCount = 0;
    function changeHandler(event2) {
        WinJS.log && WinJS.log("Change occoured in book");
    }

    function fragmentLoad(root) {
        // After the fragment is loaded into the target element,
        // CSS and JavaScript referenced in the fragment are loaded.
        // Then the loading code calls this method to process controls.
        WinJS.UI.processAll(root).then(function () {
            //Add listeners here for changes in the UI
            //WinJS.Utilities.query('.fragDatePicker', root).control({
            //    onchange: changeHandler
            //});
        });
    }

    WinJS.Namespace.define('Fragment', {
        fragmentLoad: fragmentLoad,
    });

    var ui = WinJS.UI;
    var utils = WinJS.Utilities;
})();
