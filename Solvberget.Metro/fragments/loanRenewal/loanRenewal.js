(function () {

    function showRenewalFlyout(element, navigateTo, loan) {
        var renewalFlyout = document.getElementById("renewalFlyout");
        WinJS.UI.processAll(renewalFlyout);

        document.getElementById("submitRenewalButton").addEventListener("click", function () {
            submitRenewal(loan.DocumentNumber, loan.ItemSequence, loan.Barcode, LoginFlyout.getLoggedInLibraryUserId());
        }, false);
        document.getElementById("cancelRenewalButton").addEventListener("click", cancel, false);
        document.getElementById("renewalFlyout").addEventListener("afterhide", onDismiss, false);

        renewalFlyout.winControl.show(element, "right");
        $("#renewalLoading").css("display", "none").css("visibility", "hidden");
    }
    
    var ajaxRequestLoanRenewal = function (documentNumber, itemSecq, barcode, libraryUserId) {
        var borrowerId = LoginFlyout.getLoggedInLibraryUserId();
        if (borrowerId != undefined && borrowerId !== "")
            var requestString = "?documentNumber=" + documentNumber + "&itemSecq=" + itemSecq + "&barcode=" + barcode + "&libraryUserId=" + libraryUserId;
        var requestUrl = window.Data.serverBaseUrl + "/Document/RequestLoanRenewal/" + requestString;
        var json = $.getJSON(requestUrl);
     
        return json;
    };

    function submitRenewal(documentNumber, itemSecq, barcode, libraryUserId) {
        var renewalFlyout = document.getElementById("renewalFlyout");
     
        $("#renewalLoading").css("display", "block").css("visibility", "visible");
        
       
        $.when(ajaxRequestLoanRenewal(documentNumber, itemSecq, barcode, libraryUserId))
            .then($.proxy(function(response) {
                if (response != undefined && response !== "") {
                    //Ok det er fornyet
                    var renewalDiv = document.getElementById("renewalOutputMsg");
                    renewalDiv.innerHTML = response.Reply;
                    if(response.Success) {

                        $("#renewalOutputMsg").removeClass("error");
                        $("#renewalOutputMsg").addClass("success");
                        //green
                    } else {

                        $("#renewalOutputMsg").removeClass("success");
                        $("#renewalOutputMsg").addClass("error");
                        //red
                        
                    }
                    WinJS.Binding.processAll(renewalDiv, response);
                    $("#renewalLoading").css("display", "none").css("visibility", "hidden");
                    setTimeout(function () {
                        var flyout = document.getElementById("renewalFlyout");
                        if (flyout != undefined)
                            flyout.winControl.hide();
                    }, 1200);
                    
                }
            }, this)
            );
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

    }

    WinJS.Namespace.define("LoanRenewal", {
        showFlyout: showRenewalFlyout
    });
})();