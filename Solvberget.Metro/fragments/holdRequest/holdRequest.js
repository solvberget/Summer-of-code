(function () {

    //TODO: lage denne!

    function showHoldRequestFlyout(element, navigateTo) {
        requestSent = false;
        navigateToUrl = navigateTo;

        var loginFlyout = document.getElementById("holdRequestFlyout");
        WinJS.UI.processAll(loginFlyout);

        document.getElementById("submitRequestButtonMain").addEventListener("click", submitRequestMain, false);
        document.getElementById("submitRequestButtonMadla").addEventListener("click", submitRequestMadla, false);
        document.getElementById("holdRequestFlyout").addEventListener("afterhide", onDismiss, false);

        holdRequestFlyout.winControl.show(element, "right");

    }

    function submitRequestMain() {

    }

    function submitRequestMadla() {

    }

    function onDismiss() {
        // Clear fields on dismiss
        document.getElementById("submitRequestButtonMain").value = "";
        document.getElementById("submitRequestButtonMadla").value = "";
        document.getElementById("outputMsgHoldRequest").value = "";
        
    }

    WinJS.Namespace.define("HoldRequest", {
        showFlyout: showHoldRequestFlyout
    });
})();