(function () {
    "use strict";
    var page = WinJS.UI.Pages.define("/fragments/addToFavorites/addToFavorites.html", {
        ready: function (element, options) {

        }
    });

    function showAddToFavoritesFlyout(element, success, message1, message2) {
        if (success)
            $("#message1").addClass("success");
        else
            $("#message1").addClass("error");
        $("#message1").html(message1);
        $("#message2").html(message2);
        var addToFavoritesFlyout = document.getElementById("addToFavoritesFlyout");
        WinJS.UI.processAll(addToFavoritesFlyout);
        document.getElementById("closeAddToFavoritesButton").addEventListener("click", dismiss, false);
        document.getElementById("addToFavoritesFlyout").addEventListener("afterhide", onDismiss, false);
        addToFavoritesFlyout.winControl.show(element, "right");
    }

    function dismiss() {
        var addToFavoritesFlyout = document.getElementById("addToFavoritesFlyout");
        if (addToFavoritesFlyout) {
            addToFavoritesFlyout.winControl.hide();
        }
    }

    function onDismiss() {
        // Clear fields on dismiss
        $("#message1").html("");
        $("#message2").html("");
    }

    WinJS.Namespace.define("AddToFavorites", {
        showFlyout: showAddToFavoritesFlyout
    });

})();