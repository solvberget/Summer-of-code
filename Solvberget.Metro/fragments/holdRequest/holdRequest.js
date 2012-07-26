(function () {
    "use strict";
    var page = WinJS.UI.Pages.define("/fragments/holdRequest/holdRequest.html", {
        ready: function (element, options) {

        }
    });

    var documentNumber;
    var internalLibraryUserId;
    var documentModel;
    // Ajax for reservasjon
    function ajaxDoReserve(docId, libraryInternalUserId, branch) {
        return $.getJSON("http://localhost:7089/Document/RequestReservation/" + docId + "/" + libraryInternalUserId + "/" + branch);
    }


    function showHoldRequestFlyout(element, docModel) {
        documentModel = docModel;
        documentNumber = docModel.DocumentNumber;
        internalLibraryUserId = LoginFlyout.getLoggedInLibraryUserId();

        if (internalLibraryUserId && internalLibraryUserId !== "") {
            $("#holdRequestFlyoutLoggedInSuccess").css("display", "block").css("visibility", "visible");
            $("#holdRequestFlyoutLoggedInError").css("display", "none").css("visibility", "hidden");
        }
        else {
            $("#holdRequestFlyoutLoggedInSuccess").css("display", "none").css("visibility", "hidden");
            $("#holdRequestFlyoutLoggedInError").css("display", "block").css("visibility", "visible");
        }

        var holdRequestFlyout = document.getElementById("holdRequestFlyout");
        WinJS.UI.processAll(holdRequestFlyout);



        var showBranch = branchExists("Hovedbibl.");

        if (showBranch)
            document.getElementById("submitRequestButtonMain").addEventListener("click", submitRequestMain, false);
        else
            $("#submitRequestButtonMain").attr("disabled", "disabled");

        showBranch = branchExists("Madla");
        if (showBranch)
            document.getElementById("submitRequestButtonMadla").addEventListener("click", submitRequestMadla, false);
        else
            $("#submitRequestButtonMadla").attr("disabled", "disabled");

        document.getElementById("holdRequestFlyout").addEventListener("afterhide", onDismiss, false);
        holdRequestFlyout.winControl.show(element, "right");

    }

    function submitRequestMain() {
        var branch = "Hovedbibl";
        doReservation(documentNumber, internalLibraryUserId, branch);
    }

    function branchExists(branch) {
        if (!documentModel.AvailabilityInfo)
            return false;

        for (var i = 0; i < documentModel.AvailabilityInfo.length; i++) {
            if (documentModel.AvailabilityInfo[i].Branch === branch) {
                return true;
            }
        }
        return false;
    }

    function submitRequestMadla() {
        var branch = "Madla";
        doReservation(documentNumber, internalLibraryUserId, branch);
    }

    function doReservation(docId, userId, branch) {

        $("#holdRequestLoading").css("display", "block").css("visibility", "visible");

        var outputMsg = document.getElementById("outputMsgHoldRequest");
        if (outputMsg != undefined)

            outputMsg.innerHTML = "";

        // Prevent multiple reservation by caching this request
        $.ajaxSetup({ cache: true });

        $.when(ajaxDoReserve(docId, userId, branch))
            .then($.proxy(function (response) {

                var outputMsg = document.getElementById("outputMsgHoldRequest");
                var jqueryOutputMsg = $("#outputMsgHoldRequest");

                if (response.Success) {
                    jqueryOutputMsg.addClass("success");
                    jqueryOutputMsg.removeClass("error");
                    setTimeout(function () {
                        dismiss();
                    }, 2000);
                } else {
                    jqueryOutputMsg.removeClass("success");
                    jqueryOutputMsg.addClass("error");
                }


                if (outputMsg != undefined) {
                    outputMsg.innerHTML = response.Reply;
                }


                $("#holdRequestLoading").css("display", "none").css("visibility", "hidden");

            }, this));

    }


    function dismiss() {
        var holdRequestFlyout = document.getElementById("holdRequestFlyout");
        if (holdRequestFlyout) {
            holdRequestFlyout.winControl.hide();
        }
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