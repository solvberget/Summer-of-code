(function () {

    var reservationElement;
    var allReservations;
    var reservationIndex;
    function showCancelReservationFlyout(element, reservations, index, el) {
        reservationElement = el;
        reservationIndex = index;
        allReservations = reservations;

        var flyout = document.getElementById("cancelReservationFlyout");
        WinJS.UI.processAll(flyout);

        document.getElementById("submitCancelReservationButton").addEventListener("click", function () {
            cancelReservation(reservations[index].ItemDocumentNumber, reservations[index].ItemSeq, reservations[index].CancellationSequence);
        }, false);
        document.getElementById("cancelCancelReservationButton").addEventListener("click", cancel, false);

        flyout.addEventListener("afterhide", onDismiss, false);

        flyout.winControl.show(element, "right");
        $("#cancelReservationLoading").css("display", "none").css("visibility", "hidden");
    }


    var ajaxCancelReservation = function(documentItemNumber, itemSeq, itemCancellationCode) {

        var url = window.Data.serverBaseUrl + "/Document/CancelReservation/" + documentItemNumber + "/" + itemSeq + "/" + itemCancellationCode;
        Solvberget.Queue.QueueDownload("cancelreservation", { url: url }, ajaxCancelReservationCallback, this, true);

    };

    var ajaxCancelReservationCallback = function (request, context) {
        var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
       
        if (response != undefined && response !== "") {

            var flyout = document.getElementById("cancelReservationFlyout");

            var outputMsg = document.getElementById("cancelReservationOutputMsg");
            if (!outputMsg)
                return;

            outputMsg.innerHTML = "";

            if (response.Success) {
                $("#cancelReservationOutputMsg").removeClass("error");
                $("#cancelReservationOutputMsg").addClass("success");
            } else {
                $("#cancelReservationOutputMsg").removeClass("success");
                $("#cancelReservationOutputMsg").addClass("error");
            }
            document.getElementById("cancelReservationOutputMsg").textContent = response.Reply;

            $("#cancelReservationLoading").css("display", "none").css("visibility", "hidden");
            if (response.Success) {
                setTimeout(function () {
                    flyout = document.getElementById("cancelReservationFlyout");
                    if (flyout != undefined)
                        flyout.winControl.hide();

                    allReservations[reservationIndex] = undefined;
                    MyPage.addReservationsToDom(allReservations);

                }, 1000);
            }
        }
    };


    function cancelReservation(documentItemNumber, itemSeq, itemCancellationCode) {

        $("#cancelReservationLoading").css("display", "block").css("visibility", "visible");

        ajaxCancelReservation(documentItemNumber, itemSeq, itemCancellationCode);
    }

    function cancel() {
        var flyout = document.getElementById("cancelReservationFlyout");
        flyout.winControl.hide();
        Solvberget.Queue.CancelQueue('cancelreservation');

    }


    function onDismiss() {
        // Clear fields on dismiss
        var subCancelBtn = document.getElementById("submitCancelReservationButton");
        if (subCancelBtn) {
            document.getElementById("submitCancelReservationButton").value = "";
            document.getElementById("cancelCancelReservationButton").value = "";
            document.getElementById("cancelReservationOutputMsg").value = "";
        }
    }

    WinJS.Namespace.define("CancelReservation", {
        showFlyout: showCancelReservationFlyout
    });
})();