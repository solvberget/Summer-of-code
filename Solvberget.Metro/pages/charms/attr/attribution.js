(function () {
    
    "use strict";
    var page = WinJS.UI.Pages.define("/pages/charms/attr/attribution.html", {

        ready: function (element, options) {
            // Register the handlers for dismissal
            document.getElementById("attributionSettingsFlyout").addEventListener("keydown", handleAltLeft);
            document.getElementById("attributionSettingsFlyout").addEventListener("keypress", handleBackspace);
        },

        unload: function () {
            // Remove the handlers for dismissal
            document.getElementById("attributionSettingsFlyout").removeEventListener("keydown", handleAltLeft);
            document.getElementById("attributionSettingsFlyout").removeEventListener("keypress", handleBackspace);
        },
    });

    function handleAltLeft(evt) {
        if (evt.altKey && evt.key === 'Left') {
            WinJS.UI.SettingsFlyout.show();
        }
    };

    function handleBackspace(evt) {
        if (evt.key === 'Backspace') {
            WinJS.UI.SettingsFlyout.show();
        }
    };
    
})();