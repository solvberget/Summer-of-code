(function () {
    "use strict";
    var page = WinJS.UI.Pages.define("/fragments/holdRequest/holdRequest.html", {
        ready: function (element, options) {

        }
    });

    var documentNumber;
    var internalLibraryUserId;
    var documentModel;


    // !------------ AJAX METHODS -------------! //

    var ajaxDoReserve = function (docId, libraryInternalUserId, branch) {
        var url = window.Data.serverBaseUrl + "/Document/RequestReservation/" + docId + "/" + libraryInternalUserId + "/" + branch;
        Solvberget.Queue.QueueDownload("reserve", { url: url }, ajaxDoReserveCallback, null, true);
    };

    // !------------ AJAX CALLBACKS -------------! //

    var ajaxDoReserveCallback = function (request, context) {
        var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

        if (response != undefined && response != "") {


            var outputMsg = document.getElementById("outputMsgHoldRequest");
            var jqueryOutputMsg = $("#outputMsgHoldRequest");

            if (response.Success) {
                jqueryOutputMsg.addClass("success");
                jqueryOutputMsg.removeClass("error");
                $("#sendHoldRequestButton").attr("disabled", "disabled");
                setTimeout(function () {
                    dismiss();
                }, 1000);
            } else {
                jqueryOutputMsg.removeClass("success");
                jqueryOutputMsg.addClass("error");
            }


            if (outputMsg != undefined) {
                outputMsg.innerHTML = response.Reply;
            }
        }
        $("#holdRequestLoading").hide();
    };


    function showHoldRequestFlyout(element, docModel) {
        documentModel = docModel;
        documentNumber = docModel.DocumentNumber;
        internalLibraryUserId = LoginFlyout.getLoggedInLibraryUserId();

        $("#holdRequestLoading").hide();

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


        document.getElementById("submitRequestButtonMain").addEventListener("click", submitRequestMain, false);
        document.getElementById("submitRequestButtonCancel").addEventListener("click", submitRequestCancel, false);

        document.getElementById("holdRequestFlyout").addEventListener("afterhide", onDismiss, false);
        holdRequestFlyout.winControl.show(element, "right");

    }

    function submitRequestMain() {

        var existsAtBranch = branchExists("Hovedbibl.");
        if (existsAtBranch) {
            doReservation(documentNumber, internalLibraryUserId, "Hovedbibl");
            return;
        }
        existsAtBranch = branchExists("Madla");
        if (existsAtBranch) {
            doReservation(documentNumber, internalLibraryUserId, "Madla");
            return;
        }
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

    function submitRequestCancel() {
        dismiss();
    }

    function doReservation(docId, userId, branch) {

        $("#holdRequestLoading").fadeIn();

        var outputMsg = document.getElementById("outputMsgHoldRequest");
        if (outputMsg != undefined)
            outputMsg.innerHTML = "";

        // Prevent multiple reservation by caching this request
        $.ajaxSetup({ cache: true });

        ajaxDoReserve(docId, userId, branch);

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
        document.getElementById("submitRequestButtonCancel").value = "";
        document.getElementById("outputMsgHoldRequest").value = "";
        Solvberget.Queue.CancelQueue('reserve');

    }

    WinJS.Namespace.define("HoldRequest", {
        showFlyout: showHoldRequestFlyout
    });
})();