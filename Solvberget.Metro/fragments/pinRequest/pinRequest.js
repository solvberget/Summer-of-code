(function () {

    "use strict";

    var requestUrlBase = window.Data.serverBaseUrl + "/User/RequestPinCodeToSms/";

    var page = WinJS.UI.Pages.define("/fragments/pinRequest/pinRequest.html", {

        ready: function (element, options) {

        },

    });

    // !------------ AJAX METHODS -------------! //

    var sendPinRequest = function (userId) {
        var url = requestUrlBase + userId;
        Solvberget.Queue.QueueDownload("pin", { url: url }, sendPinRequestCallback, null, true);
    };

    // !------------ AJAX CALLBACKS -------------! //

    var sendPinRequestCallback = function (request, context) {
        var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
        
        var outputMsgPinReq = document.getElementById("outputMsgPinReq");
        if (response.Success == true) {
            if (outputMsgPinReq != undefined) {
                if (response.Reply) {
                    outputMsgPinReq.innerHTML = response.Reply;
                }
                else {
                    outputMsgPinReq.innerHTML = "Din pin-kode vil bli tilsendt via SMS.";
                }
            }
            $("#outputMsgPinReq").removeClass("error");
            $("#outputMsgPinReq").addClass("success");
            $("#submitPinRequestButton").removeAttr("disabled");
            $("#cancelFlyoutButton").html("Lukk");
            $("#pinRequestLoading").css("display", "none").css("visibility", "hidden");

            setTimeout(function () {
                var pinRequestFlyout = document.getElementById("pinRequestFlyout");
                if (pinRequestFlyout != undefined)
                    pinRequestFlyout.winControl.hide();
            }, 2500);

        }
        else {
            if (outputMsgPinReq != undefined) {
                if (response.Reply) {
                    outputMsgPinReq.innerHTML = response.Reply;
                }
                else {
                    outputMsgPinReq.innerHTML = "Forespørselen kunne ikke utføres.";
                }
            }
            $("#outputMsgPinReq").removeClass("success");
            $("#outputMsgPinReq").addClass("error");
            $("#submitPinRequestButton").removeAttr("disabled");
            $("#cancelFlyoutButton").html("Lukk");
            $("#pinRequestLoading").css("display", "none").css("visibility", "hidden");
        }

    };

    // !------------ END AJAX  END -------------! //


    var submitPinRequest = function () {

        $("#submitPinRequestButton").attr("disabled", "disabled");
        $("#cancelFlyoutButton").html("Avbryt");
        $("#outputMsgPinReq").html("");

        var error = false;
        if (document.getElementById("userIdPinReq").value.trim() === "") {
            document.getElementById("userIdErrorPinReq").innerHTML = "Lånekortnummer må fylles inn";
            document.getElementById("userIdPinReq").focus();
            error = true;
            $("#submitPinRequestButton").removeAttr("disabled");
        } else {
            document.getElementById("userIdErrorPinReq").innerHTML = "";
        }
        if (!error) {
            $("#pinRequestLoading").css("display", "block").css("visibility", "visible");
            var outputMsgPinReq = document.getElementById("outputMsgPinReq");
            if (this.outputMsgPinReq != undefined)
                outputMsgPinReq.innerHTML = "";
            sendPinRequest($("#userIdPinReq").val());
        }
    };

    var dismiss = function () {
        var holdRequestFlyout = document.getElementById("pinRequestFlyout");
        if (holdRequestFlyout) {
            holdRequestFlyout.winControl.hide();
        }
    };

    var onDismiss = function () {
        document.getElementById("userIdPinReq").value = "";
        document.getElementById("userIdErrorPinReq").innerHTML = "";
        document.getElementById("outputMsgPinReq").innerHTML = "";
        $("#cancelFlyoutButton").html("Avbryt");
        var reqPinFragmentDivHolder = document.getElementById("requestPinFragmentHolder");
        if (reqPinFragmentDivHolder)
            reqPinFragmentDivHolder.innerHTML = "";
        
        Solvberget.Queue.CancelQueue('pin');

    };

    var showPinRequestFlyout = function (element, navigateTo) {
        var pinRequestFlyout = document.getElementById("pinRequestFlyout");
        WinJS.UI.processAll(pinRequestFlyout);
        document.getElementById("submitPinRequestButton").addEventListener("click", submitPinRequest, false);
        document.getElementById("cancelFlyoutButton").addEventListener("click", dismiss, false);
        document.getElementById("pinRequestFlyout").addEventListener("afterhide", onDismiss, false);
        pinRequestFlyout.winControl.show(element, "right");
    };

    WinJS.Namespace.define("PinRequestFlyout", {
        showPinRequestFlyout: showPinRequestFlyout,
    });

})();
