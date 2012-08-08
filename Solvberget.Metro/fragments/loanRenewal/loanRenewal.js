(function () {

    function showRenewalFlyout(element, navigateTo, loan) {
        var renewalFlyout = document.getElementById("renewalFlyout");
        WinJS.UI.processAll(renewalFlyout);

        document.getElementById("submitRenewalButton").addEventListener("click", function () {
            submitRenewal(loan.DocumentNumber, loan.ItemSequence, loan.Barcode, LoginFlyout.getLoggedInLibraryUserId());
        }, false);
        document.getElementById("cancelRenewalButton").addEventListener("click", cancel, false);
        document.getElementById("renewalOkButton").addEventListener("click", cancel, false);
        document.getElementById("renewalFlyout").addEventListener("afterhide", onDismiss, false);

        renewalFlyout.winControl.show(element, "right");
        $("#renewalLoading").hide();
        $("#renewalOkButton").css("display", "none").css("visibility", "hidden");
    }


    // !------------ AJAX METHODS -------------! //

    var ajaxRequestLoanRenewal = function (documentNumber, itemSecq, barcode, libraryUserId) {
        var borrowerId = LoginFlyout.getLoggedInLibraryUserId();
        var requestString = "";
        if (borrowerId != undefined && borrowerId !== "")
            requestString = "?documentNumber=" + documentNumber + "&itemSecq=" + itemSecq + "&barcode=" + barcode + "&libraryUserId=" + libraryUserId;
        var requestUrl = window.Data.serverBaseUrl + "/Document/RequestLoanRenewal/" + requestString;
        Solvberget.Queue.QueueDownload("loanrenew", { url: requestUrl }, ajaxRequestLoanRenewalCallback, this, true);
    };


    // !------------ AJAX CALLBACKS -------------! //


    var ajaxRequestLoanRenewalCallback = function (request, context) {
        var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

        if (response != undefined && response !== "") {

            //Ok, lånet er fornyet
            var renewalDiv = document.getElementById("renewalOutputMsg");
            renewalDiv.innerHTML = response.Reply;

            if (response.Success) {

                $("#submitRenewalButton").css("display", "none").css("visibility", "hidden");
                $("#cancelRenewalButton").css("display", "none").css("visibility", "hidden");

                $("#renewalOutputMsg").removeClass("error");
                $("#renewalOutputMsg").addClass("success");
                //green

                WinJS.Binding.processAll(renewalDiv, response);
                $("#renewalLoading").hide();
                setTimeout(function () {
                    var flyout = document.getElementById("renewalFlyout");
                    if (flyout != undefined)
                        flyout.winControl.hide();
                }, 1200);
            } else {

                $("#renewalOutputMsg").removeClass("success");
                $("#renewalOutputMsg").addClass("error");
                //red

                $("#renewalLoading").hide();
                $("#renewalOkButton").css("display", "block").css("visibility", "visible");
                $("#submitRenewalButton").css("display", "none").css("visibility", "hidden");
                $("#cancelRenewalButton").css("display", "none").css("visibility", "hidden");

            }
        }

    };


    // !------------ END AJAX END -------------! //



    function submitRenewal(documentNumber, itemSecq, barcode, libraryUserId) {
        var renewalFlyout = document.getElementById("renewalFlyout");

        $("#renewalLoading").fadeIn();

        ajaxRequestLoanRenewal(documentNumber, itemSecq, barcode, libraryUserId);

    }

    function cancel() {
        var flyout = document.getElementById("renewalFlyout");
        flyout.winControl.hide();
    }


    function onDismiss() {
        // Clear fields on dismiss
        document.getElementById("submitRenewalButton").value = "";
        document.getElementById("cancelRenewalButton").value = "";
        document.getElementById("renewalOutputMsg").value = "";
        document.getElementById("renewalOkButton").value = "";
        Solvberget.Queue.CancelQueue('loanrenew');

    }

    WinJS.Namespace.define("LoanRenewal", {
        showFlyout: showRenewalFlyout
    });

})();